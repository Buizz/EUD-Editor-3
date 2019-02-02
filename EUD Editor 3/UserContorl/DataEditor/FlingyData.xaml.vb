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

        SP.Init(DatFiles, ObjectID, SP.Tag, 100)
        SPEED.Init(DatFiles, ObjectID, SPEED.Tag, InputField.SFlag.None, 100)
        AC.Init(DatFiles, ObjectID, AC.Tag, InputField.SFlag.None, 100)
        HD.Init(DatFiles, ObjectID, HD.Tag, InputField.SFlag.None, 100)
        TR.Init(DatFiles, ObjectID, TR.Tag, InputField.SFlag.None, 100)
        MC.Init(DatFiles, ObjectID, MC.Tag, 100)
        UN.Init(DatFiles, ObjectID, UN.Tag, InputField.SFlag.None, 100)
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID

        UsedCodeList.ReLoad(DatFiles, ObjectID)

        NameBar.ReLoad(ObjectID, DatFiles, 0)

        SP.ReLoad(DatFiles, ObjectID, SP.Tag)
        SPEED.ReLoad(DatFiles, ObjectID, SPEED.Tag)
        AC.ReLoad(DatFiles, ObjectID, AC.Tag)
        HD.ReLoad(DatFiles, ObjectID, HD.Tag)
        TR.ReLoad(DatFiles, ObjectID, TR.Tag)
        MC.ReLoad(DatFiles, ObjectID, MC.Tag)
        UN.ReLoad(DatFiles, ObjectID, UN.Tag)
    End Sub
End Class
