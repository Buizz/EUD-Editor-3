Public Class Unit_MapEdit
    Private Const UnitDatPage As Integer = 4

    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.units

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        NameBar.Init(ObjectID, SCDatFiles.DatFiles.units, UnitDatPage)

        SAF.Init(DatFiles, ObjectID, SAF.Tag, 180)

        SGF.Init(DatFiles, ObjectID, SGF.Tag, 130)
        RS.Init(DatFiles, ObjectID, RS.Tag)
        'test.Text = pjData.Dat.Data(SCDatFiles.DatFiles.units, test.Tag, ObjectID)
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID

        NameBar.ReLoad(ObjectID, DatFiles, UnitDatPage)

        SAF.ReLoad(DatFiles, ObjectID, SAF.Tag)

        SGF.ReLoad(DatFiles, ObjectID, SGF.Tag)
        RS.ReLoad(DatFiles, ObjectID, RS.Tag)
    End Sub
End Class
