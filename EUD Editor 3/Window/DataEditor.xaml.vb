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



        Select Case pagetype
            Case EPageType.Unit
                ListResetUnit()
        End Select




    End Sub

    Private Structure UnitName
        Public Name As String
        Public index As Integer

        Public Sub New(tname As String, tindex As Integer)
            Name = tname
            index = tindex
        End Sub
    End Structure

    Private Sub ListResetUnit()
        Select Case Fliter.SortType
            Case tFliter.ESortType.n123
                CodeIndexerList.Items.Clear()
                For i = 0 To scData.SCUnitCount - 1
                    Dim unitname As String = "[" & Format(i, "000") & "]-" & pjData.UnitName(i)

                    Dim tListItem As New ListBoxItem()
                    tListItem.Tag = i
                    tListItem.Content = unitname

                    CodeIndexerList.Items.Add(tListItem)
                Next
            Case tFliter.ESortType.ABC
                Dim tList As New List(Of UnitName)
                For i = 0 To scData.SCUnitCount - 1
                    tList.Add(New UnitName(pjData.UnitName(i), i))


                Next
                tList.Sort(Function(x, y) x.Name.CompareTo(y.Name))


                CodeIndexerList.Items.Clear()
                For i = 0 To scData.SCUnitCount - 1
                    Dim index As Integer = tList(i).index

                    Dim unitname As String = "[" & Format(index, "000") & "]-" & tList(i).Name

                    Dim tListItem As New ListBoxItem()
                    tListItem.Tag = index
                    tListItem.Content = unitname
                    CodeIndexerList.Items.Add(tListItem)
                Next

            Case tFliter.ESortType.Tree
                CodeIndexerTree.Items.Clear()

                Dim strs As String() = {"Zerg", "Terran", "Protoss", "Neutral", "Undefined"}
                For i = 0 To strs.Count - 1
                    Dim treeitem As New TreeViewItem()
                    treeitem.Header = strs(i)
                    CodeIndexerTree.Items.Add(treeitem)
                Next

                For i = 0 To scData.SCUnitCount - 1
                    Dim unitname As String = "[" & Format(i, "000") & "]-" & pjData.UnitFullName(i)

                    Dim tListItem As New TreeViewItem()
                    tListItem.Tag = i
                    tListItem.Header = unitname

                    CodeIndexerTree.Items.Add(tListItem)
                Next
        End Select
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
