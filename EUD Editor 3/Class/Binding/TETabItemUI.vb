Imports System.ComponentModel

<Serializable>
Public Class TETabItemUI
    Implements INotifyPropertyChanged

    Private TEFile As TEFile


    Public Sub New(tTEFile As TEFile)
        TEFile = tTEFile
    End Sub

    Public Sub PropertyChangedPack()
        NotifyPropertyChanged("TabName")
    End Sub



    Public Property TabName As String
        Get
            Return TEFile.FileName
        End Get
        Set(value As String)
            TEFile.FileName = value
        End Set
    End Property



    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
    <NonSerialized>
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
