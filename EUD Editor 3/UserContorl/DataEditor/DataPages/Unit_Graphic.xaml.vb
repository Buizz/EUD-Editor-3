Public Class Unit_Graphic
    Private Const UnitDatPage As Integer = 3

    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.units

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        NameBar.Init(ObjectID, SCDatFiles.DatFiles.units, UnitDatPage)

        GRP.Init(DatFiles, ObjectID, GRP.Tag)

        CA.Init(DatFiles, ObjectID, CA.Tag)
        POR.Init(DatFiles, ObjectID, POR.Tag)
        EL.Init(DatFiles, ObjectID, EL.Tag)

        UD.Init(DatFiles, ObjectID, UD.Tag)

        USL.Init(DatFiles, ObjectID, USL.Tag)
        USR.Init(DatFiles, ObjectID, USR.Tag)
        USD.Init(DatFiles, ObjectID, USD.Tag)
        USU.Init(DatFiles, ObjectID, USU.Tag)

        SPBW.Init(DatFiles, ObjectID, SPBW.Tag)
        SPBH.Init(DatFiles, ObjectID, SPBH.Tag)
        AHXP.Init(DatFiles, ObjectID, AHXP.Tag)
        AVYP.Init(DatFiles, ObjectID, AVYP.Tag)
        'test.Text = pjData.Dat.Data(SCDatFiles.DatFiles.units, test.Tag, ObjectID)
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID

        NameBar.ReLoad(ObjectID, DatFiles, UnitDatPage)

        GRP.ReLoad(DatFiles, ObjectID, GRP.Tag)

        CA.ReLoad(DatFiles, ObjectID, CA.Tag)
        POR.ReLoad(DatFiles, ObjectID, POR.Tag)
        EL.ReLoad(DatFiles, ObjectID, EL.Tag)

        UD.ReLoad(DatFiles, ObjectID, UD.Tag)

        USL.ReLoad(DatFiles, ObjectID, USL.Tag)
        USR.ReLoad(DatFiles, ObjectID, USR.Tag)
        USD.ReLoad(DatFiles, ObjectID, USD.Tag)
        USU.ReLoad(DatFiles, ObjectID, USU.Tag)

        SPBW.ReLoad(DatFiles, ObjectID, SPBW.Tag)
        SPBH.ReLoad(DatFiles, ObjectID, SPBH.Tag)
        AHXP.ReLoad(DatFiles, ObjectID, AHXP.Tag)
        AVYP.ReLoad(DatFiles, ObjectID, AVYP.Tag)
    End Sub
End Class
