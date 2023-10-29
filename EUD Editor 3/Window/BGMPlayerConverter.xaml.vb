Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Threading

Public Class BGMPlayerConverter
    Public isSuccess As Boolean
    Private IsSCAScript As Boolean

    Private BGMWorker As BackgroundWorker

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        BGMWorker = New BackgroundWorker

        AddHandler BGMWorker.DoWork, AddressOf BGM_DoWork
        AddHandler BGMWorker.RunWorkerCompleted, AddressOf BGM_Complete

        BGMWorker.RunWorkerAsync()
    End Sub


    Private Sub LabelChange(text As String)
        Dispatcher.Invoke(DispatcherPriority.Normal, New Action(Sub()
                                                                    TLabel.Text = text
                                                                End Sub))
    End Sub
    Private Sub ProgressChange(v As Integer)
        Dispatcher.Invoke(DispatcherPriority.Normal, New Action(Sub()
                                                                    probar.Value = v
                                                                End Sub))
    End Sub

    Private Function BGMProcess(bgmdata As BGMData.BGMFile) As Boolean
        LabelChange(bgmdata.BGMPath & " 작업중 ...")

        If Not My.Computer.FileSystem.FileExists(bgmdata.BGMPath) Then
            Return False
        End If

        Dim sbgmCRC32V As UInteger = 0
        Dim sbgmblockCRC32V As UInteger = 0
        Dim sbgmblockCount As UInteger = 0
        Dim bgmCRC32V As UInteger = 0
        Dim bgmblockCRC32V As UInteger = 0
        Dim bgmblockCount As UInteger = 0


        Dim sbitrate As Integer = 0
        Dim ssamplerate As Integer = 0

        '세팅정보에는 음원파일의 CRC32가 들어있음
        Dim CRC32 As New CRC32
        bgmCRC32V = CRC32.GetCRC32FromFile(bgmdata.BGMPath)





        'BGM폴더 존재여부 확인
        Dim folderPath As String = BuildData.SoundFilePath() & "\" & bgmdata.BGMName
        Dim SettingPath As String = folderPath & "\bgm.ini"

        If Not My.Computer.FileSystem.DirectoryExists(folderPath) Then
            '폴더가 없음
            My.Computer.FileSystem.CreateDirectory(folderPath)
        End If



        '블럭 사운드 CRC32구하기
        For Each files As String In My.Computer.FileSystem.GetFiles(folderPath)
            Dim rfilename As String = files.Split("\").Last
            If rfilename.IndexOf("st") <> -1 Then
                bgmblockCRC32V = bgmblockCRC32V Xor CRC32.GetCRC32FromFile(files)

                Dim c As Integer = rfilename.Replace("st", "").Replace(".ogg", "")
                If bgmblockCount < c Then
                    bgmblockCount = c
                End If
            End If
        Next


        '세팅파일 구조
        'MainFileCRC32
        'SoundBlockCRC32 XorResult



        '세팅파일 확인
        If My.Computer.FileSystem.FileExists(SettingPath) Then
            '세팅파일 정보를 읽고 틀리면 새로 작성함
            Dim tfs As New FileStream(SettingPath, FileMode.Open)
            Dim sr As New StreamReader(tfs)

            sbgmCRC32V = sr.ReadLine
            sbgmblockCRC32V = sr.ReadLine
            sbgmblockCount = sr.ReadLine
            sbitrate = sr.ReadLine
            ssamplerate = sr.ReadLine

            sr.Close()
            tfs.Close()
        End If

        If sbgmCRC32V = bgmCRC32V And sbgmblockCRC32V = bgmblockCRC32V And sbgmblockCount = bgmblockCount And ssamplerate = bgmdata.BGMSampleRate And sbitrate = bgmdata.BGMBitRate Then
            bgmdata.BGMBlockCount = bgmblockCount
            Return True
        End If


        '폴더안에 파일 모두 삭제 후 작업시작
        For Each files As String In My.Computer.FileSystem.GetFiles(folderPath)
            My.Computer.FileSystem.DeleteFile(files)
        Next



        Dim ffmpegPath As String = Tool.DataPath("ffmpeg.exe")

        Dim openfile As String = bgmdata.BGMPath
        Dim output As String = folderPath & "\s"

        openfile = Chr(34) & openfile & Chr(34)
        Dim interval As Double = 2.28



        Dim prcFFMPEG As New Process
        Dim psiProcInfo As New ProcessStartInfo()
        psiProcInfo.FileName = ffmpegPath
        psiProcInfo.WindowStyle = ProcessWindowStyle.Hidden


        prcFFMPEG.StartInfo = psiProcInfo



        LabelChange(bgmdata.BGMPath & " wav로 변환 중 ...")
        ProgressChange(0)

        psiProcInfo.Arguments = "-i " & openfile & " -y " & Chr(34) & output & ".wav" & Chr(34)
        prcFFMPEG.Start()
        prcFFMPEG.WaitForExit()

        '====================================================================================================================================

        LabelChange(bgmdata.BGMPath & " ogg로 변환 중 ...")
        ProgressChange(25)

        psiProcInfo.Arguments = "-i " & Chr(34) & output & ".wav" & Chr(34) & " -y " & Chr(34) & output & ".ogg" & Chr(34)
        prcFFMPEG.Start()
        prcFFMPEG.WaitForExit()

        '====================================================================================================================================

        Dim bitrate As Integer = bgmdata.BGMBitRate
        Dim samplerate As Integer = bgmdata.BGMSampleRate

        If bitrate <> -1 Or samplerate <> -1 Then
            LabelChange(bgmdata.BGMPath & " ogg로 압축 중 ...")
            ProgressChange(50)

            Dim bitratestr As String = ""
            Dim sampleratestr As String = ""

            If bitrate <> -1 Then
                bitratestr = "-r " & bitrate * 1000
            End If
            If samplerate <> -1 Then
                sampleratestr = "-ar " & samplerate
            End If


            If bitrate <> -1 And samplerate <> -1 Then
                psiProcInfo.Arguments = "-i " & Chr(34) & output & ".ogg" & Chr(34) & " " & bitratestr & " " & sampleratestr & " -y " & Chr(34) & output & "low.ogg" & Chr(34)
            Else
                psiProcInfo.Arguments = "-i " & Chr(34) & output & ".ogg" & Chr(34) & " " & bitratestr & sampleratestr & " -y " & Chr(34) & output & "low.ogg" & Chr(34)
            End If


            prcFFMPEG.Start()
            prcFFMPEG.WaitForExit()
        End If


        '====================================================================================================================================

        LabelChange(bgmdata.BGMPath & " 파일 분활 중 ...")
        ProgressChange(75)

        If bitrate = -1 And samplerate = -1 Then
            psiProcInfo.Arguments = "-i " & Chr(34) & output & ".ogg" & Chr(34) & " -f segment -segment_time " & interval & " -y -c copy " & Chr(34) & output & "t%d" & ".ogg" & Chr(34)
        Else
            psiProcInfo.Arguments = "-i " & Chr(34) & output & "low.ogg" & Chr(34) & " -f segment -segment_time " & interval & " -y -c copy " & Chr(34) & output & "t%d" & ".ogg" & Chr(34)
        End If

        prcFFMPEG.Start()
        prcFFMPEG.WaitForExit()

        If bitrate = -1 And samplerate = -1 Then
            bgmdata.BGMCompressionSize = Tool.GetFileSize(output & ".ogg")
        Else
            bgmdata.BGMCompressionSize = Tool.GetFileSize(output & "low.ogg")
        End If
        ProgressChange(100)




        bgmblockCRC32V = 0
        bgmblockCount = 0
        '블럭 사운드 CRC32구하기
        For Each files As String In My.Computer.FileSystem.GetFiles(folderPath)
            Dim rfilename As String = files.Split("\").Last
            If rfilename.IndexOf("st") <> -1 Then
                bgmblockCRC32V = bgmblockCRC32V Xor CRC32.GetCRC32FromFile(files)

                Dim c As Integer = rfilename.Replace("st", "").Replace(".ogg", "")
                If bgmblockCount < c Then
                    bgmblockCount = c
                End If
            End If
        Next





        Dim fs As New FileStream(SettingPath, FileMode.Create)
        Dim sw As New StreamWriter(fs)

        sw.WriteLine(bgmCRC32V)
        sw.WriteLine(bgmblockCRC32V)
        sw.WriteLine(bgmblockCount)
        sw.WriteLine(bgmdata.BGMBitRate)
        sw.WriteLine(bgmdata.BGMSampleRate)

        sw.Close()
        fs.Close()



        Return True
    End Function


    Public WorkList As List(Of BGMData.BGMFile)
    Public Sub WorkListRefresh(_WorkList As List(Of BGMData.BGMFile), _IsSCAScript As Boolean)
        WorkList = _WorkList
        IsSCAScript = _IsSCAScript
    End Sub

    Private Sub BGM_DoWork(sender As Object, e As DoWorkEventArgs)
        For i = 0 To WorkList.Count - 1
            If Not BGMProcess(WorkList(i)) Then
                Throw New Exception(WorkList(i).BGMPath & " is fail!")
            End If
        Next
    End Sub
    Private Sub BGM_Complete(sender As Object, e As RunWorkerCompletedEventArgs)
        If e.Error IsNot Nothing Then
            Tool.ErrorMsgBox("BGM 변환 에러", e.Error.ToString)
            isSuccess = False
        Else
            isSuccess = True
        End If
        Me.Close()
    End Sub
End Class
