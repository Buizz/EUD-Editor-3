Public Class TestCommand
    Implements ICommand

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        MsgBox("왓?" & parameter)
        'Throw New NotImplementedException()
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
        'Throw New NotImplementedException()
    End Function
End Class
