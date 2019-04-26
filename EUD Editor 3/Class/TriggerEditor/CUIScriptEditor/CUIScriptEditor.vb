Imports System.IO

<Serializable>
Public Class CUIScriptEditor
    Inherits ScriptEditor

    Public Sub New(SType As SType)
        ScriptType = SType
    End Sub

    Private _String As String
    Public Property StringText As String
        Get
            If CheckConnect() Then
                Dim fs As New FileStream(ConnectFile, FileMode.Open)
                Dim sr As New StreamReader(fs)

                _String = sr.ReadToEnd()

                sr.Close()
                fs.Close()
            End If

            Return _String
        End Get
        Set(value As String)
            If CheckConnect() Then
                Dim fs As New FileStream(ConnectFile, FileMode.Create)
                Dim sw As New StreamWriter(fs)

                sw.Write(value)

                sw.Close()
                fs.Close()
            End If

            _String = value
        End Set
    End Property

    Public Overrides Function GetStringText() As String
        Return StringText
    End Function

    Public Overrides Function GetFileText() As String
        Return StringText
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
            If Not My.Computer.FileSystem.FileExists(_ConnectFile) And _ConnectRelativeFile <> "" Then '오픈 맵이 존재하지 않을 경우
                Dim tempConnectFileName As String = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(pjData.Filename), _ConnectRelativeFile))
                If My.Computer.FileSystem.FileExists(tempConnectFileName) Then '상대경로로 존재할 경우
                    _ConnectFile = tempConnectFileName
                End If
            End If


            Return _ConnectFile
        End Get
        Set(value As String)
            _ConnectFile = value
            If My.Computer.FileSystem.FileExists(pjData.Filename) Then
                _ConnectRelativeFile = Tool.GetRelativePath(pjData.Filename, value)
            End If
        End Set
    End Property
End Class
