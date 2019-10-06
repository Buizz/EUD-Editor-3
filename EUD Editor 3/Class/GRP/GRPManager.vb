Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()>
Public Class GRPManager
    Private DefaultDat As SCDatFiles

    Private GRPFiles(SCImageCount) As String
    Private ReadOnly Property GRPTextPath As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Texts\GRPfile.txt"
        End Get
    End Property



    Private IsLoadMPQ As Boolean
    Public ReadOnly Property LoadStarCraftData As Boolean
        Get
            Return IsLoadMPQ
        End Get
    End Property


    Private SDGRP(SCImageCount) As SDGRP
    Private HDGRP(SCImageCount) As HDGRP
    Private CTGRP(SCImageCount) As HDGRP


    Private SDICON(SCIconCount) As ICONGRP
    Private HDICON(SCIconCount) As ICONGRP
    Private CTICON(SCIconCount) As ICONGRP


    Private SDWireFrame(SCUnitCount) As WIREFRAMEGRP
    Private SDGrpFrame(SCGrpWireCount) As WIREFRAMEGRP
    Private SDTranFrame(SCMenCount) As WIREFRAMEGRP

    Private HDWireFrame(SCUnitCount) As WIREFRAMEGRP
    Private HDGrpFrame(SCGrpWireCount) As WIREFRAMEGRP
    Private HDTranFrame(SCMenCount) As WIREFRAMEGRP

    Private CTWireFrame(SCUnitCount) As WIREFRAMEGRP
    Private CTGrpFrame(SCGrpWireCount) As WIREFRAMEGRP
    Private CTTranFrame(SCMenCount) As WIREFRAMEGRP


    Private isSDGLoad As Boolean
    Private isHDGLoad As Boolean
    Private isCarbotGLoad As Boolean


    Public ReadOnly Property GetGRPImage(index As Integer, frame As Integer) As BitmapSource
        Get
            Select Case pgData.Setting(ProgramData.TSetting.Graphic)
                Case 1
                    Try
                        Return SDGRP(index).DrawGRP(frame)
                    Catch ex As Exception
                        'MsgBox(ex.ToString & vbCrLf & index)
                    End Try

                Case 2
                    Try
                        Return HDGRP(index).DrawGRP(frame)
                    Catch ex As Exception
                        'MsgBox(ex.ToString & vbCrLf & index)
                    End Try
                Case 3
                    Try
                        Return CTGRP(index).DrawGRP(frame)
                    Catch ex As Exception
                        'MsgBox(ex.ToString & vbCrLf & index)
                    End Try
            End Select


            Return New BitmapImage()
            'Return SDGRP(index).DrawGRP(frame)
        End Get
    End Property
    Public ReadOnly Property GetGrp(index As Integer, grptype As Integer) As RGRP
        Get
            Select Case grptype
                Case 1
                    Try
                        Return SDGRP(index)
                    Catch ex As Exception
                        'MsgBox(ex.ToString & vbCrLf & index)
                    End Try

                Case 2
                    Try
                        Return HDGRP(index)
                    Catch ex As Exception
                        'MsgBox(ex.ToString & vbCrLf & index)
                    End Try
                Case 3
                    Try
                        Return CTGRP(index)
                    Catch ex As Exception
                        'MsgBox(ex.ToString & vbCrLf & index)
                    End Try
            End Select

            Return Nothing
            'Return SDGRP(index)
        End Get
    End Property
    Public ReadOnly Property GetIcon(index As Integer, grptype As Integer) As BitmapSource
        Get
            Select Case grptype
                Case 1
                    Try
                        Return SDICON(index).DrawGRP(0)
                    Catch ex As Exception
                    End Try

                Case 2
                    Try
                        Return HDICON(index).DrawGRP(0)
                    Catch ex As Exception
                    End Try
                Case 3
                    Try
                        Return CTICON(index).DrawGRP(0)
                    Catch ex As Exception
                    End Try
            End Select
            Return New BitmapImage()
        End Get
    End Property
    Public ReadOnly Property GetWireFrame(index As Integer, grptype As Integer) As BitmapSource
        Get
            Select Case grptype
                Case 1
                    Try
                        Return SDWireFrame(index).DrawWire(0)
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try

                Case 2
                    Try
                        Return HDWireFrame(index).DrawWire(0)
                    Catch ex As Exception
                    End Try
                Case 3
                    Try
                        Return CTWireFrame(index).DrawWire(0)
                    Catch ex As Exception
                    End Try
            End Select
            Return New BitmapImage()
            'Return SDWireFrame.DrawGRP(index)
        End Get
    End Property
    Public ReadOnly Property GetGrpFrame(index As Integer, grptype As Integer) As BitmapSource
        Get
            Select Case grptype
                Case 1
                    Try
                        Return SDGrpFrame(index).DrawWire(0)
                    Catch ex As Exception
                    End Try

                Case 2
                    Try
                        Return HDGrpFrame(index).DrawGRP(0)
                    Catch ex As Exception
                    End Try
                Case 3
                    Try
                        Return CTGrpFrame(index).DrawGRP(0)
                    Catch ex As Exception
                    End Try
            End Select
            Return New BitmapImage()
            'Return SDGrpFrame.DrawGRP(index)
        End Get
    End Property
    Public ReadOnly Property GetTranFrame(index As Integer, grptype As Integer) As BitmapSource
        Get
            Select Case grptype
                Case 1
                    Try
                        Return SDTranFrame(index).DrawWire(0)
                    Catch ex As Exception
                    End Try

                Case 2
                    Try
                        Return HDTranFrame(index).DrawGRP(0)
                    Catch ex As Exception
                    End Try
                Case 3
                    Try
                        Return CTTranFrame(index).DrawGRP(0)
                    Catch ex As Exception
                    End Try
            End Select
            Return New BitmapImage()
            'Return SDTramFrame.DrawGRP(index)
        End Get
    End Property


    Public Sub New(_DefaultDat As SCDatFiles)
        DefaultDat = _DefaultDat

        Dim sr As StreamReader = New StreamReader(GRPTextPath)
        Dim index As Integer = 0
        While Not sr.EndOfStream
            GRPFiles(index) = sr.ReadLine().Replace("<0>", "")

            index += 1
        End While
        sr.Close()
    End Sub

    Public Sub LoadGRPData()
        Dim GRPRefresh As Boolean = False

        Dim grptype As Integer = pgData.Setting(ProgramData.TSetting.Graphic)


        If grptype = 0 Then
            IsLoadMPQ = True
            Return
        End If

        'MsgBox(pgData.Setting(ProgramData.TSetting.Graphic))


        IsLoadMPQ = False
        If Not My.Computer.FileSystem.FileExists(pgData.Setting(ProgramData.TSetting.starcraft)) Then
            pgData.Setting(ProgramData.TSetting.Graphic) = 0
            Tool.ErrorMsgBox(Tool.GetText("Error NotExistMPQ"))
            IsLoadMPQ = True
            Return
        End If
        Tool.CascData.OpenCascStorage()
        Try
            Select Case grptype
                Case 1
                    GRPRefresh = LoadSDGRP()
                Case 2
                    GRPRefresh = LoadHDGRP()
                Case 3
                    GRPRefresh = LoadCarbotGRP()
            End Select


        Catch ex As Exception
            pgData.Setting(ProgramData.TSetting.Graphic) = 0
            Tool.ErrorMsgBox(Tool.GetText("Error LoadMPQData Fail"), ex.ToString)
            IsLoadMPQ = True
            Return
        End Try
        Tool.CascData.CloseCascStorage()


        IsLoadMPQ = True

        If GRPRefresh Then
            SaveData()
        End If
    End Sub




    Private Function LoadSDGRP() As Boolean
        If isSDGLoad Then
            '프로그램에서 완전히 그래픽을 로드한 경우
            Return False
        End If







        '데이터를 추출하는 과정
        Dim pos As UInteger = 0
        Dim bytes As Byte() = Tool.CascData.ReadFileCascStorage("SD/mainSD.anim")
        If bytes.Count = 0 Then
            Throw New Exception("CascOpenError")
        End If



        BReader.ReadUint32(pos, bytes) 'unsigned Int magic; // "ANIM"
        BReader.ReadUint16(pos, bytes) 'unsigned Short version; // Version? 0x0101, 0x0202, 0x0204
        BReader.ReadUint16(pos, bytes) 'unsigned Short unk2; // 0 -- more bytes for version?
        Dim layercount As UInt16 = BReader.ReadUint16(pos, bytes) 'unsigned Short layers; 레이어의 갯수
        Dim entrycount As UInt16 = BReader.ReadUint16(pos, bytes) 'unsigned Short entries;

        Dim layerstrs As New List(Of String)
        For i = 0 To 9
            Dim tstr As String = ""
            For j = 0 To 31
                Dim tb As Byte = BReader.ReadByte(pos, bytes)

                tstr = tstr & Chr(tb)

            Next
            tstr = tstr.Split(Chr(0)).First

            layerstrs.Add(tstr)
        Next
        Dim entrypoints(entrycount) As UInteger
        For i = 0 To entrycount - 1
            entrypoints(i) = BReader.ReadUint32(pos, bytes)
        Next
        For i = 0 To entrycount - 1
            Dim grpindex As Integer = DefaultDat.Data(SCDatFiles.DatFiles.images, "GRP File", i)
            Dim DrawFunc As Integer = DefaultDat.Data(SCDatFiles.DatFiles.images, "Draw Function", i)
            Dim Remapping As Integer = DefaultDat.Data(SCDatFiles.DatFiles.images, "Remapping", i)
            Dim filename As String = "unit\" & GRPFiles(grpindex)
            Dim oldGRP As New GRP(Tool.CascData.ReadFileCascStorage(filename), DrawFunc, Remapping)

            Dim grpfile As String = GRPFiles(DefaultDat.Data(SCDatFiles.DatFiles.images, "GRP File", i)).ToLower

            pos = entrypoints(i)

            Dim framescount As UInt16 = BReader.ReadUint16(pos, bytes)
            BReader.ReadUint16(pos, bytes) 'always 0xFFFF?

            Dim grpwidth As UInt16 = BReader.ReadUint16(pos, bytes) 'width and height are 0 in SD images, and should be retrieved from the appropriate GRP file.
            Dim grpheight As UInt16 = BReader.ReadUint16(pos, bytes)


            Dim maingrpwidth As Integer
            Dim maingrpheight As Integer

            Dim frameinfoptr As UInteger = BReader.ReadUint32(pos, bytes)
            '프레임들 쫙 있음


            Dim mainbitsource As ByteBitmap = Nothing
            For j = 0 To layercount - 1
                'If i = 27 Then
                '    MsgBox("ID : " & i & " offset : " & pos)
                'End If


                Dim ddsptr As UInt32 = BReader.ReadUint32(pos, bytes)
                Dim ddssize As UInt32 = BReader.ReadUint32(pos, bytes)

                Dim width As UInt16 = BReader.ReadUint16(pos, bytes)
                Dim height As UInt16 = BReader.ReadUint16(pos, bytes)
                If ddssize = 0 Then
                    Continue For
                End If

                If layerstrs(j) = "diffuse" Then
                    Dim tpos As UInteger = ddsptr
                    Dim ms As New MemoryStream(BReader.ReadBytes(tpos, ddssize, bytes))
                    ms.Position = 0

                    mainbitsource = BitmapManager.LoadImage(ms)
                    maingrpwidth = width
                    maingrpheight = height

                    ms.Close()
                ElseIf layerstrs(j) = "teamcolor" Then
                    'MsgBox("ddsptr : " & ddsptr & vbCrLf &
                    '       "ddssize : " & ddssize & vbCrLf &
                    '       "width : " & width & vbCrLf &
                    '       "height : " & height)

                    Dim tpos As UInteger = ddsptr
                    BReader.ReadBytes(tpos, ddssize, bytes)
                End If
            Next

            Dim framedata As New List(Of FrameData)
            pos = frameinfoptr
            For j = 0 To framescount - 1
                framedata.Add(New FrameData(pos, bytes))
            Next

            SDGRP(i) = New SDGRP(i)
            If mainbitsource Is Nothing Then
                If i = 651 Then
                    SDGRP(i) = SDGRP(643)
                Else
                    For k = 0 To SDGRP.Count - 1
                        If SDGRP(k) IsNot Nothing Then
                            If SDGRP(k).grpfile = grpfile Then
                                SDGRP(i) = SDGRP(k)
                                Exit For
                            End If
                        End If
                    Next
                End If

            Else
                SDGRP(i).LoadGRP(mainbitsource, framedata, grpfile, New Size(maingrpwidth, maingrpheight))
                SDGRP(i).OldGRPLoad(oldGRP)
            End If

        Next



        Loadddsgrp("SD/unit/cmdicons/cmdicons.dds.grp", SDICON, True)
        Loadddsgrp("SD/unit/wirefram/wirefram.dds.grp", SDWireFrame, False)
        Loadddsgrp("SD/unit/wirefram/grpwire.dds.grp", SDGrpFrame, False)
        Loadddsgrp("SD/unit/wirefram/tranwire.dds.grp", SDTranFrame, False)
        '==========================================================================================================



        isSDGLoad = True

        Return True
    End Function







    Private Function LoadHDGRP() As Boolean
        If isHDGLoad Then
            Return False
        End If
        Dim pos As Integer = 0
        For imagecode = 0 To 998
            pos = 0
            Dim bytes As Byte() = Tool.CascData.ReadFileCascStorage("HD2/anim/main_" & String.Format("{0:D3}", imagecode) & ".anim")

            Dim framedata As New List(Of FrameData)
            Dim grpfile As String = GRPFiles(DefaultDat.Data(SCDatFiles.DatFiles.images, "GRP File", imagecode)).ToLower
            Dim mainbitsource As ByteBitmap = Nothing
            Dim maingrpwidth As Integer
            Dim maingrpheight As Integer
            If bytes.Count <> 0 Then
                BReader.ReadUint32(pos, bytes) 'unsigned Int magic; // "ANIM"
                BReader.ReadUint16(pos, bytes) 'unsigned Short version; // Version? 0x0101, 0x0202, 0x0204
                BReader.ReadUint16(pos, bytes) 'unsigned Short unk2; // 0 -- more bytes for version?
                Dim layercount As UInt16 = BReader.ReadUint16(pos, bytes) 'unsigned Short layers; 레이어의 갯수
                Dim entrycount As UInt16 = BReader.ReadUint16(pos, bytes) 'unsigned Short entries;

                Dim layerstrs As New List(Of String)
                For i = 0 To 9
                    Dim tstr As String = ""
                    For j = 0 To 31
                        Dim tb As Byte = BReader.ReadByte(pos, bytes)

                        tstr = tstr & Chr(tb)

                    Next
                    tstr = tstr.Split(Chr(0)).First

                    layerstrs.Add(tstr)
                Next

                Dim framescount As UInt16 = BReader.ReadUint16(pos, bytes)
                BReader.ReadUint16(pos, bytes) 'always 0xFFFF?

                Dim grpwidth As UInt16 = BReader.ReadUint16(pos, bytes) 'width and height are 0 in SD images, and should be retrieved from the appropriate GRP file.
                Dim grpheight As UInt16 = BReader.ReadUint16(pos, bytes)

                maingrpwidth = grpwidth
                maingrpheight = grpheight

                Dim frameinfoptr As UInteger = BReader.ReadUint32(pos, bytes)
                For j = 0 To layercount - 1
                    'If i = 27 Then
                    '    MsgBox("ID : " & i & " offset : " & pos)
                    'End If


                    Dim ddsptr As UInt32 = BReader.ReadUint32(pos, bytes)
                    Dim ddssize As UInt32 = BReader.ReadUint32(pos, bytes)

                    Dim width As UInt16 = BReader.ReadUint16(pos, bytes)
                    Dim height As UInt16 = BReader.ReadUint16(pos, bytes)


                    If ddssize = 0 Then
                        Continue For
                    End If

                    If layerstrs(j) = "diffuse" Then
                        Dim tpos As UInteger = ddsptr
                        Dim ms As New MemoryStream(BReader.ReadBytes(tpos, ddssize, bytes))
                        ms.Position = 0

                        mainbitsource = BitmapManager.LoadImage(ms)
                        ms.Close()
                        ms.Dispose()

                    ElseIf layerstrs(j) = "teamcolor" Then
                        Dim tpos As UInteger = ddsptr
                        BReader.ReadBytes(tpos, ddssize, bytes)
                    End If
                Next

                pos = frameinfoptr
                For j = 0 To framescount - 1
                    framedata.Add(New FrameData(pos, bytes))
                Next
            End If


            HDGRP(imagecode) = New HDGRP(imagecode)
            If mainbitsource Is Nothing Then
                If imagecode = 651 Then
                    HDGRP(imagecode) = HDGRP(643)
                Else
                    For k = 0 To HDGRP.Count - 1
                        If HDGRP(k) IsNot Nothing Then
                            If HDGRP(k).grpfile = grpfile Then
                                HDGRP(imagecode) = HDGRP(k)
                                Exit For
                            End If
                        End If
                    Next
                End If

            Else
                HDGRP(imagecode).LoadGRP(mainbitsource, framedata, grpfile, New Size(maingrpwidth, maingrpheight))

                'bytetotal += mainbitsource.Bytes.Count
            End If
        Next

        Loadddsgrp("HD2/unit/cmdicons/cmdicons.dds.grp", HDICON, True)
        Loadddsgrp("HD2/unit/wirefram/wirefram.dds.grp", HDWireFrame, False)
        Loadddsgrp("HD2/unit/wirefram/grpwire.dds.grp", HDGrpFrame, False)
        Loadddsgrp("HD2/unit/wirefram/tranwire.dds.grp", HDTranFrame, False)


        isHDGLoad = True

        Return True
    End Function


    Private Function LoadCarbotGRP() As Boolean
        If isCarbotGLoad Then
            Return False
        End If
        Dim pos As UInteger
        For imagecode = 0 To 998
            pos = 0
            Dim bytes As Byte() = Tool.CascData.ReadFileCascStorage("HD2/anim/Carbot/main_" & String.Format("{0:D3}", imagecode) & ".anim")

            Dim framedata As New List(Of FrameData)
            Dim grpfile As String = GRPFiles(DefaultDat.Data(SCDatFiles.DatFiles.images, "GRP File", imagecode)).ToLower
            Dim mainbitsource As ByteBitmap = Nothing
            Dim maingrpwidth As Integer
            Dim maingrpheight As Integer
            If bytes.Count <> 0 Then
                BReader.ReadUint32(pos, bytes) 'unsigned Int magic; // "ANIM"
                BReader.ReadUint16(pos, bytes) 'unsigned Short version; // Version? 0x0101, 0x0202, 0x0204
                BReader.ReadUint16(pos, bytes) 'unsigned Short unk2; // 0 -- more bytes for version?
                Dim layercount As UInt16 = BReader.ReadUint16(pos, bytes) 'unsigned Short layers; 레이어의 갯수
                Dim entrycount As UInt16 = BReader.ReadUint16(pos, bytes) 'unsigned Short entries;

                Dim layerstrs As New List(Of String)
                For i = 0 To 9
                    Dim tstr As String = ""
                    For j = 0 To 31
                        Dim tb As Byte = BReader.ReadByte(pos, bytes)

                        tstr = tstr & Chr(tb)

                    Next
                    tstr = tstr.Split(Chr(0)).First

                    layerstrs.Add(tstr)
                Next

                Dim framescount As UInt16 = BReader.ReadUint16(pos, bytes)
                BReader.ReadUint16(pos, bytes) 'always 0xFFFF?

                Dim grpwidth As UInt16 = BReader.ReadUint16(pos, bytes) 'width and height are 0 in SD images, and should be retrieved from the appropriate GRP file.
                Dim grpheight As UInt16 = BReader.ReadUint16(pos, bytes)

                maingrpwidth = grpwidth
                maingrpheight = grpheight

                Dim frameinfoptr As UInteger = BReader.ReadUint32(pos, bytes)
                For j = 0 To layercount - 1
                    'If i = 27 Then
                    '    MsgBox("ID : " & i & " offset : " & pos)
                    'End If


                    Dim ddsptr As UInt32 = BReader.ReadUint32(pos, bytes)
                    Dim ddssize As UInt32 = BReader.ReadUint32(pos, bytes)

                    Dim width As UInt16 = BReader.ReadUint16(pos, bytes)
                    Dim height As UInt16 = BReader.ReadUint16(pos, bytes)
                    If ddssize = 0 Then
                        Continue For
                    End If

                    If layerstrs(j) = "diffuse" Then
                        Dim tpos As UInteger = ddsptr
                        Dim ms As New MemoryStream(BReader.ReadBytes(tpos, ddssize, bytes))
                        ms.Position = 0

                        mainbitsource = BitmapManager.LoadImage(ms)
                        ms.Close()
                        ms.Dispose()

                    ElseIf layerstrs(j) = "teamcolor" Then
                        Dim tpos As UInteger = ddsptr
                        BReader.ReadBytes(tpos, ddssize, bytes)
                    End If
                Next

                pos = frameinfoptr
                For j = 0 To framescount - 1
                    framedata.Add(New FrameData(pos, bytes))
                Next
            End If


            CTGRP(imagecode) = New HDGRP(imagecode)
            If mainbitsource Is Nothing Then
                If imagecode = 651 Then
                    CTGRP(imagecode) = CTGRP(643)
                Else
                    For k = 0 To CTGRP.Count - 1
                        If CTGRP(k) IsNot Nothing Then
                            If CTGRP(k).grpfile = grpfile Then
                                CTGRP(imagecode) = CTGRP(k)
                                Exit For
                            End If
                        End If
                    Next
                End If

            Else
                CTGRP(imagecode).LoadGRP(mainbitsource, framedata, grpfile, New Size(maingrpwidth, maingrpheight))
            End If
        Next

        Loadddsgrp("HD2/Carbot/unit/cmdicons/cmdicons.dds.grp", CTICON, True)
        Loadddsgrp("HD2/Carbot/unit/wirefram/wirefram.dds.grp", CTWireFrame, False)
        Loadddsgrp("HD2/Carbot/unit/wirefram/grpwire.dds.grp", CTGrpFrame, False)
        Loadddsgrp("HD2/Carbot/unit/wirefram/tranwire.dds.grp", CTTranFrame, False)

        '프레임들 쫙 있음

        isCarbotGLoad = True


        Return True
    End Function



    Private Sub Loadddsgrp(filename As String, ByRef icongrp() As RGRP, isIcon As Boolean)

        Dim pos As Integer
        Dim iconbytes As Byte() = Tool.CascData.ReadFileCascStorage(filename)
        'Header:
        BReader.ReadUint32(pos, iconbytes) 'u32 filesize
        Dim iconframe As Integer = BReader.ReadUint16(pos, iconbytes) 'u16 Frame count
        BReader.ReadUint16(pos, iconbytes) 'u16 unknown(File version?) - -value appears to always be 0x1001 in the files I've seen.

        'which Is immediately followed by a series of File Entries

        For i = 0 To iconframe - 1
            Dim mainbitsource As ByteBitmap

            BReader.ReadUint32(pos, iconbytes) 'u32 unk - -always zero?
            Dim grpwidth As Integer = BReader.ReadUint16(pos, iconbytes) 'u16 width
            Dim grpheight As Integer = BReader.ReadUint16(pos, iconbytes) 'u16 height
            Dim ddssize As UInteger = BReader.ReadUint32(pos, iconbytes) 'u32 Size

            Dim ms As New MemoryStream(BReader.ReadBytes(pos, ddssize, iconbytes)) 'u8[Size] DDS file
            ms.Position = 0

            mainbitsource = BitmapManager.LoadImage(ms)
            ms.Close()
            ms.Dispose()

            If isIcon Then
                icongrp(i) = New ICONGRP()
            Else
                icongrp(i) = New WIREFRAMEGRP()
            End If
            icongrp(i).LoadGRP(mainbitsource, Nothing, Nothing, New Size(grpwidth, grpheight))
        Next
    End Sub


    Public Sub SaveData()
        Dim stm As Stream = File.Open(Tool.GRPSaveFilePath, FileMode.Create, FileAccess.ReadWrite)
        Dim bf As BinaryFormatter = New BinaryFormatter()
        bf.Serialize(stm, Me)
        stm.Close()
    End Sub
End Class
