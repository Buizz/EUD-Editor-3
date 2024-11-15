﻿Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Partial Public Class ProjectData
    '여기에 모든게 들어간다
    '스타 dat데이터를 클래스로 만들어 관리하자.
    Public Shared Sub Load(isNewfile As Boolean, ByRef _pjdata As ProjectData)
        If isNewfile Then
            _pjdata = New ProjectData
            _pjdata.NewFIle()
            _pjdata.InitData()
            _pjdata.InitSetting()
        Else
            Dim tFilename As String
            Dim LoadProjectDialog As New System.Windows.Forms.OpenFileDialog
            LoadProjectDialog.Filter = Tool.GetText("LoadFliter")

            LoadProjectDialog.InitialDirectory = pgData.Setting(ProgramData.TSetting.OpenPath)
            If LoadProjectDialog.ShowDialog() = Forms.DialogResult.OK Then
                tFilename = LoadProjectDialog.FileName '파일 이름 교체
                pgData.Setting(ProgramData.TSetting.OpenPath) = Path.GetDirectoryName(tFilename)
            Else
                Exit Sub
            End If

            If Tool.IsProjectLoad() Then
                '꺼야됨
                If Not _pjdata.CloseFile Then
                    Exit Sub
                End If
            End If

            Load(tFilename, _pjdata)
        End If
    End Sub

    Public Shared Sub LoadWithCheckOepn(tFilename As String, ByRef _pjdata As ProjectData)
        If Tool.IsProjectLoad() Then
            '꺼야됨
            If Not _pjdata.CloseFile Then
                Exit Sub
            End If
        End If

        Load(tFilename, _pjdata)
    End Sub

    Private Shared Sub TeFileRefresh(teFile As TEFile)
        For i = 0 To teFile.FileCount - 1
            teFile.Files(i).LoadInit()
        Next
        For i = 0 To teFile.FolderCount - 1
            teFile.Folders(i).LoadInit()
        Next
    End Sub
    Public Shared Sub Load(DataPath As String, ByRef _pjdata As ProjectData, Optional FilePath As String = "")
        If FilePath = "" Then
            FilePath = DataPath
        End If

        Tool.AddRecentFile(FilePath)

        Dim stm As Stream = System.IO.File.Open(DataPath, FileMode.Open, FileAccess.Read)

        Try
            If FilePath.Split(".").Last = "e3s" Then
                Dim bf As BinaryFormatter = New BinaryFormatter()
                _pjdata = New ProjectData
                _pjdata.NewFIle()
                _pjdata.InitData()
                _pjdata.SaveData = bf.Deserialize(stm)
                _pjdata.LoadInit(FilePath)
                _pjdata.Legacy()
                stm.Close()

                Dim mainTEFile As TEFile = _pjdata.TEData.PFIles
                TeFileRefresh(mainTEFile)

                'If _pjdata.SaveData.LastVersion.ToString <> pgData.Version.ToString Then
                '    Tool.ErrorMsgBox("테스트 버전은 다른 버전의 세이브 파일을 열 수 없습니다")
                '    pjData.CloseFile()
                'End If
            Else
                stm.Close()

                Tool.CustomMsgBox("호환성 경고" & vbCrLf &
                       "e2s파일을 불러올 경우 일부 에러가 발생할 수 있습니다." & vbCrLf &
                       "플러그인 로드 안됨" & vbCrLf &
                       "TE로드 안됨", MessageBoxButton.OK, MessageBoxImage.Exclamation)

                _pjdata = New ProjectData
                _pjdata.NewFIle()
                _pjdata.InitData()
                _pjdata.LoadInit(FilePath)

                Lagacy.LagacySaveLoad.Load(DataPath)
            End If
        Catch ex As Exception
            'Tool.ErrorMsgBox(Tool.GetText("Error SaveFileOpen"), ex.ToString)

            _pjdata.LoadInit(FilePath)
            Dim BackUpWindows As New BackUpWindows()
            BackUpWindows.ShowDialog()
            stm.Close()
            pjData.CloseFile()
            If BackUpWindows.IsOkay Then
                Load(BackUpWindows.SelectBackFile, pjData, FilePath)
            End If
        End Try

    End Sub
End Class
