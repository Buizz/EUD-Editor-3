Imports System.ComponentModel

Public Class UIManager
    Implements INotifyPropertyChanged
    '바인딩 할 때 본체도 넘겨주자고.
    '잘못된 값일 경우 또는 지원안하는 부분일 경우 없에버리기.~

    Private Datfile As SCDatFiles.DatFiles
    Private ObjectID As Integer

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged


    Public Sub New(tDatfile As SCDatFiles.DatFiles, tObjectID As Integer)
        Datfile = tDatfile
        ObjectID = tObjectID
    End Sub

    Public Property Name() As String
        Get
            Return pjData.CodeLabel(Datfile, ObjectID, True)
        End Get
        Set(value As String)

        End Set
    End Property


    Public Sub ChangeProperty()
        NotifyPropertyChanged("Back")
    End Sub

    Public ReadOnly Property Back() As SolidColorBrush
        Get
            If pjData.Dat.GetDatFile(Datfile).CheckDirty(ObjectID) Then
                Return Application.Current.Resources("MaterialDesignPaper")
            Else
                Return New SolidColorBrush(pgData.FiledEditColor)
            End If
        End Get
    End Property

    Public Property ToolTip() As String
        Get
            Return pjData.Dat.ToolTip(Datfile, ObjectID)
        End Get

        Set(ByVal tvalue As String)
            If Not (tvalue = pjData.Dat.ToolTip(Datfile, ObjectID)) Then
                pjData.Dat.ToolTip(Datfile, ObjectID) = tvalue
                NotifyPropertyChanged("ToolTip")
                NotifyPropertyChanged("Name")
            End If
        End Set
    End Property

    Public Property Group() As String
        Get
            Return pjData.Dat.Group(Datfile, ObjectID)
        End Get

        Set(ByVal tvalue As String)
            If Not (tvalue = pjData.Dat.Group(Datfile, ObjectID)) Then
                pjData.Dat.Group(Datfile, ObjectID) = tvalue
                pjData.BindingManager.RefreshCodeTree(Datfile, ObjectID)
                NotifyPropertyChanged("Group")
            End If
        End Set
    End Property

    Public Sub ToolTipReset()
        pjData.Dat.ToolTip(Datfile, ObjectID) = scData.DefaultDat.ToolTip(Datfile, ObjectID)
        NotifyPropertyChanged("ToolTip")
        NotifyPropertyChanged("Name")
    End Sub
    Public Sub GroupReset()
        pjData.Dat.Group(Datfile, ObjectID) = scData.DefaultDat.Group(Datfile, ObjectID)
        NotifyPropertyChanged("Group")
    End Sub


    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
End Class
