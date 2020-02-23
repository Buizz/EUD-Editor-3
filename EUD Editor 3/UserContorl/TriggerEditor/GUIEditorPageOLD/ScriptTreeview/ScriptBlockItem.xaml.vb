Imports MaterialDesignThemes.Wpf

Public Class ScriptBlockItem
    Public parrentScript As GUIScriptEditorUI
    Public Script As ScriptBlock
    Public Sub Init(tparrent As GUIScriptEditorUI, tScript As ScriptBlock, Optional IsValue As Boolean = False)
        Script = tScript
        parrentScript = tparrent
        'TestLabel.Content = Script.TriggerScript.SName

        Dim ts As TriggerScript = Script.TriggerScript

        Dim texts As String = Script.TriggerScript.GetTexts
        ContentPanel.Children.Clear()

        If texts = "" Then

            Dim tlable As String = Script.TriggerScript.SName
            If tlable = "FuncUse" Then
                FuncRefresh()
            Else
                BlockGraphic()
            End If


        Else
            ArgumentGraphic(ts, tScript, texts)
        End If

        If IsValue Then
            Dim keyname As String = Script.TriggerScript.Group
            Dim header As String = Tool.GetText(keyname)

            ColorZone.Background = New SolidColorBrush(TriggerScript.GetColor(keyname, 96))
        End If
    End Sub


    Private Sub FuncRefresh()
        ContentPanel.Children.Clear()

        Dim funcname As String = Script.Value

        Dim fsb As ScriptBlock = parrentScript.GetFunc(funcname)
        If fsb Is Nothing Then
            Dim label As New Label
            label.Content = "존재하지 않는 스크립트"

            ContentPanel.Children.Add(label)
        Else
            Dim label As New Label
            label.Content = Script.Value

            ContentPanel.Children.Add(label)

            If fsb.Argument.Count > 0 Then
                Dim arglist As List(Of GUIScriptTool.Arg) = GUIScriptTool.GetInfoArgList(fsb.Argument.First.Value)


                Dim vindex As Integer = 0
                For i = 0 To arglist.Count - 1
                    If arglist(i).type = "Label" Then
                        Dim tlabel As New Label
                        tlabel.Padding = New Thickness(2)
                        tlabel.VerticalAlignment = VerticalAlignment.Center
                        tlabel.Content = arglist(i).tag

                        ContentPanel.Children.Add(tlabel)
                    Else
                        If Script.Argument.Count <= vindex Then
                            Script.ArgumentName.Add(arglist(i).type)
                            Script.Argument.Add(New ScriptBlock(arglist(i).type, True))
                        End If

                        Dim btn As New ScriptTreeviewItem(parrentScript, Script.Argument(vindex), True) 'New ScriptBlock("Then"))
                        btn.Margin = New Thickness(10)
                        btn.Height = Double.NaN
                        btn.VerticalAlignment = VerticalAlignment.Center
                        ContentPanel.Children.Add(btn)
                        vindex += 1
                    End If


                Next
            End If




            'Dim vindex As Integer = 0
            'For i = 0 To fsb.ArgumentName.Count - 1
            '    If fsb.ArgumentName(i) = "Label" Then
            '        Dim tlabel As New Label
            '        tlabel.Padding = New Thickness(2)
            '        tlabel.VerticalAlignment = VerticalAlignment.Center
            '        tlabel.Content = fsb.Argument(i).Value

            '        ContentPanel.Children.Add(tlabel)
            '    Else
            '        If Script.Argument.Count <= vindex Then
            '            Script.ArgumentName.Add(fsb.ArgumentName(i))
            '            Script.Argument.Add(New ScriptBlock(fsb.ArgumentName(i), True))
            '        End If



            '        Dim btn As New ScriptTreeviewItem(parrentScript, Script.Argument(vindex), True) 'New ScriptBlock("Then"))
            '        btn.Margin = New Thickness(10)
            '        btn.Height = Double.NaN
            '        btn.VerticalAlignment = VerticalAlignment.Center
            '        ContentPanel.Children.Add(btn)

            '        vindex += 1
            '    End If
            'Next
        End If
    End Sub



    Private Sub ArgumentGraphic(ts As TriggerScript, tScript As ScriptBlock, argText As String)
        Dim textblocks() As String = argText.Split("$")

        Dim valuesindex As Integer = 0
        For i = 0 To textblocks.Count - 1
            If ts.ValuesDef.IndexOf(textblocks(i)) = -1 Then
                Dim label As New Label
                label.Padding = New Thickness(2)
                label.VerticalAlignment = VerticalAlignment.Center
                label.Content = textblocks(i)

                ContentPanel.Children.Add(label)
            Else
                Dim btn As New ScriptTreeviewItem(parrentScript, tScript.Argument(ts.ValuesDef.IndexOf(textblocks(i))), True) 'New ScriptBlock("Then"))
                btn.Margin = New Thickness(10)
                btn.Height = Double.NaN
                btn.VerticalAlignment = VerticalAlignment.Center
                ContentPanel.Children.Add(btn)

                valuesindex += 1
            End If
        Next
    End Sub


    Public Sub BlockGraphic()
        ContentPanel.Children.Clear()

        Dim tlable As String = Script.TriggerScript.SName
        Select Case tlable
            Case "Var"
                Dim ctrl As New Button()
                ctrl.MinWidth = 40
                ctrl.Padding = New Thickness(0)
                ctrl.Margin = New Thickness(4, 0, 4, 0)
                ctrl.Style = Application.Current.Resources("MaterialDesignFlatButton")
                ctrl.Foreground = Application.Current.Resources("MaterialDesignBody")
                ctrl.Height -= 6

                'textbox.Text = Script.Value
                If Script.Value = "" Then
                    ctrl.Content = Tool.GetText("Var")
                Else
                    ctrl.Content = Script.Value
                End If

                ctrl.Tag = Script

                ContentPanel.Children.Add(ctrl)
                AddHandler ctrl.Click, AddressOf VarSelectButton
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
            Case "RawCode", "Code"
                Dim textbox As New CodeEditor
                textbox.MinWidth = 40
                textbox.Text = Script.Value
                textbox.Margin = New Thickness(4, 4, 4, 4)


                textbox.Tag = Script

                ContentPanel.Children.Add(textbox)
                AddHandler textbox.TextChange, AddressOf CodeEditorBox
            Case "FunctionDefinition"
                Dim label As New Label
                label.VerticalAlignment = VerticalAlignment.Center
                label.Content = "함수 명 : "

                ContentPanel.Children.Add(label)


                Dim textbox As New TextBox
                Dim binding As New Binding()

                textbox.DataContext = New FunctionNameBinding(Me)

                binding.ValidationRules.Add(New FunctionValidationRule(Me))
                binding.ValidatesOnDataErrors = True
                binding.NotifyOnValidationError = True
                binding.Path = New PropertyPath("FunctionName")
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                binding.Mode = BindingMode.TwoWay
                textbox.SetBinding(TextBox.TextProperty, binding)


                textbox.MinWidth = 80

                textbox.Text = Script.Value
                textbox.VerticalAlignment = VerticalAlignment.Center

                textbox.Tag = Script


                'AddHandler textbox.LostKeyboardFocus, AddressOf FunctionNameRefresh
                ContentPanel.Children.Add(textbox)



                Dim tlabel As New Label
                tlabel.VerticalAlignment = VerticalAlignment.Center

                Dim argstr As String = ""


                If Script.Argument.Count > 0 Then
                    Dim arglist As List(Of GUIScriptTool.Arg) = GUIScriptTool.GetArgList(Script.Argument.First.Value)


                    For i = 0 To arglist.Count - 1
                        If arglist(i).tag = "" Then
                            argstr = argstr & arglist(i).type & ":" & arglist(i).type
                        Else
                            argstr = argstr & arglist(i).tag & ":" & arglist(i).type
                        End If



                        '    If Not Script.ArgumentName(i) = "Label" Then
                        '        If argstr <> "" Then
                        '            argstr = argstr & ","
                        '        End If
                        '        argstr = argstr & Script.Argument(i).Value & ":" & Script.ArgumentName(i)
                        '    End If
                    Next
                End If




                tlabel.Content = argstr

                ContentPanel.Children.Add(tlabel)




                Dim ctrl As New Button()
                ctrl.Foreground = Application.Current.Resources("MaterialDesignBody")

                Dim picon As New PackIcon
                picon.Kind = PackIconKind.Edit

                ctrl.Content = picon
                ctrl.Tag = Script
                ctrl.Style = Application.Current.Resources("MaterialDesignToolButton")

                ContentPanel.Children.Add(ctrl)
                AddHandler ctrl.Click, AddressOf FunctionEditButton
            Case Else
                Dim label As New Label
                label.Content = Script.TriggerScript.SName

                ContentPanel.Children.Add(label)
        End Select
    End Sub


    Public Sub VarSelectButton(sender As Object, e As EventArgs)
        Dim button As Button = sender
        Dim script As ScriptBlock = button.Tag


        Dim relativePoint As Point = TransformToAncestor(parrentScript).Transform(New Point(0, 0))
        parrentScript.VFuncEdit.Margin = New Thickness(relativePoint.X, relativePoint.Y + Me.ActualHeight, 0, 0)
        parrentScript.VFuncEdit.Width = Me.ActualWidth


        parrentScript.OpenVarSelecter(script, button, Me)
    End Sub

    Public Sub FunctionEditButton(sender As Object, e As EventArgs)
        Dim button As Button = sender
        Dim script As ScriptBlock = button.Tag


        Dim relativePoint As Point = TransformToAncestor(parrentScript).Transform(New Point(0, 0))
        parrentScript.BFuncEdit.Margin = New Thickness(relativePoint.X, relativePoint.Y + Me.ActualHeight, 0, 0)
        parrentScript.BFuncEdit.Width = Me.ActualWidth


        parrentScript.OpenFunctionEdit(script, button, Me)
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
