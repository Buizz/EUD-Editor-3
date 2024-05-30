Imports System.Text.RegularExpressions

Module ArgManager

    Public Function GetArgList(ArgumentName As String) As String()
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
                        tstr = "Switch " & (i + 1)
                    End If

                    strs.Add(tstr)
                Next
                Return strs.ToArray
            Case "TrgUnit"
                Dim strs As New List(Of String)
                '스트링
                For i = 0 To SCUnitCount - 1
                    strs.Add(pjData.EngStat_txt(i, True))
                Next

                strs.AddRange({"(any unit)", "(men)", "(buildings)", "(factories)"})

                Return strs.ToArray
            Case "WAVName"
                Dim strs As New List(Of String)
                If pjData.IsMapLoading Then
                    For i = 0 To pjData.MapData.WavCount - 1
                        Dim tstr As String
                        tstr = pjData.MapData.WavIndex(i)
                        strs.Add("""" + tstr + """")
                    Next
                End If



                For i = 0 To scData.Sound_Count - 1
                    strs.Add("""" + scData.SoundName(i) + """")
                Next



                Return strs.ToArray
            Case "BGM"
                Dim strs As New List(Of String)

                For i = 0 To pjData.TEData.BGMData.BGMList.Count - 1
                    Dim bgmFile As BGMData.BGMFile = pjData.TEData.BGMData.BGMList(i)
                    strs.Add("""" + bgmFile.BGMName + """")
                Next

                Return strs.ToArray
            Case "UnitsDat", "WeaponsDat", "FlingyDat", "SpritesDat", "ImagesDat", "UpgradesDat", "TechdataDat", "OrdersDat"
                Dim strs As New List(Of String)

                Dim dname As String = ArgumentName.Replace("Dat", "").ToLower


                Dim datindex As Integer = Datfilesname.ToList.IndexOf(dname)
                If datindex <> -1 Then
                    If SCDatFiles.CheckValidDat(datindex) Then
                        For i = 0 To pjData.Dat.GetDatFile(datindex).ParameterList.Count - 1
                            Dim Paramname As String = pjData.Dat.GetDatFile(datindex).ParameterList(i).GetParamname
                            Paramname = Paramname.Replace(" ", "_")
                            strs.Add(Paramname)
                        Next
                    End If
                End If
                Return strs.ToArray
            Case "Tbl"
                Dim strs As New List(Of String)
                '스트링
                For i = 0 To SCtbltxtCount - 1
                    strs.Add(pjData.CodeLabel(SCDatFiles.DatFiles.stattxt, i))
                Next

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
            Case "SCAScript"
                Dim strs As New List(Of String)
                For Each item In pjData.TEData.GetAllTEFile(TEFile.EFileType.SCAScript)
                    Dim scriptEditor As SCAScriptEditor = item.Scripter

                    Dim file As String = scriptEditor.GetFileText("")
                    If file = "" Then
                        Continue For
                    End If
                    Dim regex As Regex = New Regex("function\s+([\w_][\w\d_]+)")

                    Dim matches As MatchCollection = regex.Matches(file)

                    For index = 0 To matches.Count - 1
                        strs.Add("""" + matches.Item(index).Groups(1).Value + """")
                    Next

                Next

                Return strs.ToArray
            Case Else
                Return Tool.GetAutocmp(ArgumentName)
        End Select
        Return {""}
    End Function
End Module
