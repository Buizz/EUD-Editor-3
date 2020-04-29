Imports System.IO

Public Class CRC32
    Private Shared CrcTable As UInt32() = Nothing

    Shared Sub New()
        Dim c As UInt32
        CrcTable = New UInt32(255) {}
        For i As Integer = 0 To 255

            c = CType(i, UInt32)
            For j As Integer = 0 To 7
                If (c And 1) <> 0 Then
                    c = 3988292384 Xor (c >> 1)
                Else
                    c >>= 1
                End If
            Next
            CrcTable(i) = c
        Next
    End Sub

    Public Function GetCRC32(ByVal s As String) As UInt32
        Dim CRC As UInt32 = UInt32.MaxValue
        'Dim stream As MemoryStream = New MemoryStream(ByteArray)

        Dim buffer As Byte() = System.Text.Encoding.UTF8.GetBytes(s)
        For i As Integer = 0 To buffer.Length - 1
            CRC = CrcTable((CRC Xor buffer(i)) And 255) Xor (CRC >> 8)
        Next

        Return CRC Xor UInteger.MaxValue
    End Function
    Public Function GetCRC32(ByVal bytes() As Byte) As UInt32
        Dim CRC As UInt32 = UInt32.MaxValue
        'Dim stream As MemoryStream = New MemoryStream(ByteArray)

        For i As Integer = 0 To bytes.Length - 1
            CRC = CrcTable((CRC Xor bytes(i)) And 255) Xor (CRC >> 8)
        Next

        Return CRC Xor UInteger.MaxValue
    End Function

    Public Function GetCRC32FromFile(ByVal filepath As String) As UInt32
        Dim fs As New FileStream(filepath, FileMode.Open)
        Dim br As New BinaryReader(fs)

        Dim buffer As Byte() = br.ReadBytes(fs.Length)

        br.Close()
        fs.Close()

        Dim CRC As UInt32 = UInt32.MaxValue
        'Dim stream As MemoryStream = New MemoryStream(ByteArray)

        For i As Integer = 0 To buffer.Length - 1
            CRC = CrcTable((CRC Xor buffer(i)) And 255) Xor (CRC >> 8)
        Next

        Return CRC Xor UInteger.MaxValue
    End Function
End Class