Imports Microsoft.DwayneNeed.Win32.UrlMon

Public Class GUIScriptEditorUI
    Public TEGUIPage As TEGUIPage


    Public PTEFile As TEFile
    Public Script As GUIScriptEditor
    Private MainItems As List(Of ScriptBlock)

    Public ReadOnly Property GetItems(index As Integer) As ScriptBlock
        Get
            Return MainItems(index)
        End Get
    End Property
    Public Sub RemoveItems(scr As ScriptBlock)
        MainItems.Remove(scr)
        If scr.ScriptType = ScriptBlock.EBlockType.import Then
            Script.ExternLoader()
        End If
    End Sub
    Public ReadOnly Property IndexOfItem(scr As ScriptBlock) As Integer
        Get
            Return MainItems.IndexOf(scr)
        End Get

    End Property
    Public ReadOnly Property ItemCount() As Integer
        Get
            Return MainItems.Count
        End Get
    End Property
    Public Sub AddItems(Scr As ScriptBlock)
        MainItems.Add(Scr)
        If Scr.ScriptType = ScriptBlock.EBlockType.import Then
            Script.ExternLoader()
        End If
    End Sub
    Public Sub InsertItems(index As Integer, Scr As ScriptBlock)
        MainItems.Insert(index, Scr)
        If Scr.ScriptType = ScriptBlock.EBlockType.import Then
            Script.ExternLoader()
        End If
    End Sub


    Private selpos As Integer = 1


    Public Sub SetTEGUIPage(tTEGUIPage As TEGUIPage)
        TEGUIPage = tTEGUIPage
    End Sub
    Public Sub LoadScript(tTEFile As TEFile, mainScrlist As List(Of ScriptBlock))
        PTEFile = tTEFile
        MainItems = mainScrlist

        Script = PTEFile.Scripter


        For i = 0 To ItemCount - 1
            MainTreeview.Items.Add(GetItems(i).GetTreeviewitem)
        Next

    End Sub

    Public Sub Save()
        'Script.SetJsonObject(TestTextbox.Text)
    End Sub



    Private Function IsItemSelected() As Boolean
        Return (MainTreeview.SelectedItem IsNot Nothing)
    End Function
    Public Function GetSelectScriptBlock() As ScriptBlock
        If IsItemSelected() Then
            Return CType(MainTreeview.SelectedItem, TreeViewItem).Tag
        End If
        Return Nothing
    End Function


    Private Sub CopySelectItem()

    End Sub
    Private Sub CutSelectItem()

    End Sub
    Private Sub PasteSelectItem()

    End Sub
    Private Sub tUndoItem()

    End Sub
    Private Sub tRedoItem()

    End Sub
    Private Sub DeleteSelectItem()
        If IsItemSelected() Then
            If SelectedList.Count = 0 Then
                If Not f_DeleteItem(MainTreeview.SelectedItem) Then
                    SnackBarDialog("지울 수 없는 아이템입니다.")
                End If
            Else
                Dim Deleteitem As Integer = 0
                For i = 0 To SelectedList.Count - 1
                    If f_DeleteItem(SelectedList(i - Deleteitem)) Then
                        Deleteitem += 1
                    End If
                Next
                SnackBarDialog(Deleteitem & "개의 아이템이 지워졌습니다.")
            End If


        End If
    End Sub

    Private Sub EditSelectItem()
        If IsItemSelected() Then
            OpenEditWindow(MainTreeview.SelectedItem)
        End If
    End Sub











    Private Sub ContextMenu_Opened(sender As Object, e As RoutedEventArgs)
        RefreshBtn()
    End Sub
    Private Sub RefreshBtn()
        '복사한 항목이 있냐 없냐 판단
        If False Then
            '복사한 아이템이 있을 경우
            PasteItem.IsEnabled = True
            Pastebtn.IsEnabled = True
        Else
            '복사한 아이템이 없을 경우
            PasteItem.IsEnabled = False
            Pastebtn.IsEnabled = False
        End If


        If Not IsItemSelected() Then
            '선택한 아이템이 없을 경우
            CutItem.IsEnabled = False
            Cutbtn.IsEnabled = False
            CopyItem.IsEnabled = False
            Copybtn.IsEnabled = False
            deselectItem.IsEnabled = False
            DeSelectbtn.IsEnabled = False

            EditItem.IsEnabled = False
            Editbtn.IsEnabled = False

            DeleteItem.IsEnabled = False
            Deletebtn.IsEnabled = False
        Else
            '선택한 아이템이 있을 경우
            CutItem.IsEnabled = True
            Cutbtn.IsEnabled = True
            CopyItem.IsEnabled = True
            Copybtn.IsEnabled = True
            deselectItem.IsEnabled = True
            DeSelectbtn.IsEnabled = True

            EditItem.IsEnabled = True
            Editbtn.IsEnabled = True

            DeleteItem.IsEnabled = True
            Deletebtn.IsEnabled = True
        End If



    End Sub

    Private Sub CutItem_Click(sender As Object, e As RoutedEventArgs)
        CutSelectItem()
    End Sub
    Private Sub CopyItem_Click(sender As Object, e As RoutedEventArgs)
        CopySelectItem()
    End Sub
    Private Sub PasteItem_Click(sender As Object, e As RoutedEventArgs)
        PasteSelectItem()
    End Sub
    Private Sub DeleteItem_Click(sender As Object, e As RoutedEventArgs)
        DeleteSelectItem()
    End Sub
    Private Sub UndoItem_Click(sender As Object, e As RoutedEventArgs)
        tUndoItem()
    End Sub
    Private Sub RedoItem_Click(sender As Object, e As RoutedEventArgs)
        tRedoItem()
    End Sub
    Private Sub EditItem_Click(sender As Object, e As RoutedEventArgs)
        EditSelectItem()
    End Sub
    Private Sub deSelectItem_Click(sender As Object, e As RoutedEventArgs)
        tdeSelectItem()
    End Sub

    Private Sub tdeSelectItem()
        Dim tsel As TreeViewItem = MainTreeview.SelectedItem

        If tsel IsNot Nothing Then
            tsel.IsSelected = False
        End If
        For i = 0 To SelectedList.Count - 1
            Dim ttbheader As ScriptTreeviewitem = SelectedList(i).Header
            ttbheader.DeSelectItem()
        Next
        SelectedList.Clear()

        TEGUIPage.ObjectSelector.ValueListRefresh(Nothing)
        'RefreshBtn()
    End Sub

    Private leftCtrlDown As Boolean
    Private leftShiftDown As Boolean
    Private Sub MainTreeview_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If leftCtrlDown Then
            Select Case e.Key
                Case Key.C
                    CopySelectItem()
                Case Key.X
                    CutSelectItem()
                Case Key.V
                    PasteSelectItem()
                Case Key.Z
                    tUndoItem()
                Case Key.R
                    tRedoItem()
            End Select
            Return
        End If


        Select Case e.Key
            Case Key.LeftCtrl
                leftCtrlDown = True
            Case Key.LeftShift
                leftShiftDown = True
            Case Key.Delete
                DeleteSelectItem()
            Case Key.Enter
                EditSelectItem()
            Case Key.Escape
                tdeSelectItem()
        End Select
    End Sub
    Private Sub MainTreeview_PreviewKeyUp(sender As Object, e As KeyEventArgs)
        If e.Key = Key.LeftCtrl Then
            leftCtrlDown = False
        ElseIf e.Key = Key.LeftShift Then
            leftShiftDown = False
        End If
    End Sub
    Private Sub UpSel_Click(sender As Object, e As RoutedEventArgs)
        upbtn.IsEnabled = False
        downbtn.IsEnabled = True

        selpos = 0
    End Sub
    Private Sub DownSel_Click(sender As Object, e As RoutedEventArgs)
        upbtn.IsEnabled = True
        downbtn.IsEnabled = False

        selpos = 1
    End Sub
    Private Sub DeSelectbtn_Click(sender As Object, e As RoutedEventArgs)
        tdeSelectItem()
    End Sub
    Private Sub Editbtn_Click(sender As Object, e As RoutedEventArgs)
        EditSelectItem()
    End Sub
    Private Sub Cutbtn_Click(sender As Object, e As RoutedEventArgs)
        CutSelectItem()
    End Sub
    Private Sub Copybtn_Click(sender As Object, e As RoutedEventArgs)
        CopySelectItem()
    End Sub
    Private Sub Pastebtn_Click(sender As Object, e As RoutedEventArgs)
        PasteSelectItem()
    End Sub
    Private Sub Deletebtn_Click(sender As Object, e As RoutedEventArgs)
        DeleteSelectItem()
    End Sub

    Private lastSelectItem As TreeViewItem
    Private pivotSelectItem As TreeViewItem
    Private Sub MainTreeview_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object))
        RefreshBtn()
        If IsItemSelected() Then
            Dim tb As TreeViewItem = MainTreeview.SelectedItem
            Dim tbheader As ScriptTreeviewitem = tb.Header
            Dim scr As ScriptBlock = tb.Tag

            TEGUIPage.ObjectSelector.ValueListRefresh(scr)

            If leftCtrlDown Then
                If SelectedList.Count = 0 Then
                    tbheader.SelectItem()
                    SelectedList.Add(tb)
                Else
                    If SelectedList.First.Parent Is tb.Parent Then
                        If SelectedList.IndexOf(tb) = -1 Then
                            tbheader.SelectItem()
                            SelectedList.Add(tb)
                        End If
                    End If
                End If

            ElseIf leftShiftDown Then
                If lastSelectItem IsNot Nothing Then
                    If lastSelectItem.Parent Is tb.Parent Then
                        If SelectedList.Count <> 0 Then
                            lastSelectItem = pivotSelectItem

                            For i = 0 To SelectedList.Count - 1
                                Dim ttbheader As ScriptTreeviewitem = SelectedList(i).Header
                                ttbheader.DeSelectItem()
                            Next
                            SelectedList.Clear()
                        Else
                            pivotSelectItem = lastSelectItem
                        End If


                        If tb.Parent.GetType Is GetType(TreeViewItem) Then
                            Dim pnode As TreeViewItem = tb.Parent

                            Dim a1 As Integer = pnode.Items.IndexOf(lastSelectItem)
                            Dim a2 As Integer = pnode.Items.IndexOf(tb)

                            Dim startindex As Integer = Math.Min(a1, a2)
                            Dim endindex As Integer = Math.Max(a1, a2)

                            For i = startindex To endindex
                                Dim tnode As TreeViewItem = pnode.Items(i)
                                Dim th As ScriptTreeviewitem = tnode.Header
                                th.SelectItem()
                                SelectedList.Add(tnode)
                            Next
                        Else
                            Dim pnode As TreeView = tb.Parent

                            Dim a1 As Integer = pnode.Items.IndexOf(lastSelectItem)
                            Dim a2 As Integer = pnode.Items.IndexOf(tb)

                            Dim startindex As Integer = Math.Min(a1, a2)
                            Dim endindex As Integer = Math.Max(a1, a2)

                            For i = startindex To endindex
                                Dim tnode As TreeViewItem = pnode.Items(i)
                                Dim th As ScriptTreeviewitem = tnode.Header
                                th.SelectItem()
                                SelectedList.Add(tnode)
                            Next
                        End If
                    End If
                End If
            Else
                For i = 0 To SelectedList.Count - 1
                    Dim ttbheader As ScriptTreeviewitem = SelectedList(i).Header
                    ttbheader.DeSelectItem()
                Next
                SelectedList.Clear()
            End If
        End If
        lastSelectItem = MainTreeview.SelectedItem
    End Sub
    Private SelectedList As New List(Of TreeViewItem)

    Private sMessageQueue As MaterialDesignThemes.Wpf.SnackbarMessageQueue
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        RefreshBtn()
        sMessageQueue = New MaterialDesignThemes.Wpf.SnackbarMessageQueue
        ErrorSnackbar.MessageQueue = sMessageQueue
    End Sub

    Private Sub MainTreeview_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        If IsItemSelected() Then
            Dim tb As TreeViewItem = MainTreeview.SelectedItem
            Dim scr As ScriptBlock = tb.Tag
            If Not scr.isfolder Then
                OpenEditWindow(MainTreeview.SelectedItem)
            End If
        End If
    End Sub
End Class
