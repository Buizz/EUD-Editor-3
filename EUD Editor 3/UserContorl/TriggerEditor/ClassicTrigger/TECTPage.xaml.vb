Public Class TECTPage
    Private PTEFile As TEFile
    Private Scripter As ClassicTriggerEditor
    Public ReadOnly Property TEFile As TEFile
        Get
            Return PTEFile
        End Get
    End Property

    Public Function CheckTEFile(tTEfile As TEFile) As Boolean
        Return (tTEfile Is PTEFile)
    End Function


    Public Sub Init()
        '트리거 리스트를 정리
        'TListBox.Items.Clear()
        PlayerList.SelectedIndex = 0
        RefreshGlobalObject()
    End Sub

    Public Sub New(tTEFile As TEFile)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        PTEFile = tTEFile
        Scripter = PTEFile.Scripter
        'TextEditor.Init(tTEFile)
        'TextEditor.Text = CType(TEFile.Scripter, CUIScriptEditor).StringText

        Init()
    End Sub


    Public Sub SaveData()
        'CType(TEFile.Scripter, CUIScriptEditor).StringText = TextEditor.Text
        'TEFile.LastDataRefresh()
    End Sub



    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        'If TEFile.FileType = TEFile.EFileType.CUIEps Then
        'CType(TEFile.Scripter, CUIScriptEditor).StringText = TextEditor.Text
        'End If
    End Sub

    Private Sub PlayerList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        RefreshTriggerPage(PlayerList.SelectedIndex)
    End Sub




    Private Sub GlobalInsertBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim nVarname As String = "var:Var" & Scripter.globalVar.Count
        'var,pvar,array,varray


        '편집창열기!


        Scripter.globalVar.Add(nVarname)
        GlobalList.Items.Add(nVarname)
    End Sub

    Private Sub GlobalDeleteBtn_Click(sender As Object, e As RoutedEventArgs)
        If GlobalList.SelectedItem IsNot Nothing Then
            Dim t As String = GlobalList.SelectedItem
            Dim datalistindex As Integer = Scripter.globalVar.IndexOf(t)
            Dim listboxindex As Integer = GlobalList.SelectedIndex


            Scripter.globalVar.Remove(t)
            GlobalList.Items.Remove(t)

            If GlobalList.Items.Count > listboxindex Then
                GlobalList.SelectedIndex = listboxindex
            Else
                GlobalList.SelectedIndex = GlobalList.Items.Count - 1
            End If
        End If
    End Sub

    Private Sub GlobalEditBtn_Click(sender As Object, e As RoutedEventArgs)
        If GlobalList.SelectedItem IsNot Nothing Then
            Dim t As String = GlobalList.SelectedItem
            Dim datalistindex As Integer = Scripter.globalVar.IndexOf(t)
            Dim listboxindex As Integer = GlobalList.SelectedIndex

            Dim editname As String = "var:EditVar" & Scripter.globalVar.Count


            '편집창열기!


            Scripter.globalVar(datalistindex) = editname
            GlobalList.Items(listboxindex) = editname

            GlobalList.SelectedIndex = listboxindex
            'Scripter.globalVar.Remove(t)
            'GlobalList.Items.Remove(t)
        End If
    End Sub

    Private Sub GlobalList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If GlobalList.SelectedIndex = -1 Then
            GlobalEditBtn.IsEnabled = False
            GlobalDeleteBtn.IsEnabled = False
        Else
            GlobalEditBtn.IsEnabled = True
            GlobalDeleteBtn.IsEnabled = True
        End If
    End Sub

    Private Sub ImportInsertBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim nVarname As String = "var:Var" & Scripter.ImportFiles.Count
        'var,pvar,array,varray


        '편집창열기!


        Scripter.ImportFiles.Add(nVarname)
        ImportList.Items.Add(nVarname)
    End Sub

    Private Sub ImportEditBtn_Click(sender As Object, e As RoutedEventArgs)
        If ImportList.SelectedItem IsNot Nothing Then
            Dim t As String = ImportList.SelectedItem
            Dim datalistindex As Integer = Scripter.ImportFiles.IndexOf(t)
            Dim listboxindex As Integer = ImportList.SelectedIndex

            Dim editname As String = "var:EditVar" & Scripter.ImportFiles.Count


            '편집창열기!


            Scripter.ImportFiles(datalistindex) = editname
            ImportList.Items(listboxindex) = editname

            ImportList.SelectedIndex = listboxindex
        End If
    End Sub

    Private Sub ImportDeleteBtn_Click(sender As Object, e As RoutedEventArgs)
        If ImportList.SelectedItem IsNot Nothing Then
            Dim t As String = ImportList.SelectedItem
            Dim datalistindex As Integer = Scripter.ImportFiles.IndexOf(t)
            Dim listboxindex As Integer = ImportList.SelectedIndex


            Scripter.ImportFiles.Remove(t)
            ImportList.Items.Remove(t)

            If ImportList.Items.Count > listboxindex Then
                ImportList.SelectedIndex = listboxindex
            Else
                ImportList.SelectedIndex = ImportList.Items.Count - 1
            End If
        End If
    End Sub

    Private Sub ImportList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If ImportList.SelectedIndex = -1 Then
            ImportEditBtn.IsEnabled = False
            ImportDeleteBtn.IsEnabled = False
        Else
            ImportEditBtn.IsEnabled = True
            ImportDeleteBtn.IsEnabled = True
        End If
    End Sub
End Class
