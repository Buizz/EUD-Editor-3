Partial Public Class TEGUIPage
    Private FristDragMove As Boolean = False
    Private DragSelect As Boolean = False

    Private DragSelectItem As TreeViewItem

    Private Function CheckDragable() As Boolean
        '선택한 블럭들이 타입이 일치하는지 판단
        '선택한 블럭들 중에 삭제나 이동, 추가가 불가능한 블럭이 있는지 판단

        Return True
    End Function


    Private IsTop As Boolean = False
    Private Sub MainTreeviewItme_PreviewMouseMove(sender As TreeViewItem, e As MouseEventArgs)
        If IsDrag Then
            DragSelect = True

            Dim CurrentYpos As Integer = e.GetPosition(sender).Y
            Dim Heigth As Integer = sender.Header.ActualHeight

            If CurrentYpos > Heigth / 2 Then
                IsTop = False
            Else
                IsTop = True
            End If



            DragSelectItem = sender


            log.Text = "한번 클릭함 : "
            For i = 0 To SelectItems.Count - 1
                log.Text = log.Text & SelectItems(i).Tag & ", "
            Next
            log.Text = log.Text & vbCrLf & "현재 선택한 블럭 : " & sender.Tag & "  드래그 완료  IsTop : " & IsTop

        End If
    End Sub
    Private Sub MainTreeviewItme_PreviewMouseUp(sender As TreeViewItem, e As MouseEventArgs)
        If DragSelect Then
            If sender Is DragSelectItem Then
                log.Text = "한번 클릭함 : "

                For i = 0 To SelectItems.Count - 1
                    log.Text = log.Text & SelectItems(i).Tag & ", "
                Next
                log.Text = log.Text & vbCrLf & "현재 선택한 블럭 : " & DragSelectItem.Tag & "  드래그 완료  IsTop : " & IsTop

                If SelectItems.IndexOf(DragSelectItem) < 0 Then
                    '드래그 한 곳이 선택한 블럭이 아닐 경우!


                    For i = 0 To SelectItems.Count - 1
                        '드래그 한 곳이 선택한 블럭의 자식이 아닐 경우!
                        If CheckChild(SelectItems(i), DragSelectItem) Then
                            For k = 0 To SelectItems.Count - 1
                                DeleteItem(SelectItems(k))
                            Next

                            For k = 0 To SelectItems.Count - 1
                                MoveItem(sender, SelectItems(k), IsTop)
                            Next
                        End If
                    Next
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
        log.Text = "한번 클릭함 : "
        For i = 0 To SelectItems.Count - 1
            log.Text = log.Text & SelectItems(i).Tag & ", "
        Next


        DragTreeview.Items.Clear()

        For i = 0 To SelectItems.Count - 1
            Dim si As New TreeViewItem
            si.Header = "임시텍스트" 'SelectItems(i).Tag
            si.Background = Brushes.Transparent

            DragTreeview.Items.Add(si)
        Next
    End Sub
End Class
