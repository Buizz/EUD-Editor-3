Public Class HDGRP
    Inherits RGRP

    Private mainbitmap As ByteBitmap
    Private framedata As List(Of FrameData)


    Private framebitmap() As BitmapSource
    Private maxframe As Integer = 1

    Public Property grpfile As String
    Private imageid As Integer

    Public Overrides Sub Reset()

    End Sub
    Public Sub New(timageid As Integer)
        imageid = timageid
    End Sub

    Public Overrides Function LoadGRP(bitmap As ByteBitmap, tframedata As List(Of FrameData), tgrpfile As String, GRPSize As Size) As Boolean
        GRPWidth = GRPSize.Width
        GRPHeight = GRPSize.Height

        mainbitmap = bitmap

        framedata = tframedata
        grpfile = tgrpfile

        ReDim framebitmap(framedata.Count - 1)

        maxframe = framedata.Count

        Return True
    End Function

    Public Overrides Function DrawGRP(frame As Integer) As BitmapSource
        frame = frame Mod maxframe


        Dim fx As Integer
        Dim fy As Integer
        Dim fw As Integer
        Dim fh As Integer


        fx = framedata(frame).x / 2
        fy = framedata(frame).y / 2
        fw = framedata(frame).width / 2
        fh = framedata(frame).height / 2


        If framebitmap(frame) Is Nothing Then
            framebitmap(frame) = mainbitmap.GetImage(New Int32Rect(fx, fy, fw, fh))
        End If
        Return framebitmap(frame)
    End Function

    Public Overrides Function DrawImage(SCImage As SCImage) As Image
        Dim remapping As RGRP.PalettType = scData.DefaultDat.Data(SCDatFiles.DatFiles.images, "Draw Function", imageid)


        Dim timage As New Image()
        timage.Stretch = Stretch.Fill

        timage.HorizontalAlignment = HorizontalAlignment.Left
        timage.VerticalAlignment = VerticalAlignment.Top

        Dim drawFrame As Integer = SCImage.GetFrameGRP()

        Dim bitsource As BitmapSource = DrawGRP(drawFrame)

        timage.Width = bitsource.Width / 2
        timage.Height = bitsource.Height / 2



        Dim posx As Integer = SCImage.Location.X
        Dim posy As Integer = SCImage.Location.Y

        posx += framedata(drawFrame).xoff / 4
        posy += framedata(drawFrame).yoff / 4
        posx -= GRPWidth / 8
        posy -= GRPHeight / 8



        If SCImage.direction > 16 And SCImage.IsTrunable And SCImage.ControlStatus = 0 Then
            Dim myScaleTransform As New ScaleTransform()
            myScaleTransform.ScaleY = 1
            myScaleTransform.ScaleX = -1
            Dim myTransformGroup As New TransformGroup()
            myTransformGroup.Children.Add(myScaleTransform)

            timage.RenderTransform = myTransformGroup

            '             <Image.RenderTransform>
            '    <TransformGroup>
            '        <ScaleTransform ScaleY = "1" ScaleX="-1"/>
            '        <SkewTransform AngleY = "0" AngleX="0"/>
            '        <RotateTransform Angle = "0" />
            '        <TranslateTransform/>
            '                    </TransformGroup>
            '</Image.RenderTransform>
            posx += bitsource.Width / 2
        End If

        timage.Margin = New Thickness(posx, posy, 0, 0)

        Select Case remapping
            Case PalettType.shadow
                timage.Opacity = 0.5
                timage.Source = bitsource
            Case Else
                timage.Source = bitsource
        End Select


        Return timage
    End Function
End Class
