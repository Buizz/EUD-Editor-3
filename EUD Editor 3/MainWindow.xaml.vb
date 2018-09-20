Imports System.Windows.Media.Animation

Class MainWindow
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim temp As New DataEditor
        temp.Show()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        InitProgram()
    End Sub





    Private OldMousePos As Point
    'Private OldPoint As Point
    Private IsDrag As Boolean
    Private Sub ContorlPanel_MouseDown(sender As Object, e As MouseButtonEventArgs)
        OldMousePos = e.GetPosition(MainControl)


        IsDrag = True
    End Sub

    Private Sub ContorlPanel_MouseUp(sender As Object, e As MouseButtonEventArgs)
        IsDrag = False
        e.MouseDevice.Capture(Nothing)
    End Sub

    Private Sub ContorlPanel_MouseMove(sender As Object, e As MouseEventArgs)
        If IsDrag Then
            e.MouseDevice.Capture(sender)
            Dim newMousePos As Point = e.GetPosition(MainControl)
            Dim newpos As Point = newMousePos - OldMousePos

            'temp.Content = newpos.X & ", " & newpos.Y


            MainControl.Margin = New Thickness(newpos.X + MainControl.Margin.Left, newpos.Y + MainControl.Margin.Top, 0, 0)
        End If
    End Sub





    Private OldWidthPos As Double
    Private OldWidth As Double
    'Private OldPoint As Point
    Private IsDrag2 As Boolean
    Private Sub SizeContorl_MouseDown(sender As Object, e As MouseButtonEventArgs)
        OldWidthPos = e.GetPosition(MainControl).X
        OldWidth = MainControl.Width

        'OldPoint = New Point(MainControl.Margin.Left, MainControl.Margin.Top)


        'temp.Content = OldPoint.X & ", " & OldPoint.Y


        IsDrag = True
    End Sub

    Private Sub SizeContorl_MouseUp(sender As Object, e As MouseButtonEventArgs)
        IsDrag = False
        e.MouseDevice.Capture(Nothing)
    End Sub

    Private Sub SizeContorl_MouseMove(sender As Object, e As MouseEventArgs)
        If IsDrag Then
            e.MouseDevice.Capture(sender)
            Dim newWidthPos As Double = e.GetPosition(MainControl).X
            Dim newpos As Double = newWidthPos - OldWidthPos

            'temp.Content = newpos.X & ", " & newpos.Y

            If OldWidth + newpos > ProgramName.Width + sender.Width Then
                MainControl.Width = OldWidth + newpos
            Else
                MainControl.Width = ProgramName.Width + sender.Width
            End If
            MainControl.Height = MainControl.Width / 12 + 18

            '72 : 430   18

            'MainControl.Margin = New Thickness(newpos.X + MainControl.Margin.Left, newpos.Y + MainControl.Margin.Top, 0, 0)
        End If
    End Sub
End Class
