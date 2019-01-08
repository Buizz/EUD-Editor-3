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

        'test.Text = pjData.Dat.Data(SCDatFiles.DatFiles.units, test.Tag, ObjectID)



    End Sub


End Class
