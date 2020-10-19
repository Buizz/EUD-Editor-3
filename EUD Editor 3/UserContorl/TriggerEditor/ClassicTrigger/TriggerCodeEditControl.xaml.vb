Imports System.ComponentModel
Imports System.Windows.Threading

Public Class TriggerCodeEditControl
    Public Event OkayBtnEvent As RoutedEventHandler
    Public Event CancelBtnEvent As RoutedEventHandler

    Public Enum OpenType
        Action
        Contidion
        Func
    End Enum

    Public SelectTBlock As TriggerCodeBlock

    Public Sub CloseEdit()
        Me.Visibility = Visibility.Collapsed
    End Sub

    Private tLoc As String
    Public Sub OpenEdit(OType As OpenType, Optional Loc As String = "", Optional TBlock As TriggerCodeBlock = Nothing)
        Me.Visibility = Visibility.Visible



        If Loc = "" Then
            PosNode.Visibility = Visibility.Collapsed
        Else
            PosNode.Visibility = Visibility.Visible
            PosNode.Text = Loc
            tLoc = Loc
        End If


        If TBlock Is Nothing Then
            FuncSelecter.Visibility = Visibility.Visible
            SelectBtn.IsEnabled = False
            SelectTBlock = New TriggerCodeBlock
            SelectName.Text = ""
            Okay_Btn.IsEnabled = False
        Else
            Okay_Btn.IsEnabled = True
            FuncSelecter.Visibility = Visibility.Collapsed
            SelectTBlock = TBlock
            SelectName.Text = TBlock.GetCodeFunction.FName
            FuncRefresh()
        End If

        CodeTypeList.SelectedIndex = -1
        If OType = OpenType.Contidion Then
            CListItem.Visibility = Visibility.Visible
            AListItem.Visibility = Visibility.Collapsed
            CodeTypeList.SelectedIndex = 0
        ElseIf OType = OpenType.Action Then
            CListItem.Visibility = Visibility.Collapsed
            AListItem.Visibility = Visibility.Visible
            CodeTypeList.SelectedIndex = 1
        ElseIf OType = OpenType.Func Then
            CListItem.Visibility = Visibility.Collapsed
            AListItem.Visibility = Visibility.Collapsed
            CodeTypeList.SelectedIndex = 2
        End If


        SearchTextBox.Text = ""
        Dispatcher.BeginInvoke(DispatcherPriority.Input,
                               New Action(Sub()
                                              SearchTextBox.Focus()
                                          End Sub))
        BtnRefresh()





    End Sub


    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent OkayBtnEvent(SelectTBlock, e)
        CloseEdit()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        RaiseEvent CancelBtnEvent(sender, e)
        CloseEdit()
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        If FuncSelecter.Visibility = Visibility.Collapsed Then
            FuncSelecter.Visibility = Visibility.Visible
        Else
            FuncSelecter.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        '리스트 선택 버튼
        FunctionSelectBtnClick()
        BtnRefresh()
    End Sub

    Private bg As BackgroundWorker
    Private Sub ListRefresh()
        CodeList.Items.Clear()

        For Each titem As ListBoxItem In CodeTypeList.Items
            'Background="{DynamicResource MaterialDesignPaper}" Foreground="{DynamicResource MaterialDesignBody}"
            titem.Background = Application.Current.Resources("PrimaryHueLightBrush")
            titem.Foreground = Application.Current.Resources("PrimaryHueLightForegroundBrush")
        Next

        If CodeTypeList.SelectedIndex = -1 Then
            Return
        End If


        Dim listitem As ListBoxItem = CodeTypeList.SelectedItem
        listitem.Background = Application.Current.Resources("MaterialDesignPaper")
        listitem.Foreground = Application.Current.Resources("MaterialDesignBody")


        Dim typeS As String = listitem.Tag

        Dim SelType As TriggerFunction.EFType
        Select Case typeS
            Case "C"
                SelType = TriggerFunction.EFType.Condition
            Case "A"
                SelType = TriggerFunction.EFType.Action
            Case "P"
                SelType = TriggerFunction.EFType.Plib
            Case "L"
                SelType = TriggerFunction.EFType.Lua
            Case "F"
                SelType = TriggerFunction.EFType.UserFunc
            Case "E"
                SelType = TriggerFunction.EFType.ExternFunc
        End Select



        Dim TList As List(Of TriggerFunction) = tmanager.GetTriggerList(SelType)


        bg = New BackgroundWorker
        AddHandler bg.DoWork, Sub(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
                                  For i = 0 To TList.Count - 1
                                      Dim index As Integer = i
                                      Dispatcher.BeginInvoke(DispatcherPriority.Input,
                                   New Action(Sub()
                                                  Dim itemColl As ItemCollection
                                                  If TList(index).FGruop <> "" Then
                                                      '그룹이 있을 경우
                                                      itemColl = GetCodeListGroup(TList(index).FGruop).Items
                                                  Else
                                                      itemColl = CodeList.Items
                                                  End If


                                                  Dim tnode As New TreeViewItem
                                                  tnode.Style = Application.Current.Resources("ShortTreeViewItem")
                                                  tnode.Background = Application.Current.Resources("MaterialDesignPaper")
                                                  tnode.Foreground = Application.Current.Resources("MaterialDesignBody")

                                                  tnode.Header = TList(index).FName
                                                  tnode.Tag = TList(index)

                                                  itemColl.Add(tnode)



                                                  'SelectTBlock이랑 비교해야됨
                                                  If SelectTBlock IsNot Nothing Then
                                                      Dim t As TriggerFunction = SelectTBlock.GetCodeFunction
                                                      If t IsNot Nothing Then
                                                          If t.FName = TList(index).FName Then
                                                              tnode.IsSelected = True
                                                          End If
                                                      End If
                                                  End If
                                              End Sub))
                                  Next
                              End Sub

        bg.RunWorkerAsync()
    End Sub

    Private Sub FliterApply(Optional pitem As TreeViewItem = Nothing)
        Dim itemColl As ItemCollection


        If pitem Is Nothing Then
            itemColl = CodeList.Items
        Else
            itemColl = pitem.Items
        End If



        If FliterText <> "" Then
            Dim collapsedCount As Integer = 0
            Dim VisibleCount As Integer = 0

            For i = 0 To itemColl.Count - 1
                Dim titem As TreeViewItem = itemColl(i)
                Dim fName As String
                If titem.Tag Is Nothing Then
                    'fName = titem.Header
                    FliterApply(titem)
                    Continue For
                Else
                    fName = CType(titem.Tag, TriggerFunction).FName.ToUpper
                End If


                If fName.IndexOf(FliterText.ToUpper) = -1 Then
                    titem.Visibility = Visibility.Collapsed
                    collapsedCount += 1
                Else
                    titem.Visibility = Visibility.Visible
                    VisibleCount += 1
                End If

                FliterApply(titem)
            Next

            If pitem IsNot Nothing Then
                If collapsedCount <> 0 And collapsedCount = itemColl.Count Then
                    pitem.Visibility = Visibility.Collapsed
                End If
                If VisibleCount < 10 Then
                    pitem.IsExpanded = True
                Else
                    pitem.IsExpanded = False
                End If
            End If

        Else
            For i = 0 To itemColl.Count - 1
                Dim titem As TreeViewItem = itemColl(i)
                titem.IsExpanded = False
                titem.Visibility = Visibility.Visible
                FliterApply(titem)
            Next
        End If
    End Sub



    Private Sub CodeTypeList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        ListRefresh()
    End Sub


    Private Function GetCodeListGroup(GroupName As String) As TreeViewItem
        For i = 0 To CodeList.Items.Count - 1
            If CType(CodeList.Items(i), TreeViewItem).Header = GroupName Then
                '같은 그룹이면
                Return CodeList.Items(i)
            End If
        Next

        Dim tnode As New TreeViewItem
        tnode.Background = Application.Current.Resources("MaterialDesignPaper")
        tnode.Foreground = Application.Current.Resources("MaterialDesignBody")

        tnode.Header = GroupName
        CodeList.Items.Add(tnode)

        Return tnode
    End Function



    Private Sub CodeList_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object))
        TriggerSummary.Text = ""
        SelectName.Text = ""
        If CodeList.SelectedItem IsNot Nothing Then
            If CType(CodeList.SelectedItem, TreeViewItem).Items.Count = 0 Then
                SelectBtn.IsEnabled = True

                Dim SelectTrigger As TriggerFunction = CType(CodeList.SelectedItem, TreeViewItem).Tag

                Dim SummaryText As String = ""

                For i = 0 To SelectTrigger.Args.Count - 1
                    If i <> 0 Then
                        SummaryText = SummaryText & ", "
                    End If
                    SummaryText = SummaryText & SelectTrigger.Args(i).AName
                Next

                If SummaryText <> "" Then
                    SummaryText = SummaryText & vbCrLf
                End If

                SummaryText = SummaryText & SelectTrigger.FSummary



                'SelectTrigger.FSummary
                'SelectTrigger.FGruop
                'SelectTrigger.Args.Count
                TriggerSummary.Text = SummaryText

                SelectName.Text = SelectTrigger.FName
            Else
                SelectBtn.IsEnabled = False
            End If
        Else
            SelectBtn.IsEnabled = False
        End If
    End Sub

    Private Sub Button_Click_4(sender As Object, e As RoutedEventArgs)
        SearchTextBox.Text = ""
    End Sub

    Private FliterText As String
    Private Sub SearchTextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        FliterText = SearchTextBox.Text
        FliterApply()
        'ListRefresh()
    End Sub

    Private Sub CodeList_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Then
            FunctionSelectBtnClick()
            Return
        End If


        If CodeList.Items.Count <> 0 Then
            If CType(CodeList.Items(0), TreeViewItem).IsSelected = True Then
                CType(CodeList.Items(0), TreeViewItem).IsSelected = False
                Dispatcher.BeginInvoke(Windows.Threading.DispatcherPriority.Input, New Action(Sub()
                                                                                                  SearchTextBox.Focus()
                                                                                              End Sub))
            End If
        End If
    End Sub

    Private Sub SearchTextBox_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Down Then
            CodeList.Focus()
            'If CodeList.Items.Count <> 0 Then
            '    CType(CodeList.Items(0), TreeViewItem).IsSelected = True

            'End If
        End If

        If e.Key = Key.Enter Then
            FunctionSelectBtnClick()
        End If
    End Sub


    Private Sub FunctionSelectBtnClick()
        FuncSelecter.Visibility = Visibility.Collapsed

        SelectTBlock.SetFunction(CType(CodeList.SelectedItem, TreeViewItem).Tag)
        SelectTBlock.Refresh()
        '코드를 선택
        FuncRefresh()
        BtnRefresh()
    End Sub





    Private Sub BtnRefresh()
        If SelectTBlock Is Nothing Then
            Okay_Btn.IsEnabled = False
            Return
        End If

        '버튼을 리프레시한다.
        For i = 0 To SelectTBlock.Args.Count - 1
            If SelectTBlock.Args(i).IsInit Then
                Okay_Btn.IsEnabled = False
                Return
            End If
        Next

        Okay_Btn.IsEnabled = True

        '인자들의 설정여부를 확인한다.
    End Sub

    Private Sub CodeList_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        If CodeList.SelectedItem IsNot Nothing Then
            If CType(CodeList.SelectedItem, TreeViewItem).Items.Count = 0 Then
                FunctionSelectBtnClick()
            End If
        End If
    End Sub
End Class
