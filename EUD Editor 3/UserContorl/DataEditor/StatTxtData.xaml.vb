Public Class StatTxtData
    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.stattxt

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        NameBar.Init(ObjectID, DatFiles, 0)

        MainTextBox.DataContext = pjData.BindingManager.StatTxtBinding(ObjectID)

        AddPalett(&H1, "기본 색 사용")
        AddPalett(&H2, "Pale Blue")
        AddPalett(&H3, "Yellow")
        AddPalett(&H4, "White")
        AddPalett(&H5, "Grey")
        AddPalett(&H6, "Red")
        AddPalett(&H7, "Green")
        AddPalett(&H8, "Red (P1)")
        AddPalett(&HB, "<1>투명")
        AddPalett(&HC, "<1>Remove beyond")
        AddPalett(&HE, "Blue (P2)")
        AddPalett(&HF, "Teal (P3)")
        AddPalett(&H10, "Purple (P4)")
        AddPalett(&H11, "Orange (P5)")
        AddPalett(&H12, "Right Aling")
        AddPalett(&H13, "Center Align")
        AddPalett(&H14, "<1>투명")
        AddPalett(&H15, "Brown (P6)")
        AddPalett(&H16, "white (P7)")
        AddPalett(&H17, "Yellow (P8)")
        AddPalett(&H18, "Green (P9)")
        AddPalett(&H19, "Brighter Yellow (P10)")
        AddPalett(&H1A, "Cyan")
        AddPalett(&H1B, "Pinkish (P11)")
        AddPalett(&H1C, "Dark Cyan (P12)")
        AddPalett(&H1D, "Greygreen")
        AddPalett(&H1E, "BlueGrey")
        AddPalett(&H1F, "Truquoise")
    End Sub
    Private Sub AddPalett(ColorCode As Integer, tip As String)

        Dim ColoreTextbox As New ColoredTextBlock("<" & Hex(ColorCode) & "> 〈" & Hex(ColorCode).PadLeft(2, "0") & "〉" & tip)
        ColoreTextbox.HorizontalAlignment = HorizontalAlignment.Left
        ColoreTextbox.Margin = New Thickness(-8)


        Dim ListboxItems As New ListBoxItem
        ListboxItems.Tag = ColorCode
        ListboxItems.Background = Brushes.Black
        ListboxItems.Content = ColoreTextbox
        ListboxItems.Height = 16

        'Dim btn As New Button
        'btn.Content = ColoreTextbox
        'btn.Style = Application.Current.Resources("MaterialDesignFlatButton")
        'btn.Background = Brushes.Black
        'btn.Margin = New Thickness(0, -4, 0, -4)

        'ColorBtn.Children.Add(btn)
        ColorInfor.Items.Add(ListboxItems)
    End Sub
    Private Sub ColorInfor_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If ColorInfor.SelectedItem IsNot Nothing Then
            Dim ColorCode As Integer = ColorInfor.SelectedItem.Tag

            Dim InsertString As String = "<" & Hex(ColorCode) & ">"

            MainTextBox.SelectedText = InsertString
            MainTextBox.SelectionStart += InsertString.Length
            MainTextBox.SelectionLength = 0
            MainTextBox.Focus()
            ColorInfor.SelectedIndex = -1
        End If

    End Sub


    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, tObjectID As Integer)
        ObjectID = tObjectID

        NameBar.ReLoad(ObjectID, DatFiles, 0)


        MainTextBox.DataContext = pjData.BindingManager.StatTxtBinding(ObjectID)
    End Sub

    Private LastStr As String
    Private PressBackSpace As Boolean = False
    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        LimitShortCut(MainTextBox.Text)

        If PressBackSpace Then
            PressBackSpace = False

            If LastStr = ">" Then
                Dim tLastStr As String = ""


                If MainTextBox.SelectionStart >= 3 Then
                    MainTextBox.SelectionStart -= 3
                    MainTextBox.SelectionLength = 3

                    tLastStr = MainTextBox.SelectedText & LastStr

                    MainTextBox.SelectionStart += 3
                    MainTextBox.SelectionLength = 0
                Else
                    MainTextBox.SelectionLength = MainTextBox.SelectionStart
                    MainTextBox.SelectionStart = 0

                    tLastStr = MainTextBox.SelectedText & LastStr

                    MainTextBox.SelectionStart = MainTextBox.Text.Length
                    MainTextBox.SelectionLength = 0
                End If






                Dim rgx As New Text.RegularExpressions.Regex("<([A-Za-z0-9])+>", Text.RegularExpressions.RegexOptions.IgnoreCase)

                If rgx.Match(tLastStr).Success Then
                    Dim ValueStr As String = rgx.Match(tLastStr).Value
                    ValueStr = Mid(rgx.Match(tLastStr).Value, 2, ValueStr.Length - 2)
                    Dim Value As Integer
                    Try
                        Value = "&H" & ValueStr

                        MainTextBox.SelectionStart -= ValueStr.Length + 1
                        MainTextBox.SelectionLength = ValueStr.Length + 1

                        MainTextBox.SelectedText = ""
                    Catch ex As Exception

                    End Try
                End If
            End If
            'MsgBox(e.Changes.First.RemovedLength)
        End If
        Dim ColorText As String = MainTextBox.Text

        If LimitCombobx.SelectedIndex <> 0 And ShortCombobox.SelectedIndex <> 0 Then
            If ShortCombobox.SelectedIndex = 27 Then
                Dim rgx As New Text.RegularExpressions.Regex("<([A-Za-z0-9])+>", Text.RegularExpressions.RegexOptions.IgnoreCase)
                ColorText = Mid(ColorText, rgx.Matches(ColorText)(1).Length + rgx.Matches(ColorText)(1).Index + 1)
            Else
                Dim rgx As New Text.RegularExpressions.Regex("<([A-Za-z0-9])+>", Text.RegularExpressions.RegexOptions.IgnoreCase)
                ColorText = Mid(ColorText, rgx.Match(ColorText).Length + rgx.Match(ColorText).Index + 1)
            End If
        End If



        PreviewText.TextColred(ColorText)
    End Sub

    Private Sub MainTextBox_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Back Then
            If MainTextBox.SelectionStart > 0 Then
                MainTextBox.SelectionStart -= 1
                MainTextBox.SelectionLength = 1

                LastStr = MainTextBox.SelectedText

                MainTextBox.SelectionStart += 1
                MainTextBox.SelectionLength = 0
            Else
                LastStr = ""
            End If



            PressBackSpace = True
            'MsgBox(LastStr & " " & MainTextBox.Text)
        End If
    End Sub

    Private Sub MainTextBox_PreviewKeyUp(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Back Then
            PressBackSpace = False
            'MsgBox(LastStr & " " & MainTextBox.Text)
        End If
    End Sub

    Private Sub LimitShortCut(texts As String)
        DonotChangeCombobox = False

        '첫번째 글자가 ESC인지 확인
        '두번째 글자가 코드인지 확인

        'regex를 통해 첫번째로 나온 글자의 위치 파악. 만약 0일경우 ESC일 수 있으니 두번쨰꺼도 확인.

        '두번째 글자가 코드일 경우 0~5인지 확인,
        '0~5이면 첫번째 글자가 일치
        Dim rgx As New Text.RegularExpressions.Regex("<([A-Za-z0-9])+>", Text.RegularExpressions.RegexOptions.IgnoreCase)

        Dim Matchs As Text.RegularExpressions.Match = rgx.Match(texts)
        If Matchs.Success Then
            Dim LimitIndex As Integer = -1
            Dim ShortCutKey As Integer = -1

            If Matchs.Index = 0 And rgx.Matches(texts).Count > 1 Then 'ESC일 수도 있음

                Dim FristValuestr As String = Mid(Matchs.Value, 2, Matchs.Value.Length - 2)
                '첫번째 수가 타당한 수인지.


                Try
                    Dim intValue As Integer = "&H" & FristValuestr

                    If intValue <> 27 Then
                        LimitCombobx.SelectedIndex = 0
                        ShortCombobox.SelectedIndex = 0
                        DonotChangeCombobox = True
                        Return
                    End If
                Catch ex As Exception
                    DonotChangeCombobox = True
                    Return
                End Try


                Dim Startindex As Integer = rgx.Matches(texts)(1).Index
                If Startindex <> FristValuestr.Length + 2 Then
                    LimitCombobx.SelectedIndex = 0
                    ShortCombobox.SelectedIndex = 0
                    DonotChangeCombobox = True
                    Return
                End If

                Dim Valuestr As Integer = Mid(rgx.Matches(texts)(1).Value, 2, rgx.Matches(texts)(1).Value.Length - 2)
                Try
                    LimitIndex = "&H" & Valuestr
                Catch ex As Exception
                    LimitCombobx.SelectedIndex = 0
                    ShortCombobox.SelectedIndex = 0
                    DonotChangeCombobox = True
                    Return
                End Try


                ShortCutKey = 26
            ElseIf Matchs.Index = 1 Then
                Dim Valuestr As String = Mid(Matchs.Value, 2, Matchs.Value.Length - 2)
                Try
                    LimitIndex = "&H" & Valuestr
                Catch ex As Exception
                    LimitCombobx.SelectedIndex = 0
                    ShortCombobox.SelectedIndex = 0
                    DonotChangeCombobox = True
                    Return
                End Try
                Dim Shortstr As String = texts.First
                '97~122

                ShortCutKey = AscW(Shortstr.ToLower) - 97
                If ShortCutKey < 0 Or ShortCutKey > 25 Then
                    LimitCombobx.SelectedIndex = 0
                    ShortCombobox.SelectedIndex = 0
                    DonotChangeCombobox = True
                    Return
                End If
            End If
            If LimitIndex <> -1 And ShortCutKey <> -1 Then
                LimitCombobx.SelectedIndex = LimitIndex + 1
                ShortCombobox.SelectedIndex = ShortCutKey + 1
            Else
                LimitCombobx.SelectedIndex = 0
                ShortCombobox.SelectedIndex = 0
            End If

        Else
            LimitCombobx.SelectedIndex = 0
            ShortCombobox.SelectedIndex = 0
            DonotChangeCombobox = True
            Return
        End If


        DonotChangeCombobox = True
    End Sub

    'pjData.BindingManager.StatTxtBinding(ObjectID)

    Private DonotChangeCombobox As Boolean = True
    Private Sub LimitCombobx_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If DonotChangeCombobox Then
            Dim CurrentText As String = pjData.BindingManager.StatTxtBinding(ObjectID).Value

            DonotChangeCombobox = False
            If ShortCombobox.SelectedIndex = 0 Then '바뀌지 않은 상태를 의미
                ShortCombobox.SelectedIndex = 1

                CurrentText = "a<" & Hex(LimitCombobx.SelectedIndex - 1).ToUpper.PadLeft(2, "0") & ">" & CurrentText
                pjData.BindingManager.StatTxtBinding(ObjectID).Value = CurrentText

                DonotChangeCombobox = True
                Return
            End If
            If LimitCombobx.SelectedIndex = 0 Then '지우기
                If ShortCombobox.SelectedIndex = 27 Then 'ESC가 단축키일 경우
                    Dim rgx As New Text.RegularExpressions.Regex("<([A-Za-z0-9])+>", Text.RegularExpressions.RegexOptions.IgnoreCase)
                    CurrentText = Mid(CurrentText, rgx.Matches(CurrentText)(1).Length + rgx.Matches(CurrentText)(1).Index + 1)
                Else
                    Dim rgx As New Text.RegularExpressions.Regex("<([A-Za-z0-9])+>", Text.RegularExpressions.RegexOptions.IgnoreCase)
                    CurrentText = Mid(CurrentText, rgx.Match(CurrentText).Length + rgx.Match(CurrentText).Index + 1)
                End If

                ShortCombobox.SelectedIndex = 0

                pjData.BindingManager.StatTxtBinding(ObjectID).Value = CurrentText
                DonotChangeCombobox = True
                Return
            End If


            If ShortCombobox.SelectedIndex = 27 Then 'ESC가 단축키일 경우
                Dim rgx As New Text.RegularExpressions.Regex("<([A-Za-z0-9])+>", Text.RegularExpressions.RegexOptions.IgnoreCase)
                CurrentText = Replace(CurrentText, rgx.Matches(CurrentText)(1).Value, "<" & Hex(LimitCombobx.SelectedIndex - 1).ToUpper.PadLeft(2, "0") & ">", 1, 1)
            Else
                Dim rgx As New Text.RegularExpressions.Regex("<([A-Za-z0-9])+>", Text.RegularExpressions.RegexOptions.IgnoreCase)
                CurrentText = Replace(CurrentText, rgx.Match(CurrentText).Value, "<" & Hex(LimitCombobx.SelectedIndex - 1).ToUpper.PadLeft(2, "0") & ">", 1, 1)
            End If

            pjData.BindingManager.StatTxtBinding(ObjectID).Value = CurrentText
            DonotChangeCombobox = True
        End If
    End Sub

    Private Sub ShortCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If DonotChangeCombobox Then
            Dim rgx As New Text.RegularExpressions.Regex("<([A-Za-z0-9])+>", Text.RegularExpressions.RegexOptions.IgnoreCase)
            Dim CurrentText As String = pjData.BindingManager.StatTxtBinding(ObjectID).Value

            DonotChangeCombobox = False
            If LimitCombobx.SelectedIndex = 0 Then '바뀌지 않은 상태를 의미
                LimitCombobx.SelectedIndex = 1

                CurrentText = ShortCombobox.SelectedItem.Tag & "<00>" & CurrentText
                pjData.BindingManager.StatTxtBinding(ObjectID).Value = CurrentText

                DonotChangeCombobox = True
                Return
            End If
            If ShortCombobox.SelectedIndex = 0 Then '지우기
                If rgx.Matches(CurrentText).Count = 1 Then
                    CurrentText = Mid(CurrentText, rgx.Match(CurrentText).Length + rgx.Match(CurrentText).Index + 1)
                Else
                    If rgx.Matches(CurrentText)(0).Index = 0 Then
                        CurrentText = Mid(CurrentText, rgx.Matches(CurrentText)(1).Length + rgx.Matches(CurrentText)(1).Index + 1)
                    Else
                        CurrentText = Mid(CurrentText, rgx.Matches(CurrentText)(0).Length + rgx.Matches(CurrentText)(0).Index + 1)
                    End If
                End If

                pjData.BindingManager.StatTxtBinding(ObjectID).Value = CurrentText

                LimitCombobx.SelectedIndex = 0
                DonotChangeCombobox = True
                Return
            End If
            If rgx.Matches(CurrentText).Count = 1 Then
                CurrentText = CurrentText.Remove(0, 1)
                CurrentText = ShortCombobox.SelectedItem.Tag & CurrentText
            Else
                If rgx.Matches(CurrentText)(0).Index = 0 Then 'ESC가 단축키일 경우
                    CurrentText = Replace(CurrentText, rgx.Matches(CurrentText)(0).Value, ShortCombobox.SelectedItem.Tag, 1, 1)
                Else
                    CurrentText = CurrentText.Remove(0, 1)
                    CurrentText = ShortCombobox.SelectedItem.Tag & CurrentText
                End If
            End If




            pjData.BindingManager.StatTxtBinding(ObjectID).Value = CurrentText

            DonotChangeCombobox = True
        End If
    End Sub
End Class
