Imports System.Windows.Media.Animation

Public Class SCADataList

    Public Sub Refresh()
        List.ItemsSource = pjData.TEData.SCArchive.CodeDatas
        NameCombobox.Items.Clear()
        InitStartFileCombox("", pjData.TEData.PFIles)
    End Sub

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
        pjData.SetDirty(True)
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
        OpenStoryBoard.Begin(Me)
        DataName.Text = ""
        Checekname()
        TypeCB.SelectedIndex = 1
        ValueSelecter.Init(SCDatFiles.DatFiles.units, "유닛", 0)


        ValueSelecter.Visibility = Visibility.Visible
        VariableField.Visibility = Visibility.Collapsed
        CreateEditWindow.Visibility = Visibility.Visible
    End Sub
    Private Sub OpenEditWindow()
        IsEditWindowopen = True
        OpenStoryBoard.Begin(Me)
        Dim SelCodeData As StarCraftArchive.CodeData = CType(List.SelectedItem, StarCraftArchive.CodeData)

        DataName.Text = SelCodeData.TagName
        Checekname()
        TypeCB.SelectedIndex = SelCodeData.TypeIndex
        Select Case TypeCB.SelectedIndex
            Case 0
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
                        Dim variableType As String = tCfun.GetVariableType(i)
                        Dim flag As Boolean = variableType.IndexOf("PVariable") <> -1


                        If flag Then
                            VarSelecter.Items.Add(tCfun.GetVariableNames(i))
                            If SelCodeData.ValueName = tCfun.GetVariableNames(i) Then
                                VarSelecter.SelectedIndex = VarSelecter.Items.Count - 1
                            End If
                        End If
                    Next
                End If
            Case 1
                ValueSelecter.Init(SCDatFiles.DatFiles.units, "데스값", SelCodeData.ValueIndex)
            Case 2
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
                        Dim variableType As String = tCfun.GetVariableType(i)
                        Dim flag As Boolean = variableType.IndexOf("EUDArray") <> -1


                        If flag Then
                            VarSelecter.Items.Add(tCfun.GetVariableNames(i))
                            If SelCodeData.ValueName = tCfun.GetVariableNames(i) Then
                                VarSelecter.SelectedIndex = VarSelecter.Items.Count - 1
                            End If
                        End If
                    Next
                End If
        End Select
        If TypeCB.SelectedIndex = 1 Then
            InitTypeCB(SelCodeData.ValueIndex)
        Else
            InitTypeCB()
        End If



        CreateEditWindow.Visibility = Visibility.Visible
    End Sub
    Private Sub TypeCB_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If CreateEditWindow.Visibility = Visibility.Visible Then
            InitTypeCB()
        End If
    End Sub
    Private Sub InitTypeCB(Optional initvalue As Integer = 0)
        Select Case TypeCB.SelectedIndex
            Case 0
                ValueSelecter.Visibility = Visibility.Collapsed
                VariableField.Visibility = Visibility.Visible
                OkKey.IsEnabled = False
                VariableReset()
            Case 1
                ValueSelecter.Init(SCDatFiles.DatFiles.units, "데스값", initvalue)
                ValueSelecter.Visibility = Visibility.Visible
                VariableField.Visibility = Visibility.Collapsed
                Checekname()
            Case 2
                ValueSelecter.Visibility = Visibility.Collapsed
                VariableField.Visibility = Visibility.Visible
                OkKey.IsEnabled = False
                VariableReset()
        End Select
    End Sub


    Private Sub OkKey_Click(sender As Object, e As RoutedEventArgs)
        pjData.SetDirty(True)
        If IsEditWindowopen Then
            Select Case TypeCB.SelectedIndex
                Case 0
                    CType(List.SelectedItem, StarCraftArchive.CodeData).Refresh(DataName.Text, TypeCB.SelectedIndex, CType(NameCombobox.SelectedItem, ComboBoxItem).Content & "," & VarSelecter.SelectedItem)
                Case 1
                    CType(List.SelectedItem, StarCraftArchive.CodeData).Refresh(DataName.Text, TypeCB.SelectedIndex, ValueSelecter.Value)
                Case 2
                    CType(List.SelectedItem, StarCraftArchive.CodeData).Refresh(DataName.Text, TypeCB.SelectedIndex, CType(NameCombobox.SelectedItem, ComboBoxItem).Content & "," & VarSelecter.SelectedItem)
            End Select
        Else

            Select Case TypeCB.SelectedIndex
                Case 0
                    pjData.TEData.SCArchive.CodeDatas.Add(New StarCraftArchive.CodeData(DataName.Text, TypeCB.SelectedIndex, CType(NameCombobox.SelectedItem, ComboBoxItem).Content & "," & VarSelecter.SelectedItem))
                Case 1
                    pjData.TEData.SCArchive.CodeDatas.Add(New StarCraftArchive.CodeData(DataName.Text, TypeCB.SelectedIndex, ValueSelecter.Value))
                Case 2
                    pjData.TEData.SCArchive.CodeDatas.Add(New StarCraftArchive.CodeData(DataName.Text, TypeCB.SelectedIndex, CType(NameCombobox.SelectedItem, ComboBoxItem).Content & "," & VarSelecter.SelectedItem))
            End Select
        End If
        List.Items.Refresh()
        CloseStoryBoard.Begin(Me)
        pjData.SetDirty(True)
    End Sub
    Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs)
        CloseStoryBoard.Begin(Me)
    End Sub
    Private OpenStoryBoard As Storyboard
    Private CloseStoryBoard As Storyboard
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

            OpenStoryBoard = New Storyboard()
            OpenStoryBoard.Children.Add(myOpacityAnimation)
            OpenStoryBoard.Children.Add(myWidthAnimation)
            OpenStoryBoard.Children.Add(myHeightAnimation)
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

            CloseStoryBoard = New Storyboard()
            CloseStoryBoard.Children.Add(myOpacityAnimation)
            CloseStoryBoard.Children.Add(myWidthAnimation)
            CloseStoryBoard.Children.Add(myHeightAnimation)
            Storyboard.SetTargetName(myOpacityAnimation, CreateEditWindow.Name)
            Storyboard.SetTargetName(myWidthAnimation, InputDialog.Name)
            Storyboard.SetTargetName(myHeightAnimation, InputDialog.Name)
            Storyboard.SetTargetProperty(myOpacityAnimation, New PropertyPath(Border.OpacityProperty))
            Storyboard.SetTargetProperty(myHeightAnimation, New PropertyPath("RenderTransform.ScaleY"))
            Storyboard.SetTargetProperty(myWidthAnimation, New PropertyPath("RenderTransform.ScaleX"))

            AddHandler CloseStoryBoard.Completed, Sub(sender As Object, e As EventArgs)
                                                      CreateEditWindow.Visibility = Visibility.Hidden
                                                  End Sub
        End If



        'InputDialog
    End Sub


    Private Sub NameCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        VariableReset()
    End Sub
    Private Sub VariableReset()
        If CreateEditWindow.Visibility = Visibility.Visible Then
            If NameCombobox.SelectedItem IsNot Nothing Then
                If List.SelectedItem IsNot Nothing Then
                    Dim SelCodeData As StarCraftArchive.CodeData = CType(List.SelectedItem, StarCraftArchive.CodeData)



                    VarSelecter.Items.Clear()
                    Dim tCfun As New CFunc()
                    tCfun.LoadFunc(CType(CType(NameCombobox.SelectedItem, ComboBoxItem).Tag, TEFile).Scripter.GetStringText)
                    For i = 0 To tCfun.VariableCount - 1
                        Dim flag As Boolean = False
                        Dim variableType As String = tCfun.GetVariableType(i)

                        If TypeCB.SelectedIndex = 0 Then
                            flag = variableType.IndexOf("PVariable") <> -1
                        ElseIf TypeCB.SelectedIndex = 2 Then
                            flag = variableType.IndexOf("EUDArray") <> -1
                        End If

                        If flag Then
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
                        Dim flag As Boolean = False
                        Dim variableType As String = tCfun.GetVariableType(i)

                        If TypeCB.SelectedIndex = 0 Then
                            flag = variableType.IndexOf("PVariable") <> -1
                        ElseIf TypeCB.SelectedIndex = 2 Then
                            flag = variableType.IndexOf("EUDArray") <> -1
                        End If

                        If flag Then
                            VarSelecter.Items.Add(tCfun.GetVariableNames(i))
                        End If
                    Next
                    If VarSelecter.Items.Count <> 0 Then
                        VarSelecter.SelectedIndex = 0
                    End If

                End If
                Checekname()
            Else
                OkKey.IsEnabled = False
            End If


        End If
    End Sub


    Private Function CheckAlphaNumeric(str As String) As Boolean
        If str.Trim = "" Then
            Return False
        End If

        Return Text.RegularExpressions.Regex.Match(str.Trim, "^[a-zA-Z0-9]*$").Success
    End Function

    Private Sub Checekname()
        If CheckAlphaNumeric(DataName.Text) Then
            NameCondpanel.Visibility = Visibility.Collapsed

            If TypeCB.SelectedIndex <> 1 Then
                If VarSelecter.SelectedItem IsNot Nothing Then
                    If NameCombobox.SelectedItem IsNot Nothing Then
                        OkKey.IsEnabled = True
                    Else
                        OkKey.IsEnabled = False
                    End If
                Else
                    OkKey.IsEnabled = False
                End If
            Else
                OkKey.IsEnabled = True
            End If


            DataName.Foreground = Application.Current.Resources("MaterialDesignBody")
        Else
            NameCondpanel.Visibility = Visibility.Visible
            NameCond.Content = "숫자나 알파벳 문자만 가능합니다."
            OkKey.IsEnabled = False
            DataName.Foreground = Brushes.Red
        End If
    End Sub
    Private Sub DataName_TextChanged(sender As Object, e As TextChangedEventArgs)
        Checekname()
    End Sub

    Private Sub VarSelecter_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If CreateEditWindow.Visibility = Visibility.Visible Then
            If VarSelecter.SelectedItem IsNot Nothing Then
                Checekname()
            Else
                OkKey.IsEnabled = False
            End If
        End If
    End Sub
End Class

