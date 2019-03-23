Imports System.ComponentModel

Public Class RequireDataBinding
    Implements INotifyPropertyChanged

    Private ObjectID As Integer
    Private DatFile As SCDatFiles.DatFiles


    Public Sub New(tObjectID As Integer, tDatFile As SCDatFiles.DatFiles)
        ObjectID = tObjectID
        DatFile = tDatFile
    End Sub


    Private Sub PropertyChangedPack()
        Select Case DatFile
            Case SCDatFiles.DatFiles.Stechdata
                pjData.BindingManager.UIManager(SCDatFiles.DatFiles.techdata, ObjectID).ChangeProperty()
            Case Else
                pjData.BindingManager.UIManager(DatFile, ObjectID).ChangeProperty()
        End Select
        NotifyPropertyChanged("IsDefaultUse")
        NotifyPropertyChanged("IsDontUse")
        NotifyPropertyChanged("IsAlwaysUse")
        NotifyPropertyChanged("IsAlwaysCurrentUse")
        NotifyPropertyChanged("IsCustomUse")


        pjData.BindingManager.RequireCapacityBinding(DatFile).PropertyChangedPack()
    End Sub


    Public Property IsDefaultUse As Boolean
        Get
            Select Case pjData.ExtraDat.RequireData(DatFile).RequireObjectUsed(ObjectID)
                Case CRequireData.RequireUse.DefaultUse
                    Return True
            End Select
            Return False
        End Get
        Set(value As Boolean)
            If value Then
                pjData.ExtraDat.RequireData(DatFile).RequireObjectUsed(ObjectID) = CRequireData.RequireUse.DefaultUse
                PropertyChangedPack()
            End If
        End Set
    End Property

    Public Property IsDontUse As Boolean
        Get
            Select Case pjData.ExtraDat.RequireData(DatFile).RequireObjectUsed(ObjectID)
                Case CRequireData.RequireUse.DontUse
                    Return True
            End Select
            Return False
        End Get
        Set(value As Boolean)
            If value Then
                pjData.ExtraDat.RequireData(DatFile).RequireObjectUsed(ObjectID) = CRequireData.RequireUse.DontUse
                PropertyChangedPack()
            End If
        End Set
    End Property

    Public Property IsAlwaysUse As Boolean
        Get
            Select Case pjData.ExtraDat.RequireData(DatFile).RequireObjectUsed(ObjectID)
                Case CRequireData.RequireUse.AlwaysUse
                    Return True
            End Select
            Return False
        End Get
        Set(value As Boolean)
            If value Then
                pjData.ExtraDat.RequireData(DatFile).RequireObjectUsed(ObjectID) = CRequireData.RequireUse.AlwaysUse
                PropertyChangedPack()
            End If
        End Set
    End Property

    Public Property IsAlwaysCurrentUse As Boolean
        Get
            Select Case pjData.ExtraDat.RequireData(DatFile).RequireObjectUsed(ObjectID)
                Case CRequireData.RequireUse.AlwaysCurrentUse
                    Return True
            End Select
            Return False
        End Get
        Set(value As Boolean)
            If value Then
                pjData.ExtraDat.RequireData(DatFile).RequireObjectUsed(ObjectID) = CRequireData.RequireUse.AlwaysCurrentUse
                PropertyChangedPack()
            End If
        End Set
    End Property

    Public Property IsCustomUse As Boolean
        Get
            Select Case pjData.ExtraDat.RequireData(DatFile).RequireObjectUsed(ObjectID)
                Case CRequireData.RequireUse.CustomUse
                    Return True
            End Select
            Return False
        End Get
        Set(value As Boolean)
            If value Then
                pjData.ExtraDat.RequireData(DatFile).RequireObjectUsed(ObjectID) = CRequireData.RequireUse.CustomUse
                PropertyChangedPack()
            End If
        End Set
    End Property



    'DefaultUse
    '  DontUse
    '  AlwaysUse
    '  AlwaysCurrentUse
    '  CustomUse

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
