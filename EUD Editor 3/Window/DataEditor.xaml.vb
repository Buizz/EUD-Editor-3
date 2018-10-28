Public Class DataEditor
    Public Sub OpenbyMainWindow()
        CodeExpander.IsExpanded = True
        Console.IsExpanded = True
    End Sub

    '데이터 상태랑 선택한 인덱스랑 어떤 페이지인지 알고 있어야 함
    Private Fliter As New tFliter
    Private Structure tFliter
        Public fliterText As String

        Public IsEdit As Boolean
        Private TSortType As ESortType
        Public ReadOnly Property SortType As ESortType
            Get
                Return TSortType
            End Get
        End Property



        Public Enum ESortType
            ABC
            Tree
            n123
        End Enum
        Public Sub SetFliter(type As ESortType)
            TSortType = type
        End Sub
    End Structure
    Private Sub SetFliter(tfliter As tFliter.ESortType)
        Select Case tfliter
            Case DataEditor.tFliter.ESortType.n123
                Btnsortn123.IsEnabled = False
                BtnsortABC.IsEnabled = True
                BtnsortTree.IsEnabled = True
            Case DataEditor.tFliter.ESortType.ABC
                Btnsortn123.IsEnabled = True
                BtnsortABC.IsEnabled = False
                BtnsortTree.IsEnabled = True
            Case DataEditor.tFliter.ESortType.Tree
                Btnsortn123.IsEnabled = True
                BtnsortABC.IsEnabled = True
                BtnsortTree.IsEnabled = False
        End Select



        Fliter.SetFliter(tfliter)
    End Sub




    Private CurrentPage As EPageType
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
        Nottting
    End Enum

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        SetFliter(tFliter.ESortType.n123)

        ListReset(EPageType.Unit)
    End Sub

    Private Sub ListReset(Optional pagetype As EPageType = EPageType.Nottting)
        If pagetype = EPageType.Nottting Then
            pagetype = CurrentPage
        Else
            CurrentPage = pagetype
        End If

        Select Case Fliter.SortType
            Case tFliter.ESortType.ABC
                CodeIndexerTree.Visibility = Visibility.Hidden
                CodeIndexerList.Visibility = Visibility.Visible
            Case tFliter.ESortType.n123
                CodeIndexerTree.Visibility = Visibility.Hidden
                CodeIndexerList.Visibility = Visibility.Visible
            Case tFliter.ESortType.Tree
                CodeIndexerTree.Visibility = Visibility.Visible
                CodeIndexerList.Visibility = Visibility.Hidden
        End Select

        CodeIndexerList.Items.Clear()
        For i = 0 To scData.SCUnitCount - 1
            'Dim treeviewtemp As New TreeViewItem
            'treeviewtemp.Header = pjData.UnitName(i)


            CodeIndexerList.Items.Add(pjData.UnitName(i))
        Next
    End Sub




    Private Sub Btn_sortn123(sender As Object, e As RoutedEventArgs)
        SetFliter(tFliter.ESortType.n123)
        ListReset()
    End Sub
    Private Sub Btn_sortABC(sender As Object, e As RoutedEventArgs)
        SetFliter(tFliter.ESortType.ABC)
        ListReset()
    End Sub

    Private Sub Btn_sortTree(sender As Object, e As RoutedEventArgs)
        SetFliter(tFliter.ESortType.Tree)
        ListReset()
    End Sub
    Private Sub Btn_isEdit(sender As Object, e As DependencyPropertyChangedEventArgs)
        Fliter.IsEdit = e.NewValue
    End Sub

End Class
