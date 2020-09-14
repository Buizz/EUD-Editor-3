Public Class TEGUIObjectSelecter
    Private LastSelectGroup As String
    Private ScriptEditor As GUIScriptEditorUI

    Private Fliters As New Dictionary(Of String, String)


    Public Sub SetGUIScriptEditorUI(tScriptEditor As GUIScriptEditorUI)
        ScriptEditor = tScriptEditor
    End Sub
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        ToolBoxList.Items.Clear()

        For i = 0 To tescm.Tabkeys.Keys.Count - 1

            Dim keyname As String = tescm.Tabkeys.Keys(i)
            If keyname = "Value" Then
                Continue For
            End If
            Dim header As String = Tool.GetText(keyname)


            Dim colorcode As String = tescm.Tabkeys(keyname)

            Dim tlistitem As New ListBoxItem
            tlistitem.Padding = New Thickness(0)

            Dim tgrid As New Grid
            'tgrid.Margin = New Thickness(8)
            If True Then
                Dim tcoldef As New ColumnDefinition
                tcoldef.Width = New GridLength(20, GridUnitType.Pixel)
                tgrid.ColumnDefinitions.Add(tcoldef)


                Dim p As New Border
                p.Background = New SolidColorBrush(ColorConverter.ConvertFromString(colorcode))
                Grid.SetColumn(p, 0)
                tgrid.Children.Add(p)
            End If
            If True Then
                Dim tcoldef As New ColumnDefinition
                tcoldef.Width = New GridLength(1, GridUnitType.Star)
                tgrid.ColumnDefinitions.Add(tcoldef)
            End If


            Dim ttext As New Label
            ttext.Content = header
            ttext.Margin = New Thickness(8)
            Grid.SetColumn(ttext, 1)

            tgrid.Children.Add(ttext)


            tlistitem.Content = tgrid
            tlistitem.Tag = keyname
            'tlistitem.Foreground = Brushes.FloralWhite

            ToolBoxList.Items.Add(tlistitem)
        Next
        ToolBoxList.SelectedIndex = 0
    End Sub


    Private LastSelectItem As ListBoxItem
    Private Sub ToolBoxList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        If LastSelectItem IsNot Nothing Then
            LastSelectItem.Background = Nothing
        End If


        Dim Selectitem As ListBoxItem = ToolBoxList.SelectedItem
        LastSelectItem = Selectitem


        If Selectitem IsNot Nothing Then
            Dim ScriptGroup As String = Selectitem.Tag
            Dim colorcode As String = tescm.Tabkeys(ScriptGroup)

            Colorbar.Background = New SolidColorBrush(ColorConverter.ConvertFromString(colorcode))
            Selectitem.Background = New SolidColorBrush(ColorConverter.ConvertFromString(colorcode))

            If Fliters.ContainsKey(ScriptGroup) Then
                FliterText.Text = Fliters(ScriptGroup)
            Else
                Fliters.Add(ScriptGroup, "")
            End If
            If LastSelectGroup <> ScriptGroup Then '이전 선택과 다를 경우
                LastSelectGroup = ScriptGroup

                ToolBoxListRefresh(ScriptGroup, ScriptEditor.GetSelectScriptBlock)
            End If
        End If
    End Sub

    Public Sub ToolBoxListMove(ScriptGroup As String)

    End Sub

    Public Sub RefreshCurrentList()
        ToolBoxListRefresh(LastSelectGroup, ScriptEditor.GetSelectScriptBlock)
    End Sub

    Public Sub ToolBoxListRefresh(ScriptGroup As String, Optional normalscr As ScriptBlock = Nothing)

        If LastSelectGroup <> ScriptGroup Then
            Return
        End If


        ToolBox.Items.Clear()

        Select Case ScriptGroup
            Case "Control"
                ToolBox.Visibility = Visibility.Visible
                ToolTreeviewBox.Visibility = Visibility.Collapsed
                Dim strs() As String = {"if", "elseif", "for", "while", "switch", "switchcase", "break", "folder", "rawcode", "expression"}
                Dim types() As ScriptBlock.EBlockType = {ScriptBlock.EBlockType._if, ScriptBlock.EBlockType._elseif, ScriptBlock.EBlockType._for,
                    ScriptBlock.EBlockType._while, ScriptBlock.EBlockType.switch, ScriptBlock.EBlockType.switchcase,
                    ScriptBlock.EBlockType.break, ScriptBlock.EBlockType.folder, ScriptBlock.EBlockType.rawcode, ScriptBlock.EBlockType.exp}
                For i = 0 To strs.Length - 1
                    If strs(i) = "folder" Then
                        ToolBox.Items.Add(New Separator)
                    End If

                    Dim keyname As String = strs(i)

                    Dim headername As String = Tool.GetText(keyname)

                    Dim flitertext As String
                    If headername = "" Then
                        flitertext = keyname
                    Else
                        flitertext = headername
                    End If



                    If Fliters(ScriptGroup).Trim = "" Or flitertext.ToLower.IndexOf(Fliters(ScriptGroup).ToLower()) <> -1 Then
                        Dim tlistboxitem As New ListBoxItem
                        tlistboxitem.Tag = {types(i), keyname}
                        tlistboxitem.Content = flitertext
                        ToolBox.Items.Add(tlistboxitem)
                    End If

                Next

            Case "Action", "Condition", "plibFunc"
                ToolBox.Visibility = Visibility.Visible
                ToolTreeviewBox.Visibility = Visibility.Collapsed
                Dim ttype As FunctionToolTip.FType
                Dim header As ScriptBlock.EBlockType
                Select Case ScriptGroup
                    Case "Action"
                        ttype = FunctionToolTip.FType.Act
                        header = ScriptBlock.EBlockType.action
                    Case "Condition"
                        ttype = FunctionToolTip.FType.Cond
                        header = ScriptBlock.EBlockType.condition


                        Dim strs() As String = {"or", "and"}
                        Dim types() As ScriptBlock.EBlockType = {ScriptBlock.EBlockType._or, ScriptBlock.EBlockType._and}
                        For i = 0 To strs.Length - 1
                            Dim keyname As String = strs(i)

                            Dim headername As String = Tool.GetText(keyname)

                            Dim flitertext As String
                            If headername = "" Then
                                flitertext = keyname
                            Else
                                flitertext = headername
                            End If



                            If Fliters(ScriptGroup).Trim = "" Or flitertext.ToLower.IndexOf(Fliters(ScriptGroup).ToLower()) <> -1 Then
                                Dim tlistboxitem As New ListBoxItem
                                tlistboxitem.Tag = {types(i), keyname}
                                tlistboxitem.Content = flitertext
                                ToolBox.Items.Add(tlistboxitem)
                            End If

                        Next
                        ToolBox.Items.Add(New Separator)
                    Case "plibFunc"
                        ttype = FunctionToolTip.FType.Func
                        header = ScriptBlock.EBlockType.plibfun
                End Select


                For i = 0 To Tool.TEEpsDefaultFunc.FuncCount - 1
                    If Tool.TEEpsDefaultFunc.GetFuncTooltip(i).Type = ttype Then

                        Dim keyname As String = Tool.TEEpsDefaultFunc.GetFuncName(i)

                        Dim headername As String = Tool.GetText(keyname)
                        Dim flitertext As String
                        If headername = "" Then
                            flitertext = keyname
                        Else
                            flitertext = headername
                        End If



                        If Fliters(ScriptGroup).Trim = "" Or flitertext.ToLower.IndexOf(Fliters(ScriptGroup).ToLower()) <> -1 Then
                            Dim tlistboxitem As New ListBoxItem
                            tlistboxitem.Tag = {header, keyname}
                            tlistboxitem.Content = flitertext
                            ToolBox.Items.Add(tlistboxitem)
                        End If
                    End If
                Next
            Case "Variable"
                ToolBox.Visibility = Visibility.Visible
                ToolTreeviewBox.Visibility = Visibility.Collapsed
                Dim strs() As String = {"objectdefine", "vardefine"}
                Dim types() As ScriptBlock.EBlockType = {ScriptBlock.EBlockType.objectdefine, ScriptBlock.EBlockType.vardefine}
                For i = 0 To strs.Length - 1
                    Dim keyname As String = strs(i)

                    Dim headername As String = Tool.GetText(keyname)
                    Dim flitertext As String
                    If headername = "" Then
                        flitertext = keyname
                    Else
                        flitertext = headername
                    End If

                    Dim tlistboxitem As New ListBoxItem
                    tlistboxitem.Tag = {types(i), keyname}
                    tlistboxitem.Content = flitertext
                    ToolBox.Items.Add(tlistboxitem)
                Next
                'ToolBox.Items.Add(New Separator)

                'If normalscr IsNot Nothing Then
                '    Dim scrlist As List(Of ScriptBlock) = tescm.GetLocalVar(normalscr)
                '    For i = 0 To scrlist.Count - 1
                '        Dim tlistboxitem As New ListBoxItem
                '        tlistboxitem.Tag = {ScriptBlock.EBlockType.varuse, scrlist(i).value}
                '        tlistboxitem.Content = scrlist(i).value
                '        ToolBox.Items.Add(tlistboxitem)
                '    Next
                '    ToolBox.Items.Add(New Separator)
                'End If

                'If True Then
                '    Dim scrlist As List(Of ScriptBlock) = tescm.GetGlobalVar(ScriptEditor.Script)
                '    For i = 0 To scrlist.Count - 1
                '        Dim tlistboxitem As New ListBoxItem
                '        tlistboxitem.Tag = {ScriptBlock.EBlockType.varuse, scrlist(i).value}
                '        tlistboxitem.Content = scrlist(i).value
                '        ToolBox.Items.Add(tlistboxitem)
                '    Next
                'End If
            Case "Func"
                ToolBox.Visibility = Visibility.Visible
                ToolTreeviewBox.Visibility = Visibility.Collapsed
                Dim strs() As String = {"fundefine", "funargblock", "funreturn"}
                Dim types() As ScriptBlock.EBlockType = {ScriptBlock.EBlockType.fundefine, ScriptBlock.EBlockType.funargblock, ScriptBlock.EBlockType.funreturn}
                For i = 0 To strs.Length - 1
                    Dim keyname As String = strs(i)

                    Dim headername As String = Tool.GetText(keyname)
                    Dim flitertext As String
                    If headername = "" Then
                        flitertext = keyname
                    Else
                        flitertext = headername
                    End If

                    Dim tlistboxitem As New ListBoxItem
                    tlistboxitem.Tag = {types(i), keyname}
                    tlistboxitem.Content = flitertext
                    ToolBox.Items.Add(tlistboxitem)
                Next
            Case "EtcBlock"
                ToolTreeviewBox.Items.Clear()
                ToolTreeviewBox.BeginInit()
                ToolBox.Visibility = Visibility.Collapsed
                ToolTreeviewBox.Visibility = Visibility.Visible
                Dim strs() As String = {"import"}
                Dim types() As ScriptBlock.EBlockType = {ScriptBlock.EBlockType.import}

                Dim fText As String = Fliters(ScriptGroup).Trim

                For i = 0 To strs.Length - 1
                    Dim keyname As String = strs(i)

                    Dim headername As String = Tool.GetText(keyname)
                    Dim flitertext As String
                    If headername = "" Then
                        flitertext = keyname
                    Else
                        flitertext = headername
                    End If


                    If fText = "" Or flitertext.ToLower.IndexOf(Fliters(ScriptGroup).ToLower()) <> -1 Then
                        Dim tlistboxitem As New TreeViewItem
                        tlistboxitem.Tag = {types(i), keyname}
                        tlistboxitem.Header = flitertext
                        ToolTreeviewBox.Items.Add(tlistboxitem)
                    End If
                Next
                'ToolTreeviewBox.Items.Add(New Separator)

                '외부파일 돌린다.
                'Dim CFunc As List(Of CFunc) = tescm.GetExternFunc(ScriptEditor.Script)
                ScriptEditor.Script.ExternLoader()



                For j = 0 To ScriptEditor.Script.ExternFile.Count - 1
                    Dim tcfun As CFunc = ScriptEditor.Script.ExternFile(j).Funcs

                    Dim namespacename As String = ScriptEditor.Script.ExternFile(j).nameSpaceName

                    Dim tvitem As New TreeViewItem
                    tvitem.Header = namespacename


                    For i = 0 To tcfun.FuncCount - 1
                        Dim keyname As String = tcfun.GetFuncName(i)

                        If fText = "" Or keyname.ToLower.IndexOf(Fliters(ScriptGroup).ToLower()) <> -1 Then
                            Dim tlistboxitem As New TreeViewItem
                            tlistboxitem.Tag = {ScriptBlock.EBlockType.externfun, namespacename & "." & keyname}
                            tlistboxitem.Header = namespacename & "." & keyname


                            If fText <> "" Then
                                tlistboxitem.IsExpanded = True
                            End If


                            tvitem.Items.Add(tlistboxitem)
                        End If
                    Next
                    If tvitem.Items.Count <> 0 Then
                        If fText <> "" Then
                            tvitem.IsExpanded = True
                        End If
                        ToolTreeviewBox.Items.Add(tvitem)
                    End If
                Next

                ToolTreeviewBox.EndInit()


            Case "MacroFunc"
                ToolTreeviewBox.Items.Clear()
                ToolTreeviewBox.BeginInit()
                ToolBox.Visibility = Visibility.Collapsed
                ToolTreeviewBox.Visibility = Visibility.Visible

                Dim groupdic As New Dictionary(Of String, TreeViewItem)
                For i = 0 To macro.FunctionList.Count - 1
                    Dim Groupname As String = macro.FunctionList(i).LuaGroup


                    Dim tvitem As TreeViewItem
                    If Not groupdic.ContainsKey(Groupname) Then
                        tvitem = New TreeViewItem
                        tvitem.Header = Groupname
                        groupdic.Add(Groupname, tvitem)
                    Else
                        tvitem = groupdic(Groupname)
                    End If



                    Dim keyname As String = macro.FunctionList(i).Fname
                    If Fliters(ScriptGroup).Trim = "" Or keyname.ToLower.IndexOf(Fliters(ScriptGroup).ToLower()) <> -1 Then
                        Dim tlistboxitem As New TreeViewItem
                        tlistboxitem.Tag = {ScriptBlock.EBlockType.macrofun, macro.FunctionList(i).Fname}
                        tlistboxitem.Header = keyname
                        tvitem.Items.Add(tlistboxitem)
                    End If
                Next

                For i = 0 To groupdic.Keys.Count - 1
                    Dim tvitem As TreeViewItem = groupdic(groupdic.Keys(i))

                    ToolTreeviewBox.Items.Add(tvitem)
                Next
                ToolTreeviewBox.EndInit()
        End Select



        If ScriptGroup = "Func" Then
            ToolBox.Visibility = Visibility.Visible
            ToolTreeviewBox.Visibility = Visibility.Collapsed
            ToolBox.Items.Add(New Separator)



            Dim strs As List(Of String) = tescm.GetFuncList(ScriptEditor.PTEFile)
            For i = 0 To strs.Count - 1
                Dim keyname As String = strs(i)


                If Fliters(ScriptGroup).Trim = "" Or keyname.ToLower.IndexOf(Fliters(ScriptGroup).ToLower()) <> -1 Then
                    Dim tlistboxitem As New ListBoxItem
                    tlistboxitem.Tag = {ScriptBlock.EBlockType.funuse, keyname}
                    tlistboxitem.Content = keyname
                    ToolBox.Items.Add(tlistboxitem)
                End If
            Next
        End If
    End Sub




    Public Sub ValueListRefresh(normalscr As ScriptBlock)
        If LastSelectGroup = "Variable" Then
            ToolBoxListRefresh(LastSelectGroup, normalscr)
        End If

    End Sub

    Public Sub ListRefresh() 'SelectItem As TriggerScript)
        '선택한 아이템에 따라 표시되는 항목이 바뀜 
    End Sub





    Public Event ItemSelect As RoutedEventHandler
    Private Sub ToolBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim Selectitem As ListBoxItem = ToolBox.SelectedItem
        If Selectitem IsNot Nothing Then
            ToolBox.SelectedIndex = -1
            'MsgBox(Selectitem.Tag)
            RaiseEvent ItemSelect(Selectitem.Tag, e)
        End If
    End Sub

    Private Sub SearchBoxBtnClick(sender As Object, e As RoutedEventArgs)
        FliterText.Text = ""
    End Sub

    Private Sub SearchBoxTextChange(sender As Object, e As TextChangedEventArgs)
        Fliters(LastSelectGroup) = FliterText.Text
        ToolBoxListRefresh(LastSelectGroup, ScriptEditor.GetSelectScriptBlock)
    End Sub

    Private Sub ToolTreeviewBox_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object))
        Dim Selectitem As TreeViewItem = ToolTreeviewBox.SelectedItem
        If Selectitem IsNot Nothing Then
            If Selectitem.Items.Count = 0 Then
                Selectitem.IsSelected = False
                'MsgBox(Selectitem.Tag)
                RaiseEvent ItemSelect(Selectitem.Tag, e)
            End If
        End If
    End Sub
End Class
