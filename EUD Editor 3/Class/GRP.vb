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
End Enum


Public Class GRP
    Public GRPfilename As String

    Public Palett(255) As Color


    Public framecount As UInteger
    Public grpWidth As UInteger
    Public grpHeight As UInteger
    Public GRPFrame As New List(Of GRPFrameData)


    Private GRPCashing() As BitmapImage

    Private isremapping As Boolean
    Private paletttypenum As Integer
    Private DrawFunction As Integer
    Private RemappingNum As Integer

    Structure GRPFrameData
        Public frameXOffset As Byte
        Public frameYOffset As Byte
        Public frameWidth As Byte
        Public frameHeight As Byte
        Public Image() As Byte
        Public IineTableOffset As UInteger


        Public frameWidth4 As UShort
    End Structure


    Private ReadOnly Property PalletPath As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Palletes\"
        End Get
    End Property

    Public Sub New(filename As String, Optional _DrawFunction As Byte = 0, Optional _RemappingNum As Byte = 0, Optional _PalletType As PalettType = PalettType.platform)
        LoadGRP(Tool.LoadDataFromMPQ("unit\" & filename))
        LoadPalette(_PalletType, _DrawFunction, _RemappingNum)
        ReDim GRPCashing(framecount)
    End Sub

    Public Function LoadPalette(PalletNum As PalettType, Optional _DrawFunction As Byte = 0, Optional _RemappingNum As Byte = 0)
        Dim Filename As String = ""
        isremapping = False
        paletttypenum = PalletNum
        DrawFunction = _DrawFunction
        RemappingNum = _RemappingNum


        If DrawFunction = 0 Then
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
                    isremapping = True
                Case PalettType.bfire
                    Filename = PalletPath & "bfire.act"
                    isremapping = True
                Case PalettType.gfire
                    Filename = PalletPath & "gfire.act"
                    isremapping = True
                Case PalettType.ofire
                    Filename = PalletPath & "ofire.act"
                    isremapping = True
                Case PalettType.SelCircle
                    Filename = PalletPath & "SelCircle.act"
                Case PalettType.Wireframe
                    Filename = PalletPath & "Wireframe.act"
            End Select
        Else
            Select Case DrawFunction
                Case 0
                    Filename = PalletPath & "install.wpe"
                Case 8
                    Filename = PalletPath & "EMP.act"
                Case 9
                    Select Case RemappingNum
                        Case 0
                            Filename = PalletPath & "install.wpe"
                        Case 1
                            Filename = PalletPath & "ofire.act"
                            isremapping = True
                        Case 2
                            Filename = PalletPath & "gfire.act"
                            isremapping = True
                        Case 3
                            Filename = PalletPath & "bfire.act"
                            isremapping = True
                        Case 4
                            Filename = PalletPath & "bexpl.act"
                            isremapping = True
                        Case Else
                            Filename = PalletPath & "install.wpe"
                    End Select

                Case 10
                    Filename = PalletPath & "shadow.act"
                Case 16
                    Filename = PalletPath & "Hallulation.act"
                Case Else
                    Filename = PalletPath & "install.wpe"
            End Select
        End If






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
        Return 0
    End Function


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


    Private Function Bitmap2BitmapImage(bitmap As Bitmap) As BitmapImage
        Dim hBitmap As IntPtr = bitmap.GetHbitmap()
        Dim retval As New BitmapImage


        Dim bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())

        Dim encoder = New BmpBitmapEncoder()
        Dim memoryStream = New MemoryStream()

        encoder.Frames.Add(BitmapFrame.Create(bitmapSource))
        encoder.Save(memoryStream)

        retval.BeginInit()
        retval.StreamSource = New MemoryStream(memoryStream.ToArray())
        retval.EndInit()

        memoryStream.Close()

        Return retval
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

            '8~15

            Dim bmd As New BitmapData
            bmd = bm.LockBits(New Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, Imaging.PixelFormat.Format8bppIndexed)

            Dim scan0 As IntPtr = bmd.Scan0
            Dim stride As Integer = bmd.Stride

            Dim pixels(GRPFrame(frame).Image.Length - 1) As Byte
            Marshal.Copy(scan0, pixels, 0, pixels.Length)

            ' MsgBox(pixels.Length & " " & GRPFrame(frame).Image.Length)


            '138이 남는다.
            pixels = GRPFrame(frame).Image


            Marshal.Copy(pixels, 0, scan0, pixels.Length)

            bm.UnlockBits(bmd)


            Dim CPalette As Imaging.ColorPalette
            CPalette = bm.Palette
            For i = 0 To 255
                If 15 >= i And i >= 8 And Unitcolor <> 0 And isremapping = False Then
                    CPalette.Entries(i) = Palett(unitColors(i - 8))
                Else
                    CPalette.Entries(i) = Palett(i)
                End If

                'DrawFunction As Byte, RemappingNum As Byte
                'DrawFunction = 9
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




                'bexel = 64
                'bfire = 41
                'gfire = 33
                'ofire = 64


                'bexpl = 9
                'bfire = 10
                'gfire = 11
                'ofire = 12
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



    'Public Sub DrawToPictureBox(ByRef pictureBox As PictureBox, frame As Integer, Optional Unitcolor As Integer = 0, Optional istran As Boolean = False, Optional isflip As Boolean = False, Optional x As Integer = 0, Optional y As Integer = 0)
    '    frame = frame Mod framecount

    '    Dim bitmap As New Bitmap(grpWidth, grpHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
    '    Dim grp As Graphics
    '    grp = Graphics.FromImage(bitmap)

    '    Dim tempbmp As Bitmap = DrawGRP(frame, Unitcolor, istran)

    '    Dim temppoint As Point


    '    temppoint = New Point(GRPFrame(frame).frameXOffset + x, GRPFrame(frame).frameYOffset + y)

    '    Select Case DrawFunction
    '        Case 2
    '            grp.DrawImage(tempbmp, temppoint)
    '            grp.FillRectangle(New SolidBrush(Color.FromArgb(&HDD, 0, 0, 0)), New RectangleF(0, 0, grpWidth, grpHeight))

    '        Case 3
    '            grp.DrawImage(tempbmp, temppoint)
    '            grp.FillRectangle(New SolidBrush(Color.FromArgb(&H77, 0, 0, 0)), New RectangleF(0, 0, grpWidth, grpHeight))
    '        Case 4
    '            grp.DrawImage(tempbmp, temppoint)
    '            grp.FillRectangle(New SolidBrush(Color.FromArgb(&H77, 0, 0, 0)), New RectangleF(0, 0, grpWidth, grpHeight))
    '        Case 5
    '            grp.DrawImage(tempbmp, temppoint)
    '            grp.FillRectangle(New SolidBrush(Color.FromArgb(&H77, 0, 0, 0)), New RectangleF(0, 0, grpWidth, grpHeight))
    '        Case 6
    '            grp.DrawImage(tempbmp, temppoint)
    '            grp.FillRectangle(New SolidBrush(Color.FromArgb(&H77, 0, 0, 0)), New RectangleF(0, 0, grpWidth, grpHeight))
    '        Case 15
    '            grp.DrawRectangle(Pens.Lime, GRPFrame(frame).frameXOffset + x, GRPFrame(frame).frameYOffset + y,
    '                         GRPFrame(frame).frameWidth, GRPFrame(frame).frameHeight)


    '        Case Else
    '            grp.DrawImage(tempbmp, temppoint)
    '    End Select



    '    If isflip = True Then
    '        bitmap.RotateFlip(RotateFlipType.Rotate180FlipY)
    '    End If


    '    pictureBox.Image = bitmap
    'End Sub
    'Public Sub DrawToPictureBoxBackG(ByRef pictureBox As PictureBox, frame As Integer, Optional Unitcolor As Integer = 0, Optional point As Integer = 0)
    '    frame = frame Mod framecount

    '    Dim bitmap As New Bitmap(grpWidth, 256, System.Drawing.Imaging.PixelFormat.Format32bppRgb)
    '    Dim grp As Graphics
    '    grp = Graphics.FromImage(bitmap)

    '    Dim tempbmp As Bitmap = DrawGRP(frame, Unitcolor)


    '    'grpHeight - GRPFrame(frame).frameYOffset +

    '    Dim temppoint As New Point(GRPFrame(frame).frameXOffset, GRPFrame(frame).frameYOffset + (256 - grpHeight) / 2 + point) '(grpWidth - tempbmp.Width) \ 2 + 
    '    grp.DrawImage(tempbmp, temppoint)
    '    'grp.DrawRectangle(Pens.Red, New Rectangle(GRPFrame(frame).frameXOffset, GRPFrame(frame).frameYOffset + (256 - grpHeight) / 2 + point, grpWidth, grpHeight))

    '    pictureBox.BackgroundImage = bitmap
    'End Sub
    'Public Function GetFrameSize(frame As Integer) As Size
    '    frame = frame Mod framecount

    '    Return New Size(GRPFrame(frame).frameWidth, GRPFrame(frame).frameHeight)
    'End Function


    'Public Sub DrawToPictureBoxUnitGRP(ByRef pictureBox As PictureBox, frame As Integer, left As Integer, right As Integer, up As Integer, down As Integer, addX As Integer, addY As Integer, conx As Integer, cony As Integer, Optional Unitcolor As Integer = 0)
    '    frame = frame Mod framecount


    '    Dim bitmap As New Bitmap(256, 256, Imaging.PixelFormat.Format32bppRgb)
    '    Dim grp As Graphics
    '    grp = Graphics.FromImage(bitmap)

    '    Dim tempbmp As Bitmap = DrawGRP(frame, Unitcolor, True)

    '    If addX <> 0 Or addY <> 0 Then
    '        grp.DrawRectangle(New Pen(Color.Red, 1), New Rectangle(New Point(bitmap.Width \ 2 - 32 - addX,
    '                                                       bitmap.Height \ 2 - 32 - addY), New Size(128, 96)))
    '    End If




    '    '+ 128 - bitmap.Height \ 2
    '    Dim temppoint As New Point(GRPFrame(frame).frameXOffset + 128 - grpWidth \ 2,
    '                                       GRPFrame(frame).frameYOffset + 128 - grpHeight \ 2) '(grpWidth - tempbmp.Width) \ 2 + 
    '    grp.DrawImage(tempbmp, temppoint)

    '    Dim point As New Point(bitmap.Width \ 2 - left, bitmap.Height \ 2 - up)
    '    Dim size As New Size(right + left, down + up)
    '    grp.DrawRectangle(New Pen(Color.PaleGreen, 1), New Rectangle(point, size))
    '    grp.FillRectangle(New SolidBrush(Color.FromArgb(&H55ABF200)), New Rectangle(New Point(bitmap.Width \ 2 - conx \ 2,
    '                                             bitmap.Height \ 2 - cony \ 2), New Size(conx, cony)))

    '    pictureBox.Image = bitmap

    'End Sub

    'Public Function DrawGRP(frame As Integer, Optional Unitcolor As Integer = 0) As Bitmap
    '    'GRPFrame(frame).frameWidth
    '    'GRPFrame(frame).frameHeight

    '    Dim bm As New Bitmap(GRPFrame(frame).frameWidth, GRPFrame(frame).frameHeight, Imaging.PixelFormat.Format8bppIndexed)



    '    Dim bmd As New BitmapData
    '    bmd = bm.LockBits(New Rectangle(0, 0, bm.Width, bm.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format8bppIndexed)

    '    Dim scan0 As IntPtr = bmd.Scan0
    '    Dim stride As Integer = bmd.Stride

    '    Dim pixels(GRPFrame(frame).Image.Length - 1) As Byte
    '    Marshal.Copy(scan0, pixels, 0, pixels.Length)

    '    ' MsgBox(pixels.Length & " " & GRPFrame(frame).Image.Length)


    '    '138이 남는다.

    '    pixels = GRPFrame(frame).Image


    '    Marshal.Copy(pixels, 0, scan0, pixels.Length)

    '    bm.UnlockBits(bmd)


    '    Dim CPalette As Imaging.ColorPalette
    '    CPalette = bm.Palette
    '    For i = 0 To 255
    '        CPalette.Entries(i) = Palett(i)

    '    Next
    '    bm.Palette = CPalette



    '    Return bm
    'End Function



    Public Function Reset()
        framecount = 0
        grpWidth = 0
        grpHeight = 0
        GRPFrame.Clear()

        Return 0
    End Function

End Class