Public Class TechData
    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.techdata

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        UsedCodeList.Init(DatFiles, ObjectID)

        NameBar.Init(ObjectID, DatFiles, 0)

        ICO.Init(DatFiles, ObjectID, ICO.Tag)
        LAB.Init(DatFiles, ObjectID, LAB.Tag)
        MCB.Init(DatFiles, ObjectID, MCB.Tag)
        VCB.Init(DatFiles, ObjectID, VCB.Tag)
        RTB.Init(DatFiles, ObjectID, RTB.Tag)

        MR.Init(DatFiles, ObjectID, MR.Tag)
        RAC.Init(DatFiles, ObjectID, RAC.Tag)
        BF.Init(DatFiles, ObjectID, BF.Tag)
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID

        UsedCodeList.ReLoad(DatFiles, ObjectID)

        NameBar.ReLoad(ObjectID, DatFiles, 0)

        ICO.ReLoad(DatFiles, ObjectID, ICO.Tag)
        LAB.ReLoad(DatFiles, ObjectID, LAB.Tag)
        MCB.ReLoad(DatFiles, ObjectID, MCB.Tag)
        VCB.ReLoad(DatFiles, ObjectID, VCB.Tag)
        RTB.ReLoad(DatFiles, ObjectID, RTB.Tag)

        MR.ReLoad(DatFiles, ObjectID, MR.Tag)
        RAC.ReLoad(DatFiles, ObjectID, RAC.Tag)
        BF.ReLoad(DatFiles, ObjectID, BF.Tag)
    End Sub
End Class
