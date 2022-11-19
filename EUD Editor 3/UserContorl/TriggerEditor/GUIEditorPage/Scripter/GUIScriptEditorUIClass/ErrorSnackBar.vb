Partial Public Class GUIScriptEditorUI
    Private Sub SnackBarDialog(str As String)
        sMessageQueue.Enqueue(str)
    End Sub
End Class
