Partial Public Class ProjectExplorer
    Private DragMoving As Point
    Private FristDragMove As Boolean = False
    Private DragSelect As Boolean = False

    Private DragSelectItem As TreeViewItem

    Private Function CheckDragable() As Boolean
        '선택한 블럭들이 타입이 일치하는지 판단
        '선택한 블럭들 중에 삭제나 이동, 추가가 불가능한 블럭이 있는지 판단
        For i = 0 To SelectItems.Count - 1
            If GetFile(SelectItems(i)).IsTopFolder Or GetFile(SelectItems(i)).FileType = TEFile.EFileType.Setting Then
                Return False
            End If
        Next

        Return True
    End Function

    Private Sub DragWork(FolderNode As TreeViewItem)
        For k = 0 To SelectItems.Count - 1
            If GetFile(SelectItems(k)).FileType = TEFile.EFileType.Setting Then
                Continue For
            End If

            Dim CNode As TreeViewItem = SelectItems(k)
            Dim CFile As TEFile = SelectItems(k).Tag

            Dim PFile As TEFile = CType(CNode.Parent, TreeViewItem).Tag

            If CFile.FileType = TEFile.EFileType.Folder Then
                PFile.FolderRemove(CFile)
            Else
                PFile.FileRemove(CFile)
            End If
            DeleteItem(SelectItems(k))
        Next


        For k = 0 To SelectItems.Count - 1
            If GetFile(SelectItems(k)).FileType = TEFile.EFileType.Setting Then
                Continue For
            End If

            MoveItem(FolderNode, SelectItems(k), IsTop)
        Next
        SortList(FolderNode)
        pjData.SetDirty(True)
        TabItemTool.RefreshExplorer(Me)
    End Sub
    Private Function MoveItem(FolderItem As TreeViewItem, Item As TreeViewItem, Optional IsTop As Boolean = False) As Boolean
        Dim itemC As ItemCollection = FolderItem.Items




        If IsFolder(Item) Then
            Dim CFile As TEFile = Item.Tag
            GetFile(FolderItem).FolderAdd(CFile)

            Dim InsertIndex As Integer = 0

            For i = 0 To itemC.Count - 1
                If CType(CType(itemC(i), TreeViewItem).Tag, TEFile).FileType <> TEFile.EFileType.Folder Then
                    Exit For
                End If
                InsertIndex += 1
            Next
            itemC.Insert(InsertIndex, Item)
        Else
            Dim CFile As TEFile = Item.Tag
            GetFile(FolderItem).FileAdd(CFile)


            itemC.Add(Item)
        End If
        'Dim ItemIndex As Integer = itemC.IndexOf(FriendItem)


        'If IsTop Then
        '    itemC.Insert(ItemIndex, Item)
        'Else
        '    itemC.Insert(ItemIndex + 1, Item)
        'End If


        Return True
    End Function




    Private IsTop As Boolean = False
    Private Sub MainTreeviewItme_PreviewMouseMove(sender As TreeViewItem, e As MouseEventArgs)
        If IsDrag Then
            If Not CheckDragable() Or Not IsSelect Then
                IsDrag = False
                FristDragMove = False
                DragImage.Visibility = Visibility.Collapsed
            End If

            DragSelect = True

            Dim CurrentYpos As Integer = e.GetPosition(sender).Y
            Dim Heigth As Integer = sender.Header.ActualHeight

            If CurrentYpos > Heigth / 2 Then
                IsTop = False
            Else
                IsTop = True
            End If



            DragSelectItem = sender


        Else
            If MouseClick Then
                Dim x As Integer = e.GetPosition(Me).X - DragMoving.X
                Dim y As Integer = e.GetPosition(Me).Y - DragMoving.Y

                If Math.Abs(x) > 5 Or Math.Abs(y) > 5 Then
                    If Not PressCtrl And Not PressShift Then
                        IsDrag = True
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub MainTreeviewItme_PreviewMouseUp(sender As TreeViewItem, e As MouseEventArgs)
        If DragSelect Then
            If sender Is DragSelectItem Then


                If IsFolder(DragSelectItem) Then
                    If SelectItems.IndexOf(DragSelectItem) < 0 Then
                        '드래그 한 곳이 선택한 블럭이 아닐 경우!


                        For i = 0 To SelectItems.Count - 1
                            '드래그 한 곳이 선택한 블럭의 자식이 아닐 경우!
                            If CheckChild(SelectItems(i), DragSelectItem) Then
                                DragWork(sender)
                            End If
                        Next
                    End If
                End If
            End If
        End If
    End Sub



    Private Function CheckChild(parent As TreeViewItem, Child As TreeViewItem) As Boolean
        'Parent의 자식들을 모두 순환!
        For i = 0 To parent.Items.Count - 1
            '자식들 중에 Child가 있으면 False반환!
            If CType(parent.Items(i), TreeViewItem) Is Child Then
                Return False
            End If

            '부모의 자식들에게도 해당 함수 적용
            If Not CheckChild(parent.Items(i), Child) Then
                Return False
            End If
        Next

        Return True
    End Function


    Private Sub MainTreeview_PreviewMouseMove(sender As Object, e As MouseEventArgs)
        If IsDrag Then
            If Not CheckDragable() Or Not IsSelect Then
                IsDrag = False
                FristDragMove = False
                DragImage.Visibility = Visibility.Collapsed
                Exit Sub
            End If


            If Not FristDragMove Then
                CreateDragImage()
                FristDragMove = True
                DragSelect = False
            End If
            DragImage.Visibility = Visibility.Visible

            DragImage.Margin = New Thickness(e.GetPosition(sender).X + 8, e.GetPosition(sender).Y + 8, 0, 0)
        End If
    End Sub
    Private Sub CreateDragImage()



        Draglistview.Items.Clear()

        For i = 0 To SelectItems.Count - 1
            Dim si As New ListBoxItem
            si.Content = GetTreeNodeHeader(SelectItems(i).Tag)
            si.Background = Brushes.Transparent

            Draglistview.Items.Add(si)
        Next
    End Sub
End Class
