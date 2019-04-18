Imports System.Windows.Threading
Imports System.Windows.Interop


Public Class MainWindowD


    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        InitProgram()
        Thread = New Task(AddressOf ProgramLoad)
        Thread.Start()

    End Sub


    Private Sub MetroWindow_Unloaded(sender As Object, e As RoutedEventArgs)

    End Sub



    Private IsProgramLoad As Boolean = False
    Private ControlBar As ProjectControl


    Private Thread As Task

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
