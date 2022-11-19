Partial Public Class ProjectData
    Public Sub InitProject()
        tIsDirty = False
        tFilename = ""
        SaveData.OpenMapName = ""
        SaveData.SaveMapName = ""
        SaveData.Dat = New SCDatFiles(False, False, True)
        SaveData.ExtraDat = New ExtraDatFiles
        SaveData.TEData = New TriggerEditorData
        SaveData.EdsBlocks = New BuildData.EdsBlock
        'MsgBox("프로젝트 초기화")
    End Sub


    Public Sub Legacy()
        Dim SaveVersion() As String = SaveData.LastVersion.ToString.Split(".")
        Dim pgVersion() As String = pgData.Version.ToString.Split(".")



        '=======0.9.XX.XX버전 호환성========
        'TEChatEvnet추가됨
        If StringTool.CheckOldVersion(SaveVersion, 0, 9) Then
            Dim HaveChatBlock As Boolean = False
            For i = 0 To EdsBlock.Blocks.Count - 1
                If EdsBlock.Blocks(i).BType = BuildData.EdsBlockType.TEChatEvent Then
                    HaveChatBlock = True
                End If
            Next
            If Not HaveChatBlock Then
                EdsBlock.Blocks.Add(New BuildData.EdsBlock.EdsBlockItem(BuildData.EdsBlockType.TEChatEvent))
            End If

            TEData.UseChatEvent = True
            TEData.UseMSQC = True
        End If
        '===================================


        '=======0.11.XX.XX버전 호환성========
        'Switch 기능 변경
        If StringTool.CheckOldVersion(SaveVersion, 0, 11) Then
            Dim mainTEFile As TEFile = TEData.PFIles

            For i = 0 To mainTEFile.FileCount - 1
                TELegacy(mainTEFile.Files(i))
            Next
            For i = 0 To mainTEFile.FolderCount - 1
                TELegacy(mainTEFile.Folders(i))
            Next
        End If
        '===================================
    End Sub
    Public Sub TELegacy(tfile As TEFile)
        If tfile.FileType = TEFile.EFileType.GUIEps Then
            MsgBox("호환성 경고" & vbCrLf &
                        "다음 파일이 CUI로 강제 변경됩니다." & vbCrLf &
                       tfile.FileName, MsgBoxStyle.Exclamation)

            If tfile.ChagneType() Then
                pjData.SetDirty(True)
            End If



            'Dim gscr As GUIScriptEditor = CType(tfile.Scripter, GUIScriptEditor)

            'gscr.scrLegacyInit()
        Else
            For i = 0 To tfile.FileCount - 1
                TELegacy(tfile.Files(i))
            Next
        End If
    End Sub

End Class
