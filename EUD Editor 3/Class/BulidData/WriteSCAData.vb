Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Security.Cryptography
Imports System.Text
Imports EUD_Editor_3.IScript
Imports Ionic.Zip

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


        fs = New FileStream(TriggerEditorPath & "\msqcloader_sca.eps", FileMode.Create)
        sw = New StreamWriter(fs)

        sw.Write(GetMSQCLoaderEps)

        sw.Close()
        fs.Close()




        'fs = New FileStream(TriggerEditorPath & "\SCArchive.eps", FileMode.Create)
        'sw = New StreamWriter(fs)

        'sw.Write(GetSCAToolEps)

        'sw.Close()
        'fs.Close()
        WriteSCADataFile()
        WriteSCAScriptZipFile()
    End Sub


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


        Dim tagsdata As String = ""
        For i = 0 To pjData.TEData.SCArchive.CodeDatas.Count - 1
            If i <> 0 Then
                tagsdata = tagsdata & ","
            End If
            tagsdata = tagsdata & pjData.TEData.SCArchive.CodeDatas(i).TagName
        Next

        Dim datalist As New Dictionary(Of String, String) From {
            {"mapcode", mapCode},
            {"subtitle", pjData.TEData.SCArchive.SubTitle},
            {"bt", pjData.TEData.SCArchive.SCAEmail},
            {"pw", pjData.TEData.SCArchive.GetPassWord(pjData.TEData.SCArchive.SCAEmail)},
            {"vtaglist", tagsdata},
            {"isupdate", "False"}
        }

        Dim respon As String = HttpTool.Request("registermap", datalist)

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

    Public Function GetMSQCLoaderEps() As String
        Dim sb As New StringBuilder

        sb.AppendLine("var MSQCIsTransfer = 0;")
        sb.AppendLine("var MSQCSendParity = 0;")
        For index = 1 To pjData.TEData.SCArchive.MSQCSize
            sb.AppendLine("var MSQCSend" & index & " = 0;")
        Next
        For index = 1 To pjData.TEData.SCArchive.MSQCSize
            sb.AppendLine("const MSQCReceive" & index & " = PVariable();")
        Next

        sb.AppendLine("const MSQCReceiveParity = PVariable();")
        sb.AppendLine("")
        sb.AppendLine("function Init(){")
        sb.AppendLine("	EUDRegisterObjectToNamespace(""MSQCIsTransfer"", MSQCIsTransfer);")
        sb.AppendLine("	EUDRegisterObjectToNamespace(""MSQCReceiveParity"", MSQCReceiveParity);")
        sb.AppendLine("	EUDRegisterObjectToNamespace(""MSQCSendParity"", MSQCSendParity);")
        For index = 1 To pjData.TEData.SCArchive.MSQCSize
            sb.AppendLine("	EUDRegisterObjectToNamespace(""MSQCSend" & index & """, MSQCSend" & index & ");")
        Next
        sb.AppendLine("	")
        For index = 1 To pjData.TEData.SCArchive.MSQCSize
            sb.AppendLine("	EUDRegisterObjectToNamespace(""MSQCReceive" & index & """, MSQCReceive" & index & ");")
        Next
        sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("const temparray = EUDArray(" & pjData.TEData.SCArchive.MSQCSize & ");")
        sb.AppendLine("")
        sb.AppendLine("var l_errorindex = 0;")
        sb.AppendLine("const l_errorlist = EUDArray(1000);")
        sb.AppendLine("")
        sb.AppendLine("const loadstart = PVariable();")
        sb.AppendLine("const loaddatalen = PVariable();")
        sb.AppendLine("const lastrindex = PVariable();")
        sb.AppendLine("const waittimer = PVariable();")
        sb.AppendLine("")
        sb.AppendLine("const receiveindex = PVariable();")
        sb.AppendLine("var l_loadindex = 0;")
        sb.AppendLine("var l_datablockindex = 0;")
        sb.AppendLine("var l_errorcount = 0;")
        sb.AppendLine("var l_loaddatalen = 0;")
        sb.AppendLine("var l_datasendstart = 0;")
        sb.AppendLine("var l_displayloaddatalen = 0;")
        sb.AppendLine("")
        sb.AppendLine("var senddata = 0;")
        sb.AppendLine("const receivedata = PVariable();")
        sb.AppendLine("function Start(cp, tsenddata, treceivedata){")
        sb.AppendLine("	if(IsUserCP()){")
        sb.AppendLine("		l_datablockindex = 0;")
        sb.AppendLine("		l_loadindex = 0;")
        sb.AppendLine("		l_errorcount = 0;")
        sb.AppendLine("		l_datasendstart = 0;")
        sb.AppendLine("		l_loaddatalen = 9999;")
        sb.AppendLine("		l_displayloaddatalen = 0;")
        sb.AppendLine("		MSQCIsTransfer = 1;")
        sb.AppendLine("	}")
        sb.AppendLine("	")
        sb.AppendLine("	loaddatalen[cp] = 0;")
        sb.AppendLine("	waittimer[cp] = 0;")
        sb.AppendLine("	receiveindex[cp] = 0; ")
        sb.AppendLine("	lastrindex[cp] = 0;")
        sb.AppendLine("	")
        sb.AppendLine("	senddata = tsenddata;")
        sb.AppendLine("	receivedata[cp] = treceivedata;")
        sb.AppendLine("	")
        sb.AppendLine("	loadstart[cp] = 1;")
        sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("")
        sb.AppendLine("function StartLocal(len){")
        sb.AppendLine("	l_datasendstart = 1;")
        sb.AppendLine("	l_loaddatalen = len * 2;")
        sb.AppendLine("")
        sb.AppendLine("	l_displayloaddatalen = (l_loaddatalen / " & pjData.TEData.SCArchive.MSQCSize & ") * " & pjData.TEData.SCArchive.MSQCSize & ";")
        sb.AppendLine("	if(l_displayloaddatalen == 0) l_displayloaddatalen = " & pjData.TEData.SCArchive.MSQCSize & ";")
        sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("")
        sb.AppendLine("function ReadSendValue(index){")
        sb.AppendLine("	const rv = wread_epd(senddata + index / 2, (index % 2) * 2);")
        sb.AppendLine("	")
        sb.AppendLine("	//Const x = rv % 2000 + 3;")
        sb.AppendLine("	//Const y = rv / 2000 + 3;")
        sb.AppendLine("	//Return x + y * 0x10000;")
        sb.AppendLine("	")
        sb.AppendLine("	return rv;")
        sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("const mapwidth = 1900;")
        sb.AppendLine("const minwidth = 25;")
        sb.AppendLine("const maxparity = 52;")
        sb.AppendLine("const lenparity = 51;")
        sb.AppendLine("function GetSendValue(rv, p){")
        sb.AppendLine("	const rvalue = rv + p * 0x10000;")
        sb.AppendLine("	")
        sb.AppendLine("	const x = rvalue % mapwidth + minwidth;")
        sb.AppendLine("	const y = rvalue / mapwidth + minwidth;")
        sb.AppendLine("	")
        sb.AppendLine("	return x + y * 0x10000;")
        sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("function GetReceiveValue(rv){")
        sb.AppendLine("	const x = rv % 0x10000 - minwidth;")
        sb.AppendLine("	const y = rv / 0x10000 - minwidth;")
        sb.AppendLine("	")
        sb.AppendLine("	return x + y * mapwidth;")
        sb.AppendLine("}")
        sb.AppendLine("")


        sb.AppendLine("function SetMSQCSend(index, rv, p){")
        sb.AppendLine("	const rvalue = rv + p * 0x10000;")
        sb.AppendLine("	")
        sb.AppendLine("	const x = rvalue % mapwidth + minwidth;")
        sb.AppendLine("	const y = rvalue / mapwidth + minwidth;")
        sb.AppendLine("	")
        sb.AppendLine("	const value = x + y * 0x10000;")
        sb.AppendLine("	//Return x + y * 0x10000;")
        sb.AppendLine("	")
        sb.AppendLine("	if(index == 0) MSQCSend1 = value;")
        For index = 2 To pjData.TEData.SCArchive.MSQCSize
            sb.AppendLine("	else if(index == " & index - 1 & ") MSQCSend" & index & " = value;")
        Next
        sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("function ReadReceiveData(cp){")


        For index = 1 To pjData.TEData.SCArchive.MSQCSize
            sb.AppendLine("	temparray[" & index - 1 & "] = MSQCReceive" & index & "[cp];")
        Next
        sb.AppendLine("	")
        sb.AppendLine("	for (var i = 0; i < " & pjData.TEData.SCArchive.MSQCSize & "; i++) {")
        sb.AppendLine("		const x = (temparray[i]) % 0x10000 - minwidth;")
        sb.AppendLine("		const y = (temparray[i]) / 0x10000 - minwidth;")
        sb.AppendLine("		temparray[i] = x + y * mapwidth;")
        sb.AppendLine("	}")
        sb.AppendLine("}")
        sb.AppendLine("")
        sb.AppendLine("const s = StringBuffer();")
        sb.AppendLine("function Loop(cp){")
        sb.AppendLine("	if(loadstart[cp] == 1){")
        sb.AppendLine("")
        sb.AppendLine("")
        sb.AppendLine("		if(waittimer[cp] < 10){")
        sb.AppendLine("			waittimer[cp] += 1;")
        sb.AppendLine("			")
        sb.AppendLine("			const initvalue = 13;")
        sb.AppendLine("			for (var i = 0; i < " & pjData.TEData.SCArchive.MSQCSize & "; i++) {")
        sb.AppendLine("				if(IsUserCP()){")
        sb.AppendLine("					SetMSQCSend(i, initvalue, maxparity);")
        sb.AppendLine("				}")
        sb.AppendLine("			}")
        sb.AppendLine("			MSQCSendParity = GetSendValue(initvalue, maxparity);")
        sb.AppendLine("			")
        sb.AppendLine("			return;")
        sb.AppendLine("		}")
        sb.AppendLine("		")
        sb.AppendLine("		if(IsUserCP() && l_datasendstart == 1){")
        sb.AppendLine("			if((lastrindex[cp] < l_loaddatalen - 1)){")
        sb.AppendLine("				//Send")
        sb.AppendLine("				const tinedx = (l_loadindex + l_datablockindex * 50) * " & pjData.TEData.SCArchive.MSQCSize & ";")
        sb.AppendLine("				")
        sb.AppendLine("				const addindex = l_loadindex + 1;")
        sb.AppendLine("				for (var i = 0; i < " & pjData.TEData.SCArchive.MSQCSize & "; i++) {")
        sb.AppendLine("					SetMSQCSend(i, ReadSendValue(tinedx + i), addindex);")
        sb.AppendLine("				}")
        sb.AppendLine("				")
        sb.AppendLine("				//다음 루프에서 안넘어가게")
        sb.AppendLine("				if((tinedx + " & pjData.TEData.SCArchive.MSQCSize & ") < l_loaddatalen - 1){")
        sb.AppendLine("					l_loadindex += 1;")
        sb.AppendLine("				}")
        sb.AppendLine("				if(l_loadindex >= 50){")
        sb.AppendLine("					l_loadindex = 0;")
        sb.AppendLine("					l_datablockindex += 1;")
        sb.AppendLine("				}")
        sb.AppendLine("			}else{")
        sb.AppendLine("				//데이터가 넘어가면 끝 알리기")
        sb.AppendLine("				for (var i = 0; i < " & pjData.TEData.SCArchive.MSQCSize & "; i++) {")
        sb.AppendLine("					SetMSQCSend(i, l_loaddatalen, lenparity);")
        sb.AppendLine("				}")
        sb.AppendLine("			}")
        sb.AppendLine("		}")
        sb.AppendLine("		")
        sb.AppendLine("		ReadReceiveData(cp);")
        sb.AppendLine("")
        sb.AppendLine("		var isreceiveflag = 0;")
        sb.AppendLine("		for (var i = 0; i < " & pjData.TEData.SCArchive.MSQCSize & "; i++) {")
        sb.AppendLine("			//Receive")
        sb.AppendLine("			const recevie = temparray[i];")
        sb.AppendLine("			")
        sb.AppendLine("			const parity = recevie / 0x10000 - 1;")
        sb.AppendLine("			const rval = recevie % 0x10000;")
        sb.AppendLine("			")
        sb.AppendLine("			")
        sb.AppendLine("			if((parity + 1) == lenparity){")
        sb.AppendLine("				loaddatalen[cp] = rval;")
        sb.AppendLine("			}else if(parity > 50){")
        sb.AppendLine("				break;")
        sb.AppendLine("			}else{")
        sb.AppendLine("				isreceiveflag = 1;")
        sb.AppendLine("				const calcparity = receiveindex[cp] % 50;")
        sb.AppendLine("				")
        sb.AppendLine("				if(parity < calcparity){")
        sb.AppendLine("					//패리티 불일치")
        sb.AppendLine("					//receiveindex[cp] -= calcparity - parity;")
        sb.AppendLine("				}else if(parity > calcparity){")
        sb.AppendLine("					receiveindex[cp] += parity - calcparity;")
        sb.AppendLine("				}")
        sb.AppendLine("				")
        sb.AppendLine("				const rindex = receiveindex[cp] * " & pjData.TEData.SCArchive.MSQCSize & " + i;")
        sb.AppendLine("				lastrindex[cp] = rindex;")
        sb.AppendLine("				//값 받음")
        sb.AppendLine("				//WriteReceiveValue()")
        sb.AppendLine("				wwrite_epd(receivedata[cp] + rindex / 2, (rindex % 2) * 2, rval);")
        sb.AppendLine("				//const testv = dwread_epd(senddata + rindex);")
        sb.AppendLine("				")
        sb.AppendLine("				if(rindex % 2 == 0){")
        sb.AppendLine("					//이떄출력")
        sb.AppendLine("					const sendd = dwread_epd(senddata + rindex / 2 - 1);")
        sb.AppendLine("					const recevied = dwread_epd(receivedata[cp] + rindex / 2 - 1);")
        sb.AppendLine("					")
        sb.AppendLine("				}")
        sb.AppendLine("				")
        sb.AppendLine("			}")
        sb.AppendLine("			")
        sb.AppendLine("			")
        sb.AppendLine("")
        sb.AppendLine("			")
        sb.AppendLine("			")
        sb.AppendLine("			if(lastrindex[cp] >= loaddatalen[cp] - 1){")
        sb.AppendLine("				loadstart[cp] = 2;")
        sb.AppendLine("				break;")
        sb.AppendLine("			}")
        sb.AppendLine("		}")
        sb.AppendLine("		if(isreceiveflag == 1){")
        sb.AppendLine("			receiveindex[cp] += 1;")
        sb.AppendLine("		}")
        sb.AppendLine("	}else if(loadstart[cp] == 2){")
        sb.AppendLine("		//검증 구간")
        'sb.AppendLine("		DisplayText(""Debug: 검증 구간"");")
        sb.AppendLine("		if(IsUserCP()){")
        sb.AppendLine("			if(l_errorcount == 0){")
        sb.AppendLine("				l_errorcount = 100;")
        sb.AppendLine("				l_errorindex = 0;")
        sb.AppendLine("				for (var i = 0; i < loaddatalen[cp] / 2; i++) {")
        'sb.AppendLine("					s.printf("" \ x07 검증 구간 index {}, s {}, r {} "", i, dwread_epd(senddata + i), dwread_epd(receivedata[cp] + i));")
        sb.AppendLine("					if(dwread_epd(senddata + i) != dwread_epd(receivedata[cp] + i)){")
        'sb.AppendLine("						s.printf(""\x06 검증 구간 index {}, s {}, r {} "", i, dwread_epd(senddata + i), dwread_epd(receivedata[cp] + i));")
        sb.AppendLine("						l_errorcount = 200;")
        sb.AppendLine("						l_errorlist[l_errorindex] = i;")
        sb.AppendLine("						l_errorindex += 1;")
        sb.AppendLine("					}")
        sb.AppendLine("				}")
        sb.AppendLine("			}")
        sb.AppendLine("			")
        sb.AppendLine("			const sval = l_errorcount + maxparity * 0x10000;")
        sb.AppendLine("			MSQCSendParity = sval;")
        sb.AppendLine("		}")
        sb.AppendLine("		//Receive")
        sb.AppendLine("		const recevie = MSQCReceiveParity[cp];")
        sb.AppendLine("		")
        sb.AppendLine("		const parity = recevie / 0x10000;")
        sb.AppendLine("		const rval = recevie % 0x10000;")
        sb.AppendLine("		if(parity == maxparity){")
        sb.AppendLine("			//제대로 받았음")
        sb.AppendLine("			if(rval == 100){")
        sb.AppendLine("				//성공")
        sb.AppendLine("				loadstart[cp] = 0;")
        sb.AppendLine("				if(IsUserCP()){")
        sb.AppendLine("					l_datasendstart = 0;")
        sb.AppendLine("					MSQCIsTransfer = 0;")
        sb.AppendLine("				}")
        sb.AppendLine("			}else if(rval == 200){")
        sb.AppendLine("				//오류")
        sb.AppendLine("				//loadstart[cp] = 3;")
        sb.AppendLine("				")
        sb.AppendLine("				if(IsUserCP()){")
        sb.AppendLine("					l_datablockindex = 0;")
        sb.AppendLine("					l_loadindex = 0;")
        sb.AppendLine("					l_errorcount = 0;")
        sb.AppendLine("				}")
        sb.AppendLine("				waittimer[cp] = 0;")
        sb.AppendLine("				lastrindex[cp] = 0;")
        sb.AppendLine("				loadstart[cp] = 3;")
        sb.AppendLine("				")
        sb.AppendLine("			}")
        sb.AppendLine("		}")
        sb.AppendLine("	}else if(loadstart[cp] == 3){")
        sb.AppendLine("		//부분 수정 구간")
        'sb.AppendLine("		DisplayText(""Debug: 부분수정구간 진입"");")
        sb.AppendLine("		")
        sb.AppendLine("")
        sb.AppendLine("		var lastreceiveerror = 0;")
        sb.AppendLine("		//=======================오류전송=======================")
        sb.AppendLine("		if(IsUserCP()){")
        sb.AppendLine("			const tinedx = (l_loadindex + l_datablockindex * 50) * " & (pjData.TEData.SCArchive.MSQCSize \ 3) & ";")
        sb.AppendLine("			const p = l_loadindex % 50;")
        sb.AppendLine("			")
        sb.AppendLine("			")
        sb.AppendLine("			var qw = 0;")
        sb.AppendLine("			for (var i = 0; i < " & (pjData.TEData.SCArchive.MSQCSize \ 3) * 3 & "; i += 3) {")
        sb.AppendLine("				if(l_errorindex - 1 < tinedx + qw){")
        sb.AppendLine("					SetMSQCSend(i, 0xFFFF, maxparity);")
        sb.AppendLine("					SetMSQCSend(i + 1, 0xFFFF, maxparity);")
        sb.AppendLine("					SetMSQCSend(i + 2, 0xFFFF, maxparity);")
        sb.AppendLine("					break;")
        sb.AppendLine("				}")
        sb.AppendLine("				")
        sb.AppendLine("				const currentIndex = l_errorlist[tinedx + qw] * 2;")
        sb.AppendLine("				const v1 = ReadSendValue(currentIndex);")
        sb.AppendLine("				const v2 = ReadSendValue(currentIndex + 1);")
        sb.AppendLine("				//현재 인덱스를 알려줌")
        sb.AppendLine("				SetMSQCSend(i, l_errorlist[tinedx + qw], l_loadindex);")
        sb.AppendLine("				SetMSQCSend(i + 1, v1, l_loadindex);")
        sb.AppendLine("				SetMSQCSend(i + 2, v2, l_loadindex);")
        sb.AppendLine("				")
        'sb.AppendLine("				s.printfAt(qw * 2 + 1, ""\x07 currentIndex {}, send {:x}, receive {:x}, v1 {:x} , v2 {:x} "", currentIndex / 2, dwread_epd(senddata + currentIndex / 2), dwread_epd(receivedata[cp] + currentIndex / 2), v1, v2);")
        sb.AppendLine("				qw += 1;")
        sb.AppendLine("			}")
        sb.AppendLine("")
        sb.AppendLine("")
        sb.AppendLine("			if(tinedx + qw < l_errorindex){")
        sb.AppendLine("				l_loadindex += 1;")
        sb.AppendLine("			}else{")
        sb.AppendLine("				//l_loadindex = 0;")
        sb.AppendLine("				//l_datablockindex = 0;")
        sb.AppendLine("			}")
        sb.AppendLine("			")
        sb.AppendLine("			if(l_loadindex >= 50){")
        sb.AppendLine("				l_loadindex = 0;")
        sb.AppendLine("				l_datablockindex += 1;")
        sb.AppendLine("			}")
        sb.AppendLine("			")
        sb.AppendLine("			")
        'sb.AppendLine("			s.printf(""\x07Send l_errorlist {}  l_errorindex {}"", l_errorlist[l_errorindex - 1], l_errorindex);")
        sb.AppendLine("			MSQCSendParity = GetSendValue(l_errorlist[l_errorindex - 1], maxparity);")
        sb.AppendLine("		}")
        sb.AppendLine("		//=======================오류전송=======================")
        sb.AppendLine("		")
        sb.AppendLine("		")
        sb.AppendLine("		//=======================오류받기=======================")
        sb.AppendLine("		ReadReceiveData(cp);")
        sb.AppendLine("		")
        sb.AppendLine("		const lasterrorindex = GetReceiveValue(MSQCReceiveParity[cp]) % 0x10000;")
        sb.AppendLine("		")
        sb.AppendLine("		var qw = 0;")
        sb.AppendLine("		for (var i = 0; i < " & (pjData.TEData.SCArchive.MSQCSize \ 3) * 3 & "; i += 3) {")
        sb.AppendLine("			const parity = temparray[i] / 0x10000;")
        'sb.AppendLine("		    s.printf(""\x07 parity {}"",parity);")
        sb.AppendLine("			if(parity > 50){")
        'sb.AppendLine("		        DisplayText(""Debug: 패러티로 탈출"");")
        sb.AppendLine("				break;")
        sb.AppendLine("			}")
        sb.AppendLine("			")
        sb.AppendLine("			const currentIndex = temparray[i] % 0x10000;")
        sb.AppendLine("			const v1 = temparray[i + 1] % 0x10000;")
        sb.AppendLine("			const v2 = temparray[i + 2] % 0x10000;")
        sb.AppendLine("			//현재 인덱스를 알려줌")
        sb.AppendLine("			if(lasterrorindex != lastreceiveerror){")
        sb.AppendLine("				lastreceiveerror = currentIndex;")
        sb.AppendLine("			}")
        sb.AppendLine("			")
        sb.AppendLine("			const value = v2 * 0x10000 + v1;")
        sb.AppendLine("			")
        sb.AppendLine("			dwwrite_epd(receivedata[cp] + currentIndex, value);")
        'sb.AppendLine("			s.printfAt(qw + 2, ""\x07 currentIndex {}, v {:x}, send {:x}, receive {:x}, v1 {:x} , v2 {:x} "", currentIndex, value, dwread_epd(senddata + currentIndex), dwread_epd(receivedata[cp] + currentIndex), v1, v2);")
        sb.AppendLine("			")
        sb.AppendLine("			qw += 1;")
        sb.AppendLine("		}")
        sb.AppendLine("		")
        'sb.AppendLine("		s.printf(""\x07Receive lasterrorindex {}, lastreceiveerror {}"",lasterrorindex, lastreceiveerror);")
        sb.AppendLine("		if(lasterrorindex == lastreceiveerror){")
        sb.AppendLine("			loadstart[cp] = 2;")
        sb.AppendLine("			")

        sb.AppendLine("			")
        sb.AppendLine("		}else{")
        sb.AppendLine("			if(waittimer[cp] < 20){")
        sb.AppendLine("				waittimer[cp] += 1;")
        sb.AppendLine("			}else{")
        sb.AppendLine("				loadstart[cp] = 2;")
        sb.AppendLine("			}")
        sb.AppendLine("		}")
        sb.AppendLine("		")
        sb.AppendLine("		//=======================오류받기=======================")
        sb.AppendLine("")
        sb.AppendLine("	}")
        sb.AppendLine("	")
        sb.AppendLine("}")

        Return sb.ToString
    End Function

    Public Function GetSCAEps() As String
        Const CommandLength As Integer = 12
        Const FastLoadCommandLength As Integer = 116
        Const FuncCommandLength As Integer = 32
        Dim SpaceLength As Integer = pjData.TEData.SCArchive.DataSpace
        Dim FuncLength As Integer = pjData.TEData.SCArchive.FuncSpace
        Dim SCAScriptVarCount As Integer = pjData.TEData.SCArchive.SCAScriptVarCount

        Dim workSpace As New SCAWorkSpace
        workSpace.AddSpace("EntryPoint", EntryPoint.Count * 4)
        workSpace.AddSpace("UserCommandSpace", (CommandLength + SpaceLength) * 8)
        workSpace.AddSpace("FastLoadCommand", FastLoadCommandLength)
        workSpace.AddSpace("FastLoadSpace", SpaceLength)
        workSpace.AddSpace("FuncCommand", FuncCommandLength)
        workSpace.AddSpace("FuncData", FuncLength * 4)
        workSpace.AddSpace("FuncReturnTable", FuncLength * 4)
        workSpace.AddSpace("ScriptVarSpace", SCAScriptVarCount * 4)


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

        sb.AppendLine("import py_pathlib;")
        sb.AppendLine("")
        For i = 0 To NameSapces.Count - 1
            sb.AppendLine("import " & NameSapces(i) & " as n" & i & ";")
        Next
        sb.AppendLine("")

        'EntryPoint.Count * 4
        '8(플레이어) * 2(4바이트 맞추기 위해) * (CommandLength + SpaceLength) / 플레이어스페이스
        '2 * (CommandLength + SpaceLength) / 로컬로딩
        '2 * (CommandLength + SpaceLength) / 패딩
        '플레이어함수


        sb.AppendLine("const ws = Db(" & workSpace.GetAllCapacity() & ");  // workspace")

        sb.AppendLine("const EntryPointLength = " & EntryPoint.Count & ";  // EntryPointLength")
        sb.AppendLine("const SpaceLength = " & SpaceLength & ";  // DataBufferSize")
        sb.AppendLine("const FuncLength = " & FuncLength & ";  // FuncSpace / 4")

        sb.AppendLine("const FuncCommandEPD = EPD(ws + " & workSpace.GetSpaceStartOffset("FuncCommand") & ");  // FuncOrder")
        sb.AppendLine("const FuncDataEPD = EPD(ws + " & workSpace.GetSpaceStartOffset("FuncData") & ");  // FuncData")
        sb.AppendLine("const FuncReturnTableEPD = EPD(ws + " & workSpace.GetSpaceStartOffset("FuncReturnTable") & ");  // FuncReturnTable")
        sb.AppendLine("const SCAScriptVarEPD = EPD(ws + " & workSpace.GetSpaceStartOffset("ScriptVarSpace") & ");  // SCAScriptVarCount")

        'sb.AppendLine("const ws = Db(" & EntryPoint.Count * 4 + '엔트리포인트
        '              8 * (CommandLength + SpaceLength) + '일반 값
        '              (FastLoadCommandLength + SpaceLength) + '패스트 로드
        '              FuncCommandLength + FuncLength * 4 + 'FuncLoad
        '              FuncLength * 4 + 'FuncLoad
        '              SCAScriptVarCount * 4 &'FuncReturnTable
        '               ");  // workspace")
        ''sb.AppendLine("const ws = 0x58F44A;")
        'sb.AppendLine("const EntryPointLength = " & EntryPoint.Count & ";  // EntryPointLength")
        'sb.AppendLine("const SpaceLength = " & SpaceLength & ";  // DataBufferSize")
        'sb.AppendLine("const FuncLength = " & FuncLength & ";  // FuncSpace / 4")

        'sb.AppendLine("const FuncCommandEPD = EPD(ws + " & EntryPoint.Count * 4 +
        '              8 * (CommandLength + SpaceLength) + '일반 값
        '              (FastLoadCommandLength + SpaceLength) & '패스트 로드
        '              ");  // FuncOrder")
        'sb.AppendLine("const FuncDataEPD = EPD(ws + " & EntryPoint.Count * 4 +
        '              8 * (CommandLength + SpaceLength) + '일반 값
        '              (FastLoadCommandLength + SpaceLength) + '패스트 로드
        '              FuncCommandLength & 'FuncCommand
        '              ");  // FuncData")
        'sb.AppendLine("const FuncReturnTableEPD = EPD(ws + " & EntryPoint.Count * 4 +
        '              8 * (CommandLength + SpaceLength) + '일반 값
        '              (FastLoadCommandLength + SpaceLength) + '패스트 로드
        '              FuncCommandLength + 'FuncCommand
        '              FuncLength * 4 & 'FuncLoad
        '              ");  // FuncReturnTable")
        'sb.AppendLine("const SCAScriptVarEPD = EPD(ws + " & EntryPoint.Count * 4 +
        '              8 * (CommandLength + SpaceLength) + '일반 값
        '              (FastLoadCommandLength + SpaceLength) + '패스트 로드
        '              FuncCommandLength + 'FuncCommand
        '              FuncLength * 4 + 'FuncLoad
        '              FuncLength * 4 & 'FuncLoad
        '              ");  // SCAScriptVarCount")

        sb.AppendLine("const ObjectCount = " & pjData.TEData.SCArchive.CodeDatas.Count & ";  // ObjectCount")


        sb.AppendLine("")
        sb.AppendLine("function Init() {")
        sb.AppendLine("    MPQAddFile('SCARCHIVEMAPCODE', pathlib.Path('scakeyfile').read_bytes());")
        sb.AppendLine("    MPQAddFile('SCARCHIVEDATA', pathlib.Path('scadatafile').read_bytes());")
        sb.AppendLine("    MPQAddFile('SCASCRIPT', pathlib.Path('scascript').read_bytes());")

        For i = 0 To pjData.TEData.BGMData.SCABGMList.Count - 1
            Dim bgmfile As BGMData.BGMFile = pjData.TEData.BGMData.SCABGMList(i)
            Dim folderPath As String = BuildData.SoundFilePath() & "\" & bgmfile.BGMName
            Dim output As String = folderPath & "\sample.mp3"

            sb.AppendLine("    MPQAddFile('" & bgmfile.BGMName & "', pathlib.Path('" & output.Replace("\", "/") & "').read_bytes());")
            'Dim rfilename As String = files.Split("\").Last
            'If rfilename.IndexOf("st") <> -1 Then
            '    Dim c As Integer = rfilename.Replace("st", "").Replace(".ogg", "")

            '    Dim hname As String = GetSoundIndex(i)
            '    Dim tname As String = GetSoundIndex(c)


            '    sb.AppendLine("    MPQAddFile('" & hname & tname & "', pathlib.Path('" & files.Replace("\", "/") & "').read_bytes());")
            'End If
        Next


        sb.AppendLine("    DoActions(list(  // EntryPoint")
        For i = 0 To EntryPoint.Count - 1
            sb.AppendLine("        SetMemory(ws + " & 4 * i & ", SetTo, " & EntryPoint(i) & "),")
        Next

        sb.AppendLine("    ));")
        sb.AppendLine("}")
        sb.AppendLine("")
        ' Unnecessary Exec func!
        ' sb.AppendLine("function Exec() {")
        ' sb.AppendLine("    Init();")
        ' sb.AppendLine("}")
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



        sb.AppendLine("    const st = StringBuffer();")
        'sb.AppendLine("    st.print('값 전달 받음      tagNum :  ',tagNum , ' Value : ' , Value , ' index : ' ,index);")

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

        Sb.Append("!")
        Sb.Append(pjData.TEData.SCArchive.FuncSpace * 4)
        Sb.Append("!")

        Dim scriptsstrs As String = ""
        Dim scripts As List(Of String) = GetArgList("SCAScript").ToList()
        For Each item As String In scripts
            If scriptsstrs <> "" Then
                scriptsstrs = scriptsstrs & ","
            End If

            scriptsstrs = scriptsstrs & item.Replace("""", "")
        Next
        Sb.Append(scriptsstrs)
        Sb.Append("!")
        Sb.Append(pjData.TEData.SCArchive.SCAScriptVarCount)
        Sb.Append("!")
        Dim scriptvar As String = ""
        For Each item As String In macro.SCAScriptVariables
            If scriptvar <> "" Then
                scriptvar = scriptvar & ","
            End If

            scriptvar = scriptvar & item
        Next
        Sb.Append(scriptvar)









        Dim fs As New FileStream(EudPlibFilePath & "\scadatafile", FileMode.Create)
        Dim sw As New StreamWriter(fs)

        Dim checkstring As String = HttpTool.httpRequest("encrypt256", "key=" & WebUtility.UrlEncode(ConnectKey) & "&data=" & WebUtility.UrlEncode(Sb.ToString))
        sw.Write(checkstring)


        sw.Close()
        fs.Close()


        Return True
    End Function


    Public Sub WriteSCAScriptZipFile()
        If File.Exists(EudPlibFilePath & "\scascript") Then
            File.Delete(EudPlibFilePath & "\scascript")
        End If
        If Directory.Exists(EudPlibFilePath & "\scascript") Then
            Directory.Delete(EudPlibFilePath & "\scascript", True)
        End If


        If File.Exists(EudPlibFilePath & "\scascripttemp") Then
            File.Delete(EudPlibFilePath & "\scascripttemp")
        End If
        If Directory.Exists(EudPlibFilePath & "\scascripttemp") Then
            Directory.Delete(EudPlibFilePath & "\scascripttemp", True)
        End If
        Directory.CreateDirectory(EudPlibFilePath & "\scascripttemp")


        Dim Sb As New StringBuilder
        Sb.AppendLine("-- SCAScript")
        Dim tefiles As List(Of TEFile) = pjData.TEData.GetAllTEFile(TEFile.EFileType.SCAScript)
        For Each item In tefiles
            Dim scriptEditor As SCAScriptEditor = item.Scripter
            Sb.Append(scriptEditor.GetFileText(""))
        Next
        Dim crc As CRC32 = New CRC32()

        Dim fs As New FileStream(EudPlibFilePath & "\scascripttemp\script", FileMode.Create)
        Dim sw As New StreamWriter(fs)

        sw.Write(Sb.ToString)

        sw.Close()
        fs.Close()

        '파일 생성
        For Each item In pjData.TEData.SCAImageDatas
            item.SaveToFile(EudPlibFilePath & "\scascripttemp\")
        Next

        Dim zip As New ZipFile(EudPlibFilePath & "\scascript")

        zip.Password = ConnectKey
        zip.AddDirectory(EudPlibFilePath & "\scascripttemp")
        zip.Save()

    End Sub


End Class
