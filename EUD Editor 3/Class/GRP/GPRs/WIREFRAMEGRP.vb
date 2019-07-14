Imports EUD_Editor_3

Public Class WIREFRAMEGRP
    Inherits RGRP

    Private mainbitmap As ByteBitmap
    Private mainBitmapSource As BitmapSource


    Public Overrides Sub Reset()

    End Sub

    Public Overrides Function LoadGRP(bitmap As ByteBitmap, framedata As List(Of FrameData), grpfile As String, GRPSize As Size) As Boolean
        mainbitmap = bitmap
        '4개를 겹쳐그려야함.
        Return True
    End Function

    Public Overrides Function DrawGRP(frame As Integer) As BitmapSource
        If mainBitmapSource Is Nothing Then
            mainBitmapSource = mainbitmap.GetImage(New Int32Rect(0, 0, mainbitmap.Width, mainbitmap.Height))
            'mainBitmapSource = mainbitmap.GetOverlay(Color.FromRgb(25, 252, 16))
        End If
        Return mainBitmapSource
    End Function
    Public Function DrawWire(frame As Integer) As BitmapSource
        If mainBitmapSource Is Nothing Then
            mainBitmapSource = mainbitmap.GetWireFrame()
        End If
        Return mainBitmapSource
    End Function

    Public Overrides Function DrawImage(SCImage As SCImage) As Image
        Throw New NotImplementedException()
    End Function
End Class
