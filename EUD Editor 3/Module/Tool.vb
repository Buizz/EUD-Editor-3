Imports System.IO
Imports System.Windows.Forms
Imports System.Windows.Threading
Imports Newtonsoft.Json

Namespace Tool
    Module Tool
        Public Sub RefreshWindows()
            For Each win As Window In Application.Current.Windows
                If win.GetType Is GetType(DataEditor) Then
                    ' win.Dispatcher.Invoke(DispatcherPriority.Render, DispatcherPriority.Normal);
                End If
            Next
        End Sub

        Public Sub CloseWindows()
            For Each win As Window In Application.Current.Windows
                If win.GetType Is GetType(DataEditor) Then
                    win.Close()
                End If
            Next
        End Sub

        Public ReadOnly Property StarCraftPath() As String
            Get
                Return pgData.Setting(ProgramData.TSetting.starcraft).Replace("StarCraft.exe", "")
            End Get
        End Property

        Private ReadOnly MPQFiles() As String = {"BrooDat.mpq", "StarDat.mpq", "BroodWar.mpq", "Starcraft.mpq"}
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
            Dim opendialog As New OpenFileDialog With {
            .Filter = Tool.GetText("SCX Fliter"),
            .Title = Tool.GetText("SCX Select")
}

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
                    pjData.OpenMapName = ""
                    Return False
                End If
            End If
            Return False
        End Function

        Public Function SaveMapSet() As Boolean
            Dim savedialog As New SaveFileDialog With {
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


        Public SaveProjectDialog As SaveFileDialog

        Private MainWindow As MainWindow
        Public Sub Init()
            SaveProjectDialog = New SaveFileDialog
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
