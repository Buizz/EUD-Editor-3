<Serializable()>
Public Class ExtraDatFiles
    Public Shared StatNullString As String = "NULLSTRING"

    '기타 뎃파일들의 그룹, 툴팁 등을 관리.
    Public Property Group(key As SCDatFiles.DatFiles, index As Integer) As String
        Get
            Return GroupDic(key)(index)
        End Get
        Set(value As String)
            GroupDic(key)(index) = value
        End Set
    End Property
    Public Property ToolTip(key As SCDatFiles.DatFiles, index As Integer) As String
        Get
            Return ToolTipDic(key)(index)
        End Get
        Set(value As String)
            ToolTipDic(key)(index) = value
        End Set
    End Property
    Public Sub GroupReset(key As SCDatFiles.DatFiles, index As Integer)
        GroupDic(key)(index) = Math.Floor(index / 100) * 100 & " ~ " & Math.Floor(index / 100) * 100 + 100
    End Sub
    Public Sub ToolTipReset(key As SCDatFiles.DatFiles, index As Integer)
        ToolTipDic(key)(index) = index
    End Sub

    Public Sub New()
        ToolTipDic = New Dictionary(Of SCDatFiles.DatFiles, List(Of String))
        GroupDic = New Dictionary(Of SCDatFiles.DatFiles, List(Of String))

        ToolTipDic.Add(SCDatFiles.DatFiles.stattxt, New List(Of String))
        GroupDic.Add(SCDatFiles.DatFiles.stattxt, New List(Of String))

        For i = 0 To SCtbltxtCount - 1
            ToolTipDic(SCDatFiles.DatFiles.stattxt).Add("")
            GroupDic(SCDatFiles.DatFiles.stattxt).Add("")
            ToolTipReset(SCDatFiles.DatFiles.stattxt, i)
            GroupReset(SCDatFiles.DatFiles.stattxt, i)
        Next


        ReDim _Stat_txt(SCtbltxtCount)
        For i = 0 To SCtbltxtCount - 1
            _Stat_txt(i) = StatNullString
        Next
    End Sub



    Public Function CheckDirty(DatFiles As SCDatFiles.DatFiles, index As Integer) As Boolean
        Select Case DatFiles
            Case SCDatFiles.DatFiles.stattxt
                Return pjData.ExtraDat.Stat_txt(index) = StatNullString

        End Select
        Return False
    End Function


    Private ToolTipDic As Dictionary(Of SCDatFiles.DatFiles, List(Of String))
    Private GroupDic As Dictionary(Of SCDatFiles.DatFiles, List(Of String))

    Private _Stat_txt() As String
    Public Property Stat_txt(index As Integer) As String
        Get
            Dim text As String
            Try
                text = _Stat_txt(index)
                Return text
            Catch ex As Exception
                Return "Error"
            End Try
        End Get
        Set(value As String)
            _Stat_txt(index) = value
        End Set
    End Property
End Class
