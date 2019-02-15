Public Class StatusInforData
    Private Const UnitDatPage As Integer = 6

    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.units

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        NameBar.Init(ObjectID, SCDatFiles.DatFiles.units, UnitDatPage)

        'CAI.Init(DatFiles, ObjectID, CAI.Tag, 80)
        'HAI.Init(DatFiles, ObjectID, HAI.Tag, 80)
        'RTI.Init(DatFiles, ObjectID, RTI.Tag, 80)
        'AU.Init(DatFiles, ObjectID, AU.Tag, 80)
        'AM.Init(DatFiles, ObjectID, AM.Tag, 80)

        'RCA.Init(DatFiles, ObjectID, RCA.Tag)
        'AI.Init(DatFiles, ObjectID, AI.Tag, 300)
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID

        NameBar.ReLoad(ObjectID, DatFiles, UnitDatPage)

        'CAI.ReLoad(DatFiles, ObjectID, CAI.Tag)
        'HAI.ReLoad(DatFiles, ObjectID, HAI.Tag)
        'RTI.ReLoad(DatFiles, ObjectID, RTI.Tag)
        'AU.ReLoad(DatFiles, ObjectID, AU.Tag)
        'AM.ReLoad(DatFiles, ObjectID, AM.Tag)

        'RCA.ReLoad(DatFiles, ObjectID, RCA.Tag)
        'AI.ReLoad(DatFiles, ObjectID, AI.Tag)
    End Sub
End Class
