Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Partial Public Class ProjectData
    Private SaveData As SaveableData

    Public Function Save(Optional IsSaveAs As Boolean = False) As Boolean
        SaveData.LastVersion = pgData.Version
        TETempData.SaveTabitems()
        TERefreshTabITem()

        Dim lastFileName As String = Filename


        If IsSaveAs = True Then '다른이름으로 저장 일 경우 
            Tool.SaveProjectDialog.FileName = SafeFilename

            Dim exten As String() = Tool.SaveProjectDialog.Filter.Split("|")
            For i = 1 To exten.Count - 1 Step 2
                If Extension = exten(i).Split(".").Last Then
                    Tool.SaveProjectDialog.FilterIndex = ((i - 1) \ 2) + 1
                    Exit For
                End If

            Next

            Tool.SaveProjectDialog.InitialDirectory = pgData.Setting(ProgramData.TSetting.SavePath)
            If Tool.SaveProjectDialog.ShowDialog() = Forms.DialogResult.OK Then
                Filename = Tool.SaveProjectDialog.FileName '파일 이름 교체
                pgData.Setting(ProgramData.TSetting.SavePath) = Path.GetDirectoryName(Filename)
            Else
                Return False
            End If
        End If

        If Not Extension = "e3s" And Not Extension = "e2s" Then
            Filename = ""
        End If

        If Filename = "" Then ' 새파일
            Tool.SaveProjectDialog.FileName = SafeFilename
            Tool.SaveProjectDialog.InitialDirectory = pgData.Setting(ProgramData.TSetting.SavePath)
            If Tool.SaveProjectDialog.ShowDialog() = Forms.DialogResult.OK Then
                Filename = Tool.SaveProjectDialog.FileName '파일 이름 교체
                pgData.Setting(ProgramData.TSetting.SavePath) = Path.GetDirectoryName(Filename)
            Else
                Return False
            End If
        End If


        If extension = "e3s" Then
            Dim stm As Stream = File.Open(Filename, FileMode.Create, FileAccess.ReadWrite)
            Dim bf As BinaryFormatter = New BinaryFormatter()
            bf.Serialize(stm, Me.SaveData)
            stm.Close()
        Else
            Dim dialog As MsgBoxResult = MsgBox("호환성 경고" & vbCrLf &
                       "e2s파일을 저장 할 경우 일부 에러가 발생할 수 있습니다." & vbCrLf &
                       "플러그인 저장 안됨" & vbCrLf &
                       "TE로드 저장 안됨", MsgBoxStyle.Critical Or MsgBoxStyle.OkCancel)


            If dialog = MsgBoxResult.Ok Then
                FileSystem.FileCopy(lastFileName, Filename & ".e2sbackup")
            ElseIf dialog = MsgBoxResult.Cancel Then
                Return False
            End If


            Lagacy.LagacySaveLoad.Save(Filename)
        End If

        BackUpFile(Filename)

        tIsLoad = True
        tIsDirty = False

        If Not pgData.IsCompilng Then
            If AutoBuild Then
                pjData.EudplibData.Build()
            End If
        End If


        Return True
    End Function
End Class
