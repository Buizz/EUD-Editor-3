Public Class GUIScriptEditorUI
    Public TEGUIPage As TEGUIPage


    Public PTEFile As TEFile
    Private Script As GUIScriptEditor


    Private selpos As Integer = 1


    Public Sub SetTEGUIPage(tTEGUIPage As TEGUIPage)
        TEGUIPage = tTEGUIPage
    End Sub
    Public Sub LoadScript(tTEFile As TEFile)
        PTEFile = tTEFile


        Script = PTEFile.Scripter


        For i = 0 To Script.items.Count - 1
            MainTreeview.Items.Add(Script.items(i).GetTreeviewitem)
        Next

    End Sub

    Public Sub Save()
        'Script.SetJsonObject(TestTextbox.Text)
    End Sub



    Private Function IsItemSelected() As Boolean
        Return (MainTreeview.SelectedItem IsNot Nothing)
    End Function



    Private Sub CopySelectItem()

    End Sub
    Private Sub CutSelectItem()

    End Sub
    Private Sub PasteSelectItem()

    End Sub
    Private Sub tUndoItem()

    End Sub
    Private Sub tRedoItem()

    End Sub
    Private Sub DeleteSelectItem()
        If IsItemSelected() Then
            If Not f_DeleteItem(MainTreeview.SelectedItem) Then
                SnackBarDialog("지울 수 없는 아이템입니다.")
            End If
        End If
    End Sub

    Private Sub EditSelectItem()
        If IsItemSelected() Then
            OpenEditWindow(MainTreeview.SelectedItem)
        End If
    End Sub











    Private Sub ContextMenu_Opened(sender As Object, e As RoutedEventArgs)
        RefreshBtn()
    End Sub
    Private Sub RefreshBtn()
        '복사한 항목이 있냐 없냐 판단
        If False Then
            '복사한 아이템이 있을 경우
            PasteItem.IsEnabled = True
            Pastebtn.IsEnabled = True
        Else
            '복사한 아이템이 없을 경우
            PasteItem.IsEnabled = False
            Pastebtn.IsEnabled = False
        End If


        If Not IsItemSelected() Then
            '선택한 아이템이 없을 경우
            CutItem.IsEnabled = False
            Cutbtn.IsEnabled = False
            CopyItem.IsEnabled = False
            Copybtn.IsEnabled = False
            deselectItem.IsEnabled = False
            DeSelectbtn.IsEnabled = False

            EditItem.IsEnabled = False
            Editbtn.IsEnabled = False

            DeleteItem.IsEnabled = False
            Deletebtn.IsEnabled = False
        Else
            '선택한 아이템이 있을 경우
            CutItem.IsEnabled = True
            Cutbtn.IsEnabled = True
            CopyItem.IsEnabled = True
            Copybtn.IsEnabled = True
            deselectItem.IsEnabled = True
            DeSelectbtn.IsEnabled = True

            EditItem.IsEnabled = True
            Editbtn.IsEnabled = True

            DeleteItem.IsEnabled = True
            Deletebtn.IsEnabled = True
        End If



    End Sub

    Private Sub CutItem_Click(sender As Object, e As RoutedEventArgs)
        CutSelectItem()
    End Sub
    Private Sub CopyItem_Click(sender As Object, e As RoutedEventArgs)
        CopySelectItem()
    End Sub
    Private Sub PasteItem_Click(sender As Object, e As RoutedEventArgs)
        PasteSelectItem()
    End Sub
    Private Sub DeleteItem_Click(sender As Object, e As RoutedEventArgs)
        DeleteSelectItem()
    End Sub
    Private Sub UndoItem_Click(sender As Object, e As RoutedEventArgs)
        tUndoItem()
    End Sub
    Private Sub RedoItem_Click(sender As Object, e As RoutedEventArgs)
        tRedoItem()
    End Sub
    Private Sub EditItem_Click(sender As Object, e As RoutedEventArgs)
        EditSelectItem()
    End Sub
    Private Sub deSelectItem_Click(sender As Object, e As RoutedEventArgs)
        tdeSelectItem()
    End Sub

    Private Sub tdeSelectItem()
        Dim tsel As TreeViewItem = MainTreeview.SelectedItem

        If tsel IsNot Nothing Then
            tsel.IsSelected = False
        End If

        'RefreshBtn()
    End Sub

    Private leftCtrlDown As Boolean
    Private leftShiftDown As Boolean
    Private Sub MainTreeview_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If leftCtrlDown Then
            Select Case e.Key
                Case Key.C
                    CopySelectItem()
                Case Key.X
                    CutSelectItem()
                Case Key.V
                    PasteSelectItem()
                Case Key.Z
                    tUndoItem()
                Case Key.R
                    tRedoItem()
            End Select
            Return
        End If


        Select Case e.Key
            Case Key.LeftCtrl
                leftCtrlDown = True
            Case Key.LeftShift
                leftShiftDown = True
            Case Key.Delete
                DeleteSelectItem()
            Case Key.Enter
                EditSelectItem()
            Case Key.Escape
                tdeSelectItem()
        End Select
    End Sub
    Private Sub MainTreeview_PreviewKeyUp(sender As Object, e As KeyEventArgs)
        If e.Key = Key.LeftCtrl Then
            leftCtrlDown = False
        ElseIf e.Key = Key.LeftShift Then
            leftShiftDown = False
        End If
    End Sub
    Private Sub UpSel_Click(sender As Object, e As RoutedEventArgs)
        upbtn.IsEnabled = False
        downbtn.IsEnabled = True

        selpos = 0
    End Sub
    Private Sub DownSel_Click(sender As Object, e As RoutedEventArgs)
        upbtn.IsEnabled = True
        downbtn.IsEnabled = False

        selpos = 1
    End Sub
    Private Sub DeSelectbtn_Click(sender As Object, e As RoutedEventArgs)
        tdeSelectItem()
    End Sub
    Private Sub Editbtn_Click(sender As Object, e As RoutedEventArgs)
        EditSelectItem()
    End Sub
    Private Sub Cutbtn_Click(sender As Object, e As RoutedEventArgs)
        CutSelectItem()
    End Sub
    Private Sub Copybtn_Click(sender As Object, e As RoutedEventArgs)
        CopySelectItem()
    End Sub
    Private Sub Pastebtn_Click(sender As Object, e As RoutedEventArgs)
        PasteSelectItem()
    End Sub
    Private Sub Deletebtn_Click(sender As Object, e As RoutedEventArgs)
        DeleteSelectItem()
    End Sub
    Private Sub MainTreeview_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object))
        RefreshBtn()
    End Sub


    Private sMessageQueue As MaterialDesignThemes.Wpf.SnackbarMessageQueue
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        RefreshBtn()
        sMessageQueue = New MaterialDesignThemes.Wpf.SnackbarMessageQueue
        ErrorSnackbar.MessageQueue = sMessageQueue
    End Sub
End Class
