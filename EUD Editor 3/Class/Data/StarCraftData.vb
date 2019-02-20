Imports System.IO
Public Module SCConst
    Public SCCodeCount() As UShort = {228, 130, 209, 517, 999, 61, 44, 189, 220, 1144, 390, 1547, 412}
    Public Datfilesname() As String = {"units", "weapons", "flingy", "sprites", "images",
     "upgrades", "techdata", "orders", "portdata", "sfxdata", "Icon", "Startxt", "Iscript"}

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

    Public SCMenCount As Byte = 106

    Public Function CheckOverFlow(Datfiles As SCDatFiles.DatFiles, Value As Long) As Boolean
        Return SCCodeCount(Datfiles) > Value
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
    Private GRPFiles(SCImageCount) As String
    Private ImageName(SCImageCount) As String
    Private pSfxName(SCSfxdataCount) As String
    Private pPortdataName(SCPortdataCount) As String
    Private pIconName(SCIconCount) As String
    Private pIscriptName(SCIscriptCount) As String
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
    Public ReadOnly Property SfxName(index As Integer) As String
        Get
            Return pSfxName(index)
        End Get
    End Property
    Public ReadOnly Property PortdataName(index As Integer) As String
        Get
            Return pPortdataName(index)
        End Get
    End Property


    Private ExtraBtnStr(22) As String
    Public ReadOnly Property BtnStr(index As Integer) As String
        Get
            Return ExtraBtnStr(index)
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
    Public ReadOnly Property GetGRP(index As Integer, frame As Integer, IsRemaster As Boolean) As BitmapImage
        Get
            If IsRemaster Then
                Return Nothing
            Else
                Return SDGRP(index).DrawGRP(frame)
            End If
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

    Public Sub New()
        DefaultDat = New SCDatFiles(False)
        DefaultExtraDat = New ExtraDatFiles
        Offsets = New Dictionary(Of String, String)
        '오프셋 읽기

        stat_txt = New tblReader(Tool.GetTblFolder & "\stat_txt.tbl")
        stat_txt_kor_eng = New tblReader(Tool.GetTblFolder & "\stat_txt_kor_eng.tbl")
        stat_txt_kor_kor = New tblReader(Tool.GetTblFolder & "\stat_txt_kor_kor.tbl")

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

        LoadMPQData()

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
            Tool.ErrorMsgBox(Tool.GetText("Error LoadMPQData Fail"))
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




