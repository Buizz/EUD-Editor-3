Partial Public Class MacroManager
    Public luaReturnstr As String



    Public onpluginStr As New List(Of String)
    Public Sub onPluginText(t As String)
        If onpluginStr.IndexOf(t) = -1 Then
            onpluginStr.Add(t)
        End If
    End Sub
    Public beforeStr As New List(Of String)
    Public Sub beforeText(t As String)
        If beforeStr.IndexOf(t) = -1 Then
            beforeStr.Add(t)
        End If
    End Sub
    Public afterStr As New List(Of String)
    Public Sub afterText(t As String)
        If afterStr.IndexOf(t) = -1 Then
            afterStr.Add(t)
        End If
    End Sub




    Public preDefineStr As New List(Of String)
    Public Sub preDefine(t As String)
        If preDefineStr.IndexOf(t) = -1 Then
            preDefineStr.Add(t)
        End If
    End Sub



    Public Sub echo(t As String)
        luaReturnstr = luaReturnstr & t
    End Sub
    Public Function ParseUnit(unit As String) As String


        Dim rint As Integer = pjData.GetUnitIndex(unit)
        If rint = -1 Then
            Return unit
        Else
            Return rint
        End If
    End Function
    Public Function ParseLocation(loc As String) As String
        Dim rint As Integer = pjData.GetLocationIndex(loc)
        If rint = -1 Then
            Return loc
        Else
            Return rint
        End If
    End Function
    Public Function ParseSwitchName(switch As String) As String
        Dim rint As Integer = pjData.GetSwitchIndex(switch)
        If rint = -1 Then
            Return switch
        Else
            Return rint
        End If
    End Function


    Public Function ParseWeapon(ObjectName As String) As String
        Dim rint As Integer = pjData.GetWeaponIndex(ObjectName)
        If rint = -1 Then
            Return ObjectName
        Else
            Return rint
        End If
    End Function
    Public Function ParseFlingy(ObjectName As String) As String
        Dim rint As Integer = pjData.GetFlingyIndex(ObjectName)
        If rint = -1 Then
            Return ObjectName
        Else
            Return rint
        End If
    End Function
    Public Function ParseSprites(ObjectName As String) As String
        Dim rint As Integer = pjData.GetSpriteIndex(ObjectName)
        If rint = -1 Then
            Return ObjectName
        Else
            Return rint
        End If
    End Function
    Public Function ParseImages(ObjectName As String) As String
        Dim rint As Integer = pjData.GetImageIndex(ObjectName)
        If rint = -1 Then
            Return ObjectName
        Else
            Return rint
        End If
    End Function
    Public Function ParseUpgrades(ObjectName As String) As String
        Dim rint As Integer = pjData.GetUpgradeIndex(ObjectName)
        If rint = -1 Then
            Return ObjectName
        Else
            Return rint
        End If
    End Function
    Public Function ParseTechdata(ObjectName As String) As String
        Dim rint As Integer = pjData.GetTechIndex(ObjectName)
        If rint = -1 Then
            Return ObjectName
        Else
            Return rint
        End If
    End Function
    Public Function ParseOrders(ObjectName As String) As String
        Dim rint As Integer = pjData.GetOrderIndex(ObjectName)
        If rint = -1 Then
            Return ObjectName
        Else
            Return rint
        End If
    End Function

    Public Shared EUDScoreList() As String = {
        "Score Total Units Produced",
        "Score Units Produced",
        "Score Units Owned",
        "Score Units Lost",
        "Score Units Killed",
        "Score Unit Total",
        "Score Kill Total",
        "Score Structures Constructed Total",
        "Score Structures Constructed",
        "Score Structures Owned",
        "Score Structures Lost",
        "Score Structures Razed",
        "Score Buildings Total",
        "Score Razings Total",
        "Score Factories Constructed",
        "Score Factories Owned",
        "Score Factories Lost",
        "Score Factories Razed",
        "Score Custom"
    }
    Public Shared EUDScoreOffsetList() As String = {
        "0x581DE4",
        "0x581E14",
        "0x581E44",
        "0x581E74",
        "0x581EA4",
        "0x581ED4",
        "0x581F04",
        "0x581F34",
        "0x581F64",
        "0x581F94",
        "0x581FC4",
        "0x581FF4",
        "0x582024",
        "0x582054",
        "0x582084",
        "0x5820B4",
        "0x5820E4",
        "0x582114",
        "0x5822F4"
    }
    Public Shared EUDSupplyTypeList() As String = {
        "Zerg Control Available",
        "Zerg Control Used",
        "Zerg Control Max",
        "Terran Supply Available",
        "Terran Supply Used",
        "Terran Supply Max",
        "Protoss Psi Available",
        "Protoss Psi Used",
        "Protoss Psi Max"
    }
    Public Shared EUDSupplyTypeOffsetList() As String = {
        "0x582144",
        "0x582174",
        "0x5821A4",
        "0x5821D4",
        "0x582204",
        "0x582234",
        "0x582264",
        "0x582294",
        "0x5822C4"
    }
    Public Function ParseEUDScore(ObjectName As String) As String
        Dim rint As Integer = EUDScoreList.ToList.IndexOf(ObjectName)
        If rint = -1 Then
            Return ObjectName
        Else
            Return rint
        End If
    End Function
    Public Function ParseSupplyType(ObjectName As String) As String
        Dim rint As Integer = EUDSupplyTypeList.ToList.IndexOf(ObjectName)
        If rint = -1 Then
            Return ObjectName
        Else
            Return rint
        End If
    End Function
    Public Function ParseSCAScript(ObjectName As String) As String
        ObjectName = ObjectName.Replace("""", "")
        ObjectName = """" + ObjectName + """"

        Dim rint As Integer = GetArgList("SCAScript").ToList.IndexOf(ObjectName)
        If rint = -1 Then
            Return ObjectName.Replace("""", "")
        Else
            Return rint
        End If
    End Function
    Public Function ParseSCAScriptVariable(ObjectName As String) As String
        If SCAScriptVariables.Contains(ObjectName) Then
            Return SCAScriptVariables.IndexOf(ObjectName)
        End If

        SCAScriptVariables.Add(ObjectName)
        Return SCAScriptVariables.IndexOf(ObjectName)
    End Function


    Public Function GetEUDScoreOffset(index As String) As String
        Return EUDScoreOffsetList(index)
    End Function
    Public Function GetSupplyOffset(index As String) As String
        Return EUDSupplyTypeOffsetList(index)
    End Function





    Public Function IsNumber(num As String) As Boolean
        Return IsNumeric(num)
    End Function


    Public Sub GetBGMIndex(bgmname As String)
        For i = 0 To pjData.TEData.BGMData.BGMList.Count - 1
            Dim bgmFile As BGMData.BGMFile = pjData.TEData.BGMData.BGMList(i)

            If bgmname = bgmFile.BGMName Then
                echo(i)
                Return
            End If
        Next
        echo(0)
    End Sub


    Public Function GetReturnBGMIndex(bgmname As String)
        For i = 0 To pjData.TEData.BGMData.BGMList.Count - 1
            Dim bgmFile As BGMData.BGMFile = pjData.TEData.BGMData.BGMList(i)

            If bgmname = bgmFile.BGMName Then
                Return i
            End If
        Next
        Return 0
    End Function



    Public Function DatOffset(DatName As String, Parameter As String) As String
        Dim datfile As SCDatFiles.DatFiles = pjData.Dat.GetDatFileE(DatName)

        Parameter = Parameter.Replace("_", " ").Trim

        Dim Start As Integer = pjData.Dat.GetDatFile(datfile).GetParamInfo(Parameter, SCDatFiles.EParamInfo.VarStart)
        Dim Size As Byte = pjData.Dat.GetDatFile(datfile).GetParamInfo(Parameter, SCDatFiles.EParamInfo.Size)
        Dim Length As Byte = Size * pjData.Dat.GetDatFile(datfile).GetParamInfo(Parameter, SCDatFiles.EParamInfo.VarArray)

        Dim offset As String = "0x" & Hex(Tool.GetOffset(datfile, Parameter)).ToUpper()
        Return offset
    End Function
    Public Function GetDatFile(DatName As String, Parameter As String, index As String) As String
        Dim offset As String = DatOffset(DatName, Parameter)
        Dim datfile As SCDatFiles.DatFiles = pjData.Dat.GetDatFileE(DatName)

        Parameter = Parameter.Replace("_", " ").Trim

        Dim Start As Integer = pjData.Dat.GetDatFile(datfile).GetParamInfo(Parameter, SCDatFiles.EParamInfo.VarStart)
        Dim Size As Byte = pjData.Dat.GetDatFile(datfile).GetParamInfo(Parameter, SCDatFiles.EParamInfo.Size)
        Dim Length As Byte = Size * pjData.Dat.GetDatFile(datfile).GetParamInfo(Parameter, SCDatFiles.EParamInfo.VarArray)


        Dim AddOffset As String
        If Start = 0 Then
            AddOffset = index
        Else
            AddOffset = index & " - " & Start
        End If
        AddOffset = AddOffset & " * " & Length


        Dim action As String = ""
        Select Case Size
            Case 1
                action = "bread"
            Case 2
                action = "wread"
            Case 4
                action = "dwread"
        End Select
        action = action & "(" & offset & " + " & AddOffset & ")"


        Return action
    End Function
    Public Function SetDatFile(DatName As String, Parameter As String, index As String, Value As String, Modifier As String) As String
        Dim offset As String = DatOffset(DatName, Parameter)
        Dim datfile As SCDatFiles.DatFiles = pjData.Dat.GetDatFileE(DatName)

        Parameter = Parameter.Replace("_", " ").Trim

        index = ParseUnit(index)

        Dim Start As Integer = pjData.Dat.GetDatFile(datfile).GetParamInfo(Parameter, SCDatFiles.EParamInfo.VarStart)
        Dim Size As Byte = pjData.Dat.GetDatFile(datfile).GetParamInfo(Parameter, SCDatFiles.EParamInfo.Size)
        Dim Length As Byte = Size * pjData.Dat.GetDatFile(datfile).GetParamInfo(Parameter, SCDatFiles.EParamInfo.VarArray)


        Dim AddOffset As String
        If Start = 0 Then
            AddOffset = index
        Else
            AddOffset = index & " - " & Start
        End If
        AddOffset = AddOffset & " * " & Length

        Dim rOffset As String = offset & " + " & AddOffset


        Dim action As String = ""

        Select Case Size
            Case 1, 2
                If IsNumeric(index) Then
                    '인덱스가 상수일 경우
                    Dim rindex As Integer = index * Length
                    Dim f As Integer = rindex Mod 4

                    Dim flag As String
                    If Size = 1 Then
                        flag = "FF"
                    Else
                        flag = "FFFF"
                    End If

                    For i = 0 To f - 1
                        flag = flag & "00"
                    Next

                    If IsNumeric(Value) Then
                        Dim iValue As UInteger


                        iValue = Value * Math.Pow(256, f)


                        'If size = 0 Then
                        '    iValue = Value * 1
                        'Else
                        '    iValue = Value * 65536
                        'End If
                        action = String.Format("SetMemoryXEPD(EPD({0}), {1}, {2}, {3})", rOffset, Modifier, iValue, "0x" & flag)
                    Else



                        If f = 0 Then
                            action = String.Format("SetMemoryXEPD(EPD({0}), {1}, {2}, {3})", rOffset, Modifier, Value, "0x" & flag)
                        Else

                            Dim p As Integer = Math.Pow(256, f)

                            action = String.Format("SetMemoryXEPD(EPD({0}), {1}, {2}, {3})", rOffset, Modifier, Value & " * " & p, "0x" & flag)
                        End If
                    End If
                Else
                    '인덱스가 변수 일 경우
                    'local ModifierDict = {
                    '    [SetTo] =  7,
                    '    [Add] =  8,
                    '    [Subtract] =  9,
                    '}
                    Select Case Modifier
                        Case 7, "SetTo"
                            If Size = 1 Then
                                action = String.Format("bwrite({0} ,{1})", rOffset, Value)
                            Else
                                action = String.Format("wwrite({0} ,{1})", rOffset, Value)
                            End If
                        Case 8, "Add"
                            If Size = 1 Then
                                action = String.Format("bwrite({0} ,bread({0}) + {1})", rOffset, Value)
                            Else
                                action = String.Format("wwrite({0} ,wread({0}) + {1})", rOffset, Value)
                            End If
                        Case 9, "Subtract"
                            If Size = 1 Then
                                action = String.Format("bwrite({0} ,bread({0}) - {1})", rOffset, Value)
                            Else
                                action = String.Format("wwrite({0} ,wread({0}) - {1})", rOffset, Value)
                            End If
                    End Select


                End If
                    Case 4
                action = String.Format("SetMemory({0}, {1}, {2})", rOffset, Modifier, Value)
        End Select


        Return action
    End Function
    Public Function ConditionDatFile(DatName As String, Parameter As String, index As String, Value As String, Modifier As String) As String
        Dim offset As String = DatOffset(DatName, Parameter)
        Dim datfile As SCDatFiles.DatFiles = pjData.Dat.GetDatFileE(DatName)

        Parameter = Parameter.Replace("_", " ")

        Dim Start As Integer = pjData.Dat.GetDatFile(datfile).GetParamInfo(Parameter, SCDatFiles.EParamInfo.VarStart)
        Dim Size As Byte = pjData.Dat.GetDatFile(datfile).GetParamInfo(Parameter, SCDatFiles.EParamInfo.Size)
        Dim Length As Byte = Size * pjData.Dat.GetDatFile(datfile).GetParamInfo(Parameter, SCDatFiles.EParamInfo.VarArray)


        Dim AddOffset As String
        If Start = 0 Then
            AddOffset = index
        Else
            AddOffset = index & " - " & Start
        End If
        AddOffset = AddOffset & " * " & Length

        Dim rOffset As String = offset & " + " & AddOffset


        Dim action As String = ""

        Select Case Size
            Case 1, 2
                If IsNumeric(index) Then
                    '인덱스가 상수일 경우
                    Dim rindex As Integer = index * Length
                    Dim f As Integer = rindex Mod 4

                    Dim flag As String
                    If Size = 1 Then
                        flag = "FF"
                    Else
                        flag = "FFFF"
                    End If

                    For i = 0 To f - 1
                        flag = flag & "00"
                    Next

                    If IsNumeric(Value) Then
                        Dim iValue As UInteger
                        If Size = 1 Then
                            iValue = Value * 1
                        Else
                            iValue = Value * 65536
                        End If
                        action = String.Format("MemoryXEPD(EPD({0}), {1}, {2}, {3})", rOffset, Modifier, iValue, "0x" & flag)
                    Else
                        If Size = 1 Then
                            action = String.Format("MemoryXEPD(EPD({0}), {1}, {2}, {3})", rOffset, Modifier, Value, "0x" & flag)
                        Else
                            action = String.Format("MemoryXEPD(EPD({0}), {1}, {2}, {3})", rOffset, Modifier, Value & " * 65536", "0x" & flag)
                        End If
                    End If
                Else
                    'local ComparisonDict = {
                    '    [AtLeast] = 0,
                    '    [AtMost] = 1,
                    '    [Exactly] = 10,
                    '}
                    Select Case Modifier
                        Case 0
                            If Size = 1 Then
                                action = String.Format("(bread({0}) > {1})", rOffset, Value)
                            Else
                                action = String.Format("(wread({0}) > {1})", rOffset, Value)
                            End If
                        Case 1
                            If Size = 1 Then
                                action = String.Format("(bread({0}) < {1})", rOffset, Value)
                            Else
                                action = String.Format("(wread({0}) < {1})", rOffset, Value)
                            End If
                        Case 10
                            If Size = 1 Then
                                action = String.Format("(bread({0}) == {1})", rOffset, Value)
                            Else
                                action = String.Format("(wread({0}) == {1})", rOffset, Value)
                            End If
                    End Select


                End If
            Case 4
                action = String.Format("Memory({0}, {1}, {2})", rOffset, Modifier, Value)
        End Select


        Return action
    End Function


End Class
