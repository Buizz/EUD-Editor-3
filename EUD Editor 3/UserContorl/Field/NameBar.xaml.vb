Public Class NameBar
    Private ObjectID As Integer
    Private DatFile As SCDatFiles.DatFiles
    Private UnitDatPage As Integer

    Private ToolTipCommand As UICommand
    Private GroupCommand As UICommand

    Public Sub Init(tObjectID As Integer, tDatFile As SCDatFiles.DatFiles, tUnitDatPage As Integer)
        ObjectID = tObjectID
        DatFile = tDatFile
        UnitDatPage = tUnitDatPage

        ToolTipCommand = New UICommand(DatFile, ObjectID, UICommand.EUIType.ToolTip)
        GroupCommand = New UICommand(DatFile, ObjectID, UICommand.EUIType.Group)

        'ObjName.Content = pjData.UnitName(ObjectID)
        'ObjToolTip.Text = pjData.Dat.ToolTip(DatFile, ObjectID)
        'ObjGroup.Text = pjData.Dat.Group(DatFile, ObjectID)

        ObjToolTip.DataContext = pjData.BindingManager.UIManager(DatFile, ObjectID)
        ObjGroup.DataContext = pjData.BindingManager.UIManager(DatFile, ObjectID)

        If True Then
            Dim ResetKeyGesture As KeyGesture = New KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R")
            Dim ResetKeybinding As New KeyBinding(ToolTipCommand, ResetKeyGesture) With {
                    .CommandParameter = UICommand.CommandType.Reset
                }
            ObjToolTip.InputBindings.Add(ResetKeybinding)


            TtResetItem.Command = ToolTipCommand
            TtResetItem.CommandParameter = UICommand.CommandType.Reset
        End If

        If True Then
            Dim ResetKeyGesture As KeyGesture = New KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R")
            Dim ResetKeybinding As New KeyBinding(GroupCommand, ResetKeyGesture) With {
                    .CommandParameter = UICommand.CommandType.Reset
                }
            ObjGroup.InputBindings.Add(ResetKeybinding)

            GResetItem.Command = GroupCommand
            GResetItem.CommandParameter = UICommand.CommandType.Reset
        End If
    End Sub


    Public Sub ReLoad(tObjectID As Integer, tDatFile As SCDatFiles.DatFiles, tUnitDatPage As Integer)
        ObjectID = tObjectID
        DatFile = tDatFile
        UnitDatPage = tUnitDatPage

        ToolTipCommand.ReLoad(DatFile, ObjectID, UICommand.EUIType.ToolTip)
        GroupCommand.ReLoad(DatFile, ObjectID, UICommand.EUIType.Group)

        'ObjName.Content = pjData.UnitName(ObjectID)
        'ObjToolTip.Text = pjData.Dat.ToolTip(DatFile, ObjectID)
        'ObjGroup.Text = pjData.Dat.Group(DatFile, ObjectID)

        ObjToolTip.DataContext = pjData.BindingManager.UIManager(DatFile, ObjectID)
        ObjGroup.DataContext = pjData.BindingManager.UIManager(DatFile, ObjectID)
    End Sub

    Private Sub DataPageCopyClick(sender As Object, e As RoutedEventArgs)
        pjData.DataManager.CopyDatPage(DatFile, ObjectID)
    End Sub

    Private Sub DataPagePasteClick(sender As Object, e As RoutedEventArgs)
        pjData.DataManager.PasteDatPage(DatFile, ObjectID, UnitDatPage)
    End Sub

    Private Sub DataPageResetClick(sender As Object, e As RoutedEventArgs)
        pjData.DataManager.ResetDatPage(DatFile, ObjectID, UnitDatPage)
    End Sub

End Class
