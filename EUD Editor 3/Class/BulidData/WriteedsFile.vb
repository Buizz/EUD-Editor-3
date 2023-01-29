Imports System.IO
Imports System.Text

Partial Public Class BuildData
    Public Enum EdsBlockType
        Main
        DataEditor
        ExtraDataEditor
        TEMainPlugin
        DataDumper
        TEMSQC
        UserPlugin
        TEChatEvent
    End Enum


    <Serializable()>
    Public Class EdsBlock
        Public Sub New()
            pBlocks = New List(Of EdsBlockItem)
            pBlocks.Add(New EdsBlockItem(EdsBlockType.Main))
            pBlocks.Add(New EdsBlockItem(EdsBlockType.DataEditor))
            pBlocks.Add(New EdsBlockItem(EdsBlockType.ExtraDataEditor))
            pBlocks.Add(New EdsBlockItem(EdsBlockType.DataDumper))
            pBlocks.Add(New EdsBlockItem(EdsBlockType.TEMainPlugin))
            pBlocks.Add(New EdsBlockItem(EdsBlockType.TEMSQC))
            pBlocks.Add(New EdsBlockItem(EdsBlockType.TEChatEvent))
        End Sub


        Public Function GetedsString() As String
            Dim sb As New StringBuilder

            Dim checkdic As List(Of EdsBlockType) = New List(Of EdsBlockType)
            'pBlocks가 다 있나 확인
            For i = 0 To pBlocks.Count - 1
                If checkdic.IndexOf(pBlocks(i).BType) = -1 Then
                    checkdic.Add(pBlocks(i).BType)
                End If
            Next
            
            If checkdic.IndexOf(EdsBlockType.Main) = -1 Then
                pBlocks.Insert(0, New EdsBlockItem(EdsBlockType.Main))
            End If
            If checkdic.IndexOf(EdsBlockType.DataEditor) = -1 Then
                pBlocks.Add(New EdsBlockItem(EdsBlockType.DataEditor))
            End If
            If checkdic.IndexOf(EdsBlockType.ExtraDataEditor) = -1 Then
                pBlocks.Add(New EdsBlockItem(EdsBlockType.ExtraDataEditor))
            End If
            If checkdic.IndexOf(EdsBlockType.DataDumper) = -1 Then
                pBlocks.Add(New EdsBlockItem(EdsBlockType.DataDumper))
            End If
            If checkdic.IndexOf(EdsBlockType.TEMainPlugin) = -1 Then
                pBlocks.Add(New EdsBlockItem(EdsBlockType.TEMainPlugin))
            End If
            If checkdic.IndexOf(EdsBlockType.TEMSQC) = -1 Then
                pBlocks.Add(New EdsBlockItem(EdsBlockType.TEMSQC))
            End If
            If checkdic.IndexOf(EdsBlockType.TEChatEvent) = -1 Then
                pBlocks.Add(New EdsBlockItem(EdsBlockType.TEChatEvent))
            End If



            For i = 0 To pBlocks.Count - 1
                Dim texts As String = pBlocks(i).GetEdsString()

                If pjData.TEData.SCArchive.IsUsed Then
                    Dim startoffreeze As Integer = texts.IndexOf("[freeze]")
                    If startoffreeze >= 0 Then
                        If texts.IndexOf("[freeze]" & vbCrLf & "prompt: 1") = -1 Then
                            If Tool.MsgBox(Tool.GetText("Error SCA Freeze"), MessageBoxButton.OKCancel) = MsgBoxResult.Cancel Then
                                Throw New Exception()
                            End If
                            texts = texts.Insert(startoffreeze + 8, vbCrLf & "prompt: 1")
                        End If
                        'prompt: 1
                    End If
                End If

                sb.AppendLine(texts)
            Next

            Return sb.ToString
        End Function



        Private pBlocks As List(Of EdsBlockItem)
        Public ReadOnly Property Blocks As List(Of EdsBlockItem)
            Get
                Return pBlocks
            End Get
        End Property

        <Serializable()>
        Public Class EdsBlockItem
            Public BType As BuildData.EdsBlockType

            Private pTexts As String
            Public Property Texts As String
                Get
                    Return pTexts
                End Get
                Set(value As String)
                    pTexts = value
                End Set
            End Property


            Public Function Clone() As EdsBlockItem
                Dim CObject As New EdsBlockItem(BType)

                CObject.Texts = Texts

                Return CObject
            End Function

            Public Sub New(tBType As BuildData.EdsBlockType)
                BType = tBType
            End Sub

            Public Function GetEdsString() As String
                Dim sb As New StringBuilder
                Select Case BType
                    Case EdsBlockType.Main
                        sb.AppendLine("[main]")
                        sb.AppendLine("input: " & OpenMapPath)
                        sb.Append("output: " & SaveMapPath)
                    Case EdsBlockType.DataEditor
                        If My.Computer.FileSystem.FileExists(DatpyFilePath) Then
                            sb.Append("[DataEditor.py]")
                        End If
                    Case EdsBlockType.ExtraDataEditor
                        If My.Computer.FileSystem.FileExists(DatpyFilePath) Then
                            sb.Append("[ExtraDataEditor.py]")
                        End If
                    Case EdsBlockType.TEMainPlugin
                        'TriggerEditor
                        If My.Computer.FileSystem.FileExists(TriggerEditorPath & "\" & pjData.TEData.GetMainFilePath) Then
                            sb.AppendLine("[" & Tool.GetRelativePath(EdsFilePath, TriggerEditorPath & "\" & pjData.TEData.GetMainFilePath) & "]")
                        End If
                    Case EdsBlockType.DataDumper
                        sb.AppendLine("[dataDumper]")
                        If pjData.UseCustomtbl Then
                            'tbl 파일 쓰기
                            sb.AppendLine(Tool.GetRelativePath(EdsFilePath, tblFilePath) & " : 0x6D5A30, copy")
                        End If
                        'RequireData 쓰기
                        'sb.Append(Tool.GetRelativePath(EdsFilePath, requireFilePath) & " : 0x" & Hex(Tool.GetOffset("Vanilla") + 500) & ", copy")
                    Case EdsBlockType.TEMSQC
                        If pjData.TEData.UseMSQC Then
                            Dim msqccode As String = macro.GetMSQCCode

                            If pjData.TEData.SCArchive.IsUsed Then
                                msqccode = msqccode &
"MSQCSpecial.Exactly(1) : MSQCSpecialBuffer, 100
MSQCSpecial.Exactly(2) : MSQCSpecialBuffer, 200
MSQCSpecial.Exactly(3) : MSQCSpecialBuffer, 300
MSQCSpecial.Exactly(4) : MSQCSpecialBuffer, 400
MSQCCondiction.Exactly(1) ; xy , MSQCValue : MSQCBuffer
MSQCFLCondition.Exactly(1) ; val , MSQCLocalLoadingStatus : MSQCLoadingStatus
MSQCSCAIDCondition.Exactly(1) ; val , MSQCLocalSCAIDHIGH : MSQCSCAIDHIGH
MSQCSCAIDCondition.Exactly(1) ; val , MSQCLocalSCAIDLOW : MSQCSCAIDLOW
" & vbCrLf
                            End If
                            If pjData.TEData.MouseLocation <> "" Then
                                msqccode = msqccode & "mouse : " & pjData.TEData.MouseLocation & vbCrLf
                            End If

                            If msqccode <> "" Then
                                sb.AppendLine("[MSQC]")
                                sb.Append(msqccode)
                            End If
                        End If
                    Case EdsBlockType.TEChatEvent
                        If pjData.TEData.UseChatEvent Then
                            Dim chatcode As String = macro.GetChatEventCode
                            If chatcode <> "" Then
                                sb.AppendLine("[chatEvent]")
                                sb.AppendLine(chatcode)
                            End If
                        End If
                    Case EdsBlockType.UserPlugin
                        sb.Append(Texts)
                End Select



                Return sb.ToString
            End Function
            Public Function GetEdsName() As String
                Dim sb As New StringBuilder

                Select Case BType
                    Case EdsBlockType.Main
                        sb.AppendLine("Main")
                    Case EdsBlockType.DataEditor
                        sb.AppendLine("DataEditor")
                    Case EdsBlockType.ExtraDataEditor
                        sb.AppendLine("ExtraDataEditor")
                    Case EdsBlockType.TEMainPlugin
                        sb.AppendLine("TEMainPlugin")
                    Case EdsBlockType.DataDumper
                        sb.AppendLine("DataDumper")
                    Case EdsBlockType.TEMSQC
                        sb.AppendLine("TE MSQC")
                    Case EdsBlockType.TEChatEvent
                        sb.AppendLine("TE chatEvent")
                End Select



                Return sb.ToString
            End Function
        End Class
    End Class


    Private Sub WriteedsFile(IsEDD As Boolean)
        Dim sb As New StringBuilder

        sb.AppendLine(pjData.EdsBlock.GetedsString())



        '=====================================================================================================

        Dim filestreama As FileStream
        If IsEDD Then
            filestreama = New FileStream(EddFilePath, FileMode.Create)
        Else
            filestreama = New FileStream(EdsFilePath, FileMode.Create)
        End If



        Dim strWriter As New StreamWriter(filestreama) ', Encoding.UTF8)

        strWriter.Write(sb.ToString)

        strWriter.Close()
        filestreama.Close()
    End Sub
End Class
