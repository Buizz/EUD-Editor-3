Imports System.Net

Public Class SCASetting

    Private SCADAtas As SCADataList
    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = New SCABinding

        SCADAtas = New SCADataList

        MainDockPanel.Children.Add(SCADAtas)

    End Sub

    Private Loadcmp As Boolean = False
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        MapDetail.IsEnabled = infoCheckbox.IsChecked
        UMSPassWord.Password = pjData.TEData.SCArchive.PassWord
        Loadcmp = True
    End Sub


    Public Sub Refresh()
        SCADAtas.Refresh()
    End Sub


    Private Sub UMSPassWord_PasswordChanged(sender As Object, e As RoutedEventArgs)
        If Loadcmp Then
            pjData.TEData.SCArchive.PassWord = UMSPassWord.Password
        End If
    End Sub

    Private Sub UseSCA_Checked(sender As Object, e As RoutedEventArgs)
        If Loadcmp Then
            If MakerBattleTag.Text.Trim = "" Or
                UMSPassWord.Password.Trim = "" Or
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

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim mapCode As String = pjData.EudplibData.GetMapCode()


        If (mapCode = "") Or (pjData.TEData.SCArchive.SubTitle = "") Or
              (pjData.TEData.SCArchive.SCAEmail = "") Or (pjData.TEData.SCArchive.PassWord = "") Then
            Return
        End If
        Dim senddata As String = ""
        senddata = "mapcode=" & WebUtility.UrlEncode(mapCode) & "&"
        senddata = senddata & "subtitle=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.SubTitle) & "&"
        senddata = senddata & "bt=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.SCAEmail) & "&"
        senddata = senddata & "pw=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.PassWord)


        Process.Start("https://scarchive.kr/creator/main.php?" + senddata)
    End Sub

End Class
