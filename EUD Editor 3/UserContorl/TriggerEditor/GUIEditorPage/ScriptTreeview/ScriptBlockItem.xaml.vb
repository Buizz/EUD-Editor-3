Public Class ScriptBlockItem
    Private parrentScript As GUIScriptEditorUI
    Private Script As ScriptBlock
    Public Sub Init(tparrent As GUIScriptEditorUI, tScript As ScriptBlock)
        Script = tScript
        parrentScript = tparrent
        'TestLabel.Content = Script.TriggerScript.SName

        Dim ts As TriggerScript = tScript.TriggerScript

        Dim texts As String = Script.TriggerScript.GetTexts
        If texts = "" Then
            Dim label As New Label
            label.Content = Script.TriggerScript.SName

            ContentPanel.Children.Add(label)
        Else
            Dim textblocks() As String = texts.Split("$")

            For i = 0 To textblocks.Count - 1
                If ts.ValuesDef.IndexOf(textblocks(i)) = -1 Then
                    Dim label As New Label
                    label.Padding = New Thickness(2)
                    label.VerticalAlignment = VerticalAlignment.Center
                    label.Content = textblocks(i)

                    ContentPanel.Children.Add(label)
                Else
                    Dim btn As New ScriptTreeviewItem(parrentScript, New ScriptBlock("Then"))
                    btn.Margin = New Thickness(2)
                    btn.Padding = New Thickness(2)
                    btn.Height = Double.NaN
                    btn.VerticalAlignment = VerticalAlignment.Center
                    'btn.Content = textblocks(i)


                    '<Button Content = "로케이션1" Padding="0" Margin="2" Height="Auto" VerticalAlignment="Center"/>
                    '<Label Content = "에 생성합니다." Padding="0" VerticalAlignment="Center"/>-->
                    ContentPanel.Children.Add(btn)
                End If


            Next
        End If
    End Sub
End Class
