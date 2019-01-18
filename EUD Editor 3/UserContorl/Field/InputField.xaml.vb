Public Class InputField
    Private DatFile As SCDatFiles.DatFiles
    Private ObjectID As Integer
    Private Parameter As String

    Private DatCommand As DatCommand

    Private SpecialFlag As SFlag


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
        Else
            Me.DataContext = pjData.BindingManager.NomalBinding(DatFile, Parameter)
            binding.Mode = BindingMode.OneWay
            ValueText.SetBinding(TextBox.TextProperty, binding)
            ValueText.Text = Application.Current.Resources("NotUse")
            ValueText.IsEnabled = False
        End If
    End Sub


    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String, Optional _SFlag As SFlag = SFlag.None)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter
        SpecialFlag = _SFlag





        'If Parameter = "Infestation" Then
        '    MsgBox("시작")
        '    MsgBox("오브젝트아이디 : " & ObjectID)
        '    MsgBox("값 : " & pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).Value)
        'End If

        Dim PropertyName As String = "Value"
        Dim paramSize As Byte = pjData.Dat.ParamInfo(DatFile, Parameter, SCDatFiles.EParamInfo.Size)

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


        'If Parameter = "Infestation" Then
        '    MsgBox("끝")
        'End If
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
        pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).Value += ChangeValue
    End Sub


    Private Sub TextboxPreviewKeyDown(sender As Object, e As KeyEventArgs) Handles ValueText.PreviewKeyDown
        Dim ChangeValue As Integer = 1
        Select Case SpecialFlag
            Case SFlag.HP
                ChangeValue *= 256
            Case Else

        End Select
        If e.Key = Key.Up Then
            pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).Value += ChangeValue
        ElseIf e.Key = Key.Down Then
            pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).Value -= ChangeValue
        End If
    End Sub

End Class
