Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports Newtonsoft.Json

<Serializable()>
Public Class SaveableData
    Private mOpenMapName As String
    Private mSaveMapName As String
    Public Property OpenMapName As String
        Get
            Return mOpenMapName
        End Get
        Set(value As String)
            mOpenMapName = value
        End Set
    End Property

    Public Property SaveMapName As String
        Get
            Return mSaveMapName
        End Get
        Set(value As String)
            mSaveMapName = value
        End Set
    End Property

    Public Sub New()
        OpenMapName = ""
        SaveMapName = ""

        ReDim _Stat_txt(SCtbltxtCount)
    End Sub
    Private _Stat_txt() As String
    Public Property Stat_txt(index As Integer) As String
        Get
            Dim text As String
            Try
                text = _Stat_txt(index)
                Return text
            Catch ex As Exception
                Return "Error"
            End Try
        End Get
        Set(value As String)
            _Stat_txt(index) = value
        End Set
    End Property




    Public Dat As SCDatFiles
End Class



Public Class ProjectData
#Region "Filed"
    Private tFilename As String
    Public ReadOnly Property Filename As String
        Get
            Return tFilename
        End Get
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
    End Sub
#End Region


#Region "Member"
    Public ReadOnly Property Dat As SCDatFiles
        Get
            Return SaveData.Dat
        End Get
    End Property
    Public Property Stat_txt(index As Integer) As String
        Get
            If index = -1 Then
                Return "None"
            End If

            If SaveData.Stat_txt(index) = "" Then
                Return scData.GetStat_txt(index)
            Else
                Return SaveData.Stat_txt(index)
            End If

        End Get
        Set(value As String)
            SaveData.Stat_txt(index) = value
        End Set
    End Property

    Public Property OpenMapName As String
        Get
            Return SaveData.OpenMapName
        End Get
        Set(value As String)
            If My.Computer.FileSystem.FileExists(value) And SaveData.OpenMapName <> value Then
                tIsDirty = True
                SaveData.OpenMapName = value
                _MapData = New MapData(SaveData.OpenMapName)
                MapLoading = _MapData.LoadComplete
                If Not MapLoading Then
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

    Private _MapData As MapData
    Public ReadOnly Property MapData As MapData
        Get
            Return _MapData
        End Get
    End Property

    Private MapLoading As Boolean
    Public ReadOnly Property IsMapLoading As Boolean
        Get
            Return MapLoading
        End Get
    End Property

