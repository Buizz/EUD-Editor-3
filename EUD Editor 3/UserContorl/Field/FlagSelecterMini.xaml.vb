Public Class FlagSelecterMini
    Private DatFile As SCDatFiles.DatFiles
    Private ObjectID As Integer
    Private Parameter As String

    Private DatCommand As DatCommand

    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String, ItemWidth As Integer)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        Field.Init(DatFile, ObjectID, Parameter, InputField.SFlag.FLAG)

        Dim DB As DatBinding = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)
        DataContext = DB
        If True Then
            Dim tbind As New Binding
            tbind.Path = New PropertyPath("ToolTipText")
            CheckboxList.SetBinding(CheckboxList.ToolTipProperty, tbind)
            'ValueText.ToolTip = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).GetToolTip
        End If
        CheckboxList.Children.Clear()
        Dim RelaWidth As Integer = ItemWidth
        Dim itmes As String() = DB.ComboxItems
        For i = 0 To itmes.Count - 1
            Dim tBorder As New Border


            Dim tcheckBox As New CheckBox
            tcheckBox.Content = itmes(i)
            tcheckBox.Width = RelaWidth
            tcheckBox.Foreground = Application.Current.Resources("MaterialDesignBody")

            tBorder.DataContext = DB.GetFlagBinding(i)
            Dim Binding As New Binding
            Binding.Path = New PropertyPath("MiniFlag")
            tcheckBox.SetBinding(CheckBox.IsCheckedProperty, Binding)


            tBorder.Child = tcheckBox
            Dim Binding2 As New Binding
            Binding2.Path = New PropertyPath("MiniBackColor")
            tBorder.SetBinding(Border.BackgroundProperty, Binding2)


            CheckboxList.Children.Add(tBorder)
        Next
        DatCommand = New DatCommand(DatFile, Parameter, ObjectID)




        CopyItem.Command = DatCommand
        CopyItem.CommandParameter = DatCommand.CommandType.Copy

        PasteItem.Command = DatCommand
        PasteItem.CommandParameter = DatCommand.CommandType.Paste

        ResetItem.Command = DatCommand
        ResetItem.CommandParameter = DatCommand.CommandType.Reset

    End Sub
    Public Sub ReLoad(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        Field.ReLoad(DatFile, ObjectID, Parameter, InputField.SFlag.FLAG)
        Dim DB As DatBinding = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)
        DataContext = DB
        If True Then
            Dim tbind As New Binding
            tbind.Path = New PropertyPath("ToolTipText")
            CheckboxList.SetBinding(CheckboxList.ToolTipProperty, tbind)
            'ValueText.ToolTip = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).GetToolTip
        End If
        Dim itmes As String() = DB.ComboxItems
        For i = 0 To itmes.Count - 1
            Dim tBorder As Border = CheckboxList.Children.Item(i)
            Dim tcheckBox As CheckBox = tBorder.Child


            tBorder.DataContext = DB.GetFlagBinding(i)
            Dim Binding As New Binding
            Binding.Path = New PropertyPath("MiniFlag")
            tcheckBox.SetBinding(CheckBox.IsCheckedProperty, Binding)

            Dim Binding2 As New Binding
            Binding2.Path = New PropertyPath("MiniBackColor")
            tBorder.SetBinding(Border.BackgroundProperty, Binding2)
        Next



        DatCommand.ReLoad(DatFile, Parameter, ObjectID)
    End Sub

    Private Sub OpneMenu(sender As Object, e As ContextMenuEventArgs) Handles CheckboxList.ContextMenuOpening
        CopyItem.IsEnabled = DatCommand.IsEnabled(CopyItem.CommandParameter)
        PasteItem.IsEnabled = DatCommand.IsEnabled(PasteItem.CommandParameter)
        ResetItem.IsEnabled = DatCommand.IsEnabled(ResetItem.CommandParameter)
    End Sub
End Class
