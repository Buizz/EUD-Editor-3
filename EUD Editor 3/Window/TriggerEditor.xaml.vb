﻿Imports Dragablz
Imports MaterialDesignThemes.Wpf

Public Class TriggerEditor
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Me.DataContext = ProjectControlBinding

        ControlBar.HotkeyInit(Me)

        Topmost = pgData.Setting(ProgramData.TSetting.TriggerEditrTopMost)

        'Dim asdgfaqwea As Func(Of TabItem) = AddressOf Factory
        'MainTabablzControl.NewItemFactory = asdgfaqwea


        'MainTabablzControl.Style = FindResource("MaterialDesignAlternateTabablzControlStyle")


        Explorer.Init(Me)
    End Sub

    Public Sub Undo()
        'MsgBox("다시실행")

    End Sub
    Public Sub Redo()

    End Sub

    Private Sub metroWindow_LostFocus(sender As Object, e As RoutedEventArgs)
        DeactiveTabitems()
    End Sub

    Private Sub metroWindow_Deactivated(sender As Object, e As EventArgs)
        DeactiveTabitems()
    End Sub


    Public Sub DeactiveTabitems()
        For Each win As Window In Application.Current.Windows
            If win.GetType Is GetType(TriggerEditor) Then
                If Not win Is Me Then
                    Exit Sub
                End If
            End If
        Next
        DeactiveLastItem(MainTab)
    End Sub
    Private Sub DeactiveLastItem(TabablzCont As Object)
        While True
            Select Case TabablzCont.GetType
                Case GetType(TabablzControl)
                    Dim tTabablzControl As TabablzControl = TabablzCont
                    For i = 0 To tTabablzControl.Items.Count - 1
                        Dim TabItem As TabItem = tTabablzControl.Items(i)

                        If TypeOf TabItem.Content Is TECUIPage Then
                            Dim TECUIPage As TECUIPage = TabItem.Content
                            TECUIPage.Deactivated()
                        ElseIf TypeOf TabItem.Content Is TESCAScriptPage Then
                            Dim TECUIPage As TESCAScriptPage = TabItem.Content
                            TECUIPage.Deactivated()
                        End If
                        'MsgBox("Save " & i)
                    Next
                    Exit Sub
                Case GetType(Dockablz.Branch)
                    DeactiveLastItem(CType(TabablzCont, Dockablz.Branch).FirstItem)
                    DeactiveLastItem(CType(TabablzCont, Dockablz.Branch).SecondItem)
                    'MsgBox("Save Branch")
                    Exit Sub
                Case GetType(Dockablz.Layout)
                    Dim tLayout As Dockablz.Layout = TabablzCont
                    TabablzCont = tLayout.Content
            End Select
        End While
    End Sub


    Public Sub LoadLastTabItems()
        MainTab.Content = TLoadLastItem(pjData.TEData.LastOpenTabs)
    End Sub
    Private Function TLoadLastItem(TabItems As TriggerEditorData.LastTab) As Control
        '만약 TabItems에 리스트가 있을 경우
        If TabItems.Items.Count <> 0 Then
            Dim tTabablzControl As New TabablzControl

            tTabablzControl.ClosingItemCallback = AddressOf ClosingTabItemHandlerImpl
            tTabablzControl.ShowDefaultCloseButton = True
            tTabablzControl.InterTabController = New InterTabController
            tTabablzControl.InterTabController.Height = 0
            tTabablzControl.InterTabController.Partition = "QuickStart"
            tTabablzControl.Style = FindResource("MaterialDesignAlternateTabablzControlStyle")






            Dim ttBorder As New Border
            ttBorder.Height = 40
            tTabablzControl.HeaderPrefixContent = ttBorder
            For i = 0 To TabItems.Items.Count - 1
                Dim Tabitem As TabItem = Nothing

                Select Case TabItems.Items(i).FileType
                    Case TEFile.EFileType.CUIEps, TEFile.EFileType.CUIPy
                        Tabitem = GetTabItem(New TECUIPage(TabItems.Items(i)), TabItems.Items(i))
                    Case TEFile.EFileType.GUIEps, TEFile.EFileType.GUIPy
                        Tabitem = GetTabItem(New TEGUIPage(TabItems.Items(i)), TabItems.Items(i))
                    Case TEFile.EFileType.ClassicTrigger
                        Tabitem = GetTabItem(New TECTPage(TabItems.Items(i)), TabItems.Items(i))
                    Case TEFile.EFileType.SCAScript
                        Tabitem = GetTabItem(New TESCAScriptPage(TabItems.Items(i)), TabItems.Items(i))
                End Select

                'MsgBox("Load " & i)
                tTabablzControl.Items.Add(Tabitem)
            Next

            Return tTabablzControl
        ElseIf TabItems.FirstItem IsNot Nothing And TabItems.SecondItem IsNot Nothing Then
            Dim tBranch As New Dockablz.Branch
            tBranch.FirstItem = TLoadLastItem(TabItems.FirstItem)
            tBranch.SecondItem = TLoadLastItem(TabItems.SecondItem)
            tBranch.Orientation = TabItems.Orientation
            'MsgBox("Load Branch")

            Return tBranch
        End If
        Dim ttTabablzControl As New TabablzControl
        ttTabablzControl.ClosingItemCallback = AddressOf ClosingTabItemHandlerImpl
        ttTabablzControl.ShowDefaultCloseButton = True
        ttTabablzControl.InterTabController = New InterTabController
        ttTabablzControl.InterTabController.Height = 0
        ttTabablzControl.InterTabController.Partition = "QuickStart"

        Dim tBorder As New Border
        tBorder.Height = 40
        ttTabablzControl.HeaderPrefixContent = tBorder

        Return ttTabablzControl

        'While TypeOf TabablzCont IsNot TabablzControl
        '    Select Case TabablzCont.GetType
        '        Case GetType(TabablzControl)
        '            Dim tTabablzControl As TabablzControl = TabablzCont
        '            TabItems.Items.Clear()
        '            For i = 0 To tTabablzControl.Items.Count - 1
        '                Dim TabItem As TabItem = tTabablzControl.Items(i)

        '                If TypeOf TabItem.Content Is TECUIPage Then
        '                    Dim TECUIPage As TECUIPage = TabItem.Content
        '                    TabItems.Items.Add(TECUIPage.TEFile)
        '                ElseIf TypeOf TabItem.Content Is TEGUIPage Then
        '                    Dim TEGUIPage As TEGUIPage = TabItem.Content
        '                    TabItems.Items.Add(TEGUIPage.TEFile)
        '                End If
        '            Next
        '            Exit Sub
        '        Case GetType(Dockablz.Branch)
        '            TabItems.FristItem = New TriggerEditorData.LastTab
        '            TabItems.SecondItem = New TriggerEditorData.LastTab
        '            TSaveLastItem(TabItems.FristItem, CType(TabablzCont, Dockablz.Branch).FirstItem)
        '            TSaveLastItem(TabItems.SecondItem, CType(TabablzCont, Dockablz.Branch).SecondItem)

        '            Exit Sub
        '        Case GetType(Dockablz.Layout)
        '            Dim tLayout As Dockablz.Layout = TabablzCont
        '            TabablzCont = tLayout.Content
        '    End Select
        'End While
    End Function

    Public Sub SaveLastTabitems()
        For Each win As Window In Application.Current.Windows
            If win.GetType Is GetType(TriggerEditor) Then
                If Not win Is Me Then
                    Exit Sub
                End If
            End If
        Next
        TSaveLastItem(pjData.TEData.LastOpenTabs, MainTab)
    End Sub
    Private Sub TSaveLastItem(TabItems As TriggerEditorData.LastTab, TabablzCont As Object)
        While True
            Select Case TabablzCont.GetType
                Case GetType(TabablzControl)
                    Dim tTabablzControl As TabablzControl = TabablzCont
                    TabItems.Items.Clear()
                    For i = 0 To tTabablzControl.Items.Count - 1
                        Dim TabItem As TabItem = tTabablzControl.Items(i)

                        If TypeOf TabItem.Content Is TECUIPage Then
                            Dim TECUIPage As TECUIPage = TabItem.Content
                            TabItems.Items.Add(TECUIPage.TEFile)
                        ElseIf TypeOf TabItem.Content Is TEGUIPage Then
                            Dim TEGUIPage As TEGUIPage = TabItem.Content
                            TabItems.Items.Add(TEGUIPage.TEFile)
                        ElseIf TypeOf TabItem.Content Is TECTPage Then
                            Dim TECTPage As TECTPage = TabItem.Content
                            TabItems.Items.Add(TECTPage.TEFile)
                        ElseIf TypeOf TabItem.Content Is TESCAScriptPage Then
                            Dim TECTPage As TESCAScriptPage = TabItem.Content
                            TabItems.Items.Add(TECTPage.TEFile)
                        End If
                        'MsgBox("Save " & i)
                    Next
                    Exit Sub
                Case GetType(Dockablz.Branch)
                    TabItems.FirstItem = New TriggerEditorData.LastTab
                    TabItems.SecondItem = New TriggerEditorData.LastTab
                    TabItems.Orientation = CType(TabablzCont, Dockablz.Branch).Orientation
                    TSaveLastItem(TabItems.FirstItem, CType(TabablzCont, Dockablz.Branch).FirstItem)
                    TSaveLastItem(TabItems.SecondItem, CType(TabablzCont, Dockablz.Branch).SecondItem)
                    'MsgBox("Save Branch")
                    Exit Sub
                Case GetType(Dockablz.Layout)
                    Dim tLayout As Dockablz.Layout = TabablzCont
                    TabablzCont = tLayout.Content
            End Select
        End While
    End Sub


    Private NoTabItemClose As Boolean = False
    Private Sub ClosingTabItemHandlerImpl(ByVal args As ItemActionCallbackArgs(Of TabablzControl))
        NoTabItemClose = True

        Dim tTabablzControl As TabItem = args.DragablzItem.Content
        If TypeOf tTabablzControl.Content Is TECUIPage Then
            Dim TECUIPage As TECUIPage = tTabablzControl.Content
            If TECUIPage.NewTextEditor.Visibility = Visibility.Visible Then

                TECUIPage.NewTextEditor.Dispose()
            End If
        End If

    End Sub
    Private Sub MetroWindow_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        If NoTabItemClose Then
            NoTabItemClose = False
            Dim flag As Boolean = False
            For Each win As Window In Application.Current.Windows
                If win.GetType Is GetType(TriggerEditor) And win IsNot Me Then
                    flag = True
                    Exit For
                End If
            Next
            If Not flag Then

                e.Cancel = True
                Exit Sub
            End If
        End If
        SaveLastTabitems()
    End Sub

    'Public Function Factory() As TabItem
    '    MsgBox("아")
    '    Return New TabItem
    'End Function


    Public Sub OpenTabItem(tTEFile As TEFile, Optional LineHighlight As String = "", Optional StartOffset As Integer = 0)
        '모든 Winddow및 TEControl을 뒤져서 중복으로 켜져있는지 확인 후
        '중복되어 있으면 해당 파일을 Activate시킨다.
        Dim Line As Integer = -1
        If IsNumeric(LineHighlight) Then
            Line = LineHighlight
        End If
        If tTEFile Is Nothing Then
            Return
        End If

        If CheckActvateTab(tTEFile, Line) Then
            Exit Sub
        End If

        Select Case tTEFile.FileType
            Case TEFile.EFileType.CUIEps, TEFile.EFileType.CUIPy
                PlusTabItem(New TECUIPage(tTEFile, Line, StartOffset), MainTab, tTEFile, Line)
            Case TEFile.EFileType.GUIEps, TEFile.EFileType.GUIPy
                PlusTabItem(New TEGUIPage(tTEFile), MainTab, tTEFile)
            Case TEFile.EFileType.Setting
                PlusTabItem(New TriggerEditorSetting(tTEFile), MainTab, tTEFile)
            Case TEFile.EFileType.ClassicTrigger
                PlusTabItem(New TECTPage(tTEFile, Line), MainTab, tTEFile, Line)
            Case TEFile.EFileType.SCAScript
                PlusTabItem(New TESCAScriptPage(tTEFile, Line), MainTab, tTEFile, Line)
            Case TEFile.EFileType.RawText
                PlusTabItem(New TETextEditorPage(tTEFile, Line), MainTab, tTEFile, Line)
        End Select
    End Sub






    Private Function CheckActvateTab(tTEFile As TEFile, Optional SelectLine As Integer = -1) As Boolean
        '우선 모든 윈도우 돌면서 조사하자
        For Each win As Window In Application.Current.Windows
            If win.GetType Is GetType(TriggerEditor) Then
                Dim MainContent As Object = CType(win, TriggerEditor).MainTab


                If CheckBranch(tTEFile, MainContent, SelectLine) Then
                    win.Activate()
                    Return True
                End If



                'If CheckTabablzControl(tTEFile, MainContent) Then
                '    win.Activate()
                '    Return True
                'End If
            End If
        Next
        Return False
    End Function
    Private Function CheckBranch(tTEFile As TEFile, ParentBranch As Object, SelectLine As Integer) As Boolean
        '우선 모든 윈도우 돌면서 조사하자
        While TypeOf ParentBranch IsNot TabablzControl
            Select Case ParentBranch.GetType
                Case GetType(TabablzControl)
                    Exit While
                Case GetType(Dockablz.Branch)
                    Dim tBranch As Dockablz.Branch = ParentBranch

                    If CheckBranch(tTEFile, ParentBranch.FirstItem, SelectLine) Then
                        Return True
                    End If
                    If CheckBranch(tTEFile, ParentBranch.SecondItem, SelectLine) Then
                        Return True
                    End If
                    Return False
                    Exit While
                Case GetType(Dockablz.Layout)
                    Dim tLayout As Dockablz.Layout = ParentBranch
                    ParentBranch = tLayout.Content
            End Select
        End While
        Return CheckTabablzControl(tTEFile, ParentBranch, SelectLine)

        Return False
    End Function
    Private Function CheckTabablzControl(tTEFile As TEFile, Control As TabablzControl, SelectLine As Integer) As Boolean
        For i = 0 To Control.Items.Count - 1
            Dim TabContent As Object = CType(Control.Items(i), TabItem).Content

            If TypeOf TabContent Is TECUIPage Then
                Dim tPage As TECUIPage = TabContent

                If tPage.CheckTEFile(tTEFile) Then
                    Control.SelectedIndex = i
                    'tPage.TextEditor.LineHighLight(SelectLine)
                    Return True
                End If
            ElseIf TypeOf TabContent Is TEGUIPage Then
                Dim tPage As TEGUIPage = TabContent

                If tPage.CheckTEFile(tTEFile) Then
                    Control.SelectedIndex = i
                    Return True
                End If
            ElseIf TypeOf TabContent Is TECTPage Then
                Dim tPage As TECTPage = TabContent

                If tPage.CheckTEFile(tTEFile) Then
                    Control.SelectedIndex = i
                    tPage.SelectList(SelectLine)
                    Return True
                End If
            ElseIf TypeOf TabContent Is TESCAScriptPage Then
                Dim tPage As TESCAScriptPage = TabContent

                If tPage.CheckTEFile(tTEFile) Then
                    Control.SelectedIndex = i
                    Return True
                End If
            ElseIf TypeOf TabContent Is TETextEditorPage Then
                Dim tPage As TETextEditorPage = TabContent

                If tPage.CheckTEFile(tTEFile) Then
                    Control.SelectedIndex = i
                    Return True
                End If
            ElseIf TypeOf TabContent Is TriggerEditorSetting Then
                Dim tPage As TriggerEditorSetting = TabContent

                If tPage.CheckTEFile(tTEFile) Then
                    Control.SelectedIndex = i
                    Return True
                End If
            End If
        Next



        Return False
    End Function






    Private Sub PlusTabItem(tTEFileControl As UserControl, MainTab As Dockablz.Layout, tTEFile As TEFile, Optional SelectLine As Integer = -1)
        'MsgBox(tTEFile.FileName)
        If CheckActvateTab(tTEFile, SelectLine) Then
            Exit Sub
        End If



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


        Dim Tabitem As TabItem = GetTabItem(tTEFileControl, tTEFile)

        TabContent.Items.Add(Tabitem)
        TabContent.SelectedItem = Tabitem
    End Sub
    Private Function GetTabItem(tTEFileControl As UserControl, tTEFile As TEFile) As TabItem
        Dim TabItem As New TabItem
        'TabItem.Style = Nothing 'FindResource("MaterialDesignNavigationRailTabItem")
        TabItem.DataContext = tTEFile.UIBinding

        Dim tbind As New Binding
        tbind.Path = New PropertyPath("TabName")
        TabItem.SetBinding(TabItem.HeaderProperty, tbind)


        TabItem.Content = tTEFileControl


        Return TabItem
    End Function



    Private Sub MetroWindow_Closed(sender As Object, e As EventArgs)
        pgData.Setting(ProgramData.TSetting.TriggerEditrTopMost) = Me.Topmost
        CloseToolWindow()
    End Sub

    Private Sub ErrorList_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        If ErrorList.SelectedItem IsNot Nothing Then
            Dim eitem As ErrorItem = ErrorList.SelectedItem

            OpenTabItem(eitem.TargetTEFile, eitem.Line)
        End If
    End Sub


End Class
