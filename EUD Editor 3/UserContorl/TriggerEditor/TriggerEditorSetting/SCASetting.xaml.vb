Imports System.Net

Public Class SCASetting

    Private SCADAtas As SCADataList

    Private Binding As SCABinding
    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        '바인딩 하기전에 로그인 정보를 확인
        pjData.TEData.SCArchive.CheckLoginAccount()
        If pjData.TEData.SCArchive.IsLogin Then
            LoginEmail.Text = pjData.TEData.SCArchive.SCAEmail
            SCALoginButton.Content = Tool.GetLanText("SCALogout")
            MapInfor.IsEnabled = True
        Else
            SCALoginButton.Content = Tool.GetLanText("SCALogin")
            MapInfor.IsEnabled = False
        End If

        Binding = New SCABinding
        DataContext = Binding

        SCADAtas = New SCADataList

        MainDockPanel.Children.Add(SCADAtas)

    End Sub

    Private Loadcmp As Boolean = False
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        MapDetail.IsEnabled = infoCheckbox.IsChecked
        'UMSPassWord.Password = pjData.TEData.SCArchive.PassWord
        Loadcmp = True
    End Sub


    Public Sub Refresh()
        SCADAtas.Refresh()
    End Sub


    Private Sub UMSPassWord_PasswordChanged(sender As Object, e As RoutedEventArgs)
        If Loadcmp Then
            'pjData.TEData.SCArchive.PassWord = UMSPassWord.Password
        End If
    End Sub

    Private Sub UseSCA_Checked(sender As Object, e As RoutedEventArgs)
        If Loadcmp Then
            '로그인 성공했는지 여부
            If pjData.TEData.SCArchive.IsLogin = False Or
                MakerID.Text.Trim = "" Or
                subtitle.Text.Trim = "" Or
                UseMapName.Text.Trim = "" Then

                'If infoCheckbox.IsChecked Then
                '    If MapTags.Text.Trim = "" Or
                '        MakerEmail.Text.Trim = "" Or
                '        Maptitle.Text.Trim = "" Or
                '        MapLink.Text.Trim = "" Or
                '        ImageLink.Text.Trim = "" Or
                '        Mapdes.Text.Trim = "" Then
                '        UseSCA.IsChecked = False
                '        Warring.Content = "맵 정보를 모두 입력 하세요."
                '    End If
                'End If
                UseSCA.IsChecked = False
                Warring.Content = "맵 정보를 모두 입력 하세요."
            Else
                Warring.Content = ""
            End If
        End If
    End Sub
    Private Sub UseSCA_Unchecked(sender As Object, e As RoutedEventArgs)
        'MapInfor.IsEnabled = False
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

    Private Sub SCAButton_Click(sender As Object, e As RoutedEventArgs)
        Dim mapCode As String = pjData.EudplibData.GetMapCode()


        If (mapCode = "") Or (pjData.TEData.SCArchive.SubTitle = "") Or
              (pjData.TEData.SCArchive.SCAEmail = "") Or (pjData.TEData.SCArchive.GetPassWord(pjData.TEData.SCArchive.SCAEmail) = "") Then
            Return
        End If
        Dim senddata As String = ""
        senddata = "mapcode=" & WebUtility.UrlEncode(mapCode) & "&"
        senddata = senddata & "subtitle=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.SubTitle) & "&"
        senddata = senddata & "bt=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.SCAEmail) & "&"
        senddata = senddata & "pw=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.GetPassWord(pjData.TEData.SCArchive.SCAEmail))


        Process.Start("https://scarchive.kr/creator/main.php?" + senddata)
    End Sub

    Private Sub SCALoginButton_Click(sender As Object, e As RoutedEventArgs)
        If pjData.TEData.SCArchive.IsLogin Then
            '로그인 되어있으면 로그아웃버튼
            SCALoginButton.Content = Tool.GetLanText("SCALogin")
            MapInfor.IsEnabled = False

            LoginEmail.Text = ""

            UseSCA.IsChecked = False
            pjData.TEData.SCArchive.IsLogin = False
        Else
            Dim scasetting As New SCASettingWindows

            scasetting.ShowDialog()

            If scasetting.Result Then
                '리턴값이 있으면 로그인 성공
                SCALoginButton.Content = Tool.GetLanText("SCALogout")
                MapInfor.IsEnabled = True

                LoginEmail.Text = pjData.TEData.SCArchive.SCAEmail



                Binding.LoginRefresh()
            End If
        End If


    End Sub


End Class
