Imports System
Imports System.Text
Imports System.Security.Cryptography


Module mod_fn_HASH
#Region "Hash MD5"
    Private _md5 As MD5 = MD5.Create()

    Public Function GetMd5Hash(ByVal source As String) As String

        Dim data = _md5.ComputeHash(Encoding.UTF8.GetBytes(source))
        Dim sb As New StringBuilder()
        Array.ForEach(data, Function(x) sb.Append(x.ToString("X2")))
        Return sb.ToString()

    End Function

    Public Function VerifyMd5Hash(ByVal source As String, ByVal hash As String) As Boolean

        Dim sourceHash = GetMd5Hash(source)
        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase
        Return If(comparer.Compare(sourceHash, hash) = 0, True, False)

    End Function

    Public Function GetMd5HashBase64(ByVal source As String) As String

        Dim data() As Byte = _md5.ComputeHash(Encoding.UTF8.GetBytes(source))
        Return Convert.ToBase64String(data)
    End Function
#End Region
End Module
