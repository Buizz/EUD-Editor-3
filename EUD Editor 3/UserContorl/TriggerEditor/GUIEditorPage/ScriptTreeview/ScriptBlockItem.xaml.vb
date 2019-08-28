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


                    Dim btn As New ScriptTreeviewItem(parrentScript, tScript.Argument(valuesindex), True) 'New ScriptBlock("Then"))
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
            Case "Number"
                Dim textbox As New TextBox
                textbox.MinWidth = 40
                textbox.Text = Script.Value


                textbox.Tag = Script

                ContentPanel.Children.Add(textbox)
                AddHandler textbox.TextChanged, AddressOf NumberBox
            Case Else
                Dim label As New Label
                label.Content = Script.TriggerScript.SName

                ContentPanel.Children.Add(label)
        End Select
    End Sub

    Public Sub NumberBox(sender As Object, e As TextChangedEventArgs)
        Dim textbox As TextBox = sender
        Dim script As ScriptBlock = textbox.Tag


        script.Value = textbox.Text
    End Sub

End Class
