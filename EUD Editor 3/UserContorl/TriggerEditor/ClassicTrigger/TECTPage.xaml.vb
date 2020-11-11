Imports System.Windows.Media.Animation

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


    Public Sub PlayerListReset()

        Dim PlayerFlag(7) As Boolean
        Dim TriggerCount(7) As Integer

        Dim LastSelect As Integer = GetPlayerListIndex()
        PlayerList.Items.Clear()

        For i = 0 To Scripter.TriggerList.Count - 1
            For j = 0 To Scripter.TriggerList(i).PlayerEnabled.Count - 1
                If Scripter.TriggerList(i).PlayerEnabled(j) Then
                    PlayerFlag(j) = Scripter.TriggerList(i).PlayerEnabled(j)
                    TriggerCount(j) += 1
                End If
            Next
        Next

        For i = 0 To 7
            If PlayerFlag(i) Then
                Dim tlistitem As New ListBoxItem

                tlistitem.Padding = New Thickness(10)

                tlistitem.Content = "Player " & i + 1 & vbCrLf & TriggerCount(i) & "개"
                tlistitem.Tag = i

                PlayerList.Items.Add(tlistitem)
            End If
        Next

        SetPlayerListIndex(LastSelect)
    End Sub
    Public Function GetPlayerListIndex() As Integer
        If PlayerList.SelectedItem IsNot Nothing Then
            Dim SelectIndex As Integer = CType(PlayerList.SelectedItem, ListBoxItem).Tag
            Return SelectIndex
        End If
        Return -1
    End Function


    Public Sub SetPlayerListIndex(index As Integer)
        For i = 0 To PlayerList.Items.Count - 1
            Dim SelectIndex As Integer = CType(PlayerList.Items(i), ListBoxItem).Tag

            If SelectIndex = index Then
                PlayerList.SelectedIndex = i
                Return
            End If
        Next
        If PlayerList.Items.Count <> 0 Then
            PlayerList.SelectedIndex = 0
        End If
    End Sub

    Public Sub Init()
        '트리거 리스트를 정리
        'TListBox.Items.Clear()
        RefreshGlobalObject()
        AnimationInit()
        Scripter.ImportFileRefresh()

        PlayerListReset()
        SetPlayerListIndex(0)
        RefreshTriggerPage()

        GlobalPanel.Visibility = Visibility.Collapsed
        ImportPanel.Visibility = Visibility.Collapsed

        GlobalList.Items.Clear()
        ImportList.Items.Clear()

        For i = 0 To Scripter.globalVar.Count - 1
            Dim nlist As New ListBoxItem
            nlist.Content = Scripter.globalVar(i).vname & ":" & Scripter.globalVar(i).vtype
            nlist.Tag = Scripter.globalVar(i)

            GlobalList.Items.Add(nlist)
        Next



        For i = 0 To Scripter.ImportFiles.Count - 1
            Dim nlist As New ListBoxItem
            nlist.Content = Scripter.ImportFiles(i).vname & ":" & Scripter.ImportFiles(i).vfolder
            nlist.Tag = Scripter.ImportFiles(i)

            ImportList.Items.Add(nlist)
        Next







        'TListBox.Items.Clear()
        'For i = 0 To Scripter.TriggerList.Count - 1
        '    TListBox.Items.Add(GetListItem(Scripter.TriggerList(i)))
        'Next
    End Sub



    Private OpenStroyBoard As Storyboard
    Private CloseStroyBoard As Storyboard
    Private Sub AnimationInit()
        If True Then
            Dim scale1 As ScaleTransform = New ScaleTransform(1, 1)

            InputDialog.RenderTransformOrigin = New Point(0.5, 0.5)
            InputDialog.RenderTransform = scale1



            Dim myHeightAnimation As DoubleAnimation = New DoubleAnimation()
            myHeightAnimation.From = 0.5
            myHeightAnimation.To = 1.0
            myHeightAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myWidthAnimation As DoubleAnimation = New DoubleAnimation()
            myWidthAnimation.From = 0.5
            myWidthAnimation.To = 1.0
            myWidthAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myOpacityAnimation As DoubleAnimation = New DoubleAnimation()
            myOpacityAnimation.From = 0.0
            myOpacityAnimation.To = 1.0
            myOpacityAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            OpenStroyBoard = New Storyboard()
            OpenStroyBoard.Children.Add(myOpacityAnimation)
            OpenStroyBoard.Children.Add(myWidthAnimation)
            OpenStroyBoard.Children.Add(myHeightAnimation)
            Storyboard.SetTargetName(myOpacityAnimation, EditWindow.Name)
            Storyboard.SetTargetName(myWidthAnimation, InputDialog.Name)
            Storyboard.SetTargetName(myHeightAnimation, InputDialog.Name)
            Storyboard.SetTargetProperty(myOpacityAnimation, New PropertyPath(Border.OpacityProperty))
            Storyboard.SetTargetProperty(myHeightAnimation, New PropertyPath("RenderTransform.ScaleY"))
            Storyboard.SetTargetProperty(myWidthAnimation, New PropertyPath("RenderTransform.ScaleX"))
        End If
        If True Then
            Dim scale1 As ScaleTransform = New ScaleTransform(1, 1)

            InputDialog.RenderTransformOrigin = New Point(0.5, 0.5)
            InputDialog.RenderTransform = scale1



            Dim myHeightAnimation As DoubleAnimation = New DoubleAnimation()
            myHeightAnimation.From = 1.0
            myHeightAnimation.To = 0.5
            myHeightAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myWidthAnimation As DoubleAnimation = New DoubleAnimation()
            myWidthAnimation.From = 1.0
            myWidthAnimation.To = 0.5
            myWidthAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myOpacityAnimation As DoubleAnimation = New DoubleAnimation()
            myOpacityAnimation.From = 1.0
            myOpacityAnimation.To = 0.0
            myOpacityAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            CloseStroyBoard = New Storyboard()
            CloseStroyBoard.Children.Add(myOpacityAnimation)
            CloseStroyBoard.Children.Add(myWidthAnimation)
            CloseStroyBoard.Children.Add(myHeightAnimation)
            Storyboard.SetTargetName(myOpacityAnimation, EditWindow.Name)
            Storyboard.SetTargetName(myWidthAnimation, InputDialog.Name)
            Storyboard.SetTargetName(myHeightAnimation, InputDialog.Name)
            Storyboard.SetTargetProperty(myOpacityAnimation, New PropertyPath(Border.OpacityProperty))
            Storyboard.SetTargetProperty(myHeightAnimation, New PropertyPath("RenderTransform.ScaleY"))
            Storyboard.SetTargetProperty(myWidthAnimation, New PropertyPath("RenderTransform.ScaleX"))

            AddHandler CloseStroyBoard.Completed, Sub(sender As Object, e As EventArgs)
                                                      EditWindow.Visibility = Visibility.Hidden
                                                  End Sub
        End If



        'InputDialog
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
        RefreshTriggerPage()
    End Sub



    Private IsGlobalEditOpen As Boolean
    Private Sub GlobalInsertBtn_Click(sender As Object, e As RoutedEventArgs)
        ValType.SelectedIndex = -1
        ValName.Text = ""
        InitCB.IsChecked = False
        VariableCard.Height = 130
        GlobalOkayBtn.IsEnabled = False

        IsGlobalEditOpen = False
        GlobalPanel.Visibility = Visibility.Visible
    End Sub

    Private Sub GlobalDeleteBtn_Click(sender As Object, e As RoutedEventArgs)
        If GlobalList.SelectedItem IsNot Nothing Then
            Dim t As DefineVariable = CType(GlobalList.SelectedItem, ListBoxItem).Tag
            Dim datalistindex As Integer = Scripter.globalVar.IndexOf(t)
            Dim listboxindex As Integer = GlobalList.SelectedIndex

            Scripter.globalVar.RemoveAt(datalistindex)
            GlobalList.Items.RemoveAt(listboxindex)

            If GlobalList.Items.Count > listboxindex Then
                GlobalList.SelectedIndex = listboxindex
            Else
                GlobalList.SelectedIndex = GlobalList.Items.Count - 1
            End If
            pjData.SetDirty(True)
        End If
    End Sub

    Private Sub GlobalEditBtn_Click(sender As Object, e As RoutedEventArgs)
        If GlobalList.SelectedItem IsNot Nothing Then
            Dim t As DefineVariable = CType(GlobalList.SelectedItem, ListBoxItem).Tag
            Dim datalistindex As Integer = Scripter.globalVar.IndexOf(t)
            Dim ValueData As DefineVariable = t



            ValType.SelectedIndex = -1
            For i = 0 To ValType.Items.Count - 1
                Dim _type As String = CType(ValType.Items(i), ComboBoxItem).Tag
                If _type = ValueData.vtype Then
                    ValType.SelectedIndex = i
                    Exit For
                End If
            Next
            ValName.Text = ValueData.vname

            If ValueData.vinit = "NULL" Then
                GlobalInitBox.Text = ""
                InitCB.IsChecked = False
                VariableCard.Height = 130
                GlobalInitBox.IsEnabled = False
            Else
                GlobalInitBox.IsEnabled = True
                GlobalInitBox.Text = ValueData.vinit
                VariableCard.Height = Double.NaN
            End If




            IsGlobalEditOpen = True
            GlobalPanel.Visibility = Visibility.Visible
        End If
    End Sub
    Private Sub GlobalOkayBtn_Click(sender As Object, e As RoutedEventArgs)
        If IsGlobalEditOpen Then
            Dim t As DefineVariable = CType(GlobalList.SelectedItem, ListBoxItem).Tag
            Dim datalistindex As Integer = Scripter.globalVar.IndexOf(t)
            Dim listboxindex As Integer = GlobalList.SelectedIndex


            Dim _type As String = CType(ValType.SelectedItem, ComboBoxItem).Tag
            Dim nVarname As String = ValName.Text
            Dim nVartype As String = _type
            Dim nVarinit As String

            If InitCB.IsChecked Then
                nVarinit = GlobalInitBox.Text
            Else
                nVarinit = "NULL"
            End If


            t.vname = nVarname
            t.vtype = nVartype
            t.vinit = nVarinit

            '편집창열기!


            Dim tlist As ListBoxItem = GlobalList.SelectedItem
            tlist.Content = nVarname & ":" & nVartype

            GlobalList.SelectedIndex = listboxindex

            pjData.SetDirty(True)
        Else
            Dim _type As String = CType(ValType.SelectedItem, ComboBoxItem).Tag


            Dim nVarname As String = ValName.Text
            Dim nVartype As String = _type
            Dim nVarinit As String

            If InitCB.IsChecked Then
                nVarinit = GlobalInitBox.Text
            Else
                nVarinit = "NULL"
            End If

            'var,pvar,array,varray


            '편집창열기!

            Dim dv As New DefineVariable(nVarname, nVartype, nVarinit, "")


            Dim nlist As New ListBoxItem
            nlist.Content = nVarname & ":" & nVartype
            nlist.Tag = dv


            Scripter.globalVar.Add(dv)
            GlobalList.Items.Add(nlist)
            pjData.SetDirty(True)
        End If
        GlobalPanel.Visibility = Visibility.Collapsed
    End Sub

    Private Sub GlobalCancelBtn_Click(sender As Object, e As RoutedEventArgs)
        GlobalPanel.Visibility = Visibility.Collapsed
    End Sub

    Private Sub GlobalPopupBtnRefresh()
        If ValType.SelectedIndex = -1 Then
            GlobalOkayBtn.IsEnabled = False
            Return
        End If

        Dim vtext As String = ValName.Text

        If vtext.Trim = "" Then
            GlobalOkayBtn.IsEnabled = False
            Return
        End If



        GlobalOkayBtn.IsEnabled = True
    End Sub

    Private Sub InitCBChecked(sender As Object, e As RoutedEventArgs)
        If InitCB.IsChecked Then
            GlobalInitBox.IsEnabled = True
            VariableCard.Height = Double.NaN
        Else
            InitCB.IsChecked = False
            VariableCard.Height = 130
            GlobalInitBox.IsEnabled = False
        End If
    End Sub


    Private Sub ValType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        GlobalPopupBtnRefresh()
    End Sub

    Private Sub ValName_TextChanged(sender As Object, e As TextChangedEventArgs)
        GlobalPopupBtnRefresh()
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














    Private IsImportEditOpen As Boolean
    Private Sub ImportInsertBtn_Click(sender As Object, e As RoutedEventArgs)
        RefreshImportFileLst()
        ImportPanel.Visibility = Visibility.Visible
        IsImportEditOpen = False

        ImportOkayBtn.IsEnabled = False
        ImportNameTB.Text = ""
        ImportCombobox.SelectedIndex = -1
    End Sub


    Private Sub ImportEditBtn_Click(sender As Object, e As RoutedEventArgs)
        If ImportList.SelectedItem IsNot Nothing Then
            RefreshImportFileLst()
            IsImportEditOpen = True
            ImportPanel.Visibility = Visibility.Visible
            ImportOkayBtn.IsEnabled = True


            Dim t As DefineImport = CType(ImportList.SelectedItem, ListBoxItem).Tag
            ImportNameTB.Text = t.vname
            ImportCombobox.Text = t.vfolder
        End If
    End Sub

    Public Sub RefreshImportFileLst()
        ImportCombobox.Items.Clear()

        For Each efile As String In tmanager.GetImportList()
            Dim cbitem As New ComboBoxItem
            cbitem.Content = efile

            ImportCombobox.Items.Add(cbitem)
        Next

    End Sub


    Private Sub ImportDeleteBtn_Click(sender As Object, e As RoutedEventArgs)
        If ImportList.SelectedItem IsNot Nothing Then
            Dim t As DefineImport = CType(ImportList.SelectedItem, ListBoxItem).Tag
            Dim datalistindex As Integer = Scripter.ImportFiles.IndexOf(t)
            Dim listboxindex As Integer = ImportList.SelectedIndex


            Scripter.ImportFiles.RemoveAt(datalistindex)
            ImportList.Items.RemoveAt(listboxindex)

            If ImportList.Items.Count > listboxindex Then
                ImportList.SelectedIndex = listboxindex
            Else
                ImportList.SelectedIndex = ImportList.Items.Count - 1
            End If
            Scripter.ImportFileRefresh()

            pjData.SetDirty(True)
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

    Private Sub TListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        BtnRefresh()
    End Sub
    Private Sub BtnRefresh()
        If TListBox.SelectedIndex = -1 Then
            TriggerEditBtn.IsEnabled = False
            TriggerDeleteBtn.IsEnabled = False
            TriggerCopyBtn.IsEnabled = False
            TriggerCutBtn.IsEnabled = False

            TriggerUpBtn.IsEnabled = False
            TriggerDownBtn.IsEnabled = False
        Else
            TriggerEditBtn.IsEnabled = True
            TriggerDeleteBtn.IsEnabled = True
            TriggerCopyBtn.IsEnabled = True
            TriggerCutBtn.IsEnabled = True

            Dim upAble As Boolean = True
            Dim downAble As Boolean = True
            For i = 0 To TListBox.SelectedItems.Count - 1
                If TListBox.Items.IndexOf(TListBox.SelectedItems(i)) = 0 Then
                    upAble = False
                End If
            Next
            TriggerUpBtn.IsEnabled = upAble


            For i = 0 To TListBox.SelectedItems.Count - 1
                If TListBox.Items.IndexOf(TListBox.SelectedItems(i)) = TListBox.Items.Count - 1 Then
                    downAble = False
                End If
            Next
            TriggerDownBtn.IsEnabled = downAble
        End If

        TriggerPasteBtn.IsEnabled = IsPasteAble()
    End Sub

    Private Sub TListBox_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        EditTrigger()
    End Sub

    Private Sub UserControl_MouseEnter(sender As Object, e As MouseEventArgs)
        TriggerPasteBtn.IsEnabled = IsPasteAble()
    End Sub

    Private Sub ImportOkayBtn_Click(sender As Object, e As RoutedEventArgs)
        If IsImportEditOpen Then
            Dim t As DefineImport = CType(ImportList.SelectedItem, ListBoxItem).Tag
            Dim datalistindex As Integer = Scripter.ImportFiles.IndexOf(t)
            Dim listboxindex As Integer = ImportList.SelectedIndex

            Dim titem As DefineImport = Scripter.ImportFiles(datalistindex)


            Dim editname As String = ImportNameTB.Text
            Dim editfolder As String = ImportCombobox.Text


            '편집창열기!


            titem.vname = editname
            titem.vfolder = editfolder

            Dim tlistitem As ListBoxItem = ImportList.Items(listboxindex)
            tlistitem.Content = editname & ":" & editfolder

            ImportList.SelectedIndex = listboxindex
            pjData.SetDirty(True)
        Else
            Dim nVarname As String = ImportNameTB.Text
            Dim editfolder As String = ImportCombobox.Text
            'var,pvar,array,varray


            '편집창열기!
            Dim tlistitems As New ListBoxItem
            Dim nDefineImport As New DefineImport(nVarname, editfolder)

            tlistitems.Content = nVarname & ":" & editfolder
            tlistitems.Tag = nDefineImport

            Scripter.ImportFiles.Add(nDefineImport)
            ImportList.Items.Add(tlistitems)
            pjData.SetDirty(True)
        End If
        Scripter.ImportFileRefresh()
        ImportPanel.Visibility = Visibility.Collapsed
    End Sub

    Private Sub ImportCancelBtn_Click(sender As Object, e As RoutedEventArgs)
        ImportPanel.Visibility = Visibility.Collapsed
    End Sub

    Private Sub ImportCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        ImportOkayBtnRefresh()
    End Sub

    Private Sub ImportNameTB_TextChanged(sender As Object, e As TextChangedEventArgs)
        ImportOkayBtnRefresh()
    End Sub

    Private Sub ImportOkayBtnRefresh()
        If ImportCombobox.Text.Trim = "" Then
            ImportOkayBtn.IsEnabled = False
            Return
        End If

        Dim vtext As String = ImportNameTB.Text

        If vtext.Trim = "" Then
            ImportOkayBtn.IsEnabled = False
            Return
        End If


        ImportOkayBtn.IsEnabled = True
    End Sub
End Class
