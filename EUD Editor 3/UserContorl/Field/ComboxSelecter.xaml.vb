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


        DatCommand.ReLoad(DatFile, Parameter, ObjectID)
    End Sub
    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        Field.Init(DatFile, ObjectID, Parameter)

        Dim DB As DatBinding = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)
        DataContext = DB

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
End Class
