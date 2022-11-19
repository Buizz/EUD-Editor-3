Public Class OrderData
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        UsedCodeList.ReLoad(SCDatFiles.DatFiles.orders, ObjectID)

        Order_Data.ReLoad(DatFiles, ObjectID)
        RequireData.ReLoad(DatFiles, ObjectID)

        TypeListBox.DataContext = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.orders, ObjectID)
    End Sub

    Public Sub New(tObjectID As Integer)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        UsedCodeList.Init(SCDatFiles.DatFiles.orders, tObjectID)

        Order_Data = New Order_Data(tObjectID)
        RequireData = New RequireData(SCDatFiles.DatFiles.orders, tObjectID)

        _Default.Content = Order_Data
        Requir.Content = RequireData

        TypeListBox.DataContext = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.orders, tObjectID)
    End Sub

    Private Order_Data As Order_Data
    Private RequireData As RequireData

    Private Sub ListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        MainTab.SelectedIndex = TypeListBox.SelectedIndex
    End Sub
End Class
