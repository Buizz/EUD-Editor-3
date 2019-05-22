Imports System.Text.RegularExpressions


Public Class FunctionToolTip
    Private pType As FType
    Public Enum FType
        Func
        Act
        Cond
    End Enum

    Public ReadOnly Property Type As String
        Get
            Return pType
        End Get
    End Property

    Private pSummary As String
    Public ReadOnly Property Summary As String
        Get
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
    Public Sub New(InitStr As String, tFuncArgument As String)
        Dim Lanstr As String = pgData.Setting(ProgramData.TSetting.Language)
        FuncArgTooltip = New List(Of String)


        Dim FuncArgs() As String = tFuncArgument.Split(",")
        For i = 0 To FuncArgs.Count - 1
            FuncArgTooltip.Add(FuncArgs(i).Split(":").First.Trim)
        Next


        'MsgBox(tFuncArgument)
        'MsgBox(Lanstr)



        Dim Lines() As String = InitStr.Split(vbCrLf)

        Dim ReadStatus As ReadStatus
        Dim WriteParmIndex As Integer
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
                    FuncArgTooltip(WriteParmIndex) = FuncArgTooltip(WriteParmIndex) & LineStr & vbCrLf
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

        pSummary = pSummary.Trim

    End Sub
    Public Sub New()
        FuncArgTooltip = New List(Of String)
    End Sub

End Class

Public Class CFunc
    Private FuncTooltip As List(Of FunctionToolTip)
    Private FuncNames As List(Of String)
    Private FuncArgument As List(Of String)

    Public ReadOnly Property FuncCount As Integer
        Get
            Return FuncNames.Count
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




    Public Function GetPopupToolTip(name As String, Argindex As Integer) As Border
        Dim index As Integer = FuncNames.IndexOf(name)

        If index >= 0 Then
            Dim TBorder As New Border

            Dim BigPanel As New StackPanel

            Dim sp As New StackPanel
            sp.Orientation = Orientation.Horizontal

            Dim textbox1 As New TextBlock
            textbox1.Text = FuncNames(index)
            textbox1.Foreground = Brushes.Red
            sp.Children.Add(textbox1)



            Dim textbox3 As New TextBlock
            textbox3.Text = "("
            sp.Children.Add(textbox3)

            Dim Arguments() As String = FuncArgument(index).Split(",")
            For i = 0 To Arguments.Count - 1
                Dim textbox2 As New TextBlock

                If Argindex = i Then
                    textbox2.Foreground = Brushes.Blue
                    textbox2.FontWeight = FontWeights.UltraBold
                End If
                If i = 0 Then
                    textbox2.Text = Arguments(i).Trim
                Else
                    textbox2.Text = "," & Arguments(i).Trim
                End If


                sp.Children.Add(textbox2)
            Next

            Dim textbox4 As New TextBlock
            textbox4.Text = ")"
            sp.Children.Add(textbox4)


            BigPanel.Children.Add(sp)




            Dim textbox5 As New TextBlock
            textbox5.Text = GetFuncTooltip(index).GetTooltip(Argindex)
            BigPanel.Children.Add(textbox5)







            TBorder.Child = BigPanel



            Return TBorder
        End If


        Return Nothing
    End Function




    Public Sub New()
        FuncNames = New List(Of String)
        FuncArgument = New List(Of String)
        FuncTooltip = New List(Of FunctionToolTip)
    End Sub

    'Private arguments As List(Of FunArgument)

    Public Sub LoadFunc(str As String)
        FuncTooltip.Clear()
        FuncNames.Clear()
        FuncArgument.Clear()




        If True Then
            Dim fregex As New Regex("\/\*\*\*([^/]*)\*\*\*\/\s+function[\s]+([\w\d]+)\(([^{}]*)\)\s*{")

            Dim matches As MatchCollection = fregex.Matches(str)

            For i = 0 To matches.Count - 1
                FuncTooltip.Add(New FunctionToolTip(matches(i).Groups(1).Value, matches(i).Groups(3).Value))
                FuncNames.Add(matches(i).Groups(2).Value)
                FuncArgument.Add(matches(i).Groups(3).Value)
            Next
        End If


        If True Then
            Dim fregex As New Regex("function[\s]+([\w\d]+)\(([^{}]*)\)\s*{")

            Dim matches As MatchCollection = fregex.Matches(str)


            'MsgBox(matches.Count)
            For i = 0 To matches.Count - 1
                If FuncNames.IndexOf(matches(i).Groups(1).Value) = -1 Then
                    FuncTooltip.Add(New FunctionToolTip())
                    FuncNames.Add(matches(i).Groups(1).Value)
                    FuncArgument.Add(matches(i).Groups(2).Value)
                End If
            Next
        End If
    End Sub
End Class

