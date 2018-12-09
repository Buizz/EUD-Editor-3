
Public Class SettingWindows
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        If Not Tool.IsProjectLoad Then
            MainTab.RemoveFromSource(TabItem_ProjectSetting)
            'TabItem_ProjectSetting.Height = 0
        End If

        TBStarCraftexe.Text = pgData.Setting(ProgramData.TSetting.starcraft)
        TBeuddraftexe.Text = pgData.Setting(ProgramData.TSetting.euddraft)

        If Tool.IsProjectLoad Then
            TBOpenMap.Text = pjData.OpenMapName
            TBSaveMap.Text = pjData.SaveMapName
        End If


        '언어 설정
        Dim CurrentLan As String = pgData.Setting(ProgramData.TSetting.Language)
        'MsgBox(CurrentLan)
        For i = 0 To CBLanguage.Items.Count - 1
            'MsgBox(i)
            Dim SelectItem As ComboBoxItem = CBLanguage.Items(i)
            If SelectItem.Tag = CurrentLan Then
                CBLanguage.SelectedIndex = i
                Exit For
            End If
        Next

        '밝기
        If pgData.Setting(ProgramData.TSetting.Theme) = "Dark" Then
            ToggleBtn.IsChecked = True
        Else
            ToggleBtn.IsChecked = False
        End If

        CBCodeLan.SelectedIndex = pgData.Setting(ProgramData.TSetting.CDLanuage)
    End Sub





    Private Sub BtnStarCraftexe(sender As Object, e As RoutedEventArgs)
        Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
            .Filter = "StarCraft.exe|StarCraft.exe",
            .FileName = "StarCraft.exe",
            .Title = Tool.GetText("StarExeFile Select")
        }


        If opendialog.ShowDialog() = Forms.DialogResult.OK Then
            pgData.Setting(ProgramData.TSetting.starcraft) = opendialog.FileName

            TBStarCraftexe.Text = opendialog.FileName
            scData.LoadMPQData()
        End If
    End Sub

    Private Sub Btneuddraftexe(sender As Object, e As RoutedEventArgs)
        Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
            .Filter = "euddraft.exe|euddraft.exe",
            .FileName = "euddraft.exe",
            .Title = Tool.GetText("euddraftExe Select")
        }


        If opendialog.ShowDialog() = Forms.DialogResult.OK Then
            pgData.Setting(ProgramData.TSetting.euddraft) = opendialog.FileName

            TBeuddraftexe.Text = opendialog.FileName
        End If
    End Sub



    Private Sub BtnOpenMapSet_Click(sender As Object, e As RoutedEventArgs)
        If Tool.OpenMapSet Then
            TBOpenMap.Text = pjData.OpenMapName
        End If
    End Sub

    Private Sub BtnSaveMapSet_Click(sender As Object, e As RoutedEventArgs)
        If Tool.SaveMapSet Then
            TBSaveMap.Text = pjData.SaveMapName
        End If
    End Sub



    Private Sub ToggleButton_Checked(sender As Object, e As RoutedEventArgs)
        pgData.SetTheme(False)
    End Sub

    Private Sub ToggleButton_Unchecked(sender As Object, e As RoutedEventArgs)
        pgData.SetTheme(True)
    End Sub

    Private Sub CBLanguage_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If e.AddedItems.Count <> 0 Then
            Dim SelectItem As ComboBoxItem = e.AddedItems(0)

            Dim languagename As String = SelectItem.Tag

            pgData.SetLanguage(languagename)

        End If
    End Sub

    Private Sub CBCodeLan_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If e.AddedItems.Count <> 0 Then
            Dim SelectItem As ComboBox = sender


            pgData.Setting(ProgramData.TSetting.CDLanuage) = SelectItem.SelectedIndex
        End If
    End Sub

    'Private Sub CBLanguage_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CBLanguage.SelectionChanged
    '    pgData.Lan.SetLanguage(e.AddedItems(0))
    'End Sub
End Class
