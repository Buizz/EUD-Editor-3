Imports System.Windows.Media.Animation
Imports System.Windows.Threading

Class MainWindow
    Public Sub BtnRefresh()
        Dispatcher.Invoke(DispatcherPriority.Normal, New Action(Sub()
                                                                    Dim tbool As Boolean = Tool.IsProjectLoad


                                                                    BtnClose.IsEnabled = tbool
                                                                    BtnSave.IsEnabled = tbool
                                                                    Btn_DatEdit.IsEnabled = tbool
                                                                    Btn_Plugin.IsEnabled = tbool
                                                                    Btn_insert.IsEnabled = tbool
                                                                    Btn_scmd.IsEnabled = tbool
                                                                    Btn_TriggerEdit.IsEnabled = tbool

                                                                    ProgramName.Text = Tool.GetTitleName

                                                                    If tbool Then
                                                                        If My.Computer.FileSystem.FileExists(pjData.OpenMapName) Then
                                                                            Btn_scmd.Background = Brushes.White
                                                                        Else
                                                                            Btn_scmd.Background = Brushes.Pink
                                                                        End If
                                                                        If My.Computer.FileSystem.FileExists(pjData.OpenMapName) And
                                                                        My.Computer.FileSystem.FileExists(pjData.SaveMapName) And
                                                                         My.Computer.FileSystem.FileExists(pgData.Setting(ProgramData.TSetting.euddraft)) Then
                                                                            Btn_insert.Background = Brushes.White
                                                                        Else
                                                                            Btn_insert.Background = Brushes.Pink
                                                                        End If

                                                                        If scData.LoadStarCraftData Then
                                                                            Btn_DatEdit.Background = Brushes.White
                                                                            Btn_Plugin.Background = Brushes.White
                                                                            Btn_TriggerEdit.Background = Brushes.White
                                                                        Else
                                                                            Btn_DatEdit.Background = Brushes.Pink
                                                                            Btn_Plugin.Background = Brushes.Pink
                                                                            Btn_TriggerEdit.Background = Brushes.Pink
                                                                        End If
                                                                    End If
                                                                End Sub))

    End Sub




    Private Sub BtnSetting_Click(sender As Object, e As RoutedEventArgs)
        If SettiingForm Is Nothing Then '첫 실행일 경우
            SettiingForm = New SettingWindows
            SettiingForm.ShowDialog()
        Else
            If SettiingForm.IsLoaded Then '열려있을경우
                SettiingForm.Activate()
            Else '닫혀있을 경우
                SettiingForm = New SettingWindows
                SettiingForm.ShowDialog()
            End If
        End If
        BtnRefresh()
    End Sub

    Private Sub ButClose_Click(sender As Object, e As RoutedEventArgs)
        Tool.CloseWindows()
        If Tool.IsProjectLoad Then
            If pjData.CloseFile() Then
                pjData = Nothing
                Me.Close()
                Exit Sub
            Else
                Exit Sub
            End If
        End If

        Me.Close()
        BtnRefresh()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        If InitProgram() Then
            BtnRefresh()
        End If
        'MsgBox(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString())
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

            If OldWidth + newpos > 200 + sender.Width Then
                Me.Width = OldWidth + newpos
            Else
                Me.Width = 200 + sender.Width
            End If
            Me.Height = Me.Width / 12 + 18

            '72 : 430   18

            'MainControl.Margin = New Thickness(newpos.X + MainControl.Margin.Left, newpos.Y + MainControl.Margin.Top, 0, 0)
        End If
    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        Tool.CloseWindows()
        e.Cancel = ShutDownProgram()
    End Sub

    Private Sub BtnNewFile_Click(sender As Object, e As RoutedEventArgs)
        Tool.CloseWindows()
        ProjectData.Load(True, pjData)
        BtnRefresh()
    End Sub

    Private Sub Window_Activated(sender As Object, e As EventArgs)
        'For Each win As Window In Application.Current.Windows
        '    If win.GetType IsNot GetType(MainWindow) Then
        '        win.Activate()
        '    End If
        'Next
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs)

        If pjData.CloseFile() Then
            pjData = Nothing
        End If

        BtnRefresh()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As RoutedEventArgs)
        pjData.Save()
        BtnRefresh()
    End Sub

    Private Sub BtnLoad_Click(sender As Object, e As RoutedEventArgs)

        ProjectData.Load(False, pjData)
        BtnRefresh()
    End Sub

    Private Sub Btn_scmd_Click(sender As Object, e As RoutedEventArgs)
        If My.Computer.FileSystem.FileExists(pjData.OpenMapName) Then
            Process.Start(pjData.OpenMapName)
            Exit Sub
        Else
            If MsgBox(Tool.GetText("Error OpenMap is not exist reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                If Tool.OpenMapSet Then
                    BtnRefresh()
                    Process.Start(pjData.OpenMapName)
                    Exit Sub
                End If
            End If
        End If
        Tool.ErrorMsgBox(Tool.GetText("Error OpenMap is not exist!"))
    End Sub

    Private Sub Btn_insert_Click(sender As Object, e As RoutedEventArgs)
        If Not My.Computer.FileSystem.FileExists(pjData.OpenMapName) Then
            If MsgBox(Tool.GetText("Error OpenMap is not exist reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                If Not Tool.OpenMapSet Then
                    Tool.ErrorMsgBox(Tool.GetText("Error complieFail OpenMap is not exist!"))
                    Exit Sub
                End If
                BtnRefresh()
            Else
                Tool.ErrorMsgBox(Tool.GetText("Error complieFail OpenMap is not exist!"))
                Exit Sub
            End If
        End If
        If Not My.Computer.FileSystem.FileExists(pjData.SaveMapName) Then
            If MsgBox(Tool.GetText("Error SaveMap is not exist reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                If Not Tool.SaveMapSet Then
                    Tool.ErrorMsgBox(Tool.GetText("Error complieFail SaveMap is not exist!"))
                    Exit Sub
                End If
                BtnRefresh()
            Else
                Tool.ErrorMsgBox(Tool.GetText("Error complieFail SaveMap is not exist!"))
                Exit Sub
            End If
        End If
        If Not My.Computer.FileSystem.FileExists(pgData.Setting(ProgramData.TSetting.euddraft)) Then
            If MsgBox(Tool.GetText("Error euddraft is not exist reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
                .Filter = "euddraft.exe|euddraft.exe",
                .FileName = "euddraft.exe",
                .Title = Tool.GetText("euddraftExe Select")
            }


                If opendialog.ShowDialog() = Forms.DialogResult.OK Then
                    pgData.Setting(ProgramData.TSetting.euddraft) = opendialog.FileName
                    BtnRefresh()
                Else
                    Tool.ErrorMsgBox(Tool.GetText("Error complieFail euddraft is not exist!"))
                    Exit Sub
                End If
            Else
                Tool.ErrorMsgBox(Tool.GetText("Error complieFail euddraft is not exist!"))
                Exit Sub
            End If
        End If

        MsgBox("삽입완료")
    End Sub


    Private Sub BtnDataEditor_Click(sender As Object, e As RoutedEventArgs)
        If Not scData.LoadStarCraftData Then '로드가 되어있지 않을 경우 판단
            If MsgBox(Tool.GetText("Error NotExistMPQ reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
                .Filter = "StarCraft.exe|StarCraft.exe",
                .FileName = "StarCraft.exe",
                .Title = Tool.GetText("StarExeFile Select")
                }


                If opendialog.ShowDialog() = Forms.DialogResult.OK Then
                    pgData.Setting(ProgramData.TSetting.starcraft) = opendialog.FileName
                    scData.LoadMPQData()
                    BtnRefresh()
                Else
                    Tool.ErrorMsgBox(Tool.GetText("Error FailLoad DataEditor"))
                    Exit Sub
                End If
            Else
                Tool.ErrorMsgBox(Tool.GetText("Error FailLoad DataEditor"))
                Exit Sub
            End If
        End If


        'pjData.SetDirty(True)
        Dim DataEditorForm As New DataEditor
        DataEditorForm.Show()
        DataEditorForm.OpenbyMainWindow()
    End Sub

    Private Sub Btn_TriggerEdit_Click(sender As Object, e As RoutedEventArgs)
        If Not scData.LoadStarCraftData Then '로드가 되어있지 않을 경우 판단
            If MsgBox(Tool.GetText("Error NotExistMPQ reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
                .Filter = "StarCraft.exe|StarCraft.exe",
                .FileName = "StarCraft.exe",
                .Title = Tool.GetText("StarExeFile Select")
                }


                If opendialog.ShowDialog() = Forms.DialogResult.OK Then
                    pgData.Setting(ProgramData.TSetting.starcraft) = opendialog.FileName
                    scData.LoadMPQData()
                    BtnRefresh()
                Else
                    Tool.ErrorMsgBox(Tool.GetText("Error FailLoad TriggerEditor"))
                    Exit Sub
                End If
            Else
                Tool.ErrorMsgBox(Tool.GetText("Error FailLoad TriggerEditor"))
                Exit Sub
            End If
        End If


        'pjData.SetDirty(True)
        Dim TriggerEditorForm As New TriggerEditor
        TriggerEditorForm.Show()
        'DataEditorForm.OpenbyMainWindow()
    End Sub

    Private Sub Btn_Plugin_Click(sender As Object, e As RoutedEventArgs)
        If Not scData.LoadStarCraftData Then '로드가 되어있지 않을 경우 판단
            If MsgBox(Tool.GetText("Error NotExistMPQ reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
                .Filter = "StarCraft.exe|StarCraft.exe",
                .FileName = "StarCraft.exe",
                .Title = Tool.GetText("StarExeFile Select")
                }


                If opendialog.ShowDialog() = Forms.DialogResult.OK Then
                    pgData.Setting(ProgramData.TSetting.starcraft) = opendialog.FileName
                    scData.LoadMPQData()
                    BtnRefresh()
                Else
                    Tool.ErrorMsgBox(Tool.GetText("Error FailLoad PluginSet"))
                    Exit Sub
                End If
            Else
                Tool.ErrorMsgBox(Tool.GetText("Error FailLoad PluginSet"))
                Exit Sub
            End If
        End If


        'pjData.SetDirty(True)
        Dim PluginForm As New PluginWindow
        PluginForm.Show()
        'DataEditorForm.OpenbyMainWindow()
    End Sub

    Private Sub MenuItemSave_Click(sender As Object, e As RoutedEventArgs)
        pjData.Save()
        BtnRefresh()
    End Sub
    Private Sub MenuItemSaveAs_Click(sender As Object, e As RoutedEventArgs)
        pjData.Save(True)
        BtnRefresh()
    End Sub
End Class
