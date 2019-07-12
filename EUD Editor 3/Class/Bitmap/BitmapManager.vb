Imports System.IO
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

        Select Case Image.Format
            Case ImageFormat.Rgb24
                Dim rpixels(Image.Height * Image.Width * 4) As Byte
                Dim pixels() As Byte = Image.Data

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
                Return New ByteBitmap(rpixels, Image.Width, Image.Height)
            Case ImageFormat.Rgba32
                Return New ByteBitmap(Image.Data, Image.Width, Image.Height)
        End Select


        Return Nothing


        'Return BitmapSource.Create(Image.Width, Image.Height, 96.0, 96.0, format, Nothing, Image.Data, Image.Stride)
    End Function
End Class

Public Class ByteBitmap
    Public Property Bytes As Byte()
    Private Width As Integer
    Private Height As Integer



    Public Sub New(tbytes() As Byte, tWidth As Integer, tHeight As Integer)
        Bytes = tbytes
        Width = tWidth
        Height = tHeight
    End Sub


    Public Sub New(bitmap As BitmapSource)
        Dim stride As Integer = bitmap.Width * (bitmap.Format.BitsPerPixel) / 8
        Dim rstride As Integer = bitmap.Width * (32) / 8
        Dim pixels(bitmap.Height * stride) As Byte
        Dim rpixels(bitmap.Height * rstride) As Byte
        bitmap.CopyPixels(pixels, stride, 0)

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

        Width = bitmap.Width
        Height = bitmap.Height

        If bitmap.Format = PixelFormats.Bgr24 Then
            Bytes = rpixels
        Else
            Bytes = pixels
        End If
    End Sub

    Public Function GetImage(rect As Int32Rect) As BitmapSource
        Dim rbitmap As New WriteableBitmap(rect.Width, rect.Height, 96, 96, PixelFormats.Bgra32, Nothing)
        Dim stride As Integer = rect.Width * (32) / 8
        Dim rstride As Integer = Width * (32) / 8


        Dim pixels(rect.Height * stride) As Byte
        For y = 0 To rect.Height - 1
            For x = 0 To rect.Width - 1
                Dim rx As Integer = (rect.X + x) * 4
                Dim ry As Integer = (rect.Y + y) * rstride


                pixels(x * 4 + y * stride) = Bytes(rx + ry)
                pixels(x * 4 + y * stride + 1) = Bytes(rx + ry + 1)
                pixels(x * 4 + y * stride + 2) = Bytes(rx + ry + 2)
                pixels(x * 4 + y * stride + 3) = Bytes(rx + ry + 3)
            Next
        Next


        rbitmap.WritePixels(New Int32Rect(0, 0, rect.Width, rect.Height), pixels, stride, 0)

        rbitmap.Freeze()
        Return rbitmap
    End Function
End Class