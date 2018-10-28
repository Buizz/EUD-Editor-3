Public Class StarCraftData
    Public SCUnitCount As Byte = 228

    Private ReadOnly stat_txt As tblReader
    Public ReadOnly Property GetStat_txt(index As Integer, Optional TrueVal As Boolean = False) As String
        Get
            Try
                If TrueVal Then
                    Return stat_txt.Strings(index).val2
                Else
                    Return stat_txt.Strings(index).val1
                End If
            Catch ex As Exception
                Return "Not Exist"
            End Try
        End Get
    End Property


    Private ReadOnly stat_txt_kor_eng As tblReader
    Private ReadOnly stat_txt_kor_kor As tblReader




    Public DefaultDat As SCDatFiles
    Private Offsets As Dictionary(Of String, String)

    Public Sub New()
        DefaultDat = New SCDatFiles
        Offsets = New Dictionary(Of String, String)
        '오프셋 읽기

        stat_txt = New tblReader(Tool.GetTblFolder & "\stat_txt.tbl")

    End Sub
    Private Sub ReadOffetFile(filename As String)

    End Sub









    Public ReadOnly Property GetOffset(Name As String) As String
        Get
            Return Offsets(Name)
        End Get
    End Property


End Class




