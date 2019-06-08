Imports System.IO

<Serializable()>
Public Class SaveableData
#Region "Setting"
    Private mOpenMapName As String
    Private mSaveMapName As String
    Private mRelativeOpenMapName As String
    Private mRelativeSaveMapName As String
    Public Property OpenMapName As String
        Get
            If Not My.Computer.FileSystem.FileExists(mOpenMapName) And mRelativeOpenMapName <> "" Then '오픈 맵이 존재하지 않을 경우
                Dim tempOpenMapName As String = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(pjData.Filename), mRelativeOpenMapName))
                If My.Computer.FileSystem.FileExists(tempOpenMapName) Then '상대경로로 존재할 경우
                    mOpenMapName = tempOpenMapName
                End If
            End If
            Return mOpenMapName
        End Get
        Set(value As String)
            mOpenMapName = value
            RelativeDataRefresh()
        End Set
    End Property

    Public Property SaveMapName As String
        Get
            If mSaveMapName <> "" Then
                If Not My.Computer.FileSystem.DirectoryExists(Path.GetDirectoryName(mSaveMapName)) And mRelativeSaveMapName <> "" Then '저장맵의 폴더가 존재하지 않을 경우
                    Dim tempSaveMapName As String = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(pjData.Filename), mRelativeSaveMapName))
                    If My.Computer.FileSystem.DirectoryExists(Path.GetDirectoryName(tempSaveMapName)) Then '상대경로로 존재할 경우
                        mSaveMapName = tempSaveMapName
                    End If
                End If
            End If

            Return mSaveMapName
        End Get
        Set(value As String)
            mSaveMapName = value
            RelativeDataRefresh()
        End Set
    End Property

    Private mTempFileLoc As String
    Public Property TempFileLoc As String
        Get
            Return mTempFileLoc
        End Get
        Set(value As String)
            mTempFileLoc = value
        End Set
    End Property

    Private mUseCustomTbl As Boolean
    Public Property UseCustomTbl As Boolean
        Get
            Return mUseCustomTbl
        End Get
        Set(value As Boolean)
            mUseCustomTbl = value
        End Set
    End Property


    Private mAutoBuild As Boolean
    Public Property AutoBuild As Boolean
        Get
            Return mAutoBuild
        End Get
        Set(value As Boolean)
            mAutoBuild = value
        End Set
    End Property



    Private mViewLog As Boolean
    Public Property ViewLog As Boolean
        Get
            Return mViewLog
        End Get
        Set(value As Boolean)
            mViewLog = value
        End Set
    End Property


    Private Sub SettingInit()
        mOpenMapName = ""
        mSaveMapName = ""

        mTempFileLoc = 0
        UseCustomTbl = True
    End Sub
#End Region

    Public Sub New()
        SettingInit()
    End Sub

    Private mLastVersion As System.Version
    Public Property LastVersion As System.Version
        Get
            Return mLastVersion
        End Get
        Set(value As System.Version)
            mLastVersion = value
        End Set
    End Property


    Public Sub Close()
        RelativeDataRefresh()
    End Sub


    Public Sub RelativeDataRefresh()
        If My.Computer.FileSystem.FileExists(pjData.Filename) Then
            mRelativeOpenMapName = Tool.GetRelativePath(pjData.Filename, OpenMapName)
            mRelativeSaveMapName = Tool.GetRelativePath(pjData.Filename, SaveMapName)
        End If

        'MsgBox("RelativeDataRefresh")
    End Sub


    Public Dat As SCDatFiles
    Public ExtraDat As ExtraDatFiles
    Public TEData As TriggerEditorData



    Public EdsBlocks As BuildData.EdsBlock


End Class

