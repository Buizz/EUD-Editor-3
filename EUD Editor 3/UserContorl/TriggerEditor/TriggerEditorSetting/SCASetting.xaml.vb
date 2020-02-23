Public Class SCASetting
    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = New SCABinding
        MainDockPanel.Children.Add(New SCADataList)

    End Sub

    Private Loadcmp As Boolean = False
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        MapDetail.IsEnabled = infoCheckbox.IsChecked
        Loadcmp = True
    End Sub

    Private Sub UseSCA_Checked(sender As Object, e As RoutedEventArgs)
        If Loadcmp Then
            If MakerBattleTag.Text.Trim = "" Or
                UMSPassWord.Text.Trim = "" Or
                MakerID.Text.Trim = "" Or
                subtitle.Text.Trim = "" Or
                UseMapName.Text.Trim = "" Then

                If infoCheckbox.IsChecked Then
                    If MapTags.Text.Trim = "" Or
                        MakerEmail.Text.Trim = "" Or
                        Maptitle.Text.Trim = "" Or
                        MapLink.Text.Trim = "" Or
                        ImageLink.Text.Trim = "" Or
                        Mapdes.Text.Trim = "" Then
                        UseSCA.IsChecked = False
                        Warring.Content = "맵 정보를 모두 입력 하세요."
                        mapSettingex.IsExpanded = True
                    End If
                End If

                UseSCA.IsChecked = False
                Warring.Content = "맵 정보를 모두 입력 하세요."
                mapSettingex.IsExpanded = True
            Else
                Warring.Content = ""
            End If
        End If
    End Sub

    Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
        If Loadcmp = True Then
            MapDetail.IsEnabled = infoCheckbox.IsChecked
        End If
    End Sub

    Private Sub infoCheckbox_Unchecked(sender As Object, e As RoutedEventArgs)
        If Loadcmp = True Then
            MapDetail.IsEnabled = infoCheckbox.IsChecked
        End If
    End Sub
End Class
