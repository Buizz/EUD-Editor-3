Public Class BReader
    Public Shared Function ReadByte(ByRef pos As UInteger, bytes As Byte()) As Byte
        Dim val As Byte = bytes(pos)
        pos += 1
        Return val
    End Function
    Public Shared Function ReadUint16(ByRef pos As UInteger, bytes As Byte()) As UInt16
        Dim val As UInt16 = bytes(pos) + bytes(pos + 1) * &H100
        pos += 2
        Return val
    End Function
    Public Shared Function ReadUint32(ByRef pos As UInteger, bytes As Byte()) As UInt32
        Dim val As UInt32 = bytes(pos) + bytes(pos + 1) * &H100 + bytes(pos + 2) * &H10000 + bytes(pos + 3) * CUInt(&H1000000)
        pos += 4
        Return val
    End Function
    Public Shared Function ReadBytes(ByRef pos As UInteger, len As UInteger, bytes As Byte()) As Byte()
        Dim rbytes(len) As Byte
        For i = 0 To len - 1
            rbytes(i) = bytes(pos)
            pos += 1
        Next
        Return rbytes
    End Function
End Class
