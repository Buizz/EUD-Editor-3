Imports Dragablz

Public Class DataEditor
    Public Sub OpenbyMainWindow()
        CodeExpander.IsExpanded = True
        Console.IsExpanded = True
    End Sub

    '데이터 상태랑 선택한 인덱스랑 어떤 페이지인지 알고 있어야 함







    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)


        CodeList.SetFliter(CodeSelecter.ESortType.n123)
        CodeList.ListReset(CodeSelecter.EPageType.Unit, False)
    End Sub







    Private Sub CodeIndexerList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If e.AddedItems.Count <> 0 Then
            Dim index As UInteger = CType(e.AddedItems(0), ListBoxItem).Tag

            Dim TabContent As Dragablz.TabablzControl = MainTab.Content



            Dim TabItem As New TabItem
            Dim TabGrid As New Grid
            Dim TabText As New TextBlock
            Dim TabContextMenu As New ContextMenu
            TabText.Text = "asdsa"
            TabText.Foreground = Application.Current.Resources("IdealForegroundColorBrush")


            Dim tabmenuitem As New MenuItem
            tabmenuitem.Header = "닫기"
            tabmenuitem.Command = TabablzControl.CloseItemCommand



            TabContextMenu.Items.Add(tabmenuitem)




            TabGrid.ContextMenu = TabContextMenu
            TabGrid.Children.Add(TabText)

            TabItem.Header = TabGrid
            TabItem.Content = New UnitData(index)

            TabContent.Items.Add(TabItem)


            'MsgBox(TabContent.Items.Count)



            '<TabItem>
            '        <!-- with context menu -->
            '        <TabItem.Header>
            '            <Grid>
            '                <Grid.ContextMenu>
            '                    <ContextMenu>
            '                        <!--we'll be in a popup, so give dragablz a hint as to what tab header content needs closing -->
            '                        <MenuItem Command = "{x:Static dragablz:TabablzControl.CloseItemCommand}" />
            '                    </ContextMenu>
            '                            </Grid.ContextMenu>
            '                <TextBlock Foreground = "{DynamicResource IdealForegroundColorBrush}" > TAB() No. 3</TextBlock>
            '            </Grid>
            '        </TabItem.Header>
            '        <TextBlock HorizontalAlignment = "Center" VerticalAlignment="Center">I feel Like an ice cold drink</TextBlock>
            '    </TabItem>
        End If

    End Sub

    Private Sub CodeIndexer_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim SelectSender As ListBox = sender

        If SelectSender.SelectedIndex = 8 Then
            CodeList.ListReset(SCDatFiles.DatFiles.button, False)
        Else

            CodeList.ListReset(SelectSender.SelectedIndex, False)
        End If
    End Sub

    Private Sub CodeList_Select(Code As Object, e As RoutedEventArgs)
        Dim CodePage As CodeSelecter.EPageType = Code(0)
        Dim index As Integer = Code(1)


        Select Case CodePage
            Case CodeSelecter.EPageType.Unit
                Dim TabContent As Dragablz.TabablzControl = MainTab.Content

                Dim TabItem As New TabItem
                Dim TabGrid As New Grid
                Dim TabText As New TextBlock
                Dim TabContextMenu As New ContextMenu
                'TabText.Text = pjData.CodeLabel(CodePage, index)
                TabText.Foreground = Application.Current.Resources("IdealForegroundColorBrush")


                Dim myBinding As Binding = New Binding("Name")
                myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.units, index)
                TabText.SetBinding(TextBlock.TextProperty, myBinding)


                Dim tabmenuitem As New MenuItem
                tabmenuitem.Header = "닫기"
                tabmenuitem.Command = TabablzControl.CloseItemCommand

                TabContextMenu.Items.Add(tabmenuitem)

                TabGrid.ContextMenu = TabContextMenu
                TabGrid.Children.Add(TabText)

                TabItem.Header = TabGrid
                TabItem.Content = New UnitData(index)

                TabContent.Items.Add(TabItem)
                TabContent.SelectedItem = TabItem
        End Select

    End Sub

    Dim RightShiftDown As Boolean
    Private Sub ConsoleKeyDown(sender As Object, e As KeyEventArgs) Handles ConsoleText.KeyDown
        If e.Key = Key.RightShift Then
            RightShiftDown = True
        End If
        If e.Key = Key.Return Then
            If RightShiftDown Then
                ConsoleText.SelectedText = vbCrLf
                ConsoleText.CaretIndex += 1
            Else
                ConsoleLog.AppendText(vbCrLf)
                ConsoleLog.AppendText(ConsoleText.Text)

                Try
                    pgData.LuaManager.DoString(ConsoleText.Text)
                Catch ex As Exception
                    ConsoleLog.AppendText(vbCrLf)
                    ConsoleLog.AppendText(ex.Message)
                    'ConsoleLog.AppendText(ex.ToString)
                End Try

                ConsoleLog.ScrollToEnd()
                ConsoleText.Text = ""
            End If
        End If
    End Sub
    Private Sub ConsoleKeyUp(sender As Object, e As KeyEventArgs) Handles ConsoleText.KeyUp
        If e.Key = Key.RightShift Then
            RightShiftDown = False
        End If
    End Sub
End Class
