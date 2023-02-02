Imports System.IO

Public Class CascData
    Private datapath As String
    Private hStorage As IntPtr
    Private hfile As IntPtr
    Private FileHash As Dictionary(Of String, String)

    Public Sub New()
        '해시코드 읽어서 저장하기
        datapath = Tool.StarCraftPath
        FileHash = New Dictionary(Of String, String)

        Dim filestream As New FileStream(Tool.DataPath("\ROOT.txt"), FileMode.Open)
        Dim sr As New StreamReader(filestream)



        Dim strs() As String = sr.ReadToEnd.Split(vbCrLf)

        sr.Close()
        filestream.Close()



        For i = 0 To strs.Count - 1
            If strs(i).Trim.Split("|").Count = 2 Then
                FileHash.Add(strs(i).Trim.Split("|").First.ToLower, strs(i).Trim.Split("|").Last.ToLower)
            End If
        Next
    End Sub


    Public Function ReadFile(filename As String) As Byte() '파일 읽기
        filename = filename.Replace("\", "/").ToLower

        Dim Hash As String

        If FileHash.ContainsKey(filename) Then
            Hash = FileHash(filename)
        Else
            Return {}
        End If


        CascLib.CascOpenStorage(datapath, &H200, hStorage)

        Dim memstream As New MemoryStream()

        Dim bytewriter As New BinaryWriter(memstream)
        Dim bytereader As New BinaryReader(memstream)
        Dim Buffer(1024) As Byte

        CascLib.CascOpenFile(hStorage, Hash, 0, &H1, hfile)

        While (True)
            Dim dwBytesRead As UInteger = 0
            CascLib.CascReadFile(hfile, Buffer, Buffer.Length, dwBytesRead)
            If (dwBytesRead = 0) Then
                Exit While
            End If
            bytewriter.Write(Buffer)

        End While
        memstream.Position = 0
        Dim Bytes() As Byte = memstream.ToArray

        CascLib.CascCloseFile(hfile)

        bytereader.Close()
        bytewriter.Close()
        memstream.Close()



        CascLib.CascCloseStorage(hStorage)

        Return Bytes
    End Function

    Public Sub OpenCascStorage()
        CascLib.CascOpenStorage(datapath, &H200, hStorage)
    End Sub
    Public Function ReadFileCascStorage(filename As String) As Byte()
        filename = filename.Replace("\", "/").ToLower

        Dim Hash As String

        If FileHash.ContainsKey(filename) Then
            Hash = FileHash(filename)
        Else
            Return {}
        End If

        Dim memstream As New MemoryStream()

        Dim bytewriter As New BinaryWriter(memstream)
        Dim Buffer(1024) As Byte

        'CascLib.CascOpenFile(hStorage, Hash, 0, &H1, hfile)
        CascLib.CascOpenFile(hStorage, Hash, 0, 0, hfile)

        While (True)
            Dim dwBytesRead As UInteger = 0
            CascLib.CascReadFile(hfile, Buffer, Buffer.Length, dwBytesRead)
            If (dwBytesRead = 0) Then
                Exit While
            End If
            bytewriter.Write(Buffer)

        End While
        memstream.Position = 0
        Dim Bytes() As Byte = memstream.ToArray

        CascLib.CascCloseFile(hfile)

        bytewriter.Close()
        memstream.Close()
        memstream.Dispose()
        Return Bytes
    End Function
    Public Sub CloseCascStorage()
        CascLib.CascCloseStorage(hStorage)
    End Sub




    '<DllImport("CascLib.dll")>
    'Public Shared Function CascOpenStorage(szDataPath As String, dwLocaleMask As UInteger, ByRef phStorage As IntPtr) As Boolean
    'End Function

    '<DllImport("CascLib.dll")>
    'Public Shared Function CascOpenFile(hStorage As IntPtr, szFileName As String, dwLocale As UInteger, dwFlags As UInteger, ByRef phFile As IntPtr) As Boolean
    'End Function

    '<DllImport("CascLib.dll")>
    'Public Shared Function CascReadFile(hFile As IntPtr, lpBuffer() As Byte, dwToRead As UInteger, ByRef pdwRead As IntPtr) As Boolean
    'End Function

    '<DllImport("CascLib.dll")>
    'Public Shared Function CascCloseFile(hFile As IntPtr) As Boolean
    'End Function

    '<DllImport("CascLib.dll")>
    'Public Shared Function CascCloseStorage(hStorage As IntPtr) As Boolean
    'End Function
End Class
