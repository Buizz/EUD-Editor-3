Imports System.Runtime.InteropServices

Public Class GRPBox
    Private IsDelete As Boolean = False


    Private GRPImageBox As GRPImageBox
    Private ImageBox As Grid
    Private ImageID As Integer
    Private Flag As GRPImageBox.BoxType
    Private FObjectID As Integer
    Private Timer As System.Windows.Forms.Timer

    'GRP여러개가 한 공간에서 그려짐, 이미지스크립트도 같이 돌아감.,

    'GRP들이 여러개 있음
    '각각의 GRP들은 이미지스크립트를 통해 GRP를 생성 할 수 있음
    'GRP들은 이미지 스크립트에 의해 사라질 수 있음.

    Private SelectionImage As SCImage

    Public Sub ChangeIScriptType(IScrptIndex As Integer)
        '이미지 스크립트 포인터를 교체한다.
    End Sub

    Public Sub New(ImageNum As Integer, tImageBox As Grid, tGRPImageBox As GRPImageBox, AnimHeaderIndex As Integer, Optional tFlag As GRPImageBox.BoxType = GRPImageBox.BoxType.Image, Optional tFObjectID As Integer = 0)
        '자채적으로 데이터를 읽고 쓴다.
        ImageBox = tImageBox
        ImageID = ImageNum
        GRPImageBox = tGRPImageBox
        FObjectID = tFObjectID
        Flag = tFlag

        '처음 시작한다. 
        Images = New List(Of SCImage)



        Dim firstGRPID As Integer = ImageID
        Dim firstIscrptID As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.images, "Iscript ID", ImageID)

        Images.Add(New SCImage(firstGRPID, firstIscrptID, Me, AnimHeaderIndex))
        Select Case Flag
            Case GRPImageBox.BoxType.Unit
                Dim UnitDirection As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Unit Direction", FObjectID)
                If UnitDirection <> 32 Then
                    Images(0).direction = UnitDirection Mod 32
                Else
                    Dim random As New Random

                    Dim selectv As Integer = random.Next(0, 31)

                    Images(0).direction = selectv
                End If
            Case GRPImageBox.BoxType.Sprite
                If FObjectID >= 130 Then
                    Dim SelectImgaeID As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Sel.Circle Image", FObjectID)
                    Dim SelectOffset As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Sel.Circle Offset", FObjectID)

                    Dim SIscrptID As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.images, "Iscript ID", SelectImgaeID)

                    SelectionImage = (New SCImage(SelectImgaeID, SIscrptID, Me, AnimHeaderIndex))
                    SelectionImage.Location = New Windows.Point(0, SelectOffset)

                End If

        End Select


        '=========테스트============
        'Images.Add(New SCImage(scData.GetGrp(213), 0, Me))
        '=========테스트============


        Timer = New Forms.Timer
        Timer.Interval = 10
        AddHandler Timer.Tick, AddressOf Exec
        'GRPImageBox.WriteDebugText(firstGRPID)
        Timer.Enabled = True
    End Sub

    Private Sub Exec()
        If pjData Is Nothing Then
            Delete()
            Timer.Enabled = False
            Return
        End If
        If Not pjData.IsLoad Then
            Delete()
            Timer.Enabled = False
            Return
        End If

        If pgData.Setting(ProgramData.TSetting.Graphic) = 0 Then
            Return
        End If

        Dim pwidth As Integer = ImageBox.ActualWidth
        Dim pheight As Integer = ImageBox.ActualHeight

        ImageBox.Children.Clear()
        If Flag = GRPImageBox.BoxType.Sprite Then
            If FObjectID >= 130 Then
                Try
                    Dim timage As Image = SelectionImage.GetGRP.DrawImage(SelectionImage)
                    timage.Margin = New Thickness(timage.Margin.Left + pwidth / 2, timage.Margin.Top + pheight / 2, 0, 0)
                    ImageBox.Children.Add(timage)
                Catch ex As Exception

                End Try


                'SelectionImage.GetGRP.DrawToBytes(SelectionImage, Pixels, New Size(256, 256), 255)
            End If
        End If

        For i = 0 To Images.Count - 1
            Images(i).Exec()


            Try
                Dim timage As Image = Images(i).GetGRP.DrawImage(Images(i))
                timage.Margin = New Thickness(timage.Margin.Left + pwidth / 2, timage.Margin.Top + pheight / 2, 0, 0)
                ImageBox.Children.Add(timage)
            Catch ex As Exception

            End Try



        Next

        Dim ci As Integer = 0
        For i = 0 To Images.Count - 1
            If Images(ci).IsDelete Then
                Images.Remove(Images(ci))
            Else
                ci += 1
            End If
        Next



        Select Case Flag
            Case GRPImageBox.BoxType.Unit
                Dim StarEditBoxWidth As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "StarEdit Placement Box Width", FObjectID)
                Dim StarEditBoxHeight As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "StarEdit Placement Box Height", FObjectID)

                Dim UnitDirection As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Unit Direction", FObjectID)


                Dim AddonHorizontal As Integer
                Dim AddonVertical As Integer

                If FObjectID >= SCMenCount Then
                    AddonHorizontal = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Addon Horizontal (X) Position", FObjectID)
                    AddonVertical = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Addon Vertical (Y) Position", FObjectID)
                End If

                Dim UnitSizeLeft As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Unit Size Left", FObjectID)
                Dim UnitSizeUp As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Unit Size Up", FObjectID)
                Dim UnitSizeRight As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Unit Size Right", FObjectID)
                Dim UnitSizeDown As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Unit Size Down", FObjectID)


                '건물 생산 크기
                Dim stareditborder As New Border
                stareditborder.HorizontalAlignment = HorizontalAlignment.Left
                stareditborder.VerticalAlignment = VerticalAlignment.Top
                stareditborder.Margin = New Thickness(pwidth / 2 - StarEditBoxWidth \ 2, pheight / 2 - StarEditBoxHeight \ 2, 0, 0)
                stareditborder.Width = StarEditBoxWidth
                stareditborder.Height = StarEditBoxHeight
                stareditborder.Background = New SolidColorBrush(Color.FromArgb(60, 183, 240, 177))
                ImageBox.Children.Add(stareditborder)

                'GRP.FillRectangle(New SolidBrush(Color.FromArgb(100, 183, 240, 177)), New Rectangle(128 - StarEditBoxWidth \ 2, 128 - StarEditBoxHeight \ 2, StarEditBoxWidth, StarEditBoxHeight))
                'grp.DrawRectangle(New Pen(Color.FromArgb(255, 183, 240, 177)), New Rectangle(128 - StarEditBoxWidth \ 2, 128 - StarEditBoxHeight \ 2, StarEditBoxWidth, StarEditBoxHeight))

                '유닛 크기 끝선
                Dim unitsizeborder As New Border
                unitsizeborder.HorizontalAlignment = HorizontalAlignment.Left
                unitsizeborder.VerticalAlignment = VerticalAlignment.Top
                unitsizeborder.Margin = New Thickness(pwidth / 2 - UnitSizeLeft, pheight / 2 - UnitSizeUp, 0, 0)
                unitsizeborder.Width = UnitSizeRight + UnitSizeLeft
                unitsizeborder.Height = UnitSizeDown + UnitSizeUp
                unitsizeborder.BorderBrush = New SolidColorBrush(Color.FromArgb(200, 255, 0, 0))
                unitsizeborder.BorderThickness = New Thickness(1)
                ImageBox.Children.Add(unitsizeborder)


                '에드온
                If FObjectID >= SCMenCount And (AddonHorizontal <> 0 Or AddonVertical <> 0) Then
                    Dim addonborder As New Border
                    addonborder.HorizontalAlignment = HorizontalAlignment.Left
                    addonborder.VerticalAlignment = VerticalAlignment.Top
                    addonborder.Margin = New Thickness(pwidth / 2 - 32 - AddonHorizontal, pheight / 2 - 32 - AddonVertical, 0, 0)
                    addonborder.Width = 128
                    addonborder.Height = 96
                    addonborder.Background = New SolidColorBrush(Color.FromArgb(100, 180, 180, 180))
                    ImageBox.Children.Add(addonborder)

                    'GRP.FillRectangle(New SolidBrush(Color.FromArgb(100, 150, 150, 150)), New Rectangle(128 - 32 - AddonHorizontal, 128 - 32 - AddonVertical, 128, 96))
                End If

                'If UnitDirection <> 32 Then
                '    Dim diregee As Double = (UnitDirection - 8) / 16 * Math.PI

                '    grp.DrawLine(New Pen(Color.FromArgb(255, 29, 219, 22)), 128, 128, CInt(Math.Cos(diregee) * 50 + 128), CInt(Math.Sin(diregee) * 50 + 128))
                'End If
            Case GRPImageBox.BoxType.Sprite
                If FObjectID >= 130 Then
                    Dim SelectOffset As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Sel.Circle Offset", FObjectID)
                    Dim HPBar As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Health Bar", FObjectID) + 3


                    Dim HpYOffset As Integer = pheight / 2
                    Try
                        HpYOffset = pheight / 2 + (SelectOffset + SelectionImage.GetGRP.DrawGRP(0).Height / 2 + 7)
                    Catch ex As Exception

                    End Try

                    Dim Helthborder As New Border
                    Helthborder.HorizontalAlignment = HorizontalAlignment.Left
                    Helthborder.VerticalAlignment = VerticalAlignment.Top
                    Helthborder.Margin = New Thickness(pwidth / 2 - (HPBar) \ 2, HpYOffset, 0, 0)
                    Helthborder.Width = HPBar
                    Helthborder.Height = 5
                    Helthborder.Background = New SolidColorBrush(Color.FromArgb(255, 140, 255, 140))
                    ImageBox.Children.Add(Helthborder)
                    'GRP.FillRectangle(New SolidBrush(Color.FromArgb(255, 150, 150, 150)), New Rectangle(128 - (HPBar) \ 2, HpYOffset, HPBar, 5))


                End If
            Case GRPImageBox.BoxType.Weapon
                Dim Inner As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.weapons, "Inner Splash Range", FObjectID)
                Dim Medium As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.weapons, "Medium Splash Range", FObjectID)
                Dim Outer As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.weapons, "Outer Splash Range", FObjectID)

                Dim AttackAngle As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.weapons, "Attack Angle", FObjectID)
                AttackAngle = Math.Min(AttackAngle, 128)

                Dim LaunchSpin As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.weapons, "Launch Spin", FObjectID)

                Dim ForwardOffset As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.weapons, "Forward Offset", FObjectID)
                Dim UpwardOffset As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.weapons, "Upward Offset", FObjectID)

                If True Then
                    Dim Splashborder As New Ellipse
                    Splashborder.HorizontalAlignment = HorizontalAlignment.Left
                    Splashborder.VerticalAlignment = VerticalAlignment.Top
                    Splashborder.Margin = New Thickness(pwidth / 2 - (Outer) \ 2, pheight / 2 - (Outer) \ 2, 0, 0)
                    Splashborder.Width = Outer
                    Splashborder.Height = Outer
                    Splashborder.Fill = New SolidColorBrush(Color.FromArgb(100, 50, 50, 255))
                    Splashborder.Stroke = New SolidColorBrush(Color.FromArgb(255, 50, 50, 255))
                    'Splashborder.Background = New SolidColorBrush(Color.FromArgb(150, 255, 50, 50))
                    'Splashborder.BorderBrush = New SolidColorBrush(Color.FromArgb(200, 0, 255, 0))
                    'Splashborder.BorderThickness = New Thickness(1)
                    ImageBox.Children.Add(Splashborder)
                End If
                If True Then
                    Dim Splashborder As New Ellipse
                    Splashborder.HorizontalAlignment = HorizontalAlignment.Left
                    Splashborder.VerticalAlignment = VerticalAlignment.Top
                    Splashborder.Margin = New Thickness(pwidth / 2 - (Medium) \ 2, pheight / 2 - (Medium) \ 2, 0, 0)
                    Splashborder.Width = Medium
                    Splashborder.Height = Medium
                    Splashborder.Fill = New SolidColorBrush(Color.FromArgb(100, 100, 100, 255))
                    Splashborder.Stroke = New SolidColorBrush(Color.FromArgb(255, 100, 100, 255))
                    'Splashborder.Background = New SolidColorBrush(Color.FromArgb(150, 255, 100, 100))
                    'Splashborder.BorderBrush = New SolidColorBrush(Color.FromArgb(200, 0, 255, 0))
                    'Splashborder.BorderThickness = New Thickness(1)
                    ImageBox.Children.Add(Splashborder)
                End If
                If True Then
                    Dim Splashborder As New Ellipse
                    Splashborder.HorizontalAlignment = HorizontalAlignment.Left
                    Splashborder.VerticalAlignment = VerticalAlignment.Top
                    Splashborder.Margin = New Thickness(pwidth / 2 - (Inner) \ 2, pheight / 2 - (Inner) \ 2, 0, 0)
                    Splashborder.Width = Inner
                    Splashborder.Height = Inner
                    Splashborder.Fill = New SolidColorBrush(Color.FromArgb(100, 150, 150, 255))
                    Splashborder.Stroke = New SolidColorBrush(Color.FromArgb(255, 150, 150, 255))
                    'Splashborder.Background = New SolidColorBrush(Color.FromArgb(150, 255, 150, 150))
                    'Splashborder.BorderBrush = New SolidColorBrush(Color.FromArgb(200, 0, 255, 0))
                    'Splashborder.BorderThickness = New Thickness(1)
                    ImageBox.Children.Add(Splashborder)
                End If
                If True Then
                    Dim path As New Path
                    path.Stroke = Brushes.LightPink
                    path.StrokeThickness = 6
                    path.HorizontalAlignment = HorizontalAlignment.Left
                    path.VerticalAlignment = VerticalAlignment.Top
                    path.Margin = New Thickness(pwidth / 2, pheight / 2, 0, 0)



                    Dim pathGeometry As New PathGeometry
                    Dim PathFigureCollection As New PathFigureCollection
                    Dim PathSegmentCollection As New PathSegmentCollection
                    Dim ArcSegment As New ArcSegment
                    ArcSegment.Size = New Size(50, 50)
                    ArcSegment.SweepDirection = SweepDirection.Clockwise
                    ArcSegment.Point = New Point(50, 0)


                    Dim PathFigure As New PathFigure

                    Dim x As Double = Math.Cos(AttackAngle / 128 * Math.PI) * 50
                    Dim y As Double = Math.Sin(AttackAngle / 128 * Math.PI) * 50

                    PathFigure.StartPoint = New Point(x, -y)
                    PathFigure.Segments = PathSegmentCollection
                    PathSegmentCollection.Add(ArcSegment)


                    path.Data = pathGeometry
                    pathGeometry.Figures = PathFigureCollection
                    PathFigureCollection.Add(PathFigure)

                    ImageBox.Children.Add(path)
                End If
                If True Then
                    Dim path As New Path
                    path.Stroke = Brushes.LightPink
                    path.StrokeThickness = 6
                    path.HorizontalAlignment = HorizontalAlignment.Left
                    path.VerticalAlignment = VerticalAlignment.Top
                    path.Margin = New Thickness(pwidth / 2, pheight / 2, 0, 0)



                    Dim pathGeometry As New PathGeometry
                    Dim PathFigureCollection As New PathFigureCollection
                    Dim PathSegmentCollection As New PathSegmentCollection
                    Dim ArcSegment As New ArcSegment
                    ArcSegment.Size = New Size(50, 50)
                    ArcSegment.SweepDirection = SweepDirection.Counterclockwise
                    ArcSegment.Point = New Point(50, 0)


                    Dim PathFigure As New PathFigure

                    Dim x As Double = Math.Cos(AttackAngle / 128 * Math.PI) * 50
                    Dim y As Double = Math.Sin(AttackAngle / 128 * Math.PI) * 50

                    PathFigure.StartPoint = New Point(x, y)
                    PathFigure.Segments = PathSegmentCollection
                    PathSegmentCollection.Add(ArcSegment)


                    path.Data = pathGeometry
                    pathGeometry.Figures = PathFigureCollection
                    PathFigureCollection.Add(PathFigure)

                    ImageBox.Children.Add(path)
                End If
                If LaunchSpin <> 0 Then
                    If True Then
                        Dim LaunchSpinLine As New Line
                        LaunchSpinLine.Stroke = New SolidColorBrush(Color.FromArgb(192, 0, 0, 200))
                        LaunchSpinLine.X1 = pwidth / 2
                        LaunchSpinLine.Y1 = pwidth / 2
                        LaunchSpinLine.X2 = Math.Cos(LaunchSpin / 128 * Math.PI) * 50 + pwidth / 2
                        LaunchSpinLine.Y2 = Math.Sin(LaunchSpin / 128 * Math.PI) * 50 + pwidth / 2
                        LaunchSpinLine.HorizontalAlignment = HorizontalAlignment.Left
                        LaunchSpinLine.VerticalAlignment = VerticalAlignment.Top
                        LaunchSpinLine.StrokeThickness = 4
                        ImageBox.Children.Add(LaunchSpinLine)
                    End If
                    If True Then
                        Dim LaunchSpinLine As New Line
                        LaunchSpinLine.Stroke = New SolidColorBrush(Color.FromArgb(192, 0, 0, 200))
                        LaunchSpinLine.X1 = pwidth / 2
                        LaunchSpinLine.Y1 = pwidth / 2
                        LaunchSpinLine.X2 = Math.Cos(LaunchSpin / 128 * Math.PI) * 50 + pwidth / 2
                        LaunchSpinLine.Y2 = -Math.Sin(LaunchSpin / 128 * Math.PI) * 50 + pwidth / 2
                        LaunchSpinLine.HorizontalAlignment = HorizontalAlignment.Left
                        LaunchSpinLine.VerticalAlignment = VerticalAlignment.Top
                        LaunchSpinLine.StrokeThickness = 4
                        ImageBox.Children.Add(LaunchSpinLine)
                    End If
                End If


                If True Then
                    Dim LaunchSpinLine As New Line
                    LaunchSpinLine.StrokeDashArray = New DoubleCollection({2, 2})
                    LaunchSpinLine.Stroke = New SolidColorBrush(Color.FromArgb(192, 40, 200, 40))
                    LaunchSpinLine.X1 = 0
                    LaunchSpinLine.Y1 = pheight / 2 + UpwardOffset
                    LaunchSpinLine.X2 = pwidth
                    LaunchSpinLine.Y2 = pheight / 2 + UpwardOffset
                    LaunchSpinLine.HorizontalAlignment = HorizontalAlignment.Left
                    LaunchSpinLine.VerticalAlignment = VerticalAlignment.Top
                    LaunchSpinLine.StrokeThickness = 2
                    ImageBox.Children.Add(LaunchSpinLine)
                End If
                If True Then
                    Dim LaunchSpinLine As New Line
                    LaunchSpinLine.StrokeDashArray = New DoubleCollection({2, 2})
                    LaunchSpinLine.Stroke = New SolidColorBrush(Color.FromArgb(192, 40, 200, 40))
                    LaunchSpinLine.X1 = pwidth / 2 - ForwardOffset
                    LaunchSpinLine.Y1 = 0
                    LaunchSpinLine.X2 = pwidth / 2 - ForwardOffset
                    LaunchSpinLine.Y2 = pheight
                    LaunchSpinLine.HorizontalAlignment = HorizontalAlignment.Left
                    LaunchSpinLine.VerticalAlignment = VerticalAlignment.Top
                    LaunchSpinLine.StrokeThickness = 2
                    ImageBox.Children.Add(LaunchSpinLine)
                End If


        End Select







        If IsDelete Then
            Timer.Enabled = False
        End If
    End Sub






    Public Sub DeleteImage(tImages As SCImage)
        Images.Remove(tImages)
    End Sub
    Public Sub CreateImage(imageID As Integer, x As Integer, y As Integer, Optional Flag As Integer = 0)
        'u16<image#> u8<x> u8<y>
        Dim firstIscrptID As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.images, "Iscript ID", imageID)
        Select Case Flag
            Case 0
                Images.Add(New SCImage(imageID, firstIscrptID, New Windows.Point(x, y), Me))
            Case &H9
                Images.Insert(0, New SCImage(imageID, firstIscrptID, New Windows.Point(x, y), Me))
        End Select

    End Sub




    Public Sub Delete()
        IsDelete = True
    End Sub
    Private Images As List(Of SCImage)
End Class
