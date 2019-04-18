Public Class RequireListBoxItem
    Private RequireData As CRequireData.RequireBlock
    Public ReadOnly Property GettRequireData As CRequireData.RequireBlock
        Get
            Return RequireData
        End Get
    End Property




    Public Sub New(tRequireData As CRequireData.RequireBlock)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        RequireData = tRequireData


        If RequireData.HasValue Then
            TextV.Visibility = Visibility.Visible
            IconB.Visibility = Visibility.Visible

            Textb.Text = RequireData.opText
            TextV.Text = RequireData.ValueText

            Dim DatFile As SCDatFiles.DatFiles
            Dim isIcon As Boolean = False
            Dim ImageIndex As Integer

            If RequireData.opCode = CRequireData.EOpCode.Is_researched Then
                DatFile = SCDatFiles.DatFiles.techdata
            Else
                DatFile = SCDatFiles.DatFiles.units
            End If

            Select Case DatFile
                Case SCDatFiles.DatFiles.units
                    Dim tGraphics As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Graphics", RequireData.Value)
                    Dim tSprite As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", tGraphics)
                    Dim timage As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", tSprite)

                    ImageIndex = timage

                Case SCDatFiles.DatFiles.techdata
                    Dim tIcon As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.techdata, "Icon", RequireData.Value)

                    isIcon = True
                    ImageIndex = tIcon
            End Select
            If isIcon Then
                Icon.Source = scData.GetIcon(ImageIndex, False)
            Else
                Icon.Source = scData.GetGRPImage(ImageIndex, 12, False)
            End If
        Else
            TextV.Visibility = Visibility.Collapsed
            IconB.Visibility = Visibility.Collapsed
            Textb.Text = RequireData.opText

        End If
    End Sub


    Public Sub SelectTopBorder()
        TopBorder.Visibility = Visibility.Visible
        DownBorder.Visibility = Visibility.Hidden
    End Sub
    Public Sub SelectDownBorder()
        TopBorder.Visibility = Visibility.Hidden
        DownBorder.Visibility = Visibility.Visible
    End Sub
    Public Sub DSelectBorder()
        TopBorder.Visibility = Visibility.Hidden
        DownBorder.Visibility = Visibility.Hidden
    End Sub


    Private Sub DockPanel_MouseMove(sender As Object, e As MouseEventArgs)
        'If e.GetPosition(Me).Y > Me.ActualHeight / 2 Then
        '    TopBorder.Visibility = Visibility.Hidden
        '    DownBorder.Visibility = Visibility.Visible
        'Else
        '    TopBorder.Visibility = Visibility.Visible
        '    DownBorder.Visibility = Visibility.Hidden
        'End If


        'If e.GetPosition(Me).Y > Me.ActualHeight / 3 * 2 Then
        '    TopBorder.Visibility = Visibility.Hidden
        '    DownBorder.Visibility = Visibility.Visible
        'ElseIf e.GetPosition(Me).Y < Me.ActualHeight / 3 Then
        '    TopBorder.Visibility = Visibility.Visible
        '    DownBorder.Visibility = Visibility.Hidden
        'Else
        '    TopBorder.Visibility = Visibility.Hidden
        '    DownBorder.Visibility = Visibility.Hidden
        'End If
        'Textb.Text = e.GetPosition(Me).Y
    End Sub

    Private Sub UserControl_MouseLeave(sender As Object, e As MouseEventArgs)
        'TopBorder.Visibility = Visibility.Hidden
        'DownBorder.Visibility = Visibility.Hidden
    End Sub
End Class
