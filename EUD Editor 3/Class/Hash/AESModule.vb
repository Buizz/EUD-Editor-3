Imports System.Security.Cryptography
Imports System.IO
Imports System.Text


Module AESModule
    Public Function EncryptString128Bit(ByVal vstrTextToBeEncrypted As String,
                                        ByVal vstrEncryptionKey As String) As String

        Dim bytValue() As Byte
        Dim bytKey() As Byte
        Dim bytEncoded() As Byte = {}
        Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
        Dim intLength As Integer
        Dim intRemaining As Integer
        Dim objMemoryStream As New MemoryStream()
        Dim objCryptoStream As CryptoStream
        Dim objRijndaelManaged As RijndaelManaged


        vstrTextToBeEncrypted = StripNullCharacters(vstrTextToBeEncrypted)
        bytValue = Encoding.UTF8.GetBytes(vstrTextToBeEncrypted.ToCharArray)
        intLength = Len(vstrEncryptionKey)


        If intLength >= 32 Then
            vstrEncryptionKey = Strings.Left(vstrEncryptionKey, 32)
        Else
            intLength = Len(vstrEncryptionKey)
            intRemaining = 32 - intLength
            vstrEncryptionKey = vstrEncryptionKey & Strings.StrDup(intRemaining, "X")
        End If


        bytKey = Encoding.ASCII.GetBytes(vstrEncryptionKey.ToCharArray)


        objRijndaelManaged = New RijndaelManaged()


        Try
            objCryptoStream = New CryptoStream(objMemoryStream,
            objRijndaelManaged.CreateEncryptor(bytKey, bytIV),
              CryptoStreamMode.Write)
            objCryptoStream.Write(bytValue, 0, bytValue.Length)


            objCryptoStream.FlushFinalBlock()


            bytEncoded = objMemoryStream.ToArray
            objMemoryStream.Close()
            objCryptoStream.Close()
        Catch
        End Try


        Return Convert.ToBase64String(bytEncoded)
    End Function


    Public Function DecryptString128Bit(ByVal vstrStringToBeDecrypted As String,
                                        ByVal vstrDecryptionKey As String) As String
        Dim bytDataToBeDecrypted() As Byte
        Dim bytTemp() As Byte
        Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
        Dim objRijndaelManaged As New RijndaelManaged()
        Dim objMemoryStream As MemoryStream
        Dim objCryptoStream As CryptoStream
        Dim bytDecryptionKey() As Byte


        Dim intLength As Integer
        Dim intRemaining As Integer
        Dim strReturnString As String = String.Empty


        bytDataToBeDecrypted = Convert.FromBase64String(vstrStringToBeDecrypted)
        intLength = Len(vstrDecryptionKey)



        If intLength >= 32 Then
            vstrDecryptionKey = Strings.Left(vstrDecryptionKey, 32)
        Else
            intLength = Len(vstrDecryptionKey)
            intRemaining = 32 - intLength
            vstrDecryptionKey = vstrDecryptionKey & Strings.StrDup(intRemaining, "X")
        End If

        bytDecryptionKey = Encoding.ASCII.GetBytes(vstrDecryptionKey.ToCharArray)

        ReDim bytTemp(bytDataToBeDecrypted.Length)


        objMemoryStream = New MemoryStream(bytDataToBeDecrypted)


        Try
            objCryptoStream = New CryptoStream(objMemoryStream,
            objRijndaelManaged.CreateDecryptor(bytDecryptionKey, bytIV),
               CryptoStreamMode.Read)

            objCryptoStream.Read(bytTemp, 0, bytTemp.Length)

            objCryptoStream.FlushFinalBlock()
            objMemoryStream.Close()
            objCryptoStream.Close()
        Catch

        End Try


        Return StripNullCharacters(Encoding.UTF8.GetString(bytTemp))
    End Function



    Public Function StripNullCharacters(ByVal vstrStringWithNulls As String) As String
        Dim intPosition As Integer
        Dim strStringWithOutNulls As String


        intPosition = 1
        strStringWithOutNulls = vstrStringWithNulls


        Do While intPosition > 0
            intPosition = InStr(intPosition, vstrStringWithNulls, vbNullChar)

            If intPosition > 0 Then
                strStringWithOutNulls = Left$(strStringWithOutNulls, intPosition - 1) &
                Right$(strStringWithOutNulls, Len(strStringWithOutNulls) - intPosition)
            End If

            If intPosition > strStringWithOutNulls.Length Then
                Exit Do
            End If
        Loop

        Return strStringWithOutNulls
    End Function
End Module
