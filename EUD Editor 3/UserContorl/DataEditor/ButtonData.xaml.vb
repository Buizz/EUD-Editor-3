Imports System.Windows.Media.Animation

Public Class ButtonData
    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.ButtonData

    Public Property ObjectID As Integer

    Private ButtonSet As CButtonSet
    Private SelectButton As CButtonData

    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        AnimationInit()

        DataContext = pjData
        ObjectID = tObjectID

        UsedCodeList.Init(DatFiles, ObjectID)

        NameBar.Init(ObjectID, DatFiles, 0)

        ButtonSet = pjData.ExtraDat.ButtonData.GetButtonSet(ObjectID)
        toggleBtn.DataContext = pjData.BindingManager.ExtraDatBinding(DatFiles, "ButtonData", ObjectID)
        ButtonListReset()


        BIconSelecter.Init(SCDatFiles.DatFiles.Icon, Tool.GetText("BtnIcon"), 0)
        UseStrSelecter.Init(SCDatFiles.DatFiles.stattxt, Tool.GetText("BtnEnaStr"), 0)
        DUseStrSelecter.Init(SCDatFiles.DatFiles.stattxt, Tool.GetText("BtnDisStr"), 0)

        SelectButton = Nothing
        SelectBtnAction()
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID

        UsedCodeList.ReLoad(DatFiles, ObjectID)

        NameBar.ReLoad(ObjectID, DatFiles, 0)

        ButtonSet = pjData.ExtraDat.ButtonData.GetButtonSet(ObjectID)
        toggleBtn.DataContext = pjData.BindingManager.ExtraDatBinding(DatFiles, "ButtonData", ObjectID)
        ButtonListReset()
    End Sub

    Private Sub ButtonListReset()
        buttonList.Items.Clear()
        Dim btnCount As Integer = ButtonSet.ButtonS.Count
        For i = 0 To btnCount - 1
            buttonList.Items.Add(ButtonSet.ButtonS(i).GetListBoxItem)
        Next

        PreviewDraw()
    End Sub
    Private Sub PreviewDraw()
        Dim SelectBtnIndex As Integer = -1
        Dim imageDrawings As DrawingGroup = New DrawingGroup()

        Dim btnCount As Integer = ButtonSet.ButtonS.Count
        For i = 0 To btnCount - 1
            If SelectButton Is ButtonSet.ButtonS(i) Then
                SelectBtnIndex = i
            Else
                Dim loc As Point = ButtonSet.ButtonS(i).GetPos

                Dim rectDrawing As New GeometryDrawing(Brushes.Black,
                       Nothing, New RectangleGeometry(New Rect(loc.X, loc.Y, 32, 32)))
                imageDrawings.Children.Add(rectDrawing)


                Dim imageIcon As BitmapSource = ButtonSet.ButtonS(i).GetIcon

                Dim IconIMage As ImageDrawing = New ImageDrawing()
                IconIMage.Rect = New Rect(loc.X + (32 - imageIcon.Width) / 2, loc.Y + (32 - imageIcon.Height) / 2, imageIcon.Width, imageIcon.Height)
                IconIMage.ImageSource = imageIcon
                imageDrawings.Children.Add(IconIMage)
            End If
        Next

        If SelectBtnIndex <> -1 Then
            Dim loc As Point = ButtonSet.ButtonS(SelectBtnIndex).GetPos

            Dim imageIcon As BitmapSource = ButtonSet.ButtonS(SelectBtnIndex).GetIcon

            Dim IconIMage As ImageDrawing = New ImageDrawing()
            IconIMage.Rect = New Rect(loc.X + (32 - imageIcon.Width) / 2, loc.Y + (32 - imageIcon.Height) / 2, imageIcon.Width, imageIcon.Height)
            IconIMage.ImageSource = imageIcon
            imageDrawings.Children.Add(IconIMage)

            Dim rectDrawing As New GeometryDrawing(New SolidColorBrush(Color.FromArgb(102, 181, 243, 20)),
                       Nothing, New RectangleGeometry(New Rect(loc.X, loc.Y, 32, 32)))
            imageDrawings.Children.Add(rectDrawing)

            TopText.TextColred(SelectButton.GetEnaStr)
            BottomText.TextColred(SelectButton.GetDisStr)
        Else
            TopText.TextColred("")
            BottomText.TextColred("")
        End If


        Dim drawingImageSource As DrawingImage = New DrawingImage(imageDrawings)
        drawingImageSource.Freeze()

        PreviewImage.Source = drawingImageSource
    End Sub



    Private Sub ToggleBtn_Checked(sender As Object, e As RoutedEventArgs)
        buttonList.IsEnabled = False
        buttonList.SelectedIndex = -1
        ButtonListReset()
    End Sub

    Private Sub ToggleBtn_Unchecked(sender As Object, e As RoutedEventArgs)
        buttonList.IsEnabled = True
        ButtonListReset()
    End Sub

    Private Sub ButtonList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If buttonList.SelectedItem Is Nothing Then
            SelectButton = Nothing
            SelectBtnAction()
        Else
            SelectButton = CType(buttonList.SelectedItem, ListBoxItem).Tag
            SelectBtnAction()
        End If
    End Sub

    Private Sub SelectBtnAction()
        If SelectButton IsNot Nothing Then
            DefaultInfor.Visibility = Visibility.Visible
            ButtonAction.Visibility = Visibility.Visible

            '선택 작업
            BtnLocation.Text = SelectButton.pos
            BIconSelecter.Value = SelectButton.icon
            UseStrSelecter.Value = SelectButton.enaStr - 1
            DUseStrSelecter.Value = SelectButton.disStr - 1
        Else
            DefaultInfor.Visibility = Visibility.Hidden
            ButtonAction.Visibility = Visibility.Hidden
        End If
        PreviewDraw()
    End Sub

    Private Sub ContextMenu_Opened(sender As Object, e As RoutedEventArgs)
        If buttonList.SelectedItem Is Nothing Then
            CutItem.IsEnabled = False
            CopyItem.IsEnabled = False
            DeleteItem.IsEnabled = False
            CoverWriteItem.IsEnabled = False
        Else
            CutItem.IsEnabled = True
            CopyItem.IsEnabled = True
            DeleteItem.IsEnabled = True
            If ButtonSet.Pasteable Then
                CoverWriteItem.IsEnabled = True
                PasteItem.IsEnabled = True
            Else
                CoverWriteItem.IsEnabled = False
                PasteItem.IsEnabled = False
            End If
        End If

        If ButtonSet.Pasteable Then
            PasteItem.IsEnabled = True
        Else
            PasteItem.IsEnabled = False
        End If

    End Sub

    Private Sub NewItem_Click(sender As Object, e As RoutedEventArgs)
        OpenNewWindow()
    End Sub

    Private Sub CoverWriteItem_Click(sender As Object, e As RoutedEventArgs)
        SelectButton.OverWriteData()
        If buttonList.SelectedItem IsNot Nothing Then
            CType(buttonList.SelectedItem, ListBoxItem).Content = SelectButton.GetListBoxContent
        End If
    End Sub

    Private Sub CutItem_Click(sender As Object, e As RoutedEventArgs)
        SelectButton.CopyData()
        ButtonSet.ButtonS.Remove(SelectButton)
        buttonList.Items.Remove(buttonList.SelectedItem)
    End Sub

    Private Sub CopyItem_Click(sender As Object, e As RoutedEventArgs)
        SelectButton.CopyData()
    End Sub

    Private Sub PasteItem_Click(sender As Object, e As RoutedEventArgs)
        ButtonSet.PasteBtn()
        ButtonListReset()
    End Sub

    Private Sub DeleteItem_Click(sender As Object, e As RoutedEventArgs)
        ButtonSet.ButtonS.Remove(SelectButton)
        buttonList.Items.Remove(buttonList.SelectedItem)
    End Sub

    Private Sub DialogCancelBtn_Click(sender As Object, e As RoutedEventArgs)
        CloseStroyBoard.Begin(Me)
    End Sub

    Private Sub DialogOkayBtn_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub OpenNewWindow()
        OpenStroyBoard.Begin(Me)
        'CodeSelecter.SelectedIndex = 2
        CreateEditWindow.Visibility = Visibility.Visible
    End Sub


    Private OpenStroyBoard As Storyboard
    Private CloseStroyBoard As Storyboard
    Private Sub AnimationInit()
        If True Then
            Dim scale1 As ScaleTransform = New ScaleTransform(1, 1)

            InputDialog.RenderTransformOrigin = New Point(0.5, 0.5)
            InputDialog.RenderTransform = scale1



            Dim myHeightAnimation As DoubleAnimation = New DoubleAnimation()
            myHeightAnimation.From = 0.0
            myHeightAnimation.To = 1.0
            myHeightAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myWidthAnimation As DoubleAnimation = New DoubleAnimation()
            myWidthAnimation.From = 0.0
            myWidthAnimation.To = 1.0
            myWidthAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myOpacityAnimation As DoubleAnimation = New DoubleAnimation()
            myOpacityAnimation.From = 0.0
            myOpacityAnimation.To = 1.0
            myOpacityAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            OpenStroyBoard = New Storyboard()
            OpenStroyBoard.Children.Add(myOpacityAnimation)
            OpenStroyBoard.Children.Add(myWidthAnimation)
            OpenStroyBoard.Children.Add(myHeightAnimation)
            Storyboard.SetTargetName(myOpacityAnimation, CreateEditWindow.Name)
            Storyboard.SetTargetName(myWidthAnimation, InputDialog.Name)
            Storyboard.SetTargetName(myHeightAnimation, InputDialog.Name)
            Storyboard.SetTargetProperty(myOpacityAnimation, New PropertyPath(Border.OpacityProperty))
            Storyboard.SetTargetProperty(myHeightAnimation, New PropertyPath("RenderTransform.ScaleY"))
            Storyboard.SetTargetProperty(myWidthAnimation, New PropertyPath("RenderTransform.ScaleX"))
        End If
        If True Then
            Dim scale1 As ScaleTransform = New ScaleTransform(1, 1)

            InputDialog.RenderTransformOrigin = New Point(0.5, 0.5)
            InputDialog.RenderTransform = scale1



            Dim myHeightAnimation As DoubleAnimation = New DoubleAnimation()
            myHeightAnimation.From = 1.0
            myHeightAnimation.To = 0.0
            myHeightAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myWidthAnimation As DoubleAnimation = New DoubleAnimation()
            myWidthAnimation.From = 1.0
            myWidthAnimation.To = 0.0
            myWidthAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myOpacityAnimation As DoubleAnimation = New DoubleAnimation()
            myOpacityAnimation.From = 1.0
            myOpacityAnimation.To = 0.0
            myOpacityAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            CloseStroyBoard = New Storyboard()
            CloseStroyBoard.Children.Add(myOpacityAnimation)
            CloseStroyBoard.Children.Add(myWidthAnimation)
            CloseStroyBoard.Children.Add(myHeightAnimation)
            Storyboard.SetTargetName(myOpacityAnimation, CreateEditWindow.Name)
            Storyboard.SetTargetName(myWidthAnimation, InputDialog.Name)
            Storyboard.SetTargetName(myHeightAnimation, InputDialog.Name)
            Storyboard.SetTargetProperty(myOpacityAnimation, New PropertyPath(Border.OpacityProperty))
            Storyboard.SetTargetProperty(myHeightAnimation, New PropertyPath("RenderTransform.ScaleY"))
            Storyboard.SetTargetProperty(myWidthAnimation, New PropertyPath("RenderTransform.ScaleX"))

            AddHandler CloseStroyBoard.Completed, Sub(sender As Object, e As EventArgs)
                                                      CreateEditWindow.Visibility = Visibility.Hidden
                                                  End Sub
        End If



        'InputDialog
    End Sub

    Private Sub BIconSelecter_ValueChange(sender As Object, e As RoutedEventArgs)
        If SelectButton IsNot Nothing Then
            SelectButton.icon = BIconSelecter.Value
            CType(buttonList.SelectedItem, ListBoxItem).Content = SelectButton.GetListBoxContent
            PreviewDraw()
        End If
    End Sub

    Private Sub UseStrSelecter_ValueChange(sender As Object, e As RoutedEventArgs)
        If SelectButton IsNot Nothing Then
            SelectButton.enaStr = UseStrSelecter.Value + 1
            CType(buttonList.SelectedItem, ListBoxItem).Content = SelectButton.GetListBoxContent
            PreviewDraw()
        End If
    End Sub

    Private Sub DUseStrSelecter_ValueChange(sender As Object, e As RoutedEventArgs)
        If SelectButton IsNot Nothing Then
            SelectButton.disStr = DUseStrSelecter.Value + 1
            CType(buttonList.SelectedItem, ListBoxItem).Content = SelectButton.GetListBoxContent
            PreviewDraw()
        End If
    End Sub

    Private Sub BtnLocation_TextChanged(sender As Object, e As TextChangedEventArgs)
        If SelectButton IsNot Nothing Then
            If IsNumeric(BtnLocation.Text) Then
                SelectButton.pos = BtnLocation.Text
                PreviewDraw()
            End If
        End If
    End Sub
End Class
