Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Windows.Threading
Imports Newtonsoft.Json



Public Class ProjectData
#Region "Filed"
    Private tFilename As String
    Public Property Filename As String
        Get
            Return tFilename
        End Get
        Set(value As String)
            tFilename = value
            SaveData.RelativeDataRefresh()
        End Set
    End Property
    Public ReadOnly Property SafeFilename As String
        Get
            If tFilename = "" Then
                Return Tool.GetText("NoName")
            Else
                Return tFilename.Split("\").Last
            End If
        End Get
    End Property
    Public ReadOnly Property Extension As String
        Get
            If tFilename = "" Then
                Return "e3s"
            Else
                Return tFilename.Split(".").Last.Trim
            End If

        End Get
    End Property

    Public Function IsNewFile() As Boolean
        If tFilename = "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private tIsLoad As Boolean
    Public ReadOnly Property IsLoad As Boolean
        Get
            Return tIsLoad
        End Get
    End Property

    Private tIsDirty As Boolean
    Public ReadOnly Property IsDirty As Boolean
        Get
            Return tIsDirty
        End Get
    End Property

    Public Sub SetDirty(Isd As Boolean)
        tIsDirty = Isd
        Tool.RefreshMainWindow()
    End Sub
    Public Property UseCustomtbl As Boolean
        Get
            Dim Lasttbls As Integer
            For i = 0 To SCtbltxtCount - 1
                If Not pjData.BuildStat_txtIsDefault(i) Then
                    Lasttbls = i + 1
                End If
            Next
            If Lasttbls = 0 Then
                Return False
            End If

            Return SaveData.UseCustomTbl
        End Get
        Set(value As Boolean)
            tIsDirty = True
            SaveData.UseCustomTbl = value
        End Set
    End Property

    Public Property AutoBuild As Boolean
        Get
            Return SaveData.AutoBuild
        End Get
        Set(value As Boolean)
            tIsDirty = True
            SaveData.AutoBuild = value
        End Set
    End Property

    Public Property ViewLog As Boolean
        Get
            Return SaveData.ViewLog
        End Get
        Set(value As Boolean)
            tIsDirty = True
            SaveData.ViewLog = value
        End Set
    End Property
#End Region


#Region "Member"

    Public ReadOnly Property ExtraDat As ExtraDatFiles
        Get
            Return SaveData.ExtraDat
        End Get
    End Property
    Public ReadOnly Property Dat As SCDatFiles
        Get
            Return SaveData.Dat
        End Get
    End Property
    Public ReadOnly Property TEData As TriggerEditorData
        Get
            Return SaveData.TEData
        End Get
    End Property

    Public ReadOnly Property EdsBlock As BuildData.EdsBlock
        Get
            Return SaveData.EdsBlocks
        End Get
    End Property

    Private TriggerEditorTempData As TriggerEditorTempData
    Public ReadOnly Property TETempData As TriggerEditorTempData
        Get
            Return TriggerEditorTempData
        End Get
    End Property

    Public ReadOnly Property BuildStat_txtIsDefault(index As Integer) As Boolean
        Get
            Return SaveData.ExtraDat.Stat_txt(index) = ExtraDatFiles.StatNullString
        End Get
    End Property


    Public ReadOnly Property EngStat_txt(index As Integer) As String
        Get
            If index = -1 Then
                Return Tool.GetText("None")
            End If
            Dim RealName As String = scData.GetEndStat_txt(index)
            If IsMapLoading Then
                Dim strindex As Integer = MapData.DatFile.Data(SCDatFiles.DatFiles.units, "Unit Map String", index)
                Dim ToolTipText As String = SaveData.Dat.ToolTip(SCDatFiles.DatFiles.units, index)


                If strindex = 0 Then
                    Return RealName
                Else
                    Return MapData.Str(strindex - 1)
                End If
            Else
                Return RealName
            End If




        End Get
    End Property
    Public ReadOnly Property BuildStat_txt(index As Integer) As String
        Get
            If index = -1 Then
                Return Tool.GetText("None")
            End If

            If SaveData.ExtraDat.Stat_txt(index) = ExtraDatFiles.StatNullString Then
                Return scData.GetStat_txt(index)
            Else
                Return SaveData.ExtraDat.Stat_txt(index)
            End If
        End Get
    End Property
    Public Property Stat_txt(index As Integer) As String
        Get
            If index = -1 Then
                Return Tool.GetText("None")
            End If

            If pgData.Setting(ProgramData.TSetting.CDLanguageChange) Then
                If SaveData.ExtraDat.Stat_txt(index) = ExtraDatFiles.StatNullString Then
                    Return scData.GetStat_txt(index)
                Else
                    Return SaveData.ExtraDat.Stat_txt(index)
                End If
            Else
                Return scData.GetStat_txt(index)
            End If

        End Get
        Set(value As String)
            SaveData.ExtraDat.Stat_txt(index) = value
        End Set
    End Property

    Public Property TempFileLoc As String
        Get
            Select Case SaveData.TempFileLoc
                Case "DefaultFolder"
                    Return 0
                Case "MapFolder"
                    Return 1
                Case Else
                    Return SaveData.TempFileLoc
            End Select
        End Get
        Set(value As String)
            SetDirty(True)
            Select Case value
                Case 0
                    SaveData.TempFileLoc = "DefaultFolder"
                Case 1
                    SaveData.TempFileLoc = "MapFolder"
                Case Else
                    SaveData.TempFileLoc = value
            End Select
        End Set
    End Property
    Public Property OpenMapName As String
        Get
            Return SaveData.OpenMapName
        End Get
        Set(value As String)
            If My.Computer.FileSystem.FileExists(value) And SaveData.OpenMapName <> value Then
                IsMapLoading = False
                tIsDirty = True
                SaveData.OpenMapName = value
                _MapData = New MapData(SaveData.OpenMapName)

                IsMapLoading = _MapData.LoadComplete
                If Not IsMapLoading Then
                    SaveData.OpenMapName = ""
                End If
            End If
        End Set
    End Property
    Public Property SaveMapName As String
        Get
            Return SaveData.SaveMapName
        End Get
        Set(value As String)
            If SaveData.SaveMapName <> value Then
                tIsDirty = True
                SaveData.SaveMapName = value
            End If
        End Set
    End Property
    Public ReadOnly Property OpenMapSafeName As String
        Get
            Return SaveData.OpenMapName.Split("\").Last
        End Get
    End Property
    Public ReadOnly Property SaveMapSafeName As String
        Get
            Return SaveData.SaveMapName.Split("\").Last
        End Get
    End Property
    Public ReadOnly Property OpenMapdirectory As String
        Get
            Dim returnval As String
            Try
                returnval = Path.GetDirectoryName(SaveData.OpenMapName)
            Catch ex As Exception
                Return ""
            End Try
            Return returnval
        End Get
    End Property
    Public ReadOnly Property SaveMapdirectory As String
        Get
            Dim returnval As String
            Try
                returnval = Path.GetDirectoryName(SaveData.SaveMapName)
            Catch ex As Exception
                Return ""
            End Try
            Return returnval
        End Get
    End Property

    Private _MapData As MapData
    Public ReadOnly Property MapData As MapData
        Get
            If _MapData IsNot Nothing Then
                Return _MapData
            Else
                Tool.ErrorMsgBox("시스템에러 맵데이터가 없음")
                Return Nothing
            End If

        End Get
    End Property

    Private MapLoading As Boolean
    Public Property IsMapLoading As Boolean
        Get

            Return MapLoading
        End Get
        Set(value As Boolean)
            If value Then
                LastModifyTimer = File.GetLastWriteTime(SaveData.OpenMapName)
            End If

            MapLoading = value
        End Set
    End Property

#End Region
    Public Property CodeSelecters As List(Of CodeSelecter)


    Private Bd As BindingManager
    Public ReadOnly Property BindingManager As BindingManager
        Get
            Return Bd
        End Get
    End Property

    Private Dm As DataManager
    Public ReadOnly Property DataManager As DataManager
        Get
            Return Dm
        End Get
    End Property

    Private Ed As BuildData
    Public ReadOnly Property EudplibData As BuildData
        Get
            Return Ed
        End Get
    End Property



    Public ReadOnly Property GetUnitIndex(name As String) As Integer
        Get
            Dim unitstr() As String = CodeEditor.GetArgList("TrgUnit")
            For i = 0 To SCUnitCount - 1
                If unitstr(i) = name Then
                    Return i
                End If
            Next




            Return -1
        End Get
    End Property
    Public ReadOnly Property GetWeaponIndex(name As String) As Integer
        Get
            For i = 0 To SCWeaponCount - 1
                If CodeLabel(SCDatFiles.DatFiles.weapons, i) = name Then
                    Return i
                End If
            Next

            Return -1
        End Get
    End Property
    Public ReadOnly Property GetFlingyIndex(name As String) As Integer
        Get
            For i = 0 To SCFlingyCount - 1
                If CodeLabel(SCDatFiles.DatFiles.flingy, i) = name Then
                    Return i
                End If
            Next

            Return -1
        End Get
    End Property
    Public ReadOnly Property GetSpriteIndex(name As String) As Integer
        Get
            For i = 0 To SCSpriteCount - 1
                If CodeLabel(SCDatFiles.DatFiles.sprites, i) = name Then
                    Return i
                End If
            Next

            Return -1
        End Get
    End Property
    Public ReadOnly Property GetImageIndex(name As String) As Integer
        Get
            For i = 0 To SCImageCount - 1
                If CodeLabel(SCDatFiles.DatFiles.images, i) = name Then
                    Return i
                End If
            Next

            Return -1
        End Get
    End Property
    Public ReadOnly Property GetUpgradeIndex(name As String) As Integer
        Get
            For i = 0 To SCUpgradeCount - 1
                If CodeLabel(SCDatFiles.DatFiles.upgrades, i) = name Then
                    Return i
                End If
            Next

            Return -1
        End Get
    End Property
    Public ReadOnly Property GetTechIndex(name As String) As Integer
        Get
            For i = 0 To SCTechCount - 1
                If CodeLabel(SCDatFiles.DatFiles.techdata, i) = name Then
                    Return i
                End If
            Next

            Return -1
        End Get
    End Property
    Public ReadOnly Property GetOrderIndex(name As String) As Integer
        Get
            For i = 0 To SCOrderCount - 1
                If CodeLabel(SCDatFiles.DatFiles.orders, i) = name Then
                    Return i
                End If
            Next

            Return -1
        End Get
    End Property





    Public ReadOnly Property GetSwitchIndex(name As String) As Integer
        Get
            For i = 0 To 255
                If pjData.MapData.SwitchName(i) = name Then
                    Return i
                End If
            Next

            Return -1
        End Get
    End Property
    Public ReadOnly Property GetLocationIndex(name As String) As Integer
        Get
            For i = 0 To 255
                If pjData.MapData.LocationName(i) = name Then
                    Return i
                End If
            Next

            Return -1
        End Get
    End Property

    Private Function CodeTrimer(texts As String) As String
        Dim rgx As New Text.RegularExpressions.Regex("<([A-Za-z0-9])+>", Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim MatchCollection As Text.RegularExpressions.MatchCollection = rgx.Matches(texts)

        For i = 0 To MatchCollection.Count - 1
            texts = texts.Replace(MatchCollection(i).Value, "")
            If i = 0 And MatchCollection(i).Index = 1 Then
                texts = Mid(texts, 2)
            End If
        Next

        Return texts
    End Function
    Public ReadOnly Property CodeLabel(Datfile As SCDatFiles.DatFiles, index As Integer, Optional IsFullname As Boolean = False) As String
        Get
            Dim ReturnStr As String = Tool.GetText("None")
            Try

                Select Case Datfile
                    Case SCDatFiles.DatFiles.units
                        ReturnStr = UnitName(index)
                    Case SCDatFiles.DatFiles.weapons
                        Dim tLabel As Integer = Dat.Data(SCDatFiles.DatFiles.weapons, "Label", index)

                        ReturnStr = CodeTrimer(Stat_txt(tLabel - 1))
                    Case SCDatFiles.DatFiles.flingy
                        'Dim tSprite As Integer = Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", index)
                        'Dim timage As Integer = Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", tSprite)

                        ReturnStr = scData.flingyStr(index)
                    Case SCDatFiles.DatFiles.sprites
                        'Dim timage As Integer = Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", index)

                        ReturnStr = scData.SpriteStr(index)
                    Case SCDatFiles.DatFiles.images
                        ReturnStr = scData.ImageStr(index)
                    Case SCDatFiles.DatFiles.upgrades
                        Dim tLabel As Integer = Dat.Data(SCDatFiles.DatFiles.upgrades, "Label", index)

                        ReturnStr = CodeTrimer(Stat_txt(tLabel - 1))
                    Case SCDatFiles.DatFiles.techdata
                        Dim tLabel As Integer = Dat.Data(SCDatFiles.DatFiles.techdata, "Label", index)

                        ReturnStr = CodeTrimer(Stat_txt(tLabel - 1))
                    Case SCDatFiles.DatFiles.orders
                        Dim tLabel As Integer = Dat.Data(SCDatFiles.DatFiles.orders, "Label", index)

                        ReturnStr = CodeTrimer(Stat_txt(tLabel - 1))
                    Case SCDatFiles.DatFiles.portdata
                        ReturnStr = scData.PortdataName(index)

                    Case SCDatFiles.DatFiles.sfxdata
                        ReturnStr = scData.SfxCodeName(index)
                    Case SCDatFiles.DatFiles.Icon
                        ReturnStr = scData.IconName(index)
                    Case SCDatFiles.DatFiles.stattxt
                        ReturnStr = Stat_txt(index - 1)

                    Case SCDatFiles.DatFiles.IscriptID
                        ReturnStr = scData.IscriptName(index)
                    Case SCDatFiles.DatFiles.wireframe
                        ReturnStr = UnitName(index)
                    Case SCDatFiles.DatFiles.ButtonData
                        If index < SCUnitCount Then
                            ReturnStr = UnitName(index)
                        Else
                            ReturnStr = scData.BtnStr(index - SCUnitCount)
                        End If
                    Case SCDatFiles.DatFiles.Location
                        If pjData.IsMapLoading Then
                            ReturnStr = pjData.MapData.LocationName(index)
                        Else
                            ReturnStr = "Location " & index + 1
                        End If
                        If ReturnStr = "" Then
                            ReturnStr = "Location " & index + 1
                        End If
                End Select
                Dim ToolTipText As String = ""
                If SCDatFiles.CheckValidDat(Datfile) Then
                    ToolTipText = SaveData.Dat.ToolTip(Datfile, index)
                Else
                    If Datfile = SCDatFiles.DatFiles.stattxt Then
                        ToolTipText = SaveData.ExtraDat.ToolTip(Datfile, index - 1)
                    ElseIf Datfile = SCDatFiles.DatFiles.ButtonData Then
                        ToolTipText = SaveData.ExtraDat.ToolTip(Datfile, index)
                    End If
                End If
                If Datfile <> SCDatFiles.DatFiles.units Then
                    If IsFullname And ToolTipText <> "" Then
                        ReturnStr = ReturnStr & "(" & ToolTipText & ")"
                    End If
                End If
            Catch ex As Exception
                'ReturnStr = ex.ToString
            End Try


            Return ReturnStr
        End Get
    End Property




    Private ReadOnly Property UnitName(index As Byte) As String
        Get
            If index > 228 Then
                Dim aunit() As String = {"(any unit)", "(men)", "(buildings)", "(factories)"}
                Return aunit(index - 229)
            End If


            Dim RealName As String = Stat_txt(index)
            Dim DefaultName As String = scData.GetStat_txt(index)

            If IsMapLoading Then
                Dim strindex As Integer = MapData.DatFile.Data(SCDatFiles.DatFiles.units, "Unit Map String", index)
                Dim ToolTipText As String = SaveData.Dat.ToolTip(SCDatFiles.DatFiles.units, index)

                If ToolTipText.Trim <> "" Then

                    If strindex = 0 Then
                        Return RealName & " (" & ToolTipText & ")"
                    Else
                        Return MapData.Str(strindex - 1) & " (" & ToolTipText & ")" & vbCrLf & DefaultName
                    End If

                Else
                    If strindex = 0 Then
                        Return RealName
                    Else
                        Return MapData.Str(strindex - 1) & vbCrLf & DefaultName
                    End If
                End If

                If strindex = 0 Then
                    Return RealName
                Else
                    Return MapData.Str(strindex - 1) & " (" & ToolTipText & ")" & vbCrLf & DefaultName
                End If
            Else
                Dim ToolTipText As String = SaveData.Dat.ToolTip(SCDatFiles.DatFiles.units, index)

                If ToolTipText <> "" Then
                    Return RealName & " (" & ToolTipText & ")"
                Else
                    Return RealName
                End If

                'Return SaveData.Dat.Group(SCDatFiles.DatFiles.units, index) & RealName
            End If
            'Return index & "미상"
        End Get
    End Property



    Public ReadOnly Property UnitInGameName(index As Byte) As String
        Get
            Dim returnStr As String
            Dim RealName As String = Stat_txt(index)
            Dim DefaultName As String = scData.GetStat_txt(index)

            If IsMapLoading Then
                Dim strindex As Integer = MapData.DatFile.Data(SCDatFiles.DatFiles.units, "Unit Map String", index)

                If strindex = 0 Then
                    returnStr = RealName
                Else
                    returnStr = MapData.Str(strindex - 1)
                End If
            Else
                returnStr = RealName
            End If
            returnStr = tblWriter.StrExecuter(returnStr, True)
            For i = 0 To 31
                returnStr = returnStr.Replace(Chr(i), "")
            Next

            Return returnStr
            'Return index & "미상"
        End Get
    End Property

    Public ReadOnly Property UnitFullName(index As Byte) As String
        Get
            Return scData.GetStat_txt(index, True)
            'Return index & "미상"
        End Get
    End Property

    Public Sub New()
        '초기화
        SaveData = New SaveableData
        TriggerEditorTempData = New TriggerEditorTempData


        Dim FileChangeCheckTimer As DispatcherTimer = New DispatcherTimer()
        FileChangeCheckTimer.Interval = TimeSpan.FromSeconds(2)
        AddHandler FileChangeCheckTimer.Tick, AddressOf FileCheck_Tick
        FileChangeCheckTimer.Start()
    End Sub



    Public Sub InitData()
        Bd = New BindingManager
        Dm = New DataManager
        Ed = New BuildData
        CodeSelecters = New List(Of CodeSelecter)
    End Sub





    Public Sub LoadInit(_filename As String)
        tIsLoad = True
        tIsDirty = False
        Filename = _filename
        'MsgBox("로드 프로젝트 초기화")
        If My.Computer.FileSystem.FileExists(SaveData.OpenMapName) Then
            _MapData = New MapData(SaveData.OpenMapName)
            IsMapLoading = _MapData.LoadComplete
            If Not IsMapLoading Then
                SaveData.OpenMapName = ""
            End If
        Else
            _MapData = Nothing
            SaveData.OpenMapName = ""
        End If
        'For i = 0 To TEData.BGMData.BGMList.Count - 1

        'Next



        If Not My.Computer.FileSystem.DirectoryExists(SaveMapdirectory) Then
            SaveData.SaveMapName = ""
        End If
    End Sub

    Public Sub NewFIle()
        InitProject()
        tIsLoad = True
    End Sub








    Public Function CloseFile() As Boolean
        If IsDirty Then '파일이 변형되었을 경우
            Dim dialog As MsgBoxResult = MsgBox(Tool.GetText("ColseSaveMsg").Replace("%S1", SafeFilename), MsgBoxStyle.YesNoCancel)
            If dialog = MsgBoxResult.Yes Then
                If Save() Then
                    Tool.CloseOtherWindow()
                    tIsLoad = False
                    Return True
                Else
                    Return False
                End If
            ElseIf dialog = MsgBoxResult.No Then
                Tool.CloseOtherWindow()
                tIsLoad = False
                Return True
            ElseIf dialog = MsgBoxResult.Cancel Then
                Return False
            End If

        End If

        Tool.CloseOtherWindow()
        tIsLoad = False
        SaveData.Close()
        Return True
    End Function

    '일단 코드불러오는거 먼저 하자. stat_txt.bin을 불러오고 그걸 바탕으로 만들자.(이미지나 스프라이트를 제외하고는 한글 이름으로 가능하니까 이미지나 스프라이트는 데이터로 준비)

End Class
