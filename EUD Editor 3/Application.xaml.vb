Class Application

    ' Startup, Exit 및 DispatcherUnhandledException 같은 응용 프로그램 수준 이벤트는
    ' 이 파일에서 처리할 수 있습니다.

    Private Sub Application_DispatcherUnhandledException(ByVal sender As Object, ByVal e As System.Windows.Threading.DispatcherUnhandledExceptionEventArgs)
        Dim ExceptionDialog As New ExceptionErrorDialog(e.Exception)
        ExceptionDialog.ShowDialog()
        If ExceptionDialog.IsClose Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    'Private Sub Application_Startup(ByVal sender As Object, ByVal e As StartupEventArgs)
    '    AddHandler AppDomain.CurrentDomain.UnhandledException, New UnhandledExceptionEventHandler(AddressOf CurrentDomain_UnhandledException)
    'End Sub

    'Private Sub CurrentDomain_UnhandledException(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
    '    Dim ex As Exception = TryCast(e.ExceptionObject, Exception)
    '    MessageBox.Show(ex.Message, "Uncaught Thread Exception", MessageBoxButton.OK, MessageBoxImage.[Error])
    'End Sub

End Class
