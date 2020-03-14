Imports System.Collections.ObjectModel

<Serializable>
Public Class ScriptBlock
    Public name As String
    Public isexpand As Boolean
    Public flag As Boolean
    Public value As String
    Public child As List(Of ScriptBlock)

    Public Parent As ScriptBlock

    Public Scripter As GUIScriptEditor

    Public Sub AddChild(scr As ScriptBlock)
        scr.Parent = Me
        child.Add(scr)
    End Sub



    Public Sub InsertChild(i As Integer, scr As ScriptBlock)
        scr.Parent = Me
        child.Insert(i, scr)
    End Sub

    Public Sub New(tname As String, tisexpand As Boolean, tflag As Boolean, tvalue As String, tScripter As GUIScriptEditor)
        name = tname
        isexpand = tisexpand
        flag = tflag
        value = tvalue
        Scripter = tScripter

        child = New List(Of ScriptBlock)


        Select Case name
            Case "function"
                AddChild(New ScriptBlock("funargs", False, False, "", Scripter))
                AddChild(New ScriptBlock("funcontent", False, False, "", Scripter))
            Case "if"
                AddChild(New ScriptBlock("ifcondition", False, False, "", Scripter))
                AddChild(New ScriptBlock("ifthen", False, False, "", Scripter))
            Case "elseif"
                AddChild(New ScriptBlock("ifcondition", False, False, "", Scripter))
                AddChild(New ScriptBlock("ifthen", False, False, "", Scripter))
            Case "for"
                AddChild(New ScriptBlock("forcontent", False, False, "", Scripter))
                AddChild(New ScriptBlock("foraction", False, False, "", Scripter))
            Case "forcontent"
                AddChild(New ScriptBlock("forinit", False, False, "", Scripter))
                AddChild(New ScriptBlock("forcondition", False, False, "", Scripter))
                AddChild(New ScriptBlock("forincre", False, False, "", Scripter))
            Case "while"
                AddChild(New ScriptBlock("whilecondition", False, False, "", Scripter))
                AddChild(New ScriptBlock("whileaction", False, False, "", Scripter))
            Case "switch"
                AddChild(New ScriptBlock("switchvar", False, False, "", Scripter))
            Case "folder"
                AddChild(New ScriptBlock("folderaction", False, False, "", Scripter))
            Case "rawcode", "import", "var", "funargs", "funargblock",
                 "funcontent", "ifcondition", "ifthen",
                 "ifelse", "foraction", "forinit",
                 "forcondition", "forincre", "whilecondition",
                 "whileaction", "switchvar", "switchcase", "or",
                "and", "folderaction"
            Case Else
                Dim index As Integer = Tool.TEEpsDefaultFunc.SearchFunc(name)

                If index >= 0 Then
                    Dim functooltip As FunctionToolTip = Tool.TEEpsDefaultFunc.GetFuncTooltip(index)
                    Dim argument As String = Tool.TEEpsDefaultFunc.GetFuncArgument(index)
                    If argument.Trim <> "" Then
                        Dim args() As String = argument.Split(",")
                        For i = 0 To args.Length - 1
                            Dim tstr() As String = args(i).Split(":")

                            Dim vname As String = "defaultvalue" 'tstr.First
                            Dim vtype As String = tstr.Last

                            AddChild(New ScriptBlock(vtype, False, False, vname, Scripter))
                        Next
                    End If
                Else
                    Dim func As ScriptBlock = tescm.GetFuncInfor(name, Scripter)
                    If func Is Nothing Then
                        If Not IsValue() Then
                            name = "funcisnotexist"
                        End If
                    Else
                        Dim args As List(Of ScriptBlock) = tescm.GetFuncArgs(func)


                        For i = 0 To args.Count - 1
                            Dim vname As String = args(i).value
                            Dim vtype As String = args(i).name

                            AddChild(New ScriptBlock(vtype, False, False, vname, Scripter))
                        Next

                    End If
                End If
        End Select
    End Sub


    Private Sub AddText(Inline As InlineCollection, text As String, brush As Brush)
        Dim Run As New Run(text)
        If brush IsNot Nothing Then
            Run.Foreground = brush
        End If
        Inline.Add(Run)
    End Sub

    Public Function ValueCoder() As String
        If value.Trim = "defaultvalue" Then
            Return name.Trim
        End If

        Return value.Trim
    End Function
    Public Sub ArgCoder(lnlines As InlineCollection)
        AddText(lnlines, "이름 : ", Nothing)
        AddText(lnlines, value, tescm.HighlightBrush)
        AddText(lnlines, "    타입 : ", Nothing)
        AddText(lnlines, name, tescm.HighlightBrush)
    End Sub


    Public Sub FuncCoder(lnlines As InlineCollection)
        Dim i As Integer = Tool.TEEpsDefaultFunc.SearchFunc(name)

        If i >= 0 Then
            Dim functooltip As FunctionToolTip = Tool.TEEpsDefaultFunc.GetFuncTooltip(i)
            If functooltip.Summary.Trim = "" Then
                DefaultCoder(lnlines)
                Return
            End If





            Dim argumentstr As String = functooltip.Summary.Split(vbCrLf).First
            argumentstr = functooltip.Summary.Replace(argumentstr, "").Trim

            Dim arglist As New List(Of String)
            Dim args() As String = Tool.TEEpsDefaultFunc.GetFuncArgument(i).Split(",")

            Dim vcount As Integer = 0
            For k = 0 To args.Count - 1
                Dim aname As String = args(k).Split(":").First.Trim

                arglist.Add(aname)


                If argumentstr.IndexOf("[" & aname & "]") <> -1 Then
                    vcount += 1
                End If
                argumentstr = argumentstr.Replace("[" & aname & "]", "ᐱᐳ" & aname & "ᐱ")

            Next
            Dim values() As String = argumentstr.Split("ᐱ")
            If vcount <> args.Count Then
                DefaultCoder(lnlines)
                Return
            End If


            For k = 0 To values.Count - 1
                If values(k).Trim <> "" Then
                    If values(k)(0) = "ᐳ" Then
                        Dim vname As String = Mid(values(k), 2)

                        AddText(lnlines, child(arglist.IndexOf(vname)).ValueCoder(), tescm.HighlightBrush)
                    Else
                        AddText(lnlines, values(k), Nothing)
                    End If
                End If

            Next

        Else
            DefaultCoder(lnlines)
        End If
    End Sub

    Private Sub DefaultCoder(lnlines As InlineCollection)
        AddText(lnlines, name, Nothing)
        AddText(lnlines, "(", Nothing)
        For i = 0 To child.Count - 1
            If i <> 0 Then
                AddText(lnlines, ", ", Nothing)
            End If

            AddText(lnlines, child(i).ValueCoder(), tescm.HighlightBrush)

        Next
        AddText(lnlines, ")", Nothing)




        'Dim func As ScriptBlock = tescm.GetFuncInfor(name, Scripter)
        'Dim args As List(Of ScriptBlock) = tescm.GetFuncArgs(func)


        'For j = 0 To args.Count - 1
        '    Dim vname As String = args(j).value
        '    Dim vtype As String = args(j).name

        '    AddChild(New ScriptBlock(vtype, False, False, vname, Scripter))
        'Next
    End Sub


    Public Enum SType
        con
        act
        plibfun
        fun
        none
    End Enum

    Public Function GetSType() As SType
        Dim scrb As ScriptBlock = tescm.GetFuncInfor(name, Scripter)
        If scrb IsNot Nothing Then
            Return SType.fun
        End If

        Dim i As Integer = Tool.TEEpsDefaultFunc.SearchFunc(name)
        If Tool.TEEpsDefaultFunc.GetFuncTooltip(i).Type = FunctionToolTip.FType.Cond Then
            Return SType.con
        ElseIf Tool.TEEpsDefaultFunc.GetFuncTooltip(i).Type = FunctionToolTip.FType.Act Then
            Return SType.act
        ElseIf Tool.TEEpsDefaultFunc.GetFuncTooltip(i).Type = FunctionToolTip.FType.Func Then
            Return SType.plibfun
        End If
        Return SType.none
    End Function

    Public Function IsCondition() As Boolean
        Dim i As Integer = Tool.TEEpsDefaultFunc.SearchFunc(name)
        If i = -1 Then
            Return False
        End If

        If Tool.TEEpsDefaultFunc.GetFuncTooltip(i).Type = FunctionToolTip.FType.Cond Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function IsAction() As Boolean
        Dim i As Integer = Tool.TEEpsDefaultFunc.SearchFunc(name)
        If i = -1 Then
            Return False
        End If

        If Tool.TEEpsDefaultFunc.GetFuncTooltip(i).Type = FunctionToolTip.FType.Act Then
            Return True
        Else
            Return False
        End If
    End Function


    Public Function IsEditFunc() As Boolean
        Dim scrb As ScriptBlock = tescm.GetFuncInfor(name, Scripter)
        If scrb IsNot Nothing Then
            Return True
        End If


        Dim i As Integer = Tool.TEEpsDefaultFunc.SearchFunc(name)
        If i = -1 Then
            Return False
        End If
        Return True
    End Function
    Public Function IsFunction() As Boolean
        Dim scrb As ScriptBlock = tescm.GetFuncInfor(name, Scripter)
        If scrb IsNot Nothing Then
            Return True
        End If


        Dim i As Integer = Tool.TEEpsDefaultFunc.SearchFunc(name)
        If i = -1 Then
            Return False
        End If

        If Tool.TEEpsDefaultFunc.GetFuncTooltip(i).Type = FunctionToolTip.FType.Func Then
            Return True
        Else
            Return False
        End If
    End Function


    Public Function IsDeleteAble() As Boolean
        Select Case name
            Case "funargs", "funcontent", "ifcondition", "ifthen",
                "forcontent", "foraction", "forinit",
                 "forcondition", "forincre", "whilecondition",
                 "whileaction", "switchvar",
                 "folderaction"
            Case Else
                Return True
        End Select
        Return False
    End Function
    Public Function IsValue() As Boolean
        Select Case name
            Case "rawcode", "import", "var", "function", "funargs", "funargblock",
                 "funcontent", "if", "elseif", "ifcondition", "ifthen",
                 "ifelse", "for", "forcontent", "foraction", "forinit",
                 "forcondition", "forincre", "while", "whilecondition",
                 "whileaction", "switch", "switchvar", "switchcase", "or",
                "and", "folder", "folderaction"
            Case Else
                If Not value = "" Then
                    Return True
                End If
        End Select
        Return False
    End Function
    Public Function IsArgument() As Boolean
        If name = "funargblock" Then
            Return True
        End If
        If IsValue() Then
            If Parent.name = "funargs" Then
                Return True
            End If
        End If
        Return False
    End Function




    Public Function IsFolderScript() As Boolean
        Select Case name
            Case "ifcondition", "ifthen", "ifelse", "funargs", "funcontent", "whilecondition", "whileaction", "switchcase", "or", "and", "folderaction"
                Return True
        End Select
        Return False
    End Function


    Private Function GetScriptBlockItem() As ScriptTreeviewitem
        Dim stvi As New ScriptTreeviewitem(Me)

        Return stvi
    End Function

    Public Sub RefreshListBox(Tvitem As TreeViewItem)
        Tvitem.Header = GetScriptBlockItem()

    End Sub


    Public Sub treeviewcollapsed(sender As TreeViewItem, e As RoutedEventArgs)
        Dim sb As ScriptBlock = sender.Tag

        sb.isexpand = sender.IsExpanded
    End Sub

    Public Function GetTreeviewitem() As TreeViewItem
        Dim treeviewitem As New TreeViewItem
        treeviewitem.Tag = Me
        treeviewitem.Header = GetScriptBlockItem()
        treeviewitem.IsExpanded = isexpand

        AddHandler treeviewitem.Collapsed, AddressOf treeviewcollapsed

        Dim valuesaction As Boolean = False
        Dim groupname As String = ""
        Select Case name
            Case "rawcode"
            Case "import"
                groupname = "EtcBlock"
            Case "var"
                groupname = "Variable"
            Case "function"
                groupname = "Func"
            Case "funargs", "funcontent"
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Func"
            Case "funargblock"
                groupname = "Func"
            Case "if"
                groupname = "Control"
            Case "elseif"
                groupname = "Control"
            Case "ifcondition"
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Control"
            Case "ifthen"
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Control"
            Case "ifelse"
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Control"
            Case "for"
                groupname = "Control"
            Case "forcontent", "foraction"
                groupname = "Control"
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case "forinit", "forcondition", "forincre"
                groupname = "Control"
            Case "while"
                groupname = "Control"
            Case "whilecondition"
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Control"
            Case "whileaction"
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Control"
            Case "switch", "switchvar"
                groupname = "Control"
            Case "switchcase"
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Control"
            Case "or"
                treeviewitem.Background = Brushes.YellowGreen
                'treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case "and"
                treeviewitem.Background = Brushes.GreenYellow
                'treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case "folder"
                groupname = "EtcBlock"
            Case "folderaction"
                groupname = "EtcBlock"
            Case Else
                If value = "" Then
                    Dim index As Integer = Tool.TEEpsDefaultFunc.SearchFunc(name)

                    If index >= 0 Then
                        If Tool.TEEpsDefaultFunc.GetFuncTooltip(index).Type = FunctionToolTip.FType.Act Then
                            groupname = "Action"
                        ElseIf Tool.TEEpsDefaultFunc.GetFuncTooltip(index).Type = FunctionToolTip.FType.Cond Then
                            groupname = "Condition"
                        ElseIf Tool.TEEpsDefaultFunc.GetFuncTooltip(index).Type = FunctionToolTip.FType.Func Then
                            groupname = "plibFunc"
                        End If
                    Else
                        If IsFunction() Then
                            groupname = "Func"
                        End If
                    End If
                    valuesaction = True
                Else
                    groupname = "Value"
                End If

        End Select
        If groupname <> "" Then
            Dim colorcode As String = tescm.Tabkeys(groupname)
            treeviewitem.Background = New SolidColorBrush(ColorConverter.ConvertFromString(colorcode))
        End If


        'treeviewitem.Header = name


        If Not valuesaction Then
            For i = 0 To child.Count - 1
                treeviewitem.Items.Add(child(i).GetTreeviewitem())
            Next
        End If


        Return treeviewitem
    End Function
End Class