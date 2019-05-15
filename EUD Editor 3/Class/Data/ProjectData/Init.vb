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
        MsgBox("레거시작동 : " & SaveData.LastVersion.ToString)
        'If SaveData.Dat Is Nothing Then
        '    SaveData.Dat = New SCDatFiles(False, False, True)
        'End If
        'If SaveData.ExtraDat Is Nothing Then
        '    SaveData.ExtraDat = New ExtraDatFiles
        'End If
        'If SaveData.TEData Is Nothing Then
        '    SaveData.TEData = New TriggerEditorData
        'End If
        'If SaveData.EdsBlocks Is Nothing Then
        '    SaveData.EdsBlocks = New BuildData.EdsBlock
        'End If
    End Sub
End Class
