Imports Dragablz
Imports MaterialDesignThemes.Wpf

Public Class TriggerEditor
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Me.DataContext = ProjectControlBinding

        ControlBar.HotkeyInit(Me)

        Topmost = pgData.Setting(ProgramData.TSetting.TriggerEditrTopMost)

        'Dim asdgfaqwea As Func(Of TabItem) = AddressOf Factory
        'MainTabablzControl.NewItemFactory = asdgfaqwea


        MainTabablzControl.ClosingItemCallback = AddressOf ClosingTabItemHandlerImpl

        Explorer.Init(Me)
    End Sub

    Private NoTabItemClose As Boolean = False
    Private Sub ClosingTabItemHandlerImpl(ByVal args As ItemActionCallbackArgs(Of TabablzControl))
        If MsgBox("아이템끌꺼야?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            NoTabItemClose = True
        Else
            args.Cancel()
        End If


    End Sub
    Private Sub MetroWindow_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        If NoTabItemClose Then
            NoTabItemClose = False
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Public Function Factory() As TabItem
        MsgBox("아")
        Return New TabItem
    End Function


    Public Sub OpenTabItem(tTEFile As TEFile)
        'MainTabablzControl.Items.Add()
        Dim TabContent As TabablzControl = MainTab.Content

        Select Case tTEFile.FileType
            Case TEFile.EFileType.CUIEps, TEFile.EFileType.CUIPy
                PlusTabItem(New TECUIPage, MainTab)
            Case TEFile.EFileType.CUIPy, TEFile.EFileType.GUIPy
                PlusTabItem(New TEGUIPage, MainTab)
        End Select
    End Sub

    Private Sub PlusTabItem(tTEFile As UserControl, MainTab As Dockablz.Layout)
        Dim MainContent As Object = MainTab.Content
        While MainContent.GetType <> GetType(TabablzControl)
            Select Case MainContent.GetType
                Case GetType(TabablzControl)
                    Exit While
                Case GetType(Dockablz.Branch)
                    Dim tBranch As Dockablz.Branch = MainContent
                    MainContent = tBranch.FirstItem
            End Select
        End While

        Dim TabContent As TabablzControl = MainContent

        Dim TabItem As New TabItem
        TabItem.Content = tTEFile
        TabContent.Items.Add(TabItem)
        TabContent.SelectedItem = TabItem
    End Sub


    Private Sub MetroWindow_Closed(sender As Object, e As EventArgs)
        pgData.Setting(ProgramData.TSetting.TriggerEditrTopMost) = Me.Topmost
        CloseToolWindow()
    End Sub
End Class
