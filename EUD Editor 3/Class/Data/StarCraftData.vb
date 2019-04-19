Imports System.IO
Public Module SCConst
    Public SCUnitCount As Byte = 228
    Public SCWeaponCount As Byte = 130
    Public SCFlingyCount As Byte = 209
    Public SCSpriteCount As UShort = 517
    Public SCImageCount As UShort = 999
    Public SCUpgradeCount As Byte = 61
    Public SCTechCount As Byte = 44
    Public SCOrderCount As UShort = 189
    Public SCPortdataCount As UShort = 220
    Public SCSfxdataCount As UShort = 1144
    Public SCIconCount As UShort = 390
    Public SCtbltxtCount As UShort = 1547
    Public SCIscriptCount As UShort = 412


    Public SCButtonCount As Byte = 250

    Public SCMenCount As Byte = 106
    Public ASCIICount As UShort = 127


    Public SCCodeCount() As UShort = {
        SCUnitCount,'units
        SCWeaponCount,
        SCFlingyCount,
        SCSpriteCount,
        SCImageCount,
        SCUpgradeCount,
        SCTechCount,
        SCOrderCount,
        SCPortdataCount,
        SCSfxdataCount,
        SCIconCount,
        SCtbltxtCount,
        SCIscriptCount,
        228,'statusinfor
        228,'wireframe
        228,'Unitrequire
        61,'Upgraderequire
        44,'TechResearchrequire
        44,'TechUserequire
        189,'Orderrequire
        SCButtonCount,
        SCButtonCount,'butto
        SCTechCount
    }
    Public Datfilesname() As String = {
        "units",
        "weapons",
        "flingy",
        "sprites",
        "images",
        "upgrades",
        "techdata",
        "orders",
        "portdata",
        "sfxdata",
        "Icon",
        "Startxt",
        "Iscript",
        "statusinfor",
        "wireframe",
        "Unitrequire",
        "Upgraderequire",
        "TechResearchrequire",
        "TechUserequire",
        "Orderrequire",
        "button",
        "buttonSet",
        "Stechdata"
    }



    Public Function CheckOverFlow(Datfiles As SCDatFiles.DatFiles, Value As Long) As Boolean
        Return (SCCodeCount(Datfiles) > Value) And Value >= 0
    End Function

    Public ColorTable() As Color = {
    Nothing,
    Color.FromRgb(184, 184, 232),
    Color.FromRgb(184, 184, 232),
    Color.FromRgb(220, 220, 60),
    Color.FromRgb(255, 255, 255),
    Color.FromRgb(132, 116, 116),
    Color.FromRgb(200, 24, 24),
    Color.FromRgb(16, 252, 24),
    Color.FromRgb(244, 4, 4),
    Nothing,
    Nothing,
    Color.FromArgb(0, 0, 0, 0),
    Nothing,
    Nothing,
    Color.FromRgb(12, 72, 204),
    Color.FromRgb(44, 180, 148),
    Color.FromRgb(136, 64, 156),
    Color.FromRgb(248, 140, 20),
    Nothing,'우측정렬
    Nothing,'가운대정렬
    Color.FromArgb(0, 0, 0, 0),
    Color.FromRgb(112, 48, 20),
    Color.FromRgb(204, 224, 208),
    Color.FromRgb(252, 252, 56),
    Color.FromRgb(8, 128, 8),
    Color.FromRgb(252, 252, 124),
    Color.FromRgb(184, 184, 232),
    Color.FromRgb(236, 196, 176),
    Color.FromRgb(64, 104, 212),
    Color.FromRgb(116, 164, 124),
    Color.FromRgb(144, 144, 184),
    Color.FromRgb(0, 228, 252)}
End Module

