Imports System.IO
Imports System.Text

Partial Public Class BuildData
    Private Sub WriteRequireData()
        Dim Ptr() As UInt16 = {1096, 840, 320, 688, 1316}

        Dim fileCreator As New FileStream(requireFilePath, FileMode.Create)
        Dim filebinaryw As New BinaryWriter(fileCreator)

        Dim Datfiles() As SCDatFiles.DatFiles = {SCDatFiles.DatFiles.units, SCDatFiles.DatFiles.upgrades, SCDatFiles.DatFiles.techdata, SCDatFiles.DatFiles.Stechdata, SCDatFiles.DatFiles.orders}

        For i = 0 To 4
            Dim StartOffset As UInteger = 0
            fileCreator.Position = 0
            For j = 0 To i - 1
                fileCreator.Position += Ptr(j)
            Next
            StartOffset = fileCreator.Position \ 2

            filebinaryw.Write(CUShort(0))

            For ObjID = 0 To SCCodeCount(Datfiles(i)) - 1
                Dim pos As Integer = pjData.ExtraDat.RequireData(Datfiles(i)).GetRequireObject(ObjID).StartPos


                Select Case pjData.ExtraDat.RequireData(Datfiles(i)).RequireObjectUsed(ObjID)
                    Case CRequireData.RequireUse.DefaultUse
                        If pos <> 0 Then

                            If i = 4 Then
                                filebinaryw.Write(CUShort(ObjID)) '시작 부호 입력
                            End If


                            pjData.ExtraDat.RequireData(Datfiles(i)).GetRequireObject(ObjID).StartPos = fileCreator.Position \ 2 - StartOffset
                            pjData.ExtraDat.RequireData(Datfiles(i)).WriteBinaryData(ObjID, filebinaryw)


                            filebinaryw.Write(CUShort(&HFFFF))
                        End If
                    Case CRequireData.RequireUse.DontUse
                        pjData.ExtraDat.RequireData(Datfiles(i)).GetRequireObject(ObjID).StartPos = 0
                    Case CRequireData.RequireUse.AlwaysUse
                        If i = 4 Then
                            filebinaryw.Write(CUShort(ObjID)) '시작 부호 입력
                        End If
                        pjData.ExtraDat.RequireData(Datfiles(i)).GetRequireObject(ObjID).StartPos = fileCreator.Position \ 2 - StartOffset
                        filebinaryw.Write(CUShort(&HFFFF))
                    Case CRequireData.RequireUse.AlwaysCurrentUse
                        If i = 4 Then
                            filebinaryw.Write(CUShort(ObjID)) '시작 부호 입력
                        End If

                        pjData.ExtraDat.RequireData(Datfiles(i)).GetRequireObject(ObjID).StartPos = fileCreator.Position \ 2 - StartOffset
                        pjData.ExtraDat.RequireData(Datfiles(i)).WriteBinaryData(ObjID, filebinaryw)


                        filebinaryw.Write(CUShort(&HFFFF))
                    Case CRequireData.RequireUse.CustomUse
                        If i = 4 Then
                            filebinaryw.Write(CUShort(ObjID)) '시작 부호 입력
                        End If

                        pjData.ExtraDat.RequireData(Datfiles(i)).GetRequireObject(ObjID).StartPos = fileCreator.Position \ 2 - StartOffset
                        pjData.ExtraDat.RequireData(Datfiles(i)).WriteBinaryData(ObjID, filebinaryw)

                        filebinaryw.Write(CUShort(&HFFFF))
                End Select
            Next




            filebinaryw.Write(CUShort(&HFFFF))
        Next




        filebinaryw.Close()
        fileCreator.Close()
    End Sub

    Private Sub WriteRequirement(sb As StringBuilder)
        Dim pointers() As String = {"FG_PReqUnit", "FG_PReqUpg", "FG_PReqTechUpg", "FG_PReqTechUse", "FG_PReqOrder"}
        Dim Datfiles() As SCDatFiles.DatFiles = {SCDatFiles.DatFiles.units, SCDatFiles.DatFiles.upgrades, SCDatFiles.DatFiles.techdata, SCDatFiles.DatFiles.Stechdata, SCDatFiles.DatFiles.orders}


        sb.AppendLine("    DoActions([")
        For i = 0 To 4
            Dim pointer As String = Hex(Tool.GetOffset(pointers(i)))

            Dim value As UInteger = 0
            For k = 0 To SCCodeCount(Datfiles(i)) - 1
                Dim pos As Integer = pjData.ExtraDat.RequireData(Datfiles(i)).GetRequireObject(k).StartPos
                If k Mod 2 = 0 Then
                    value = pos
                Else
                    value += pos * 65536
                End If
                If k Mod 2 = 1 Then
                    sb.AppendLine("        SetMemory(0x" & pointer & " + " & (k * 2) - 2 & ", SetTo, " & value & "),")
                    'returntext.AppendLine("        # " & value Mod 256 & " " & value \ 65536)
                End If

            Next

        Next

        sb.AppendLine("    ])")
    End Sub
End Class
