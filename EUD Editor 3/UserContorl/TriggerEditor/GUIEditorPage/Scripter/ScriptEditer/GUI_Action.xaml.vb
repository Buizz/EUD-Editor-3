Public Class GUI_Action

    Public Sub CrlInit()
        '//////////////////////////////
        '초기화 식
        valueEditPanel.Init(ValueSelect, dotscr, p._GUIScriptEditorUI)
        AddHandler valueEditPanel.BtnRefresh, AddressOf ArgSelecter.argBtnRefresh


        AddHandler ArgSelecter.ArgBtnRefreshEvent, AddressOf ArgBtnRefreshEvent
        AddHandler ArgSelecter.ArgBtnClickEvent, AddressOf ArgBtnClickEvent

        'Dim colorcode As String = ""
        'Select Case scr.ScriptType
        '    Case ScriptBlock.EBlockType.action
        '        colorcode = tescm.Tabkeys("Action")
        '    Case ScriptBlock.EBlockType.condition
        '        colorcode = tescm.Tabkeys("Condition")
        '    Case ScriptBlock.EBlockType.plibfun
        '        colorcode = tescm.Tabkeys("plibFunc")
        '    Case ScriptBlock.EBlockType.funuse
        '        colorcode = tescm.Tabkeys("Func")
        '    Case ScriptBlock.EBlockType.externfun
        '        colorcode = tescm.Tabkeys("Func")
        '    Case ScriptBlock.EBlockType.macrofun
        '        colorcode = tescm.Tabkeys("MacroFunc")
        'End Select
        'colorbox.Background = New SolidColorBrush(ColorConverter.ConvertFromString(colorcode))
        ArgSelecter.CrlInit(scr)

        'fname.Text = scr.name
        valueEditPanel.Visibility = Visibility.Collapsed
    End Sub

    Private Sub ArgBtnRefreshEvent(sender As Object, e As RoutedEventArgs)
        btnRefresh()
    End Sub
    Private Sub ArgBtnClickEvent(sender As Object, e As RoutedEventArgs)
        valueEditPanel.Visibility = Visibility.Visible
        valueEditPanel.ComboboxInit(sender)
    End Sub




    Public Sub OkayAction(sender As Object, e As RoutedEventArgs)
        '//////////////////////////////
        '스크립트 갱신
        ArgSelecter.UpdateValue()

        'For i = 0 To scr.child.Count - 1
        '    scr.ReplaceChild(ArgSelecter.EditValues(i), i)
        'Next
    End Sub


    Public Shared Function IsDefaultValue(str As String) As Boolean
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

    Private Function CheckEditable() As Boolean
        '//////////////////////////////
        '확인을 누를 수 있는지 확인
        For i = 0 To ArgSelecter.EditValues.Count - 1
            If IsDefaultValue(ArgSelecter.EditValues(i).value) Then
                Return False
            End If
        Next
        Return True
    End Function







    Public Class tagcontainer
        Public Sub New(_scr As ScriptBlock, _desscr As ScriptBlock, _des As String)
            scr = _scr
            desscr = _desscr
            des = _des
        End Sub
        Public scr As ScriptBlock
        Public desscr As ScriptBlock
        Public des As String
    End Class





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
    Public Sub btnRefresh()
        If CheckEditable() Then
            p.OkBtn.IsEnabled = True
        Else
            p.OkBtn.IsEnabled = False
        End If
    End Sub

End Class
