Imports System.IO
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.Drawing

Public Enum PalettType
    ashworld = 0
    badlands = 1
    desert = 2
    ice = 3
    install = 4
    jungle = 5
    platform = 6
    twilight = 7

    Icons = 8

    bexpl = 9
    bfire = 10
    gfire = 11
    ofire = 12
    SelCircle = 13
    Wireframe = 14

    shadow = 15
    Hallulation = 16
    EMP = 17
End Enum


Namespace MGRP
    Public Module GRPMoudle
        Public tilesetname() As String = {"badlands", "platform", "install", "ashworld", "jungle", "desert", "ice", "twilight"}

        Private pPalett() As CPalett
        Public ReadOnly Property Palett(PType As PalettType) As CPalett
            Get
                Return pPalett(PType)
            End Get
        End Property

        Private pRemappingPallet() As CPCX
        Public ReadOnly Property RemappingPallet(PType As Remapping) As CPCX
            Get
                Return pRemappingPallet(PType)
            End Get
        End Property

        Public Sub GRPMoudleInit()
            ReDim pPalett(17)
            For i = 0 To 17
                pPalett(i) = New CPalett(i)
            Next

            ReDim pRemappingPallet(6)

            Dim pcxstr() As String = {"ofire", "gfire", "bfire", "bexpl", "trans50", "dark", "shift"}
            'RemappingPallet(0) = New CPCX
            'RemappingPallet(0).LoadPCX(mpq.ReaddatFile("tileset\" & tilesetname(TileSetType) & "\" & pcxstr(0) & ".pcx"))

            For i = 0 To 6
                pRemappingPallet(i) = New CPCX
                pRemappingPallet(i).LoadPCX(Tool.CascData.ReadFile("tileset\" & tilesetname(3) & "\" & pcxstr(i) & ".pcx"))
            Next
        End Sub


    End Module
    Public Class CPCX
        Private ppheight As Integer
        Private PCXdata(,) As Byte 'Byte(,)
        Public Function GetPixel(OrigP As Byte, RemapP As Byte) As Byte
            If RemapP < (PCXdata.Length \ 256) Then
                If RemapP > 0 Then
                    Return PCXdata(RemapP - 1, OrigP)
                Else
                    Return PCXdata(RemapP, OrigP)
                End If
            Else
                Return PCXdata(10, OrigP)
            End If
        End Function

        Public Sub LoadPCX(buffer() As Byte)
            Dim memStream As New MemoryStream(buffer)
            Dim binaReader As New BinaryReader(memStream)

            memStream.Position = &H8
            Dim pwidth As UInt16 = binaReader.ReadUInt16()
            Dim pheight As UInt16 = binaReader.ReadUInt16()
            ReDim PCXdata(pheight, pwidth)
            ppheight = pheight
            memStream.Position = &H80

            Dim xpos As UInt16

            Dim opcode As Byte
            'MsgBox(pheight & " Start")


            For ypos = 0 To pheight
                'If buffer.Length = 9455 Then
                '    MsgBox(ypos & "," & pheight)
                'End If
                xpos = 0
                While xpos <= pwidth
                    opcode = binaReader.ReadByte()
                    'If buffer.Length = 9455 Then
                    '    MsgBox(xpos & "," & pwidth & "," & (opcode And 192) & "," & (opcode And 63))
                    'End If
                    If opcode >= &HC0 Then '만큼 다음 바이트 출력
                        Dim newxtcode As Byte = binaReader.ReadByte()

                        For i = 0 To opcode - &HC1
                            If xpos <= pwidth Then
                                PCXdata(ypos, xpos) = newxtcode
                            End If
                            xpos += 1
                        Next
                    Else
                        PCXdata(ypos, xpos) = opcode
                        xpos += 1
                    End If
                End While
            Next
            'MsgBox(pheight & " End")



            'Dim str As String = ""
            'For i = 0 To 0
            '    If pheight = 0 Then
            '        For j = 0 To pwidth
            '            str = str & Hex(PCXdata(i, j)) & " "
            '        Next
            '        str = str & vbCrLf
            '        MsgBox(str)
            '    End If
            'Next




            binaReader.Close()
            memStream.Close()
        End Sub
    End Class


    Public Enum Remapping
        ofire = 0
        gfire = 1
        bfire = 2
        bexpl = 3
        trans50 = 4

        dark = 5
        shift = 6

        'DrawFunction = 5 trans50.pcx
        'DrawFunction = 6 trans50.pcx
        'DrawFunction = 7 trans50.pcx
    End Enum
End Namespace

Public Class CPalett
    Private ReadOnly Property PalletPath As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Palletes\"
        End Get
    End Property

    Private Palett(255) As Color
    Public ReadOnly Property GetColor(index As Integer) As Color
        Get
            Return Palett(index)
        End Get
    End Property

    Public Sub New(PalletNum As PalettType)
        Dim Filename As String = ""

        Select Case PalletNum
            Case PalettType.ashworld
                Filename = PalletPath & "ashworld.wpe"
            Case PalettType.badlands
                Filename = PalletPath & "badlands.wpe"
            Case PalettType.desert
                Filename = PalletPath & "desert.wpe"
            Case PalettType.ice
                Filename = PalletPath & "ice.wpe"
            Case PalettType.install
                Filename = PalletPath & "install.wpe"
            Case PalettType.jungle
                Filename = PalletPath & "jungle.wpe"
            Case PalettType.platform
                Filename = PalletPath & "platform.wpe"
            Case PalettType.twilight
                Filename = PalletPath & "twilight.wpe"
            Case PalettType.Icons
                Filename = PalletPath & "Icons.act"
            Case PalettType.bexpl
                Filename = PalletPath & "bexpl.act"
            Case PalettType.bfire
                Filename = PalletPath & "bfire.act"
            Case PalettType.gfire
                Filename = PalletPath & "gfire.act"
            Case PalettType.ofire
                Filename = PalletPath & "ofire.act"
            Case PalettType.SelCircle
                Filename = PalletPath & "SelCircle.act"
            Case PalettType.Wireframe
                Filename = PalletPath & "Wireframe.act"
            Case PalettType.shadow
                Filename = PalletPath & "shadow.act"
            Case PalettType.Hallulation
                Filename = PalletPath & "Hallulation.act"
            Case PalettType.EMP
                Filename = PalletPath & "EMP.act"
        End Select






        Dim filestream As New FileStream(Filename, FileMode.Open)
        Dim binaryreader As New BinaryReader(filestream)

        If filestream.Length = 256 * 3 Then
            For i = 0 To 255
                Dim r, g, b As Byte
                r = binaryreader.ReadByte()
                g = binaryreader.ReadByte()
                b = binaryreader.ReadByte()
                Palett(i) = Color.FromArgb(r, g, b)
            Next
        Else
            For i = 0 To 255
                Dim r, g, b As Byte
                r = binaryreader.ReadByte()
                g = binaryreader.ReadByte()
                b = binaryreader.ReadByte()
                binaryreader.ReadByte()

                'If DrawFunction = 16 Then
                '    r = 150
                '    g = 150
                '    Try
                '        b = b + 150
                '    Catch ex As Exception
                '        b = 255
                '    End Try

                'End If


                Palett(i) = Color.FromArgb(r, g, b)
            Next
        End If


        binaryreader.Close()
        filestream.Close()
    End Sub
End Class

Public Class GRP
    Public GRPfilename As String

    Public Palett As CPalett
    Public RemappingPalett As MGRP.CPCX


    Public framecount As UInteger
    Public grpWidth As UInteger
    Public grpHeight As UInteger
    Public GRPFrame As New List(Of GRPFrameData)


    Private GRPCashing() As BitmapImage

    Private isremapping As Boolean
    Private paletttypenum As Integer
    Private DrawFunction As Integer
    Private RemappingNum As PalettType

    Structure GRPFrameData
        Public frameXOffset As Byte
        Public frameYOffset As Byte
        Public frameWidth As Byte
        Public frameHeight As Byte
        Public Image() As Byte
        Public IineTableOffset As UInteger


        Public frameWidth4 As UShort
    End Structure



    Public Shared Function Bitmap2BitmapImage(bitmap As Bitmap) As BitmapImage
        Dim hBitmap As IntPtr = bitmap.GetHbitmap()
        Dim retval As New BitmapImage


        Dim bitmapSource As BitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())

        Dim encoder = New BmpBitmapEncoder()
        Dim memoryStream = New MemoryStream()

        encoder.Frames.Add(BitmapFrame.Create(bitmapSource))
        encoder.Save(memoryStream)

        retval.BeginInit()
        retval.CacheOption = BitmapCacheOption.OnLoad
        retval.StreamSource = New MemoryStream(memoryStream.ToArray())
        retval.EndInit()

        memoryStream.Close()
        retval.Freeze()


        'Dim prgbaSource As BitmapSource = New FormatConvertedBitmap(retval, PixelFormats.Pbgra32, Nothing, 0)
        'Dim bmp As WriteableBitmap = New WriteableBitmap(prgbaSource)
        'Dim w As Integer = bmp.PixelWidth
        'Dim h As Integer = bmp.PixelHeight
        'Dim pixelData(w * h) As Integer

        'Dim widthInBytes As Integer = bmp.PixelWidth * (bmp.Format.BitsPerPixel / 8)


        'bmp.CopyPixels(pixelData, widthInBytes, 0)
        'bmp.WritePixels(New Int32Rect(0, 0, w, h), pixelData, widthInBytes, 0)
        'retval = Nothing


        'Return bmp

        Return retval
    End Function


    Public Sub New(filename As String, Optional _DrawFunction As Byte = 0, Optional _RemappingNum As Byte = 0, Optional _PalletType As PalettType = PalettType.platform)
        'LoadGRP(Tool.LoadDataFromMPQ("unit\" & filename))
        LoadGRP(Tool.CascData.ReadFile("unit\" & filename))

        isremapping = False
        DrawFunction = _DrawFunction
        RemappingNum = _RemappingNum

        If DrawFunction = 0 Then
            Select Case _PalletType
                Case PalettType.bexpl
                    isremapping = True
                Case PalettType.bfire
                    isremapping = True
                Case PalettType.gfire
                    isremapping = True
                Case PalettType.ofire
                    isremapping = True
            End Select
        Else
            Select Case DrawFunction
                Case 8
                    _PalletType = PalettType.EMP
                Case 9
                    Select Case RemappingNum
                        Case 0
                            _PalletType = PalettType.install
                        Case 1
                            RemappingPalett = MGRP.RemappingPallet(MGRP.Remapping.ofire)
                            _PalletType = PalettType.ofire
                            isremapping = True
                        Case 2
                            RemappingPalett = MGRP.RemappingPallet(MGRP.Remapping.gfire)
                            _PalletType = PalettType.gfire
                            isremapping = True
                        Case 3
                            RemappingPalett = MGRP.RemappingPallet(MGRP.Remapping.bfire)
                            _PalletType = PalettType.bfire
                            isremapping = True
                        Case 4
                            RemappingPalett = MGRP.RemappingPallet(MGRP.Remapping.bexpl)
                            _PalletType = PalettType.bexpl
                            isremapping = True
                        Case Else
                            _PalletType = PalettType.install
                    End Select

                Case 10
                    RemappingPalett = MGRP.RemappingPallet(MGRP.Remapping.dark)
                    _PalletType = PalettType.shadow
                    isremapping = True
                Case 16
                    RemappingPalett = MGRP.RemappingPallet(MGRP.Remapping.shift)
                    _PalletType = PalettType.Hallulation
                Case Else
                    _PalletType = PalettType.install
            End Select
        End If

        Palett = MGRP.Palett(_PalletType)
        'LoadPalette(_PalletType, _DrawFunction, _RemappingNum)
        paletttypenum = _PalletType

        ReDim GRPCashing(framecount)
    End Sub


    Public Sub New(filebytes As Byte(), Optional _DrawFunction As Byte = 0, Optional _RemappingNum As Byte = 0, Optional _PalletType As PalettType = PalettType.platform)
        'LoadGRP(Tool.LoadDataFromMPQ("unit\" & filename))
        LoadGRP(filebytes)

        isremapping = False
        DrawFunction = _DrawFunction
        RemappingNum = _RemappingNum

        If DrawFunction = 0 Then
            Select Case _PalletType
                Case PalettType.bexpl
                    isremapping = True
                Case PalettType.bfire
                    isremapping = True
                Case PalettType.gfire
                    isremapping = True
                Case PalettType.ofire
                    isremapping = True
            End Select
        Else
            Select Case DrawFunction
                Case 8
                    _PalletType = PalettType.EMP
                Case 9
                    Select Case RemappingNum
                        Case 0
                            _PalletType = PalettType.install
                        Case 1
                            RemappingPalett = MGRP.RemappingPallet(MGRP.Remapping.ofire)
                            _PalletType = PalettType.ofire
                            isremapping = True
                        Case 2
                            RemappingPalett = MGRP.RemappingPallet(MGRP.Remapping.gfire)
                            _PalletType = PalettType.gfire
                            isremapping = True
                        Case 3
                            RemappingPalett = MGRP.RemappingPallet(MGRP.Remapping.bfire)
                            _PalletType = PalettType.bfire
                            isremapping = True
                        Case 4
                            RemappingPalett = MGRP.RemappingPallet(MGRP.Remapping.bexpl)
                            _PalletType = PalettType.bexpl
                            isremapping = True
                        Case Else
                            _PalletType = PalettType.install
                    End Select

                Case 10
                    RemappingPalett = MGRP.RemappingPallet(MGRP.Remapping.dark)
                    _PalletType = PalettType.shadow
                    isremapping = True
                Case 16
                    RemappingPalett = MGRP.RemappingPallet(MGRP.Remapping.shift)
                    _PalletType = PalettType.Hallulation
                Case Else
                    _PalletType = PalettType.install
            End Select
        End If

        Palett = MGRP.Palett(_PalletType)
        'LoadPalette(_PalletType, _DrawFunction, _RemappingNum)
        paletttypenum = _PalletType

        ReDim GRPCashing(framecount)
    End Sub




    Public Overloads Function LoadGRP(buffer As Byte())
        Dim memreader As New MemoryStream(buffer)
        Dim binaryreader As New BinaryReader(memreader)


        memreader.Position = 0
        framecount = binaryreader.ReadUInt16()
        grpWidth = binaryreader.ReadUInt16()
        grpHeight = binaryreader.ReadUInt16()

        'MsgBox(framecount)
        Dim framePos As UInteger = memreader.Position
        For i = 0 To framecount - 1 '프레임 수만큼.
            Dim TempGRP As GRPFrameData
            memreader.Position = framePos
            TempGRP.frameXOffset = binaryreader.ReadByte()
            TempGRP.frameYOffset = binaryreader.ReadByte()
            TempGRP.frameWidth = binaryreader.ReadByte()
            TempGRP.frameHeight = binaryreader.ReadByte()
            TempGRP.IineTableOffset = binaryreader.ReadUInt32()
            framePos = memreader.Position



            Dim tempimage() As Byte

            Dim size As UInteger
            If TempGRP.frameWidth Mod 4 = 0 Then
                size = CInt(TempGRP.frameWidth) * CInt(TempGRP.frameHeight) - 1
                TempGRP.frameWidth4 = CInt(CInt(TempGRP.frameWidth))
            Else
                size = CInt(TempGRP.frameWidth) * CInt(TempGRP.frameHeight) - 1 + (4 - (TempGRP.frameWidth Mod 4)) * CInt(TempGRP.frameHeight)

                TempGRP.frameWidth4 = CInt(TempGRP.frameWidth) + (4 - (TempGRP.frameWidth Mod 4))
            End If
            ReDim tempimage(size)

            Dim tempimageindex As Integer
            tempimageindex = 0

            Dim opcode As UInteger
            Dim nextcode As UInteger
            Dim count As Integer
            Dim temp As UInteger
            For j = 0 To TempGRP.frameHeight - 1 '가로 줄 수.
                If TempGRP.frameWidth Mod 4 <> 0 Then
                    tempimageindex = (TempGRP.frameWidth + 4 - (TempGRP.frameWidth Mod 4)) * j
                End If

                'MsgBox()
                memreader.Position = TempGRP.IineTableOffset + j * 2
                temp = binaryreader.ReadUInt16() '상대적 좌표.
                memreader.Position = TempGRP.IineTableOffset + temp '실제 라인 데이터
                'IineTableOffset으로 부터 Y만큼 2바이트씩 그게.각 가로줄 하나.
                '가로줄 하나는 세로줄의 길이를 가지고 있음.
                'Grpdata(TempGRP.IineTableOffset)

                'Byte >= 0x80 : (byte - 0x80)만큼 0을 출력
                '0x80 > byte >= 0x40 : (byte - 0x40)만큼 다음 바이트를 반복해서 출력
                '0x40 > byte : 다음 byte만큼의 byte를 그대로 출력

                'MsgBox("Line " & j)
                count = 0
                While (True)
                    opcode = binaryreader.ReadByte()

                    'MsgBox(count & " before " & TempGRP.frameWidth & " " & Format(opcode, "X"))
                    If opcode >= &H80 Then
                        For k As Integer = 1 To (opcode - &H80)
                            tempimage(tempimageindex) = 0
                            tempimageindex += 1
                            count += 1
                        Next
                    ElseIf &H80 > opcode And opcode >= &H40 Then
                        nextcode = binaryreader.ReadByte()
                        For k As Integer = 1 To (opcode - &H40)
                            tempimage(tempimageindex) = nextcode
                            tempimageindex += 1
                            count += 1
                        Next
                    ElseIf &H40 > opcode Then
                        For k As Integer = 1 To opcode
                            tempimage(tempimageindex) = binaryreader.ReadByte()
                            tempimageindex += 1
                            count += 1
                        Next
                    End If
                    'MsgBox(count & " after " & TempGRP.frameWidth)
                    If count = TempGRP.frameWidth Then

                        Exit While
                    End If
                End While

            Next

            TempGRP.Image = tempimage


            GRPFrame.Add(TempGRP)
        Next

        memreader.Close()
        binaryreader.Close()

        Return True
    End Function


    Public Function DrawToBytes(Images As SCImage, ByRef Pixels() As Byte, Size As Size, Optional Unitcolor As Integer = 0) As Boolean
        Dim Frame As Integer = Images.GetFrameGRP
        Dim Pos As Windows.Point = Images.Location

        If Frame >= framecount Then
            For y = 0 To Size.Height - 1
                For x = 0 To Size.Width - 1
                    Pixels(x + y * Size.Width) = 125
                Next
            Next

            Return False
        End If


        Dim gp() As Byte = GRPFrame(Frame).Image

        Dim grph As Integer = GRPFrame(Frame).frameHeight
        Dim grpw As Integer = GRPFrame(Frame).frameWidth4

        Dim grpox As Integer = GRPFrame(Frame).frameXOffset
        Dim grpoy As Integer = GRPFrame(Frame).frameYOffset
        If Images.direction > 16 And Images.IsTrunable And Images.ControlStatus = 0 Then
            grpox = grpWidth - grpox - grpw
        End If



        Dim cenx As Integer = Size.Width \ 2 - grpWidth \ 2
        Dim ceny As Integer = Size.Height \ 2 - grpHeight \ 2

        For y = 0 To grph - 1
            For x = 0 To grpw - 1
                Dim cvalue As Integer

                If Images.direction > 16 And Images.IsTrunable And Images.ControlStatus = 0 Then
                    cvalue = gp(grpw - 1 - x + y * grpw)
                Else
                    cvalue = gp(x + y * grpw)
                End If
                If Unitcolor > 127 And cvalue <> 0 Then
                    cvalue = 135
                End If

                If cvalue <> 0 Then
                    Dim px As Integer = Pos.X + x + grpox + cenx
                    Dim py As Integer = Pos.Y + y + grpoy + ceny

                    If (0 <= px And px < Size.Width) And (0 <= py And py < Size.Height) Then
                        If isremapping Then
                            Dim gvalue As Integer = Pixels(px + py * Size.Width)
                            If paletttypenum = PalettType.shadow Then
                                Pixels(px + py * Size.Width) = RemappingPalett.GetPixel(gvalue, 30)
                            Else
                                Pixels(px + py * Size.Width) = RemappingPalett.GetPixel(gvalue, cvalue)
                            End If

                        Else
                            Pixels(px + py * Size.Width) = cvalue
                        End If


                    End If
                End If
            Next
        Next



        Return True
    End Function




    Public Function DrawGRP(frame As Integer, Optional Unitcolor As Integer = 0, Optional FileBackGround As Boolean = False) As BitmapImage

        frame = frame Mod framecount
        If GRPCashing(frame) IsNot Nothing Then
            Return GRPCashing(frame)
        End If

        Dim unitColors(7) As Byte

        If Unitcolor <> 0 Then
            Dim filestream As New FileStream(My.Application.Info.DirectoryPath & "\Data\" & "Colorunit.dat", FileMode.Open)
            Dim binaryreader As New BinaryReader(filestream)

            filestream.Position = 8 * (Unitcolor - 1)
            For i = 0 To 7
                unitColors(i) = binaryreader.ReadByte()
            Next
            binaryreader.Close()
            filestream.Close()
        End If

        Try
            Dim bm As New Bitmap(GRPFrame(frame).frameWidth, GRPFrame(frame).frameHeight, Imaging.PixelFormat.Format8bppIndexed)
            Dim bmd As New BitmapData
            bmd = bm.LockBits(New Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, Imaging.PixelFormat.Format8bppIndexed)

            Dim scan0 As IntPtr = bmd.Scan0
            Dim stride As Integer = bmd.Stride

            Dim pixels(GRPFrame(frame).Image.Length - 1) As Byte
            'Marshal.Copy(scan0, pixels, 0, pixels.Length)

            pixels = GRPFrame(frame).Image

            Marshal.Copy(pixels, 0, scan0, pixels.Length)

            bm.UnlockBits(bmd)


            Dim CPalette As Imaging.ColorPalette
            CPalette = bm.Palette
            For i = 0 To 255
                If 15 >= i And i >= 8 And Unitcolor <> 0 And isremapping = False Then
                    CPalette.Entries(i) = Palett.GetColor(unitColors(i - 8))
                Else
                    CPalette.Entries(i) = Palett.GetColor(i)
                End If

                If RemappingNum = 4 Or RemappingNum = 1 Then
                    If i > 64 Then
                        CPalette.Entries(i) = Color.DarkRed
                    End If
                ElseIf RemappingNum = 3 Then
                    If i > 41 Then
                        CPalette.Entries(i) = Color.DarkRed
                    End If
                ElseIf RemappingNum = 2 Then
                    If i > 33 Then
                        CPalette.Entries(i) = Color.DarkRed
                    End If
                End If
            Next
            bm.Palette = CPalette


            If FileBackGround = True Then
                bm.MakeTransparent(Color.Black)
            End If


            GRPCashing(frame) = Bitmap2BitmapImage(bm)
            Return GRPCashing(frame)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function




    Public Function Reset()
        framecount = 0
        grpWidth = 0
        grpHeight = 0
        GRPFrame.Clear()

        Return 0
    End Function

End Class