
Imports System.ComponentModel

Public Class SettingWindows
    Private DatLoad As Boolean = False
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        If Not Tool.IsProjectLoad Then
            MainTab.Items.Remove(TabItem_ProjectSetting)
            'TabItem_ProjectSetting.Height = 0
        End If

        TBStarCraftexe.Text = pgData.Setting(ProgramData.TSetting.starcraft)
        TBeuddraftexe.Text = pgData.Setting(ProgramData.TSetting.euddraft)
        CheckeuddraftVersion(pgData.Setting(ProgramData.TSetting.euddraft))
        '언어 설정
        Dim CurrentLan As String = pgData.Setting(ProgramData.TSetting.Language)
        'MsgBox(CurrentLan)
        For i = 0 To CBLanguage.Items.Count - 1
            'MsgBox(i)
            Dim SelectItem As ComboBoxItem = CBLanguage.Items(i)
            If SelectItem.Tag = CurrentLan Then
                CBLanguage.SelectedIndex = i
                Exit For
            End If
        Next

        CBGraphic.SelectedIndex = pgData.Setting(ProgramData.TSetting.Graphic)

        CBCodeLan.SelectedIndex = pgData.Setting(ProgramData.TSetting.CDLanguage)
        ChangeTblUse.IsChecked = pgData.Setting(ProgramData.TSetting.CDLanguageChange)
        TopMostforce.IsChecked = pgData.Setting(ProgramData.TSetting.DataEditorTopMost)
        TopMostTEforce.IsChecked = pgData.Setting(ProgramData.TSetting.TriggerEditrTopMost)
        TETestCodeEditorUse.IsChecked = pgData.Setting(ProgramData.TSetting.TestCodeEditorUse)
        TECESmoothScroolUse.IsChecked = pgData.Setting(ProgramData.TSetting.TECEUseSmoothScrool)
        TopMostPluginforce.IsChecked = pgData.Setting(ProgramData.TSetting.PluginSettingTopMost)
        Mute.IsChecked = pgData.Setting(ProgramData.TSetting.MuteSound)


        Dim fsize As Integer = pgData.Setting(ProgramData.TSetting.TEFontSize)
        For i = 1 To 40
            Dim cbitem As New ComboBoxItem
            cbitem.Content = i & "px"
            cbitem.Tag = i

            FontSizecb.Items.Add(cbitem)


            If fsize = i Then
                FontSizecb.SelectedIndex = FontSizecb.Items.Count - 1
            End If
        Next



        If Tool.IsProjectLoad Then
            TBOpenMap.Text = pjData.OpenMapName
            TBSaveMap.Text = pjData.SaveMapName

            UseCustomTbl.IsChecked = pjData.UseCustomtbl
            AutoBuild.IsChecked = pjData.AutoBuild
            LogView.IsChecked = pjData.ViewLog



            Select Case pjData.TempFileLoc
                Case "0", "1", "2"
                    TempFileCombobox.SelectedIndex = pjData.TempFileLoc
                    TempFileCombobox.Width = 529
                    TempFiletextbox.Visibility = Visibility.Collapsed
                    TempFilebtn.Visibility = Visibility.Collapsed
                Case Else
                    TempFiletextbox.Text = pjData.TempFileLoc
                    TempFileCombobox.SelectedIndex = 3
                    TempFileCombobox.Width = 80
                    TempFiletextbox.Visibility = Visibility.Visible
                    TempFilebtn.Visibility = Visibility.Visible
            End Select
        End If

        e3scb.IsChecked = Not Tool.CheckexeConnect("e3s")
        e2scb.IsChecked = Not Tool.CheckexeConnect("e2s")
        'e2pcb.IsChecked = Not Tool.CheckexeConnect("e2p")
        'eescb.IsChecked = Not Tool.CheckexeConnect("ees")
        'memcb.IsChecked = Not Tool.CheckexeConnect("mem")



        regCheck.IsChecked = pgData.Setting(ProgramData.TSetting.CheckReg)

        upDataCheck.IsChecked = pgData.Setting(ProgramData.TSetting.CheckUpdate)




        PatchNote.AppendText("Loading...")
        UpDateBtn.IsEnabled = False
        UpDateBtn.Content = "Loading..."


        UpdateChecker = New BackgroundWorker()
        AddHandler UpdateChecker.DoWork, AddressOf UpdateChecker_DoWork
        AddHandler UpdateChecker.RunWorkerCompleted, AddressOf UpdateChecker_RunWorkerCompleted
        UpdateChecker.RunWorkerAsync()


        Dim selectencode As String = ""
        If pjData IsNot Nothing Then
            If pjData.TextEncoding IsNot Nothing Then
                selectencode = pjData.TextEncoding.EncodingName
            End If
        End If

        If True Then
            Dim cb As New ComboBoxItem

            cb.Content = Tool.GetText("None")
            cb.Tag = Nothing
            EncodingCombobox.Items.Add(cb)
            If selectencode = "" Then
                EncodingCombobox.SelectedItem = cb
            End If
        End If

        If True Then
            Dim cb As New ComboBoxItem

            Dim ed As System.Text.Encoding = System.Text.Encoding.GetEncoding(0)

            cb.Content = ed.EncodingName
            cb.Tag = ed
            EncodingCombobox.Items.Add(cb)
            If selectencode = ed.EncodingName Then
                EncodingCombobox.SelectedItem = cb
            End If
        End If

        If True Then
            Dim cb As New ComboBoxItem

            Dim ed As System.Text.Encoding = System.Text.Encoding.UTF8

            cb.Content = ed.EncodingName
            cb.Tag = ed
            EncodingCombobox.Items.Add(cb)
            If selectencode = ed.EncodingName Then
                EncodingCombobox.SelectedItem = cb
            End If
        End If

        DatLoad = True
    End Sub



    Private Sub FontSize_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim cbitem As ComboBoxItem = FontSizecb.SelectedItem


        pgData.Setting(ProgramData.TSetting.TEFontSize) = cbitem.Tag
    End Sub



    Private VersionText As String
    Private PatchText As String


    Private UpdateChecker As BackgroundWorker
    Private Sub UpdateChecker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        Try
            With CreateObject("WinHttp.WinHttpRequest.5.1")
                .Open("GET", "https://raw.githubusercontent.com/Buizz/EUD-Editor-3/master/EUD%20Editor%203/Version.txt")
                .Send
                .WaitForResponse

                VersionText = .ResponseText

            End With
        Catch ex As Exception
            VersionText = "Error"
        End Try

        PatchNoteLoader()
    End Sub
    Private Sub UpdateChecker_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        Dim version As String

        Dim lines1() As String = VersionText.Trim.Split(vbLf)

        If lines1.Count > 1 Then
            version = lines1(0).Trim
            If pgData.Version.ToString = version Then
                UpDateBtn.Content = Tool.GetText("LastVersion")
                UpDateBtn.IsEnabled = False
            Else
                UpDateBtn.Content = version & " " & Tool.GetText("Update")
                UpDateBtn.IsEnabled = True
            End If
        Else
            UpDateBtn.Content = Tool.GetText("NotFoundServer")
            UpDateBtn.IsEnabled = False
        End If


        Dim myFlowDoc As FlowDocument = New FlowDocument()


        Dim lines2() As String = PatchText.Trim.Split(vbLf)


        For i = 0 To lines2.Count - 1
            Dim line As String = lines2(i).Trim

            If i = lines2.Count - 1 Then
                line = line.Replace("-", "    └ ")
            Else
                If lines2(i + 1).Trim.IndexOf("-") <> -1 Then
                    line = line.Replace("-", "    ├ ")
                Else
                    line = line.Replace("-", "    └ ")
                End If
            End If

            Dim SizeUp As Integer = 0
            If line.Count > 0 Then
                If line.Chars(0) = "#" Then
                    SizeUp = 10
                    line = line.Remove(0, 1).Trim
                End If
                If line.Chars(0) = "+" Then
                    SizeUp = 5
                    line = line.Remove(0, 1).Trim
                    line = "  " & line
                End If
            End If


            Dim myRun As Run = New Run(line)
            Dim myParagraph As Paragraph = New Paragraph()
            myParagraph.FontSize += SizeUp
            myParagraph.LineHeight = 1
            myParagraph.Inlines.Add(myRun)
            myFlowDoc.Blocks.Add(myParagraph)
        Next


        PatchNote.Document = myFlowDoc
    End Sub



    Private Sub PatchNoteLoader()
        Try
            With CreateObject("WinHttp.WinHttpRequest.5.1")
                .Open("GET", "https://raw.githubusercontent.com/Buizz/EUD-Editor-3/master/EUD%20Editor%203/PatchNote.txt")
                .Send
                .WaitForResponse

                PatchText = .ResponseText

            End With
        Catch ex As Exception
            PatchText = "Error"
        End Try
    End Sub




    Private Sub BtnStarCraftexe(sender As Object, e As RoutedEventArgs)
        Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
            .Filter = "StarCraft Launcher.exe|StarCraft Launcher.exe",
            .FileName = "StarCraft Launcher.exe",
            .Title = Tool.GetText("StarExeFile Select")
        }


        If opendialog.ShowDialog() = Forms.DialogResult.OK Then
            pgData.Setting(ProgramData.TSetting.starcraft) = opendialog.FileName

            TBStarCraftexe.Text = opendialog.FileName
            scData.LoadGRPData()
        End If
    End Sub

    Private Sub Btneuddraftexe(sender As Object, e As RoutedEventArgs)
        Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
            .Filter = "euddraft.exe|euddraft.exe",
            .FileName = "euddraft.exe",
            .Title = Tool.GetText("euddraftExe Select")
        }


        If opendialog.ShowDialog() = Forms.DialogResult.OK Then
            CheckeuddraftVersion(opendialog.FileName)


            pgData.Setting(ProgramData.TSetting.euddraft) = opendialog.FileName
            TBeuddraftexe.Text = opendialog.FileName
        End If
    End Sub
    Private Sub CheckeuddraftVersion(filename As String)
        If My.Computer.FileSystem.FileExists(filename) Then
            CheckThreading = New System.Threading.Thread(AddressOf CheckProgress)
            CheckThreading.Start(filename)
        Else
            euddraftVersion1.Content = Nothing
            euddraftVersion2.Content = Nothing
        End If
    End Sub
    Private CheckThreading As Threading.Thread
    Private Sub CheckProgress(filename As String)
        Dim process As New Process
        Dim startInfo As New ProcessStartInfo

        startInfo.FileName = filename

        'startInfo.StandardOutputEncoding = Text.Encoding.UTF32
        'startInfo.StandardErrorEncoding = Text.Encoding.UTF32
        startInfo.RedirectStandardOutput = True
        startInfo.RedirectStandardInput = True
        startInfo.RedirectStandardError = True
        startInfo.WindowStyle = ProcessWindowStyle.Hidden
        startInfo.CreateNoWindow = True

        startInfo.UseShellExecute = False

        process.StartInfo = startInfo
        process.Start()

        Dim StandardOutput As String = ""
        Dim StandardError As String = ""

        While Not process.HasExited
            process.StandardInput.Write(vbCrLf)
        End While
        StandardOutput = process.StandardOutput.ReadLine

        If StandardOutput Is Nothing Then
            Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Normal,
            New Action(Sub()
                           euddraftVersion1.Content = Tool.GetText("euddraftVersion1") & "ERROR"
                           euddraftVersion2.Content = Tool.GetText("euddraftVersion2") & "euddraft " & pgData.RecommendeuddraftVersion.ToString
                       End Sub))
        Else
            Dim FrishLine As String = StandardOutput.Split(":")(0).Trim

            Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Normal,
            New Action(Sub()
                           euddraftVersion1.Content = Tool.GetText("euddraftVersion1") & FrishLine
                           euddraftVersion2.Content = Tool.GetText("euddraftVersion2") & "euddraft " & pgData.RecommendeuddraftVersion.ToString
                       End Sub))
        End If
    End Sub



    Private Sub BtnOpenMapSet_Click(sender As Object, e As RoutedEventArgs)
        If Tool.OpenMapSet Then
            TBOpenMap.Text = pjData.OpenMapName
        End If
    End Sub

    Private Sub BtnSaveMapSet_Click(sender As Object, e As RoutedEventArgs)
        If Tool.SaveMapSet Then
            TBSaveMap.Text = pjData.SaveMapName
        End If
    End Sub





    Private Sub CBLanguage_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If e.AddedItems.Count <> 0 Then
            Dim SelectItem As ComboBoxItem = e.AddedItems(0)

            Dim languagename As String = SelectItem.Tag

            pgData.SetLanguage(languagename)

        End If
    End Sub

    Private Sub CBCodeLan_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If e.AddedItems.Count <> 0 Then
            Dim SelectItem As ComboBox = sender


            pgData.Setting(ProgramData.TSetting.CDLanguage) = SelectItem.SelectedIndex
        End If
    End Sub

    Private Sub ChangeTblUse_Checked(sender As Object, e As RoutedEventArgs)
        pgData.Setting(ProgramData.TSetting.CDLanguageChange) = ChangeTblUse.IsChecked
    End Sub

    Private Sub TopMostforce_Checked(sender As Object, e As RoutedEventArgs)
        pgData.Setting(ProgramData.TSetting.DataEditorTopMost) = TopMostforce.IsChecked
    End Sub

    Private Sub TempFilebtn_Click(sender As Object, e As RoutedEventArgs)
        Dim folderSelect As New System.Windows.Forms.FolderBrowserDialog

        If folderSelect.ShowDialog = Forms.DialogResult.OK Then
            pjData.TempFileLoc = folderSelect.SelectedPath

            TempFiletextbox.Text = pjData.TempFileLoc
        End If
    End Sub

    Private Sub TempFileCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If DatLoad Then
            Select Case TempFileCombobox.SelectedIndex
                Case 0, 1, 2
                    pjData.TempFileLoc = TempFileCombobox.SelectedIndex
                    TempFileCombobox.Width = 529
                    TempFiletextbox.Visibility = Visibility.Collapsed
                    TempFilebtn.Visibility = Visibility.Collapsed
                Case Else
                    If pjData.TempFileLoc = "0" Or pjData.TempFileLoc = "1" Or pjData.TempFileLoc = "2" Then
                        pjData.TempFileLoc = ""
                        TempFiletextbox.Text = ""
                    Else
                        TempFiletextbox.Text = pjData.TempFileLoc
                    End If
                    TempFileCombobox.Width = 80
                    TempFiletextbox.Visibility = Visibility.Visible
                    TempFilebtn.Visibility = Visibility.Visible
            End Select
        End If
    End Sub

    Private Sub EncodingCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If DatLoad Then
            If EncodingCombobox.SelectedItem IsNot Nothing Then
                Dim cb As ComboBoxItem = EncodingCombobox.SelectedItem
                Dim ed As Text.Encoding = cb.Tag

                pjData.TextEncoding = ed

                pjData.ReloadMap()
            End If
        End If
    End Sub


    Private Sub UseCustomTbl_Checked(sender As Object, e As RoutedEventArgs)
        If DatLoad Then
            pjData.UseCustomtbl = UseCustomTbl.IsChecked
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        If My.Computer.FileSystem.DirectoryExists(pjData.OpenMapdirectory) Then
            Process.Start("explorer.exe", "/root," & pjData.OpenMapdirectory)
        End If
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        If My.Computer.FileSystem.DirectoryExists(pjData.SaveMapdirectory) Then
            Process.Start("explorer.exe", "/root," & pjData.SaveMapdirectory)
        End If
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        If My.Computer.FileSystem.DirectoryExists(BuildData.TempFloder) Then
            Process.Start("explorer.exe", "/root," & BuildData.TempFloder)
        End If
    End Sub

    Private Sub ExeConnectbtn_Click(sender As Object, e As RoutedEventArgs)
        Dim argument As String = ""
        If e3scb.IsChecked Then
            If argument = "" Then
                argument = "e3s"
            Else
                argument = argument & "," & "e3s"
            End If
        End If
        If e2scb.IsChecked Then
            If argument = "" Then
                argument = "e2s"
            Else
                argument = argument & "," & "e2s"
            End If
        End If
        'If e2pcb.IsChecked Then
        '    If argument = "" Then
        '        argument = "e2p"
        '    Else
        '        argument = argument & "," & "e2p"
        '    End If
        'End If
        'If eescb.IsChecked Then
        '    If argument = "" Then
        '        argument = "ees"
        '    Else
        '        argument = argument & "," & "ees"
        '    End If
        'End If
        'If memcb.IsChecked Then
        '    If argument = "" Then
        '        argument = "mem"
        '    Else
        '        argument = argument & "," & "mem"
        '    End If
        'End If

        Tool.StartRegSetter(argument)
    End Sub

    Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
        pgData.Setting(ProgramData.TSetting.CheckReg) = True
    End Sub

    Private Sub CheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        pgData.Setting(ProgramData.TSetting.CheckReg) = False
    End Sub

    Private Sub TECESmoothScroolUse_Checked(sender As Object, e As RoutedEventArgs)
        pgData.Setting(ProgramData.TSetting.TECEUseSmoothScrool) = TECESmoothScroolUse.IsChecked
    End Sub

    Private Sub TETestCodeEditorUse_Checked(sender As Object, e As RoutedEventArgs)
        pgData.Setting(ProgramData.TSetting.TestCodeEditorUse) = TETestCodeEditorUse.IsChecked
    End Sub

    Private Sub TopMostTEforce_Checked(sender As Object, e As RoutedEventArgs)
        pgData.Setting(ProgramData.TSetting.TriggerEditrTopMost) = TopMostTEforce.IsChecked
    End Sub

    Private Sub TopMostPluginforce_Checked(sender As Object, e As RoutedEventArgs)
        pgData.Setting(ProgramData.TSetting.PluginSettingTopMost) = TopMostPluginforce.IsChecked
    End Sub


    Private Sub UpDataCheck_Unchecked(sender As Object, e As RoutedEventArgs)
        pgData.Setting(ProgramData.TSetting.CheckUpdate) = False
    End Sub

    Private Sub UpDataCheck_Checked(sender As Object, e As RoutedEventArgs)
        pgData.Setting(ProgramData.TSetting.CheckUpdate) = True
    End Sub


    Private Sub UpDateBtn_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.Close()
        If Not Tool.IsProjectLoad Then
            Tool.StartUpdaterSetter()
            Application.Current.Shutdown()
        End If
    End Sub

    Private Sub AutoBuild_Checked(sender As Object, e As RoutedEventArgs)
        If DatLoad Then
            pjData.AutoBuild = AutoBuild.IsChecked
        End If
    End Sub

    Private Sub LogView_Checked(sender As Object, e As RoutedEventArgs)
        If DatLoad Then
            pjData.ViewLog = LogView.IsChecked
        End If
    End Sub

    Private Sub CBGraphic_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If CBGraphic.SelectedIndex <> -1 Then
            pgData.Setting(ProgramData.TSetting.Graphic) = CBGraphic.SelectedIndex
            scData.LoadGRPData()


            CBGraphic.SelectedIndex = pgData.Setting(ProgramData.TSetting.Graphic)
        End If
    End Sub

    Private Sub BtnOpenMapCreate_Click(sender As Object, e As RoutedEventArgs)
        Me.Visibility = Visibility.Hidden
        If Tool.CreateMapSet(Me) Then
            TBOpenMap.Text = pjData.OpenMapName
        End If
        Me.Visibility = Visibility.Visible
        'MsgBox("ㅎㅇ")
    End Sub

    Private Sub Mute_Checked(sender As Object, e As RoutedEventArgs)
        pgData.Setting(ProgramData.TSetting.MuteSound) = Mute.IsChecked
    End Sub


    'Private Sub CBLanguage_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CBLanguage.SelectionChanged
    '    pgData.Lan.SetLanguage(e.AddedItems(0))
    'End Sub
End Class
