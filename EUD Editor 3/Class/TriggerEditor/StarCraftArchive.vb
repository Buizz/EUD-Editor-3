

Imports System.Net.NetworkInformation

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
        _PassWord = ""
        _TestMode = True
    End Sub

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


    Public ReadOnly Property MacAddr As String
        Get
            Dim _MacAddr As String = (From nic In NetworkInterface.GetAllNetworkInterfaces()
                                      Where nic.OperationalStatus = OperationalStatus.Up
                                      Select nic.GetPhysicalAddress()).FirstOrDefault().ToString()


            Return _MacAddr
        End Get
    End Property


    Private _SCAEmail As String
    Public Property SCAEmail As String
        Get
            Try
                Return AESModule.DecryptString128Bit(_SCAEmail, MacAddr)
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(value As String)
            _SCAEmail = AESModule.EncryptString128Bit(value, MacAddr)
        End Set
    End Property


    Private _PassWord As String
    Public Property PassWord As String
        Get
            Try
                Return AESModule.DecryptString128Bit(_PassWord, MacAddr)
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(value As String)
            _PassWord = AESModule.EncryptString128Bit(value, MacAddr)
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
                Return AESModule.DecryptString128Bit(_MakerServerName, MacAddr)
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(value As String)
            _MakerServerName = AESModule.EncryptString128Bit(value, MacAddr)
        End Set
    End Property


    Private _SubTitle As String
    Public Property SubTitle As String
        Get
            Try
                Return AESModule.DecryptString128Bit(_SubTitle, MacAddr)
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(value As String)
            _SubTitle = AESModule.EncryptString128Bit(value, MacAddr)
        End Set
    End Property


    Private _MapName As String
    Public Property MapName As String
        Get
            Try
                Return AESModule.DecryptString128Bit(_MapName, MacAddr)
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(value As String)
            _MapName = AESModule.EncryptString128Bit(value, MacAddr)
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
