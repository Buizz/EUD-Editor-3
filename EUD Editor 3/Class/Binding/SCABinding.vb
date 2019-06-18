Imports System.ComponentModel

Public Class SCABinding
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Sub PropertyChangedPack()
        NotifyPropertyChanged("MapCode")
        'NotifyPropertyChanged("MiniBackColor")
    End Sub



    Public Property BattleTag() As String
        Get
            Return pjData.TEData.SCArchive.MakerBattleTag
        End Get
        Set(value As String)
            pjData.TEData.SCArchive.MakerBattleTag = value
            PropertyChangedPack()
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return pjData.TEData.SCArchive.MakerServerName
        End Get
        Set(value As String)
            pjData.TEData.SCArchive.MakerServerName = value
            PropertyChangedPack()
        End Set
    End Property



    Public Property MapName() As String
        Get
            Return pjData.TEData.SCArchive.MapName
        End Get
        Set(value As String)
            pjData.TEData.SCArchive.MapName = value
            PropertyChangedPack()
        End Set
    End Property
    Public Property Password() As String
        Get
            Return pjData.TEData.SCArchive.PassWord
        End Get
        Set(value As String)
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
