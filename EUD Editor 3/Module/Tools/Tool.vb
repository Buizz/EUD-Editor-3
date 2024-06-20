Imports System.IO
Imports System.Media
Imports System.Windows.Threading
Imports BingsuCodeEditor.EpScript
Imports BingsuCodeEditor.Lua
Imports Dragablz
Imports ICSharpCode.AvalonEdit
Imports ICSharpCode.AvalonEdit.Folding
Imports MahApps.Metro.Controls.Dialogs
Imports MaterialDesignThemes.Wpf
Imports Microsoft.WindowsAPICodePack.Shell
Imports Newtonsoft.Json

Namespace Tool
    Module Tool


        Public Sub ArgWindowInit()
            'Try
            '    TriggerArgsEdit.Visibility = Visibility.Visible
            '    TriggerArgsEdit.Visibility = Visibility.Collapsed
            'Catch ex As Exception
            '    TriggerArgsEdit = New TriggerEditValueSelecterWindow
            '    TriggerArgsEdit.Visibility = Visibility.Visible
            '    TriggerArgsEdit.Visibility = Visibility.Collapsed
            'End Try
        End Sub

        Public Sub OpenArgWindow(_scripter As ScriptEditor, _tCode As TriggerCodeBlock, _ArgIndex As Integer, StartPos As Point, _FunctionAddPanel As Grid, Optional _Loc As String = "", Optional ButtonHeight As Integer = 0)

            TriggerArgsEdit.Open(_scripter, _tCode, _ArgIndex, StartPos, _FunctionAddPanel, _Loc, ButtonHeight)
            'Try
            'Catch ex As Exception
            '    TriggerArgsEdit = New TriggerEditValueSelecterWindow

            '    TriggerArgsEdit.Open(_scripter, _tCode, _ArgIndex, StartPos, _FunctionAddPanel, _Loc, ButtonHeight)
            'End Try
        End Sub






        Private cmps As New Dictionary(Of String, List(Of String))
        Public Sub LoadArgs()
            For Each file As String In My.Computer.FileSystem.GetFiles(AutocmpFolderPath)
                Dim f As New FileStream(file, FileMode.Open)

                Dim argllist As New List(Of String)
                Dim sr As New StreamReader(f)

                While (Not sr.EndOfStream)
                    argllist.Add("""" + sr.ReadLine.Trim + """")
                End While


                Dim safefilename As String = file.Split("\").Last.Split(".").First

                cmps.Add(safefilename, argllist)

                sr.Close()
                f.Close()
            Next
        End Sub

        Public Function GetArgTypeList() As List(Of String)
            Return cmps.Keys.ToList
        End Function
        Public Function GetArgTypeArray() As String()
            Return cmps.Keys.ToArray
        End Function


        Public Function GetDefaultArgTypeList() As List(Of String)
            Return DefaultArgList.ToList
        End Function
        Private DefaultArgList() As String = {"TrgAllyStatus", "TrgComparison", "TrgCount", "TrgModifier", "TrgOrder",
            "TrgPlayer", "TrgProperty", "TrgPropState", "TrgResource", "TrgScore", "TrgSwitchAction", "TrgSwitchState",
            "TrgAIScript", "TrgLocation", "TrgSwitch", "TrgUnit", "WAVName", "BGM", "FormatString", "Arguments", "Tbl", "UnitsDat", "WeaponsDat", "FlingyDat",
            "SpritesDat", "ImagesDat", "UpgradesDat", "TechdataDat", "OrdersDat", "Weapon", "Flingy",
            "Sprite", "Image", "Upgrade", "Tech", "Order", "Icon", "Portrait", "EUDScore", "SupplyType"}




        Public Function GetAutocmp(argname As String) As String()
            If (argname Is Nothing) Then
                Return {}
            End If
            If cmps.ContainsKey(argname) Then
                Return cmps(argname).ToArray
            ElseIf cmps.ContainsKey(argname + "+FLAG") Then
                Return cmps(argname + "+FLAG").ToArray
            Else
                Return {}
            End If
        End Function
















        Public Function GetFileSize(filepath As String) As ULong
            Dim fileinfo As FileInfo = My.Computer.FileSystem.GetFileInfo(filepath)

            Return fileinfo.Length
        End Function





        Public Function GetLanText(Str As String) As String
            Dim rstr As String = GetText(Str)
            If rstr = "" Then
                Return Str
            End If
            Return rstr
        End Function


        Public DotPaint As New DotPaint



        Public ErrorBitmap As BitmapSource

        Public BlackOverlaybitmap As BitmapSource




        Public ReadOnly Property CascData As CascData
            Get
                If tCascData Is Nothing Then
                    If My.Computer.FileSystem.DirectoryExists(Tool.StarCraftPath) Then
                        tCascData = New CascData
                    Else
                        Return Nothing
                    End If
                End If
                Return tCascData
            End Get
        End Property



        Private tCascData As CascData

        Public SaveProjectDialog As System.Windows.Forms.SaveFileDialog
        Public TEEpsDefaultFunc As CFunc
        Public EpsImportManager As EpsImportManager
        Public LuaImportManager As LuaImportManager
        Public SCAScriptImportManager As SCALuaImportManager

        'Private MainWindow As MainWindow
        Public Sub Init()
            LoadArgs()
            SaveProjectDialog = New System.Windows.Forms.SaveFileDialog
            SaveProjectDialog.Filter = GetText("SaveFliter")

            OffsetDicInit()

            EpsImportManager = New EpsImportManager()
            EpScriptDefaultCompletionData.GetArgDataList = AddressOf GetArgList
            EpScriptDefaultCompletionData.GetArgKeyWordList = AddressOf GetArgTypeArray


            LuaImportManager = New LuaImportManager()
            LuaDefaultCompletionData.GetArgDataList = AddressOf GetArgList
            LuaDefaultCompletionData.GetArgKeyWordList = AddressOf GetArgTypeArray


            SCAScriptImportManager = New SCALuaImportManager()
            LuaDefaultCompletionData.GetArgDataList = AddressOf GetArgList
            LuaDefaultCompletionData.GetArgKeyWordList = AddressOf GetArgTypeArray


            TEEpsDefaultFunc = New CFunc


            Dim fs As New FileStream(TriggerEditorPath("epsFunctions.txt"), FileMode.Open)
            Dim sr As New StreamReader(fs)
            Try
                TEEpsDefaultFunc.LoadFunc(sr.ReadToEnd)
            Catch ex As Exception
                CustomMsgBox("함수 초기화 실패", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
            sr.Close()
            fs.Close()

            BlackOverlaybitmap = New BitmapImage(New Uri(ResourcesPath("BlackOverlay.png")))
            ErrorBitmap = New BitmapImage(New Uri(ResourcesPath("NoFrame.png")))


            CodeGrouping = New CodeGrouping
        End Sub
        Public CodeGrouping As CodeGrouping


        Private TextBlockColorTable() As SolidColorBrush = {
        Nothing, '기본
        New SolidColorBrush(Color.FromRgb(86, 156, 214)),'연파
         New SolidColorBrush(Color.FromRgb(128, 193, 132)), '연초 enum색
         New SolidColorBrush(Color.FromRgb(72, 180, 142)),'청록
         New SolidColorBrush(Color.FromRgb(181, 206, 168)), '연초2
         New SolidColorBrush(Color.FromRgb(255, 167, 167))} '붉은색 (주의)
        Public Function TextColorBlock(textstr As String, Optional colorinvert As Boolean = True) As TextBlock
            Dim TextBlcck As New TextBlock

            Dim inlines As InlineCollection = TextBlcck.Inlines
            inlines.Clear()
            Dim MainText As String = textstr '.Replace(vbCrLf, "<A>")


            Dim rgx As New Text.RegularExpressions.Regex("<([A-Za-z0-9])+>", Text.RegularExpressions.RegexOptions.IgnoreCase)

            Dim LastColor As Integer = 0
            Dim LastCode As Integer = 0
            Dim Startindex As Integer = 1
            For i = 0 To rgx.Matches(MainText).Count - 1
                Dim tMatch As Text.RegularExpressions.Match = rgx.Matches(MainText).Item(i)

                Dim Value As String = tMatch.Value
                Value = Mid(Value, 2, Value.Length - 2)

                Dim ColorCode As Integer = -1
                Try
                    ColorCode = "&H" & Value
                    If ColorCode > TextBlockColorTable.Count - 1 Then
                        Continue For
                    End If
                Catch ex As Exception
                    Continue For
                End Try

                Dim AddedText As String = Mid(MainText, Startindex, tMatch.Index - Startindex + 1)


                Dim Run As New Run(AddedText)
                If LastColor = 0 Then
                    If colorinvert Then
                        Run.Foreground = Application.Current.Resources("MaterialDesignPaper")
                    Else
                        Run.Foreground = Application.Current.Resources("MaterialDesignBody")
                    End If
                Else
                    Run.Foreground = TextBlockColorTable(LastColor)
                End If

                inlines.Add(Run)
                Startindex = tMatch.Index + Value.Length + 3
                LastCode = ColorCode
                If ColorCode <> -1 Then
                    LastColor = ColorCode
                End If
                If LastCode = &HA Then
                    inlines.Add(vbCrLf)
                    LastColor = 0
                End If
            Next

            If True Then
                Dim AddedText As String = Mid(MainText, Startindex, MainText.Length - Startindex + 1)
                Dim Run As New Run(AddedText)
                If LastColor = 0 Then
                    If colorinvert Then
                        Run.Foreground = Application.Current.Resources("MaterialDesignPaper")
                    Else
                        Run.Foreground = Application.Current.Resources("MaterialDesignBody")
                    End If
                Else
                    Run.Foreground = TextBlockColorTable(LastColor)
                End If
                inlines.Add(Run)
            End If

            Return TextBlcck
        End Function

        Public Function ParameterParser(PName As String, Datname As String) As String
            If ParameterList.IndexOf(PName) >= 0 Then
                Return PName
            End If

            For i = 0 To ParameterList.Count - 1
                'MsgBox(GetText(ParamaterList(i)) & " " & PName)
                If GetText(Datname & "_" & ParameterList(i)) = PName Then
                    Return ParameterList(i)
                End If
            Next


            Return PName
        End Function


        Private ParameterList As List(Of String)
        Private OffsetDic As Dictionary(Of String, UInteger)
        Public Function GetOffset(dat As SCDatFiles.DatFiles, ParamName As String) As UInteger
            Return OffsetDic(Datfilesname(dat) & "_" & ParamName)
        End Function
        Public Function GetOffset(FullName As String) As UInteger
            Return OffsetDic(FullName)
        End Function
        Private Sub OffsetDicInit()
            ParameterList = New List(Of String)
            OffsetDic = New Dictionary(Of String, UInteger)
            Dim fs As New FileStream(OffsetPath, FileMode.Open)
            Dim sr As New StreamReader(fs)

            While Not sr.EndOfStream
                Dim str As String = sr.ReadLine()

                Dim Key As String = str.Split("=")(0)
                Dim Value As UInteger = "&H" & str.Split("=")(1).Remove(0, 2)

                OffsetDic.Add(Key, Value)
                If Key.IndexOf("_") <> -1 Then
                    ParameterList.Add(Key.Split("_")(1))
                End If
            End While



            sr.Close()
            fs.Close()
        End Sub

        Public Function GetRelativePath(BasePath As String, RelativePath As String) As String
            Dim resultPath As String = ""

            'MsgBox("원본 주소 :" & BasePath & vbCrLf & "대상 주소 :" & RelativePath)
            Dim BaseSplit() As String = BasePath.Split("\")
            Dim RelativeSplit() As String = RelativePath.Split("\")

            For index = 0 To Math.Min(BaseSplit.Count, RelativeSplit.Count) - 1
                If index = 0 And BaseSplit(index) <> RelativeSplit(index) Then
                    'MsgBox("드라이버가 다르잖아 ㅅㅂ..")
                    Return RelativePath
                End If
                If BaseSplit(index) <> RelativeSplit(index) Then
                    For i = 0 To BaseSplit.Count - index - 2
                        resultPath = resultPath & "..\"
                    Next
                    For i = index To RelativeSplit.Count - 1
                        If i = index Then
                            resultPath = resultPath & RelativeSplit(i)
                        Else
                            resultPath = resultPath & "\" & RelativeSplit(i)
                        End If
                    Next
                    Exit For
                End If
            Next

            'zzz\ asd \ c \ ㅎㅎ.txt
            '4개

            'zzz\ asd \ bcx \ aqw \ zxv \ 하이.txt
            '6개

            '..\..\..\c\ㅎㅎ.txt

            '2부터 다름.

            '6 - 2 - 1 = 3개의 ..\를 넣고
            '4 - 2 = 2의 주소부터 넣는다.

            'MsgBox(resultPath)
            Return resultPath
        End Function



        Public ReadOnly ProhibitParam() As String = {"Unit Map String", "Unknown1", "Unknown2", "Health Bar", "Sel.Circle Image", "Sel.Circle Offset", "Unused", "Unknown 4", "Unknown6", "Unknown17"}


        Public Sub CloseOtherWindow()
            For Each win As Window In Application.Current.Windows
                'MsgBox(win.GetType.ToString)
                If win.GetType IsNot GetType(MainWindowD) Then
                    win.Close()
                End If
                'If win.GetType Is GetType(DataEditor) Or win.GetType Is GetType(TriggerEditor) Or win.GetType Is GetType(PluginWindow) Then
                '    win.Close()
                'End If
            Next
        End Sub

        Public ReadOnly Property StarCraftPath() As String
            Get
                Return pgData.Setting(ProgramData.TSetting.starcraft).Replace("StarCraft Launcher.exe", "")
            End Get
        End Property

        Private ReadOnly MPQFiles() As String = {"patch_rt.mpq", "patch_ed.mpq", "BrooDat.mpq", "BroodWar.mpq", "StarDat.mpq"}
        Public Function LoadDataFromMPQ(filename As String) As Byte()
            Return CascData.ReadFile(filename)

            'Dim hmpq As UInteger
            'Dim hfile As UInteger
            'Dim buffer() As Byte
            'Dim filesize As UInteger

            'Dim pdwread As IntPtr

            'For i = 0 To MPQFiles.Count - 1
            '    Dim mpqname As String = StarCraftPath & MPQFiles(i)
            '    SFmpq.SFileOpenArchive(mpqname, 0, 0, hmpq)


            '    SFmpq.SFileOpenFileEx(hmpq, filename, 0, hfile)

            '    If hfile <> 0 Then
            '        filesize = SFmpq.SFileGetFileSize(hfile, filesize)
            '        ReDim buffer(filesize)

            '        SFmpq.SFileReadFile(hfile, buffer, filesize, pdwread, 0)

            '        SFmpq.SFileCloseFile(hfile)
            '        SFmpq.SFileCloseArchive(hmpq)
            '        Return buffer
            '    End If
            '    SFmpq.SFileCloseArchive(hmpq)
            'Next

            Tool.ErrorMsgBox("File Load Fail from MPQ. " & filename)
            Throw New System.Exception("File Load Fail from MPQ. " & filename)
            Return Nothing
        End Function

        Public Function LoadDataFromMAP(filename As String) As Byte()
            Dim hmpq As UInteger
            Dim hfile As UInteger
            Dim buffer() As Byte = Nothing
            Dim filesize As UInteger

            Dim pdwread As IntPtr


            Dim mpqname As String = pjData.OpenMapName
            StormLib.SFileOpenArchive(mpqname, 0, 0, hmpq)
            filename = filename.Replace("/", "\")

            StormLib.SFileOpenFileEx(hmpq, filename, 0, hfile)

            If hfile <> 0 Then
                filesize = StormLib.SFileGetFileSize(hfile, filesize)
                ReDim buffer(filesize)

                StormLib.SFileReadFile(hfile, buffer, filesize, pdwread, 0)

                StormLib.SFileCloseFile(hfile)
                StormLib.SFileCloseArchive(hmpq)
                Return buffer
            End If
            StormLib.SFileCloseArchive(hmpq)
            Return buffer
        End Function



        Public Sub PlaySoundFromMPQ(filename As String)
            If Not pgData.Setting(ProgramData.TSetting.MuteSound) Then
                Dim bytes() As Byte = LoadDataFromMPQ(filename)


                Dim sp As New SoundPlayer(New IO.MemoryStream(bytes))
                sp.Play()
            End If
        End Sub

        Public Sub PlaySoundFromMPQIndex(soundindex As Integer)
            Dim pureFilename As String = scData.SfxFileName(pjData.Dat.Data(SCDatFiles.DatFiles.sfxdata, "Sound File", soundindex) - 1)
            pureFilename = Replace(pureFilename, pureFilename.Split(".").Last, "") & "wav"


            PlaySoundFromMPQ("sound/" & pureFilename)
        End Sub

        Public Function IsProjectLoad() As Boolean
            If pjData IsNot Nothing Then
                If pjData.IsLoad Then
                    Return True
                End If
            End If
            Return False
        End Function


        Public Function GetText(Text As String) As String
            Return Application.Current.Resources(Text)
        End Function



        Public Function GetText(Text As String, ParamArray list() As String) As String
            Dim t As String = Application.Current.Resources(Text)

            For i = 0 To list.Length - 1
                t = t.Replace("%S" & i + 1, list(i))

            Next
            Return t
        End Function


        Public Function CustomMsgBox(message As String, boxButton As MessageBoxButton, Optional boxImage As MessageBoxImage = MessageBoxImage.Information)
            Dim dailog As MsgDialog = New MsgDialog(message, "", boxButton, boxImage)
            dailog.ShowDialog()
            Return dailog.msgresult
        End Function



        Public Sub ErrorMsgBox(str As String, Optional Logstr As String = "")

            Try
                Dim msg As MsgDialog = New MsgDialog(str, Logstr, MessageBoxButton.OK, MessageBoxImage.Error)
                msg.ShowDialog()
            Catch ex As Exception
                MsgBox(str, MsgBoxStyle.Critical, Tool.GetText("ErrorMsgbox"))
            End Try

            'MsgBox(str, MsgBoxStyle.Critical, Tool.GetText("ErrorMsgbox"))


        End Sub


        Public Function CreateMapSet(sWindow As SettingWindows) As Boolean
            '우선 맵의 유형을 정한다.
            Dim sampleWindow As New SampleMapSetting
            sampleWindow.Owner = sWindow

            sampleWindow.ShowDialog()

            If Not sampleWindow.IsOkay Then
                Return False
            End If



            '맵을 저장할 위치를 정한다.
            Dim savedialog As New System.Windows.Forms.SaveFileDialog With {
            .Filter = Tool.GetText("SCX Fliter"),
            .Title = Tool.GetText("SCX Save Select"),
            .OverwritePrompt = True
            }


            Dim LastOpenMapName As String = pjData.OpenMapName
            '맵이 저장맵이랑 이름이 같은지 검사해야됨.
            If savedialog.ShowDialog() = Forms.DialogResult.OK Then
                If pjData.SaveMapName = savedialog.FileName Then
                    Tool.ErrorMsgBox(Tool.GetText("Error OpenMap is not SaveMap"))
                    Return False
                End If
            Else
                Return False
            End If




            '맵을 생성합니다.
            My.Computer.FileSystem.CopyFile(sampleWindow.SelectSampleMap, savedialog.FileName, True)
            pjData.OpenMapName = savedialog.FileName
            Return True





            ''맵이 플텍맵인지 아닌지 검사해야됨.
            'pjData.OpenMapName = savedialog.FileName
            'If pjData.IsMapLoading Then
            '    Return True
            'Else
            '    pjData.OpenMapName = LastOpenMapName
            '    '샘플맵이 프로텍트 맵입니다.


            '    '생성된 맵을 다시 삭제합니다.
            '    My.Computer.FileSystem.DeleteFile(savedialog.FileName)


            '    Return False
            'End If

        End Function

        Public Function OpenMapSet() As Boolean
            Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
            .Filter = Tool.GetText("SCX Fliter"),
            .Title = Tool.GetText("SCX Select"),
            .InitialDirectory = pgData.Setting(ProgramData.TSetting.OpenMapPath)
            }
            Dim LastOpenMapName As String = pjData.OpenMapName
            '맵이 플텍맵인지 아닌지 검사해야됨.
            '맵이 저장맵이랑 이름이 같은지 검사해야됨.
            If opendialog.ShowDialog() = Forms.DialogResult.OK Then
                If pjData.SaveMapName = opendialog.FileName Then
                    Tool.ErrorMsgBox(Tool.GetText("Error OpenMap is not SaveMap"))
                    Return False
                End If

                '맵이 플텍맵인지 아닌지 검사해야됨.
                pjData.OpenMapName = opendialog.FileName
                If pjData.IsMapLoading Then
                    pgData.Setting(ProgramData.TSetting.OpenMapPath) = Path.GetDirectoryName(opendialog.FileName)
                    Return True
                Else
                    pjData.OpenMapName = LastOpenMapName
                    Return False
                End If
            End If
            Return False
        End Function

        Public Function SaveMapSet() As Boolean
            Dim savedialog As New System.Windows.Forms.SaveFileDialog With {
            .Filter = Tool.GetText("SCX Fliter"),
            .Title = Tool.GetText("SCX Save Select"),
            .InitialDirectory = pgData.Setting(ProgramData.TSetting.SaveMapPath),
            .OverwritePrompt = False
         }


            '맵이 오픈맵이랑 이름이 같은지 검사해야됨.
            If savedialog.ShowDialog() = Forms.DialogResult.OK Then
                If pjData.OpenMapName = savedialog.FileName Then
                    Tool.ErrorMsgBox(Tool.GetText("Error OpenMap is not SaveMap"))
                    Return False
                End If

                pjData.SaveMapName = savedialog.FileName
                pgData.Setting(ProgramData.TSetting.SaveMapPath) = Path.GetDirectoryName(pjData.SaveMapName)
                Return True
            End If
            Return False
        End Function


        Public ReadOnly Property AutocmpFolderPath() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\TriggerEditor\Autocmp"
            End Get
        End Property
        Public ReadOnly Property SampleDataFolderPath() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Sample"
            End Get
        End Property
        Public ReadOnly Property TriggerEditorPath(paths As String) As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\TriggerEditor\" & paths
            End Get
        End Property

        Public ReadOnly Property ResourcesPath(Filename As String) As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Resources\" & Filename
            End Get
        End Property
        Public ReadOnly Property DataPath(Filename As String) As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\" & Filename
            End Get
        End Property
        Public Sub CreateDataPath(Filename As String)
            Dim tpath As String = System.AppDomain.CurrentDomain.BaseDirectory & "Data\" & Filename
            If Not My.Computer.FileSystem.DirectoryExists(tpath) Then
                My.Computer.FileSystem.CreateDirectory(tpath)
            End If
        End Sub
        Public ReadOnly Property GRPSaveFilePath As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "\Data\GRPDATA"
            End Get
        End Property
        Public ReadOnly Property FiregraftActFunPath() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Texts\FireGraftActFun.txt"
            End Get
        End Property
        Public ReadOnly Property FiregraftConFunPath() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Texts\FireGraftConFun.txt"
            End Get
        End Property
        Public ReadOnly Property OffsetPath() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Offset.txt"
            End Get
        End Property
        Public ReadOnly Property GetSettingFile() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Setting.ini"
            End Get
        End Property
        Public ReadOnly Property GetDatFolder() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\DatFiles"
            End Get
        End Property

        Public ReadOnly Property GetCodeEditorSettingFolder() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory
            End Get
        End Property

        Public Function GetTitleName() As String
            If pjData IsNot Nothing Then
                If pjData.IsLoad Then
                    Return GetProjectName() & " - EUD Editor 3 v" & pgData.Version.ToString
                End If
            End If
            Return "EUD Editor 3 v" & pgData.Version.ToString
        End Function
        Public Function GetProjectName() As String
            If pjData IsNot Nothing Then
                If pjData.IsLoad Then
                    Dim IsDirty As String = ""
                    Dim Filename As String = pjData.SafeFilename
                    If pjData.IsDirty Then
                        IsDirty = "*"
                    End If
                    If pjData.SafeFilename = "" Then
                        Filename = GetText("NoName")
                    End If

                    Return Filename & IsDirty
                End If
            End If
            Return ""
        End Function


        Public ReadOnly Property GetLanguageFolder() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Language"
            End Get
        End Property

        Public ReadOnly Property GetTblFolder() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Tbls"
            End Get
        End Property

        Public ReadOnly Property GetRegSettingFile() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\RegSetting.exe"
            End Get
        End Property

        Public ReadOnly Property GetDownloaderFile() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Downloader\EUDEditorDownloader.exe"
            End Get
        End Property

        Public Function GetDirectoy(Path As String) As String
            Dim BaseDirectPath As String = System.AppDomain.CurrentDomain.BaseDirectory
            Dim PathBlock() As String = Path.Split("\")

            Dim FullPath As String = BaseDirectPath & Path

            If Not My.Computer.FileSystem.DirectoryExists(FullPath) Then
                For index = 0 To PathBlock.Count - 1
                    FullPath = BaseDirectPath
                    For i = 0 To index
                        If i = 0 Then
                            FullPath = FullPath & PathBlock(i)
                        Else
                            FullPath = FullPath & "\" & PathBlock(i)
                        End If
                    Next
                    If Not My.Computer.FileSystem.DirectoryExists(FullPath) Then
                        My.Computer.FileSystem.CreateDirectory(FullPath)
                    End If
                Next
            End If

            Return BaseDirectPath & Path
        End Function
        Public Function GetDirectory(BaseDirect As String, Path As String) As String
            Dim BaseDirectPath As String = BaseDirect
            Dim PathBlock() As String = Path.Split("\")

            If Path = "" Or BaseDirectPath = "" Then
                Return ""
            End If

            Dim FullPath As String = BaseDirectPath & Path
            If Not My.Computer.FileSystem.DirectoryExists(FullPath) Then
                For index = 0 To PathBlock.Count - 1
                    FullPath = BaseDirectPath
                    For i = 0 To index
                        If i = 0 Then
                            FullPath = FullPath & PathBlock(i)
                        Else
                            FullPath = FullPath & "\" & PathBlock(i)
                        End If
                    Next
                    If Not My.Computer.FileSystem.DirectoryExists(FullPath) Then
                        My.Computer.FileSystem.CreateDirectory(FullPath)
                    End If
                Next
            End If

            Return BaseDirectPath & Path
        End Function

        Public Function StripFileName(ByVal name As String) As String
            Return System.Text.RegularExpressions.Regex.Replace(name, "[\\/:*?""<>|]", "_")
        End Function



        Public Function isFileLock(filePath As String) As Boolean
            Dim f As New FileInfo(filePath)

            Dim stream As FileStream = Nothing

            Try
                stream = f.Open(FileMode.Open, FileAccess.Read, FileShare.None)
            Catch __unusedIOException1__ As IOException
                Return True
            Finally
                If stream IsNot Nothing Then stream.Close()
            End Try

            Return False
        End Function


        Public Function OpenTexteditor(str As String) As String
            For Each win As Window In Application.Current.Windows
                win.UpdateLayout()
            Next
            Application.Current.MainWindow.Visibility = Visibility.Visible

            Dim Window As New TextEditorWindow(str)
            Window.Owner = Application.Current.MainWindow
            Window.ShowDialog()

            Return Window.TextString
        End Function


        Public Sub RefreshAllWindow()
            For Each win As Window In Application.Current.Windows
                win.UpdateLayout()
            Next
        End Sub

        Public Sub RefreshMainWindow()
            Try
                ProjectControlBinding.PropertyChangedPack()
            Catch ex As Exception

            End Try
        End Sub


        Public Sub SetRegistry()
            Dim str As String() = {"e3s"} ', "e3p", "e2s", "e2p", "ees", "mem"}

            For Each Extension As String In str
                My.Computer.Registry.ClassesRoot.CreateSubKey("." & Extension & "").SetValue("",
                    "" & Extension & "", Microsoft.Win32.RegistryValueKind.String)
                My.Computer.Registry.ClassesRoot.CreateSubKey("" & Extension & "\shell\open\command").SetValue("",
                System.Windows.Forms.Application.ExecutablePath & " ""%l"" ", Microsoft.Win32.RegistryValueKind.String)
                My.Computer.Registry.ClassesRoot.CreateSubKey("" & Extension & "\DefaultIcon").SetValue("",
               System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Icons\" & Extension & ".ico" & ",0", Microsoft.Win32.RegistryValueKind.String)
            Next

        End Sub

        Public Sub StartRegSetter(Argument As String)
            Dim p As New Process()
            p.StartInfo.FileName = GetRegSettingFile
            p.StartInfo.UseShellExecute = True
            p.StartInfo.Verb = "runas" 'Verb를 runas로 (관리자 권한으로 실행 명령)
            p.StartInfo.Arguments = Argument

            p.Start()
        End Sub

        Public Sub StartUpdaterSetter()
            Dim Argument As String
            Argument = pgData.Setting(ProgramData.TSetting.PrimaryHueMidBrush)
            Argument = Argument & "," & pgData.Setting(ProgramData.TSetting.PrimaryHueMidForegroundBrush)
            Argument = Argument & "," & pgData.Setting(ProgramData.TSetting.PrimaryHueDarkBrush)


            Dim p As New Process()
            p.StartInfo.FileName = GetDownloaderFile
            p.StartInfo.Arguments = Argument

            p.Start()
        End Sub

        Public Function CheckexeConnect(Extension As String) As Boolean
            Dim RegistryKeys As String = My.Computer.Registry.GetValue("HKEY_CLASSES_ROOT\" & Extension & "\shell\open\command", Nothing, "")
            If RegistryKeys Is Nothing Then
                Return True
            End If

            Dim RegisPath As String
            Try
                RegisPath = RegistryKeys.Replace(RegistryKeys.Split(".").Last, "") & "exe"
            Catch ex As Exception
                Return True
            End Try




            Return RegisPath <> System.Windows.Forms.Application.ExecutablePath
        End Function
    End Module
End Namespace


Namespace TabItemTool
    Module TabItemTool
        Public Sub WindowTabItem(Datfile As SCDatFiles.DatFiles, index As Integer)
            Dim DataEditorForm As New DataEditor(GetTabItem(Datfile, index), Datfile, index)
            'DataEditorForm.OpenbyOthers(GetTabItem(Datfile, index), Datfile)
            DataEditorForm.Show()
        End Sub

        Private TabTypeArray As Type() = {
            GetType(UnitData),
            GetType(WeaponData),
            GetType(FlingyData),
            GetType(SpriteData),
            GetType(ImageData),
            GetType(UpgradeData),
            GetType(TechData),
            GetType(OrderData),
            Nothing,
            Nothing,
            Nothing,
            GetType(StatTxtData), '11
            Nothing,
            Nothing,
            Nothing,
            Nothing,
            Nothing,'16
            Nothing,
            Nothing,
            Nothing,
            GetType(ButtonData)'20
        }


        'units = 0
        'weapons = 1
        'flingy = 2
        'sprites = 3
        'images = 4
        'upgrades = 5
        'techdata = 6
        'orders = 7
        'portdata = 8
        'sfxdata = 9

        'Icon = 10
        'stattxt = 11
        'IscriptID = 12

        ''Firegraft
        'statusinfor = 13
        'wireframe = 14
        'Unitrequire = 15
        'Upgraderequire = 16
        'TechResearchrequire = 17
        'TechUserequire = 18
        'Orderrequire = 19
        'button = 20
        Public Sub ChanageTabItem(Datfile As SCDatFiles.DatFiles, index As Integer, MainTab As Dockablz.Layout)
            Dim MainContent As Object = MainTab.Content
            While MainContent.GetType <> GetType(TabablzControl)
                Select Case MainContent.GetType
                    Case GetType(TabablzControl)
                        Exit While
                    Case GetType(Dockablz.Branch)
                        Dim tBranch As Dockablz.Branch = MainContent
                        MainContent = tBranch.FirstItem
                End Select
            End While

            Dim TabContent As TabablzControl = MainContent



            If TabContent.Items.Count <> 0 Then
                Dim ChangesTabItem As TabItem = TabContent.Items(0)
                If ChangesTabItem.Content.GetType() = TabTypeArray(Datfile) Then '같은거 일 경우
                    Dim TGrid As Grid = ChangesTabItem.Header
                    Dim TabText As TextBlock = TGrid.Children.Item(0)


                    Dim myBinding As Binding = New Binding("TabName")
                    myBinding.Source = pjData.BindingManager.UIManager(Datfile, index)
                    TabText.SetBinding(TextBlock.TextProperty, myBinding)


                    ChangesTabItem.Content.ReLoad(Datfile, index)
                    TabContent.SelectedItem = ChangesTabItem
                Else
                    Dim TabItem As TabItem = GetTabItem(Datfile, index)
                    TabContent.Items.RemoveAt(0)
                    TabContent.Items.Insert(0, TabItem)
                    TabContent.SelectedItem = TabItem
                End If

            Else
                Dim TabItem As TabItem = GetTabItem(Datfile, index)
                TabContent.Items.Add(TabItem)
                TabContent.SelectedItem = TabItem
            End If
        End Sub
        Public Sub PlusTabItem(Datfile As SCDatFiles.DatFiles, index As Integer, MainTab As Dockablz.Layout)
            Dim MainContent As Object = MainTab.Content
            While MainContent.GetType <> GetType(TabablzControl)
                Select Case MainContent.GetType
                    Case GetType(TabablzControl)
                        Exit While
                    Case GetType(Dockablz.Branch)
                        Dim tBranch As Dockablz.Branch = MainContent
                        MainContent = tBranch.FirstItem
                End Select
            End While

            Dim TabContent As TabablzControl = MainContent

            Dim TabItem As TabItem = GetTabItem(Datfile, index)
            TabContent.Items.Add(TabItem)
            TabContent.SelectedItem = TabItem
        End Sub

        Public Function GetTabItem(Datfile As SCDatFiles.DatFiles, index As Integer) As TabItem
            Dim TabItem As New TabItem
            Dim TabGrid As New Grid
            Dim TabText As New TextBlock
            Dim TabContextMenu As New ContextMenu
            'TabText.Text = pjData.CodeLabel(CodePage, index)

            TabText.SetResourceReference(TextBlock.ForegroundProperty, "PrimaryHueMidForegroundBrush")
            TabText.HorizontalAlignment = HorizontalAlignment.Center
            TabText.VerticalAlignment = VerticalAlignment.Center





            Dim TabCloseCommand As New TabCloseCommand(TabItem)

            Dim RightCloseMenuItem As New MenuItem
            Dim OtherCloseMenuItem As New MenuItem
            If True Then
                Dim tabmenuitem As New MenuItem
                tabmenuitem.Header = Tool.GetText("TabClose")
                tabmenuitem.Command = TabablzControl.CloseItemCommand

                Dim PIcon As New PackIcon()
                PIcon.Kind = PackIconKind.Close
                tabmenuitem.Icon = PIcon
                TabContextMenu.Items.Add(tabmenuitem)
            End If

            If True Then
                RightCloseMenuItem.Header = Tool.GetText("RightTabsClose")
                RightCloseMenuItem.CommandParameter = TabCloseCommand.CommandType.RightClose
                RightCloseMenuItem.Command = TabCloseCommand

                Dim PIcon As New PackIcon()
                PIcon.Kind = PackIconKind.ArrowExpandRight
                RightCloseMenuItem.Icon = PIcon
                TabContextMenu.Items.Add(RightCloseMenuItem)
            End If

            If True Then
                OtherCloseMenuItem.Header = Tool.GetText("OtherTabsClose")
                OtherCloseMenuItem.CommandParameter = TabCloseCommand.CommandType.OtherClose
                OtherCloseMenuItem.Command = TabCloseCommand

                Dim PIcon As New PackIcon()
                PIcon.Kind = PackIconKind.ArrowSplitVertical
                OtherCloseMenuItem.Icon = PIcon
                TabContextMenu.Items.Add(OtherCloseMenuItem)
            End If

            Dim TabCloseEnabled As New TabCloseEnabled(TabItem, RightCloseMenuItem, OtherCloseMenuItem)
            TabItem.AddHandler(MenuItem.ContextMenuOpeningEvent, New RoutedEventHandler(AddressOf TabCloseEnabled.OpenEvent))

            'TabGrid.Background = Application.Current.Resources("PrimaryHueMidBrush")
            TabGrid.ContextMenu = TabContextMenu
            TabGrid.Height = 34
            TabGrid.Margin = New Thickness(0, -5, 0, -5)
            TabGrid.Children.Add(TabText)


            TabItem.Header = TabGrid


            Dim myBinding As Binding = New Binding("TabName")
            Select Case Datfile
                Case SCDatFiles.DatFiles.units
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.units, index)
                    TabItem.Content = New UnitData(index)
                Case SCDatFiles.DatFiles.weapons
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.weapons, index)
                    TabItem.Content = New WeaponData(index)
                Case SCDatFiles.DatFiles.flingy
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.flingy, index)
                    TabItem.Content = New FlingyData(index)
                Case SCDatFiles.DatFiles.sprites
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.sprites, index)
                    TabItem.Content = New SpriteData(index)
                Case SCDatFiles.DatFiles.images
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.images, index)
                    TabItem.Content = New ImageData(index)
                Case SCDatFiles.DatFiles.upgrades
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.upgrades, index)
                    TabItem.Content = New UpgradeData(index)
                Case SCDatFiles.DatFiles.techdata
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.techdata, index)
                    TabItem.Content = New TechData(index)
                Case SCDatFiles.DatFiles.orders
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.orders, index)
                    TabItem.Content = New OrderData(index)
                Case SCDatFiles.DatFiles.stattxt
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.stattxt, index)
                    TabItem.Content = New StatTxtData(index)
                Case SCDatFiles.DatFiles.ButtonData
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.ButtonData, index)
                    TabItem.Content = New ButtonData(index)
            End Select
            TabText.SetBinding(TextBlock.TextProperty, myBinding)

            Return TabItem
        End Function

        Public Sub RefreshExplorer(SelfObject As ProjectExplorer)
            For Each win As Window In Application.Current.Windows
                If win.GetType Is GetType(TriggerEditor) Then
                    If SelfObject IsNot CType(win, TriggerEditor).Explorer Then
                        CType(win, TriggerEditor).Explorer.ResetList()
                    End If
                End If
            Next
        End Sub
    End Module


End Namespace