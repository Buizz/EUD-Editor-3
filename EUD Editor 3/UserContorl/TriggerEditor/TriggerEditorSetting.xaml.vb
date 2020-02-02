Public Class TriggerEditorSetting
    Private PTEFile As TEFile
    Public ReadOnly Property TEFile As TEFile
        Get
            Return PTEFile
        End Get
    End Property

    Public Function CheckTEFile(tTEfile As TEFile) As Boolean
        Return (tTEfile Is PTEFile)
    End Function


    Public Sub New(tTEFile As TEFile)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        PTEFile = tTEFile
    End Sub


    Private Sub StartFileCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If LoadCmp Then
            pjData.SetDirty(True)
            If StartFileCombobox.SelectedItem IsNot Nothing Then
                If StartFileCombobox.SelectedIndex = 0 Then
                    pjData.TEData.MainFile = Nothing
                Else
                    pjData.TEData.MainFile = CType(StartFileCombobox.SelectedItem, ComboBoxItem).Tag
                End If
            End If
        End If
    End Sub

    Private LoadCmp As Boolean = False
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        StartFileCombobox.Items.Clear()
        StartFileCombobox.Items.Add(Tool.GetText("None"))
        StartFileCombobox.SelectedIndex = 0
        InitStartFileCombox("", pjData.TEData.PFIles)
        LoadCmp = True








        'UseSCA.IsChecked = pjData.TEData.SCArchive.IsUsed
        'MakerBattleTag.Text = pjData.TEData.SCArchive.MakerBattleTag
        'MakerID.Text = pjData.TEData.SCArchive.MakerServerName
        'UseMapName.Text = pjData.TEData.SCArchive.MapName
        'UMSPassWord.Text = pjData.TEData.SCArchive.PassWord
        'DataBufferSize.Text = pjData.TEData.SCArchive.DataSpace
    End Sub
    Private Sub InitStartFileCombox(Path As String, tTEfile As TEFile)
        For i = 0 To tTEfile.FileCount - 1
            Dim Filename As String = Path & tTEfile.Files(i).RealFileName
            Dim tComboboxitem As New ComboBoxItem
            tComboboxitem.Tag = tTEfile.Files(i)
            tComboboxitem.Content = Filename
            StartFileCombobox.Items.Add(tComboboxitem)

            If pjData.TEData.MainFile Is tTEfile.Files(i) Then
                StartFileCombobox.SelectedIndex = StartFileCombobox.Items.Count - 1
            End If
        Next


        For i = 0 To tTEfile.FolderCount - 1
            If tTEfile.Folders(i).FileType <> TEFile.EFileType.Setting Then
                Dim Filename As String = Path & tTEfile.Folders(i).FileName & "\"
                InitStartFileCombox(Filename, tTEfile.Folders(i))
            End If
        Next
    End Sub


    'Private Sub MakerBattleTag_TextChanged(sender As Object, e As TextChangedEventArgs)
    '    If LoadCmp Then
    '        pjData.SetDirty(True)
    '        pjData.TEData.SCArchive.MakerBattleTag = MakerBattleTag.Text
    '    End If
    'End Sub

    'Private Sub MakerID_TextChanged(sender As Object, e As TextChangedEventArgs)
    '    If LoadCmp Then
    '        pjData.SetDirty(True)
    '        pjData.TEData.SCArchive.MakerServerName = MakerID.Text
    '    End If
    'End Sub

    'Private Sub UseMapName_TextChanged(sender As Object, e As TextChangedEventArgs)
    '    If LoadCmp Then
    '        pjData.SetDirty(True)
    '        pjData.TEData.SCArchive.MapName = UseMapName.Text
    '    End If
    'End Sub

    'Private Sub UMSPassWord_TextChanged(sender As Object, e As TextChangedEventArgs)
    '    If LoadCmp Then
    '        pjData.SetDirty(True)
    '        pjData.TEData.SCArchive.PassWord = UMSPassWord.Text
    '    End If
    'End Sub

    'Private Sub DataBufferSize_TextChanged(sender As Object, e As TextChangedEventArgs)
    '    If LoadCmp Then
    '        pjData.SetDirty(True)
    '        Try
    '            pjData.TEData.SCArchive.DataSpace = DataBufferSize.Text
    '        Catch ex As Exception

    '        End Try
    '    End If
    'End Sub
End Class
