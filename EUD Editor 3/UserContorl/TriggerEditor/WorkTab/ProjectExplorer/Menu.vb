Imports System.Windows.Threading

Partial Public Class ProjectExplorer
    Private RenameTreeview As TreeViewItem

    Private Sub ContextMenu_Opened(sender As Object, e As RoutedEventArgs)
        If RenameTreeview IsNot Nothing Then
            ChangeToRenameMode(RenameTreeview, False)
            RenameTreeview = Nothing
        End If

        '선택한 아이템이 있을 경우
        If SelectItems.Count > 0 Then
            MenuOpen.IsEnabled = True
            MenuExport.IsEnabled = True
            MenuDelete.IsEnabled = True
            MenuCut.IsEnabled = True
            MenuCopy.IsEnabled = True
        Else
            MenuOpen.IsEnabled = False
            MenuExport.IsEnabled = False
            MenuDelete.IsEnabled = False
            MenuCut.IsEnabled = False
            MenuCopy.IsEnabled = False
        End If

        '마지막으로 선택한 아이템이 있을 경우
        If LastSelectItem IsNot Nothing Then
            MenuRename.IsEnabled = True
        Else
            MenuRename.IsEnabled = False
        End If


        '마지막으로 선택한 아이템이 폴더 일 경우
        If LastSelectItem IsNot Nothing Then
            If IsFolder(LastSelectItem) Then
                MenuAdd.Visibility = Visibility.Visible
            Else
                MenuAdd.Visibility = Visibility.Collapsed
            End If
        Else
            MenuAdd.Visibility = Visibility.Visible
        End If



        '마지막으로 선택한 아이템이 폴더 일 경우
        If LastSelectItem IsNot Nothing Then
            If IsFolder(LastSelectItem) And CopyItems.Count > 0 Then
                MenuPaste.IsEnabled = True
            Else
                MenuPaste.IsEnabled = False
            End If
        Else
            MenuPaste.IsEnabled = False
        End If


        If LastSelectItem IsNot Nothing Then
            If GetFile(LastSelectItem).IsTopFolder And SelectItems.Count = 1 Then
                MenuOpen.Visibility = Visibility.Collapsed
                MenuCut.Visibility = Visibility.Collapsed
                MenuCopy.Visibility = Visibility.Collapsed
                MenuRename.Visibility = Visibility.Collapsed
                MenuExport.Visibility = Visibility.Collapsed
                MenuDelete.Visibility = Visibility.Collapsed
            Else
                If IsFolder(LastSelectItem) And SelectItems.Count = 1 Then
                    MenuOpen.Visibility = Visibility.Collapsed
                Else
                    MenuOpen.Visibility = Visibility.Visible
                End If
                MenuCut.Visibility = Visibility.Visible
                MenuCopy.Visibility = Visibility.Visible
                MenuRename.Visibility = Visibility.Visible
                MenuExport.Visibility = Visibility.Visible
                MenuDelete.Visibility = Visibility.Visible
            End If
        Else
            MenuOpen.Visibility = Visibility.Collapsed
            MenuCut.Visibility = Visibility.Collapsed
            MenuCopy.Visibility = Visibility.Collapsed
            MenuRename.Visibility = Visibility.Collapsed
            MenuExport.Visibility = Visibility.Collapsed
            MenuDelete.Visibility = Visibility.Collapsed
        End If
    End Sub



    'HotKey Add
    Private Sub AddHotKeys()
        Dim OpenItem As New RoutedCommand()
        OpenItem.InputGestures.Add(New KeyGesture(Key.Enter))
        CommandBindings.Add(New CommandBinding(OpenItem, AddressOf OpenPageFormKey))


        Dim DeleteItem As New RoutedCommand()
        DeleteItem.InputGestures.Add(New KeyGesture(Key.Delete))
        CommandBindings.Add(New CommandBinding(DeleteItem, AddressOf DeleteSelectItem))


        Dim tAddCUIEps As New RoutedCommand()
        tAddCUIEps.InputGestures.Add(New KeyGesture(Key.W, ModifierKeys.Alt))
        CommandBindings.Add(New CommandBinding(tAddCUIEps, AddressOf AddCUIEps))
        Dim tAddGUIEps As New RoutedCommand()
        tAddGUIEps.InputGestures.Add(New KeyGesture(Key.W, ModifierKeys.Control))
        CommandBindings.Add(New CommandBinding(tAddGUIEps, AddressOf AddGUIEps))
        Dim tAddCUIPy As New RoutedCommand()
        tAddCUIPy.InputGestures.Add(New KeyGesture(Key.Q, ModifierKeys.Alt))
        CommandBindings.Add(New CommandBinding(tAddCUIPy, AddressOf AddCUIPy))
        Dim tAddGUIPy As New RoutedCommand()
        tAddGUIPy.InputGestures.Add(New KeyGesture(Key.Q, ModifierKeys.Control))
        CommandBindings.Add(New CommandBinding(tAddGUIPy, AddressOf AddGUIPy))
        Dim tAddFolder As New RoutedCommand()
        tAddFolder.InputGestures.Add(New KeyGesture(Key.F, ModifierKeys.Control))
        CommandBindings.Add(New CommandBinding(tAddFolder, AddressOf NAddFolder))


        Dim tCopyItem As New RoutedCommand()
        tCopyItem.InputGestures.Add(New KeyGesture(Key.C, ModifierKeys.Control))
        CommandBindings.Add(New CommandBinding(tCopyItem, AddressOf CopyItem))
        Dim tPasteItem As New RoutedCommand()
        tPasteItem.InputGestures.Add(New KeyGesture(Key.V, ModifierKeys.Control))
        CommandBindings.Add(New CommandBinding(tPasteItem, AddressOf PasteItem))
        Dim tCutItem As New RoutedCommand()
        tCutItem.InputGestures.Add(New KeyGesture(Key.X, ModifierKeys.Control))
        CommandBindings.Add(New CommandBinding(tCutItem, AddressOf CutItem))


        Dim tRenameItem As New RoutedCommand()
        tRenameItem.InputGestures.Add(New KeyGesture(Key.R, ModifierKeys.Control))
        CommandBindings.Add(New CommandBinding(tRenameItem, AddressOf RenameItem))
    End Sub


    Private Sub DeleteSelectItem()
        SelectListDelete()
        pjData.SetDirty(True)
        SaveExpandedStatusExec()
        TabItemTool.RefreshExplorer(Me)
    End Sub


    Private Sub CopyItem()
        CopyItems.Clear()
        For i = 0 To SelectItems.Count - 1
            If Not GetFile(SelectItems(i)).IsTopFolder Then
                CopyItems.Add(GetFile(SelectItems(i)).Clone)
            End If
        Next
    End Sub
    Private Sub PasteItem()
        For i = 0 To CopyItems.Count - 1
            Select Case CopyItems(i).FileType
                Case TEFile.EFileType.Folder
                    AddFolder(CopyItems(i).Clone)
                Case Else
                    FileCreate(CopyItems(i).Clone)
            End Select
        Next
    End Sub
    Private Sub CutItem()
        CopyItem()
        DeleteSelectItem()
    End Sub




    Private Sub CopyItem_Click(sender As Object, e As RoutedEventArgs)
        CopyItem()
    End Sub

    Private Sub CutItem_Click(sender As Object, e As RoutedEventArgs)
        CutItem()
    End Sub

    Private Sub PasteItem_Click(sender As Object, e As RoutedEventArgs)
        PasteItem()
    End Sub

    Private Sub RenameTextBox_LostFocus(sender As Object, e As RoutedEventArgs)
        If RenameTreeview IsNot Nothing Then
            ChangeToRenameMode(RenameTreeview, False)

            RenameTreeview = Nothing
        End If
    End Sub
    Private Sub RenameTextBox_Enter(sender As Object, e As KeyEventArgs)
        If RenameTreeview IsNot Nothing Then
            If e.Key = Key.Enter Then
                ChangeToRenameMode(RenameTreeview, False)

                RenameTreeview = Nothing
            End If
        End If
    End Sub

    Private Sub FileCreate(tTEFile As TEFile)
        '파일 생성
        If LastSelectItem Is Nothing Then
            AddNewFile(MainTreeview.Items(0), tTEFile)

            'TEData.PFIles.Add(New TEFile("안녕하삼", True))
        Else
            If IsFolder(LastSelectItem) Then '폴더 일 경우
                AddNewFile(LastSelectItem, tTEFile)
                'GetFile(LastSelectItem).Files.Add(New TEFile("안녕하삼", True))
            End If
        End If
        pjData.SetDirty(True)
        SaveExpandedStatusExec()
        TabItemTool.RefreshExplorer(Me)
    End Sub


    Private Sub RenameItem()
        If RenameTreeview IsNot Nothing Then
            ChangeToRenameMode(RenameTreeview, False)
            RenameTreeview = Nothing
            Exit Sub
        End If

        If LastSelectItem IsNot Nothing Then
            If Not GetFile(LastSelectItem).IsTopFolder Then
                RenameTreeview = LastSelectItem
                ChangeToRenameMode(LastSelectItem, True)
            End If
        End If
    End Sub
    Private Sub OpenPageFormKey()
        If Not FliterTextBox.IsFocused Then
            OpenPage()
        End If

    End Sub
    Private Sub OpenPage()
        If RenameTreeview Is Nothing Then
            For i = 0 To SelectItems.Count - 1
                If Not GetFile(SelectItems(i)).IsTopFolder And Not IsFolder(SelectItems(i)) Then
                    ParentWindow.OpenTabItem(SelectItems(i).Tag)
                End If
            Next
        End If
    End Sub


    Private Sub AddCUIEps()
        FileCreate(New TEFile(Tool.GetText("NewEpsScript"), TEFile.EFileType.CUIEps))
    End Sub
    Private Sub AddCUIPy()
        FileCreate(New TEFile(Tool.GetText("NewPyScript"), TEFile.EFileType.CUIPy))
    End Sub
    Private Sub AddGUIEps()
        FileCreate(New TEFile(Tool.GetText("NewEpsScript"), TEFile.EFileType.GUIEps))
    End Sub
    Private Sub AddGUIPy()
        FileCreate(New TEFile(Tool.GetText("NewPyScript"), TEFile.EFileType.GUIPy))
    End Sub


    Private Sub AddFolder(tTEFile As TEFile)
        '선택이 없을 경우
        If LastSelectItem Is Nothing Then
            AddNewFolder(MainTreeview.Items(0), tTEFile)

            'TEData.PFIles.Add(New TEFile("안녕하삼", True))
        Else
            If IsFolder(LastSelectItem) Then '폴더 일 경우
                AddNewFolder(LastSelectItem, tTEFile)
                'GetFile(LastSelectItem).Files.Add(New TEFile("안녕하삼", True))
            End If
        End If

        pjData.SetDirty(True)
        SaveExpandedStatusExec()
        TabItemTool.RefreshExplorer(Me)
    End Sub
    Private Sub NAddFolder()
        AddFolder(New TEFile(Tool.GetText("NewFolder"), TEFile.EFileType.Folder))
    End Sub
    Private Sub MenuOpen_Click(sender As Object, e As RoutedEventArgs)
        OpenPage()
    End Sub
    Private Sub RenameBtn_Click(sender As Object, e As RoutedEventArgs)
        RenameItem()
    End Sub
    Private Sub CUIEps_Click(sender As Object, e As RoutedEventArgs)
        AddCUIEps()
    End Sub
    Private Sub CUIPy_Click(sender As Object, e As RoutedEventArgs)
        AddCUIPy()
    End Sub

    Private Sub GUIEps_Click(sender As Object, e As RoutedEventArgs)
        AddGUIEps()
    End Sub
    Private Sub GUIPy_Click(sender As Object, e As RoutedEventArgs)
        AddGUIPy()
    End Sub
    Private Sub AddFolderBtn_Click(sender As Object, e As RoutedEventArgs)
        AddFolder(New TEFile(Tool.GetText("NewFolder"), TEFile.EFileType.Folder))
    End Sub
End Class
