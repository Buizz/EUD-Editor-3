Imports System.Text
Imports System.Windows.Threading

Public Class BuildLogWindow

    Private LogText As String
    Private IsErrorLogBox As Boolean
    Public Sub New(tLogText As String, tIsErrorLogBox As Boolean)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.


        LogText = tLogText
        IsErrorLogBox = tIsErrorLogBox
    End Sub


    Private tCloseTimer As DispatcherTimer
    Private Sub MetroWindow_Loaded(sender As Object, e As RoutedEventArgs)
        If IsErrorLogBox Then
            Title = Tool.GetText("CompileFail")
        Else
            Title = Tool.GetText("CompileSucesse")
        End If

        LogTextBox.Text = LogText

        tCloseTimer = New DispatcherTimer()
        tCloseTimer.Interval = TimeSpan.FromMilliseconds(10)
        AddHandler tCloseTimer.Tick, AddressOf CloseTimer
        tCloseTimer.Start()
    End Sub


    Private SecTime As Integer
    Private Sub CloseTimer()
        If LogTextBox.IsKeyboardFocused Then
            Me.Opacity = 1
            SecTime = 0
        Else
            '3초가 지나면 시작
            If SecTime > 200 Then
                Me.Opacity = 1 - (SecTime - 200) / 200
                If SecTime > 400 Then
                    Me.Close()
                End If
            End If

            SecTime += 1
        End If
    End Sub

    Private Sub MetroWindow_Unloaded(sender As Object, e As RoutedEventArgs)
        tCloseTimer.Stop()
        tCloseTimer = Nothing
    End Sub
End Class
