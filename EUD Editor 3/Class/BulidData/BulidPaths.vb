Partial Public Class BuildData
#Region "Paths"
    Public Shared ReadOnly Property GetSCArchiveeps() As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\TriggerEditor\SCArchive.eps"
        End Get
    End Property
    Public Shared ReadOnly Property GetSCATooleps() As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\TriggerEditor\SCATool.eps"
        End Get
    End Property
    Public Shared ReadOnly Property GetBGMTooleps() As String
        Get
            Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\TriggerEditor\BGMPlayer.eps"
        End Get
    End Property
    Public Shared ReadOnly Property GetTriggerEditorFolderPath() As String
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
    Public Shared ReadOnly Property WireFrameEpsFilePath() As String
        Get
            Return EudPlibFilePath & "\WireFrameDataEditor.eps"
        End Get
    End Property
    Public Shared ReadOnly Property TriggerEditorPath() As String
        Get
            Return EudPlibFilePath & "\TriggerEditor"
        End Get
    End Property

    Public Shared ReadOnly Property EudPlibFilePath() As String
        Get
            Return Tool.GetDirectory(TempFloder & "\", "eudplibData")
        End Get
    End Property
    Public Shared ReadOnly Property TempFilePath() As String
        Get
            Return Tool.GetDirectory(TempFloder & "\", "temp")
        End Get
    End Property
    Public Shared ReadOnly Property BackupFilePath() As String
        Get
            Return Tool.GetDirectory(TempFloder & "\", "backup")
        End Get
    End Property
    Public Shared ReadOnly Property SoundFilePath() As String
        Get
            Return Tool.GetDirectory(TempFloder & "\", "sound")
        End Get
    End Property
    Public Shared ReadOnly Property TempFloder() As String
        Get

            Try
                If pjData.TempFileLoc = "0" Then
                    '기본 데이터 폴더 사용할 경우.
                    Return Tool.GetDirectoy("Data\temp\BuildData_" & Tool.StripFileName(pjData.SafeFilename.Split(".").First))
                ElseIf pjData.TempFileLoc = "1" Then
                    '맵 폴더가 같을 경우
                    Return Tool.GetDirectory(pjData.OpenMapdirectory & "\BuildData_", Tool.StripFileName(pjData.SafeFilename.Split(".").First))

                ElseIf pjData.TempFileLoc = "2" Then
                    '맵 폴더가 같을 경우
                    Return Tool.GetDirectory(System.IO.Path.GetDirectoryName(pjData.Filename) & "\BuildData_", Tool.StripFileName(pjData.SafeFilename.Split(".").First))
                Else
                    Return Tool.GetDirectory(pjData.TempFileLoc, "\BuildData_" & Tool.StripFileName(pjData.SafeFilename.Split(".").First))
                    'If pjData.OpenMapdirectory = pjData.SaveMapdirectory And pjData.OpenMapdirectory = pjData.TempFileLoc Then

                    'End If
                End If
            Catch ex As System.UnauthorizedAccessException
                Dim arr As String() = ex.Message.Split("'")
                Dim tname As String = ex.Message
                If arr.Length = 3 Then
                    tname = ex.Message.Split("'")(1)
                End If

                Tool.CustomMsgBox(Tool.GetLanText("TempFolderError").Replace("%S1", tname), MessageBoxButton.OK, MessageBoxImage.Exclamation)

                pjData.TempFileLoc = "0"
                Return Tool.GetDirectoy("Data\temp\BuildData_" & Tool.StripFileName(pjData.SafeFilename.Split(".").First))
            End Try

        End Get
    End Property
#End Region
End Class
