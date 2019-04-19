Public Class ImageData
    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.images

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        UsedCodeList.Init(DatFiles, ObjectID)

        NameBar.Init(ObjectID, DatFiles, 0)

        II.Init(DatFiles, ObjectID, II.Tag)
        GT.Init(DatFiles, ObjectID, GT.Tag)
        CA.Init(DatFiles, ObjectID, CA.Tag)
        UFI.Init(DatFiles, ObjectID, UFI.Tag)
        DIC.Init(DatFiles, ObjectID, DIC.Tag)
        DF.Init(DatFiles, ObjectID, DF.Tag)
        REMA.Init(DatFiles, ObjectID, REMA.Tag)

        GRPPlayer.Init(ObjectID, 0)


        Dim IscriptID As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.images, "Iscript ID", ObjectID)
        ImageScripts.Items.Clear()
        For i = 0 To scData.IscriptData.iscriptEntry(scData.IscriptData.key(IscriptID)).EntryType - 1
            ImageScripts.Items.Add(Format(scData.IscriptData.iscriptEntry(scData.IscriptData.key(IscriptID)).AnimHeader(i), "00000") & " " & IScript.HEADERNAME(i))
        Next
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, tObjectID As Integer)
        ObjectID = tObjectID

        UsedCodeList.ReLoad(DatFiles, ObjectID)

        NameBar.ReLoad(ObjectID, DatFiles, 0)

        II.ReLoad(DatFiles, ObjectID, II.Tag)
        GT.ReLoad(DatFiles, ObjectID, GT.Tag)
        CA.ReLoad(DatFiles, ObjectID, CA.Tag)
        UFI.ReLoad(DatFiles, ObjectID, UFI.Tag)
        DIC.ReLoad(DatFiles, ObjectID, DIC.Tag)
        DF.ReLoad(DatFiles, ObjectID, DF.Tag)
        REMA.ReLoad(DatFiles, ObjectID, REMA.Tag)

        GRPPlayer.Init(ObjectID, 0)

        Dim IscriptID As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.images, "Iscript ID", ObjectID)
        ImageScripts.Items.Clear()
        For i = 0 To scData.IscriptData.iscriptEntry(scData.IscriptData.key(IscriptID)).EntryType - 1
            ImageScripts.Items.Add(Format(scData.IscriptData.iscriptEntry(scData.IscriptData.key(IscriptID)).AnimHeader(i), "00000") & " " & IScript.HEADERNAME(i))
        Next
    End Sub

    Private Sub ImageScripts_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If ImageScripts.SelectedIndex <> -1 Then
            GRPPlayer.Init(ObjectID, ImageScripts.SelectedIndex)
        End If
    End Sub

    Private Sub II_ValueChange(sender As Object, e As RoutedEventArgs)
        GRPPlayer.Init(ObjectID, 0)

        Dim IscriptID As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.images, "Iscript ID", ObjectID)
        ImageScripts.Items.Clear()
        For i = 0 To scData.IscriptData.iscriptEntry(scData.IscriptData.key(IscriptID)).EntryType - 1
            ImageScripts.Items.Add(Format(scData.IscriptData.iscriptEntry(scData.IscriptData.key(IscriptID)).AnimHeader(i), "00000") & " " & IScript.HEADERNAME(i))
        Next
    End Sub
End Class
