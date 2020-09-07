Imports System.IO

<Serializable>
Public Class DotData
    Public ItemName As String

    Private PixelOrgData As System.Drawing.Bitmap
    Public ImagePallet As List(Of System.Drawing.Color)

    Public ColorFlag As Boolean()



    Public LoadSucess As Boolean = False


    Private Pixels(,) As Byte
    Private imgWidth As Integer
    Private imgHeight As Integer


    Public Sub New(filename As String)
        PixelOrgData = New System.Drawing.Bitmap(filename)
        If PixelOrgData.Width * PixelOrgData.Height >= 1000 Then
            Return
        End If
        LoadSucess = True


        imgWidth = PixelOrgData.Width
        imgHeight = PixelOrgData.Height

        ReDim Pixels(imgWidth, imgHeight)



        ImagePallet = New List(Of System.Drawing.Color)
        For y = 0 To PixelOrgData.Height - 1
            For x = 0 To PixelOrgData.Width - 1
                Dim pixelColor As System.Drawing.Color = PixelOrgData.GetPixel(x, y)
                If ImagePallet.IndexOf(pixelColor) = -1 Then
                    ImagePallet.Add(pixelColor)
                End If
            Next
        Next



        ItemName = filename.Split("\").Last.Split(".").First
    End Sub


    Public Function GetBitMap() As BitmapSource
        Dim MS As MemoryStream = New MemoryStream()
        PixelOrgData.Save(MS, System.Drawing.Imaging.ImageFormat.Png)
        MS.Position = 0
        Dim bi As New BitmapImage
        bi.BeginInit()
        bi.StreamSource = MS
        bi.EndInit()

        Return bi
    End Function

    Public Function GetepsData() As String
        ' 첫번쨰 인자가 0일경우 white칠하고 1일경우 뒷 배경이 white인것
        '"EUDArray(list(0,6,0,0,0,10,0,9))"
        Dim listdata As String = ""

        Dim itemcount As Integer = 0
        For y = 0 To imgHeight - 1
            For x = 0 To imgWidth - 1
                Dim pixelColor As System.Drawing.Color = PixelOrgData.GetPixel(x, y)

                'If pixelColor.R = 255 And pixelColor.G = 255 And pixelColor.B = 255 Then
                '    Continue For
                'End If


                Dim indexcolor As Integer = Tool.DotPaint.GetColorIndex(pixelColor)




                itemcount += 3
                'listdata = listdata & "," & x * 3 & "," & y * 3 & "," & indexcolor
                listdata = listdata & "," & indexcolor
            Next
        Next

        listdata = "1," & imgWidth & "," & imgHeight & listdata



        Return "EUDArray(list(" & listdata & "))"
    End Function

    '21색
    '데이터
End Class
