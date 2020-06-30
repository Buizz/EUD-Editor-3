Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles

Partial Public Class CodeEditor

    Private ExternFiles As List(Of ExternFile)

    Public Shared Function _FildFile(currentPath As TEFile, paths() As String) As TEFile
        For i = 0 To paths.Count - 1
            'Log.Text = Log.Text & "Path" & i & " : " & paths(i) & "  "

            If paths(i) = "" Then '상위 폴더로
                If i = 0 Then
                    currentPath = currentPath.Parent.Parent
                Else
                    currentPath = currentPath.Parent
                End If
                Continue For
            End If


            'currentPath.Parent 해당 파일의 상위 파일
            Dim isFind As Boolean = False

            If currentPath.FileType <> TEFile.EFileType.Folder Then
                currentPath = currentPath.Parent
            End If

            For k = 0 To currentPath.FolderCount - 1
                If currentPath.Folders(k).FileName = paths(i) Then '골랐을 경우
                    currentPath = currentPath.Folders(k)
                    isFind = True
                    Exit For
                End If
            Next
            If Not isFind Then
                If currentPath.FileType <> TEFile.EFileType.Folder Then
                    currentPath = currentPath.Parent
                End If

                For k = 0 To currentPath.FileCount - 1
                    If currentPath.Files(k).FileName = paths(i) Then '파일을 찾았을 경우
                        currentPath = currentPath.Files(k)
                        Exit For
                    End If
                Next
            End If
        Next

        Return currentPath
    End Function
    Public Shared Function FineFile(TEFile As TEFile, path As String) As TEFile
        Dim paths() As String = path.Split(".")

        'Log.Text = Log.Text & "Path : " & path & "   "
        Dim currentPath As TEFile = TEFile
        currentPath = _FildFile(currentPath, paths)


        If path <> currentPath.FileName Then
            currentPath = pjData.TEData.PFIles
            currentPath = _FildFile(currentPath, paths)

        End If





        Return currentPath
        '선택한게 폴더일 경우와 파일일 경우로 나눠지긴 하지만 일단 구하긴함
        'Log.Text = Log.Text & currentPath.FileName & vbCrLf
    End Function


    Public Sub ExternerLoader()
        If TEFile IsNot Nothing Then
            For i = 0 To ExternFiles.Count - 1
                ExternFiles(i).CheckFlag = False
            Next



            'Log.Text = ""

            Dim Str As String = TextEditor.Text

            Dim fregex As New Regex("import\s+(.*);")

            Dim matches As MatchCollection = fregex.Matches(Str)

            'MsgBox(TEFile.Parent.FileName)
            For i = 0 To matches.Count - 1
                Dim filePath As String = matches(i).Groups(1).Value.Trim

                Dim val() As String = filePath.Split(" ")

                Dim nameSpaceName As String = ""
                Dim FileName As String = ""
                If val.Count = 1 Then 'As지정자가 없을 경우
                    nameSpaceName = val(0).Trim
                    FileName = val(0).Trim
                ElseIf val(1) = "as" Then
                    nameSpaceName = val.Last.Trim
                    FileName = val.First.Trim
                End If
                nameSpaceName = nameSpaceName.Split(".").Last

                Dim tTEfile As TEFile = Nothing
                Try
                    Dim iscmp As Boolean = False
                    Dim externList As List(Of String) = tescm.GetExternFileList
                    Dim realPath As List(Of String) = tescm.GetExternFileList(True)
                    For k = 0 To externList.Count - 1
                        If externList(k) = FileName & ".eps" Then
                            Dim SCATEFile As New TEFile(FileName, TEFile.EFileType.CUIEps)
                            CType(SCATEFile.Scripter, CUIScriptEditor).StringText = My.Computer.FileSystem.ReadAllText(realPath(k))

                            iscmp = True
                            tTEfile = SCATEFile
                            Exit For
                        End If
                    Next
                    If Not iscmp Then
                        tTEfile = FineFile(TEFile, FileName)
                    End If
                Catch ex As Exception
                    Continue For
                End Try

                'Log.Text = tTEfile.FileName & vbCrLf & "ExternFilesCount : " & ExternFiles.Count
                Dim CheckFlag As Boolean = False
                For k = 0 To ExternFiles.Count - 1
                    If ExternFiles(k).TEFile Is tTEfile Then '파일이 같다면?
                        ExternFiles(k).CheckFlag = True
                        ExternFiles(k).nameSpaceName = nameSpaceName
                        CheckFlag = True

                        If Not ExternFiles(k).CheckFIleChange Then '파일 체크해서 다를경우
                            ExternFiles(k).DateRefresh()
                            'MsgBox("파일 갱신됨 : " & tTEfile.FileName)
                            'Log.Text = TEFile.FileName & " " & ExternFiles(k).LastDate.ToString
                        End If
                        Exit For
                    End If
                Next

                If Not CheckFlag Then '만약 파일목록에 없었다면 새로추가
                    'Log.Text = Log.Text & vbCrLf & "항목 추가"
                    ExternFiles.Add(New ExternFile(tTEfile, nameSpaceName))
                    'MsgBox("파일 갱신됨 : " & tTEfile.FileName)
                End If


                'ExternFunc.LoadExtern(nameSpaceName, FileName, TEFile)
            Next

            Dim index As Integer = 0
            For i = 0 To ExternFiles.Count - 1
                If Not ExternFiles(index).CheckFlag Then '만약 목록에 없었을 경우
                    ExternFiles.RemoveAt(index)
                Else
                    index += 1
                End If
            Next
        End If
    End Sub
End Class


Public Class ExternFile
    '외부파일을 관리하는 곳
    '외부 파일들의 최종 수정 날짜를 판단함



    '
    Public Property nameSpaceName As String


    Public ReadOnly Property Funcs As CFunc



    Public Property TEFile As TEFile '지정된 TE파일


    Public LastDate As Date

    Public Property CheckFlag As Boolean = False

    Public Sub New(tTEFile As TEFile, tnameSpaceName As String)
        TEFile = tTEFile
        nameSpaceName = tnameSpaceName
        CheckFlag = True

        Funcs = New CFunc

        DateRefresh()
    End Sub

    Public Sub DateRefresh()
        '파일을 읽어서 CFunc를 작성함
        Funcs.Init()
        Try
            Funcs.LoadFunc(TEFile.Scripter.GetStringText)
        Catch ex As Exception
            Funcs.Init()
        End Try

        'Try
        '    MsgBox("데이터 불러오기 : " & TEFile.FileName)
        'Catch ex As Exception
        'End Try


        If TEFile IsNot Nothing Then
            LastDate = TEFile.LastDate '생성시 마지막 날짜를 기록
        End If
    End Sub



    Public Function CheckFIleChange() As Boolean

        If TEFile IsNot Nothing Then
            Return LastDate.ToString = TEFile.LastDate.ToString
        Else
            Return False
        End If
    End Function
End Class