Public Class ComboxSelecter
    Private DatFile As SCDatFiles.DatFiles
    Private ObjectID As Integer
    Private Parameter As String

    Private DatCommand As DatCommand

    Public Sub ReLoad(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        Field.ReLoad(DatFile, ObjectID, Parameter)
        Dim DB As DatBinding = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)

        DataContext = DB
        If True Then
            Dim tbind As New Binding
            tbind.Path = New PropertyPath("ToolTipText")
            MainComboBox.SetBinding(ComboBox.ToolTipProperty, tbind)
            'ValueText.ToolTip = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).GetToolTip
        End If

        DatCommand.ReLoad(DatFile, Parameter, ObjectID)
    End Sub
    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String, Optional TextWidth As Integer = 0)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        Field.Init(DatFile, ObjectID, Parameter, InputField.SFlag.None, TextWidth)

        Dim DB As DatBinding = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)
        DataContext = DB
        If True Then
            Dim tbind As New Binding
            tbind.Path = New PropertyPath("ToolTipText")
            MainComboBox.SetBinding(ComboBox.ToolTipProperty, tbind)
            'ValueText.ToolTip = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).GetToolTip
        End If

        Dim itmes As String() = DB.ComboxItems
        For i = 0 To itmes.Count - 1
            MainComboBox.Items.Add(itmes(i))
        Next

        DatCommand = New DatCommand(DatFile, Parameter, ObjectID)



        CopyItem.Command = DatCommand
        CopyItem.CommandParameter = DatCommand.CommandType.Copy

        PasteItem.Command = DatCommand
        PasteItem.CommandParameter = DatCommand.CommandType.Paste

        ResetItem.Command = DatCommand
        ResetItem.CommandParameter = DatCommand.CommandType.Reset
    End Sub
    Private Sub OpneMenu(sender As Object, e As ContextMenuEventArgs) Handles MainComboBox.ContextMenuOpening
        CopyItem.IsEnabled = DatCommand.IsEnabled(CopyItem.CommandParameter)
        PasteItem.IsEnabled = DatCommand.IsEnabled(PasteItem.CommandParameter)
        ResetItem.IsEnabled = DatCommand.IsEnabled(ResetItem.CommandParameter)
    End Sub

    Private Sub combobox_PreeviewMouseWheel(sender As Object, e As MouseWheelEventArgs) Handles MainComboBox.MouseWheel
        If Not MainComboBox.IsDropDownOpen Then
            Dim ChangeValue As Integer = e.Delta / 100
            pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).Value += ChangeValue
        End If
    End Sub
End Class
