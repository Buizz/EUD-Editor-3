Imports System.Windows.Media.Animation

Public Class SCADataList


    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        AnimationInit()
        List.ItemsSource = pjData.TEData.SCArchive.CodeDatas
        NameCombobox.Items.Clear()
        InitStartFileCombox("", pjData.TEData.PFIles)
    End Sub
    Private Sub InitStartFileCombox(Path As String, tTEfile As TEFile)
        For i = 0 To tTEfile.FileCount - 1
            Dim Filename As String = Path & tTEfile.Files(i).RealFileName
            Dim tComboboxitem As New ComboBoxItem
            tComboboxitem.Tag = tTEfile.Files(i)
            tComboboxitem.Content = Filename
            NameCombobox.Items.Add(tComboboxitem)

            'If pjData.TEData.MainFile Is tTEfile.Files(i) Then
            '    NameCombobox.SelectedIndex = NameCombobox.Items.Count - 1
            'End If
        Next


        For i = 0 To tTEfile.FolderCount - 1
            If tTEfile.Folders(i).FileType <> TEFile.EFileType.Setting Then
                Dim Filename As String = Path & tTEfile.Folders(i).FileName & "\"
                InitStartFileCombox(Filename, tTEfile.Folders(i))
            End If
        Next
        If NameCombobox.Items.Count <> 0 Then
            NameCombobox.SelectedIndex = 0
        End If

    End Sub




    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        OpenNewWindow()
        'pjData.TEData.SCArchive.CodeDatas.Add(New StarCraftArchive.CodeData("레벨", "데스값 " & List.Items.Count, "마린"))
        'List.Items.Refresh()
    End Sub

    Private Sub EditItem_Click(sender As Object, e As RoutedEventArgs)
        OpenEditWindow()
    End Sub

    Private Sub DeleteItem_Click(sender As Object, e As RoutedEventArgs)
        pjData.TEData.SCArchive.CodeDatas.Remove(CType(List.SelectedItem, StarCraftArchive.CodeData))
        List.Items.Refresh()
    End Sub

    Private Sub ContextMenu_Opened(sender As Object, e As RoutedEventArgs)
        If List.SelectedItem Is Nothing Then
            EditItem.IsEnabled = False
            DeleteItem.IsEnabled = False
        Else
            EditItem.IsEnabled = True
            DeleteItem.IsEnabled = True
        End If
    End Sub

    Private IsEditWindowopen As Boolean
    Private Sub OpenNewWindow()
        IsEditWindowopen = False
        OpenStroyBoard.Begin(Me)
        DataName.Text = ""
        TypeCB.SelectedIndex = 1
        ValueSelecter.Init(SCDatFiles.DatFiles.units, "유닛", 0)


        ValueSelecter.Visibility = Visibility.Visible
        VariableField.Visibility = Visibility.Collapsed
        CreateEditWindow.Visibility = Visibility.Visible
    End Sub
    Private Sub OpenEditWindow()
        IsEditWindowopen = True
        OpenStroyBoard.Begin(Me)
        Dim SelCodeData As StarCraftArchive.CodeData = CType(List.SelectedItem, StarCraftArchive.CodeData)

        DataName.Text = SelCodeData.TagName
        TypeCB.SelectedIndex = SelCodeData.TypeIndex
        If TypeCB.SelectedIndex = 0 Then
            ValueSelecter.Visibility = Visibility.Collapsed
            VariableField.Visibility = Visibility.Visible

            For i = 0 To NameCombobox.Items.Count - 1
                If SelCodeData.NameSpaceName = CType(NameCombobox.Items(i), ComboBoxItem).Content Then
                    NameCombobox.SelectedIndex = i
                    Exit For
                End If
            Next

            VarSelecter.Items.Clear()
            If NameCombobox.SelectedItem IsNot Nothing Then
                Dim tCfun As New CFunc()
                tCfun.LoadFunc(CType(CType(NameCombobox.SelectedItem, ComboBoxItem).Tag, TEFile).Scripter.GetStringText)
                For i = 0 To tCfun.VariableCount - 1
                    VarSelecter.Items.Add(tCfun.GetVariableNames(i))
                    If SelCodeData.ValueName = tCfun.GetVariableNames(i) Then
                        VarSelecter.SelectedIndex = i
                    End If
                Next
            End If



        Else
            ValueSelecter.Init(SCDatFiles.DatFiles.units, "데스값", SelCodeData.ValueIndex)
        End If

        CreateEditWindow.Visibility = Visibility.Visible
    End Sub
    Private Sub TypeCB_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If CreateEditWindow.Visibility = Visibility.Visible Then
            If TypeCB.SelectedIndex = 0 Then
                ValueSelecter.Visibility = Visibility.Collapsed
                VariableField.Visibility = Visibility.Visible
            Else
                ValueSelecter.Init(SCDatFiles.DatFiles.units, "데스값", 0)
                ValueSelecter.Visibility = Visibility.Visible
                VariableField.Visibility = Visibility.Collapsed
            End If
        End If
    End Sub

    Private Sub OkKey_Click(sender As Object, e As RoutedEventArgs)
        pjData.SetDirty(True)
        If IsEditWindowopen Then
            If TypeCB.SelectedIndex = 0 Then
                CType(List.SelectedItem, StarCraftArchive.CodeData).Refresh(DataName.Text, TypeCB.SelectedIndex, CType(NameCombobox.SelectedItem, ComboBoxItem).Content & "," & VarSelecter.SelectedItem)
            Else
                CType(List.SelectedItem, StarCraftArchive.CodeData).Refresh(DataName.Text, TypeCB.SelectedIndex, ValueSelecter.Value)
            End If


        Else
            If TypeCB.SelectedIndex = 0 Then
                pjData.TEData.SCArchive.CodeDatas.Add(New StarCraftArchive.CodeData(DataName.Text, TypeCB.SelectedIndex, CType(NameCombobox.SelectedItem, ComboBoxItem).Content & "," & VarSelecter.SelectedItem))
            Else
                pjData.TEData.SCArchive.CodeDatas.Add(New StarCraftArchive.CodeData(DataName.Text, TypeCB.SelectedIndex, ValueSelecter.Value))
            End If
        End If
        List.Items.Refresh()
        CloseStroyBoard.Begin(Me)
    End Sub
    Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs)
        CloseStroyBoard.Begin(Me)
    End Sub
    Private OpenStroyBoard As Storyboard
    Private CloseStroyBoard As Storyboard
    Private Sub AnimationInit()
        If True Then
            Dim scale1 As ScaleTransform = New ScaleTransform(1, 1)

            InputDialog.RenderTransformOrigin = New Point(0.5, 0.5)
            InputDialog.RenderTransform = scale1



            Dim myHeightAnimation As DoubleAnimation = New DoubleAnimation()
            myHeightAnimation.From = 0.0
            myHeightAnimation.To = 1.0
            myHeightAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myWidthAnimation As DoubleAnimation = New DoubleAnimation()
            myWidthAnimation.From = 0.0
            myWidthAnimation.To = 1.0
            myWidthAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myOpacityAnimation As DoubleAnimation = New DoubleAnimation()
            myOpacityAnimation.From = 0.0
            myOpacityAnimation.To = 1.0
            myOpacityAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            OpenStroyBoard = New Storyboard()
            OpenStroyBoard.Children.Add(myOpacityAnimation)
            OpenStroyBoard.Children.Add(myWidthAnimation)
            OpenStroyBoard.Children.Add(myHeightAnimation)
            Storyboard.SetTargetName(myOpacityAnimation, CreateEditWindow.Name)
            Storyboard.SetTargetName(myWidthAnimation, InputDialog.Name)
            Storyboard.SetTargetName(myHeightAnimation, InputDialog.Name)
            Storyboard.SetTargetProperty(myOpacityAnimation, New PropertyPath(Border.OpacityProperty))
            Storyboard.SetTargetProperty(myHeightAnimation, New PropertyPath("RenderTransform.ScaleY"))
            Storyboard.SetTargetProperty(myWidthAnimation, New PropertyPath("RenderTransform.ScaleX"))
        End If
        If True Then
            Dim scale1 As ScaleTransform = New ScaleTransform(1, 1)

            InputDialog.RenderTransformOrigin = New Point(0.5, 0.5)
            InputDialog.RenderTransform = scale1



            Dim myHeightAnimation As DoubleAnimation = New DoubleAnimation()
            myHeightAnimation.From = 1.0
            myHeightAnimation.To = 0.0
            myHeightAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myWidthAnimation As DoubleAnimation = New DoubleAnimation()
            myWidthAnimation.From = 1.0
            myWidthAnimation.To = 0.0
            myWidthAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myOpacityAnimation As DoubleAnimation = New DoubleAnimation()
            myOpacityAnimation.From = 1.0
            myOpacityAnimation.To = 0.0
            myOpacityAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            CloseStroyBoard = New Storyboard()
            CloseStroyBoard.Children.Add(myOpacityAnimation)
            CloseStroyBoard.Children.Add(myWidthAnimation)
            CloseStroyBoard.Children.Add(myHeightAnimation)
            Storyboard.SetTargetName(myOpacityAnimation, CreateEditWindow.Name)
            Storyboard.SetTargetName(myWidthAnimation, InputDialog.Name)
            Storyboard.SetTargetName(myHeightAnimation, InputDialog.Name)
            Storyboard.SetTargetProperty(myOpacityAnimation, New PropertyPath(Border.OpacityProperty))
            Storyboard.SetTargetProperty(myHeightAnimation, New PropertyPath("RenderTransform.ScaleY"))
            Storyboard.SetTargetProperty(myWidthAnimation, New PropertyPath("RenderTransform.ScaleX"))

            AddHandler CloseStroyBoard.Completed, Sub(sender As Object, e As EventArgs)
                                                      CreateEditWindow.Visibility = Visibility.Hidden
                                                  End Sub
        End If



        'InputDialog
    End Sub

    Private Sub NameCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If CreateEditWindow.Visibility = Visibility.Visible Then
            If NameCombobox.SelectedItem IsNot Nothing Then
                If List.SelectedItem IsNot Nothing Then
                    Dim SelCodeData As StarCraftArchive.CodeData = CType(List.SelectedItem, StarCraftArchive.CodeData)



                    VarSelecter.Items.Clear()
                    Dim tCfun As New CFunc()
                    tCfun.LoadFunc(CType(CType(NameCombobox.SelectedItem, ComboBoxItem).Tag, TEFile).Scripter.GetStringText)
                    For i = 0 To tCfun.VariableCount - 1
                        If tCfun.GetVariableType(i).IndexOf("PVariable") <> -1 Then
                            VarSelecter.Items.Add(tCfun.GetVariableNames(i))
                            If SelCodeData.ValueName = tCfun.GetVariableNames(i) Then
                                VarSelecter.SelectedIndex = i
                            End If
                        End If
                    Next
                Else
                    VarSelecter.Items.Clear()
                    Dim tCfun As New CFunc()
                    tCfun.LoadFunc(CType(CType(NameCombobox.SelectedItem, ComboBoxItem).Tag, TEFile).Scripter.GetStringText)
                    For i = 0 To tCfun.VariableCount - 1
                        If tCfun.GetVariableType(i).IndexOf("PVariable") <> -1 Then
                            VarSelecter.Items.Add(tCfun.GetVariableNames(i))
                        End If
                    Next
                    If VarSelecter.Items.Count <> 0 Then
                        VarSelecter.SelectedIndex = 0
                    End If

                End If
            End If
        End If
    End Sub

End Class

