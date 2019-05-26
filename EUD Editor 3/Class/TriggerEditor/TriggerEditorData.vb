Imports Dragablz

<Serializable>
Public Class TriggerEditorData
    Private _SCArchive As StarCraftArchive
    Public ReadOnly Property SCArchive As StarCraftArchive
        Get
            Return _SCArchive
        End Get
    End Property





    Public Const TopFileName As String = "​ProjectMain"


    Private _MainFile As TEFile
    Public Property MainFile As TEFile
        Get
            Return _MainFile
        End Get
        Set(value As TEFile)
            _MainFile = value
            pjData.SetDirty(True)
        End Set
    End Property
    Public Function GetMainFilePath() As String
        Return _GetMainFilePath("", ProjectFile)
    End Function
    Private Function _GetMainFilePath(Path As String, tTEFile As TEFile) As String
        For i = 0 To tTEFile.FileCount - 1
            If tTEFile.Files(i) Is _MainFile Then
                Return Path & tTEFile.Files(i).RealFileName
            End If
        Next
        For i = 0 To tTEFile.FolderCount - 1
            Dim CPath As String = Path & tTEFile.Folders(i).FileName & "\"
            If _GetMainFilePath(CPath, tTEFile.Folders(i)) <> "" Then
                Return _GetMainFilePath(CPath, tTEFile.Folders(i))
            End If
        Next
        Return ""
    End Function



    Private ProjectFile As TEFile
    Public ReadOnly Property PFIles As TEFile
        Get
            Return ProjectFile
        End Get
    End Property



    Private _LastOpenTabs As LastTab
    Public ReadOnly Property LastOpenTabs As LastTab
        Get
            Return _LastOpenTabs
        End Get
    End Property
    <Serializable>
    Public Class LastTab
        Public Items As List(Of TEFile)
        Public FristItem As LastTab
        Public SecondItem As LastTab
        Public Orientation As Orientation

        Public Sub New()
            Items = New List(Of TEFile)
        End Sub
    End Class

    Public Sub New()
        ProjectFile = New TEFile(TopFileName, TEFile.EFileType.Folder)
        ProjectFile.FolderAdd(New TEFile("​​" & Tool.GetText("Setting"), TEFile.EFileType.Setting))

        _LastOpenTabs = New LastTab
        _SCArchive = New StarCraftArchive
    End Sub
End Class

Public Class TriggerEditorTempData
    Public Sub SaveTabitems()
        TESaveTabITem()
    End Sub

    Private Sub TESaveTabITem()
        '우선 모든 윈도우 돌면서 조사하자
        For Each win As Window In Application.Current.Windows
            If win.GetType Is GetType(TriggerEditor) Then
                Dim MainContent As Object = CType(win, TriggerEditor).MainTab


                If CheckBranch(MainContent) Then
                    Exit Sub
                End If



                'If CheckTabablzControl(tTEFile, MainContent) Then
                '    win.Activate()
                '    Return True
                'End If
            End If
        Next
    End Sub
    Private Function CheckBranch(ParentBranch As Object) As Boolean
        '우선 모든 윈도우 돌면서 조사하자
        While TypeOf ParentBranch IsNot TabablzControl
            Select Case ParentBranch.GetType
                Case GetType(TabablzControl)
                    Exit While
                Case GetType(Dockablz.Branch)
                    Dim tBranch As Dockablz.Branch = ParentBranch

                    If CheckBranch(ParentBranch.FirstItem) Then
                        Return True
                    End If
                    If CheckBranch(ParentBranch.SecondItem) Then
                        Return True
                    End If
                    Return False
                    Exit While
                Case GetType(Dockablz.Layout)
                    Dim tLayout As Dockablz.Layout = ParentBranch
                    ParentBranch = tLayout.Content
            End Select
        End While
        Return CheckTabablzControl(ParentBranch)

        Return False
    End Function
    Private Function CheckTabablzControl(Control As TabablzControl) As Boolean
        For i = 0 To Control.Items.Count - 1
            Dim TabContent As Object = CType(Control.Items(i), TabItem).Content

            If TypeOf TabContent Is TECUIPage Then
                Dim tPage As TECUIPage = TabContent
                tPage.SaveData()
            ElseIf TypeOf TabContent Is TEGUIPage Then
                Dim tPage As TEGUIPage = TabContent
                tPage.SaveData()
            End If
        Next


        Return False
    End Function
End Class