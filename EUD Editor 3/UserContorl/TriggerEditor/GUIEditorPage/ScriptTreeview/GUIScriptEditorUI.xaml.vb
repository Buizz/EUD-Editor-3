Public Class GUIScriptEditorUI
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
        If Script.items.Count = 0 Then

            MainTreeview.Items.Add(GetTreeviewItem("If"))
            MainTreeview.Items.Add(GetTreeviewItem("CreateUnit"))
        End If

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
        treeitem.Header = New ScriptTreeviewItem(Me, New ScriptBlock(key))
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
            Return False
        End If

        Select Case parrentScript.FolderType
            Case TriggerScript.ScriptType.Both
                If childScript.SType <> TriggerScript.ScriptType.Action Or childScript.SType <> TriggerScript.ScriptType.Condition Then
                    Return False
                End If
            Case TriggerScript.ScriptType.Action
                If childScript.SType <> TriggerScript.ScriptType.Action Then
                    Return False
                End If
            Case TriggerScript.ScriptType.Condition
                If childScript.SType <> TriggerScript.ScriptType.Condition Then
                    Return False
                End If
            Case TriggerScript.ScriptType.Special
                If childScript.SType <> TriggerScript.ScriptType.Special Then
                    Return False
                End If
            Case TriggerScript.ScriptType.Null, TriggerScript.ScriptType.Special
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
                        Return False
                    End If
                Next
            End If
        End If


        Return True
    End Function

    Public Sub AddItemClick(keyname As String)
        Dim tvitem As TreeViewItem = GetTreeviewItem(keyname)

        If SelectedScript IsNot Nothing Then
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
                        Dim ttreeview As TreeView = insertitem.Parent
                        itemcollcteion = ttreeview.Items
                        PlaceAble = True
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
                    insertitem.IsExpanded = True
                Else
                    itemcollcteion.Insert(tindex, tvitem)
                End If
            End If

        End If
        'MainTreeview.Items.Add(New ScriptTreeviewItem(Me, New ScriptBlock(keyname)))
        'MsgBox("스크립터에서 받음 : " & keyname)
    End Sub


    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub

    Private SelectedScript As ScriptTreeviewItem
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
    Private Sub MainTreeview_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.LeftCtrl Then
            leftCtrlDown = True
        End If
    End Sub

    Private Sub MainTreeview_PreviewKeyUp(sender As Object, e As KeyEventArgs)
        If e.Key = Key.LeftCtrl Then
            leftCtrlDown = False
        End If
    End Sub

    Private Sub MainTreeview_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object))
        If leftCtrlDown Then
            Dim treeviewitem As TreeViewItem = e.NewValue
            Dim ScriptTreeview As ScriptTreeviewItem = treeviewitem.Header
            ScriptTreeview.MulitSelect()
            'treeviewitem.Background = New SolidColorBrush(Color.FromArgb(150, 100, 100, 170))
        End If
    End Sub
End Class
