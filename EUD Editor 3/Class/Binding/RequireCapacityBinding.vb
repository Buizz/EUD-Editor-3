Imports System.ComponentModel

Public Class RequireCapacityBinding
    Implements INotifyPropertyChanged

    Private DatFile As SCDatFiles.DatFiles

    Public Sub New(tDatFile As SCDatFiles.DatFiles)
        DatFile = tDatFile
    End Sub


    Public Sub PropertyChangedPack()

        NotifyPropertyChanged("ReqCapacity")
        NotifyPropertyChanged("ReqCapacityText")
    End Sub

    Public Property ReqCapacity As Integer
        Get
            Return pjData.ExtraDat.RequireData(DatFile).GetCapacity() / pjData.ExtraDat.RequireData(DatFile).GetMaxCapacity() * 100
        End Get
        Set(value As Integer)

        End Set
    End Property
    Public Property ReqCapacityText As String
        Get
            Return "(" & pjData.ExtraDat.RequireData(DatFile).GetCapacity() & "/" & pjData.ExtraDat.RequireData(DatFile).GetMaxCapacity() & " byte)"
        End Get
        Set(value As String)

        End Set
    End Property


    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
