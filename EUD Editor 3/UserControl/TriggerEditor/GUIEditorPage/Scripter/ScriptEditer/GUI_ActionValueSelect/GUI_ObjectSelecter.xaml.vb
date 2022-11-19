Public Class GUI_ObjectSelecter
    Public Event SelectEvent As RoutedEventHandler

    '  RaiseEvent ItemSelect(Selectitem.Tag, e)

    Private scripter As GUIScriptEditor

    Private fliter As String = ""
    Public Sub New(_scripter As GUIScriptEditor)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        scripter = _scripter
        ListReset()
    End Sub
    Private Sub ListReset()
        mainlist.Items.Clear()

        If True Then
            Dim ObjectList As List(Of ScriptBlock) = tescm.GetObjectFromCFunc(Tool.TEEpsDefaultFunc, scripter)
            For i = 0 To ObjectList.Count - 1
                If fliter.Trim = "" Or ObjectList(i).value.ToLower.IndexOf(fliter.ToLower) <> -1 Then
                    Dim listitem As New ListBoxItem
                    listitem.Content = ObjectList(i).value
                    listitem.Tag = ObjectList(i).value
                    mainlist.Items.Add(listitem)
                End If
            Next
        End If
        mainlist.Items.Add(New Separator)
        If True Then
            Dim ObjectList As List(Of ScriptBlock) = tescm.GetGlobalObject(scripter)
            For i = 0 To ObjectList.Count - 1
                If fliter.Trim = "" Or ObjectList(i).value.ToLower.IndexOf(fliter.ToLower) <> -1 Then
                    Dim listitem As New ListBoxItem
                    listitem.Content = ObjectList(i).value
                    listitem.Tag = ObjectList(i).value
                    mainlist.Items.Add(listitem)
                End If
            Next
        End If
        mainlist.Items.Add(New Separator)

        If True Then
            For e = 0 To scripter.ExternFile.Count - 1
                Dim nspace As String = scripter.ExternFile(e).nameSpaceName
                Dim ObjectList As List(Of ScriptBlock) = tescm.GetExternObject(scripter, nspace)
                For i = 0 To ObjectList.Count - 1
                    If fliter.Trim = "" Or ObjectList(i).value.ToLower.IndexOf(fliter.ToLower) <> -1 Then
                        Dim listitem As New ListBoxItem
                        listitem.Content = nspace & "." & ObjectList(i).value
                        listitem.Tag = nspace & "." & ObjectList(i).value
                        mainlist.Items.Add(listitem)
                    End If
                Next
            Next
        End If
    End Sub

    Private Sub mainlist_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim item As ListBoxItem = mainlist.SelectedItem


        RaiseEvent SelectEvent(item.Tag, e)
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
