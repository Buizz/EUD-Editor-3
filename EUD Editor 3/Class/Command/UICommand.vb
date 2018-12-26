Public Class UICommand
    Implements ICommand

    Public Enum CommandType
        Copy
        Paste
        Reset
    End Enum
    Public Enum EUIType
        ToolTip
        Group
    End Enum

    Private UIType As EUIType

    Private ObjectID As Integer
    Private DatFile As SCDatFiles.DatFiles

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub New(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _UIType As EUIType)
        DatFile = _DatFile
        ObjectID = _ObjectID
        UIType = _UIType
    End Sub

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Select Case parameter
            Case CommandType.Copy
            Case CommandType.Paste
            Case CommandType.Reset
                Select Case UIType
                    Case EUIType.ToolTip
                        pjData.BindingManager.UIManager(DatFile, ObjectID).ToolTipReset()
                    Case EUIType.Group
                        pjData.BindingManager.UIManager(DatFile, ObjectID).GroupReset()
                End Select
        End Select
    End Sub

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
