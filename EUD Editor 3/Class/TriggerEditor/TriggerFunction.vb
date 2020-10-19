Imports System.IO
Imports System.Text.RegularExpressions

Public Class TriggerManager
    'TE에 관련된 트리거를 관리하는 클래스
    Public HighlightBrush As SolidColorBrush = New SolidColorBrush(Color.FromRgb(232, 90, 113))
    Public Function GetTriggerList(ftype As TriggerFunction.EFType) As List(Of TriggerFunction)
        Select Case ftype
            Case TriggerFunction.EFType.Action
                Return ActionList
            Case TriggerFunction.EFType.Condition
                Return ConditionList
            Case TriggerFunction.EFType.Plib
                Return PlibList
            Case TriggerFunction.EFType.Lua
                Return LuaList
            Case TriggerFunction.EFType.UserFunc
                'TODO:UserFunc 만들어야됨
            Case TriggerFunction.EFType.ExternFunc
                'TODO:UserFunc ExternFunc만들어야됨
        End Select
    End Function




    Private ActionList As New List(Of TriggerFunction)
    Private ConditionList As New List(Of TriggerFunction)
    Private PlibList As New List(Of TriggerFunction)
    Private LuaList As New List(Of TriggerFunction)


    Public Sub New()
        Dim fs As New FileStream(Tool.TriggerEditorPath("epsFunctions.txt"), FileMode.Open)
        Dim sr As New StreamReader(fs)

        Dim funclist As List(Of TriggerFunction) = Nothing
        Try
            funclist = GetListFromEpScript(sr.ReadToEnd)
        Catch ex As Exception
            MsgBox("기본 함수 초기화 실패")
            Return
        End Try
        sr.Close()
        fs.Close()


        '루아함수 변형
        For i = 0 To macro.FunctionList.Count - 1
            Dim func As MacroManager.LuaFunction = macro.FunctionList(i)

            Dim nTF As New TriggerFunction

            nTF.FName = func.Fname
            nTF.FSummary = func.Fcomment
            nTF.FGruop = func.LuaGroup

            nTF.FType = TriggerFunction.EFType.Lua

            For j = 0 To func.ArgName.Count - 1
                nTF.Args.Add(New TriggerFunction.Arg(func.ArgName(j), "", func.ArgType(j)))
            Next

            nTF.SetArgComment()


            LuaList.Add(nTF)
        Next




        '함수 분류
        For i = 0 To funclist.Count - 1
            Select Case funclist(i).FType
                Case TriggerFunction.EFType.Action
                    ActionList.Add(funclist(i))
                Case TriggerFunction.EFType.Condition
                    ConditionList.Add(funclist(i))
                Case TriggerFunction.EFType.Plib
                    PlibList.Add(funclist(i))
            End Select
        Next
    End Sub



    Public Function GetListFromEpScript(EpScript As String) As List(Of TriggerFunction)
        Dim rFunc As New List(Of TriggerFunction)


        Dim tooltipregex As New Regex("(\/\*\*\*[^\/]*\*\*\*\/\s+)function\s+([\w_]+)\(([\/\*\w\,_\:\s]*)\)\s*[^\;]")
        Dim normalregex As New Regex("function\s+([\w_]+)\(([\/\*\w\,_\:\s]*)\)\s*[^\;]")


        Dim matches As MatchCollection
        matches = tooltipregex.Matches(EpScript)
        For i = 0 To matches.Count - 1
            Dim tooltips As String = matches(i).Groups(1).Value
            Dim funcname As String = matches(i).Groups(2).Value
            Dim args As String = matches(i).Groups(3).Value

            Dim tfun As TriggerFunction = TriggerFunction.GetFunctionFromStr(tooltips, funcname, args)

            If tfun IsNot Nothing Then
                tfun.SetArgComment()
                rFunc.Add(tfun)
            End If
        Next

        matches = normalregex.Matches(EpScript)
        For i = 0 To matches.Count - 1
            Dim tooltips As String = ""
            Dim funcname As String = matches(i).Groups(1).Value
            Dim args As String = matches(i).Groups(2).Value

            Dim tfun As TriggerFunction = TriggerFunction.GetFunctionFromStr(tooltips, funcname, args)

            If tfun IsNot Nothing Then
                rFunc.Add(tfun)
            End If
        Next



        Return rFunc
    End Function

End Class


