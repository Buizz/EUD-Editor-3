Partial Public Class TEGUIPage
    Private PreviewSelectItems As List(Of TreeViewItem)
    Private SelectItems As List(Of TreeViewItem)
    Private PressCtrl As Boolean = False
    Private PressShift As Boolean = False

    Private LastSelectItem As TreeViewItem

    Private OneClick As Integer

    Private IsDrag As Boolean = False


    '컨트롤이 눌렸을때 OneClick수가 존나 크면 저번 선택을 지운다!
    '컨트롤 키가 눌리지 않았을 때 마지막으로 누른 아이템이 중복이었을 경우가 아니면 하나빼고 다지운다.
    Private Sub AddSelectItem(SelectItem As TreeViewItem)
        If SelectItems.IndexOf(SelectItem) < 0 Then '중복 방지
            '컨트롤 또는 쉬프트가 안눌렸으면 다중 선택 방지!
            If Not PressCtrl And Not PressShift Then

                '선택한 아이템이 많을 경우
                If SelectItems.Count > 0 Then
                    SelectListClear()
                End If
            End If



            OneClick += 1
            'log.Text = log.Text & " [" & SelectItem.Header & " " & OneClick & "]"

            If PressCtrl Then
                If OneClick > 1 Then
                    OneClick -= 1
                    SelectListRemoveAt(-1)
                End If
            End If

            '일반 선택 작업
            SelectItems.Add(SelectItem)
            SelectItem.Background = SelectColor()

            LastSelectItem = SelectItem

            If Not PressCtrl And Not PressShift Then
                '선택한 아이템이 많을 경우
                If PreviewSelectItems.IndexOf(SelectItem) >= 0 Then
                    SelectListRecover()
                    LastSelectItem = SelectItem
                End If
            End If
        End If
    End Sub

    Private Sub ListClick(sender As TreeViewItem, e As MouseButtonEventArgs)
        If PressShift Then '쉬프트키가 눌렸을 경우
            If LastSelectItem IsNot Nothing Then '첫번째 선택 블록이 있을 경우
                '현재 누른 아이템이 라스트샐랙트 아이템과 부모가 같은지 판단.
                If LastSelectItem.Parent Is sender.Parent Then
                    Dim RFristSelectItem As TreeViewItem = LastSelectItem

                    Dim ParentItemCollection As ItemCollection
                    If TypeOf LastSelectItem.Parent Is TreeView Then
                        ParentItemCollection = CType(LastSelectItem.Parent, TreeView).Items
                    Else
                        ParentItemCollection = CType(LastSelectItem.Parent, TreeViewItem).Items
                    End If


                    Dim FristSelectIndex As Integer = ParentItemCollection.IndexOf(LastSelectItem)
                    Dim senderSelectIndex As Integer = ParentItemCollection.IndexOf(sender)



                    If FristSelectIndex > senderSelectIndex Then
                        For i = senderSelectIndex To FristSelectIndex
                            AddSelectItem(ParentItemCollection(i))
                        Next
                    Else
                        For i = FristSelectIndex To senderSelectIndex
                            AddSelectItem(ParentItemCollection(i))
                        Next
                    End If
                End If
            End If
        Else
            AddSelectItem(sender)
        End If
    End Sub


    Private Sub MainTreeview_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.LeftCtrl Or e.Key = Key.RightCtrl Then
            PressCtrl = True
        End If
        If e.Key = Key.LeftShift Or e.Key = Key.RightShift Then
            PressShift = True
        End If

        If e.Key = Key.Delete Then
            SelectListDelete()
        End If

        If PressCtrl Or PressShift Then
            MainTreeview.Background = Brushes.LightSkyBlue
        End If
    End Sub

    Private Sub MainTreeview_PreviewKeyUp(sender As Object, e As KeyEventArgs)
        If PressCtrl Or PressShift Then
            MainTreeview.Background = DefaultColor()
        End If

        PressCtrl = False
        PressShift = False
    End Sub

    Private Sub MainTreeview_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        OneClick = 0

        PreviewSelectItems.Clear()
        For i = 0 To SelectItems.Count - 1
            PreviewSelectItems.Add(SelectItems(i))
        Next

        If Not PressCtrl And Not PressShift Then
            If CheckDragable() Then
                IsDrag = True
            End If
        End If
        DragSelect = False
    End Sub

    Private Sub MainTreeview_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        IsDrag = False
        FristDragMove = False
        DragImage.Visibility = Visibility.Collapsed
    End Sub



    Private Sub SelectListDelete()
        For i = 0 To SelectItems.Count - 1
            If DeleteItem(SelectItems(i)) Then
                If SelectItems(i) Is LastSelectItem Then
                    LastSelectItem = Nothing
                End If
            Else
                SelectItems(i).Background = DefaultColor()
            End If
        Next
        SelectItems.Clear()
    End Sub
    Private Sub SelectListRecover()
        SelectListClear()
        For i = 0 To PreviewSelectItems.Count - 1
            If SelectItems.IndexOf(PreviewSelectItems(i)) < 0 Then
                SelectItems.Add(PreviewSelectItems(i))
            End If
            PreviewSelectItems(i).Background = SelectColor()
        Next
    End Sub
    Private Sub SelectListClear()
        For i = 0 To SelectItems.Count - 1
            SelectItems(i).Background = DefaultColor()
        Next
        LastSelectItem = Nothing
        SelectItems.Clear()
    End Sub
    Private Sub SelectListRemoveAt(index As Integer)
        If index >= 0 Then
            If SelectItems.Count > index Then
                SelectItems(index).Background = DefaultColor()

                If LastSelectItem Is SelectItems(index) Then
                    LastSelectItem = Nothing
                End If
                SelectItems.RemoveAt(index)
            End If
        Else
            Dim LastIndex As Integer = SelectItems.Count + index
            If SelectItems.Count > LastIndex And SelectItems.Count > 0 Then
                SelectItems(LastIndex).Background = DefaultColor()

                If LastSelectItem Is SelectItems(LastIndex) Then
                    LastSelectItem = Nothing
                End If
                SelectItems.RemoveAt(LastIndex)
            End If
        End If
    End Sub
End Class
