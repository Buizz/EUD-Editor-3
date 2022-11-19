Imports System.Windows.Media.Animation

Public Class ButtonData
    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.ButtonData

    Public Property ObjectID As Integer

    Private ButtonSet As CButtonSet
    Private SelectButton As CButtonData

    Public Shared NewItemKeyInputCommand As RoutedUICommand = New RoutedUICommand("myCommand", "myCommand", GetType(ButtonData))
    Public Shared CutItemKeyInputCommand As RoutedUICommand = New RoutedUICommand("myCommand", "myCommand", GetType(ButtonData))
    Public Shared CopyItemKeyInputCommand As RoutedUICommand = New RoutedUICommand("myCommand", "myCommand", GetType(ButtonData))
    Public Shared PasteItemKeyInputCommand As RoutedUICommand = New RoutedUICommand("myCommand", "myCommand", GetType(ButtonData))
    Public Shared DeleteItemKeyInputCommand As RoutedUICommand = New RoutedUICommand("myCommand", "myCommand", GetType(ButtonData))


    Private Sub NewItemCommandExecute(ByVal target As Object, ByVal e As ExecutedRoutedEventArgs)
        SNewItem()
    End Sub
    Private Sub CutItemCommandExcute(ByVal target As Object, ByVal e As ExecutedRoutedEventArgs)
        SCutItem()
    End Sub
    Private Sub CopyItemCommandExecute(ByVal target As Object, ByVal e As ExecutedRoutedEventArgs)
        SCopyItem()
    End Sub
    Private Sub PasteItemCommandExecute(ByVal target As Object, ByVal e As ExecutedRoutedEventArgs)
        SPasteItem()
    End Sub
    Private Sub DeleteItemCommandExecute(ByVal target As Object, ByVal e As ExecutedRoutedEventArgs)
        SDeleteItem()
    End Sub

    Private borders As List(Of Border)
    Private images As List(Of Image)

    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        borders = New List(Of Border)
        images = New List(Of Image)


        For i = 0 To 8
            Dim tb As New Border
            Dim ti As New Image

            tb.Background = Brushes.Black
            tb.Width = 32
            tb.Height = 32
            tb.Child = ti



            borders.Add(tb)
            images.Add(ti)

            xpanel.Children.Add(tb)
        Next







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

        For i = 0 To scData.FuncConDict.Values.Count - 1
            ConFunc.Items.Add(scData.FuncConDict.Values(i).Name)
        Next
        For i = 0 To scData.FuncActDict.Values.Count - 1
            ActFunc.Items.Add(scData.FuncActDict.Values(i).Name)
        Next

        Dim TypeSelectListString() As String = Tool.GetText("BtnNewIndex").Split("|")

        For i = 0 To TypeSelectListString.Count - 1
            TypeSelectListBox.Items.Add(TypeSelectListString(i))
        Next


        CreateEditWindow.Visibility = Visibility.Hidden


        ReDim ImageBrush(8)
        For y = 0 To 2
            For x = 0 To 2
                Dim btn As New Button
                'btn.Style = Application.Current.Resources("MaterialDesignFlatButton")

                btn.Width = Double.NaN
                btn.Height = Double.NaN
                btn.BorderBrush = Brushes.Transparent

                AddHandler btn.Click, AddressOf BtnClick

                Grid.SetRow(btn, y)
                Grid.SetColumn(btn, x)

                btn.Content = x + y * 3 + 1 & vbCrLf & vbCrLf & vbCrLf
                btn.Tag = x + y * 3
                Dim Brush As New ImageBrush()
                'Brush.ImageSource = scData.GetIcon(0, False)
                Brush.Stretch = Stretch.None

                ImageBrush(x + y * 3) = Brush

                btn.Background = Brush


                ButtonPreviewSelecter.Children.Add(btn)
            Next
        Next
    End Sub
    Private ImageBrush() As ImageBrush
    Private Sub BtnClick(sender As Object, e As RoutedEventArgs)
        SelectPos = sender.Tag
        TypeSelecterPreviewReset()
    End Sub
    Private SelectPos As Integer = 0
    Private Sub DialogOkayBtn_Click(sender As Object, e As RoutedEventArgs)
        CloseStoryBoard.Begin(Me)

        ButtonSet.AddButtonType(TypeSelectListBox.SelectedIndex, SelectPos, ValueSelecter.Value)
        ButtonListReset()
    End Sub
    Private Sub TypeSelecterPreviewReset()
        Dim BType As CButtonSet.BType = TypeSelectListBox.SelectedIndex

        For i = 0 To 8
            ImageBrush(i).ImageSource = Nothing
        Next

        If pgData.Setting(ProgramData.TSetting.Graphic) = 0 Then
            Exit Sub
        End If
        Select Case BType
            Case CButtonSet.BType.DefaultCommand
                ImageBrush(0).ImageSource = scData.GetIcon(228)
                ImageBrush(1).ImageSource = scData.GetIcon(229)
                ImageBrush(2).ImageSource = scData.GetIcon(230)
                ImageBrush(3).ImageSource = scData.GetIcon(254)
                ImageBrush(4).ImageSource = scData.GetIcon(255)
            Case CButtonSet.BType.MovingablebuildingCommand
                ImageBrush(0).ImageSource = scData.GetIcon(228)
                ImageBrush(1).ImageSource = scData.GetIcon(229)
                ImageBrush(8).ImageSource = scData.GetIcon(283)
            Case CButtonSet.BType.BurrowCommand
                ImageBrush(8).ImageSource = scData.GetIcon(259)
            Case CButtonSet.BType.GatherCommand
                ImageBrush(4).ImageSource = scData.GetIcon(231)
                ImageBrush(5).ImageSource = scData.GetIcon(233)
            Case CButtonSet.BType.TransportCommand
                ImageBrush(7).ImageSource = scData.GetIcon(309)
                ImageBrush(8).ImageSource = scData.GetIcon(312)
            Case CButtonSet.BType.EmptyButton
                ImageBrush(SelectPos).ImageSource = scData.GetIcon(0)
            Case CButtonSet.BType.UnitTraning
                ImageBrush(SelectPos).ImageSource = scData.GetIcon(ValueSelecter.Value)
            Case CButtonSet.BType.UnitMorph
                ImageBrush(SelectPos).ImageSource = scData.GetIcon(ValueSelecter.Value)
            Case CButtonSet.BType.UpgradeResearch
                Dim IconIndex As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.upgrades, "Icon", ValueSelecter.Value)

                ImageBrush(SelectPos).ImageSource = scData.GetIcon(IconIndex)
            Case CButtonSet.BType.TechResearch
                Dim IconIndex As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.techdata, "Icon", ValueSelecter.Value)

                ImageBrush(SelectPos).ImageSource = scData.GetIcon(IconIndex)
            Case CButtonSet.BType.TechUse
                Dim IconIndex As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.techdata, "Icon", ValueSelecter.Value)

                ImageBrush(SelectPos).ImageSource = scData.GetIcon(IconIndex)
            Case CButtonSet.BType.BuildingMorph
                ImageBrush(SelectPos).ImageSource = scData.GetIcon(ValueSelecter.Value)
            Case CButtonSet.BType.BuildingBuild_Morph
                ImageBrush(SelectPos).ImageSource = scData.GetIcon(ValueSelecter.Value)
            Case CButtonSet.BType.BuildingBuild_Terran
                ImageBrush(SelectPos).ImageSource = scData.GetIcon(ValueSelecter.Value)
            Case CButtonSet.BType.BuildingBuild_Protoss
                ImageBrush(SelectPos).ImageSource = scData.GetIcon(ValueSelecter.Value)
            Case CButtonSet.BType.BuildingBuild_Addon
                ImageBrush(SelectPos).ImageSource = scData.GetIcon(ValueSelecter.Value)
        End Select
    End Sub


    Private Sub TypeSelectListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If TypeSelectListBox.SelectedIndex <> -1 Then
            Dim datfile As SCDatFiles.DatFiles = ButtonSet.ButtonTypeDatFile(TypeSelectListBox.SelectedIndex)
            If datfile = SCDatFiles.DatFiles.None Then
                ValueSelecter.Visibility = Visibility.Collapsed
            Else
                ValueSelecter.Init(datfile, Tool.GetText("BtnNewString"), 0)
                ValueSelecter.Visibility = Visibility.Visible
            End If
        End If
        TypeSelecterPreviewReset()
    End Sub



    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID

        UsedCodeList.ReLoad(DatFiles, ObjectID)

        NameBar.ReLoad(ObjectID, DatFiles, 0)

        ButtonSet = pjData.ExtraDat.ButtonData.GetButtonSet(ObjectID)
        toggleBtn.DataContext = pjData.BindingManager.ExtraDatBinding(DatFiles, "ButtonData", ObjectID)
        ButtonListReset()
        CreateEditWindow.Visibility = Visibility.Hidden
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
        'Dim imageDrawings As DrawingGroup = New DrawingGroup()

        Dim btnCount As Integer = ButtonSet.ButtonS.Count

        For i = 0 To 8
            borders(i).Opacity = 1
            images(i).Source = Nothing
        Next


        For i = 0 To btnCount - 1
            If SelectButton Is ButtonSet.ButtonS(i) Then
                SelectBtnIndex = i
            Else
                If pgData.Setting(ProgramData.TSetting.Graphic) > 0 Then
                    Dim loc As Integer = ButtonSet.ButtonS(i).GetPosindex - 1

                    images(loc).Source = ButtonSet.ButtonS(i).GetIcon


                    'PreviewImage.Source = ButtonSet.ButtonS(i).GetIcon

                    'Dim rectDrawing As New GeometryDrawing(Brushes.Black,
                    '       Nothing, New RectangleGeometry(New Rect(loc.X, loc.Y, 32, 32)))
                    'imageDrawings.Children.Add(rectDrawing)


                    '    Dim imageIcon As BitmapSource = ButtonSet.ButtonS(i).GetIcon

                    '    Dim IconIMage As New ImageDrawing()

                    '    IconIMage.Rect = New Rect(loc.X + (32 - imageIcon.Width) / 2, loc.Y + (32 - imageIcon.Height) / 2, 32, 32)


                    '    IconIMage.ImageSource = imageIcon

                    '    imageDrawings.Children.Add(IconIMage)
                End If
            End If
        Next

        If SelectBtnIndex <> -1 Then
            Dim loc As Integer = ButtonSet.ButtonS(SelectBtnIndex).GetPosindex - 1
            If (loc >= 9) Then
                Return
            End If


            images(loc).Source = ButtonSet.ButtonS(SelectBtnIndex).GetIcon
            borders(loc).Opacity = 0.5



            'If pgData.Setting(ProgramData.TSetting.Graphic) > 0 Then
            '    Dim imageIcon As BitmapSource = ButtonSet.ButtonS(SelectBtnIndex).GetIcon
            '    Dim IconIMage As ImageDrawing = New ImageDrawing()
            '    IconIMage.Rect = New Rect(loc.X + (32 - imageIcon.Width) / 2, loc.Y + (32 - imageIcon.Height) / 2, 32, 32)
            '    IconIMage.ImageSource = imageIcon
            '    imageDrawings.Children.Add(IconIMage)

            '    Dim rectDrawing As New GeometryDrawing(New SolidColorBrush(Color.FromArgb(102, 181, 243, 20)),
            '   Nothing, New RectangleGeometry(New Rect(loc.X + (32 - imageIcon.Width) / 2, loc.Y + (32 - imageIcon.Height) / 2, 32, 32)))
            '    imageDrawings.Children.Add(rectDrawing)
            'Else
            '    Dim rectDrawing As New GeometryDrawing(New SolidColorBrush(Color.FromArgb(102, 181, 243, 20)),
            '   Nothing, New RectangleGeometry(New Rect(loc.X + (32 - 32) / 2, loc.Y + (32 - 32) / 2, 32, 32)))
            '    imageDrawings.Children.Add(rectDrawing)
            'End If




            TopText.TextColred(SelectButton.GetEnaStr)
            BottomText.TextColred(SelectButton.GetDisStr)
        Else
            TopText.TextColred("")
            BottomText.TextColred("")
        End If


        'Dim drawingImageSource As DrawingImage = New DrawingImage(imageDrawings)
        'drawingImageSource.Freeze()

        'PreviewImage.Source = drawingImageSource
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
            UseStrSelecter.Value = SelectButton.enaStr
            DUseStrSelecter.Value = SelectButton.disStr

            Try
                ConFunc.SelectedIndex = scData.FuncConDict(SelectButton.con).Index
                ConFuncText.Text = scData.FuncConDict(SelectButton.con).Index
                If scData.FuncConDict(SelectButton.con).DatType <> SCDatFiles.DatFiles.None Then
                    ConVal.Visibility = Visibility.Visible
                    ConVal.Init(scData.FuncConDict(SelectButton.con).DatType, Tool.GetText("BtnConVal"), SelectButton.conval)
                Else
                    ConVal.Visibility = Visibility.Hidden
                End If

            Catch ex As Exception
                ConFunc.SelectedIndex = -1
                ConFuncText.Text = ""
                ConVal.Visibility = Visibility.Hidden
            End Try
            Try
                ActFunc.SelectedIndex = scData.FuncActDict(SelectButton.act).Index
                ActFuncText.Text = scData.FuncActDict(SelectButton.act).Index
                If scData.FuncActDict(SelectButton.act).DatType <> SCDatFiles.DatFiles.None Then
                    ActVal.Visibility = Visibility.Visible
                    ActVal.Init(scData.FuncActDict(SelectButton.act).DatType, Tool.GetText("BtnActVal"), SelectButton.actval)
                Else
                    ActVal.Visibility = Visibility.Hidden
                End If
            Catch ex As Exception
                ActFunc.SelectedIndex = -1
                ActFuncText.Text = ""
                ActVal.Visibility = Visibility.Hidden
            End Try
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




    Private Sub SNewItem()
        OpenNewWindow()
    End Sub
    Private Sub SCutItem()
        If buttonList.SelectedItem IsNot Nothing Then
            SelectButton.CopyData()
            ButtonSet.ButtonS.Remove(SelectButton)
            buttonList.Items.Remove(buttonList.SelectedItem)
        End If
    End Sub
    Private Sub SCopyItem()
        If buttonList.SelectedItem IsNot Nothing Then
            SelectButton.CopyData()
        End If
    End Sub
    Private Sub SPasteItem()
        If ButtonSet.Pasteable Then
            ButtonSet.PasteBtn()
            ButtonListReset()
        End If
    End Sub
    Private Sub SDeleteItem()
        If buttonList.SelectedItem IsNot Nothing Then
            ButtonSet.ButtonS.Remove(SelectButton)
            buttonList.Items.Remove(buttonList.SelectedItem)
        End If
    End Sub
    Private Sub NewItemCommandCanExecute(ByVal sender As Object, ByVal e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub
    Private Sub CopyCommandCanExecute(ByVal sender As Object, ByVal e As CanExecuteRoutedEventArgs)
        If buttonList.SelectedItem IsNot Nothing Then
            e.CanExecute = True
        Else
            e.CanExecute = False
        End If
    End Sub
    Private Sub PasteCommandCanExecute(ByVal sender As Object, ByVal e As CanExecuteRoutedEventArgs)
        If ButtonSet.Pasteable Then
            e.CanExecute = True
        Else
            e.CanExecute = False
        End If
    End Sub
    Private Sub CutCommandCanExecute(ByVal sender As Object, ByVal e As CanExecuteRoutedEventArgs)
        If buttonList.SelectedItem IsNot Nothing Then
            e.CanExecute = True
        Else
            e.CanExecute = False
        End If
    End Sub
    Private Sub DeleteCommandCanExecute(ByVal sender As Object, ByVal e As CanExecuteRoutedEventArgs)
        If buttonList.SelectedItem IsNot Nothing Then
            e.CanExecute = True
        Else
            e.CanExecute = False
        End If
    End Sub






    Private Sub NewItem_Click(sender As Object, e As RoutedEventArgs)
        SNewItem()
    End Sub

    Private Sub CoverWriteItem_Click(sender As Object, e As RoutedEventArgs)
        SelectButton.OverWriteData()
        If buttonList.SelectedItem IsNot Nothing Then
            CType(buttonList.SelectedItem, ListBoxItem).Content = SelectButton.GetListBoxContent
        End If
    End Sub

    Private Sub CutItem_Click(sender As Object, e As RoutedEventArgs)
        SCutItem()
    End Sub

    Private Sub CopyItem_Click(sender As Object, e As RoutedEventArgs)
        SCopyItem()
    End Sub

    Private Sub PasteItem_Click(sender As Object, e As RoutedEventArgs)
        SPasteItem()
    End Sub

    Private Sub DeleteItem_Click(sender As Object, e As RoutedEventArgs)
        SDeleteItem()
    End Sub

    Private Sub DialogCancelBtn_Click(sender As Object, e As RoutedEventArgs)
        CloseStoryBoard.Begin(Me)
    End Sub






    Private Sub OpenNewWindow()
        OpenStoryBoard.Begin(Me)
        'CodeSelecter.SelectedIndex = 2
        CreateEditWindow.Visibility = Visibility.Visible

        TypeSelectListBox.SelectedIndex = 0
        Dim datfile As SCDatFiles.DatFiles = ButtonSet.ButtonTypeDatFile(TypeSelectListBox.SelectedIndex)
        If datfile = SCDatFiles.DatFiles.None Then
            ValueSelecter.Visibility = Visibility.Collapsed
        Else
            ValueSelecter.Init(datfile, Tool.GetText("BtnNewString"), 0)
            ValueSelecter.Visibility = Visibility.Visible
        End If
        TypeSelecterPreviewReset()
    End Sub


    Private OpenStoryBoard As Storyboard
    Private CloseStoryBoard As Storyboard
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

            OpenStoryBoard = New Storyboard()
            OpenStoryBoard.Children.Add(myOpacityAnimation)
            OpenStoryBoard.Children.Add(myWidthAnimation)
            OpenStoryBoard.Children.Add(myHeightAnimation)
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

            CloseStoryBoard = New Storyboard()
            CloseStoryBoard.Children.Add(myOpacityAnimation)
            CloseStoryBoard.Children.Add(myWidthAnimation)
            CloseStoryBoard.Children.Add(myHeightAnimation)
            Storyboard.SetTargetName(myOpacityAnimation, CreateEditWindow.Name)
            Storyboard.SetTargetName(myWidthAnimation, InputDialog.Name)
            Storyboard.SetTargetName(myHeightAnimation, InputDialog.Name)
            Storyboard.SetTargetProperty(myOpacityAnimation, New PropertyPath(Border.OpacityProperty))
            Storyboard.SetTargetProperty(myHeightAnimation, New PropertyPath("RenderTransform.ScaleY"))
            Storyboard.SetTargetProperty(myWidthAnimation, New PropertyPath("RenderTransform.ScaleX"))

            AddHandler CloseStoryBoard.Completed, Sub(sender As Object, e As EventArgs)
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
            SelectButton.enaStr = UseStrSelecter.Value
            CType(buttonList.SelectedItem, ListBoxItem).Content = SelectButton.GetListBoxContent
            PreviewDraw()
        End If
    End Sub

    Private Sub DUseStrSelecter_ValueChange(sender As Object, e As RoutedEventArgs)
        If SelectButton IsNot Nothing Then
            SelectButton.disStr = DUseStrSelecter.Value
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

    Private Sub ConFunc_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If SelectButton IsNot Nothing Then
            If ConFunc.SelectedIndex <> -1 Then
                SelectButton.con = scData.FuncConDict.Keys(ConFunc.SelectedIndex)
                If scData.FuncConDict(SelectButton.con).DatType <> SCDatFiles.DatFiles.None Then
                    ConVal.Visibility = Visibility.Visible
                    ConVal.Init(scData.FuncConDict(SelectButton.con).DatType, Tool.GetText("BtnConVal"), SelectButton.conval)
                Else
                    ConVal.Visibility = Visibility.Hidden
                End If
                ConFuncText.Text = ConFunc.SelectedIndex
                PreviewDraw()
            End If
        End If
    End Sub

    Private Sub ConVal_ValueChange(sender As Object, e As RoutedEventArgs)
        If SelectButton IsNot Nothing Then
            SelectButton.conval = ConVal.Value
            PreviewDraw()
        End If
    End Sub

    Private Sub ActFunc_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If SelectButton IsNot Nothing Then
            If ActFunc.SelectedIndex <> -1 Then
                SelectButton.act = scData.FuncActDict.Keys(ActFunc.SelectedIndex)
                If scData.FuncActDict(SelectButton.act).DatType <> SCDatFiles.DatFiles.None Then
                    ActVal.Visibility = Visibility.Visible
                    ActVal.Init(scData.FuncActDict(SelectButton.act).DatType, Tool.GetText("BtnActVal"), SelectButton.actval)
                Else
                    ActVal.Visibility = Visibility.Hidden
                End If
                ActFuncText.Text = ActFunc.SelectedIndex
                PreviewDraw()
            End If
        End If
    End Sub

    Private Sub ActVal_ValueChange(sender As Object, e As RoutedEventArgs)
        If SelectButton IsNot Nothing Then
            SelectButton.actval = ActVal.Value
            PreviewDraw()
        End If
    End Sub

    Private Sub ValueSelecter_ValueChange(sender As Object, e As RoutedEventArgs)
        TypeSelecterPreviewReset()
    End Sub

    Private Sub ButtonList_MouseEnter(sender As Object, e As MouseEventArgs)
        If buttonList.Items.Count <> ButtonSet.ButtonS.Count Then
            ButtonListReset()
        Else
            Dim flag As Boolean = False
            For i = 0 To buttonList.Items.Count - 1
                Dim Listboxitem As ListBoxItem = buttonList.Items(i)



                If Listboxitem.Tag IsNot ButtonSet.ButtonS(i) Then
                    flag = True
                    Exit For
                End If

            Next
            If flag Then
                ButtonListReset()
            End If
        End If
    End Sub

    Private Sub ConFunc_ValueChanged(sender As Object, e As TextChangedEventArgs)
        If IsNumeric(ConFuncText.Text) Then
            ConFunc.SelectedIndex = ConFuncText.Text
        End If
    End Sub

    Private Sub ActsFunc_ValueChanged(sender As Object, e As TextChangedEventArgs)
        If IsNumeric(ActFuncText.Text) Then
            ActFunc.SelectedIndex = ActFuncText.Text
        End If
    End Sub
End Class
