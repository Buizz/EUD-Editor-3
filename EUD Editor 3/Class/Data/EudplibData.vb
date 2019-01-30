Imports System.IO
Imports System.Media
Imports System.Text

Public Class EudplibData
    Public ReadOnly Property EudplibFloder() As String
        Get
            If Not My.Computer.FileSystem.DirectoryExists(System.AppDomain.CurrentDomain.BaseDirectory & "Data\eudplibData") Then
                My.Computer.FileSystem.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory & "Data\eudplibData")
            End If

            Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\eudplibData"
        End Get
    End Property

    '1. 일단 내장된 파일을 빌드할때 뺴내는 형식에서
    '파일을 내장하지 않고 외부에 빼놓는다
    '2. 외부에 빼놓은 파일을 사용하지 않고 플립설치폴더나 직접 위치를 지정해 해당 플러그인을 기본으로 사용가능

    '3. 임시파일을 어디다가 생성할지 결정, 폴더가 없을 경우 기본 폴더에 저장
    '맵 파일 위치와 동일한 부분으로 설정 할 경우 맵 파일들이 자동으로 상대경로로 저장!
    Private MainThread As Threading.Thread
    Public Sub Build()
        MainThread = New System.Threading.Thread(AddressOf BuildProgress)
        MainThread.Start()
    End Sub

    Private Sub BuildProgress()
        '일단 프로그램 못끄게 막고 빌드중이라는 문구와 함께 진행 바 넣어야됨
        pgData.IsCompilng = True
        Tool.RefreshMainWindow()

        '각각의 임시파일들을 만들어야함



        'Dat설정 파일을 저장하는 py제작

        'TE관련 삽입
        'MSQC, 채팅인식, 언리미터, AI스크립트교체, CT

        'eds파일을 만들고 해당 파일 실행
        WriteedsFile()
        Starteds()

        Threading.Thread.Sleep(500)







        pgData.IsCompilng = False
        Tool.RefreshMainWindow()

        Dim notificationSound As New SoundPlayer(My.Resources.success)
        notificationSound.PlaySync()
    End Sub


    Private Sub WriteedsFile()
        Dim sb As New StringBuilder

        sb.AppendLine("[main]")
        sb.AppendLine("input: " & pjData.OpenMapName)
        sb.AppendLine("output: " & pjData.SaveMapName)

        '[EUDEditor.py]

        '[TriggerEditor.eps]

        '[dataDumper]
        'D\:\source\repos\EUDEditor\EUD Editor\bin\x86\Release\Data\temp\RequireData : 0x58D740, copy

        Dim filestreama As New FileStream(EudplibFloder & "\EUDEditor.eds", FileMode.Create)
        Dim strWriter As New StreamWriter(filestreama)

        strWriter.Write(sb.ToString)

        strWriter.Close()
        filestreama.Close()
    End Sub



    Private Sub Starteds()
        Dim process As New Process
        Dim startInfo As New ProcessStartInfo

        Dim filename As String = EudplibFloder & "\EUDEditor.eds"

        startInfo.FileName = pgData.Setting(ProgramData.TSetting.euddraft)
        startInfo.Arguments = """" & filename & """"

        startInfo.RedirectStandardOutput = True
        startInfo.RedirectStandardInput = True
        startInfo.WindowStyle = ProcessWindowStyle.Hidden
        startInfo.CreateNoWindow = True

        startInfo.UseShellExecute = False

        process.StartInfo = startInfo
        process.Start()
        process.WaitForExit()
    End Sub


End Class
