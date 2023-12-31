Imports System.IO
Imports System.Text

Partial Public Class BuildData
    Public Sub WriteBGMData()

        Dim fs As New FileStream(TriggerEditorPath & "\BGMFlexible.eps", FileMode.Create)
        Dim sw As New StreamWriter(fs)

        sw.Write(GetBGMEps)

        sw.Close()
        fs.Close()
    End Sub

    Private Function GetSoundIndex(index As Integer, Optional IsValue As Boolean = False) As String
        Dim hexd As String = Hex(index).ToUpper

        hexd = hexd.PadLeft(4, "0")
        Dim rv As String = ""

        For i = 3 To 0 Step -1
            Dim Asci As Integer = Asc(hexd(i))
            Dim num As Integer

            If Asci > 60 Then
                num = Asci - 55
            Else
                num = Asci - 48
            End If

            If IsValue Then
                rv = rv & Hex(num + &H40).ToUpper
            Else
                rv = Convert.ToChar(num + &H40) & rv
            End If

        Next
        If IsValue Then
            rv = "0x" & rv
        End If

        Return rv
    End Function


    Public Function GetBGMEps() As String
        Dim sb As New StringBuilder

        sb.Append("const bgmlen = [")
        For i = 0 To pjData.TEData.BGMData.BGMList.Count - 1
            If i <> 0 Then
                sb.Append(", ")
            End If
            Dim bgmfile As BGMData.BGMFile = pjData.TEData.BGMData.BGMList(i)
            sb.Append(GetSoundIndex(bgmfile.BGMBlockCount, True))
        Next

        sb.AppendLine("];")
        sb.AppendLine("const str = StringBuffer();")
        sb.AppendLine("function PlayOGG(bgmindex ,bgmcode, track){
    SetMemoryEPD(str.epd, SetTo, bgmcode);
    SetMemoryEPD(str.epd + 1, SetTo, track);
    str.Play();

    if (MemoryEPD(EPD(bgmlen) + bgmindex, Exactly, track)){
        return 0;
    }else{
        return 1;
    }
}")

        sb.AppendLine("function loadSound(){")

        sb.AppendLine("EUDPlayerLoop()();")
        sb.AppendLine("    str.insert(0, '');")
        sb.AppendLine("    str.append('@@@@@@@@');")
        sb.AppendLine("EUDEndPlayerLoop();")


        For i = 0 To pjData.TEData.BGMData.BGMList.Count - 1
            Dim bgmfile As BGMData.BGMFile = pjData.TEData.BGMData.BGMList(i)
            Dim folderPath As String = BuildData.SoundFilePath() & "\" & bgmfile.BGMName
            Dim output As String = folderPath & "\st"

            For Each files As String In My.Computer.FileSystem.GetFiles(folderPath)
                Dim rfilename As String = files.Split("\").Last
                If rfilename.IndexOf("st") <> -1 Then
                    Dim c As Integer = rfilename.Replace("st", "").Replace(".ogg", "")

                    Dim hname As String = GetSoundIndex(i)
                    Dim tname As String = GetSoundIndex(c)


                    sb.AppendLine("    MPQAddFile('" & hname & tname & "', py_open('" & files.Replace("\", "/") & "', 'rb').read());")
                End If
            Next

        Next

        sb.AppendLine("}")




        Return sb.ToString
    End Function
    Public Function SoundConverter() As Boolean
        Dim BGMWindow As New BGMPlayerConverter
        BGMWindow.WorkListRefresh(pjData.TEData.BGMData.BGMList, False)
        BGMWindow.ShowDialog()

        Return BGMWindow.isSuccess
    End Function
    Public Function SoundSCAScriptConverter() As Boolean
        Dim BGMWindow As New BGMPlayerConverter
        BGMWindow.WorkListRefresh(pjData.TEData.BGMData.SCABGMList, True)
        BGMWindow.ShowDialog()

        Return BGMWindow.isSuccess
    End Function
End Class
