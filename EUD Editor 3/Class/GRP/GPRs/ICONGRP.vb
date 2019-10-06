<Serializable()>
Public Class ICONGRP
    Inherits RGRP

    Private mainbitmap As ByteBitmap

    <NonSerialized()>
    Private mainBitmapSource As BitmapSource


    Public Overrides Sub Reset()

    End Sub

    Public Overrides Function LoadGRP(bitmap As ByteBitmap, framedata As List(Of FrameData), grpfile As String, GRPSize As Size) As Boolean
        mainbitmap = bitmap

        Return True
    End Function

    Public Overrides Function DrawGRP(frame As Integer) As BitmapSource
        If mainBitmapSource Is Nothing Then

            mainBitmapSource = mainbitmap.GetOverlay(Color.FromRgb(240, 240, 0))
            'mainBitmapSource = mainbitmap.GetImage(New Int32Rect(0, 0, mainbitmap.Width, mainbitmap.Height))
        End If
        Return mainBitmapSource
    End Function

    Public Overrides Function DrawImage(SCImage As SCImage) As Image
        Throw New NotImplementedException()
    End Function
End Class
