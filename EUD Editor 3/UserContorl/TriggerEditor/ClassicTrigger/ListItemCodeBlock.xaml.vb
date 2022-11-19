Public Class ListItemCodeBlock
    Public _tcode As TriggerCodeBlock
    Private scripter As ScriptEditor

    Public Sub New(_scripter As ScriptEditor, tcode As TriggerCodeBlock)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        _tcode = tcode
        scripter = _scripter
        RefreshItem()
    End Sub


    Public Sub RefreshItem()
        Wrap.Children.Clear()



        '함수를 재정렬하여 쓴다.
        Dim t As TriggerFunction = _tcode.GetCodeFunction(scripter)
        Dim isCmpTrigger As Boolean
        Dim IsEmpty As Boolean = False

        If t Is Nothing Then
            Dim ttb As New Label
            ttb.Foreground = tmanager.HighlightBrush
            ttb.Content = _tcode.FName & vbCrLf & "존재하지 않거나 참조할 수 없는 함수입니다.  "
            ttb.VerticalAlignment = VerticalAlignment.Center

            Wrap.Children.Add(ttb)

            '없는 함수
            '그냥 인자들만 출력시킨다.
            isCmpTrigger = False
            IsEmpty = True
        Else
            isCmpTrigger = t.IsCmpTrigger
        End If

        _tcode.LoadArgText(scripter)

        If isCmpTrigger Then
            If t.SortArgList.Count = 0 Then
                Dim ttb As New Label
                ttb.VerticalContentAlignment = VerticalAlignment.Center
                ttb.VerticalAlignment = VerticalAlignment.Center
                If Not IsEmpty Then
                    If t.FSummary = "" Then
                        ttb.Content = t.FName
                    Else
                        ttb.Content = t.FSummary
                    End If
                End If

                Wrap.Children.Add(ttb)
            Else
                '트리거
                For i = 0 To t.SortArgList.Count - 1
                    Dim tstr As String = t.SortArgList(i)

                    If TriggerFunction.IsArg(tstr) Then
                        'Arg텍스트
                        Dim tindex As Integer = TriggerFunction.GetArgIndex(tstr)

                        Dim ttb As New Label
                        ttb.VerticalAlignment = VerticalAlignment.Center
                        ttb.Foreground = tmanager.HighlightBrush

                        ttb.Content = _tcode.LoadedArgString(tindex)

                        Wrap.Children.Add(ttb)
                    Else
                        Dim ttb As New Label
                        ttb.VerticalContentAlignment = VerticalAlignment.Center

                        ttb.Content = tstr

                        Wrap.Children.Add(ttb)
                    End If
                Next
            End If


        Else
            '인자들만 나열
            If True Then
                '설명
                If t IsNot Nothing Then
                    If t.FSummary <> "" Then
                        Dim ttb As New Label
                        ttb.VerticalContentAlignment = VerticalAlignment.Center

                        If Not IsEmpty Then
                            ttb.Content = t.FSummary
                        End If

                        Wrap.Children.Add(ttb)
                    Else
                        Dim ttb As New Label
                        ttb.VerticalContentAlignment = VerticalAlignment.Center

                        If Not IsEmpty Then
                            ttb.Content = t.FName
                        End If

                        Wrap.Children.Add(ttb)
                    End If
                End If
            End If

            '인자들을 나열한다.
            For i = 0 To _tcode.Args.Count - 1
                'Arg텍스트
                Dim ttb As New Label
                ttb.VerticalAlignment = VerticalAlignment.Center
                ttb.Foreground = tmanager.HighlightBrush

                ttb.Content = _tcode.LoadedArgString(i)

                Wrap.Children.Add(ttb)
            Next
        End If

        If Wrap.Children.Count = 0 Then
            If t IsNot Nothing Then
                Dim ttb As New Label
                ttb.VerticalAlignment = VerticalAlignment.Center
                ttb.Content = t.FName

                Wrap.Children.Add(ttb)
            End If
        End If
    End Sub
End Class
