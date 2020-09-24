Public Class GUI_Switch
    Public Sub CrlInit()
        '//////////////////////////////
        '초기화 식
        valueEditPanel.Init(ValueSelect, dotscr, p._GUIScriptEditorUI)
        valueEditPanel.typebtn3.Visibility = Visibility.Collapsed

        valueEditPanel.ComboboxInit(scr.VChild)

        'ValuePreview.Text = scr.VChild.ValueCoder

        'Dim varlist As List(Of ScriptBlock) = tescm.GetAllVar(dotscr, p._GUIScriptEditorUI.Script)
        'Dim index As Integer = -1
        'For i = 0 To varlist.Count - 1
        '    Dim cbitem As New ComboBoxItem
        '    cbitem.Tag = varlist(i).value
        '    cbitem.Content = varlist(i).value

        '    'ValueSelect.Items.Add(cbitem)
        'Next

        'If scr.value = "defaultvalue" Then
        '    'ValueSelect.Text = ""
        'Else
        '    'ValueSelect.Text = scr.value
        'End If
    End Sub
    Public Sub OkayAction(sender As Object, e As RoutedEventArgs)
        '//////////////////////////////
        '스크립트 갱신
        'scr.value = ValueSelect.Text
    End Sub
    Private Function CheckEditable() As Boolean
        '//////////////////////////////
        '확인을 누를 수 있는지 확인
        'If ValueSelect.Text.Trim = "" Then
        '    Return False
        'End If

        Return True
    End Function


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

    Private Sub NameText_TextChanged(sender As Object, e As TextChangedEventArgs)
        btnRefresh()
    End Sub

    Private Sub ValueSelect_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        btnRefresh()
    End Sub
End Class
