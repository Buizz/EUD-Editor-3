Public Class DatCommand
    Implements ICommand

    Public Enum CommandType
        Copy
        Paste
        Reset
    End Enum

    Private Datfile As SCDatFiles.DatFiles
    Private ParameterName As String
    Private ObjectID As Integer


    Public Sub New(tDatfile As SCDatFiles.DatFiles, tParameter As String, tObjectID As Integer)
        Datfile = tDatfile
        ParameterName = tParameter
        ObjectID = tObjectID
    End Sub


    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged


    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Select Case parameter
            Case CommandType.Copy
                My.Computer.Clipboard.SetText("안녕")
            Case CommandType.Paste
            Case CommandType.Reset
                pjData.BindingManager.DatBinding(Datfile, ParameterName, ObjectID).DataReset()
        End Select
        'Throw New NotImplementedException()
    End Sub

    '[
    '  {
    '    "unit.dat": {
    '      "Hit Point": [
    '        127,
    '        993214
    '      ]
    '    },
    '    "sprite.dat": 4
    '  }
    ']


    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return IsEnabled(parameter)
    End Function

    Public ReadOnly Property IsEnabled(parameter As String) As Boolean
        Get
            Select Case parameter
                Case CommandType.Copy
                    Return True
                Case CommandType.Paste
                    Dim clipboard As String = My.Computer.Clipboard.GetText
                    If clipboard = "수열" Then
                        Return True
                    End If
                    Return False
                Case CommandType.Reset
                    Return True
            End Select
            Return True
        End Get
    End Property
End Class
