Public Class EPSViewer
    Public Sub New(tTEFile As TEFile)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.

        CodeText.Text = tTEFile.Scripter.GetFileText()
        CodeText.TextEditor.IsReadOnly = True
    End Sub
End Class
