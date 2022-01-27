Imports System.Text.RegularExpressions

Public Class TextEditorWindow
    Public TextString As String

    Private KorFont As FontFamily

    Public Sub New(InitStr As String)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        KorFont = New FontFamily(New Uri("pack://application:,,,/resources/"), "./#Kostar")


        Dim str As String() = Tool.GetText("ColorNames").Split("|")
        For i = 0 To str.Count - 1
            If str(i) <> "NULL" Then
                AddPalett(i + 1, str(i))
            End If
        Next


        TextString = InitStr
        Dim inittext As String = TextString
        inittext = inittext.Replace("\n", vbCrLf)
        ''전처리기
        'Dim pattern As String = "\\x([\d\w][\d\w])"
        'Dim matches As MatchCollection = Regex.Matches(inittext, pattern)
        'For Each Match As Match In matches
        '    Dim v As String = Match.Value
        '    Dim hexv As String = "&H" & Mid(v, 3)
        '    Dim num As Integer
        '    Try
        '        num = hexv
        '    Catch ex As Exception
        '        Continue For
        '    End Try

        '    inittext = inittext.Replace(v, Chr(num))
        'Next

        EditTextbox.Text = inittext

        EditTextbox.Focus()
        'MainTextBox.AppendText(inittext)
        'MainTextBox.FontFamily = KorFont
        'MainTextBox.FontSize = 24.0

        TextBoxRender()
    End Sub

    Private Function NewTextBlock() As TextBlock
        Dim tbox As New TextBlock
        tbox.Inlines.Clear()
        tbox.FontFamily = KorFont
        tbox.FontSize = 24.0

        Return tbox
    End Function
    Private Sub TextBoxRender()
        RenderTextBox.BeginInit()
        RenderTextBox.Children.Clear()
        Dim tstr As String = TextString.Replace("\n", vbCrLf)
        Dim Strs() As String = tstr.Split(vbCrLf)

        Dim pattern As String = "\\x([\d\w][\d\w])"
        Dim rg As New Regex(pattern)
        For i = 0 To Strs.Count - 1
            Dim defaultBrush As New SolidColorBrush(ColorTable(1))
            Dim LineStr As String = Strs(i).Trim

            Dim matches As MatchCollection = rg.Matches(LineStr)


            Dim LeftTextBox As TextBlock = NewTextBlock()
            Dim CenterTextBox As TextBlock = NewTextBlock()

            CenterTextBox.HorizontalAlignment = HorizontalAlignment.Center
            Dim RightTextBox As TextBlock = NewTextBlock()
            DockPanel.SetDock(RightTextBox, Dock.Right)

            Dim Inlines As InlineCollection = LeftTextBox.Inlines

            Dim lastindex As Integer = 0
            For k = 0 To matches.Count - 1
                Dim v As String = matches(k).Groups(1).Value
                Dim colorcode As Integer
                Try
                    colorcode = "&H" & v
                Catch ex As Exception

                End Try

                Dim startindex As Integer = matches(k).Index

                Dim ctext As String = Mid(LineStr, lastindex + 1, startindex - lastindex)

                Dim tRun As New Run(ctext)
                tRun.Foreground = defaultBrush
                Inlines.Add(tRun)


                lastindex = startindex + matches(k).Length

                Select Case colorcode
                    Case &H12 '오른쪽
                        Inlines = RightTextBox.Inlines
                    Case &H13 '가운대
                        Inlines = CenterTextBox.Inlines
                    Case Else
                        If ColorTable.Count < colorcode Then
                            Continue For
                        End If
                        defaultBrush = New SolidColorBrush(ColorTable(colorcode))
                End Select
            Next

            Dim Run As New Run(Mid(LineStr, lastindex + 1))
            Run.Foreground = defaultBrush
            Inlines.Add(Run)

            Dim tdock As New DockPanel
            tdock.Margin = New Thickness(0, 4, 0, 4)
            DockPanel.SetDock(tdock, Dock.Top)
            tdock.Children.Add(LeftTextBox)
            tdock.Children.Add(RightTextBox)
            tdock.Children.Add(CenterTextBox)


            RenderTextBox.Children.Add(tdock)
        Next

        'Dim _tr As New TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd)
        '_tr.ApplyPropertyValue(ForegroundProperty, New SolidColorBrush(ColorTable(1)))
        '_tr.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left)



        'Dim pattern As String = "[\x01-\x1F]"
        'Dim rg As New Regex(pattern)

        'Dim bc As BlockCollection = MainTextBox.Document.Blocks
        'For i = 0 To bc.Count - 1
        '    Dim tr As New TextRange(bc(i).ContentStart, bc(i).ContentEnd)
        '    Dim matches As MatchCollection = rg.Matches(tr.Text)
        '    Dim pointer As TextPointer = bc(i).ContentStart

        '    Dim stflag As Integer = 0
        '    For k = 0 To matches.Count - 1
        '        Dim colorCode As Integer = Asc(matches(k).Value)
        '        Dim startIndex As Integer = matches(k).Index + 1 + k * 2 + stflag
        '        If matches(k).Index = 0 Then
        '            stflag = -1
        '        End If

        '        Dim start As TextPointer = pointer.GetPositionAtOffset(startIndex)

        '        'MsgBox("찾은 값 : " & Match.Value & "   찾은 시작 값 : " & startIndex)


        '        Dim ttr As New TextRange(start, bc(i).ContentEnd)

        '        Select Case colorCode
        '            Case &H12 '오른쪽
        '                ttr.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right)
        '            Case &H13 '가운대
        '                ttr.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center)
        '            Case &H9 '탭
        '            Case Else
        '                ttr.ApplyPropertyValue(ForegroundProperty, New SolidColorBrush(ColorTable(colorCode)))
        '        End Select
        '    Next

        'Next


        RenderTextBox.EndInit()
    End Sub






    Private Sub ColorInfor_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If ColorInfor.SelectedItem IsNot Nothing Then
            Dim ColorCode As Integer = ColorInfor.SelectedItem.Tag

            Dim InsertString As String = "\x" & Hex(ColorCode).ToUpper.PadLeft(2, "0")
            EditTextbox.SelectedText = InsertString
            Dim len As Integer = EditTextbox.SelectionLength
            EditTextbox.SelectionLength = 0
            EditTextbox.SelectionStart += len


            ColorInfor.SelectedIndex = -1
            EditTextbox.Focus()
        End If

    End Sub

    Private Sub FormatFunc_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If FormatFunc.SelectedItem IsNot Nothing Then
            Dim FormatCode As String = FormatFunc.SelectedItem.Tag

            Dim InsertString As String = "{" & FormatCode & "}"
            EditTextbox.SelectedText = InsertString
            Dim len As Integer = EditTextbox.SelectionLength
            EditTextbox.SelectionLength = 0
            EditTextbox.SelectionStart += len

            FormatFunc.SelectedIndex = -1
            EditTextbox.Focus()
        End If

    End Sub

    Private Sub AddPalett(ColorCode As Integer, tip As String)

        Dim ColoreTextbox As New ColoredTextBlock("<" & Hex(ColorCode) & ">\x" & Hex(ColorCode).ToUpper.PadLeft(2, "0") & "   " & tip)
        ColoreTextbox.HorizontalAlignment = HorizontalAlignment.Left
        ColoreTextbox.Margin = New Thickness(-8)


        Dim ListboxItems As New ListBoxItem

        ListboxItems.Tag = ColorCode
        ListboxItems.Background = Brushes.Black
        ListboxItems.Content = ColoreTextbox
        ListboxItems.Height = 16
        ColorInfor.Items.Add(ListboxItems)
    End Sub
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Sub EditTextbox_TextChanged(sender As Object, e As TextChangedEventArgs)
        TextString = EditTextbox.Text.Replace(vbCrLf, "\n")
        TextBoxRender()
    End Sub

    Private Sub EditTextbox_KeyUp(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Escape Then
            Close()
        End If
    End Sub
End Class
