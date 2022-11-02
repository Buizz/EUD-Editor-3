Public Class BoolSelecter
    Private DatFile As SCDatFiles.DatFiles
    Private ObjectID As Integer
    Private Parameter As String

    Private DatCommand As DatCommand

    Public Sub ReLoad(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        If True Then
            Dim tbind As New Binding
            tbind.Path = New PropertyPath("ToolTipText")
            chbox.SetBinding(CheckBox.ToolTipProperty, tbind)
            'ValueText.ToolTip = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).GetToolTip
        End If

        DataContext = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)
        DatCommand.ReLoad(DatFile, Parameter, ObjectID)
    End Sub

    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String, Optional TextWidth As Integer = 0)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        If TextWidth <> 0 Then
            tBorder.Margin = New Thickness(TextWidth, 5, 0, 3)
        End If
        'Field.Init(DatFile, ObjectID, Parameter)
        If True Then
            Dim tbind As New Binding
            tbind.Path = New PropertyPath("ToolTipText")
            chbox.SetBinding(CheckBox.ToolTipProperty, tbind)
            'ValueText.ToolTip = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).GetToolTip
        End If

        DataContext = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)
        DatCommand = New DatCommand(DatFile, Parameter, ObjectID)

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



    Private Sub OpenMenu(sender As Object, e As ContextMenuEventArgs) Handles chbox.ContextMenuOpening
        CopyItem.IsEnabled = DatCommand.IsEnabled(CopyItem.CommandParameter)
        PasteItem.IsEnabled = DatCommand.IsEnabled(PasteItem.CommandParameter)
        ResetItem.IsEnabled = DatCommand.IsEnabled(ResetItem.CommandParameter)
    End Sub
End Class
