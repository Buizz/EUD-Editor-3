Public Class DataEditor
    '데이터 상태랑 선택한 인덱스랑 어떤 페이지인지 알고 있어야 함
    Private Fliter As New tFliter
    Private Structure tFliter
        Public fliterText As String

        Public IsEdit As Boolean
        Public SortType As ESortType

        Public Enum ESortType
            ABC
            Tree
            n123
        End Enum
    End Structure
    Private Enum EPageType
        Unit
        Weapon
        Fligy
        Sprite
        Image
        Upgrade
        Tech
        Order
        ButtonSet
    End Enum

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        CodeIndexerList.Items.Clear()


        For i = 0 To scData.SCUnitCount - 1
            'Dim treeviewtemp As New TreeViewItem
            'treeviewtemp.Header = pjData.UnitName(i)


            CodeIndexerList.Items.Add(pjData.UnitName(i))
        Next

    End Sub

    Private Sub ListReset(pagetype As EPageType)
        Select Case Fliter.SortType
            Case tFliter.ESortType.ABC
            Case tFliter.ESortType.n123
            Case tFliter.ESortType.Tree
        End Select


    End Sub





    Private Sub Btn_sortABC(sender As Object, e As RoutedEventArgs)
        Fliter.SortType = tFliter.ESortType.ABC
    End Sub
    Private Sub Btn_sortn123(sender As Object, e As RoutedEventArgs)
        Fliter.SortType = tFliter.ESortType.n123
    End Sub
    Private Sub Btn_sortTree(sender As Object, e As RoutedEventArgs)
        Fliter.SortType = tFliter.ESortType.Tree
    End Sub
    Private Sub Btn_isEdit(sender As Object, e As DependencyPropertyChangedEventArgs)
        Fliter.IsEdit = e.NewValue
    End Sub

End Class
