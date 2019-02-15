Public Class TechData
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        UsedCodeList.ReLoad(SCDatFiles.DatFiles.techdata, ObjectID)

        Tech_Data.ReLoad(DatFiles, ObjectID)
        RequireUseData.ReLoad(DatFiles, ObjectID, True)
        RequireReserachData.ReLoad(DatFiles, ObjectID)

        TypeListBox.DataContext = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.techdata, ObjectID)
    End Sub

    Public Sub New(tObjectID As Integer)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        UsedCodeList.Init(SCDatFiles.DatFiles.techdata, tObjectID)

        Tech_Data = New Tech_Data(tObjectID)
        RequireUseData = New RequireData(SCDatFiles.DatFiles.techdata, tObjectID, True)
        RequireReserachData = New RequireData(SCDatFiles.DatFiles.techdata, tObjectID)

        Defualt.Content = Tech_Data
        ReserachRequir.Content = RequireUseData
        UseRequir.Content = RequireReserachData

        TypeListBox.DataContext = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.techdata, tObjectID)
    End Sub

    Private Tech_Data As Tech_Data
    Private RequireUseData As RequireData
    Private RequireReserachData As RequireData

    Private Sub ListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        MainTab.SelectedIndex = TypeListBox.SelectedIndex
    End Sub
End Class
