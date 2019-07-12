Public Class HDGRP
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

        Dim fx As Integer = framedata(frame).x / 2
        Dim fy As Integer = framedata(frame).y / 2
        Dim fw As Integer = framedata(frame).width / 2
        Dim fh As Integer = framedata(frame).height / 2



        If framebitmap(frame) Is Nothing Then
            framebitmap(frame) = mainbitmap.GetImage(New Int32Rect(fx, fy, fw, fh))
        End If
        Return framebitmap(frame)
    End Function
End Class
