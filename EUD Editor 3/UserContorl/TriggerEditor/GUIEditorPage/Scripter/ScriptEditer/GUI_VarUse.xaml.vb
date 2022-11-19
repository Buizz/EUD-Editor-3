Public Class GUI_VarUse
    Private isload As Boolean = False

    Public Sub CrlInit()
        '//////////////////////////////
        '초기화 식
        'Dim values As List(Of String) = GUIScriptManager.SplitText(scr.value)

        '변수가 객체인지 아닌지 판단.
        If True Then
            Dim n As String() = {"대입", "덧셈", "곱셈", "뺄셈", "나눗셈"}
            Dim t As String() = {"SetTo", "Add", "Mul", "Sub", "나눗셈"}
            For i = 0 To n.Count - 1
                Dim cbitem As New ComboBoxItem()

                maincombobox.Items.Add(cbitem)
            Next
        End If


        isload = True
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
