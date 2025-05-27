Imports System.IO
Imports System.Net

Module HttpTool

    Public Function Login(email As String, password As String) As String
        Dim senddata As New Dictionary(Of String, String)

        senddata.Add("email", email)
        senddata.Add("password", password)

        Return Request("login", senddata)
    End Function


    Public Function Request(filename As String, senddata As Dictionary(Of String, String)) As String
        Dim send As String = ""

        For Each i In senddata
            If send <> "" Then
                send &= "&"
            End If

            send &= i.Key & "=" & WebUtility.UrlEncode(i.Value)
        Next

        Return httpRequest(filename, send)
    End Function


    Public Function httpRequest(filename As String, senddata As String, Optional cookie As CookieContainer = Nothing) As String
        Try
            Dim url As String = "https://scarchive.kr/eudeditor/" & filename & ".php"

            Dim strResult As String = ""
            Dim req As HttpWebRequest = WebRequest.Create(url)
            req.Method = "POST"
            req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
            req.ContentLength = senddata.Length
            req.KeepAlive = True
            req.CookieContainer = cookie

            Dim sw As StreamWriter = New StreamWriter(req.GetRequestStream())
            sw.Write(senddata)
            sw.Close()

            Dim result As HttpWebResponse = req.GetResponse()

            If result.StatusCode = HttpStatusCode.OK Then
                Dim strReceiveStream As Stream = result.GetResponseStream()
                Dim reqStreamReader As StreamReader = New StreamReader(strReceiveStream, Text.Encoding.UTF8)

                strResult = reqStreamReader.ReadToEnd()

                req.Abort()
                strReceiveStream.Close()
                reqStreamReader.Close()

            Else
                strResult = "ERROR"
            End If
            Return strResult
        Catch ex As Exception

        End Try


        Return "ERROR"
    End Function


End Module
