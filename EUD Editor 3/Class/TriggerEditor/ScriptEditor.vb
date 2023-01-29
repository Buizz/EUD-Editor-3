<Serializable>
Public MustInherit Class ScriptEditor
    Public Enum SType
        Eps
        Py
    End Enum
    Protected ScriptType As SType




    Public MustOverride Function GetFileText(filename As String) As String
    Public MustOverride Function GetStringText() As String

    Public MustOverride Function CheckConnect() As Boolean
    Public MustOverride Property ConnectFile() As String

    Public Function IsMain() As Boolean
        If pjData.TEData.MainFile IsNot Nothing Then
            Return pjData.TEData.MainFile.Scripter Is Me
        Else
            Return False
        End If
    End Function
End Class
