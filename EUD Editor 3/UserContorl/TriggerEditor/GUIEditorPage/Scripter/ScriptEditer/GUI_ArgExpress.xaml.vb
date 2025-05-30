Imports MaterialDesignThemes.Wpf

Public Class GUI_ArgExpress
    Private scr As ScriptBlock
    Public Event ArgExpressRefreshEvent As RoutedEventHandler
    '이벤트의 외형이 바뀌었을때 호출됩니다.

    Public Event ArgBtnClickEvent As RoutedEventHandler
    'Value가 선택되었을때 호출됩니다. 인자로 선택한 scr을 보냅니다.

    Private isLoad As Boolean = False
    Public Sub UpdateValue()
        scr.child.Clear()
        ItemPanel.UpdateLayout()
        For i = 0 To ItemPanel.Children.Count - 1
            Dim tchip As Chip = ItemPanel.Children(i)

            scr.AddChild(tchip.Tag)
        Next
    End Sub
    Public Sub CrlInit(_scr As ScriptBlock)
        isLoad = False
        scr = _scr

        ItemPanel.Children.Clear()

        For i = 0 To scr.child.Count - 1
            If scr.child(i).ScriptType = ScriptBlock.EBlockType.sign Then
                InsertChip(-1, NewChipItem(False, scr.child(i).value, scr.child(i)))
            Else
                InsertChip(-1, NewChipItem(True, scr.child(i).ValueCoder, scr.child(i)))
            End If
        Next

        isLoad = True
    End Sub
    Private Sub Chip_PreviewMouseMove(sender As Object, e As MouseEventArgs)
        'Dim chips As Chip = sender

        'Dim cpos As Point = chips.TranslatePoint(New Point(0, 0), maingrid)

        'Pos.Margin = New Thickness(cpos.X, cpos.Y, 0, 0)
    End Sub

    Private unAbleClick As Boolean = False
    Private lsLeft As Boolean
    Private SelectlsLeft As Boolean
    Private SelectIndex As Integer = -1
    Private OverSelectlsLeft As Boolean
    Private OverSelectIndex As Integer = -1

    Private SelectPos As Point
    Private IsDrag As Boolean = False
    Private DragItem As Chip

    Public Sub argBtnRefresh(sender As Object, e As RoutedEventArgs)
        '모든 블럭을 리프레시합니다.
        For i = 0 To ItemPanel.Children.Count - 1
            Dim tchip As Chip = ItemPanel.Children(i)
            Dim cscr As ScriptBlock = tchip.Tag
            tchip.Content = cscr.ValueCoder
        Next
    End Sub

    Private Sub maingrid_PreviewMouseMove(sender As Object, e As MouseEventArgs)
        If IsDrag Then
            Dim mnpos As Point = e.GetPosition(maingrid)
            DragImage.Margin = New Thickness(mnpos.X - SelectPos.X - 4, mnpos.Y - SelectPos.Y - 4, 0, 0)

            DragImage.Visibility = Visibility.Visible

            If e.LeftButton = MouseButtonState.Released Then
                unAbleClick = True
                If ItemPanel.Children(OverSelectIndex) Is DragItem Then
                    DragItem.Opacity = 1
                Else
                    Dim cscr As ScriptBlock = DragItem.Tag
                    Dim nchip As Chip
                    If cscr.ScriptType = ScriptBlock.EBlockType.sign Then
                        nchip = NewChipItem(False, cscr.value, cscr)
                    Else
                        nchip = NewChipItem(True, cscr.ValueCoder, cscr)
                    End If

                    If OverSelectlsLeft Then
                        InsertChip(OverSelectIndex, nchip, True)
                    Else
                        InsertChip(OverSelectIndex + 1, nchip, True)
                    End If



                    ItemPanel.Children.Remove(DragItem)
                End If

                UpdateValue()
                RaiseEvent ArgExpressRefreshEvent(Nothing, New RoutedEventArgs)


                IsDrag = False
                DragImage.Child = Nothing
                DragImage.Visibility = Visibility.Collapsed

                unAbleClick = False
            End If
        End If


        If ItemPanel.Children.Count = 0 Then
            Pos.Visibility = Visibility.Hidden
            Return
        End If
        Pos.Visibility = Visibility.Hidden
        For i = 0 To ItemPanel.Children.Count - 1
            Dim tchip As Chip = ItemPanel.Children(i)
            Dim aw As Integer = tchip.ActualWidth
            Dim ah As Integer = tchip.ActualHeight

            Dim mpos As Point = e.GetPosition(tchip)

            If 0 < mpos.X And mpos.X < aw And
                    0 < mpos.Y And mpos.Y < ah Then
                Dim di As Integer = 0

                If mpos.X < aw / 2 Then
                    '왼쪽 부분
                    lsLeft = True
                Else
                    '오른쪽 부분
                    lsLeft = False
                    di += aw + 8
                End If


                Dim cpos As Point = tchip.TranslatePoint(New Point(0, 0), maingrid)



                Pos.Margin = New Thickness(cpos.X - 7 + di, cpos.Y - 2, 0, 0)


                If Not IsDrag Then
                    If e.LeftButton = MouseButtonState.Pressed Then
                        SelectPos = mpos

                        DragItem = tchip
                        DragItem.Opacity = 0.5
                        Dim cscr As ScriptBlock = tchip.Tag
                        Dim ghostchip As Chip
                        If cscr.ScriptType = ScriptBlock.EBlockType.sign Then
                            ghostchip = NewChipItem(False, tchip.Content, cscr, True)
                        Else
                            ghostchip = NewChipItem(True, tchip.Content, cscr, True)
                        End If
                        ghostchip.IsHitTestVisible = False
                        ghostchip.Focusable = False

                        DragImage.Child = ghostchip

                        SelectlsLeft = lsLeft
                        SelectIndex = i
                        unAbleClick = True
                        IsDrag = True
                    End If
                End If

                Pos.Visibility = Visibility.Visible
                OverSelectlsLeft = lsLeft
                OverSelectIndex = i
                Exit For
            End If
        Next

        If SelectIndex <> -1 Then
            Selecter.Visibility = Visibility.Visible
            SelectPosRefresh()
        Else
            Selecter.Visibility = Visibility.Hidden
        End If

        'Dim cpos As Point = e.GetPosition(sender)

    End Sub


    Private Sub SelectPosRefresh()
        If Not (ItemPanel.Children.Count > SelectIndex) Then
            SelectIndex = -1
            Selecter.Visibility = Visibility.Hidden
        End If
        If SelectIndex = -1 Then
            Return
        End If


        Dim tchip As Chip = ItemPanel.Children(SelectIndex)
        Dim aw As Integer = tchip.ActualWidth
        Dim ah As Integer = tchip.ActualHeight

        Dim di As Integer = 0
        If SelectlsLeft Then
            '왼쪽 부분
        Else
            '오른쪽 부분
            di += aw + 8
        End If


        Dim cpos As Point = tchip.TranslatePoint(New Point(0, 0), maingrid)


        Selecter.Margin = New Thickness(cpos.X - 7 + di, cpos.Y - 2, 0, 0)
    End Sub


    Private Sub maingrid_MouseLeave(sender As Object, e As MouseEventArgs)
        Pos.Visibility = Visibility.Collapsed
    End Sub

    Private Sub maingrid_MouseEnter(sender As Object, e As MouseEventArgs)
        Pos.Visibility = Visibility.Visible
    End Sub

    Private Sub ToolBtnClick(sender As Object, e As RoutedEventArgs)
        Dim btn As Button = sender
        Dim nchip As Chip
        If btn.Tag = "Var" Then
            nchip = NewChipItem(True, "Value", New ScriptBlock(ScriptBlock.EBlockType.constVal, "Number", False, False, "0", scr.Scripter))
        Else
            nchip = NewChipItem(False, btn.Content, New ScriptBlock(ScriptBlock.EBlockType.sign, "Sign", False, False, btn.Content, scr.Scripter))
        End If

        If SelectlsLeft Then
            InsertChip(SelectIndex, nchip)
        Else
            If SelectIndex <> -1 Then
                InsertChip(SelectIndex + 1, nchip)
            Else
                InsertChip(SelectIndex, nchip)
            End If
        End If
        If SelectIndex <> -1 Then
            SelectIndex += 1
        End If

        ItemPanel.UpdateLayout()
        SelectPosRefresh()
    End Sub

    Private Function NewChipItem(isVal As Boolean, Content As String, Scr As ScriptBlock, Optional isOnlyImage As Boolean = False) As Chip
        Dim nchip As New Chip
        If isVal Then
            nchip.Margin = New Thickness(4)
            nchip.IsDeletable = True
            nchip.Foreground = Application.Current.Resources("MaterialDesign.Brush.Primary.Foreground")
            nchip.Background = Application.Current.Resources("MaterialDesign.Brush.Primary")

            nchip.Content = Content

            If Not isOnlyImage Then
                nchip.Tag = Scr
            End If
        Else
            nchip.Margin = New Thickness(4)
            nchip.IsDeletable = True
            nchip.Foreground = Application.Current.Resources("MaterialDesignPaper")
            nchip.Background = Application.Current.Resources("MaterialDesignSnackbarBackground")

            nchip.Content = Content

            If Not isOnlyImage Then
                nchip.Tag = New ScriptBlock(ScriptBlock.EBlockType.sign, "Number", False, False, Content, Scr.Scripter)
            End If
        End If
        If Not isOnlyImage Then
            AddHandler nchip.DeleteClick, AddressOf chipRemove
            AddHandler nchip.Click, AddressOf chipClick
        End If
        Return nchip
    End Function


    Private Sub chipClick(sender As Object, e As RoutedEventArgs)
        If Not unAbleClick Then
            Dim chip As Chip = sender

            RaiseEvent ArgBtnClickEvent(chip.Tag, e)
            RaiseEvent ArgExpressRefreshEvent(Nothing, New RoutedEventArgs)
        End If
    End Sub
    Private Sub chipRemove(sender As Object, e As RoutedEventArgs)
        Dim chip As Chip = sender
        ItemPanel.Children.Remove(chip)
        Pos.Visibility = Visibility.Hidden


        UpdateValue()
        RaiseEvent ArgBtnClickEvent(Nothing, e)
        RaiseEvent ArgExpressRefreshEvent(Nothing, New RoutedEventArgs)
        SelectPosRefresh()
    End Sub



    Private Sub InsertChip(index As Integer, nchip As Chip, Optional IsDrag As Boolean = False)
        If index = -1 Then
            If ItemPanel.Children.Count <> 0 Then
                Dim lastchip As Chip = ItemPanel.Children(ItemPanel.Children.Count - 1)
                If LastSelMarge(lastchip, nchip) Then
                    If isLoad Then
                        RaiseEvent ArgExpressRefreshEvent(Nothing, New RoutedEventArgs)
                    End If
                    Return
                End If
            End If
            ItemPanel.Children.Add(nchip)
            If isLoad And Not IsDrag Then
                UpdateValue()
                RaiseEvent ArgBtnClickEvent(nchip.Tag, New RoutedEventArgs)
                RaiseEvent ArgExpressRefreshEvent(Nothing, New RoutedEventArgs)
            End If
        Else
            If ItemPanel.Children.Count > index Then
                Dim lastchip As Chip = ItemPanel.Children(index)
                If LastSelMarge(lastchip, nchip) Then
                    If isLoad Then
                        RaiseEvent ArgExpressRefreshEvent(Nothing, New RoutedEventArgs)
                    End If
                    Return
                End If
            End If




            ItemPanel.Children.Insert(index, nchip)
            If isLoad And Not IsDrag Then
                UpdateValue()
                RaiseEvent ArgBtnClickEvent(nchip.Tag, New RoutedEventArgs)
                RaiseEvent ArgExpressRefreshEvent(Nothing, New RoutedEventArgs)
            End If
        End If
    End Sub

    Private Function LastSelMarge(lastchip As Chip, nchip As Chip) As Boolean
        Dim lastscr As ScriptBlock = lastchip.Tag
        Dim nscr As ScriptBlock = nchip.Tag


        If nscr.ScriptType = ScriptBlock.EBlockType.sign And lastscr.ScriptType = ScriptBlock.EBlockType.sign Then
            Dim t As String = ""

            If nscr.value = "=" Then
                t = lastscr.value & nscr.value
            ElseIf lastchip.Content = "=" Then
                t = nscr.value & lastscr.value
            Else
                If nscr.value = "&" And lastscr.value = "&" Then
                    lastscr.value = "&&"
                    lastchip.Content = "&&"
                    Return True
                ElseIf nscr.value = "|" And lastscr.value = "|" Then
                    lastscr.value = "||"
                    lastchip.Content = "||"
                    Return True
                End If
            End If
            Select Case t
                Case "==", "<=", ">=", "!=", "+=", "-=", "*=", "/="
                    lastscr.value = t
                    lastchip.Content = t
                    Return True
            End Select
        End If
        Return False
    End Function
End Class
