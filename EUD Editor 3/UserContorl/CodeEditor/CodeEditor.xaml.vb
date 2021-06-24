Imports System.Text.RegularExpressions
Imports System.Windows.Threading
Imports MaterialDesignThemes.Wpf

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


    Public Sub LineHightLight(Line As Integer)
        If Line = -1 Then
            Return
        End If

        Dim t As String = Text



        Dim sStart As Integer
        Dim sEnd As Integer
        Dim sLen As Integer

        Dim linestart As Integer = 0
        For i = 1 To Line
            sStart = linestart
            linestart = t.IndexOf(vbCrLf, linestart)
            sEnd = linestart
            linestart += 1
        Next
        If sEnd = -1 Then
            sLen = t.Length - sStart
        Else
            sLen = sEnd - sStart + 1
        End If


        TextEditor.SelectionStart = sStart + 1
        TextEditor.SelectionLength = sLen - 1


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

        TextEditor.FontSize = pgData.Setting(ProgramData.TSetting.TEFontSize)
    End Sub
    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)
        SPanel.Open()
        SearchBoxOpen()
    End Sub
    Private Sub UserControl_LostFocus(sender As Object, e As RoutedEventArgs)
        'TooltipHide()
    End Sub

    Private Sub TextEditor_TextChanged(sender As Object, e As EventArgs)
        pjData.SetDirty(True)
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
        If TextEditor.IsReadOnly Then
            Return
        End If
        If e.Key = Key.LeftCtrl Then
            isctrl = True
        End If
        If e.Key = Key.F And isctrl Then
            SPanel.Open()
            SearchBoxOpen()
            isctrl = False

        End If
        If e.Key = Key.T And isctrl Then
            TextEditorOpen()
            isctrl = False
        End If
        If e.Key = Key.U And isctrl Then
            GUIEditorOpen()
            isctrl = False
        End If
        If e.Key = Key.S And isctrl Then
            'pjData.Save()
            isctrl = False
        End If
    End Sub


    Private Sub UserControl_PreviewKeyUp(sender As Object, e As KeyEventArgs)
        If e.Key = Key.LeftCtrl Then
            isctrl = False
        End If
    End Sub

    Public Sub SearchBoxOpen()
        SPanel.Visibility = Visibility.Hidden
        TextSearchBox.Visibility = Visibility.Visible
        TextSearchBox.UpdateLayout()
        FindText.Focus()
    End Sub

    Private Sub SearchCloseBtn_Click(sender As Object, e As RoutedEventArgs) Handles SearchCloseBtn.Click
        SPanel.Close()
        TextSearchBox.Visibility = Visibility.Collapsed
    End Sub
    Private Sub FindBtn_Click(sender As Object, e As RoutedEventArgs) Handles FindBtn.Click
        SPanel.FindNext()
    End Sub
    Private Sub ReplaceBtn_Click(sender As Object, e As RoutedEventArgs) Handles ReplaceBtn.Click
        TextEditor.SelectedText = ReplaceText.Text
        SPanel.FindNext()
    End Sub

    Private Sub FindText_TextChanged(sender As Object, e As TextChangedEventArgs)
        SPanel.SearchPattern = FindText.Text
        pjData.SetDirty(True)
    End Sub

    Private Sub FindText_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Then
            SPanel.FindNext()
        End If
    End Sub

    Private Sub ReplaceText_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Then
            TextEditor.SelectedText = ReplaceText.Text
            SPanel.FindNext()
        End If
    End Sub

    Private Sub ReplaceAllBtn_Click(sender As Object, e As RoutedEventArgs) Handles ReplaceAllBtn.Click
        TextEditor.Text = TextEditor.Text.Replace(FindText.Text, ReplaceText.Text)
    End Sub

    Private Sub TextEditor_Click(sender As Object, e As RoutedEventArgs)
        TextEditorOpen()
    End Sub

    Private Sub TextEditorOpen()
        TextEditor.IsEnabled = False
        Dim TEditor As New TextEditorWindow(TextEditor.SelectedText)

        TEditor.ShowDialog()
        TextEditor.SelectedText = TEditor.TextString
        TextEditor.IsEnabled = True
    End Sub

    Private Sub ContextMenu_Opened(sender As Object, e As RoutedEventArgs)
        If TextEditor.IsReadOnly Then
            TExtEditorMenuBtn.IsEnabled = False
        End If
    End Sub

    Private Sub GUIEditor_Click(sender As Object, e As RoutedEventArgs)
        GUIEditorOpen()
    End Sub


    Private Sub GUIEditorOpen()
        TextEditor.IsEnabled = False
        'Dim TEditor As New TextEditorWindow(TextEditor.SelectedText)

        'TEditor.ShowDialog()
        'TextEditor.SelectedText = TEditor.TextString
        TextEditor.IsEnabled = True
    End Sub
End Class
