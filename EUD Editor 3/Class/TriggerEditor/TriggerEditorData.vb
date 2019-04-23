<Serializable>
Public Class TriggerEditorData
    Public Const TopFileName As String = "​ProjectMain"

    Private ProjectFile As TEFile
    Public ReadOnly Property PFIles As TEFile
        Get
            Return ProjectFile
        End Get
    End Property



    Private _LastOpenTabs As LastTab
    Public ReadOnly Property LastOpenTabs As LastTab
        Get
            Return _LastOpenTabs
        End Get
    End Property
    <Serializable>
    Public Class LastTab
        Public Items As List(Of TEFile)
        Public FristItem As LastTab
        Public SecondItem As LastTab
        Public Orientation As Orientation

        Public Sub New()
            Items = New List(Of TEFile)
        End Sub
    End Class

    Public Sub New()
        ProjectFile = New TEFile(TopFileName, TEFile.EFileType.Folder)
        _LastOpenTabs = New LastTab
    End Sub
End Class
