<Serializable>
Public Class CUIScriptEditor
    Inherits ScriptEditor

    Public Sub New(SType As SType)
        ScriptType = SType
    End Sub

    Private _String As String
    Public Property StringText As String
        Get
            Return _String
        End Get
        Set(value As String)
            _String = value
        End Set
    End Property
End Class