Public Class TriggerFunction
    Public Shared Function GetFunctionFromStr(Tooltipstr As String, funcname As String, args As String) As TriggerFunction
        Dim rTrg As New TriggerFunction
        rTrg.FName = funcname

        If args.Trim <> "" Then
            Dim arglist() As String = args.Split(",")
            For i = 0 To arglist.Count - 1
                Dim argname As String = ""
                Dim argtype As String = ""

                Dim t() As String = arglist(i).Split(":")

                If t.Length = 1 Then
                    Dim st() As String = arglist(i).Split("/*")

                    If st.Length = 1 Then
                        argname = arglist(i).Trim
                    Else
                        argname = st(0).Trim
                        argtype = st(1).Replace("*/", "").Replace("*", "").Trim
                    End If

                Else
                    argname = t(0).Trim
                    argtype = t(1).Trim
                End If

                rTrg.Args.Add(New Arg(argname, "", argtype))
            Next
        End If



        Tooltipstr = Tooltipstr.Trim
        If Tooltipstr = "" Then
            rTrg.FType = EFType.Plib
            Return rTrg
        End If

        '툴팁
        Dim tooltiplist() As String = Tooltipstr.Split(vbCrLf)

        Dim IsType As Boolean = False
        Dim IsSummary As Boolean = False
        Dim IsParam As Boolean = False
        Dim IsGroup As Boolean = False
        Dim lan As String = ""
        Dim paramName As String = ""
        For i = 0 To tooltiplist.Count - 1
            Dim tstr As String = tooltiplist(i).Replace(vbCrLf, "").Replace(vbCr, "").Replace(vbLf, "")
            If i = 0 Then
                '첫줄
                If tstr = "/***" Then
                    Continue For
                Else
                    Exit For
                End If
            ElseIf i = tooltiplist.Count - 1 Then
                '끝줄
                If tstr = "***/" Then
                    Continue For
                Else
                    Exit For
                End If
            End If

            If tstr.IndexOf(" * ") <> 0 Then
                Exit For
            End If

            tstr = tstr.Replace(" * ", "")

            Dim tstrtype() As String = tstr.Split(".")

            Select Case tstrtype(0)
                Case "@Type"
                    IsType = True
                    IsSummary = False
                    IsParam = False
                    IsGroup = False

                Case "@Summary"
                    lan = tstrtype.Last

                    IsType = False
                    IsSummary = True
                    IsParam = False
                    IsGroup = False

                    rTrg.FSummary = ""
                Case "@param"
                    lan = tstrtype.Last

                    IsType = False
                    IsSummary = False
                    IsParam = True
                    IsGroup = False

                    paramName = tstrtype(1)

                    Dim targ As Arg = rTrg.GetArg(paramName)
                    If targ IsNot Nothing Then
                        rTrg.GetArg(paramName).ASummary = ""
                    End If

                Case "@Group"

                    IsType = False
                    IsSummary = False
                    IsParam = False
                    IsGroup = True


                Case Else
                    If lan = pgData.Setting(ProgramData.TSetting.Language) Then
                        If IsSummary Then
                            rTrg.FSummary = rTrg.FSummary & tstr
                        ElseIf IsParam Then
                            Dim targ As Arg = rTrg.GetArg(paramName)
                            If targ IsNot Nothing Then
                                targ.ASummary = targ.ASummary & tstr
                            End If
                        End If
                    End If
                    If IsType Then
                        Select Case tstr
                            Case "F"
                                rTrg.FType = EFType.Plib
                            Case "C"
                                rTrg.FType = EFType.Condition
                            Case "A"
                                rTrg.FType = EFType.Action
                        End Select
                    ElseIf IsGroup Then
                        rTrg.FGruop = tstr
                    End If
            End Select
        Next





        'MsgBox("Tooltipstr : " & Tooltipstr)
        'MsgBox("funcname : " & funcname)
        'MsgBox("args : " & args)
        Return rTrg
    End Function

    '모든 TE에 관련된 트리거를 관리하는 클래스
    Public Enum EFType
        Action
        Condition
        Plib
        Lua
        UserFunc
        ExternFunc
        Args
    End Enum

    Public FType As EFType
    Public FSummary As String
    Public FName As String

    Public FGruop As String

    Public ReadOnly Property IsVariableArgument As Boolean
        Get
            Return Args.Last.AName(0) = "*"
        End Get
    End Property


    Public ReadOnly Property GetArg(AName As String) As Arg
        Get
            Return Args.Find(Function(x As Arg) As Boolean
                                 Return (AName = x.AName)
                             End Function)
        End Get
    End Property
    Public Args As New List(Of Arg)
    Public Class Arg
        Public AName As String = ""
        Public ASummary As String = ""
        Public AType As String = ""

        Public Sub New(_AName As String, _ASummary As String, _AType As String)
            AName = _AName
            ASummary = _ASummary
            AType = _AType
        End Sub
    End Class


    '코드가 완벽한 틀을 하였는가.
    Public IsCmpTrigger As Boolean = False
    Public SortArgList As New List(Of String)
    Public Sub SetArgComment()
        '코맨트로부터 Arg를 추출하여 정렬.
        SortArgList.Clear()

        Dim tsummary As String = FSummary
        IsCmpTrigger = False
        Dim iscmp As Boolean = False

        If Args.Count = 0 Then
            IsCmpTrigger = True
            Return
        End If


        'FSummary에서 Arg를 가져옴
        For i = 0 To Args.Count - 1
            Dim aname As String = "[" & Args(i).AName.Trim & "]"

            If tsummary.IndexOf(aname) = -1 Then
                Return
            End If
            tsummary = tsummary.Replace(aname, "▦§" & i & "▦")
        Next

        Dim argsb() As String = tsummary.Split(CType("▦", Char()), StringSplitOptions.RemoveEmptyEntries)
        SortArgList.AddRange(argsb)
        IsCmpTrigger = True
    End Sub

    Public Shared Function IsArg(str As String) As Boolean
        If str.Length > 0 Then
            Return str(0) = "§"
        Else
            Return False
        End If
    End Function

    Public Shared Function GetArgIndex(str As String) As Integer
        Dim tstr As String = str.Replace("§", "")

        Return tstr
    End Function
End Class
