Public Class GUIScriptEditerWindow
    Public Event OkayBtnEvent As RoutedEventHandler
    Public Event CancelBtnEvent As RoutedEventHandler

    Public _GUIScriptEditorUI As GUIScriptEditorUI

    Private IsCreateOpen As Boolean
    Private scr As ScriptBlock
    Private ntreeview As TreeViewItem
    Public Sub New(tscr As ScriptBlock, tntreeview As TreeViewItem, tIsCreateOpen As Boolean, tGUIScriptEditorUI As GUIScriptEditorUI)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        scr = tscr
        ntreeview = tntreeview
        IsCreateOpen = tIsCreateOpen

        _GUIScriptEditorUI = tGUIScriptEditorUI
    End Sub


    Public Function Init() As Boolean
        If scr.IsArgument Then
            Dim editer As New GUI_FuncArgVal(Me, scr)

            cborder.Child = editer

            maingrid.Width = editer.Width + 32
            maingrid.Height = editer.Height + 32 + 48
            Return True
        End If


        Select Case scr.name
            Case "function"
                'MsgBox("함수추가")
                Dim editer As New GUI_FuncEdit(Me, scr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case Else
                If scr.IsEditFunc Then
                    Dim editer As New GUI_Action(Me, scr)

                    cborder.Child = editer

                    maingrid.Width = editer.Width + 32
                    maingrid.Height = editer.Height + 32 + 48
                Else
                    Return False
                End If

        End Select



        Return True
    End Function




    Private Sub OkBtn_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent OkayBtnEvent(scr, e)


        If Not IsCreateOpen Then
            scr.RefreshListBox(ntreeview)
        End If

        pjData.SetDirty(True)
    End Sub
    Private Sub CancleBtn_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent CancelBtnEvent(sender, e)
    End Sub

    Private Sub maingrid_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Then
            RaiseEvent OkayBtnEvent(scr, e)


            If Not IsCreateOpen Then
                scr.RefreshListBox(ntreeview)
            End If
        ElseIf e.Key = Key.Escape Then
            RaiseEvent CancelBtnEvent(sender, e)
        End If
    End Sub
End Class
