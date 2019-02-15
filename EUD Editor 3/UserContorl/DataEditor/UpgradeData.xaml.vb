Public Class UpgradeData
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        UsedCodeList.ReLoad(SCDatFiles.DatFiles.upgrades, ObjectID)

        Upgrade_Data.ReLoad(DatFiles, ObjectID)
        RequireData.ReLoad(DatFiles, ObjectID)

        TypeListBox.DataContext = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.upgrades, ObjectID)
    End Sub

    Public Sub New(tObjectID As Integer)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        UsedCodeList.Init(SCDatFiles.DatFiles.upgrades, tObjectID)

        Upgrade_Data = New Upgrade_Data(tObjectID)
        RequireData = New RequireData(SCDatFiles.DatFiles.upgrades, tObjectID)

        Defualt.Content = Upgrade_Data
        Requir.Content = RequireData

        TypeListBox.DataContext = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.upgrades, tObjectID)
    End Sub

    Private Upgrade_Data As Upgrade_Data
    Private RequireData As RequireData

    Private Sub ListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        MainTab.SelectedIndex = TypeListBox.SelectedIndex
    End Sub
End Class
