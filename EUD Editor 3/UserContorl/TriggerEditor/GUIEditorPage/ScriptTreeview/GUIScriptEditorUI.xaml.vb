Imports BondTech.HotKeyManagement.WPF._4

Public Class GUIScriptEditorUI
    Private popupScript As ScriptBlock
    Private popupbutton As Button
    Public Sub OpenCodeSelecter(script As ScriptBlock, btn As Button)
        popupScript = script
        popupbutton = btn


        Popupbox.Width = 360
        Popupbox.IsOpen = True

        Dim value As Integer = 0
        If IsNumeric(popupScript.Value) Then
            value = popupScript.Value
        End If
        CodeList.ListReset(script.ValueType, True, value)
    End Sub

    Private Sub CodeList_Select(sender As Object, e As RoutedEventArgs)
        Popupbox.IsOpen = False
        popupScript.Value = sender(1)
        popupbutton.Content = pjData.CodeLabel(popupScript.ValueType, popupScript.Value)
        'ValueText.Text = sender(1)
    End Sub

    'Private Sub MyHotKeyManager_LocalHotKeyPressed(sender As Object, e As LocalHotKeyEventArgs)
    '    Select Case e.HotKey.Name
    '        Case "Undo"
    '            Undo()
    '        Case "Redo"
    '            Redo()
    '    End Select
    'End Sub


    'Private MyHotKeyManager As HotKeyManager
    'Public Sub HotkeyInit(twindow As Window)
    '    MyHotKeyManager = New HotKeyManager(twindow)
    '    Dim hUndo As New LocalHotKey("Undo", ModifierKeys.Control, Keys.Z)
    '    Dim hRedo As New LocalHotKey("Redo", ModifierKeys.Control, Keys.R)

    '    MyHotKeyManager.AddLocalHotKey(hUndo)
    '    MyHotKeyManager.AddLocalHotKey(hRedo)

    '    AddHandler MyHotKeyManager.LocalHotKeyPressed, AddressOf MyHotKeyManager_LocalHotKeyPressed
    'End Sub

    'Public Shared UndoKeyInputCommand As RoutedUICommand = New RoutedUICommand("myCommand", "myCommand", GetType(GUIScriptEditorUI))
    'Public Shared RedoItemKeyInputCommand As RoutedUICommand = New RoutedUICommand("myCommand", "myCommand", GetType(GUIScriptEditorUI))
    'Private Sub UndoCommandExcute(ByVal target As Object, ByVal e As ExecutedRoutedEventArgs)
    '    Undo()
    'End Sub
    'Private Sub RedoCommandExcute(ByVal target As Object, ByVal e As ExecutedRoutedEventArgs)
    '    Redo()
    'End Sub

    Private MulitSelectItems As New List(Of ScriptTreeviewItem)
    Public Function AddMulitSelectItem(STreeviewItem As ScriptTreeviewItem) As Boolean
        If MulitSelectItems.IndexOf(STreeviewItem) = -1 Then
            If MulitSelectItems.Count > 0 Then
                Dim TargetTreevieweItem As TreeViewItem = MulitSelectItems(0).Parent

                Dim CurrentTreevieweItem As TreeViewItem = STreeviewItem.Parent

                If TargetTreevieweItem.Parent IsNot CurrentTreevieweItem.Parent Then
                    Return False
                End If
            End If

            If STreeviewItem.Script.TriggerScript.IsLock Then
                Return False
            End If

            Dim tTreevieweItem As TreeViewItem = STreeviewItem.Parent

            If tTreevieweItem.Parent.GetType Is GetType(TreeViewItem) Then
                Dim tt As TreeViewItem = tTreevieweItem.Parent
                STreeviewItem.DragIndex = tt.Items.IndexOf(tTreevieweItem)
            Else
                Dim tt As TreeView = tTreevieweItem.Parent
                STreeviewItem.DragIndex = tt.Items.IndexOf(tTreevieweItem)
            End If


            MulitSelectItems.Add(STreeviewItem)
            Return True
        Else
            If Not leftShiftDown Then
                DeleteMulitSelectItem(STreeviewItem)
                Return False
            End If
        End If
        Return True
    End Function
    Public Sub DeleteMulitSelectItem(STreeviewItem As ScriptTreeviewItem)
        MulitSelectItems.Remove(STreeviewItem)
    End Sub

    Private tOverScriptTreeviewItem As New List(Of ScriptTreeviewItem)

    Public Sub AddOverScriptTreeviewItem(Treeview As ScriptTreeviewItem)
        If tOverScriptTreeviewItem.IndexOf(Treeview) = -1 Then
            tOverScriptTreeviewItem.Add(Treeview)
        End If
    End Sub
    Public Sub RemoveOverScriptTreeviewItem(Treeview As ScriptTreeviewItem)
        If tOverScriptTreeviewItem.IndexOf(Treeview) <> -1 Then
            tOverScriptTreeviewItem.Remove(Treeview)
        End If
    End Sub
    Public Function LastOverScriptTreeviewItem() As ScriptTreeviewItem
        If tOverScriptTreeviewItem.Count = 0 Then
            Return Nothing
        End If

        Return tOverScriptTreeviewItem.Last
    End Function



    Private PTEFile As TEFile

    Public Sub Save()
        Dim Script As GUIScriptEditor = PTEFile.Scripter


        Script.items.Clear()

        For i = 0 To MainTreeview.Items.Count - 1
            Script.items.Add(SaveItems(MainTreeview.Items(i)))
        Next


        'CType(PTEFile.Scripter, GUIScriptEditor).ItemCollection = MainTreeview.Items
    End Sub
    Private Function SaveItems(titem As TreeViewItem) As ScriptBlock
        Dim returnBlock As ScriptBlock

        Dim header As ScriptTreeviewItem = titem.Header
        returnBlock = header.Script



        returnBlock.Child.Clear()

        For i = 0 To titem.Items.Count - 1
            returnBlock.Child.Add(SaveItems(titem.Items(i)))
        Next


        Return returnBlock
    End Function





    Public Sub LoadScript(tTEFile As TEFile)
        PTEFile = tTEFile


        Dim Script As GUIScriptEditor = PTEFile.Scripter

        For i = 0 To Script.items.Count - 1
            MainTreeview.Items.Add(GetTreeviewItem(Script.items(i)))
        Next
        'If Script.items.Count = 0 Then

        '    MainTreeview.Items.Add(GetTreeviewItem("If"))
        '    MainTreeview.Items.Add(GetTreeviewItem("CreateUnit"))
        'End If

    End Sub




    Public Function CopyTreeview(treeviewitem As TreeViewItem) As TreeViewItem
        '깊은 복사를 하는 함수.
        Dim ScriptTreeviewItem As ScriptTreeviewItem = treeviewitem.Header
        Dim returntreeviewitem As TreeViewItem = GetTreeviewItem(ScriptTreeviewItem.Script)

        returntreeviewitem.IsExpanded = treeviewitem.IsExpanded

        For i = 0 To treeviewitem.Items.Count - 1
            returntreeviewitem.Items.Add(CopyTreeview(treeviewitem.Items(i)))
        Next

        Return returntreeviewitem
    End Function



    Public Function GetTreeviewItem(sb As ScriptBlock) As TreeViewItem
        Dim key As String = sb.TriggerScript.SName
        Dim treeitem As New TreeViewItem
        Dim groupname As String = tescm.GetTriggerScript(key).Group
        treeitem.Background = New SolidColorBrush(TriggerScript.GetColor(groupname))
        treeitem.Header = New ScriptTreeviewItem(Me, New ScriptBlock(sb))
        If tescm.GetTriggerScript(key).shortheader Then
            treeitem.Style = Application.Current.Resources("ShortTreeViewItem")
        End If

        For i = 0 To sb.Child.Count - 1
            treeitem.Items.Add(GetTreeviewItem(sb.Child(i)))
        Next



        Return treeitem
    End Function

    Public Function GetTreeviewItem(key As String) As TreeViewItem
        Dim treeitem As New TreeViewItem
        Dim groupname As String = tescm.GetTriggerScript(key).Group

        treeitem.Background = New SolidColorBrush(TriggerScript.GetColor(groupname))
        treeitem.Header = New ScriptTreeviewItem(Me, New ScriptBlock(key))
        If tescm.GetTriggerScript(key).shortheader Then
            treeitem.Style = Application.Current.Resources("ShortTreeViewItem")
        End If

        If tescm.GetTriggerScript(key).InitBlock.Count > 0 Then
            If tescm.GetTriggerScript(key).InitBlock(0) <> "" Then
                For i = 0 To tescm.GetTriggerScript(key).InitBlock.Count - 1
                    Dim initkey As String = tescm.GetTriggerScript(key).InitBlock(i)

                    Dim tgroupname As String = tescm.GetTriggerScript(initkey).Group
                    Dim ttreeitem As New TreeViewItem
                    ttreeitem.Style = Application.Current.Resources("ShortTreeViewItem")
                    ttreeitem.Background = New SolidColorBrush(TriggerScript.GetColor(tgroupname))
                    ttreeitem.Header = New ScriptTreeviewItem(Me, New ScriptBlock(initkey))
                    treeitem.Items.Add(ttreeitem)
                Next
            End If
        End If

        Return treeitem
    End Function


    Private Function CheckPlaceAble(parrent As TriggerScript, child As TriggerScript, parenttree As TreeViewItem) As Boolean
        Dim parrentScript As TriggerScript = parrent
        Dim childScript As TriggerScript = child

        If Not parrentScript.IsFolder Then
            '폴더가 아니면 당연히 나가야지
            ErrorPopup("자식을 가질 수 없는 스크립트 입니다.")
            Return False
        End If

        Select Case parrentScript.FolderType
            Case TriggerScript.ScriptType.Both
                If childScript.SType <> TriggerScript.ScriptType.Action Or childScript.SType <> TriggerScript.ScriptType.Condition Then
                    ErrorPopup("액션이나 조건만 넣을 수 있습니다.")
                    Return False
                End If
            Case TriggerScript.ScriptType.Action
                If childScript.SType <> TriggerScript.ScriptType.Action And childScript.SType <> TriggerScript.ScriptType.Both Then
                    ErrorPopup("액션만 넣을 수 있습니다.")
                    Return False
                End If
            Case TriggerScript.ScriptType.Condition
                If childScript.SType <> TriggerScript.ScriptType.Condition And childScript.SType <> TriggerScript.ScriptType.Both Then
                    ErrorPopup("조건만 넣을 수 있습니다.")
                    Return False
                End If
            Case TriggerScript.ScriptType.Special
                If childScript.SType <> TriggerScript.ScriptType.Special Then
                    Return False
                End If
            Case TriggerScript.ScriptType.Null ', TriggerScript.ScriptType.Special
                Return False
        End Select


        Dim folderrullexist As Boolean = True
        If parrentScript.FolderRull.Count = 1 Then
            If parrentScript.FolderRull(0) = "" Then
                folderrullexist = False
            End If
        End If

        If folderrullexist Then
            If parrentScript.FolderRull.IndexOf(childScript.SName) = -1 Then
                '폴더롤이 존재하고 그 안에 없을 경우
                ErrorPopup("해당 위치에 넣을 수 없는 블럭입니다.")
                Return False
            End If
        End If
        If childScript.Uniqueness Then
            '존재하지만 유일한 존재인지 파악해야됨
            If parenttree IsNot Nothing Then
                For i = 0 To parenttree.Items.Count - 1
                    Dim items As TreeViewItem = parenttree.Items(i)
                    Dim header As ScriptTreeviewItem = items.Header

                    If header.Script.TriggerScript.SName = childScript.SName Then
                        ErrorPopup(childScript.SName & "은 하나만 넣을 수 있습니다.")
                        Return False
                    End If
                Next
            End If
        End If


        Return True
    End Function

    Public Sub AddItemClick(keyname As String)
        Dim tvitem As TreeViewItem = GetTreeviewItem(keyname)


        pAddItemScript(tvitem, keyname)
        'MainTreeview.Items.Add(New ScriptTreeviewItem(Me, New ScriptBlock(keyname)))
        'MsgBox("스크립터에서 받음 : " & keyname)
    End Sub

    Private Sub ValueAddItem(AddedTriggerScript As TriggerScript, keyname As String)
        '값일 경우

        If Not (AddedTriggerScript.SType = TriggerScript.ScriptType.Value Or AddedTriggerScript.SType = TriggerScript.ScriptType.Both) Then
            ErrorPopup("함수나 값이 들어가야 합니다.")

            Exit Sub
        End If

        Dim parentScript As ScriptBlock = CType(CType(CType(SelectedScript.Parent, WrapPanel).Parent, Grid).Parent, ScriptBlockItem).Script
        Dim currentScript As ScriptBlock = SelectedScript.ContentPanel.Script



        Dim argumentindex As Integer = parentScript.Argument.IndexOf(currentScript)



        Dim newScript As New ScriptBlock(keyname)

        parentScript.Argument.RemoveAt(argumentindex)
        parentScript.Argument.Insert(argumentindex, newScript)
        SelectedScript.Reload(newScript)
        SelectedScript.ContentPanel.Init(Me, newScript, True)
    End Sub

    Private Sub ValueChangeItem(AddedScriptBlock As ScriptBlock, keyname As String)
        Dim AddedTriggerScript As TriggerScript = AddedScriptBlock.TriggerScript
        '값일 경우

        If Not (AddedTriggerScript.SType = TriggerScript.ScriptType.Value Or AddedTriggerScript.SType = TriggerScript.ScriptType.Both) Then
            ErrorPopup("함수나 값이 들어가야 합니다.")

            Exit Sub
        End If

        Dim parentScript As ScriptBlock = CType(CType(CType(SelectedScript.Parent, WrapPanel).Parent, Grid).Parent, ScriptBlockItem).Script
        Dim currentScript As ScriptBlock = SelectedScript.ContentPanel.Script



        Dim argumentindex As Integer = parentScript.Argument.IndexOf(currentScript)



        Dim newScript As New ScriptBlock(AddedScriptBlock)

        parentScript.Argument.RemoveAt(argumentindex)
        parentScript.Argument.Insert(argumentindex, newScript)
        SelectedScript.Reload(newScript)
        SelectedScript.ContentPanel.Init(Me, newScript, True)
    End Sub


    Private Function pAddItemScript(tvitem As TreeViewItem, keyname As String) As Boolean
        Dim AddedTriggerScript As TriggerScript = tescm.GetTriggerScript(keyname)
        If SelectedScript IsNot Nothing Then
            '부모 타입이 뭔지 알아보기
            If SelectedScript.Parent.GetType Is GetType(WrapPanel) Then
                ValueAddItem(AddedTriggerScript, keyname)
            Else
                Dim itemcollcteion As ItemCollection = Nothing
                Dim tindex As Integer = -1
                Dim insertitem As TreeViewItem = SelectedScript.Parent
                Dim parentScript As ScriptTreeviewItem = Nothing
                Dim parentTreeView As TreeViewItem = Nothing
                Dim PlaceAble As Boolean = False

                Select Case SelectedScript.SelectLevel
                    Case 1, 3
                        '부모를 구해야됨
                        If insertitem.Parent.GetType Is GetType(TreeView) Then

                            If AddedTriggerScript.SType = TriggerScript.ScriptType.OutSide Then
                                Dim ttreeview As TreeView = insertitem.Parent
                                itemcollcteion = ttreeview.Items
                                PlaceAble = True
                            Else
                                ErrorPopup("함수나 변수 선언문만 들어 갈 수 있습니다.")
                                PlaceAble = False
                                Return False
                            End If
                        Else
                            Dim ttreeviewItem As TreeViewItem = insertitem.Parent
                            itemcollcteion = ttreeviewItem.Items
                            parentScript = ttreeviewItem.Header
                            parentTreeView = ttreeviewItem
                        End If

                        tindex = itemcollcteion.IndexOf(insertitem)
                        If SelectedScript.SelectLevel = 3 Then
                            tindex += 1
                        End If

                    Case 2
                        parentTreeView = insertitem
                        parentScript = insertitem.Header
                End Select

                If Not PlaceAble Then
                    PlaceAble = CheckPlaceAble(parentScript.Script.TriggerScript, tescm.GetTriggerScript(keyname), parentTreeView)
                End If


                If PlaceAble Then
                    If tindex = -1 Then
                        insertitem.Items.Add(tvitem)
                        Dim tst As ScriptTreeviewItem = tvitem.Header
                        'tst.GetPosition(insertitem, insertitem.Items.Count - 1)


                        CreateWork(tst)
                        insertitem.IsExpanded = True
                    Else
                        itemcollcteion.Insert(tindex, tvitem)



                        Dim tst As ScriptTreeviewItem = tvitem.Header
                        'tst.GetPosition(ttreeviewItem, tindex)


                        CreateWork(tst)
                    End If
                Else
                    Return False
                End If
            End If
        Else
            If AddedTriggerScript.SType = TriggerScript.ScriptType.OutSide Or AddedTriggerScript.SType = TriggerScript.ScriptType.Free Then
                Dim ttreeviewItem As TreeViewItem = GetTreeviewItem(keyname)

                MainTreeview.Items.Add(ttreeviewItem)


                Dim tst As ScriptTreeviewItem = ttreeviewItem.Header
                CreateWork(tst)
            Else
                ErrorPopup("함수나 변수 선언문만 들어 갈 수 있습니다.")
                Return False
            End If
        End If
        Return True
    End Function


    Private sMessageQueue As MaterialDesignThemes.Wpf.SnackbarMessageQueue
    Private Sub ErrorPopup(text As String)
        sMessageQueue.Enqueue(text)

        'Dim sm As New MaterialDesignThemes.Wpf.SnackbarMessage
        'sm.Content = text

        'ErrorSnackbar.Message = sm
        'ErrorSnackbar.IsActive = True
    End Sub


    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        sMessageQueue = New MaterialDesignThemes.Wpf.SnackbarMessageQueue
        ErrorSnackbar.MessageQueue = sMessageQueue
    End Sub

    Public SelectedScript As ScriptTreeviewItem
    Public Sub SelectScript(item As ScriptTreeviewItem, level As Byte)
        If SelectedScript IsNot Nothing Then
            SelectedScript.dSelectthis()

            SelectedScript = item
            SelectedScript.Selectthis(level)
        Else
            SelectedScript = item
            SelectedScript.Selectthis(level)
        End If
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
        End Select
    End Sub

    Private Sub MainTreeview_PreviewKeyUp(sender As Object, e As KeyEventArgs)
        If e.Key = Key.LeftCtrl Then
            leftCtrlDown = False
        ElseIf e.Key = Key.LeftShift Then
            leftShiftDown = False
        End If
    End Sub

    Private Sub MainTreeview_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object))
        If leftCtrlDown Then
            Dim treeviewitem As TreeViewItem = e.NewValue
            Dim ScriptTreeview As ScriptTreeviewItem = treeviewitem.Header


            If SelectedScript IsNot Nothing And MulitSelectItems.Count = 0 Then
                SelectedScript.MulitSelect()
            End If
            If MulitSelectItems.Count = 0 Then
                Return
            End If
            ScriptTreeview.MulitSelect()
        ElseIf leftShiftDown Then
            Dim CurrentTreevieweItem As TreeViewItem = e.NewValue


            If MulitSelectItems.Count = 0 Then
                If SelectedScript IsNot Nothing Then
                    SelectedScript.MulitSelect()
                Else
                    Return
                End If
            End If
            If MulitSelectItems.Count = 0 Then
                Return
            End If



            Dim TargetTreevieweItem As TreeViewItem = MulitSelectItems(0).Parent

            If TargetTreevieweItem.Parent Is CurrentTreevieweItem.Parent Then
                Dim ParentItemCollection As ItemCollection


                If TargetTreevieweItem.Parent.GetType Is GetType(TreeViewItem) Then
                    Dim ParentTreeviewitem As TreeViewItem = TargetTreevieweItem.Parent
                    ParentItemCollection = ParentTreeviewitem.Items
                Else
                    Dim ParentTreeview As TreeView = TargetTreevieweItem.Parent
                    ParentItemCollection = ParentTreeview.Items
                End If

                Dim index1 As Integer = ParentItemCollection.IndexOf(CurrentTreevieweItem)
                Dim index2 As Integer = ParentItemCollection.IndexOf(TargetTreevieweItem)


                Dim maxindex As Integer = Math.Max(index1, index2)
                Dim minindex As Integer = Math.Min(index1, index2)

                For i = minindex To maxindex
                    Dim tit As TreeViewItem = ParentItemCollection(i)
                    Dim ScriptTreeview As ScriptTreeviewItem = tit.Header


                    ScriptTreeview.MulitSelect()
                Next
            End If
        Else
            For i = 0 To MulitSelectItems.Count - 1
                MulitSelectItems(0).dMulitSelect()
            Next
        End If
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
    Private Sub CutItem_Click(sender As Object, e As RoutedEventArgs)
        CutSelectItem()
    End Sub
    Private Sub UndoItem_Click(sender As Object, e As RoutedEventArgs)
        Undo()
    End Sub

    Private Sub RedoItem_Click(sender As Object, e As RoutedEventArgs)
        Redo()
    End Sub
    Private Sub ContextMenu_Opened(sender As Object, e As RoutedEventArgs)
        If SelectedScript IsNot Nothing Then '선택된 아이템이 존재 할 경우
            If SelectedScript.Script.TriggerScript.IsLock Then
                CopyItem.IsEnabled = False
            Else
                CopyItem.IsEnabled = True
            End If


            If SelectedScript.IsValue Then
                CutItem.IsEnabled = False
                DeleteItem.IsEnabled = False
            Else
                If SelectedScript.Script.TriggerScript.IsLock Then
                    CutItem.IsEnabled = False
                    DeleteItem.IsEnabled = False
                Else
                    CutItem.IsEnabled = True
                    DeleteItem.IsEnabled = True
                End If
            End If
        Else
            CopyItem.IsEnabled = False
            DeleteItem.IsEnabled = False
            DeleteItem.IsEnabled = False
        End If

        If Copyitems.Count = 0 Then
            PasteItem.IsEnabled = False
        Else
            PasteItem.IsEnabled = True
        End If

        If currentWork >= 0 Then
            UndoItem.IsEnabled = True
        Else
            UndoItem.IsEnabled = False
        End If
        If currentWork < RecentWork.Count - 1 Then
            RedoItem.IsEnabled = True
        Else
            RedoItem.IsEnabled = False
        End If
    End Sub

    Private Sub DeleteSelectItem()
        If MulitSelectItems.Count = 0 Then
            If SelectedScript IsNot Nothing Then
                Dim t As ScriptTreeviewItem = Nothing
                t = SelectedScript
                t.GetPosition()

                If SelectedScript.Delete() Then
                    SelectedScript = Nothing
                    'MsgBox("하시발")
                    SelectLast(t)
                End If
            End If
        Else
            If MulitSelectItems.IndexOf(SelectedScript) >= 0 Then
                SelectedScript = Nothing
            End If
            For i = 0 To MulitSelectItems.Count - 1
                MulitSelectItems(i).Delete()
            Next
            MulitSelectItems.Clear()
        End If


    End Sub


    Private Copyitems As New List(Of ScriptTreeviewItem)
    Private IsValueCopy As Boolean
    Private Sub CopySelectItem()
        IsValueCopy = False
        If MulitSelectItems.Count = 0 Then
            If SelectedScript IsNot Nothing Then
                Copyitems.Clear()

                IsValueCopy = SelectedScript.IsValue

                Copyitems.Add(SelectedScript)
                ErrorPopup("선택한 스크립트가 복사되었습니다.")
            End If
        Else
            Copyitems.Clear()

            For i = 0 To MulitSelectItems.Count - 1
                Copyitems.Add(MulitSelectItems(i))
            Next
            ErrorPopup(MulitSelectItems.Count & "개의 스크립트가 복사되었습니다.")
        End If
    End Sub
    Private Sub PasteSelectItem()
        MoveListsItem(Copyitems, IsValueCopy)


    End Sub

    Private Function MoveListsItem(s As List(Of ScriptTreeviewItem), IsValueMove As Boolean, Optional IsDragMove As Boolean = False) As Integer
        Dim count As Integer

        Dim dragtreeviewitems As New List(Of ScriptTreeviewItem)
        Dim selectdragtreeviewitems As New List(Of ScriptTreeviewItem)


        s.Sort(Function(x, y) x.DragIndex.CompareTo(y.DragIndex))


        Dim scount As Integer = s.Count
        If IsValueMove And s.Count <> 0 Then

            Dim copyscript As New ScriptBlock(s.First.Script)
            Dim treeviewitem As TreeViewItem = GetTreeviewItem(copyscript)

            If SelectedScript IsNot Nothing Then
                If SelectedScript.IsValue Then
                    '넣을곳이 값일 경우
                    ValueChangeItem(copyscript, copyscript.TriggerScript.SName)
                    Return 1
                End If
            End If

            If pAddItemScript(treeviewitem, copyscript.TriggerScript.SName) Then
                If IsDragMove Then
                    dragtreeviewitems.Add(s.First)
                    'Dim st As ScriptTreeviewItem = treeviewitem.Header

                    'st.Selectthis()
                End If
                count += 1
            End If

        Else
            '셀렉트 레벨에 따라 순서 변경
            Dim isUp As Boolean = False
            If SelectedScript IsNot Nothing Then
                If Not SelectedScript.IsValue Then
                    Select Case SelectedScript.SelectLevel
                        Case 3
                            isUp = True
                    End Select

                End If
            End If

            'If Not IsDragMove Then
            '    isUp = False
            '    If SelectedScript IsNot Nothing Then
            '        If Not SelectedScript.IsValue Then
            '            Select Case SelectedScript.SelectLevel
            '                Case 3
            '                    isUp = True
            '            End Select

            '        End If
            '    End If
            'Else
            '    If SelectedScript IsNot Nothing Then
            '        If Not SelectedScript.IsValue Then
            '            Select Case SelectedScript.SelectLevel
            '                Case 3
            '                    isUp = False
            '            End Select

            '        End If
            '    End If
            'End If


            Dim index As Integer
            For i = 0 To scount - 1 Step 1
                If isUp Then
                    index = scount - 1 - i
                Else
                    index = i
                End If

                If Not s(index).IsValue Then
                    Dim treeviewitem As TreeViewItem = CopyTreeview(s(index).Parent)

                    If SelectedScript IsNot Nothing Then
                        If SelectedScript.IsValue Then
                            '넣을곳이 값일 경우
                            ValueChangeItem(s(index).Script, s(index).Script.TriggerScript.SName)
                            Return 1
                        End If
                    End If


                    If pAddItemScript(treeviewitem, s(index).Script.TriggerScript.SName) Then
                        If IsDragMove Then
                            dragtreeviewitems.Add(s(index))


                            Dim st As ScriptTreeviewItem = treeviewitem.Header
                            selectdragtreeviewitems.Add(st)



                        End If
                        count += 1
                    End If
                End If
            Next




        End If
        If IsDragMove Then
            For i = 0 To dragtreeviewitems.Count - 1
                dragtreeviewitems(i).Delete()
            Next
            If scount <> 1 Then
                For i = 0 To selectdragtreeviewitems.Count - 1
                    selectdragtreeviewitems(i).MulitSelect()
                Next
            End If
        End If

        Return count
    End Function



    Private Sub CutSelectItem()
        CopySelectItem()
        DeleteSelectItem()
    End Sub


    Private isDrag As Boolean
    Private isDragAble As Boolean
    Private LastSelect As ScriptTreeviewItem

    Private DragPos As Point
    Private Sub MainTreeview_MouseDown(sender As Object, e As MouseButtonEventArgs)
        If LastOverScriptTreeviewItem() IsNot Nothing Then
            LastSelect = LastOverScriptTreeviewItem()

            If Not LastSelect.IsValue Then
                isDrag = True
                DragPos = e.GetPosition(LastSelect)
                pmouseup = False
            End If
        End If
        '드래그시작
    End Sub

    Private Sub MainTreeview_MouseUp(sender As Object, e As MouseButtonEventArgs)
        If isDrag Then
            DragImage.Visibility = Visibility.Collapsed
            isDrag = False




            Dim goalScript As ScriptTreeviewItem = LastOverScriptTreeviewItem()
            '목적지(SelectedScript)의 부모중에 옮기려는 대상이 없어야 함.
            '목적지가 값일 경우 부모 판단하는 걸 주의해야 함.

            For i = 0 To MulitSelectItems.Count - 1
                MulitSelectItems(i).Opacity = 1
            Next
            If LastSelect IsNot Nothing Then
                LastSelect.Opacity = 1
            End If

            If goalScript Is Nothing Then
                Return
            End If



            For i = 0 To MulitSelectItems.Count - 1
                If MulitSelectItems(i) Is goalScript Then
                    Return
                End If
            Next




            If LastSelect IsNot LastOverScriptTreeviewItem() Then
                If goalScript.IsValue Then
                    '목적지가 값인 경우.
                    '선택한 항목이 하나여야함.
                    If MulitSelectItems.Count > 0 Then
                        ErrorPopup("해당 위치로 드래그 할 수 없습니다.")
                        Return
                    End If

                    '목적지의 부모를 값이 아닐 때 까지 추적하여 선택한 값이 없는지 확인해야 함.

                    Dim stvi As ScriptTreeviewItem
                    stvi = goalScript

                    While stvi.IsValue
                        Dim twp As WrapPanel = stvi.Parent
                        Dim tgrid As Grid = twp.Parent
                        Dim sbi As ScriptBlockItem = tgrid.Parent
                        Dim gd As Grid = sbi.Parent
                        stvi = gd.Parent

                        If stvi Is LastSelect Then
                            '목적지와 선택한 값이 같을 경우
                            'Drag불가능
                            ErrorPopup("해당 위치로 드래그 할 수 없습니다.")
                            Return
                        End If
                    End While


                    '아닌 경우 탈출
                    '즉 드래그 기능 넣기.
                    '############
                    '####DRAG####
                    '############
                    If LastSelect.IsValue Then
                        '벨류가 아닌걸 넣는 상황
                        'LastSelect를 이동
                        AddMulitSelectItem(LastSelect)

                        'pAddItemScript(TreeViewItem, copyscript.TriggerScript.SName)
                        MoveListsItem(MulitSelectItems, False, True)
                        LastSelect.Delete()
                    End If
                Else
                    'LastSelect
                    'LastOverScriptTreeviewItem

                    '마지막 선택의 자식이 목적지가 아니여야함
                    '목적지의 부모를 가보자.
                    If Not LastSelect.IsValue Then
                        Dim StandTreeview As TreeViewItem = LastSelect.Parent


                        Dim ttreeviewitem As TreeViewItem = goalScript.Parent
                        While ttreeviewitem.Parent.GetType IsNot GetType(TreeView)
                            If ttreeviewitem Is StandTreeview Then
                                ErrorPopup("자신의 하위 항목으로 이동할 수 없습니다.")
                                Return
                            End If

                            ttreeviewitem = ttreeviewitem.Parent
                        End While
                        If MulitSelectItems.Count = 0 Then
                            'LastSelect를 이동
                            AddMulitSelectItem(LastSelect)

                            'pAddItemScript(TreeViewItem, copyscript.TriggerScript.SName)
                            MoveListsItem(MulitSelectItems, False, True)
                        Else
                            '선택된 항목들을 모두 이동.


                            '이동이 성공 하면 선택된 항목들을 지우고

                            '아니면 지우지 않는다.

                            '새로 추가된 항목들은 다시 선택된 항목으로 지정해준다.
                            MoveListsItem(MulitSelectItems, False, True)
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub MainTreeview_MouseMove(sender As Object, e As MouseEventArgs)
        If isDrag Then
            If LastSelect IsNot LastOverScriptTreeviewItem() Then
                If DragImage.Visibility <> Visibility.Visible Then
                    DragImage.Visibility = Visibility.Visible

                    DragTreeview.Items.Clear()
                    If MulitSelectItems.Count > 0 Then
                        MulitSelectItems.Sort(Function(x, y) x.DragIndex.CompareTo(y.DragIndex))
                        For i = 0 To MulitSelectItems.Count - 1
                            Dim previewblock As New ScriptBlockItem
                            previewblock.Init(Me, MulitSelectItems(i).Script)
                            DragTreeview.Items.Add(previewblock)
                            MulitSelectItems(i).Opacity = 0.2
                        Next
                    Else
                        Dim previewblock As New ScriptBlockItem
                        previewblock.Init(Me, LastSelect.Script)
                        DragTreeview.Items.Add(previewblock)
                        LastSelect.Opacity = 0.2
                    End If


                End If

            End If

            DragImage.Margin = New Thickness(e.GetPosition(MainGrid).X - DragPos.X, e.GetPosition(MainGrid).Y - DragPos.Y, 0, 0)
            DragTreeview.Width = MainTreeview.ActualWidth

            If pmouseup Then

                DragImage.Visibility = Visibility.Collapsed
                isDrag = False
                pmouseup = False
            End If
        End If
    End Sub

    Private pmouseup As Boolean
    Private Sub MainTreeview_PreviewMouseUp(sender As Object, e As MouseButtonEventArgs)
        pmouseup = True
    End Sub

    Private Sub MainTreeview_MouseMove_1(sender As Object, e As MouseEventArgs)
        DragImage.Margin = New Thickness(e.GetPosition(MainGrid).X - DragPos.X, e.GetPosition(MainGrid).Y - DragPos.Y, 0, 0)
    End Sub

    Private Sub MainTreeview_MouseWheel(sender As Object, e As MouseWheelEventArgs)
        If leftCtrlDown Then
            Dim scaler = TryCast(MainTreeview.LayoutTransform, ScaleTransform)

            If scaler Is Nothing Then
                MainTreeview.LayoutTransform = New ScaleTransform(1, 1)
            Else
                Dim curZoomFactor As Double = scaler.ScaleX

                If scaler.HasAnimatedProperties Then
                    scaler.BeginAnimation(ScaleTransform.ScaleXProperty, Nothing)
                    scaler.BeginAnimation(ScaleTransform.ScaleYProperty, Nothing)
                End If
                If e.Delta > 0 Then
                    scaler.ScaleX *= 1.1
                    scaler.ScaleY *= 1.1
                ElseIf e.Delta < 0 Then
                    scaler.ScaleX /= 1.1
                    scaler.ScaleY /= 1.1
                End If

                'If curZoomFactor = 1.0 Then
                '    scaler.ScaleX = 1.5
                '    scaler.ScaleY = 1.5
                'Else
                '    scaler.ScaleX = 1.0
                '    scaler.ScaleY = 1.0
                'End If
                ErrorSnackbar.IsActive = True
                SnackbarContent.Content = Math.Round(scaler.ScaleX * 100) & "%"
            End If
        End If
    End Sub

    Private RecentWork As New List(Of RecentWork)
    Private currentWork As Integer = -1
    Private currentWorkIndex As Integer = -1


    Private Sub CreateWork(CreteTreeviewItem As List(Of ScriptTreeviewItem))
        WorkAdd(EUD_Editor_3.RecentWork.WorkType.ScriptInsert, CreteTreeviewItem, Nothing)
    End Sub
    Private Sub CreateWork(CreteTreeviewItem As ScriptTreeviewItem)
        Dim ttv As New List(Of ScriptTreeviewItem)
        ttv.Add(CreteTreeviewItem)
        WorkAdd(EUD_Editor_3.RecentWork.WorkType.ScriptInsert, ttv, Nothing)
    End Sub
    Public Sub RemoveWork(CreteTreeviewItem As ScriptTreeviewItem)
        Dim ttv As New List(Of ScriptTreeviewItem)
        ttv.Add(CreteTreeviewItem)
        WorkAdd(EUD_Editor_3.RecentWork.WorkType.ScriptRemove, ttv, Nothing)
    End Sub
    Private Sub MoveWork()
        'WorkAdd(EUD_Editor_3.RecentWork.WorkType.ScriptMove, Nothing)
    End Sub

    Public Sub RemoveCancel()
        RecentWork.RemoveAt(RecentWork.Count - 1)
        currentWork = RecentWork.Count - 1
    End Sub
    Private Sub WorkAdd(wType As RecentWork.WorkType, TreeviewItem As List(Of ScriptTreeviewItem), Values As List(Of Integer))
        If currentWork <> RecentWork.Count - 1 Then
            RecentWork.Clear()
        End If
        RecentWork.Add(New RecentWork(wType, currentWorkIndex, TreeviewItem, Values))
        currentWork = RecentWork.Count - 1

        'ErrorPopup(currentWork & " " & RecentWork.Count)
    End Sub



    Public Sub Undo()
        If currentWork >= 0 Then
            Select Case RecentWork(currentWork).WType
                Case EUD_Editor_3.RecentWork.WorkType.ScriptInsert
                    For i = 0 To RecentWork(currentWork).items.Count - 1
                        Dim titem As ScriptTreeviewItem = RecentWork(currentWork).items(i)

                        Dim ttreeitem As TreeViewItem = SelectTreeview(titem.Position)


                        CType(ttreeitem.Header, ScriptTreeviewItem).Delete(False)
                    Next
                Case EUD_Editor_3.RecentWork.WorkType.ScriptRemove
                    For i = 0 To RecentWork(currentWork).items.Count - 1
                        Dim titem As ScriptTreeviewItem = RecentWork(currentWork).items(i)

                        Dim ttreeitem As TreeViewItem = SelectTreeview(titem.Position, 1)


                        Dim copyscript As New ScriptBlock(titem.Script)
                        Dim treeviewitem As TreeViewItem = GetTreeviewItem(copyscript)


                        If ttreeitem Is Nothing Then
                            MainTreeview.Items.Insert(titem.Position(0), treeviewitem)
                            treeviewitem.IsExpanded = True
                        Else
                            ttreeitem.Items.Insert(titem.Position(0), treeviewitem)
                            treeviewitem.IsExpanded = True
                        End If

                    Next
            End Select

            currentWork -= 1

            MainTreeview.Focus()
        End If
    End Sub
    Public Sub Redo()
        If currentWork < RecentWork.Count - 1 Then
            currentWork += 1

            Select Case RecentWork(currentWork).WType
                Case EUD_Editor_3.RecentWork.WorkType.ScriptInsert
                    For i = 0 To RecentWork(currentWork).items.Count - 1
                        Dim titem As ScriptTreeviewItem = RecentWork(currentWork).items(i)

                        Dim ttreeitem As TreeViewItem = SelectTreeview(titem.Position, 1)

                        Dim copyscript As New ScriptBlock(titem.Script)
                        Dim treeviewitem As TreeViewItem = GetTreeviewItem(copyscript)


                        If ttreeitem Is Nothing Then
                            MainTreeview.Items.Insert(titem.Position(0), treeviewitem)
                            treeviewitem.IsExpanded = True
                        Else
                            ttreeitem.Items.Insert(titem.Position(0), treeviewitem)
                            treeviewitem.IsExpanded = True
                        End If

                    Next
                Case EUD_Editor_3.RecentWork.WorkType.ScriptRemove
                    For i = 0 To RecentWork(currentWork).items.Count - 1
                        Dim titem As ScriptTreeviewItem = RecentWork(currentWork).items(i)

                        Dim ttreeitem As TreeViewItem = SelectTreeview(titem.Position)


                        CType(ttreeitem.Header, ScriptTreeviewItem).Delete(False)
                    Next
            End Select

            MainTreeview.Focus()
        End If
    End Sub

    Public Sub SelectLast(tSelectedScript As ScriptTreeviewItem)
        Dim treeviewitem As TreeViewItem = SelectTreeview(tSelectedScript.Position, 1)
        Dim itemcollection As ItemCollection

        Dim lastint As Integer = tSelectedScript.Position(0)


        If treeviewitem IsNot Nothing Then
            itemcollection = treeviewitem.Items
        Else
            itemcollection = MainTreeview.Items
        End If



        If itemcollection.Count <> 0 Then
            If itemcollection.Count > lastint Then
                treeviewitem = itemcollection(lastint)

            Else
                treeviewitem = itemcollection(itemcollection.Count - 1)
            End If
        End If


        If treeviewitem IsNot Nothing Then
            treeviewitem.IsSelected = True
            Dim scriptitem As ScriptTreeviewItem = treeviewitem.Header
            'scriptitem.Selectthis(0)
            SelectScript(scriptitem, 0)
            SelectedScript = scriptitem
        End If


        'MainTreeview.Focus()
    End Sub

    Private Function SelectTreeview(points As List(Of Integer), Optional LastItemsUse As Integer = 0) As TreeViewItem
        Dim ttreeitem As TreeViewItem = Nothing
        For j = points.Count - 1 To LastItemsUse Step -1
            If j = points.Count - 1 Then
                ttreeitem = MainTreeview.Items(points(j))
            Else
                ttreeitem = ttreeitem.Items(points(j))
            End If
            'MsgBox(titem.Position(j))
        Next

        Return ttreeitem
    End Function

End Class

Public Class RecentWork
    Public WType As WorkType
    Public Enum WorkType
        ValueChange
        ValueInsert
        ScriptInsert
        ScriptRemove 'Pos데이터 필요.
        ScriptMove
    End Enum

    Public items As List(Of ScriptTreeviewItem)
    Public Value As List(Of Integer)

    Public index As Integer

    Public Sub New(twType As WorkType, index As Integer, TreeviewItem As List(Of ScriptTreeviewItem), Values As List(Of Integer))
        WType = twType
        items = TreeviewItem
        Value = Values

        For i = 0 To TreeviewItem.Count - 1
            TreeviewItem(i).GetPosition()
        Next
    End Sub

    '값변경
    '변경전 변경후 변경한 스크립트

    '삭제
    '삭제한 블럭 위치 삭제된 스크립트
    '취소 시 블럭 위치에 생성

    '생성
    '생성한 블럭
    '취소 시 생성한 블럭 삭제.
End Class