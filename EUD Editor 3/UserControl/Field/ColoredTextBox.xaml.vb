Imports System.Globalization
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Web.UI.WebControls
Imports MaterialDesignColors

Public Class ColoredTextBox
    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        KorFont = New FontFamily(New Uri("pack://application:,,,/resources/"), "./#Kostar")
    End Sub
    Private KorFont As FontFamily
    Public Sub New(text As String)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        TextColred(text)
        KorFont = New FontFamily(New Uri("pack://application:,,,/resources/"), "./#Kostar")
    End Sub




    Public Sub TextColred(text As String)
        Texts.FontFamily = KorFont
        Texts.FontSize = 12.0


        Dim FirstChar As String = StringTool.GetCharAt(0, text)
        Dim SecondChar As String = StringTool.GetCharAt(1, text)

        Dim Flag As Boolean = False

        If IsNumeric("&H" & SecondChar) Then
            If "&H" & SecondChar <= 5 Then
                Flag = True
            End If
        End If

        If Flag Then
            text = StringTool.ChangeCharAt(1, text, "")
            text = StringTool.ChangeCharAt(0, text, "")
        End If


        Dim TextDocument As FlowDocument = Texts.Document
        TextDocument.Blocks.Clear()

        Dim tParagraph As New Paragraph
        TextDocument.Blocks.Add(tParagraph)

        Dim inlines As InlineCollection = tParagraph.Inlines

        Dim MainText As String = text '.Replace(vbCrLf, "<0A>")


        Dim rgx As New Regex("<[^>\\]*(?:\\.[^>\\]*)*>", RegexOptions.IgnoreCase)


        Dim matchcollection As MatchCollection = rgx.Matches(MainText)


        Dim matchlist As Dictionary(Of String, String) = New Dictionary(Of String, String)
        Dim matchcolorlist As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)


        For Each item As Match In matchcollection
            Dim Value As String = item.Value
            Value = Mid(Value, 2, Value.Length - 2)



            If Value.Length > 2 Then
                '색 코드 아님
                Dim index As Integer = scData.GetASCIIIndex(Value)
                Dim changedString As String = ""

                If index <> -1 Then
                    changedString = Chr(index)
                Else
                    Continue For
                End If

                If Not matchlist.ContainsKey(Value) Then
                    matchlist.Add(Value, changedString)
                End If
            Else
                '색 코드 일수있음
                Try
                    Dim ColorCode As Integer
                    ColorCode = "&H" & Value
                    If ColorCode > ColorTable.Count - 1 Then
                        '색 코드가 아님
                        If Not matchlist.ContainsKey(Value) Then
                            matchlist.Add(Value, Chr(ColorCode))
                        End If
                        Continue For
                    Else
                        '색코드임
                        If ColorTable(ColorCode) = Nothing Then
                            If Not matchlist.ContainsKey(Value) Then
                                matchlist.Add(Value, Chr(ColorCode))
                            End If
                        Else
                            If Not matchcolorlist.ContainsKey(Value) Then
                                matchcolorlist.Add(Value, ColorCode)
                            End If
                        End If
                    End If
                Catch ex As Exception

                    Continue For
                End Try
            End If
        Next

        For Each item In matchlist
            MainText = MainText.Replace("<" + item.Key + ">", item.Value)
        Next



        For Each item In matchcolorlist
            MainText = MainText.Replace("<" + item.Key + ">", "ᚘꁠ" + item.Value.ToString().PadLeft(2))
        Next


        Dim tlist As List(Of String) = MainText.Split("ᚘ").ToList()


        For Each strtext In tlist
            If strtext.Length = 0 Then
                Continue For
            End If

            Dim inputText As String = strtext
            Dim colorcode As Integer = 1

            If strtext(0) = "ꁠ" Then
                '색코드가 입력되어 있을 경우

                colorcode = strtext.Substring(1, 2)
                inputText = strtext.Substring(3)
            Else
                colorcode = 1
            End If


            Dim Run As New Run(inputText)
            Run.Foreground = New SolidColorBrush(ColorTable(colorcode))
            inlines.Add(Run)
        Next





        'Dim LastColor As Integer = 1
        'Dim LastCode As Integer = 1
        'Dim Startindex As Integer = 1
        'Dim MatchIndex As Integer = 0
        'For i = 0 To rgx.Matches(MainText).Count - 1
        '    Dim tMatch As Match = rgx.Matches(MainText).Item(i - MatchIndex)

        '    Dim Value As String = tMatch.Value
        '    Value = Mid(Value, 2, Value.Length - 2)

        '    Dim ColorCode As Integer = -1
        '    Try
        '        For keys = 0 To SCConst.ASCIICount
        '            If scData.ASCIICode(keys) = Value Then 'Virtual KEY일 경우
        '                If keys > ColorTable.Count - 1 Then
        '                    MainText = Replace(MainText, "<" & Value & ">", Chr(keys), 1, 1)
        '                    MatchIndex += 1
        '                Else
        '                    ColorCode = keys
        '                End If
        '                Exit For
        '            End If
        '        Next
        '        If ColorCode = -1 Then
        '            ColorCode = "&H" & Value
        '        End If

        '        If ColorCode > ColorTable.Count - 1 Then
        '            MainText = Replace(MainText, "<" & Value & ">", Chr(ColorCode), 1, 1)
        '            MatchIndex += 1
        '            Continue For
        '        End If
        '    Catch ex As Exception
        '        Continue For
        '    End Try


        '    '스페이스 하나당 4픽셀

        '    Dim AddedText As String = Mid(MainText, Startindex, tMatch.Index - Startindex + 1)
        '    AddedText = StringTool.ChangeSlash(AddedText)




        '    Dim Run As New Run(AddedText)
        '    Run.Foreground = New SolidColorBrush(ColorTable(LastColor))
        '    inlines.Add(Run)
        '    Startindex = tMatch.Index + Value.Length + 3
        '    LastCode = ColorCode
        '    If ColorCode <> -1 And ColorTable(ColorCode) <> Nothing Then
        '        LastColor = ColorCode
        '    End If
        '    If LastCode = &H12 Then
        '        tParagraph.TextAlignment = TextAlignment.Right
        '    End If
        '    If LastCode = &H13 Then
        '        tParagraph.TextAlignment = TextAlignment.Center
        '    End If
        '    If LastCode = &HA Then

        '        Dim tRun As New Run(vbCrLf)
        '        inlines.Add(tRun)

        '        'tParagraph = New Paragraph
        '        'TextDocument.Blocks.Add(tParagraph)
        '        'inlines = tParagraph.Inlines
        '        LastColor = 1
        '    End If
        'Next

        'If True Then
        '    Dim AddedText As String = Mid(MainText, Startindex, MainText.Length - Startindex + 1)
        '    AddedText = StringTool.ChangeSlash(AddedText)
        '    Dim Run As New Run(AddedText)
        '    Run.Foreground = New SolidColorBrush(ColorTable(LastColor))
        '    inlines.Add(Run)
        'End If


    End Sub

    'Private Function GetTextLen(str As String) As Integer
    '    Dim FormattedText As New FormattedText(str, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
    '                      New Typeface(Texts.FontFamily, Texts.FontStyle, Texts.FontWeight, Texts.FontStretch),
    '                       Texts.FontSize, Brushes.Black, New NumberSubstitution(), TextFormattingMode.Display)
    '    Return FormattedText.Width
    'End Function

End Class
