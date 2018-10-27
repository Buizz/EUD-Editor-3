Imports System.Windows.Forms

Public Class SettingWindows
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        If Not Tool.IsProjectLoad Then
            TabItem_ProjectSetting.IsEnabled = False
            MainTab.SelectedIndex = 1
        End If

        TBStarCraftexe.Text = pgData.Setting(ProgramData.TSetting.starcraft)
        TBeuddraftexe.Text = pgData.Setting(ProgramData.TSetting.euddraft)

        If Tool.IsProjectLoad Then
            TBOpenMap.Text = pjData.OpenMapName
            TBSaveMap.Text = pjData.SaveMapName
        End If


        '언어 설정
        'Dim CurrentLan As String = pgData.Lan.GetLan("Language")

        'If CurrentLan Is Nothing Then

        'Else

        'End If

        'CBLanguage.Items.Clear()
        'For Each LanNames As String In pgData.Lan.GetLanguageArray
        '    CBLanguage.Items.Add(LanNames)

        'Next
        'CBLanguage.SelectedItem = CurrentLan
    End Sub





    Private Sub BtnStarCraftexe(sender As Object, e As RoutedEventArgs)
        Dim opendialog As New OpenFileDialog With {
            .Filter = "StarCraft.exe|StarCraft.exe",
            .FileName = "StarCraft.exe",
            .Title = Tool.GetText("StarExeFile Select")
        }


        If opendialog.ShowDialog() = Forms.DialogResult.OK Then
            pgData.Setting(ProgramData.TSetting.starcraft) = opendialog.FileName

            TBStarCraftexe.Text = opendialog.FileName
        End If
    End Sub

    Private Sub Btneuddraftexe(sender As Object, e As RoutedEventArgs)
        Dim opendialog As New OpenFileDialog With {
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

    'Private Sub CBLanguage_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CBLanguage.SelectionChanged
    '    pgData.Lan.SetLanguage(e.AddedItems(0))
    'End Sub
End Class
