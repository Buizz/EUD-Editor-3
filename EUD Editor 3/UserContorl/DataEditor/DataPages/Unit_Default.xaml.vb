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
        HP.Init(DatFiles, ObjectID, HP.Tag)
        SA.Init(DatFiles, ObjectID, SA.Tag)
        SE.Init(DatFiles, ObjectID, SE.Tag)
        AU.Init(DatFiles, ObjectID, AU.Tag)
        AM.Init(DatFiles, ObjectID, AM.Tag)



        'test.Text = pjData.Dat.Data(SCDatFiles.DatFiles.units, test.Tag, ObjectID)



    End Sub


End Class
