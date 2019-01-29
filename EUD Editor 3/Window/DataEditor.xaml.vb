Imports Dragablz
Imports MaterialDesignThemes.Wpf

Public Class DataEditor
    Public Sub OpenbyMainWindow()
        Dim TabContent As TabablzControl = MainTab.Content
        Dim mainTah As TabItem = TabItemTool.GetTabItem(SCDatFiles.DatFiles.units, 0)
        TabContent.Items.Add(mainTah)
        TabContent.SelectedItem = mainTah
        CodeExpander.IsExpanded = True
        Console.IsExpanded = False

    End Sub

    Public Sub OpenbyOthers(tab As TabItem, Optional Page As Integer = 0)
        Dim TabContent As TabablzControl = MainTab.Content
        TabContent.Items.Add(tab)
        TabContent.SelectedItem = tab

        CodeExpander.IsExpanded = False
        Console.IsExpanded = False

        If Page <> 0 Then
            ListReset(Page)
        End If

    End Sub
    '데이터 상태랑 선택한 인덱스랑 어떤 페이지인지 알고 있어야 함




    Private completion As ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        'MsgBox("생성")
        pjData.CodeSelecters.Add(CodeList)
        CodeList.SetFliter(CodeSelecter.ESortType.n123)
        CodeList.ListReset(SCDatFiles.DatFiles.units, False)

        'Dim asdgfaqwea As Func(Of TabItem) = AddressOf dsafads
        'MainTab.NewItemFactory = asdgfaqwea
        AddHandler ConsoleText.TextArea.TextEntering, AddressOf textEditor_TextArea_TextEntering
        AddHandler ConsoleText.TextArea.TextEntered, AddressOf textEditor_TextArea_TextEntered

    End Sub

    Private completionWindow As ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow
    Private Sub textEditor_TextArea_TextEntered(sender As Object, e As TextCompositionEventArgs)
        If (e.Text = ".") Then
            'Open code completion after the user has pressed dot
            completionWindow = New ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow(ConsoleText.TextArea)
            Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = completionWindow.CompletionList.CompletionData
            data.Add(New MyCompletionData("Item1"))
            data.Add(New MyCompletionData("Item2"))
            data.Add(New MyCompletionData("Item3"))
            completionWindow.Show()
            AddHandler completionWindow.Closed, Sub()
                                                    completionWindow = Nothing
                                                End Sub
        End If



    End Sub

    Private Sub textEditor_TextArea_TextEntering(sender As Object, e As TextCompositionEventArgs)
        If (e.Text.Length > 0 And completionWindow IsNot Nothing) Then
            If (Not Char.IsLetterOrDigit(e.Text(0))) Then

                completionWindow.CompletionList.RequestInsertion(e)
            End If
        End If




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
        TabItemTool.PlusTabItem(CodeList.Page, index, MainTab)
    End Sub

    Private Sub CodeList_OpenWindow(sender As Object, e As RoutedEventArgs)
        Dim index As Integer = sender
        TabItemTool.WindowTabItem(CodeList.Page, index)
    End Sub

    Private Sub CodeIndexer_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim SelectSender As ListBox = sender
        Dim SelectDatType As SCDatFiles.DatFiles = SelectSender.SelectedItem.Tag
        ListReset(SelectDatType)
    End Sub
    Private Sub ListReset(SelectIndex As Integer)
        CodeList.ListReset(SelectIndex, False)
    End Sub


    Private Sub CodeList_Select(Code As Object, e As RoutedEventArgs)
        Dim CodePage As SCDatFiles.DatFiles = Code(0)
        Dim index As Integer = Code(1)

        TabItemTool.ChanageTabItem(CodePage, index, MainTab)
    End Sub



    Private RightShiftDown As Boolean
    Private ClearText As Boolean
    Private Sub ConsoleKeyDown(sender As Object, e As KeyEventArgs) Handles ConsoleText.PreviewKeyDown
        If e.Key = Key.RightShift Then
            RightShiftDown = True
        End If
        If e.Key = Key.Return Then
            If RightShiftDown Then
                'ConsoleText.SelectedText = vbCrLf
                'ConsoleText.SelectionStart += 1
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
                ClearText = True

            End If
        End If
    End Sub
    Private Sub ConsoleKeyUp(sender As Object, e As KeyEventArgs) Handles ConsoleText.KeyUp
        If e.Key = Key.RightShift Then
            RightShiftDown = False
        End If
        If ClearText Then
            ConsoleText.Clear()
            ClearText = False
        End If
    End Sub



    Private LastSize As Integer
    Private Sub CodeExpander_SizeChanged(sender As Object, e As SizeChangedEventArgs)
        If e.WidthChanged Then
            Dim Gap As Integer = e.NewSize.Width - LastSize
            LastSize = e.NewSize.Width

            Me.Width += Gap
        End If
    End Sub
End Class