#End Region


    Private SaveData As SaveableData


    Private Bd As BindingManager
    Public ReadOnly Property BindingManager As BindingManager
        Get
            Return Bd
        End Get
    End Property

    Public Property CodeLabel(Datfile As SCDatFiles.DatFiles, index As Integer)
        Get
            Select Case Datfile
                Case SCDatFiles.DatFiles.units
                    Return UnitName(index)
                Case SCDatFiles.DatFiles.weapons
                    Dim tLabel As Integer = Dat.Data(SCDatFiles.DatFiles.weapons, "Label", index) - 1

                    Return Stat_txt(tLabel)
                Case SCDatFiles.DatFiles.flingy
                    Dim tSprite As Integer = Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", index)
                    Dim timage As Integer = Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", tSprite)

                    Return scData.ImageStr(timage)
                Case SCDatFiles.DatFiles.sprites
                    Dim timage As Integer = Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", index)

                    Return scData.ImageStr(timage)
                Case SCDatFiles.DatFiles.images
                    Return scData.ImageStr(index)
                Case SCDatFiles.DatFiles.upgrades
                    Dim tLabel As Integer = Dat.Data(SCDatFiles.DatFiles.upgrades, "Label", index) - 1

                    Return Stat_txt(tLabel)
                Case SCDatFiles.DatFiles.techdata
                    Dim tLabel As Integer = Dat.Data(SCDatFiles.DatFiles.techdata, "Label", index) - 1

                    Return Stat_txt(tLabel)
                Case SCDatFiles.DatFiles.orders
                    Dim tLabel As Integer = Dat.Data(SCDatFiles.DatFiles.orders, "Label", index) - 1

                    Return Stat_txt(tLabel)
                Case SCDatFiles.DatFiles.button
                    If index < SCUnitCount - 1 Then
                        Return UnitName(index)
                    Else

                        Return scData.BtnStr(index - SCUnitCount + 1)
                    End If
            End Select
            Return "Error"
        End Get
        Set(value)

        End Set
    End Property




    Private ReadOnly Property UnitName(index As Byte) As String
        Get
            Dim RealName As String = Stat_txt(index)
            If MapLoading Then
                Dim strindex As Integer = MapData.DatFile.Data(SCDatFiles.DatFiles.units, "Unit Map String", index)
                Dim ToolTipText As String = SaveData.Dat.ToolTip(SCDatFiles.DatFiles.units, index)

                If ToolTipText.Trim <> "" Then

                    If strindex = 0 Then
                        Return RealName & " (" & ToolTipText & ")"
                    Else
                        Return MapData.Str(strindex - 1) & " (" & ToolTipText & ")" & vbCrLf & RealName
                    End If

                Else
                    If strindex = 0 Then
                        Return RealName
                    Else
                        Return MapData.Str(strindex - 1) & vbCrLf & RealName
                    End If
                End If

                If strindex = 0 Then
                    Return RealName
                Else
                    Return MapData.Str(strindex - 1) & " (" & ToolTipText & ")" & vbCrLf & RealName
                End If
            Else
                Dim ToolTipText As String = SaveData.Dat.ToolTip(SCDatFiles.DatFiles.units, index)

                If ToolTipText.Trim <> "" Then
                    Return RealName & " (" & ToolTipText & ")"
                Else
                    Return RealName
                End If

                'Return SaveData.Dat.Group(SCDatFiles.DatFiles.units, index) & RealName
            End If
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
        Bd = New BindingManager
    End Sub



    Public Sub InitProject()
        tIsDirty = False
        tFilename = ""
        SaveData.OpenMapName = ""
        SaveData.SaveMapName = ""
        SaveData.Dat = New SCDatFiles(False, False, True)

        'MsgBox("프로젝트 초기화")
    End Sub

    Public Sub LoadInit(filename As String)
        tIsLoad = True
        tIsDirty = False
        tFilename = filename
        'MsgBox("로드 프로젝트 초기화")
        If My.Computer.FileSystem.FileExists(SaveData.OpenMapName) Then
            _MapData = New MapData(SaveData.OpenMapName)
            MapLoading = _MapData.LoadComplete
            If Not MapLoading Then
                SaveData.OpenMapName = ""
            End If
        Else
            _MapData = Nothing
            SaveData.OpenMapName = ""
        End If
    End Sub

    Public Sub NewFIle()
        InitProject()
        tIsLoad = True
    End Sub


    '여기에 모든게 들어간다
    '스타 dat데이터를 클래스로 만들어 관리하자.
    Public Shared Sub Load(isNewfile As Boolean, ByRef _pjdata As ProjectData)
        If isNewfile Then
            _pjdata = New ProjectData
            _pjdata.NewFIle()
        Else
            Dim tFilename As String
            Dim LoadProjectDialog As New System.Windows.Forms.OpenFileDialog
            LoadProjectDialog.Filter = Tool.GetText("LoadFliter")

            If LoadProjectDialog.ShowDialog() = Forms.DialogResult.OK Then
                tFilename = LoadProjectDialog.FileName '파일 이름 교체
            Else
                Exit Sub
            End If

            If Tool.IsProjectLoad() Then
                '꺼야됨
                If Not _pjdata.CloseFile Then
                    Exit Sub
                End If
            End If

            Load(tFilename, _pjdata)
        End If
    End Sub
    Public Shared Sub Load(FileName As String, ByRef _pjdata As ProjectData)
        Dim stm As Stream = System.IO.File.Open(FileName, FileMode.Open, FileAccess.Read)
        Dim bf As BinaryFormatter = New BinaryFormatter()
        _pjdata = New ProjectData
        _pjdata.NewFIle()
        _pjdata.SaveData = bf.Deserialize(stm)
        _pjdata.LoadInit(FileName)
        stm.Close()

        'Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(ProjectData))
        'Dim file As New System.IO.StreamReader(FileName)
        '_pjdata = CType(reader.Deserialize(file), ProjectData)
        '_pjdata.LoadInit(FileName)

        'file.Close()
    End Sub


    Public Function Save(Optional IsSaveAs As Boolean = False) As Boolean
        If IsSaveAs = True Then '다른이름으로 저장 일 경우 
            Tool.SaveProjectDialog.FileName = SafeFilename

            Dim exten As String() = Tool.SaveProjectDialog.Filter.Split("|")
            For i = 1 To exten.Count - 1 Step 2
                If Extension = exten(i).Split(".").Last Then
                    Tool.SaveProjectDialog.FilterIndex = ((i - 1) \ 2) + 1
                    Exit For
                End If

            Next




            If Tool.SaveProjectDialog.ShowDialog() = Forms.DialogResult.OK Then
                tFilename = Tool.SaveProjectDialog.FileName '파일 이름 교체
            Else
                Return False
            End If
        End If
        If tFilename = "" Then ' 새파일
            Tool.SaveProjectDialog.FileName = SafeFilename
            If Tool.SaveProjectDialog.ShowDialog() = Forms.DialogResult.OK Then
                tFilename = Tool.SaveProjectDialog.FileName '파일 이름 교체
            Else
                Return False
            End If
        End If

        Dim stm As Stream = File.Open(tFilename, FileMode.Create, FileAccess.ReadWrite)
        Dim bf As BinaryFormatter = New BinaryFormatter()
        bf.Serialize(stm, Me.SaveData)
        stm.Close()

        'Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(ProjectData))
        'Dim file As New System.IO.StreamWriter(tFilename)
        'writer.Serialize(file, Me)
        'file.Close()


        'Dim fs As New FileStream(tFilename, FileMode.Create)
        'Dim sw As New StreamWriter(fs)


        'sw.Write(JsonConvert.SerializeObject(pjData))

        'sw.Close()
        'fs.Close()

        tIsLoad = True
        tIsDirty = False
        Return True
    End Function


    Public Function CloseFile() As Boolean
        If IsDirty Then '파일이 변형되었을 경우
            Dim dialog As MsgBoxResult = MsgBox(Tool.GetText("ColseSaveMsg").Replace("%S1", SafeFilename), MsgBoxStyle.YesNoCancel)
            If dialog = MsgBoxResult.Yes Then
                If Save() Then
                    tIsLoad = False
                    Return True
                Else
                    Return False
                End If
            ElseIf dialog = MsgBoxResult.No Then
                tIsLoad = False
                Return True
            ElseIf dialog = MsgBoxResult.Cancel Then
                Return False
            End If

        End If

        tIsLoad = False
        Return True
    End Function

    '일단 코드불러오는거 먼저 하자. stat_txt.bin을 불러오고 그걸 바탕으로 만들자.(이미지나 스프라이트를 제외하고는 한글 이름으로 가능하니까 이미지나 스프라이트는 데이터로 준비)


End Class
