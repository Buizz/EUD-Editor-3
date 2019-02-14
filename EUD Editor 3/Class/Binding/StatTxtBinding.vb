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

                If ObjectID < 228 Then
                    pjData.BindingManager.UIManager(SCDatFiles.DatFiles.units, ObjectID).NameRefresh()
                End If
                '해당 라벨을 쓰는 것들에게 전부 리프레시 명령을 내려야함..
                Dim CodeGroup As CodeConnectGroup = pjData.BindingManager.CodeConnectGroup(SCDatFiles.DatFiles.stattxt)
                For i = 0 To CodeGroup.Count - 1
                    Dim DatFile As SCDatFiles.DatFiles = CodeGroup.GetDatType(i)
                    Dim ParamName As String = CodeGroup.GetParamName(i)

                    For k = 0 To SCCodeCount(DatFile) - 1
                        If pjData.Dat.Data(DatFile, ParamName, k) = ObjectID Then '현재 이 Stat를 사용중일 경우
                            pjData.BindingManager.UIManager(DatFile, k).NameRefresh()
                        End If
                    Next

                Next
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
