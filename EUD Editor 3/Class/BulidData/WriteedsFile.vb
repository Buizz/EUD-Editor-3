Imports System.IO
Imports System.Text

Partial Public Class BuildData
    Public Enum EdsBlockType
        Main
        DataEditor
        ExtraDataEditor
        TEMainPlugin
        DataDumper
        Etc
    End Enum


    <Serializable()>
    Public Class EdsBlock
        Public Sub New()
            Blocks = New List(Of EdsBlockItem)
            Blocks.Add(New EdsBlockItem(EdsBlockType.Main))
            Blocks.Add(New EdsBlockItem(EdsBlockType.DataEditor))
            Blocks.Add(New EdsBlockItem(EdsBlockType.ExtraDataEditor))
            Blocks.Add(New EdsBlockItem(EdsBlockType.TEMainPlugin))
            Blocks.Add(New EdsBlockItem(EdsBlockType.DataDumper))
        End Sub


        Public Function GetedsString() As String
            Dim sb As New StringBuilder

            For i = 0 To Blocks.Count - 1
                sb.AppendLine(Blocks(i).GetEdsString())
            Next

            Return sb.ToString
        End Function



        Public ReadOnly Property BlocksLen As Integer
            Get
                Return Blocks.Count
            End Get
        End Property


        Public ReadOnly Property BlocksStr(index As Integer) As String
            Get
                Return Blocks(index).GetEdsString
            End Get
        End Property
        Public ReadOnly Property BlocksName(index As Integer) As String
            Get
                Return Blocks(index).GetEdsName
            End Get
        End Property


        Private Blocks As List(Of EdsBlockItem)
        <Serializable()>
        Private Class EdsBlockItem
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
                            sb.Append("[" & Tool.GetRelativePath(EdsFilePath, TriggerEditorPath & "\" & pjData.TEData.GetMainFilePath) & "]")
                        End If
                    Case EdsBlockType.DataDumper
                        sb.AppendLine("[dataDumper]")
                        If pjData.UseCustomtbl Then
                            'tbl 파일 쓰기
                            sb.AppendLine(Tool.GetRelativePath(EdsFilePath, tblFilePath) & " : 0x6D5A30, copy")
                        End If
                        'RequireData 쓰기
                        sb.Append(Tool.GetRelativePath(EdsFilePath, requireFilePath) & " : 0x" & Hex(Tool.GetOffset("Vanilla")) & ", copy")
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
                End Select



                Return sb.ToString
            End Function
        End Class
    End Class


    Private Sub WriteedsFile()
        Dim sb As New StringBuilder

        sb.AppendLine(pjData.EdsBlock.GetedsString())




        '=====================================================================================================

        Dim filestreama As New FileStream(EdsFilePath, FileMode.Create)
        Dim strWriter As New StreamWriter(filestreama)

        strWriter.Write(sb.ToString)

        strWriter.Close()
        filestreama.Close()
    End Sub
End Class
