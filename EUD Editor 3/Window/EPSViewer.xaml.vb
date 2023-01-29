Public Class EPSViewer
    Public Sub New(tTEFile As TEFile)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.

        macro.macroErrorList.Clear()
        CodeText.Text = tTEFile.Scripter.GetFileText("")
        If macro.macroErrorList.Count <> 0 Then
            Dim m As String = ""
            For Each msg In macro.macroErrorList
                m = m + msg + vbCrLf
            Next
            Tool.ErrorMsgBox("Lua Script Inner Error", m)
        End If

        CodeText.TextEditor.IsReadOnly = True
    End Sub
End Class
