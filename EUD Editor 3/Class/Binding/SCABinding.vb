Imports System.ComponentModel

Public Class SCABinding
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Sub PropertyChangedPack()
        NotifyPropertyChanged("MapCode")
        'NotifyPropertyChanged("MiniBackColor")
    End Sub



    Public Property SCAUse() As Boolean
        Get
            Return pjData.TEData.SCArchive.IsUsed
        End Get
        Set(value As Boolean)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.IsUsed = value
            PropertyChangedPack()
        End Set
    End Property

    Public Property SCATestMode() As Integer
        Get
            Return CInt(pjData.TEData.SCArchive.TestMode)
        End Get
        Set(value As Integer)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.TestMode = value
            PropertyChangedPack()
        End Set
    End Property
    Public Property BattleTag() As String
        Get
            Return pjData.TEData.SCArchive.MakerBattleTag
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.MakerBattleTag = value
            PropertyChangedPack()
        End Set
    End Property

    Public Property DataSize() As String
        Get
            Return pjData.TEData.SCArchive.DataSpace
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.DataSpace = value
            PropertyChangedPack()
        End Set
    End Property
    Public Property UserName() As String
        Get
            Return pjData.TEData.SCArchive.MakerServerName
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.MakerServerName = value
            PropertyChangedPack()
        End Set
    End Property


    Public Property MakerEmail() As String
        Get
            Return pjData.TEData.SCArchive.MakerEmail
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.MakerEmail = value
            PropertyChangedPack()
        End Set
    End Property


    Public Property MapName() As String
        Get
            Return pjData.TEData.SCArchive.MapName
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.MapName = value
            PropertyChangedPack()
        End Set
    End Property
    Public Property Password() As String
        Get
            Return pjData.TEData.SCArchive.PassWord
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.PassWord = value
        End Set
    End Property


    Public Property MapCode() As String
        Get
            Return pjData.EudplibData.GetMapCode
        End Get
        Set(value As String)

        End Set
    End Property
    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
End Class
