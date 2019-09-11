Namespace StringTool
    Module StringTool
        Public Function ChangeSlash(text As String) As String
            text = text.Replace("\\", "\")
            text = text.Replace("\<", "<")
            text = text.Replace("\>", ">")

            Return text
        End Function


        Private Function ExtratStr(str As String) As String
            Return Mid(str, 2, str.Length - 2)
        End Function

        Public Function ChangeCharAt(index As Integer, str As String, tostr As String) As String
            If str.Length <= index Then
                Return ""
            End If

            'ᚎ ᚍ ᚌ
            str = str.Replace("\\", "ᚎ")
            str = str.Replace("\<", "ᚍ")
            str = str.Replace("\>", "ᚌ")

            Dim MatchPass As Integer = 0

            Dim TempChar As String = "ᚏ"
            Dim SpecialKeys As New List(Of String)
            Dim SpecialKeyPos As New List(Of Integer)
            Dim OriginalKeys As New List(Of String)

            Dim rgx As New Text.RegularExpressions.Regex("<[^>\\]*(?:\\.[^>\\]*)*>", Text.RegularExpressions.RegexOptions.IgnoreCase)
            Dim Matchs As Text.RegularExpressions.MatchCollection = rgx.Matches(str)
            'Rgx들을 문자 치환하고 해당 번지를 돌려주자.
            '만약 Virtual안에 있으면 해당 번호로 돌려줌.
            For i = 0 To Matchs.Count - 1
                Dim tMatchs As Text.RegularExpressions.MatchCollection = rgx.Matches(str)

                Dim pureStr As String = ExtratStr(tMatchs(MatchPass).Value)
                If pureStr = "ᚎ" Then
                    pureStr = "\\"
                ElseIf pureStr = "ᚍ" Then
                    pureStr = "\<"
                ElseIf pureStr = "ᚌ" Then
                    pureStr = "\>"
                End If
                Dim ResultStr As String = pureStr

                Dim PassFlag As Boolean = False
                '값의 순수성 검사.(Virtual KEY문자이거나 16진수 이거나 판단)
                For keys = 0 To SCConst.ASCIICount
                    If scData.ASCIICode(keys) = pureStr Then 'Virtual KEY일 경우
                        ResultStr = Hex(keys).PadLeft(2, "0")
                        PassFlag = True
                    End If
                Next
                If Not PassFlag Then
                    Try
                        Dim Isnum As Long = "&H" & pureStr '16진수인지 판별
                        ResultStr = Hex(Isnum).PadLeft(2, "0")
                        PassFlag = True
                    Catch ex As Exception

                    End Try
                End If
                If Not PassFlag Then
                    MatchPass += 1
                    Continue For
                End If

                OriginalKeys.Add(tMatchs(MatchPass).Value)
                SpecialKeys.Add(ResultStr)
                SpecialKeyPos.Add(tMatchs(MatchPass).Index)
                str = Replace(str, tMatchs(MatchPass).Value, TempChar, 1, 1)
            Next
            If str(index) = TempChar Then
                For i = 0 To SpecialKeyPos.Count - 1
                    If SpecialKeyPos(i) = index Then
                        str = Replace(str, TempChar, tostr, 1, 1)
                    Else
                        str = Replace(str, TempChar, OriginalKeys(i), 1, 1)
                    End If
                Next
            Else
                '12 3 45  index = 2
                str = Mid(str, 1, index) & tostr & Mid(str, index + 2)
                For i = 0 To SpecialKeyPos.Count - 1
                    str = Replace(str, TempChar, OriginalKeys(i), 1, 1)
                Next
            End If

            str = str.Replace("ᚎ", "\\")
            str = str.Replace("ᚍ", "\<")
            str = str.Replace("ᚌ", "\>")

            Return str
        End Function
        Public Function GetCharAt(index As Integer, str As String) As String
            If str.Count > index Then
                str = str.Replace("\\", "ᚎ")
                str = str.Replace("\<", "ᚍ")
                str = str.Replace("\>", "ᚌ")

                Dim MatchPass As Integer = 0

                Dim TempChar As String = "ᚏ"
                Dim SpecialKeys As New List(Of String)
                Dim SpecialKeyPos As New List(Of Integer)

                Dim rgx As New Text.RegularExpressions.Regex("<[^>\\]*(?:\\.[^>\\]*)*>", Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim Matchs As Text.RegularExpressions.MatchCollection = rgx.Matches(str)
                'Rgx들을 문자 치환하고 해당 번지를 돌려주자.
                '만약 Virtual안에 있으면 해당 번호로 돌려줌.
                For i = 0 To Matchs.Count - 1
                    Dim tMatchs As Text.RegularExpressions.MatchCollection = rgx.Matches(str)

                    Dim pureStr As String = ExtratStr(tMatchs(MatchPass).Value)
                    If pureStr = "ᚎ" Then
                        pureStr = "\\"
                    ElseIf pureStr = "ᚍ" Then
                        pureStr = "\<"
                    ElseIf pureStr = "ᚌ" Then
                        pureStr = "\>"
                    End If
                    Dim ResultStr As String = pureStr


                    Dim PassFlag As Boolean = False
                    '값의 순수성 검사.(Virtual KEY문자이거나 16진수 이거나 판단)
                    For keys = 0 To SCConst.ASCIICount
                        If scData.ASCIICode(keys) = pureStr Then 'Virtual KEY일 경우
                            ResultStr = Hex(keys).PadLeft(2, "0")
                            PassFlag = True
                        End If
                    Next
                    If Not PassFlag Then
                        Try
                            Dim Isnum As Long = "&H" & pureStr '16진수인지 판별
                            ResultStr = Hex(Isnum).PadLeft(2, "0")
                            PassFlag = True
                        Catch ex As Exception

                        End Try
                    End If
                    If Not PassFlag Then
                        MatchPass += 1
                        Continue For
                    End If


                    SpecialKeys.Add(ResultStr)
                    SpecialKeyPos.Add(tMatchs(MatchPass).Index)
                    str = Replace(str, tMatchs(MatchPass).Value, TempChar, 1, 1)
                Next

                'str = str.Replace("ᚎ", "\")
                'str = str.Replace("ᚍ", "<")
                'str = str.Replace("ᚌ", ">")
                If str.Count > index Then
                    If str(index) = TempChar Then
                        For i = 0 To SpecialKeyPos.Count - 1
                            If SpecialKeyPos(i) = index Then
                                Return SpecialKeys(i)
                            End If
                        Next
                    Else
                        Return str(index)
                    End If
                End If
            End If

            Return " "
        End Function
    End Module

End Namespace