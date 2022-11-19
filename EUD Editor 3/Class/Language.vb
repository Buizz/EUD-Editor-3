Imports System.IO
Imports Newtonsoft.Json


Public Class Language
    Private LanguageData As List(Of LanguageData)

    Private CurrentLan As LanguageData
    Public Sub SetLanguage(name As String)
        For i = 0 To LanguageData.Count - 1
            If LanguageData(i).GetName = name Then
                CurrentLan = LanguageData(i)
                'pgData.Setting(ProgramData.TSetting.language) = CurrentLan.GetName
                Exit Sub
            End If
        Next
    End Sub


    Public Function GetLan(Key As String) As String
        If CurrentLan Is Nothing Then
            Return Nothing
        End If
        Return CurrentLan.GetText(Key)
    End Function


    Public Function GetLanguageArray() As String()
        Dim str As New List(Of String)

        For i = 0 To LanguageData.Count - 1
            str.Add(LanguageData(i).GetName)
        Next

        Return str.ToArray
    End Function



    Public Sub New(lanname As String)
        LanguageData = New List(Of LanguageData)

        For Each Filename As String In My.Computer.FileSystem.GetFiles(Tool.GetLanguageFolder)
            Dim filestream As New FileStream(Filename, FileMode.Open)
            Dim streamreader As New StreamReader(filestream, System.Text.Encoding.Default)

            Dim jsonString As String = streamreader.ReadToEnd

            Dim dic As Dictionary(Of String, String) = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(jsonString)
            LanguageData.Add(New LanguageData(dic))

            If dic("Language") = lanname Then
                CurrentLan = LanguageData.Last
            End If

            streamreader.Close()
            filestream.Close()
        Next

        If lanname = "" Then
            CurrentLan = LanguageData.Last
        End If
    End Sub


    Private Class LanguageData
        Public ReadOnly Property GetName As String



        Private Lans As Dictionary(Of String, String)
        Public ReadOnly Property GetText(key As String) As String
            Get
                Return Lans(key)
            End Get
        End Property

        Public Sub New(dic As Dictionary(Of String, String))
            Lans = dic
            GetName = Lans("Language")
        End Sub
    End Class
End Class
