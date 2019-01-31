Imports System.IO
Imports System.Text

Partial Public Class BuildData
    Private Sub WriteDatFile()
        Dim sb As New StringBuilder

        sb.AppendLine("from eudplib import *")
        sb.AppendLine("")
        sb.AppendLine("def onPluginStart():")
        sb.AppendLine("    DoActions([]) # NULL에러 방지용")

        '기본 Dat파일 저장
        For DatFile = 0 To SCDatFiles.DatFiles.orders - 1
            For Pindex = 0 To pjData.Dat.DatFileList(DatFile).ParamaterList.Count - 1
                Dim ParamaterData As SCDatFiles.CDatFile.CParamater = pjData.Dat.DatFileList(DatFile).ParamaterList(Pindex)

                If ParamaterData.GetInfo(SCDatFiles.EParamInfo.IsEnabled) Then
                    Dim Parameter As String = ParamaterData.GetParamname
                    Dim Offset As UInteger = Tool.GetOffset(DatFile, Parameter)
                    Dim Size As Byte = ParamaterData.GetInfo(SCDatFiles.EParamInfo.Size)


                    For ObjectID = 0 To ParamaterData.GetValueCount - 1
                        If ParamaterData.PureData(ObjectID).Enabled And Not ParamaterData.PureData(ObjectID).IsDefault Then
                            Select Case Size
                                Case 4
                                    sb.Append("    f_dwwrite(0x" & Hex(Offset + ObjectID * Size).ToUpper & "," & ParamaterData.PureData(ObjectID).Data & ")")
                                Case 2
                                    sb.Append("    f_wwrite(0x" & Hex(Offset + ObjectID * Size).ToUpper & "," & ParamaterData.PureData(ObjectID).Data & ")")
                                Case 1
                                    sb.Append("    f_bwrite(0x" & Hex(Offset + ObjectID * Size).ToUpper & "," & ParamaterData.PureData(ObjectID).Data & ")")
                            End Select

                            sb.AppendLine("# " & Datfilesname(DatFile) & ":" & Parameter & "  index:" & ObjectID & "  To " & ParamaterData.PureData(ObjectID).Data)
                        End If
                    Next
                    'pjData.Dat.Data(DatFile, Parameter, ObjectID)
                End If
            Next
        Next
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
