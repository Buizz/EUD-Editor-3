Imports System.IO

Public Class tblReader
    Public ReadOnly Strings As List(Of TBLString)

    Public Structure TBLString
        Public val1 As String
        Public val2 As String

        Public Sub New(tval1 As String, tval2 As String)
            val1 = tval1
            val2 = tval2
        End Sub
    End Structure


    Public Sub New(tblFile As String)
        Strings = New List(Of TBLString)


        Dim fs As New FileStream(tblFile, FileMode.Open)
        Dim br As New BinaryReader(fs)

        Dim header() As UInt16


        Dim count As UInt16 = br.ReadUInt16()
        ReDim header(count - 1)


        For i = 0 To count - 1
            header(i) = br.ReadUInt16()
        Next

        For i = 0 To count - 1
            fs.Position = header(i)
            Dim bytes1 As New List(Of Byte)
            Dim bytes2 As New List(Of Byte)
            While True
                Dim val As Byte = br.ReadByte() '바이트씩 읽으면서 0이 나오면 종료.
                If val = 0 Then
                    If i = count - 1 Then
                        Exit While
                    Else
                        While fs.Position < header(i + 1)
                            If val = 0 Then
                                bytes2.Add(124)
                            Else
                                bytes2.Add(val)
                            End If
                            val = br.ReadByte()
                        End While



                    End If





                    'If fs.Position < fs.Length Then
                    '    val = br.ReadByte()
                    'Else
                    '    Exit While
                    'End If


                    Exit While
                Else
                    bytes1.Add(val)
                    bytes2.Add(val)
                End If
            End While
            Dim val1 As String = System.Text.Encoding.Default.GetChars(bytes1.ToArray)
            Dim val2 As String = System.Text.Encoding.Default.GetChars(bytes2.ToArray)

            Dim ttbl As New TBLString(val1, val2)


            Strings.Add(ttbl)

            'If i > 1000 And i < 1010 Then
            '    MsgBox(i & vbCrLf & Strings.Last.val1 & vbCrLf & Strings.Last.val2)
            'End If

        Next


        br.Close()
        fs.Close()
    End Sub
End Class
