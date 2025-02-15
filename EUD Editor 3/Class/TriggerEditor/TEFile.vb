﻿Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable>
Public Class TEFile
    '프로젝트 각 파일들
    Public Enum EFileType
        Folder
        CUIEps
        CUIPy
        GUIEps
        GUIPy

        Setting

        ClassicTrigger
        SCAScript
        RawText
    End Enum


    '형태가 폴더 이거나 파일 이거나 둘 중 하나
    '파일 안에는 파일이 들어 있을 수 있는 구조.

    <NonSerialized>
    Public ParentPage As TECUIPage



    '========================이전 데이터 저장용========================
    Private _IsExpaned As Boolean
    '프로그램 끌때 항목 저장. 지금은 임시로 True로
    Public Property IsExpanded As Boolean
        Get
            Return _IsExpaned
        End Get
        Set(value As Boolean)
            _IsExpaned = value
        End Set
    End Property

    '==================================================================

    Private CreateDate As Date
    Private pLastDate As Date
    Public Sub LastDataRefresh()
        pLastDate = Now
    End Sub

    Public ReadOnly Property LastDate As Date
        Get
            Return pLastDate
        End Get
    End Property



    Private LastConnectTimer() As Date
    Public Function RefreshData() As String
        Select Case FileType
            Case EFileType.CUIEps, EFileType.CUIPy
                If Scripter.CheckConnect Then
                    If pLastDate <> File.GetLastWriteTime(Scripter.ConnectFile) Then
                        pLastDate = File.GetLastWriteTime(Scripter.ConnectFile)

                        'CType(Scripter, CUIScriptEditor).co
                        'Scripter.ExternerLoader
                        Return Scripter.GetStringText()
                    End If
                    Return ""
                Else
                    Return ""
                End If

            Case EFileType.GUIEps, EFileType.GUIPy
                CType(Scripter, GUIScriptEditor).ExternLoader()
                Return ""
            Case EFileType.ClassicTrigger

        End Select

        Return ""
    End Function




    Private _UIBinding As TETabItemUI
    Public ReadOnly Property UIBinding As TETabItemUI
        Get
            Return _UIBinding
        End Get
    End Property


    Private _Scripter As ScriptEditor
    Public ReadOnly Property Scripter As ScriptEditor
        Get
            Return _Scripter
        End Get
    End Property

    Private IsTopFile As Boolean = False
    Public ReadOnly Property IsTopFolder As Boolean
        Get
            Return IsTopFile
        End Get
    End Property

    Public ReadOnly Property IsFile As Boolean
        Get
            Select Case FileType
                Case EFileType.CUIEps
                    Return True
                Case EFileType.CUIPy
                    Return True
                Case EFileType.GUIEps
                    Return True
                Case EFileType.GUIPy
                    Return True
                Case EFileType.ClassicTrigger
                    Return True
                Case Else
                    Return False
            End Select
        End Get
    End Property

    Public Sub LoadInit()
        If FileType = EFileType.GUIEps Then
            CType(Scripter, GUIScriptEditor).LoadInit()

        ElseIf FileType = EFileType.ClassicTrigger Then
            CType(Scripter, ClassicTriggerEditor).LoadInit()
        Else
            For i = 0 To FileCount - 1
                Files(i).LoadInit()
            Next
        End If
    End Sub


    Public Sub New(TFName As String, FileType As EFileType)
        _UIBinding = New TETabItemUI(Me)

        _Folders = New List(Of TEFile)
        _Files = New List(Of TEFile)

        _FileName = TFName


        _FileType = FileType

        Select Case FileType
            Case EFileType.CUIEps
                _Scripter = New CUIScriptEditor(ScriptEditor.SType.Eps)
            Case EFileType.CUIPy
                _Scripter = New CUIScriptEditor(ScriptEditor.SType.Py)
            Case EFileType.GUIEps
                _Scripter = New GUIScriptEditor(ScriptEditor.SType.Eps, Me)
            Case EFileType.GUIPy
                _Scripter = New GUIScriptEditor(ScriptEditor.SType.Py, Me)
            Case EFileType.ClassicTrigger
                _Scripter = New ClassicTriggerEditor()
            Case EFileType.SCAScript
                _Scripter = New SCAScriptEditor(ScriptEditor.SType.Lua)
            Case EFileType.RawText
                _Scripter = New RawTextScriptEditor(ScriptEditor.SType.RawText)
        End Select


        CreateDate = Now
        pLastDate = Now
        If _FileName = TriggerEditorData.TopFileName Then
            IsTopFile = True
        End If
    End Sub


    Private _Folders As List(Of TEFile)
    Public ReadOnly Property Folders(index As Integer) As TEFile
        Get
            Return _Folders(index)
        End Get
    End Property
    Public ReadOnly Property FolderCount As Integer
        Get
            Return _Folders.Count
        End Get
    End Property
    Public Sub FolderAdd(tTEFile As TEFile)
        _Folders.Add(tTEFile)
        tTEFile.ParentFolder = Me
        FolderSort()
    End Sub
    Public Sub FolderRemove(tTEFile As TEFile)
        _Folders.Remove(tTEFile)
    End Sub
    Public Sub FolderSort()
        _Folders.Sort(Function(x, y) x.FileName.CompareTo(y.FileName))
    End Sub


    Private ParentFolder As TEFile
    Public ReadOnly Property Parent As TEFile
        Get
            Return ParentFolder
        End Get
    End Property


    Private _Files As List(Of TEFile)
    Public ReadOnly Property Files(index As Integer) As TEFile
        Get
            Return _Files(index)
        End Get
    End Property
    Public ReadOnly Property FileCount As Integer
        Get
            Return _Files.Count
        End Get
    End Property
    Public Sub FileAdd(tTEFile As TEFile)
        _Files.Add(tTEFile)
        tTEFile.ParentFolder = Me
        FileSort()
    End Sub
    Public Sub FileRemove(tTEFile As TEFile)
        _Files.Remove(tTEFile)
    End Sub
    Public Sub FileSort()
        _Files.Sort(Function(x, y) x.FileName.CompareTo(y.FileName))
    End Sub


    Public Function CheckFliter(FliterText As String) As Boolean
        Return FileName.ToLower.IndexOf(FliterText.ToLower) >= 0
    End Function



    Private _FileType As EFileType
    Public ReadOnly Property FileType As EFileType
        Get
            Return _FileType
        End Get
    End Property


    Public Function GetPullPath() As String
        Dim rpath As String = ""

        Dim tfile As TEFile = Me

        While Not tfile.IsTopFile
            If rpath <> "" Then
                rpath = "." + rpath
            End If
            rpath = tfile.FileName + rpath
            tfile = tfile.Parent
        End While
        Return rpath
    End Function


    Private _FileName As String
    Public Property FileName As String
        Get
            If _FileName = TriggerEditorData.TopFileName Then
                Return pjData.SafeFilename
            Else
                Return _FileName
            End If
        End Get
        Set(value As String)
            pjData.SetDirty(True)
            _FileName = value
        End Set
    End Property
    Public ReadOnly Property RealFileName As String
        Get
            Select Case FileType
                Case EFileType.CUIEps
                    Return _FileName & ".eps"
                Case EFileType.CUIPy
                    Return _FileName & ".py"
                Case EFileType.GUIEps
                    Return _FileName & ".eps"
                Case EFileType.GUIPy
                    Return _FileName & ".py"
                Case EFileType.ClassicTrigger
                    Return _FileName & ".eps"
                Case EFileType.SCAScript
                    Return _FileName & ".lua"
            End Select


            Return _FileName
        End Get
    End Property

    Public ReadOnly Property GetTooltip() As String
        Get
            Dim returnText As String = ""

            Select Case _FileType
                Case EFileType.Folder
                    returnText = "​" & Tool.GetText("Folder") & vbCrLf
                Case EFileType.GUIEps, EFileType.ClassicTrigger
                    returnText = Tool.GetText("EpsScriptFile") & vbCrLf
                Case EFileType.CUIEps
                    returnText = Tool.GetText("EpsScriptFile") & vbCrLf
                    returnText &= Tool.GetText("ConnectFile") & " : " & vbCrLf & Scripter.ConnectFile & vbCrLf
                Case EFileType.GUIPy, EFileType.GUIEps
                    returnText = Tool.GetText("PyScriptFile") & vbCrLf
                Case EFileType.Setting
                    returnText = "​​"
            End Select

            returnText = returnText & Tool.GetText("FIleName") & " : " & FileName & vbCrLf &
                Tool.GetText("CreateDate") & " : " & vbCrLf & CreateDate.ToString & vbCrLf &
                Tool.GetText("LastDate") & " : " & vbCrLf & pLastDate.ToString

            Return returnText
        End Get
    End Property


    Public Function Clone() As TEFile
        Dim memory_stream = New MemoryStream()

        Dim formatter As New BinaryFormatter()
        formatter.Serialize(memory_stream, Me)

        memory_stream.Position = 0
        Return CType(formatter.Deserialize(memory_stream), TEFile)
    End Function

    Public Function ChagneType() As Boolean
        pjData.TETempData.SaveTabitems()

        Select Case _FileType
            Case EFileType.CUIEps
                Dim la As New Lexical_Analyzer


                Try
                    Dim tokenlist As List(Of Token) = la.DoWork(_Scripter.GetStringText)


                    Dim parser As New Parser(tokenlist)

                    Dim GUIScrEditor As New GUIScriptEditor(ScriptEditor.SType.Eps, Me)

                    GUIScrEditor.SetItemsList(parser.GetGUIScripter())
                    GUIScrEditor.LoadInit()

                    _Scripter = GUIScrEditor

                    _FileType = EFileType.GUIEps

                    Return True
                Catch ex As Exception
                    Tool.ErrorMsgBox(Tool.GetText("TEFileChangeError"), ex.ToString)

                    Return False
                End Try
            Case EFileType.GUIEps
                Try

                    Dim CUIScrEditor As New CUIScriptEditor(ScriptEditor.SType.Eps)

                    CUIScrEditor.StringText = CType(_Scripter, GUIScriptEditor).GetStringText()

                    _Scripter = CUIScrEditor

                    _FileType = EFileType.CUIEps
                    Return True
                Catch ex As Exception
                    Tool.ErrorMsgBox(Tool.GetText("TEFileChangeError"), ex.ToString)

                    Return False
                End Try
        End Select
        Return False
    End Function
End Class

