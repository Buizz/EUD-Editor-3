Imports Dragablz
Imports MaterialDesignThemes.Wpf

Public Class DataEditor
    Private LuaManager As LuaManager

    Private WindowOpenType As OpenType
    Public Enum OpenType
        MainWindow
        Orders
        Drag
    End Enum

    Private OpenPage As SCDatFiles.DatFiles = SCDatFiles.DatFiles.units
    Public Sub New(OType As OpenType)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        Select Case OType
            Case OpenType.MainWindow
                OpenbyMainWindow()
        End Select
        WindowOpenType = OpenType.MainWindow
    End Sub
    Public Sub New(tab As TabItem, Optional Page As SCDatFiles.DatFiles = SCDatFiles.DatFiles.None)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        WindowOpenType = OpenType.Orders
        OpenbyOthers(tab, Page)
    End Sub
    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        WindowOpenType = OpenType.Drag
    End Sub
    Public Sub CodeViewFold()
        CodeExpander.IsExpanded = Not CodeExpander.IsExpanded
    End Sub



    Private Sub OpenbyMainWindow()
        Dim TabContent As TabablzControl = MainTab.Content
        Dim mainTah As TabItem = TabItemTool.GetTabItem(SCDatFiles.DatFiles.units, 0)
        TabContent.Items.Add(mainTah)
        TabContent.SelectedItem = mainTah
        CodeExpander.IsExpanded = True
        Console.IsExpanded = False

    End Sub

    Private Sub OpenbyOthers(tab As TabItem, Optional Page As SCDatFiles.DatFiles = SCDatFiles.DatFiles.None)
        Dim TabContent As TabablzControl = MainTab.Content
        TabContent.Items.Add(tab)
        TabContent.SelectedItem = tab

        CodeExpander.IsExpanded = False
        Console.IsExpanded = False

        'If Page <> SCDatFiles.DatFiles.None Then
        '    ListReset(Page)
        'End If
        OpenPage = Page
    End Sub
    '데이터 상태랑 선택한 인덱스랑 어떤 페이지인지 알고 있어야 함

    Private LastSize As Integer
    Private Sub CodeExpander_SizeChanged(sender As Object, e As SizeChangedEventArgs)
        If e.WidthChanged Then
            Dim Gap As Integer = e.NewSize.Width - LastSize
            LastSize = e.NewSize.Width

            Me.Width += Gap
        End If
    End Sub


    Private completion As ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Me.DataContext = ProjectControlBinding
        pjData.CodeSelecters.Add(CodeList)
        CodeList.SetFliter(CodeSelecter.ESortType.n123)
        CodeList.ListReset(OpenPage, False)

        'Dim asdgfaqwea As Func(Of TabItem) = AddressOf dsafads
        'MainTab.NewItemFactory = asdgfaqwea
        AddHandler ConsoleText.TextArea.TextEntering, AddressOf textEditor_TextArea_TextEntering
        AddHandler ConsoleText.TextArea.TextEntered, AddressOf textEditor_TextArea_TextEntered
        ConsoleText.Clear()

        LuaManager = New LuaManager(ConsoleLog)
        If WindowOpenType = OpenType.Drag Then
            Me.Width = 740 + 450 + 48 + 40
        End If
        Topmost = pgData.Setting(ProgramData.TSetting.DataEditorTopMost)

        ControlBar.HotkeyInit(Me)
    End Sub

    Private completionWindow As ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow


    Private permissionChar() As String = {" ", "(", ")", "[", "]", "{", "}", vbTab}
    Private Sub ConsoleText_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        Dim inputkey As String = e.Key.ToString

        If inputkey.Length <> 1 Then
            Return
        End If

        Dim flag As Boolean
        If completionWindow Is Nothing Then

            Dim LastSelectStart As Integer = ConsoleText.SelectionStart
            Dim LastSelectLength As Integer = ConsoleText.SelectionLength

            If ConsoleText.SelectionStart > 1 Then
                ConsoleText.SelectionStart -= 1
                ConsoleText.SelectionLength = 1
                'MsgBox(ConsoleText.SelectedText)
                flag = (permissionChar.ToList.IndexOf(ConsoleText.SelectedText) >= 0)
                'MsgBox(ConsoleText.SelectedText)
                ConsoleText.SelectionLength = LastSelectLength
                ConsoleText.SelectionStart = LastSelectStart
            Else
                flag = True
            End If

        Else
            flag = True
        End If


        If completionWindow Is Nothing And LuaManager.CheckExistFunc(inputkey) And flag Then
            completionWindow = New ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow(ConsoleText.TextArea)
            Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = completionWindow.CompletionList.CompletionData

            For i = 0 To LuaManager.Functions.Count - 1
                data.Add(New DataEditCompletionData(LuaManager.Functions(i), LuaManager.ToolTips(i), ConsoleText, DataEditCompletionData.EIconType.Funcname))
            Next
            For i = 0 To LuaManager.Propertys.Count - 1
                data.Add(New DataEditCompletionData(LuaManager.Propertys(i), LuaManager.PropertyToolTips(i), ConsoleText, DataEditCompletionData.EIconType.SettingValue))
            Next
            For i = 0 To LuaManager.LuaKeyWord.Count - 1
                data.Add(New DataEditCompletionData(LuaManager.LuaKeyWord(i), Nothing, ConsoleText, DataEditCompletionData.EIconType.KeyWord))
            Next

            'completionWindow.CompletionList.Foreground = Brushes.Black
            completionWindow.CompletionList.ListBox.Background = Application.Current.Resources("MaterialDesignPaper")
            completionWindow.CompletionList.ListBox.BorderBrush = Application.Current.Resources("MaterialDesignPaper")

            completionWindow.Show()
            completionWindow.CompletionList.SelectItem(inputkey)

            'completionWindow.CompletionList.ListBox.Style = Application.Current.Resources("MaterialDesignToolToggleListBox")
            AddHandler completionWindow.Closed, Sub()
                                                    completionWindow = Nothing
                                                End Sub
        End If
    End Sub



    Private Sub textEditor_TextArea_TextEntered(sender As Object, e As TextCompositionEventArgs)
    End Sub

    Private Sub textEditor_TextArea_TextEntering(sender As Object, e As TextCompositionEventArgs)
        If (e.Text.Length > 0 And completionWindow IsNot Nothing) Then
            If e.Text = vbTab Then
                completionWindow.CompletionList.RequestInsertion(e)
            ElseIf Not Char.IsLetterOrDigit(e.Text(0)) Then
                completionWindow.Close()
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
        pgData.Setting(ProgramData.TSetting.DataEditorTopMost) = Me.Topmost
        CloseToolWindow()

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
    Private Sub ListReset(SelectIndex As SCDatFiles.DatFiles)
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
                If ConsoleText.Text.Trim <> "" Then
                    ConsoleLog.AppendText(vbCrLf)
                    ConsoleLog.AppendText(ConsoleText.Text)

                    Try
                        LuaManager.DoString(ConsoleText.Text)
                    Catch ex As Exception
                        'MsgBox(ex.ToString)
                        ConsoleLog.AppendText(vbCrLf)
                        ConsoleLog.AppendText(ex.Message)
                        'ConsoleLog.AppendText(ex.ToString)
                    End Try

                    ConsoleLog.ScrollToEnd()
                    ConsoleText.Clear()
                    ClearText = True
                Else
                    ConsoleText.Clear()
                    ClearText = True
                End If
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





    Private Sub OpenFucnFolder(sender As Object, e As RoutedEventArgs)
        Process.Start("explorer.exe", "/root," & LuaManager.LuaFloderPath)
    End Sub

    Private Sub refreshLua(sender As Object, e As RoutedEventArgs)
        LuaManager = New LuaManager(ConsoleLog)
    End Sub

    Private Sub LogClear(sender As Object, e As RoutedEventArgs)
        ConsoleLog.Clear()
    End Sub

End Class
