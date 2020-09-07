Public Class GUI_Var
    Private isload As Boolean = False

    Private EditValues As New List(Of ScriptBlock)


    Private Sub VarFuncRefresh(sender As Object, e As RoutedEventArgs)
        If SelectBtn IsNot Nothing Then
            SelectBtn.Content = CType(SelectBtn.Tag, ScriptBlock).ValueCoder
        End If
    End Sub


    Public Sub CrlInit()
        '//////////////////////////////
        '초기화 식
        'Dim values As List(Of String) = GUIScriptManager.SplitText(scr.value)
        vname.Text = scr.value

        valueEditPanel.Init(ValueSelecter, dotscr, p._GUIScriptEditorUI, True)
        AddHandler valueEditPanel.BtnRefresh, AddressOf AgrbtnRefresh


        AddHandler varFunction.RefreshEvent, AddressOf VarFuncRefresh


        For i = 0 To scr.child.Count - 1
            EditValues.Add(scr.child(i).DeepCopy)
        Next




        Select Case scr.value2
            Case "var"
                _VariableType = VariableType.Variable
            Case "static"
                _VariableType = VariableType.Static_
            Case "const"
                _VariableType = VariableType.Const_
            Case "object"
                _VariableType = VariableType.Object_
        End Select


        CrlRefresh()
        isload = True
    End Sub

    Private _VariableType As VariableType
    Private Enum VariableType
        Variable
        Static_
        Const_
        Object_
    End Enum
    Private ReadOnly Property IsHaveInit As Boolean
        Get
            Return (EditValues.Count > 0)
        End Get
    End Property


    Private Sub CrlRefresh()
        '기초 공사도 여기서 한다.
        SelectBtn = Nothing
        valueEditPanel.Visibility = Visibility.Collapsed
        varObjectBorder.Visibility = Visibility.Collapsed
        ObjectFunc.Visibility = Visibility.Collapsed
        ValueSelecter.Visibility = Visibility.Collapsed
        varFunction.Visibility = Visibility.Collapsed
        EditValuesPanel.Visibility = Visibility.Visible
        initcb.Visibility = Visibility.Collapsed
        Select Case _VariableType
            Case VariableType.Variable
                initcb.Visibility = Visibility.Visible
                VarType.SelectedIndex = 0

                initcb.IsChecked = IsHaveInit
            Case VariableType.Static_
                initcb.Visibility = Visibility.Visible
                VarType.SelectedIndex = 1

                initcb.IsChecked = IsHaveInit
            Case VariableType.Const_
                VarType.SelectedIndex = 2
            Case VariableType.Object_
                VarType.SelectedIndex = 3
                varObjectBorder.Visibility = Visibility.Visible
        End Select

        Dim vstr As String = vname.Text

        Dim vargs() As String = vstr.Split(",")
        Dim vcount As Integer = vargs.Count

        If Not IsHaveInit And (_VariableType = VariableType.Variable Or _VariableType = VariableType.Static_) Then
            EditValuesPanel.Visibility = Visibility.Collapsed
            Return
        End If
        '값목록을 갱신합니다.
        If vcount > EditValues.Count Then
            '값이 더 적어서 생산해야됨
            For i = 1 To vcount - EditValues.Count
                If _VariableType = VariableType.Object_ Then
                    EditValues.Add(New ScriptBlock(ScriptBlock.EBlockType.varuse, "init", True, False, "init", Nothing))
                Else
                    EditValues.Add(New ScriptBlock(ScriptBlock.EBlockType.constVal, "Number", False, False, 0, Nothing))
                End If
            Next
        ElseIf vcount < EditValues.Count Then
            '값이 더 많아서 지워야 함
            EditValues.RemoveRange(vcount - 1, EditValues.Count - vcount)
        End If


        For i = 0 To EditValues.Count - 1
            If EditValues(i).ScriptType = ScriptBlock.EBlockType.varuse Then
                If _VariableType <> VariableType.Object_ Then
                    EditValues(i).DuplicationBlock(New ScriptBlock(ScriptBlock.EBlockType.constVal, "Number", False, False, 0, Nothing))
                End If
            Else
                If _VariableType = VariableType.Object_ Then
                    EditValues(i).DuplicationBlock(New ScriptBlock(ScriptBlock.EBlockType.varuse, "init", True, False, "init", Nothing))
                End If
            End If


        Next



        EditValuesPanel.Children.Clear()
        For i = 0 To EditValues.Count - 1
            Dim btn As New Button
            btn.Content = EditValues(i).ValueCoder
            btn.Tag = EditValues(i)

            btn.Style = Application.Current.Resources("MaterialDesignRaisedAccentButton")
            btn.Margin = New Thickness(2)

            AddHandler btn.Click, AddressOf InitVBtnClick

            EditValuesPanel.Children.Add(btn)
        Next




        btnRefresh()
    End Sub

    Private SelectBtn As Button
    Private Sub InitVBtnClick(sender As Button, e As RoutedEventArgs)
        Dim btnscr As ScriptBlock = sender.Tag
        If btnscr.ScriptType = ScriptBlock.EBlockType.varuse Then
            '오브젝트임


            If btnscr.name = "init" Or btnscr.value = "init" Then
                SelectBtn = sender
                varFunction.Visibility = Visibility.Collapsed
                ObjectFunc.Visibility = Visibility.Collapsed
                Dim ListSelecter As New GUI_ObjectSelecter(p._GUIScriptEditorUI.Script)
                ValueSelecter.Child = ListSelecter
                ValueSelecter.Visibility = Visibility.Visible
                AddHandler ListSelecter.SelectEvent, AddressOf ObjectSelect

                Return
            End If

            If SelectBtn Is sender Then
                Dim ListSelecter As New GUI_ObjectSelecter(p._GUIScriptEditorUI.Script)
                ValueSelecter.Child = ListSelecter
                ValueSelecter.Visibility = Visibility.Visible
                AddHandler ListSelecter.SelectEvent, AddressOf ObjectSelect
            Else
                ValueSelecter.Visibility = Visibility.Collapsed
                ValueSelecter.Child = Nothing
            End If
            SelectBtn = sender


            ObjectFunc.Visibility = Visibility.Visible
            varFunction.Visibility = Visibility.Visible
            varFunction.CrlInit(btnscr, dotscr, p._GUIScriptEditorUI, valueEditPanel)

            Select Case btnscr.value
                Case "constructor"
                    ObjectFunc.SelectedIndex = 0

                    If btnscr.name = "EUDArray" Or btnscr.name = "EUDVArray" Then
                        SpFlag.Visibility = Visibility.Visible
                        SpFlag.IsChecked = btnscr.flag
                    End If

                Case "cast"
                    ObjectFunc.SelectedIndex = 1
                Case "alloc"
                    ObjectFunc.SelectedIndex = 2
            End Select
        Else
            SelectBtn = sender
            ValueSelecter.Visibility = Visibility.Visible
            valueEditPanel.Visibility = Visibility.Visible
            valueEditPanel.ComboboxInit(btnscr)
        End If
    End Sub
    Private Sub VarType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        '변수의 타입을 정한다
        If isload Then
            '변수
            '스태틱변수
            '상수
            '오브젝트
            Select Case VarType.SelectedIndex
                Case VariableType.Variable
                    _VariableType = VariableType.Variable
                    EditValues.Clear()
                Case VariableType.Static_
                    _VariableType = VariableType.Static_
                    EditValues.Clear()
                Case VariableType.Const_
                    _VariableType = VariableType.Const_
                Case VariableType.Object_
                    _VariableType = VariableType.Object_
            End Select
            CrlRefresh()
            btnRefresh()
        End If
    End Sub





    Public Sub AgrbtnRefresh(sender As String, e As RoutedEventArgs)
        'sender.Last 선택한 값
        'valuetb.Text = NNNNNNNNNNEditValues.ValueCoder
        If SelectBtn IsNot Nothing Then
            SelectBtn.Content = CType(SelectBtn.Tag, ScriptBlock).ValueCoder
        End If
    End Sub
    Private Function CheckEditable() As Boolean
        '//////////////////////////////
        '확인을 누를 수 있는지 확인
        If vname.Text.Trim = "" Then
            Return False
        End If
        Return True
    End Function



    Private Sub initcb_Checked(sender As Object, e As RoutedEventArgs)
        If isload Then
            If _VariableType = VariableType.Static_ Or _VariableType = VariableType.Variable Then
                EditValues.Add(New ScriptBlock(ScriptBlock.EBlockType.constVal, "Number", False, False, "0", Nothing))
                CrlRefresh()
            End If
        End If
    End Sub

    Private Sub initcb_Unchecked(sender As Object, e As RoutedEventArgs)
        If isload Then
            If _VariableType = VariableType.Static_ Or _VariableType = VariableType.Variable Then
                EditValues.Clear()
                CrlRefresh()
            End If
        End If
    End Sub

    Private Sub vname_TextChanged(sender As Object, e As TextChangedEventArgs)
        If isload Then
            CrlRefresh()
            btnRefresh()
        End If
    End Sub









    Private ObjectfuncLoad As Boolean = True
    Private Sub ObjectSelect(sender As Object, e As RoutedEventArgs)
        '리스트에서 오브젝트를 선택했을 때 뜨는 창.
        If isload Then
            Dim btnscr As ScriptBlock = SelectBtn.Tag

            ObjectfuncLoad = False
            btnscr.DuplicationBlock(New ScriptBlock(ScriptBlock.EBlockType.varuse, sender, True, False, "constructor", p._GUIScriptEditorUI.Script))

            ObjectFunc.Visibility = Visibility.Visible
            ObjectFunc.SelectedIndex = 0

            varFunction.Visibility = Visibility.Visible
            varFunction.CrlInit(btnscr, dotscr, p._GUIScriptEditorUI, valueEditPanel, True)


            ValueSelecter.Visibility = Visibility.Collapsed
            ValueSelecter.Child = Nothing
            btnRefresh()
            'CrlRefresh()
            ObjectfuncLoad = True
            'MsgBox("오브젝트선택")
            SelectBtn.Content = CType(SelectBtn.Tag, ScriptBlock).ValueCoder
        End If
    End Sub

    Private Sub ObjectFunc_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        '오브젝트가 생성자, 할당, 변환인지 결정한다
        If isload And ObjectfuncLoad Then
            Dim btnscr As ScriptBlock = SelectBtn.Tag
            If ObjectFunc.SelectedItem IsNot Nothing Then
                btnscr.value = CType(ObjectFunc.SelectedItem, ComboBoxItem).Tag
                SpFlag.Visibility = Visibility.Collapsed
                If btnscr.value = "constructor" Then
                    If btnscr.name = "EUDArray" Or btnscr.name = "EUDVArray" Then
                        SpFlag.Visibility = Visibility.Visible
                        SpFlag.IsChecked = btnscr.flag
                    End If
                End If

                varFunction.CrlInit(btnscr, dotscr, p._GUIScriptEditorUI, valueEditPanel)
                btnRefresh()
            End If
        End If
    End Sub

    Private Sub SpFlag_Checked(sender As Object, e As RoutedEventArgs)
        '인게임 초기화를 정한다. 값이 EUDArray, EUDVArray일 경우

        Dim btnscr As ScriptBlock = SelectBtn.Tag
        If btnscr IsNot Nothing Then
            btnscr.flag = True
        End If
    End Sub
    Private Sub SpFlag_Unchecked(sender As Object, e As RoutedEventArgs)
        '인게임 초기화를 정한다. 값이 EUDArray, EUDVArray일 경우

        Dim btnscr As ScriptBlock = SelectBtn.Tag
        If btnscr IsNot Nothing Then
            btnscr.flag = False
        End If
    End Sub


    Public Sub OkayAction(sender As Object, e As RoutedEventArgs)
        '//////////////////////////////
        '스크립트 갱신
        scr.value = vname.Text

        scr.child.Clear()
        For i = 0 To EditValues.Count - 1
            scr.AddChild(EditValues(i))
        Next


        Select Case _VariableType
            Case VariableType.Variable
                scr.value2 = "var"
                scr.flag = False
            Case VariableType.Static_
                scr.value2 = "static"
                scr.flag = True
            Case VariableType.Const_
                scr.value2 = "const"
                scr.flag = True
            Case VariableType.Object_
                scr.value2 = "object"
                scr.flag = True
        End Select
    End Sub

    Public Sub btnRefresh()
        If CheckEditable() Then
            p.OkBtn.IsEnabled = True
        Else
            p.OkBtn.IsEnabled = False
        End If
    End Sub

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
End Class
