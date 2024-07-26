Imports System.IO
Imports System.Text.RegularExpressions
Imports LuaInterface

Public Class MacroManager
    Private lua As Lua

    Public Shared ReadOnly Property LuaFloderPath As String
        Get
            If Not My.Computer.FileSystem.DirectoryExists(System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua\TriggerEditor") Then
                If Not My.Computer.FileSystem.DirectoryExists(System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua") Then
                    My.Computer.FileSystem.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua")
                End If
                My.Computer.FileSystem.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua\TriggerEditor")
            End If

            Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua\TriggerEditor"
        End Get
    End Property

    Public Shared ReadOnly Property SCAFloderPath As String
        Get
            If Not My.Computer.FileSystem.DirectoryExists(System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua\SCAScript") Then
                If Not My.Computer.FileSystem.DirectoryExists(System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua") Then
                    My.Computer.FileSystem.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua")
                End If
                My.Computer.FileSystem.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua\SCAScript")
            End If

            Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua\SCAScript"
        End Get
    End Property

    Public FunctionList As List(Of LuaFunction)
    Public ReadOnly Property GetFunction(name As String) As LuaFunction
        Get
            For i = 0 To FunctionList.Count - 1
                If FunctionList(i).Fname = name Then
                    Return FunctionList(i)
                End If
            Next
            Return Nothing
        End Get
    End Property
    Public Sub New()
        lua = New Lua()
        FunctionList = New List(Of LuaFunction)


        lua.RegisterFunction("preDefine", Me, Me.GetType().GetMethod("preDefine"))
        FunctionList.Add(New LuaFunction("preDefine", "값을 미리 선언합니다.", "내부함수", "str", "str"))

        lua.RegisterFunction("onPluginText", Me, Me.GetType().GetMethod("onPluginText"))
        FunctionList.Add(New LuaFunction("onPluginText", "메인 함수의 onPluginExec에 내용을 추가합니다.", "내부함수", "str", "str"))

        lua.RegisterFunction("beforeText", Me, Me.GetType().GetMethod("beforeText"))
        FunctionList.Add(New LuaFunction("beforeText", "메인 함수의 beforeText에 내용을 추가합니다.", "내부함수", "str", "str"))

        lua.RegisterFunction("afterText", Me, Me.GetType().GetMethod("afterText"))
        FunctionList.Add(New LuaFunction("afterText", "메인 함수의 afterText에 내용을 추가합니다.", "내부함수", "str", "str"))

        lua.RegisterFunction("echo", Me, Me.GetType().GetMethod("echo"))
        FunctionList.Add(New LuaFunction("echo", "값을 반환합니다.", "내부함수", "str", "str"))


        lua.RegisterFunction("GetBGMIndex", Me, Me.GetType().GetMethod("GetBGMIndex"))
        FunctionList.Add(New LuaFunction("GetBGMIndex", "사운드 인덱스를 반환합니다.", "내부함수", "bgmname", "BGM"))


        lua.RegisterFunction("GetReturnBGMIndex", Me, Me.GetType().GetMethod("GetReturnBGMIndex"))
        FunctionList.Add(New LuaFunction("GetReturnBGMIndex", "사운드 인덱스를 반환합니다.", "내부함수", "bgmname", "BGM"))


        lua.RegisterFunction("ParseUnit", Me, Me.GetType().GetMethod("ParseUnit"))
        lua.RegisterFunction("ParseWeapon", Me, Me.GetType().GetMethod("ParseWeapon"))
        lua.RegisterFunction("ParseFlingy", Me, Me.GetType().GetMethod("ParseFlingy"))
        lua.RegisterFunction("ParseSprites", Me, Me.GetType().GetMethod("ParseSprites"))
        lua.RegisterFunction("ParseImages", Me, Me.GetType().GetMethod("ParseImages"))
        lua.RegisterFunction("ParseUpgrades", Me, Me.GetType().GetMethod("ParseUpgrades"))
        lua.RegisterFunction("ParseTechdata", Me, Me.GetType().GetMethod("ParseTechdata"))
        lua.RegisterFunction("ParseOrders", Me, Me.GetType().GetMethod("ParseOrders"))
        lua.RegisterFunction("ParseLocation", Me, Me.GetType().GetMethod("ParseLocation"))
        lua.RegisterFunction("ParseSwitchName", Me, Me.GetType().GetMethod("ParseSwitchName"))
        lua.RegisterFunction("ParseEUDScore", Me, Me.GetType().GetMethod("ParseEUDScore"))
        lua.RegisterFunction("ParseSupplyType", Me, Me.GetType().GetMethod("ParseSupplyType"))
        lua.RegisterFunction("ParseSCAScript", Me, Me.GetType().GetMethod("ParseSCAScript"))
        lua.RegisterFunction("ParseSCAScriptVariable", Me, Me.GetType().GetMethod("ParseSCAScriptVariable"))


        lua.RegisterFunction("GetEUDScoreOffset", Me, Me.GetType().GetMethod("GetEUDScoreOffset"))
        lua.RegisterFunction("GetSupplyOffset", Me, Me.GetType().GetMethod("GetSupplyOffset"))


        lua.RegisterFunction("AddMouseEvent", Me, Me.GetType().GetMethod("AddMouseEvent"))
        lua.RegisterFunction("AddMSQCPlugin", Me, Me.GetType().GetMethod("AddMSQCPlugin"))
        lua.RegisterFunction("AddChatEventPlugin", Me, Me.GetType().GetMethod("AddChatEventPlugin"))


        lua.RegisterFunction("IsNumber", Me, Me.GetType().GetMethod("IsNumber"))



        lua.RegisterFunction("DatOffset", Me, Me.GetType().GetMethod("DatOffset"))
        lua.RegisterFunction("GetDatFile", Me, Me.GetType().GetMethod("GetDatFile"))
        lua.RegisterFunction("SetDatFile", Me, Me.GetType().GetMethod("SetDatFile"))
        lua.RegisterFunction("ConditionDatFile", Me, Me.GetType().GetMethod("ConditionDatFile"))


        For Each files As String In My.Computer.FileSystem.GetFiles(LuaFloderPath)
            If files.IndexOf(".lua") <> -1 Then
                Try
                    lua.DoFile(files)
                Catch ex As Exception
                    Tool.ErrorMsgBox("TE Lua Init Error: " & files, ex.Message)
                End Try

                Dim fs As New FileStream(files, FileMode.Open)
                Dim sr As New StreamReader(fs)

                Dim fileText As String = sr.ReadToEnd
                'Dim regex As New Regex("function\s+([a-zA-Z_0-9]+)\(([a-zA-Z_0-9,. ]*)\)\s+--(.*)\/(.*)\/(.*)")

                'Dim matchs As MatchCollection = regex.Matches(fileText)

                ''MsgBox(files & vbCrLf & matchs.Count)
                'For i = 0 To matchs.Count - 1
                '    Dim fname As String = matchs(i).Groups(1).Value
                '    Dim args As String = matchs(i).Groups(2).Value
                '    Dim GrpupText As String = matchs(i).Groups(3).Value
                '    Dim argType As String = matchs(i).Groups(4).Value
                '    Dim comment As String = matchs(i).Groups(5).Value

                '    'MsgBox(comment)

                '    FunctionList.Add(New LuaFunction(fname, comment, GrpupText, args, argType))
                'Next


                Dim tooltipregex As New Regex("--\[================================\[\s([^=]*)\s\]================================\]\s+function\s+([a-zA-Z_0-9]+)\(([a-zA-Z_0-9,. ]*)\)")
                Dim normalregex As New Regex("^function\s+([a-zA-Z_0-9]+)\(([a-zA-Z_0-9,. ]*)\)")


                Dim matches As MatchCollection
                matches = tooltipregex.Matches(fileText)
                For i = matches.Count - 1 To 0 Step -1
                    Dim tooltips As String = matches(i).Groups(1).Value
                    Dim funcname As String = matches(i).Groups(2).Value
                    Dim args As String = matches(i).Groups(3).Value

                    FunctionList.Add(New LuaFunction(funcname, args, tooltips))

                    fileText = Mid(fileText, 1, matches(i).Index) & Mid(fileText, matches(i).Index + matches(i).Length)
                Next

                matches = normalregex.Matches(fileText)
                For i = matches.Count - 1 To 0 Step -1
                    Dim tooltips As String = ""
                    Dim funcname As String = matches(i).Groups(1).Value
                    Dim args As String = matches(i).Groups(2).Value

                    FunctionList.Add(New LuaFunction(funcname, args, tooltips))
                Next



                sr.Close()
                fs.Close()
            End If
        Next
    End Sub









    Public ErrorMsg As String
    Public Function MacroApply(str As String, isMain As Boolean, Optional filename As String = "") As String
        ErrorMsg = ""

        If str Is Nothing Then
            str = ""
        End If
        lua.DoString("LuaPlayerVariable = ""getcurpl()""")

        '스트링을 받아서 Regex를 통해 루아함수를 찾는다.
        Dim rstr As String = str


        '@KeyDown(P1, "`")
        Dim shortMacro As New List(Of String)

        Dim luaregex As New Regex("@[\w_]+[\w\d_]+", RegexOptions.Singleline)
        Dim luamatches As MatchCollection = luaregex.Matches(rstr)
        For i = 0 To luamatches.Count - 1


            Dim truestr As String = luamatches(i).Value
            Dim index As Integer = luamatches(i).Index + truestr.Length

            Dim funcname As String = truestr.Substring(1)

            Dim funccontent As String = ""


            Dim braceCount As Integer = 0
            If rstr(index) <> "(" Then
                Continue For
            End If

            While True
                If rstr.Length <= index Then
                    Exit While
                End If

                If rstr(index) = "(" Then
                    braceCount += 1
                End If

                If rstr(index) = ")" Then
                    braceCount -= 1
                End If

                funccontent = funccontent & rstr(index)

                If braceCount = 0 Then
                    Exit While
                End If
                index += 1
            End While

            shortMacro.Add(funcname & funccontent)
        Next

        For Each t In shortMacro
            Dim ctext As String = t

            Dim braceCount As Integer = 0
            Dim Issentence As Boolean = False
            Dim newtext As String = ctext

            Dim index As Integer = 0

            Dim args As New List(Of String)

            Dim funcname As String = ""
            While newtext(index) <> "("
                funcname = funcname & newtext(index)
                If newtext.Length <= index Then
                    Exit While
                End If

                index += 1
            End While

            Dim argcontent As String = ""
            While True
                If newtext.Length <= index Then
                    Exit While
                End If

                '문자열 감지
                If braceCount >= 1 And newtext(index) = """" Then
                    If Issentence Then
                        If Not (index > 0 And newtext(index - 1) = "\") Then
                            Issentence = False
                        End If
                    Else
                        Issentence = True
                    End If
                End If


                If Not Issentence Then
                    If newtext(index) = "(" Then
                        braceCount += 1
                        If (braceCount = 1) Then
                            '첫 루프는 로직통과
                            index += 1
                            Continue While
                        End If
                    End If


                    If newtext(index) = ")" Then
                        braceCount -= 1
                        If braceCount = 0 Then
                            args.Add(argcontent.Trim())
                            Exit While
                        End If
                    End If


                    If braceCount = 1 Then
                        '밖일 경우
                        If newtext(index) = "," Then
                            args.Add(argcontent.Trim())
                            argcontent = ""
                        End If
                    End If
                End If



                If Issentence Then
                    argcontent = argcontent & newtext(index)
                Else
                    'braceCount 0, 1일 경우?
                    If braceCount = 1 Then
                        If (newtext(index) <> ",") Then
                            argcontent = argcontent & newtext(index)
                        End If
                    ElseIf braceCount = 1 Then
                        If (newtext(index) <> "(") And (newtext(index) <> ")") And (newtext(index) <> ",") Then
                            argcontent = argcontent & newtext(index)
                        End If
                    Else
                        argcontent = argcontent & newtext(index)
                    End If
                End If





                index += 1
            End While

            Dim rctext As String = ""
            For Each s In args
                Dim quote As Boolean = False
                If s = "" Then
                    rctext = funcname & "("
                    Exit For
                End If

                If s.Length <= 2 Then
                    '2보다 커야 ""로 덮힌것.
                    quote = True
                End If

                If s.Substring(0, 1) <> """" And s.Substring(s.Length - 1, 1) <> """" Then
                    quote = True
                End If

                If rctext <> "" Then
                    rctext = rctext & ", "
                Else
                    rctext = funcname & "("
                End If

                If quote Then
                    If s.First() = "@" Then
                        s = s.Substring(1)
                        quote = False
                    End If
                End If


                If quote Then
                    rctext = rctext & """" & s & """"
                Else
                    rctext = rctext & s
                End If
            Next
            rctext = rctext & ")"


            rstr = Replace(rstr, "@" & ctext, "<?" & rctext & "?>", 1, 1)
        Next





        'global extended singleline multiline
        Dim regex As New Regex("(<\?|<\?php)(.+?)\?>", RegexOptions.Multiline Or RegexOptions.Singleline Or RegexOptions.ExplicitCapture)
        Dim matches As MatchCollection = regex.Matches(rstr)

        preDefineStr.Clear()
        For i = 0 To matches.Count - 1

            luaReturnstr = ""

            Dim truestr As String = matches(i).Value
            Dim mstr As String = Mid(truestr, 3, truestr.Length - 4)
            'Lua프로세싱


            Try
                lua.DoString(mstr)
            Catch ex As Exception
                Dim line As Integer = -1
                Try
                    line = rstr.Substring(0, matches(i).Index).Replace(vbLf, vbCrLf).Replace(vbCr, vbCrLf).Split(vbCrLf).Count - 2
                Catch ttt As Exception

                End Try

                ErrorMsg = ex.ToString
                If filename <> "" Then
                    ErrorMsg = "==============================================================================" & vbCrLf &
                        "File : " & filename & vbCrLf &
                        "Line " & line & " : " & mstr + " <-" & vbCrLf &
                        "==============================================================================" & vbCrLf &
                        ErrorMsg
                Else
                    ErrorMsg = "==============================================================================" & vbCrLf &
                        "Line " & line & " : " & mstr + " <-" & vbCrLf &
                        "==============================================================================" & vbCrLf &
                        ErrorMsg
                End If


                macroErrorList.Add(ErrorMsg)

                'Tool.ErrorMsgBox("Lua스크립트오류", ErrorMsg)
                'MsgBox(ErrorMsg)
                Continue For
            End Try


            rstr = Replace(rstr, truestr, luaReturnstr, 1, 1)
        Next

        For i = 0 To preDefineStr.Count - 1
            rstr = preDefineStr(i) & vbCrLf & rstr
        Next


        '만약 main파일일 경우
        If isMain Then
            If True Then
                Dim temp As String = ""

                For i = 0 To onpluginStr.Count - 1
                    temp = temp & vbTab & onpluginStr(i) & vbCrLf
                Next

                Dim tregex As New Regex("function\s*onPluginStart\(\s*\)\s*\{")
                Dim tmatch As Match = tregex.Match(rstr)
                If tmatch.Success Then
                    Dim startindex As Integer = tmatch.Index
                    Dim vallen As Integer = tmatch.Value.Length

                    rstr = rstr.Insert(startindex + vallen, vbCrLf & temp)

                Else
                    'onPluginStart가 없을 경우 맨 밑에 새로 추가.
                    rstr = rstr & "function onPluginStart(){" & vbCrLf
                    rstr = rstr & temp & vbCrLf
                    rstr = rstr & "}" & vbCrLf
                End If
            End If

            If True Then
                Dim temp As String = ""

                For i = 0 To beforeStr.Count - 1
                    temp = temp & vbTab & beforeStr(i) & vbCrLf
                Next

                Dim tregex As New Regex("function\s*beforeTriggerExec\(\s*\)\s*\{")
                Dim tmatch As Match = tregex.Match(rstr)
                If tmatch.Success Then
                    Dim startindex As Integer = tmatch.Index
                    Dim vallen As Integer = tmatch.Value.Length

                    rstr = rstr.Insert(startindex + vallen, vbCrLf & temp)
                Else
                    'onPluginStart가 없을 경우 맨 밑에 새로 추가.
                    rstr = rstr & "function beforeTriggerExec(){" & vbCrLf
                    rstr = rstr & temp & vbCrLf
                    rstr = rstr & "}" & vbCrLf
                End If
            End If
            If True Then
                Dim temp As String = ""

                For i = 0 To afterStr.Count - 1
                    temp = temp & vbTab & afterStr(i) & vbCrLf
                Next

                Dim tregex As New Regex("function\s*afterTriggerExec\(\s*\)\s*\{")
                Dim tmatch As Match = tregex.Match(rstr)
                If tmatch.Success Then
                    Dim startindex As Integer = tmatch.Index
                    Dim vallen As Integer = tmatch.Value.Length

                    rstr = rstr.Insert(startindex + vallen, vbCrLf & temp)
                Else
                    'onPluginStart가 없을 경우 맨 밑에 새로 추가.
                    rstr = rstr & "function afterTriggerExec(){" & vbCrLf
                    rstr = rstr & temp & vbCrLf
                    rstr = rstr & "}" & vbCrLf
                End If
            End If
        End If




        Return rstr
    End Function


    Public Class LuaFunction
        Public LuaGroup As String = ""
        Public Fname As String = ""
        Public Fcomment As String = ""
        Public ArgName As List(Of String)
        Public ArgType As List(Of String)
        Public ArgSummary As List(Of String)

        Public ArgLists As List(Of ArgBlock)

        Public IsCompleteFunction As Boolean = False



        Public Sub New(_fname As String, _fargs As String, _tooltip As String)
            ArgName = New List(Of String)
            ArgType = New List(Of String)
            ArgLists = New List(Of ArgBlock)
            ArgSummary = New List(Of String)



            Fname = _fname.Trim

            If _fargs.Trim <> "" Then
                Dim _t() As String = _fargs.Split(",")
                For i = 0 To _t.Count - 1
                    Dim targ As String = _t(i).Trim

                    ArgName.Add(targ)
                    ArgSummary.Add(targ)
                    ArgType.Add(targ)
                Next
            End If


            Dim cLan As String = pgData.Setting(ProgramData.TSetting.Language)


            Dim tooltips() As String = _tooltip.Split(vbCrLf)

            Dim CollectLan As Boolean = False

            Dim commandType As Integer = 0
            Dim tparam As String = ""
            Dim targtype As String = ""


            Dim LastMsg As String = ""
            For i = 0 To tooltips.Count - 1
                Dim line As String = tooltips(i).Trim

                If line.Count = 0 Then
                    Continue For
                End If

                If line(0) = "@" Then
                    '커맨드

                    Dim commands() As String = line.Split(".")

                    Select Case commands.First
                        Case "@Language"
                            LastMsg = ""
                            commandType = 0
                            Dim tLan As String = commands(1)
                            If tLan = cLan Then
                                CollectLan = True
                            Else
                                CollectLan = False
                            End If
                        Case "@Summary"
                            LastMsg = ""
                            If CollectLan Then
                                commandType = 1
                            End If
                        Case "@Group"
                            LastMsg = ""
                            If CollectLan Then
                                commandType = 2
                            End If

                        Case "@param"
                            LastMsg = ""
                            If CollectLan Then
                                commandType = 3
                                tparam = commands(1)
                                targtype = commands(2)
                                Dim argindex As Integer = ArgName.IndexOf(tparam)
                                ArgType(argindex) = targtype
                                ArgSummary(argindex) = ""
                            End If
                    End Select


                    Continue For
                End If
                LastMsg = LastMsg & line & vbCrLf
                Select Case commandType
                    Case 1
                        Fcomment = LastMsg.Trim
                    Case 2
                        LuaGroup = LastMsg.Trim
                    Case 3
                        Dim argindex As Integer = ArgName.IndexOf(tparam)
                        ArgType(argindex) = targtype
                        ArgSummary(argindex) = LastMsg.Trim
                End Select
            Next

            '@Language.ko-KR
            '@Summary
            '채팅 [Chat]를 인식합니다.
            '@Group
            '채팅인식
            '@param.Player.TrgPlayer
            '대상 플레이어입니다.
            '@param.Chat.TrgString
            '인식할 채팅입니다.




            'Fcomment = _fcomment.Trim
            'LuaGroup = _group.Trim




            Dim tstr As String = Fcomment
            Dim vcount As Integer = 0
            If tstr IsNot Nothing Then
                For i = 0 To ArgName.Count - 1
                    If tstr.IndexOf("[" & ArgName(i).Trim & "]") <> -1 Then
                        tstr = tstr.Replace("[" & ArgName(i).Trim & "]", "*$" & i & "*")


                        vcount += 1
                    End If
                Next
            End If


            If vcount = ArgName.Count Then
                IsCompleteFunction = True
            Else
                Return
            End If

            If tstr IsNot Nothing Then
                Dim tstrlist() As String = tstr.Split("*")
                For i = 0 To tstrlist.Count - 1
                    If tstrlist(i).Trim <> "" Then
                        If tstrlist(i)(0) = "$" Then
                            '숫자
                            Dim n As String = Mid(tstrlist(i), 2, tstrlist(i).Length - 1)

                            If IsNumeric(n) Then
                                ArgLists.Add(New ArgBlock(ArgBlock.BlockType.Arg, n, ""))
                            Else
                                ArgLists.Add(New ArgBlock(ArgBlock.BlockType.Label, -1, tstrlist(i)))
                            End If
                        Else
                            '문자
                            ArgLists.Add(New ArgBlock(ArgBlock.BlockType.Label, -1, tstrlist(i)))
                        End If
                    End If
                Next
            End If
        End Sub


        Public Sub New(_fname As String, _fcomment As String, _group As String, _fargs As String, _fargtype As String)
            ArgName = New List(Of String)
            ArgType = New List(Of String)
            ArgLists = New List(Of ArgBlock)
            ArgSummary = New List(Of String)

            Fname = _fname.Trim
            Fcomment = _fcomment.Trim
            LuaGroup = _group.Trim


            If _fargs.Trim <> "" Then
                ArgName.AddRange(_fargs.Split(","))
            End If


            If _fargtype.Trim <> "" Then
                ArgType.AddRange(_fargtype.Split(","))
                ArgSummary.AddRange(_fargtype.Split(","))
            End If





            Dim tstr As String = Fcomment
            Dim vcount As Integer = 0
            For i = 0 To ArgName.Count - 1

                If tstr.IndexOf("[" & ArgName(i).Trim & "]") <> -1 Then
                    tstr = tstr.Replace("[" & ArgName(i).Trim & "]", "*$" & i & "*")


                    vcount += 1
                End If
            Next

            If vcount = ArgName.Count Then
                IsCompleteFunction = True
            Else
                Return
            End If

            Dim tstrlist() As String = tstr.Split("*")

            For i = 0 To tstrlist.Count - 1
                If tstrlist(i).Trim <> "" Then
                    If tstrlist(i)(0) = "$" Then
                        '숫자
                        Dim n As String = Mid(tstrlist(i), 2, tstrlist(i).Length - 1)

                        If IsNumeric(n) Then
                            ArgLists.Add(New ArgBlock(ArgBlock.BlockType.Arg, n, ""))
                        Else
                            ArgLists.Add(New ArgBlock(ArgBlock.BlockType.Label, -1, tstrlist(i)))
                        End If
                    Else
                        '문자
                        ArgLists.Add(New ArgBlock(ArgBlock.BlockType.Label, -1, tstrlist(i)))
                    End If
                End If
            Next
        End Sub


        Public Structure ArgBlock
            Public Enum BlockType
                Label
                Arg
            End Enum
            Public BType As BlockType
            Public ArgIndex As Integer
            Public Label As String
            Public Sub New(_BType As BlockType, _ArgIndex As Integer, _Label As String)
                BType = _BType
                ArgIndex = _ArgIndex
                Label = _Label.Trim
            End Sub
        End Structure
    End Class
End Class