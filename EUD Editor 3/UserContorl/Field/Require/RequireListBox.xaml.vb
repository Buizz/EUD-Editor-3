Public Class RequireListBox
    Private DatFile As SCDatFiles.DatFiles
    Private ObjectID As Integer
    Private Parameter As String
    Private RequireList As List(Of CRequireData.RequireBlock) = pjData.ExtraDat.RequireData(DatFile).GetRequireObject(ObjectID)

    Public Sub ListReset()
        Dim RequireTexts As String() = Tool.GetText("RequireText").Split("|")

        MainListBox.Items.Clear()


        For i = 0 To RequireList.Count - 1
            Dim listboxitem As New ListBoxItem
            listboxitem.HorizontalContentAlignment = HorizontalAlignment.Stretch
            listboxitem.VerticalContentAlignment = VerticalAlignment.Stretch

            listboxitem.Padding = New Thickness(-1)

            Dim ListItem As New RequireListBoxItem(RequireList(i))


            listboxitem.Content = ListItem
            '만든 아이템에데가 오브젝트 연결해서 알아서 바뀌도록...


            MainListBox.Items.Add(listboxitem)

            'Dim listboxitem As New ListBoxItem
            'Dim Stackpanle As New StackPanel




            'If RequireList(i).HasValue Then
            '    Dim tb As New TextBlock
            '    tb.Text = RequireList(i).opText & vbCrLf & RequireList(i).Value

            '    Dim ISe As New IconSelecter()


            '    Stackpanle.Children.Add(tb)
            '    Stackpanle.Children.Add(ISe)
            'Else
            '    Dim tb As New TextBlock
            '    tb.Text = RequireList(i).opText

            '    Stackpanle.Children.Add(tb)
            'End If






            'ListBoxItem.Content = Stackpanle
            'MainListBox.Items.Add(listboxitem)
        Next
        Dim Requse As CRequireData.RequireUse = pjData.ExtraDat.RequireData(DatFile).GetRequireUseStatus(ObjectID)

        Select Case Requse
            Case CRequireData.RequireUse.CustomUse
                MainListBox.IsEnabled = True
            Case Else
                MainListBox.IsEnabled = False

        End Select
    End Sub

    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        DataContext = pjData.BindingManager.RequireDataBinding(ObjectID, DatFile)

        RequireList = pjData.ExtraDat.RequireData(DatFile).GetRequireObject(ObjectID)

        ListReset()
    End Sub
    Public Sub ReLoad(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        DataContext = pjData.BindingManager.RequireDataBinding(ObjectID, DatFile)

        RequireList = pjData.ExtraDat.RequireData(DatFile).GetRequireObject(ObjectID)

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



    Private Sub NewItem_Click(sender As Object, e As RoutedEventArgs)
        Dim InsertIndex As Integer
        If MainListBox.SelectedItem Is Nothing Then
            InsertIndex = 0
        Else
            InsertIndex = MainListBox.SelectedIndex
            If Not IsTopSelect Then
                InsertIndex += 1
            End If
        End If




        Dim listboxitem As New ListBoxItem
        listboxitem.HorizontalContentAlignment = HorizontalAlignment.Stretch
        listboxitem.VerticalContentAlignment = VerticalAlignment.Stretch

        listboxitem.Padding = New Thickness(-1)


        Dim tItem As New CRequireData.RequireBlock(CRequireData.EOpCode.Current_unit_is, 0)

        Dim ListItem As New RequireListBoxItem(tItem)


        listboxitem.Content = ListItem
        '만든 아이템에데가 오브젝트 연결해서 알아서 바뀌도록...

        RequireList.Insert(InsertIndex, tItem)
        MainListBox.Items.Insert(InsertIndex, listboxitem)
        pjData.BindingManager.RequireCapacityBinding(DatFile).PropertyChangedPack()
    End Sub


    Private Sub CopyItem_Click(sender As Object, e As RoutedEventArgs)
        Dim Selectindex As Integer = MainListBox.SelectedIndex


        ClipBoard = New CRequireData.RequireBlock(RequireList(Selectindex).opCode, RequireList(Selectindex).Value)
    End Sub

    Private Sub PasteItem_Click(sender As Object, e As RoutedEventArgs)
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

        listboxitem.Padding = New Thickness(-1)


        Dim tItem As New CRequireData.RequireBlock(ClipBoard.opCode, ClipBoard.Value)

        Dim ListItem As New RequireListBoxItem(tItem)


        listboxitem.Content = ListItem
        '만든 아이템에데가 오브젝트 연결해서 알아서 바뀌도록...

        RequireList.Insert(InsertIndex, tItem)
        MainListBox.Items.Insert(InsertIndex, listboxitem)
        pjData.BindingManager.RequireCapacityBinding(DatFile).PropertyChangedPack()
    End Sub

    Private Sub DeleteItem_Click(sender As Object, e As RoutedEventArgs)
        Dim Selectindex As Integer = MainListBox.SelectedIndex


        RequireList.RemoveAt(Selectindex)
        MainListBox.Items.RemoveAt(Selectindex)

        pjData.BindingManager.RequireCapacityBinding(DatFile).PropertyChangedPack()
    End Sub

    Private Sub CutItem_Click(sender As Object, e As RoutedEventArgs)
        Dim Selectindex As Integer = MainListBox.SelectedIndex


        ClipBoard = New CRequireData.RequireBlock(RequireList(Selectindex).opCode, RequireList(Selectindex).Value)


        RequireList.RemoveAt(Selectindex)
        MainListBox.Items.RemoveAt(Selectindex)

        pjData.BindingManager.RequireCapacityBinding(DatFile).PropertyChangedPack()
    End Sub


    Private Sub ContextMenu_Opened(sender As Object, e As RoutedEventArgs)
        If ClipBoard Is Nothing Then
            PasteItem.IsEnabled = False
        Else
            PasteItem.IsEnabled = True
        End If
        If MainListBox.SelectedItem IsNot Nothing Then
            CopyItem.IsEnabled = True
            CutItem.IsEnabled = True
            DeleteItem.IsEnabled = True
        Else
            CopyItem.IsEnabled = False
            CutItem.IsEnabled = False
            DeleteItem.IsEnabled = False
        End If
    End Sub

    Private Sub RadioButton_Checked(sender As Object, e As RoutedEventArgs)
        ListReset()
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

        'If DragSelectItem Is Nothing Then
        '    Exit Sub
        'End If


        ''===========================================================================
        Dim InsertIndex As Integer = MainListBox.SelectedIndex

        If Not IsTopSelect Then
            InsertIndex += 1
        End If


        Dim listboxitem As New ListBoxItem
        listboxitem.HorizontalContentAlignment = HorizontalAlignment.Stretch
        listboxitem.VerticalContentAlignment = VerticalAlignment.Stretch

        listboxitem.Padding = New Thickness(-1)

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

                    listboxitem.Padding = New Thickness(-1)

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

        'If IsDrag Then
        '    If DragSelectItem Is Nothing Then
        '        IsDrag = False
        '        DragImage.Visibility = Visibility.Collapsed
        '        Exit Sub
        '    End If

        '    Dim listboxitem As New ListBoxItem
        '    listboxitem.HorizontalContentAlignment = HorizontalAlignment.Stretch
        '    listboxitem.VerticalContentAlignment = VerticalAlignment.Stretch

        '    listboxitem.Padding = New Thickness(-1)

        '    Dim ListItem As New RequireListBoxItem(RequireList(DragSelectindex))


        '    listboxitem.Content = ListItem

        '    DragImage.Child = listboxitem

        '    IsDragStart = True
        '    DragImage.Visibility = Visibility.Visible
        '    DragImage.Margin = New Thickness(e.GetPosition(MainListBox).X + 5, e.GetPosition(MainListBox).Y + 5, 0, 0)

        '    If MainListBox.SelectedItem IsNot Nothing Then
        '        Dim RealHeight As Integer = CType(MainListBox.SelectedItem, ListBoxItem).ActualHeight
        '        Dim InsertIndex As Integer = MainListBox.SelectedIndex
        '        Dim LastPos As Point = e.GetPosition(MainListBox.SelectedItem)

        '        If LastPos.Y < RealHeight / 2 Then
        '            IsTopSelect = True
        '        Else
        '            IsTopSelect = False
        '        End If
        '    Else
        '        IsTopSelect = True
        '    End If
        'End If
    End Sub


End Class
