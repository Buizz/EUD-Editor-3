Imports System.IO
Imports System.Windows.Threading
Imports Dragablz
Imports MaterialDesignThemes.Wpf
Imports Newtonsoft.Json

Namespace Tool
    Module Tool
        Public ReadOnly ProhibitParam() As String = {"Unit Map String", "Unknown1", "Unknown2", "Health Bar", "Sel.Circle Image", "Sel.Circle Offset", "Unused", "Unknown 4", "Unknown6", "Unknown17"}


        Public Sub CloseWindows()
            For Each win As Window In Application.Current.Windows
                If win.GetType Is GetType(DataEditor) Or win.GetType Is GetType(TriggerEditor) Or win.GetType Is GetType(PluginWindow) Then
                    win.Close()
                End If
            Next
        End Sub

        Public ReadOnly Property StarCraftPath() As String
            Get
                Return pgData.Setting(ProgramData.TSetting.starcraft).Replace("StarCraft.exe", "")
            End Get
        End Property

        Private ReadOnly MPQFiles() As String = {"patch_rt.mpq", "patch_ed.mpq", "BrooDat.mpq", "BroodWar.mpq", "StarDat.mpq"}
        Public Function LoadDataFromMPQ(filename As String) As Byte()
            Dim hmpq As UInteger
            Dim hfile As UInteger
            Dim buffer() As Byte
            Dim filesize As UInteger

            Dim pdwread As IntPtr

            For i = 0 To MPQFiles.Count - 1
                Dim mpqname As String = StarCraftPath & MPQFiles(i)
                SFmpq.SFileOpenArchive(mpqname, 0, 0, hmpq)


                SFmpq.SFileOpenFileEx(hmpq, filename, 0, hfile)

                If hfile <> 0 Then
                    filesize = SFmpq.SFileGetFileSize(hfile, filesize)
                    ReDim buffer(filesize)

                    SFmpq.SFileReadFile(hfile, buffer, filesize, pdwread, 0)

                    SFmpq.SFileCloseFile(hfile)
                    SFmpq.SFileCloseArchive(hmpq)
                    Return buffer
                End If
                SFmpq.SFileCloseArchive(hmpq)
            Next


            Throw New System.Exception("File Load Fail from MPQ. " & filename)
            Return Nothing
        End Function



        Public Function IsProjectLoad() As Boolean
            If pjData IsNot Nothing Then
                If pjData.IsLoad Then
                    Return True
                End If
            End If
            Return False
        End Function


        Public Function GetText(Text As String) As String
            Return Application.Current.Resources(Text)
        End Function

        Public Sub ErrorMsgBox(str As String, Optional Logstr As String = "")
            MsgBox(str, MsgBoxStyle.Critical, Tool.GetText("ErrorMsgbox"))
            MsgBox("테스트용 로그 확인" & vbCrLf & Logstr)
        End Sub

        Public Function OpenMapSet() As Boolean
            Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
            .Filter = Tool.GetText("SCX Fliter"),
            .Title = Tool.GetText("SCX Select")
            }
            Dim LastOpenMapName As String = pjData.OpenMapName
            '맵이 플텍맵인지 아닌지 검사해야됨.
            '맵이 저장맵이랑 이름이 같은지 검사해야됨.
            If opendialog.ShowDialog() = Forms.DialogResult.OK Then
                If pjData.SaveMapName = opendialog.FileName Then
                    Tool.ErrorMsgBox(Tool.GetText("Error OpenMap is not SaveMap"))
                    Return False
                End If

                '맵이 플텍맵인지 아닌지 검사해야됨.
                pjData.OpenMapName = opendialog.FileName
                If pjData.IsMapLoading Then
                    Return True
                Else
                    pjData.OpenMapName = LastOpenMapName
                    Return False
                End If
            End If
            Return False
        End Function

        Public Function SaveMapSet() As Boolean
            Dim savedialog As New System.Windows.Forms.SaveFileDialog With {
            .Filter = Tool.GetText("SCX Fliter"),
            .Title = Tool.GetText("SCX Save Select"),
            .OverwritePrompt = False
         }


            '맵이 오픈맵이랑 이름이 같은지 검사해야됨.
            If savedialog.ShowDialog() = Forms.DialogResult.OK Then
                If pjData.OpenMapName = savedialog.FileName Then
                    Tool.ErrorMsgBox(Tool.GetText("Error OpenMap is not SaveMap"))
                    Return False
                End If

                pjData.SaveMapName = savedialog.FileName
                Return True
            End If
            Return False
        End Function

        Public ReadOnly Property GetSettingFile() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Setting.ini"
            End Get
        End Property
        Public ReadOnly Property GetDatFolder() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\DatFiles"
            End Get
        End Property

        Public Function GetTitleName() As String
            If pjData IsNot Nothing Then
                If pjData.IsLoad Then
                    Dim IsDirty As String = ""
                    Dim Filename As String = pjData.SafeFilename
                    If pjData.IsDirty Then
                        IsDirty = "*"
                    End If
                    If pjData.SafeFilename = "" Then
                        Filename = GetText("NoName")
                    End If

                    Return Filename & IsDirty & " - EUD Editor 3 v" & pgData.Version
                End If
            End If
            Return "EUD Editor 3 v" & pgData.Version
        End Function

        Public ReadOnly Property GetLanguageFolder() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Language"
            End Get
        End Property

        Public ReadOnly Property GetTblFolder() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Tbls"
            End Get
        End Property


        Public SaveProjectDialog As System.Windows.Forms.SaveFileDialog

        Private MainWindow As MainWindow
        Public Sub Init()
            SaveProjectDialog = New System.Windows.Forms.SaveFileDialog
            SaveProjectDialog.Filter = GetText("SaveFliter")




            For Each win As Window In Application.Current.Windows
                If win.GetType Is GetType(MainWindow) Then
                    MainWindow = win
                End If
            Next
        End Sub

        Public Sub RefreshMainWindow()
            Try
                MainWindow.BtnRefresh()
            Catch ex As Exception

            End Try
        End Sub


        Public Sub SetRegistry()
            Dim str As String() = {"e3s"} ', "e3p", "e2s", "e2p", "ees", "mem"}

            For Each Extension As String In str
                My.Computer.Registry.ClassesRoot.CreateSubKey("." & Extension & "").SetValue("",
                    "" & Extension & "", Microsoft.Win32.RegistryValueKind.String)
                My.Computer.Registry.ClassesRoot.CreateSubKey("" & Extension & "\shell\open\command").SetValue("",
                System.Windows.Forms.Application.ExecutablePath & " ""%l"" ", Microsoft.Win32.RegistryValueKind.String)
                My.Computer.Registry.ClassesRoot.CreateSubKey("" & Extension & "\DefaultIcon").SetValue("",
               System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Icons\" & Extension & ".ico" & ",0", Microsoft.Win32.RegistryValueKind.String)
            Next

        End Sub
    End Module
End Namespace
Namespace TabItemTool
    Module TabItemTool
        Public Sub WindowTabItem(Datfile As SCDatFiles.DatFiles, index As Integer)
            Dim DataEditorForm As New DataEditor
            DataEditorForm.Show()
            DataEditorForm.OpenbyOthers(GetTabItem(Datfile, index), Datfile)
        End Sub

        Private TabTypeArray As Type() = {GetType(UnitData), GetType(WeaponData), GetType(FlingyData), GetType(SpriteData), GetType(ImageData), GetType(UpgradeData), GetType(TechData), GetType(OrderData), Nothing, Nothing, Nothing, GetType(StatTxtData)}
        Public Sub ChanageTabItem(Datfile As SCDatFiles.DatFiles, index As Integer, MainTab As Dockablz.Layout)
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



            If TabContent.Items.Count <> 0 Then
                Dim ChangesTabItem As TabItem = TabContent.Items(0)
                If ChangesTabItem.Content.GetType() = TabTypeArray(Datfile) Then '같은거 일 경우
                    Dim TGrid As Grid = ChangesTabItem.Header
                    Dim TabText As TextBlock = TGrid.Children.Item(0)

                    Dim myBinding As Binding = New Binding("Name")
                    myBinding.Source = pjData.BindingManager.UIManager(Datfile, index)
                    TabText.SetBinding(TextBlock.TextProperty, myBinding)


                    ChangesTabItem.Content.ReLoad(Datfile, index)
                    TabContent.SelectedItem = ChangesTabItem
                Else
                    Dim TabItem As TabItem = GetTabItem(Datfile, index)
                    TabContent.Items.RemoveAt(0)
                    TabContent.Items.Insert(0, TabItem)
                    TabContent.SelectedItem = TabItem
                End If

            Else
                Dim TabItem As TabItem = GetTabItem(Datfile, index)
                TabContent.Items.Add(TabItem)
                TabContent.SelectedItem = TabItem
            End If
        End Sub
        Public Sub PlusTabItem(Datfile As SCDatFiles.DatFiles, index As Integer, MainTab As Dockablz.Layout)
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

            Dim TabItem As TabItem = GetTabItem(Datfile, index)
            TabContent.Items.Add(TabItem)
            TabContent.SelectedItem = TabItem
        End Sub

        Public Function GetTabItem(Datfile As SCDatFiles.DatFiles, index As Integer) As TabItem
            Dim TabItem As New TabItem
            Dim TabGrid As New Grid
            Dim TabText As New TextBlock
            Dim TabContextMenu As New ContextMenu
            'TabText.Text = pjData.CodeLabel(CodePage, index)

            TabText.SetResourceReference(TextBlock.ForegroundProperty, "PrimaryHueMidForegroundBrush")
            TabText.HorizontalAlignment = HorizontalAlignment.Center
            TabText.VerticalAlignment = VerticalAlignment.Center





            Dim TabCloseCommand As New TabCloseCommand(TabItem)

            Dim RightCloseMenuItem As New MenuItem
            Dim OtherCloseMenuItem As New MenuItem
            If True Then
                Dim tabmenuitem As New MenuItem
                tabmenuitem.Header = Tool.GetText("TabClose")
                tabmenuitem.Command = TabablzControl.CloseItemCommand

                Dim PIcon As New PackIcon()
                PIcon.Kind = PackIconKind.Close
                tabmenuitem.Icon = PIcon
                TabContextMenu.Items.Add(tabmenuitem)
            End If

            If True Then
                RightCloseMenuItem.Header = Tool.GetText("RightTabsClose")
                RightCloseMenuItem.CommandParameter = TabCloseCommand.CommandType.RightClose
                RightCloseMenuItem.Command = TabCloseCommand

                Dim PIcon As New PackIcon()
                PIcon.Kind = PackIconKind.ArrowExpandRight
                RightCloseMenuItem.Icon = PIcon
                TabContextMenu.Items.Add(RightCloseMenuItem)
            End If

            If True Then
                OtherCloseMenuItem.Header = Tool.GetText("OtherTabsClose")
                OtherCloseMenuItem.CommandParameter = TabCloseCommand.CommandType.OtherClose
                OtherCloseMenuItem.Command = TabCloseCommand

                Dim PIcon As New PackIcon()
                PIcon.Kind = PackIconKind.ArrowSplitVertical
                OtherCloseMenuItem.Icon = PIcon
                TabContextMenu.Items.Add(OtherCloseMenuItem)
            End If

            Dim TabCloseEnabled As New TabCloseEnabled(TabItem, RightCloseMenuItem, OtherCloseMenuItem)
            TabItem.AddHandler(MenuItem.ContextMenuOpeningEvent, New RoutedEventHandler(AddressOf TabCloseEnabled.OpenEvent))

            'TabGrid.Background = Application.Current.Resources("PrimaryHueMidBrush")
            TabGrid.ContextMenu = TabContextMenu
            TabGrid.Height = 34
            TabGrid.Margin = New Thickness(0, -5, 0, -5)
            TabGrid.Children.Add(TabText)


            TabItem.Header = TabGrid


            Dim myBinding As Binding = New Binding("Name")
            Select Case Datfile
                Case SCDatFiles.DatFiles.units
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.units, index)
                    TabItem.Content = New UnitData(index)
                Case SCDatFiles.DatFiles.weapons
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.weapons, index)
                    TabItem.Content = New WeaponData(index)
                Case SCDatFiles.DatFiles.flingy
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.flingy, index)
                    TabItem.Content = New FlingyData(index)
                Case SCDatFiles.DatFiles.sprites
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.sprites, index)
                    TabItem.Content = New SpriteData(index)
                Case SCDatFiles.DatFiles.images
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.images, index)
                    TabItem.Content = New ImageData(index)
                Case SCDatFiles.DatFiles.upgrades
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.upgrades, index)
                    TabItem.Content = New UpgradeData(index)
                Case SCDatFiles.DatFiles.techdata
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.techdata, index)
                    TabItem.Content = New TechData(index)
                Case SCDatFiles.DatFiles.orders
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.orders, index)
                    TabItem.Content = New OrderData(index)
                Case SCDatFiles.DatFiles.stattxt
                    myBinding.Source = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.stattxt, index)
                    TabItem.Content = New StatTxtData(index)
            End Select
            TabText.SetBinding(TextBlock.TextProperty, myBinding)

            Return TabItem
        End Function
    End Module


End Namespace