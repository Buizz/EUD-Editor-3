Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

Public Class GRPBox
    Private IsDelete As Boolean = False

    Private Bitmap As Bitmap
    Private Pixels() As Byte

    Private GRPImageBox As GRPImageBox
    Private ImageBox As Forms.PictureBox
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

    Public Sub New(ImageNum As Integer, tImageBox As Forms.PictureBox, tGRPImageBox As GRPImageBox, AnimHeaderIndex As Integer, Optional tFlag As GRPImageBox.BoxType = GRPImageBox.BoxType.Image, Optional tFObjectID As Integer = 0)
        '자채적으로 데이터를 읽고 쓴다.
        ImageBox = tImageBox
        ImageID = ImageNum
        GRPImageBox = tGRPImageBox
        FObjectID = tFObjectID
        Flag = tFlag

        ReDim Pixels(256 * 256 - 1)
        Bitmap = New Bitmap(256, 256, Imaging.PixelFormat.Format8bppIndexed)

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
        '픽셀 초기화
        For y = 0 To 255
            For x = 0 To 255
                Pixels(y * 256 + x) = 1
            Next
        Next

        If Flag = GRPImageBox.BoxType.Sprite Then
            If FObjectID >= 130 Then
                SelectionImage.GetGRP.DrawToBytes(SelectionImage, Pixels, New Size(256, 256), 255)
            End If
        End If

        For i = 0 To Images.Count - 1
            Images(i).Exec()

            Images(i).GetGRP.DrawToBytes(Images(i), Pixels, New Size(256, 256))
            'ImageBox.Source = Images(i).GetGRP.DrawGRP(Images(i).GetFrameGRP)
        Next

        Dim ci As Integer = 0
        For i = 0 To Images.Count - 1
            If Images(ci).IsDelete Then
                Images.Remove(Images(ci))
            Else
                ci += 1
            End If
        Next



        Dim bm As Bitmap = Bitmap
        Dim bmd As New BitmapData
        bmd = bm.LockBits(New Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, Imaging.PixelFormat.Format8bppIndexed)

        Dim scan0 As IntPtr = bmd.Scan0
        Dim stride As Integer = bmd.Stride

        Try
            Marshal.Copy(Pixels, 0, scan0, Pixels.Length)
        Catch ex As Exception

        End Try


        bm.UnlockBits(bmd)


        Dim CPalette As Imaging.ColorPalette
        CPalette = bm.Palette
        For i = 0 To 255
            CPalette.Entries(i) = MGRP.Palett(PalettType.install).GetColor(i)
        Next
        bm.Palette = CPalette

        Select Case Flag
            Case GRPImageBox.BoxType.Unit
                Dim tBitmap As Bitmap = New Bitmap(Bitmap)

                Dim grp As Graphics
                grp = Graphics.FromImage(tBitmap)



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

                'StarEdit Placement Box Width
                'StarEdit Placement Box Height

                'Addon Horizontal (X) Position
                'Addon Vertical (Y) Position


                'Unit Size Left
                'Unit Size Up
                'Unit Size Right
                'Unit Size Down




                '건물 생산 크기
                grp.FillRectangle(New SolidBrush(Color.FromArgb(100, 183, 240, 177)), New Rectangle(128 - StarEditBoxWidth \ 2, 128 - StarEditBoxHeight \ 2, StarEditBoxWidth, StarEditBoxHeight))
                'grp.DrawRectangle(New Pen(Color.FromArgb(255, 183, 240, 177)), New Rectangle(128 - StarEditBoxWidth \ 2, 128 - StarEditBoxHeight \ 2, StarEditBoxWidth, StarEditBoxHeight))

                '유닛 크기 끝선
                'grp.FillRectangle(New SolidBrush(Color.FromArgb(40, 255, 0, 251)), New Rectangle(128 - UnitSizeLeft, 128 - UnitSizeUp, UnitSizeRight + UnitSizeLeft, UnitSizeDown + UnitSizeUp))
                grp.DrawRectangle(New Pen(Color.FromArgb(255, 255, 0, 0)), New Rectangle(128 - UnitSizeLeft, 128 - UnitSizeUp, UnitSizeRight + UnitSizeLeft, UnitSizeDown + UnitSizeUp))

                If FObjectID >= SCMenCount And (AddonHorizontal <> 0 Or AddonVertical <> 0) Then
                    grp.FillRectangle(New SolidBrush(Color.FromArgb(100, 150, 150, 150)), New Rectangle(128 - 32 - AddonHorizontal, 128 - 32 - AddonVertical, 128, 96))
                End If

                If UnitDirection <> 32 Then
                    Dim diregee As Double = (UnitDirection - 8) / 16 * Math.PI

                    grp.DrawLine(New Pen(Color.FromArgb(255, 29, 219, 22)), 128, 128, CInt(Math.Cos(diregee) * 50 + 128), CInt(Math.Sin(diregee) * 50 + 128))
                End If


                ImageBox.Image = tBitmap
            Case GRPImageBox.BoxType.Sprite
                If FObjectID >= 130 Then
                    Dim tBitmap As Bitmap = New Bitmap(Bitmap)

                    Dim grp As Graphics
                    grp = Graphics.FromImage(tBitmap)
                    Dim SelectOffset As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Sel.Circle Offset", FObjectID)
                    Dim HPBar As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Health Bar", FObjectID) + 3

                    Dim HpYOffset As Integer = 128 + (SelectOffset + SelectionImage.GetGRP.GRPFrame(0).frameHeight \ 2) + 7


                    grp.FillRectangle(New SolidBrush(Color.FromArgb(255, 150, 150, 150)), New Rectangle(128 - (HPBar) \ 2, HpYOffset, HPBar, 5))


                    ImageBox.Image = tBitmap
                Else
                    ImageBox.Image = Bitmap
                End If


            Case Else
                ImageBox.Image = Bitmap
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
