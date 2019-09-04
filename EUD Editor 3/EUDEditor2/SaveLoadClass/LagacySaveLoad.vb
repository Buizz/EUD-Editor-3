Imports System.IO
Imports System.Text

Public Class LagacySaveLoad
    Private Function FindSection(base As String, key As String) As String
        Try
            Dim length = InStr(base, "E_" & key) - InStr(base, "S_" & key) - key.Count - 2

            Return Mid(base, InStr(base, "S_" & key) + key.Count + 4, length - 4)
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Private Function FindSetting(base As String, key As String, Optional strflag As Boolean = False) As String
        Try
            Dim text As String = Mid(base, InStr(base, key & " "))
            If InStr(text, vbCrLf) = 0 Then
                Return Mid(text, key.Count + 4)
            Else
                Return Mid(text, key.Count + 4, InStr(text, vbCrLf) - key.Count - 4)
            End If

        Catch ex As Exception
            If strflag Then
                Return ""
            Else
                Return "0"
            End If

        End Try
    End Function

    Public Sub Load(FileName As String)
        Dim fileinfo As New FileInfo(FileName)
        'Main.LastData = fileinfo.LastWriteTime
        Dim extension As String = Mid(FileName, FileName.Length - 3)
        Select Case extension
            Case ".e2s", ".e2p"
                Dim iszipfile As Boolean = False

                '파일이 존재할 경우 파일의 확장자를 확인해라
                If extension = ".e2p" Then
                    iszipfile = True
                End If

                Dim file As FileStream = Nothing

                file = New FileStream(FileName, FileMode.Open, FileAccess.Read)

                Dim stream As StreamReader = New StreamReader(file)

                Dim savefileVersion As String = ""
                Dim text As String = stream.ReadToEnd()
                stream.Close()
                file.Close()

                Dim isUseCHKData As Boolean
                Try
                    Dim Section_ProjectSET As String = FindSection(text, "ProjectSET")

                    savefileVersion = FindSetting(Section_ProjectSET, "Version")
                    If savefileVersion = "0" Then
                        Tool.ErrorMsgBox(Tool.GetText("Invalide2s"))
                        Exit Sub
                    End If





                    pjData.OpenMapName = FindSetting(Section_ProjectSET, "InputMap")
                    pjData.SaveMapName = FindSetting(Section_ProjectSET, "OutputMap")

                    isUseCHKData = FindSetting(Section_ProjectSET, "loadfromCHK")
                Catch ex As Exception
                    Tool.ErrorMsgBox(Tool.GetText("LodingError").Replace("$S0$", "Setting"))
                    Exit Sub
                End Try





                Try
                    Dim Section_DatEditSET As String = FindSection(text, "DatEditSET")

                    Dim i, j, k As Integer

                    Dim Setdata() As String = Section_DatEditSET.Split(vbCrLf)
                    For p = 0 To Setdata.Count - 1
                        If Setdata(p).Trim <> "" Then
                            Dim temp() As String = Setdata(p).Trim.Split(",")
                            i = temp(0)
                            j = temp(1)
                            k = temp(2)
                            If temp(3) <> 0 Then
                                Dim mapdata As Long = 0
                                If Not pjData.MapData.DatFile.GetDatFile(i).ParamaterList(j).PureData(k).IsDefault Then
                                    mapdata = pjData.MapData.DatFile.GetDatFile(i).ParamaterList(j).PureData(k).Data
                                Else
                                    mapdata = scData.DefaultDat.GetDatFile(i).ParamaterList(j).PureData(k).Data
                                End If



                                'mapdata += scData.DefaultDat.GetDatFile(i).ParamaterList(j).PureData(k).Data
                                pjData.Dat.GetDatFile(i).ParamaterList(j).PureData(k).Data = temp(3) + mapdata
                                pjData.Dat.GetDatFile(i).ParamaterList(j).PureData(k).IsDefault = False
                            End If
                        End If
                    Next
                Catch ex As Exception
                    Tool.ErrorMsgBox(Tool.GetText("LodingError").Replace("$S0$", "DatEdit"), ex.ToString)
                    Exit Sub
                End Try


                Try
                    Dim Section_FireGraftSET As String = FindSection(text, "FireGraftSET")


                    For i = 0 To 227
                        If FindSetting(Section_FireGraftSET, "FireGraft" & i) <> "0" Then
                            pjData.ExtraDat.StatusFunction1(i) = scData.DefaultExtraDat.StatusFunction1(i) + FindSetting(Section_FireGraftSET, "FireGraft" & i).Split(",")(0)
                            pjData.ExtraDat.StatusFunction2(i) = scData.DefaultExtraDat.StatusFunction2(i) + FindSetting(Section_FireGraftSET, "FireGraft" & i).Split(",")(1)

                            pjData.ExtraDat.DefaultStatusFunction1(i) = False
                            pjData.ExtraDat.DefaultStatusFunction2(i) = False
                        End If
                    Next


                    Dim Section_BtnSET As String = FindSection(text, "BtnSET")


                    For i = 0 To 249
                        If FindSetting(Section_BtnSET, "BtnUse" & i) = True Then
                            'MsgBox("버튼번호 : " & i & "   기본 설정 : " & pjData.ExtraDat.DefaultButtonSet(i))
                            'pjData.ExtraDat.DefaultButtonSet(i) = False 'FindSetting(Section_BtnSET, "BtnUse" & i)
                            pjData.ExtraDat.ButtonData.GetButtonSet(i).IsDefault = False

                            Dim Setdata() As String = FindSetting(Section_BtnSET, "BtnData" & i).Split(",")

                            pjData.ExtraDat.ButtonData.GetButtonSet(i).ButtonS.Clear()

                            For p = 0 To ((Setdata.Count - 1) \ 8) - 1
                                Dim btnData As New CButtonData()

                                btnData.pos = Setdata(0 + p * 8)
                                btnData.icon = Setdata(1 + p * 8)
                                btnData.con = Setdata(2 + p * 8)
                                btnData.act = Setdata(3 + p * 8)
                                btnData.conval = Setdata(4 + p * 8)
                                btnData.actval = Setdata(5 + p * 8)
                                btnData.enaStr = Setdata(6 + p * 8)
                                btnData.disStr = Setdata(7 + p * 8)
                                pjData.ExtraDat.ButtonData.GetButtonSet(i).ButtonS.Add(btnData)
                            Next

                        End If
                    Next

                    Dim DatTyps() As SCDatFiles.DatFiles = {SCDatFiles.DatFiles.units, SCDatFiles.DatFiles.upgrades,
                        SCDatFiles.DatFiles.techdata, SCDatFiles.DatFiles.Stechdata, SCDatFiles.DatFiles.orders}
                    Dim Section_ReqSET As String = FindSection(text, "ReqSET")
                    For i = 0 To DatTyps.Count - 1
                        For j = 0 To SCCodeCount(DatTyps(i)) - 1
                            Dim reqvalue As CRequireData.RequireUse = FindSetting(Section_ReqSET, "ReqUse" & i & "," & j)

                            If reqvalue = CRequireData.RequireUse.AlwaysCurrentUse Then
                                reqvalue = CRequireData.RequireUse.CustomUse
                            End If

                            pjData.ExtraDat.RequireData(DatTyps(i)).RequireObjectUsed(j) = reqvalue
                            If reqvalue = CRequireData.RequireUse.CustomUse Then '커스텀 유즈일 경우
                                Dim codes As New List(Of UShort)
                                For p As Integer = 0 To FindSetting(Section_ReqSET, "ReqCount" & i & "," & j) - 1
                                    codes.Add(FindSetting(Section_ReqSET, "ReqData" & i & "," & j & "," & p))
                                Next
                                pjData.ExtraDat.RequireData(DatTyps(i)).GetRequireObject(j).ReLoad(j, DatTyps(i), codes, FindSetting(Section_ReqSET, "ReqPos" & i & "," & j))

                                pjData.ExtraDat.RequireData(DatTyps(i)).GetRequireObject(j).UseStatus = CRequireData.RequireUse.CustomUse
                            End If


                        Next
                    Next
                Catch ex As Exception
                    Tool.ErrorMsgBox(Tool.GetText("LodingError").Replace("$S0$", "FireGraft"), ex.ToString)
                    Exit Sub
                End Try



                Try
                    Dim Section_FileManagerSET As String = FindSection(text, "FileManagerSET")


                    For i = 0 To FindSetting(Section_FileManagerSET, "stattextdicCount") - 1
                        pjData.ExtraDat.Stat_txt(FindSetting(Section_FileManagerSET, "stattextdickey" & i)) = LagacyClass.STRdecToHec(FindSetting(Section_FileManagerSET, "stattextdicvalue" & i))
                    Next

                    If FindSetting(Section_FileManagerSET, "wireuse") Then
                        For i = 0 To 227
                            If FindSetting(Section_FileManagerSET, "wireframData" & i) <> 0 Then
                                pjData.ExtraDat.WireFrame(i) = FindSetting(Section_FileManagerSET, "wireframData" & i) - 1
                                pjData.ExtraDat.DefaultWireFrame(i) = False
                            End If

                            If FindSetting(Section_FileManagerSET, "grpwireData" & i) <> 0 Then
                                pjData.ExtraDat.GrpFrame(i) = FindSetting(Section_FileManagerSET, "grpwireData" & i) - 1
                                pjData.ExtraDat.DefaultGrpFrame(i) = False
                            End If

                            If i < SCMenCount Then
                                If FindSetting(Section_FileManagerSET, "tranwireData" & i) <> 0 Then
                                    pjData.ExtraDat.TranFrame(i) = FindSetting(Section_FileManagerSET, "tranwireData" & i) - 1
                                    pjData.ExtraDat.DefaultTranFrame(i) = False
                                End If
                            End If
                        Next
                    End If


                Catch ex As Exception
                    Tool.ErrorMsgBox(Tool.GetText("LodingError").Replace("$S0$", "FileManager"), ex.ToString)
                    Exit Sub
                End Try




                'Try
                '    Dim Section_PluginSET As String = FindSection(text, "PluginSET")
                '    soundstopper = FindSetting(Section_PluginSET, "soundstopper")
                '    scmloader = FindSetting(Section_PluginSET, "scmloader")
                '    noAirCollision = FindSetting(Section_PluginSET, "noAirColsion")
                '    unlimiter = FindSetting(Section_PluginSET, "unlimiter")
                '    keepSTR = FindSetting(Section_PluginSET, "keepSTR")
                '    eudTurbo = FindSetting(Section_PluginSET, "eudTurbo")
                '    iscriptPatcher = FindSetting(Section_PluginSET, "iscriptPatcher")
                '    iscriptPatcheruse = FindSetting(Section_PluginSET, "iscriptPatcheruse")
                '    unpatcher = FindSetting(Section_PluginSET, "unpatcher")
                '    unpatcheruse = FindSetting(Section_PluginSET, "unpatcheruse")
                '    nqcuse = FindSetting(Section_PluginSET, "nqcuse")
                '    nqcunit = FindSetting(Section_PluginSET, "nqcunit")
                '    nqclocs = FindSetting(Section_PluginSET, "nqclocs", True)
                '    nqccommands = FindSetting(Section_PluginSET, "nqccommands", True)


                '    grpinjectoruse = FindSetting(Section_PluginSET, "grpinjectoruse")

                '    grpinjector_arrow = FindSetting(Section_PluginSET, "grpinjector_arrow")
                '    grpinjector_drag = FindSetting(Section_PluginSET, "grpinjector_drag")
                '    grpinjector_illegal = FindSetting(Section_PluginSET, "grpinjector_illegal")


                '    dataDumperuse = FindSetting(Section_PluginSET, "dataDumperuse")
                '    dataDumper_user = FindSetting(Section_PluginSET, "dataDumper_user")
                '    dataDumper_grpwire = FindSetting(Section_PluginSET, "dataDumper_grpwire")
                '    dataDumper_tranwire = FindSetting(Section_PluginSET, "dataDumper_tranwire")
                '    dataDumper_wirefram = FindSetting(Section_PluginSET, "dataDumper_wirefram")
                '    dataDumper_cmdicons = FindSetting(Section_PluginSET, "dataDumper_cmdicons")
                '    dataDumper_stat_txt = FindSetting(Section_PluginSET, "dataDumper_stat_txt")
                '    dataDumper_AIscript = FindSetting(Section_PluginSET, "dataDumper_AIscript")
                '    dataDumper_iscript = FindSetting(Section_PluginSET, "dataDumper_iscript")

                '    dataDumper_grpwire_f = FindSetting(Section_PluginSET, "dataDumper_grpwire_f")
                '    dataDumper_tranwire_f = FindSetting(Section_PluginSET, "dataDumper_tranwire_f")
                '    dataDumper_wirefram_f = FindSetting(Section_PluginSET, "dataDumper_wirefram_f")
                '    dataDumper_cmdicons_f = FindSetting(Section_PluginSET, "dataDumper_cmdicons_f")
                '    dataDumper_stat_txt_f = FindSetting(Section_PluginSET, "dataDumper_stat_txt_f")
                '    dataDumper_AIscript_f = FindSetting(Section_PluginSET, "dataDumper_AIscript_f")
                '    dataDumper_iscript_f = FindSetting(Section_PluginSET, "dataDumper_iscript_f")



                '    extraedssetting = FindSetting(Section_PluginSET, "extraedssetting")

                'Catch ex As Exception
                '    Tool.ErrorMsgBox(Tool.GetText("LodingError").Replace("$S0$", "Plugin"))
                '    Exit Sub
                'End Try

                'Try
                '    Dim Section_SCDBSET As String = FindSection(text, "SCDBSet")
                '    SCDBDeath = FindSetting(Section_SCDBSET, "SCDBDeath").Split({","}, StringSplitOptions.RemoveEmptyEntries).ToList
                '    SCDBLoc = FindSetting(Section_SCDBSET, "SCDBLoc").Split({","}, StringSplitOptions.RemoveEmptyEntries).ToList
                '    SCDBLocLoad = FindSetting(Section_SCDBSET, "SCDBLocLoad").Split({","}, StringSplitOptions.RemoveEmptyEntries).ToList
                '    SCDBVariable = FindSetting(Section_SCDBSET, "SCDBVariable").Split({","}, StringSplitOptions.RemoveEmptyEntries).ToList
                '    SCDBMaker = FindSetting(Section_SCDBSET, "SCDBMaker")
                '    SCDBMapName = FindSetting(Section_SCDBSET, "SCDBMapName")

                '    '호환성
                '    If SCDBLoc.Count <> SCDBLocLoad.Count Then
                '        SCDBLoc.Clear()
                '        SCDBLocLoad.Clear()
                '    End If

                '    SCDBUse = FindSetting(Section_SCDBSET, "SCDBUse")
                '    SCDBSerial = FindSetting(Section_SCDBSET, "SCDBSerial")
                '    SCDBDataSize = FindSetting(Section_SCDBSET, "SCDBDataSize")
                '    'If SCDBUse Then
                '    '    If SCDBLoginForm.ShowDialog() <> DialogResult.Yes Then
                '    '        SCDBUse = False
                '    '    End If
                '    'End If



                'Catch ex As Exception
                '    Tool.ErrorMsgBox(Tool.GetText("LodingError").Replace("$S0$", "SCDB"))
                '    Exit Sub
                'End Try




                'Try
                '    Dim Section As String = FindSection(text, "TriggerEditorSET")
                '    LoadTriggerFile(Section, True)
                'Catch ex As Exception
                '    Tool.ErrorMsgBox(Tool.GetText("LodingError").Replace("$S0$", "TriggerEditor"))
                '    Exit Sub
                'End Try





                'Try
                '    LoadCHKdata()
                'Catch ex As Exception
                '    Tool.ErrorMsgBox(Tool.GetText("LodingError").Replace("$S0$", "scenario.chk"))
                '    Exit Sub
                'End Try


                'ProjectSet.filename = MapName
                'ProjectSet.saveStatus = True
                'ProjectSet.isload = True
        End Select

    End Sub



    Sub DeleteFilesFromFolder(Folder As String)

        If Directory.Exists(Folder) Then
            For Each _file As String In Directory.GetFiles(Folder)
                File.Delete(_file)
            Next
            For Each _folder As String In Directory.GetDirectories(Folder)

                DeleteFilesFromFolder(_folder)
            Next
        End If
    End Sub

    Public Sub Save(MapName As String)
        '    Dim issavefilezip As Boolean = False

        '    If MapName.EndsWith(".e2p") Then
        '        '집 파일이면
        '        issavefilezip = True
        '    End If



        '    Dim isnewfile As Boolean = False


        '    If CheckFileExist(MapName) Then
        '        isnewfile = True
        '    End If




        Dim _stringbdl As New StringBuilder


        '    DeleteFilesFromFolder(My.Application.Info.DirectoryPath & "\Data\temp")
        '    Directory.CreateDirectory(My.Application.Info.DirectoryPath & "\Data\temp\saveFile")


        '    Dim count As Integer = 0

        '    If issavefilezip And isnewfile Then
        '        ProjectSet.filename = MapName.Replace(GetSafeName(MapName), "") & GetSafeName(MapName).Split(".").First & "\" & GetSafeName(MapName)
        '    Else
        '        ProjectSet.filename = MapName
        '    End If
        '    ProjectSet.saveStatus = True
        '    ProjectSet.isload = True

        Dim file As FileStream

        Dim savefilename As String
        '    If issavefilezip = True And isnewfile = True Then
        '        Directory.CreateDirectory(MapName.Replace(".e2p", ""))
        '        savefilename = MapName.Replace(".e2p", "") & "\" & GetSafeName(MapName)
        '    Else
        savefilename = MapName
        '    End If

        file = New FileStream(savefilename, FileMode.Create, FileAccess.Write)

        '    'Dim file As FileStream = New FileStream(MapName, FileMode.Create, FileAccess.Write)
        Dim stream As StreamWriter = New StreamWriter(file)



        _stringbdl.Append("S_ProjectSET" & vbCrLf) 'ProjectSET Start
        _stringbdl.Append("Version : " & pgData.Version.ToString & vbCrLf)
        _stringbdl.Append("InputMap : " & pjData.OpenMapName & vbCrLf)
        _stringbdl.Append("OutputMap : " & pjData.SaveMapName & vbCrLf)
        _stringbdl.Append("euddraftuse : " & True & vbCrLf)
        _stringbdl.Append("loadfromCHK : " & True & vbCrLf)
        For i = 0 To 8
            _stringbdl.Append("UsedSetting" & i & " : " & True & vbCrLf)
        Next
        _stringbdl.Append("EUDEditorDebug : " & False & vbCrLf)
        _stringbdl.Append("epTraceDebug : " & False & vbCrLf)

        _stringbdl.Append("triggerSetTouse : " & True & vbCrLf)
        _stringbdl.Append("triggerPlayer : " & 7 & vbCrLf)

        _stringbdl.Append("E_ProjectSET" & vbCrLf)



        _stringbdl.Append("S_DatEditSET" & vbCrLf) 'DatEditSET Start
        For i = 0 To pjData.Dat.DatFileList.Count - 1
            For j = 0 To pjData.Dat.DatFileList(i).ParamaterList.Count - 1
                For k = 0 To pjData.Dat.DatFileList(i).ParamaterList(j).GetInfo(SCDatFiles.EParamInfo.VarCount) - 1
                    If Not pjData.Dat.DatFileList(i).ParamaterList(j).PureData(k).IsDefault Then
                        Dim value As Long = pjData.Dat.DatFileList(i).ParamaterList(j).PureData(k).Data

                        If Not pjData.MapData.DatFile.DatFileList(i).ParamaterList(j).PureData(k).IsDefault Then
                            value -= pjData.MapData.DatFile.DatFileList(i).ParamaterList(j).PureData(k).Data
                        Else
                            value -= scData.DefaultDat.DatFileList(i).ParamaterList(j).PureData(k).Data
                        End If


                        _stringbdl.Append(i & "," & j & "," & k & "," & value & vbCrLf)
                    End If
                Next
            Next
        Next
        _stringbdl.Append("E_DatEditSET" & vbCrLf)

        _stringbdl.Append("S_FireGraftSET" & vbCrLf) 'DatEditSET Start
        For i = 0 To 227
            If Not pjData.ExtraDat.DefaultStatusFunction1(i) Or Not pjData.ExtraDat.DefaultStatusFunction2(i) Then
                Dim StatusFunction1 As Integer
                Dim StatusFunction2 As Integer

                If Not pjData.ExtraDat.DefaultStatusFunction1(i) Then
                    StatusFunction1 = pjData.ExtraDat.StatusFunction1(i)
                Else
                    StatusFunction1 = scData.DefaultExtraDat.StatusFunction1(i)
                End If


                If Not pjData.ExtraDat.DefaultStatusFunction2(i) Then
                    StatusFunction2 = pjData.ExtraDat.StatusFunction2(i)
                Else
                    StatusFunction2 = scData.DefaultExtraDat.StatusFunction2(i)
                End If
                StatusFunction1 -= scData.DefaultExtraDat.StatusFunction1(i)
                StatusFunction2 -= scData.DefaultExtraDat.StatusFunction2(i)

                'pjData.ExtraDat.StatusFunction1(i) = scData.DefaultExtraDat.StatusFunction1(i) + FindSetting(Section_FireGraftSET, "FireGraft" & i).Split(",")(0)
                'pjData.ExtraDat.StatusFunction2(i) = scData.DefaultExtraDat.StatusFunction2(i) + FindSetting(Section_FireGraftSET, "FireGraft" & i).Split(",")(1)


                If (pjData.ExtraDat.StatusFunction1(i) <> 0) Or (pjData.ExtraDat.StatusFunction2(i) <> 0) Then
                    _stringbdl.Append("FireGraft" & i & " : " & StatusFunction1 & "," & StatusFunction2 & "," & i & vbCrLf)
                End If
            End If
        Next
        _stringbdl.Append("E_FireGraftSET" & vbCrLf)
        _stringbdl.Append("S_BtnSET" & vbCrLf) 'DatEditSET Start
        For i = 0 To 249
            If Not pjData.ExtraDat.ButtonData.GetButtonSet(i).IsDefault Then
                Dim tstr As String = ""

                _stringbdl.Append("BtnUse" & i & " : " & (Not pjData.ExtraDat.ButtonData.GetButtonSet(i).IsDefault) & vbCrLf)


                Dim btnstr As String = pjData.ExtraDat.ButtonData.GetButtonSet(i).GetCopyString
                btnstr = btnstr.Replace(".", ",")

                tstr = "BtnData" & i & " : " & btnstr & ","

                _stringbdl.Append(tstr & vbCrLf)
            End If
        Next
        _stringbdl.Append("E_BtnSET" & vbCrLf)

        Dim DatTyps() As SCDatFiles.DatFiles = {SCDatFiles.DatFiles.units, SCDatFiles.DatFiles.upgrades,
                        SCDatFiles.DatFiles.techdata, SCDatFiles.DatFiles.Stechdata, SCDatFiles.DatFiles.orders}
        _stringbdl.Append("S_ReqSET" & vbCrLf) 'DatEditSET Start
        For i = 0 To DatTyps.Count - 1
            For j = 0 To SCCodeCount(DatTyps(i)) - 1
                If pjData.ExtraDat.RequireData(DatTyps(i)).RequireObjectUsed(j) <> CRequireData.RequireUse.DefaultUse Then
                    Dim UsedStatus As CRequireData.RequireUse = pjData.ExtraDat.RequireData(DatTyps(i)).RequireObjectUsed(j)


                    If UsedStatus = CRequireData.RequireUse.CustomUse Then
                        UsedStatus = CRequireData.RequireUse.AlwaysCurrentUse
                    End If

                    _stringbdl.Append("ReqUse" & i & "," & j & " : " & UsedStatus & vbCrLf)
                    _stringbdl.Append("ReqPos" & i & "," & j & " : " & pjData.ExtraDat.RequireData(DatTyps(i)).GetRequireObject(j).StartPos & vbCrLf)



                    If UsedStatus = CRequireData.RequireUse.AlwaysCurrentUse Then
                        Dim codes As List(Of UShort) = pjData.ExtraDat.RequireData(DatTyps(i)).GetRequireObject(j).LagacyGetCodes

                        _stringbdl.Append("ReqCount" & i & "," & j & " : " & codes.Count & vbCrLf)
                        For p = 0 To codes.Count - 1
                            _stringbdl.Append("ReqData" & i & "," & j & "," & p & " : " & codes(p) & vbCrLf)

                        Next
                    End If
                End If
            Next

        Next
        _stringbdl.Append("E_ReqSET" & vbCrLf)





        _stringbdl.Append("S_FileManagerSET" & vbCrLf) 'FileManagerSET Start


        _stringbdl.Append("statlang : " & pgData.Setting(ProgramData.TSetting.CDLanuage) & vbCrLf)

        Dim strcount As Integer = 0
        For i = 0 To SCtbltxtCount - 1
            If pjData.ExtraDat.Stat_txt(i) <> ExtraDatFiles.StatNullString Then
                _stringbdl.Append("stattextdickey" & strcount & " : " & i & vbCrLf)
                _stringbdl.Append("stattextdicvalue" & strcount & " : " & LagacyClass.STRhecTodec(pjData.ExtraDat.Stat_txt(i)) & vbCrLf)
                strcount += 1
            End If
        Next
        _stringbdl.Append("stattextdicCount : " & strcount & vbCrLf)


        _stringbdl.Append("wireuse : True" & vbCrLf)
        For i = 0 To 227
            If Not pjData.ExtraDat.DefaultWireFrame(i) Then
                _stringbdl.Append("wireframData" & i & " : " & pjData.ExtraDat.WireFrame(i) + 1 & vbCrLf)
            End If
            If Not pjData.ExtraDat.DefaultGrpFrame(i) Then
                _stringbdl.Append("grpwireData" & i & " : " & pjData.ExtraDat.GrpFrame(i) + 1 & vbCrLf)
            End If
            If i < SCMenCount Then
                If Not pjData.ExtraDat.DefaultTranFrame(i) Then
                    _stringbdl.Append("tranwireData" & i & " : " & pjData.ExtraDat.TranFrame(i) + 1 & vbCrLf)
                End If
            End If
        Next




        _stringbdl.Append("E_FileManagerSET" & vbCrLf)



        '    _stringbdl.Append("S_TriggerEditorSET" & vbCrLf)
        '    _stringbdl.Append(SaveTrigger() & vbCrLf)
        '    _stringbdl.Append("E_TriggerEditorSET" & vbCrLf)





        '    _stringbdl.Append("S_PluginSET" & vbCrLf) 'PluginSET Start
        '    _stringbdl.Append("soundstopper : " & soundstopper & vbCrLf)
        '    _stringbdl.Append("scmloader : " & scmloader & vbCrLf)
        '    _stringbdl.Append("noAirColsion : " & noAirCollision & vbCrLf)
        '    _stringbdl.Append("unlimiter : " & unlimiter & vbCrLf)
        '    _stringbdl.Append("keepSTR : " & keepSTR & vbCrLf)
        '    _stringbdl.Append("eudTurbo : " & eudTurbo & vbCrLf)
        '    _stringbdl.Append("iscriptPatcher : " & iscriptPatcher & vbCrLf)
        '    _stringbdl.Append("iscriptPatcheruse : " & iscriptPatcheruse & vbCrLf)
        '    _stringbdl.Append("unpatcher : " & unpatcher & vbCrLf)
        '    _stringbdl.Append("unpatcheruse : " & unpatcheruse & vbCrLf)
        '    _stringbdl.Append("nqcuse : " & nqcuse & vbCrLf)
        '    _stringbdl.Append("nqcunit : " & nqcunit & vbCrLf)
        '    _stringbdl.Append("nqclocs : " & nqclocs & vbCrLf)
        '    _stringbdl.Append("nqccommands : " & nqccommands & vbCrLf)

        '    _stringbdl.Append("grpinjectoruse : " & grpinjectoruse & vbCrLf)

        '    _stringbdl.Append("grpinjector_arrow : " & grpinjector_arrow & vbCrLf)
        '    _stringbdl.Append("grpinjector_drag : " & grpinjector_drag & vbCrLf)
        '    _stringbdl.Append("grpinjector_illegal : " & grpinjector_illegal & vbCrLf)

        '    _stringbdl.Append("dataDumperuse : " & dataDumperuse & vbCrLf)

        '    _stringbdl.Append("dataDumper_user : " & dataDumper_user & vbCrLf)

        '    _stringbdl.Append("dataDumper_grpwire : " & dataDumper_grpwire & vbCrLf)
        '    _stringbdl.Append("dataDumper_tranwire : " & dataDumper_tranwire & vbCrLf)
        '    _stringbdl.Append("dataDumper_wirefram : " & dataDumper_wirefram & vbCrLf)
        '    _stringbdl.Append("dataDumper_cmdicons : " & dataDumper_cmdicons & vbCrLf)
        '    _stringbdl.Append("dataDumper_stat_txt : " & dataDumper_stat_txt & vbCrLf)
        '    _stringbdl.Append("dataDumper_AIscript : " & dataDumper_AIscript & vbCrLf)
        '    _stringbdl.Append("dataDumper_iscript : " & dataDumper_iscript & vbCrLf)

        '    _stringbdl.Append("dataDumper_grpwire_f : " & dataDumper_grpwire_f & vbCrLf)
        '    _stringbdl.Append("dataDumper_tranwire_f : " & dataDumper_tranwire_f & vbCrLf)
        '    _stringbdl.Append("dataDumper_wirefram_f : " & dataDumper_wirefram_f & vbCrLf)
        '    _stringbdl.Append("dataDumper_cmdicons_f : " & dataDumper_cmdicons_f & vbCrLf)
        '    _stringbdl.Append("dataDumper_stat_txt_f : " & dataDumper_stat_txt_f & vbCrLf)
        '    _stringbdl.Append("dataDumper_AIscript_f : " & dataDumper_AIscript_f & vbCrLf)
        '    _stringbdl.Append("dataDumper_iscript_f : " & dataDumper_iscript_f & vbCrLf)

        '    _stringbdl.Append("extraedssetting : " & extraedssetting & vbCrLf)

        '    _stringbdl.Append("E_PluginSET" & vbCrLf)
        _stringbdl.Append("S_TileSET" & vbCrLf)

        _stringbdl.Append("ProjectTileUseFile : False
ProjectTileSetFileName : 
ProjectTIleMSetCount : 24209
ProjectTIleMSetArray : 
ProjectTileSetDataCount : 0
ProjectMTXMDATACount : 16384
ProjectMTXMDATAArray : " & vbCrLf)


        _stringbdl.Append("E_TileSET" & vbCrLf)



        stream.Write(_stringbdl.ToString)


        stream.Close()
        file.Close()

        '    If issavefilezip = True Then
        '        If isnewfile = True Then
        '            Dim foldername As String = MapName.Replace(".e2p", "")
        '            '세이브파일 폴더에 파일들을 몽땅 넣어버린다.
        '            Directory.CreateDirectory(foldername & "\Resource")
        '            Directory.CreateDirectory(foldername & "\Map")
        '            Directory.CreateDirectory(foldername & "\eudplibdata")
        '            Directory.CreateDirectory(foldername & "\Grp")
        '            Directory.CreateDirectory(foldername & "\Sound")
        '            Directory.CreateDirectory(foldername & "\temp")

        '            '이름을 모두 상대주소로 저장해 버린다.
        '            '우선 맵 먼저
        '            MoveFileAll(foldername)
        '        Else
        '            Dim foldername As String = MapName.Replace("\" & GetSafeName(MapName), "")
        '            Directory.CreateDirectory(foldername & "\Resource")
        '            Directory.CreateDirectory(foldername & "\Map")
        '            Directory.CreateDirectory(foldername & "\eudplibdata")
        '            Directory.CreateDirectory(foldername & "\Grp")
        '            Directory.CreateDirectory(foldername & "\Sound")
        '            Directory.CreateDirectory(foldername & "\temp")
        '            '일단 할건 없는 거로...
        '            MoveFileAll(foldername)

        '            DeleteDumpFileAll()
        '        End If
        '    End If

        '    DeleteFilesFromFolder(My.Application.Info.DirectoryPath & "\Data\temp")
    End Sub
End Class
