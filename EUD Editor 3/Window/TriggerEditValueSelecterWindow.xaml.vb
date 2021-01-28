Imports System.ComponentModel
Imports System.Windows.Threading

Public Class TriggerEditValueSelecterWindow
    Public Event ValueChange As RoutedEventHandler


    Private PaddingV As Integer = 5

    Public Sub ChangeComplete()
        RaiseEvent ValueChange(Nothing, New RoutedEventArgs)
    End Sub

    Private IsActivity As Boolean = False


    Private Sub PanelVisibilityCollapsed()
        CodeSelecterPanel.Visibility = Visibility.Collapsed
        CountPanel.Visibility = Visibility.Collapsed
        RawCodePanel.Visibility = Visibility.Collapsed
        ListboxPanel.Visibility = Visibility.Collapsed
        NumberPanel.Visibility = Visibility.Collapsed
        TrgStringPanel.Visibility = Visibility.Collapsed
        TrgUnitPorpertyPanel.Visibility = Visibility.Collapsed
        VariablePanel.Visibility = Visibility.Collapsed
        FunctionPanel.Visibility = Visibility.Collapsed
        FormatStringPanel.Visibility = Visibility.Collapsed
        ArgumentStringPanel.Visibility = Visibility.Collapsed
    End Sub

    Private Sub CloseP()
        If IsActivity Then
            LoadCmp = False
            PanelVisibilityCollapsed()

            For Each ctr As Control In CodeSelecterPanel.Children
                ctr.Visibility = Visibility.Collapsed
            Next
            TypeList.SelectedIndex = -1

            Me.Width = 0
            Me.Height = 0

            Visibility = Visibility.Collapsed
            IsActivity = False
        End If
    End Sub


    Private Sub Window_Deactivated(sender As Object, e As EventArgs)
        CloseP()
    End Sub


    Private tCode As TriggerCodeBlock
    Private ArgIndex As Integer



    Public Sub New()
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        MaxHeight = 0
        MinHeight = 0

        Dim tHeader() As String = {Tool.GetText("ArgDefault"), Tool.GetText("ArgVariable"), Tool.GetText("ArgFunction"), Tool.GetText("ArgUserCode")}
        Dim tTag() As String = {"Default", "Variable", "Function", "RawCode"}

        For i = 0 To tHeader.Count - 1
            Dim ListItem As New ComboBoxItem

            ListItem.Content = tHeader(i)
            ListItem.Tag = tTag(i)

            TypeList.Items.Add(ListItem)
        Next

        TypeList.Items.Add(New Separator)


        Dim dArgTypeList As List(Of String) = Tool.GetDefaultArgTypeList
        Dim ArgTypeList As List(Of String) = Tool.GetArgTypeList

        For i = 0 To dArgTypeList.Count - 1
            Dim ListItem As New ComboBoxItem

            ListItem.Content = dArgTypeList(i)
            ListItem.Tag = dArgTypeList(i)

            TypeList.Items.Add(ListItem)
        Next
        TypeList.Items.Add(New Separator)

        For i = 0 To ArgTypeList.Count - 1
            Dim ListItem As New ComboBoxItem

            ListItem.Content = ArgTypeList(i)
            ListItem.Tag = ArgTypeList(i)

            TypeList.Items.Add(ListItem)
        Next



        AddHandler UPorperty.SelectEvent, Sub(rval As String, e As RoutedEventArgs)
                                              If LoadCmp Then
                                                  tCode.Args(ArgIndex).ValueString = rval
                                                  tCode.Args(ArgIndex).IsInit = False
                                                  ChangeComplete()
                                              End If
                                          End Sub


        PanelVisibilityCollapsed()

        Dispatcher.BeginInvoke(Windows.Threading.DispatcherPriority.Input, New Action(Sub()
                                                                                          Show()
                                                                                          Visibility = Visibility.Collapsed
                                                                                          IsActivity = False
                                                                                          MaxHeight = 400
                                                                                          MinHeight = 40
                                                                                      End Sub))


    End Sub


    Private scripter As ScriptEditor
    Private IsFirstOpen As Boolean = True

    Private LoadCmp As Boolean = False
    Private FunctionAddPanel As Grid
    Private Loc As String
    Public Sub Open(_scripter As ScriptEditor, _tCode As TriggerCodeBlock, _ArgIndex As Integer, StartPos As Point, _FunctionAddPanel As Grid, Optional _Loc As String = "", Optional ButtonHeight As Integer = 0)
        Loc = _Loc

        scripter = _scripter
        LoadCmp = False
        LastSelectType = "EMPTYSTRING"
        tCode = _tCode
        ArgIndex = _ArgIndex
        FunctionAddPanel = _FunctionAddPanel


        For Each ctr As Control In CodeSelecterPanel.Children
            ctr.Visibility = Visibility.Collapsed
        Next
        PanelVisibilityCollapsed()



        '현재 ArgType
        Dim cArgType As String = _tCode.Args(ArgIndex).ValueType
        Dim cDefaultArgType As String = _tCode.Args(ArgIndex).DefaultType

        CType(TypeList.Items(0), ComboBoxItem).Content = Tool.GetText("ArgDefault") & "(" & cDefaultArgType & ")"

        Dim LastIndex As Integer
        If cArgType = cDefaultArgType Then
            '기본 값일 경우
            LastIndex = 0
        Else
            Dim IsNoneType As Boolean = True

            For i = 0 To TypeList.Items.Count - 1
                If TypeList.Items(i).GetType Is GetType(ComboBoxItem) Then
                    Dim tcbitem As ComboBoxItem = TypeList.Items(i)
                    Dim tTag As String = tcbitem.Tag

                    If tTag = cArgType Then
                        '타입설정
                        LastIndex = i
                        IsNoneType = False
                        Exit For
                    End If
                End If
            Next
            If IsNoneType Then
                LastIndex = 3
            End If

        End If
        TypeList.SelectedIndex = LastIndex
        ArgSelectBoxRefresh()




        Dim width As Integer = System.Windows.SystemParameters.VirtualScreenWidth
        Dim height As Integer = System.Windows.SystemParameters.VirtualScreenHeight

        Me.Left = StartPos.X - PaddingV

        'CtrHeight + Top가 Height보다 큰지 확인

        If Me.Height + StartPos.Y + ButtonHeight > height Then
            '위쪽으로 열어야됨
            Me.Top = StartPos.Y - Me.Height + PaddingV
        Else
            '아래로 열어야됨
            Me.Top = StartPos.Y + ButtonHeight - PaddingV
        End If



        LoadCmp = True

        Visibility = Visibility.Visible
        IsActivity = True
    End Sub









    Private LastSelectType As String = "EMPTYSTRING"
    Private Sub TypeList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If LoadCmp Then
            If TypeList.SelectedItem IsNot Nothing Then
                Dim tcbitem As ComboBoxItem = TypeList.SelectedItem
                Dim tTag As String = tcbitem.Tag


                If tTag = "Default" Then
                    tCode.Args(ArgIndex).ValueType = tCode.Args(ArgIndex).DefaultType
                Else
                    tCode.Args(ArgIndex).ValueType = tTag
                End If
                ArgSelectBoxRefresh()
            End If

        End If
    End Sub



    Private Dic As New Dictionary(Of String, CodeSelecter)

    Private Sub ArgSelectBoxRefresh()
        LoadCmp = False

        If LastSelectType = tCode.Args(ArgIndex).ValueType Then
            '리프레쉬 하지 않는다.
            Return
        Else
            LastSelectType = tCode.Args(ArgIndex).ValueType
        End If

        Dim aType As String = tCode.Args(ArgIndex).ValueType
        PanelVisibilityCollapsed()

        Select Case aType
            Case "RawCode"
                Width = 300
                Height = 200

                tCode.Args(ArgIndex).IsArgNumber = False
                tCode.Args(ArgIndex).IsLangageable = False
                tCode.Args(ArgIndex).IsQuotation = False


                CodeEditor.Text = tCode.Args(ArgIndex).ValueString
                RawCodePanel.Visibility = Visibility.Visible
            Case "Variable"
                Width = 250
                Height = 340

                tCode.Args(ArgIndex).IsArgNumber = False
                tCode.Args(ArgIndex).IsLangageable = False
                tCode.Args(ArgIndex).IsQuotation = False

                VariableCodeEditor.Text = tCode.Args(ArgIndex).ValueString2
                VariablePanel.Visibility = Visibility.Visible

                VariableTreeview.Items.Clear()

                Dim cEditor As ClassicTriggerEditor = scripter



                If True Then
                    Dim cpTreeitem As New TreeViewItem
                    cpTreeitem.Style = Application.Current.Resources("ShortTreeViewItem")
                    cpTreeitem.Background = Application.Current.Resources("MaterialDesignPaper")
                    cpTreeitem.Foreground = Application.Current.Resources("MaterialDesignBody")
                    Dim tcpdv As New DefineVariable("cp", "Const", "", "")

                    cpTreeitem.Tag = tcpdv
                    cpTreeitem.Header = Tool.GetText("CurrentPlayer")


                    VariableTreeview.Items.Add(cpTreeitem)
                End If



                Dim localTreeitem As New TreeViewItem
                localTreeitem.Background = Application.Current.Resources("MaterialDesignPaper")
                localTreeitem.Foreground = Application.Current.Resources("MaterialDesignBody")

                localTreeitem.Header = Tool.GetText("ArgGlobal")
                For i = 0 To cEditor.globalVar.Count - 1
                    Dim tnode As New TreeViewItem
                    tnode.Style = Application.Current.Resources("ShortTreeViewItem")
                    tnode.Background = Application.Current.Resources("MaterialDesignPaper")
                    tnode.Foreground = Application.Current.Resources("MaterialDesignBody")
                    tnode.Tag = cEditor.globalVar(i)
                    tnode.Header = cEditor.globalVar(i).vname


                    localTreeitem.Items.Add(tnode)
                Next
                VariableTreeview.Items.Add(localTreeitem)


                Dim tdic As New Dictionary(Of String, TreeViewItem)
                For i = 0 To cEditor.ImportVars.Count - 1
                    Dim vgroup As String = cEditor.ImportVars(i).vgroup
                    Dim vname As String = cEditor.ImportVars(i).vname

                    Dim pnode As TreeViewItem
                    If tdic.ContainsKey(vgroup) Then
                        pnode = tdic(vgroup)
                    Else
                        pnode = New TreeViewItem
                        pnode.Header = vgroup
                        tdic.Add(vgroup, pnode)
                    End If


                    Dim tnode As New TreeViewItem
                    tnode.Style = Application.Current.Resources("ShortTreeViewItem")
                    tnode.Background = Application.Current.Resources("MaterialDesignPaper")
                    tnode.Foreground = Application.Current.Resources("MaterialDesignBody")
                    tnode.Tag = cEditor.ImportVars(i)
                    tnode.Header = vname



                    pnode.Items.Add(tnode)
                Next

                For i = 0 To tdic.Values.Count - 1
                    VariableTreeview.Items.Add(tdic.Values.ToArray(i))
                Next


            Case "Function"
                Width = 250
                Height = 100


                tCode.Args(ArgIndex).IsArgNumber = False
                tCode.Args(ArgIndex).IsLangageable = False
                tCode.Args(ArgIndex).IsQuotation = False


                If tCode.Args(ArgIndex).CodeBlock Is Nothing Then
                    FunctionBtn.Content = Tool.GetText("ArgFuncSelect")
                Else
                    FunctionBtn.Content = tCode.Args(ArgIndex).CodeBlock.FName
                End If


                FunctionPanel.Visibility = Visibility.Visible
            Case "TrgString"
                Width = 400
                Height = 300

                tCode.Args(ArgIndex).IsArgNumber = False
                tCode.Args(ArgIndex).IsLangageable = False
                tCode.Args(ArgIndex).IsQuotation = True


                TrgTextBox.Text = tCode.Args(ArgIndex).ValueString
                TrgStringPanel.Visibility = Visibility.Visible
            Case "Number"
                Width = 120
                Height = 70
                If tCode.Args(ArgIndex).IsInit Then
                    tCode.Args(ArgIndex).ValueString = 0
                End If

                tCode.Args(ArgIndex).IsArgNumber = False
                tCode.Args(ArgIndex).IsLangageable = False
                tCode.Args(ArgIndex).IsQuotation = False


                NumberTB.Text = tCode.Args(ArgIndex).ValueString

                NumberPanel.Visibility = Visibility.Visible
            Case "TrgCount"
                Width = 120
                Height = 70
                If tCode.Args(ArgIndex).IsInit Then
                    tCode.Args(ArgIndex).ValueString = 1
                End If


                tCode.Args(ArgIndex).IsArgNumber = False
                tCode.Args(ArgIndex).IsLangageable = True
                tCode.Args(ArgIndex).IsQuotation = False



                Dim v As String = tCode.Args(ArgIndex).ValueString
                If v = "All" Then
                    CountTB.Text = tCode.Args(ArgIndex).ValueString
                    CountTB.IsEnabled = False
                    CountAll.IsChecked = True
                Else
                    CountTB.Text = tCode.Args(ArgIndex).ValueString
                    CountTB.IsEnabled = True
                    CountAll.IsChecked = False
                End If

                CountPanel.Visibility = Visibility.Visible
            Case "TrgProperty"
                Width = 400
                Height = 260

                tCode.Args(ArgIndex).IsArgNumber = False
                tCode.Args(ArgIndex).IsLangageable = False
                tCode.Args(ArgIndex).IsQuotation = False


                UPorperty.ResetValue(tCode.Args(ArgIndex).ValueString)

                TrgUnitPorpertyPanel.Visibility = Visibility.Visible
            Case "WAVName", "BGM", "EUDScore", "SupplyType", "UnitsDat", "WeaponsDat",
                 "FlingyDat", "SpritesDat", "ImagesDat", "UpgradesDat", "TechdataDat", "OrdersDat",
                 "TrgAIScript", "TrgSwitch", "TrgAllyStatus", "TrgComparison", "TrgModifier",
                 "TrgOrder", "TrgPlayer", "TrgPropState", "TrgResource", "TrgScore",
                 "TrgSwitchAction", "TrgSwitchState"
                FliterTB.Text = ""


                tCode.Args(ArgIndex).IsArgNumber = False
                tCode.Args(ArgIndex).IsLangageable = True
                Select Case aType
                    Case "WAVName", "BGM", "EUDScore", "SupplyType", "UnitsDat",
                     "WeaponsDat", "FlingyDat", "SpritesDat", "ImagesDat",
                     "UpgradesDat", "TechdataDat", "OrdersDat", "TrgAIScript", "TrgSwitch"
                        tCode.Args(ArgIndex).IsQuotation = True
                    Case Else
                        tCode.Args(ArgIndex).IsQuotation = False
                End Select



                ListReset()

                If aType = "WAVName" Then
                    Width = 500
                Else
                    Width = 250
                End If


                ListboxPanel.Visibility = Visibility.Visible

            Case "TrgUnit", "Weapon", "Flingy", "Sprite", "Image", "Upgrade", "Tech", "Order", "Image", "TrgLocation"
                Width = 250
                Height = 340

                CodeSelecterPanel.Visibility = Visibility.Visible


                tCode.Args(ArgIndex).IsArgNumber = True
                tCode.Args(ArgIndex).IsLangageable = True
                tCode.Args(ArgIndex).IsQuotation = False



                Dim cCodeSelecter As CodeSelecter
                If Dic.ContainsKey(aType) Then
                    '포함되어 있을 경우 새로 만들지 않는다.
                    cCodeSelecter = Dic(aType)
                    Dim initValue As Integer = tCode.Args(ArgIndex).ValueNumber
                    If tCode.Args(ArgIndex).IsInit Then
                        initValue = -1
                    End If
                    cCodeSelecter.Refresh(initValue)


                    cCodeSelecter.Visibility = Visibility.Visible
                Else
                    cCodeSelecter = New CodeSelecter()

                    Dim DatFileType As SCDatFiles.DatFiles
                    Dim aTypeStrList() As String = {"TrgUnit", "Weapon", "Flingy", "Sprite", "Image", "Upgrade", "Tech", "Order", "Image", "TrgLocation", "TrgLocationIndex"}
                    Dim aTypeEnumList() As SCDatFiles.DatFiles = {SCDatFiles.DatFiles.units, SCDatFiles.DatFiles.weapons, SCDatFiles.DatFiles.flingy, SCDatFiles.DatFiles.sprites,
                        SCDatFiles.DatFiles.images, SCDatFiles.DatFiles.upgrades, SCDatFiles.DatFiles.techdata, SCDatFiles.DatFiles.orders, SCDatFiles.DatFiles.images, SCDatFiles.DatFiles.Location,
                    SCDatFiles.DatFiles.Location}




                    AddHandler cCodeSelecter.ListSelect, Sub(returnval() As Integer, e As RoutedEventArgs)
                                                             tCode.Args(ArgIndex).ValueNumber = returnval(1)
                                                             tCode.Args(ArgIndex).ValueString = pjData.CodeLabel(DatFileType, returnval(1))


                                                             tCode.Args(ArgIndex).IsInit = False
                                                             ChangeComplete()
                                                             CloseP()
                                                         End Sub


                    DatFileType = aTypeEnumList(aTypeStrList.ToList.IndexOf(aType))

                    Dim initValue As Integer = tCode.Args(ArgIndex).ValueNumber
                    If tCode.Args(ArgIndex).IsInit Then
                        initValue = -1
                    End If



                    cCodeSelecter.ListReset(DatFileType, _StartIndex:=initValue, combobox:=True, _unitFlag:=True)
                    Dic.Add(aType, cCodeSelecter)

                    CodeSelecterPanel.Children.Add(cCodeSelecter)
                End If
            Case "FormatString"
                Width = 350
                Height = 200
                If tCode.Args(ArgIndex).IsInit Then
                    tCode.Args(ArgIndex).ValueString = ""
                End If


                tCode.Args(ArgIndex).IsArgNumber = False
                tCode.Args(ArgIndex).IsLangageable = False
                tCode.Args(ArgIndex).IsQuotation = False


                FormatStringTB.Text = tCode.Args(ArgIndex).ValueString

                FormatStringPanel.Visibility = Visibility.Visible
            Case "Arguments"
                Width = 350
                Height = 200
                If tCode.Args(ArgIndex).IsInit Then
                    tCode.Args(ArgIndex).ValueString = ""
                End If


                tCode.Args(ArgIndex).IsArgNumber = False
                tCode.Args(ArgIndex).IsLangageable = False
                tCode.Args(ArgIndex).IsQuotation = False


                Arguments.Text = tCode.Args(ArgIndex).ValueString

                ArgumentStringPanel.Visibility = Visibility.Visible
            Case "Tbl"
                '사용자 정의 Arg임
                Width = 250
                Height = 340

                If tCode.Args(ArgIndex).IsInit Then
                    tCode.Args(ArgIndex).IsArgNumber = True
                    tCode.Args(ArgIndex).IsLangageable = False
                    tCode.Args(ArgIndex).IsQuotation = True

                End If

                ListReset()
                ListboxPanel.Visibility = Visibility.Visible
            Case Else
                '사용자 정의 Arg임
                Width = 250
                Height = 340


                tCode.Args(ArgIndex).IsArgNumber = False
                tCode.Args(ArgIndex).IsLangageable = False
                tCode.Args(ArgIndex).IsQuotation = True


                ListReset()
                ListboxPanel.Visibility = Visibility.Visible
        End Select






        LoadCmp = True
    End Sub



    Private Sub ListReset()
        Dim FliterText As String = FliterTB.Text.Trim.ToUpper

        'FliterBox


        SelectListbox.Items.Clear()

        Dim aType As String = tCode.Args(ArgIndex).ValueType


        Dim arglist() As String = GetArgList(aType)

        For i = 0 To arglist.Count - 1
            Dim lanstr As String = Tool.GetLanText("TrgArg" & arglist(i))
            If lanstr = "TrgArg" & arglist(i) Then
                lanstr = arglist(i)
            Else
                lanstr = lanstr
            End If



            If FliterText = "" Then
                Dim listbox As New ListBoxItem

                listbox.Content = lanstr
                listbox.Tag = arglist(i)

                SelectListbox.Items.Add(listbox)
            Else
                If lanstr.ToUpper.IndexOf(FliterText.ToUpper) <> -1 Then
                    Dim listbox As New ListBoxItem

                    listbox.Content = lanstr
                    listbox.Tag = arglist(i)

                    SelectListbox.Items.Add(listbox)
                End If
            End If


        Next
        If FliterText = "" Then
            If SelectListbox.Items.Count <= 24 Then
                FliterBox.Visibility = Visibility.Collapsed
            Else
                FliterBox.Visibility = Visibility.Visible
            End If
            Height = SelectListbox.Items.Count * 34 + 40
        Else
            Height = SelectListbox.Items.Count * 34 + 40 + 40
        End If


    End Sub


    Private Sub CountTB_TextChanged(sender As Object, e As TextChangedEventArgs)
        If LoadCmp Then
            tCode.Args(ArgIndex).ValueString = CountTB.Text
            tCode.Args(ArgIndex).IsInit = False
            ChangeComplete()
        End If
    End Sub

    Private Sub CountAll_Checked(sender As Object, e As RoutedEventArgs)
        If LoadCmp Then
            tCode.Args(ArgIndex).ValueString = "All"
            CountTB.IsEnabled = False
            tCode.Args(ArgIndex).IsInit = False
            ChangeComplete()
        End If
    End Sub

    Private Sub CountAll_Unchecked(sender As Object, e As RoutedEventArgs)
        If LoadCmp Then
            tCode.Args(ArgIndex).ValueString = CountTB.Text
            CountTB.IsEnabled = True
            tCode.Args(ArgIndex).IsInit = False
            ChangeComplete()
        End If
    End Sub

    Private Sub CodeEditor_TextChange(sender As Object, e As RoutedEventArgs)
        If LoadCmp Then
            tCode.Args(ArgIndex).ValueString = CodeEditor.Text
            tCode.Args(ArgIndex).IsInit = False
            ChangeComplete()
        End If
    End Sub

    Private Sub SelectListbox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If LoadCmp Then
            If SelectListbox.SelectedItem IsNot Nothing Then
                Dim item As ListBoxItem = SelectListbox.SelectedItem

                tCode.Args(ArgIndex).ValueString = item.Tag
                tCode.Args(ArgIndex).ValueNumber = SelectListbox.SelectedIndex
                tCode.Args(ArgIndex).IsInit = False
                ChangeComplete()
                CloseP()
            End If
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        If True Then
            FliterTB.Text = ""
        End If
    End Sub

    Private Sub FliterTB_TextChanged(sender As Object, e As TextChangedEventArgs)
        If True Then
            ListReset()
        End If
    End Sub

    Private Sub NumberTBTB_TextChanged(sender As Object, e As TextChangedEventArgs)
        If LoadCmp Then
            tCode.Args(ArgIndex).ValueString = NumberTB.Text
            tCode.Args(ArgIndex).IsInit = False
            ChangeComplete()
        End If
    End Sub

    Private Sub TrgTextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        If LoadCmp Then
            tCode.Args(ArgIndex).ValueString = TrgTextBox.Text
            tCode.Args(ArgIndex).IsInit = False
            ChangeComplete()
        End If
    End Sub

    Private Sub FunctionBtn_Click(sender As Object, e As RoutedEventArgs)
        If LoadCmp Then
            Dim Arg As ArgValue = tCode.Args(ArgIndex)
            tCode.Args(ArgIndex).IsInit = False

            Dim nTriggerCodeEditControl As New TriggerCodeEditControl

            AddHandler nTriggerCodeEditControl.OkayBtnEvent, Sub(_sender As Object, _e As RoutedEventArgs)
                                                                 If Arg.CodeBlock Is Nothing Then
                                                                     Arg.CodeBlock = nTriggerCodeEditControl.SelectTBlock.DeepCopy
                                                                 Else
                                                                     nTriggerCodeEditControl.SelectTBlock.CopyTo(Arg.CodeBlock)
                                                                 End If
                                                                 FunctionAddPanel.Children.Remove(nTriggerCodeEditControl)
                                                                 ChangeComplete()
                                                             End Sub
            AddHandler nTriggerCodeEditControl.CancelBtnEvent, Sub(_sender As Object, _e As RoutedEventArgs)
                                                                   FunctionAddPanel.Children.Remove(nTriggerCodeEditControl)
                                                               End Sub

            FunctionAddPanel.Children.Add(nTriggerCodeEditControl)


            If Arg.CodeBlock IsNot Nothing Then
                nTriggerCodeEditControl.OpenEdit(scripter, TriggerCodeEditControl.OpenType.Func, Loc:=Loc, TBlock:=Arg.CodeBlock.DeepCopy)
            Else
                nTriggerCodeEditControl.OpenEdit(scripter, TriggerCodeEditControl.OpenType.Func, Loc:=Loc)
            End If



            CloseP()
        End If

        'Dim TriggerFuncEditWindow As New TriggerFuncEditWindow(tCode.Args(ArgIndex))

        'TriggerFuncEditWindow.ShowDialog()
    End Sub

    Private Sub VariableTreeview_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object))
        If LoadCmp Then
            If VariableTreeview.SelectedItem IsNot Nothing Then
                Dim tnode As TreeViewItem = VariableTreeview.SelectedItem

                If tnode.Tag Is Nothing Then
                    Return
                End If

                Dim vDefine As DefineVariable = tnode.Tag
                If vDefine.vgroup = "" Then
                    tCode.Args(ArgIndex).ValueString = vDefine.vname
                Else
                    tCode.Args(ArgIndex).ValueString = vDefine.vgroup & "." & vDefine.vname
                End If
                tCode.Args(ArgIndex).IsInit = False
                Select Case vDefine.vtype
                    Case "PVariable"
                        tCode.Args(ArgIndex).ValueString2 = "[cp]"
                    Case "Array"
                        tCode.Args(ArgIndex).ValueString2 = "[0]"
                    Case "VArray"
                        tCode.Args(ArgIndex).ValueString2 = "[0]"
                    Case "Const"
                        tCode.Args(ArgIndex).ValueString2 = ""
                    Case "Default"
                        tCode.Args(ArgIndex).ValueString2 = ""
                End Select

                ChangeComplete()
                CloseP()
            End If
        End If


    End Sub

    Private Sub FormatStringTB_TextChanged(sender As Object, e As TextChangedEventArgs)
        If LoadCmp Then
            tCode.Args(ArgIndex).ValueString = FormatStringTB.Text
            tCode.Args(ArgIndex).IsInit = False
            ChangeComplete()
        End If
    End Sub

    Private Sub FormatStringOpenClick(sender As Object, e As RoutedEventArgs)
        Dim tEditor As New TextEditorWindow(FormatStringTB.Text)

        IsActivity = False
        tEditor.ShowDialog()
        IsActivity = True
        FormatStringTB.Text = tEditor.TextString
    End Sub


    Private Sub ArgumentCodeEditor_TextChange(sender As Object, e As RoutedEventArgs)
        If LoadCmp Then
            tCode.Args(ArgIndex).ValueString = Arguments.Text
            tCode.Args(ArgIndex).IsInit = False
            ChangeComplete()
        End If
    End Sub

    Private Sub VariableCodeEditor_TextChange(sender As Object, e As RoutedEventArgs)
        If LoadCmp Then
            tCode.Args(ArgIndex).ValueString2 = VariableCodeEditor.Text
            tCode.Args(ArgIndex).IsInit = False
            ChangeComplete()
        End If
    End Sub








    '{"TrgAllyStatus", "TrgComparison", "TrgCount", "TrgModifier", "TrgOrder",
    '        "TrgPlayer", "TrgProperty", "TrgPropState", "TrgResource", "TrgScore", "TrgSwitchAction", "TrgSwitchState",
    '        "TrgAIScript", "TrgLocation", "TrgSwitch", "TrgUnit", "WAVName", "BGM", "UnitsDat", "Weapon", "Flingy",
    '        "Sprite", "Image", "Upgrade", "Tech", "Order", "EUDScore", "SupplyType"}







    '인자 번호와 해당 트리거를 넣는다.

    '핸들러를 넣어서 리프레시가 가능하게 해준다.
End Class
