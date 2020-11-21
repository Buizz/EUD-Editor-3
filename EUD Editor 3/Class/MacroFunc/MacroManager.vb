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
        lua.RegisterFunction("GetEUDScoreOffset", Me, Me.GetType().GetMethod("GetEUDScoreOffset"))
        lua.RegisterFunction("GetSupplyOffset", Me, Me.GetType().GetMethod("GetSupplyOffset"))


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
    Public Function MacroApply(str As String, isMain As Boolean) As String
        ErrorMsg = ""

        If str Is Nothing Then
            str = ""
        End If


        '스트링을 받아서 Regex를 통해 루아함수를 찾는다.
        Dim rstr As String = str
        'global extended singleline multiline
        Dim regex As New Regex("(<\?|<\?php)(.+?)\?>", RegexOptions.Multiline Or RegexOptions.Singleline Or RegexOptions.ExplicitCapture)
        Dim matches As MatchCollection = regex.Matches(str)

        preDefineStr.Clear()
        For i = 0 To matches.Count - 1

            luaReturnstr = ""

            Dim truestr As String = matches(i).Value
            Dim mstr As String = Mid(truestr, 3, truestr.Length - 4)
            'Lua프로세싱
            Try
                lua.DoString(mstr)
            Catch ex As Exception
                ErrorMsg = ex.ToString
                MsgBox(ErrorMsg)
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