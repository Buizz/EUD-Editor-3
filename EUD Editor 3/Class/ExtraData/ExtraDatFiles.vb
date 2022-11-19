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

    Private _ButtonSet() As Byte
    Private _DefaultButtonSet() As Boolean

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
        Select Case key
            Case SCDatFiles.DatFiles.ButtonData
                If scData IsNot Nothing Then
                    If index < SCUnitCount Then
                        GroupDic(key)(index) = scData.DefaultDat.Group(SCDatFiles.DatFiles.units, index)
                    Else
                        GroupDic(key)(index) = Tool.GetText("buttonSetEtcGroup")
                    End If
                Else
                    GroupDic(key)(index) = Tool.GetText("buttonSetEtcGroup")
                End If
            Case SCDatFiles.DatFiles.stattxt
                GroupDic(key)(index) = Tool.CodeGrouping.StrGroup(index)
            Case Else
                GroupDic(key)(index) = Math.Floor(index / 100) * 100 & " ~ " & Math.Floor(index / 100) * 100 + 100
        End Select


    End Sub
    Public Sub ToolTipReset(key As SCDatFiles.DatFiles, index As Integer)
        If key = SCDatFiles.DatFiles.stattxt Then
            ToolTipDic(key)(index) = index + 1
        Else
            ToolTipDic(key)(index) = index
        End If
    End Sub

    Public Sub New()
        LoadStatusData()
        LoadWireFrame()
        LoadButtonSet()
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



        ToolTipDic.Add(SCDatFiles.DatFiles.ButtonData, New List(Of String))
        GroupDic.Add(SCDatFiles.DatFiles.ButtonData, New List(Of String))

        For i = 0 To SCButtonCount - 1
            ToolTipDic(SCDatFiles.DatFiles.ButtonData).Add("")
            GroupDic(SCDatFiles.DatFiles.ButtonData).Add("")
            ToolTipReset(SCDatFiles.DatFiles.ButtonData, i)
            GroupReset(SCDatFiles.DatFiles.ButtonData, i)
        Next
    End Sub


    Public ReadOnly Property RequireData(DType As SCDatFiles.DatFiles) As CRequireData
        Get
            Select Case DType
                Case SCDatFiles.DatFiles.units
                    Return RequireData(0)
                Case SCDatFiles.DatFiles.upgrades
                    Return RequireData(1)
                Case SCDatFiles.DatFiles.techdata
                    Return RequireData(2)
                Case SCDatFiles.DatFiles.Stechdata
                    Return RequireData(3)
                Case SCDatFiles.DatFiles.orders
                    Return RequireData(4)
            End Select
            Return RequireData(0)
        End Get
    End Property


    Private RequireData(4) As CRequireData


    Public Class SReqDATA
        '요구사항 각 내용.
        Public pos As UInt16
        Public Code As List(Of UShort)
    End Class
    Private Sub LoadReqdata()
        Dim RequireData(4) As List(Of SReqDATA)

        Dim filepath As String = Tool.GetDatFolder & "\require"


        Dim fs As New FileStream(filepath & ".dat", FileMode.Open)
        Dim br As New BinaryReader(fs)

        For i = 0 To 4
            RequireData(i) = New List(Of SReqDATA)
        Next
        For i = 0 To SCCodeCount(SCDatFiles.DatFiles.units) - 1
            RequireData(0).Add(New SReqDATA)

            Dim cot As Integer = RequireData(0).Count - 1
            RequireData(0)(cot).pos = br.ReadUInt16()
        Next
        For i = 0 To SCCodeCount(SCDatFiles.DatFiles.upgrades) - 1
            RequireData(1).Add(New SReqDATA)

            Dim cot As Integer = RequireData(1).Count - 1
            RequireData(1)(cot).pos = br.ReadUInt16()
        Next
        For i = 0 To SCCodeCount(SCDatFiles.DatFiles.techdata) - 1
            RequireData(2).Add(New SReqDATA)

            Dim cot As Integer = RequireData(2).Count - 1
            RequireData(2)(cot).pos = br.ReadUInt16()
        Next
        For i = 0 To SCCodeCount(SCDatFiles.DatFiles.techdata) - 1
            RequireData(3).Add(New SReqDATA)

            Dim cot As Integer = RequireData(3).Count - 1
            RequireData(3)(cot).pos = br.ReadUInt16()
        Next
        For i = 0 To SCCodeCount(SCDatFiles.DatFiles.orders) - 1
            RequireData(4).Add(New SReqDATA)

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


        RequireData(0) = New CRequireData(SCDatFiles.DatFiles.units, RequireData(0))
        RequireData(1) = New CRequireData(SCDatFiles.DatFiles.upgrades, RequireData(1))
        RequireData(2) = New CRequireData(SCDatFiles.DatFiles.techdata, RequireData(2))
        RequireData(3) = New CRequireData(SCDatFiles.DatFiles.techdata, RequireData(3), True)
        RequireData(4) = New CRequireData(SCDatFiles.DatFiles.orders, RequireData(4))
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
            If index < SCGrpWireCount Then
                Return _DefaultGrpFrame(index)
            Else
                Return True
            End If
        End Get
        Set(value As Boolean)
            If index < SCGrpWireCount Then
                _DefaultGrpFrame(index) = value
            End If
        End Set
    End Property
    Public Property DefaultTranFrame(index As Integer) As Boolean
        Get
            If index < SCMenCount Then
                Return _DefaultTranFrame(index)
            Else
                Return True
            End If
        End Get
        Set(value As Boolean)
            If index < SCMenCount Then
                _DefaultTranFrame(index) = value
            End If
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
            If index < SCGrpWireCount Then
                Return _GrpFrame(index)
            Else
                Return 0
            End If
        End Get
        Set(value As Byte)
            If index < SCGrpWireCount Then
                _GrpFrame(index) = value
            Else

            End If
        End Set
    End Property
    Public Property TranFrame(index As Integer) As Byte
        Get
            If index < SCMenCount Then
                Return _TranFrame(index)
            Else
                Return 0
            End If

        End Get
        Set(value As Byte)
            If index < SCMenCount Then
                _TranFrame(index) = value
            Else

            End If

        End Set
    End Property

    Private Sub LoadWireFrame()
        ReDim _WireFrame(SCUnitCount - 1)
        ReDim _GrpFrame(SCGrpWireCount - 1)
        ReDim _TranFrame(SCMenCount - 1)

        ReDim _DefaultWireFrame(SCUnitCount - 1)
        ReDim _DefaultGrpFrame(SCGrpWireCount - 1)
        ReDim _DefaultTranFrame(SCMenCount - 1)

        For i = 0 To SCUnitCount - 1
            _DefaultWireFrame(i) = True

            _WireFrame(i) = i

            If i < SCGrpWireCount Then
                _DefaultGrpFrame(i) = True
                _GrpFrame(i) = i
            End If
            If i < SCMenCount Then
                _DefaultTranFrame(i) = True
                _TranFrame(i) = i
            End If
        Next
    End Sub


    Public Property DefaultButtonSet(index As Integer) As Boolean
        Get
            Return _DefaultButtonSet(index)
        End Get
        Set(value As Boolean)
            _DefaultButtonSet(index) = value
        End Set
    End Property

    Public Property ButtonSet(index As Integer) As Byte
        Get
            Return _ButtonSet(index)
        End Get
        Set(value As Byte)
            _ButtonSet(index) = value
        End Set
    End Property

    Public ReadOnly Property ButtonData As CButtonSets
        Get
            Return _ButtonData
        End Get
    End Property


    Private _ButtonData As CButtonSets

    Private Sub LoadButtonSet()
        ReDim _ButtonSet(SCButtonCount - 1)
        ReDim _DefaultButtonSet(SCButtonCount - 1)

        For i = 0 To SCButtonCount - 1
            _DefaultButtonSet(i) = True

            _ButtonSet(i) = i
        Next

        Dim filepath As String = Tool.GetDatFolder & "\btnset" & ".dat"
        _ButtonData = New CButtonSets(filepath)
    End Sub

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
        Dim statusFnVal1 As New List(Of UInteger)
        Dim statusFnVal2 As New List(Of UInteger)
        statusFnVal1.AddRange({4343040, 4344192, 4346240, 4345616, 4344656, 4344560, 4344512, 4348160, 4343072})
        statusFnVal2.AddRange({4353872, 4356240, 4357264, 4355232, 4355040, 4354656, 4357424, 4353760, 4349664})
        ReDim _statusFn1(SCUnitCount - 1)
        ReDim _statusFn2(SCUnitCount - 1)
        ReDim _statusFn1IsDefault(SCUnitCount - 1)
        ReDim _statusFn2IsDefault(SCUnitCount - 1)


        Dim filepath As String = Tool.GetDatFolder & "\statusInfor"


        Dim fs As New FileStream(filepath & ".dat", FileMode.Open)
        Dim br As New BinaryReader(fs)


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
            Case SCDatFiles.DatFiles.ButtonSet
                Return DefaultButtonSet(index)
            Case SCDatFiles.DatFiles.ButtonData
                Return ButtonData.GetButtonSet(index).IsDefault
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
