Public Class UnitData
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        Unit_Default.ReLoad(DatFiles, ObjectID)
        Unit_Special.ReLoad(DatFiles, ObjectID)
        Unit_Sound.ReLoad(DatFiles, ObjectID)
        Unit_Graphic.ReLoad(DatFiles, ObjectID)
    End Sub

    Public Sub New(tObjectID As Integer)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        Unit_Default = New Unit_Default(tObjectID)
        Unit_Special = New Unit_Special(tObjectID)
        Unit_Sound = New Unit_Sound(tObjectID)
        Unit_Graphic = New Unit_Graphic(tObjectID)

        Defualt.Content = Unit_Default
        Special.Content = Unit_Special
        Sound.Content = Unit_Sound
        Graphic.Content = Unit_Graphic
    End Sub

    Private Unit_Default As Unit_Default
    Private Unit_Special As Unit_Special
    Private Unit_Sound As Unit_Sound
    Private Unit_Graphic As Unit_Graphic

    Private Sub ListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        MainTab.SelectedIndex = TypeListBox.SelectedIndex
    End Sub
End Class
