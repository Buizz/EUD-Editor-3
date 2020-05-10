Imports ICSharpCode.AvalonEdit.CodeCompletion

Public Class CodeTextEditor
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        InitTextEditor()
        DataCreate()
    End Sub

    Private TEFile As TEFile

    Public Event TextChange As RoutedEventHandler
    Private Sub TextEditor_TextChanged(sender As Object, e As EventArgs)
        RaiseEvent TextChange(Me, Nothing)
    End Sub


    Public Sub Init(tTEFile As TEFile)
        TEFile = tTEFile

    End Sub
    Public Property Text As String
        Get
            Return TextEditor.Text
        End Get
        Set(value As String)
            TextEditor.Text = value
        End Set
    End Property
    Public Sub ExternerLoader()

    End Sub

    Private CompletionData As New List(Of TriggerEditorCompletionData)
    Private Sub DataCreate()
        For i = 0 To 7000
            CompletionData.Add(New TriggerEditorCompletionData(0, "표시 : " & i, "입력 : " & i, "설명 : " & i, TextEditor, TECompletionData.EIconType.StarConst))
        Next
    End Sub


    Private completionWindow As CompletionWindow
    Private Sub textEditor_TextArea_TextEntered(sender As Object, e As TextCompositionEventArgs)
        If (e.Text = ".") Then
            'open code completion after the user has pressed dot
            completionWindow = New CompletionWindow(TextEditor.TextArea)
            'provide AvalonEdit with the data


            Dim data As IList(Of ICompletionData) = completionWindow.CompletionList.CompletionData
            For i = 0 To CompletionData.Count - 1
                data.Add(CompletionData(i))
            Next


            completionWindow.Show()
            AddHandler completionWindow.Closed, Sub()
                                                    completionWindow = Nothing
                                                End Sub
        End If
    End Sub
    Private Sub textEditor_TextArea_TextEntering(sender As Object, e As TextCompositionEventArgs)
        If (e.Text.Length > 0 And completionWindow IsNot Nothing) Then
            If (Not Char.IsLetterOrDigit(e.Text(0))) Then
                completionWindow.CompletionList.RequestInsertion(e)
            End If


        End If







    End Sub
End Class
