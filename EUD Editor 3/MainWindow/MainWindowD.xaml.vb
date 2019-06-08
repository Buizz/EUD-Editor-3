Imports System.Windows.Threading
Imports System.Windows.Interop
Imports System.ComponentModel

Public Class MainWindowD
    Private BackGroundWorker As BackgroundWorker

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        InitProgram()
        BackGroundWorker = New BackgroundWorker()
        AddHandler BackGroundWorker.DoWork, AddressOf BackgroundWorker1_DoWork
        AddHandler BackGroundWorker.RunWorkerCompleted, AddressOf BackgroundWorker1_RunWorkerCompleted

        BackGroundWorker.RunWorkerAsync()

        GetMainWindow()
    End Sub



    Public Sub LogTextBoxView(tLogText As String, IsErrorLogBox As Boolean)
        Dispatcher.Invoke(DispatcherPriority.Normal, New Action(Sub()
                                                                    Dim BuildLog As New BuildLogWindow(tLogText, IsErrorLogBox)
                                                                    BuildLog.ShowDialog()
                                                                End Sub))
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ProgramLoad()
    End Sub
    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        ProgramLoadCmp()
    End Sub

    Private Sub MetroWindow_Unloaded(sender As Object, e As RoutedEventArgs)

    End Sub

    Private IsProgramLoad As Boolean = False
    Private ControlBar As ProjectControl

    Private Sub ProgramLoad()
        InitProgramDatas()

        IsProgramLoad = True
        Dispatcher.Invoke(DispatcherPriority.Normal, New Action(Sub()
                                                                    LoadPanel.Visibility = Visibility.Collapsed
                                                                    Me.DataContext = ProjectControlBinding
                                                                    ControlBar = New ProjectControl
                                                                    MainGrid.Children.Add(ControlBar)
                                                                    ControlBar.HotkeyInit(Me)

                                                                    ProjectControlBinding.PropertyChangedPack()
                                                                End Sub))
    End Sub
    Private Sub ProgramLoadCmp()
        If UpdateCheck() Then
            Me.Visibility = Visibility.Hidden
            SettiingForm = New SettingWindows
            'SettiingForm.MainTab.RemoveFromSource(SettiingForm.UpdatePage)
            SettiingForm.MainTab.RemoveFromSource(SettiingForm.DefaultPage)
            SettiingForm.MainTab.RemoveFromSource(SettiingForm.ThemePage)
            SettiingForm.MainTab.RemoveFromSource(SettiingForm.EditorPage)
            SettiingForm.ShowDialog()
            Me.Visibility = Visibility.Visible
        End If
        If pgData.Setting(ProgramData.TSetting.CheckReg) Then

            If Tool.CheckexeConnect("e3s") Then
                Dim dialogResult As MsgBoxResult = MsgBox(Tool.GetText("RegistryConnect"), MsgBoxStyle.YesNoCancel)
                If dialogResult = MsgBoxResult.Yes Then
                    Me.Visibility = Visibility.Hidden
                    SettiingForm = New SettingWindows
                    SettiingForm.MainTab.RemoveFromSource(SettiingForm.UpdatePage)
                    'SettiingForm.MainTab.RemoveFromSource(SettiingForm.DefaultPage)
                    SettiingForm.MainTab.RemoveFromSource(SettiingForm.ThemePage)
                    SettiingForm.MainTab.RemoveFromSource(SettiingForm.EditorPage)
                    SettiingForm.ShowDialog()

                    Me.Visibility = Visibility.Visible
                    'Tool.StartRegSetter()
                ElseIf dialogResult = MsgBoxResult.Cancel Then
                    pgData.Setting(ProgramData.TSetting.CheckReg) = False
                End If
            End If
        End If
    End Sub




    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        If IsProgramLoad Then
            If pgData.IsCompilng Then
                Tool.ErrorMsgBox(Tool.GetText("Error compiling"))
                e.Cancel = True
            Else
                Tool.CloseOtherWindow()
                If Tool.IsProjectLoad Then
                    If pjData.CloseFile() Then
                        pjData = Nothing
                    Else
                        e.Cancel = True
                        Return
                    End If
                End If
                ShutDownProgram()
                'e.Cancel = ShutDownProgram()
            End If
        Else
            Tool.ErrorMsgBox(Tool.GetText("Error ProgramLoading"))
            e.Cancel = True
        End If
    End Sub


End Class
