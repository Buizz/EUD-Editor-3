Public Class GUI_ArgEditor
    Public Event BtnRefresh As RoutedEventHandler
    Public scr As ScriptBlock
    Private normalscrscr As ScriptBlock
    Private Scripter As GUIScriptEditorUI


    Private valuepanel As Border

    Public Sub Init(valueselecter As Border, _normalscr As ScriptBlock, _Scripter As GUIScriptEditorUI, Optional IsHaveObject As Boolean = False)
        valuepanel = valueselecter
        Scripter = _Scripter
        normalscrscr = _normalscr

        valuetypecb.Items.Clear()
        For i = 0 To tescm.SCValueType.Count - 1
            Dim comboxitem As New ComboBoxItem
            comboxitem.Tag = tescm.SCValueType(i)
            comboxitem.Content = Tool.GetLanText(tescm.SCValueType(i))
            valuetypecb.Items.Add(comboxitem)
        Next
        AddHandler ArgSelecter.ArgBtnClickEvent, AddressOf ArgSelecterEvent
        AddHandler ArgExpress.ArgBtnClickEvent, AddressOf ArgSelecterEvent
        AddHandler ArgExpress.ArgExpressRefreshEvent, AddressOf ArgExpressRefresh
        AddHandler VarUseFunc.RefreshEvent, AddressOf VarUseFunc_Change
        'AddHandler VarUseFunc.ArgBtnRefreshEvent, AddressOf ArgExpressRefresh
        'AddHandler VarUseFunc.ArgBtnClickEvent, AddressOf ArgSelecterEvent
    End Sub






    Public Sub ArgExpressRefresh(sender As Object, e As RoutedEventArgs)
        RaiseEvent BtnRefresh("", e)
        ArgExpress.UpdateValue()
    End Sub

    Public Sub ArgSelecterEvent(_sender As ScriptBlock, _e As RoutedEventArgs)
        If _sender Is Nothing Then
            Return
        End If
        If _sender.ScriptType = ScriptBlock.EBlockType.sign Then
            Return
        End If

        Dim graywindow As New GUI_GrayWindow


        graywindow.Height = Scripter.TEGUIPage.ActualHeight
        graywindow.Width = Scripter.TEGUIPage.ActualWidth
        Dim cpos As Point = Scripter.TEGUIPage.PointToScreen(New Point(0, 0))

        graywindow.Top = cpos.Y
        graywindow.Left = cpos.X

        graywindow.Show()


        Dim argwindow As New GUI_ArgEditorWindow(graywindow)

        argwindow.Top = cpos.Y + Scripter.TEGUIPage.ActualHeight / 2 - argwindow.Height / 2
        argwindow.Left = cpos.X + Scripter.TEGUIPage.ActualWidth / 2 - argwindow.Width / 2


        'ArgSelecter를 건내줘야되 그래야 리프레쉬됨
        'GUI_ArgEditor


        argwindow.ArgEditor.Init(argwindow.ValueSelecter, normalscrscr, Scripter)
        If scr.ScriptType = ScriptBlock.EBlockType.exp Then
            AddHandler argwindow.ArgEditor.BtnRefresh, AddressOf ArgExpress.argBtnRefresh
        Else
            AddHandler argwindow.ArgEditor.BtnRefresh, AddressOf ArgSelecter.argBtnRefresh
        End If


        argwindow.ArgEditor.Visibility = Visibility.Visible
        argwindow.ArgEditor.ComboboxInit(_sender)
        argwindow.ShowDialog()


        If scr.ScriptType = ScriptBlock.EBlockType.exp Then
            ArgExpress.UpdateValue()
        Else
            ArgSelecter.UpdateValue()
        End If
    End Sub

    Private typecombobxload As Boolean = False
    Public Sub ComboboxInit(_scr As ScriptBlock)
        scr = _scr

        typecombobxload = False
        Select Case scr.ScriptType
            Case ScriptBlock.EBlockType.varuse, ScriptBlock.EBlockType.externvaruse
                typebtn1.IsChecked = True
            Case ScriptBlock.EBlockType.funuse, ScriptBlock.EBlockType.plibfun, ScriptBlock.EBlockType.macrofun, ScriptBlock.EBlockType.externfun
                typebtn2.IsChecked = True
            Case ScriptBlock.EBlockType.exp
                typebtn3.IsChecked = True
            Case ScriptBlock.EBlockType.rawcode
                typebtn4.IsChecked = True
            Case ScriptBlock.EBlockType.constVal
                typebtn0.IsChecked = True
        End Select
        TypeboxInit()
        typecombobxload = True
    End Sub

    Private typecomboboxSelectedIndex As Integer
    Private Sub typebtn_Checked(sender As Object, e As RoutedEventArgs)
        Dim radbtn As RadioButton = sender
        typecomboboxSelectedIndex = radbtn.Tag
        If typecombobxload Then
            TypeboxInit()
        End If
    End Sub

    Private valuetypecbload As Boolean = False
    Private Sub TypeboxInit()
        Dim bd As Border = valuepanel
        bd.Visibility = Visibility.Collapsed
        bd.Child = Nothing
        CountTextbox.Visibility = Visibility.Collapsed
        valuetypecb.Visibility = Visibility.Collapsed
        Vartab.Visibility = Visibility.Collapsed
        functab.Visibility = Visibility.Collapsed
        ArgExpressTab.Visibility = Visibility.Collapsed
        Select Case typecomboboxSelectedIndex
            Case 0 '기본
                If scr.ScriptType <> ScriptBlock.EBlockType.constVal Then
                    Dim nsb As New ScriptBlock(ScriptBlock.EBlockType.constVal, "Number", True, False, "0", scr.Scripter)

                    scr.DuplicationBlock(nsb)
                    RaiseEvent BtnRefresh("", New RoutedEventArgs)
                End If


                valuetypecb.Visibility = Visibility.Visible

                Dim tvtype As String = scr.name.Trim

                Dim vindex As Integer = tescm.SCValueType.ToList.IndexOf(tvtype)
                If vindex = -1 Then
                    tvtype = "Number"
                End If

                For i = 0 To valuetypecb.Items.Count - 1
                    If CType(valuetypecb.Items(i), ComboBoxItem).Tag = tvtype Then
                        valuetypecbload = False
                        valuetypecb.SelectedIndex = i
                        valuetypecbinit()
                        valuetypecbload = True
                        Exit For
                    End If
                Next
            Case 1 '변수
                Vartab.Visibility = Visibility.Visible

                '"const"
                '"default"
                '"array"
                '"object"

                '"local"
                '"global"
                '"extern"
                Dim initstr() As String = {"init", "init"}
                If scr.ScriptType = ScriptBlock.EBlockType.varuse Then
                    initstr(0) = scr.name
                    initstr(1) = scr.value
                    VarUseFunc.CrlInit(scr, normalscrscr, Scripter, Me)
                    VarUseFunc.Visibility = Visibility.Visible
                Else
                    VarUseFunc.Visibility = Visibility.Collapsed
                End If


                Dim ListSelecter As New GUI_VariableSelecter(initstr, Scripter, normalscrscr)

                AddHandler ListSelecter.SelectEvent, AddressOf VariableListSelect
                bd.Child = ListSelecter
                bd.Visibility = Visibility.Visible
            Case 2 '함수
                functab.Visibility = Visibility.Visible

                'TODO: functype 콤보박스 인덱스 셀렉트하기

                Dim typename As String = ""
                Select Case scr.ScriptType
                    Case ScriptBlock.EBlockType.funuse
                        typename = "default"
                        ArgSelecter.CrlInit(scr)
                        ArgSelecter.Visibility = Visibility.Visible
                    Case ScriptBlock.EBlockType.plibfun
                        typename = "plib"
                        ArgSelecter.CrlInit(scr)
                        ArgSelecter.Visibility = Visibility.Visible
                    Case ScriptBlock.EBlockType.macrofun
                        typename = "macro"
                        ArgSelecter.CrlInit(scr)
                        ArgSelecter.Visibility = Visibility.Visible
                    Case ScriptBlock.EBlockType.externfun
                        typename = "extern"
                        ArgSelecter.CrlInit(scr)
                        ArgSelecter.Visibility = Visibility.Visible
                    Case Else
                        ArgSelecter.Visibility = Visibility.Collapsed
                End Select

                Dim ListSelecter As New GUI_FunctionSelecter(typename, Scripter)

                AddHandler ListSelecter.SelectEvent, AddressOf FuncListSelect
                bd.Child = ListSelecter
                bd.Visibility = Visibility.Visible
            Case 3 '수식
                ArgExpressTab.Visibility = Visibility.Visible

                If scr.ScriptType <> ScriptBlock.EBlockType.exp Then
                    Dim nsb As New ScriptBlock(ScriptBlock.EBlockType.exp, "expression", True, False, "", scr.Scripter)
                    scr.DuplicationBlock(nsb)

                    RaiseEvent BtnRefresh("", New RoutedEventArgs)
                End If

                ArgExpress.CrlInit(scr)

            Case 4 '사용자정의
                CountTextbox.Visibility = Visibility.Visible

                If scr.ScriptType <> ScriptBlock.EBlockType.rawcode Then
                    Dim nsb As New ScriptBlock(ScriptBlock.EBlockType.rawcode, "rawcode", True, False, "rawcode", scr.Scripter)

                    scr.DuplicationBlock(nsb)
                    RaiseEvent BtnRefresh("", New RoutedEventArgs)
                End If


                If IsDefaultValue(scr.value) Then
                    CountTextbox.Text = ""
                Else
                    CountTextbox.Text = scr.value
                End If

        End Select
    End Sub

    Private Sub valuetypecb_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If valuetypecbload Then
            valuetypecbinit()
        End If
    End Sub
    Private Sub valuetypecbinit()
        Dim bd As Border = valuepanel
        bd.Visibility = Visibility.Collapsed
        bd.Child = Nothing


        Dim vtype As String = CType(valuetypecb.SelectedItem, ComboBoxItem).Tag
        Dim vindex As Integer = tescm.SCValueType.ToList.IndexOf(vtype)


        Select Case vtype
            Case "Number"
                Dim ListSelecter As New GUI_Action_Count(scr.value, False)

                AddHandler ListSelecter.SelectEvent, AddressOf ListTextSelect
                bd.Child = ListSelecter
                bd.Visibility = Visibility.Visible
            Case "TrgCount"
                Dim ListSelecter As New GUI_Action_Count(scr.value, True)

                AddHandler ListSelecter.SelectEvent, AddressOf ListTextSelect
                bd.Child = ListSelecter
                bd.Visibility = Visibility.Visible
            Case "TrgProperty"
                Dim ListSelecter As New GUI_Action_UnitProperty(scr.value)

                AddHandler ListSelecter.SelectEvent, AddressOf ListSelect
                bd.Child = ListSelecter
                bd.Visibility = Visibility.Visible
            Case "TrgString", "FormatText"
                Dim ListSelecter As New GUI_Action_String(scr.value)

                AddHandler ListSelecter.SelectEvent, AddressOf ListTextSelect
                bd.Child = ListSelecter
                bd.Visibility = Visibility.Visible
            Case "WAVName"
                Dim ListSelecter As New GUI_WavSelecter(scr.value)

                AddHandler ListSelecter.SelectEvent, AddressOf ListTextSelect
                bd.Child = ListSelecter
                bd.Visibility = Visibility.Visible
            Case Else
                Dim ListSelecter As New GUI_Action_ListSelecter(CodeEditor.GetArgList(vtype))

                AddHandler ListSelecter.SelectEvent, AddressOf ListSelect
                bd.Child = ListSelecter
                bd.Visibility = Visibility.Visible
        End Select
    End Sub

    Public Sub ListTextSelect(sender As Object, e As RoutedEventArgs)
        Dim value As String = sender

        scr.value = value
        RaiseEvent BtnRefresh("", e)
    End Sub
    Public Sub ListSelect(sender As Object, e As RoutedEventArgs)
        Dim value As String = sender
        Dim vtype As String = CType(valuetypecb.SelectedItem, ComboBoxItem).Tag
        scr.value = value
        scr.name = vtype
        RaiseEvent BtnRefresh("Next", e)

        '현재 선택한 인자가 몇번째 인자인지 조사.
    End Sub


    Private Function IsDefaultValue(str As String) As Boolean
        Dim strs() As String = str.Split(";")
        If strs.Length <> 2 Then
            Return False
        Else
            If strs.First = "defaultvalue" Then
                Return True
            End If
        End If
        Return False
    End Function


    Public Sub VariableListSelect(sender() As Object, e As RoutedEventArgs)
        'TODO: 받아서 값에다가 이름을 넣는 식으로 해보자
        Dim nsb As New ScriptBlock(ScriptBlock.EBlockType.varuse, sender(0), True, False, sender(1), scr.Scripter)

        Dim ismethod As Boolean = False
        If sender.Count = 3 Then
            If CType(sender(2), ScriptBlock.EBlockType) = ScriptBlock.EBlockType.fundefine Then
                nsb.value2 = "method"
                ismethod = True
            ElseIf CType(sender(2), ScriptBlock.EBlockType) = ScriptBlock.EBlockType.vardefine Then
                nsb.value2 = "fields"
            End If
        Else
            nsb.value2 = "value"
        End If

        scr.DuplicationBlock(nsb)
        VarUseFunc.CrlInit(scr, normalscrscr, Scripter, Me, True)
        VarUseFunc.Visibility = Visibility.Visible




        RaiseEvent BtnRefresh("", e)
        '현재 선택한 인자가 몇번째 인자인지 조사.
    End Sub
    Private Sub VarUseFunc_Change(sender As Object, e As RoutedEventArgs)
        RaiseEvent BtnRefresh("", e)
    End Sub
    Public Sub FuncListSelect(sender() As Object, e As RoutedEventArgs)
        'TODO: 받아서 값에다가 이름을 넣는 식으로 해보자
        Dim nsb As New ScriptBlock(sender.First, sender.Last, True, False, "", scr.Scripter)
        scr.DuplicationBlock(nsb)
        ArgSelecter.CrlInit(scr)
        ArgSelecter.Visibility = Visibility.Visible




        RaiseEvent BtnRefresh("", e)
        '현재 선택한 인자가 몇번째 인자인지 조사.
    End Sub


    Private Sub CountTextbox_TextChange(sender As Object, e As RoutedEventArgs)
        If scr.ScriptType = ScriptBlock.EBlockType.rawcode Then
            scr.value = CountTextbox.Text
            RaiseEvent BtnRefresh("", e)
        End If
    End Sub
End Class
