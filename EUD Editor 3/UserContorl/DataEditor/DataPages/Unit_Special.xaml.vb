Public Class Unit_Special
    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.units

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        NameBar.Init(ObjectID, SCDatFiles.DatFiles.units)

        SAF.Init(DatFiles, ObjectID, SAF.Tag, 135)

        IFE.Init(DatFiles, ObjectID, IFE.Tag)
        S1.Init(DatFiles, ObjectID, S1.Tag)
        S2.Init(DatFiles, ObjectID, S2.Tag)

        UDM.Init(DatFiles, ObjectID, UDM.Tag, 75)
        'test.Text = pjData.Dat.Data(SCDatFiles.DatFiles.units, test.Tag, ObjectID)
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID

        NameBar.ReLoad(ObjectID, DatFiles)

        SAF.ReLoad(DatFiles, ObjectID, SAF.Tag)

        IFE.ReLoad(DatFiles, ObjectID, IFE.Tag)
        S1.ReLoad(DatFiles, ObjectID, S1.Tag)
        S2.ReLoad(DatFiles, ObjectID, S2.Tag)

        UDM.ReLoad(DatFiles, ObjectID, UDM.Tag)
    End Sub

End Class
