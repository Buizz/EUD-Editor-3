Public Class GUI_Folder
    Public Sub CrlInit()
        '//////////////////////////////
        '초기화 식

        Dim sstr() As String = scr.value.Split("ᗋ")
        Dim head As String = sstr.First
        Dim tail As String = sstr.Last

        headerText.Text = head
        TailText.Text = tail
        If scr.value <> "" Then
            TailText.IsEnabled = False
            If head = tail Then
                TailText.IsEnabled = True
                chbox.IsChecked = True
            End If
        End If
        isLoad = True
    End Sub
    Public Sub OkayAction(sender As Object, e As RoutedEventArgs)
        '//////////////////////////////
        '스크립트 갱신
        scr.value = headerText.Text & "ᗋ" & TailText.Text
    End Sub
    Private Function CheckEditable() As Boolean
        '//////////////////////////////
        '확인을 누를 수 있는지 확인
        If headerText.Text.Trim = "" Then
            Return False
        End If

        Return True
    End Function


    Private isLoad As Boolean = False


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

    Private Sub chbox_Checked(sender As Object, e As RoutedEventArgs)
        If isLoad Then
            TailText.IsEnabled = True
        End If
    End Sub

    Private Sub chbox_Unchecked(sender As Object, e As RoutedEventArgs)
        If isLoad Then
            TailText.Text = headerText.Text
            TailText.IsEnabled = False
        End If
    End Sub

    Private Sub headerText_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not TailText.IsEnabled Then
            TailText.Text = headerText.Text
        End If
        btnRefresh()
    End Sub

    Private Sub TailText_TextChanged(sender As Object, e As TextChangedEventArgs)
        If TailText.IsEnabled Then
            btnRefresh()
        End If
    End Sub
End Class
