Imports System.IO
Imports System.Text

<Serializable>
Public Class GUIScriptEditor
    Inherits ScriptEditor

    Public Sub New(SType As SType)
        ScriptType = SType
        items = New List(Of ScriptBlock)
    End Sub


    Public items As List(Of ScriptBlock)


    Public Overrides Function GetFileText() As String
        Return GetepsText()
    End Function
    Public Overrides Function GetStringText() As String
        Return GetepsText()
    End Function


    Private Function GetepsText() As String
        Dim returnstr As New StringBuilder

        For i = 0 To items.Count - 1
            returnstr.Append(items(i).ToCode())
        Next
        Return returnstr.ToString
    End Function



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
