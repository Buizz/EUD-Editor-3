Imports System.Windows.Forms

Namespace Tool
    Module Tool
        Public ReadOnly Property GetSettingFile() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Setting.ini"
            End Get
        End Property

        Public Function GetTitleName() As String
            If pjData.IsLoad Then
                Return pjData.SafeFilename & " - EUD Editor 3 v" & pgData.Version
            Else
                Return "EUD Editor 3 v" & pgData.Version
            End If
        End Function


        Public SaveProjectDialog As SaveFileDialog
        Public LoadProjectDialog As OpenFileDialog


        Public Sub Init()
            SaveProjectDialog = New SaveFileDialog
            SaveProjectDialog.Filter = "EUD Editor 3 저장 파일|*.e3s|EUD Editor 3 프로젝트 파일|*.e3p|EUD Editor 2 저장 파일|*.e2s|EUD Editor 2 프로젝트 파일|*.e2p"


            LoadProjectDialog = New OpenFileDialog
            LoadProjectDialog.Filter = "EUD Editor 저장 파일|*.e3s;*.e3p;*.e2s;*.e2p;*.ees;*.mem|EUD Editor 3 저장 파일|*.e3s|EUD Editor 3 프로젝트 파일|*.e3p|EUD Editor 2 저장 파일|*.e2s|EUD Editor 2 프로젝트 파일|*.e2p|EUD Editor 1 저장 파일|*.ees|EUD Editor 메모리 파일|*.mem"
        End Sub
    End Module
End Namespace
