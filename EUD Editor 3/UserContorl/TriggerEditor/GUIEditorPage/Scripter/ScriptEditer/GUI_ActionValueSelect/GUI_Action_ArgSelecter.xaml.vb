Public Class GUI_Action_ArgSelecter
    Public Event ArgBtnRefreshEvent As RoutedEventHandler
    Public Event ArgBtnClickEvent As RoutedEventHandler




    Private scr As ScriptBlock



    Private isuseDefaultArg As Boolean = False

    Public EditValues As List(Of ScriptBlock)
    Private Sub ValueCountUpdate()
        scr.RefreshValue()
        EditValues = New List(Of ScriptBlock)
        For i = 0 To scr.child.Count - 1
            EditValues.Add(scr.child(i).DeepCopy)
        Next
    End Sub

    Public Sub UpdateValue()
        For i = 0 To scr.child.Count - 1
            scr.ReplaceChild(EditValues(i), i)
        Next
    End Sub


    Public Sub CrlInit(_scr As ScriptBlock)
        scr = _scr


        ValueCountUpdate()
        ExtraTipPanel.Visibility = Visibility.Collapsed
        ToolTipPanel.Visibility = Visibility.Collapsed
        ValuePanel.Children.Clear()

        If scr.ScriptType = ScriptBlock.EBlockType.macrofun Then
            Dim luafunc As MacroManager.LuaFunction = macro.GetFunction(scr.name)

            If Not luafunc.IsCompleteFunction Then
                DefaultCoder()
            End If

            For i = 0 To luafunc.ArgLists.Count - 1
                Dim argblock As MacroManager.LuaFunction.ArgBlock = luafunc.ArgLists(i)

                If argblock.BType = MacroManager.LuaFunction.ArgBlock.BlockType.Arg Then
                    Dim btn As New Button
                    btn.Padding = New Thickness(5, 0, 5, 0)
                    btn.Height = 22

                    btn.Tag = New GUI_Action.tagcontainer(scr.child(argblock.ArgIndex), EditValues(argblock.ArgIndex), "")
                    AddHandler btn.Click, AddressOf argBtnClick

                    btn.Content = scr.child(argblock.ArgIndex).ValueCoder()
                    ValuePanel.Children.Add(btn)
                    btnlist.Add(btn)
                Else
                    Dim tbox As New TextBlock
                    tbox.TextWrapping = TextWrapping.Wrap
                    tbox.VerticalAlignment = VerticalAlignment.Center
                    tbox.HorizontalAlignment = HorizontalAlignment.Center
                    tbox.Text = argblock.Label

                    ValuePanel.Children.Add(tbox)
                End If
            Next


            Return
        End If

        If scr.ScriptType = ScriptBlock.EBlockType.funuse Then
            Dim func As ScriptBlock = tescm.GetFuncInfor(scr.name, scr.Scripter)
            If func Is Nothing Then
                DefaultCoder()
            Else
                Dim argumentstr As String = func.GetFuncTooltip
                ExtraTip.Text = argumentstr
                Dim argsb As List(Of ScriptBlock) = tescm.GetFuncArgs(func)
                Dim args As New List(Of String)
                Dim argtooltip As New List(Of String)

                Dim arglist As New List(Of String)
                Dim argTooltiplist As New List(Of String)
                For i = 0 To argsb.Count - 1
                    Dim argname As String = argsb(i).value
                    Dim sargtooltip As String = argsb(i).value2
                    argtooltip.Add(sargtooltip)
                    args.Add(argname)
                Next

                Dim vcount As Integer = 0
                For k = 0 To args.Count - 1
                    Dim aname As String = args(k)

                    arglist.Add(aname)
                    argTooltiplist.Add(argtooltip(k))

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

                            btn.Tag = New GUI_Action.tagcontainer(scr.child(listindex), EditValues(listindex), argTooltiplist(listindex))
                            AddHandler btn.Click, AddressOf argBtnClick

                            btn.Content = scr.child(arglist.IndexOf(vname)).ValueCoder()
                            ValuePanel.Children.Add(btn)
                            btnlist.Add(btn)
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
            End If
        Else
            Dim i As Integer = Tool.TEEpsDefaultFunc.SearchFunc(scr.name)
            Dim cfun As CFunc = Nothing
            If i >= 0 Then
                cfun = Tool.TEEpsDefaultFunc
            Else
                Dim Scripter As GUIScriptEditor = scr.Scripter

                Dim sname As String = scr.name

                Dim _namespace As String = sname.Split(".").First
                Dim funcname As String = sname.Split(".").Last
                For k = 0 To Scripter.ExternFile.Count - 1
                    If _namespace = Scripter.ExternFile(k).nameSpaceName Then
                        i = Scripter.ExternFile(k).Funcs.SearchFunc(funcname)
                        cfun = Scripter.ExternFile(k).Funcs
                        Exit For
                    End If
                Next
            End If

            If i >= 0 Then
                Dim functooltip As FunctionToolTip = cfun.GetFuncTooltip(i)
                If functooltip.Summary.Trim = "" Then
                    DefaultCoder()
                    Return
                End If

                Dim argumentstr As String = functooltip.Summary.Split(vbCrLf).First
                argumentstr = functooltip.Summary.Replace(argumentstr, "").Trim
                ExtraTip.Text = argumentstr

                Dim arglist As New List(Of String)
                Dim argTooltiplist As New List(Of String)
                Dim args() As String = cfun.GetFuncArgument(i).Split(",")

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

                            btn.Tag = New GUI_Action.tagcontainer(scr.child(listindex), EditValues(listindex), argTooltiplist(listindex))
                            AddHandler btn.Click, AddressOf argBtnClick

                            btn.Content = scr.child(arglist.IndexOf(vname)).ValueCoder()
                            ValuePanel.Children.Add(btn)
                            btnlist.Add(btn)
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




        End If
    End Sub







    Private btnlist As New List(Of Button)
    Private Sub BtnvalueRefresh()
        For i = 0 To btnlist.Count - 1
            btnlist(i).Content = CType(btnlist(i).Tag, GUI_Action.tagcontainer).desscr.ValueCoder()
        Next
        RaiseEvent ArgBtnRefreshEvent(Nothing, New RoutedEventArgs)
    End Sub

    Private SelectCont As GUI_Action.tagcontainer
    Public Sub argBtnRefresh(sender As String, e As RoutedEventArgs)
        BtnvalueRefresh()

        If sender = "Next" Then
            Dim isCurscr As Boolean = False
            Dim btnindex As Integer = -1
            For i = 0 To btnlist.Count - 1
                Dim cont As GUI_Action.tagcontainer = btnlist(i).Tag

                If isCurscr Then
                    If GUI_Action.IsDefaultValue(cont.desscr.value) Then
                        btnindex = i
                    Else
                        isCurscr = False
                    End If
                    Exit For
                End If


                If cont.desscr Is SelectCont.desscr Then
                    isCurscr = True
                End If
            Next
            If isCurscr And btnindex <> -1 Then
                ArgbtnClickExec(btnlist(btnindex))
            End If
        End If
    End Sub


    Private Sub DefaultCoder()
        isuseDefaultArg = True
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

            Dim des As String = scr.child(i).value2

            Dim btn As New Button
            btn.Padding = New Thickness(5, 0, 5, 0)
            btn.Height = 22
            btn.Tag = New GUI_Action.tagcontainer(scr.child(i), EditValues(i), des)

            AddHandler btn.Click, AddressOf argBtnClick

            btn.Content = scr.child(i).ValueCoder()
            ValuePanel.Children.Add(btn)
            btnlist.Add(btn)
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




    Private Sub argBtnClick(sender As Object, e As RoutedEventArgs)
        ArgbtnClickExec(sender)
    End Sub

    Private Sub ArgbtnClickExec(btn As Button)
        For i = 0 To btnlist.Count - 1
            If btnlist(i) Is btn Then
                btnlist(i).Opacity = 0.5
                'btnlist(i).IsEnabled = False
            Else
                btnlist(i).Opacity = 1
                'btnlist(i).IsEnabled = True
            End If
        Next


        Dim cont As GUI_Action.tagcontainer = btn.Tag
        SelectCont = cont
        ToolTipPanel.Visibility = Visibility.Visible
        TipInfor.Text = cont.des

        RaiseEvent ArgBtnClickEvent(cont.desscr, New RoutedEventArgs)
    End Sub


End Class
