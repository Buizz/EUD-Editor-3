Partial Public Class GUIScriptEditorUI
    Public Sub AddItemClick(type As ScriptBlock.EBlockType, tagname As String)
        Dim selectitem As TreeViewItem = MainTreeview.SelectedItem
        Dim normalscr As ScriptBlock = Nothing
        Dim ssb As ScriptBlock = Nothing
        Dim addindex As Integer = -1



        If selectitem IsNot Nothing Then
            normalscr = selectitem.Tag
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

        Dim nsb As New ScriptBlock(type, tagname, True, False, "", Script)


        nsb.Parent = ssb

        insertIndex = addindex
        insertScriptBlock = nsb
        parentScriptBlock = ssb
        parentTreeviewitem = selectitem
        selectitemIsNothing = selectitem IsNot Nothing
        IsCreateOpen = True

        If selectitem IsNot Nothing Then
            If CheckValidated(nsb, ssb) Then
                Dim ntreeview As TreeViewItem = nsb.GetTreeviewitem

                If OpenNewWindow(nsb, ntreeview, normalscr) Then
                    Return
                End If

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
                SetRepeatCount(1)

                MainTreeview.UpdateLayout()

                ntreeview.IsSelected = True
            End If
        Else
            If CheckValidated(nsb, MainScr) Then
                Dim ntreeview As TreeViewItem = nsb.GetTreeviewitem

                If OpenNewWindow(nsb, ntreeview, normalscr) Then
                    Return
                End If

                If addindex = -1 Then
                    AddItems(nsb)
                    TreeAddItems(ntreeview)
                    'MainTreeview.Items.Add(ntreeview)
                Else
                    InsertItems(addindex, nsb)
                    TreeAddItems(ntreeview)
                    'MainTreeview.Items.Insert(addindex, ntreeview)
                End If
                'MsgBox("아이템생성 새 창없이")
                AddInsertTask(ntreeview)
                SetRepeatCount(1)

                MainTreeview.UpdateLayout()

                ntreeview.IsSelected = True
            End If
        End If
    End Sub
    Public Sub TreeAddItems(ntreeview As TreeViewItem)
        Dim Scr As ScriptBlock = ntreeview.Tag




        For i = 0 To MainItems.Count - 1
            If Scr Is MainItems(i) Then
                MainTreeview.Items.Insert(i, ntreeview)
                Exit For
            End If
        Next


        TEGUIPage.globalObjectListRefreah()
    End Sub
    'Public Sub TreeInsertItems(index As Integer, ntreeview As TreeViewItem)
    '    Dim Scr As ScriptBlock = ntreeview.Tag



    '    MainTreeview.Items.Insert(index, ntreeview)
    '    TEGUIPage.globalObjectListRefreah()
    'End Sub





    Public insertIndex As Integer
    Public insertScriptBlock As ScriptBlock
    Public parentScriptBlock As ScriptBlock
    Public parentTreeviewitem As TreeViewItem
    Public selectitemIsNothing As Boolean
    Public IsCreateOpen As Boolean



    Private Sub OpenEditWindow(ntreeview As TreeViewItem)
        IsCreateOpen = False

        OpenNewWindow(ntreeview.Tag, ntreeview)
    End Sub

    Private LastScrOrgin As ScriptBlock
    Private LastScr As ScriptBlock
    Private LastTreeview As TreeViewItem
    Private Function OpenNewWindow(scr As ScriptBlock, ntreeview As TreeViewItem, Optional dotscriptblock As ScriptBlock = Nothing) As Boolean
        LastScrOrgin = scr.DeepCopy
        LastScr = scr
        LastTreeview = ntreeview
        'MsgBox(LastScr.ValueCoder)

        If dotscriptblock Is Nothing Then
            dotscriptblock = scr
        End If

        Dim scriptEdit As New GUIScriptEditerWindow(scr, ntreeview, dotscriptblock, IsCreateOpen, Me)

        If Not scriptEdit.Init Then
            Return False
        End If

        AddHandler scriptEdit.OkayBtnEvent, AddressOf OkayBtnEvent
        AddHandler scriptEdit.CancelBtnEvent, AddressOf CancelBtnEvent

        If IsCreateOpen Then
            TEGUIPage.OpenEditWindow(scriptEdit, Nothing)
        Else
            If MainTreeview.SelectedItem IsNot Nothing Then
                Dim tv As TreeViewItem = MainTreeview.SelectedItem

                'TEGUIPage.OpenEditWindow(scriptEdit, Nothing)
                TEGUIPage.OpenEditWindow(scriptEdit, tv.TransformToAncestor(MainTreeview).Transform(New Point(0, 0)))
            Else
                TEGUIPage.OpenEditWindow(scriptEdit, Nothing)
            End If
        End If

        Return True
    End Function

    Public Sub OkayBtnEvent(scr As ScriptBlock, e As RoutedEventArgs)
        If IsCreateOpen Then
            Dim ntreeview As TreeViewItem = insertScriptBlock.GetTreeviewitem
            If selectitemIsNothing Then
                If CheckValidated(insertScriptBlock, parentScriptBlock) Then
                    parentTreeviewitem.IsExpanded = True
                    If insertIndex = -1 Then
                        parentScriptBlock.AddChild(insertScriptBlock)
                        parentTreeviewitem.Items.Add(ntreeview)
                    Else
                        parentScriptBlock.InsertChild(insertIndex, insertScriptBlock)
                        parentTreeviewitem.Items.Insert(insertIndex, ntreeview)
                    End If
                    'MsgBox("아이템생성 창 있음")
                    AddInsertTask(ntreeview)
                    SetRepeatCount(1)


                    MainTreeview.UpdateLayout()

                    ntreeview.IsSelected = True
                End If
            Else
                If CheckValidated(insertScriptBlock, Nothing) Then
                    If insertIndex = -1 Then
                        AddItems(insertScriptBlock)
                        TreeAddItems(ntreeview)
                    Else
                        InsertItems(insertIndex, insertScriptBlock)
                        TreeAddItems(ntreeview)
                    End If
                    'MsgBox("아이템생성 창 있음")
                    AddInsertTask(ntreeview)
                    SetRepeatCount(1)


                    MainTreeview.UpdateLayout()

                    ntreeview.IsSelected = True
                End If
            End If
        Else
            Script.ExternLoader()
        End If

        Select Case scr.ScriptType
            Case ScriptBlock.EBlockType.fundefine, ScriptBlock.EBlockType.vardefine,
                 ScriptBlock.EBlockType.action, ScriptBlock.EBlockType.condition,
                 ScriptBlock.EBlockType.plibfun, ScriptBlock.EBlockType.externfun,
                 ScriptBlock.EBlockType.funuse, ScriptBlock.EBlockType.import
                TEGUIPage.ObjectSelector.RefreshCurrentList()
        End Select
        If Not IsCreateOpen Then
            'MsgBox("오케이이벤트 : " & scr.ValueCoder & vbCrLf & "바뀌기 전 이벤트 : " & LastScrOrgin.ValueCoder)
            AddEditTask(scr, LastScrOrgin, LastTreeview)
            SetRepeatCount(1)
        End If



        TEGUIPage.CloseEditWindow()
    End Sub
    Public Sub CancelBtnEvent()
        LastScr.DuplicationBlock(LastScrOrgin)


        TEGUIPage.CloseEditWindow()
    End Sub
End Class
