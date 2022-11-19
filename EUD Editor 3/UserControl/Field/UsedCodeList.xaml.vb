Public Class UsedCodeList
    Private Datfile As SCDatFiles.DatFiles
    Private ObjectID As Integer

    Private MyList As CodeCollection
    'Dat종류, 오브젝트 아이디만 있으면 됨

    'Dat종류에 따른 출처들을 미리 정리해두자
    'ex Dat종류 Weapon의 경우, 모든 유닛들의 UnitDat_GroundWeapon을 조사하여 오브젝트 아이디가 같으면 해당 아이디를 보유한 유닛을 추가.

    '이 관계를 이용하여 연결된 오브젝트의 이름도 변경해주면 좋겠음
    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer)
        Datfile = _DatFile
        ObjectID = _ObjectID

        MyList = pjData.BindingManager.CodeConnecter(Datfile, ObjectID).Items

        Dim bind As New Binding
        bind.Source = MyList
        MainListBox.SetBinding(ListBox.ItemsSourceProperty, bind)

        'MainListBox.ItemsSource = New Binding


        'Me.DataContext = pjData.BindingManager.CodeConnecter(Datfile, ObjectID)
    End Sub


    Public Sub ReLoad(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer)
        Datfile = _DatFile
        ObjectID = _ObjectID

        pjData.BindingManager.CodeConnecter(Datfile, ObjectID).DeleteList(MyList)
        MyList = pjData.BindingManager.CodeConnecter(Datfile, ObjectID).Items

        Dim bind As New Binding
        bind.Source = MyList
        MainListBox.SetBinding(ListBox.ItemsSourceProperty, bind)


        'Me.DataContext = pjData.BindingManager.CodeConnecter(Datfile, ObjectID)
    End Sub


    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        Try
            pjData.BindingManager.CodeConnecter(Datfile, ObjectID).DeleteList(MyList)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub MainListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        'MsgBox(MainListBox.SelectedItem.Tag)
        If MainListBox.SelectedItem IsNot Nothing Then
            Dim Tags() As String = MainListBox.SelectedItem.Tag.ToString.Split(",")

            Dim DatFiles As SCDatFiles.DatFiles = Tags(0)
            Dim index As Integer = Tags(2)
            TabItemTool.WindowTabItem(DatFiles, index)
            MainListBox.SelectedIndex = -1
        End If

    End Sub
End Class
