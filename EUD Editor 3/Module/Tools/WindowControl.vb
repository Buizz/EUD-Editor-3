Module WindowControl
    Public Sub CloseToolWindow()
        '다른 윈도우들이 하나라도 남아 있는지 판단

        Dim flag As Boolean = False
        Dim MainWindow As Window = Nothing
        For Each win As Window In Application.Current.Windows
            If win.GetType Is GetType(DataEditor) Or win.GetType Is GetType(TriggerEditor) Or win.GetType Is GetType(PluginWindow) Then
                flag = True '다른 윈도우가 남아있음
            ElseIf win.GetType Is GetType(MainWindowD) Then
                MainWindow = win
            End If
        Next
        If Not flag Then '다른 윈도우가 없음
            MainWindow.Visibility = Visibility.Visible
        End If

    End Sub
    Public Sub OpenToolWindow()
        '메인 윈도우 숨기기

        For Each win As Window In Application.Current.Windows
            If win.GetType Is GetType(MainWindowD) Then
                win.Visibility = Visibility.Hidden
            End If
        Next
    End Sub




End Module
Namespace WindowMenu
    Public Module WindowMenus
        Public Sub MenuItemSave()
            pjData.Save()
            ProjectControlBinding.PropertyChangedPack()
        End Sub
        Public Sub MenuItemSaveAs()
            pjData.Save(True)
            ProjectControlBinding.PropertyChangedPack()
        End Sub

        Public Sub NewFile()
            Tool.CloseOtherWindow()
            ProjectData.Load(True, pjData)
            ProjectControlBinding.PropertyChangedPack()
        End Sub

        Public Sub Setting()
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
            ProjectControlBinding.PropertyChangedPack()
        End Sub

        Public Sub Close()

            If pjData.CloseFile() Then
                pjData = Nothing
            End If

            ProjectControlBinding.PropertyChangedPack()
        End Sub

        Public Sub Save()
            pjData.Save()
            ProjectControlBinding.PropertyChangedPack()
        End Sub

        Public Sub Load()

            ProjectData.Load(False, pjData)
            ProjectControlBinding.PropertyChangedPack()
        End Sub

        Public Sub ScmdOpen()
            ProjectControlBinding.PropertyChangedPack()

            If My.Computer.FileSystem.FileExists(pjData.OpenMapName) Then
                Process.Start(pjData.OpenMapName)
                Exit Sub
            Else
                If MsgBox(Tool.GetText("Error OpenMap is not exist reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                    If Tool.OpenMapSet Then
                        ProjectControlBinding.PropertyChangedPack()
                        Process.Start(pjData.OpenMapName)
                        Exit Sub
                    End If
                End If
            End If
            Tool.ErrorMsgBox(Tool.GetText("Error OpenMap is not exist!"))
        End Sub

        Public Sub insert()
            ProjectControlBinding.PropertyChangedPack()

            pjData.EudplibData.Build()
        End Sub


        Public Sub OpenDataEditor()
            ProjectControlBinding.PropertyChangedPack()

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
                        ProjectControlBinding.PropertyChangedPack()
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
            Dim DataEditorForm As New DataEditor(DataEditor.OpenType.MainWindow)
            DataEditorForm.Show()
            OpenToolWindow()
        End Sub

        Public Sub OpenTriggerEdit()
            ProjectControlBinding.PropertyChangedPack()

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
                        ProjectControlBinding.PropertyChangedPack()
                    Else
                        Tool.ErrorMsgBox(Tool.GetText("Error FailLoad TriggerEditor"))
                        Exit Sub
                    End If
                Else
                    Tool.ErrorMsgBox(Tool.GetText("Error FailLoad TriggerEditor"))
                    Exit Sub
                End If
            End If


            Dim flag As Boolean = False
            For Each win As Window In Application.Current.Windows
                If win.GetType Is GetType(TriggerEditor) Then
                    win.Activate()

                    flag = True
                    Exit For
                End If
            Next
            If Not flag Then
                Dim TriggerEditorForm As New TriggerEditor
                TriggerEditorForm.Show()
                OpenToolWindow()
            End If
        End Sub

        Public Sub OpenPlugin()
            ProjectControlBinding.PropertyChangedPack()

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
                        ProjectControlBinding.PropertyChangedPack()
                    Else
                        Tool.ErrorMsgBox(Tool.GetText("Error FailLoad PluginSet"))
                        Exit Sub
                    End If
                Else
                    Tool.ErrorMsgBox(Tool.GetText("Error FailLoad PluginSet"))
                    Exit Sub
                End If
            End If

            Dim flag As Boolean = False
            For Each win As Window In Application.Current.Windows
                If win.GetType Is GetType(PluginWindow) Then
                    win.Activate()

                    flag = True
                    Exit For
                End If
            Next
            If Not flag Then
                'pjData.SetDirty(True)
                Dim PluginForm As New PluginWindow
                PluginForm.Show()
                OpenToolWindow()
            End If
        End Sub

        Public Sub CodeFold()
            For Each win As Window In Application.Current.Windows
                If win.GetType Is GetType(DataEditor) Then
                    If win.IsActive Then
                        CType(win, DataEditor).CodeViewFold()
                    End If
                End If
            Next
        End Sub
    End Module
End Namespace