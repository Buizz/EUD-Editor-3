Public Class MsgDialog
    Public msgresult As MessageBoxResult = MessageBoxResult.None


    Public Sub Init(ErrorMsg As String, ErrorLog As String, boxButton As MessageBoxButton, boxImage As MessageBoxImage)
        ErrorText.Text = ErrorMsg

        If ErrorLog <> "" Then
            LogPanel.Visibility = Visibility.Visible
            Width = 800
            Height = 400
            LogText.Text = ErrorLog
        Else
            Width = 400
            Height = 150
        End If

        Select Case boxImage
            Case MessageBoxImage.[Error]
                msgIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.[Error]
                System.Media.SystemSounds.Hand.Play()
            Case MessageBoxImage.Asterisk
                msgIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Information
                System.Media.SystemSounds.Exclamation.Play()
            Case MessageBoxImage.Warning
                msgIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Warning
                System.Media.SystemSounds.Hand.Play()
            Case MessageBoxImage.Question
                msgIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.QuestionMark
                System.Media.SystemSounds.Question.Play()
        End Select

        Select Case boxButton
            Case MessageBoxButton.OK
                If True Then
                    Dim button As Button = New Button()
                    button.Content = "확인"
                    button.Style = CType(Application.Current.Resources("MaterialDesignFlatButton"), Style)
                    AddHandler button.Click, New RoutedEventHandler(Sub(ByVal sender As Object, ByVal e As RoutedEventArgs)
                                                                        msgresult = MessageBoxResult.OK
                                                                        Close()
                                                                    End Sub)
                    BtnPanel.Children.Add(button)
                End If
            Case MessageBoxButton.OKCancel
                If True Then
                    Dim button As Button = New Button()
                    button.Content = "확인"
                    button.Style = CType(Application.Current.Resources("MaterialDesignFlatButton"), Style)
                    AddHandler button.Click, New RoutedEventHandler(Sub(ByVal sender As Object, ByVal e As RoutedEventArgs)
                                                                        msgresult = MessageBoxResult.OK
                                                                        Close()
                                                                    End Sub)
                    BtnPanel.Children.Add(button)
                End If

                If True Then
                    Dim button As Button = New Button()
                    button.Content = "취소"
                    button.Style = CType(Application.Current.Resources("MaterialDesignFlatButton"), Style)
                    AddHandler button.Click, New RoutedEventHandler(Sub(ByVal sender As Object, ByVal e As RoutedEventArgs)
                                                                        msgresult = MessageBoxResult.Cancel
                                                                        Close()
                                                                    End Sub)
                    BtnPanel.Children.Add(button)
                End If
            Case MessageBoxButton.YesNo
                If True Then
                    Dim button As Button = New Button()
                    button.Content = "예"
                    button.Style = CType(Application.Current.Resources("MaterialDesignFlatButton"), Style)
                    AddHandler button.Click, New RoutedEventHandler(Sub(ByVal sender As Object, ByVal e As RoutedEventArgs)
                                                                        msgresult = MessageBoxResult.Yes
                                                                        Close()
                                                                    End Sub)
                    BtnPanel.Children.Add(button)
                End If

                If True Then
                    Dim button As Button = New Button()
                    button.Content = "아니요"
                    button.Style = CType(Application.Current.Resources("MaterialDesignFlatButton"), Style)
                    AddHandler button.Click, New RoutedEventHandler(Sub(ByVal sender As Object, ByVal e As RoutedEventArgs)
                                                                        msgresult = MessageBoxResult.No
                                                                        Close()
                                                                    End Sub)
                    BtnPanel.Children.Add(button)
                End If
            Case MessageBoxButton.YesNoCancel
                If True Then
                    Dim button As Button = New Button()
                    button.Content = "예"
                    button.Style = CType(Application.Current.Resources("MaterialDesignFlatButton"), Style)
                    AddHandler button.Click, New RoutedEventHandler(Sub(ByVal sender As Object, ByVal e As RoutedEventArgs)
                                                                        msgresult = MessageBoxResult.Yes
                                                                        Close()
                                                                    End Sub)
                    BtnPanel.Children.Add(button)
                End If

                If True Then
                    Dim button As Button = New Button()
                    button.Content = "아니요"
                    button.Style = CType(Application.Current.Resources("MaterialDesignFlatButton"), Style)
                    AddHandler button.Click, New RoutedEventHandler(Sub(ByVal sender As Object, ByVal e As RoutedEventArgs)
                                                                        msgresult = MessageBoxResult.No
                                                                        Close()
                                                                    End Sub)
                    BtnPanel.Children.Add(button)
                End If

                If True Then
                    Dim button As Button = New Button()
                    button.Content = "취소"
                    button.Style = CType(Application.Current.Resources("MaterialDesignFlatButton"), Style)
                    AddHandler button.Click, New RoutedEventHandler(Sub(ByVal sender As Object, ByVal e As RoutedEventArgs)
                                                                        msgresult = MessageBoxResult.Cancel
                                                                        Close()
                                                                    End Sub)
                    BtnPanel.Children.Add(button)
                End If
        End Select
    End Sub

    Public Sub New(ErrorMsg As String, ErrorLog As String, boxButton As MessageBoxButton, boxImage As MessageBoxImage)
        InitializeComponent()

        Init(ErrorMsg, ErrorLog, boxButton, boxImage)
    End Sub
End Class
