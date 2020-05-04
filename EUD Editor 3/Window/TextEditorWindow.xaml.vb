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
        '전처리기
        Dim pattern As String = "\\x([\d\w][\d\w])"
        Dim matches As MatchCollection = Regex.Matches(inittext, pattern)
        For Each Match As Match In matches
            Dim v As String = Match.Value
            Dim hexv As String = "&H" & Mid(v, 3)
            Dim num As Integer
            Try
                num = hexv
            Catch ex As Exception
                Continue For
            End Try

            inittext = inittext.Replace(v, Chr(num))
        Next


        NoneChange = True
        MainTextBox.AppendText(inittext)
        MainTextBox.FontFamily = KorFont
        MainTextBox.FontSize = 24.0

        MainTextBox.Focus()

        TextBoxRender()
        NoneChange = False
    End Sub

    Private Sub Window_Unloaded(sender As Object, e As RoutedEventArgs)
        NoneChange = True
        MainTextBox.UpdateLayout()
        ChangeText()
    End Sub

    Private NoneChange As Boolean = False
    Private Sub MainTextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not NoneChange Then
            ChangeText()
        End If
    End Sub
    Private Sub ChangeText()
        Dim tr As New TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd)
        tr.ApplyPropertyValue(FontFamilyProperty, KorFont)
        tr.ApplyPropertyValue(FontSizeProperty, 24.0)

        TextString = tr.Text
        TextString = TextString.Replace(vbCrLf, "\n")
        If TextString.Length <> 0 Then
            TextString = Mid(TextString, 1, TextString.Length - 2)

            Dim pattern As String = "[\x01-\x1F]"
            Dim matches As MatchCollection = Regex.Matches(TextString, pattern)
            For Each Match As Match In matches
                Dim v As String = Match.Value
                Dim colorCode As Integer = Asc(v)

                Dim ccode As String = "\x" & Hex(colorCode).PadLeft(2, "0")
                TextString = TextString.Replace(v, ccode)
            Next

        End If
        NoneChange = True
        TextBoxRender()
        NoneChange = False
    End Sub

    Private Sub TextBoxRender()
        Dim _tr As New TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd)
        _tr.ApplyPropertyValue(ForegroundProperty, New SolidColorBrush(ColorTable(1)))
        _tr.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left)



        Dim pattern As String = "[\x01-\x1F]"
        Dim rg As New Regex(pattern)

        Dim bc As BlockCollection = MainTextBox.Document.Blocks
        For i = 0 To bc.Count - 1
            Dim tr As New TextRange(bc(i).ContentStart, bc(i).ContentEnd)
            Dim matches As MatchCollection = rg.Matches(tr.Text)
            Dim pointer As TextPointer = bc(i).ContentStart


            For k = 0 To matches.Count - 1
                Dim colorCode As Integer = Asc(matches(k).Value)
                Dim startIndex As Integer = matches(k).Index + 1 + k * 2
                Dim start As TextPointer = pointer.GetPositionAtOffset(startIndex)

                'MsgBox("찾은 값 : " & Match.Value & "   찾은 시작 값 : " & startIndex)


                Dim ttr As New TextRange(start, bc(i).ContentEnd)

                Select Case colorCode
                    Case &H12 '오른쪽
                        ttr.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right)
                    Case &H13 '가운대
                        ttr.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center)
                    Case &H9 '탭
                    Case Else
                        ttr.ApplyPropertyValue(ForegroundProperty, New SolidColorBrush(ColorTable(colorCode)))
                End Select
            Next

        Next



        'Dim pointer As TextPointer = MainTextBox.Document.ContentStart

        'While (pointer IsNot Nothing)
        '    If (pointer.GetPointerContext(LogicalDirection.Forward) = TextPointerContext.Text) Then
        '        Dim textRun As String = pointer.GetTextInRun(LogicalDirection.Forward)
        '        Dim matches As MatchCollection = rg.Matches(textRun)
        '        For Each Match As Match In matches
        '            Dim colorCode As Integer = Asc(Match.Groups(1).Value)

        '            Dim startIndex As Integer = Match.Groups(2).Index
        '            Dim length As Integer = Match.Groups(2).Length
        '            Dim start As TextPointer = pointer.GetPositionAtOffset(startIndex)
        '            Dim tend As TextPointer = start.GetPositionAtOffset(length)

        '            Dim ttr As New TextRange(start, tend)

        '            Select Case colorCode
        '                Case &H12 '오른쪽
        '                    ttr.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right)
        '                Case &H13 '가운대
        '                    ttr.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center)
        '                Case &H9 '탭
        '                Case Else
        '                    ttr.ApplyPropertyValue(ForegroundProperty, New SolidColorBrush(ColorTable(colorCode)))
        '            End Select



        '        Next


        '    End If

        '    pointer = pointer.GetNextContextPosition(LogicalDirection.Forward)
        'End While
    End Sub





    Private Sub ColorInfor_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If ColorInfor.SelectedItem IsNot Nothing Then
            Dim ColorCode As Integer = ColorInfor.SelectedItem.Tag

            Dim InsertString As String = Chr(ColorCode)
            MainTextBox.Selection.Text = InsertString
            Dim tptr As TextPointer = MainTextBox.Selection.End
            MainTextBox.Selection.Select(tptr, tptr)


            ColorInfor.SelectedIndex = -1
            MainTextBox.Focus()
        End If

    End Sub
    Private Sub AddPalett(ColorCode As Integer, tip As String)

        Dim ColoreTextbox As New ColoredTextBlock("<" & Hex(ColorCode) & ">  " & tip)
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

    Private Sub FormatFunc_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If FormatFunc.SelectedItem IsNot Nothing Then
            Dim FormatCode As String = FormatFunc.SelectedItem.Tag

            Dim InsertString As String = "{" & FormatCode & "}"
            MainTextBox.Selection.Text = InsertString
            Dim tptr As TextPointer = MainTextBox.Selection.End
            MainTextBox.Selection.Select(tptr, tptr)


            FormatFunc.SelectedIndex = -1
            MainTextBox.Focus()
        End If

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

End Class
