Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class TriggerManager
    'TE에 관련된 트리거를 관리하는 클래스
    Public HighlightBrush As SolidColorBrush = New SolidColorBrush(Color.FromRgb(232, 90, 113))
    Public Function GetTriggerList(ftype As TriggerFunction.EFType, Scripter As ScriptEditor) As List(Of TriggerFunction)
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

                If Scripter.GetType Is GetType(ClassicTriggerEditor) Then
                    Dim t As List(Of TriggerFunction) = CType(Scripter, ClassicTriggerEditor).ImportFuncs

                    If t Is Nothing Then
                        CType(Scripter, ClassicTriggerEditor).ImportFileRefresh()
                        t = CType(Scripter, ClassicTriggerEditor).ImportFuncs
                    End If

                    Return t
                End If
        End Select
        Return Nothing
    End Function


    Public DefaultTEFiles As New Dictionary(Of String, TEFile)






    Public Function GetImportList() As String()
        Dim rstrlist As New List(Of String)


        For i = 0 To DefaultTEFiles.Keys.Count - 1
            rstrlist.Add("TriggerEditor." & DefaultTEFiles.Keys(i))
        Next



        If pjData IsNot Nothing Then
            GetFiles(pjData.TEData.PFIles, "NULL", rstrlist)
        End If

        Return rstrlist.ToArray
    End Function
    Private Sub GetFiles(tdata As TEFile, fname As String, rstrlist As List(Of String))
        If fname = "NULL" Then
            fname = ""
        ElseIf fname = "" Then
            fname = tdata.FileName
        Else
            fname = fname & "." & tdata.FileName
        End If


        If tdata.IsFile Then
            '파일 일 경우
            rstrlist.Add(fname)
        Else
            For i = 0 To tdata.FileCount - 1
                GetFiles(tdata.Files(i), fname, rstrlist)
            Next
            For i = 0 To tdata.FolderCount - 1
                GetFiles(tdata.Folders(i), fname, rstrlist)
            Next
        End If
    End Sub




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
            Tool.CustomMsgBox("LoadFail epsFunctions.txt" & vbCrLf & ex.ToString, MessageBoxButton.OK)
            sr.Close()
            fs.Close()
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
            nTF.FGroup = func.LuaGroup

            nTF.FType = TriggerFunction.EFType.Lua

            For j = 0 To func.ArgName.Count - 1
                nTF.Args.Add(New TriggerFunction.Arg(func.ArgName(j), func.ArgSummary(j), func.ArgType(j)))
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

        '기본 TEFile로드
        Dim folderpath As String = BuildData.GetTriggerEditorFolderPath

        For Each files As String In My.Computer.FileSystem.GetFiles(folderpath)
            Dim fileinfo As FileInfo = My.Computer.FileSystem.GetFileInfo(files)

            If fileinfo.Extension <> ".eps" Then
                Continue For
            End If

            Dim filename As String = fileinfo.Name.Split(".").First

            fs = New IO.FileStream(files, FileMode.Open)
            sr = New IO.StreamReader(fs)

            Dim str As String = sr.ReadToEnd


            sr.Close()
            fs.Close()



            Dim tfile As New TEFile(filename, TEFile.EFileType.CUIEps)
            CType(tfile.Scripter, CUIScriptEditor).StringText = str

            DefaultTEFiles.Add(filename, tfile)
        Next
    End Sub


    Public Function GetVariableFromEpScript(EpScript As String, Optional _Group As String = "") As List(Of DefineVariable)
        Dim rVars As New List(Of DefineVariable)

        Dim sr As New StringReader(EpScript)
        Dim ci As Integer = -1
        Dim c As Char

        Dim st As New Stack


        Dim sb As New StringBuilder
        While True
            ci = sr.Read()
            If ci = -1 Then
                Exit While
            End If

            c = ChrW(ci)


            Select Case c
                Case "("
                    st.Push(c)
                Case "{"
                    st.Push(c)
                Case "["
                    st.Push(c)
            End Select
            If st.Count = 0 Then
                sb.Append(c)
            End If
            Select Case c
                Case ")"
                    If st.Peek = "(" Then
                        st.Pop()
                    Else
                        '오류
                        Exit While
                    End If
                Case "}"
                    If st.Peek = "{" Then
                        st.Pop()
                    Else
                        '오류
                        Exit While
                    End If
                Case "]"
                    If st.Peek = "[" Then
                        st.Pop()
                    Else
                        '오류
                        Exit While
                    End If
            End Select
        End While

        sr.Close()

        Dim tstr As String = sb.ToString

        Dim varregex As New Regex("var\s+([\w\d_]+)")
        Dim constvaregex As New Regex("const\s+([\w\d_]+)")


        Dim matches As MatchCollection
        matches = varregex.Matches(tstr)
        For i = 0 To matches.Count - 1
            Dim varname As String = matches(i).Groups(1).Value

            Dim vDefine As New DefineVariable(varname, "", "", _Group)
            rVars.Add(vDefine)
        Next

        matches = constvaregex.Matches(tstr)
        For i = 0 To matches.Count - 1
            Dim varname As String = matches(i).Groups(1).Value

            Dim vDefine As New DefineVariable(varname, "", "", _Group)
            rVars.Add(vDefine)
        Next






        Return rVars
    End Function



    Public Function GetListFromEpScript(EpScript As String, Optional _FType As TriggerFunction.EFType = TriggerFunction.EFType.None, Optional _Group As String = "") As List(Of TriggerFunction)
        EpScript = vbCrLf & EpScript & vbCrLf

        Dim sr As New StringReader(EpScript)
        Dim ci As Integer = -1
        Dim c As Char

        Dim st As New Stack


        Dim sb As New StringBuilder
        While True
            ci = sr.Read()
            If ci = -1 Then
                Exit While
            End If

            c = ChrW(ci)


            Select Case c
                Case "{"
                    st.Push(c)
                    'sb.Append(c)
            End Select
            If st.Count = 0 Then
                sb.Append(c)
            End If
            Select Case c
                Case "}"
                    If st.Peek = "{" Then
                        st.Pop()
                        'sb.Append(c)
                    Else
                        '오류
                        Exit While
                    End If
            End Select
        End While

        sr.Close()

        EpScript = sb.ToString



        Dim rFunc As New List(Of TriggerFunction)


        Dim tooltipregex As New Regex("(\/\*\*\*[^\/]*\*\*\*\/\s+)function\s+([\w_]+)\(([\/\*\w\,_\:\s]*)\)[^\;]")
        Dim normalregex As New Regex("function\s+([\w_]+)\(([\/\*\w\,_\:\s]*)\)[^\;]")


        Dim matches As MatchCollection
        matches = tooltipregex.Matches(EpScript)
        For i = matches.Count - 1 To 0 Step -1
            Dim tooltips As String = matches(i).Groups(1).Value
            Dim funcname As String = matches(i).Groups(2).Value
            Dim args As String = matches(i).Groups(3).Value

            Dim tfun As TriggerFunction = TriggerFunction.GetFunctionFromStr(tooltips, funcname, args)
            If _FType <> TriggerFunction.EFType.None Then
                tfun.FType = _FType
            End If
            If _Group <> "" Then
                tfun.FGroup = _Group
            End If



            If tfun IsNot Nothing Then
                tfun.SetArgComment()
                rFunc.Add(tfun)
            End If

            EpScript = Mid(EpScript, 1, matches(i).Index) & Mid(EpScript, matches(i).Index + matches(i).Length)
        Next

        matches = normalregex.Matches(EpScript)
        For i = matches.Count - 1 To 0 Step -1
            Dim tooltips As String = ""
            Dim funcname As String = matches(i).Groups(1).Value
            Dim args As String = matches(i).Groups(2).Value

            Dim tfun As TriggerFunction = TriggerFunction.GetFunctionFromStr(tooltips, funcname, args)
            If _FType <> TriggerFunction.EFType.None Then
                tfun.FType = _FType
            End If
            If _Group <> "" Then
                tfun.FGroup = _Group
            End If

            If tfun IsNot Nothing Then
                rFunc.Add(tfun)
            End If
        Next

        rFunc.Sort(Function(x As TriggerFunction, y As TriggerFunction)
                       Return x.FName.CompareTo(y.FName)
                   End Function)

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
                    If lan <> pgData.Setting(ProgramData.TSetting.Language) Then
                        IsType = False
                        IsSummary = False
                        IsParam = False
                        IsGroup = False
                        Continue For
                    End If

                    IsType = False
                    IsSummary = True
                    IsParam = False
                    IsGroup = False

                    rTrg.FSummary = ""
                Case "@param"
                    lan = tstrtype.Last
                    If lan <> pgData.Setting(ProgramData.TSetting.Language) Then
                        IsType = False
                        IsSummary = False
                        IsParam = False
                        IsGroup = False
                        Continue For
                    End If
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
                        rTrg.FGroup = tstr
                    End If

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
        None
    End Enum

    Public FType As EFType
    Public FSummary As String = ""
    Public FName As String = ""

    Public FGroup As String = ""

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
