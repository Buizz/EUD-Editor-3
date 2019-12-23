Public Class ScriptTreeviewItem
    Public Property DragIndex As Integer

    '연결된 스크립트블럭
    Public Property Script As ScriptBlock
    Private pScriptEditor As GUIScriptEditorUI
    Private tisValue As Boolean

    Public Sub New(tparrent As GUIScriptEditorUI, tScript As ScriptBlock, Optional tIsValue As Boolean = False)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        Script = tScript
        pScriptEditor = tparrent
        Me.tisValue = tIsValue
        Position = New List(Of Integer)

        ContentPanel.Init(pScriptEditor, Script, tIsValue)
    End Sub
    Public Sub Reload(tScript As ScriptBlock)
        Script = tScript
    End Sub


    Private level As Byte



    Public Position As List(Of Integer)
    Public Sub GetPosition()
        Position.Clear()

        Dim Treeview As TreeViewItem = Parent
        Dim ParentItemCollection As ItemCollection

        While (Treeview.Parent.GetType IsNot GetType(TreeView))
            Dim pTreeview As TreeViewItem = Treeview.Parent
            ParentItemCollection = pTreeview.Items

            Position.Add(ParentItemCollection.IndexOf(Treeview))

            Treeview = pTreeview
        End While


        Dim parentTreeview As TreeView = Treeview.Parent
        Position.Add(parentTreeview.Items.IndexOf(Treeview))
    End Sub
    'Public Sub GetPosition(parent As TreeViewItem, pos As Integer)
    '    Position.Clear()

    '    Position.Add(pos)
    '    Dim Treeview As TreeViewItem = parent
    '    Dim ParentItemCollection As ItemCollection

    '    While (Treeview.Parent.GetType IsNot GetType(TreeView))
    '        Dim pTreeview As TreeViewItem = Treeview.Parent
    '        ParentItemCollection = pTreeview.Items



    '        Position.Add(ParentItemCollection.IndexOf(Treeview))

    '        Treeview = pTreeview
    '    End While


    '    Dim parentTreeview As TreeView = Treeview.Parent
    '    Position.Add(parentTreeview.Items.IndexOf(Treeview))
    'End Sub


    Public Function IsValue()
        If Me.Parent.GetType Is GetType(TreeViewItem) Then
            Return False
        End If

        Return True
    End Function




    Private Sub Grid_PreviewMouseMove(sender As Object, e As MouseEventArgs)
        If pScriptEditor.LastOverScriptTreeviewItem IsNot Me Then
            If SelectLevel = 0 Or SelectLevel = 3 Then
                TopBorder.Visibility = Visibility.Hidden
            End If
            If SelectLevel <= 1 Then
                BottomBorder.Visibility = Visibility.Hidden
            End If
            Return
        End If



        Dim acturalHeight As Integer = mainGrid.ActualHeight

        If Not isMouseDown Then
            If IsValue() Then
                level = 1
                TopBorder.Visibility = Visibility.Visible
                BottomBorder.Visibility = Visibility.Visible
                Exit Sub
            End If

            If Script.TriggerScript.IsFolder Then
                If Script.TriggerScript.IsLock Then
                    level = 1
                    TopBorder.Visibility = Visibility.Visible
                    BottomBorder.Visibility = Visibility.Visible
                Else
                    Select Case e.GetPosition(mainGrid).Y
                        Case 0 To acturalHeight / 8 * 3
                            level = 0

                            TopBorder.Visibility = Visibility.Visible
                            If SelectLevel <= 1 Then
                                BottomBorder.Visibility = Visibility.Hidden
                            End If
                        Case acturalHeight / 8 * 3 To acturalHeight / 8 * 5
                            level = 1
                            TopBorder.Visibility = Visibility.Visible
                            BottomBorder.Visibility = Visibility.Visible
                        Case acturalHeight / 8 * 5 To acturalHeight
                            level = 2
                            If SelectLevel = 0 Or SelectLevel = 3 Then
                                TopBorder.Visibility = Visibility.Hidden
                            End If
                            BottomBorder.Visibility = Visibility.Visible
                    End Select
                End If


            Else
                Select Case e.GetPosition(mainGrid).Y
                    Case 0 To acturalHeight / 2
                        level = 0

                        TopBorder.Visibility = Visibility.Visible
                        If SelectLevel <= 1 Then
                            BottomBorder.Visibility = Visibility.Hidden
                        End If
                    Case acturalHeight / 2 To acturalHeight
                        level = 2
                        If SelectLevel = 0 Or SelectLevel = 3 Then
                            TopBorder.Visibility = Visibility.Hidden
                        End If
                        BottomBorder.Visibility = Visibility.Visible
                End Select
            End If
        End If
    End Sub

    Private Sub MainGrid_MouseEnter(sender As Object, e As MouseEventArgs)
        pScriptEditor.AddOverScriptTreeviewItem(Me)
    End Sub

    Private Sub MainGrid_MouseLeave(sender As Object, e As MouseEventArgs)
        pScriptEditor.RemoveOverScriptTreeviewItem(Me)


        If SelectLevel = 0 Or SelectLevel = 3 Then
            TopBorder.Visibility = Visibility.Hidden
        End If
        If SelectLevel <= 1 Then
            BottomBorder.Visibility = Visibility.Hidden
        End If

        'mainGrid.Background = New SolidColorBrush(Color.FromArgb(1, 128, 128, 128))
        isMouseDown = False
    End Sub

    Private isMouseDown As Boolean
    Private Sub MainGrid_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        'mainGrid.Background = New SolidColorBrush(Color.FromArgb(150, 100, 100, 170))
        isMouseDown = True
    End Sub


    Public SelectLevel As Byte

    Private Sub MainGrid_PreviewMouseUp(sender As Object, e As MouseButtonEventArgs)
        isMouseDown = False
        pScriptEditor.SelectScript(Me, level)
    End Sub

    Public Sub Selectthis(level As Byte)
        SelectLevel = level + 1
        If Script.TriggerScript.IsLock Then
            '잠겨있을 경우 위 아래에 추가 불가.

            If Script.TriggerScript.IsFolder Then
                SelectLevel = 2
            Else
                '폴더도 아닐 경우 선택불가
                SelectLevel = 0
                Exit Sub
            End If
        End If
        If Script.TriggerScript.IsChildLock Then
            If SelectLevel = 2 Then
                SelectLevel = 1
            End If
        End If

        If IsValue() Then
            SelectLevel = 2
        End If


        Select Case SelectLevel
            Case 1
                TopBorder.Opacity = 1
                BottomBorder.Opacity = 0.5
                TopBorder.Visibility = Visibility.Visible
                BottomBorder.Visibility = Visibility.Hidden
            Case 2
                TopBorder.Opacity = 1
                BottomBorder.Opacity = 1
                TopBorder.Visibility = Visibility.Visible
                BottomBorder.Visibility = Visibility.Visible
            Case 3
                TopBorder.Opacity = 0.5
                BottomBorder.Opacity = 1
                TopBorder.Visibility = Visibility.Hidden
                BottomBorder.Visibility = Visibility.Visible
        End Select
    End Sub
    Public Sub dSelectthis()


        SelectLevel = 0
        TopBorder.Opacity = 0.5
        BottomBorder.Opacity = 0.5
        TopBorder.Visibility = Visibility.Hidden
        BottomBorder.Visibility = Visibility.Hidden
    End Sub


    '#01808080
    Public Sub MulitSelect()
        If pScriptEditor.AddMulitSelectItem(Me) Then
            mainGrid.Background = New SolidColorBrush(Color.FromArgb(150, 100, 100, 170))
        Else
            mainGrid.Background = New SolidColorBrush(Color.FromArgb(1, &H80, &H80, &H80))
        End If
    End Sub
    Public Sub dMulitSelect()
        mainGrid.Background = New SolidColorBrush(Color.FromArgb(1, &H80, &H80, &H80))
        pScriptEditor.DeleteMulitSelectItem(Me)
    End Sub


    Public Function Delete(Optional isWorkAdd As Boolean = True) As Boolean
        If tisValue Then
            Return False
        End If
        If Script.TriggerScript.IsLock Then
            Return False
        End If

        Dim ttreeviewitem As TreeViewItem = Me.Parent

        If pScriptEditor.SelectedScript Is Me Then
            pScriptEditor.SelectedScript = Nothing
        End If

        If isWorkAdd Then
            pScriptEditor.RemoveWork(Me)
        End If
        Try
            If ttreeviewitem.Parent.GetType Is GetType(TreeViewItem) Then
                Dim parenttreeviewitem As TreeViewItem = ttreeviewitem.Parent
                parenttreeviewitem.Items.Remove(ttreeviewitem)
            Else
                Dim parenttreeviewitem As TreeView = ttreeviewitem.Parent
                parenttreeviewitem.Items.Remove(ttreeviewitem)
            End If
        Catch ex As Exception
            If isWorkAdd Then
                pScriptEditor.RemoveCancel()
            End If
            Return False
        End Try


        If Script.TriggerScript.SName = "FunctionDefinition" Then
            pScriptEditor.ObjectSelecter.ToolBoxListRefresh("Func")
        End If

        If Script.TriggerScript.SName = "FuncUse" Then
            pScriptEditor.DeleteFuncUseList(Me)
        End If




        Return True
    End Function



    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub

End Class
