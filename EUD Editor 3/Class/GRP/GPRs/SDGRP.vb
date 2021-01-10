<Serializable()>
Public Class SDGRP
    Inherits RGRP

    Private oldgrp As GRP

    Private mainbitmap As ByteBitmap
    Private framedata As List(Of FrameData)

    <NonSerialized()>
    Private framebitmap() As BitmapSource
    Private maxframe As Integer = 1

    Public Property grpfile As String
    Private imageid As Integer

    Public Overrides Sub Reset()

    End Sub


    Public Sub New(timageid As Integer)
        imageid = timageid
    End Sub


    Public Sub OldGRPLoad(grp As GRP)
        oldgrp = grp
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
        If framebitmap Is Nothing Then
            If framedata IsNot Nothing Then
                ReDim framebitmap(framedata.Count - 1)
            Else
                ReDim framebitmap(0)
            End If
        End If

        frame = frame Mod maxframe

        If framebitmap(frame) Is Nothing Then
            framebitmap(frame) = mainbitmap.GetImage(New Int32Rect(framedata(frame).x, framedata(frame).y, framedata(frame).width, framedata(frame).height))
        End If
        Return framebitmap(frame)
    End Function

    Public Overrides Function DrawImage(SCImage As SCImage) As Image
        Dim remapping As RGRP.PalettType = scData.DefaultDat.Data(SCDatFiles.DatFiles.images, "Draw Function", imageid)



        Dim timage As New Image()
        timage.Stretch = Stretch.None
        timage.HorizontalAlignment = HorizontalAlignment.Left
        timage.VerticalAlignment = VerticalAlignment.Top




        Dim drawFrame As Integer = SCImage.GetFrameGRP()

        If maxframe < drawFrame Then
            timage.Margin = New Thickness(-48, -48, 0, 0)
            timage.Source = Tool.ErrorBitmap
            Return timage
        End If
        Dim bitsource As BitmapSource = DrawGRP(drawFrame)


        Dim posx As Integer = SCImage.Location.X
        Dim posy As Integer = SCImage.Location.Y


        posx += framedata(drawFrame).xoff - oldgrp.grpWidth / 2 ' - GRPWidth / 2
        posy += framedata(drawFrame).yoff - oldgrp.grpHeight / 2 ' - GRPHeight / 2


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
            posx += bitsource.Width
        End If

        timage.Margin = New Thickness(posx, posy, 0, 0)

        Select Case remapping
            Case PalettType.shadow
                Dim opacity As New ImageBrush(bitsource)
                opacity.AlignmentX = AlignmentX.Left
                opacity.AlignmentY = AlignmentY.Top
                opacity.Stretch = Stretch.None


                timage.OpacityMask = opacity
                timage.Opacity = 0.5


                timage.Source = Tool.BlackOverlaybitmap
            Case Else
                timage.Source = bitsource
        End Select


        Return timage
    End Function
End Class
