Imports System.IO
Imports System.Text

Partial Public Class BuildData
    Public Enum EdsBlockType
        Main
        DataEditor
        ExtraDataEditor
        TEMainPlugin
        DataDumper
        SCAPlugin
        UserPlugin
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
            pBlocks.Add(New EdsBlockItem(EdsBlockType.SCAPlugin))
        End Sub


        Public Function GetedsString() As String
            Dim sb As New StringBuilder

            For i = 0 To pBlocks.Count - 1
                Dim texts As String = pBlocks(i).GetEdsString()

                If pjData.TEData.SCArchive.IsUsed Then
                    Dim startoffreeze As Integer = texts.IndexOf("[freeze]")
                    If startoffreeze >= 0 Then
                        If texts.IndexOf("[freeze]" & vbCrLf & "prompt: 1") = -1 Then
                            If MsgBox(Tool.GetText("Error SCA Freeze"), MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
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
                    Case EdsBlockType.SCAPlugin
                        If pjData.TEData.SCArchive.IsUsed Then
                            sb.Append("[MSQC]
MSQCSpecial.Exactly(1) : MSQCSpecialBuffer, 100
MSQCSpecial.Exactly(2) : MSQCSpecialBuffer, 200
MSQCSpecial.Exactly(3) : MSQCSpecialBuffer, 300
MSQCSpecial.Exactly(4) : MSQCSpecialBuffer, 400
MSQCCondiction.Exactly(1) ; xy , MSQCValue : MSQCBuffer")
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
                    Case EdsBlockType.SCAPlugin
                        sb.AppendLine("SCAPlugin")
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



        Dim strWriter As New StreamWriter(filestreama)

        strWriter.Write(sb.ToString)

        strWriter.Close()
        filestreama.Close()
    End Sub
End Class
