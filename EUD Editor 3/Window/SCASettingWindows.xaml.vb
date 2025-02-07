Imports Microsoft.DwayneNeed.Win32.User32

Public Class SCASettingWindows
    Public IsOkay As Boolean = False
    Public SelectSampleMap As String

    Public Result As Boolean = False

    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        LoginPage.Visibility = Visibility.Visible
        LoginAlert.Visibility = Visibility.Collapsed

        AlertText.Text = Tool.GetLanText("SCADoubleLoginAlert").Replace("\n", System.Environment.NewLine)
    End Sub


    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        '샘플 맵을 읽어온다.

    End Sub

    Private Sub TextBox_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Then
            Login()
        End If
    End Sub

    Private Sub SignUp_Click(sender As Object, e As RoutedEventArgs)
        Process.Start("https://scarchive.kr/accounts/signup.php")
    End Sub

    Private Sub LoginProblem_Click(sender As Object, e As RoutedEventArgs)
        Process.Start("https://scarchive.kr/accounts/troubleshooter.php")
    End Sub

    Private hash As String
    Private Sub LoginBtn_Click(sender As Object, e As RoutedEventArgs)
        Login()
    End Sub

    Private Sub Login()
        Dim email As String = EmailTextBox.Text
        Dim pw As String = PasswordBox.Password

        Dim returnval As String = HttpTool.Login(email, pw)
        Select Case returnval
            Case "BANUSER"
                ErrorTextBox.Text = "이용이 금지된 계정입니다."
                ErrorTextBox.Visibility = Visibility.Visible
                Return
            Case "NOACCOUNT"
                ErrorTextBox.Text = "이메일과 비밀번호를 확인하세요."
                ErrorTextBox.Visibility = Visibility.Visible
                Return
        End Select
        hash = returnval

        If Not pjData.TEData.SCArchive.CheckLoginHash(hash) Then
            LoginPage.Visibility = Visibility.Collapsed
            LoginAlert.Visibility = Visibility.Visible
            Return
        End If


        pjData.TEData.SCArchive.SaveLoginHash(hash)

        If AutoLogin.IsChecked Then
            pjData.TEData.SCArchive.SavePassWord(email, pw)
        End If

        '성공하면
        pjData.TEData.SCArchive.SCAEmail = EmailTextBox.Text
        pjData.TEData.SCArchive.IsLogin = True
        pjData.TEData.SCArchive.TempPassWord = pw
        Result = True

        Close()
    End Sub

    Private Sub MetroWindow_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
    End Sub

    Private Sub LoginApply_Click(sender As Object, e As RoutedEventArgs)
        Dim email As String = EmailTextBox.Text
        Dim pw As String = PasswordBox.Password

        pjData.TEData.SCArchive.MakerServerName = ""
        pjData.TEData.SCArchive.SubTitle = ""
        pjData.TEData.SCArchive.MapName = ""
        pjData.TEData.SCArchive.SaveLoginHash(hash)

        If AutoLogin.IsChecked Then
            pjData.TEData.SCArchive.SavePassWord(email, PW)
        End If

        '성공하면
        pjData.TEData.SCArchive.SCAEmail = EmailTextBox.Text
        pjData.TEData.SCArchive.IsLogin = True
        pjData.TEData.SCArchive.TempPassWord = pw
        Result = True

        Close()
    End Sub

    Private Sub LoginCancel_Click(sender As Object, e As RoutedEventArgs)
        LoginPage.Visibility = Visibility.Visible
        LoginAlert.Visibility = Visibility.Collapsed
    End Sub
End Class
