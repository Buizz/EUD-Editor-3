Imports System.IO

Public Class CodeGrouping
    Public Sub New()
        ReDim tFlingyGroup(SCFlingyCount)
        ReDim tImageGroup(SCImageCount)
        ReDim tSpriteGroup(SCSpriteCount)
        ReDim tStrGroup(SCtbltxtCount)


        ReadGroup("Texts\FlingyGroup.txt", tFlingyGroup)
        ReadGroup("Texts\ImageGroup.txt", tImageGroup)
        ReadGroup("Texts\SpriteGroup.txt", tSpriteGroup)
        ReadGroup("Texts\StrGroup.txt", tStrGroup)
    End Sub
    Private Sub ReadGroup(tFileName As String, ByRef Arrays() As String)
        For i = 0 To Arrays.Count - 1
            Arrays(i) = ""
        Next


        Dim fliename As String = Tool.DataPath(tFileName)

        Dim fs As New FileStream(fliename, FileMode.Open)
        Dim sr As New StreamReader(fs)


        While (Not sr.EndOfStream)
            Dim GroupText As String = sr.ReadLine
            Dim Objs As String = sr.ReadLine
            Dim obj() As String = Objs.Replace("{", "").Replace("}", "").Split(",")

            'MsgBox(GroupText)
            For i = 0 To obj.Count - 1
                Dim number As Integer
                Try
                    number = obj(i).Trim
                Catch ex As Exception
                    Continue For
                End Try
                Arrays(number) = GroupText
            Next
        End While

        sr.Close()
        fs.Close()
    End Sub


    Private tFlingyGroup() As String
    Private tImageGroup() As String
    Private tSpriteGroup() As String
    Private tStrGroup() As String



    Public ReadOnly Property FlingyGroup(index As Integer) As String
        Get
            Return tFlingyGroup(index)
        End Get
    End Property
    Public ReadOnly Property ImageGroup(index As Integer) As String
        Get
            Return tImageGroup(index)
        End Get
    End Property
    Public ReadOnly Property SpriteGroup(index As Integer) As String
        Get
            Return tSpriteGroup(index)
        End Get
    End Property
    Public ReadOnly Property StrGroup(index As Integer) As String
        Get
            Return tStrGroup(index)
        End Get
    End Property



    Public Shared Sub CodeGrouping(DatfileName As String, ByRef GroupCode As String(), ByRef ToolTip As String())
        Dim DatType As SCDatFiles.DatFiles = Datfilesname.ToList.IndexOf(DatfileName)
        Dim tempDat As SCDatFiles
        Dim tstat_txt As tblReader = Nothing
        If scData Is Nothing Then
            tempDat = New SCDatFiles(False, True)
            tstat_txt = New tblReader(Tool.GetTblFolder & "\stat_txt.tbl")
        Else
            tempDat = scData.DefaultDat
        End If

        Select Case DatType
            Case SCDatFiles.DatFiles.orders
                ReDim GroupCode(SCOrderCount)
                ReDim ToolTip(SCOrderCount)

                For i = 0 To SCOrderCount - 1
                    GroupCode(i) = ""
                    ToolTip(i) = ""
                Next
            Case SCDatFiles.DatFiles.techdata
                Dim Type1 As String() = {"Zerg", "Terran", "Protoss", "Other"}

                ReDim GroupCode(SCTechCount)
                ReDim ToolTip(SCTechCount)

                For i = 0 To SCTechCount - 1
                    Dim Race As Byte = tempDat.Data(SCDatFiles.DatFiles.techdata, "Race", i)

                    GroupCode(i) = Type1(Race)
                    ToolTip(i) = ""
                Next
            Case SCDatFiles.DatFiles.upgrades
                Dim Type1 As String() = {"Zerg", "Terran", "Protoss", "Other"}

                ReDim GroupCode(SCUpgradeCount)
                ReDim ToolTip(SCUpgradeCount)

                For i = 0 To SCUpgradeCount - 1
                    Dim Race As Byte = tempDat.Data(SCDatFiles.DatFiles.upgrades, "Race", i)

                    GroupCode(i) = Type1(Race)
                    ToolTip(i) = ""
                Next
            Case SCDatFiles.DatFiles.images
                ReDim GroupCode(SCImageCount)
                ReDim ToolTip(SCImageCount)

                For i = 0 To SCImageCount - 1
                    GroupCode(i) = Tool.CodeGrouping.ImageGroup(i)
                    ToolTip(i) = ""
                Next
            Case SCDatFiles.DatFiles.sprites
                ReDim GroupCode(SCSpriteCount)
                ReDim ToolTip(SCSpriteCount)

                For i = 0 To SCSpriteCount - 1
                    GroupCode(i) = Tool.CodeGrouping.SpriteGroup(i)
                    ToolTip(i) = ""
                Next
            Case SCDatFiles.DatFiles.flingy
                ReDim GroupCode(SCFlingyCount)
                ReDim ToolTip(SCFlingyCount)

                For i = 0 To SCFlingyCount - 1
                    GroupCode(i) = Tool.CodeGrouping.FlingyGroup(i)
                    ToolTip(i) = ""
                Next

            Case SCDatFiles.DatFiles.weapons
                Dim Type1 As String() = {"Zerg", "Terran", "Protoss", "Neutral", "Undefined"}
                Dim Type2 As String() = {"Air", "Ground", "Air & Ground", "Spell"}


                ReDim GroupCode(SCWeaponCount)
                ReDim ToolTip(SCWeaponCount)

                '저그
                '   공중
                '   지상
                '   공중지상
                '   마법
                '테란
                '프로토스

                'Ground Weapon
                'Air Weapon

                'Staredit Group Flags
                Dim WeaponRace(SCWeaponCount) As Byte
                Dim WeaponUnit(SCWeaponCount) As String

                For i = 0 To SCUnitCount - 1
                    Dim GroundWeapon As Byte = tempDat.Data(SCDatFiles.DatFiles.units, "Ground Weapon", i)
                    Dim AirWeapon As Byte = tempDat.Data(SCDatFiles.DatFiles.units, "Air Weapon", i)

                    If GroundWeapon = AirWeapon And GroundWeapon < 130 Then
                        Dim WeaponN As Byte = GroundWeapon

                        Dim flag As Byte = tempDat.Data(SCDatFiles.DatFiles.units, "Staredit Group Flags", i)

                        WeaponRace(WeaponN) = flag + 1

                        Dim unitnmae As String
                        If scData Is Nothing Then
                            unitnmae = tstat_txt.Strings(i).val1
                        Else
                            unitnmae = scData.GetStat_txt(i)
                        End If
                        If WeaponUnit(WeaponN) <> "" Then
                            WeaponUnit(WeaponN) = WeaponUnit(WeaponN) & ", " & unitnmae
                        Else
                            WeaponUnit(WeaponN) = unitnmae
                        End If
                    Else
                        If GroundWeapon < 130 Then
                            Dim WeaponN As Byte = GroundWeapon

                            Dim flag As Byte = tempDat.Data(SCDatFiles.DatFiles.units, "Staredit Group Flags", i)

                            WeaponRace(WeaponN) = flag + 1

                            Dim unitnmae As String
                            If scData Is Nothing Then
                                unitnmae = tstat_txt.Strings(i).val1 & " G"
                            Else
                                unitnmae = scData.GetStat_txt(i) & " G"
                            End If
                            If WeaponUnit(WeaponN) <> "" Then
                                WeaponUnit(WeaponN) = WeaponUnit(WeaponN) & ", " & unitnmae
                            Else
                                WeaponUnit(WeaponN) = unitnmae
                            End If
                        End If
                        If AirWeapon < 130 Then
                            Dim WeaponN As Byte = AirWeapon

                            Dim flag As Byte = tempDat.Data(SCDatFiles.DatFiles.units, "Staredit Group Flags", i)

                            WeaponRace(WeaponN) = flag + 1

                            Dim unitnmae As String
                            If scData Is Nothing Then
                                unitnmae = tstat_txt.Strings(i).val1 & " A"
                            Else
                                unitnmae = scData.GetStat_txt(i) & " A"
                            End If
                            If WeaponUnit(WeaponN) <> "" Then
                                WeaponUnit(WeaponN) = WeaponUnit(WeaponN) & ", " & unitnmae
                            Else
                                WeaponUnit(WeaponN) = unitnmae
                            End If
                        End If
                    End If



                Next
                '13Name=Targeting
                '13Size=1

                '14Name=Energy
                '14Size=1
                Dim WeaponSpell(SCWeaponCount) As Byte
                For i = 0 To SCOrderCount - 1
                    Dim Targeting As Integer = tempDat.Data(SCDatFiles.DatFiles.orders, "Targeting", i)
                    Dim Energy As Byte = tempDat.Data(SCDatFiles.DatFiles.orders, "Energy", i)

                    WeaponSpell(Targeting) = Energy + 1
                Next

                For i = 0 To SCWeaponCount - 1
                    Dim Icon As Integer = tempDat.Data(SCDatFiles.DatFiles.weapons, "Icon", i)

                    Dim type1v As Byte
                    Dim type2v As UShort = tempDat.Data(SCDatFiles.DatFiles.weapons, "Target Flags", i)

                    Dim flag As Byte = WeaponRace(i)
                    If flag = 0 Then
                        type1v = 4
                    Else
                        flag -= 1
                        If (flag And 1) = 1 Then '저그
                            type1v = 0
                        ElseIf (flag And 2) = 2 Then '테란
                            type1v = 1
                        ElseIf (flag And 4) = 4 Then '프로토스
                            type1v = 2
                        Else '자연
                            type1v = 3
                        End If
                    End If

                    If (type2v And 3) = 3 Then '공중 지상
                        type2v = 2
                    ElseIf (type2v And 1) = 1 Then '지상
                        type2v = 0
                    ElseIf (type2v And 2) = 2 Then '지상
                        type2v = 1
                    End If

                    If WeaponSpell(i) <> 45 And WeaponSpell(i) <> 0 Then '스펠일 경우
                        type1v = tempDat.Data(SCDatFiles.DatFiles.techdata, "Race", WeaponSpell(i) - 1)
                        type2v = 3
                    End If




                    'type3v = tempDat.Data(SCDatFiles.DatFiles.weapons, "Staredit Group Flags", i)


                    GroupCode(i) = Type1(type1v) & "\" & Type2(type2v)
                    ToolTip(i) = WeaponUnit(i)

                Next
            Case SCDatFiles.DatFiles.units
                ReDim GroupCode(SCUnitCount)
                ReDim ToolTip(SCUnitCount)

                Dim Type1 As String() = {"Zerg", "Terran", "Protoss", "Neutral", "Undefined"}
                Dim TypeKeyname As String() = {"*", "Ground Units", "Air Units", "Heroes", "Buildings", "Special Buildings", "Special"}
                Dim TypeNetKeyname As String() = {"Critters", "Doodads", "Powerups", "Resources", "Neutral", "Special", "Start Location", "Zerg", "Protoss"}


                'stat_txt.Strings(index).val2

                For i = 0 To SCUnitCount - 1
                    ToolTip(i) = ""
                    Dim unitpullname As String

                    'Dim unitname As String = pjData.UnitName(i)
                    If scData Is Nothing Then
                        unitpullname = tstat_txt.Strings(i).val2
                    Else
                        unitpullname = scData.GetStat_txt(i, True)
                    End If

                    Dim Flags() As String = unitpullname.Split("|")
                    Dim GroupPath As String = ""
                    'Dim imgSource As ImageSource = New BitmapImage(New Uri("C:\Users\이정훈\Desktop\제목 없음.png"))
                    'Dim bitmap As New Image
                    'bitmap.Source = imgSource


                    'Dim tGraphics As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Graphics", i)
                    'Dim tSprite As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", tGraphics)
                    'Dim timage As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", tSprite)

                    'Dim textblock As New TextBlock
                    'textblock.Text = unitname
                    'textblock.Padding = New Thickness(15, 0, 0, 0)

                    'Dim stackpanel As New StackPanel
                    'stackpanel.Orientation = Orientation.Horizontal
                    'stackpanel.Children.Add(GetImage(timage, Fliter.IsIcon))
                    'stackpanel.Children.Add(textblock)


                    'Dim tListItem As New TreeViewItem()
                    'tListItem.Tag = i
                    'tListItem.Header = stackpanel


                    'MsgBox(Flags(1)) 


                    Dim flag As Byte

                    flag = tempDat.Data(SCDatFiles.DatFiles.units, "Staredit Group Flags", i)



                    Dim Race As Byte
                    If (flag And 1) = 1 Then '저그
                        Race = 0
                    ElseIf (flag And 2) = 2 Then '테란
                        Race = 1
                    ElseIf (flag And 4) = 4 Then '프로토스
                        Race = 2
                    ElseIf (flag And &H80) = &H80 Then '자연
                        Race = 3
                    Else '미분류
                        Race = 4
                    End If
                    GroupPath = Type1(Race) & "\"

                    If 0 <= Race And Race <= 2 Then
                        If Flags(1) = "*" Then '일반 유닛
                            GroupPath = GroupPath & Flags(2) & "\"
                        Else
                            ToolTip(i) = Flags(1)
                            GroupPath = GroupPath & TypeKeyname(3)
                        End If
                    ElseIf Race = 3 Then
                        If Flags(1) <> "*" Then '일반 유닛
                            ToolTip(i) = Flags(1)
                        End If
                        GroupPath = GroupPath & Flags(2) & "\"

                    End If

                    'If i = 0 Then
                    '    MsgBox(GroupPath)
                    'End If

                    GroupCode(i) = GroupPath

                Next
        End Select



    End Sub
End Class
