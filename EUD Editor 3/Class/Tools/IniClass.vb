Imports System.IO

Public Class IniClass
    Private Filename As String
    Private Data As Dictionary(Of String, String)

    Public Sub New()
        Data = New Dictionary(Of String, String)
    End Sub
    Public Sub New(tfilename As String)
        Data = New Dictionary(Of String, String)
        ReadIni(tfilename)
    End Sub

    Public Property SettingData(keys As String) As String
        Get
            If Data.ContainsKey(keys) Then
                Return Data(keys)
            Else
                Return Nothing
            End If

        End Get
        Set(value As String)
            If Data.ContainsKey(keys) Then
                Data(keys) = value
            Else
                Data.Add(keys, value)
            End If
        End Set
    End Property





    Public Sub ReadIni(tfilename As String)
        Data.Clear()
        Filename = tfilename
        If My.Computer.FileSystem.FileExists(Filename) Then
            Dim filestream As New FileStream(tfilename, FileMode.Open)
            Dim strReader As New StreamReader(filestream)

            While (strReader.EndOfStream = False)
                Dim str() As String = strReader.ReadLine().Split("=")
                Dim Keyname As String = str(0).Trim
                Dim Value As String = str(1).Trim

                Data.Add(Keyname, Value)
            End While



            strReader.Close()
            filestream.Close()
        Else
            Dim filestream As New FileStream(tfilename, FileMode.Create)
            filestream.Close()
        End If
    End Sub
    Public Sub WriteIni()
        'MsgBox("ini저장")
        Dim filestream As New FileStream(Filename, FileMode.Create)
        Dim strWriter As New StreamWriter(filestream)

        'Dim tstr As String
        For i = 0 To Data.Count - 1
            'tstr = tstr & vbCrLf & Data.Keys(i) & " = " & Data.Values(i)
            strWriter.Write(Data.Keys(i) & " = " & Data.Values(i) & vbCrLf)
        Next
        'MsgBox(tstr)



        strWriter.Close()
        filestream.Close()
    End Sub
End Class
