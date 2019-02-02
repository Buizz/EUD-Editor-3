Imports System.IO
Imports System.Text

Partial Public Class BuildData
    Private Sub WriteDatFile()
        Dim sb As New StringBuilder

        sb.AppendLine("from eudplib import *")
        sb.AppendLine("")
        sb.AppendLine("def onPluginStart():")
        sb.AppendLine("    DoActions([]) # NULL에러 방지용")
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


        sb.AppendLine("def beforeTriggerExec():")
        sb.AppendLine("    DoActions([]) # NULL에러 방지용")
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
End Class
