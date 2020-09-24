

<Serializable>
Public Class ClassicTriggerEditor
    Inherits ScriptEditor


    '모든 트리거가 모여있음
    Public TriggerList As New List(Of Trigger)


    '임포트된 파일들 모음
    Public ImportFiles As New List(Of String)


    '글로벌 변수 모음
    Public globalVar As New List(Of String)




    Public Overrides Property ConnectFile As String
        Get
            'Throw New NotImplementedException()
            Return ""
        End Get
        Set(value As String)
            'Throw New NotImplementedException()
        End Set
    End Property

    Public Overrides Function GetFileText() As String
        'Throw New NotImplementedException()
    End Function

    Public Overrides Function GetStringText() As String
        'Throw New NotImplementedException()
    End Function

    Public Overrides Function CheckConnect() As Boolean
        Return False 'Throw New NotImplementedException()
    End Function
End Class
