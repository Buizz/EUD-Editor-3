Public Class GUI_SwitchCase
    Public Sub CrlInit()
        '//////////////////////////////
        '초기화 식
        vtb.Text = scr.value
        breakcb.IsChecked = scr.flag
    End Sub
    Public Sub OkayAction(sender As Object, e As RoutedEventArgs)
        '//////////////////////////////
        '스크립트 갱신
        scr.value = vtb.Text
        scr.flag = breakcb.IsChecked
    End Sub
    Private Function CheckEditable() As Boolean
        '//////////////////////////////
        '확인을 누를 수 있는지 확인
        If vtb.Text.Trim = "" Then
            Return False
        End If
        Return True
    End Function


    Private p As GUIScriptEditerWindow
    Private scr As ScriptBlock
    Public Sub New(tp As GUIScriptEditerWindow, tscr As ScriptBlock)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        p = tp
        scr = tscr

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

    Private Sub vtb_TextChanged(sender As Object, e As TextChangedEventArgs)
        btnRefresh()
    End Sub

    Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
        btnRefresh()
    End Sub
End Class
