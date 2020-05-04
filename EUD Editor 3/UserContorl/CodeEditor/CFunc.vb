Imports System.Text.RegularExpressions


Public Class FunctionToolTip
    Private pType As FType
    Public Enum FType
        Func
        Act
        Cond
    End Enum

    Public ReadOnly Property Type As FType
        Get
            Return pType
        End Get
    End Property

    Private pSummary As String
    Public ReadOnly Property Summary As String
        Get
            If pSummary Is Nothing Then
                Return ""
            End If

            Return pSummary
        End Get
    End Property


    Private FuncArgTooltip As List(Of String)
    Public ReadOnly Property GetTooltip(index As Integer) As String
        Get
            If index < FuncArgTooltip.Count Then
                Return FuncArgTooltip(index).Trim
            Else
                Return ""
            End If
        End Get
    End Property

    Public ReadOnly Property TooltipCount As Integer
        Get
            Return FuncArgTooltip.Count
        End Get
    End Property
    '/***
    ' * @Type
    ' * A
    ' * @Summary.ko-KR
    ' * 유닛을 생성하는 트리거
    ' *
    ' * 
    ' *
    ' * @param.Number.ko-KR
    ' * 생성 될 유닛의 수
    ' * @param.Unit.ko-KR
    ' * 생성될 유닛의 종류
    ' * @param.Where.ko-KR
    ' * 생성될 로케이션
    ' * @param.ForPlayer.ko-KR
    ' * 대상 플레이어
    '***/ 
    'function CreateUnit(Number, Unit : TrgUnit, Where : TrgLocation, ForPlayer : TrgPlayer){}

    Private Enum ReadStatus
        None
        Type
        Summary
        Param
    End Enum
    Public Sub New(InitStr As String, tFuncArgument As String, FName As String, Optional AddOrg As Boolean = True)
        Dim Lanstr As String = pgData.Setting(ProgramData.TSetting.Language)
        FuncArgTooltip = New List(Of String)

        Dim FuncArgs() As String = tFuncArgument.Split(",")
        For i = 0 To FuncArgs.Count - 1
            Dim argtext As String = FuncArgs(i)
            If argtext.IndexOf("/*") <> -1 And argtext.IndexOf("*/") <> -1 And argtext.IndexOf(":") = -1 Then
                argtext = argtext.Replace("*/", "")
                argtext = argtext.Replace("/*", ":")
            End If


            FuncArgTooltip.Add(argtext.Split(":").First.Trim)
        Next


        'MsgBox(tFuncArgument)
        'MsgBox(Lanstr)



        Dim Lines() As String = InitStr.Split(vbCrLf)

        Dim ReadStatus As ReadStatus
        Dim WriteParmIndex As Integer
        Dim FristParam As Boolean = False
        '만약 @지령이 Summary라면
        For i = 0 To Lines.Count - 1
            Dim LineStr As String = Lines(i).Replace("*", "").Trim
            If LineStr = "" Then
                Continue For
            End If



            Select Case LineStr
                Case "@Type"
                    ReadStatus = ReadStatus.Type
                    Continue For
                Case "@Summary." & Lanstr
                    ReadStatus = ReadStatus.Summary
                    Continue For
                Case Else
                    If LineStr.IndexOf("@param") = 0 Then
                        Dim ParamSplitter() As String = LineStr.Split(".")
                        If ParamSplitter.Count = 3 Then
                            If ParamSplitter.Last = Lanstr Then
                                If FuncArgTooltip.IndexOf(ParamSplitter(1)) >= 0 Then
                                    WriteParmIndex = FuncArgTooltip.IndexOf(ParamSplitter(1))
                                    ReadStatus = ReadStatus.Param
                                    FristParam = True
                                    Continue For
                                End If
                            End If
                        End If
                    End If
            End Select

            Select Case ReadStatus
                Case ReadStatus.Summary
                    pSummary = pSummary & LineStr & vbCrLf
                Case ReadStatus.Param
                    If FristParam Then
                        FristParam = False
                        FuncArgTooltip(WriteParmIndex) = LineStr & vbCrLf
                    Else
                        FuncArgTooltip(WriteParmIndex) = FuncArgTooltip(WriteParmIndex) & " : " & LineStr & vbCrLf
                    End If
                Case ReadStatus.Type
                    Select Case LineStr
                        Case "A"
                            pType = FType.Act
                        Case "C"
                            pType = FType.Cond
                        Case "F"
                            pType = FType.Func
                    End Select
            End Select


            'MsgBox(LineStr)
        Next
        If pSummary IsNot Nothing Then
            pSummary = pSummary.Trim
            If AddOrg Then
                pSummary = FName & "(" & tFuncArgument & ")" & vbCrLf & pSummary
            End If
        End If

    End Sub
    Public Sub New()
        FuncArgTooltip = New List(Of String)
    End Sub

