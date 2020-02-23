Imports System.IO
Imports System.Net
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
        WriteSCADataFile()
    End Sub


    Private cookie As CookieContainer
    Public Function httpRequest(filename As String, senddata As String) As String
        Dim url As String = "https://scarchive.kr/eudeditor/" & filename & ".php"

        Dim strResult As String = ""
        Dim req As HttpWebRequest = WebRequest.Create(url)
        req.Method = "POST"
        req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
        req.ContentLength = senddata.Length
        req.KeepAlive = True
        req.CookieContainer = cookie

        Dim sw As StreamWriter = New StreamWriter(req.GetRequestStream())
        sw.Write(senddata)
        sw.Close()

        Dim result As HttpWebResponse = req.GetResponse()

        If result.StatusCode = HttpStatusCode.OK Then
            Dim strReceiveStream As Stream = result.GetResponseStream()
            Dim reqStreamReader As StreamReader = New StreamReader(strReceiveStream, Text.Encoding.UTF8)

            strResult = reqStreamReader.ReadToEnd()

            req.Abort()
            strReceiveStream.Close()
            reqStreamReader.Close()

        Else
            strResult = "ERROR"
        End If

        Return strResult
    End Function
    Private Function ConnecterStart() As Boolean

        '$mapcode = $_POST['mapcode'];
        '$bt = $_POST['bt'];
        '$pw = $_POST['pw'];
        '$mapname = $_POST['mapname'];
        '$email = $_POST['email'];
        '$maplink = $_POST['maplink'];
        '$mapinfor = $_POST['mapinfor'];

        Dim senddata As String = ""
        senddata = "mapcode=" & WebUtility.UrlEncode(GetMapCode()) & "&"
        senddata = senddata & "subtitle=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.SubTitle) & "&"
        senddata = senddata & "bt=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.MakerBattleTag) & "&"
        senddata = senddata & "pw=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.PassWord) & "&"

        Dim tagsdata As String = ""
        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            If i <> 0 Then
                tagsdata = tagsdata & ","
            End If
            tagsdata = tagsdata & pjData.TEData.SCArchive.CodeDatas(i).TagName
        Next
        senddata = senddata & "vtaglist=" & WebUtility.UrlEncode(tagsdata) & "&"

        If pjData.TEData.SCArchive.updateinfo Then
            senddata = senddata & "mapname=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.MapTitle) & "&"
            senddata = senddata & "email=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.MakerEmail) & "&"
            senddata = senddata & "maplink=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.DownLink) & "&"
            senddata = senddata & "imglink=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.ImageLink) & "&"
            senddata = senddata & "mapinfor=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.MapDes) & "&"
            senddata = senddata & "viewpublic=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.ViewPublic) & "&"
            senddata = senddata & "maptag=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.MapTags) & "&"
            senddata = senddata & "isupdate=True"
        Else
            senddata = senddata & "isupdate=False"
        End If
        'MsgBox(senddata)


        Dim respon As String = httpRequest("registermap", senddata)

        Select Case respon
            Case "NOACCOUNT"
                MsgBox(Tool.GetText("Error SCA") & vbCrLf & "계정 정보가 올바르지 않습니다.", MsgBoxStyle.Critical)
                Return False
            Case "BANUSER"
                MsgBox(Tool.GetText("Error SCA") & vbCrLf & "SCA사용이 금지된 아이디입니다.", MsgBoxStyle.Critical)
                Return False
        End Select


        ConnectKey = respon

        Return True
    End Function


    Public Function GetSCAMainEps() As String
        Return My.Computer.FileSystem.ReadAllText(GetSCArchiveeps)
    End Function



    Public Function GetSCAEps() As String
        'Const CommandLength As Integer = 12
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
        sb.AppendLine("    MPQAddFile('SCARCHIVEMAPCODE', py_open('scakeyfile', 'rb').read());")
        sb.AppendLine("    MPQAddFile('SCARCHIVEDATA', py_open('scadatafile', 'rb').read());")
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
        sb.AppendLine("    const cp = getcurpl();")
        sb.AppendLine("    switch(tagNum){")

        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            sb.AppendLine("    case " & i & ":")
            Select Case pjData.TEData.SCArchive.CodeDatas(i).TypeIndex
                Case StarCraftArchive.CodeData.CodeType.Variable
                    '변수
                    Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                    names = names.Split(".").First
                    names = names.Replace("\", ".")
                    sb.AppendLine("        n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName & "[cp] = 0;")
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
                    sb.AppendLine("            " & arrayname & "[alen * cp + i] = 0;")
                    sb.AppendLine("        }")

                    sb.AppendLine("        break;")
            End Select

        Next

        sb.AppendLine("    }")
        sb.AppendLine("}")

        sb.AppendLine("")
        sb.AppendLine("")
        sb.AppendLine("function SaveValue(tagNum, Value, index){")
        sb.AppendLine("    const cp = getcurpl();")



        sb.AppendLine("    switch(tagNum){")

        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            sb.AppendLine("    case " & i & ":")
            Select Case pjData.TEData.SCArchive.CodeDatas(i).TypeIndex
                Case StarCraftArchive.CodeData.CodeType.Variable
                    '변수
                    Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                    names = names.Split(".").First
                    names = names.Replace("\", ".")
                    sb.AppendLine("        n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName & "[cp] = Value;")
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
                    sb.AppendLine("        " & arrayname & "[alen * cp + index] = Value;")
                    sb.AppendLine("        break;")
            End Select

        Next

        sb.AppendLine("    }")
        sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("")
        '저장시 메모리에 tagNum을 적는다. 리턴값으로는 전진한 만큼을 기록한다.
        sb.AppendLine("function SaveDataWriteValue(tagNum, BaseAddress, index){")
        sb.AppendLine("    const cp = getcurpl();")
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

                    sb.AppendLine("        const objValue = n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName & "[cp];")
                Case StarCraftArchive.CodeData.CodeType.Deaths
                    '데스값
                    sb.AppendLine("        const objValue = dwread_epd(" & pjData.TEData.SCArchive.CodeDatas(i).ValueIndex & " * 12 + cp);")
                Case StarCraftArchive.CodeData.CodeType.Array
                    '배열
                    Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                    names = names.Split(".").First
                    names = names.Replace("\", ".")

                    Dim arrayname As String = "n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName

                    sb.AppendLine("        const alen = " & arrayname & ".length / 8;")
                    sb.AppendLine("        for(var i = 0 ; i < alen ; i ++){")
                    sb.AppendLine("            const objValue = " & arrayname & "[alen * cp + i];")
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
        'MsgBox(MakerBattleTag & MakerServerName & MapName)
        Return GetMd5HashBase64(MakerBattleTag & MakerServerName & MapName)
    End Function


    Public Function WriteSCADataFile() As Boolean
        'Dim hmpq As UInteger
        'Dim hfile As UInteger
        'Dim buffer() As Byte
        'Dim filesize As UInteger
        'Dim pdwread As IntPtr



        'Dim openFilename As String = "staredit\scenario.chk"


        'System.Threading.Thread.Sleep(1000)


        'StormLib.SFileOpenArchive(pjData.SaveMapName, 0, 0, hmpq)
        'If hmpq = 0 Then
        'Return False
        'End If
        'StormLib.SFileOpenFileEx(hmpq, openFilename, 0, hfile)
        'filesize = StormLib.SFileGetFileSize(hfile, filesize)
        'ReDim buffer(filesize)
        'MsgBox("파일 크기 : " & filesize)
        'StormLib.SFileReadFile(hfile, buffer, filesize, pdwread, 0)

        'Dim crc32 As New CRC32
        'Dim checksumv As UInteger = crc32.GetCRC32(buffer)
        'MsgBox("체크섬 값 : " & checksumv)



        Dim Sb As New StringBuilder
        Sb.Append("SCArchive by BingSu")
        Sb.Append("!")
        Sb.Append(pjData.TEData.SCArchive.DataSpace)
        Sb.Append("!")

        Dim EntryStr As String = ""
        For i = 0 To EntryPoint.Count - 1
            If i = 0 Then
                EntryStr = EntryPoint(i)
            Else
                EntryStr = EntryStr & "," & EntryPoint(i)
            End If
        Next


        Sb.Append(EntryStr)
        Sb.Append("!")



        Dim KeyHeaderStr As String = ""
        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            If i = 0 Then
                KeyHeaderStr = pjData.TEData.SCArchive.CodeDatas(i).TagName
            Else
                KeyHeaderStr = KeyHeaderStr & "," & pjData.TEData.SCArchive.CodeDatas(i).TagName
            End If
        Next

        Sb.Append(KeyHeaderStr)
        Sb.Append("!")
        Sb.Append(pjData.TEData.SCArchive.TestMode)





        Dim fs As New FileStream(EudPlibFilePath & "\scadatafile", FileMode.Create)
        Dim sw As New StreamWriter(fs)

        Dim checkstring As String = httpRequest("encrypt", "key=" & WebUtility.UrlEncode(ConnectKey) & "&data=" & WebUtility.UrlEncode(Sb.ToString))
        sw.Write(checkstring)


        'sw.Write(AESModule.EncryptString128Bit(Sb.ToString, ConnectKey))

        '2데이터태그 실제데이터위치 배열
        '엔트리포인트(현재 시간과 제작코드 등을 이용해 만들기, 빌드 시 마다 달라짐)
        'chk값

        'ConnectKey를 이용해 파일 암호화

        sw.Close()
        fs.Close()


        'StormLib.SFileAddFile(hmpq, TempFilePath & "\scadatafile", "SCARCHIVEDATA", StormLib.MPQ_FILE_COMPRESS)

        'StormLib.SFileAddFile(hmpq, EudPlibFilePath & "\scakeyfile", "SCARCHIVEMAPCODE", StormLib.MPQ_FILE_COMPRESS)



        'StormLib.SFileCloseFile(hfile)
        'StormLib.SFileCloseArchive(hmpq)




        Return True
        'MsgBox("완료")
    End Function
End Class
