

Imports System.IO
Imports System.Net
Imports System.Net.NetworkInformation
Imports KopiLua
Imports net.r_eg.Conari
Imports Newtonsoft
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports NVorbis.Contracts

<Serializable>
Public Class StarCraftArchive
    Public Sub New()
        DataSpace = 300

        FuncSpace = 4000
        SCAScriptVarCount = 200


        _CodeDatas = New List(Of CodeData)
        _MakerBattleTag = ""
        _MakerServerName = ""
        _MapName = ""
        _TestMode = True
    End Sub


    ''' <summary>
    ''' SCA가 사용으로 되어있을때 현재 계정정보가 올바른지 확인하고 올바르지 않으면 IsUsed를 비활성화 합니다.
    ''' </summary>
    ''' <returns></returns>
    Public Function CheckLoginAccount() As Boolean
        If IsLogin Then
            'SCA에 로그인 중일 때
            '로그인 체크 (현재 이메일을 기준으로 비밀번호를 가져온 다음에 가져온 정보로 로그인 서버로 보낸다)

            Dim email As String = pjData.TEData.SCArchive.SCAEmail
            Dim pw As String = GetPassWord(email)

            Dim returnval As String = HttpTool.Login(email, pw)

            Dim success As Boolean = False
            Select Case returnval
                Case "BANUSER"
                    success = False
                Case "NOACCOUNT"
                    success = False
                Case "ERROR"
                    success = False
                Case Else
                    success = True
                    LoginHash = returnval
            End Select


            If success Then
                '로그인 성공
                Return True
            Else
                '로그인 실패 SCA를 사용안함으로 돌림
                _IsLogin = False
                _IsUsed = False
                Return False
            End If

        End If


        Return False
    End Function


    Public Sub SavePassWord(email As String, pw As String)
        Dim document As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim foldername As String = "EUD Editor3"

        Dim folderpath As String = document & "\" & foldername
        Dim filepath As String = document & "\" & foldername & "\user.json"

        If Not Directory.Exists(folderpath) Then
            Directory.CreateDirectory(folderpath)
        End If

        If Not File.Exists(filepath) Then
            Dim configData As New JObject()
            configData.Add(email, pw)

            File.WriteAllText(filepath, configData.ToString())
        Else
            Dim reader As New JsonTextReader(File.OpenText(filepath))
            Dim Json As JObject = JToken.ReadFrom(reader)

            Json(email) = pw

            reader.Close()


            File.WriteAllText(filepath, Json.ToString())
        End If
    End Sub

    Public Function GetPassWord(email As String) As String
        If TempPassWord = "" Then
            Dim document As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            Dim foldername As String = "EUD Editor3"

            Dim folderpath As String = document & "\" & foldername
            Dim filepath As String = document & "\" & foldername & "\user.json"

            If Not Directory.Exists(folderpath) Then
                Directory.CreateDirectory(foldername)
            End If

            If Not File.Exists(filepath) Then
                Return ""
            End If

            Dim reader As New JsonTextReader(File.OpenText(filepath))
            Dim Json As JObject = JToken.ReadFrom(reader)

            If Json.ContainsKey(email) Then
                Return Json(email)
            Else
                Return ""
            End If
        Else
            Return TempPassWord
        End If
    End Function





    Public Function Login(Email As String, Password As String) As String

        Dim datalist As New Dictionary(Of String, String) From {
            {"email", Email},
            {"password", Password}
        }

        Dim returnrequest As String = HttpTool.Request("login", datalist)

        Return True
    End Function





    <NonSerialized>
    Public LoginHash As String

    <NonSerialized>
    Public TempPassWord As String


    Private _LastLoginHash As String

    ''' <summary>
    ''' 식별값을 저장할때 어떤 계정으로 저장되었는지 확인하는 값
    ''' 특정한 문장을 LoginHash로 암호화한 값 만약 이메일로 로그인 했을 때 해당 값을 복호화 했을때 상수가 나타나지 않으면
    ''' 다른 계정으로 로그인한것이므로 기존의 데이터를 모두 날림
    ''' </summary>
    ''' <returns></returns>
    Public Property LastLoginHash As String
        Get
            Try
                Return AESModule.DecryptString128Bit(_LastLoginHash, LoginHash)
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(value As String)
            _LastLoginHash = AESModule.EncryptString128Bit(value, LoginHash)
        End Set
    End Property

    Public Sub SaveLoginHash(hash As String)
        LoginHash = hash

        LastLoginHash = "CHECK"
    End Sub

    ''' <summary>
    ''' 현재 저장된 해시와 동일한 기록인지 확인합니다.
    ''' </summary>
    ''' <returns></returns>
    Public Function CheckLoginHash(hash As String) As Boolean
        If _LastLoginHash = Nothing Then
            Return False
        End If

        If AESModule.DecryptString128Bit(_LastLoginHash, hash) = "CHECK" Then
            Return True
        End If

        Return False
    End Function


    Private _IsLogin As Boolean
    Public Property IsLogin As Boolean
        Get
            Return _IsLogin
        End Get
        Set(value As Boolean)
            _IsLogin = value
        End Set
    End Property




    Private _IsUsed As Boolean
    Public Property IsUsed As Boolean
        Get
            Return _IsUsed
        End Get
        Set(value As Boolean)
            _IsUsed = value
        End Set
    End Property


    Private _ViewPublic As Boolean
    Public Property ViewPublic As Boolean
        Get
            Return _ViewPublic
        End Get
        Set(value As Boolean)
            _ViewPublic = value
        End Set
    End Property
    Private _TestMode As Boolean
    Private _newTestMode As EGameMode = EGameMode.TestMode
    Public Enum EGameMode
        TestMode = 0
        MultyMode = 1
        FreeMode = 2
    End Enum
    Public Property TestMode As EGameMode
        Get
            Return _newTestMode
        End Get
        Set(value As EGameMode)
            _newTestMode = value
        End Set
    End Property

    Private _DataSpace As Integer
    Public Property DataSpace As Integer
        Get
            Return _DataSpace
        End Get
        Set(value As Integer)
            _DataSpace = value
        End Set
    End Property

    Private _FuuncSpace As Integer
    Public Property FuncSpace As Integer
        Get
            Return _FuuncSpace
        End Get
        Set(value As Integer)
            _FuuncSpace = value
        End Set
    End Property

    Private _SCAScriptVarCount As Integer
    Public Property SCAScriptVarCount As Integer
        Get
            Return _SCAScriptVarCount
        End Get
        Set(value As Integer)
            _SCAScriptVarCount = value
        End Set
    End Property

    Private _MakerBattleTag As String
    Public Property MakerBattleTag As String
        Get
            Return _MakerBattleTag
        End Get
        Set(value As String)
            _MakerBattleTag = value
        End Set
    End Property


    Private _SCAEmail As String
    Public Property SCAEmail As String
        Get
            Return _SCAEmail
        End Get
        Set(value As String)
            _SCAEmail = value
        End Set
    End Property





    Private _IsUseOldBattleTag As Boolean
    Public Property IsUseOldBattleTag As Boolean
        Get
            Return _IsUseOldBattleTag
        End Get
        Set(value As Boolean)
            _IsUseOldBattleTag = value
        End Set
    End Property



    Private _MakerEmail As String
    Public Property MakerEmail As String
        Get
            Return _MakerEmail
        End Get
        Set(value As String)
            _MakerEmail = value
        End Set
    End Property


    '===================================================================================
    Private _MakerServerName As String
    Public Property MakerServerName As String
        Get
            Try
                Return AESModule.DecryptString128Bit(_MakerServerName, LoginHash)
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(value As String)
            _MakerServerName = AESModule.EncryptString128Bit(value, LoginHash)
        End Set
    End Property


    Private _SubTitle As String
    Public Property SubTitle As String
        Get
            Try
                Return AESModule.DecryptString128Bit(_SubTitle, LoginHash)
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(value As String)
            _SubTitle = AESModule.EncryptString128Bit(value, LoginHash)
        End Set
    End Property


    Private _MapName As String
    Public Property MapName As String
        Get
            Try
                Return AESModule.DecryptString128Bit(_MapName, LoginHash)
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(value As String)
            _MapName = AESModule.EncryptString128Bit(value, LoginHash)
        End Set
    End Property
    '===================================================================================


    Private _CodeDatas As List(Of CodeData)
    Public ReadOnly Property CodeDatas As List(Of CodeData)
        Get
            If _CodeDatas Is Nothing Then
                _CodeDatas = New List(Of CodeData)
            End If
            Return _CodeDatas
        End Get
    End Property




    Private _MapTitle As String
    Public Property MapTitle As String
        Get
            Return _MapTitle
        End Get
        Set(value As String)
            _MapTitle = value
        End Set
    End Property

    Private _MapTags As String
    Public Property MapTags As String
        Get
            Return _MapTags
        End Get
        Set(value As String)
            _MapTags = value
        End Set
    End Property

    Private _updateinfo As Boolean
    Public Property updateinfo As Boolean
        Get
            Return _updateinfo
        End Get
        Set(value As Boolean)
            _updateinfo = value
        End Set
    End Property



    Private _DownLink As String
    Public Property DownLink As String
        Get
            Return _DownLink
        End Get
        Set(value As String)
            _DownLink = value
        End Set
    End Property



    Private _ImageLink As String
    Public Property ImageLink As String
        Get
            Return _ImageLink
        End Get
        Set(value As String)
            _ImageLink = value
        End Set
    End Property


    Private _MapDes As String
    Public Property MapDes As String
        Get
            Return _MapDes
        End Get
        Set(value As String)
            _MapDes = value
        End Set
    End Property




    <Serializable>
    Public Class CodeData
        Public Property TagName As String

        Private TypeNames() As String = {"변수", "데스값", "배열"}
        Public Property ECodeType As String
            Get
                Return TypeNames(TypeIndex)
            End Get
            Set(value As String)
                TypeIndex = value
            End Set
        End Property

        Public TypeIndex As CodeType
        Public Enum CodeType
            Variable
            Deaths
            Array
        End Enum

        Public Property Value As String
            Get
                Select Case TypeIndex
                    Case CodeType.Variable
                        '변수
                        Return NameSpaceName & " " & ValueName
                    Case CodeType.Deaths
                        '데스값
                        Return pjData.CodeLabel(SCDatFiles.DatFiles.units, ValueIndex)
                    Case CodeType.Array
                        '변수
                        Return NameSpaceName & " " & ValueName
                End Select
                Return ""
            End Get
            Set(value As String)
                Select Case TypeIndex
                    Case CodeType.Variable
                        '변수
                        NameSpaceName = value.Split(",").First
                        ValueName = value.Split(",").Last
                    Case CodeType.Deaths
                        '데스값
                        ValueIndex = value
                    Case CodeType.Array
                        '변수
                        NameSpaceName = value.Split(",").First
                        ValueName = value.Split(",").Last
                End Select
            End Set
        End Property
        Public ValueName As String
        Public NameSpaceName As String

        Public ValueIndex As Integer


        Public Sub New(tTag As String, tType As Integer, tValue As String)
            TagName = tTag
            TypeIndex = tType
            Value = tValue
        End Sub
        Public Sub Refresh(tTag As String, tType As Integer, tValue As String)
            TagName = tTag
            TypeIndex = tType
            Value = tValue
        End Sub
    End Class
End Class
