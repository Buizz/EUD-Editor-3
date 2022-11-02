Public Class Unit_Sound
    Private Const UnitDatPage As Integer = 2

    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.units

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        NameBar.Init(ObjectID, SCDatFiles.DatFiles.units, UnitDatPage)

        RS.Init(DatFiles, ObjectID, RS.Tag, 100)
        YSS.Init(DatFiles, ObjectID, YSS.Tag, 100)
        YSE.Init(DatFiles, ObjectID, YSE.Tag, 100)
        WSS.Init(DatFiles, ObjectID, WSS.Tag, 100)
        WSE.Init(DatFiles, ObjectID, WSE.Tag, 100)
        PSS.Init(DatFiles, ObjectID, PSS.Tag, 100)
        PSE.Init(DatFiles, ObjectID, PSE.Tag, 100)
        'test.Text = pjData.Dat.Data(SCDatFiles.DatFiles.units, test.Tag, ObjectID)
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID

        NameBar.ReLoad(ObjectID, DatFiles, UnitDatPage)

        RS.ReLoad(DatFiles, ObjectID, RS.Tag)
        YSS.ReLoad(DatFiles, ObjectID, YSS.Tag)
        YSE.ReLoad(DatFiles, ObjectID, YSE.Tag)
        WSS.ReLoad(DatFiles, ObjectID, WSS.Tag)
        WSE.ReLoad(DatFiles, ObjectID, WSE.Tag)
        PSS.ReLoad(DatFiles, ObjectID, PSS.Tag)
        PSE.ReLoad(DatFiles, ObjectID, PSE.Tag)
    End Sub

End Class
