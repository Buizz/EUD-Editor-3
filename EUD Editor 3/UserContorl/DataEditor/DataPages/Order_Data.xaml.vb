Public Class Order_Data
    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.orders

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        NameBar.Init(ObjectID, DatFiles, 0)

        TG.Init(DatFiles, ObjectID, TG.Tag, 100)
        EN.Init(DatFiles, ObjectID, EN.Tag, 100)
        OO.Init(DatFiles, ObjectID, OO.Tag, 100)
        LAB.Init(DatFiles, ObjectID, LAB.Tag, 100)
        ANI.Init(DatFiles, ObjectID, ANI.Tag, 100)
        HIGH.Init(DatFiles, ObjectID, HIGH.Tag, 100)

        FLAG1.Init(DatFiles, ObjectID, FLAG1.Tag)
        FLAG2.Init(DatFiles, ObjectID, FLAG2.Tag)
        FLAG3.Init(DatFiles, ObjectID, FLAG3.Tag)
        FLAG4.Init(DatFiles, ObjectID, FLAG4.Tag)
        FLAG5.Init(DatFiles, ObjectID, FLAG5.Tag)
        FLAG6.Init(DatFiles, ObjectID, FLAG6.Tag)
        FLAG7.Init(DatFiles, ObjectID, FLAG7.Tag)
        FLAG8.Init(DatFiles, ObjectID, FLAG8.Tag)
        FLAG9.Init(DatFiles, ObjectID, FLAG9.Tag)
        FLAG10.Init(DatFiles, ObjectID, FLAG10.Tag)
        FLAG11.Init(DatFiles, ObjectID, FLAG11.Tag)
        FLAG12.Init(DatFiles, ObjectID, FLAG12.Tag)
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID

        NameBar.ReLoad(ObjectID, DatFiles, 0)

        TG.ReLoad(DatFiles, ObjectID, TG.Tag)
        EN.ReLoad(DatFiles, ObjectID, EN.Tag)
        OO.ReLoad(DatFiles, ObjectID, OO.Tag)
        LAB.ReLoad(DatFiles, ObjectID, LAB.Tag)
        ANI.ReLoad(DatFiles, ObjectID, ANI.Tag)
        HIGH.ReLoad(DatFiles, ObjectID, HIGH.Tag)

        FLAG1.ReLoad(DatFiles, ObjectID, FLAG1.Tag)
        FLAG2.ReLoad(DatFiles, ObjectID, FLAG2.Tag)
        FLAG3.ReLoad(DatFiles, ObjectID, FLAG3.Tag)
        FLAG4.ReLoad(DatFiles, ObjectID, FLAG4.Tag)
        FLAG5.ReLoad(DatFiles, ObjectID, FLAG5.Tag)
        FLAG6.ReLoad(DatFiles, ObjectID, FLAG6.Tag)
        FLAG7.ReLoad(DatFiles, ObjectID, FLAG7.Tag)
        FLAG8.ReLoad(DatFiles, ObjectID, FLAG8.Tag)
        FLAG9.ReLoad(DatFiles, ObjectID, FLAG9.Tag)
        FLAG10.ReLoad(DatFiles, ObjectID, FLAG10.Tag)
        FLAG11.ReLoad(DatFiles, ObjectID, FLAG11.Tag)
        FLAG12.ReLoad(DatFiles, ObjectID, FLAG12.Tag)
    End Sub
End Class
