Public Class GUI_Var
    Private valueEditPanel As GUI_ArgEditor
    Private isload As Boolean = False


    Private EditValues As ScriptBlock
    Public Sub CrlInit()
        '//////////////////////////////
        '초기화 식
        'Dim values As List(Of String) = GUIScriptManager.SplitText(scr.value)
        vname.Text = scr.value




        valueEditPanel = New GUI_ArgEditor
        valueEditPanel.Margin = New Thickness(5)
        DockPanel.SetDock(valueEditPanel, Dock.Top)


        MainStackPanel.Children.Add(valueEditPanel)
        valueEditPanel.Init(p._GUIScriptEditorUI.TEGUIPage.ValueSelecter, dotscr, p._GUIScriptEditorUI, True)
        AddHandler valueEditPanel.BtnRefresh, AddressOf AgrbtnRefresh


        valueEditPanel.Visibility = Visibility.Collapsed
        varObjectBorder.Visibility = Visibility.Collapsed
        initcb.Visibility = Visibility.Collapsed

        If scr.child.Count > 0 Then
            EditValues = scr.child(0).DeepCopy
        End If

        Select Case scr.value2
            Case "var"
                VarType.SelectedIndex = 0
                initcb.Visibility = Visibility.Visible
                If scr.child.Count = 0 Then
                    initcb.IsChecked = False
                    valueEditPanel.Visibility = Visibility.Collapsed
                Else
                    initcb.IsChecked = True
                    valueEditPanel.Visibility = Visibility.Visible
                    valueEditPanel.ComboboxInit(scr.child(0))
                End If
            Case "static"
                VarType.SelectedIndex = 1
                initcb.Visibility = Visibility.Visible
                If scr.child.Count = 0 Then
                    initcb.IsChecked = False
                    valueEditPanel.Visibility = Visibility.Collapsed
                Else
                    initcb.IsChecked = True
                    valueEditPanel.Visibility = Visibility.Visible
                    valueEditPanel.ComboboxInit(scr.child(0))
                End If
            Case "const"
                VarType.SelectedIndex = 2
                valueEditPanel.Visibility = Visibility.Visible
                valueEditPanel.ComboboxInit(scr.child(0))
            Case "object"
                VarType.SelectedIndex = 3
                varObjectBorder.Visibility = Visibility.Visible
                valuetb.Visibility = Visibility.Collapsed
                If EditValues Is Nothing Then
                    ObjectFunc.Visibility = Visibility.Collapsed
                    varFunction.Visibility = Visibility.Collapsed
                Else
                    ObjectFunc.Visibility = Visibility.Visible
                    varFunction.Visibility = Visibility.Visible



                    Select Case EditValues.value
                        Case "constructor"
                            ObjectFunc.SelectedIndex = 0
                        Case "cast"
                            ObjectFunc.SelectedIndex = 1
                        Case "alloc"
                            ObjectFunc.SelectedIndex = 2
                    End Select


                    varFunction.CrlInit(EditValues, dotscr, p._GUIScriptEditorUI, valueEditPanel)
                End If
                Dim ListSelecter As New GUI_ObjectSelecter(p._GUIScriptEditorUI.Script)

                p._GUIScriptEditorUI.TEGUIPage.ValueSelecter.Child = ListSelecter
                p._GUIScriptEditorUI.TEGUIPage.ValueSelecter.Visibility = Visibility.Visible

                AddHandler ListSelecter.SelectEvent, AddressOf ObjectSelect
                'varFunction.ComboboxInit(EditValues)
        End Select


        isload = True
    End Sub
    Public Sub AgrbtnRefresh(sender As String, e As RoutedEventArgs)
        'sender.Last 선택한 값
        valuetb.Text = EditValues.ValueCoder
    End Sub

    Public Sub OkayAction(sender As Object, e As RoutedEventArgs)
        '//////////////////////////////
        '스크립트 갱신
        scr.value = vname.Text

        If EditValues Is Nothing Then
            scr.child.Clear()
        Else
            scr.child.Clear()
            scr.AddChild(EditValues)
        End If

        Select Case VarType.SelectedIndex
            Case 0
                scr.value2 = "var"
                scr.flag = False
            Case 1
                scr.value2 = "static"
                scr.flag = True
            Case 2
                scr.value2 = "const"
                scr.flag = True
            Case 3
                scr.value2 = "object"
                scr.flag = True
        End Select


    End Sub
    Private Function CheckEditable() As Boolean
        '//////////////////////////////
        '확인을 누를 수 있는지 확인
        If vname.Text.Trim = "" Then
            Return False
        End If


        If VarType.SelectedIndex = 3 Then
            If EditValues Is Nothing Then
                Return False
            End If
        End If


        Return True
    End Function


    Private p As GUIScriptEditerWindow
    Private scr As ScriptBlock
    Private dotscr As ScriptBlock
    Public Sub New(tp As GUIScriptEditerWindow, tscr As ScriptBlock, _dotscr As ScriptBlock)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        p = tp
        scr = tscr
        dotscr = _dotscr

        AddHandler p.OkayBtnEvent, AddressOf OkayAction

        CrlInit()
        btnRefresh()
    End Sub
    Public Sub btnRefresh()
        If CheckEditable() Then
            p.OkBtn.IsEnabled = True
        Else
            p.OkBtn.IsEnabled = False
        End If
    End Sub

    Private Sub initcb_Checked(sender As Object, e As RoutedEventArgs)
        If isload Then
            EditValues = New ScriptBlock(ScriptBlock.EBlockType.constVal, "Number", True, False, "1", p._GUIScriptEditorUI.Script)

            valueEditPanel.ComboboxInit(EditValues)
            valueEditPanel.Visibility = Visibility.Visible
        End If
    End Sub

    Private Sub initcb_Unchecked(sender As Object, e As RoutedEventArgs)
        If isload Then
            EditValues = Nothing
            valueEditPanel.Visibility = Visibility.Hidden
            p._GUIScriptEditorUI.TEGUIPage.ValueSelecter.Child = Nothing
            p._GUIScriptEditorUI.TEGUIPage.ValueSelecter.Visibility = Visibility.Collapsed
            valuetb.Text = ""
        End If
    End Sub

    Private Sub vname_TextChanged(sender As Object, e As TextChangedEventArgs)
        If isload Then
            btnRefresh()
        End If
    End Sub

    Private Sub VarType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If isload Then
            valueEditPanel.Visibility = Visibility.Collapsed
            varObjectBorder.Visibility = Visibility.Collapsed
            initcb.Visibility = Visibility.Collapsed
            valuetb.Visibility = Visibility.Collapsed
            p._GUIScriptEditorUI.TEGUIPage.ValueSelecter.Child = Nothing
            p._GUIScriptEditorUI.TEGUIPage.ValueSelecter.Visibility = Visibility.Collapsed
            Select Case VarType.SelectedIndex
                Case 0
                    valuetb.Visibility = Visibility.Visible
                    valueEditPanel.Visibility = Visibility.Collapsed
                    initcb.Visibility = Visibility.Visible
                    initcb.IsChecked = False
                    EditValues = Nothing
                    p._GUIScriptEditorUI.TEGUIPage.ValueSelecter.Child = Nothing
                    p._GUIScriptEditorUI.TEGUIPage.ValueSelecter.Visibility = Visibility.Collapsed
                Case 1
                    valuetb.Visibility = Visibility.Visible
                    valueEditPanel.Visibility = Visibility.Collapsed
                    initcb.Visibility = Visibility.Visible
                    initcb.IsChecked = False
                    EditValues = Nothing
                    p._GUIScriptEditorUI.TEGUIPage.ValueSelecter.Child = Nothing
                    p._GUIScriptEditorUI.TEGUIPage.ValueSelecter.Visibility = Visibility.Collapsed
                Case 2
                    valuetb.Visibility = Visibility.Visible
                    valueEditPanel.Visibility = Visibility.Visible
                    EditValues = New ScriptBlock(ScriptBlock.EBlockType.constVal, "Number", True, False, "1", p._GUIScriptEditorUI.Script)

                    valueEditPanel.ComboboxInit(EditValues)
                Case 3
                    varObjectBorder.Visibility = Visibility.Visible

                    ObjectFunc.Visibility = Visibility.Collapsed
                    varFunction.Visibility = Visibility.Collapsed
                    'varFunction.ComboboxInit(EditValues)
                    EditValues = Nothing

                    Dim ListSelecter As New GUI_ObjectSelecter(p._GUIScriptEditorUI.Script)



                    p._GUIScriptEditorUI.TEGUIPage.ValueSelecter.Child = ListSelecter
                    p._GUIScriptEditorUI.TEGUIPage.ValueSelecter.Visibility = Visibility.Visible

                    AddHandler ListSelecter.SelectEvent, AddressOf ObjectSelect


                    'EditValues = New ScriptBlock(ScriptBlock.EBlockType.varuse, "StringBuffer", True, False, "constructor", p._GUIScriptEditorUI.Script)

                    'valueEditPanel.ComboboxInit(EditValues)
            End Select
        End If
        btnRefresh()
    End Sub

    Private ObjectfuncLoad As Boolean = True
    Private Sub ObjectSelect(sender As Object, e As RoutedEventArgs)
        If isload Then
            ObjectfuncLoad = False
            EditValues = New ScriptBlock(ScriptBlock.EBlockType.varuse, sender, True, False, "constructor", p._GUIScriptEditorUI.Script)

            ObjectFunc.Visibility = Visibility.Visible
            ObjectFunc.SelectedIndex = 0

            varFunction.Visibility = Visibility.Visible
            varFunction.CrlInit(EditValues, dotscr, p._GUIScriptEditorUI, valueEditPanel)

            btnRefresh()
            ObjectfuncLoad = True
            'MsgBox("오브젝트선택")
        End If
    End Sub

    Private Sub ObjectFunc_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If isload And ObjectfuncLoad Then
            If ObjectFunc.SelectedItem IsNot Nothing Then
                EditValues.value = CType(ObjectFunc.SelectedItem, ComboBoxItem).Tag
                varFunction.CrlInit(EditValues, dotscr, p._GUIScriptEditorUI, valueEditPanel)
                btnRefresh()
            End If
        End If
    End Sub
End Class
