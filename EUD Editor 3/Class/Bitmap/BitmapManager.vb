Imports System.IO
Imports System.IO.Compression
Imports Pfim

Public Class BitmapManager
    Public Shared Function Cutoff(bitmap As BitmapSource, rect As Int32Rect) As BitmapSource

        Dim stride As Integer = rect.Width * (bitmap.Format.BitsPerPixel) / 8
        Dim rstride As Integer = rect.Width * (32) / 8
        Dim pixels(rect.Height * stride) As Byte
        Dim rpixels(rect.Height * rstride) As Byte
        bitmap.CopyPixels(rect, pixels, stride, 0)

        If bitmap.Format = PixelFormats.Bgr24 Then
            For i = 0 To pixels.Count / 3 - 1
                rpixels(i * 4) = pixels(i * 3)
                rpixels(i * 4 + 1) = pixels(i * 3 + 1)
                rpixels(i * 4 + 2) = pixels(i * 3 + 2)
                If pixels(i * 3) = 0 And pixels(i * 3 + 1) = 0 And pixels(i * 3 + 2) = 0 Then
                    rpixels(i * 4 + 3) = 0
                Else
                    rpixels(i * 4 + 3) = 255
                End If
            Next
        End If


        Dim rbitmap As New WriteableBitmap(rect.Width, rect.Height, 96, 96, PixelFormats.Bgra32, Nothing)

        If bitmap.Format = PixelFormats.Bgr24 Then
            rbitmap.WritePixels(rect, rpixels, rstride, 0)
        Else
            rbitmap.WritePixels(rect, pixels, stride, 0)
        End If

        rbitmap.Freeze()
        Return rbitmap
    End Function

    Public Shared Function LoadImage(ms As MemoryStream) As ByteBitmap


        Dim Image As IImage = Pfim.Pfim.FromStream(ms)
        Try
            Select Case Image.Format
                Case ImageFormat.Rgb24
                    Dim stride As Integer = Image.Stride
                    Dim rstride As Integer = Image.Width * 4


                    Dim pixels() As Byte = Image.Data
                    Dim rpixels(Image.Width * Image.Height * 4) As Byte


                    For y = 0 To Image.Height - 1
                        For x = 0 To Image.Width - 1
                            rpixels(y * rstride + x * 4) = pixels(y * stride + x * 3)
                            rpixels(y * rstride + x * 4 + 1) = pixels(y * stride + x * 3 + 1)
                            rpixels(y * rstride + x * 4 + 2) = pixels(y * stride + x * 3 + 2)

                            If pixels(y * stride + x * 3) = 0 And pixels(y * stride + x * 3 + 1) = 0 And pixels(y * stride + x * 3 + 2) = 0 Then
                                rpixels(y * rstride + x * 4 + 3) = 0
                            Else
                                rpixels(y * rstride + x * 4 + 3) = 255
                            End If

                        Next
                    Next

                    'Dim pixels() As Byte = Image.Data
                    'Dim rpixels(pixels.Count * 4 / 3) As Byte

                    'For i = 0 To pixels.Count / 3 - 1
                    '    rpixels(i * 4) = pixels(i * 3)
                    '    rpixels(i * 4 + 1) = pixels(i * 3 + 1)
                    '    rpixels(i * 4 + 2) = pixels(i * 3 + 2)
                    '    If pixels(i * 3) = 0 And pixels(i * 3 + 1) = 0 And pixels(i * 3 + 2) = 0 Then
                    '        rpixels(i * 4 + 3) = 0
                    '    Else
                    '        rpixels(i * 4 + 3) = 255
                    '    End If
                    'Next
                    Return New ByteBitmap(rpixels, Image.Width, Image.Height)
                Case ImageFormat.Rgba32
                    Return New ByteBitmap(Image.Data, Image.Stride / 4, Image.Height)
                    'If Image.Stride = Image.Width * 4 Then
                    '    Return New ByteBitmap(Image.Data, Image.Width, Image.Height)
                    'Else
                    '    'MsgBox(Image.Width & " " & Image.Stride)

                    '    Return New ByteBitmap(Image.Data, Image.Stride / 4, Image.Height)
                    'End If
            End Select
        Catch ex As Exception
            MsgBox(Image.Data.Count & vbCrLf & Image.Width & vbCrLf & Image.Height)
        End Try



        Return Nothing


        'Return BitmapSource.Create(Image.Width, Image.Height, 96.0, 96.0, format, Nothing, Image.Data, Image.Stride)
    End Function
End Class

