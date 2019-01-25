Public Class WeaponData
    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.weapons

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        NameBar.Init(ObjectID, DatFiles, 0)

        DA.Init(DatFiles, ObjectID, DA.Tag)
        DB.Init(DatFiles, ObjectID, DB.Tag)
        WC.Init(DatFiles, ObjectID, WC.Tag)
        DF.Init(DatFiles, ObjectID, DF.Tag)

        WT.Init(DatFiles, ObjectID, WT.Tag, 60)
        ET.Init(DatFiles, ObjectID, ET.Tag, 60)

        UN.Init(DatFiles, ObjectID, UN.Tag)
        DU.Init(DatFiles, ObjectID, DU.Tag)


        MinR.Init(DatFiles, ObjectID, MinR.Tag)
        MaxR.Init(DatFiles, ObjectID, MaxR.Tag)

        ISR.Init(DatFiles, ObjectID, ISR.Tag)
        MSR.Init(DatFiles, ObjectID, MSR.Tag)
        OSR.Init(DatFiles, ObjectID, OSR.Tag)

        LAL.Init(DatFiles, ObjectID, LAL.Tag)
        TEM.Init(DatFiles, ObjectID, TEM.Tag)

        WB.Init(DatFiles, ObjectID, WB.Tag)
        GRP.Init(DatFiles, ObjectID, GRP.Tag)
        ICON.Init(DatFiles, ObjectID, ICON.Tag)
        RA.Init(DatFiles, ObjectID, RA.Tag)

        AA.Init(DatFiles, ObjectID, AA.Tag)
        LS.Init(DatFiles, ObjectID, LS.Tag)
        FO.Init(DatFiles, ObjectID, FO.Tag)
        UO.Init(DatFiles, ObjectID, UO.Tag)

        TF.Init(DatFiles, ObjectID, TF.Tag, 90)
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID
        NameBar.ReLoad(ObjectID, DatFiles, 0)

        DA.ReLoad(DatFiles, ObjectID, DA.Tag)
        DB.ReLoad(DatFiles, ObjectID, DB.Tag)
        DF.ReLoad(DatFiles, ObjectID, DF.Tag)
        WC.ReLoad(DatFiles, ObjectID, WC.Tag)

        WT.ReLoad(DatFiles, ObjectID, WT.Tag)
        ET.ReLoad(DatFiles, ObjectID, ET.Tag)

        UN.ReLoad(DatFiles, ObjectID, UN.Tag)
        DU.ReLoad(DatFiles, ObjectID, DU.Tag)


        MinR.ReLoad(DatFiles, ObjectID, MinR.Tag)
        MaxR.ReLoad(DatFiles, ObjectID, MaxR.Tag)

        ISR.ReLoad(DatFiles, ObjectID, ISR.Tag)
        MSR.ReLoad(DatFiles, ObjectID, MSR.Tag)
        OSR.ReLoad(DatFiles, ObjectID, OSR.Tag)

        LAL.ReLoad(DatFiles, ObjectID, LAL.Tag)
        TEM.ReLoad(DatFiles, ObjectID, TEM.Tag)

        WB.ReLoad(DatFiles, ObjectID, WB.Tag)
        GRP.ReLoad(DatFiles, ObjectID, GRP.Tag)
        ICON.ReLoad(DatFiles, ObjectID, ICON.Tag)
        RA.ReLoad(DatFiles, ObjectID, RA.Tag)

        AA.ReLoad(DatFiles, ObjectID, AA.Tag)
        LS.ReLoad(DatFiles, ObjectID, LS.Tag)
        FO.ReLoad(DatFiles, ObjectID, FO.Tag)
        UO.ReLoad(DatFiles, ObjectID, UO.Tag)

        TF.ReLoad(DatFiles, ObjectID, TF.Tag)
    End Sub
End Class
