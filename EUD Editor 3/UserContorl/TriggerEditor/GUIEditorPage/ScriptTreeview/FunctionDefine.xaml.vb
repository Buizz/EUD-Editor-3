Imports MaterialDesignThemes.Wpf

Public Class FunctionDefine
    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        valueList.Children.Clear()


        Dim ttChip As New Chip
        ttChip.Content = Tool.GetText("Label")
        ttChip.Tag = "Label"
        AddHandler ttChip.Click, AddressOf valueAdd
        valueList.Children.Add(ttChip)

        For Each s As String In GUIScriptEditorUI.GetArgumentType

            Dim tChip As New Chip
            tChip.Tag = s
            tChip.Background = New SolidColorBrush(Color.FromRgb(&HFF, &H80, &H40))

            Dim tt As String
            tt = Tool.GetText(s)
            If tt = "" Then
                tChip.Content = s
            Else
                tChip.Content = tt
            End If




            AddHandler tChip.Click, AddressOf valueAdd
            valueList.Children.Add(tChip)
        Next
    End Sub

    Private script As ScriptBlock
    Private sb As ScriptBlockItem


    Private Sub Grprefresh()
        ValuePanel.Children.Clear()

        For i = 0 To script.Argument.Count - 1
            Dim tagname As String = script.ArgumentName(i)




            Dim tChip As New Chip
            tChip.Tag = i
            tChip.IsDeletable = True
            tChip.Height = Double.NaN

            If tagname = "Label" Then
                Dim textbox As New TextBox
                textbox.MinWidth = 40
                textbox.Text = script.Argument(i).Value
                textbox.VerticalAlignment = VerticalAlignment.Bottom

                textbox.Tag = script.Argument(i)

                tChip.Content = textbox
                AddHandler textbox.TextChanged, AddressOf labelchange
            Else
                Dim sp As New StackPanel
                sp.Orientation = Orientation.Horizontal

                Dim textbox As New TextBox
                textbox.MinWidth = 40
                textbox.VerticalAlignment = VerticalAlignment.Center


                textbox.Tag = script.Argument(i)

                sp.Children.Add(textbox)
                AddHandler textbox.TextChanged, AddressOf labelchange


                'textbox.Style = Application.Current.Resources("MaterialDesignFloatingHintTextBox")


                Dim tt As String
                tt = Tool.GetText(tagname)

                Dim tstr As String
                If tt = "" Then
                    tstr = tagname
                Else
                    tstr = tt
                End If
                HintAssist.SetHint(textbox, tstr)
                If script.Argument(i).Value = "" Then
                    textbox.Text = tstr
                Else
                    textbox.Text = script.Argument(i).Value
                End If

                tChip.Background = New SolidColorBrush(Color.FromRgb(&HFF, &H80, &H40))

                tChip.Content = sp
            End If



            AddHandler tChip.DeleteClick, AddressOf valueDelete
            ValuePanel.Children.Add(tChip)
        Next
    End Sub

    Private Sub labelchange(sender As Object, e As TextChangedEventArgs)
        Dim textbox As TextBox = sender

        Dim arg As ScriptBlock = textbox.Tag
        arg.Value = textbox.Text


        sb.BlockGraphic()
    End Sub

    Public Sub init(tscript As ScriptBlock, tsb As ScriptBlockItem)
        script = tscript
        sb = tsb


        Grprefresh()
    End Sub



    Private Sub valueDelete(sender As Object, e As RoutedEventArgs)
        Dim tChip As Chip = sender
        Dim index As Integer = tChip.Tag

        script.Argument.RemoveAt(index)
        script.ArgumentName.RemoveAt(index)



        Grprefresh()
        sb.BlockGraphic()
    End Sub
    Private Sub valueAdd(sender As Object, e As RoutedEventArgs)
        Dim tChip As Chip = sender
        Dim keyname As String = tChip.Tag

        script.Argument.Add(New ScriptBlock(keyname, True))
        script.ArgumentName.Add(keyname)

        'MsgBox(tChip.Tag)

        sb.BlockGraphic()
        Grprefresh()
    End Sub
End Class
