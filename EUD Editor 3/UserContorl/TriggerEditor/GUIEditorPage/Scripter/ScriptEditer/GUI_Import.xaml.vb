Public Class GUI_Import



    Public Sub CrlInit()
        '//////////////////////////////
        '초기화 식
        Dim astext As String = scr.value.Split(" ").Last.Trim
        tb.Text = astext

        InitStartFileCombox("", pjData.TEData.PFIles)
    End Sub

    Private Sub InitStartFileCombox(Path As String, tTEfile As TEFile)
        Dim cfile As String = scr.value.Split(" ").First.Trim
        cfile = cfile.Replace(".", "\") & ".eps"

        For i = 0 To tTEfile.FileCount - 1
            If p._GUIScriptEditorUI.PTEFile IsNot tTEfile.Files(i) Then
                Dim Filename As String = Path & tTEfile.Files(i).RealFileName
                Dim tComboboxitem As New ComboBoxItem
                tComboboxitem.Tag = Filename
                tComboboxitem.Content = Filename
                StartFileCombobox.Items.Add(tComboboxitem)

                If cfile = Filename Then
                    StartFileCombobox.SelectedIndex = StartFileCombobox.Items.Count - 1
                End If
            End If
        Next
        If True Then
            Dim tComboboxitem As New ComboBoxItem
            tComboboxitem.Tag = "SCArchive"
            tComboboxitem.Content = "SCArchive"
            StartFileCombobox.Items.Add(tComboboxitem)
        End If

        For i = 0 To tTEfile.FolderCount - 1
            If tTEfile.Folders(i).FileType <> TEFile.EFileType.Setting Then
                Dim Filename As String = Path & tTEfile.Folders(i).FileName & "\"
                InitStartFileCombox(Filename, tTEfile.Folders(i))
            End If
        Next
    End Sub

    Public Sub OkayAction(sender As Object, e As RoutedEventArgs)
        '//////////////////////////////
        '스크립트 갱신
        Dim path As String = CType(StartFileCombobox.SelectedItem, ComboBoxItem).Tag

        scr.value = path.Replace("\", ".").Replace(".eps", "") & " as " & tb.Text
    End Sub
    Private Function CheckEditable() As Boolean
        '//////////////////////////////
        '확인을 누를 수 있는지 확인
        If tb.Text.Trim = "" Or StartFileCombobox.SelectedIndex = -1 Then
            Return False
        End If

        Return True
    End Function


    Private p As GUIScriptEditerWindow
    Private scr As ScriptBlock
    Public Sub New(tp As GUIScriptEditerWindow, tscr As ScriptBlock)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        p = tp
        scr = tscr

        AddHandler p.OkayBtnEvent, AddressOf OkayAction

        CrlInit()
        btnRefresh()
    End Sub
    Public Sub btnRefresh()
        If CheckEditable() Then
            p.OkBtn.IsEnabled = True
        Else
            p.OkBtn.IsEnabled = False
        End If
    End Sub

    Private Sub tb_TextChanged(sender As Object, e As TextChangedEventArgs)
        btnRefresh()
    End Sub

    Private Sub StartFileCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        btnRefresh()
    End Sub
End Class
