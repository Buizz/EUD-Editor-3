Imports Dragablz
Imports MaterialDesignThemes.Wpf

Public Class DataEditor
    Private LuaManager As LuaManager

    Private InitIndex As Integer

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
    Public Sub New(tab As TabItem, Optional Page As SCDatFiles.DatFiles = SCDatFiles.DatFiles.None, Optional InitIndex As Integer = 0)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()
        Me.InitIndex = InitIndex

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

        CodeExpander.IsExpanded = True
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


    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Me.DataContext = ProjectControlBinding
        pjData.CodeSelecters.Add(CodeList)
        CodeList.SetFliter(CodeSelecter.ESortType.n123)
        CodeList.ListReset(OpenPage, False, _StartIndex:=InitIndex)

        'Dim asdgfaqwea As Func(Of TabItem) = AddressOf dsafads
        'MainTab.NewItemFactory = asdgfaqwea

        If WindowOpenType = OpenType.Drag Then
            Me.Width = 740 + 450 + 48 + 40
        End If
        Topmost = pgData.Setting(ProgramData.TSetting.DataEditorTopMost)

        CodeList.ScrollToSelectItem()
        ControlBar.HotkeyInit(Me)
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

    Private Sub CodeExpander_Expanded(sender As Object, e As RoutedEventArgs)
        CodeList.ScrollToSelectItem()
    End Sub
End Class
