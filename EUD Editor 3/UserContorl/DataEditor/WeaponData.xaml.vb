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

        DA.Init(DatFiles, ObjectID, DA.Tag, InputField.SFlag.None, 70)
        DB.Init(DatFiles, ObjectID, DB.Tag, InputField.SFlag.None, 70)
        DF.Init(DatFiles, ObjectID, DF.Tag, InputField.SFlag.None, 70)
        WC.Init(DatFiles, ObjectID, WC.Tag, InputField.SFlag.None, 70)

        WT.Init(DatFiles, ObjectID, WT.Tag, 70)
        ET.Init(DatFiles, ObjectID, ET.Tag, 70)

        UN.Init(DatFiles, ObjectID, UN.Tag)
        DU.Init(DatFiles, ObjectID, DU.Tag)


        MinR.Init(DatFiles, ObjectID, MinR.Tag)
        MaxR.Init(DatFiles, ObjectID, MaxR.Tag)

        ISR.Init(DatFiles, ObjectID, ISR.Tag)
        MSR.Init(DatFiles, ObjectID, MSR.Tag)
        OSR.Init(DatFiles, ObjectID, OSR.Tag)
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
    End Sub
End Class
