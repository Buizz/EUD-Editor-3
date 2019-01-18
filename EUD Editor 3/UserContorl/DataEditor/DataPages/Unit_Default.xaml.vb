Public Class Unit_Default
    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.units

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        NameBar.Init(ObjectID, SCDatFiles.DatFiles.units)
        HP.Init(DatFiles, ObjectID, HP.Tag, InputField.SFlag.HP)
        HP2.Init(DatFiles, ObjectID, HP.Tag, InputField.SFlag.HPV)
        SA.Init(DatFiles, ObjectID, SA.Tag)
        SE.Init(DatFiles, ObjectID, SE.Tag)
        AU.Init(DatFiles, ObjectID, AU.Tag)
        AM.Init(DatFiles, ObjectID, AM.Tag)


        MC.Init(DatFiles, ObjectID, MC.Tag)
        VC.Init(DatFiles, ObjectID, VC.Tag)
        BT.Init(DatFiles, ObjectID, BT.Tag)
        BUF.Init(DatFiles, ObjectID, BUF.Tag)


        GW.Init(DatFiles, ObjectID, GW.Tag)
        MGH.Init(DatFiles, ObjectID, MGH.Tag)
        AW.Init(DatFiles, ObjectID, AW.Tag)
        MAH.Init(DatFiles, ObjectID, MAH.Tag)

        SP.Init(DatFiles, ObjectID, SP.Tag)
        SR.Init(DatFiles, ObjectID, SR.Tag)

        SPP.Init(DatFiles, ObjectID, SPP.Tag)
        SPR.Init(DatFiles, ObjectID, SPR.Tag)

        BS.Init(DatFiles, ObjectID, BS.Tag)
        DS.Init(DatFiles, ObjectID, DS.Tag)

        US.Init(DatFiles, ObjectID, US.Tag)
        SIR.Init(DatFiles, ObjectID, SIR.Tag)
        TAR.Init(DatFiles, ObjectID, TAR.Tag)
        'test.Text = pjData.Dat.Data(SCDatFiles.DatFiles.units, test.Tag, ObjectID)
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID

        NameBar.ReLoad(ObjectID, DatFiles)
        HP.ReLoad(DatFiles, ObjectID, HP.Tag, InputField.SFlag.HP)
        HP2.ReLoad(DatFiles, ObjectID, HP.Tag, InputField.SFlag.HPV)
        SA.ReLoad(DatFiles, ObjectID, SA.Tag)
        SE.ReLoad(DatFiles, ObjectID, SE.Tag)
        AU.ReLoad(DatFiles, ObjectID, AU.Tag)
        AM.ReLoad(DatFiles, ObjectID, AM.Tag)


        MC.ReLoad(DatFiles, ObjectID, MC.Tag)
        VC.ReLoad(DatFiles, ObjectID, VC.Tag)
        BT.ReLoad(DatFiles, ObjectID, BT.Tag)
        BUF.ReLoad(DatFiles, ObjectID, BUF.Tag)


        GW.ReLoad(DatFiles, ObjectID, GW.Tag)
        MGH.ReLoad(DatFiles, ObjectID, MGH.Tag)
        AW.ReLoad(DatFiles, ObjectID, AW.Tag)
        MAH.ReLoad(DatFiles, ObjectID, MAH.Tag)

        SP.ReLoad(DatFiles, ObjectID, SP.Tag)
        SR.ReLoad(DatFiles, ObjectID, SR.Tag)

        SPP.ReLoad(DatFiles, ObjectID, SPP.Tag)
        SPR.ReLoad(DatFiles, ObjectID, SPR.Tag)

        BS.ReLoad(DatFiles, ObjectID, BS.Tag)
        DS.ReLoad(DatFiles, ObjectID, DS.Tag)

        US.ReLoad(DatFiles, ObjectID, US.Tag)
        SIR.ReLoad(DatFiles, ObjectID, SIR.Tag)
        TAR.ReLoad(DatFiles, ObjectID, TAR.Tag)
    End Sub

End Class
