Imports System.Windows.Media.Animation
Imports System.Windows.Threading

Class MainWindow
    Private Sub BtnRefresh()
        Dispatcher.Invoke(DispatcherPriority.Normal, New Action(Sub()
                                                                    Dim tbool As Boolean = pjData.IsLoad

                                                                    BtnClose.IsEnabled = tbool
                                                                    BtnSave.IsEnabled = tbool
                                                                    Btn_DatEdit.IsEnabled = tbool
                                                                    Btn_insert.IsEnabled = tbool
                                                                    Btn_Plugin.IsEnabled = tbool
                                                                    Btn_scmd.IsEnabled = tbool
                                                                    Btn_TriggerEdit.IsEnabled = tbool

                                                                    ProgramName.Text = Tool.GetTitleName
                                                                End Sub))

    End Sub

    Private Sub BtnDataEditor_Click(sender As Object, e As RoutedEventArgs)
        If DataEditorForm Is Nothing Then '첫 실행일 경우
            DataEditorForm = New DataEditor
            DataEditorForm.Show()
        Else
            If DataEditorForm.IsLoaded Then '열려있을경우
                DataEditorForm.Activate()
            Else '닫혀있을 경우
                DataEditorForm = New DataEditor
                DataEditorForm.Show()
            End If
        End If
    End Sub



    Private Sub BtnSetting_Click(sender As Object, e As RoutedEventArgs)
        If SettiingForm Is Nothing Then '첫 실행일 경우
            SettiingForm = New SettingWindows
            SettiingForm.Show()
        Else
            If SettiingForm.IsLoaded Then '열려있을경우
                SettiingForm.Activate()
            Else '닫혀있을 경우
                SettiingForm = New SettingWindows
                SettiingForm.Show()
            End If
        End If
    End Sub

    Private Sub ButClose_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        InitProgram()
        BtnRefresh()
    End Sub





    Private Sub ContorlPanel_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Me.DragMove()
    End Sub





    Private OldWidthPos As Double
    Private OldWidth As Double
    'Private OldPoint As Point
    Private IsDrag As Boolean
    Private Sub SizeContorl_MouseDown(sender As Object, e As MouseButtonEventArgs)
        OldWidthPos = e.GetPosition(MainControl).X
        OldWidth = Me.Width

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
                Me.Width = OldWidth + newpos
            Else
                Me.Width = ProgramName.Width + sender.Width
            End If
            Me.Height = Me.Width / 12 + 18

            '72 : 430   18

            'MainControl.Margin = New Thickness(newpos.X + MainControl.Margin.Left, newpos.Y + MainControl.Margin.Top, 0, 0)
        End If
    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        ShutDownProgram()
    End Sub

    Private Sub BtnNewFile_Click(sender As Object, e As RoutedEventArgs)
        pjData.NewFIle()
        BtnRefresh()
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs)
        pjData.CloseFile()
        BtnRefresh()
    End Sub
End Class
