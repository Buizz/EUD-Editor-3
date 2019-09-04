Public Class ScriptBlockItem
    Private parrentScript As GUIScriptEditorUI
    Public Script As ScriptBlock
    Public Sub Init(tparrent As GUIScriptEditorUI, tScript As ScriptBlock, Optional IsValue As Boolean = False)
        Script = tScript
        parrentScript = tparrent
        'TestLabel.Content = Script.TriggerScript.SName

        Dim ts As TriggerScript = tScript.TriggerScript

        Dim texts As String = Script.TriggerScript.GetTexts
        ContentPanel.Children.Clear()

        If texts = "" Then
            BlockGraphic()
        Else
            Dim textblocks() As String = texts.Split("$")

            Dim valuesindex As Integer = 0
            For i = 0 To textblocks.Count - 1
                If ts.ValuesDef.IndexOf(textblocks(i)) = -1 Then
                    Dim label As New Label
                    label.Padding = New Thickness(2)
                    label.VerticalAlignment = VerticalAlignment.Center
                    label.Content = textblocks(i)

                    ContentPanel.Children.Add(label)
                Else
                    'textblocks(i)



                    Dim btn As New ScriptTreeviewItem(parrentScript, tScript.Argument(ts.ValuesDef.IndexOf(textblocks(i))), True) 'New ScriptBlock("Then"))
                    btn.Margin = New Thickness(10)
                    'btn.Padding = New Thickness(10)
                    btn.Height = Double.NaN
                    btn.VerticalAlignment = VerticalAlignment.Center
                    'btn.Content = textblocks(i)


                    '<Button Content = "로케이션1" Padding="0" Margin="2" Height="Auto" VerticalAlignment="Center"/>
                    '<Label Content = "에 생성합니다." Padding="0" VerticalAlignment="Center"/>-->
                    ContentPanel.Children.Add(btn)

                    valuesindex += 1
                End If



            Next
        End If

        If IsValue Then
            Dim keyname As String = Script.TriggerScript.Group
            Dim header As String = Tool.GetText(keyname)

            ColorZone.Background = New SolidColorBrush(TriggerScript.GetColor(keyname, 96))
        End If
    End Sub

    Private Sub BlockGraphic()
        Dim tlable As String = Script.TriggerScript.SName
        Select Case tlable
            Case "Unit", "Location"
                Dim ctrl As New Button()
                ctrl.MinWidth = 40
                ctrl.Padding = New Thickness(0)
                ctrl.Margin = New Thickness(4, 0, 4, 0)
                ctrl.Style = Application.Current.Resources("MaterialDesignFlatButton")
                ctrl.Foreground = Application.Current.Resources("MaterialDesignBody")
                ctrl.Height -= 6

                'textbox.Text = Script.Value

                Select Case tlable
                    Case "Unit"
                        Script.ValueType = SCDatFiles.DatFiles.units
                    Case "Location"
                        Script.ValueType = SCDatFiles.DatFiles.Location
                End Select

                ctrl.Content = pjData.CodeLabel(Script.ValueType, Script.Value)


                ctrl.Tag = Script

                ContentPanel.Children.Add(ctrl)
                AddHandler ctrl.Click, AddressOf CodeSelecterButton
            Case "Player"
                Dim combobox As New ComboBox
                combobox.Margin = New Thickness(4, 0, 4, 0)

                combobox.MinWidth = 40


                Dim players() As String = {"Player 1", "Player 2", "Player 3", "Player 4", "Player 5", "Player 6", "Player 7", "Player 8", "Player 9", "Player 10",
"Player 11", "Player 12", "Unknown", "CurrentPlayer", "Foes", "Allies", "NeutralPlayers", "AllPlayers", "Force 1", "Force 2", "Force 3", "Force 4",
"Unknown", "Unknown", "Unknown", "Unknown", "NonAlliedVictoryPlayers"}

                For i = 0 To players.Count - 1
                    Dim cbitem As New ComboBoxItem
                    cbitem.Content = players(i)

                    cbitem.Background = Application.Current.Resources("MaterialDesignPaper")
                    combobox.Items.Add(cbitem)
                Next
                combobox.SelectedIndex = Script.Value



                combobox.Tag = Script

                ContentPanel.Children.Add(combobox)
                AddHandler combobox.SelectionChanged, AddressOf ComboboxSelecter

            Case "Number"
                Dim textbox As New TextBox
                textbox.MinWidth = 40
                textbox.Text = Script.Value


                textbox.Tag = Script

                ContentPanel.Children.Add(textbox)
                AddHandler textbox.TextChanged, AddressOf NumberBox
            Case "RawCode"
                Dim textbox As New CodeEditor
                textbox.MinWidth = 40
                textbox.Text = Script.Value
                textbox.Margin = New Thickness(4, 4, 4, 4)


                textbox.Tag = Script

                ContentPanel.Children.Add(textbox)
                AddHandler textbox.TextChange, AddressOf CodeEditorBox
            Case Else
                Dim label As New Label
                label.Content = Script.TriggerScript.SName

                ContentPanel.Children.Add(label)
        End Select
    End Sub



    Public Sub CodeSelecterButton(sender As Object, e As EventArgs)
        Dim button As Button = sender
        Dim script As ScriptBlock = button.Tag


        Dim relativePoint As Point = TransformToAncestor(parrentScript).Transform(New Point(0, 0))
        parrentScript.CodeSelect.Margin = New Thickness(relativePoint.X, relativePoint.Y + Me.ActualHeight, 0, 0)
        parrentScript.OpenCodeSelecter(script, button)
    End Sub
    Public Sub ComboboxSelecter(sender As Object, e As SelectionChangedEventArgs)
        Dim combobox As ComboBox = sender
        Dim script As ScriptBlock = combobox.Tag


        script.Value = combobox.SelectedIndex
    End Sub
    Public Sub NumberBox(sender As Object, e As TextChangedEventArgs)
        Dim textbox As TextBox = sender
        Dim script As ScriptBlock = textbox.Tag


        script.Value = textbox.Text
    End Sub

    Public Sub CodeEditorBox(sender As Object, e As RoutedEventArgs)
        Dim textbox As CodeEditor = sender
        Dim script As ScriptBlock = textbox.Tag


        script.Value = textbox.Text
    End Sub

End Class
