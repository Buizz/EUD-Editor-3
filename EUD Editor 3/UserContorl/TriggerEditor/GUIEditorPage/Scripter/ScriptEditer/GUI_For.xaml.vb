Public Class GUI_For
    Private isLoad As Boolean = False
    Public Sub CrlInit()
        '//////////////////////////////
        '초기화 식


        '값 유형
        'var i = 0 ; i < 5 ; i++
        If scr.value = "defaultvalue" Then
            SetCombobox("CountRepeat", "")
            isLoad = True
            Return
        End If

        Dim type As String = scr.value.Split(";").First
        Dim vvalue As String = scr.value.Split(";").Last

        SetCombobox(type, vvalue)
        'Dim foreachvalue() As String = scr.value.Split(":")
        'If foreachvalue.Count = 2 Then

        'End If


        isLoad = True
    End Sub

    Private Sub SetCombobox(tags As String, value As String)
        For i = 0 To ForTypeCB.Items.Count - 1
            If ForTypeCB.Items(i).GetType Is GetType(ComboBoxItem) Then
                If CType(ForTypeCB.Items(i), ComboBoxItem).Tag = tags Then
                    ForTypeCB.SelectedIndex = i
                    Exit For
                End If
            End If
        Next

        ResetControl(value)
    End Sub



    Public Sub OkayAction(sender As Object, e As RoutedEventArgs)
        '//////////////////////////////
        '스크립트 갱신
        Dim ForType As String = CType(ForTypeCB.SelectedItem, ComboBoxItem).Tag
        Dim rvalue As String = ""

        Select Case ForType
            Case "UserEdit"
                If forrbtn.IsChecked Then
                    rvalue = "For"
                End If
                If foreachrbtn.IsChecked Then
                    rvalue = "ForEach"
                End If

                rvalue = rvalue & "ᗢ" & useredittextbox.Text
            Case "CountRepeat"
                Dim vname As String = count_vname.Text
                Dim vinit As Integer = count_vinit.Text
                Dim vcount As Integer = count_vcount.Text

                rvalue = vname & "ᗢ" & vinit & "ᗢ" & vcount
                'rvalue = "var " & vname & " = " & vinit & "; " & vname & " < " & vinit + vcount & "; " & vname & "++"
            Case "EUDLoopNewUnit", "EUDLoopUnit", "EUDLoopUnit2", "EUDLoopSprite"
                Dim vname As String = ptr_vname.Text & ", " & epd_vname.Text
                '원래 아무것도 없음
                rvalue = vname
            Case "Timeline"
                Dim vname As String = Timeline_vname.Text
                rvalue = vname & "ᗢ" & timetextbox.Text
            Case "EUDLoopPlayerUnit"
                Dim vname As String = PlayerUnit_v1.Text & ", " & PlayerUnit_v2.Text
                rvalue = vname & "ᗢ" & playerselect.SelectedIndex
            Case "EUDLoopPlayer"
                Dim vname As String = playerloop_vname.Text
                rvalue = vname & "ᗢ" & CType(playerloopcb1.SelectedItem, ComboBoxItem).Tag & "ᗢ" &
                    CType(playerloopcb2.SelectedItem, ComboBoxItem).Tag & "ᗢ" &
                    CType(playerloopcb3.SelectedItem, ComboBoxItem).Tag & "ᗢ" & playerloopcheckbox.IsChecked
            Case "EUDPlayerLoop"
                '원래 아무것도 업슴
        End Select

        scr.value = ForType & ";" & rvalue
    End Sub
    Private Function CheckEditable() As Boolean
        '//////////////////////////////
        '확인을 누를 수 있는지 확인
        If ForTypeCB.SelectedItem Is Nothing Then
            Return False
        End If

        Dim ForType As String = CType(ForTypeCB.SelectedItem, ComboBoxItem).Tag
        Select Case ForType
            Case "UserEdit"
                If useredittextbox.Text.Trim = "" Or (Not forrbtn.IsChecked And Not foreachrbtn.IsChecked) Then
                    Return False
                End If
            Case "CountRepeat"
                If count_vname.Text.Trim = "" Or Not IsNumeric(count_vinit.Text) Or Not IsNumeric(count_vcount.Text) Then
                    Return False
                End If
            Case "EUDLoopNewUnit", "EUDLoopUnit", "EUDLoopUnit2", "EUDLoopSprite"
                If ptr_vname.Text.Trim = "" Or epd_vname.Text.Trim = "" Then
                    Return False
                End If
                '원래 아무것도 없음
            Case "Timeline"
                If Not IsNumeric(timetextbox.Text) Or Timeline_vname.Text.Trim = "" Then
                    Return False
                End If
            Case "EUDLoopPlayerUnit"
                If playerselect.SelectedIndex = -1 Or PlayerUnit_v1.Text.Trim = "" Or PlayerUnit_v2.Text.Trim = "" Then
                    Return False
                End If
            Case "EUDLoopPlayer"
                If playerloopcb1.SelectedItem Is Nothing Or playerloopcb2.SelectedItem Is Nothing Or playerloopcb3.SelectedItem Is Nothing Or playerloop_vname.Text.Trim = "" Then
                    Return False
                End If
            Case "EUDPlayerLoop"
                '원래 아무것도 없음
        End Select


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

    Private Sub ForTypeCB_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If isLoad Then
            ResetControl("")
            btnRefresh()
        End If
    End Sub

    Private Sub ResetControl(val As String)
        countgrid.Visibility = Visibility.Collapsed
        usereditgrid.Visibility = Visibility.Collapsed
        tipLabel.Visibility = Visibility.Collapsed
        playerloop.Visibility = Visibility.Collapsed
        TimelinePanel.Visibility = Visibility.Collapsed
        PlayerUnitPanel.Visibility = Visibility.Collapsed
        doubleptr.Visibility = Visibility.Collapsed
        Dim ForType As String = CType(ForTypeCB.SelectedItem, ComboBoxItem).Tag

        Select Case ForType
            Case "UserEdit"
                usereditgrid.Visibility = Visibility.Visible

                Dim v() As String = val.Split("ᗢ")

                If v.Count = 2 Then
                    If v(0) = "For" Then
                        forrbtn.IsChecked = True
                    Else
                        foreachrbtn.IsChecked = True
                    End If

                    useredittextbox.Text = v(1)
                End If
            Case "CountRepeat"
                countgrid.Visibility = Visibility.Visible

                Dim v() As String = val.Split("ᗢ")

                If v.Count = 3 Then
                    count_vname.Text = v(0)
                    count_vinit.Text = v(1)
                    count_vcount.Text = v(2)
                Else
                    count_vname.Text = "i"
                    count_vinit.Text = 0
                    count_vcount.Text = 10
                End If
            Case "EUDLoopNewUnit"
                tipLabel.Visibility = Visibility.Visible
                doubleptr.Visibility = Visibility.Visible
                tipLabel.Content = "새로 생성된 유닛을 루프합니다."
                Dim ptrepd() As String = val.Split(",")

                If ptrepd.Count = 2 Then
                    ptr_vname.Text = ptrepd(0)
                    epd_vname.Text = ptrepd(1)
                Else
                    ptr_vname.Text = "ptr"
                    epd_vname.Text = "epd"
                End If
            Case "EUDLoopUnit"
                tipLabel.Visibility = Visibility.Visible
                doubleptr.Visibility = Visibility.Visible
                tipLabel.Content = "모든 유닛을 연결리스트를 따라 루프합니다."
                Dim ptrepd() As String = val.Split(",")

                If ptrepd.Count = 2 Then
                    ptr_vname.Text = ptrepd(0)
                    epd_vname.Text = ptrepd(1)
                Else
                    ptr_vname.Text = "ptr"
                    epd_vname.Text = "epd"
                End If
            Case "EUDLoopUnit2"
                tipLabel.Visibility = Visibility.Visible
                doubleptr.Visibility = Visibility.Visible
                tipLabel.Content = "모든 유닛을 루프합니다."
                Dim ptrepd() As String = val.Split(",")

                If ptrepd.Count = 2 Then
                    ptr_vname.Text = ptrepd(0)
                    epd_vname.Text = ptrepd(1)
                Else
                    ptr_vname.Text = "ptr"
                    epd_vname.Text = "epd"
                End If
            Case "EUDLoopSprite"
                tipLabel.Visibility = Visibility.Visible
                doubleptr.Visibility = Visibility.Visible
                tipLabel.Content = "모든 스프라이트를 루프합니다."
                Dim ptrepd() As String = val.Split(",")

                If ptrepd.Count = 2 Then
                    ptr_vname.Text = ptrepd(0)
                    epd_vname.Text = ptrepd(1)
                Else
                    ptr_vname.Text = "ptr"
                    epd_vname.Text = "epd"
                End If
            Case "Timeline"
                TimelinePanel.Visibility = Visibility.Visible
                Dim v() As String = val.Split("ᗢ")
                If v.Count = 2 Then
                    Timeline_vname.Text = v(0)

                    If IsNumeric(v(1)) Then
                        timetextbox.Text = v(1)
                    End If
                Else
                    Timeline_vname.Text = "t"
                    timetextbox.Text = 200
                End If
            Case "EUDLoopPlayerUnit"
                PlayerUnitPanel.Visibility = Visibility.Visible
                Dim v() As String = val.Split("ᗢ")
                If v.Count = 2 Then
                    Dim ptrepd() As String = v(0).Split(",")

                    PlayerUnit_v1.Text = ptrepd(0).Trim
                    PlayerUnit_v2.Text = ptrepd(1).Trim

                    If IsNumeric(v(1)) Then
                        playerselect.SelectedIndex = v(1)
                    End If
                Else
                    PlayerUnit_v1.Text = "ptr"
                    PlayerUnit_v2.Text = "epd"

                End If
            Case "EUDLoopPlayer"
                playerloop.Visibility = Visibility.Visible
                Dim v() As String = val.Split("ᗢ")

                If v.Count = 5 Then
                    playerloop_vname.Text = v(0)

                    For i = 0 To playerloopcb1.Items.Count - 1
                        If CType(playerloopcb1.Items(i), ComboBoxItem).Tag = v(1) Then
                            playerloopcb1.SelectedIndex = i
                            Exit For
                        End If
                    Next
                    For i = 0 To playerloopcb2.Items.Count - 1
                        If CType(playerloopcb2.Items(i), ComboBoxItem).Tag = v(2) Then
                            playerloopcb2.SelectedIndex = i
                            Exit For
                        End If
                    Next
                    For i = 0 To playerloopcb3.Items.Count - 1
                        If CType(playerloopcb3.Items(i), ComboBoxItem).Tag = v(3) Then
                            playerloopcb3.SelectedIndex = i
                            Exit For
                        End If
                    Next
                    playerloopcheckbox.IsChecked = v(4)
                Else
                    playerloop_vname.Text = "p"
                    playerloopcb1.SelectedIndex = 0
                    playerloopcb2.SelectedIndex = 0
                    playerloopcb3.SelectedIndex = 0
                End If
            Case "EUDPlayerLoop"
                tipLabel.Visibility = Visibility.Visible
                tipLabel.Content = "모든 플레이어를 순환합니다."
        End Select
    End Sub
    'Human
    'Computer
    'Rescuable
    'Neutral

    'Zerg
    'Terran
    'Protoss

    'Force1
    'Force2
    'Force3
    'Force4

    Private Sub Refresh_TextChanged(sender As Object, e As TextChangedEventArgs)
        If isLoad Then
            btnRefresh()
        End If
    End Sub

    Private Sub combobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If isLoad Then
            btnRefresh()
        End If
    End Sub
End Class
