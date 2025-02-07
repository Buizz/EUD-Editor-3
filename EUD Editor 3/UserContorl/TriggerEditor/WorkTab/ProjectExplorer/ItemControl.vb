Imports System.ComponentModel
Imports MaterialDesignThemes.Wpf

Partial Public Class ProjectExplorer
    Private Sub SortList(parent As TreeViewItem)
        GetFile(parent).FileSort()
        GetFile(parent).FolderSort()
        parent.Items.IsLiveSorting = True
        parent.Items.SortDescriptions.Add(New SortDescription("ToolTip", ListSortDirection.Ascending))
    End Sub


    Private Sub LoadFromData(parent As ItemCollection, TEFile As TEFile)
        If TEFile Is TEData.PFIles Then
            MainTreeview.Items.Clear()
            LoadTreeviewItem(MainTreeview.Items, TEFile)
            parent = CType(MainTreeview.Items(0), TreeViewItem).Items
        End If

        For i = 0 To TEFile.FolderCount - 1
            'If FliterText <> "" Then
            '    If TEFile.Folders(i).FileName.ToLower.IndexOf(FliterText.ToLower) < 0 Then
            '        Continue For
            '    End If
            'End If
            Dim child As TreeViewItem = LoadTreeviewItem(parent, TEFile.Folders(i))
            LoadFromData(child.Items, TEFile.Folders(i))


            If FliterText <> "" Then
                '만약 Child의 Items가 0이고 Child의 TEfFiles가 0이면 Child죽이기
                If child.Items.Count = 0 Then
                    Dim pparent As TreeViewItem = child.Parent
                    pparent.Items.Remove(child)
                    'MainTreeview.Items.Remove(child)
                End If
            End If
        Next
        For k = 0 To TEFile.FileCount - 1
            If FliterText <> "" Then
                If Not TEFile.Files(k).CheckFliter(FliterText) Then
                    Continue For
                End If
            End If

            LoadTreeviewItem(parent, TEFile.Files(k))
        Next
    End Sub



    Private Sub AddNewFile(parent As Object, FIle As TEFile)
        Dim tItemCollection As ItemCollection = CType(parent.items, ItemCollection)
        Dim FilesParent As TEFile = CType(parent.Tag, TEFile)

        Dim items As TreeViewItem = GetTreeNode(FIle)
        tItemCollection.Add(items)
        FilesParent.FileAdd(FIle)

        items.IsExpanded = True
    End Sub
    Private Sub AddNewFolder(parent As Object, FIle As TEFile)
        Dim tItemCollection As ItemCollection = CType(parent.items, ItemCollection)
        Dim FilesParent As TEFile = CType(parent.Tag, TEFile)

        Dim items As TreeViewItem = GetTreeNode(FIle)

        Dim InsertIndex As Integer = 0

        For i = 0 To tItemCollection.Count - 1
            Dim FileType As TEFile.EFileType = CType(CType(tItemCollection(i), TreeViewItem).Tag, TEFile).FileType

            If FileType <> TEFile.EFileType.Folder And FileType <> TEFile.EFileType.Setting Then
                Exit For
            End If
            InsertIndex += 1
        Next
        tItemCollection.Insert(InsertIndex, items)


        FilesParent.FolderAdd(FIle)

        items.IsExpanded = True
    End Sub


    Private Function LoadTreeviewItem(parent As ItemCollection, FIle As TEFile) As TreeViewItem
        Dim items As TreeViewItem = GetTreeNode(FIle)

        items.IsExpanded = FIle.IsExpanded
        parent.Add(items)
        Return items
    End Function


    Private Function GetTreeNode(FIle As TEFile) As TreeViewItem
        Dim items As New TreeViewItem
        items.Style = Application.Current.Resources("BackGroundTreeViewItem")

        Dim DPanel As DockPanel = GetTreeNodeHeader(FIle)
        items.ToolTip = FIle.GetTooltip
        items.Header = DPanel
        items.Tag = FIle

        AddHandler items.PreviewMouseLeftButtonDown, AddressOf ListClick
        AddHandler items.PreviewMouseMove, AddressOf MainTreeviewItme_PreviewMouseMove
        AddHandler items.PreviewMouseLeftButtonUp, AddressOf MainTreeviewItme_PreviewMouseUp
        AddHandler items.PreviewMouseRightButtonUp, AddressOf MainTreeviewItme_PreviewMouseRightButtonDown
        AddHandler items.PreviewMouseDoubleClick, AddressOf MainTreeviewItme_PreviewMouseDoubleClick

        Return items
    End Function
    Private Function GetTreeNodeHeader(File As TEFile) As DockPanel
        Dim DPanel As New DockPanel
        DPanel.LastChildFill = False

        Dim Icon As New PackIcon

        Select Case File.FileType
            Case TEFile.EFileType.Folder
                If File.IsTopFolder Then
                    Icon.Kind = PackIconKind.LanguageTypescript
                Else
                    Icon.Kind = PackIconKind.Folder
                End If
            Case TEFile.EFileType.CUIPy, TEFile.EFileType.CUIEps
                If File.Scripter.CheckConnect Then
                    Icon.Kind = PackIconKind.PageNextOutline
                Else
                    Icon.Kind = PackIconKind.FormatText
                End If
            Case TEFile.EFileType.GUIEps, TEFile.EFileType.GUIPy
                Icon.Kind = PackIconKind.Application
            Case TEFile.EFileType.Setting
                Icon.Kind = PackIconKind.SettingsOutline
            Case TEFile.EFileType.ClassicTrigger
                Icon.Kind = PackIconKind.FormatListCheckbox
            Case TEFile.EFileType.SCAScript
                Icon.Kind = PackIconKind.ScriptText
            Case TEFile.EFileType.RawText
                Icon.Kind = PackIconKind.Raw
        End Select



        DPanel.Margin = New Thickness(-6)
        DPanel.Children.Add(Icon)

        Dim tTextBlock As New TextBlock
        tTextBlock.Text = File.FileName
        DPanel.Children.Add(tTextBlock)

        Dim tTextBox As New TextBox
        tTextBox.Text = File.FileName
        tTextBox.Margin = New Thickness(0, -4, 0, -4)
        tTextBox.Visibility = Visibility.Hidden
        DPanel.Children.Add(tTextBox)



        AddHandler tTextBox.LostFocus, AddressOf RenameTextBox_LostFocus
        AddHandler tTextBox.PreviewKeyUp, AddressOf RenameTextBox_Enter
        'AddHandler tTextBox.PreviewKeyUp, AddressOf RenameTextBox_Enter

        Return DPanel
    End Function
    Private Sub ChangeToRenameMode(TreeNode As TreeViewItem, Mode As Boolean)
        Dim DPanel As DockPanel = TreeNode.Header

        Dim tTextBlock As TextBlock = DPanel.Children.Item(1)
        Dim tTextBox As TextBox = DPanel.Children.Item(2)
        If Mode Then
            tTextBlock.Visibility = Visibility.Collapsed
            tTextBox.Visibility = Visibility.Visible

            tTextBox.Focus()
            tTextBox.Select(0, tTextBox.Text.Length)
            Keyboard.Focus(tTextBox)
        Else
            tTextBlock.Text = tTextBox.Text

            GetFile(TreeNode).FileName = tTextBox.Text
            GetFile(TreeNode).UIBinding.PropertyChangedPack()

            TreeNode.ToolTip = GetFile(TreeNode).GetTooltip
            TabItemTool.RefreshExplorer(Me)
            pjData.SetDirty(True)
            SortList(TreeNode.Parent)

            tTextBlock.Visibility = Visibility.Visible
            tTextBox.Visibility = Visibility.Hidden
        End If
    End Sub


    Private Function GetFile(Item As TreeViewItem) As TEFile
        Return CType(Item.Tag, TEFile)
    End Function
    Private Function IsFolder(Item As TreeViewItem) As Boolean
        Return (CType(Item.Tag, TEFile).FileType = TEFile.EFileType.Folder)
    End Function
End Class
