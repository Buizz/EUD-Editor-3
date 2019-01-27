Public Class ImageData
    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.images

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        NameBar.Init(ObjectID, DatFiles, 0)

        II.Init(DatFiles, ObjectID, II.Tag)
        GT.Init(DatFiles, ObjectID, GT.Tag)
        CA.Init(DatFiles, ObjectID, CA.Tag)
        UFI.Init(DatFiles, ObjectID, UFI.Tag)
        DIC.Init(DatFiles, ObjectID, DIC.Tag)
        DF.Init(DatFiles, ObjectID, DF.Tag)
        REMA.Init(DatFiles, ObjectID, REMA.Tag)
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID
        NameBar.ReLoad(ObjectID, DatFiles, 0)

        II.ReLoad(DatFiles, ObjectID, II.Tag)
        GT.ReLoad(DatFiles, ObjectID, GT.Tag)
        CA.ReLoad(DatFiles, ObjectID, CA.Tag)
        UFI.ReLoad(DatFiles, ObjectID, UFI.Tag)
        DIC.ReLoad(DatFiles, ObjectID, DIC.Tag)
        DF.ReLoad(DatFiles, ObjectID, DF.Tag)
        REMA.ReLoad(DatFiles, ObjectID, REMA.Tag)
    End Sub
End Class
