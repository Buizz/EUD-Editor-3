Imports System.Windows.Threading
Imports System.Windows.Interop
Imports System.ComponentModel
Imports Microsoft.WindowsAPICodePack
Imports net.r_eg.Conari.Extension
Imports System.Runtime.InteropServices
Imports System.IO

Public Class MainWindowD
    Private BackGroundWorker As BackgroundWorker
    Private UpdateChecker As BackgroundWorker

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
    Public Sub ErrorHandleStart(BuildLog As String, ErrorLog As String)
        Dispatcher.Invoke(DispatcherPriority.Normal, New Action(Sub()
                                                                    ErrorHandle(BuildLog, ErrorLog)
                                                                End Sub))
    End Sub
    Public Sub ErrorHandleCloseStart()
        Dispatcher.Invoke(DispatcherPriority.Normal, New Action(Sub()
                                                                    ErrorHandleClose()
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


    Private UpdataCheckb As Boolean = False
    Private Sub UpdateChecker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        UpdataCheckb = UpdateCheck()
    End Sub
    Private Sub UpdateChecker_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        If UpdataCheckb Then
            Me.Visibility = Visibility.Hidden
            SettiingForm = New SettingWindows
            'SettiingForm.MainTab.RemoveFromSource(SettiingForm.UpdatePage)
            SettiingForm.MainTab.Items.Remove(SettiingForm.DefaultPage)
            SettiingForm.MainTab.Items.Remove(SettiingForm.ThemePage)
            SettiingForm.MainTab.Items.Remove(SettiingForm.EditorPage)
            SettiingForm.MainTab.Items.Remove(SettiingForm.Donate)
            SettiingForm.ShowDialog()
            Me.Visibility = Visibility.Visible
        End If
    End Sub

    Private Sub ProgramLoadCmp()

        UpdateChecker = New BackgroundWorker()
        AddHandler UpdateChecker.DoWork, AddressOf UpdateChecker_DoWork
        AddHandler UpdateChecker.RunWorkerCompleted, AddressOf UpdateChecker_RunWorkerCompleted
        UpdateChecker.RunWorkerAsync()



        If pgData.Setting(ProgramData.TSetting.CheckReg) Then

            If Tool.CheckexeConnect("e3s") Then
                Dim dialogResult As MsgBoxResult = Tool.CustomMsgBox(Tool.GetText("RegistryConnect"), MessageBoxButton.YesNoCancel)
                If dialogResult = MsgBoxResult.Yes Then
                    Me.Visibility = Visibility.Hidden
                    SettiingForm = New SettingWindows
                    SettiingForm.MainTab.Items.Remove(SettiingForm.TabItem_ProjectSetting)
                    SettiingForm.MainTab.Items.Remove(SettiingForm.UpdatePage)
                    'SettiingForm.MainTab.RemoveFromSource(SettiingForm.DefaultPage)
                    SettiingForm.MainTab.Items.Remove(SettiingForm.ThemePage)
                    SettiingForm.MainTab.Items.Remove(SettiingForm.EditorPage)
                    SettiingForm.MainTab.Items.Remove(SettiingForm.Donate)
                    SettiingForm.ShowDialog()

                    Me.Visibility = Visibility.Visible
                    'Tool.StartRegSetter()
                ElseIf dialogResult = MsgBoxResult.Cancel Then
                    pgData.Setting(ProgramData.TSetting.CheckReg) = False
                End If
            End If
        End If


        If pgData.Setting(ProgramData.TSetting.DonateMsg) = False Then
            Me.Visibility = Visibility.Hidden
            SettiingForm = New SettingWindows
            SettiingForm.MainTab.Items.Remove(SettiingForm.TabItem_ProjectSetting)
            SettiingForm.MainTab.Items.Remove(SettiingForm.UpdatePage)
            SettiingForm.MainTab.Items.Remove(SettiingForm.DefaultPage)
            SettiingForm.MainTab.Items.Remove(SettiingForm.ThemePage)
            SettiingForm.MainTab.Items.Remove(SettiingForm.EditorPage)
            SettiingForm.Title = Tool.GetText("Donate")
            SettiingForm.MinWidth = 0
            SettiingForm.MinHeight = 0
            SettiingForm.Width = 300
            SettiingForm.Height = 200
            SettiingForm.ShowDialog()
            Me.Visibility = Visibility.Visible
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
            If Tool.CustomMsgBox(Tool.GetText("Error ProgramLoading"), MessageBoxButton.YesNo) = MsgBoxResult.No Then
                e.Cancel = True
            End If
        End If
    End Sub

End Class
