Public Class TriggerEditControl
    Public Event OkayBtnEvent As RoutedEventHandler
    Public Event CancelBtnEvent As RoutedEventHandler

    Private ptrg As Trigger
    Private scripter As ScriptEditor

    Private IsLoad As Boolean = False
    Public Sub New(_scripter As ScriptEditor, trg As Trigger)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        scripter = _scripter
        ptrg = trg
        CommentTB.Text = ptrg.CommentStringProperty

        For Each cb As CheckBox In PlayerTab.Children
            Dim cbtag As Integer = cb.Tag
            cb.IsChecked = ptrg.PlayerEnabled(cbtag)
        Next

        AddHandler TriggerCodeEdit.OkayBtnEvent, AddressOf TriggerCodeEditOkayEvent

        cList.Items.Clear()

        For i = 0 To ptrg.Condition.Count - 1
            Dim listitem As New ListBoxItem
            listitem.Content = New ListItemCodeBlock(scripter, ptrg.Condition(i))

            cList.Items.Add(listitem)
        Next
        aList.Items.Clear()

        For i = 0 To ptrg.Actions.Count - 1
            Dim listitem As New ListBoxItem
            listitem.Content = New ListItemCodeBlock(scripter, ptrg.Actions(i))

            aList.Items.Add(listitem)
        Next
        IsTriggerEnabled.IsChecked = ptrg.IsEnabled

        IsPreservedCB.IsChecked = ptrg.IsPreserved

        IsCodeOnly.IsChecked = ptrg.IsOnlyCode
        CodeText.Text = ptrg.CodeText


        IsLoad = True
    End Sub
    Public Sub TriggerCodeEditOkayEvent(sender As TriggerCodeBlock, e As RoutedEventArgs)
        'Okay가 옴
        Dim tlist As ListBox
        Dim ttriglist As List(Of TriggerCodeBlock)
        If GetPageIndex = 1 Then
            tlist = cList
            ttriglist = ptrg.Condition
        Else
            tlist = aList
            ttriglist = ptrg.Actions
        End If


        If IsEditOpen Then
            '편집으로 열었음
            Dim tCodeBlock As TriggerCodeBlock = sender


            tCodeBlock.CopyTo(ttriglist(LastSelectListBoxIndex))
            CType(CType(tlist.Items(LastSelectListBoxIndex), ListBoxItem).Content, ListItemCodeBlock).RefreshItem()
        Else
            '새창으로 열었음
            Dim tCodeBlock As TriggerCodeBlock = sender


            Dim listitem As New ListBoxItem
            listitem.Content = New ListItemCodeBlock(scripter, tCodeBlock)

            ttriglist.Insert(LastSelectListBoxIndex, tCodeBlock)
            tlist.Items.Insert(LastSelectListBoxIndex, listitem)
        End If
        ButtonRefresh()
    End Sub


    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        ptrg.CommentStringProperty = CommentTB.Text


        RaiseEvent OkayBtnEvent(sender, e)
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        RaiseEvent CancelBtnEvent(sender, e)
    End Sub

    Private ReadOnly Property GetPageIndex As Integer
        Get
            Return MainTab.SelectedIndex
        End Get
    End Property

    Private LastSelectListBoxIndex As Integer
    Private IsEditOpen As Boolean




    Private Sub List_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        ButtonRefresh()
    End Sub

    Private Sub ButtonRefresh()
        Dim newbtn As Button
        Dim editbtn As Button
        Dim cutbtn As Button
        Dim copybtn As Button
        Dim pastebtn As Button
        Dim deletebtn As Button
        Dim upbtn As Button
        Dim downbtn As Button

        Dim tlist As ListBox
        If GetPageIndex = 1 Then
            newbtn = cNew
            editbtn = cEdit
            cutbtn = cCut
            copybtn = cCopy
            pastebtn = cPaste
            deletebtn = cDelete
            upbtn = cUp
            downbtn = cDown

            tlist = cList
        Else
            newbtn = aNew
            editbtn = aEdit
            cutbtn = aCut
            copybtn = aCopy
            pastebtn = aPaste
            deletebtn = aDelete
            upbtn = aUp
            downbtn = aDown

            tlist = aList
        End If

        If tlist.SelectedIndex = -1 Then
            editbtn.IsEnabled = False
            cutbtn.IsEnabled = False
            copybtn.IsEnabled = False
            deletebtn.IsEnabled = False
            upbtn.IsEnabled = False
            downbtn.IsEnabled = False
        Else
            editbtn.IsEnabled = True
            cutbtn.IsEnabled = True
            copybtn.IsEnabled = True
            deletebtn.IsEnabled = True

            '맨 위인지 확인
            upbtn.IsEnabled = True
            downbtn.IsEnabled = True




            Dim upAble As Boolean = True
            Dim downAble As Boolean = True
            For i = 0 To tlist.SelectedItems.Count - 1
                If tlist.Items.IndexOf(tlist.SelectedItems(i)) = 0 Then
                    upAble = False
                End If
            Next
            upbtn.IsEnabled = upAble


            For i = 0 To tlist.SelectedItems.Count - 1
                If tlist.Items.IndexOf(tlist.SelectedItems(i)) = tlist.Items.Count - 1 Then
                    downAble = False
                End If
            Next
            downbtn.IsEnabled = downAble
        End If


        Dim tcheck As Boolean = False
        For k = 0 To 7
            If ptrg.PlayerEnabled(k) Then
                tcheck = True
            End If
        Next
        OkayBtn.IsEnabled = tcheck



        pastebtn.IsEnabled = IsPasteAble()
    End Sub


    Private Sub PlayerCheck(sender As Object, e As RoutedEventArgs)
        Dim cb As CheckBox = sender

        Dim pindex As Integer = cb.Tag

        ptrg.PlayerEnabled(pindex) = cb.IsChecked



        Dim tcheck As Boolean = False
        For k = 0 To 7
            If ptrg.PlayerEnabled(k) Then
                tcheck = True
            End If
        Next
        OkayBtn.IsEnabled = tcheck
    End Sub

    Private Sub IsTriggerEnabled_Checked(sender As Object, e As RoutedEventArgs)
        ptrg.IsEnabled = True
    End Sub

    Private Sub IsTriggerEnabled_Unchecked(sender As Object, e As RoutedEventArgs)
        ptrg.IsEnabled = False
    End Sub

    Private Sub UserControl_MouseEnter(sender As Object, e As MouseEventArgs)
        cPaste.IsEnabled = IsPasteAble()
        aPaste.IsEnabled = IsPasteAble()
    End Sub

    Private Sub ListDouble_Click(sender As Object, e As MouseButtonEventArgs)
        EditFunc()
    End Sub

    Private Sub IsPreservedCB_Checked(sender As Object, e As RoutedEventArgs)
        If IsLoad Then
            ptrg.IsPreserved = IsPreservedCB.IsChecked
        End If
    End Sub

    Private Sub IsCodeOnly_Checked(sender As Object, e As RoutedEventArgs)
        If IsLoad Then
            ptrg.IsOnlyCode = IsCodeOnly.IsChecked
        End If
    End Sub

    Private Sub CodeText_TextChange(sender As Object, e As RoutedEventArgs)
        If IsLoad Then
            ptrg.CodeText = CodeText.Text
        End If
    End Sub
End Class
