Public Class RequireListBox
    Private DatFile As SCDatFiles.DatFiles
    Private ObjectID As Integer
    Private Parameter As String


    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        DataContext = pjData.BindingManager.RequireDataBinding(ObjectID, DatFile)

        Dim RequireList As List(Of CRequireData.RequireBlock) = pjData.ExtraDat.RequireData(DatFile).GetRequireObject(ObjectID)
        MainListBox.Items.Clear()

        For i = 0 To RequireList.Count - 1
            Dim listboxitem As New ListBoxItem
            Dim Stackpanle As New StackPanel



            If RequireList(i).HasValue Then
                Dim tb As New TextBlock
                tb.Text = RequireList(i).opCode & vbCrLf & RequireList(i).Value
                Stackpanle.Children.Add(tb)
            Else
                Dim tb As New TextBlock
                tb.Text = RequireList(i).opCode

                Stackpanle.Children.Add(tb)
            End If

            listboxitem.Content = Stackpanle
            MainListBox.Items.Add(listboxitem)
        Next

    End Sub
    Public Sub ReLoad(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        DataContext = pjData.BindingManager.RequireDataBinding(ObjectID, DatFile)

        Dim RequireList As List(Of CRequireData.RequireBlock) = pjData.ExtraDat.RequireData(DatFile).GetRequireObject(ObjectID)
        MainListBox.Items.Clear()

        For i = 0 To RequireList.Count - 1
            Dim listboxitem As New ListBoxItem
            Dim Stackpanle As New StackPanel



            If RequireList(i).HasValue Then
                Dim tb As New TextBlock
                tb.Text = RequireList(i).opCode & vbCrLf & RequireList(i).Value
                Stackpanle.Children.Add(tb)
            Else
                Dim tb As New TextBlock
                tb.Text = RequireList(i).opCode

                Stackpanle.Children.Add(tb)
            End If

            listboxitem.Content = Stackpanle
            MainListBox.Items.Add(listboxitem)
        Next
    End Sub

    Private Sub Sample1_DialogHost_OnDialogClosing(sender As Object, eventArgs As MaterialDesignThemes.Wpf.DialogClosingEventArgs)

    End Sub
End Class
