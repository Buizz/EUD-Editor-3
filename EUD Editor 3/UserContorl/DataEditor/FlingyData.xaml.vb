Public Class FlingyData
    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.flingy

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        UsedCodeList.Init(DatFiles, ObjectID)

        NameBar.Init(ObjectID, DatFiles, 0)

        SP.Init(DatFiles, ObjectID, SP.Tag)
        SPEED.Init(DatFiles, ObjectID, SPEED.Tag, InputField.SFlag.None)
        AC.Init(DatFiles, ObjectID, AC.Tag, InputField.SFlag.None)
        HD.Init(DatFiles, ObjectID, HD.Tag, InputField.SFlag.None)
        TR.Init(DatFiles, ObjectID, TR.Tag, InputField.SFlag.None)
        MC.Init(DatFiles, ObjectID, MC.Tag)
        UN.Init(DatFiles, ObjectID, UN.Tag, InputField.SFlag.None)



        Dim Sprite As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", ObjectID)
        Dim ImageID As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", Sprite)

        GRPImageBox.Init(ImageID, 0)
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, tObjectID As Integer)
        ObjectID = tObjectID

        UsedCodeList.ReLoad(DatFiles, ObjectID)

        NameBar.ReLoad(ObjectID, DatFiles, 0)

        SP.ReLoad(DatFiles, ObjectID, SP.Tag)
        SPEED.ReLoad(DatFiles, ObjectID, SPEED.Tag)
        AC.ReLoad(DatFiles, ObjectID, AC.Tag)
        HD.ReLoad(DatFiles, ObjectID, HD.Tag)
        TR.ReLoad(DatFiles, ObjectID, TR.Tag)
        MC.ReLoad(DatFiles, ObjectID, MC.Tag)
        UN.ReLoad(DatFiles, ObjectID, UN.Tag)

        Dim Sprite As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", ObjectID)
        Dim ImageID As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", Sprite)

        GRPImageBox.Init(ImageID, 0)
    End Sub

    Private Sub SP_ValueChange(sender As Object, e As RoutedEventArgs)
        Dim Sprite As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", ObjectID)
        Dim ImageID As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", Sprite)

        GRPImageBox.Init(ImageID, 0)
    End Sub
End Class
