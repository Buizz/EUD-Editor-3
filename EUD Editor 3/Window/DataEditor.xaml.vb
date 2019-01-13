Imports Dragablz
Imports MaterialDesignThemes.Wpf

Public Class DataEditor
    Public Sub OpenbyMainWindow()
        Dim TabContent As TabablzControl = MainTab.Content
        Dim mainTah As TabItem = GetTabItem(SCDatFiles.DatFiles.units, 0)
        TabContent.Items.Add(mainTah)
        TabContent.SelectedItem = mainTah
        CodeExpander.IsExpanded = True
        Console.IsExpanded = False
    End Sub

    Public Sub OpenbyMainWindow(tab As TabItem)
        Dim TabContent As TabablzControl = MainTab.Content
        TabContent.Items.Add(tab)
        TabContent.SelectedItem = tab

        CodeExpander.IsExpanded = True
        Console.IsExpanded = False
    End Sub
    '데이터 상태랑 선택한 인덱스랑 어떤 페이지인지 알고 있어야 함






    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        'MsgBox("생성")
        pjData.CodeSelecters.Add(CodeList)
        CodeList.SetFliter(CodeSelecter.ESortType.n123)
        CodeList.ListReset(CodeSelecter.EPageType.Unit, False)

        'Dim asdgfaqwea As Func(Of TabItem) = AddressOf dsafads
        'MainTab.NewItemFactory = asdgfaqwea

    End Sub

    'Private Function dsafads() As TabItem
    '    Dim index As Integer = 20

    '    Dim TabItem As New TabItem
    '    Dim TabGrid As New Grid
    '    Dim TabText As New TextBlock
    '    Dim TabContextMenu As New ContextMenu
    '    'TabText.Text = pjData.CodeLabel(CodePage, index)
    '    TabText.Foreground = Application.Current.Resources("IdealForegroundColorBrush")


    '    Dim myBinding As Binding = New Binding("Name")
    '    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.units, index)
    '    TabText.SetBinding(TextBlock.TextProperty, myBinding)


    '    Dim tabmenuitem As New MenuItem
    '    tabmenuitem.Header = "닫기"
    '    tabmenuitem.Command = TabablzControl.CloseItemCommand

    '    TabContextMenu.Items.Add(tabmenuitem)



    '    TabGrid.ContextMenu = TabContextMenu
    '    TabGrid.Children.Add(TabText)


    '    TabItem.Header = TabGrid
    '    TabItem.Content = New UnitData(index)

    '    Return TabItem
    'End Function


    Private Sub MetroWindow_Closed(sender As Object, e As EventArgs)
        pjData.CodeSelecters.Remove(CodeList)
        'MsgBox("파괴")
    End Sub




    'Private Sub CodeIndexerList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
    '    If e.AddedItems.Count <> 0 Then
    '        Dim index As UInteger = CType(e.AddedItems(0), ListBoxItem).Tag

    '        Dim TabContent As Dragablz.TabablzControl = MainTab



    '        Dim TabItem As New TabItem
    '        Dim TabGrid As New Grid
    '        Dim TabText As New TextBlock
    '        Dim TabContextMenu As New ContextMenu
    '        TabText.Text = "asdsa"
    '        TabText.Foreground = Application.Current.Resources("IdealForegroundColorBrush")


    '        Dim tabmenuitem As New MenuItem
    '        tabmenuitem.Header = "닫기"
    '        tabmenuitem.Command = TabablzControl.CloseItemCommand



    '        TabContextMenu.Items.Add(tabmenuitem)




    '        TabGrid.ContextMenu = TabContextMenu
    '        TabGrid.Children.Add(TabText)

    '        TabItem.Header = TabGrid
    '        TabItem.Content = New UnitData(index)

    '        TabContent.Items.Add(TabItem)


    '        'MsgBox(TabContent.Items.Count)



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
    '    End If

    'End Sub


    Private Sub CodeList_OpenTab(sender As Object, e As RoutedEventArgs)
        Dim index As Integer = sender
        PlusTabItem(CodeList.Page, index)
    End Sub

    Private Sub CodeList_OpenWindow(sender As Object, e As RoutedEventArgs)
        Dim index As Integer = sender
        WindowTabItem(CodeList.Page, index)
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
        Dim CodePage As SCDatFiles.DatFiles = Code(0)
        Dim index As Integer = Code(1)

        ChanageTabItem(CodePage, index)
    End Sub
    Private Sub WindowTabItem(Datfile As SCDatFiles.DatFiles, index As Integer)
        Dim DataEditorForm As New DataEditor
        DataEditorForm.Show()
        DataEditorForm.OpenbyMainWindow(GetTabItem(Datfile, index))
    End Sub
    Private Sub ChanageTabItem(Datfile As SCDatFiles.DatFiles, index As Integer)
        Dim MainContent As Object = MainTab.Content
        While MainContent.GetType <> GetType(TabablzControl)
            Select Case MainContent.GetType
                Case GetType(TabablzControl)
                    Exit While
                Case GetType(Dockablz.Branch)
                    Dim tBranch As Dockablz.Branch = MainContent
                    MainContent = tBranch.FirstItem
            End Select
        End While

        Dim TabContent As TabablzControl = MainContent

        Dim TabItem As TabItem = GetTabItem(Datfile, index)
        If TabContent.Items.Count <> 0 Then
            Dim ChangesTabItem As TabItem = TabContent.Items(0)
            If ChangesTabItem.Content.GetType() = TabItem.Content.GetType() Then '같은거 일 경우
                Dim TGrid As Grid = ChangesTabItem.Header
                Dim TabText As TextBlock = TGrid.Children.Item(0)

                Dim myBinding As Binding = New Binding("Name")
                myBinding.Source = pjData.BindingManager.UIManager(Datfile, index)
                TabText.SetBinding(TextBlock.TextProperty, myBinding)


                ChangesTabItem.Content.ReLoad(Datfile, index)
                TabContent.SelectedItem = ChangesTabItem
            Else
                TabContent.Items.RemoveAt(0)
                TabContent.Items.Insert(0, TabItem)
                TabContent.SelectedItem = TabItem
            End If

        Else
            TabContent.Items.Add(TabItem)
            TabContent.SelectedItem = TabItem
        End If
    End Sub
    Private Sub PlusTabItem(Datfile As SCDatFiles.DatFiles, index As Integer)
        Dim MainContent As Object = MainTab.Content
        While MainContent.GetType <> GetType(TabablzControl)
            Select Case MainContent.GetType
                Case GetType(TabablzControl)
                    Exit While
                Case GetType(Dockablz.Branch)
                    Dim tBranch As Dockablz.Branch = MainContent
                    MainContent = tBranch.FirstItem
            End Select
        End While

        Dim TabContent As TabablzControl = MainContent

        Dim TabItem As TabItem = GetTabItem(Datfile, index)
        TabContent.Items.Add(TabItem)
        TabContent.SelectedItem = TabItem
    End Sub

    Private Function GetTabItem(Datfile As SCDatFiles.DatFiles, index As Integer) As TabItem
        Dim TabItem As New TabItem
        Dim TabGrid As New Grid
        Dim TabText As New TextBlock
        Dim TabContextMenu As New ContextMenu
        'TabText.Text = pjData.CodeLabel(CodePage, index)
        TabText.Foreground = Application.Current.Resources("IdealForegroundColorBrush")
        TabText.HorizontalAlignment = HorizontalAlignment.Center
        TabText.VerticalAlignment = VerticalAlignment.Center





        Dim TabCloseCommand As New TabCloseCommand(TabItem)

        Dim RightCloseMenuItem As New MenuItem
        Dim OtherCloseMenuItem As New MenuItem
        If True Then
            Dim tabmenuitem As New MenuItem
            tabmenuitem.Header = Tool.GetText("TabClose")
            tabmenuitem.Command = TabablzControl.CloseItemCommand

            Dim PIcon As New PackIcon()
            PIcon.Kind = PackIconKind.Close
            tabmenuitem.Icon = PIcon
            TabContextMenu.Items.Add(tabmenuitem)
        End If

        If True Then
            RightCloseMenuItem.Header = Tool.GetText("RightTabsClose")
            RightCloseMenuItem.CommandParameter = TabCloseCommand.CommandType.RightClose
            RightCloseMenuItem.Command = TabCloseCommand

            Dim PIcon As New PackIcon()
            PIcon.Kind = PackIconKind.ArrowExpandRight
            RightCloseMenuItem.Icon = PIcon
            TabContextMenu.Items.Add(RightCloseMenuItem)
        End If

        If True Then
            OtherCloseMenuItem.Header = Tool.GetText("OtherTabsClose")
            OtherCloseMenuItem.CommandParameter = TabCloseCommand.CommandType.OtherClose
            OtherCloseMenuItem.Command = TabCloseCommand

            Dim PIcon As New PackIcon()
            PIcon.Kind = PackIconKind.ArrowSplitVertical
            OtherCloseMenuItem.Icon = PIcon
            TabContextMenu.Items.Add(OtherCloseMenuItem)
        End If

        Dim TabCloseEnabled As New TabCloseEnabled(TabItem, RightCloseMenuItem, OtherCloseMenuItem)
        TabItem.AddHandler(MenuItem.ContextMenuOpeningEvent, New RoutedEventHandler(AddressOf TabCloseEnabled.OpenEvent))

        'TabGrid.Background = Application.Current.Resources("PrimaryHueMidBrush")
        TabGrid.ContextMenu = TabContextMenu
        TabGrid.Height = 34
        TabGrid.Margin = New Thickness(0, -5, 0, -5)
        TabGrid.Children.Add(TabText)


        TabItem.Header = TabGrid


        Dim myBinding As Binding = New Binding("Name")
        Select Case Datfile
            Case SCDatFiles.DatFiles.units
                myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.units, index)
                TabItem.Content = New UnitData(index)
        End Select
        TabText.SetBinding(TextBlock.TextProperty, myBinding)

        Return TabItem
    End Function


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
