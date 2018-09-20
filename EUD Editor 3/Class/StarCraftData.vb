Public Class StarCraftData
    Public SCUnitCount As Byte = 228
    Public DefaultDat As SCDatFiles
    Private Offsets As Dictionary(Of String, String)

    Public Sub New()
        DefaultDat = New SCDatFiles
        Offsets = New Dictionary(Of String, String)
        '오프셋 읽기

    End Sub
    Private Sub ReadOffetFile(filename As String)

    End Sub









    Public ReadOnly Property GetOffset(Name As String) As String
        Get
            Return Offsets(Name)
        End Get
    End Property


End Class