Public Class ByteBitmap
    Private compressByte() As Byte
    Public Property Bytes As Byte()
        Get
            Return Decompress(compressByte)
        End Get
        Set(value As Byte())
            compressByte = Compress(value)
        End Set
    End Property
    Public Width As Integer
    Public Height As Integer

    '    // out data some byte array byte[] buffer = Encoding.UTF8.GetBytes ("large text"); // compress byte[] compressed = Lz4Net.Lz4.CompressBytes (buffer, 0, buffer.Length, Lz4Net.Lz4Mode.Fast);
    '```
    'Simple byte.md decompression
    '```
    '// decompress byte[] buffer = Lz4Net.Lz4.DecompressBytes (compressed);
    Private Function CompressBuffer(ByVal byteArray As Byte()) As Byte()
        Return Lz4Net.Lz4.CompressBytes(byteArray)
    End Function

    Private Function DeCompressBuffer(ByVal byteArray As Byte()) As Byte()
        Return Lz4Net.Lz4.DecompressBytes(byteArray)
    End Function

    Public Shared Function Compress(ByVal data As Byte()) As Byte()
        Dim output As MemoryStream = New MemoryStream()

        Using dstream As DeflateStream = New DeflateStream(output, CompressionLevel.Fastest)
            dstream.Write(data, 0, data.Length)
        End Using

        Return output.ToArray()
    End Function

    Public Shared Function Decompress(ByVal data As Byte()) As Byte()
        Dim input As MemoryStream = New MemoryStream(data)
        Dim output As MemoryStream = New MemoryStream()

        Using dstream As DeflateStream = New DeflateStream(input, CompressionMode.Decompress)
            dstream.CopyTo(output)
        End Using

        Return output.ToArray()
    End Function


    Public Sub New(tbytes() As Byte, tWidth As Integer, tHeight As Integer)
        'Dim tcbb() As Byte = {&H6C, &H6F, &H63, &H61, &H6C, &H65, &H73, &H2F, &H65, &H6E, &H55}
        'Dim ttbb() As Byte = Compress(tcbb)
        'Dim ttbbb() As Byte = Decompress(ttbb)

        'MsgBox(bytestr(tcbb) & vbCrLf & bytestr(ttbb) & vbCrLf & bytestr(ttbbb))

        Bytes = tbytes
        Width = tWidth
        Height = tHeight
    End Sub
    Private Function bytestr(bytes() As Byte) As String
        Dim tstr As String = ""

        For i = 0 To bytes.Count - 1
            tstr = tstr & Hex(bytes(i)) & " "
        Next
        Return tstr
    End Function




    'Public Sub New(bitmap As BitmapSource)
    '    Dim stride As Integer = bitmap.Width * (bitmap.Format.BitsPerPixel) / 8
    '    Dim rstride As Integer = bitmap.Width * (32) / 8
    '    Dim pixels(bitmap.Height * stride) As Byte
    '    Dim rpixels(bitmap.Height * rstride) As Byte
    '    bitmap.CopyPixels(pixels, stride, 0)

    '    If bitmap.Format = PixelFormats.Bgr24 Then
    '        For i = 0 To pixels.Count / 3 - 1
    '            rpixels(i * 4) = pixels(i * 3)
    '            rpixels(i * 4 + 1) = pixels(i * 3 + 1)
    '            rpixels(i * 4 + 2) = pixels(i * 3 + 2)
    '            If pixels(i * 3) = 0 And pixels(i * 3 + 1) = 0 And pixels(i * 3 + 2) = 0 Then
    '                rpixels(i * 4 + 3) = 0
    '            Else
    '                rpixels(i * 4 + 3) = 255
    '            End If
    '        Next
    '    End If

    '    Width = bitmap.Width
    '    Height = bitmap.Height

    '    If bitmap.Format = PixelFormats.Bgr24 Then
    '        Bytes = rpixels
    '    Else
    '        Bytes = pixels
    '    End If
    'End Sub

    Public Function GetOverlay(color As Color) As BitmapSource
        Dim rbitmap As New WriteableBitmap(Width, Height, 96, 96, PixelFormats.Bgra32, Nothing)

        Dim abytes() As Byte = Bytes
        Dim stride As Integer = Width * 4
        Dim br As Single = color.R / 256
        Dim bg As Single = color.G / 256
        Dim bb As Single = color.B / 256


        Dim pixels(Height * stride) As Byte

        For y = 0 To Height - 1
            For x = 0 To Width - 1
                Dim rx As Integer = x * 4
                Dim ry As Integer = y * stride

                Dim ar As Single = abytes(rx + ry + 2) / 256
                Dim ag As Single = abytes(rx + ry + 1) / 256
                Dim ab As Single = abytes(rx + ry) / 256


                Dim rr As Single
                Dim rg As Single
                Dim rb As Single


                If ar < 0.5 Then
                    rr = (ar * br * 2) * 256
                Else
                    rr = (1 - 2 * (1 - ar) * (1 - br)) * 256
                End If

                If ag < 0.5 Then
                    rg = (ag * bg * 2) * 256
                Else
                    rg = (1 - 2 * (1 - ag) * (1 - bg)) * 256
                End If
                If ab < 0.5 Then
                    rb = (ab * bb * 2) * 256
                Else
                    rb = (1 - 2 * (1 - ab) * (1 - bb)) * 256
                End If

                pixels(rx + ry + 2) = Math.Min(rr, 255)
                pixels(rx + ry + 1) = Math.Min(rg, 255)
                pixels(rx + ry) = Math.Min(rb, 255)

                pixels(rx + ry + 3) = abytes(rx + ry + 3)
            Next
        Next
        '2ab
        '1 - 2(1-a)(1-b)
        rbitmap.WritePixels(New Int32Rect(0, 0, Width, Height), pixels, stride, 0)

        rbitmap.Freeze()
        Return rbitmap
    End Function

    Public Function GetImage(rect As Int32Rect) As BitmapSource
        Dim rbitmap As New WriteableBitmap(rect.Width, rect.Height, 96, 96, PixelFormats.Bgra32, Nothing)
        Dim stride As Integer = rect.Width * 4
        Dim rstride As Integer = Width * 4

        Dim orgbytes() As Byte = Bytes

        Dim pixels(rect.Height * stride) As Byte
        For y = 0 To rect.Height - 1
            For x = 0 To rect.Width - 1
                Dim rx As Integer = (rect.X + x) * 4
                Dim ry As Integer = (rect.Y + y) * rstride


                pixels(x * 4 + y * stride) = orgbytes(rx + ry)
                pixels(x * 4 + y * stride + 1) = orgbytes(rx + ry + 1)
                pixels(x * 4 + y * stride + 2) = orgbytes(rx + ry + 2)
                pixels(x * 4 + y * stride + 3) = orgbytes(rx + ry + 3)
            Next
        Next
        rbitmap.WritePixels(New Int32Rect(0, 0, rect.Width, rect.Height), pixels, stride, 0)

        rbitmap.Freeze()
        Return rbitmap
    End Function


    Public Function GetWireFrame() As BitmapSource
        Dim rWidth As Integer = Height
        Dim rHeight As Integer = Height

        Dim rbitmap As New WriteableBitmap(rWidth, rHeight, 96, 96, PixelFormats.Bgra32, Nothing)
        Dim stride As Integer = rWidth * (32) / 8
        Dim rstride As Integer = Width * (32) / 8

        Dim orgbytes() As Byte = Bytes

        Dim pixels(rHeight * stride) As Byte

        Dim Count As Integer = Width / Height


        If Count = 6 Then
            Dim startX As Integer = 0
            Dim r As Byte
            Dim g As Byte
            Dim b As Byte
            For i = 0 To 3
                Select Case i
                    Case 0
                        r = 25
                        g = 252
                        b = 16
                    Case 1
                        r = 56
                        g = 252
                        b = 252
                    Case 2
                        r = 20
                        g = 140
                        b = 248
                    Case 3
                        r = 24
                        g = 24
                        b = 200
                End Select

                For y = 0 To rHeight - 1
                    For x = 0 To rWidth - 1
                        Dim rx As Integer = (startX + x) * 4
                        Dim ry As Integer = (y) * rstride

                        If orgbytes(rx + ry + 3) <> 0 And orgbytes(rx + ry) > 200 And orgbytes(rx + ry + 1) > 200 And orgbytes(rx + ry + 2) > 200 Then
                            pixels(x * 4 + y * stride) = r 'orgbytes(rx + ry)
                            pixels(x * 4 + y * stride + 1) = g 'orgbytes(rx + ry + 1)
                            pixels(x * 4 + y * stride + 2) = b 'orgbytes(rx + ry + 2)
                            pixels(x * 4 + y * stride + 3) = orgbytes(rx + ry + 3)
                        End If

                    Next
                Next
                startX += rWidth
            Next
        Else
            Dim startX As Integer = rWidth * 6

            For y = 0 To rHeight - 1
                For x = 0 To rWidth - 1
                    Dim rx As Integer = (startX + x) * 4
                    Dim ry As Integer = (y) * rstride


                    Dim br As Single = 25 / 256
                    Dim bg As Single = 252 / 256
                    Dim bb As Single = 16 / 256

                    Dim ar As Single = orgbytes(rx + ry + 2) / 256
                    Dim ag As Single = orgbytes(rx + ry + 1) / 256
                    Dim ab As Single = orgbytes(rx + ry) / 256

                    Dim rr As Single = (br * ar) * 256
                    Dim rg As Single = (bg * ag) * 256
                    Dim rb As Single = (bb * ab) * 256




                    'If ar < 0.5 Then
                    '    rr = (ar * br * 2) * 256
                    'Else
                    '    rr = (1 - 2 * (1 - ar) * (1 - br)) * 256
                    'End If
                    'If ag < 0.5 Then
                    '    rg = (ag * bg * 2) * 256
                    'Else
                    '    rg = (1 - 2 * (1 - ag) * (1 - bg)) * 256
                    'End If
                    'If ab < 0.5 Then
                    '    rb = (ab * bb * 2) * 256
                    'Else
                    '    rb = (1 - 2 * (1 - ab) * (1 - bb)) * 256
                    'End If

                    pixels(x * 4 + y * stride) = Math.Min(rb, 255)
                    pixels(x * 4 + y * stride + 1) = Math.Min(rg, 255)
                    pixels(x * 4 + y * stride + 2) = Math.Min(rr, 255)
                    pixels(x * 4 + y * stride + 3) = orgbytes(rx + ry + 3)
                Next
            Next
        End If


        rbitmap.WritePixels(New Int32Rect(0, 0, rWidth, rHeight), pixels, stride, 0)

        rbitmap.Freeze()
        Return rbitmap
    End Function
End Class