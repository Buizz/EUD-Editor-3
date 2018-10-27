Imports System.IO
Imports System.Windows.Forms
Imports Newtonsoft.Json

Namespace Tool
    Module Tool
        Public Sub LoadProject(isNewfile As Boolean)
            If isNewfile Then
                pjData = New ProjectData
                pjData.NewFIle()
            Else
                Dim tFilename As String
                If Tool.LoadProjectDialog.ShowDialog() = Forms.DialogResult.OK Then
                    tFilename = Tool.LoadProjectDialog.FileName '파일 이름 교체
                Else
                    Exit Sub
                End If

                Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(ProjectData))
                Dim file As New System.IO.StreamReader(tFilename)
                pjData = CType(reader.Deserialize(file), ProjectData)
                pjData.LoadInit(tFilename)
                ' pjData.IsLoad = True

                'Dim fs As New FileStream(tFilename, FileMode.Open)
                'Dim sr As New StreamReader(fs)

                'pjData = CType( JsonConvert.DeserializeObject(sr.ReadToEnd), ProjectData )

                'sr.Close()
                'fs.Close()
            End If

        End Sub

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
                Return True
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


        Public SaveProjectDialog As SaveFileDialog
        Public LoadProjectDialog As OpenFileDialog


        Public Sub Init()
            SaveProjectDialog = New SaveFileDialog
            SaveProjectDialog.Filter = GetText("SaveFliter")


            LoadProjectDialog = New OpenFileDialog
            LoadProjectDialog.Filter = GetText("LoadFliter")
        End Sub
    End Module
End Namespace
