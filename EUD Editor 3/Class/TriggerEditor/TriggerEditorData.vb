<Serializable>
Public Class TriggerEditorData
    Public Const TopFileName As String = "​ProjectMain"

    Private ProjectFile As TEFile
    Public ReadOnly Property PFIles As TEFile
        Get
            Return ProjectFile
        End Get
    End Property



    Public Sub New()
        ProjectFile = New TEFile(TopFileName, TEFile.EFileType.Folder)
    End Sub
End Class
