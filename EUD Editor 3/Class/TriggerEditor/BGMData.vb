Imports System.Net.Sockets
Imports System.IO
Imports NAudio.Vorbis
Imports NAudio.Wave

<Serializable>
Public Class BGMData
    Public BGMList As New List(Of BGMFile)


    Private _SCABGMList As New List(Of BGMFile)
    Public ReadOnly Property SCABGMList As List(Of BGMFile)
        Get
            If _SCABGMList Is Nothing Then
                _SCABGMList = New List(Of BGMFile)
            End If

            Return _SCABGMList
        End Get
    End Property



    <Serializable>
    Public Class BGMFile
        Public Property BGMFile As BGMFile
        Public Sub RelativeBGMRefresh()
            If My.Computer.FileSystem.FileExists(pjData.Filename) Then
                _BGMRelativePath = Tool.GetRelativePath(pjData.Filename, _BGMPath)
            End If

            'MsgBox("RelativeDataRefresh")
        End Sub

        Private _BGMName As String
        Public Property BGMName As String
            Get
                Return _BGMName
            End Get
            Set(value As String)
                _BGMName = value
            End Set
        End Property
        Private _BGMPath As String
        Private _BGMRelativePath As String
        Public Property BGMPath As String
            Get
                If Not My.Computer.FileSystem.FileExists(_BGMPath) And _BGMRelativePath <> "" Then '오픈 맵이 존재하지 않을 경우
                    Dim tempBgmPath As String = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(pjData.Filename), _BGMRelativePath))
                    If My.Computer.FileSystem.FileExists(tempBgmPath) Then '상대경로로 존재할 경우
                        _BGMPath = tempBgmPath
                    End If
                End If

                Return _BGMPath
            End Get
            Set(value As String)
                _BGMPath = value

                If My.Computer.FileSystem.FileExists(_BGMPath) Then
                    Dim extension As String = _BGMPath.Split(".").Last.ToLower

                    Select Case extension
                        Case "ogg"
                            Dim vr As New VorbisWaveReader(_BGMPath)

                            _BGMLen = vr.TotalTime.TotalSeconds
                        Case "mp3", "wav"
                            Dim af As New AudioFileReader(_BGMPath)
                            _BGMLen = af.TotalTime.TotalSeconds
                    End Select
                Else
                    _BGMLen = -1
                End If

                BGMorgSize = Tool.GetFileSize(_BGMPath)



                RelativeBGMRefresh()
            End Set
        End Property
        Private _BGMLen As Integer
        Public ReadOnly Property BGMLen As String
            Get
                Return _BGMLen \ 60 & ":" & _BGMLen Mod 60
            End Get
        End Property

        Private _BGMCount As Integer

        Private _BGMSampleRate As Integer
        Public ReadOnly Property BGMSampleRate As Integer
            Get
                Return _BGMSampleRate
            End Get
        End Property
        Private _BGMBitRate As Integer
        Public ReadOnly Property BGMBitRate As Integer
            Get
                Return _BGMBitRate
            End Get
        End Property


        Public ReadOnly Property BGMSampleRateStr As String
            Get
                If _BGMSampleRate = -1 Then
                    Return "원본"
                End If

                Return _BGMSampleRate & "Hz"
            End Get
        End Property
        Public ReadOnly Property BGMBitRateStr As String
            Get
                If _BGMBitRate = -1 Then
                    Return "원본"
                End If

                Return _BGMBitRate & "kbps"
            End Get
        End Property

        Public BGMorgSize As Long
        Public ReadOnly Property BGMOrgSizestr As String
            Get
                Return BGMorgSize \ 1024 & "kbyte"
            End Get
        End Property
        Public BGMCompressionSize As Long
        Public Property BGMCompressionSizestr As String
            Get
                If BGMCompressionSize = -1 Then
                    Return "알수없음"
                End If

                Return BGMCompressionSize \ 1024 & "kbyte"
            End Get
            Set(value As String)

            End Set
        End Property
        Public BGMBlockCount As Integer


        Public Sub New(_filepath As String, _bgmname As String, _sample As Integer, _bitrate As Integer)
            BGMBlockCount = 0
            BGMFile = Me
            BGMCompressionSize = -1

            _BGMSampleRate = _sample
            _BGMBitRate = _bitrate

            BGMName = _bgmname
            BGMPath = _filepath
        End Sub
        Public Sub Refresh(_filepath As String, _bgmname As String, _sample As Integer, _bitrate As Integer)
            BGMBlockCount = 0
            BGMCompressionSize = -1

            BGMName = _bgmname
            BGMPath = _filepath

            _BGMSampleRate = _sample
            _BGMBitRate = _bitrate
        End Sub

    End Class
End Class
