Public Class UnitData
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        UsedCodeList.ReLoad(SCDatFiles.DatFiles.units, ObjectID)

        Unit_Default.ReLoad(DatFiles, ObjectID)
        Unit_Special.ReLoad(DatFiles, ObjectID)
        Unit_Sound.ReLoad(DatFiles, ObjectID)
        Unit_Graphic.ReLoad(DatFiles, ObjectID)
        Unit_MapEdit.ReLoad(DatFiles, ObjectID)
        Unit_AIOrder.ReLoad(DatFiles, ObjectID)
        StatusInforData.ReLoad(DatFiles, ObjectID)
        Unit_WireFrame.ReLoad(DatFiles, ObjectID)
        RequireData.ReLoad(DatFiles, ObjectID)

        TypeListBox.DataContext = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.units, ObjectID)
    End Sub

    Public Sub New(tObjectID As Integer)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        UsedCodeList.Init(SCDatFiles.DatFiles.units, tObjectID)

        Unit_Default = New Unit_Default(tObjectID)
        Unit_Special = New Unit_Special(tObjectID)
        Unit_Sound = New Unit_Sound(tObjectID)
        Unit_Graphic = New Unit_Graphic(tObjectID)
        Unit_MapEdit = New Unit_MapEdit(tObjectID)
        Unit_AIOrder = New Unit_AIOrder(tObjectID)
        StatusInforData = New StatusInforData(tObjectID)
        Unit_WireFrame = New Unit_WireFrame(tObjectID)
        RequireData = New RequireData(SCDatFiles.DatFiles.units, tObjectID)

        Defualt.Content = Unit_Default
        Special.Content = Unit_Special
        Sound.Content = Unit_Sound
        Graphic.Content = Unit_Graphic
        MapEdit.Content = Unit_MapEdit
        AIOrder.Content = Unit_AIOrder
        SInforData.Content = StatusInforData
        WireFrame.Content = Unit_WireFrame
        Requir.Content = RequireData

        TypeListBox.DataContext = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.units, tObjectID)
    End Sub

    Private Unit_Default As Unit_Default
    Private Unit_Special As Unit_Special
    Private Unit_Sound As Unit_Sound
    Private Unit_Graphic As Unit_Graphic
    Private Unit_MapEdit As Unit_MapEdit
    Private Unit_AIOrder As Unit_AIOrder
    Private StatusInforData As StatusInforData
    Private Unit_WireFrame As Unit_WireFrame
    Private RequireData As RequireData

    Private Sub ListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        MainTab.SelectedIndex = TypeListBox.SelectedIndex
    End Sub
End Class
