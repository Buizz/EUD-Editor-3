

<Serializable>
Public Class StarCraftArchive
    Public Sub New()
        DataSpace = 100
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



    Private _DataSpace As Integer
    Public Property DataSpace As Integer
        Get
            Return _DataSpace
        End Get
        Set(value As Integer)
            _DataSpace = value
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


    Private _MakerServerName As String
    Public Property MakerServerName As String
        Get
            Return _MakerServerName
        End Get
        Set(value As String)
            _MakerServerName = value
        End Set
    End Property


    Private _MapName As String
    Public Property MapName As String
        Get
            Return _MapName
        End Get
        Set(value As String)
            _MapName = value
        End Set
    End Property



    Private _PassWord As String
    Public Property PassWord As String
        Get
            Return _PassWord
        End Get
        Set(value As String)
            _PassWord = value
        End Set
    End Property

    Private _CodeDatas As List(Of CodeData)
    Public ReadOnly Property CodeDatas As List(Of CodeData)
        Get
            If _CodeDatas Is Nothing Then
                _CodeDatas = New List(Of CodeData)
            End If
            Return _CodeDatas
        End Get
    End Property



    <Serializable>
    Public Class CodeData
        Public Property TagName As String

        Private TypeNames() As String = {"변수", "데스값"}
        Public Property CodeType As String
            Get
                Return TypeNames(TypeIndex)
            End Get
            Set(value As String)
                TypeIndex = value
            End Set
        End Property

        Public TypeIndex As Integer

        Public Property Value As String
            Get
                If TypeIndex = 0 Then
                    '변수
                    Return NameSpaceName & " " & ValueName
                Else
                    '데스값
                    Return pjData.CodeLabel(SCDatFiles.DatFiles.units, ValueIndex)
                End If
            End Get
            Set(value As String)
                If TypeIndex = 0 Then
                    '변수
                    NameSpaceName = value.Split(",").First
                    ValueName = value.Split(",").Last
                Else
                    '데스값
                    ValueIndex = value
                End If
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
