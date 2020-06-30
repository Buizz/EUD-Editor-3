Imports Dragablz

Module WindowControl
    Private MainWindowW As MainWindowD
    Public Function GetMainWindow() As MainWindowD
        If MainWindowW IsNot Nothing Then
            Return MainWindowW
        End If
        For Each win As Window In Application.Current.Windows
            If win.GetType Is GetType(MainWindowD) Then
                MainWindowW = win
                Return win
            End If
        Next
        Return Nothing
    End Function


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


    Public Enum CloseType
        All
        Other
        This
    End Enum


    Public Sub TECloseAllTabITem(tTEFile As TEFile)
        '우선 모든 윈도우 돌면서 조사하자
        For Each win As Window In Application.Current.Windows
            If win.GetType Is GetType(TriggerEditor) Then
                Dim MainContent As Object = CType(win, TriggerEditor).MainTab


                If CloseCheckBranch(tTEFile, MainContent, CloseType.All) Then
                    Exit Sub
                End If



                'If CheckTabablzControl(tTEFile, MainContent) Then
                '    win.Activate()
                '    Return True
                'End If
            End If
        Next
    End Sub

    Public Sub TECloseOtherTabITem(tTEFile As TEFile)
        '우선 모든 윈도우 돌면서 조사하자
        For Each win As Window In Application.Current.Windows
            If win.GetType Is GetType(TriggerEditor) Then
                Dim MainContent As Object = CType(win, TriggerEditor).MainTab


                If CloseCheckBranch(tTEFile, MainContent, CloseType.Other) Then
                    Exit Sub
                End If



                'If CheckTabablzControl(tTEFile, MainContent) Then
                '    win.Activate()
                '    Return True
                'End If
            End If
        Next
    End Sub


    Public Sub TECloseTabITem(tTEFile As TEFile)
        '우선 모든 윈도우 돌면서 조사하자
        For Each win As Window In Application.Current.Windows
            If win.GetType Is GetType(TriggerEditor) Then
                Dim MainContent As Object = CType(win, TriggerEditor).MainTab


                If CloseCheckBranch(tTEFile, MainContent, CloseType.This) Then
                    Exit Sub
                End If



                'If CheckTabablzControl(tTEFile, MainContent) Then
                '    win.Activate()
                '    Return True
                'End If
            End If
        Next
    End Sub
    Private Function CloseCheckBranch(tTEFile As TEFile, ParentBranch As Object, CloseType As CloseType) As Boolean
        '우선 모든 윈도우 돌면서 조사하자
        While TypeOf ParentBranch IsNot TabablzControl
            Select Case ParentBranch.GetType
                Case GetType(TabablzControl)
                    Exit While
                Case GetType(Dockablz.Branch)
                    Dim tBranch As Dockablz.Branch = ParentBranch

                    If CloseCheckBranch(tTEFile, ParentBranch.FirstItem, CloseType) Then
                        Return True
                    End If
                    If CloseCheckBranch(tTEFile, ParentBranch.SecondItem, CloseType) Then
                        Return True
                    End If
                    Return False
                    Exit While
                Case GetType(Dockablz.Layout)
                    Dim tLayout As Dockablz.Layout = ParentBranch
                    ParentBranch = tLayout.Content
            End Select
        End While
        Return CloseCheckTabablzControl(tTEFile, ParentBranch, CloseType)

        Return False
    End Function
    Private Function CloseCheckTabablzControl(tTEFile As TEFile, Control As TabablzControl, CloseType As CloseType) As Boolean

        Dim tefile As TEFile = Nothing
        Dim index As Integer

        For i = 0 To Control.Items.Count - 1
            Dim TabContent As Object = CType(Control.Items(index), TabItem).Content

            If TypeOf TabContent Is TECUIPage Then
                Dim tPage As TECUIPage = TabContent
                tefile = tPage.TEFile
            ElseIf TypeOf TabContent Is TEGUIPage Then
                Dim tPage As TEGUIPage = TabContent
                tefile = tPage.TEFile
            End If


            If tefile Is tTEFile Then
                Select Case CloseType
                    Case CloseType.This
                        Control.Items.RemoveAt(index)
                        Return True
                    Case CloseType.All
                        Control.Items.RemoveAt(index)
                    Case CloseType.Other
                        index += 1
                End Select
            Else
                Select Case CloseType
                    Case CloseType.This
                        index += 1
                    Case CloseType.All
                        Control.Items.RemoveAt(index)
                    Case CloseType.Other
                        Control.Items.RemoveAt(index)
                End Select
            End If
        Next


        Return False
    End Function