End Class

Public Class CFunc
    Private FuncTooltip As List(Of FunctionToolTip)
    Private FuncNames As List(Of String)
    Private FuncArgument As List(Of String)

    Private VariableNames As List(Of String)
    Private VariableType As List(Of String)

    Private Objects As List(Of CObject)



    Public ReadOnly Property ObjectCount As Integer
        Get
            Return Objects.Count
        End Get
    End Property
    Public ReadOnly Property GetObject(index As Integer) As CObject
        Get
            Return Objects(index)
        End Get
    End Property



    Public ReadOnly Property FuncCount As Integer
        Get
            Return FuncNames.Count
        End Get
    End Property

    Public ReadOnly Property VariableCount As Integer
        Get
            Return VariableNames.Count
        End Get
    End Property
    Public ReadOnly Property GetFuncName(index As Integer) As String
        Get
            Return FuncNames(index)
        End Get
    End Property
    Public ReadOnly Property GetFuncArgument(index As Integer) As String
        Get
            Return FuncArgument(index)
        End Get
    End Property


    Public ReadOnly Property GetVariableNames(index As Integer) As String
        Get
            Return VariableNames(index)
        End Get
    End Property
    Public ReadOnly Property GetVariableType(index As Integer) As String
        Get
            Return VariableType(index)
        End Get
    End Property



    Public ReadOnly Property GetFuncTooltip(index As Integer) As FunctionToolTip
        Get
            Return FuncTooltip(index)
        End Get
    End Property


    Public Function FindArgument(name As String, Argindex As Integer) As String
        Dim index As Integer = FuncNames.IndexOf(name)

        If index >= 0 Then
            Dim Arguments() As String = FuncArgument(index).Split(",")

            If Argindex < Arguments.Count Then
                Return Arguments(Argindex)
            End If
        End If

        Return ""
    End Function

    Public Function SearchFunc(name As String) As Integer
        Dim index As Integer = FuncNames.IndexOf(name)
        Return index
    End Function


    Public Function GetPopupToolTip(name As String, Argindex As Integer) As Border
        Dim index As Integer = FuncNames.IndexOf(name)

        If index >= 0 Then
            Dim TBorder As New Border

            Dim BigPanel As New StackPanel

            Dim sp As New StackPanel
            sp.Orientation = Orientation.Horizontal

            Dim textbox1 As New TextBlock
            textbox1.Text = FuncNames(index)
            textbox1.Foreground = Brushes.DodgerBlue
            sp.Children.Add(textbox1)



            Dim textbox3 As New TextBlock
            textbox3.Text = "("
            sp.Children.Add(textbox3)

            Dim Arguments() As String = FuncArgument(index).Split(",")
            For i = 0 To Arguments.Count - 1
                Dim textbox2 As New TextBlock

                If Argindex = i Then
                    textbox2.Foreground = Brushes.OrangeRed
                    textbox2.FontWeight = FontWeights.UltraBold
                End If
                If i = 0 Then
                    textbox2.Text = Arguments(i).Split(":").First.Trim
                Else
                    textbox2.Text = "," & Arguments(i).Split(":").First.Trim
                End If


                sp.Children.Add(textbox2)
            Next

            Dim textbox4 As New TextBlock
            textbox4.Text = ")"
            sp.Children.Add(textbox4)





            BigPanel.Children.Add(sp)


            Dim textbox6 As New TextBlock
            textbox6.Text = GetFuncTooltip(index).Summary
            textbox6.Foreground = New SolidColorBrush(Color.FromArgb(200, 150, 150, 150))
            If textbox6.Text <> "" Then
                BigPanel.Children.Add(textbox6)
            End If


            Dim textbox5 As New TextBlock
            textbox5.Text = GetFuncTooltip(index).GetTooltip(Argindex)
            If textbox5.Text <> "" Then
                BigPanel.Children.Add(textbox5)
            End If







            TBorder.Child = BigPanel



            Return TBorder
        End If


        Return Nothing
    End Function




    Public Sub New()
        FuncNames = New List(Of String)
        FuncArgument = New List(Of String)
        FuncTooltip = New List(Of FunctionToolTip)

        VariableNames = New List(Of String)
        VariableType = New List(Of String)

        Objects = New List(Of CObject)
    End Sub

    'Private arguments As List(Of FunArgument)

    Public Sub Init()
        FuncTooltip.Clear()
        FuncNames.Clear()
        FuncArgument.Clear()

        VariableNames.Clear()
        VariableType.Clear()
        Objects.Clear()
    End Sub


    Private Function BraketLiner(str As String, StartIndex As Integer) As Integer
        Dim si As Long = StartIndex + 1
        Dim stack As New Stack()

        stack.Push("{")
        While si < str.Length
            Dim chr As String = Mid(str, si, 1)

            Select Case chr
                Case "{", "[", "("
                    stack.Push(chr)
                Case ")"
                    If stack.Peek = "(" Then
                        stack.Pop()
                    Else
                        Exit While
                    End If
                Case "]"
                    If stack.Peek = "[" Then
                        stack.Pop()
                    Else
                        Exit While
                    End If
                Case "}"
                    If stack.Peek = "{" Then
                        stack.Pop()
                    Else
                        Exit While
                    End If
            End Select

            si += 1
            If stack.Count = 0 Then
                Exit While
            End If
        End While
        If stack.Count = 0 Then
            Return si
        Else
            Return -1
        End If
    End Function
    Public Sub LoadFunc(str As String, Optional StartPos As Integer = -1, Optional NameSpace_ As String = "")
        If StartPos <> -1 Then
            str = Mid(str, 1, StartPos)
        End If

        'const\s+[\w\d_]+\s+=\s+(.*);
        If str Is Nothing Then
            Return
        End If

        If True Then
            Dim fregex As New Regex("object[\s]+([\w\d]+)\s*({)")

            Dim matches As MatchCollection = fregex.Matches(str)
            Dim changesStr As New List(Of String)


            'MsgBox(matches.Count)
            For i = 0 To matches.Count - 1
                Dim ObjName As String = matches(i).Groups(1).Value
                Dim ObjContents As String

                Dim StartIndex As Integer = matches(i).Index + matches(i).Length

                Dim bl As Integer = BraketLiner(str, StartIndex)
                If bl = -1 Then
                    Continue For
                End If

                ObjContents = Mid(str, StartIndex, bl - StartIndex)

                'MsgBox(ObjName) '오브젝트 이름
                'MsgBox(ObjContents)
                Objects.Add(New CObject(ObjName, ObjContents))
                changesStr.Add(matches(i).Value & Mid(ObjContents, 2))
            Next
            For i = 0 To changesStr.Count - 1
                str = Replace(str, changesStr(i), "", 1, 1)
            Next
        End If



        If True Then
            Dim fregex As New Regex("\/\*\*\*([^/]*)\*\*\*\/\s+function[\s]+([\w\d]+)\(([^{};]*)\)\s*{")

            Dim matches As MatchCollection = fregex.Matches(str)

            Dim changesStr As New List(Of String)
            For i = 0 To matches.Count - 1
                Dim ObjContents As String

                Dim argment As String = matches(i).Groups(3).Value
                If argment.IndexOf("/*") <> -1 And argment.IndexOf("*/") <> -1 And argment.IndexOf(":") = -1 Then
                    argment = argment.Replace("*/", "")
                    argment = argment.Replace("/*", ":")
                End If
                Dim StartIndex As Integer = matches(i).Index + matches(i).Length

                Dim bl As Integer = BraketLiner(str, StartIndex)
                If bl = -1 Then
                    Continue For
                End If

                ObjContents = Mid(str, StartIndex, bl - StartIndex)
                changesStr.Add(matches(i).Value & Mid(ObjContents, 2))

                FuncTooltip.Add(New FunctionToolTip(matches(i).Groups(1).Value, matches(i).Groups(3).Value, matches(i).Groups(2).Value))
                FuncNames.Add(matches(i).Groups(2).Value)

                FuncArgument.Add(argment)
            Next
            For i = 0 To changesStr.Count - 1
                str = Replace(str, changesStr(i), "", 1, 1)
            Next
        End If

        If True Then
            Dim fregex As New Regex("function[\s]+([\w\d]+)\(([^{};]*)\)\s*{")

            Dim matches As MatchCollection = fregex.Matches(str)

            Dim changesStr As New List(Of String)
            'MsgBox(matches.Count)
            For i = 0 To matches.Count - 1
                Dim ObjContents As String

                Dim StartIndex As Integer = matches(i).Index + matches(i).Length

                Dim bl As Integer = BraketLiner(str, StartIndex)
                If bl = -1 Then
                    Continue For
                End If

                If FuncNames.IndexOf(matches(i).Groups(1).Value) = -1 Then
                    FuncTooltip.Add(New FunctionToolTip())
                    FuncNames.Add(matches(i).Groups(1).Value)


                    Dim argment As String = matches(i).Groups(2).Value
                    If argment.IndexOf("/*") <> -1 And argment.IndexOf("*/") <> -1 And argment.IndexOf(":") = -1 Then
                        argment = argment.Replace("*/", "")
                        argment = argment.Replace("/*", ":")
                    End If

                    FuncArgument.Add(argment)
                End If

                ObjContents = Mid(str, StartIndex, bl - StartIndex)
                changesStr.Add(matches(i).Value & Mid(ObjContents, 2))

            Next
            For i = 0 To changesStr.Count - 1
                str = Replace(str, changesStr(i), "", 1, 1)
            Next
        End If




        If True Then
            Dim fregex As New Regex("const\s+([\w\d_]+)([^;]*);")

            Dim matches As MatchCollection = fregex.Matches(str)

            For i = 0 To matches.Count - 1
                If VariableNames.IndexOf(matches(i).Groups(1).Value) = -1 Then
                    'FuncTooltip.Add(New FunctionToolTip(matches(i).Groups(1).Value, matches(i).Groups(3).Value))
                    VariableNames.Add(matches(i).Groups(1).Value)
                    VariableType.Add(matches(i).Groups(2).Value.Split("=").Last.Trim)
                End If

            Next
        End If
        If True Then
            Dim fregex As New Regex("var\s+([\w\d_]+)([^;]*);")

            Dim matches As MatchCollection = fregex.Matches(str)

            For i = 0 To matches.Count - 1
                If VariableNames.IndexOf(matches(i).Groups(1).Value) = -1 Then
                    'FuncTooltip.Add(New FunctionToolTip(matches(i).Groups(1).Value, matches(i).Groups(3).Value))
                    VariableNames.Add(matches(i).Groups(1).Value)
                    VariableType.Add(" ")
                End If
            Next
        End If







    End Sub
End Class

