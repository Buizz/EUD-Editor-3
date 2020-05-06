Imports System.Windows.Markup

Public Class GUI_VariableSelecter
    Public Event SelectEvent As RoutedEventHandler
    Private GUIEditor As GUIScriptEditorUI

    Private dotscr As ScriptBlock
    Private scr As ScriptBlock

    Private fliter As String = ""
    Private fliter2 As String = ""


    Private VariableName As String
    Private VariableFuncName As String

    Private Function CheckVar(varname As String, scrlist As List(Of ScriptBlock)) As Boolean
        '리스트를 선택하는 함수.
        If scrlist.Count = 1 Then
            If scrlist(0).value = varname Then
                Select Case scrlist(0).value2
                    Case "var"
                        variabletype.SelectedIndex = 2
                    Case "static"
                        variabletype.SelectedIndex = 3
                    Case "const"
                        variabletype.SelectedIndex = 1
                    Case "object"
                        variabletype.SelectedIndex = 4
                End Select
            End If

            Return True
        End If


        Return False
    End Function
    Public Sub New(initvalue() As String, _GUIEditor As GUIScriptEditorUI, _dotscr As ScriptBlock)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        GUIEditor = _GUIEditor
        dotscr = _dotscr

        VariableName = initvalue(0)
        VariableFuncName = initvalue(1)

        Dim flag As Boolean = False
        If VariableName = "init" Then
            variabletype.SelectedIndex = 0
            flag = True
        End If
        If VariableFuncName = "init" Then
            variablepos.SelectedIndex = 0
            flag = True
        End If

        '타입 체크해야됨
        If Not flag Then
            Dim tf As Boolean = False
            tf = CheckVar(VariableName, tescm.GetLocalVar(dotscr, "", VariableName))
            If tf Then
                variablepos.SelectedIndex = 1
            Else
                tf = CheckVar(VariableName, tescm.GetGlobalVar(GUIEditor.Script, "", VariableName))
                If tf Then
                    variablepos.SelectedIndex = 2
                Else
                    tf = CheckVar(VariableName, tescm.GetExternVar(GUIEditor.Script, "", VariableName))
                    If tf Then
                        variablepos.SelectedIndex = 3
                    End If
                End If
            End If
        End If

        ListReset()


        'For i = 0 To functype.Items.Count - 1
        '    Dim itemtag As String = CType(functype.Items(i), ComboBoxItem).Tag
        '    If itemtag = initvalue Then
        '        functype.SelectedIndex = i
        '        Exit For
        '    End If
        'Next

        'If functype.SelectedIndex = -1 Then
        '    functype.SelectedIndex = 0
        'End If





        '해야할것
        'Object이름  StringBuffer

        'const array = EUDArray(8);
        'array[0] = StringBuffer.alloc();



        'const a = StringBuffer.cast(array[0]);
        'a.print;





        isLoad = True
    End Sub

    Private isLoad As Boolean = False

    Private Sub ListReset()
        If variabletype.SelectedIndex = -1 Or variablepos.SelectedIndex = -1 Then
            Return
        End If
        mainlist.Items.Clear()

        Dim vartype As String = ""
        Select Case variabletype.SelectedIndex
            Case 0
                vartype = ""
            Case 1
                vartype = "const"
            Case 2
                vartype = "var"
            Case 3
                vartype = "static"
            Case 4
                vartype = "object"
        End Select


        Dim scrlist As New List(Of ScriptBlock)
        Select Case variablepos.SelectedIndex
            Case 0
                '전체
                scrlist.AddRange(tescm.GetLocalVar(dotscr, vartype))
                scrlist.AddRange(tescm.GetGlobalVar(GUIEditor.Script, vartype))
                scrlist.AddRange(tescm.GetExternVar(GUIEditor.Script, vartype))
            Case 1
                '로컬
                scrlist.AddRange(tescm.GetLocalVar(dotscr, vartype))
            Case 2
                '글로벌
                scrlist.AddRange(tescm.GetGlobalVar(GUIEditor.Script, vartype))
            Case 3
                '외부파일
                scrlist.AddRange(tescm.GetExternVar(GUIEditor.Script, vartype))
        End Select


        For i = 0 To scrlist.Count - 1
            Dim listitem As New ListBoxItem

            listitem.Content = scrlist(i).value
            listitem.Tag = scrlist(i)

            mainlist.Items.Add(listitem)
        Next

        '"const"
        '"default"
        '"object"

        '"local"
        '"global"
        '"extern"

    End Sub

    Private Sub FuncAddList(scrlist As List(Of ScriptBlock))
        For i = 0 To scrlist.Count - 1

            Dim listitem As New ListBoxItem

            listitem.Content = scrlist(i).value
            listitem.Tag = scrlist(i)

            funclist.Items.Add(listitem)
        Next
    End Sub
    Private Sub FuncListReset()
        funclist.Items.Clear()
        'MsgBox(VariableName)

        Dim isExtern As Boolean = False

        Dim k As ScriptBlock

        Dim tscrlist As List(Of ScriptBlock) = tescm.GetLocalVar(dotscr, "", VariableName)

        If tscrlist.Count = 0 Then
            tscrlist = tescm.GetGlobalVar(GUIEditor.Script, "", VariableName)
            If tscrlist.Count = 0 Then
                tscrlist = tescm.GetExternVar(GUIEditor.Script, "", VariableName)
                If tscrlist.Count = 0 Then
                    Return
                End If
                k = tscrlist.First
                isExtern = True
            Else
                k = tscrlist.First
            End If
        Else
            k = tscrlist.First
        End If


        'MsgBox("VariableName : " & VariableName)
        'MsgBox("선언한 변수 명" & k.value)
        'k = 변수 선언부, 이 변수의 child를 이용

        'MsgBox("선언한 변수의 초기값" & k.child.Count)
        'k = 변수 선언부, 이 변수의 child를 이용
        If k.child.Count = 0 Then
            Return
        End If

        'MsgBox("오브젝트 타입" & k.child(0).ScriptType.ToString)
        'MsgBox("오브젝트 이름" & k.child(0).name & "," & k.child(0).value & "," & k.child(0).value2)

        Dim n As String = ""
        If VariableName.IndexOf(".") >= 0 Then
            n = VariableName.Split(".").First
        End If

        Dim scr As ScriptBlock = tescm.GetObjectByName(k.child(0).name, GUIEditor.Script, n)
        '변수명을 이용해 본래 변수 정의 즉 var을 찾아야됨.


        If scr Is Nothing Then
            Return
        End If


        If True Then
            Dim listitem As New ListBoxItem

            listitem.Content = "주소"
            listitem.Tag = "!default"

            funclist.Items.Add(listitem)
        End If
        If True Then
            Dim listitem As New ListBoxItem

            listitem.Content = "인덱스"
            listitem.Tag = "!index"

            funclist.Items.Add(listitem)
        End If


        funclist.Items.Add(New Separator)

        FuncAddList(tescm.GetObjectMethod(scr))


        funclist.Items.Add(New Separator)


        FuncAddList(tescm.GetObjectField(scr))

    End Sub



    Private Sub FliterText_TextChanged(sender As Object, e As TextChangedEventArgs)
        fliter = FliterText.Text
        ListReset()
    End Sub
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        FliterText.Text = ""
    End Sub
    Private Sub FliterText2_TextChanged(sender As Object, e As TextChangedEventArgs)
        fliter2 = FliterText2.Text
        FuncListReset()
    End Sub
    Private Sub Fliter2_Click(sender As Object, e As RoutedEventArgs)
        FliterText2.Text = ""
    End Sub

    Private Sub variabletype_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If isLoad Then
            ListReset()
        End If
    End Sub
    Private Sub variablepos_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If isLoad Then
            ListReset()
        End If
    End Sub

    Private Sub mainlist_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim item As ListBoxItem = mainlist.SelectedItem
        Dim itemscr As ScriptBlock
        If item IsNot Nothing Then
            itemscr = item.Tag
            VariableName = itemscr.value
        Else
            Return
        End If

        If itemscr.value2 = "object" Then
            '만약 선택한게 오브젝트일 경우
            VarSelecter.Visibility = Visibility.Collapsed
            FuncSelecter.Visibility = Visibility.Visible
            FuncListReset()
        Else
            '아닐 경우
            RaiseEvent SelectEvent({VariableName, ""}, e)
        End If
        mainlist.SelectedIndex = -1
    End Sub
    Private Sub funclist_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim item As ListBoxItem = funclist.SelectedItem
        If item IsNot Nothing Then
            If item.Tag.GetType Is GetType(String) Then
                RaiseEvent SelectEvent({VariableName, item.Tag, ScriptBlock.EBlockType.constVal}, e)
            Else
                Dim scr As ScriptBlock = item.Tag


                VariableFuncName = scr.value

                RaiseEvent SelectEvent({VariableName, VariableFuncName, scr.ScriptType}, e)
            End If
        Else
            Return
        End If
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        VarSelecter.Visibility = Visibility.Visible
        FuncSelecter.Visibility = Visibility.Collapsed
    End Sub

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        FliterText.Focus()
    End Sub
End Class