End Module
Namespace WindowMenu
    Public Module WindowMenus
        Public Sub MenuItemSave()
            If Not pgData.IsCompilng And Tool.IsProjectLoad Then
                pjData.Save()
                ProjectControlBinding.PropertyChangedPack()
            End If
        End Sub
        Public Sub MenuItemSaveAs()
            If Not pgData.IsCompilng And Tool.IsProjectLoad Then
                pjData.Save(True)
                ProjectControlBinding.PropertyChangedPack()
            End If
        End Sub

        Public Sub NewFile()
            If Not pgData.IsCompilng Then
                Tool.CloseOtherWindow()
                ProjectData.Load(True, pjData)
                ProjectControlBinding.PropertyChangedPack()
            End If
        End Sub

        Public Sub Setting()
            If Not pgData.IsCompilng Then
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
            End If
        End Sub

        Public Sub Close()
            If Not pgData.IsCompilng And Tool.IsProjectLoad Then
                If pjData.CloseFile() Then
                    pjData = Nothing
                End If

                ProjectControlBinding.PropertyChangedPack()
            End If
        End Sub

        Public Sub Save()
            If Not pgData.IsCompilng And Tool.IsProjectLoad Then
                pjData.Save()
                ProjectControlBinding.PropertyChangedPack()
            End If
        End Sub

        Public Sub Load()
            If Not pgData.IsCompilng Then
                ProjectData.Load(False, pjData)
                ProjectControlBinding.PropertyChangedPack()
            End If
        End Sub

        Public Sub ScmdOpen()
            If Tool.IsProjectLoad Then
                ProjectControlBinding.PropertyChangedPack()

                If My.Computer.FileSystem.FileExists(pjData.OpenMapName) Then
                    Try
                        Process.Start(pjData.OpenMapName)
                    Catch ex As ComponentModel.Win32Exception
                        MsgBox(Tool.GetText("ConnectProgramNoExist"), MsgBoxStyle.Critical)
                    End Try

                    Exit Sub
                Else
                    If MsgBox(Tool.GetText("Error OpenMap is not exist reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                        If Tool.OpenMapSet Then
                            ProjectControlBinding.PropertyChangedPack()
                            Try
                                Process.Start(pjData.OpenMapName)
                            Catch ex As ComponentModel.Win32Exception
                                MsgBox(Tool.GetText("ConnectProgramNoExist"), MsgBoxStyle.Critical)
                            End Try

                            Exit Sub
                        End If
                    End If
                End If
                Tool.ErrorMsgBox(Tool.GetText("Error OpenMap is not exist!"))
            End If
        End Sub

        Public Sub insert(Optional IsEdd As Boolean = False)
            If Tool.IsProjectLoad Then
                ProjectControlBinding.PropertyChangedPack()

                pjData.EudplibData.Build(IsEdd)
            End If
        End Sub


        Public Sub OpenDataEditor()
            If Tool.IsProjectLoad Then
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
                            scData.LoadGRPData()
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


                If scData.LoadStarCraftData Then
                    'pjData.SetDirty(True)
                    Dim DataEditorForm As New DataEditor(DataEditor.OpenType.MainWindow)
                    DataEditorForm.Show()
                    OpenToolWindow()
                End If
            End If
        End Sub

        Public Sub OpenTriggerEdit()
            If Tool.IsProjectLoad Then
                ProjectControlBinding.PropertyChangedPack()

                'Dim TriggerEditorForm As New FastTreeview
                'TriggerEditorForm.Show()

                If Not scData.LoadStarCraftData Then '로드가 되어있지 않을 경우 판단
                    If MsgBox(Tool.GetText("Error NotExistMPQ reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                        Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
                        .Filter = "StarCraft.exe|StarCraft.exe",
                        .FileName = "StarCraft.exe",
                        .Title = Tool.GetText("StarExeFile Select")
                        }


                        If opendialog.ShowDialog() = Forms.DialogResult.OK Then
                            pgData.Setting(ProgramData.TSetting.starcraft) = opendialog.FileName
                            scData.LoadGRPData()
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

                If scData.LoadStarCraftData Then
                    Dim flag As Boolean = False
                    For Each win As Window In Application.Current.Windows
                        If win.GetType Is GetType(TriggerEditor) Then
                            win.Activate()

                            flag = True
                            Exit For
                        End If
                    Next

                    'flag가 True이면 윈도우 첫 실행이므로 TabItem을 불러와야됨.
                    If Not flag Then
                        Dim TriggerEditorForm As New TriggerEditor
                        TriggerEditorForm.Show()
                        TriggerEditorForm.LoadLastTabItems()
                        OpenToolWindow()
                    End If
                End If
            End If

        End Sub

        Public Sub OpenPlugin()
            If Tool.IsProjectLoad Then
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
                            scData.LoadGRPData()
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


                If scData.LoadStarCraftData Then
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
                End If
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