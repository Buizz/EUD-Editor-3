Imports System.IO
Imports System.Net

Partial Public Class BuildData
    Public Function WriteChecksum() As Boolean
        'ConnectKey
        Dim mapstream As New FileStream(pjData.SaveMapName, FileMode.Open)
        Dim br As New BinaryReader(mapstream)
        Dim bw As New BinaryWriter(mapstream)


        Dim buffer As Byte()


        br.ReadUInt32() '이름
        br.ReadUInt32() '헤더길이
        Dim fileSize As UInteger = br.ReadUInt32() 'mpq길이


        mapstream.Position = 0
        buffer = br.ReadBytes(fileSize)


        Dim crc32 As New CRC32
        Dim checksumv As UInteger = crc32.GetCRC32(buffer)

        'MsgBox(mapstream.Position & "," & mapstream.Length)



        Dim checkstring As String = httpRequest("encrypt", "key=" & WebUtility.UrlEncode(ConnectKey) & "&data=" & WebUtility.UrlEncode(checksumv))
        'Dim checkstring As String = AESModule.EncryptString128Bit(checksumv, ConnectKey)

        bw.Write(checkstring)
        'MsgBox(checkstring)

        br.Close()
        bw.Close()
        mapstream.Close()


        Return True
    End Function
End Class
