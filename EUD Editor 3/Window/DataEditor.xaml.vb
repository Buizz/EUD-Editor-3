Imports Dragablz

Public Class DataEditor
    Public Sub OpenbyMainWindow()
        CodeExpander.IsExpanded = True
        Console.IsExpanded = True
    End Sub

    '데이터 상태랑 선택한 인덱스랑 어떤 페이지인지 알고 있어야 함







    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)


        CodeList.SetFliter(CodeSelecter.ESortType.n123)
        CodeList.ListReset(CodeSelecter.EPageType.Unit)
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

        CodeList.ListReset(SelectSender.SelectedIndex)
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
                TabText.Text = pjData.UnitName(index)
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
                TabContent.SelectedItem = TabItem
        End Select

    End Sub
End Class
