Imports System.IO
Imports Dragablz

Partial Public Class ProjectData
    Private LastModifiyTimer As Date

    Private Sub FileCheck_Tick(sender As Object, e As EventArgs)
        If IsMapLoading Then
            If My.Computer.FileSystem.FileExists(OpenMapName) Then
                If LastModifiyTimer <> File.GetLastWriteTime(OpenMapName) Then
                    IsMapLoading = MapData.ReLoad(OpenMapName)
                    _MapData = New MapData(OpenMapName)
                    IsMapLoading = _MapData.LoadComplete
                    If Not IsMapLoading Then
                        _MapData = Nothing
                        OpenMapName = ""
                    End If

                    If Not MapData.ReLoad(OpenMapName) Then

                    End If
                End If
            Else
                _MapData = Nothing
                IsMapLoading = False
                OpenMapName = ""
            End If
        End If
        TERefreshTabITem()
    End Sub
    Private Sub TERefreshTabITem()
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

                tPage.RefreshData()
            End If
        Next



        Return False
    End Function
End Class
