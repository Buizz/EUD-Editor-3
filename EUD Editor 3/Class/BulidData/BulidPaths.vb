Partial Public Class BuildData
#Region "Paths"
    Public ReadOnly Property GetSCArchiveeps() As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\TriggerEditor\SCArchive.eps"
        End Get
    End Property
    Public ReadOnly Property GetSCATooleps() As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\TriggerEditor\SCATool.eps"
        End Get
    End Property
    Public ReadOnly Property GetBGMTooleps() As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\TriggerEditor\BGMPlayer.eps"
        End Get
    End Property
    Public ReadOnly Property GetTriggerEditorFolderPath() As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\TriggerEditor"
        End Get
    End Property


    Public Shared ReadOnly Property OpenMapPath() As String
        Get
            Return Tool.GetRelativePath(EdsFilePath, pjData.OpenMapName)
        End Get
    End Property
    Public Shared ReadOnly Property SaveMapPath() As String
        Get
            Return Tool.GetRelativePath(EdsFilePath, pjData.SaveMapName)
        End Get
    End Property
    Public Shared ReadOnly Property tblFilePath() As String
        Get
            Return TempFilePath & "\custom_txt.tbl"
        End Get
    End Property
    Public Shared ReadOnly Property requireFilePath() As String
        Get
            Return TempFilePath & "\RequireData"
        End Get
    End Property
    Public Shared ReadOnly Property EdsFilePath() As String
        Get
            Return EudPlibFilePath & "\EUDEditor.eds"
        End Get
    End Property
    Public Shared ReadOnly Property EddFilePath() As String
        Get
            Return EudPlibFilePath & "\EUDEditor.edd"
        End Get
    End Property
    Public Shared ReadOnly Property DatpyFilePath() As String
        Get
            Return EudPlibFilePath & "\DataEditor.py"
        End Get
    End Property
    Public Shared ReadOnly Property ExtraDatpyFilePath() As String
        Get
            Return EudPlibFilePath & "\ExtraDataEditor.py"
        End Get
    End Property
    Public Shared ReadOnly Property TriggerEditorPath() As String
        Get
            Return EudPlibFilePath & "\TriggerEditor"
        End Get
    End Property
    Public Shared ReadOnly Property EudPlibFilePath() As String
        Get
            Return Tool.GetDirectoy(TempFloder & "\", "eudplibData")
        End Get
    End Property
    Public Shared ReadOnly Property TempFilePath() As String
        Get
            Return Tool.GetDirectoy(TempFloder & "\", "temp")
        End Get
    End Property
    Public Shared ReadOnly Property SoundFilePath() As String
        Get
            Return Tool.GetDirectoy(TempFloder & "\", "sound")
        End Get
    End Property
    Public Shared ReadOnly Property TempFloder() As String
        Get
            If pjData.TempFileLoc = "0" Then
                '기본 데이터 폴더 사용할 경우.
                Return Tool.GetDirectoy("Data\temp\BulidData_" & Tool.StripFileName(pjData.SafeFilename.Split(".").First))
            ElseIf pjData.TempFileLoc = "1" Then
                '맵 폴더가 같을 경우
                Return Tool.GetDirectoy(pjData.OpenMapdirectory & "\BulidData_", Tool.StripFileName(pjData.SafeFilename.Split(".").First))

            ElseIf pjData.TempFileLoc = "2" Then
                '맵 폴더가 같을 경우
                Return Tool.GetDirectoy(System.IO.Path.GetDirectoryName(pjData.Filename) & "\BulidData_", Tool.StripFileName(pjData.SafeFilename.Split(".").First))
            Else
                Return Tool.GetDirectoy(pjData.TempFileLoc, "\BulidData_" & Tool.StripFileName(pjData.SafeFilename.Split(".").First))
                'If pjData.OpenMapdirectory = pjData.SaveMapdirectory And pjData.OpenMapdirectory = pjData.TempFileLoc Then


                'End If
            End If
        End Get
    End Property
#End Region
End Class
