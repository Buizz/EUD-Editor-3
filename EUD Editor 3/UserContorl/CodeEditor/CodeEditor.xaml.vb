Public Class CodeEditor
    Public Property Text As String
        Get
            Return TextEditor.Text
        End Get
        Set(value As String)
            TextEditor.Text = value
        End Set
    End Property

    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        InitTextEditor()
    End Sub
End Class
