Imports System.ComponentModel

Public Class ProjectExplorer
    Private TEData As TriggerEditorData
    Private FliterText As String = ""

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        CopyItems = New List(Of TEFile)
        SelectItems = New List(Of TreeViewItem)
        PreviewSelectItems = New List(Of TreeViewItem)
        MainTreeview.HorizontalContentAlignment = HorizontalAlignment.Left

        TEData = pjData.TEData
        MainTreeview.Tag = TEData.PFIles

        ResetList()

        AddHotKeys()
    End Sub
    Public Sub ResetList()
        MainTreeview.BeginInit()
        MainTreeview.Items.Clear()
        LoadFromData(MainTreeview.Items, TEData.PFIles)

        For i = 0 To MainTreeview.Items.Count - 1
            CType(MainTreeview.Items(i), TreeViewItem).IsExpanded = True
        Next
        MainTreeview.EndInit()
    End Sub

    Private Sub SaveExpandedStatusExec()
        For i = 0 To MainTreeview.Items.Count - 1
            SaveExpandedStatus(MainTreeview.Items(i))
        Next
    End Sub
    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        SaveExpandedStatusExec()
    End Sub
    Private Sub SaveExpandedStatus(parent As TreeViewItem)
        CType(parent.Tag, TEFile).IsExpanded = parent.IsExpanded
        For i = 0 To parent.Items.Count - 1
            SaveExpandedStatus(parent.Items(i))
        Next
    End Sub

    Private Sub FliterText_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Then
            SaveExpandedStatusExec()

            FliterText = FliterTextBox.Text
            MainTreeview.Items.Clear()
            LoadFromData(MainTreeview.Items, TEData.PFIles)

            For i = 0 To MainTreeview.Items.Count - 1
                CType(MainTreeview.Items(i), TreeViewItem).IsExpanded = True
            Next
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        SaveExpandedStatusExec()

        FliterTextBox.Text = ""
        FliterText = ""
        MainTreeview.Items.Clear()
        LoadFromData(MainTreeview.Items, TEData.PFIles)

        For i = 0 To MainTreeview.Items.Count - 1
            CType(MainTreeview.Items(i), TreeViewItem).IsExpanded = True
        Next
    End Sub
End Class
