Public Class NameBar
    Private ObjectID As Integer
    Private DatFile As SCDatFiles.DatFiles

    Private ToolTipCommand As UICommand
    Private GroupCommand As UICommand

    Public Sub Init(tObjectID As Integer, tDatFile As SCDatFiles.DatFiles)
        ObjectID = tObjectID
        DatFile = tDatFile

        ToolTipCommand = New UICommand(DatFile, ObjectID, UICommand.EUIType.ToolTip)
        GroupCommand = New UICommand(DatFile, ObjectID, UICommand.EUIType.Group)

        'ObjName.Content = pjData.UnitName(ObjectID)
        'ObjToolTip.Text = pjData.Dat.ToolTip(DatFile, ObjectID)
        'ObjGroup.Text = pjData.Dat.Group(DatFile, ObjectID)

        ObjToolTip.DataContext = pjData.BindingManager.UIManager(DatFile, ObjectID)
        ObjGroup.DataContext = pjData.BindingManager.UIManager(DatFile, ObjectID)

        If True Then
            Dim CopyKeyGesture As KeyGesture = New KeyGesture(Key.C, ModifierKeys.Control, "Ctrl+C")
            Dim CopyKeybinding As New KeyBinding(ToolTipCommand, CopyKeyGesture) With {
                    .CommandParameter = UICommand.CommandType.Copy
                }
            ObjToolTip.InputBindings.Add(CopyKeybinding)

            Dim PasteKeyGesture As KeyGesture = New KeyGesture(Key.V, ModifierKeys.Control, "Ctrl+V")
            Dim PasteKeybinding As New KeyBinding(ToolTipCommand, PasteKeyGesture) With {
                    .CommandParameter = UICommand.CommandType.Paste
                }
            ObjToolTip.InputBindings.Add(PasteKeybinding)

            Dim ResetKeyGesture As KeyGesture = New KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R")
            Dim ResetKeybinding As New KeyBinding(ToolTipCommand, ResetKeyGesture) With {
                    .CommandParameter = UICommand.CommandType.Reset
                }
            ObjToolTip.InputBindings.Add(ResetKeybinding)


            TtCopyItem.Command = ToolTipCommand
            TtCopyItem.CommandParameter = UICommand.CommandType.Copy

            TtPasteItem.Command = ToolTipCommand
            TtPasteItem.CommandParameter = UICommand.CommandType.Paste

            TtResetItem.Command = ToolTipCommand
            TtResetItem.CommandParameter = UICommand.CommandType.Reset
        End If

        If True Then
            Dim CopyKeyGesture As KeyGesture = New KeyGesture(Key.C, ModifierKeys.Control, "Ctrl+C")
            Dim CopyKeybinding As New KeyBinding(GroupCommand, CopyKeyGesture) With {
                    .CommandParameter = UICommand.CommandType.Copy
                }
            ObjGroup.InputBindings.Add(CopyKeybinding)

            Dim PasteKeyGesture As KeyGesture = New KeyGesture(Key.V, ModifierKeys.Control, "Ctrl+V")
            Dim PasteKeybinding As New KeyBinding(GroupCommand, PasteKeyGesture) With {
                    .CommandParameter = UICommand.CommandType.Paste
                }
            ObjGroup.InputBindings.Add(PasteKeybinding)

            Dim ResetKeyGesture As KeyGesture = New KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R")
            Dim ResetKeybinding As New KeyBinding(GroupCommand, ResetKeyGesture) With {
                    .CommandParameter = UICommand.CommandType.Reset
                }
            ObjGroup.InputBindings.Add(ResetKeybinding)


            GCopyItem.Command = GroupCommand
            GCopyItem.CommandParameter = UICommand.CommandType.Copy

            GPasteItem.Command = GroupCommand
            GPasteItem.CommandParameter = UICommand.CommandType.Paste

            GResetItem.Command = GroupCommand
            GResetItem.CommandParameter = UICommand.CommandType.Reset
        End If
    End Sub


    Public Sub ReLoad(tObjectID As Integer, tDatFile As SCDatFiles.DatFiles)
        ObjectID = tObjectID
        DatFile = tDatFile

        ToolTipCommand.ReLoad(DatFile, ObjectID, UICommand.EUIType.ToolTip)
        GroupCommand.ReLoad(DatFile, ObjectID, UICommand.EUIType.Group)

        'ObjName.Content = pjData.UnitName(ObjectID)
        'ObjToolTip.Text = pjData.Dat.ToolTip(DatFile, ObjectID)
        'ObjGroup.Text = pjData.Dat.Group(DatFile, ObjectID)

        ObjToolTip.DataContext = pjData.BindingManager.UIManager(DatFile, ObjectID)
        ObjGroup.DataContext = pjData.BindingManager.UIManager(DatFile, ObjectID)
    End Sub

End Class
