Imports System.Windows.Media.Animation
Imports MaterialDesignThemes.Wpf

Public Class RequireListBox
    Private DatFile As SCDatFiles.DatFiles
    Private ObjectID As Integer
    Private Parameter As String
    Private RequireList As List(Of CRequireData.RequireBlock) = pjData.ExtraDat.RequireData(DatFile).GetRequireBlocks(ObjectID)


    Public Shared NewItemKeyInputCommand As New RoutedUICommand("myCommand", "myCommand", GetType(RequireListBox))
    Public Shared CutItemKeyInputCommand As New RoutedUICommand("myCommand", "myCommand", GetType(RequireListBox))
    Public Shared CopyItemKeyInputCommand As New RoutedUICommand("myCommand", "myCommand", GetType(RequireListBox))
    Public Shared PasteItemKeyInputCommand As New RoutedUICommand("myCommand", "myCommand", GetType(RequireListBox))
    Public Shared DeleteItemKeyInputCommand As New RoutedUICommand("myCommand", "myCommand", GetType(RequireListBox))

    Private Sub __Init()
        Dim items As String() = Tool.GetText("RequireText").Split("|")
        For i = 0 To items.Count - 1
            CodeSelecter.Items.Add(items(i))
        Next
    End Sub


    Private Sub NewItemCommandExecute(ByVal target As Object, ByVal e As ExecutedRoutedEventArgs)
        SNewItem()
    End Sub
    Private Sub CutItemCommandExecute(ByVal target As Object, ByVal e As ExecutedRoutedEventArgs)
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

    Private Sub NewItemCommandCanExecute(ByVal sender As Object, ByVal e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub
    Private Sub CopyCommandCanExecute(ByVal sender As Object, ByVal e As CanExecuteRoutedEventArgs)
        If MainListBox.SelectedItem IsNot Nothing Then
            e.CanExecute = True
        Else
            e.CanExecute = False
        End If
    End Sub
    Private Sub PasteCommandCanExecute(ByVal sender As Object, ByVal e As CanExecuteRoutedEventArgs)
        If ClipBoard Is Nothing Then
            e.CanExecute = False
        Else
            e.CanExecute = True
        End If
    End Sub
    Private Sub CutCommandCanExecute(ByVal sender As Object, ByVal e As CanExecuteRoutedEventArgs)
        If MainListBox.SelectedItem IsNot Nothing Then
            e.CanExecute = True
        Else
            e.CanExecute = False
        End If
    End Sub
    Private Sub DeleteCommandCanExecute(ByVal sender As Object, ByVal e As CanExecuteRoutedEventArgs)
        If MainListBox.SelectedItem IsNot Nothing Then
            e.CanExecute = True
        Else
            e.CanExecute = False
        End If
    End Sub

    Private Function CheckDirty() As Boolean
        If MainListBox.Items.Count <> RequireList.Count Then
            Return True
        Else
            For i = 0 To MainListBox.Items.Count - 1
                Dim listboxitem As RequireListBoxItem = CType(MainListBox.Items(i), ListBoxItem).Content

                If listboxitem.GettRequireData IsNot RequireList(i) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function
    Public Sub ListReset()
        MainListBox.Items.Clear()


        For i = 0 To RequireList.Count - 1
            Dim listboxitem As New ListBoxItem
            listboxitem.HorizontalContentAlignment = HorizontalAlignment.Stretch
            listboxitem.VerticalContentAlignment = VerticalAlignment.Stretch

            listboxitem.Padding = New Thickness(-2)

            Dim ListItem As New RequireListBoxItem(RequireList(i))


            listboxitem.Content = ListItem

            MainListBox.Items.Add(listboxitem)
        Next
        Dim Requse As CRequireData.RequireUse = pjData.ExtraDat.RequireData(DatFile).RequireObjectUsed(ObjectID)

        Select Case Requse
            Case CRequireData.RequireUse.CustomUse
                MainListBox.IsEnabled = True
            Case Else
                MainListBox.IsEnabled = False

        End Select
    End Sub

    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        AnimationInit()
        __Init()

        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        DataContext = pjData.BindingManager.RequireDataBinding(ObjectID, DatFile)

        RequireList = pjData.ExtraDat.RequireData(DatFile).GetRequireBlocks(ObjectID)
        CreateEditWindow.Visibility = Visibility.Hidden
        ListReset()
    End Sub
    Public Sub ReLoad(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        DataContext = pjData.BindingManager.RequireDataBinding(ObjectID, DatFile)

        RequireList = pjData.ExtraDat.RequireData(DatFile).GetRequireBlocks(ObjectID)
        CreateEditWindow.Visibility = Visibility.Hidden
        ListReset()
    End Sub



    Private ClipBoard As CRequireData.RequireBlock


    Private IsTopSelect As Boolean
    Private Sub MainListBox_MouseUp(sender As Object, e As MouseButtonEventArgs)
        If MainListBox.SelectedItem IsNot Nothing Then
            Dim RealHeight As Integer = CType(MainListBox.SelectedItem, ListBoxItem).ActualHeight
            Dim InsertIndex As Integer = MainListBox.SelectedIndex
            Dim LastPos As Point = e.GetPosition(MainListBox.SelectedItem)

            If LastPos.Y < RealHeight / 2 Then
                IsTopSelect = True
            Else
                IsTopSelect = False
            End If
        Else
            IsTopSelect = True
        End If
    End Sub


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




    Private OpenStoryBoard As Storyboard
    Private CloseStoryBoard As Storyboard
    Private Sub NewItem_Click(sender As Object, e As RoutedEventArgs)
        SNewItem()
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

    Private Sub CutItem_Click(sender As Object, e As RoutedEventArgs)
        SCutItem()
    End Sub
    Private Sub SNewItem()
        OpenNewWindow()
    End Sub



    Private Sub SCopyItem()
        Dim Selectindex As Integer = MainListBox.SelectedIndex

        ClipBoard = New CRequireData.RequireBlock(RequireList(Selectindex).opCode, RequireList(Selectindex).Value)
    End Sub

    Private Sub SPasteItem()
        Dim InsertIndex As Integer
        If MainListBox.SelectedItem Is Nothing Then
            InsertIndex = 0
        Else
            InsertIndex = MainListBox.SelectedIndex
        End If


        If Not IsTopSelect Then
            InsertIndex += 1
        End If


        Dim listboxitem As New ListBoxItem
        listboxitem.HorizontalContentAlignment = HorizontalAlignment.Stretch
        listboxitem.VerticalContentAlignment = VerticalAlignment.Stretch

        listboxitem.Padding = New Thickness(-2)


        Dim tItem As New CRequireData.RequireBlock(ClipBoard.opCode, ClipBoard.Value)

        Dim ListItem As New RequireListBoxItem(tItem)


        listboxitem.Content = ListItem
        '만든 아이템에데가 오브젝트 연결해서 알아서 바뀌도록...

        RequireList.Insert(InsertIndex, tItem)
        MainListBox.Items.Insert(InsertIndex, listboxitem)
        pjData.BindingManager.RequireCapacityBinding(DatFile).PropertyChangedPack()
        pjData.SetDirty(True)
    End Sub

    Private Sub SDeleteItem()
        Dim Selectindex As Integer = MainListBox.SelectedIndex


        RequireList.RemoveAt(Selectindex)
        MainListBox.Items.RemoveAt(Selectindex)

        If MainListBox.Items.Count > Selectindex Then
            CType(MainListBox.Items(Selectindex), ListBoxItem).IsSelected = True
        ElseIf MainListBox.Items.Count <> 0 Then
            CType(MainListBox.Items(MainListBox.Items.Count - 1), ListBoxItem).IsSelected = True
        End If


        pjData.BindingManager.RequireCapacityBinding(DatFile).PropertyChangedPack()
        pjData.SetDirty(True)
    End Sub

    Private Sub SCutItem()
        Dim Selectindex As Integer = MainListBox.SelectedIndex


        ClipBoard = New CRequireData.RequireBlock(RequireList(Selectindex).opCode, RequireList(Selectindex).Value)


        RequireList.RemoveAt(Selectindex)
        MainListBox.Items.RemoveAt(Selectindex)

        pjData.BindingManager.RequireCapacityBinding(DatFile).PropertyChangedPack()
        pjData.SetDirty(True)
    End Sub













    Private Sub RadioButton_Checked(sender As Object, e As RoutedEventArgs)
        ListReset()
        pjData.SetDirty(True)
    End Sub

    Private DragSelectindex As Integer
    Private DragSelectItem As ListBoxItem
    Private DragSelctData As CRequireData.RequireBlock
    Private IsClick As Boolean
    Private DragPos As Point
    Private Sub MainListBox_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        DragImage.Margin = New Thickness(e.GetPosition(MainListBox).X, e.GetPosition(MainListBox).Y, 0, 0)

        IsClick = True
    End Sub

    Private Sub MainListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        'If Not IsDragStart And MainListBox.SelectedItem IsNot Nothing Then
        '    DragSelectindex = MainListBox.SelectedIndex
        '    DragSelectItem = MainListBox.SelectedItem
        '    DragSelctData = RequireList(DragSelectindex)
        '    DragImage.Width = DragSelectItem.ActualWidth
        '    DragImage.Height = DragSelectItem.ActualHeight
        'End If
    End Sub



    Private Sub MainListBox_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        IsClick = False
        DragImage.Visibility = Visibility.Collapsed

        If DragSelectItem Is Nothing Then
            Exit Sub
        End If


        ''===========================================================================
        Dim InsertIndex As Integer = MainListBox.SelectedIndex

        If Not IsTopSelect Then
            InsertIndex += 1
        End If


        Dim listboxitem As New ListBoxItem
        listboxitem.HorizontalContentAlignment = HorizontalAlignment.Stretch
        listboxitem.VerticalContentAlignment = VerticalAlignment.Stretch

        listboxitem.Padding = New Thickness(-2)

        Dim tItem As New CRequireData.RequireBlock(DragSelctData.opCode, DragSelctData.Value)

        Dim ListItem As New RequireListBoxItem(tItem)

        listboxitem.Content = ListItem
        '만든 아이템에데가 오브젝트 연결해서 알아서 바뀌도록...

        RequireList.Insert(InsertIndex, tItem)
        MainListBox.Items.Insert(InsertIndex, listboxitem)


        RequireList.Remove(DragSelctData)
        MainListBox.Items.Remove(DragSelectItem)

        listboxitem.IsSelected = True




        DragSelectItem = Nothing
        pjData.BindingManager.RequireCapacityBinding(DatFile).PropertyChangedPack()
        ''===========================================================================
        'If MainListBox.SelectedItem IsNot Nothing Then
        '    DragSelectItem = MainListBox.SelectedItem
        '    DragImage.Width = DragSelectItem.ActualWidth
        '    DragImage.Height = DragSelectItem.ActualHeight
        'End If
    End Sub

    Private Sub MainListBox_PreviewMouseMove(sender As Object, e As MouseEventArgs)
        For i = 0 To MainListBox.Items.Count - 1
            DSelectBorder(i)
        Next
        For i = 0 To MainListBox.Items.Count - 1
            Dim RealHeight As Integer = CType(MainListBox.Items(i), ListBoxItem).ActualHeight
            Dim InsertIndex As Integer = i
            Dim LastPos As Point = e.GetPosition(MainListBox.Items(i))

            If LastPos.Y > 0 And RealHeight / 2 > LastPos.Y Then
                SelectTopBorder(InsertIndex)
                SelectDownBorder(InsertIndex - 1)
            ElseIf LastPos.Y > RealHeight / 2 And RealHeight > LastPos.Y Then
                SelectDownBorder(InsertIndex)
                SelectTopBorder(InsertIndex + 1)
            End If

        Next



        If IsClick Then '만약 클릭 중 일경우
            '아이템이 선택 중일 경우. 그 아이템을 드래그 아이템으로 지정.
            If DragSelectItem Is Nothing Then
                If MainListBox.SelectedItem IsNot Nothing Then '드래그 아이템이 지정되어 있지 않을 경우
                    DragSelectindex = MainListBox.SelectedIndex
                    DragSelectItem = MainListBox.SelectedItem
                    DragSelctData = RequireList(DragSelectindex)
                    DragImage.Width = DragSelectItem.ActualWidth
                    DragImage.Height = DragSelectItem.ActualHeight

                    Dim listboxitem As New ListBoxItem
                    listboxitem.HorizontalContentAlignment = HorizontalAlignment.Stretch
                    listboxitem.VerticalContentAlignment = VerticalAlignment.Stretch

                    listboxitem.Padding = New Thickness(-2)

                    Dim ListItem As New RequireListBoxItem(RequireList(DragSelectindex))


                    listboxitem.Content = ListItem

                    DragImage.Child = listboxitem
                End If
            Else '드래그 중
                DragImage.Visibility = Visibility.Visible
                DragImage.Margin = New Thickness(e.GetPosition(MainListBox).X + 5, e.GetPosition(MainListBox).Y + 5, 0, 0)

                If MainListBox.SelectedItem IsNot Nothing Then
                    Dim RealHeight As Integer = CType(MainListBox.SelectedItem, ListBoxItem).ActualHeight
                    Dim InsertIndex As Integer = MainListBox.SelectedIndex
                    Dim LastPos As Point = e.GetPosition(MainListBox.SelectedItem)

                    If LastPos.Y < RealHeight / 2 Then
                        IsTopSelect = True
                    Else
                        IsTopSelect = False
                    End If
                Else
                    IsTopSelect = True
                End If
            End If
        End If
    End Sub
    Private Sub SelectTopBorder(index As Integer)
        If MainListBox.Items.Count > index And index >= 0 Then
            Dim RListbox As RequireListBoxItem = CType(MainListBox.Items(index), ListBoxItem).Content
            RListbox.SelectTopBorder()
        End If
    End Sub
    Private Sub SelectDownBorder(index As Integer)
        If MainListBox.Items.Count > index And index >= 0 Then
            Dim RListbox As RequireListBoxItem = CType(MainListBox.Items(index), ListBoxItem).Content
            RListbox.SelectDownBorder()
        End If
    End Sub
    Private Sub DSelectBorder(index As Integer)
        If MainListBox.Items.Count > index And index >= 0 Then
            Dim RListbox As RequireListBoxItem = CType(MainListBox.Items(index), ListBoxItem).Content
            RListbox.DSelectBorder()
        End If
    End Sub




    Private Sub Grid_MouseEnter(sender As Object, e As MouseEventArgs)
        If CheckDirty() Then
            ListReset()
        End If
    End Sub

    Private Sub EditItem_Click(sender As Object, e As RoutedEventArgs)
        OpenEditWindow()
    End Sub


    Private IsEditWindowopen As Boolean
    Private Sub OpenNewWindow()
        IsEditWindowopen = False
        OpenStoryBoard.Begin(Me)
        CodeSelecter.SelectedIndex = 2
        CreateEditWindow.Visibility = Visibility.Visible
        IconSelecterRefresh()
        pjData.SetDirty(True)
    End Sub
    Private Sub OpenEditWindow()
        'MsgBox(MainListBox.SelectedIndex)
        'MsgBox(MainListBox.SelectedItems Is Nothing)
        IsEditWindowopen = True
        OpenStoryBoard.Begin(Me)

        Dim CodeData As CRequireData.RequireBlock = RequireList(MenuSeletIndex)

        CodeSelecter.SelectedIndex = CodeData.opCodeIndex
        If CodeData.HasValue Then
            ValueSelecter.Visibility = Visibility.Visible
        Else
            ValueSelecter.Visibility = Visibility.Hidden
        End If
        CreateEditWindow.Visibility = Visibility.Visible
        IconSelecterRefresh()
        pjData.SetDirty(True)
    End Sub

    Private Sub IconSelecterRefresh()
        If IsEditWindowopen Then
            Dim CodeData As CRequireData.RequireBlock = RequireList(MenuSeletIndex)

            If CodeData.opCode = CRequireData.EOpCode.Is_researched Then
                ValueSelecter.Init(SCDatFiles.DatFiles.techdata, "기술", CodeData.Value)
            Else
                ValueSelecter.Init(SCDatFiles.DatFiles.units, "유닛", CodeData.Value)
            End If
        Else
            ValueSelecter.Init(SCDatFiles.DatFiles.units, "유닛", 0)
        End If

    End Sub



    Private Sub OkKey_Click(sender As Object, e As RoutedEventArgs)
        If IsEditWindowopen Then
            Dim SaveIndex As Integer = MenuSeletIndex

            Dim CodeData As CRequireData.RequireBlock = RequireList(MenuSeletIndex)
            Dim ChangeValue As Integer = ValueSelecter.Value


            CodeData.opCode = CodeSelecter.SelectedIndex
            If CRequireData.HasValueOpCode(CodeSelecter.SelectedIndex) Then
                CodeData.Value = ChangeValue
            End If
            Dim SelectItem As ListBoxItem = MainListBox.Items(MenuSeletIndex)
            Dim tItem As New CRequireData.RequireBlock(CodeSelecter.SelectedIndex, ChangeValue)
            Dim ListItem As New RequireListBoxItem(tItem)
            SelectItem.Content = ListItem

            'MainListBox.SelectedIndex = -1
            'SelectItem.IsSelected = False
        Else
            Dim InsertIndex As Integer
            If MainListBox.SelectedItem Is Nothing Then
                InsertIndex = 0
            Else
                InsertIndex = MenuSeletIndex
                If Not IsTopSelect Then
                    InsertIndex += 1
                End If
            End If




            Dim listboxitem As New ListBoxItem
            listboxitem.HorizontalContentAlignment = HorizontalAlignment.Stretch
            listboxitem.VerticalContentAlignment = VerticalAlignment.Stretch

            listboxitem.Padding = New Thickness(-2)
            Dim ChangeValue As Integer = ValueSelecter.Value


            Dim tItem As New CRequireData.RequireBlock(CodeSelecter.SelectedIndex, ChangeValue)

            Dim ListItem As New RequireListBoxItem(tItem)


            listboxitem.Content = ListItem
            '만든 아이템에데가 오브젝트 연결해서 알아서 바뀌도록...

            RequireList.Insert(InsertIndex, tItem)
            MainListBox.Items.Insert(InsertIndex, listboxitem)
            pjData.BindingManager.RequireCapacityBinding(DatFile).PropertyChangedPack()
        End If


        CloseStoryBoard.Begin(Me)
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        CloseStoryBoard.Begin(Me)
    End Sub

    Private Sub CodeSelecter_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If CRequireData.HasValueOpCode(CodeSelecter.SelectedIndex) Then
            ValueSelecter.Visibility = Visibility.Visible

            If CodeSelecter.SelectedIndex = CRequireData.EOpCode.Is_researched Then
                ValueSelecter.Init(SCDatFiles.DatFiles.techdata, "기술", ValueSelecter.Value)
            Else
                ValueSelecter.Init(SCDatFiles.DatFiles.units, "유닛", ValueSelecter.Value)
            End If
        Else
            ValueSelecter.Visibility = Visibility.Hidden
        End If
    End Sub

    Private Sub ContextMenu_Opened(sender As Object, e As RoutedEventArgs)
        If ClipBoard Is Nothing Then
            PasteItem.IsEnabled = False
        Else
            PasteItem.IsEnabled = True
        End If
        If MainListBox.SelectedItem IsNot Nothing Then
            EditItem.IsEnabled = True
            CopyItem.IsEnabled = True
            CutItem.IsEnabled = True
            DeleteItem.IsEnabled = True
        Else
            EditItem.IsEnabled = False
            CopyItem.IsEnabled = False
            CutItem.IsEnabled = False
            DeleteItem.IsEnabled = False
        End If
        MenuSeletIndex = MainListBox.SelectedIndex
        'NewItem.Header = "ContextOpen : " & MainListBox.SelectedIndex
    End Sub
    Private MenuSeletIndex As Integer

    Private Sub MainListBox_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        MenuSeletIndex = MainListBox.SelectedIndex
        If MenuSeletIndex <> -1 Then
            OpenEditWindow()
        End If
    End Sub
End Class