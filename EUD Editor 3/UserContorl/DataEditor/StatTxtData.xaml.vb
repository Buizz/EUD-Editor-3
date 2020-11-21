Public Class StatTxtData
    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.stattxt

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        UsedCodeList.Init(DatFiles, ObjectID + 1)

        NameBar.Init(ObjectID, DatFiles, 0)

        MainTextBox.DataContext = pjData.BindingManager.StatTxtBinding(ObjectID)

        Dim str As String() = Tool.GetText("ColorNames").Split("|")

        For i = 0 To str.Count - 1
            If str(i) <> "NULL" Then
                AddPalett(i + 1, str(i))
            End If
        Next

        For i = 0 To SCConst.ASCIICount
            If scData.ASCIICode(i) <> "NUL" Then
                Dim tcomboboxitem As New ComboBoxItem
                tcomboboxitem.Content = StringTool.ChangeSlash(scData.ASCIICode(i))
                tcomboboxitem.Tag = i


                ShortCombobox.Items.Add(tcomboboxitem)
            End If
        Next

    End Sub
    Private Sub AddPalett(ColorCode As Integer, tip As String)

        Dim ColoreTextbox As New ColoredTextBlock("<" & Hex(ColorCode) & "> 〈" & Hex(ColorCode).PadLeft(2, "0") & "〉" & tip)
        ColoreTextbox.HorizontalAlignment = HorizontalAlignment.Left
        ColoreTextbox.Margin = New Thickness(-8)


        Dim ListboxItems As New ListBoxItem
        ListboxItems.Tag = ColorCode
        ListboxItems.Background = Brushes.Black
        ListboxItems.Content = ColoreTextbox
        ListboxItems.Height = 16

        'Dim btn As New Button
        'btn.Content = ColoreTextbox
        'btn.Style = Application.Current.Resources("MaterialDesignFlatButton")
        'btn.Background = Brushes.Black
        'btn.Margin = New Thickness(0, -4, 0, -4)

        'ColorBtn.Children.Add(btn)
        ColorInfor.Items.Add(ListboxItems)
    End Sub
    Private Sub ColorInfor_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If ColorInfor.SelectedItem IsNot Nothing Then
            Dim ColorCode As Integer = ColorInfor.SelectedItem.Tag

            Dim InsertString As String = "<" & Hex(ColorCode) & ">"

            MainTextBox.SelectedText = InsertString
            MainTextBox.SelectionStart += InsertString.Length
            MainTextBox.SelectionLength = 0
            MainTextBox.Focus()
            ColorInfor.SelectedIndex = -1
        End If

    End Sub


    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, tObjectID As Integer)
        ObjectID = tObjectID

        UsedCodeList.ReLoad(DatFiles, ObjectID + 1)

        NameBar.ReLoad(ObjectID, DatFiles, 0)


        MainTextBox.DataContext = pjData.BindingManager.StatTxtBinding(ObjectID)
    End Sub

    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        If MainTextBox.Text.Count > 1 Then
            LimitShortCut(MainTextBox.Text)
        Else
            LimitCombobx.SelectedIndex = 0
            ShortCombobox.SelectedIndex = 0
        End If






        '컬러박스 정리
        PreviewText.TextColred(MainTextBox.Text)
    End Sub

    Private DonotChangeCombobox As Boolean = True
    Private Sub LimitShortCut(texts As String)
        DonotChangeCombobox = False

        If texts = "" Then
            Return
        End If

        'MsgBox(GetCharAt(0, texts) & "_" & GetCharAt(1, texts))
        Dim FristKey As String = StringTool.GetCharAt(0, texts)
        Dim LastKey As String = StringTool.GetCharAt(1, texts)

        Dim LastKeyIndex As Integer = -1
        If LastKey.Count > 1 Then
            Try
                LastKeyIndex = LastKey
            Catch ex As Exception

            End Try
        End If



        If LastKeyIndex >= 0 And LastKeyIndex <= 5 Then
            Dim KeyExist As Boolean = False
            If FristKey.Count > 1 Then
                Dim KeyValue As Integer
                Try
                    KeyValue = "&H" & FristKey
                    If KeyValue <= 255 Then
                        For i = 0 To ShortCombobox.Items.Count - 1
                            Dim comboboxit As ComboBoxItem = ShortCombobox.Items(i)

                            If KeyValue = comboboxit.Tag Then
                                ShortCombobox.SelectedIndex = i
                                KeyExist = True
                                Exit For
                            End If
                        Next
                    End If
                Catch ex As Exception

                End Try
            End If

            If Not KeyExist Then
                If FristKey.Count = 1 Then
                    Dim tKeyValue As Integer = AscW(FristKey)
                    If tKeyValue <= 255 Then
                        For i = 0 To ShortCombobox.Items.Count - 1
                            Dim comboboxit As ComboBoxItem = ShortCombobox.Items(i)

                            If tKeyValue = comboboxit.Tag Then
                                ShortCombobox.SelectedIndex = i
                                LimitCombobx.SelectedIndex = LastKey + 1
                                KeyExist = True
                                Exit For
                            End If
                        Next
                    End If
                End If


                If Not KeyExist Then
                    LimitCombobx.SelectedIndex = 0
                    ShortCombobox.SelectedIndex = 0
                End If
            Else
                LimitCombobx.SelectedIndex = LastKey + 1
            End If
        Else
            LimitCombobx.SelectedIndex = 0
            ShortCombobox.SelectedIndex = 0
        End If



        DonotChangeCombobox = True
    End Sub




    Private Function ExtratStr(str As String) As String
        Return Mid(str, 2, str.Length - 2)
    End Function


    Private Sub MainTextBox_PreviewKeyDown(sender As Object, e As KeyEventArgs)

    End Sub

    Private Sub MainTextBox_PreviewKeyUp(sender As Object, e As KeyEventArgs)

    End Sub

    Private Sub LimitCombobx_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If DonotChangeCombobox Then
            DonotChangeCombobox = False

            Dim CurrentText As String = pjData.BindingManager.StatTxtBinding(ObjectID).Value



            If ShortCombobox.SelectedIndex = 0 Then '선택이 안되어 있을 경우, 즉 처음일 경우
                CurrentText = "<" & scData.ASCIICode(&H61) & "><" & Hex(LimitCombobx.SelectedIndex - 1).PadLeft(2, "0") & ">" & CurrentText
            Else
                If LimitCombobx.SelectedIndex = 0 Then '둘다 0으로 선택되었을 경우
                    CurrentText = StringTool.ChangeCharAt(1, CurrentText, "")
                    CurrentText = StringTool.ChangeCharAt(0, CurrentText, "")
                    ShortCombobox.SelectedIndex = 0
                Else
                    CurrentText = StringTool.ChangeCharAt(1, CurrentText, "<" & Hex(LimitCombobx.SelectedIndex - 1).PadLeft(2, "0") & ">")
                End If
            End If

            pjData.BindingManager.StatTxtBinding(ObjectID).Value = CurrentText

            DonotChangeCombobox = True
        End If
    End Sub

    Private Sub ShortCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If DonotChangeCombobox Then
            DonotChangeCombobox = False

            Dim CurrentText As String = pjData.BindingManager.StatTxtBinding(ObjectID).Value

            Dim tcomboboxitem As ComboBoxItem = ShortCombobox.SelectedItem
            Dim SelectText As String = scData.ASCIICode(tcomboboxitem.Tag)

            If LimitCombobx.SelectedIndex = 0 Then '선택이 안되어 있을 경우, 즉 처음일 경우
                CurrentText = "<" & SelectText & ">" & "<00>" & CurrentText
            Else
                If ShortCombobox.SelectedIndex = 0 Then '둘다 0으로 선택되었을 경우
                    CurrentText = StringTool.ChangeCharAt(1, CurrentText, "")
                    CurrentText = StringTool.ChangeCharAt(0, CurrentText, "")
                    LimitCombobx.SelectedIndex = 0
                Else
                    CurrentText = StringTool.ChangeCharAt(0, CurrentText, "<" & SelectText & ">")
                End If
            End If

            pjData.BindingManager.StatTxtBinding(ObjectID).Value = CurrentText

            DonotChangeCombobox = True
        End If
    End Sub
End Class
