Public Class GUI_Return
    Private isload As Boolean = False

    Private EditValues As ScriptBlock
    Public Sub CrlInit()
        '//////////////////////////////
        '초기화 식
        EditValues = scr.child(0).DeepCopy

        valueEditPanel.Init(p._GUIScriptEditorUI.TEGUIPage.ValueSelecter, dotscr, p._GUIScriptEditorUI, True)
        AddHandler valueEditPanel.BtnRefresh, AddressOf AgrbtnRefresh
        valueEditPanel.ComboboxInit(EditValues)

        returnVal.Text = "반환 값 : " & EditValues.ValueCoder

        isload = True
    End Sub
    Public Sub AgrbtnRefresh(sender As String, e As RoutedEventArgs)
        'sender.Last 선택한 값
        returnVal.Text = "반환 값 : " & EditValues.ValueCoder
    End Sub

    Public Sub OkayAction(sender As Object, e As RoutedEventArgs)
        '//////////////////////////////
        '스크립트 갱신
        scr.child(0).DuplicationBlock(EditValues)
    End Sub
    Private Function CheckEditable() As Boolean
        '//////////////////////////////
        '확인을 누를 수 있는지 확인

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
End Class
