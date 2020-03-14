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

    Public items As List(Of ScriptBlock)
    Public Sub New(SType As SType)
        ScriptType = SType
        items = New List(Of ScriptBlock)
    End Sub


    Public Function GetJsonString() As String
        Return JsonConvert.SerializeObject(items, Formatting.Indented)
    End Function


    Public Sub SetJsonObject(str As String)
        items = JsonConvert.DeserializeObject(Of List(Of ScriptBlock))(str)
    End Sub







    Public Overrides Function GetFileText() As String
        Dim strb As New StringBuilder
        Dim indend As Integer = 3
        GUIScriptManager.GetScriptText(items, strb, indend)
        Return strb.ToString
    End Function
    Public Overrides Function GetStringText() As String
        Dim strb As New StringBuilder
        Dim indend As Integer = 3
        GUIScriptManager.GetScriptText(items, strb, indend)
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
