Imports System.ComponentModel

Public Class ToolTipManager
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private customerNameValue As String
    Public Property CustomerName() As String
        Get
            Return customerNameValue
        End Get

        Set(ByVal value As String)
            If Not (value = customerNameValue) Then
                MsgBox("뭐야 " & value)
                customerNameValue = value
                NotifyPropertyChanged("CustomerName")
            End If
        End Set
    End Property


    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
End Class