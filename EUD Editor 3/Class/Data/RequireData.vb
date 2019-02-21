Public Class RequireData
    Private Datfile As SCDatFiles.DatFiles


    Private ReauireDatas As List(Of RequireObject)



    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.

    End Sub







    Private Class RequireObject
        Private ReauireBlocks As List(Of RequireObject)
    End Class
    Private Class RequireBlock


    End Class
End Class
