Imports System.Windows.Threading

Public Class CodeEditor
    Public Property Text As String
        Get
            Return TextEditor.Text
        End Get
        Set(value As String)
            TextEditor.Text = value
        End Set
    End Property

    Public Event TextChange As RoutedEventHandler


    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        InitTextEditor()
    End Sub

    Private Sub TextEditor_PreviewMouseDown(sender As Object, e As MouseButtonEventArgs)
        TooltipHide()
    End Sub

    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        foldingUpdateTimer.Stop()
        PopupToolTip.Close()
    End Sub

    Private sMessageQueue As MaterialDesignThemes.Wpf.SnackbarMessageQueue

    Private timer As DispatcherTimer
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        PopupToolTip = New PopupToolTip
        PopupToolTip.Topmost = True
        PopupToolTip.Show()

        PopupToolTip.Visibility = Visibility.Hidden

        sMessageQueue = New MaterialDesignThemes.Wpf.SnackbarMessageQueue
        ErrorSnackbar.MessageQueue = sMessageQueue


        timer = New DispatcherTimer()
        timer.Interval = TimeSpan.FromMilliseconds(100)
        AddHandler timer.Tick, AddressOf timer_Tick
    End Sub

    Private Sub UserControl_LostFocus(sender As Object, e As RoutedEventArgs)
        'TooltipHide()
    End Sub

    Private Sub TextEditor_TextChanged(sender As Object, e As EventArgs)
        RaiseEvent TextChange(Me, Nothing)
    End Sub


    Private time As Integer
    Private Sub timer_Tick(sender As Object, e As EventArgs)
        time += 1
        If time > 10 Then
            ErrorSnackbar.IsActive = False
            timer.Stop()
        End If
    End Sub




    Private Sub TextEditor_MouseWheel(sender As Object, e As MouseWheelEventArgs)
        If isctrl Then
            If e.Delta > 0 Then
                TextEditor.FontSize += 1
            ElseIf e.Delta < 0 Then
                If TextEditor.FontSize > 1 Then
                    TextEditor.FontSize -= 1
                End If
            End If
            'sMessageQueue.Enqueue("FontSize : " & TextEditor.FontSize & "px (기본 14px)")

            ErrorSnackbar.IsActive = True
            SnackbarContent.Content = "FontSize : " & TextEditor.FontSize & "px (기본 16px)"
            time = 0
            timer.Start()
        End If
    End Sub




    Private isctrl As Boolean
    Private Sub UserControl_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.LeftCtrl Then
            isctrl = True
        End If
    End Sub

    Private Sub UserControl_PreviewKeyUp(sender As Object, e As KeyEventArgs)
        If e.Key = Key.LeftCtrl Then
            isctrl = False
        End If
    End Sub
End Class
