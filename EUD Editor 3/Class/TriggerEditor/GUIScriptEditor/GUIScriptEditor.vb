Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Text
Imports Newtonsoft.Json

<Serializable>
Public Class GUIScriptEditor
    Inherits ScriptEditor

    'Public tstring As String =
    '"{
    '  ""name"": ""If"",
    '  ""isexpand"": false,
    '  ""child"": [
    '    {
    '      ""name"": ""If"",
    '      ""isexpand"": false,
    '      ""child"": []
    '    },
    '    {
    '      ""name"": ""If"",
    '      ""isexpand"": false,
    '      ""child"": []
    '    }]
    '}"

    Private items As List(Of ScriptBlock)
    Public ReadOnly Property GetItemsList As List(Of ScriptBlock)
        Get
            Return items
        End Get
    End Property
    Public ReadOnly Property GetItems(index As Integer) As ScriptBlock
        Get
            Return items(index)
        End Get
    End Property
    Public Sub RemoveItems(scr As ScriptBlock)
        items.Remove(scr)
        If scr.ScriptType = ScriptBlock.EBlockType.import Then
            ExternLoader()
        End If
    End Sub
    Public ReadOnly Property IndexOfItem(scr As ScriptBlock) As Integer
        Get
            Return items.IndexOf(scr)
        End Get

    End Property
    Public ReadOnly Property ItemCount() As Integer
        Get
            Return items.Count
        End Get
    End Property
    Public Sub AddItems(Scr As ScriptBlock)
        items.Add(Scr)
        If Scr.ScriptType = ScriptBlock.EBlockType.import Then
            ExternLoader()
        End If
    End Sub
    Public Sub InsertItems(index As Integer, Scr As ScriptBlock)
        items.Insert(index, Scr)
        If Scr.ScriptType = ScriptBlock.EBlockType.import Then
            ExternLoader()
        End If
    End Sub


    Private TEFile As TEFile
    Public Sub New(SType As SType, _TEFile As TEFile)
        ScriptType = SType
        TEFile = _TEFile
        items = New List(Of ScriptBlock)
    End Sub



    <NonSerialized>
    Public ExternFile As New List(Of ExternFile)

    Public Sub ExternLoader()
        ExternFile = New List(Of ExternFile)

        ExternFile.Clear()
        For i = 0 To items.Count - 1
            If items(i).ScriptType = ScriptBlock.EBlockType.import Then
                'MsgBox(items(i).value)

                Dim t As String = items(i).value
                t = t.Replace("as", "!")
                Dim strs() As String = t.Split("!")
                Dim path As String = strs.First.Trim
                Dim n As String = strs.Last.Trim


                Dim fTEFile As TEFile
                If path = "SCArchive" Then
                    Dim SCATEFile As New TEFile("SCAFile", TEFile.EFileType.CUIEps)
                    CType(SCATEFile.Scripter, CUIScriptEditor).StringText = pjData.EudplibData.GetSCAMainEps


                    fTEFile = SCATEFile
                Else
                    fTEFile = CodeEditor.FineFile(TEFile, path)
                End If


                'MsgBox(fTEFile.FileName)

                ExternFile.Add(New ExternFile(fTEFile, n))
            End If
        Next
        'MsgBox("Extern로드, 함수, 변수, 오브젝트를 불러옴")
    End Sub




    Public Function GetJsonString() As String
        Return JsonConvert.SerializeObject(items, Formatting.Indented)
    End Function


    Public Sub SetJsonObject(str As String)
        items = JsonConvert.DeserializeObject(Of List(Of ScriptBlock))(str)
    End Sub







    Public Overrides Function GetFileText() As String
        Dim strb As New StringBuilder
        Dim indend As Integer = 0
        GUIScriptManager.GetScriptText(items, strb, indend, "")
        Return macro.MacroApply(strb.ToString)
    End Function
    Public Overrides Function GetStringText() As String
        Dim strb As New StringBuilder
        Dim indend As Integer = 0
        GUIScriptManager.GetScriptText(items, strb, indend, "")
        Return strb.ToString
    End Function


    'Private Function GetepsText() As String
    '    Dim returnstr As New StringBuilder

    '    For i = 0 To items.Count - 1
    '        returnstr.Append(items(i).ToCode())
    '    Next
    '    Return returnstr.ToString
    'End Function



    Private _ConnectFile As String
    Private _ConnectRelativeFile As String

    Public Overrides Function CheckConnect() As Boolean
        If My.Computer.FileSystem.FileExists(_ConnectFile) Or
            My.Computer.FileSystem.FileExists(_ConnectRelativeFile) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Overrides Property ConnectFile() As String
        Get
            Return False
        End Get
        Set(value As String)
        End Set
    End Property
End Class
