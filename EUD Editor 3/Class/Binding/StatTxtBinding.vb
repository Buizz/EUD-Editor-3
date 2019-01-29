Imports System.ComponentModel

Public Class StatTxtBinding
    Implements INotifyPropertyChanged

    Private ObjectID As Integer

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged



    Public Sub New(tObjectID As Integer)
        ObjectID = tObjectID
    End Sub



    Public Property Value() As String
        Get
            If pjData.ExtraDat.Stat_txt(ObjectID) = ExtraDatFiles.StatNullString Then
                Return pjData.Stat_txt(ObjectID)
            End If
            Return pjData.ExtraDat.Stat_txt(ObjectID)
        End Get

        Set(ByVal tvalue As String)
            If Not (tvalue = pjData.ExtraDat.Stat_txt(ObjectID)) Then
                pjData.SetDirty(True)
                'MsgBox("데이터 파인딩 셋")
                pjData.ExtraDat.Stat_txt(ObjectID) = tvalue
                NotifyPropertyChanged("Value")
                pjData.BindingManager.UIManager(SCDatFiles.DatFiles.stattxt, ObjectID).NameRefresh()
                pjData.BindingManager.UIManager(SCDatFiles.DatFiles.stattxt, ObjectID).BackColorRefresh()
            End If
        End Set
    End Property
    Public Sub DataReset()
        Value = ExtraDatFiles.StatNullString
    End Sub

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
End Class
