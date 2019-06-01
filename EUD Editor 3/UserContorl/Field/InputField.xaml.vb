Public Class InputField
    Private DatFile As SCDatFiles.DatFiles
    Private ObjectID As Integer
    Private Parameter As String

    Private DatCommand As DatCommand

    Private SpecialFlag As SFlag

    Public Event ValueChange As RoutedEventHandler

    Public Enum SFlag
        None
        HP
        HPV
        FLAG
    End Enum

    Public Sub ReLoad(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String, Optional _SFlag As SFlag = SFlag.None)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter
        SpecialFlag = _SFlag

        Select Case DatFile
            Case SCDatFiles.DatFiles.statusinfor, SCDatFiles.DatFiles.wireframe, SCDatFiles.DatFiles.ButtonSet

                If pjData.BindingManager.ExtraDatBinding(DatFile, Parameter, ObjectID) IsNot Nothing Then
                    ValueText.IsEnabled = True

                    If DatCommand IsNot Nothing Then
                        DatCommand.ReLoad(DatFile, Parameter, ObjectID)
                    Else
                        DatCommand = New DatCommand(DatFile, Parameter, ObjectID)
                    End If


                    Me.DataContext = pjData.BindingManager.ExtraDatBinding(DatFile, Parameter, ObjectID)
                    'If True Then
                    '    Dim tbind As New Binding
                    '    tbind.Path = New PropertyPath("ToolTipText")
                    '    ValueText.SetBinding(TextBox.ToolTipProperty, tbind)
                    'End If

                    'DatCommand = New DatCommand(DatFile, Parameter, ObjectID)
                    Dim binding As New Binding()
                    Select Case SpecialFlag
                        Case SFlag.FLAG
                            binding.ValidationRules.Add(New HexValidationRule)
                        Case Else
                            binding.ValidationRules.Add(New NotTextValidationRule)
                    End Select
                    binding.ValidatesOnDataErrors = True
                    binding.NotifyOnValidationError = True
                    binding.Path = New PropertyPath("Value")
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    binding.Mode = BindingMode.TwoWay
                    ValueText.SetBinding(TextBox.TextProperty, binding)

                    Dim CopyKeyGesture As KeyGesture = New KeyGesture(Key.C, ModifierKeys.Control, "Ctrl+C")
                    Dim CopyKeybinding As New KeyBinding(DatCommand, CopyKeyGesture) With {
                            .CommandParameter = DatCommand.CommandType.Copy
                        }
                    ValueText.InputBindings.Add(CopyKeybinding)

                    Dim PasteKeyGesture As KeyGesture = New KeyGesture(Key.V, ModifierKeys.Control, "Ctrl+V")
                    Dim PasteKeybinding As New KeyBinding(DatCommand, PasteKeyGesture) With {
                            .CommandParameter = DatCommand.CommandType.Paste
                        }
                    ValueText.InputBindings.Add(PasteKeybinding)

                    Dim ResetKeyGesture As KeyGesture = New KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R")
                    Dim ResetKeybinding As New KeyBinding(DatCommand, ResetKeyGesture) With {
                            .CommandParameter = DatCommand.CommandType.Reset
                        }
                    ValueText.InputBindings.Add(ResetKeybinding)



                    CopyItem.Command = DatCommand
                    CopyItem.CommandParameter = DatCommand.CommandType.Copy

                    PasteItem.Command = DatCommand
                    PasteItem.CommandParameter = DatCommand.CommandType.Paste

                    ResetItem.Command = DatCommand
                    ResetItem.CommandParameter = DatCommand.CommandType.Reset
                Else
                    Dim binding As New Binding()
                    binding.Mode = BindingMode.OneWay
                    ValueText.SetBinding(TextBox.TextProperty, Binding)
                    ValueText.Text = Application.Current.Resources("NotUse")
                    ValueText.IsEnabled = False
                    Exit Sub
                End If



            Case Else
                If Parameter = "UnitName" Then
                    ValueText.Text = ObjectID
                Else
                    Dim PropertyName As String = "Value"

                    Select Case SpecialFlag
                        Case SFlag.HP
                            PropertyName = "HPValue"
                        Case SFlag.HPV
                        Case SFlag.FLAG
                            PropertyName = "ValueFlag"
                    End Select
                    DatCommand.ReLoad(DatFile, Parameter, ObjectID)

                    Me.DataContext = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)


                    If True Then
                        Dim tbind As New Binding
                        tbind.Path = New PropertyPath("ToolTipText")
                        ValueText.SetBinding(TextBox.ToolTipProperty, tbind)
                        'ValueText.ToolTip = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).GetToolTip
                    End If




                    Dim binding As New Binding()


                    If pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID) IsNot Nothing Then
                        Me.DataContext = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)
                        Select Case SpecialFlag
                            Case SFlag.FLAG
                                binding.ValidationRules.Add(New HexValidationRule)
                            Case Else
                                binding.ValidationRules.Add(New NotTextValidationRule)
                        End Select
                        binding.ValidatesOnDataErrors = True
                        binding.NotifyOnValidationError = True
                        binding.Path = New PropertyPath(PropertyName)
                        binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        binding.Mode = BindingMode.TwoWay
                        ValueText.SetBinding(TextBox.TextProperty, binding)
                        ValueText.IsEnabled = True


                        Dim CopyKeyGesture As KeyGesture = New KeyGesture(Key.C, ModifierKeys.Control, "Ctrl+C")
                        Dim CopyKeybinding As New KeyBinding(DatCommand, CopyKeyGesture) With {
                                .CommandParameter = DatCommand.CommandType.Copy
                            }
                        ValueText.InputBindings.Add(CopyKeybinding)

                        Dim PasteKeyGesture As KeyGesture = New KeyGesture(Key.V, ModifierKeys.Control, "Ctrl+V")
                        Dim PasteKeybinding As New KeyBinding(DatCommand, PasteKeyGesture) With {
                                .CommandParameter = DatCommand.CommandType.Paste
                            }
                        ValueText.InputBindings.Add(PasteKeybinding)

                        Dim ResetKeyGesture As KeyGesture = New KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R")
                        Dim ResetKeybinding As New KeyBinding(DatCommand, ResetKeyGesture) With {
                                .CommandParameter = DatCommand.CommandType.Reset
                            }
                        ValueText.InputBindings.Add(ResetKeybinding)



                        CopyItem.Command = DatCommand
                        CopyItem.CommandParameter = DatCommand.CommandType.Copy

                        PasteItem.Command = DatCommand
                        PasteItem.CommandParameter = DatCommand.CommandType.Paste

                        ResetItem.Command = DatCommand
                        ResetItem.CommandParameter = DatCommand.CommandType.Reset
                    Else
                        Me.DataContext = pjData.BindingManager.NomalBinding(DatFile, Parameter)
                        binding.Mode = BindingMode.OneWay
                        ValueText.SetBinding(TextBox.TextProperty, binding)
                        ValueText.Text = Application.Current.Resources("NotUse")
                        ValueText.IsEnabled = False
                    End If
                End If
        End Select
    End Sub


    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String, Optional _SFlag As SFlag = SFlag.None, Optional TextWidth As Integer = 0)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter
        SpecialFlag = _SFlag

        Select Case DatFile
            Case SCDatFiles.DatFiles.statusinfor, SCDatFiles.DatFiles.wireframe, SCDatFiles.DatFiles.ButtonSet
                ValueText.Width = 7 * 3
                If Parameter = "Status" Then
                    TextStr.Text = Tool.GetText("FG_Status")
                ElseIf Parameter = "Display" Then
                    TextStr.Text = Tool.GetText("FG_Display")
                ElseIf Parameter = "Joint" Then
                    TextStr.Text = Tool.GetText("FG_StatusInfor1")
                ElseIf Parameter = "wire" Then
                    TextStr.Text = Tool.GetText("FG_WireFrame")
                ElseIf Parameter = "grp" Then
                    TextStr.Text = Tool.GetText("FG_GrpFrame")
                ElseIf Parameter = "tran" Then
                    TextStr.Text = Tool.GetText("FG_TranFrame")
                ElseIf Parameter = "ButtonSet" Then
                    TextStr.Text = Tool.GetText("buttonSet")
                End If
                If pjData.BindingManager.ExtraDatBinding(DatFile, Parameter, ObjectID) IsNot Nothing Then
                    ValueText.IsEnabled = True
                    Me.DataContext = pjData.BindingManager.ExtraDatBinding(DatFile, Parameter, ObjectID)
                    'If True Then
                    '    Dim tbind As New Binding
                    '    tbind.Path = New PropertyPath("ToolTipText")
                    '    ValueText.SetBinding(TextBox.ToolTipProperty, tbind)
                    'End If

                    DatCommand = New DatCommand(DatFile, Parameter, ObjectID)
                    Dim binding As New Binding()
                    Select Case SpecialFlag
                        Case SFlag.FLAG
                            binding.ValidationRules.Add(New HexValidationRule)
                        Case Else
                            binding.ValidationRules.Add(New NotTextValidationRule)
                    End Select
                    binding.ValidatesOnDataErrors = True
                    binding.NotifyOnValidationError = True
                    binding.Path = New PropertyPath("Value")
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    binding.Mode = BindingMode.TwoWay
                    ValueText.SetBinding(TextBox.TextProperty, binding)

                    Dim CopyKeyGesture As KeyGesture = New KeyGesture(Key.C, ModifierKeys.Control, "Ctrl+C")
                    Dim CopyKeybinding As New KeyBinding(DatCommand, CopyKeyGesture) With {
                        .CommandParameter = DatCommand.CommandType.Copy
                    }
                    ValueText.InputBindings.Add(CopyKeybinding)

                    Dim PasteKeyGesture As KeyGesture = New KeyGesture(Key.V, ModifierKeys.Control, "Ctrl+V")
                    Dim PasteKeybinding As New KeyBinding(DatCommand, PasteKeyGesture) With {
                        .CommandParameter = DatCommand.CommandType.Paste
                    }
                    ValueText.InputBindings.Add(PasteKeybinding)

                    Dim ResetKeyGesture As KeyGesture = New KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R")
                    Dim ResetKeybinding As New KeyBinding(DatCommand, ResetKeyGesture) With {
                        .CommandParameter = DatCommand.CommandType.Reset
                    }
                    ValueText.InputBindings.Add(ResetKeybinding)



                    CopyItem.Command = DatCommand
                    CopyItem.CommandParameter = DatCommand.CommandType.Copy

                    PasteItem.Command = DatCommand
                    PasteItem.CommandParameter = DatCommand.CommandType.Paste

                    ResetItem.Command = DatCommand
                    ResetItem.CommandParameter = DatCommand.CommandType.Reset
                Else
                    Dim binding As New Binding()
                    Me.DataContext = pjData.BindingManager.NomalBinding(DatFile, Parameter)
                    Binding.Mode = BindingMode.OneWay
                    ValueText.SetBinding(TextBox.TextProperty, Binding)
                    ValueText.Text = Application.Current.Resources("NotUse")
                    ValueText.IsEnabled = False
                    Exit Sub
                End If

            Case Else
                If TextWidth <> 0 Then
                    TextStr.Width = TextWidth
                End If

                If Parameter = "UnitName" Then
                    TextStr.Text = Tool.GetText("UnitName")
                    ValueText.Width = 7 * 5
                    ValueText.Text = ObjectID
                    ValueText.IsEnabled = False
                Else
                    Dim PropertyName As String = "Value"
                    Dim paramSize As Byte = pjData.Dat.ParamInfo(DatFile, Parameter, SCDatFiles.EParamInfo.Size)
                    Dim valueType As SCDatFiles.DatFiles = pjData.Dat.ParamInfo(DatFile, Parameter, SCDatFiles.EParamInfo.ValueType)
                    If valueType = SCDatFiles.DatFiles.None Then
                        Dim CharLen As Byte = 5
                        Select Case paramSize
                            Case 1
                                CharLen = 3
                            Case 2
                                CharLen = 5
                            Case 4
                                CharLen = 10
                        End Select
                        ValueText.Width = 7 * CharLen
                    Else
                        If valueType <= SCDatFiles.DatFiles.portdata Or valueType = SCDatFiles.DatFiles.Icon Then
                            ValueText.MinWidth = 7 * 3
                        Else
                            ValueText.MinWidth = 7 * 4
                        End If
                    End If




                    DatCommand = New DatCommand(DatFile, Parameter, ObjectID)
                    Dim binding As New Binding()
                    Select Case SpecialFlag
                        Case SFlag.HP
                            PropertyName = "HPValue"
                            ValueText.Width = 7 * 8
                        Case SFlag.HPV
                            TextStr.Margin = New Thickness(0)
                            TextStr.Text = ""
                        Case SFlag.FLAG
                            PropertyName = "ValueFlag"
                            ValueText.Width = 7 * 2 * paramSize
                    End Select


                    If pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID) IsNot Nothing Then
                        Me.DataContext = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)
                        If True Then
                            Dim tbind As New Binding
                            tbind.Path = New PropertyPath("ToolTipText")
                            ValueText.SetBinding(TextBox.ToolTipProperty, tbind)
                            'ValueText.ToolTip = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).GetToolTip
                        End If
                        Select Case SpecialFlag
                            Case SFlag.FLAG
                                binding.ValidationRules.Add(New HexValidationRule)
                            Case Else
                                binding.ValidationRules.Add(New NotTextValidationRule)
                        End Select
                        binding.ValidatesOnDataErrors = True
                        binding.NotifyOnValidationError = True
                        binding.Path = New PropertyPath(PropertyName)
                        binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        binding.Mode = BindingMode.TwoWay
                        ValueText.SetBinding(TextBox.TextProperty, binding)
                        ValueText.IsEnabled = True
                    Else
                        Me.DataContext = pjData.BindingManager.NomalBinding(DatFile, Parameter)
                        binding.Mode = BindingMode.OneWay
                        ValueText.SetBinding(TextBox.TextProperty, binding)
                        ValueText.Text = Application.Current.Resources("NotUse")
                        ValueText.IsEnabled = False
                        Exit Sub
                    End If



                    Dim CopyKeyGesture As KeyGesture = New KeyGesture(Key.C, ModifierKeys.Control, "Ctrl+C")
                    Dim CopyKeybinding As New KeyBinding(DatCommand, CopyKeyGesture) With {
                    .CommandParameter = DatCommand.CommandType.Copy
                }
                    ValueText.InputBindings.Add(CopyKeybinding)

                    Dim PasteKeyGesture As KeyGesture = New KeyGesture(Key.V, ModifierKeys.Control, "Ctrl+V")
                    Dim PasteKeybinding As New KeyBinding(DatCommand, PasteKeyGesture) With {
                    .CommandParameter = DatCommand.CommandType.Paste
                }
                    ValueText.InputBindings.Add(PasteKeybinding)

                    Dim ResetKeyGesture As KeyGesture = New KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R")
                    Dim ResetKeybinding As New KeyBinding(DatCommand, ResetKeyGesture) With {
                    .CommandParameter = DatCommand.CommandType.Reset
                }
                    ValueText.InputBindings.Add(ResetKeybinding)



                    CopyItem.Command = DatCommand
                    CopyItem.CommandParameter = DatCommand.CommandType.Copy

                    PasteItem.Command = DatCommand
                    PasteItem.CommandParameter = DatCommand.CommandType.Paste

                    ResetItem.Command = DatCommand
                    ResetItem.CommandParameter = DatCommand.CommandType.Reset
                End If


                'If Parameter = "Infestation" Then
                '    MsgBox("시작")
                '    MsgBox("오브젝트아이디 : " & ObjectID)
                '    MsgBox("값 : " & pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).Value)
                'End If




                'If Parameter = "Infestation" Then
                '    MsgBox("끝")
                'End If
        End Select

    End Sub



    Private Sub OpneMenu(sender As Object, e As ContextMenuEventArgs) Handles ValueText.ContextMenuOpening
        CopyItem.IsEnabled = DatCommand.IsEnabled(CopyItem.CommandParameter)
        PasteItem.IsEnabled = DatCommand.IsEnabled(PasteItem.CommandParameter)
        ResetItem.IsEnabled = DatCommand.IsEnabled(ResetItem.CommandParameter)
    End Sub

    Private Sub TextBoxMouseWhell(sender As Object, e As MouseWheelEventArgs) Handles ValueText.MouseWheel
        Dim ChangeValue As Integer = e.Delta / 100
        Select Case SpecialFlag
            Case SFlag.HP
                ChangeValue *= 256
            Case Else

        End Select

        Select Case DatFile
            Case SCDatFiles.DatFiles.statusinfor, SCDatFiles.DatFiles.wireframe, SCDatFiles.DatFiles.ButtonSet
                pjData.BindingManager.ExtraDatBinding(DatFile, Parameter, ObjectID).Value += ChangeValue
            Case Else
                pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).Value += ChangeValue
        End Select


    End Sub


    Private Sub TextboxPreviewKeyDown(sender As Object, e As KeyEventArgs) Handles ValueText.PreviewKeyDown
        Dim ChangeValue As Integer = 1
        Select Case SpecialFlag
            Case SFlag.HP
                ChangeValue *= 256
            Case Else

        End Select
        If e.Key = Key.Up Then
            Select Case DatFile
                Case SCDatFiles.DatFiles.statusinfor, SCDatFiles.DatFiles.wireframe, SCDatFiles.DatFiles.ButtonSet
                    pjData.BindingManager.ExtraDatBinding(DatFile, Parameter, ObjectID).Value += ChangeValue
                Case Else
                    pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).Value += ChangeValue
            End Select
        ElseIf e.Key = Key.Down Then
            Select Case DatFile
                Case SCDatFiles.DatFiles.statusinfor, SCDatFiles.DatFiles.wireframe, SCDatFiles.DatFiles.ButtonSet
                    pjData.BindingManager.ExtraDatBinding(DatFile, Parameter, ObjectID).Value -= ChangeValue
                Case Else
                    pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).Value -= ChangeValue
            End Select
        End If
    End Sub

    Private Sub ValueText_TextChanged(sender As Object, e As TextChangedEventArgs)
        RaiseEvent ValueChange(Me, e)
    End Sub
End Class
