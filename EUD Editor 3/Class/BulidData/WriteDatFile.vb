Imports System.IO
Imports System.Text

Partial Public Class BuildData
    Private Sub WriteDatFile()
        Dim sb As New StringBuilder

        sb.AppendLine("from eudplib import *")
        sb.AppendLine("")
        sb.AppendLine("")
        sb.AppendLine("def onPluginStart():")
        sb.AppendLine("    DoActions([  # Basic DatFile Actions")

        '기본 Dat파일 저장
        For DatFile = 0 To SCDatFiles.DatFiles.orders
            For Pindex = 0 To pjData.Dat.DatFileList(DatFile).ParameterList.Count - 1
                Dim ParameterData As SCDatFiles.CDatFile.CParamater = pjData.Dat.DatFileList(DatFile).ParameterList(Pindex)

                If Not ParameterData.GetInfo(SCDatFiles.EParamInfo.IsEnabled) Then
                    Continue For
                End If
                Dim Parameter As String = ParameterData.GetParamname
                Dim Offset As UInteger = Tool.GetOffset(DatFile, Parameter)
                Dim Size As Byte = ParameterData.GetInfo(SCDatFiles.EParamInfo.Size)
                Dim Length As Byte = Size * ParameterData.GetInfo(SCDatFiles.EParamInfo.VarArray)


                For ObjectID = 0 To ParameterData.GetValueCount - 1
                    If (Not ParameterData.PureData(ObjectID).Enabled) Or
                        ParameterData.PureData(ObjectID).IsDefault Then
                        Continue For
                    End If
                    Dim OldValue As Long
                    Dim NewValue As Long = ParameterData.PureData(ObjectID).Data
                    Dim CurrentValue As Long
                    Dim CalOffset As UInteger = Offset + ObjectID * Length
                    Dim ByteShift As Byte = CalOffset Mod 4
                    Dim RealValue As Long
                    Dim RealOffset As UInteger = CalOffset - CalOffset Mod 4

                    Try
                        If pjData.MapData.DatFile.GetDatFile(DatFile).GetParamValue(Parameter, ObjectID).IsDefault Then '맵 데이터가 없을 경우
                            OldValue = scData.DefaultDat.GetDatFile(DatFile).ParameterList(Pindex).PureData(ObjectID).Data
                        Else
                            OldValue = pjData.MapData.DatFile.GetDatFile(DatFile).ParameterList(Pindex).PureData(ObjectID).Data
                        End If
                    Catch ex As Exception
                        OldValue = scData.DefaultDat.GetDatFile(DatFile).ParameterList(Pindex).PureData(ObjectID).Data
                    End Try


                    CurrentValue = NewValue - OldValue
                    RealValue = CurrentValue * Math.Pow(256, ByteShift)

                    sb.Append("        SetMemory(0x" & Hex(RealOffset).ToUpper & ", Add, " & RealValue & "),")


                    'Select Case Size
                    '    Case 4
                    '        sb.Append("    f_dwwrite_epd(EPD(0x" & Hex(Offset + ObjectID * Length).ToUpper & ")," & ParamaterData.PureData(ObjectID).Data & ")")
                    '    Case 2
                    '        sb.Append("    f_wwrite_epd(EPD(0x" & Hex(Offset + ObjectID * Length).ToUpper & ")," & ParamaterData.PureData(ObjectID).Data & ", " & (Offset + ObjectID * Length) Mod 4 & ")")
                    '    Case 1
                    '        sb.Append("    f_bwrite_epd(EPD(0x" & Hex(Offset + ObjectID * Length).ToUpper & ")," & ParamaterData.PureData(ObjectID).Data & ", " & (Offset + ObjectID * Length) Mod 4 & ")")
                    'End Select


                    sb.AppendLine("# " & Datfilesname(DatFile) & ":" & Parameter & "  index:" & ObjectID & "    from " & OldValue & " To " & NewValue)
                Next
                'pjData.Dat.Data(DatFile, Parameter, ObjectID)
            Next
        Next

        sb.AppendLine("    ])")
        sb.AppendLine("")


        '[EUDEditor.py]

        '[dataDumper]
        'D\:\source\repos\EUDEditor\EUD Editor\bin\x86\Release\Data\temp\RequireData : 0x58D740, copy

        '[TriggerEditor.eps]


        Dim filestreama As New FileStream(DatpyFilePath, FileMode.Create)
        Dim strWriter As New StreamWriter(filestreama, Encoding.UTF8)

        strWriter.Write(sb.ToString)

        strWriter.Close()
        filestreama.Close()
    End Sub


    Private Sub WriteExtraDatFile()
        Dim sb As New StringBuilder

        sb.AppendLine("import WireFrameDataEditor")
        sb.AppendLine("from eudplib import *")

        'WriteWireFrame(sb)
        sb.AppendLine("")
        sb.AppendLine("")
        sb.AppendLine("def onPluginStart():")
        sb.AppendLine("    try:")
        sb.AppendLine("        init_wireframe()")
        sb.AppendLine("    except NameError:")
        sb.AppendLine("        pass")

        '와이어프레임, 버튼셋, 요구사항, 상태플래그
        Try
            WriteStatusInfor(sb)
        Catch ex As Exception
            Tool.ErrorMsgBox(Tool.GetText("Error WriteStatusInfor"), ex.ToString)
        End Try
        WriteButtonSet(sb)
        sb.AppendLine("    inputData = open('" & Tool.GetRelativePath(EdsFilePath, requireFilePath).Replace("\", "/") & "', 'rb').read()")
        sb.AppendLine("    inputData_db = Db(inputData)")
        sb.AppendLine("    inputDwordN = (len(inputData) + 3) // 4")
        sb.AppendLine("")
        sb.AppendLine("    addrEPD = EPD(0x" & Hex(Tool.GetOffset("FG_ReqUnit")) & ")")
        sb.AppendLine("    f_repmovsd_epd(addrEPD, EPD(inputData_db), inputDwordN)")

        'sb.AppendLine("    DoActions([ # RequreData 포인터 작성")
        'sb.AppendLine("        SetMemory(0x" & Hex(Tool.GetOffset("Vanilla") + 500) & ", SetTo, 0x" & Hex(Tool.GetOffset("FG_ReqUnit")) & ")")
        'sb.AppendLine("    ])")

        sb.AppendLine("")
        sb.AppendLine("")


        sb.AppendLine("def beforeTriggerExec():")
        WriteRequirment(sb)
        sb.AppendLine("    ")
        sb.AppendLine("")

        '[EUDEditor.py]

        '[TriggerEditor.eps]

        '[dataDumper]
        'D\:\source\repos\EUDEditor\EUD Editor\bin\x86\Release\Data\temp\RequireData : 0x58D740, copy

        Dim filestreama As New FileStream(ExtraDatpyFilePath, FileMode.Create)
        Dim strWriter As New StreamWriter(filestreama, Encoding.UTF8)

        strWriter.Write(sb.ToString)

        strWriter.Close()
        filestreama.Close()
    End Sub


    Private Sub WriteStatusInfor(sb As StringBuilder)
        sb.AppendLine("    DoActions([ # 스테이터스인포메이션")
        For i = 0 To SCUnitCount - 1
            If Not pjData.ExtraDat.DefaultStatusFunction1(i) Then
                Dim _lastvalue As Long = scData.statusFnVal1(pjData.ExtraDat.StatusFunction1(i))

                Dim _offsetNum As Long = Tool.GetOffset("FG_Status") + 12 * i
                Dim _offset As String = Hex(_offsetNum)

                sb.AppendLine("        SetMemory(0x" & _offset & ", SetTo, " & _lastvalue & "),")
            End If
            If Not pjData.ExtraDat.DefaultStatusFunction2(i) Then
                Dim _lastvalue As Long = scData.statusFnVal2(pjData.ExtraDat.StatusFunction2(i))

                Dim _offsetNum As Long = Tool.GetOffset("FG_Display") + 12 * i
                Dim _offset As String = Hex(_offsetNum)

                sb.AppendLine("        SetMemory(0x" & _offset & ", SetTo, " & _lastvalue & "),")
            End If
        Next
        sb.AppendLine("    ])")
    End Sub


    Private Sub WriteWireFrame(sb As StringBuilder)
        '바뀐게 있는지 체크.

        File.Copy(Tool.DataPath("TriggerEditor\wireframe.eps"), WireFrameEpsFilePath, True)
        sb.AppendLine("try:")
        sb.AppendLine("    InitialWireframe")
        sb.AppendLine("except NameError:")
        sb.AppendLine("")
        sb.AppendLine("    def init_wireframe():")
        sb.AppendLine("        WireFrameDataEditor.WireFrameInit()")
        'E4 234 wireframe
        '6A 106 tranwire
        '83 131 grpwire

        Dim wireframecount As Integer = 228
        Dim tranwirecount As Integer = 106
        Dim grpwirecount As Integer = 131

        For i = 0 To wireframecount - 1
            If Not pjData.ExtraDat.DefaultWireFrame(i) Then
                Dim wirenewframe As Byte = pjData.ExtraDat.WireFrame(i)
                Dim trannewframe As Byte = pjData.ExtraDat.TranFrame(i)
                Dim grpnewframe As Byte = pjData.ExtraDat.GrpFrame(i)


                sb.AppendLine("        WireFrameDataEditor.ChangeWireframe(" & i & ", " & pjData.ExtraDat.WireFrame(i) & ")")
                If (i <= tranwirecount - 1) And (trannewframe <= tranwirecount - 1) Then
                    sb.AppendLine("        WireFrameDataEditor.ChangeTranframe(" & i & ", " & pjData.ExtraDat.TranFrame(i) & ")")
                End If

                If (i <= grpwirecount - 1) And (grpnewframe <= grpwirecount - 1) Then
                    sb.AppendLine("        WireFrameDataEditor.ChangeGrpframe(" & i & ", " & pjData.ExtraDat.GrpFrame(i) & ")")
                End If

            End If
        Next
        sb.AppendLine("")
        sb.AppendLine("else:") 'euddraft 0.9.8.6부터 지원 (와프 초기화 트리거 용량 최소화)
        For i = 0 To wireframecount - 1
            If Not pjData.ExtraDat.DefaultWireFrame(i) Then
                Dim wirenewframe As Byte = pjData.ExtraDat.WireFrame(i)
                Dim trannewframe As Byte = pjData.ExtraDat.TranFrame(i)
                Dim grpnewframe As Byte = pjData.ExtraDat.GrpFrame(i)


                sb.AppendLine("    InitialWireframe.wirefram(" & i & ", " & pjData.ExtraDat.WireFrame(i) & ")")
                If (i <= tranwirecount - 1) And (trannewframe <= tranwirecount - 1) Then
                    sb.AppendLine("    InitialWireframe.tranwire(" & i & ", " & pjData.ExtraDat.TranFrame(i) & ")")
                End If

                If (i <= grpwirecount - 1) And (grpnewframe <= grpwirecount - 1) Then
                    sb.AppendLine("    InitialWireframe.grpwire(" & i & ", " & pjData.ExtraDat.GrpFrame(i) & ")")
                End If
            End If
        Next





        'Dim checkflag As Boolean = False
        'For i = 0 To SCUnitCount - 1
        '    If Not pjData.ExtraDat.DefaultWireFrame(i) Then
        '        checkflag = True
        '        Exit For
        '    ElseIf Not pjData.ExtraDat.DefaultGrpFrame(i) Then
        '        checkflag = True
        '        Exit For
        '    End If
        'Next
        'If checkflag = False Then
        '    For i = 0 To SCMenCount - 1
        '        If Not pjData.ExtraDat.DefaultTranFrame(i) Then
        '            checkflag = True
        '            Exit For
        '        End If
        '    Next
        'End If


        'If checkflag Then



        'Dim fs As FileStream
        'Dim fs64 As FileStream
        'Dim br As BinaryReader
        'Dim br64 As BinaryReader

        'Dim grpframecount As UInt16

        'sb.AppendLine("    # 와이어 프레임")

        ''#########################################################################################



        'fs = New FileStream(Tool.DataPath("wirefram.grp"), FileMode.Open)
        'br = New BinaryReader(fs)


        'fs64 = New FileStream(Tool.DataPath("wirefram64.grp"), FileMode.Open)
        'br64 = New BinaryReader(fs64)


        'grpframecount = br.ReadUInt16
        'Dim grpdata(grpframecount - 1) As UInt64
        'Dim grpdata64(grpframecount - 1) As UInt64
        'fs.Position = 6
        'For i = 0 To grpframecount - 1
        '    grpdata(i) = br.ReadUInt64
        '    grpdata64(i) = br64.ReadUInt64
        'Next
        'br.Close()
        'fs.Close()


        'br64.Close()
        'fs64.Close()


        'sb.AppendLine("    WireOffset = f_epdread_epd(EPD(0x" & Hex(Tool.GetOffset("wirefram.grp")) & "))")
        'sb.AppendLine("    DoActions([")
        'For i = 0 To grpframecount - 1
        '    If Not pjData.ExtraDat.DefaultWireFrame(i) Then
        '        Dim offsetName As String = "WireOffset"

        '        Dim newframe As Byte = pjData.ExtraDat.WireFrame(i)
        '        If newframe >= grpframecount Then
        '            Continue For
        '        End If

        '        Dim bytes() As Byte = BitConverter.GetBytes(grpdata(newframe))


        '        Dim v1 As UShort = bytes(0) + bytes(1) * 256
        '        Dim v2 As UInteger = bytes(2) + bytes(3) * 256 + CUInt(bytes(4)) * CUInt(65536) + CUInt(bytes(5)) * CUInt(16777216)
        '        Dim v3 As UShort = bytes(6) + bytes(7) * 256
        '        sb.AppendLine("        SetMemoryXEPD(" & offsetName & " + " & 1 + 2 * i & ", SetTo, " & v1 * 65536 & ", 0xFFFF0000),")
        '        sb.AppendLine("        SetMemoryEPD(" & offsetName & " + " & 2 + 2 * i & ", SetTo, " & v2 & "),")
        '        sb.AppendLine("        SetMemoryXEPD(" & offsetName & " + " & 3 + 2 * i & ", SetTo, " & v3 & ", 0xFFFF),")
        '    End If
        'Next
        'sb.AppendLine("    ])")



        ''#########################################################################################

        'fs = New FileStream(Tool.DataPath("grpwire.grp"), FileMode.Open)
        'br = New BinaryReader(fs)

        'grpframecount = br.ReadUInt16
        'ReDim grpdata(grpframecount - 1)
        'fs.Position = 6
        'For i = 0 To grpframecount - 1
        '    grpdata(i) = br.ReadUInt64
        'Next
        'br.Close()
        'fs.Close()


        'sb.AppendLine("    GrpOffset = f_epdread_epd(EPD(0x" & Hex(Tool.GetOffset("grpwire.grp")) & "))")
        'sb.AppendLine("    DoActions([")
        'For i = 0 To grpframecount - 1
        '    If Not pjData.ExtraDat.DefaultGrpFrame(i) Then
        '        Dim offsetName As String = "GrpOffset"

        '        Dim newframe As Byte = pjData.ExtraDat.GrpFrame(i)
        '        If newframe >= grpframecount Then
        '            Continue For
        '        End If

        '        Dim bytes() As Byte = BitConverter.GetBytes(grpdata(newframe))


        '        Dim v1 As UShort = bytes(0) + bytes(1) * 256
        '        Dim v2 As UInteger = bytes(2) + bytes(3) * 256 + CUInt(bytes(4)) * CUInt(65536) + CUInt(bytes(5)) * CUInt(16777216)
        '        Dim v3 As UShort = bytes(6) + bytes(7) * 256
        '        sb.AppendLine("        SetMemoryXEPD(" & offsetName & " + " & 1 + 2 * i & ", SetTo, " & v1 * 65536 & ", 0xFFFF0000),")
        '        sb.AppendLine("        SetMemoryEPD(" & offsetName & " + " & 2 + 2 * i & ", SetTo, " & v2 & "),")
        '        sb.AppendLine("        SetMemoryXEPD(" & offsetName & " + " & 3 + 2 * i & ", SetTo, " & v3 & ", 0xFFFF),")
        '    End If
        'Next
        'sb.AppendLine("    ])")


        ''#########################################################################################

        'fs = New FileStream(Tool.DataPath("tranwire.grp"), FileMode.Open)
        'br = New BinaryReader(fs)

        'grpframecount = br.ReadUInt16
        'ReDim grpdata(grpframecount - 1)
        'fs.Position = 6
        'For i = 0 To grpframecount - 1
        '    grpdata(i) = br.ReadUInt64
        'Next
        'br.Close()
        'fs.Close()


        'sb.AppendLine("    tranOffset = f_epdread_epd(EPD(0x" & Hex(Tool.GetOffset("tranwire.grp")) & "))")
        'sb.AppendLine("    DoActions([")
        'For i = 0 To grpframecount - 1
        '    If Not pjData.ExtraDat.DefaultTranFrame(i) Then
        '        Dim offsetName As String = "tranOffset"

        '        Dim newframe As Byte = pjData.ExtraDat.TranFrame(i)
        '        If newframe >= grpframecount Then
        '            Continue For
        '        End If

        '        Dim bytes() As Byte = BitConverter.GetBytes(grpdata(newframe))


        '        Dim v1 As UShort = bytes(0) + bytes(1) * 256
        '        Dim v2 As UInteger = bytes(2) + bytes(3) * 256 + CUInt(bytes(4)) * CUInt(65536) + CUInt(bytes(5)) * CUInt(16777216)
        '        Dim v3 As UShort = bytes(6) + bytes(7) * 256
        '        sb.AppendLine("        SetMemoryXEPD(" & offsetName & " + " & 1 + 2 * i & ", SetTo, " & v1 * 65536 & ", 0xFFFF0000),")
        '        sb.AppendLine("        SetMemoryEPD(" & offsetName & " + " & 2 + 2 * i & ", SetTo, " & v2 & "),")
        '        sb.AppendLine("        SetMemoryXEPD(" & offsetName & " + " & 3 + 2 * i & ", SetTo, " & v3 & ", 0xFFFF),")
        '    End If
        'Next
        'sb.AppendLine("    ])")


        ''#########################################################################################

        'End If
    End Sub

    'Public Sub _WriteWireFrame(sb As StringBuilder, filename As String)
    '    Dim fs As FileStream
    '    Dim fs64 As FileStream
    '    Dim br As BinaryReader
    '    Dim br64 As BinaryReader

    '    Dim grpframecount As UInt16

    '    sb.AppendLine("    # 와이어 프레임")

    '    '#########################################################################################


    '    For i = 0 To grpframecount - 1
    '        If Not pjData.ExtraDat.DefaultWireFrame(i) Then
    '            Dim newframe As Byte = pjData.ExtraDat.WireFrame(i)
    '            If newframe >= grpframecount Then
    '                Continue For
    '            End If

    '            Dim bytes() As Byte = BitConverter.GetBytes(grpdata(newframe))


    '            Dim v1 As UShort = bytes(0) + bytes(1) * 256
    '            Dim v2 As UInteger = bytes(2) + bytes(3) * 256 + CUInt(bytes(4)) * CUInt(65536) + CUInt(bytes(5)) * CUInt(16777216)
    '            Dim v3 As UShort = bytes(6) + bytes(7) * 256
    '            sb.AppendLine("        WireFrameDataEditor.WireFrameInit()")
    '        End If
    '    Next
    '    sb.AppendLine("    ])")

    'End Sub



End Class
