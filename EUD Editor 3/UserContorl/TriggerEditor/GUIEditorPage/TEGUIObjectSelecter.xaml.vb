Public Class TEGUIObjectSelecter
    Private LastSelectGroup As String
    Private ScriptEditor As GUIScriptEditorUI

    Public Sub SetGUIScriptEditorUI(tScriptEditor As GUIScriptEditorUI)
        ScriptEditor = tScriptEditor
    End Sub
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
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
            If LastSelectGroup <> ScriptGroup Then '이전 선택과 다를 경우
                LastSelectGroup = ScriptGroup

                ToolBoxListRefresh(ScriptGroup)
            End If
        End If
    End Sub

    Public Sub ToolBoxListMove(ScriptGroup As String)

    End Sub

    Public Sub RefreshCurrentList()
        ToolBoxListRefresh(LastSelectGroup)
    End Sub

    Public Sub ToolBoxListRefresh(ScriptGroup As String)
        If LastSelectGroup <> ScriptGroup Then
            Return
        End If


        ToolBox.Items.Clear()


        Select Case ScriptGroup
            Case "Control"
                Dim strs() As String = {"if", "elseif", "for", "while", "switch", "switchcase", "or", "and", "folder", "rawcode"}
                For i = 0 To strs.Length - 1
                    If strs(i) = "folder" Then
                        ToolBox.Items.Add(New Separator)
                    End If

                    Dim tlistboxitem As New ListBoxItem
                    Dim keyname As String = strs(i)
                    tlistboxitem.Tag = keyname

                    Dim headername As String = Tool.GetText(keyname)
                    If headername = "" Then
                        tlistboxitem.Content = keyname
                    Else
                        tlistboxitem.Content = headername
                    End If


                    ToolBox.Items.Add(tlistboxitem)
                Next

            Case "Action", "Condition", "plibFunc"
                Dim ttype As FunctionToolTip.FType

                Select Case ScriptGroup
                    Case "Action"
                        ttype = FunctionToolTip.FType.Act
                    Case "Condition"
                        ttype = FunctionToolTip.FType.Cond
                    Case "plibFunc"
                        ttype = FunctionToolTip.FType.Func
                End Select


                For i = 0 To Tool.TEEpsDefaultFunc.FuncCount - 1
                    If Tool.TEEpsDefaultFunc.GetFuncTooltip(i).Type = ttype Then

                        Dim tlistboxitem As New ListBoxItem
                        Dim keyname As String = Tool.TEEpsDefaultFunc.GetFuncName(i)
                        tlistboxitem.Tag = keyname

                        Dim headername As String = Tool.GetText(keyname)
                        If headername = "" Then
                            tlistboxitem.Content = keyname
                        Else
                            tlistboxitem.Content = headername
                        End If


                        ToolBox.Items.Add(tlistboxitem)
                    End If
                Next
            Case "Variable"
                Dim strs() As String = {"var"}
                For i = 0 To strs.Length - 1
                    Dim tlistboxitem As New ListBoxItem
                    Dim keyname As String = strs(i)
                    tlistboxitem.Tag = keyname

                    Dim headername As String = Tool.GetText(keyname)
                    If headername = "" Then
                        tlistboxitem.Content = keyname
                    Else
                        tlistboxitem.Content = headername
                    End If
                    ToolBox.Items.Add(tlistboxitem)
                Next

            Case "Func"
                Dim strs() As String = {"function", "funargblock"}
                For i = 0 To strs.Length - 1
                    Dim tlistboxitem As New ListBoxItem
                    Dim keyname As String = strs(i)
                    tlistboxitem.Tag = keyname

                    Dim headername As String = Tool.GetText(keyname)
                    If headername = "" Then
                        tlistboxitem.Content = keyname
                    Else
                        tlistboxitem.Content = headername
                    End If
                    ToolBox.Items.Add(tlistboxitem)

                Next
            Case "EtcBlock"
                Dim strs() As String = {"import"}
                For i = 0 To strs.Length - 1
                    Dim tlistboxitem As New ListBoxItem
                    Dim keyname As String = strs(i)
                    tlistboxitem.Tag = keyname

                    Dim headername As String = Tool.GetText(keyname)
                    If headername = "" Then
                        tlistboxitem.Content = keyname
                    Else
                        tlistboxitem.Content = headername
                    End If
                    ToolBox.Items.Add(tlistboxitem)

                Next
        End Select



        If ScriptGroup = "Variable" Then
            ToolBox.Items.Add(New Separator)
        End If

        If ScriptGroup = "Func" Then
            ToolBox.Items.Add(New Separator)



            Dim strs As List(Of String) = tescm.GetFuncList(ScriptEditor.PTEFile)
            For i = 0 To strs.Count - 1
                Dim tlistboxitem As New ListBoxItem
                Dim keyname As String = strs(i)
                tlistboxitem.Tag = keyname

                tlistboxitem.Content = keyname
                ToolBox.Items.Add(tlistboxitem)
            Next
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
            RaiseEvent ItemSelect(Selectitem.Tag, e)
        End If
    End Sub
End Class
