Public Class UnitData
    Public Sub New(tObjectID As Integer)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        Defualt.Content = New Unit_Default(tObjectID)
    End Sub
End Class
