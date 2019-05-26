

<Serializable>
Public Class StarCraftArchive
    Private _IsUsed As Boolean
    Public Property IsUsed As Boolean
        Get
            Return _IsUsed
        End Get
        Set(value As Boolean)
            _IsUsed = value
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



End Class
