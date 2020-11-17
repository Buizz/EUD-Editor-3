Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Text.RegularExpressions

Module BuildErrorHandling



    Public Sub ErrorHandleClose()
        For Each win As Window In Application.Current.Windows
            If win.GetType Is GetType(TriggerEditor) Then
                Dim TEWindow As TriggerEditor = CType(win, TriggerEditor)

                TEWindow.ErrorListExpander.IsExpanded = False
                TEWindow.ErrorList.ItemsSource = Nothing
            End If
        Next
    End Sub



    Public Sub ErrorHandle(BuildLog As String, ErrorLog As String)
        Dim regex As New Regex("\[Error.*\] Module ""(.*)"" Line (\d+) : (.+)")
        Dim t As New ObservableCollection(Of ErrorItem)


        Dim mcol As MatchCollection = regex.Matches(ErrorLog)
        For i = 0 To mcol.Count - 1
            Dim File As String = mcol(i).Groups(1).Value.Trim
            Dim Line As String = mcol(i).Groups(2).Value.Trim
            Dim Description As String = "epScript 컴파일러 오류 : " & mcol(i).Groups(3).Value.Trim




            t.Add(New ErrorItem(File, Description, Line))
        Next


        If mcol.Count = 0 Then
            '일반 에러
            Dim Errorregex As New Regex("\[Error\](.*)Traceback \(most recent call last\):")
            mcol = Errorregex.Matches(ErrorLog)
            Dim Description As String = ""
            For i = 0 To mcol.Count - 1
                Description = mcol(i).Groups(1).Value
            Next

            Dim filepathregex As New Regex("File ""(.*)"", line (\d+), in ([\w_]+)")
            mcol = filepathregex.Matches(ErrorLog)

            Dim FilePath As String = ""
            Dim Line As String = ""
            If mcol.Count > 0 Then
                FilePath = mcol(0).Groups(1).Value
                Line = mcol(0).Groups(2).Value
                FilePath = FilePath.Split("\").Last.Split(".").First
            End If





            t.Add(New ErrorItem(FilePath, Description, Line))
        End If





        For Each win As Window In Application.Current.Windows
            If win.GetType Is GetType(TriggerEditor) Then
                Dim TEWindow As TriggerEditor = CType(win, TriggerEditor)

                TEWindow.ErrorListExpander.IsExpanded = True
                TEWindow.ErrorList.ItemsSource = t
            End If
        Next
    End Sub

    Public Class ErrorItem
        Public Property File As String
        Public Property Description As String
        Public Property Line As String


        Public TargetTEFile As TEFile

        Public Sub New(_File As String, _Description As String, _Line As String)
            Dim teFile As TEFile = GetTEFile(pjData.TEData.PFIles, _File)
            If teFile IsNot Nothing Then
                TargetTEFile = teFile
                Select Case teFile.FileType
                    Case TEFile.EFileType.CUIEps
                        Dim OrgStr As String = CType(teFile.Scripter, CUIScriptEditor).GetStringText
                        Dim BuildStr As String = CType(teFile.Scripter, CUIScriptEditor).LastBulidText


                        Dim OrgLines() As String = OrgStr.Split(vbCrLf)
                        Dim BuildLines() As String = BuildStr.Split(vbCrLf)


                        Dim bline As Integer = 0
                        For i = 0 To OrgLines.Count - 1
                            Dim s As Integer = bline + 1
                            Dim e As Integer

                            While OrgLines(i).Trim <> BuildLines(bline).Trim
                                bline += 1
                            End While
                            e = bline + 1
                            If s <= _Line And _Line <= e Then
                                _Line = i + 1
                                Exit For
                            End If
                        Next
                    Case TEFile.EFileType.ClassicTrigger
                        Dim scripter As ClassicTriggerEditor = CType(teFile.Scripter, ClassicTriggerEditor)

                        _Line = scripter.GetLine(_Line)
                End Select
            End If



            File = _File
            Description = _Description
            Line = _Line
        End Sub

        Private Function GetTEFile(tFile As TEFile, fName As String) As TEFile
            If tFile.IsFile Then
                If tFile.FileName = fName Then
                    Return tFile
                End If
            Else
                For i = 0 To tFile.FileCount - 1
                    Dim _t As TEFile = GetTEFile(tFile.Files(i), fName)
                    If _t IsNot Nothing Then
                        Return _t
                    End If
                Next
                For i = 0 To tFile.FolderCount - 1
                    Dim _t As TEFile = GetTEFile(tFile.Folders(i), fName)
                    If _t IsNot Nothing Then
                        Return _t
                    End If
                Next
            End If

            Return Nothing
        End Function


    End Class

End Module
