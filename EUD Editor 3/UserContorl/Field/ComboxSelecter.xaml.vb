Public Class ComboxSelecter
    Private DatFile As SCDatFiles.DatFiles
    Private ObjectID As Integer
    Private Parameter As String

    Private DatCommand As DatCommand

    Public Event ValueChange As RoutedEventHandler

    Public Sub ReLoad(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        Select Case DatFile
            Case SCDatFiles.DatFiles.statusinfor
                Field.ReLoad(DatFile, ObjectID, Parameter)

                DataContext = pjData.BindingManager.ExtraDatBinding(DatFile, Parameter, ObjectID)
                DatCommand.ReLoad(DatFile, Parameter, ObjectID)
            Case Else
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
        End Select
    End Sub

    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String, Optional TextWidth As Integer = 0)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter
        Select Case DatFile
            Case SCDatFiles.DatFiles.statusinfor
                Field.Init(DatFile, ObjectID, Parameter, InputField.SFlag.None, TextWidth)

                DataContext = pjData.BindingManager.ExtraDatBinding(DatFile, Parameter, ObjectID)

                Dim itmes As String() = Tool.GetText("FG_StatusInfor1_V").Split("|")
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
            Case Else
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
        End Select
    End Sub

    Private Sub OpneMenu(sender As Object, e As ContextMenuEventArgs) Handles MainComboBox.ContextMenuOpening
        CopyItem.IsEnabled = DatCommand.IsEnabled(CopyItem.CommandParameter)
        PasteItem.IsEnabled = DatCommand.IsEnabled(PasteItem.CommandParameter)
        ResetItem.IsEnabled = DatCommand.IsEnabled(ResetItem.CommandParameter)
    End Sub

    Private Sub combobox_PreeviewMouseWheel(sender As Object, e As MouseWheelEventArgs) Handles MainComboBox.MouseWheel
        If Not MainComboBox.IsDropDownOpen Then
            Dim ChangeValue As Integer = e.Delta / 100

            Select Case DatFile
                Case SCDatFiles.DatFiles.statusinfor
                    pjData.BindingManager.ExtraDatBinding(DatFile, Parameter, ObjectID).Value += ChangeValue
                Case Else
                    pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).Value += ChangeValue
            End Select
        End If
    End Sub

    Private Sub Field_ValueChange(sender As Object, e As RoutedEventArgs)
        RaiseEvent ValueChange(Me, e)
    End Sub
End Class
