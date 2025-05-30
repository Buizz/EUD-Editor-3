﻿Imports System.ComponentModel

Public Class SCABinding
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Sub PropertyChangedPack()
        NotifyPropertyChanged("MapCode")

        'NotifyPropertyChanged("MiniBackColor")
    End Sub
    Public Sub LoginRefresh()
        NotifyPropertyChanged("UserName")
        NotifyPropertyChanged("MapName")
        NotifyPropertyChanged("SubTitle")
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
    Public Property ViewPublic() As Boolean
        Get
            Return pjData.TEData.SCArchive.ViewPublic
        End Get
        Set(value As Boolean)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.ViewPublic = value
            PropertyChangedPack()
        End Set
    End Property

    Public Property UseOldBattleTag() As Boolean
        Get
            Return pjData.TEData.SCArchive.IsUseOldBattleTag
        End Get
        Set(value As Boolean)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.IsUseOldBattleTag = value
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


    Public Property SCAEmail() As String
        Get
            Return pjData.TEData.SCArchive.SCAEmail
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.SCAEmail = value
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
    Public Property MSQCSize() As String
        Get
            Return pjData.TEData.SCArchive.MSQCSize
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.MSQCSize = value
            PropertyChangedPack()
        End Set
    End Property




    Public Property FuncSize() As String
        Get
            Return pjData.TEData.SCArchive.FuncSpace
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.FuncSpace = value
            PropertyChangedPack()
        End Set
    End Property
    Public Property SCAScriptVarCount() As String
        Get
            Return pjData.TEData.SCArchive.SCAScriptVarCount
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.SCAScriptVarCount = value
            PropertyChangedPack()
        End Set
    End Property



    Public Property UserName() As String
        Get
            If pjData.TEData.SCArchive.IsLogin Then
                Return pjData.TEData.SCArchive.MakerServerName
            Else
                Return "****"
            End If

        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.MakerServerName = value
            PropertyChangedPack()
        End Set
    End Property

    Public Property MapName() As String
        Get
            If pjData.TEData.SCArchive.IsLogin Then
                Return pjData.TEData.SCArchive.MapName
            Else
                Return "****"
            End If
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.MapName = value
            PropertyChangedPack()
        End Set
    End Property

    Public Property SubTitle() As String
        Get
            If pjData.TEData.SCArchive.IsLogin Then
                Return pjData.TEData.SCArchive.SubTitle
            Else
                Return "****"
            End If
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.SubTitle = value
            PropertyChangedPack()
        End Set
    End Property




    Public Property updateinfo() As Boolean
        Get
            Return pjData.TEData.SCArchive.updateinfo
        End Get
        Set(value As Boolean)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.updateinfo = value
            PropertyChangedPack()
        End Set
    End Property
    Public Property MapTags() As String
        Get
            Return pjData.TEData.SCArchive.MapTags
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.MapTags = value
            PropertyChangedPack()
        End Set
    End Property
    Public Property MapTitle() As String
        Get
            Return pjData.TEData.SCArchive.MapTitle
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.MapTitle = value
            PropertyChangedPack()
        End Set
    End Property
    Public Property ImageLink() As String
        Get
            Return pjData.TEData.SCArchive.ImageLink
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.ImageLink = value
            PropertyChangedPack()
        End Set
    End Property
    Public Property DownLink() As String
        Get
            Return pjData.TEData.SCArchive.DownLink
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.DownLink = value
            PropertyChangedPack()
        End Set
    End Property
    Public Property MapDes() As String
        Get
            Return pjData.TEData.SCArchive.MapDes
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            pjData.TEData.SCArchive.MapDes = value
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



    'Public Property Password() As String
    '    Get
    '        Return pjData.TEData.SCArchive.PassWord
    '    End Get
    '    Set(value As String)
    '        pjData.SetDirty(True)
    '        pjData.TEData.SCArchive.PassWord = value
    '    End Set
    'End Property


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
