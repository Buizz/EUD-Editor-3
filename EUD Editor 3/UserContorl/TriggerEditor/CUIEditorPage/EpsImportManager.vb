Imports BingsuCodeEditor

Public Class EpsImportManager : Inherits ImportManager
    Public Sub New()
        CodeType = BingsuCodeEditor.CodeTextEditor.CodeType.epScript
    End Sub

    Private Function GetTEFileContent(Path As String(), tTEfile As TEFile) As String
        Dim cTEfile As TEFile = tTEfile

        For index = 0 To Path.Length - 2
            '마지막아니면 폴더에서 찾기
            For i = 0 To cTEfile.FolderCount - 1
                If cTEfile.Folders(i).FileType <> TEFile.EFileType.Setting Then
                    If cTEfile.Folders(i).FileName = Path(index) Then
                        '파일

                        cTEfile = cTEfile.Folders(i)
                        Exit For
                    End If
                    'rlist.AddRange(GetTEFIleList(Filename, tTEfile.Folders(i)))
                End If
            Next
        Next


        'Dim rlist As List(Of String) = New List(Of String)

        For i = 0 To cTEfile.FileCount - 1
            Dim Filename As String = cTEfile.Files(i).FileName

            If cTEfile.Files(i).FileName = Path.Last Then
                '파일
                Return cTEfile.Files(i).Scripter.GetStringText()
            End If

            'Dim tComboboxitem As New ComboBoxItem
            'tComboboxitem.Tag = tTEfile.Files(i)
            'tComboboxitem.Content = Filename

            'rlist.Add(Filename.Substring(0, Filename.LastIndexOf(".")))
        Next


        Dim PullFileName As String = ""

        For Each item In Path
            If PullFileName <> "" Then
                PullFileName += "."
            End If
            PullFileName += item
        Next


        Dim externList As List(Of String) = tescm.GetExternFileList
        Dim realPath As List(Of String) = tescm.GetExternFileList(True)
        For k = 0 To externList.Count - 1
            If externList(k) = PullFileName & ".eps" Then
                Return IO.File.ReadAllText(realPath(k))
            End If
        Next



        'Return rlist
        Return ""
    End Function



    Public Overrides Function GetFIleContent(pullpath As String) As String
        Select Case pullpath
            Case "DEFAULTFUNCTIONLIST"
                Return System.IO.File.ReadAllText(Tool.TriggerEditorPath("epsFunctions_safe.txt"))
            Case Else
                Return GetTEFileContent(pullpath.Split("."), pjData.TEData.PFIles)
        End Select
        'Return "var test1;"
    End Function



    Private Function GetTEFIleList(Path As String, tTEfile As TEFile) As List(Of String)
        Dim rlist As List(Of String) = New List(Of String)

        For i = 0 To tTEfile.FileCount - 1
            Dim Filename As String = Path & tTEfile.Files(i).RealFileName
            'Dim tComboboxitem As New ComboBoxItem
            'tComboboxitem.Tag = tTEfile.Files(i)
            'tComboboxitem.Content = Filename



            rlist.Add(Filename.Substring(0, Filename.LastIndexOf(".")))

        Next


        For i = 0 To tTEfile.FolderCount - 1
            If tTEfile.Folders(i).FileType <> TEFile.EFileType.Setting Then
                Dim Filename As String = Path & tTEfile.Folders(i).FileName & "."
                rlist.AddRange(GetTEFIleList(Filename, tTEfile.Folders(i)))
            End If
        Next

        Return rlist
    End Function


    Public Overrides Function GetImportedFileList(Optional basefilename As String = "") As List(Of String)



        Dim rlist As List(Of String) = New List(Of String)

        If pjData.TEData.PFIles Is Nothing Then
            Return rlist
        End If
        rlist.AddRange(GetTEFIleList("", pjData.TEData.PFIles))

        For Each item In tescm.GetExternFileList()
            rlist.Add(item.Substring(0, item.LastIndexOf(".")))
        Next


        'rlist.Add("a")
        'rlist.Add("b")
        'rlist.Add("c")
        'rlist.Add("d.f")
        'rlist.Add("functest")

        Return rlist
    End Function

    Public Overrides Function GetDefaultFunctions() As String
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetFIleList() As List(Of String)
        Throw New NotImplementedException()
    End Function
End Class
