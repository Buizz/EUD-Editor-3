Imports System.IO

<Serializable()>
Public Class ExtraDatFiles
    Public Shared StatNullString As String = "NULLSTRING"
    Private _WireFrame() As Byte
    Private _GrpFrame() As Byte
    Private _TranFrame() As Byte
    Private _DefaultWireFrame() As Boolean
    Private _DefaultGrpFrame() As Boolean
    Private _DefaultTranFrame() As Boolean

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
        LoadStatusData()
        LoadWireFrame()
        LoadReqdata()

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

    Public Enum RequireUse
        DefaultUse
        DontUse
        AlwaysUse
        AlwaysCurrentUse
        CustomUse
    End Enum

    Private RequireData(4) As List(Of SReqDATA)
    Private RequireDataUSE(4) As List(Of Boolean)
    Private Class SReqDATA
        '요구사항 각 내용.
        Public pos As UInt16
        Public Code As List(Of UShort)
    End Class
    Private Sub LoadReqdata()
        Dim filepath As String = Tool.GetDatFolder & "\require"


        Dim fs As New FileStream(filepath & ".dat", FileMode.Open)
        Dim br As New BinaryReader(fs)

        For i = 0 To 4
            RequireData(i) = New List(Of SReqDATA)
            RequireDataUSE(i) = New List(Of Boolean)
        Next
        For i = 0 To SCCodeCount(SCDatFiles.DatFiles.units) - 1
            RequireData(0).Add(New SReqDATA)
            RequireDataUSE(0).Add(True)

            Dim cot As Integer = RequireData(0).Count - 1
            RequireData(0)(cot).pos = br.ReadUInt16()
        Next
        For i = 0 To SCCodeCount(SCDatFiles.DatFiles.upgrades) - 1
            RequireData(1).Add(New SReqDATA)
            RequireDataUSE(1).Add(True)

            Dim cot As Integer = RequireData(1).Count - 1
            RequireData(1)(cot).pos = br.ReadUInt16()
        Next
        For i = 0 To SCCodeCount(SCDatFiles.DatFiles.techdata) - 1
            RequireData(2).Add(New SReqDATA)
            RequireDataUSE(2).Add(True)

            Dim cot As Integer = RequireData(2).Count - 1
            RequireData(2)(cot).pos = br.ReadUInt16()
        Next
        For i = 0 To SCCodeCount(SCDatFiles.DatFiles.techdata) - 1
            RequireData(3).Add(New SReqDATA)
            RequireDataUSE(3).Add(True)

            Dim cot As Integer = RequireData(3).Count - 1
            RequireData(3)(cot).pos = br.ReadUInt16()
        Next
        For i = 0 To SCCodeCount(SCDatFiles.DatFiles.orders) - 1
            RequireData(4).Add(New SReqDATA)
            RequireDataUSE(4).Add(True)

            Dim cot As Integer = RequireData(4).Count - 1
            RequireData(4)(cot).pos = br.ReadUInt16()
        Next




        Dim pos() As UInteger = {&H46C, &H8B4, &HBFC, &HD3C, &HFEC}
        Dim tnum As Integer
        Dim codetype() As Integer = {SCDatFiles.DatFiles.units, SCDatFiles.DatFiles.upgrades, SCDatFiles.DatFiles.techdata, SCDatFiles.DatFiles.techdata, SCDatFiles.DatFiles.orders}
        For k = 0 To 4
            tnum = k

            Dim count As Integer = SCCodeCount(codetype(k)) - 1

            For i = 0 To count
                RequireData(tnum)(i).Code = New List(Of UInt16)


                If RequireData(tnum)(i).pos <> 0 Then
                    fs.Position = pos(k) + RequireData(tnum)(i).pos * 2

                    Dim opcode As UInt16 = 1
                    While True
                        Dim issubeol As Boolean
                        opcode = br.ReadUInt16()
                        If opcode = &HFF1F Or opcode = &HFF20 Then
                            issubeol = True
                        End If

                        If opcode <> &HFFFF Then
                            RequireData(tnum)(i).Code.Add(opcode)
                        End If
                        If opcode = &HFFFF Then
                            If issubeol Then
                                RequireData(tnum)(i).Code.Add(opcode)
                                issubeol = False
                            Else
                                Exit While
                            End If
                        End If
                    End While
                End If
            Next
        Next

        br.Close()
        fs.Close()
    End Sub



    Public Property DefaultWireFrame(index As Integer) As Boolean
        Get
            Return _DefaultWireFrame(index)
        End Get
        Set(value As Boolean)
            _DefaultWireFrame(index) = value
        End Set
    End Property
    Public Property DefaultGrpFrame(index As Integer) As Boolean
        Get
            Return _DefaultGrpFrame(index)
        End Get
        Set(value As Boolean)
            _DefaultGrpFrame(index) = value
        End Set
    End Property
    Public Property DefaultTranFrame(index As Integer) As Boolean
        Get
            Return _DefaultTranFrame(index)
        End Get
        Set(value As Boolean)
            _DefaultTranFrame(index) = value
        End Set
    End Property


    Public Property WireFrame(index As Integer) As Byte
        Get
            Return _WireFrame(index)
        End Get
        Set(value As Byte)
            _WireFrame(index) = value
        End Set
    End Property
    Public Property GrpFrame(index As Integer) As Byte
        Get
            Return _GrpFrame(index)
        End Get
        Set(value As Byte)
            _GrpFrame(index) = value
        End Set
    End Property
    Public Property TranFrame(index As Integer) As Byte
        Get
            Return _TranFrame(index)
        End Get
        Set(value As Byte)
            _TranFrame(index) = value
        End Set
    End Property

    Private Sub LoadWireFrame()
        ReDim _WireFrame(SCUnitCount - 1)
        ReDim _GrpFrame(SCUnitCount - 1)
        ReDim _TranFrame(SCMenCount - 1)

        ReDim _DefaultWireFrame(SCUnitCount - 1)
        ReDim _DefaultGrpFrame(SCUnitCount - 1)
        ReDim _DefaultTranFrame(SCMenCount - 1)

        For i = 0 To SCUnitCount - 1
            _DefaultWireFrame(i) = True
            _DefaultGrpFrame(i) = True

            _WireFrame(i) = i
            _GrpFrame(i) = i

            If i < SCMenCount Then
                _DefaultTranFrame(i) = True
                _TranFrame(i) = i
            End If
        Next
    End Sub



    Private statusFnVal1 As List(Of UInteger)
    Private statusFnVal2 As List(Of UInteger)
    Private _statusFn1() As Byte
    Private _statusFn1IsDefault() As Boolean
    Public Property StatusFunction1(index As Integer) As Byte
        Get
            Return _statusFn1(index)
        End Get
        Set(value As Byte)
            _statusFn1(index) = value
        End Set
    End Property
    Public Property DefaultStatusFunction1(index As Integer) As Boolean
        Get
            Return _statusFn1IsDefault(index)
        End Get
        Set(value As Boolean)
            _statusFn1IsDefault(index) = value
        End Set
    End Property

    Private _statusFn2() As Byte
    Private _statusFn2IsDefault() As Boolean
    Public Property StatusFunction2(index As Integer) As Byte
        Get
            Return _statusFn2(index)
        End Get
        Set(value As Byte)
            _statusFn2(index) = value
        End Set
    End Property
    Public Property DefaultStatusFunction2(index As Integer) As Boolean
        Get
            Return _statusFn2IsDefault(index)
        End Get
        Set(value As Boolean)
            _statusFn2IsDefault(index) = value
        End Set
    End Property



    Private Sub LoadStatusData()
        statusFnVal1 = New List(Of UInteger)
        statusFnVal2 = New List(Of UInteger)

        ReDim _statusFn1(SCUnitCount - 1)
        ReDim _statusFn2(SCUnitCount - 1)
        ReDim _statusFn1IsDefault(SCUnitCount - 1)
        ReDim _statusFn2IsDefault(SCUnitCount - 1)


        Dim filepath As String = Tool.GetDatFolder & "\statusInfor"


        Dim fs As New FileStream(filepath & ".dat", FileMode.Open)
        Dim br As New BinaryReader(fs)

        statusFnVal1.AddRange({4343040, 4344192, 4346240, 4345616, 4344656, 4344560, 4344512, 4348160, 4343072})
        statusFnVal2.AddRange({4353872, 4356240, 4357264, 4355232, 4355040, 4354656, 4357424, 4353760, 4349664})

        For i = 0 To SCUnitCount - 1
            _statusFn1IsDefault(i) = True
            _statusFn2IsDefault(i) = True

            Dim value As UInteger
            br.ReadUInt32()
            value = br.ReadUInt32
            _statusFn1(i) = statusFnVal1.IndexOf(value)

            value = br.ReadUInt32
            _statusFn2(i) = statusFnVal2.IndexOf(value)

        Next

        
        br.Close()
        fs.Close()
    End Sub



    Public Function CheckDirty(DatFiles As SCDatFiles.DatFiles, index As Integer) As Boolean
        Select Case DatFiles
            Case SCDatFiles.DatFiles.stattxt
                Return pjData.ExtraDat.Stat_txt(index) = StatNullString
            Case SCDatFiles.DatFiles.statusinfor
                Return _statusFn1IsDefault(index) And _statusFn2IsDefault(index)
            Case SCDatFiles.DatFiles.wireframe
                If index < SCMenCount Then
                    Return DefaultWireFrame(index) And DefaultGrpFrame(index) And DefaultTranFrame(index)
                Else
                    Return DefaultWireFrame(index) And DefaultGrpFrame(index)
                End If


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
