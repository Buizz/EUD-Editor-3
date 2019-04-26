<Serializable>
Public MustInherit Class ScriptEditor
    Public Enum SType
        Eps
        Py
    End Enum
    Protected ScriptType As SType

    Public MustOverride Function GetFileText() As String
    Public MustOverride Function GetStringText() As String

    Public MustOverride Function CheckConnect() As Boolean
    Public MustOverride Property ConnectFile() As String
End Class
