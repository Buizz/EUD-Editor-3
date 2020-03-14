Public Class GUI_Action
    Public Sub CrlInit()
        '//////////////////////////////
        '초기화 식
        ValueCountUpdate()


        Dim stype As ScriptBlock.SType = scr.GetSType

        Dim colorcode As String = ""
        Select Case stype
            Case ScriptBlock.SType.act
                colorcode = tescm.Tabkeys("Action")
            Case ScriptBlock.SType.con
                colorcode = tescm.Tabkeys("Condition")
            Case ScriptBlock.SType.plibfun
                colorcode = tescm.Tabkeys("plibFunc")
            Case ScriptBlock.SType.fun
                colorcode = tescm.Tabkeys("Func")
        End Select
        colorbox.Background = New SolidColorBrush(ColorConverter.ConvertFromString(colorcode))

        fname.Text = scr.name

        ExtraTipPanel.Visibility = Visibility.Collapsed
        ToolTipPanel.Visibility = Visibility.Collapsed
        valueEditPanel.Visibility = Visibility.Collapsed
        ValuePanel.Children.Clear()
        If stype <> ScriptBlock.SType.fun Then
            Dim i As Integer = Tool.TEEpsDefaultFunc.SearchFunc(scr.name)

            If i >= 0 Then
                Dim functooltip As FunctionToolTip = Tool.TEEpsDefaultFunc.GetFuncTooltip(i)
                If functooltip.Summary.Trim = "" Then
                    DefaultCoder()
                    Return
                End If

                Dim argumentstr As String = functooltip.Summary.Split(vbCrLf).First
                argumentstr = functooltip.Summary.Replace(argumentstr, "").Trim
                ExtraTip.Text = argumentstr

                Dim arglist As New List(Of String)
                Dim argTooltiplist As New List(Of String)
                Dim args() As String = Tool.TEEpsDefaultFunc.GetFuncArgument(i).Split(",")

                Dim vcount As Integer = 0
                For k = 0 To args.Count - 1
                    Dim aname As String = args(k).Split(":").First.Trim

                    arglist.Add(aname)
                    argTooltiplist.Add(functooltip.GetTooltip(k))

                    If argumentstr.IndexOf("[" & aname & "]") <> -1 Then
                        vcount += 1
                    End If
                    argumentstr = argumentstr.Replace("[" & aname & "]", "ᐱᐳ" & aname & "ᐱ")

                Next
                Dim values() As String = argumentstr.Split("ᐱ")

                If vcount <> args.Count Then
                    DefaultCoder()
                    Return
                End If


                For k = 0 To values.Count - 1
                    If values(k).Trim <> "" Then
                        If values(k)(0) = "ᐳ" Then
                            Dim vname As String = Mid(values(k), 2)

                            Dim btn As New Button
                            btn.Padding = New Thickness(5, 0, 5, 0)
                            btn.Height = 22
                            Dim listindex As Integer = arglist.IndexOf(vname)

                            btn.Tag = New tagcontainer(scr.child(listindex), argTooltiplist(listindex))
                            AddHandler btn.Click, AddressOf argBtnClick

                            btn.Content = scr.child(arglist.IndexOf(vname)).ValueCoder()
                            ValuePanel.Children.Add(btn)
                        Else
                            Dim tbox As New TextBlock
                            tbox.TextWrapping = TextWrapping.Wrap
                            tbox.VerticalAlignment = VerticalAlignment.Center
                            tbox.HorizontalAlignment = HorizontalAlignment.Center
                            tbox.Text = values(k)

                            ValuePanel.Children.Add(tbox)
                        End If
                    End If

                Next

            Else
                DefaultCoder()
            End If
        Else
            DefaultCoder()
        End If
    End Sub

    Private Sub DefaultCoder()
        ValuePanel.Children.Clear()
        ExtraTipPanel.Visibility = Visibility.Visible

        If True Then
            Dim tbox As New TextBlock
            tbox.TextWrapping = TextWrapping.Wrap
            tbox.VerticalAlignment = VerticalAlignment.Center
            tbox.HorizontalAlignment = HorizontalAlignment.Center
            tbox.Text = scr.name & "("
            ValuePanel.Children.Add(tbox)
        End If

        For i = 0 To scr.child.Count - 1
            If i <> 0 Then
                Dim tbox As New TextBlock
                tbox.TextWrapping = TextWrapping.Wrap
                tbox.VerticalAlignment = VerticalAlignment.Center
                tbox.HorizontalAlignment = HorizontalAlignment.Center
                tbox.Text = " , "
                ValuePanel.Children.Add(tbox)
            End If

            Dim btn As New Button
            btn.Padding = New Thickness(5, 0, 5, 0)
            btn.Height = 22
            btn.Tag = New tagcontainer(scr.child(i), "")

            AddHandler btn.Click, AddressOf argBtnClick

            btn.Content = scr.child(i).ValueCoder()
            ValuePanel.Children.Add(btn)
        Next


        If True Then
            Dim tbox As New TextBlock
            tbox.TextWrapping = TextWrapping.Wrap
            tbox.VerticalAlignment = VerticalAlignment.Center
            tbox.HorizontalAlignment = HorizontalAlignment.Center
            tbox.Text = ")"
            ValuePanel.Children.Add(tbox)
        End If

    End Sub

    Public Sub OkayAction(sender As Object, e As RoutedEventArgs)
        '//////////////////////////////
        '스크립트 갱신
    End Sub


    Private Function CheckEditable() As Boolean
        '//////////////////////////////
        '확인을 누를 수 있는지 확인
        Return True
    End Function

    Private SelectArg As ScriptBlock
    Private Sub argBtnClick(sender As Object, e As RoutedEventArgs)
        Dim cont As tagcontainer = CType(sender, Button).Tag
        SelectArg = cont.scr

        ToolTipPanel.Visibility = Visibility.Visible
        valueEditPanel.Visibility = Visibility.Visible
        TipInfor.Text = cont.des


        Dim vtype As String = cont.scr.name

        '<ComboBoxItem Content = "기본" />
        '<ComboBoxItem Content = "변수"/>
        '<ComboBoxItem Content = "함수" />
        '<ComboBoxItem Content = "매크로함수"/>
        '<ComboBoxItem Content = "사용자정의" />

        'TODO:현재 
        typecombobox.SelectedIndex = 0
    End Sub

    Private comboboxloading As Boolean = False
    Private Sub typecombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        CountTextbox.Visibility = Visibility.Collapsed
        valuetypecb.Visibility = Visibility.Collapsed
        Select Case typecombobox.SelectedIndex
            Case 0
                valuetypecb.Visibility = Visibility.Visible
                Dim bd As Border = p._GUIScriptEditorUI.TEGUIPage.ValueSelecter

                Dim code As New CodeSelecter
                code.ListReset(SCDatFiles.DatFiles.Location)
                bd.Child = code
            Case 4
                CountTextbox.Visibility = Visibility.Visible
        End Select
    End Sub



    Private Sub ValueCountUpdate()

    End Sub


    Private Class tagcontainer
        Public Sub New(_scr As ScriptBlock, _des As String)
            scr = _scr
            des = _des
        End Sub
        Public scr As ScriptBlock
        Public des As String
    End Class





    Private p As GUIScriptEditerWindow
    Private scr As ScriptBlock
    Public Sub New(tp As GUIScriptEditerWindow, tscr As ScriptBlock)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        p = tp
        scr = tscr

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

End Class
