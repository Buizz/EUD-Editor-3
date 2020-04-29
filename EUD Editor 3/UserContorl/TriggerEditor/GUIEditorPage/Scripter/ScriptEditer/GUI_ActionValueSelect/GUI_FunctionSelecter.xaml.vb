Public Class GUI_FunctionSelecter
    Public Event SelectEvent As RoutedEventHandler
    Private GUIEditor As GUIScriptEditorUI


    Private initvalue As String
    Private fliter As String = ""

    Public Sub New(_initvalue As String, _GUIEditor As GUIScriptEditorUI)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        GUIEditor = _GUIEditor
        initvalue = _initvalue



        For i = 0 To functype.Items.Count - 1
            Dim itemtag As String = CType(functype.Items(i), ComboBoxItem).Tag
            If itemtag = initvalue Then
                functype.SelectedIndex = i
                Exit For
            End If
        Next

        If functype.SelectedIndex = -1 Then
            functype.SelectedIndex = 0
        End If



        isLoad = True
    End Sub

    Private isLoad As Boolean = False

    Private Sub ListReset()
        mainlist.Items.Clear()

        Select Case initvalue
            Case "action"
                For i = 0 To Tool.TEEpsDefaultFunc.FuncCount - 1
                    If Tool.TEEpsDefaultFunc.GetFuncTooltip(i).Type = FunctionToolTip.FType.Act Then

                        Dim keyname As String = Tool.TEEpsDefaultFunc.GetFuncName(i)

                        Dim headername As String = Tool.GetText(keyname)
                        Dim flitertext As String
                        If headername = "" Then
                            flitertext = keyname
                        Else
                            flitertext = headername
                        End If


                        If fliter.Trim = "" Or flitertext.ToLower.IndexOf(fliter.ToLower()) <> -1 Then
                            Dim tlistboxitem As New ListBoxItem
                            tlistboxitem.Tag = {ScriptBlock.EBlockType.action, keyname}
                            tlistboxitem.Content = flitertext
                            mainlist.Items.Add(tlistboxitem)
                        End If
                    End If
                Next
            Case "condition"
                For i = 0 To Tool.TEEpsDefaultFunc.FuncCount - 1
                    If Tool.TEEpsDefaultFunc.GetFuncTooltip(i).Type = FunctionToolTip.FType.Cond Then

                        Dim keyname As String = Tool.TEEpsDefaultFunc.GetFuncName(i)

                        Dim headername As String = Tool.GetText(keyname)
                        Dim flitertext As String
                        If headername = "" Then
                            flitertext = keyname
                        Else
                            flitertext = headername
                        End If


                        If fliter.Trim = "" Or flitertext.ToLower.IndexOf(fliter.ToLower()) <> -1 Then
                            Dim tlistboxitem As New ListBoxItem
                            tlistboxitem.Tag = {ScriptBlock.EBlockType.condition, keyname}
                            tlistboxitem.Content = flitertext
                            mainlist.Items.Add(tlistboxitem)
                        End If
                    End If
                Next

            Case "default"
                Dim strs As List(Of String) = tescm.GetFuncList(GUIEditor.PTEFile)
                For i = 0 To strs.Count - 1
                    Dim keyname As String = strs(i)


                    If fliter.Trim = "" Or keyname.ToLower.IndexOf(fliter.ToLower()) <> -1 Then
                        Dim tlistboxitem As New ListBoxItem
                        tlistboxitem.Tag = {ScriptBlock.EBlockType.funuse, keyname}
                        tlistboxitem.Content = keyname
                        mainlist.Items.Add(tlistboxitem)
                    End If
                Next

            Case "plib"
                For i = 0 To Tool.TEEpsDefaultFunc.FuncCount - 1
                    If Tool.TEEpsDefaultFunc.GetFuncTooltip(i).Type = FunctionToolTip.FType.Func Then

                        Dim keyname As String = Tool.TEEpsDefaultFunc.GetFuncName(i)

                        Dim headername As String = Tool.GetText(keyname)
                        Dim flitertext As String
                        If headername = "" Then
                            flitertext = keyname
                        Else
                            flitertext = headername
                        End If


                        If fliter.Trim = "" Or flitertext.ToLower.IndexOf(fliter.ToLower()) <> -1 Then
                            Dim tlistboxitem As New ListBoxItem
                            tlistboxitem.Tag = {ScriptBlock.EBlockType.plibfun, keyname}
                            tlistboxitem.Content = flitertext
                            mainlist.Items.Add(tlistboxitem)
                        End If
                    End If
                Next
            Case "macro"
                For i = 0 To macro.FunctionList.Count - 1
                    Dim keyname As String = macro.FunctionList(i).Fname
                    If fliter.Trim = "" Or keyname.ToLower.IndexOf(fliter.ToLower()) <> -1 Then
                        Dim tlistboxitem As New ListBoxItem
                        tlistboxitem.Tag = {ScriptBlock.EBlockType.macrofun, macro.FunctionList(i).Fname}
                        tlistboxitem.Content = keyname
                        mainlist.Items.Add(tlistboxitem)
                    End If
                Next
            Case "extern"
                For j = 0 To GUIEditor.Script.ExternFile.Count - 1
                    Dim tcfun As CFunc = GUIEditor.Script.ExternFile(j).Funcs

                    Dim namespacename As String = GUIEditor.Script.ExternFile(j).nameSpaceName

                    For i = 0 To tcfun.FuncCount - 1
                        Dim keyname As String = tcfun.GetFuncName(i)

                        If fliter.Trim = "" Or keyname.ToLower.IndexOf(fliter.ToLower()) <> -1 Then
                            Dim tlistboxitem As New ListBoxItem
                            tlistboxitem.Tag = {ScriptBlock.EBlockType.externfun, namespacename & "." & keyname}
                            tlistboxitem.Content = namespacename & "." & keyname
                            mainlist.Items.Add(tlistboxitem)
                        End If
                    Next
                Next
        End Select



    End Sub

    Private Sub mainlist_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim item As ListBoxItem = mainlist.SelectedItem
        If item IsNot Nothing Then
            RaiseEvent SelectEvent(item.Tag, e)
        End If
    End Sub

    Private Sub FliterText_TextChanged(sender As Object, e As TextChangedEventArgs)
        fliter = FliterText.Text
        ListReset()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        FliterText.Text = ""
    End Sub

    Private Sub functype_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If functype.SelectedIndex <> -1 Then
            initvalue = CType(functype.SelectedItem, ComboBoxItem).Tag
            ListReset()
        End If
    End Sub
End Class
