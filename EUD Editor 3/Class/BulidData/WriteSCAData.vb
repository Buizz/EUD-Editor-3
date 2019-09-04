Imports System.IO
Imports System.Text

Partial Public Class BuildData
    Private ConnectKey As String
    Private EntryPoint() As UInteger


    Public Sub GetEntryPoint()
        Dim MakerBattleTag As String = pjData.TEData.SCArchive.MakerBattleTag
        Dim MakerServerName As String = pjData.TEData.SCArchive.MakerServerName
        Dim MapName As String = pjData.TEData.SCArchive.MapName

        Dim rnd As New Random()
        ReDim EntryPoint(7)
        For i = 0 To 7
            EntryPoint(i) = CUInt(rnd.Next) + CUInt(rnd.Next)
        Next



        'EntryPoint = GetMapCode() & Now.Ticks
        'Return GetMd5HashBase64(MakerBattleTag & MakerServerName & MapName)
    End Sub
    Private Sub WriteSCAScript()
        Dim fs As New FileStream(EudPlibFilePath & "\scakeyfile", FileMode.Create)
        Dim sw As New StreamWriter(fs)
        sw.Write(GetMapCode)
        sw.Close()
        fs.Close()


        'fs = New FileStream(filePath & "\SCArchive.eps", FileMode.Create)
        fs = New FileStream(TriggerEditorPath & "\SCAFlexible.eps", FileMode.Create)
        sw = New StreamWriter(fs)

        sw.Write(GetSCAEps)

        sw.Close()
        fs.Close()

        My.Computer.FileSystem.CopyFile(GetSCArchiveeps, TriggerEditorPath & "\SCArchive.eps", True)
        My.Computer.FileSystem.CopyFile(GetSCATooleps, TriggerEditorPath & "\SCATool.eps", True)



        'fs = New FileStream(TriggerEditorPath & "\SCArchive.eps", FileMode.Create)
        'sw = New StreamWriter(fs)

        'sw.Write(GetSCAToolEps)

        'sw.Close()
        'fs.Close()
    End Sub



    Private Function ConnecterStart() As Boolean
        'Private EntryPoint As String
        'Private ConnectKey As String


        Dim process As New Process
        Dim startInfo As New ProcessStartInfo

        startInfo.FileName = GetConnectPath
        startInfo.Arguments = pjData.TEData.SCArchive.MakerEmail & " " & GetMapCode() & " " & pjData.TEData.SCArchive.MakerBattleTag & " " & pjData.TEData.SCArchive.PassWord


        'startInfo.StandardOutputEncoding = Text.Encoding.UTF32
        'startInfo.StandardErrorEncoding = Text.Encoding.UTF32
        startInfo.RedirectStandardOutput = True
        startInfo.RedirectStandardInput = True
        startInfo.RedirectStandardError = True
        startInfo.WindowStyle = ProcessWindowStyle.Hidden
        startInfo.CreateNoWindow = True

        startInfo.UseShellExecute = False


        process.StartInfo = startInfo
        Try
            process.Start()
        Catch ex As Exception
            MsgBox(Tool.GetText("Error SCA") & vbCrLf & "SCArchiveConnecter.exe파일을 찾을 수 없습니다.", MsgBoxStyle.Critical)
            Return False
        End Try
        Dim OutputString As String = ""
        While (Not process.HasExited)
            OutputString = process.StandardOutput.ReadLine
            If OutputString <> "" Then
                ConnectKey = OutputString.Trim
                Threading.Thread.Sleep(100)
                If Not process.HasExited Then
                    process.Kill()
                End If

            End If
        End While

        'process.WaitForExit()
        'OutputString = process.StandardOutput.ReadToEnd

        'ConnectKey = OutputString.Trim

        Select Case ConnectKey
            Case "BTERROR"
                MsgBox(Tool.GetText("Error SCA") & vbCrLf & "존재하지 않는 배틀태그 입니다.", MsgBoxStyle.Critical)
                Return False
            Case "ERROR"
                MsgBox(Tool.GetText("Error SCA") & vbCrLf & Tool.GetText("Error SCAUnKnow"), MsgBoxStyle.Critical)
                Return False
            Case "PWERROR"
                MsgBox(Tool.GetText("Error SCA") & vbCrLf & Tool.GetText("Error SCAPassWord"), MsgBoxStyle.Critical)
                Return False
            Case "DBERROR"
                MsgBox(Tool.GetText("Error SCA") & vbCrLf & Tool.GetText("Error SCANresponDB"), MsgBoxStyle.Critical)
                Return False
            Case "FALGERROR"
                MsgBox(Tool.GetText("Error SCA") & vbCrLf & "SCA사용이 금지된 아이디입니다.", MsgBoxStyle.Critical)
                Return False
        End Select


        Return True
    End Function


    Public Function GetSCAMainEps() As String
        Return My.Computer.FileSystem.ReadAllText(GetSCArchiveeps)
    End Function



    Public Function GetSCAEps() As String
        Const CommandLength As Integer = 12
        Dim SpaceLength As Integer = pjData.TEData.SCArchive.DataSpace


        Dim sb As New StringBuilder

        Dim NameSapces As New List(Of String)
        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            If pjData.TEData.SCArchive.CodeDatas(i).TypeIndex = 0 Then
                '변수
                Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                names = names.Split(".").First
                names = names.Replace("\", ".")

                If NameSapces.IndexOf(names) = -1 Then
                    NameSapces.Add(names)
                End If
            End If
        Next

        For i = 0 To NameSapces.Count - 1
            sb.AppendLine("import " & NameSapces(i) & " as n" & i & ";")
        Next
        sb.AppendLine("")
        sb.AppendLine("const ws = 0x58F44A;") 'Db(" & EntryPoint.Count * 4 + 8 * (CommandLength + SpaceLength) & ");//workspace")
        sb.AppendLine("const EntryPointLength = " & EntryPoint.Count & ";//EntryPointLength")
        sb.AppendLine("const SpaceLength = " & pjData.TEData.SCArchive.DataSpace & ";//DataBufferSize")
        sb.AppendLine("const ObjectCount = " & pjData.TEData.SCArchive.CodeDatas.Count & ";//ObjectCount")

        sb.AppendLine("")
        sb.AppendLine("function Init(){")
        'sb.AppendLine("    MPQAddFile('SCARCHIVEMAPCODE', py_open('scakeyfile', 'rb').read());")
        'sb.AppendLine("f_eprintln(""시발 오류"", dwread_epd(EPD(ws)), ""  "",  dwread_epd(EPD(ws) + 1));")
        sb.AppendLine("    //EntryPoint")
        For i = 0 To EntryPoint.Count - 1
            'sb.AppendLine("    dwwrite_epd(EPD(ws) + " & i & ", " & 98761234 & ");")
            sb.AppendLine("    dwwrite_epd(EPD(ws) + " & i & ", " & EntryPoint(i) & ");")
        Next

        sb.AppendLine("    ")
        sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("")
        sb.AppendLine("function Exec(){")
        sb.AppendLine("    Init();")

        'sb.AppendLine("    const s = StringBuffer();")
        'sb.AppendLine("    s.print(""asf"", dwread_epd(EPD(ws) + EntryPointLength));")
        sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("")

        sb.AppendLine("function ResetValue(tagNum, index){")
        sb.AppendLine("    switch(tagNum){")

        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            sb.AppendLine("    case " & i & ":")
            Select Case pjData.TEData.SCArchive.CodeDatas(i).TypeIndex
                Case StarCraftArchive.CodeData.CodeType.Variable
                    '변수
                    Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                    names = names.Split(".").First
                    names = names.Replace("\", ".")
                    sb.AppendLine("        n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName & "[getcurpl()] = 0;")
                    sb.AppendLine("        break;")
                Case StarCraftArchive.CodeData.CodeType.Deaths
                    '데스값 p, Setto, 0, Unit
                    sb.AppendLine("        SetDeaths(CurrentPlayer, SetTo, 0, " & pjData.TEData.SCArchive.CodeDatas(i).ValueIndex & ");")
                    sb.AppendLine("        break;")
                Case StarCraftArchive.CodeData.CodeType.Array
                    '배열
                    Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                    names = names.Split(".").First
                    names = names.Replace("\", ".")

                    Dim arrayname As String = "n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName

                    sb.AppendLine("        const alen = " & arrayname & ".length / 8;")
                    sb.AppendLine("        for(var i = 0 ; i < alen; i++){")
                    sb.AppendLine("            " & arrayname & "[alen * getcurpl() + i] = 0;")
                    sb.AppendLine("        }")

                    sb.AppendLine("        break;")
            End Select

        Next

        sb.AppendLine("    }")
        sb.AppendLine("}")

        'sb.AppendLine("")
        'sb.AppendLine("")
        'sb.AppendLine("function LoadValue(tagNum, index){")
        'sb.AppendLine("    switch(tagNum){")

        'For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
        '    sb.AppendLine("    case " & i & ":")


        '    Select Case pjData.TEData.SCArchive.CodeDatas(i).TypeIndex
        '        Case StarCraftArchive.CodeData.CodeType.Variable
        '            '변수
        '            Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
        '            names = names.Split(".").First
        '            names = names.Replace("\", ".")

        '            sb.AppendLine("        return n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName & "[getcurpl()];")
        '        Case StarCraftArchive.CodeData.CodeType.Deaths
        '            '데스값
        '            sb.AppendLine("        return dwread_epd(" & pjData.TEData.SCArchive.CodeDatas(i).ValueIndex & " * 12 + getcurpl());")
        '        Case StarCraftArchive.CodeData.CodeType.Array
        '            '배열
        '            Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
        '            names = names.Split(".").First
        '            names = names.Replace("\", ".")

        '            Dim arrayname As String = "n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName

        '            sb.AppendLine("        const alen = " & arrayname & ".length / 8;")
        '            sb.AppendLine("        return " & arrayname & "[alen * getcurpl() + index];")
        '    End Select
        'Next

        'sb.AppendLine("    }")
        'sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("")
        sb.AppendLine("function SaveValue(tagNum, Value, index){")
        sb.AppendLine("    switch(tagNum){")

        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            sb.AppendLine("    case " & i & ":")



            Select Case pjData.TEData.SCArchive.CodeDatas(i).TypeIndex
                Case StarCraftArchive.CodeData.CodeType.Variable
                    '변수
                    Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                    names = names.Split(".").First
                    names = names.Replace("\", ".")
                    sb.AppendLine("        n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName & "[getcurpl()] = Value;")
                    sb.AppendLine("        break;")
                Case StarCraftArchive.CodeData.CodeType.Deaths
                    '데스값 p, Setto, 0, Unit
                    sb.AppendLine("        SetDeaths(CurrentPlayer, SetTo, Value, " & pjData.TEData.SCArchive.CodeDatas(i).ValueIndex & ");")
                    sb.AppendLine("        break;")
                Case StarCraftArchive.CodeData.CodeType.Array
                    '배열
                    Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                    names = names.Split(".").First
                    names = names.Replace("\", ".")

                    Dim arrayname As String = "n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName

                    sb.AppendLine("        const alen = " & arrayname & ".length / 8;")
                    sb.AppendLine("        " & arrayname & "[alen * getcurpl() + index] = Value;")
                    sb.AppendLine("        break;")
            End Select

        Next

        sb.AppendLine("    }")
        sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("")
        '저장시 메모리에 tagNum을 적는다. 리턴값으로는 전진한 만큼을 기록한다.
        sb.AppendLine("function SaveDataWriteValue(tagNum, BaseAddress, index){")
        sb.AppendLine("    switch(tagNum){")

        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            sb.AppendLine("    case " & i & ":")
            sb.AppendLine("        {")


            Select Case pjData.TEData.SCArchive.CodeDatas(i).TypeIndex
                Case StarCraftArchive.CodeData.CodeType.Variable
                    '변수
                    Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                    names = names.Split(".").First
                    names = names.Replace("\", ".")

                    sb.AppendLine("        const objValue = n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName & "[getcurpl()];")
                Case StarCraftArchive.CodeData.CodeType.Deaths
                    '데스값
                    sb.AppendLine("        const objValue = dwread_epd(" & pjData.TEData.SCArchive.CodeDatas(i).ValueIndex & " * 12 + getcurpl());")
                Case StarCraftArchive.CodeData.CodeType.Array
                    '배열
                    Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                    names = names.Split(".").First
                    names = names.Replace("\", ".")

                    Dim arrayname As String = "n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName

                    sb.AppendLine("        const alen = " & arrayname & ".length / 8;")
                    sb.AppendLine("        for(var i = 0 ; i < alen ; i ++){")
                    sb.AppendLine("            const objValue = " & arrayname & "[alen * getcurpl() + i];")
                    sb.AppendLine("            if(objValue != 0){")
                    sb.AppendLine("                if (objValue > 0xFFFF){")
                    sb.AppendLine("                    wwrite_epd(BaseAddress + index / 2, (index % 2) * 2, 0x3000 + tagNum);")
                    sb.AppendLine("                    index++;")
                    sb.AppendLine("                    wwrite_epd(BaseAddress + index / 2, (index % 2) * 2, i);")
                    sb.AppendLine("                    index++;")
                    sb.AppendLine("                    wwrite_epd(BaseAddress + index / 2, (index % 2) * 2, objValue / 0x10000);")
                    sb.AppendLine("                    index++;")
                    sb.AppendLine("                    wwrite_epd(BaseAddress + index / 2, (index % 2) * 2, objValue % 0x10000);")
                    sb.AppendLine("                    index++;")
                    sb.AppendLine("                 }else{")
                    sb.AppendLine("                    wwrite_epd(BaseAddress + index / 2, (index % 2) * 2, 0x4000 + tagNum);")
                    sb.AppendLine("                    index++;")
                    sb.AppendLine("                    wwrite_epd(BaseAddress + index / 2, (index % 2) * 2, i);")
                    sb.AppendLine("                    index++;")
                    sb.AppendLine("                    wwrite_epd(BaseAddress + index / 2, (index % 2) * 2, objValue);")
                    sb.AppendLine("                    index++;")
                    sb.AppendLine("                 }")
                    sb.AppendLine("            }")
                    sb.AppendLine("        }")
            End Select

            Select Case pjData.TEData.SCArchive.CodeDatas(i).TypeIndex
                Case StarCraftArchive.CodeData.CodeType.Variable, StarCraftArchive.CodeData.CodeType.Deaths
                    sb.AppendLine("        if(objValue != 0){")
                    sb.AppendLine("            if (objValue > 0xFFFF){")
                    sb.AppendLine("                wwrite_epd(BaseAddress + index / 2, (index % 2) * 2, 0x1000 + tagNum);")
                    sb.AppendLine("                index++;")
                    sb.AppendLine("                wwrite_epd(BaseAddress + index / 2, (index % 2) * 2, objValue / 0x10000);")
                    sb.AppendLine("                index++;")
                    sb.AppendLine("                wwrite_epd(BaseAddress + index / 2, (index % 2) * 2, objValue % 0x10000);")
                    sb.AppendLine("                index++;")
                    sb.AppendLine("             }else{")
                    sb.AppendLine("                wwrite_epd(BaseAddress + index / 2, (index % 2) * 2, 0x2000 + tagNum);")
                    sb.AppendLine("                index++;")
                    sb.AppendLine("                wwrite_epd(BaseAddress + index / 2, (index % 2) * 2, objValue);")
                    sb.AppendLine("                index++;")
                    sb.AppendLine("             }")
                    sb.AppendLine("         }")
            End Select
            sb.AppendLine("        }")
            sb.AppendLine("        break;")
        Next
        sb.AppendLine("    }")
        sb.AppendLine("    ")
        sb.AppendLine("    return index;")
        sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("")
        '
        sb.AppendLine("function LoadDataReadValue(BaseAddress, i){")
        sb.AppendLine("   	const indicator = wread_epd(BaseAddress + i / 2, (i % 2) * 2);")
        sb.AppendLine("   	")
        sb.AppendLine("   	if (indicator != 0){")
        sb.AppendLine("   		const spec = indicator / 0x1000;")
        sb.AppendLine("   		const ObjNum = indicator % 0x1000;")
        sb.AppendLine("   		var vindex = 0;")
        sb.AppendLine("   		var value = 0;")
        sb.AppendLine("   		if (spec == 1){")
        sb.AppendLine("   			//4바이트 지정")
        sb.AppendLine("   			i++;")
        sb.AppendLine("   			const value1 = wread_epd(BaseAddress + i / 2, (i % 2) * 2);")
        sb.AppendLine("   			i++;")
        sb.AppendLine("   			const value2 = wread_epd(BaseAddress + i / 2, (i % 2) * 2);")
        sb.AppendLine("   			value = value1 * 0x10000 + value2;")
        sb.AppendLine("   		}else if (spec == 2){")
        sb.AppendLine("   			//2바이트 지정")
        sb.AppendLine("   			i++;")
        sb.AppendLine("   			value = wread_epd(BaseAddress + i / 2, (i % 2) * 2);")
        sb.AppendLine("   		}else if (spec == 3){")
        sb.AppendLine("   			//4바이트 지정")
        sb.AppendLine("   			i++;")
        sb.AppendLine("   			vindex = wread_epd(BaseAddress + i / 2, (i % 2) * 2);")
        sb.AppendLine("   			i++;")
        sb.AppendLine("   			const value1 = wread_epd(BaseAddress + i / 2, (i % 2) * 2);")
        sb.AppendLine("   			i++;")
        sb.AppendLine("   			const value2 = wread_epd(BaseAddress + i / 2, (i % 2) * 2);")
        sb.AppendLine("   			value = value1 * 0x10000 + value2;")
        sb.AppendLine("   		}else if (spec == 4){")
        sb.AppendLine("   			//2바이트 지정")
        sb.AppendLine("   			i++;")
        sb.AppendLine("   			vindex = wread_epd(BaseAddress + i / 2, (i % 2) * 2);")
        sb.AppendLine("   			i++;")
        sb.AppendLine("   			value = wread_epd(BaseAddress + i / 2, (i % 2) * 2);")
        sb.AppendLine("   		}")
        sb.AppendLine("   		SaveValue(ObjNum, value, vindex);")
        sb.AppendLine("   	}")
        sb.AppendLine("   	return i;")
        sb.AppendLine("}")


        Return sb.ToString
    End Function




    Public Function GetMapCode() As String
        Dim MakerBattleTag As String = pjData.TEData.SCArchive.MakerBattleTag
        Dim MakerServerName As String = pjData.TEData.SCArchive.MakerServerName
        Dim MapName As String = pjData.TEData.SCArchive.MapName


        Return GetMd5HashBase64(MakerBattleTag & MakerServerName & MapName)
    End Function


    Public Function WriteSCADataFile() As Boolean
        Dim hmpq As UInteger
        Dim hfile As UInteger
        Dim buffer() As Byte
        Dim filesize As UInteger
        Dim pdwread As IntPtr



        Dim openFilename As String = "staredit\scenario.chk"


        System.Threading.Thread.Sleep(1000)


        StormLib.SFileOpenArchive(pjData.SaveMapName, 0, 0, hmpq)
        If hmpq = 0 Then
            Return False
        End If
        StormLib.SFileOpenFileEx(hmpq, openFilename, 0, hfile)
        filesize = StormLib.SFileGetFileSize(hfile, filesize)
        ReDim buffer(filesize)
        'MsgBox("파일 크기 : " & filesize)
        StormLib.SFileReadFile(hfile, buffer, filesize, pdwread, 0)

        Dim crc32 As New CRC32
        Dim checksumv As UInteger = crc32.GetCRC32(buffer)
        'MsgBox("체크섬 값 : " & checksumv)





        Dim Sb As New StringBuilder
        Sb.AppendLine(checksumv)
        Sb.AppendLine(pjData.TEData.SCArchive.DataSpace)

        Dim EntryStr As String = ""
        For i = 0 To EntryPoint.Count - 1
            If i = 0 Then
                EntryStr = EntryPoint(i)
            Else
                EntryStr = EntryStr & "," & EntryPoint(i)
            End If
        Next


        Sb.AppendLine(EntryStr)



        Dim KeyHeaderStr As String = ""
        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            If i = 0 Then
                KeyHeaderStr = pjData.TEData.SCArchive.CodeDatas(i).TagName
            Else
                KeyHeaderStr = KeyHeaderStr & "," & pjData.TEData.SCArchive.CodeDatas(i).TagName
            End If
        Next

        Sb.AppendLine(KeyHeaderStr)

        Sb.AppendLine(pjData.TEData.SCArchive.TestMode)









        Dim fs As New FileStream(TempFilePath & "\scadatafile", FileMode.Create)
        Dim sw As New StreamWriter(fs)

        sw.Write(AESModule.EncryptString128Bit(Sb.ToString, ConnectKey))




        '2데이터태그 실제데이터위치 배열
        '엔트리포인트(현재 시간과 제작코드 등을 이용해 만들기, 빌드 시 마다 달라짐)
        'chk값

        'ConnectKey를 이용해 파일 암호화

        sw.Close()
        fs.Close()


        StormLib.SFileAddFile(hmpq, TempFilePath & "\scadatafile", "SCARCHIVEDATA", StormLib.MPQ_FILE_COMPRESS)

        StormLib.SFileAddFile(hmpq, EudPlibFilePath & "\scakeyfile", "SCARCHIVEMAPCODE", StormLib.MPQ_FILE_COMPRESS)



        StormLib.SFileCloseFile(hfile)
        StormLib.SFileCloseArchive(hmpq)




        Return True
        'MsgBox("완료")
    End Function
End Class
