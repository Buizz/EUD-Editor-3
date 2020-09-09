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


    Private Function CheckOldVersion(ver() As String, Optional a As Integer = Integer.MaxValue, Optional b As Integer = Integer.MaxValue, Optional c As Integer = Integer.MaxValue, Optional d As Integer = Integer.MaxValue) As Boolean
        Dim ta As Integer = ver(0)
        Dim tb As Integer = ver(1)
        Dim tc As Integer = ver(2)
        Dim td As Integer = ver(3)

        Dim isOld As Boolean = True

        If ta > a Then
            isOld = False
        End If
        If tb > b Then
            isOld = False
        End If
        If tc > c Then
            isOld = False
        End If
        If td > d Then
            isOld = False
        End If


        Return isOld
    End Function
    Public Sub Legacy()
        Dim SaveVersion() As String = SaveData.LastVersion.ToString.Split(".")
        Dim pgVersion() As String = pgData.Version.ToString.Split(".")



        '=======0.9.XX.XX버전 호환성========
        If CheckOldVersion(SaveVersion, 0, 9) Then
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
