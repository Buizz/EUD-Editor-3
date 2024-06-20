Imports MaterialDesignThemes.Wpf
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
    Public Sub RemoveAtItems(index As Integer)
        If MainItems(index).ScriptType = ScriptBlock.EBlockType.import Then
            Script.ExternLoader()
        End If
        MainItems.RemoveAt(index)
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

        Select Case Scr.ScriptType
            Case ScriptBlock.EBlockType.import, ScriptBlock.EBlockType.vardefine, ScriptBlock.EBlockType.objectdefine, ScriptBlock.EBlockType.fundefine
                ItemFixer(Scr)
                Return
        End Select

        MainItems.Add(Scr)
        If MainItems.First.Parent IsNot Nothing Then
            Scr.Parent = MainItems.First.Parent
        End If
        If Scr.ScriptType = ScriptBlock.EBlockType.import Then
            Script.ExternLoader()
        End If
    End Sub
    Public Sub InsertItems(index As Integer, Scr As ScriptBlock)

        Select Case Scr.ScriptType
            Case ScriptBlock.EBlockType.import, ScriptBlock.EBlockType.vardefine, ScriptBlock.EBlockType.objectdefine, ScriptBlock.EBlockType.fundefine
                If MainItems.Count > index Then
                    If MainItems(index).ScriptType <> Scr.ScriptType Then
                        ItemFixer(Scr)
                        If Scr.ScriptType = ScriptBlock.EBlockType.import Then
                            Script.ExternLoader()
                        End If
                        Return
                    End If
                Else
                    ItemFixer(Scr)
                    If Scr.ScriptType = ScriptBlock.EBlockType.import Then
                        Script.ExternLoader()
                    End If
                    Return
                End If
        End Select

        MainItems.Insert(index, Scr)
        If MainItems.First.Parent IsNot Nothing Then
            Scr.Parent = MainItems.First.Parent
        End If
        If Scr.ScriptType = ScriptBlock.EBlockType.import Then
            Script.ExternLoader()
        End If
    End Sub
    Private Sub ItemFixer(item As ScriptBlock)
        Dim TypeLisT() As ScriptBlock.EBlockType = {ScriptBlock.EBlockType.import, ScriptBlock.EBlockType.vardefine,
            ScriptBlock.EBlockType.objectdefine, ScriptBlock.EBlockType.fundefine}

        Dim bflag() As Boolean = {False, False, False, False}

        For k = 0 To TypeLisT.Count - 1
            If TypeLisT(k) = item.ScriptType Then
                For i = MainItems.Count - 1 To 0 Step -1
                    If MainItems(i).ScriptType = TypeLisT(k) Then
                        MainItems.Insert(i, item)
                        Return
                    End If
                Next
            End If
        Next

        For i = 0 To MainItems.Count - 1
            If TypeLisT.ToList.IndexOf(MainItems(i).ScriptType) <> -1 Then
                bflag(TypeLisT.ToList.IndexOf(MainItems(i).ScriptType)) = True
            End If
        Next

        Dim ctypeindex As Integer = TypeLisT.ToList.IndexOf(item.ScriptType)


        Select Case item.ScriptType
            Case ScriptBlock.EBlockType.import
                MainItems.Insert(0, item)
            Case ScriptBlock.EBlockType.vardefine
                If Not bflag(0) Then
                    MainItems.Insert(0, item)
                Else
                    For i = MainItems.Count - 1 To 0 Step -1
                        If MainItems(i).ScriptType = ScriptBlock.EBlockType.import Then
                            MainItems.Insert(i + 1, item)
                            Return
                        End If
                    Next
                End If
            Case ScriptBlock.EBlockType.objectdefine
                If Not bflag(3) Then
                    MainItems.Add(item)
                Else
                    For i = 0 To MainItems.Count - 1
                        If MainItems(i).ScriptType = ScriptBlock.EBlockType.fundefine Then
                            MainItems.Insert(i, item)
                            Return
                        End If
                    Next
                End If
            Case ScriptBlock.EBlockType.fundefine
                MainItems.Add(item)
        End Select
    End Sub




    Private selpos As Integer = 1

    Public MainScr As ScriptBlock = Nothing
    Public Sub SetTEGUIPage(tTEGUIPage As TEGUIPage)
        TEGUIPage = tTEGUIPage
    End Sub
    Public Sub LoadScript(tTEFile As TEFile, mainScrlist As List(Of ScriptBlock))
        MainTreeview.Items.Clear()
        Tasklist.Clear()
        TaskIndex = -1


        PTEFile = tTEFile
        MainItems = mainScrlist

        Script = PTEFile.Scripter


        For i = 0 To ItemCount - 1
            MainTreeview.Items.Add(GetItems(i).GetTreeviewitem)
        Next
        MainScr = Nothing
        If mainScrlist.Count <> 0 Then
            If mainScrlist.First.Parent IsNot Nothing Then
                MainScr = mainScrlist.First.Parent
            End If
        End If



        RefreshBtn()
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

    Private Sub CopyItemListSort(ByRef list As List(Of ScriptBlock))
        If list.First.Parent IsNot Nothing Then
            list = list.OrderBy(Function(x) x.Parent.child.IndexOf(x)).ToList()
        End If

        'CopyItemList.Sort()
    End Sub

    'Private CopyItemList As New List(Of ScriptBlock)
    Private Sub CopySelectItem()
        Dim CopyItemList As New List(Of ScriptBlock)

        If IsItemSelected() Then
            CopyItemList.Clear()
            If SelectedList.Count = 0 Then
                Dim tscr As ScriptBlock = CType(MainTreeview.SelectedItem, TreeViewItem).Tag
                If tscr.IsDeleteAble Then
                    CopyItemList.Add(tscr)
                End If
            Else
                For i = 0 To SelectedList.Count - 1
                    Dim tscr As ScriptBlock = CType(SelectedList(i), TreeViewItem).Tag
                    If tscr.IsDeleteAble Then
                        CopyItemList.Add(tscr)
                    End If
                Next
            End If
        End If
        CopyItemListSort(CopyItemList)
        Clipboard.SetDataObject(CopyItemList)

        RefreshBtn()
    End Sub



    Private Function IsCopyable() As Boolean
        If SelectedList.Count = 0 Then
            Dim tscr As ScriptBlock = CType(MainTreeview.SelectedItem, TreeViewItem).Tag
            If Not tscr.IsDeleteAble Then
                Return False
            End If
        Else
            For i = 0 To SelectedList.Count - 1
                Dim tscr As ScriptBlock = CType(SelectedList(i), TreeViewItem).Tag
                If Not tscr.IsDeleteAble Then
                    Return False
                End If
            Next
        End If
        Return True
    End Function


    Private Sub PasteSelectItem()
        pjData.SetDirty(True)
        Dim CopyItemList As List(Of ScriptBlock) = Nothing
        Dim data_object As IDataObject = Clipboard.GetDataObject()
        If (data_object.GetDataPresent(GetType(List(Of ScriptBlock)).FullName)) Then
            CopyItemList = data_object.GetData(GetType(List(Of ScriptBlock)).FullName)
        End If
        If CopyItemList Is Nothing Then
            Return
        End If
        For i = 0 To CopyItemList.Count - 1
            Script.scrLoadRefresh(CopyItemList(i))
        Next

        For i = 0 To SelectedList.Count - 1
            Dim ttbheader As ScriptTreeviewitem = SelectedList(i).Header
            ttbheader.DeSelectItem()
        Next
        SelectedList.Clear()

        Dim selectitem As TreeViewItem = MainTreeview.SelectedItem
        Dim ssb As ScriptBlock = Nothing
        Dim addindex As Integer = -1


        Dim tagname As String = CopyItemList.First.name

        If selectitem IsNot Nothing Then
            ssb = selectitem.Tag
            Dim isfolder As Boolean = ssb.isfolder

            If tagname = "switchcase" Then
                If ssb.ScriptType = ScriptBlock.EBlockType.switch Then
                    isfolder = True
                ElseIf ssb.ScriptType = ScriptBlock.EBlockType.switchcase Then
                    isfolder = False
                End If
            End If

            If Not isfolder Then
                If selectitem.Parent.GetType Is GetType(TreeView) Then
                    addindex = Script.IndexOfItem(ssb) + selpos

                    ssb = Nothing
                    selectitem = Nothing
                Else
                    Dim tsb As ScriptBlock = CType(selectitem.Parent, TreeViewItem).Tag

                    addindex = tsb.child.IndexOf(ssb) + selpos

                    ssb = tsb
                    selectitem = selectitem.Parent
                End If
            End If
        End If

        'MsgBox("시작")
        Dim sucesscount As Integer = 0
        For i = 0 To CopyItemList.Count - 1
            Dim nsb As ScriptBlock = CopyItemList(i) '.DeepCopy
            nsb.Scripter = Me.Script

            If selectitem IsNot Nothing Then
                If CheckValidated(nsb, ssb) Then
                    sucesscount += 1
                    Dim ntreeview As TreeViewItem = nsb.GetTreeviewitem

                    selectitem.IsExpanded = True
                    If addindex = -1 Then
                        ssb.AddChild(nsb)
                        selectitem.Items.Add(ntreeview)
                    Else
                        ssb.InsertChild(addindex, nsb)
                        selectitem.Items.Insert(addindex, ntreeview)
                    End If
                    'MsgBox("아이템생성 새 창없이")
                    AddInsertTask(ntreeview)
                End If
            Else
                If CheckValidated(nsb, MainScr) Then
                    sucesscount += 1
                    Dim ntreeview As TreeViewItem = nsb.GetTreeviewitem

                    If addindex = -1 Then
                        MainItems.Add(nsb)
                        If MainItems.First.Parent IsNot Nothing Then
                            nsb.Parent = MainItems.First.Parent
                        End If
                        MainTreeview.Items.Add(ntreeview)
                    Else
                        MainItems.Insert(addindex, nsb)
                        If MainItems.First.Parent IsNot Nothing Then
                            nsb.Parent = MainItems.First.Parent
                        End If
                        MainTreeview.Items.Insert(addindex, ntreeview)
                    End If
                    'MsgBox("아이템생성 새 창없이")
                    AddInsertTask(ntreeview)
                End If
            End If
        Next
        SetRepeatCount(sucesscount)

        TEGUIPage.ObjectSelector.RefreshCurrentList()

        'MsgBox("끝  걸린시간 : " & time.Subtract(Now).Milliseconds)



        'For i = 0 To CopyItemList.Count - 1
        '    Dim tscr As ScriptBlock = CopyItemList(i)
        '    AddItemClick(ScriptBlock.EBlockType.none, "", tscr)
        'Next
    End Sub


    Private Sub CutSelectItem()
        CopySelectItem()
        DeleteSelectItem()
    End Sub

    Private Sub tUndoItem()
        If Undoable() Then
            Dim repeatcount As Integer = GetRepeatCount(False)
            For i = 0 To repeatcount - 1
                UndoTask()
            Next


            Undobtn.IsEnabled = Undoable()
            Redobtn.IsEnabled = Redoable()
        End If
    End Sub
    Private Sub tRedoItem()
        If Redoable() Then
            Dim repeatcount As Integer = GetRepeatCount(True)
            For i = 0 To repeatcount - 1
                RedoTask()
            Next


            Undobtn.IsEnabled = Undoable()
            Redobtn.IsEnabled = Redoable()
        End If
    End Sub
    Private Sub DeleteSelectItem()
        If IsItemSelected() Then
            If SelectedList.Count = 0 Then
                If Not f_DeleteItem(MainTreeview.SelectedItem) Then
                    SnackBarDialog(Tool.GetText("NotDeleteitem"))
                Else
                    SetRepeatCount(1)
                End If
            Else
                Dim tlist As New List(Of TreeViewItem)
                For i = 0 To SelectedList.Count - 1
                    tlist.Add(SelectedList(i))
                Next


                Dim Deleteitem As Integer = 0
                Dim FaileDeleteItem As Integer = 0
                For i = 0 To tlist.Count - 1
                    If f_DeleteItem(tlist(i)) Then
                        Deleteitem += 1
                    Else
                        FaileDeleteItem += 1
                    End If
                Next
                SnackBarDialog(Tool.GetText("NitemDelete", Deleteitem))
                SetRepeatCount(Deleteitem)
            End If
        End If
    End Sub


    Private Sub MoveUpItem()
        If IsItemSelected() Then
            If SelectedList.Count = 0 Then
                Dim titem As TreeViewItem = MainTreeview.SelectedItem
                Dim scr As ScriptBlock = titem.Tag



                Dim tp As ItemCollection


                If titem.Parent.GetType Is GetType(TreeViewItem) Then
                    tp = CType(titem.Parent, TreeViewItem).Items
                Else
                    tp = CType(titem.Parent, TreeView).Items
                End If





                If Not scr.IsDeleteAble Then
                    SnackBarDialog("옮길 수 없는 아이템입니다.")
                    Return
                End If

                Dim pos As List(Of Integer) = GetTreeviewPos(titem)

                Dim cpos As Integer = pos(0)

                If cpos = 0 Then
                    Return
                End If





                Dim nsb As ScriptBlock = scr.DeepCopy
                Dim ntreeview As TreeViewItem = nsb.GetTreeviewitem


                f_DeleteItem(titem)
                tp.Insert(cpos - 1, ntreeview)


                If scr.Parent Is Nothing Then
                    MainItems.Insert(cpos - 1, nsb)
                Else
                    scr.Parent.InsertChild(cpos - 1, nsb)
                End If
                AddInsertTask(ntreeview)
                SetRepeatCount(2)


                ntreeview.IsSelected = True
            Else
                SnackBarDialog("하나의 아이템만 옮길 수 있습니다.")
            End If
        End If
    End Sub


    Private Sub MoveDownItem()
        If IsItemSelected() Then
            If SelectedList.Count = 0 Then
                Dim titem As TreeViewItem = MainTreeview.SelectedItem
                Dim scr As ScriptBlock = titem.Tag



                Dim tp As ItemCollection


                If titem.Parent.GetType Is GetType(TreeViewItem) Then
                    tp = CType(titem.Parent, TreeViewItem).Items
                Else
                    tp = CType(titem.Parent, TreeView).Items
                End If





                If Not scr.IsDeleteAble Then
                    SnackBarDialog("옮길 수 없는 아이템입니다.")
                    Return
                End If

                Dim pos As List(Of Integer) = GetTreeviewPos(titem)

                Dim cpos As Integer = pos(0)

                If cpos + 1 = tp.Count Then
                    Return
                End If





                Dim nsb As ScriptBlock = scr.DeepCopy
                Dim ntreeview As TreeViewItem = nsb.GetTreeviewitem


                f_DeleteItem(titem)
                tp.Insert(cpos + 1, ntreeview)


                If scr.Parent Is Nothing Then
                    MainItems.Insert(cpos + 1, nsb)
                Else
                    scr.Parent.InsertChild(cpos + 1, nsb)
                End If
                AddInsertTask(ntreeview)
                SetRepeatCount(2)


                ntreeview.IsSelected = True
            Else
                SnackBarDialog("하나의 아이템만 옮길 수 있습니다.")
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
        Undobtn.IsEnabled = Undoable()
        Redobtn.IsEnabled = Redoable()

        Dim CopyItemList As List(Of ScriptBlock) = Nothing
        Dim data_object As IDataObject = Clipboard.GetDataObject()
        If (data_object.GetDataPresent(GetType(List(Of ScriptBlock)).FullName)) Then
            CopyItemList = data_object.GetData(GetType(List(Of ScriptBlock)).FullName)
        End If


        '복사한 항목이 있냐 없냐 판단
        If CopyItemList IsNot Nothing Then
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
            FoldUnder.IsEnabled = False
            UnFoldUnder.IsEnabled = False
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
            upbtn.IsEnabled = False
            downbtn.IsEnabled = False
        Else
            '선택한 아이템이 있을 경우
            FoldUnder.IsEnabled = True
            UnFoldUnder.IsEnabled = True
            CutItem.IsEnabled = True
            Cutbtn.IsEnabled = True
            If IsCopyable() Then
                CopyItem.IsEnabled = True
                Copybtn.IsEnabled = True
            Else
                CopyItem.IsEnabled = False
                Copybtn.IsEnabled = False
            End If

            deselectItem.IsEnabled = True
            DeSelectbtn.IsEnabled = True

            EditItem.IsEnabled = True
            Editbtn.IsEnabled = True

            DeleteItem.IsEnabled = True
            Deletebtn.IsEnabled = True
            upbtn.IsEnabled = True
            downbtn.IsEnabled = True
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
    End Sub
    Private Sub MainTreeview_PreviewKeyUp(sender As Object, e As KeyEventArgs)
        If e.Key = Key.LeftCtrl Then
            leftCtrlDown = False
        ElseIf e.Key = Key.LeftShift Then
            leftShiftDown = False
        End If
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
                    Dim ttbheader As ScriptTreeviewitem = lastSelectItem.Header
                    ttbheader.SelectItem()
                    SelectedList.Add(lastSelectItem)
                    If SelectedList.First.Parent Is tb.Parent Then
                        tbheader.SelectItem()
                        SelectedList.Add(tb)
                    End If
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

                            'MainTreeview.UpdateLayout()
                            'MsgBox(startindex & "TO" & endindex)

                            For i = startindex To endindex
                                Dim tnode As TreeViewItem = pnode.Items(i)
                                If SelectedList.IndexOf(tnode) = -1 Then
                                    Dim th As ScriptTreeviewitem = tnode.Header
                                    th.SelectItem()
                                    SelectedList.Add(tnode)
                                End If
                            Next
                        Else
                            Dim pnode As TreeView = tb.Parent

                            Dim a1 As Integer = pnode.Items.IndexOf(lastSelectItem)
                            Dim a2 As Integer = pnode.Items.IndexOf(tb)

                            Dim startindex As Integer = Math.Min(a1, a2)
                            Dim endindex As Integer = Math.Max(a1, a2)

                            'MainTreeview.UpdateLayout()
                            'MsgBox(startindex & "TO" & endindex)

                            For i = startindex To endindex
                                Dim tnode As TreeViewItem = pnode.Items(i)
                                If SelectedList.IndexOf(tnode) = -1 Then
                                    Dim th As ScriptTreeviewitem = tnode.Header
                                    th.SelectItem()
                                    SelectedList.Add(tnode)
                                End If
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
            If tb.Items.Count = 0 Then
                OpenEditWindow(MainTreeview.SelectedItem)
            End If
        End If
    End Sub

    Private Sub Undobtn_Click(sender As Object, e As RoutedEventArgs)
        tUndoItem()
        Dim t As New TreeViewItem
    End Sub

    Private Sub Redobtn_Click(sender As Object, e As RoutedEventArgs)
        tRedoItem()
    End Sub

    Private Sub FoldUnder_Click(sender As Object, e As RoutedEventArgs)
        If IsItemSelected() Then
            If SelectedList.Count = 0 Then
                Dim tb As TreeViewItem = MainTreeview.SelectedItem
                If tb IsNot Nothing Then
                    tb.IsExpanded = True
                    For i = 0 To tb.Items.Count - 1
                        ExpandTreeviewItem(tb.Items(i), True)
                    Next
                End If
            Else
                For i = 0 To SelectedList.Count - 1
                    Dim tb As TreeViewItem = SelectedList(i)
                    If tb IsNot Nothing Then
                        tb.IsExpanded = True
                        For j = 0 To tb.Items.Count - 1
                            ExpandTreeviewItem(tb.Items(j), True)
                        Next
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub UnFoldUnder_Click(sender As Object, e As RoutedEventArgs)
        If IsItemSelected() Then
            If SelectedList.Count = 0 Then
                Dim tb As TreeViewItem = MainTreeview.SelectedItem
                If tb IsNot Nothing Then
                    tb.IsExpanded = False
                    For i = 0 To tb.Items.Count - 1
                        ExpandTreeviewItem(tb.Items(i), True)
                    Next
                End If
            Else
                For i = 0 To SelectedList.Count - 1
                    Dim tb As TreeViewItem = SelectedList(i)
                    If tb IsNot Nothing Then
                        tb.IsExpanded = False
                        For j = 0 To tb.Items.Count - 1
                            ExpandTreeviewItem(tb.Items(j), True)
                        Next
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub ExpandTreeviewItem(titem As TreeViewItem, IsExapend As Boolean)
        titem.IsExpanded = IsExapend
        For i = 0 To titem.Items.Count - 1
            ExpandTreeviewItem(titem.Items(i), IsExapend)
        Next
    End Sub

    Private Sub InsertPos_Click(sender As Object, e As RoutedEventArgs)
        If selpos = 0 Then
            selpos = 1
            insertPosIcon.Kind = PackIconKind.ArrowExpandDown
        Else
            selpos = 0
            insertPosIcon.Kind = PackIconKind.ArrowExpandUp
        End If
    End Sub


    Private Sub UpSel_Click(sender As Object, e As RoutedEventArgs)
        MoveUpItem()

        'upbtn.IsEnabled = False
        'downbtn.IsEnabled = True

        'selpos = 0
    End Sub
    Private Sub DownSel_Click(sender As Object, e As RoutedEventArgs)
        MoveDownItem()
        'upbtn.IsEnabled = True
        'downbtn.IsEnabled = False

        'selpos = 1
    End Sub
End Class
