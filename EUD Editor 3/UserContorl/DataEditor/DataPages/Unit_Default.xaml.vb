Public Class Unit_Default
    Private ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        test.Text = scData.DefaultDat.Data(SCDatFiles.DatFiles.units, test.Tag, ObjectID) + pjData.Dat.Data(SCDatFiles.DatFiles.units, test.Tag, ObjectID)
        NameBar.Init(ObjectID, SCDatFiles.DatFiles.units)
    End Sub

    Private Sub TTTextChange(sender As Object, e As TextChangedEventArgs) Handles test.TextChanged
        pjData.Dat.Data(SCDatFiles.DatFiles.units, test.Tag, ObjectID) = 10
    End Sub
End Class
