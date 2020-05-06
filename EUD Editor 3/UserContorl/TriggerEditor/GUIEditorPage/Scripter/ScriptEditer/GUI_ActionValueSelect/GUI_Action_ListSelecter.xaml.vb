Public Class GUI_Action_ListSelecter
    Public Event SelectEvent As RoutedEventHandler

    '  RaiseEvent ItemSelect(Selectitem.Tag, e)

    Private strs() As String
    Private fliter As String = ""
    Public Sub New(_strs() As String)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        strs = _strs
        ListReset()
    End Sub
    Private Sub ListReset()
        mainlist.Items.Clear()

        For i = 0 To strs.Count - 1
            If strs(i).Trim = "" Then
                Continue For
            End If

            If fliter.Trim = "" Or strs(i).ToLower.IndexOf(fliter.ToLower) <> -1 Then
                Dim listitem As New ListBoxItem
                listitem.Content = strs(i)
                listitem.Tag = strs(i)
                mainlist.Items.Add(listitem)
            End If
        Next
    End Sub

    Private Sub mainlist_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim item As ListBoxItem = mainlist.SelectedItem
        If item IsNot Nothing Then
            RaiseEvent SelectEvent(item.Tag, e)
        End If
    End Sub

    Private Sub FliterText_TextChanged(sender As Object, e As TextChangedEventArgs)
        fliter = FliterText.Text
        ListReset()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        FliterText.Text = ""
    End Sub

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        FliterText.Focus()
    End Sub
End Class
