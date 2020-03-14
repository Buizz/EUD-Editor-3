Partial Public Class GUIScriptEditorUI
    Public Sub AddItemClick(tagname As String)
        Dim selectitem As TreeViewItem = MainTreeview.SelectedItem
        Dim ssb As ScriptBlock = Nothing
        Dim addindex As Integer = -1


        If selectitem IsNot Nothing Then
            ssb = selectitem.Tag
            If Not ssb.IsFolderScript Then
                If selectitem.Parent.GetType Is GetType(TreeView) Then
                    addindex = Script.items.IndexOf(ssb) + selpos

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

        Dim nsb As New ScriptBlock(tagname, True, False, "", Script)
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
                If Not OpenNewWindow(nsb, ntreeview) Then
                    selectitem.IsExpanded = True
                    If addindex = -1 Then
                        ssb.AddChild(nsb)
                        selectitem.Items.Add(ntreeview)
                    Else
                        ssb.InsertChild(addindex, nsb)
                        selectitem.Items.Insert(addindex, ntreeview)
                    End If
                End If
            End If
        Else
            If CheckValidated(nsb, Nothing) Then
                Dim ntreeview As TreeViewItem = nsb.GetTreeviewitem
                If Not OpenNewWindow(nsb, ntreeview) Then
                    If addindex = -1 Then
                        Script.items.Add(nsb)
                        MainTreeview.Items.Add(ntreeview)
                    Else
                        Script.items.Insert(addindex, nsb)
                        MainTreeview.Items.Insert(addindex, ntreeview)
                    End If
                End If
            End If
        End If
    End Sub

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
    Private Function OpenNewWindow(scr As ScriptBlock, ntreeview As TreeViewItem) As Boolean
        Dim scriptEdit As New GUIScriptEditerWindow(scr, ntreeview, IsCreateOpen, Me)

        If Not scriptEdit.Init Then
            Return False
        End If

        AddHandler scriptEdit.OkayBtnEvent, AddressOf OkayBtnEvent
        AddHandler scriptEdit.CancelBtnEvent, AddressOf CancelBtnEvent

        If MainTreeview.SelectedItem IsNot Nothing Then
            Dim tv As TreeViewItem = MainTreeview.SelectedItem



            TEGUIPage.OpenEditWindow(scriptEdit, tv.TransformToAncestor(MainTreeview).Transform(New Point(0, 0)))
        Else
            TEGUIPage.OpenEditWindow(scriptEdit, Nothing)
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
                End If
            Else
                If CheckValidated(insertScriptBlock, Nothing) Then
                    If insertIndex = -1 Then
                        Script.items.Add(insertScriptBlock)
                        MainTreeview.Items.Add(ntreeview)
                    Else
                        Script.items.Insert(insertIndex, insertScriptBlock)
                        MainTreeview.Items.Insert(insertIndex, ntreeview)
                    End If
                End If
            End If
        End If

        Select Case scr.name
            Case "function"
                TEGUIPage.ObjectSelector.RefreshCurrentList()
            Case Else
                If scr.IsFunction Then
                    TEGUIPage.ObjectSelector.RefreshCurrentList()
                End If
        End Select



        TEGUIPage.CloseEditWindow()
    End Sub
    Public Sub CancelBtnEvent()


        TEGUIPage.CloseEditWindow()
    End Sub
End Class
