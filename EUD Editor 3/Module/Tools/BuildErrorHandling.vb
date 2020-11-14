Imports System.Collections.ObjectModel
Imports System.Text.RegularExpressions

Module BuildErrorHandling



    Public Sub ErrorHandleClose()
        For Each win As Window In Application.Current.Windows
            If win.GetType Is GetType(TriggerEditor) Then
                Dim TEWindow As TriggerEditor = CType(win, TriggerEditor)

                TEWindow.ErrorListExpander.IsExpanded = False
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



        Public Sub New(_File As String, _Description As String, _Line As String)
            File = _File
            Description = _Description
            Line = _Line

        End Sub
    End Class

End Module
