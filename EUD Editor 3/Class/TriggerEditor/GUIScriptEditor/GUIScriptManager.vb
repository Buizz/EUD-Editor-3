Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Text

Public Class GUIScriptManager
    Public HighlightBrush As SolidColorBrush = Brushes.SlateBlue


    Public Tabkeys As Dictionary(Of String, String)

    Public Sub New()
        Tabkeys = New Dictionary(Of String, String)
        Tabkeys.Add("Control", "#FFFFC500")
        Tabkeys.Add("plibFunc", "#FFCF2AFF")
        Tabkeys.Add("MacroFunc", "#FFA566FF")
        Tabkeys.Add("Condition", "#FF45EC5B")
        Tabkeys.Add("Action", "#FF6799FF")
        Tabkeys.Add("Func", "#FF6B66FF")
        Tabkeys.Add("Variable", "#FFFF6850")
        Tabkeys.Add("Value", "#FFFF8040")
        Tabkeys.Add("EtcBlock", "#FFCC3D3D")
    End Sub


    Public Function GetExternFileList(Optional IsFullPath As Boolean = False) As List(Of String)
        Dim rstrlist As New List(Of String)




        Dim folderpath As String = pjData.EudplibData.GetTriggerEditorFolderPath

        For Each files As String In My.Computer.FileSystem.GetFiles(folderpath)
            Dim fileinfo As FileInfo = My.Computer.FileSystem.GetFileInfo(files)

            If fileinfo.Extension = ".eps" Then
                If IsFullPath Then
                    rstrlist.Add(files)
                Else
                    rstrlist.Add("TriggerEditor." & fileinfo.Name)
                End If
            End If
        Next

        Return rstrlist
    End Function




    Public Function GetFuncList(tefile As TEFile) As List(Of String)
        Dim rstr As New List(Of String)

        Dim Script As GUIScriptEditor = tefile.Scripter



        For i = 0 To Script.ItemCount - 1
            If Script.GetItems(i).ScriptType = ScriptBlock.EBlockType.fundefine Then
                If Script.GetItems(i).value <> "onPluginStart" And Script.GetItems(i).value <> "beforeTriggerExec" And Script.GetItems(i).value <> "afterTriggerExec" Then
                    rstr.Add(Script.GetItems(i).value)
                End If

            End If
        Next

        Return rstr
    End Function



    Public Function GetFuncInfor(name As String, tScript As GUIScriptEditor) As ScriptBlock
        If tScript IsNot Nothing Then
            For i = 0 To tScript.ItemCount - 1
                If tScript.GetItems(i).ScriptType = ScriptBlock.EBlockType.fundefine Then

                    If tScript.GetItems(i).value = name Then
                        Return tScript.GetItems(i)
                    End If
                End If
            Next
        End If

        Return Nothing
    End Function


    Public Function GetExternFuncInfor(name As String, tScript As GUIScriptEditor) As ScriptBlock
        For i = 0 To tScript.ItemCount - 1
            If tScript.GetItems(i).ScriptType = ScriptBlock.EBlockType.fundefine Then

                If tScript.GetItems(i).value = name Then
                    Return tScript.GetItems(i)
                End If
            End If
        Next
        Return Nothing
    End Function


    Public SCValueType() As String = {
        "None",
        "Number",
        "TrgAllyStatus",
        "TrgComparison",
        "TrgCount",
        "TrgModifier",
        "TrgOrder",
        "TrgPlayer",
        "TrgProperty",
        "TrgPropState",
        "TrgResource",
        "TrgScore",
        "TrgSwitchAction",
        "TrgSwitchState",
        "TrgAIScript",
        "TrgLocation",
        "TrgLocationIndex",
        "TrgString",
        "TrgSwitch",
        "TrgUnit",
        "WAVName",
        "Variable",
        "FormatText",
        "BGM",
        "Tag",
        "UnitsDat",
        "WeaponsDat",
        "FlingyDat",
        "SpritesDat",
        "ImagesDat",
        "UpgradesDat",
        "TechdataDat",
        "OrdersDat",
        "Weapon",
        "Flingy",
        "Sprite",
        "Image",
        "Upgrade",
        "Tech",
        "Order",
        "EUDScore",
        "SupplyType"}

    Public SCValueNoneType() As String = {
        "None",
        "Number",
        "WAVName",
        "Variable",
        "FormatText",
        "BGM",
        "Tag",
        "UnitsDat",
        "WeaponsDat",
        "FlingyDat",
        "SpritesDat",
        "ImagesDat",
        "UpgradesDat",
        "TechdataDat",
        "OrdersDat",
        "Weapon",
        "Flingy",
        "Sprite",
        "Image",
        "Upgrade",
        "Tech",
        "Order",
        "EUDScore",
        "SupplyType"}

    Public Function GetFuncArgs(scr As ScriptBlock) As List(Of ScriptBlock)
        If scr.ScriptType = ScriptBlock.EBlockType.fundefine Then
            For i = 0 To scr.child.Count - 1
                If scr.child(i).ScriptType = ScriptBlock.EBlockType.funargs Then
                    Return scr.child(i).child
                End If
            Next
        End If

        Return Nothing
    End Function

    Public Function GetFuncContent(scr As ScriptBlock) As List(Of ScriptBlock)
        If scr.ScriptType = ScriptBlock.EBlockType.fundefine Then
            For i = 0 To scr.child.Count - 1
                If scr.child(i).ScriptType = ScriptBlock.EBlockType.funcontent Then
                    Return scr.child(i).child
                End If
            Next
        End If

        Return Nothing
    End Function





    Public Shared Sub TrimText(text As String, ByRef index As ULong)
        Dim token As String = Mid(text, index, 1)
        index += 1
        While (token.Trim = "" Or token = vbCrLf)
            token = Mid(text, index, 1)
            index += 1
        End While
        index -= 1
    End Sub




    'a .






    Public Shared Function GetIntend(intend As Integer) As String
        Dim str As String = ""

        For i = 0 To intend - 1
            str = str & vbTab
        Next

        Return str
    End Function


    Public Shared Sub GetItemScriptText(scr As ScriptBlock, strb As StringBuilder, ByRef intend As Integer, spliter As String, isCondition As Boolean, isAnd As Boolean, isFuncArg As Boolean, isValCover As Boolean, isLastValue As Boolean, isOneValue As Boolean)


        Dim sscr As ScriptBlock = scr
        Dim stype As String = scr.ScriptType
        Dim sname As String = scr.name
        Dim svalue As String = scr.value
        Dim svalue2 As String = scr.value2
        Dim flag As Boolean = scr.flag
        Dim schild As List(Of ScriptBlock) = scr.child

        If isValCover And sscr.ScriptType <> ScriptBlock.EBlockType.constVal Then
            strb.Append("""")
        End If
        Select Case stype
            Case ScriptBlock.EBlockType.import
                strb.Append(GetIntend(intend))
                strb.Append("import ")
                strb.Append(svalue)
                strb.AppendLine(";")
            Case ScriptBlock.EBlockType.rawcode
                'flag true = raw, false = intend
                strb.Append(svalue)

                If scr.Parent IsNot Nothing Then
                    If scr.Parent.isfolder Then
                        strb.AppendLine("")
                    End If
                End If

                    'If flag Then
                    '    strb.Append(svalue)
                    'Else
                    '    Dim tv As String = svalue.Replace(vbLf, vbCrLf & GetIntend(intend))

                    '    strb.Append(GetIntend(intend))
                    '    strb.AppendLine(tv)
                    'End If
            Case ScriptBlock.EBlockType.vardefine
                'flag true = isconst
                strb.Append(GetIntend(intend))

                If flag Then
                    If svalue2 = "static" Then
                        strb.Append("static ")
                    Else
                        strb.Append("const ")
                    End If
                Else
                    strb.Append("var ")
                End If




                strb.Append(svalue)
                If schild.Count <> 0 Then
                    strb.Append(" = ")
                    GetScriptText(schild, strb, intend, ",")
                End If
                strb.AppendLine(";")
            Case ScriptBlock.EBlockType.objectdefine
                strb.Append(GetIntend(intend))
                strb.Append("object ")
                strb.Append(svalue)
                strb.AppendLine("{")

                intend += 1
                GetScriptText(schild, strb, intend, "")
                intend -= 1

                strb.Append(GetIntend(intend))
                strb.AppendLine("};")
            Case ScriptBlock.EBlockType.objectfields
                GetScriptText(schild, strb, intend, "")
            Case ScriptBlock.EBlockType.objectmethod
                GetScriptText(schild, strb, intend, "")
            Case ScriptBlock.EBlockType.fundefine
                Dim clan As String = pgData.Setting(ProgramData.TSetting.Language)

                If spliter <> "PREDEF" Then
                    strb.AppendLine(GetIntend(intend) & "/***")
                    strb.AppendLine(GetIntend(intend) & " * @Type")
                    strb.AppendLine(GetIntend(intend) & " * F")
                    strb.AppendLine(GetIntend(intend) & " * @Summary." & clan)
                    strb.AppendLine(GetIntend(intend) & " * " & sscr.GetFuncTooltip)
                    strb.AppendLine(GetIntend(intend) & " * ")

                    For k = 0 To tescm.GetFuncArgs(sscr).Count - 1
                        Dim argname As String = tescm.GetFuncArgs(sscr)(k).value
                        Dim argtooltip As String = tescm.GetFuncArgs(sscr)(k).value2


                        strb.AppendLine(GetIntend(intend) & " * @param." & argname & "." & clan)
                        strb.AppendLine(GetIntend(intend) & " * " & argtooltip)
                    Next
                    strb.AppendLine(GetIntend(intend) & "***/")
                End If




                strb.Append(GetIntend(intend))
                strb.Append("function ")
                strb.Append(svalue)

                If spliter = "PREDEF" Then
                    strb.Append("(")
                    GetScriptText(tescm.GetFuncArgs(scr), strb, intend, ", ", False, False, True)
                    strb.Append(")")



                    strb.AppendLine(";")
                    Return
                End If
                GetScriptText(schild, strb, intend, "", False, False, True)
            Case ScriptBlock.EBlockType.funargs
                strb.Append("(")

                GetScriptText(schild, strb, intend, ", ", False, True, True)

                strb.Append(")")
            Case ScriptBlock.EBlockType.funcontent
                strb.AppendLine("{")

                intend += 1
                GetScriptText(schild, strb, intend, "")
                intend -= 1

                strb.Append(GetIntend(intend))
                strb.AppendLine("}")
            Case ScriptBlock.EBlockType._if
                strb.Append(GetIntend(intend))
                strb.Append("if")

                GetScriptText(schild, strb, intend, "")

                strb.AppendLine("")
            Case ScriptBlock.EBlockType._elseif
                strb.Append(GetIntend(intend))
                strb.Append("else if")

                GetScriptText(schild, strb, intend, "")

                strb.AppendLine("")
            Case ScriptBlock.EBlockType.ifcondition
                strb.AppendLine("(")

                intend += 1
                GetScriptText(schild, strb, intend, "", True)
                intend -= 1

                strb.Append(GetIntend(intend))
                strb.Append(")")
            Case ScriptBlock.EBlockType.ifthen
                strb.AppendLine("{")

                intend += 1
                GetScriptText(schild, strb, intend, "")
                intend -= 1

                strb.Append(GetIntend(intend))
                strb.Append("}")
            Case ScriptBlock.EBlockType.ifelse
                strb.AppendLine("else{")

                intend += 1
                GetScriptText(schild, strb, intend, "")
                intend -= 1

                strb.Append(GetIntend(intend))
                strb.Append("}")
            Case ScriptBlock.EBlockType._for
                Dim ForType As String = svalue.Split("ᚢ").First

                strb.Append(GetIntend(intend))
                strb.Append(scr.ForScriptCoder)
                If ForType <> "EUDPlayerLoop" Then
                    strb.AppendLine("{")
                Else
                    strb.AppendLine("")
                End If


                intend += 1
                If ForType = "EUDLoopPlayer" Then
                    If svalue.Split("ᗢ").Last Then
                        strb.Append(GetIntend(intend))
                        strb.AppendLine("setcurpl(" & svalue.Split("ᗢ").First.Split("ᚢ").Last.Trim & ");")
                    End If
                End If

                GetScriptText(schild, strb, intend, "")
                intend -= 1


                strb.Append(GetIntend(intend))
                If ForType <> "EUDPlayerLoop" Then
                    strb.AppendLine("}")
                Else
                    strb.AppendLine("EUDEndPlayerLoop();")
                End If
            Case ScriptBlock.EBlockType.foraction

                GetScriptText(schild, strb, intend, "")

            Case ScriptBlock.EBlockType._while
                strb.Append(GetIntend(intend))
                strb.Append("while")

                GetScriptText(schild, strb, intend, "")

            Case ScriptBlock.EBlockType.whilecondition
                strb.AppendLine("(")

                intend += 1
                GetScriptText(schild, strb, intend, True, "")
                intend -= 1

                strb.Append(GetIntend(intend))
                strb.Append(")")
            Case ScriptBlock.EBlockType.whileaction
                strb.AppendLine("{")

                intend += 1
                GetScriptText(schild, strb, intend, "")
                intend -= 1

                strb.Append(GetIntend(intend))
                strb.AppendLine("}")
            Case ScriptBlock.EBlockType.switch
                strb.Append(GetIntend(intend))
                strb.Append("switch")

                strb.Append("(")
                strb.Append(svalue)
                strb.Append(")")
                strb.AppendLine("{")
                intend += 1
                GetScriptText(schild, strb, intend, "")
                intend -= 1

                strb.Append(GetIntend(intend))
                strb.AppendLine("}")
            Case ScriptBlock.EBlockType.switchcase
                'flag true = break
                strb.Append(GetIntend(intend))
                strb.Append("case ")
                strb.Append(svalue)
                strb.AppendLine(":")

                intend += 1
                GetScriptText(schild, strb, intend, "")
                If flag Then
                    strb.Append(GetIntend(intend))
                    strb.AppendLine("break;")
                End If
                intend -= 1
            Case ScriptBlock.EBlockType._or
                strb.Append(GetIntend(intend))
                strb.AppendLine("(")

                intend += 1
                GetScriptText(schild, strb, intend, "", True, False)
                intend -= 1

                strb.Append(GetIntend(intend))
                strb.Append(")")

                If Not isLastValue Then
                    If isAnd Then
                        strb.Append(" && ")
                    Else
                        strb.Append(" || ")
                    End If
                End If
                strb.AppendLine("")
            Case ScriptBlock.EBlockType._and
                strb.Append(GetIntend(intend))
                strb.AppendLine("(")

                intend += 1
                GetScriptText(schild, strb, intend, "", True, True)
                intend -= 1

                strb.Append(GetIntend(intend))
                strb.Append(")")

                If Not isLastValue Then
                    If isAnd Then
                        strb.Append(" && ")
                    Else
                        strb.Append(" || ")
                    End If
                End If
                strb.AppendLine("")
            Case ScriptBlock.EBlockType.folder
                'flag true = iscondition

                Dim temp As String() = svalue.Split("ᗋ")

                Dim head As String = temp.First
                Dim tail As String = temp.Last

                strb.Append(GetIntend(intend))
                strb.AppendLine(head)


                intend += 1
                GetScriptText(schild, strb, intend, "", flag)
                intend -= 1


                strb.Append(GetIntend(intend))
                strb.AppendLine(tail)
            Case ScriptBlock.EBlockType.folderaction
                GetScriptText(schild, strb, intend, "", flag)
            Case ScriptBlock.EBlockType.exp
                If scr.Parent.isfolder Then
                    strb.Append(GetIntend(intend))
                End If


                If isCondition And Not isOneValue Then
                    strb.Append("(")
                End If
                GetScriptText(schild, strb, intend, " ")

                If isCondition And Not isOneValue Then
                    strb.Append(")")
                End If

                If isCondition Then
                    If Not isLastValue Then
                        If isAnd Then
                            strb.Append(" && ")
                        Else
                            strb.Append(" || ")
                        End If
                    End If
                Else
                    If scr.Parent.isfolder Then
                        strb.Append(";")
                    End If
                End If


                If scr.Parent.isfolder Then
                    strb.AppendLine("")
                End If
            Case ScriptBlock.EBlockType.sign
                strb.Append(svalue)

            Case ScriptBlock.EBlockType.break
                strb.Append(GetIntend(intend))
                strb.AppendLine("break;")
            Case ScriptBlock.EBlockType.varuse
                'strb.Append(sname)
                'strb.Append("/")
                'strb.Append(svalue)
                'strb.Append("/")
                'strb.Append(svalue2)
                'strb.Append("////")
                Select Case svalue
                    Case "constructor"
                        'TODO : 배열관련된것 넣어야됨
                        '// 인게임 초기화 해당 코드에서 매번 초기화합니다(전역 스코프면 게임 시작 때).
                        '// 함수 리턴값처럼 게임 안에서 정해지는 값도 넣을 수 있습니다.
                        'Const a = [getuserplayerid(), 0, 0];
                        'Const b = VArray(GetTBLAddr(1), GetTBLAddr(2), GetTBLAddr(3));

                        '// 컴파일시간 초기화 맵에 삽입될 때 초기화합니다. 상수 표현식만 넣을 수 있습니다.
                        'Const a = EUDArray(List(1, 2, 3, 4));
                        'Const b = EUDVArray(4)(List(EPD(a), EPD(a) + 1, EPD(a) + 2, EPD(a) + 3));
                        Select Case sname
                            Case "EUDArray"
                                If schild.Count = 1 Then
                                    strb.Append(sname)
                                    strb.Append("(")
                                    GetScriptText(schild, strb, intend, "")
                                    strb.Append(")")
                                Else
                                    If flag Then
                                        strb.Append("[")
                                        GetScriptText(schild, strb, intend, ", ")
                                        strb.Append("]")
                                    Else
                                        strb.Append(sname)
                                        strb.Append("(List(")
                                        GetScriptText(schild, strb, intend, ", ")
                                        strb.Append("))")
                                    End If
                                End If
                            Case "EUDVArray"
                                If schild.Count = 1 Then
                                    strb.Append(sname)
                                    strb.Append("(")
                                    GetScriptText(schild, strb, intend, "")
                                    strb.Append(")")
                                    strb.Append("(")
                                    strb.Append(")")
                                Else
                                    If flag Then
                                        strb.Append("VArray")
                                        strb.Append("(")
                                        GetScriptText(schild, strb, intend, ", ")
                                        strb.Append(")")
                                    Else
                                        strb.Append(sname)
                                        strb.Append("(" & schild.Count & ")(List(")
                                        GetScriptText(schild, strb, intend, ", ")
                                        strb.Append("))")
                                    End If
                                End If
                            Case Else
                                strb.Append(sname)
                                strb.Append("(")
                                GetScriptText(schild, strb, intend, ", ")
                                strb.Append(")")
                        End Select

                    Case "cast"
                        strb.Append(sname)
                        strb.Append(".cast(")
                        GetScriptText(schild, strb, intend, "")
                        strb.Append(")")
                    Case "alloc"
                        strb.Append(sname)
                        strb.Append(".alloc(")
                        GetScriptText(schild, strb, intend, "")
                        strb.Append(")")
                    Case "!index"
                        strb.Append(sname)
                        strb.Append("[")
                        GetScriptText(schild, strb, intend, "")
                        strb.Append("]")
                    Case "!default"
                        strb.Append(sname)
                End Select

                Select Case svalue2
                    Case "value"
                        strb.Append(sname)
                    Case "method"
                        strb.Append(sname)
                        strb.Append(".")
                        strb.Append(svalue)

                        strb.Append("(")
                        GetScriptText(schild, strb, intend, ", ")
                        strb.Append(")")
                    Case "fields"
                        strb.Append(sname)
                        strb.Append(".")
                        strb.Append(svalue)
                End Select


                    '    v1 value +=Test method method + Test f1 fields+13;
            Case ScriptBlock.EBlockType.macrofun
                If scr.Parent IsNot Nothing Then
                    If scr.Parent.isfolder Then
                        strb.Append(GetIntend(intend))
                    End If
                End If

                strb.Append("<?")
                strb.Append(sname)
                strb.Append("(")
                GetScriptText(schild, strb, intend, ", ", False, True, False, True)
                strb.Append(")?>")
                If isCondition Then
                    If Not isLastValue Then
                        If isAnd Then
                            strb.Append(" && ")
                        Else
                            strb.Append(" || ")
                        End If
                    End If
                Else
                    If scr.Parent.isfolder Then
                        strb.Append(";")
                    End If
                End If

                If scr.Parent IsNot Nothing Then
                    If scr.Parent.isfolder Then
                        strb.AppendLine("")
                    End If
                End If

            Case ScriptBlock.EBlockType.funreturn
                strb.Append(GetIntend(intend))
                strb.Append("return ")
                GetScriptText(schild, strb, intend, "")
                strb.AppendLine(";")
            Case Else
                If isFuncArg Then
                    If tescm.SCValueNoneType.ToList.IndexOf(sname) <> -1 Then
                        If sname = "WAVName" Then
                            strb.Append(svalue & ":TrgString")
                        Else
                            strb.Append(svalue)
                        End If

                    Else
                        strb.Append(svalue & ":" & sname)
                    End If
                Else
                    If svalue = "" Then
                        If scr.Parent.isfolder Then
                            strb.Append(GetIntend(intend))
                        End If

                        strb.Append(sname)
                        strb.Append("(")

                        GetScriptText(schild, strb, intend, ", ")

                        strb.Append(")")
                        If isCondition Then
                            If Not isLastValue Then
                                If isAnd Then
                                    strb.Append(" && ")
                                Else
                                    strb.Append(" || ")
                                End If
                            End If
                        Else
                            If scr.Parent.isfolder Then
                                strb.Append(";")
                            End If
                        End If


                        If scr.Parent.isfolder Then
                            strb.AppendLine("")
                        End If
                    Else

                        'strb.Append("{" & i & "," & (scr.Count - 1) & "}")
                        Select Case sname.Trim
                            Case "TrgString", "TrgAIScript", "TrgUnit", "TrgLocation", "TrgSwitch", "UnitsDat", "WeaponsDat", "FlingyDat",
                                 "SpritesDat", "ImagesDat", "UpgradesDat", "TechdataDat", "OrdersDat", "WAVName"
                                strb.Append("""" & svalue & """")
                            Case "TrgLocationIndex"
                                Dim v As Integer

                                Dim loactions As New List(Of String)
                                loactions.AddRange(CodeEditor.GetArgList("TrgLocationIndex"))

                                v = loactions.IndexOf(svalue)
                                If v = -1 Then
                                    strb.Append(0)
                                Else
                                    strb.Append(v)
                                End If
                            Case "TrgProperty"
                                strb.Append("UnitProperty(" & svalue & ")")
                            Case "BGM"
                                strb.Append("<?GetBGMIndex(""" & svalue & """)?>")
                            Case Else
                                strb.Append(svalue)
                        End Select


                        'strb.Append("{" & i & "," & (scr.Count - 1) & "}")

                    End If
                End If

                '액션,조건등
        End Select
        If isValCover And sscr.ScriptType <> ScriptBlock.EBlockType.constVal Then
            strb.Append("""")
        End If

        If Not isLastValue Then
            strb.Append(spliter)
        End If
    End Sub
    Public Shared Sub GetScriptText(scr As List(Of ScriptBlock), strb As StringBuilder, ByRef intend As Integer, spliter As String, Optional isCondition As Boolean = False, Optional isAnd As Boolean = True, Optional isFuncArg As Boolean = False, Optional isValCover As Boolean = False)
        For i = 0 To scr.Count - 1
            Dim isLastValue As Boolean = (i = (scr.Count - 1))

            GetItemScriptText(scr(i), strb, intend, spliter, isCondition, isAnd, isFuncArg, isValCover, isLastValue, scr.Count = 1)
        Next
    End Sub
End Class
