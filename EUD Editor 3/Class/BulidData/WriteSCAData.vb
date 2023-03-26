Imports System.IO
Imports System.Net
Imports System.Text

Partial Public Class BuildData
    Private ConnectKey As String
    Private EntryPoint() As UInteger


    Public Sub GetEntryPoint()
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

        Dim mapCode As String = GetMapCode()


        If (mapCode = "") Then
            Return False
        End If
        Dim senddata As String = ""
        senddata = "mapcode=" & WebUtility.UrlEncode(mapCode) & "&"
        senddata = senddata & "subtitle=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.SubTitle) & "&"
        senddata = senddata & "bt=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.SCAEmail) & "&"
        senddata = senddata & "pw=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.PassWord) & "&"

        Dim tagsdata As String = ""
        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            If i <> 0 Then
                tagsdata = tagsdata & ","
            End If
            tagsdata = tagsdata & pjData.TEData.SCArchive.CodeDatas(i).TagName
        Next
        senddata = senddata & "vtaglist=" & WebUtility.UrlEncode(tagsdata) & "&"

        If pjData.TEData.SCArchive.updateinfo And False Then
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
                Tool.ErrorMsgBox(Tool.GetText("Error SCA") & vbCrLf & "계정 정보가 올바르지 않습니다.")
                Return False
            Case "BANUSER"
                Tool.ErrorMsgBox(Tool.GetText("Error SCA") & vbCrLf & "SCA사용이 금지된 아이디입니다.")
                Return False
        End Select


        ConnectKey = respon

        Return True
    End Function


    Public Function GetSCAMainEps() As String
        Return My.Computer.FileSystem.ReadAllText(GetSCArchiveeps)
    End Function
    Public Function GetBGMMainEps() As String
        Return My.Computer.FileSystem.ReadAllText(GetBGMTooleps)
    End Function



    Public Function GetSCAEps() As String
        Const CommandLength As Integer = 12
        Dim SpaceLength As Integer = pjData.TEData.SCArchive.DataSpace
        Dim FuncLength As Integer = pjData.TEData.SCArchive.FuncSpace


        Dim sb As New StringBuilder

        Dim NameSapces As New List(Of String)
        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            If pjData.TEData.SCArchive.CodeDatas(i).TypeIndex <> StarCraftArchive.CodeData.CodeType.Deaths Then
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

        'EntryPoint.Count * 4
        '8(플레이어) * 2(4바이트 맞추기 위해) * (CommandLength + SpaceLength) / 플레이어스페이스
        '2 * (CommandLength + SpaceLength) / 로컬로딩
        '2 * (CommandLength + SpaceLength) / 패딩
        '플레이어함수


        sb.AppendLine("const ws = Db(" & EntryPoint.Count * 4 + 16 * (CommandLength + SpaceLength) + FuncLength * 4 & ");  // workspace")
        'sb.AppendLine("const ws = 0x58F44A;")
        sb.AppendLine("const EntryPointLength = " & EntryPoint.Count & ";  // EntryPointLength")
        sb.AppendLine("const SpaceLength = " & SpaceLength & ";  // DataBufferSize")
        sb.AppendLine("const FuncLength = " & FuncLength & ";  // FuncSpace")
        sb.AppendLine("const FuncStartEPD = EPD(ws + " & EntryPoint.Count * 4 + 16 * (CommandLength + SpaceLength) & ");  // FuncStart")
        sb.AppendLine("const ObjectCount = " & pjData.TEData.SCArchive.CodeDatas.Count & ";  // ObjectCount")


        sb.AppendLine("")
        sb.AppendLine("function Init() {")
        sb.AppendLine("    MPQAddFile('SCARCHIVEMAPCODE', py_open('scakeyfile', 'rb').read());")
        sb.AppendLine("    MPQAddFile('SCARCHIVEDATA', py_open('scadatafile', 'rb').read());")
        sb.AppendLine("    DoActions(list(  // EntryPoint")
        For i = 0 To EntryPoint.Count - 1
            sb.AppendLine("        SetMemory(ws + " & 4*i & ", SetTo, " & EntryPoint(i) & "),")
        Next

        sb.AppendLine("    ));")
        sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("function Exec() {")
        sb.AppendLine("    Init();")
        sb.AppendLine("}")
        sb.AppendLine("")

        sb.AppendLine("function ResetValue(tagNum, index) {")
        sb.AppendLine("    const cp = getcurpl();")
        sb.AppendLine("    const ResetArray = function(alen, epd) {")
        sb.AppendLine("        for (var i = 0 ; i < alen ; i++) {")
        sb.AppendLine("            dwwrite_epd(epd + i, 0);")
        sb.AppendLine("        }")
        sb.AppendLine("    };")
        sb.AppendLine("    switch (tagNum) {")

        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            sb.AppendLine("    case " & i & ": {")

            Select Case pjData.TEData.SCArchive.CodeDatas(i).TypeIndex
                Case StarCraftArchive.CodeData.CodeType.Variable
                    '변수
                    Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                    names = names.Split(".").First
                    names = names.Replace("\", ".")
                    sb.AppendLine("        n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName & "[cp] = 0;")
                Case StarCraftArchive.CodeData.CodeType.Deaths
                    '데스값 p, Setto, 0, Unit
                    sb.AppendLine("        SetDeaths(CurrentPlayer, SetTo, 0, " & pjData.TEData.SCArchive.CodeDatas(i).ValueIndex & ");")
                Case StarCraftArchive.CodeData.CodeType.Array
                    '배열
                    Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                    names = names.Split(".").First
                    names = names.Replace("\", ".")

                    Dim arrayname As String = "n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName

                    sb.AppendLine("        const alen = " & arrayname & ".length / 8;")
                    sb.AppendLine("        const aepd = alen * cp + EPD(" & arrayname & ");")
                    sb.AppendLine("        ResetArray(alen, aepd);")

            End Select
            sb.AppendLine("        break; }")
        Next

        sb.AppendLine("    }")
        sb.AppendLine("}")

        sb.AppendLine("")
        sb.AppendLine("function SaveValue(tagNum, Value, index) {")
        sb.AppendLine("    const cp = getcurpl();")



        sb.AppendLine("    switch (tagNum) {")

        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            sb.AppendLine("    case " & i & ": {")
            Select Case pjData.TEData.SCArchive.CodeDatas(i).TypeIndex
                Case StarCraftArchive.CodeData.CodeType.Variable
                    '변수
                    Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                    names = names.Split(".").First
                    names = names.Replace("\", ".")
                    sb.AppendLine("        n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName & "[cp] = Value;")
                Case StarCraftArchive.CodeData.CodeType.Deaths
                    '데스값 p, Setto, 0, Unit
                    sb.AppendLine("        SetDeaths(CurrentPlayer, SetTo, Value, " & pjData.TEData.SCArchive.CodeDatas(i).ValueIndex & ");")
                Case StarCraftArchive.CodeData.CodeType.Array
                    '배열
                    Dim names As String = pjData.TEData.SCArchive.CodeDatas(i).NameSpaceName
                    names = names.Split(".").First
                    names = names.Replace("\", ".")

                    Dim arrayname As String = "n" & NameSapces.IndexOf(names) & "." & pjData.TEData.SCArchive.CodeDatas(i).ValueName

                    sb.AppendLine("        const alen = " & arrayname & ".length / 8;")
                    sb.AppendLine("        " & arrayname & "[alen * cp + index] = Value;")
            End Select
            sb.AppendLine("        break; }")
        Next

        sb.AppendLine("    }")
        sb.AppendLine("}")
        sb.AppendLine("")
        '저장시 메모리에 tagNum을 적는다. 리턴값으로는 전진한 만큼을 기록한다.
        sb.AppendLine("function SaveDataWriteValue(tagNum, BaseAddress, index) {")
        sb.AppendLine("    const cp = getcurpl();")
        sb.AppendLine("    const indexQ = BaseAddress + index / 2;")
        sb.AppendLine("    var indexR, rvalue = 0, 0;")
        sb.AppendLine("    Trigger(index.ExactlyX(1, 1), indexR.SetNumber(2));")
        sb.AppendLine("    const AdvanceIndex = function() {")
        sb.AppendLine("        SetVariables(list(index, indexR), list(1, 2), list(Add, Add));")
        sb.AppendLine("        Trigger(indexR.AtLeast(4), list(indexR.SetNumber(0), indexQ.AddNumber(1)));")
        sb.AppendLine("    };")
        sb.AppendLine("")
        sb.AppendLine("    const SaveArray = function(alen, epd) {")
        sb.AppendLine("        for (var i = 0 ; i < alen ; i++) {")
        sb.AppendLine("            const objValue = dwread_epd(epd + i);")
        sb.AppendLine("            EUDContinueIf(objValue == 0);")
        sb.AppendLine("            rvalue += objValue;")
        sb.AppendLine("            if (objValue > 0xFFFF) {")
        sb.AppendLine("                wwrite_epd(indexQ, indexR, 0x3000 + tagNum);")
        sb.AppendLine("                AdvanceIndex();")
        sb.AppendLine("                wwrite_epd(indexQ, indexR, i);")
        sb.AppendLine("                AdvanceIndex();")
        sb.AppendLine("                const objQ, objR = div(objValue, 0x10000);")
        sb.AppendLine("                wwrite_epd(indexQ, indexR, objQ);")
        sb.AppendLine("                AdvanceIndex();")
        sb.AppendLine("                wwrite_epd(indexQ, indexR, objR);")
        sb.AppendLine("                AdvanceIndex();")
        sb.AppendLine("            } else {")
        sb.AppendLine("                wwrite_epd(indexQ, indexR, 0x4000 + tagNum);")
        sb.AppendLine("                AdvanceIndex();")
        sb.AppendLine("                wwrite_epd(indexQ, indexR, i);")
        sb.AppendLine("                AdvanceIndex();")
        sb.AppendLine("                wwrite_epd(indexQ, indexR, objValue);")
        sb.AppendLine("                AdvanceIndex();")
        sb.AppendLine("            }")
        sb.AppendLine("        }")
        sb.AppendLine("    };")
        sb.AppendLine("")
        sb.AppendLine("    const SaveDCV = function(objValue) {")
        sb.AppendLine("        if (objValue == 0) return;")
        sb.AppendLine("        rvalue += objValue;")
        sb.AppendLine("        if (objValue > 0xFFFF) {")
        sb.AppendLine("            wwrite_epd(indexQ, indexR, 0x1000 + tagNum);")
        sb.AppendLine("            AdvanceIndex();")
        sb.AppendLine("            const objQ, objR = div(objValue, 0x10000);")
        sb.AppendLine("            wwrite_epd(indexQ, indexR, objQ);")
        sb.AppendLine("            AdvanceIndex();")
        sb.AppendLine("            wwrite_epd(indexQ, indexR, objR);")
        sb.AppendLine("            AdvanceIndex();")
        sb.AppendLine("        } else {")
        sb.AppendLine("            wwrite_epd(indexQ, indexR, 0x2000 + tagNum);")
        sb.AppendLine("            AdvanceIndex();")
        sb.AppendLine("            wwrite_epd(indexQ, indexR, objValue);")
        sb.AppendLine("            AdvanceIndex();")
        sb.AppendLine("         }")
        sb.AppendLine("    };")
        sb.AppendLine("")
        sb.AppendLine("    switch (tagNum) {")

        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            sb.AppendLine("    case " & i & ": {")


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
                    sb.AppendLine("        SaveArray(alen, alen * cp + EPD(" & arrayname & "));")
            End Select

            Select Case pjData.TEData.SCArchive.CodeDatas(i).TypeIndex
                Case StarCraftArchive.CodeData.CodeType.Variable, StarCraftArchive.CodeData.CodeType.Deaths
                    sb.AppendLine("        SaveDCV(objValue);")
            End Select
            sb.AppendLine("        break; }")
        Next
        sb.AppendLine("    }")
        sb.AppendLine("    return index, rvalue;")
        sb.AppendLine("}")
        sb.AppendLine("")
        '
        sb.AppendLine("function LoadDataReadValue(BaseAddress, i) {")
        sb.AppendLine("    const iQ = BaseAddress + i / 2;")
        sb.AppendLine("    var iR = 0;")
        sb.AppendLine("    Trigger(i.ExactlyX(1, 1), iR.SetNumber(2));")
        sb.AppendLine("    const AdvanceI = function() {")
        sb.AppendLine("        SetVariables(list(i, iR), list(1, 2), list(Add, Add));")
        sb.AppendLine("        Trigger(iR.AtLeast(4), list(iR.SetNumber(0), iQ.AddNumber(1)));")
        sb.AppendLine("    };")
        sb.AppendLine("    const indicator = wread_epd(iQ, iR);")
        sb.AppendLine("    if (indicator == 0) return i;")
        sb.AppendLine("    AdvanceI();")
        sb.AppendLine("    const spec, ObjNum = div(indicator, 0x1000);")
        sb.AppendLine("    var vindex = 0;")
        sb.AppendLine("    var value;")
        sb.AppendLine("    if (spec == 1) {  // 4바이트 지정")
        sb.AppendLine("        const value1 = wread_epd(iQ, iR);")
        sb.AppendLine("        AdvanceI();")
        sb.AppendLine("        const value2 = wread_epd(iQ, iR);")
        sb.AppendLine("        value = value1 * 0x10000 + value2;")
        sb.AppendLine("    } else if (spec == 2) {  // 2바이트 지정")
        sb.AppendLine("        value = wread_epd(iQ, iR);")
        sb.AppendLine("    } else if (spec == 3) {  // 4바이트 지정")
        sb.AppendLine("        vindex = wread_epd(iQ, iR);")
        sb.AppendLine("        AdvanceI();")
        sb.AppendLine("        const value1 = wread_epd(iQ, iR);")
        sb.AppendLine("        AdvanceI();")
        sb.AppendLine("        const value2 = wread_epd(iQ, iR);")
        sb.AppendLine("        value = value1 * 0x10000 + value2;")
        sb.AppendLine("    } else if (spec == 4) {  //2바이트 지정")
        sb.AppendLine("        vindex = wread_epd(iQ, iR);")
        sb.AppendLine("        AdvanceI();")
        sb.AppendLine("        value = wread_epd(iQ, iR);")
        sb.AppendLine("    }")
        sb.AppendLine("    SaveValue(ObjNum, value, vindex);")
        sb.AppendLine("    return i;")
        sb.AppendLine("}")


        Return sb.ToString
    End Function




    Public Function GetMapCode() As String
        'Dim senddata As String = ""
        'senddata = "email=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.MakerBattleTag) & "&"
        'senddata = senddata & "pw=" & WebUtility.UrlEncode(pjData.TEData.SCArchive.PassWord)

        'Dim respon As String = httpRequest("getbattleTag", senddata)

        'If respon = "NOACCOUNT" Then
        '    MsgBox(Tool.GetText("Error SCA") & vbCrLf & "계정 정보가 올바르지 않습니다.", MsgBoxStyle.Critical)
        '    Return ""
        'End If
        Dim MakerBattleTag As String

        If pjData.TEData.SCArchive.IsUseOldBattleTag Then
            MakerBattleTag = pjData.TEData.SCArchive.MakerBattleTag
        Else
            MakerBattleTag = pjData.TEData.SCArchive.SCAEmail
        End If


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

        Dim checkstring As String = httpRequest("encrypt256", "key=" & WebUtility.UrlEncode(ConnectKey) & "&data=" & WebUtility.UrlEncode(Sb.ToString))
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