Public Class StarCraftData
    Private pstatusFnVal1 As List(Of UInteger)
    Private pstatusFnVal2 As List(Of UInteger)
    Public ReadOnly Property statusFnVal1 As List(Of UInteger)
        Get
            Return pstatusFnVal1
        End Get
    End Property

    Public ReadOnly Property statusFnVal2 As List(Of UInteger)
        Get
            Return pstatusFnVal2
        End Get
    End Property
    Private piscriptData As IScript.CIScript
    Public ReadOnly Property IscriptData As IScript.CIScript
        Get
            Return piscriptData
        End Get
    End Property


    Private GRPFiles(SCImageCount) As String
    Private ImageName(SCImageCount) As String
    Private pSfxName(SCSfxdataCount) As String
    Private pSfxFileName As tblReader

    Private pPortdataName(SCPortdataCount) As String
    Private pIconName(SCIconCount) As String
    Private pIscriptName(SCIscriptCount) As String
    Private pvirtualCode(255) As String
    Private pASCIICode(128) As String
    Public ReadOnly Property IscriptName(index As Integer) As String
        Get
            Return pIscriptName(index)
        End Get
    End Property
    Public ReadOnly Property IconName(index As Integer) As String
        Get
            Return pIconName(index)
        End Get
    End Property
    Public ReadOnly Property ImageStr(index As Integer) As String
        Get
            Return ImageName(index)
        End Get
    End Property
    Public ReadOnly Property SfxCodeName(index As Integer) As String
        Get
            Return pSfxName(index)
        End Get
    End Property
    Public ReadOnly Property SfxFileName(index As Integer) As String
        Get
            Return pSfxFileName.Strings(index).val1
        End Get
    End Property
    Public ReadOnly Property PortdataName(index As Integer) As String
        Get
            Return pPortdataName(index)
        End Get
    End Property

    Public ReadOnly Property VirtualCode(index As Integer) As String
        Get
            Return pvirtualCode(index)
        End Get
    End Property

    Public ReadOnly Property ASCIICode(index As Integer) As String
        Get
            Return pASCIICode(index)
        End Get
    End Property

    Private ExtraBtnStr(22) As String
    Public ReadOnly Property BtnStr(index As Integer) As String
        Get
            Return ExtraBtnStr(index)
        End Get
    End Property

    Public ReadOnly Property VirtualKeyCodesPath As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Texts\Virtual-Key Codes.txt"
        End Get
    End Property
    Public ReadOnly Property ASCIICodesPath As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Texts\ASCII Codes.txt"
        End Get
    End Property

    Private ReadOnly Property IscirptPath As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Texts\IscriptIDList.txt"
        End Get
    End Property
    Private ReadOnly Property IconPath As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Texts\Icon.txt"
        End Get
    End Property
    Private ReadOnly Property GRPTextPath As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Texts\GRPfile.txt"
        End Get
    End Property
    Private ReadOnly Property ImageStrPath As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Texts\Images.txt"
        End Get
    End Property
    Private ReadOnly Property SfxNamePath As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Texts\Sfxdata.txt"
        End Get
    End Property
    Private ReadOnly Property PordataPath As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Texts\Portdata.txt"
        End Get
    End Property
    Private ReadOnly Property BtnStrPath As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Texts\UnitBtn.txt"
        End Get
    End Property



    Private ReadOnly stat_txt As tblReader
    Public ReadOnly Property GetStat_txt(index As Integer, Optional TrueVal As Boolean = False) As String
        Get
            Try
                If TrueVal Then
                    Return stat_txt.Strings(index).val2
                Else
                    Select Case pgData.Setting(ProgramData.TSetting.CDLanuage)
                        Case 1
                            Return stat_txt_kor_eng.Strings(index).val1
                        Case 2
                            Return stat_txt_kor_kor.Strings(index).val1
                        Case Else
                            Return stat_txt.Strings(index).val1
                    End Select

                    Return "Error"
                End If
            Catch ex As Exception
                Return "None"
            End Try
        End Get
    End Property


    Private ReadOnly stat_txt_kor_eng As tblReader
    Private ReadOnly stat_txt_kor_kor As tblReader

    Private SDGRP(SCImageCount) As GRP
    Private HDGRP(SCImageCount) As GRP
    Public ReadOnly Property GetGRPImage(index As Integer, frame As Integer, IsRemaster As Boolean) As BitmapImage
        Get
            If IsRemaster Then
                Return Nothing
            Else
                Return SDGRP(index).DrawGRP(frame)
            End If
        End Get
    End Property
    Public ReadOnly Property GetGrp(index As Integer) As GRP
        Get
            Return SDGRP(index)
        End Get
    End Property




    Private SDICON As GRP
    Private HDICON As GRP
    Public ReadOnly Property GetIcon(index As Integer, IsRemaster As Boolean) As BitmapImage
        Get
            If IsRemaster Then
                Return Nothing
            Else
                Return SDICON.DrawGRP(index)
            End If
        End Get
    End Property


    Private SDWireFrame As GRP
    Private HDWireFrame As GRP
    Private SDGrpFrame As GRP
    Private HDGrpFrame As GRP
    Private SDTramFrame As GRP
    Private HDTramFrame As GRP
    Public ReadOnly Property GetWireFrame(index As Integer, IsRemaster As Boolean) As BitmapImage
        Get
            If IsRemaster Then
                Return Nothing
            Else
                Return SDWireFrame.DrawGRP(index)
            End If
        End Get
    End Property


    Public ReadOnly Property GetGrpFrame(index As Integer, IsRemaster As Boolean) As BitmapImage
        Get
            If IsRemaster Then
                Return Nothing
            Else
                Return SDGrpFrame.DrawGRP(index)
            End If
        End Get
    End Property


    Public ReadOnly Property GetTranFrame(index As Integer, IsRemaster As Boolean) As BitmapImage
        Get
            If IsRemaster Then
                Return Nothing
            Else
                Return SDTramFrame.DrawGRP(index)
            End If
        End Get
    End Property


    Private IsLoadMPQ As Boolean
    Public ReadOnly Property LoadStarCraftData As Boolean
        Get
            Return IsLoadMPQ
        End Get
    End Property



    Public DefaultExtraDat As ExtraDatFiles
    Public DefaultDat As SCDatFiles
    Private Offsets As Dictionary(Of String, String)

    Private _StatusCode As List(Of Byte())
    Public ReadOnly Property StatusCode(index As Integer) As Byte()
        Get
            Return _StatusCode(index)
        End Get
    End Property
    Public ReadOnly Property StatusCodeCount() As Integer
        Get
            Return _StatusCode.Count
        End Get
    End Property

    Public Sub New()
        _StatusCode = New List(Of Byte())
        _StatusCode.Add({2, 1})
        _StatusCode.Add({1, 0})
        _StatusCode.Add({4, 3})
        _StatusCode.Add({3, 2})
        _StatusCode.Add({7, 6})
        _StatusCode.Add({8, 7})
        _StatusCode.Add({6, 5})
        _StatusCode.Add({5, 4})
        _StatusCode.Add({0, 8})
        _StatusCode.Add({1, 8})
        _StatusCode.Add({2, 8})


        DefaultDat = New SCDatFiles(False)
        DefaultExtraDat = New ExtraDatFiles
        Offsets = New Dictionary(Of String, String)
        '오프셋 읽기

        stat_txt = New tblReader(Tool.GetTblFolder & "\stat_txt.tbl")
        stat_txt_kor_eng = New tblReader(Tool.GetTblFolder & "\stat_txt_kor_eng.tbl")
        stat_txt_kor_kor = New tblReader(Tool.GetTblFolder & "\stat_txt_kor_kor.tbl")

        pSfxFileName = New tblReader(Tool.GetTblFolder & "\sfxdata.tbl")


        LoadTexts()

        ReadActConCode()

        MGRP.GRPMoudleInit()
        LoadMPQData()

        piscriptData = New IScript.CIScript
        piscriptData.LoadIscriptToBuff(Tool.LoadDataFromMPQ("scripts\iscript.bin"))
        IScript.readOpcodes()

        pstatusFnVal1 = New List(Of UInteger)
        pstatusFnVal2 = New List(Of UInteger)
        statusFnVal1.AddRange({4343040, 4344192, 4346240, 4345616, 4344656, 4344560, 4344512, 4348160, 4343072})
        statusFnVal2.AddRange({4353872, 4356240, 4357264, 4355232, 4355040, 4354656, 4357424, 4353760, 4349664})
    End Sub

    Public ReadOnly Property FuncConDict As Dictionary(Of UInteger, ButtonFunc)
        Get
            Return ConDict
        End Get
    End Property
    Public ReadOnly Property FuncActDict As Dictionary(Of UInteger, ButtonFunc)
        Get
            Return ActDict
        End Get
    End Property


    Private ConDict As Dictionary(Of UInteger, ButtonFunc) '코드 번호에 대응하는 이름
    Private ActDict As Dictionary(Of UInteger, ButtonFunc) '코드 번호에 대응하는 이름
    Private Sub ReadActConCode()
        ConDict = New Dictionary(Of UInteger, ButtonFunc)
        ActDict = New Dictionary(Of UInteger, ButtonFunc)


        Dim fs As New FileStream(Tool.FiregraftConFunPath, FileMode.Open)
        Dim sr As New StreamReader(fs)

        While (Not sr.EndOfStream)
            Dim str As String = sr.ReadLine
            Dim values() As String = str.Split(vbTab)

            Dim DatType As SCDatFiles.DatFiles


            If values.Count <= 2 Then
                DatType = SCDatFiles.DatFiles.None
            Else
                DatType = values(2)
            End If
            Dim BtnFunc As New ButtonFunc(values(0), DatType, ConDict.Count)

            ConDict.Add("&H" & values(1), BtnFunc)
        End While

        sr.Close()
        fs.Close()

        fs = New FileStream(Tool.FiregraftActFunPath, FileMode.Open)
        sr = New StreamReader(fs)

        While (Not sr.EndOfStream)
            Dim str As String = sr.ReadLine
            Dim values() As String = str.Split(vbTab)

            Dim DatType As SCDatFiles.DatFiles


            If values.Count <= 2 Then
                DatType = SCDatFiles.DatFiles.None
            Else
                DatType = values(2)
            End If
            Dim BtnFunc As New ButtonFunc(values(0), DatType, ActDict.Count)

            ActDict.Add("&H" & values(1), BtnFunc)
        End While

        sr.Close()
        fs.Close()
    End Sub


    Public Structure ButtonFunc
        Public Index As Integer
        Public Name As String
        Public DatType As SCDatFiles.DatFiles
        Public Sub New(tName As String, tDatType As SCDatFiles.DatFiles, tindex As Integer)
            Name = tName
            DatType = tDatType
            Index = tindex
        End Sub
    End Structure

    Private Sub LoadTexts()
        'ReadGRPFilenames
        Dim sr As StreamReader = New StreamReader(GRPTextPath)
        Dim index As Integer = 0
        While Not sr.EndOfStream
            GRPFiles(index) = sr.ReadLine().Replace("<0>", "")

            index += 1
        End While
        sr.Close()

        sr = New StreamReader(ImageStrPath)
        index = 0
        While Not sr.EndOfStream
            ImageName(index) = sr.ReadLine()

            index += 1
        End While
        sr.Close()

        sr = New StreamReader(BtnStrPath)
        index = 0
        While Not sr.EndOfStream
            ExtraBtnStr(index) = sr.ReadLine()

            index += 1
        End While
        sr.Close()

        sr = New StreamReader(SfxNamePath)
        index = 0
        While Not sr.EndOfStream
            pSfxName(index) = sr.ReadLine()

            index += 1
        End While
        sr.Close()


        sr = New StreamReader(PordataPath)
        index = 0
        While Not sr.EndOfStream
            pPortdataName(index) = sr.ReadLine()

            index += 1
        End While
        sr.Close()

        sr = New StreamReader(IconPath)
        index = 0
        While Not sr.EndOfStream
            pIconName(index) = sr.ReadLine()

            index += 1
        End While
        sr.Close()


        sr = New StreamReader(IscirptPath)
        index = 0
        While Not sr.EndOfStream
            pIscriptName(index) = sr.ReadLine()

            index += 1
        End While
        sr.Close()


        sr = New StreamReader(VirtualKeyCodesPath)
        index = 0
        While Not sr.EndOfStream
            pvirtualCode(index) = sr.ReadLine()

            index += 1
        End While
        sr.Close()


        sr = New StreamReader(ASCIICodesPath)
        index = 0
        While Not sr.EndOfStream
            pASCIICode(index) = sr.ReadLine()

            index += 1
        End While
        sr.Close()

    End Sub
    Public Sub LoadMPQData()
        IsLoadMPQ = False
        If Not My.Computer.FileSystem.FileExists(pgData.Setting(ProgramData.TSetting.starcraft)) Then
            Tool.ErrorMsgBox(Tool.GetText("Error NotExistMPQ"))
            Return
        End If
        Try
            'MPQ파일을 미리 다 읽어서 메모리에 올리자.
            'GRP먼저
            For i = 0 To SCImageCount - 1
                Dim grpindex As Integer = DefaultDat.Data(SCDatFiles.DatFiles.images, "GRP File", i)
                Dim DrawFunc As Integer = DefaultDat.Data(SCDatFiles.DatFiles.images, "Draw Function", i)
                Dim Remapping As Integer = DefaultDat.Data(SCDatFiles.DatFiles.images, "Remapping", i)
                Dim filename As String = GRPFiles(grpindex)
                SDGRP(i) = New GRP(filename, DrawFunc, Remapping)
            Next

            SDICON = New GRP("cmdbtns\cmdicons.grp", 0, 0, PalettType.Icons)


            SDWireFrame = New GRP("wirefram\wirefram.grp", 0, 0, PalettType.Icons)
            SDGrpFrame = New GRP("wirefram\grpwire.grp", 0, 0, PalettType.Icons)
            SDTramFrame = New GRP("wirefram\tranwire.grp", 0, 0, PalettType.Icons)
        Catch ex As Exception
            Tool.ErrorMsgBox(Tool.GetText("Error LoadMPQData Fail"), ex.ToString)
            Return
        End Try



        IsLoadMPQ = True
    End Sub






    Private Sub ReadOffetFile(filename As String)

    End Sub









    Public ReadOnly Property GetOffset(Name As String) As String
        Get
            Return Offsets(Name)
        End Get
    End Property


End Class




