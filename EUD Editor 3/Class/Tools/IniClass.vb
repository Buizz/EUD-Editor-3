Imports System.IO

Public Class IniClass
    Private Filename As String
    Private Datas As Dictionary(Of String, String)

    Public Sub New()
        Datas = New Dictionary(Of String, String)
    End Sub
    Public Sub New(tfilename As String)
        Datas = New Dictionary(Of String, String)
        ReadIni(tfilename)
    End Sub

    Public Property SettingData(keys As String) As String
        Get
            If Datas.ContainsKey(keys) Then
                Return Datas(keys)
            Else
                Return Nothing
            End If

        End Get
        Set(value As String)
            If Datas.ContainsKey(keys) Then
                Datas(keys) = value
            Else
                Datas.Add(keys, value)
            End If
        End Set
    End Property





    Public Sub ReadIni(tfilename As String)
        Datas.Clear()
        Filename = tfilename
        If My.Computer.FileSystem.FileExists(Filename) Then
            Dim filestream As New FileStream(tfilename, FileMode.Open)
            Dim strReader As New StreamReader(filestream)

            While (strReader.EndOfStream = False)
                Dim str() As String = strReader.ReadLine().Split("=")
                Dim Keyname As String = str(0).Trim
                Dim Value As String = str(1).Trim

                Datas.Add(Keyname, Value)
            End While



            strReader.Close()
            filestream.Close()
        Else
            Dim filestream As New FileStream(tfilename, FileMode.Create)
            filestream.Close()
        End If
    End Sub
    Public Sub WriteIni()
        Dim filestream As New FileStream(Filename, FileMode.Create)
        Dim strWriter As New StreamWriter(filestream)

        For i = 0 To Datas.Count - 1
            strWriter.Write(Datas.Keys(i) & " = " & Datas.Values(i) & vbCrLf)
        Next




        strWriter.Close()
        filestream.Close()
    End Sub
End Class
