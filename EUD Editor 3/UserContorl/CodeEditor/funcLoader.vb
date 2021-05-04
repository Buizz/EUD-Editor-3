Imports System.CodeDom
Imports ICSharpCode.AvalonEdit
Imports ICSharpCode.AvalonEdit.CodeCompletion
Imports ICSharpCode.AvalonEdit.Document
Imports ICSharpCode.AvalonEdit.Editing

Partial Public Class CodeEditor

    Public Shared Function GetArgList(ArgumentName As String) As String()
        Select Case ArgumentName
            Case "TrgAllyStatus"
                Dim strs() As String = {"Enemy", "Ally", "AlliedVictory"}

                Return strs
            Case "TrgComparison"
                Dim strs() As String = {"AtLeast", "AtMost", "Exactly"}

                Return strs
            Case "TrgCount"
                Dim strs() As String = {"All"}

                Return strs
            Case "TrgModifier"
                '상수
                Dim strs() As String = {"SetTo", "Add", "Subtract"}

                Return strs
            Case "TrgOrder"
                Dim strs() As String = {"Move", "Patrol", "Attack"}

                Return strs
            Case "TrgPlayer"
                Dim strs() As String = {"P1", "P2", "P3", "P4", "P5", "P6", "P7", "P8", "P9", "P10", "P11", "P12",
                    "CurrentPlayer", "Foes", "Allies", "NeutralPlayers", "AllPlayers", "Force1", "Force2", "Force3", "Force4", "NonAlliedVictoryPlayers"}

                Return strs
            Case "TrgProperty"
                Dim strs() As String = {"UnitProperty"}

                Return strs
            Case "TrgPropState"
                Dim strs() As String = {"Enable", "Disable", "Toggle"}

                Return strs
            Case "TrgResource"
                Dim strs() As String = {"Ore", "Gas", "OreAndGas"}

                Return strs
            Case "TrgScore"
                Dim strs() As String = {"Total", "Units", "Buildings", "UnitsAndBuildings", "Kills",
                    "Razings", "KillsAndRazings", "Custom"}

                Return strs
            Case "TrgSwitchAction"
                Dim strs() As String = {"Set", "Clear", "Toggle", "Random"}

                Return strs
            Case "TrgSwitchState"
                Dim strs() As String = {"Set", "Cleared"}

                Return strs
            Case "TrgAIScript"
                Dim strs() As String = {"Terran Custom Level", "Zerg Custom Level", "Protoss Custom Level", "Terran Expansion Custom Level",
"Zerg Expansion Custom Level", "Protoss Expansion Custom Level", "Terran Campaign Easy", "Terran Campaign Medium", "Terran Campaign Difficult",
"Terran Campaign Insane", "Terran Campaign Area Town", "Zerg Campaign Easy", "Zerg Campaign Medium", "Zerg Campaign Difficult",
"Zerg Campaign Insane", "Zerg Campaign Area Town", "Protoss Campaign Easy", "Protoss Campaign Medium", "Protoss Campaign Difficult",
"Protoss Campaign Insane", "Protoss Campaign Area Town", "Expansion Terran Campaign Easy", "Expansion Terran Campaign Medium", "Expansion Terran Campaign Difficult",
"Expansion Terran Campaign Insane", "Expansion Terran Campaign Area Town", "Expansion Zerg Campaign Easy", "Expansion Zerg Campaign Medium", "Expansion Zerg Campaign Difficult",
"Expansion Zerg Campaign Insane", "Expansion Zerg Campaign Area Town", "Expansion Protoss Campaign Easy", "Expansion Protoss Campaign Medium", "Expansion Protoss Campaign Difficult",
"Expansion Protoss Campaign Insane", "Expansion Protoss Campaign Area Town", "Send All Units on Strategic Suicide Missions", "Send All Units on Random Suicide Missions",
"Switch Computer Player to Rescue Passive", "Turn ON Shared Vision for Player 1", "Turn ON Shared Vision for Player 2", "Turn ON Shared Vision for Player 3",
"Turn ON Shared Vision for Player 4", "Turn ON Shared Vision for Player 5", "Turn ON Shared Vision for Player 6", "Turn ON Shared Vision for Player 7",
"Turn ON Shared Vision for Player 8", "Turn OFF Shared Vision for Player 1", "Turn OFF Shared Vision for Player 2", "Turn OFF Shared Vision for Player 3",
"Turn OFF Shared Vision for Player 4", "Turn OFF Shared Vision for Player 5", "Turn OFF Shared Vision for Player 6", "Turn OFF Shared Vision for Player 7",
"Turn OFF Shared Vision for Player 8", "Move Dark Templars to Region", "Clear Previous Combat Data", "Set Player to Enemy", "Set Player to Ally  ",
"Value This Area Higher", "Enter Closest Bunker", "Set Generic Command Target", "Make These Units Patrol", "Enter Transport", "Exit Transport",
"AI Nuke Here", "AI Harass Here", "Set Unit Order To: Junk Yard Dog", "Disruption Web Here", "Recall Here", "Terran 3 - Zerg Town", "Terran 5 - Terran Main Town",
"Terran 5 - Terran Harvest Town", "Terran 6 - Air Attack Zerg", "Terran 6 - Ground Attack Zerg", "Terran 6 - Zerg Support Town", "Terran 7 - Bottom Zerg Town",
"Terran 7 - Right Zerg Town", "Terran 7 - Middle Zerg Town", "Terran 8 - Confederate Town", "Terran 9 - Light Attack", "Terran 9 - Heavy Attack", "Terran 10 - Confederate Towns",
"Terran 11 - Zerg Town", "Terran 11 - Lower Protoss Town", "Terran 11 - Upper Protoss Town", "Terran 12 - Nuke Town", "Terran 12 - Phoenix Town", "Terran 12 - Tank Town",
"Terran 1 - Electronic Distribution", "Terran 2 - Electronic Distribution", "Terran 3 - Electronic Distribution", "Terran 1 - Shareware",
"Terran 2 - Shareware", "Terran 3 - Shareware", "Terran 4 - Shareware", "Terran 5 - Shareware",
"Zerg 1 - Terran Town", "Zerg 2 - Protoss Town", "Zerg 3 - Terran Town", "Zerg 4 - Right Terran Town",
"Zerg 4 - Lower Terran Town", "Zerg 6 - Protoss Town", "Zerg 7 - Air Town", "Zerg 7 - Ground Town",
"Zerg 7 - Support Town", "Zerg 8 - Scout Town", "Zerg 8 - Templar Town", "Zerg 9 - Teal Protoss",
"Zerg 9 - Left Yellow Protoss", "Zerg 9 - Right Yellow Protoss", "Zerg 9 - Left Orange Protoss", "Zerg 9 - Right Orange Protoss",
"Zerg 10 - Left Teal (Attack", "Zerg 10 - Right Teal (Support", "Zerg 10 - Left Yellow (Support", "Zerg 10 - Right Yellow (Attack",
"Zerg 10 - Red Protoss", "Protoss 1 - Zerg Town", "Protoss 2 - Zerg Town", "Protoss 3 - Air Zerg Town",
"Protoss 3 - Ground Zerg Town", "Protoss 4 - Zerg Town", "Protoss 5 - Zerg Town Island", "Protoss 5 - Zerg Town Base",
"Protoss 7 - Left Protoss Town", "Protoss 7 - Right Protoss Town", "Protoss 7 - Shrine Protoss", "Protoss 8 - Left Protoss Town",
"Protoss 8 - Right Protoss Town", "Protoss 8 - Protoss Defenders", "Protoss 9 - Ground Zerg", "Protoss 9 - Air Zerg",
"Protoss 9 - Spell Zerg", "Protoss 10 - Mini-Towns", "Protoss 10 - Mini-Town Master", "Protoss 10 - Overmind Defenders",
"Brood Wars Protoss 1 - Town A", "Brood Wars Protoss 1 - Town B", "Brood Wars Protoss 1 - Town C", "Brood Wars Protoss 1 - Town D",
"Brood Wars Protoss 1 - Town E", "Brood Wars Protoss 1 - Town F", "Brood Wars Protoss 2 - Town A", "Brood Wars Protoss 2 - Town B",
"Brood Wars Protoss 2 - Town C", "Brood Wars Protoss 2 - Town D", "Brood Wars Protoss 2 - Town E", "Brood Wars Protoss 2 - Town F",
"Brood Wars Protoss 3 - Town A", "Brood Wars Protoss 3 - Town B", "Brood Wars Protoss 3 - Town C", "Brood Wars Protoss 3 - Town D",
"Brood Wars Protoss 3 - Town E", "Brood Wars Protoss 3 - Town F", "Brood Wars Protoss 4 - Town A", "Brood Wars Protoss 4 - Town B",
"Brood Wars Protoss 4 - Town C", "Brood Wars Protoss 4 - Town D", "Brood Wars Protoss 4 - Town E", "Brood Wars Protoss 4 - Town F",
"Brood Wars Protoss 5 - Town A", "Brood Wars Protoss 5 - Town B", "Brood Wars Protoss 5 - Town C", "Brood Wars Protoss 5 - Town D",
"Brood Wars Protoss 5 - Town E", "Brood Wars Protoss 5 - Town F", "Brood Wars Protoss 6 - Town A", "Brood Wars Protoss 6 - Town B",
"Brood Wars Protoss 6 - Town C", "Brood Wars Protoss 6 - Town D", "Brood Wars Protoss 6 - Town E", "Brood Wars Protoss 6 - Town F",
"Brood Wars Protoss 7 - Town A", "Brood Wars Protoss 7 - Town B", "Brood Wars Protoss 7 - Town C", "Brood Wars Protoss 7 - Town D",
"Brood Wars Protoss 7 - Town E", "Brood Wars Protoss 7 - Town F", "Brood Wars Protoss 8 - Town A", "Brood Wars Protoss 8 - Town B",
"Brood Wars Protoss 8 - Town C", "Brood Wars Protoss 8 - Town D", "Brood Wars Protoss 8 - Town E", "Brood Wars Protoss 8 - Town F",
"Brood Wars Terran 1 - Town A", "Brood Wars Terran 1 - Town B", "Brood Wars Terran 1 - Town C", "Brood Wars Terran 1 - Town D",
"Brood Wars Terran 1 - Town E", "Brood Wars Terran 1 - Town F", "Brood Wars Terran 2 - Town A", "Brood Wars Terran 2 - Town B",
"Brood Wars Terran 2 - Town C", "Brood Wars Terran 2 - Town D", "Brood Wars Terran 2 - Town E", "Brood Wars Terran 2 - Town F",
"Brood Wars Terran 3 - Town A", "Brood Wars Terran 3 - Town B", "Brood Wars Terran 3 - Town C", "Brood Wars Terran 3 - Town D",
"Brood Wars Terran 3 - Town E", "Brood Wars Terran 3 - Town F", "Brood Wars Terran 4 - Town A", "Brood Wars Terran 4 - Town B",
"Brood Wars Terran 4 - Town C", "Brood Wars Terran 4 - Town D", "Brood Wars Terran 4 - Town E", "Brood Wars Terran 4 - Town F",
"Brood Wars Terran 5 - Town A", "Brood Wars Terran 5 - Town B", "Brood Wars Terran 5 - Town C", "Brood Wars Terran 5 - Town D",
"Brood Wars Terran 5 - Town E", "Brood Wars Terran 5 - Town F", "Brood Wars Terran 6 - Town A", "Brood Wars Terran 6 - Town B",
"Brood Wars Terran 6 - Town C", "Brood Wars Terran 6 - Town D", "Brood Wars Terran 6 - Town E", "Brood Wars Terran 6 - Town F",
"Brood Wars Terran 7 - Town A", "Brood Wars Terran 7 - Town B", "Brood Wars Terran 7 - Town C", "Brood Wars Terran 7 - Town D",
"Brood Wars Terran 7 - Town E", "Brood Wars Terran 7 - Town F", "Brood Wars Terran 8 - Town A", "Brood Wars Terran 8 - Town B",
"Brood Wars Terran 8 - Town C", "Brood Wars Terran 8 - Town D", "Brood Wars Terran 8 - Town E", "Brood Wars Terran 8 - Town F",
"Brood Wars Zerg 1 - Town A", "Brood Wars Zerg 1 - Town B", "Brood Wars Zerg 1 - Town C", "Brood Wars Zerg 1 - Town D",
"Brood Wars Zerg 1 - Town E", "Brood Wars Zerg 1 - Town F", "Brood Wars Zerg 2 - Town A", "Brood Wars Zerg 2 - Town B",
"Brood Wars Zerg 2 - Town C", "Brood Wars Zerg 2 - Town D", "Brood Wars Zerg 2 - Town E", "Brood Wars Zerg 2 - Town F",
"Brood Wars Zerg 3 - Town A", "Brood Wars Zerg 3 - Town B", "Brood Wars Zerg 3 - Town C", "Brood Wars Zerg 3 - Town D",
"Brood Wars Zerg 3 - Town E", "Brood Wars Zerg 3 - Town F", "Brood Wars Zerg 4 - Town A", "Brood Wars Zerg 4 - Town B",
"Brood Wars Zerg 4 - Town C", "Brood Wars Zerg 4 - Town D", "Brood Wars Zerg 4 - Town E", "Brood Wars Zerg 4 - Town F",
"Brood Wars Zerg 5 - Town A", "Brood Wars Zerg 5 - Town B", "Brood Wars Zerg 5 - Town C", "Brood Wars Zerg 5 - Town D",
"Brood Wars Zerg 5 - Town E", "Brood Wars Zerg 5 - Town F", "Brood Wars Zerg 6 - Town A", "Brood Wars Zerg 6 - Town B",
"Brood Wars Zerg 6 - Town C", "Brood Wars Zerg 6 - Town D", "Brood Wars Zerg 6 - Town E", "Brood Wars Zerg 6 - Town F",
"Brood Wars Zerg 7 - Town A", "Brood Wars Zerg 7 - Town B", "Brood Wars Zerg 7 - Town C", "Brood Wars Zerg 7 - Town D",
"Brood Wars Zerg 7 - Town E", "Brood Wars Zerg 7 - Town F", "Brood Wars Zerg 8 - Town A", "Brood Wars Zerg 8 - Town B",
"Brood Wars Zerg 8 - Town C", "Brood Wars Zerg 8 - Town D", "Brood Wars Zerg 8 - Town E", "Brood Wars Zerg 8 - Town F",
"Brood Wars Zerg 9 - Town A", "Brood Wars Zerg 9 - Town B", "Brood Wars Zerg 9 - Town C", "Brood Wars Zerg 9 - Town D",
"Brood Wars Zerg 9 - Town E", "Brood Wars Zerg 9 - Town F", "Brood Wars Zerg 10 - Town A", "Brood Wars Zerg 10 - Town B",
"Brood Wars Zerg 10 - Town C", "Brood Wars Zerg 10 - Town D", "Brood Wars Zerg 10 - Town E", "Brood Wars Zerg 10 - Town F"}

                Return strs
            Case "TrgLocation", "TrgLocationIndex"
                Dim strs As New List(Of String)

                For i = 0 To 254
                    Dim tstr As String = pjData.CodeLabel(SCDatFiles.DatFiles.Location, i)


                    strs.Add(tstr)
                Next
                Return strs.ToArray
            Case "TrgSwitch"
                Dim strs As New List(Of String)
                For i = 0 To 255
                    Dim tstr As String
                    If pjData.IsMapLoading Then
                        tstr = pjData.MapData.SwitchName(i)
                    Else
                        tstr = "Switch " & i
                    End If

                    strs.Add(tstr)
                Next
                Return strs.ToArray
            Case "TrgUnit"
                Dim strs As New List(Of String)
                '스트링
                For i = 0 To SCUnitCount - 1
                    strs.Add(pjData.EngStat_txt(i))
                Next

                strs.AddRange({"(men)", "(any unit)", "(factories)", "(buildings)"})

                Return strs.ToArray
            Case "WAVName"
                Dim strs As New List(Of String)
                For i = 0 To pjData.MapData.WavCount - 1
                    Dim tstr As String
                    If pjData.IsMapLoading Then
                        tstr = pjData.MapData.WavIndex(i)
                        strs.Add(tstr)
                    End If
                Next

                For i = 0 To scData.Sound_Count - 1
                    strs.Add(scData.SoundName(i))
                Next



                Return strs.ToArray
            Case "BGM"
                Dim strs As New List(Of String)

                For i = 0 To pjData.TEData.BGMData.BGMList.Count - 1
                    Dim bgmFile As BGMData.BGMFile = pjData.TEData.BGMData.BGMList(i)
                    strs.Add(bgmFile.BGMName)
                Next

                Return strs.ToArray
            Case "UnitsDat", "WeaponsDat", "FlingyDat", "SpritesDat", "ImagesDat", "UpgradesDat", "TechdataDat", "OrdersDat"
                Dim strs As New List(Of String)

                Dim dname As String = ArgumentName.Replace("Dat", "").ToLower


                Dim datindex As Integer = Datfilesname.ToList.IndexOf(dname)
                If datindex <> -1 Then
                    If SCDatFiles.CheckValidDat(datindex) Then
                        For i = 0 To pjData.Dat.GetDatFile(datindex).ParamaterList.Count - 1
                            Dim Paramname As String = pjData.Dat.GetDatFile(datindex).ParamaterList(i).GetParamname
                            Paramname = Paramname.Replace(" ", "_")
                            strs.Add(Paramname)
                        Next
                    End If
                End If
                Return strs.ToArray
            Case "Weapon"
                Dim strs As New List(Of String)
                '스트링
                For i = 0 To SCWeaponCount - 1
                    strs.Add(pjData.CodeLabel(SCDatFiles.DatFiles.weapons, i))
                Next

                Return strs.ToArray
            Case "Flingy"
                Dim strs As New List(Of String)
                '스트링
                For i = 0 To SCFlingyCount - 1
                    strs.Add(pjData.CodeLabel(SCDatFiles.DatFiles.flingy, i))
                Next

                Return strs.ToArray
            Case "Sprite"
                Dim strs As New List(Of String)
                '스트링
                For i = 0 To SCSpriteCount - 1
                    strs.Add(pjData.CodeLabel(SCDatFiles.DatFiles.sprites, i))
                Next

                Return strs.ToArray
            Case "Image"
                Dim strs As New List(Of String)
                '스트링
                For i = 0 To SCImageCount - 1
                    strs.Add(pjData.CodeLabel(SCDatFiles.DatFiles.images, i))
                Next

                Return strs.ToArray
            Case "Upgrade"
                Dim strs As New List(Of String)
                '스트링
                For i = 0 To SCUpgradeCount - 1
                    strs.Add(pjData.CodeLabel(SCDatFiles.DatFiles.upgrades, i))
                Next

                Return strs.ToArray
            Case "Tech"
                Dim strs As New List(Of String)
                '스트링
                For i = 0 To SCTechCount - 1
                    strs.Add(pjData.CodeLabel(SCDatFiles.DatFiles.techdata, i))
                Next

                Return strs.ToArray
            Case "Order"
                Dim strs As New List(Of String)
                '스트링
                For i = 0 To SCOrderCount - 1
                    strs.Add(pjData.CodeLabel(SCDatFiles.DatFiles.orders, i))
                Next

                Return strs.ToArray
            Case "EUDScore"
                Return MacroManager.EUDScoreList
            Case "SupplyType"
                Return MacroManager.EUDSupplyTypeList
            Case Else
                Return Tool.GetAutocmp(ArgumentName)
        End Select
        Return {""}
    End Function


    Public Function LoadData(TextEditor As ICSharpCode.AvalonEdit.TextEditor, cmpData As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData), FuncNameas As String, ArgumentCount As Integer, IsFirstArgumnet As Boolean, LastStr As String) As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData)
        Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = cmpData

        'TrgAllyStatus
        'TrgComparison
        'TrgCount
        'TrgModifier
        'TrgOrder
        'TrgPlayer
        'TrgProperty
        'TrgPropState
        'TrgResource
        'TrgScore
        'TrgSwitchAction
        'TrgSwitchState
        'TrgAIScript
        'TrgLocation
        'TrgLocationIndex
        'TrgString
        'TrgSwitch
        'TrgUnit


        Dim Argument As String = GetArgument(FuncNameas, ArgumentCount, LastStr)
        Dim ArgumentType As String = Argument.Split(":").Last.Trim
        Dim ArgName As String = Argument.Split(":").First.Trim
        If ArgName = "WAVName" Then
            ArgumentType = "WAVName"
        End If

        Select Case FuncNameas
            Case "$S"
                IsFirstArgumnet = True
                ArgumentType = "TrgSwitch"
            Case "$U"
                IsFirstArgumnet = True
                ArgumentType = "TrgUnit"
            Case "$L"
                IsFirstArgumnet = True
                ArgumentType = "TrgLocation"
        End Select

        If IsFirstArgumnet Then
            Select Case ArgumentType
                Case "TrgAllyStatus", "TrgComparison", "TrgCount", "TrgModifier", "TrgOrder", "TrgPlayer",
                     "TrgProperty", "TrgPropState", "TrgResource", "TrgScore", "TrgSwitchAction", "TrgSwitchState"
                    Dim strs() As String = GetArgList(ArgumentType)

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "BGM"
                    Dim strs() As String = GetArgList(ArgumentType)

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        Dim tstr As String = "<?GetBGMIndex(""" & strs(i) & """)?>"

                        tb.Text = tstr

                        data.Add(New TECompletionData(0, strs(i), tstr, tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgAIScript", "WAVName"
                    Dim strs() As String = GetArgList(ArgumentType)

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, """" & strs(i) & """", """" & strs(i) & """", tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgLocation", "TrgLocationIndex"
                    Dim strs() As String = GetArgList(ArgumentType)

                    For i = 0 To strs.Length - 1
                        Dim tstr As String = strs(i)

                        If tstr <> "" Then
                            Dim tb As New TextBox
                            tb.Text = "[" & (i + 1) & "] " & tstr

                            data.Add(New TECompletionData(0, """" & tstr & """", """" & tstr & """", tb, TextEditor, TECompletionData.EIconType.StarStringConst))
                        End If
                    Next
                Case "TrgSwitch"
                    Dim strs() As String = GetArgList(ArgumentType)

                    For i = 0 To strs.Length - 1
                        Dim tstr As String = strs(i)

                        Dim tb As New TextBox
                        tb.Text = "[" & i & "] " & tstr

                        data.Add(New TECompletionData(0, """" & tstr & """", """" & tstr & """", tb, TextEditor, TECompletionData.EIconType.StarStringConst))
                    Next
                Case "TrgUnit"
                    '스트링
                    For i = 0 To SCUnitCount - 1
                        Dim tb As New TextBox
                        tb.Text = "[" & i & "] " & pjData.EngStat_txt(i)

                        data.Add(New TECompletionData(0, """" & pjData.CodeLabel(SCDatFiles.DatFiles.units, i) & """", """" & pjData.EngStat_txt(i) & """", tb, TextEditor, TECompletionData.EIconType.StarStringConst))
                    Next

                    Dim strs() As String = {"(men)", "(any unit)", "(factories)", "(buildings)"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, """" & strs(i) & """", """" & strs(i) & """", tb, TextEditor, TECompletionData.EIconType.StarStringConst))
                    Next
            End Select
        End If


        For i = 0 To ExternFiles.Count - 1
            data.Add(New TECompletionData(6, ExternFiles(i).nameSpaceName, ExternFiles(i).nameSpaceName, Nothing, TextEditor, TECompletionData.EIconType.NameSpace_))
        Next



        For i = 0 To LocalFunc.VariableCount - 1
            data.Add(New TECompletionData(5, LocalFunc, i, TextEditor, TECompletionData.EIconType.Variable))
        Next
        'For i = 0 To ExternFunc.VariableCount - 1
        '    data.Add(New TECompletionData(6, ExternFunc, i, TextEditor, TECompletionData.EIconType.Variable))
        'Next
        For i = 0 To Tool.TEEpsDefaultFunc.VariableCount - 1
            data.Add(New TECompletionData(7, Tool.TEEpsDefaultFunc, i, TextEditor, TECompletionData.EIconType.Variable))
        Next


        For i = 0 To LocalFunc.FuncCount - 1
            data.Add(New TECompletionData(10, LocalFunc, i, TextEditor, TECompletionData.EIconType.localFunction))
        Next
        'For i = 0 To ExternFunc.FuncCount - 1
        '    data.Add(New TECompletionData(15, ExternFunc, i, TextEditor, TECompletionData.EIconType.Auto))
        'Next
        For i = 0 To Tool.TEEpsDefaultFunc.FuncCount - 1
            data.Add(New TECompletionData(20, Tool.TEEpsDefaultFunc, i, TextEditor, TECompletionData.EIconType.Auto))
        Next

        'KeyWord추가
        For i = 0 To KeyWords.Count - 1
            data.Add(New TECompletionData(30, KeyWords(i), KeyWords(i), Nothing, TextEditor, TECompletionData.EIconType.KeyWord))
        Next

        'StarKeyWord추가
        For i = 0 To StarKeyWords.Count - 1
            data.Add(New TECompletionData(25, StarKeyWords(i), StarKeyWords(i), Nothing, TextEditor, TECompletionData.EIconType.StarKeyWord))
        Next
        Return data
    End Function


    Public Function LoaddotData(TextEditor As ICSharpCode.AvalonEdit.TextEditor, cmpData As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData), LastStr As String) As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData)
        Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = cmpData

        Dim pieceStr() As String = LastStr.Split(".")


        Dim NameSpace_ As String = pieceStr.First

        Dim VarName As String = pieceStr.First

        If pieceStr.Length > 2 Then
            VarName = pieceStr(pieceStr.Length - 2)
        End If


        For i = 0 To LocalFunc.VariableCount - 1
            If VarName = LocalFunc.GetVariableNames(i) Then
                Dim VarType As String = LocalFunc.GetVariableType(i)
                If VarType.IndexOf("(") >= 0 Then
                    'Object나 함수인 경우
                    Dim ObjectName As String = VarType.Split("(").First





                    '다른거 다 조사하기
                    For k = 0 To LocalFunc.ObjectCount - 1
                        If ObjectName = LocalFunc.GetObject(k).ObjName Then
                            Dim tempfunc As CFunc = LocalFunc.GetObject(k).Functions


                            For l = 0 To tempfunc.VariableCount - 1
                                data.Add(New TECompletionData(30, tempfunc, l, TextEditor, TECompletionData.EIconType.Variable))
                            Next

                            For l = 0 To tempfunc.FuncCount - 1
                                data.Add(New TECompletionData(30, tempfunc, l, TextEditor, TECompletionData.EIconType.localFunction))
                            Next

                        End If
                    Next


                    For k = 0 To Tool.TEEpsDefaultFunc.ObjectCount - 1
                        If ObjectName = Tool.TEEpsDefaultFunc.GetObject(k).ObjName Then
                            Dim tempfunc As CFunc = Tool.TEEpsDefaultFunc.GetObject(k).Functions

                            For l = 0 To tempfunc.VariableCount - 1
                                data.Add(New TECompletionData(30, tempfunc, l, TextEditor, TECompletionData.EIconType.Variable))
                            Next
                            For l = 0 To tempfunc.FuncCount - 1
                                data.Add(New TECompletionData(30, tempfunc, l, TextEditor, TECompletionData.EIconType.localFunction))
                            Next
                        End If
                    Next
                End If
            End If
        Next




        For i = 0 To ExternFiles.Count - 1
            If ExternFiles(i).nameSpaceName = NameSpace_ Then
                For k = 0 To ExternFiles(i).Funcs.VariableCount - 1
                    data.Add(New TECompletionData(5, ExternFiles(i).Funcs, k, TextEditor, TECompletionData.EIconType.Variable))
                Next
                For k = 0 To ExternFiles(i).Funcs.FuncCount - 1
                    data.Add(New TECompletionData(10, ExternFiles(i).Funcs, k, TextEditor, TECompletionData.EIconType.localFunction))
                Next
            End If
        Next



        Return data
    End Function

    Public Function LoadMacroData(TextEditor As ICSharpCode.AvalonEdit.TextEditor, cmpData As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData), FuncNameas As String, ArgumentCount As Integer, IsFirstArgumnet As Boolean, LastStr As String) As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData)
        Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = cmpData



        Dim func As MacroManager.LuaFunction = macro.GetFunction(FuncName)
        Dim Argument As String = ""
        Dim ArgumentType As String = ""
        If func IsNot Nothing Then
            If func.ArgName.Count > ArgumentCount Then
                Argument = func.ArgName(ArgumentCount).Trim
                ArgumentType = func.ArgType(ArgumentCount).Trim
            End If
            'MsgBox(Argument & "<" & ArgumentType & "," & IsFirstArgumnet)
        End If


        Select Case FuncNameas
            Case "$S"
                IsFirstArgumnet = True
                ArgumentType = "TrgSwitch"
            Case "$U"
                IsFirstArgumnet = True
                ArgumentType = "TrgUnit"
            Case "$L"
                IsFirstArgumnet = True
                ArgumentType = "TrgLocation"
        End Select

        If IsFirstArgumnet Then
            Select Case ArgumentType
                Case "TrgAllyStatus", "TrgComparison", "TrgCount", "TrgModifier", "TrgOrder", "TrgPlayer",
                     "TrgProperty", "TrgPropState", "TrgResource", "TrgScore", "TrgSwitchAction", "TrgSwitchState"
                    Dim strs() As String = GetArgList(ArgumentType)

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgAIScript", "BGM"
                    Dim strs() As String = GetArgList(ArgumentType)

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), """" & strs(i) & """", tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgLocation", "TrgLocationIndex"
                    Dim strs() As String = GetArgList(ArgumentType)

                    For i = 0 To strs.Length - 1
                        Dim tstr As String = strs(i)

                        If tstr <> "" Then
                            Dim tb As New TextBox
                            tb.Text = "[" & i & "] " & tstr

                            data.Add(New TECompletionData(0, """" & tstr & """", """" & tstr & """", tb, TextEditor, TECompletionData.EIconType.StarStringConst))
                        End If
                    Next
                Case "TrgSwitch"
                    Dim strs() As String = GetArgList(ArgumentType)

                    For i = 0 To strs.Length - 1
                        Dim tstr As String = strs(i)

                        Dim tb As New TextBox
                        tb.Text = "[" & i & "] " & tstr

                        data.Add(New TECompletionData(0, """" & tstr & """", """" & tstr & """", tb, TextEditor, TECompletionData.EIconType.StarStringConst))
                    Next
                Case "TrgUnit"
                    '스트링
                    For i = 0 To SCUnitCount - 1
                        Dim tb As New TextBox
                        tb.Text = "[" & i & "] " & pjData.UnitInGameName(i)

                        data.Add(New TECompletionData(0, """" & pjData.CodeLabel(SCDatFiles.DatFiles.units, i) & """", """" & pjData.UnitInGameName(i) & """", tb, TextEditor, TECompletionData.EIconType.StarStringConst))
                    Next

                    Dim strs() As String = {"(men)", "(any unit)", "(factories)", "(buildings)"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, """" & strs(i) & """", """" & strs(i) & """", tb, TextEditor, TECompletionData.EIconType.StarStringConst))
                    Next
                Case Else
                    Dim strs() As String = GetArgList(ArgumentType)

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, """" & strs(i) & """", """" & strs(i) & """", tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
            End Select
        End If


        For i = 0 To macro.FunctionList.Count - 1
            Dim tb As New TextBox
            tb.Text = macro.FunctionList(i).Fcomment

            data.Add(New TECompletionData(30, macro.FunctionList(i).Fname, macro.FunctionList(i).Fname, tb, TextEditor, TECompletionData.EIconType.localFunction))
        Next



        'KeyWord추가
        For i = 0 To LuaKeyWords.Count - 1
            data.Add(New TECompletionData(30, LuaKeyWords(i), LuaKeyWords(i), Nothing, TextEditor, TECompletionData.EIconType.LuaKeyWord))
        Next

        Return data
    End Function
    Public Function LoadFuncNameData(TextEditor As ICSharpCode.AvalonEdit.TextEditor, cmpData As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData)) As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData)
        Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = cmpData


        Dim strs() As String = {"onPluginStart", "beforeTriggerExec", "afterTriggerExec"}

        For i = 0 To strs.Length - 1
            Dim tb As New TextBox
            tb.Text = Tool.GetText(strs(i))

            data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
        Next

        Return data
    End Function
    Public Function LoadFuncWriteData(TextEditor As ICSharpCode.AvalonEdit.TextEditor, cmpData As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData)) As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData)
        Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = cmpData


        Dim strs() As String = {"TrgAllyStatus", "TrgComparison", "TrgCount", "TrgModifier", "TrgOrder", "TrgPlayer", "TrgProperty", "TrgPropState", "TrgResource", "TrgScore", "TrgSwitchAction",
"TrgSwitchState", "TrgAIScript", "TrgLocation", "TrgLocationIndex", "TrgString", "TrgSwitch", "TrgUnit", "selftype"}
        For i = 0 To strs.Length - 1
            Dim tb As New TextBox
            tb.Text = strs(i)

            data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
        Next

        Return data
    End Function

    Public Function LoadImportWriteData(TextEditor As ICSharpCode.AvalonEdit.TextEditor, cmpData As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData)) As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData)
        Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = cmpData


        Dim externList As List(Of String) = tescm.GetExternFileList
        For i = 0 To externList.Count - 1
            Dim str As String = externList(i)

            Dim filestr As String = ""
            Dim strarray() As String = str.Split(".")
            For k = 0 To strarray.Count - 2
                If k <> 0 Then
                    filestr = filestr & "."
                End If
                filestr = filestr & strarray(k)
            Next


            Dim tb As New TextBox
            tb.Text = filestr
            data.Add(New TECompletionData(0, filestr, filestr, tb, TextEditor, TECompletionData.EIconType.NameSpace_))
        Next

        'If pjData.TEData.SCArchive.IsUsed Then
        '    Dim str As String = "SCArchive"

        '    Dim tb As New TextBox
        '    tb.Text = str
        '    data.Add(New TECompletionData(0, Str, Str, tb, TextEditor, TECompletionData.EIconType.NameSpace_))
        'End If


        If TEFile IsNot Nothing Then
            For i = 0 To TEFile.Parent.FolderCount - 1
                If TEFile.Parent.Folders(i).FileType <> TEFile.EFileType.Setting Then
                    Dim str As String = TEFile.Parent.Folders(i).FileName

                    Dim tb As New TextBox
                    tb.Text = str

                    data.Add(New TECompletionData(0, str, str, tb, TextEditor, TECompletionData.EIconType.NameSpace_))
                End If
            Next
            For i = 0 To TEFile.Parent.FileCount - 1
                Dim str As String = TEFile.Parent.Files(i).FileName

                Dim tb As New TextBox
                tb.Text = str

                data.Add(New TECompletionData(0, str, str, tb, TextEditor, TECompletionData.EIconType.NameSpace_))
            Next


            'Dim euddraftFile As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(pgData.Setting(ProgramData.TSetting.euddraft))
            'If euddraftFile.Exists Then
            '    For Each s As String In My.Computer.FileSystem.GetDirectories(euddraftFile.DirectoryName & "\lib")
            '        Dim pyfile As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(s)
            '        If pyfile.Extension Then

            '        End If
            '    Next
            'End If
        End If
        Return data
    End Function


    Private LuaKeyWords() As String = {"and", "break", "do", "else", "elseif", "end", "false", "for", "function",
        "if", "in", "local", "nil", "not", "or", "repeat", "return", "then", "true", "until", "while"}

    Private KeyWords() As String = {"object", "static", "once", "if", "else", "while", "for", "function", "foreach",
        "return", "true", "True", "false", "False", "switch", "case", "break", "var", "const", "import", "as", "continue"}


    Private StarKeyWords() As String = {"hitpoint", "shield", "energy", "resource", "hanger", "cloaked", "burrowed", "intransit", "hallucinated", "invincible"}



    Private Function GetArgument(name As String, Argindex As Integer, LastStr As String) As String
        'Log.Text = LastStr
        Dim str As String

        Dim NameSpace_ As String = name.Split(".").First
        Dim FuncName As String = name.Split(".").Last
        If name.IndexOf(".") >= 0 Then
            For i = 0 To ExternFiles.Count - 1
                If ExternFiles(i).nameSpaceName = NameSpace_ Then
                    str = ExternFiles(i).Funcs.FindArgument(FuncName, Argindex)
                    If str <> "" Then
                        Return str
                    End If
                End If
            Next

        Else
            str = LocalFunc.FindArgument(name, Argindex)
            If str <> "" Then
                Return str
            End If

            str = Tool.TEEpsDefaultFunc.FindArgument(name, Argindex)
            If str <> "" Then
                Return str
            End If
        End If






        'str = ExternFunc.FindArgument(name, Argindex)
        'If str <> "" Then
        '    Return str
        'End If

        Return ""
    End Function
End Class


Public Class TECompletionData
    Implements ICompletionData

    Private ToolTip As DependencyObject
    Private TextEditor As TextEditor
    Private IconType As EIconType
    Public Enum EIconType
        Auto

        '키워드
        KeyWord
        LuaKeyWord
        SettingValue
        Funcname
        StarKeyWord

        '에디터 설정 값(AtLeast, AtMost, Enabled등등
        StarConst

        '스타 스트링 값
        StarStringConst

        '액션들
        Action

        '조건들
        Condiction

        'eps기본 함수
        plibFunction

        '로컬 함수
        localFunction

        '로컬 변수들
        Variable

        NameSpace_


        '로컬 항목 및 매개 변수
        '상수
        '속성
        '필드
        '매서드
        '인터페이스
        '클래스
        '모듈
        '구조
        '열거형
        '네임스페이스
        '키워드
    End Enum


    Private inputtext As String
    Public Sub New(tPriority As Double, tlisttext As String, tinputtext As String, tToolTip As DependencyObject, tTextEditor As TextEditor, tIconType As EIconType)
        Me.Text = tlisttext
        inputtext = tinputtext

        If tToolTip Is Nothing Then
            Dim textb As New TextBlock
            textb.Text = tinputtext
            ToolTip = textb
        Else
            ToolTip = tToolTip
        End If
        TextEditor = tTextEditor
        IconType = tIconType
        _Priority = tPriority
    End Sub
    Public Sub New(tPriority As Double, tfunc As CFunc, FuncIndex As Integer, tTextEditor As TextEditor, tIconType As EIconType)

        'LocalFunc.GetFuncName(i), LocalFunc.GetFuncName(i), LocalFunc.GetFuncTooltip(i)



        IconType = tIconType
        If tIconType = EIconType.Auto Then
            Select Case tfunc.GetFuncTooltip(FuncIndex).Type
                Case FunctionToolTip.FType.Act
                    IconType = EIconType.Action
                Case FunctionToolTip.FType.Cond
                    IconType = EIconType.Condiction
                Case FunctionToolTip.FType.Func
                    IconType = EIconType.Funcname
            End Select
        End If

        _Priority = tPriority

        If IconType = EIconType.Variable Then
            Text = tfunc.GetVariableNames(FuncIndex)
            inputtext = tfunc.GetVariableNames(FuncIndex)
            TextEditor = tTextEditor
            Dim textblock As New TextBlock
            textblock.Text = tfunc.GetVariableType(FuncIndex)
            ToolTip = textblock
        Else
            Text = tfunc.GetFuncName(FuncIndex)
            inputtext = tfunc.GetFuncName(FuncIndex)
            TextEditor = tTextEditor
            Dim textblock As New TextBlock
            textblock.Text = tfunc.GetFuncTooltip(FuncIndex).Summary
            ToolTip = textblock
        End If

    End Sub

    Public ReadOnly Property Image As ImageSource Implements ICompletionData.Image
        Get
            Select Case IconType
                Case EIconType.NameSpace_
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/nameSpace.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.StarConst, EIconType.StarKeyWord
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Const.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.StarStringConst
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Const.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.Condiction
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Condiction.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.Action
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Action.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.KeyWord, EIconType.LuaKeyWord
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/KeyWord.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.SettingValue
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Setting.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.Variable
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Variable.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.Funcname, EIconType.localFunction
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Function.png"))
                    Imageicon.Freeze()
                    Return Imageicon
            End Select
            Return Nothing
        End Get
    End Property

    '검색기준
    Public Property Text As String Implements ICompletionData.Text

    Public ReadOnly Property Content As Object Implements ICompletionData.Content
        Get
            Return Me.Text
        End Get
    End Property

    Public ReadOnly Property Description As Object Implements ICompletionData.Description
        Get
            Return ToolTip
        End Get
    End Property

    Private _Priority As Double
    Public ReadOnly Property Priority As Double Implements ICompletionData.Priority
        Get
            Return _Priority
        End Get
    End Property


    Public Sub Complete(ByVal textArea As TextArea, ByVal completionSegment As ISegment, ByVal insertionRequestEventArgs As EventArgs) Implements ICompletionData.Complete
        Dim ResultStr As String = ""

        'TextEditor.SelectionStart = completionSegment.Offset - 1
        'TextEditor.SelectionLength = completionSegment.Length + 1

        Select Case IconType
            Case EIconType.LuaKeyWord
                ResultStr = inputtext
                Select Case ResultStr
                    Case "for"
                        ResultStr = "for i = 0, 10 do" & vbCrLf & "end"
                    Case "if"
                        ResultStr = "if condition then" & vbCrLf & "end"
                    Case "while"
                        ResultStr = "while condition do" & vbCrLf & "end"
                    Case "function"
                        ResultStr = "function name(args)" & vbCrLf & "end"
                    Case "elseif"
                        ResultStr = "elseif condition then" & vbCrLf & "end"
                    Case "refeat"
                        ResultStr = "repeat" & vbCrLf & "until condition"
                End Select
            Case EIconType.KeyWord
                ResultStr = inputtext
                If ResultStr = "for" Then
                    ResultStr = "for(;;){}"
                End If
            Case EIconType.StarStringConst
                ResultStr = inputtext
            Case EIconType.Funcname
                ResultStr = inputtext
            Case Else
                ResultStr = inputtext
        End Select

        'TextEditor.SelectedText = ResultStr
        'TextEditor.SelectionLength = 0
        'TextEditor.SelectionStart += ResultStr.Length
        textArea.Document.Replace(completionSegment, ResultStr)
    End Sub

End Class
