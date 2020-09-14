Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Security.AccessControl
Imports LuaInterface
Imports Microsoft.DwayneNeed.Win32.Gdi32

<Serializable>
Public Class ScriptBlock
    Public Enum EBlockType
        none = -1

        constVal

        action
        condition

        plibfun
        macrofun
        funuse
        externfun

        fundefine
        funargs
        funargblock

        funcontent

        _if
        ifcondition
        ifthen
        _elseif
        ifelse

        _for
        foraction

        _while
        whilecondition
        whileaction

        switch
        switchvar
        switchcase

        vardefine
        varuse
        externvaruse

        folder
        folderaction

        objectdefine
        objectfields
        objectmethod

        rawcode
        import
        _or
        _and
        exp
        sign

        funreturn
        break
    End Enum

    Public ScriptType As EBlockType
    Public isfolder As Boolean
    Public IsDeleteAble As Boolean


    Public name As String
    Public isexpand As Boolean
    Public flag As Boolean
    Public flag2 As Boolean = False
    Public value As String
    Public value2 As String

    <NonSerialized>
    Public tobject As Object



    Public child As List(Of ScriptBlock)

    Public Parent As ScriptBlock

    <NonSerialized>
    Public Scripter As GUIScriptEditor

    Public Sub AddChild(scr As ScriptBlock)
        scr.Parent = Me
        child.Add(scr)
    End Sub

    Public Sub ReplaceChild(scr As ScriptBlock, index As Integer)
        If child.Count = index Then
            AddChild(scr)
            Return
        ElseIf child.Count > index Then
            child.RemoveAt(index)
            InsertChild(index, scr)
        End If
    End Sub

    Public Sub InsertChild(i As Integer, scr As ScriptBlock)
        scr.Parent = Me
        child.Insert(i, scr)
    End Sub

    Public Sub New(_ScriptType As EBlockType, tname As String, tisexpand As Boolean, tflag As Boolean, tvalue As String, tScripter As GUIScriptEditor, Optional ValueRefresh As Boolean = True)
        name = tname
        isexpand = tisexpand
        flag = tflag
        ScriptType = _ScriptType

        value = tvalue
        Scripter = tScripter

        child = New List(Of ScriptBlock)



        Select Case ScriptType
            Case EBlockType.ifcondition, EBlockType.ifthen, EBlockType.ifelse,
                 EBlockType.funargs, EBlockType.whilecondition, EBlockType.whileaction,
                EBlockType.switchcase, EBlockType._or, EBlockType._and,
                 EBlockType.folderaction, EBlockType.funcontent, EBlockType.foraction,
                 EBlockType.objectfields, EBlockType.objectmethod
                isfolder = True
            Case Else
                isfolder = False
        End Select



        Select Case ScriptType
            Case EBlockType.funargs, EBlockType.funcontent, EBlockType.ifcondition,
                   EBlockType.ifthen, EBlockType.ifelse, EBlockType.foraction,
                 EBlockType.whilecondition, EBlockType.whileaction, EBlockType.folderaction,
                 EBlockType.objectfields, EBlockType.objectmethod
                IsDeleteAble = False
            Case Else
                IsDeleteAble = True
        End Select



        Select Case ScriptType
            Case EBlockType.constVal
                If name = "Variable" Then
                    ScriptType = EBlockType.varuse
                    name = "init"
                    value = "init"
                End If
            Case EBlockType.fundefine
                AddChild(New ScriptBlock(EBlockType.funargs, "funargs", False, False, "", Scripter))
                AddChild(New ScriptBlock(EBlockType.funcontent, "funcontent", False, False, "", Scripter))
            Case EBlockType._if
                AddChild(New ScriptBlock(EBlockType.ifcondition, "ifcondition", False, False, "", Scripter))
                AddChild(New ScriptBlock(EBlockType.ifthen, "ifthen", False, False, "", Scripter))
            Case EBlockType._elseif
                AddChild(New ScriptBlock(EBlockType.ifcondition, "ifcondition", False, False, "", Scripter))
                AddChild(New ScriptBlock(EBlockType.ifthen, "ifthen", False, False, "", Scripter))
            Case EBlockType._for
                value = "defaultvalue"
                AddChild(New ScriptBlock(EBlockType.foraction, "foraction", False, False, "", Scripter))
            Case EBlockType._while
                AddChild(New ScriptBlock(EBlockType.whilecondition, "whilecondition", False, False, "", Scripter))
                AddChild(New ScriptBlock(EBlockType.whileaction, "whileaction", False, False, "", Scripter))
            Case EBlockType.switch
                value = "defaultvalue"
                'AddChild(New ScriptBlock(EBlockType.constVal, "defaultvalue;init", False, False, "defaultvalue;init", Scripter))
            Case EBlockType.folder
                AddChild(New ScriptBlock(EBlockType.folderaction, "folderaction", False, False, "", Scripter))
            Case EBlockType.objectdefine
                AddChild(New ScriptBlock(EBlockType.objectfields, "objectfields", False, False, "", Scripter))
                AddChild(New ScriptBlock(EBlockType.objectmethod, "objectmethod", False, False, "", Scripter))
            Case EBlockType.vardefine
                value2 = "var"
                'AddChild(New ScriptBlock("", False, False, "defaultvalue;init", Scripter))
            Case EBlockType.plibfun, EBlockType.funuse, EBlockType.externfun, EBlockType.action, EBlockType.condition, EBlockType.macrofun
                If ValueRefresh Then
                    RefreshValue()
                End If
            Case EBlockType.funreturn
                AddChild(New ScriptBlock(EBlockType.constVal, "defaultvalue;init", False, False, "defaultvalue;init", Scripter))
        End Select
    End Sub

    Public Sub SetFuncTooltip(str As String)
        For i = 0 To child.Count - 1
            If child(i).ScriptType = EBlockType.funargs Then
                child(i).value = str
            End If
        Next
    End Sub
    Public Function GetFuncTooltip() As String
        For i = 0 To child.Count - 1
            If child(i).ScriptType = EBlockType.funargs Then
                Return child(i).value
            End If
        Next
        Return ""
    End Function



    Public Function GetFuncArgsCount() As Integer
        Dim valuecount As Integer = -1

        Select Case ScriptType
            Case EBlockType.funuse
                Dim tscr As ScriptBlock = tescm.GetFuncInfor(name, Scripter)
                If tscr IsNot Nothing Then
                    valuecount = tescm.GetFuncArgs(tscr).Count
                End If
            Case EBlockType.externfun
                Dim _namespace As String = name.Split(".").First
                Dim funcname As String = name.Split(".").Last

                For i = 0 To Scripter.ExternFile.Count - 1

                    If _namespace = Scripter.ExternFile(i).nameSpaceName Then
                        Dim j As Integer = Scripter.ExternFile(i).Funcs.SearchFunc(funcname)
                        If j >= 0 Then
                            valuecount = Scripter.ExternFile(i).Funcs.GetFuncArgument(j).Split(",").Count
                        End If
                    End If
                Next
            Case EBlockType.macrofun
                Return macro.GetFunction(name).ArgName.Count
            Case Else
                Dim i As Integer = Tool.TEEpsDefaultFunc.SearchFunc(name)
                If i >= 0 Then
                    valuecount = Tool.TEEpsDefaultFunc.GetFuncArgument(i).Split(",").Count
                End If
        End Select

        Return valuecount
    End Function


    Private Sub lamdaRefreshValue(functooltip As FunctionToolTip, argument As String)
        If argument.Trim <> "" Then
            Dim args() As String = argument.Split(",")
            For i = 0 To args.Length - 1
                If child.Count <= i Then
                    Dim vname As String
                    Dim vtype As String

                    Dim argtext As String = args(i)



                    If argtext.IndexOf("/*") <> -1 And argtext.IndexOf("*/") <> -1 And argtext.IndexOf(":") = -1 Then
                        argtext = argtext.Replace("*/", "")
                        argtext = argtext.Replace("/*", ":")
                    End If

                    Dim tstr() As String = argtext.Split(":")

                    vname = "defaultvalue;" & tstr.First.Trim
                    vtype = tstr.Last.Trim

                    If tstr.First.Trim = "WAVName" And vtype = "TrgString" Then
                        vtype = "WAVName"
                    End If

                    'ReplaceChild(New ScriptBlock(vtype, False, False, vname, Scripter), i)

                    AddChild(New ScriptBlock(EBlockType.constVal, vtype, False, False, vname, Scripter))
                End If
            Next
        End If
    End Sub
    Public Sub RefreshValue()
        Dim FuncDefine As New FuncDefine(Me)

        For i = 0 To FuncDefine.Args.Count - 1
            If child.Count <= i Then
                FuncDefine.ResetScriptBlock(i, Me)
            End If
        Next
        Return

        Dim curentvals As Integer = child.Count
        If name.IndexOf(".") = -1 Then
            If ScriptType = EBlockType.macrofun Then
                Dim luafunc As MacroManager.LuaFunction = macro.GetFunction(name)

                For i = 0 To luafunc.ArgName.Count - 1
                    If child.Count <= i Then
                        Dim vname As String = "defaultvalue;" & luafunc.ArgName(i).Trim
                        Dim vtype As String = luafunc.ArgType(i).Trim

                        'ReplaceChild(New ScriptBlock(vtype, False, False, vname, Scripter), i)


                        Dim tscr As New ScriptBlock(EBlockType.constVal, vtype, False, False, vname, Scripter)
                        AddChild(tscr)
                    End If
                Next
            Else
                Dim index As Integer = Tool.TEEpsDefaultFunc.SearchFunc(name)

                If index >= 0 Then
                    Dim functooltip As FunctionToolTip = Tool.TEEpsDefaultFunc.GetFuncTooltip(index)
                    Dim argument As String = Tool.TEEpsDefaultFunc.GetFuncArgument(index)
                    lamdaRefreshValue(functooltip, argument)
                Else
                    Dim func As ScriptBlock = tescm.GetFuncInfor(name, Scripter)
                    'DEBUG
                    If func Is Nothing Then
                        If Not IsValue() Then
                            name = "funcisnotexist"
                        End If
                    Else
                        Dim args As List(Of ScriptBlock) = tescm.GetFuncArgs(func)


                        For i = 0 To args.Count - 1
                            If child.Count <= i Then
                                Dim vname As String = "defaultvalue;" & args(i).value.Trim
                                Dim vtype As String = args(i).name

                                'ReplaceChild(New ScriptBlock(vtype, False, False, vname, Scripter), i)

                                Dim tscr As New ScriptBlock(EBlockType.constVal, vtype, False, False, vname, Scripter)
                                tscr.value2 = args(i).value2
                                AddChild(tscr)
                            End If
                        Next

                    End If
                End If
            End If

        Else
            Dim _namespace As String = name.Split(".").First
            Dim funcname As String = name.Split(".").Last

            For i = 0 To Scripter.ExternFile.Count - 1
                If _namespace = Scripter.ExternFile(i).nameSpaceName Then
                    Dim index As Integer = Scripter.ExternFile(i).Funcs.SearchFunc(funcname)
                    If index >= 0 Then
                        Dim functooltip As FunctionToolTip = Scripter.ExternFile(i).Funcs.GetFuncTooltip(index)
                        Dim argument As String = Scripter.ExternFile(i).Funcs.GetFuncArgument(index)
                        lamdaRefreshValue(functooltip, argument)
                        Exit For
                    End If
                End If
            Next
        End If


        '만약 마지막 define된 arg의 이름이 *로 시작 할 경우

        'Dim funcargs As Integer = GetFuncArgsCount()
        'If funcargs <> -1 Then
        '    If funcargs < curentvals Then
        '        For i = 0 To curentvals - funcargs - 1
        '            child.RemoveAt(funcargs)
        '        Next
        '    End If
        'End If
    End Sub


    Private Sub AddText(Inline As InlineCollection, text As String, brush As Brush)
        Dim Run As New Run(text & " ")
        If brush IsNot Nothing Then
            Run.Foreground = brush
        End If
        Inline.Add(Run)
    End Sub

    Public Function ValueCoder() As String
        Dim rstr As String = ""


        Select Case ScriptType
            Case EBlockType.vardefine
                If flag Then
                    If value2 = "static" Then
                        rstr = "static "
                    Else
                        rstr = "const "
                    End If
                Else
                    rstr = "var "
                End If
                rstr = rstr & value

                If child.Count <> 0 Then
                    rstr = rstr & " = "

                    rstr = rstr & child(0).ValueCoder
                End If

            Case EBlockType.constVal

                Dim scdat As SCDatFiles.DatFiles
                If Not IsNumeric(value) Then
                    If value Is Nothing Then
                        value = ""
                    End If
                    Dim strs() As String = value.Trim.Split(";")
                    If strs.Length = 2 Then
                        If strs.First = "defaultvalue" Then
                            rstr = strs.Last
                        End If
                    Else
                        rstr = value.Trim
                    End If
                    Return rstr
                End If
                Select Case name
                    Case "Weapon"
                        scdat = SCDatFiles.DatFiles.weapons
                        rstr = pjData.CodeLabel(scdat, value)
                    Case "Flingy"
                        scdat = SCDatFiles.DatFiles.flingy
                        rstr = pjData.CodeLabel(scdat, value)
                    Case "Sprite"
                        scdat = SCDatFiles.DatFiles.sprites
                        rstr = pjData.CodeLabel(scdat, value)
                    Case "Image"
                        scdat = SCDatFiles.DatFiles.images
                        rstr = pjData.CodeLabel(scdat, value)
                    Case "Upgrade"
                        scdat = SCDatFiles.DatFiles.upgrades
                        rstr = pjData.CodeLabel(scdat, value)
                    Case "Tech"
                        scdat = SCDatFiles.DatFiles.techdata
                        rstr = pjData.CodeLabel(scdat, value)
                    Case "Order"
                        scdat = SCDatFiles.DatFiles.orders
                        rstr = pjData.CodeLabel(scdat, value)
                    Case Else
                        Dim strs() As String = value.Trim.Split(";")
                        If strs.Length = 2 Then
                            If strs.First = "defaultvalue" Then
                                rstr = strs.Last
                            End If
                        Else
                            rstr = value.Trim
                        End If
                End Select





            Case EBlockType.funuse, EBlockType.plibfun, EBlockType.externfun, EBlockType.macrofun
                rstr = "함수:" & name
            Case EBlockType.action
                rstr = "액션:" & name
            Case EBlockType.condition
                rstr = "조건:" & name
            Case EBlockType.rawcode
                rstr = value
            Case EBlockType.exp
                rstr = ""
                For i = 0 To child.Count - 1
                    If child(i).ScriptType = EBlockType.sign Then
                        rstr = rstr & " " & child(i).value & " "
                    Else
                        rstr = rstr & child(i).ValueCoder()
                    End If
                Next
            Case EBlockType.sign
                rstr = value
            Case EBlockType.varuse
                If value.Trim <> "" Then
                    If value = "!index" Then
                        rstr = name & "[" & child(0).ValueCoder() & "]"
                        Return rstr
                    End If
                    If value = "!cp" Then
                        rstr = name & "[해당플레이어]"
                        Return rstr
                    End If
                    If value = "!default" Then
                        rstr = name & ".ptr"
                        Return rstr
                    End If

                    rstr = name & "." & value

                    If value2 = "method" Then
                        rstr = rstr & "("
                        For i = 0 To child.Count - 1
                            If i <> 0 Then
                                rstr = rstr & ","
                            End If
                            rstr = rstr & child(i).ValueCoder()
                        Next
                        rstr = rstr & ")"
                    Else
                        Select Case value
                            Case "constructor", "cast", "alloc"
                                If value = "constructor" Then
                                    rstr = name
                                End If

                                rstr = rstr & "("
                                For i = 0 To child.Count - 1
                                    If i <> 0 Then
                                        rstr = rstr & ","
                                    End If
                                    rstr = rstr & child(i).ValueCoder()
                                Next
                                rstr = rstr & ")"
                        End Select
                    End If
                Else
                    rstr = name
                End If
        End Select

        Return rstr
    End Function

    Public Function ForScriptCoder() As String
        'ᗢ

        Dim ForType As String = value.Split("ᚢ").First
        Dim vvalue As String = value.Split("ᚢ").Last

        Dim rvalue As String = ""
        Select Case ForType
            Case "UserEdit"
                Dim v() As String = vvalue.Split("ᗢ")


                rvalue = v(0).ToLower & "(" & v(1) & ")"
            Case "CountRepeat"
                Dim v() As String = vvalue.Split("ᗢ")

                If v.Count = 3 Then
                    Dim vname As String = v(0)
                    Dim vinit As String = v(1)
                    Dim vcount As String = v(2)

                    Dim lt As String = ""
                    If IsNumeric(vinit) And IsNumeric(vcount) Then
                        Dim tv As Integer

                        tv = vinit + vcount

                        lt = tv
                    Else
                        lt = vinit & " + " & vcount
                    End If


                    rvalue = "for(var " & vname & " = " & vinit & "; " & vname & " < " & lt & "; " & vname & "++)"
                End If
            Case "EUDLoopNewUnit", "EUDLoopUnit", "EUDLoopUnit2", "EUDLoopSprite"
                rvalue = "foreach(" & vvalue & " : " & ForType & "())"
            Case "Timeline"
                Dim v() As String = vvalue.Split("ᗢ")
                rvalue = "foreach(" & v(0) & " : " & ForType & "(" & v(1) & "))"
            Case "EUDLoopPlayerUnit"
                Dim v() As String = vvalue.Split("ᗢ")
                rvalue = "foreach(" & v(0) & " : " & ForType & "(" & v(1) & "))"
            Case "EUDLoopPlayer"

                Dim v() As String = vvalue.Split("ᗢ")
                Dim vname As String = v(0)
                Dim owner As String = v(1)
                Dim forces As String = v(2)
                Dim race As String = v(3)
                If owner <> "None" Then
                    owner = """" & owner & """"
                End If
                If race <> "None" Then
                    race = """" & race & """"
                End If
                rvalue = "foreach(" & vname & " : " & ForType & "(" & owner & "," & forces & "," & race & "))"


            Case "EUDPlayerLoop"
                rvalue = "EUDPlayerLoop()();"
        End Select


        Return rvalue
    End Function

    Public Function ForvarName() As String()
        Dim ForType As String = value.Split(";").First
        Dim vvalue As String = value.Split(";").Last

        Dim rvalue As New List(Of String)
        Select Case ForType
            Case "UserEdit"
                Dim t As String = vvalue.Split("ᗢ").Last

                Dim vt As String = t.Split(";").First
                Dim vname As String = vt.Split("=").First.Replace("var", "").Trim


                rvalue.Add(vname)
            Case "CountRepeat"
                Dim v() As String = vvalue.Split("ᗢ")
                If v.Count = 3 Then
                    rvalue.Add(v(0))
                End If
            Case "EUDLoopNewUnit"
                rvalue.AddRange(vvalue.Split(","))
            Case "EUDLoopUnit"
                rvalue.AddRange(vvalue.Split(","))
            Case "EUDLoopUnit2"
                rvalue.AddRange(vvalue.Split(","))
            Case "EUDLoopSprite"
                rvalue.AddRange(vvalue.Split(","))
            Case "Timeline"
                Dim v() As String = vvalue.Split("ᗢ")
                rvalue.Add(v(0))
            Case "EUDLoopPlayerUnit"
                Dim v() As String = vvalue.Split("ᗢ")
                rvalue.Add(v(0))
            Case "EUDLoopPlayer"
                Dim v() As String = vvalue.Split("ᗢ")
                Dim vname As String = v(0)

                rvalue.Add(vname)
        End Select

        Return rvalue.ToArray
    End Function

    Public Function ForCoder() As String
        Dim ForType As String = value.Split("ᚢ").First
        Dim vvalue As String = value.Split("ᚢ").Last

        Dim rvalue As String = ""
        Select Case ForType
            Case "UserEdit"
                rvalue = vvalue.Replace("ᗢ", " : ")
            Case "CountRepeat"
                Dim v() As String = vvalue.Split("ᗢ")

                If v.Count = 3 Then
                    rvalue = v(0) & " : "
                    rvalue = rvalue & "초기값=" & v(1)
                    rvalue = rvalue & ", 횟수=" & v(2)
                Else
                    rvalue = rvalue & "오류"
                End If
            Case "EUDLoopNewUnit"
                rvalue = vvalue & " : 새로 생성된 유닛을 루프합니다."
            Case "EUDLoopUnit"
                rvalue = vvalue & " : 모든 유닛을 연결리스트를 따라 루프합니다."
            Case "EUDLoopUnit2"
                rvalue = vvalue & " : 모든 유닛을 루프합니다."
            Case "EUDLoopSprite"
                rvalue = vvalue & " : 모든 스프라이트를 루프합니다."
            Case "Timeline"
                Dim v() As String = vvalue.Split("ᗢ")
                rvalue = v(0) & " : " & v(1) & "틱 만큼 진행합니다."
            Case "EUDLoopPlayerUnit"
                Dim v() As String = vvalue.Split("ᗢ")
                rvalue = v(0) & " : Player " & v(1) + 1 & "의 유닛을 루프합니다."
            Case "EUDLoopPlayer"
                Dim v() As String = vvalue.Split("ᗢ")
                Dim vname As String = v(0)
                Dim owner As String = v(1)
                Dim forces As String = v(2)
                Dim race As String = v(3)
                If owner = "None" Then
                    owner = "모두"
                End If
                If forces = "None" Then
                    forces = "모두"
                End If
                If race = "None" Then
                    race = "모두"
                End If


                rvalue = vname & " : "
                rvalue = rvalue & "제어권 : " & owner
                rvalue = rvalue & "   세력 : " & forces
                rvalue = rvalue & "   종족 : " & race
                rvalue = rvalue & "   해당플레이어 설정 : " & v(4)

            Case "EUDPlayerLoop"
                rvalue = "모든 플레이어를 순환합니다."
        End Select



        Return rvalue
    End Function

    Public Sub ArgCoder(lnlines As InlineCollection)
        AddText(lnlines, "이름 : ", Nothing)
        AddText(lnlines, value, tescm.HighlightBrush)
        AddText(lnlines, "    타입 : ", Nothing)
        AddText(lnlines, name, tescm.HighlightBrush)
    End Sub


    Public Sub ExpCoder(lnlines As InlineCollection)
        For i = 0 To child.Count - 1
            If child(i).ScriptType = EBlockType.sign Then
                AddText(lnlines, " " & child(i).value & " ", Nothing)
            Else
                AddText(lnlines, child(i).ValueCoder(), tescm.HighlightBrush)
            End If
        Next
    End Sub

    Public Sub FuncCoder(lnlines As InlineCollection)
        If ScriptType = EBlockType.macrofun Then
            Dim luafunc As MacroManager.LuaFunction = macro.GetFunction(name)
            If luafunc IsNot Nothing Then
                If luafunc.IsCompleteFunction Then
                    For k = 0 To luafunc.ArgLists.Count - 1
                        If luafunc.ArgLists(k).BType = MacroManager.LuaFunction.ArgBlock.BlockType.Arg Then
                            AddText(lnlines, child(luafunc.ArgLists(k).ArgIndex).ValueCoder(), tescm.HighlightBrush)
                        Else
                            AddText(lnlines, luafunc.ArgLists(k).Label, Nothing)
                        End If
                    Next
                Else
                    DefaultCoder(lnlines)
                End If
            Else
                DefaultCoder(lnlines)
            End If

            Return
        End If


        Dim i As Integer = Tool.TEEpsDefaultFunc.SearchFunc(name)

        If i >= 0 Then
            Dim functooltip As FunctionToolTip = Tool.TEEpsDefaultFunc.GetFuncTooltip(i)
            If functooltip.Summary.Trim = "" Then
                DefaultCoder(lnlines)
                Return
            End If
            Dim argumentstr As String = functooltip.Summary.Split(vbCrLf).First
            argumentstr = functooltip.Summary.Replace(argumentstr, "").Trim

            Dim arglist As New List(Of String)
            Dim args() As String = Tool.TEEpsDefaultFunc.GetFuncArgument(i).Split(",")
            Dim vcount As Integer = 0
            Dim argscount As Integer = 0
            For k = 0 To args.Count - 1
                If args(k).Trim <> "" Then
                    argscount += 1
                    Dim aname As String = args(k).Split(":").First.Trim
                    arglist.Add(aname)

                    If argumentstr.IndexOf("[" & aname & "]") <> -1 Then
                        vcount += 1
                    End If
                    argumentstr = argumentstr.Replace("[" & aname & "]", "ᐱᐳ" & aname & "ᐱ")
                End If
            Next
            Dim values() As String = argumentstr.Split("ᐱ")

            If vcount <> argscount Then
                DefaultCoder(lnlines)
                Return
            End If
            If argscount = 0 And argumentstr = "" Then
                DefaultCoder(lnlines)
                Return
            End If
            If vcount <> child.Count Then
                AddText(lnlines, "인자수가 정의랑 일치하지 않습니다. : ", Brushes.MediumVioletRed)
                DefaultCoder(lnlines)
                Return
            End If

            For k = 0 To values.Count - 1
                If values(k).Trim <> "" Then
                    If values(k)(0) = "ᐳ" Then
                        Dim vname As String = Mid(values(k), 2)
                        AddText(lnlines, child(arglist.IndexOf(vname)).ValueCoder(), tescm.HighlightBrush)
                    Else
                        AddText(lnlines, values(k), Nothing)
                    End If
                End If
            Next
        Else
            Dim func As ScriptBlock = tescm.GetFuncInfor(name, Scripter)

            If func Is Nothing Then
                DefaultCoder(lnlines)
            Else
                Dim argumentstr As String = func.GetFuncTooltip
                Dim argsb As List(Of ScriptBlock) = tescm.GetFuncArgs(func)
                Dim args As New List(Of String)
                Dim argtooltip As New List(Of String)

                Dim arglist As New List(Of String)
                Dim argTooltiplist As New List(Of String)
                For i = 0 To argsb.Count - 1
                    Dim argname As String = argsb(i).value
                    Dim sargtooltip As String = argsb(i).value2
                    argtooltip.Add(sargtooltip)
                    args.Add(argname)
                Next
                Dim vcount As Integer = 0
                For k = 0 To args.Count - 1
                    Dim aname As String = args(k).Split(":").First.Trim
                    arglist.Add(aname)

                    If argumentstr.IndexOf("[" & aname & "]") <> -1 Then
                        vcount += 1
                    End If
                    argumentstr = argumentstr.Replace("[" & aname & "]", "ᐱᐳ" & aname & "ᐱ")
                Next
                Dim values() As String = argumentstr.Split("ᐱ")
                If vcount <> args.Count Then
                    DefaultCoder(lnlines)
                    Return
                End If
                If args.Count = 0 And argumentstr = "" Then
                    DefaultCoder(lnlines)
                    Return
                End If
                If vcount <> child.Count Then
                    AddText(lnlines, "인자수가 정의랑 일치하지 않습니다. : ", Brushes.MediumVioletRed)
                    DefaultCoder(lnlines)
                    Return
                End If

                For k = 0 To values.Count - 1
                    If values(k).Trim <> "" Then
                        If values(k)(0) = "ᐳ" Then
                            Dim vname As String = Mid(values(k), 2)
                            AddText(lnlines, child(arglist.IndexOf(vname)).ValueCoder(), tescm.HighlightBrush)
                        Else
                            AddText(lnlines, values(k), Nothing)
                        End If
                    End If
                Next

            End If

        End If
    End Sub

    Private Sub DefaultCoder(lnlines As InlineCollection)
        AddText(lnlines, name, Nothing)
        AddText(lnlines, "(", Nothing)
        For i = 0 To child.Count - 1
            If i <> 0 Then
                AddText(lnlines, ", ", Nothing)
            End If

            AddText(lnlines, child(i).ValueCoder(), tescm.HighlightBrush)

        Next
        AddText(lnlines, ")", Nothing)




        'Dim func As ScriptBlock = tescm.GetFuncInfor(name, Scripter)
        'Dim args As List(Of ScriptBlock) = tescm.GetFuncArgs(func)


        'For j = 0 To args.Count - 1
        '    Dim vname As String = args(j).value
        '    Dim vtype As String = args(j).name

        '    AddChild(New ScriptBlock(vtype, False, False, vname, Scripter))
        'Next
    End Sub




    Public Function IsValue() As Boolean
        If ScriptType = EBlockType.constVal Then
            Return True
        Else
            If Parent Is Nothing Then
                Return False
            End If
            If Parent.isfolder Then
                Return False
            Else
                Return True
            End If
        End If
    End Function
    Public Function IsArgument() As Boolean
        If ScriptType = EBlockType.funargblock Then
            Return True
        End If
        If IsValue() Then
            If Parent.ScriptType = EBlockType.funargs Then
                Return True
            End If
        End If
        Return False
    End Function






    Function GetScriptBlockItem() As ScriptTreeviewitem
        Dim stvi As New ScriptTreeviewitem(Me)

        Return stvi
    End Function

    Public Sub RefreshListBox(Tvitem As TreeViewItem)
        Tvitem.Header = GetScriptBlockItem()

    End Sub


    Public Sub treeviewcollapsed(sender As TreeViewItem, e As RoutedEventArgs)
        Dim sb As ScriptBlock = sender.Tag

        sb.isexpand = sender.IsExpanded
    End Sub

    Public Function GetTreeviewitem() As TreeViewItem
        Dim treeviewitem As New TreeViewItem
        treeviewitem.Background = Application.Current.Resources("MaterialDesignPaper")
        treeviewitem.Foreground = Application.Current.Resources("MaterialDesignBody")


        treeviewitem.Tag = Me

        Dim scritem As ScriptTreeviewitem = GetScriptBlockItem()
        treeviewitem.Header = scritem

        treeviewitem.IsExpanded = isexpand

        AddHandler treeviewitem.Collapsed, AddressOf treeviewcollapsed
        AddHandler treeviewitem.Expanded, AddressOf treeviewcollapsed

        Dim isAllPaint As Boolean = False

        Dim drawchild As Boolean = True
        Dim groupname As String = ""
        Select Case ScriptType
            Case EBlockType.rawcode
                groupname = "EtcBlock"
            Case EBlockType.import
                groupname = "EtcBlock"
            Case EBlockType.vardefine
                groupname = "Variable"
                drawchild = False
            Case EBlockType.varuse
                groupname = "Variable"
                drawchild = False
            Case EBlockType.objectdefine
                groupname = "Variable"
            Case EBlockType.objectfields, EBlockType.objectmethod
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Variable"
            Case EBlockType.fundefine
                groupname = "Func"
            Case EBlockType.funcontent
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Func"
            Case EBlockType.funargs
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Func"
            Case EBlockType.funargblock
                groupname = "Func"
            Case EBlockType._if
                groupname = "Control"
            Case EBlockType._elseif
                groupname = "Control"
            Case EBlockType.ifcondition
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Control"
                isAllPaint = True
            Case EBlockType.ifthen
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Control"
                isAllPaint = True
            Case EBlockType.ifelse
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Control"
                isAllPaint = True
            Case EBlockType.break
                groupname = "Control"
                isAllPaint = True
            Case EBlockType._for
                isAllPaint = True
                groupname = "Control"
            Case EBlockType.foraction
                groupname = "Control"
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case EBlockType._while
                groupname = "Control"
                isAllPaint = True
            Case EBlockType.whilecondition
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Control"
                isAllPaint = True
            Case EBlockType.whileaction
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Control"
                isAllPaint = True
            Case EBlockType.switch, EBlockType.switchvar
                groupname = "Control"
                isAllPaint = True
            Case EBlockType.switchcase
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
                groupname = "Control"
                isAllPaint = True
            Case EBlockType._or
                treeviewitem.Background = Brushes.YellowGreen
                'treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case EBlockType._and
                treeviewitem.Background = Brushes.GreenYellow
                'treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case EBlockType.folder
                groupname = "EtcBlock"
                isAllPaint = True
            Case EBlockType.folderaction
                groupname = "EtcBlock"
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case EBlockType.exp
                groupname = "Control"
                drawchild = False
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case EBlockType.action
                groupname = "Action"
                drawchild = False
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case EBlockType.condition
                groupname = "Condition"
                drawchild = False
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case EBlockType.funuse
                groupname = "Func"
                drawchild = False
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case EBlockType.plibfun
                groupname = "plibFunc"
                drawchild = False
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case EBlockType.macrofun
                groupname = "MacroFunc"
                drawchild = False
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case EBlockType.externfun
                groupname = "Func"
                drawchild = False
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
            Case EBlockType.funreturn
                groupname = "Func"
                drawchild = False
                treeviewitem.Style = Application.Current.Resources("ShortTreeViewItem")
        End Select
        If groupname <> "" Then
            Dim colorcode As String = tescm.Tabkeys(groupname)
            treeviewitem.Background = New SolidColorBrush(ColorConverter.ConvertFromString(colorcode))
        End If


        'treeviewitem.Header = name


        For i = 0 To child.Count - 1
            If drawchild Then
                treeviewitem.Items.Add(child(i).GetTreeviewitem())
            End If
        Next

        If isAllPaint Then
            scritem.HeaderPaint(treeviewitem.Background)
        End If
        Return treeviewitem
    End Function

    Public Sub DuplicationBlock(scr As ScriptBlock)
        ScriptType = scr.ScriptType

        isfolder = scr.isfolder
        IsDeleteAble = scr.IsDeleteAble


        name = scr.name
        isexpand = scr.isexpand
        flag = scr.flag
        value = scr.value
        value2 = scr.value2

        child.Clear()
        For i = 0 To scr.child.Count - 1
            AddChild(scr.child(i))
        Next


        'Public child As List(Of ScriptBlock)



        Parent = scr.Parent
        Scripter = scr.Scripter
    End Sub



    Public Function DeepCopy() As ScriptBlock
        Dim stream As MemoryStream = New MemoryStream()


        Dim formatter As BinaryFormatter = New BinaryFormatter()

        formatter.Serialize(stream, Me)
        stream.Position = 0

        Dim rScriptBlock As ScriptBlock = formatter.Deserialize(stream)
        rScriptBlock.Scripter = Me.Scripter
        rScriptBlock.Parent = Me.Parent


        Return rScriptBlock
    End Function


End Class