Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class ColoredTextBlock
    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
    End Sub
    Public Sub New(text As String)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        TextColred(text)
    End Sub




    Public Sub TextColred(text As String)
        Dim inlines As InlineCollection = Texts.Inlines
        inlines.Clear()
        Dim MainText As String = text.Replace(vbCrLf, "<A>")


        Dim rgx As New Regex("<([A-Za-z0-9])+>", RegexOptions.IgnoreCase)

        Dim LastColor As Integer = 1
        Dim LastCode As Integer = 1
        Dim Startindex As Integer = 1
        For i = 0 To rgx.Matches(MainText).Count - 1
            Dim tMatch As Match = rgx.Matches(MainText).Item(i)

            Dim Value As String = tMatch.Value
            Value = Mid(Value, 2, Value.Length - 2)

            Dim ColorCode As Integer = -1
            Try
                ColorCode = "&H" & Value
                If ColorCode > ColorTable.Count - 1 Then
                    Continue For
                End If
            Catch ex As Exception
                Continue For
            End Try


            '스페이스 하나당 4픽셀

            Dim AddedText As String = Mid(MainText, Startindex, tMatch.Index - Startindex + 1)
            'MsgBox(AddedText & vbCrLf & "   매치 인덱스:" & tMatch.Index & "  시작인덱스:" & Startindex)

            '남은 공간


            If LastCode = &H12 Or LastCode = &H13 Then '12 라이트   13 중앙
                Dim TextBoxWidth As Integer = Texts.ActualWidth
                Dim TextWidth As Integer
                Dim AddexWidth As Integer = GetTextLen(AddedText)

                If MainText.IndexOf(vbCrLf) = -1 Then '개행이 없을 경우
                    TextWidth = GetTextLen(MainText)
                Else

                End If

                MsgBox("TextBoxWidth:" & TextBoxWidth & "    TextWidth:" & TextWidth & "    AddexWidth:" & AddexWidth)
            End If


            'Return New Size(FormattedText.Width, FormattedText.Height);




            Dim Run As New Run(AddedText)
            Run.Foreground = New SolidColorBrush(ColorTable(LastColor))
            inlines.Add(Run)
            Startindex = tMatch.Index + Value.Length + 3
            LastCode = ColorCode
            If ColorCode <> -1 And ColorTable(ColorCode) <> Nothing Then
                LastColor = ColorCode
            End If
            If LastCode = &HA Then
                inlines.Add(vbCrLf)
                LastColor = 1
            End If
        Next

        If True Then
            Dim AddedText As String = Mid(MainText, Startindex, MainText.Length - Startindex + 1)
            Dim Run As New Run(AddedText)
            Run.Foreground = New SolidColorBrush(ColorTable(LastColor))
            inlines.Add(Run)
        End If


    End Sub

    Private Function GetTextLen(str As String) As Integer
        Dim FormattedText As New FormattedText(str, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                          New Typeface(Texts.FontFamily, Texts.FontStyle, Texts.FontWeight, Texts.FontStretch),
                           Texts.FontSize, Brushes.Black, New NumberSubstitution(), TextFormattingMode.Display)
        Return FormattedText.Width
    End Function

End Class
