Public Class SDGRP
    Inherits RGRP

    Private mainbitmap As ByteBitmap
    Private framedata As List(Of FrameData)


    Private framebitmap() As BitmapSource
    Private maxframe As Integer = 1

    Public Property grpfile As String

    Public Overrides Sub Reset()

    End Sub

    Public Overrides Function LoadGRP(bitmap As ByteBitmap, tframedata As List(Of FrameData), tgrpfile As String) As Boolean
        mainbitmap = bitmap

        framedata = tframedata
        grpfile = tgrpfile

        ReDim framebitmap(framedata.Count - 1)

        maxframe = framedata.Count

        Return True
    End Function

    Public Overrides Function DrawGRP(frame As Integer) As BitmapSource
        frame = frame Mod maxframe

        If framebitmap(frame) Is Nothing Then
            framebitmap(frame) = mainbitmap.GetImage(New Int32Rect(framedata(frame).x, framedata(frame).y, framedata(frame).width, framedata(frame).height))
        End If
        Return framebitmap(frame)
    End Function
End Class
