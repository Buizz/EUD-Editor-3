Imports System.Drawing.Imaging
Imports System.IO

<Serializable>
Public Class SCAScriptImageData
    Public ItemName As String

    Private PixelOrgData As System.Drawing.Bitmap

    Public Sub SaveToFile(filepath As String)
        PixelOrgData.Save(filepath & ItemName & ".png", ImageFormat.Png)
    End Sub

    Public LoadSuccess As Boolean = False

    Public imgWidth As Integer
    Public imgHeight As Integer

    Public Sub New(filename As String)
        LoadImage(filename)
    End Sub

    Public Sub LoadImage(filename As String)
        PixelOrgData = New System.Drawing.Bitmap(filename)

        LoadSuccess = True

        imgWidth = PixelOrgData.Width
        imgHeight = PixelOrgData.Height

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

    '21색
    '데이터
End Class
