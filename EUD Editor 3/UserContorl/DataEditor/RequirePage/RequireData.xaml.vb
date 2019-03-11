Public Class RequireData
    Private Const UnitDatPage As Integer = 6

    Private DatFiles As SCDatFiles.DatFiles
    Private RealDatFiles As SCDatFiles.DatFiles
    Private IsUseRequire As Boolean = False

    Public Property ObjectID As Integer


    Public Sub New(tDatFiles As SCDatFiles.DatFiles, tObjectID As Integer, Optional tIsUseRequire As Boolean = False)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DatFiles = tDatFiles
        ObjectID = tObjectID
        IsUseRequire = tIsUseRequire

        Select Case DatFiles
            Case SCDatFiles.DatFiles.Stechdata
                RealDatFiles = SCDatFiles.DatFiles.techdata
            Case Else
                RealDatFiles = DatFiles
        End Select


        DataContext = pjData

        NameBar.Init(ObjectID, DatFiles, UnitDatPage)
        If tIsUseRequire Then
            RequireListbox.Init(SCDatFiles.DatFiles.Stechdata, ObjectID, RequireListbox.Tag)
        Else
            RequireListbox.Init(DatFiles, ObjectID, RequireListbox.Tag)
        End If


        'CAI.Init(DatFiles, ObjectID, CAI.Tag, 80)
        'HAI.Init(DatFiles, ObjectID, HAI.Tag, 80)
        'RTI.Init(DatFiles, ObjectID, RTI.Tag, 80)
        'AU.Init(DatFiles, ObjectID, AU.Tag, 80)
        'AM.Init(DatFiles, ObjectID, AM.Tag, 80)

        'RCA.Init(DatFiles, ObjectID, RCA.Tag)
        'AI.Init(DatFiles, ObjectID, AI.Tag, 300)
    End Sub
    Public Sub ReLoad(tDatFiles As SCDatFiles.DatFiles, tObjectID As Integer, Optional tIsUseRequire As Boolean = False)
        DatFiles = tDatFiles
        ObjectID = tObjectID
        IsUseRequire = tIsUseRequire

        Select Case DatFiles
            Case SCDatFiles.DatFiles.Stechdata
                RealDatFiles = SCDatFiles.DatFiles.techdata
            Case Else
                RealDatFiles = DatFiles
        End Select

        NameBar.ReLoad(ObjectID, DatFiles, UnitDatPage)
        If tIsUseRequire Then
            RequireListbox.ReLoad(SCDatFiles.DatFiles.Stechdata, ObjectID, RequireListbox.Tag)
        Else
            RequireListbox.ReLoad(DatFiles, ObjectID, RequireListbox.Tag)
        End If
        'CAI.ReLoad(DatFiles, ObjectID, CAI.Tag)
        'HAI.ReLoad(DatFiles, ObjectID, HAI.Tag)
        'RTI.ReLoad(DatFiles, ObjectID, RTI.Tag)
        'AU.ReLoad(DatFiles, ObjectID, AU.Tag)
        'AM.ReLoad(DatFiles, ObjectID, AM.Tag)

        'RCA.ReLoad(DatFiles, ObjectID, RCA.Tag)
        'AI.ReLoad(DatFiles, ObjectID, AI.Tag)
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        For i = 0 To SCCodeCount(RealDatFiles) - 1
            pjData.BindingManager.RequireDataBinding(i, DatFiles).IsDefaultUse = True
        Next
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        For i = 0 To SCCodeCount(RealDatFiles) - 1
            pjData.BindingManager.RequireDataBinding(i, DatFiles).IsDontUse = True
        Next
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        For i = 0 To SCCodeCount(RealDatFiles) - 1
            pjData.BindingManager.RequireDataBinding(i, DatFiles).IsAlwaysUse = True
        Next
    End Sub

    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        For i = 0 To SCCodeCount(RealDatFiles) - 1
            pjData.BindingManager.RequireDataBinding(i, DatFiles).IsAlwaysCurrentUse = True
        Next
    End Sub
End Class
