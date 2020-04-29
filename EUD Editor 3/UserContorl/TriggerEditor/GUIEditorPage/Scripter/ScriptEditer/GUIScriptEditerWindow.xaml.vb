Public Class GUIScriptEditerWindow
    Public Event OkayBtnEvent As RoutedEventHandler
    Public Event CancelBtnEvent As RoutedEventHandler

    Public _GUIScriptEditorUI As GUIScriptEditorUI

    Private IsCreateOpen As Boolean
    Private scr As ScriptBlock
    Private dotscr As ScriptBlock
    Private ntreeview As TreeViewItem
    Public Sub New(tscr As ScriptBlock, tntreeview As TreeViewItem, dotscriptblock As ScriptBlock, tIsCreateOpen As Boolean, tGUIScriptEditorUI As GUIScriptEditorUI)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        scr = tscr
        dotscr = dotscriptblock
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


        Select Case scr.ScriptType
            Case ScriptBlock.EBlockType.fundefine
                Dim editer As New GUI_FuncEdit(Me, scr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case ScriptBlock.EBlockType._if, ScriptBlock.EBlockType._elseif
                Dim editer As New GUI_If(Me, scr, ntreeview)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case ScriptBlock.EBlockType.switch
                Dim editer As New GUI_Switch(Me, scr, dotscr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case ScriptBlock.EBlockType.switchcase
                Dim editer As New GUI_SwitchCase(Me, scr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case ScriptBlock.EBlockType._for
                Dim editer As New GUI_For(Me, scr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case ScriptBlock.EBlockType.folder
                Dim editer As New GUI_Folder(Me, scr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case ScriptBlock.EBlockType.objectdefine
                Dim editer As New GUI_Object(Me, scr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case ScriptBlock.EBlockType.vardefine
                Dim editer As New GUI_Var(Me, scr, dotscr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case ScriptBlock.EBlockType.import
                Dim editer As New GUI_Import(Me, scr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case ScriptBlock.EBlockType.varuse
                Dim editer As New GUI_VarUse(Me, scr, dotscr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case ScriptBlock.EBlockType.exp
                Dim editer As New GUI_Express(Me, scr, dotscr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case ScriptBlock.EBlockType.funreturn
                Dim editer As New GUI_Return(Me, scr, dotscr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case ScriptBlock.EBlockType.rawcode
                Dim editer As New GUI_RawCode(Me, scr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case ScriptBlock.EBlockType.plibfun, ScriptBlock.EBlockType.action, ScriptBlock.EBlockType.condition,
                 ScriptBlock.EBlockType.externfun, ScriptBlock.EBlockType.funuse, ScriptBlock.EBlockType.macrofun
                Dim editer As New GUI_Action(Me, scr, dotscr)

                cborder.Child = editer

                maingrid.Width = editer.Width + 32
                maingrid.Height = editer.Height + 32 + 48
            Case Else
                Return False
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

    Private isShift As Boolean = False
    Private Sub maingrid_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.RightShift Then
            isShift = True
        End If

        If e.Key = Key.Enter And isShift Then
            RaiseEvent OkayBtnEvent(scr, e)


            If Not IsCreateOpen Then
                scr.RefreshListBox(ntreeview)
            End If
        ElseIf e.Key = Key.Escape Then
            RaiseEvent CancelBtnEvent(sender, e)
        End If
    End Sub

    Private Sub UserControl_PreviewKeyUp(sender As Object, e As KeyEventArgs)
        If e.Key = Key.RightShift Then
            isShift = False
        End If
    End Sub
End Class
