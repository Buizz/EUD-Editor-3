Public Class BoolSelecter
    Private DatFile As SCDatFiles.DatFiles
    Private ObjectID As Integer
    Private Parameter As String

    Private DatCommand As DatCommand

    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        'Field.Init(DatFile, ObjectID, Parameter)

        DatCommand = New DatCommand(DatFile, Parameter, ObjectID)
        DataContext = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)

        Dim CopyKeyGesture As KeyGesture = New KeyGesture(Key.C, ModifierKeys.Control, "Ctrl+C")
        Dim CopyKeybinding As New KeyBinding(DatCommand, CopyKeyGesture) With {
                .CommandParameter = DatCommand.CommandType.Copy
            }
        chbox.InputBindings.Add(CopyKeybinding)

        Dim PasteKeyGesture As KeyGesture = New KeyGesture(Key.V, ModifierKeys.Control, "Ctrl+V")
        Dim PasteKeybinding As New KeyBinding(DatCommand, PasteKeyGesture) With {
                .CommandParameter = DatCommand.CommandType.Paste
            }
        chbox.InputBindings.Add(PasteKeybinding)

        Dim ResetKeyGesture As KeyGesture = New KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R")
        Dim ResetKeybinding As New KeyBinding(DatCommand, ResetKeyGesture) With {
                .CommandParameter = DatCommand.CommandType.Reset
            }
        chbox.InputBindings.Add(ResetKeybinding)



        CopyItem.Command = DatCommand
        CopyItem.CommandParameter = DatCommand.CommandType.Copy

        PasteItem.Command = DatCommand
        PasteItem.CommandParameter = DatCommand.CommandType.Paste

        ResetItem.Command = DatCommand
        ResetItem.CommandParameter = DatCommand.CommandType.Reset
    End Sub



    Private Sub OpneMenu(sender As Object, e As ContextMenuEventArgs) Handles chbox.ContextMenuOpening
        CopyItem.IsEnabled = DatCommand.IsEnabled(CopyItem.CommandParameter)
        PasteItem.IsEnabled = DatCommand.IsEnabled(PasteItem.CommandParameter)
        ResetItem.IsEnabled = DatCommand.IsEnabled(ResetItem.CommandParameter)
    End Sub
End Class
