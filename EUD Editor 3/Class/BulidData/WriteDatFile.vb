Imports System.IO
Imports System.Text

Partial Public Class BuildData
    Private Sub WriteDatFile()
        Dim sb As New StringBuilder

        sb.AppendLine("from eudplib import *")
        sb.AppendLine("")
        sb.AppendLine("def onPluginStart():")
        sb.AppendLine("    pass # NULL에러 방지용")
        sb.AppendLine("    DoActions([ # 기본 Dat파일 액션")

        '기본 Dat파일 저장
        For DatFile = 0 To SCDatFiles.DatFiles.orders - 1
            For Pindex = 0 To pjData.Dat.DatFileList(DatFile).ParamaterList.Count - 1
                Dim ParamaterData As SCDatFiles.CDatFile.CParamater = pjData.Dat.DatFileList(DatFile).ParamaterList(Pindex)

                If ParamaterData.GetInfo(SCDatFiles.EParamInfo.IsEnabled) Then
                    Dim Parameter As String = ParamaterData.GetParamname
                    Dim Offset As UInteger = Tool.GetOffset(DatFile, Parameter)
                    Dim Size As Byte = ParamaterData.GetInfo(SCDatFiles.EParamInfo.Size)
                    Dim Length As Byte = Size * ParamaterData.GetInfo(SCDatFiles.EParamInfo.VarArray)


                    For ObjectID = 0 To ParamaterData.GetValueCount - 1
                        If ParamaterData.PureData(ObjectID).Enabled And Not ParamaterData.PureData(ObjectID).IsDefault Then
                            Dim OldValue As Long
                            Dim NewValue As Long = ParamaterData.PureData(ObjectID).Data
                            Dim CurrentValue As Long
                            Dim CalOffset As UInteger = Offset + ObjectID * Length
                            Dim ByteShift As Byte = CalOffset Mod 4
                            Dim RealValue As Long
                            Dim RealOffset As UInteger = CalOffset - CalOffset Mod 4


                            If pjData.MapData.DatFile.GetDatFile(DatFile).GetParamValue(Parameter, ObjectID).IsDefault Then '맵 데이터가 없을 경우
                                OldValue = scData.DefaultDat.GetDatFile(DatFile).ParamaterList(Pindex).PureData(ObjectID).Data
                            Else
                                OldValue = pjData.MapData.DatFile.GetDatFile(DatFile).ParamaterList(Pindex).PureData(ObjectID).Data
                            End If

                            CurrentValue = NewValue - OldValue
                            RealValue = CurrentValue * Math.Pow(256, ByteShift)

                            sb.Append("        SetMemoryEPD(EPD(0x" & Hex(RealOffset).ToUpper & "),Add ," & RealValue & "),")


                            'Select Case Size
                            '    Case 4
                            '        sb.Append("    f_dwwrite_epd(EPD(0x" & Hex(Offset + ObjectID * Length).ToUpper & ")," & ParamaterData.PureData(ObjectID).Data & ")")
                            '    Case 2
                            '        sb.Append("    f_wwrite_epd(EPD(0x" & Hex(Offset + ObjectID * Length).ToUpper & ")," & ParamaterData.PureData(ObjectID).Data & ", " & (Offset + ObjectID * Length) Mod 4 & ")")
                            '    Case 1
                            '        sb.Append("    f_bwrite_epd(EPD(0x" & Hex(Offset + ObjectID * Length).ToUpper & ")," & ParamaterData.PureData(ObjectID).Data & ", " & (Offset + ObjectID * Length) Mod 4 & ")")
                            'End Select


                            sb.AppendLine("# " & Datfilesname(DatFile) & ":" & Parameter & "  index:" & ObjectID & "    from " & OldValue & " To " & NewValue)
                        End If
                    Next
                    'pjData.Dat.Data(DatFile, Parameter, ObjectID)
                End If
            Next
        Next

        sb.AppendLine("    ])")
        sb.AppendLine("    ")
        sb.AppendLine("")



        '[EUDEditor.py]

        '[TriggerEditor.eps]

        '[dataDumper]
        'D\:\source\repos\EUDEditor\EUD Editor\bin\x86\Release\Data\temp\RequireData : 0x58D740, copy

        Dim filestreama As New FileStream(DatpyFilePath, FileMode.Create)
        Dim strWriter As New StreamWriter(filestreama, Encoding.UTF8)

        strWriter.Write(sb.ToString)

        strWriter.Close()
        filestreama.Close()
    End Sub


    Private Sub WriteExtraDatFile()
        Dim sb As New StringBuilder

        sb.AppendLine("from eudplib import *")
        sb.AppendLine("")
        sb.AppendLine("def onPluginStart():")
        sb.AppendLine("    pass # NULL에러 방지용")

        '와이어프레임, 버튼셋, 요구사항, 상태플래그
        WriteWireFrame(sb)
        WriteStatusInfor(sb)
        WriteButtonSet(sb)
        sb.AppendLine("    DoActions([ # RequreData 포인터 작성")
        sb.AppendLine("       SetMemory(0x" & Hex(Tool.GetOffset("Vanilla")) & ", SetTo, 0x" & Hex(Tool.GetOffset("FG_ReqUnit")) & ")")
        sb.AppendLine("    ])")

        sb.AppendLine("    ")
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

                sb.AppendLine("       SetMemory(" & "0x" & _offset & ", SetTo, " & _lastvalue & "),")
            End If
            If Not pjData.ExtraDat.DefaultStatusFunction2(i) Then
                Dim _lastvalue As Long = scData.statusFnVal2(pjData.ExtraDat.StatusFunction2(i))

                Dim _offsetNum As Long = Tool.GetOffset("FG_Display") + 12 * i
                Dim _offset As String = Hex(_offsetNum)

                sb.AppendLine("       SetMemory(" & "0x" & _offset & ", SetTo, " & _lastvalue & "),")
            End If
        Next
        sb.AppendLine("    ])")
    End Sub
    Private Sub WriteWireFrame(sb As StringBuilder)
        '바뀐게 있는지 체크.
        Dim checkflag As Boolean = False
        For i = 0 To SCUnitCount - 1
            If Not pjData.ExtraDat.DefaultWireFrame(i) Then
                checkflag = True
                Exit For
            End If
        Next
        If checkflag = False Then
            For i = 0 To SCUnitCount - 1
                If Not pjData.ExtraDat.DefaultGrpFrame(i) Then
                    checkflag = True
                    Exit For
                End If
            Next
            If checkflag = False Then
                For i = 0 To SCMenCount - 1
                    If Not pjData.ExtraDat.DefaultTranFrame(i) Then
                        checkflag = True
                        Exit For
                    End If
                Next
            End If
        End If


        If checkflag Then
            Dim memStream As MemoryStream
            Dim binaryReader As BinaryReader
            Dim binaryWriter As BinaryWriter

            Dim grpframecount As UInt16

            sb.AppendLine("    # 와이어 프레임")

            '#########################################################################################

            memStream = New MemoryStream(Tool.LoadDataFromMPQ("unit\wirefram\wirefram.grp"))
            binaryReader = New BinaryReader(memStream)
            binaryWriter = New BinaryWriter(memStream)

            grpframecount = binaryReader.ReadUInt16
            Dim grpdata(grpframecount - 1) As UInt64
            For i = 0 To grpframecount - 1
                If Not pjData.ExtraDat.DefaultWireFrame(i) Then
                    memStream.Position = 6 + 8 * pjData.ExtraDat.WireFrame(i)
                    grpdata(i) = binaryReader.ReadUInt64
                End If
            Next

            For i = 0 To grpframecount - 1
                If Not pjData.ExtraDat.DefaultWireFrame(i) Then
                    memStream.Position = 6 + 8 * i
                    binaryWriter.Write(grpdata(i))
                End If
            Next


            sb.AppendLine("    WireOffset = f_dwread_epd(EPD(0x" & Hex(Tool.GetOffset("wirefram.grp")) & "))")
            sb.AppendLine("    DoActions([")
            For i = 0 To grpframecount - 1
                If Not pjData.ExtraDat.DefaultWireFrame(i) Then
                    memStream.Position = 4 + 8 * i
                    sb.AppendLine("    SetMemoryEPD(EPD(WireOffset + " & 4 + 8 * i & "), SetTo, " & binaryReader.ReadUInt32 & "),")
                    sb.AppendLine("    SetMemoryEPD(EPD(WireOffset + " & 8 + 8 * i & "), SetTo, " & binaryReader.ReadUInt32 & "),")
                    sb.AppendLine("    SetMemoryEPD(EPD(WireOffset + " & 12 + 8 * i & "), SetTo, " & binaryReader.ReadUInt32 & "),")
                End If
            Next
            sb.AppendLine("    ])")

            binaryReader.Close()
            binaryWriter.Close()
            memStream.Close()

            '#########################################################################################

            memStream = New MemoryStream(Tool.LoadDataFromMPQ("unit\wirefram\grpwire.grp"))
            binaryReader = New BinaryReader(memStream)
            binaryWriter = New BinaryWriter(memStream)

            grpframecount = binaryReader.ReadUInt16
            ReDim grpdata(grpframecount - 1)
            For i = 0 To grpframecount - 1
                If Not pjData.ExtraDat.DefaultGrpFrame(i) Then
                    memStream.Position = 6 + 8 * pjData.ExtraDat.GrpFrame(i)

                    grpdata(i) = binaryReader.ReadUInt64
                End If
            Next

            For i = 0 To grpframecount - 1
                If Not pjData.ExtraDat.DefaultGrpFrame(i) Then
                    memStream.Position = 6 + 8 * i
                    binaryWriter.Write(grpdata(i))
                End If
            Next


            sb.AppendLine("    GrpOffset = f_dwread_epd(EPD(0x" & Hex(Tool.GetOffset("grpwire.grp")) & "))")
            sb.AppendLine("    DoActions([")
            For i = 0 To grpframecount - 1
                If Not pjData.ExtraDat.DefaultGrpFrame(i) Then
                    memStream.Position = 4 + 8 * i
                    sb.AppendLine("    SetMemoryEPD(EPD(GrpOffset + " & 4 + 8 * i & "), SetTo, " & binaryReader.ReadUInt32 & "),")
                    sb.AppendLine("    SetMemoryEPD(EPD(GrpOffset + " & 8 + 8 * i & "), SetTo, " & binaryReader.ReadUInt32 & "),")
                    sb.AppendLine("    SetMemoryEPD(EPD(GrpOffset + " & 12 + 8 * i & "), SetTo, " & binaryReader.ReadUInt32 & "),")
                End If
            Next
            sb.AppendLine("    ])")

            binaryReader.Close()
            binaryWriter.Close()
            memStream.Close()

            '#########################################################################################

            memStream = New MemoryStream(Tool.LoadDataFromMPQ("unit\wirefram\tranwire.grp"))
            binaryReader = New BinaryReader(memStream)
            binaryWriter = New BinaryWriter(memStream)

            grpframecount = binaryReader.ReadUInt16
            ReDim grpdata(grpframecount - 1)
            For i = 0 To grpframecount - 1
                If Not pjData.ExtraDat.DefaultTranFrame(i) Then
                    memStream.Position = 6 + 8 * pjData.ExtraDat.TranFrame(i)

                    grpdata(i) = binaryReader.ReadUInt64
                End If
            Next

            For i = 0 To grpframecount - 1
                If Not pjData.ExtraDat.DefaultTranFrame(i) Then
                    memStream.Position = 6 + 8 * i
                    binaryWriter.Write(grpdata(i))
                End If
            Next


            sb.AppendLine("    tranOffset = f_dwread_epd(EPD(0x" & Hex(Tool.GetOffset("tranwire.grp")) & "))")
            sb.AppendLine("    DoActions([")
            For i = 0 To grpframecount - 1
                If Not pjData.ExtraDat.DefaultTranFrame(i) Then
                    memStream.Position = 4 + 8 * i
                    sb.AppendLine("    SetMemoryEPD(EPD(tranOffset + " & 4 + 8 * i & "), SetTo, " & binaryReader.ReadUInt32 & "),")
                    sb.AppendLine("    SetMemoryEPD(EPD(tranOffset + " & 8 + 8 * i & "), SetTo, " & binaryReader.ReadUInt32 & "),")
                    sb.AppendLine("    SetMemoryEPD(EPD(tranOffset + " & 12 + 8 * i & "), SetTo, " & binaryReader.ReadUInt32 & "),")
                End If
            Next
            sb.AppendLine("    ])")

            binaryReader.Close()
            binaryWriter.Close()
            memStream.Close()

            '#########################################################################################





        End If
    End Sub

End Class
