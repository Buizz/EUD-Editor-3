Imports System.Windows.Threading
Imports Microsoft.WindowsAPICodePack.Dialogs

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
            MenuepsFileView.IsEnabled = True
            MenuCopy.IsEnabled = True
            MenuConnect.IsEnabled = True


            'ToGUI.Visibility = Visibility.Visible
            'ToCUI.Visibility = Visibility.Visible
        Else
            MenuOpen.IsEnabled = False
            MenuExport.IsEnabled = False
            MenuDelete.IsEnabled = False
            MenuCut.IsEnabled = False
            MenuepsFileView.IsEnabled = False
            MenuCopy.IsEnabled = False
            MenuConnect.IsEnabled = False
            'ToGUI.Visibility = Visibility.Collapsed
            'ToCUI.Visibility = Visibility.Collapsed
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
                MenuConnect.Visibility = Visibility.Collapsed
                MenuDisConnect.Visibility = Visibility.Collapsed
                'ToGUI.Visibility = Visibility.Collapsed
                'ToCUI.Visibility = Visibility.Collapsed
                'GUISeparator.Visibility = Visibility.Collapsed
                MenuepsFileView.Visibility = Visibility.Collapsed
            Else
                MenuAdd.Visibility = Visibility.Collapsed
                MenuepsFileView.Visibility = Visibility.Visible

                '커넥트확인
                If SelectItems.Count = 1 And GetFile(LastSelectItem).FileType <> TEFile.EFileType.Setting Then

                    If GetFile(LastSelectItem).Scripter.CheckConnect Then
                        MenuConnect.Visibility = Visibility.Collapsed
                        MenuDisConnect.Visibility = Visibility.Visible
                    Else
                        If GetFile(LastSelectItem).FileType = TEFile.EFileType.GUIEps Or
                                 GetFile(LastSelectItem).FileType = TEFile.EFileType.GUIPy Or
                                 GetFile(LastSelectItem).FileType = TEFile.EFileType.ClassicTrigger Or
                                 GetFile(LastSelectItem).FileType = TEFile.EFileType.SCAScript Then
                            MenuConnect.Visibility = Visibility.Collapsed
                            MenuDisConnect.Visibility = Visibility.Collapsed
                            'ToGUI.Visibility = Visibility.Collapsed
                        Else
                            MenuConnect.Visibility = Visibility.Visible
                            MenuDisConnect.Visibility = Visibility.Collapsed
                            'ToCUI.Visibility = Visibility.Collapsed
                        End If
                    End If
                Else
                    MenuConnect.Visibility = Visibility.Collapsed
                    MenuDisConnect.Visibility = Visibility.Collapsed
                End If
            End If
        Else
            MenuAdd.Visibility = Visibility.Visible
            MenuConnect.Visibility = Visibility.Collapsed
            MenuDisConnect.Visibility = Visibility.Collapsed
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
            If (GetFile(LastSelectItem).IsTopFolder Or GetFile(LastSelectItem).FileType = TEFile.EFileType.Setting) And SelectItems.Count = 1 Then

                If GetFile(LastSelectItem).FileType = TEFile.EFileType.Setting Then
                    MenuOpen.Visibility = Visibility.Visible
                    MenuPaste.Visibility = Visibility.Collapsed
                    Separator1.Visibility = Visibility.Collapsed
                Else
                    MenuOpen.Visibility = Visibility.Collapsed
                    MenuPaste.Visibility = Visibility.Visible
                    Separator1.Visibility = Visibility.Visible
                End If
                MenuCut.Visibility = Visibility.Collapsed
                MenuepsFileView.Visibility = Visibility.Collapsed
                MenuCopy.Visibility = Visibility.Collapsed
                MenuRename.Visibility = Visibility.Collapsed
                MenuExport.Visibility = Visibility.Collapsed
                MenuDelete.Visibility = Visibility.Collapsed
                Separator2.Visibility = Visibility.Collapsed
                MenuConnect.Visibility = Visibility.Collapsed
                'ToGUI.Visibility = Visibility.Collapsed
                'ToCUI.Visibility = Visibility.Collapsed
                'GUISeparator.Visibility = Visibility.Collapsed
            Else
                If IsFolder(LastSelectItem) And SelectItems.Count = 1 Then
                    MenuOpen.Visibility = Visibility.Collapsed
                Else
                    MenuOpen.Visibility = Visibility.Visible
                End If
                MenuCut.Visibility = Visibility.Visible
                'MenuepsFileView.Visibility = Visibility.Visible
                MenuCopy.Visibility = Visibility.Visible
                MenuRename.Visibility = Visibility.Visible
                MenuExport.Visibility = Visibility.Visible
                MenuDelete.Visibility = Visibility.Visible
                Separator1.Visibility = Visibility.Visible
                Separator2.Visibility = Visibility.Visible
                'GUISeparator.Visibility = Visibility.Visible
            End If
        Else
            MenuOpen.Visibility = Visibility.Collapsed
            MenuCut.Visibility = Visibility.Collapsed
            MenuepsFileView.Visibility = Visibility.Collapsed
            MenuCopy.Visibility = Visibility.Collapsed
            MenuRename.Visibility = Visibility.Collapsed
            MenuExport.Visibility = Visibility.Collapsed
            MenuDelete.Visibility = Visibility.Collapsed
            Separator2.Visibility = Visibility.Collapsed
            MenuConnect.Visibility = Visibility.Collapsed
            'ToGUI.Visibility = Visibility.Collapsed
            'ToCUI.Visibility = Visibility.Collapsed
            'GUISeparator.Visibility = Visibility.Collapsed
        End If
    End Sub



    Private Sub MenuConnect_Click(sender As Object, e As RoutedEventArgs)
        If LastSelectItem IsNot Nothing Then
            Dim openFileDialog As System.Windows.Forms.OpenFileDialog = Nothing

            If GetFile(LastSelectItem).FileType = TEFile.EFileType.CUIEps Or
                GetFile(LastSelectItem).FileType = TEFile.EFileType.GUIEps Then
                openFileDialog = New System.Windows.Forms.OpenFileDialog With {
                   .Filter = Tool.GetText("OpenEpsFileFliter"),
                   .Title = Tool.GetText("OpenEpsTitle")
                }
            ElseIf GetFile(LastSelectItem).FileType = TEFile.EFileType.CUIPy Or
                GetFile(LastSelectItem).FileType = TEFile.EFileType.GUIPy Then
                openFileDialog = New System.Windows.Forms.OpenFileDialog With {
                   .Filter = Tool.GetText("OpenPyFileFliter"),
                   .Title = Tool.GetText("OpenPyTitle")
                }
            End If


            If openFileDialog.ShowDialog = Forms.DialogResult.OK Then
                TECloseTabITem(GetFile(LastSelectItem))
                GetFile(LastSelectItem).Scripter.ConnectFile = openFileDialog.FileName
            End If
        End If
    End Sub

    Private Sub MenuDisConnect_Click(sender As Object, e As RoutedEventArgs)
        If LastSelectItem IsNot Nothing Then
            If GetFile(LastSelectItem).IsFile And SelectItems.Count = 1 Then
                GetFile(LastSelectItem).Scripter.ConnectFile = ""
            End If
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
        tAddCUIEps.InputGestures.Add(New KeyGesture(Key.T, ModifierKeys.Alt))
        CommandBindings.Add(New CommandBinding(tAddCUIEps, AddressOf AddCUIEps))
        Dim tAddCT As New RoutedCommand()
        tAddCT.InputGestures.Add(New KeyGesture(Key.U, ModifierKeys.Alt))
        CommandBindings.Add(New CommandBinding(tAddCT, AddressOf AddCT))
        'Dim tAddGUIEps As New RoutedCommand()
        'tAddGUIEps.InputGestures.Add(New KeyGesture(Key.W, ModifierKeys.Control))
        'CommandBindings.Add(New CommandBinding(tAddGUIEps, AddressOf AddGUIEps))


        'Dim tAddCUIPy As New RoutedCommand()
        'tAddCUIPy.InputGestures.Add(New KeyGesture(Key.Q, ModifierKeys.Alt))
        'CommandBindings.Add(New CommandBinding(tAddCUIPy, AddressOf AddCUIPy))
        'Dim tAddGUIPy As New RoutedCommand()
        'tAddGUIPy.InputGestures.Add(New KeyGesture(Key.Q, ModifierKeys.Control))
        'CommandBindings.Add(New CommandBinding(tAddGUIPy, AddressOf AddGUIPy))


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


    Private Sub MenuDelete_Click(sender As Object, e As RoutedEventArgs)
        DeleteSelectItem()
    End Sub
    Private Sub DeleteSelectItem()
        SelectListDelete()
        pjData.SetDirty(True)
        SaveExpandedStatusExec()
        TabItemTool.RefreshExplorer(Me)
        TERefreshSetting()
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
        TERefreshSetting()
    End Sub
    Private Sub CutItem()
        CopyItem()
        DeleteSelectItem()
        TERefreshSetting()
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
            TERefreshSetting()
        End If
    End Sub
    Private Sub RenameTextBox_Enter(sender As Object, e As KeyEventArgs)
        If RenameTreeview IsNot Nothing Then
            If e.Key = Key.Enter Then
                ChangeToRenameMode(RenameTreeview, False)

                RenameTreeview = Nothing
                TERefreshSetting()
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

    Private Sub ToGUI_Click(sender As Object, e As RoutedEventArgs)
        If LastSelectItem IsNot Nothing Then
            Dim cfile As TEFile = GetFile(LastSelectItem)

            If cfile.FileType = TEFile.EFileType.CUIEps Then
                If cfile.ChagneType() Then
                    LastSelectItem.Header = GetTreeNodeHeader(cfile)
                    pjData.SetDirty(True)
                    TECloseTabITem(cfile)
                End If
            End If
        End If
    End Sub

    Private Sub ToCUI_Click(sender As Object, e As RoutedEventArgs)
        If LastSelectItem IsNot Nothing Then
            Dim cfile As TEFile = GetFile(LastSelectItem)

            If cfile.FileType = TEFile.EFileType.GUIEps Then

                If cfile.ChagneType() Then
                    LastSelectItem.Header = GetTreeNodeHeader(cfile)
                    pjData.SetDirty(True)
                    TECloseTabITem(cfile)
                End If
            End If
        End If
    End Sub

    Private Sub AddCUIEps()
        FileCreate(New TEFile(Tool.GetText("NewEpsScript"), TEFile.EFileType.CUIEps))
        TERefreshSetting()
    End Sub
    Private Sub AddCUIPy()
        FileCreate(New TEFile(Tool.GetText("NewPyScript"), TEFile.EFileType.CUIPy))
        TERefreshSetting()
    End Sub
    Private Sub AddGUIEps()
        FileCreate(New TEFile(Tool.GetText("NewEpsScript"), TEFile.EFileType.GUIEps))
        TERefreshSetting()
    End Sub
    Private Sub AddGUIPy()
        FileCreate(New TEFile(Tool.GetText("NewPyScript"), TEFile.EFileType.GUIPy))
        TERefreshSetting()
    End Sub
    Private Sub AddSCAScript()
        FileCreate(New TEFile(Tool.GetText("NewSCAScript"), TEFile.EFileType.SCAScript))
        TERefreshSetting()
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
    Private Sub SCAScript_Click(sender As Object, e As RoutedEventArgs)
        AddSCAScript()
    End Sub
    Private Sub AddFolderBtn_Click(sender As Object, e As RoutedEventArgs)
        AddFolder(New TEFile(Tool.GetText("NewFolder"), TEFile.EFileType.Folder))
    End Sub


    Private Sub MenuImport_Click(sender As Object, e As RoutedEventArgs)
        Dim openDialog As New Forms.OpenFileDialog()
        openDialog.Title = Tool.GetText("OpenEpsTitle")
        openDialog.Filter = Tool.GetText("OpenEpsFileFliter")
        openDialog.Multiselect = True

        If openDialog.ShowDialog = Forms.DialogResult.OK Then
            For i = 0 To openDialog.FileNames.Count - 1
                Dim tTEFile As New TEFile(openDialog.SafeFileNames(i).Split(".").First, TEFile.EFileType.CUIEps)

                Dim fs As New System.IO.FileStream(openDialog.FileNames(i), IO.FileMode.Open)
                Dim sr As New System.IO.StreamReader(fs)

                CType(tTEFile.Scripter, CUIScriptEditor).StringText = sr.ReadToEnd

                sr.Close()
                fs.Close()

                FileCreate(tTEFile)
            Next
        End If
    End Sub

    Private Sub MenuExport_Click(sender As Object, e As RoutedEventArgs)
        Dim dialog As New CommonOpenFileDialog
        dialog.InitialDirectory = ""
        dialog.IsFolderPicker = True
        If dialog.ShowDialog() = CommonFileDialogResult.Ok Then
            For i = 0 To SelectItems.Count - 1
                ExportFile(GetFile(SelectItems(i)), dialog.FileName)

                If GetFile(SelectItems(i)).FileType = TEFile.EFileType.CUIEps Or
                    GetFile(SelectItems(i)).FileType = TEFile.EFileType.ClassicTrigger Or
                    GetFile(SelectItems(i)).FileType = TEFile.EFileType.SCAScript Then
                    Dim fs As New System.IO.FileStream(dialog.FileName & "\" & GetFile(SelectItems(i)).FileName & ".eps", IO.FileMode.Create)
                    Dim sw As New System.IO.StreamWriter(fs)
                    sw.Write(GetFile(SelectItems(i)).Scripter.GetStringText())

                    sw.Close()
                    fs.Close()
                End If
            Next
        End If
    End Sub

    Private Sub ExportFile(teFile As TEFile, folder As String)
        If teFile.FileType = TEFile.EFileType.CUIEps Or teFile.FileType = TEFile.EFileType.ClassicTrigger Or teFile.FileType = TEFile.EFileType.SCAScript Then
            Dim fs As New System.IO.FileStream(folder & "\" & teFile.FileName & ".eps", IO.FileMode.Create)
            Dim sw As New System.IO.StreamWriter(fs)
            sw.Write(teFile.Scripter.GetStringText())
            sw.Close()
            fs.Close()
        ElseIf teFile.FileType = TEFile.EFileType.Folder Then
            Dim foldername As String = folder & "\" & teFile.FileName

            If Not IO.Directory.Exists(foldername) Then
                IO.Directory.CreateDirectory(foldername)
            End If

            For index = 0 To teFile.FolderCount - 1
                ExportFile(teFile.Folders(index), foldername)
            Next
            For index = 0 To teFile.FileCount - 1
                ExportFile(teFile.Files(index), foldername)
            Next
        End If
    End Sub


    Private Sub MenuEPSView_Click(sender As Object, e As RoutedEventArgs)
        If LastSelectItem IsNot Nothing Then
            pjData.Save()
            ProjectControlBinding.PropertyChangedPack()
            Dim cfile As TEFile = GetFile(LastSelectItem)

            Dim win As New EPSViewer(cfile)

            win.ShowDialog()
        End If
    End Sub
End Class
