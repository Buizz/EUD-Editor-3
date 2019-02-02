Imports System.ComponentModel

Public Class CodeConnecter
    Implements INotifyPropertyChanged

    Private Datfile As SCDatFiles.DatFiles
    Private ObjectID As Integer

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    'Dat종류, 오브젝트 아이디만 있으면 됨

    'Dat종류에 따른 출처들을 미리 정리해두자
    'ex Dat종류 Weapon의 경우, 모든 유닛들의 UnitDat_GroundWeapon을 조사하여 오브젝트 아이디가 같으면 해당 아이디를 보유한 유닛을 추가.

    '이 관계를 이용하여 연결된 오브젝트의 이름도 변경해주면 좋겠음


    Public Sub New(tDatfile As SCDatFiles.DatFiles, tObjectID As Integer)
        Datfile = tDatfile
        ObjectID = tObjectID
    End Sub



    Public ReadOnly Property Items() As ListBoxItem()
        Get
            Dim listboxitems As New List(Of ListBoxItem)

            For i = 0 To 10
                Dim asfaga As New ListBoxItem
                asfaga.Content = "asdfa " & i
                listboxitems.Add(asfaga)
            Next





            Return listboxitems.ToArray
        End Get
    End Property






    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
End Class
