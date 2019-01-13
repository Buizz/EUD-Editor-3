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
    End Enum

    Public Sub ReLoad(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String, Optional _SFlag As SFlag = SFlag.None)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter
        SpecialFlag = _SFlag

        Dim PropertyName As String = "Value"


        DatCommand.ReLoad(DatFile, Parameter, ObjectID)

        Me.DataContext = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)

        Dim binding As New Binding()

        Select Case SpecialFlag
            Case SFlag.HP
                PropertyName = "HPValue"
                ValueText.Width = 7 * 8
            Case SFlag.HPV
                TextStr.Margin = New Thickness(0)
                TextStr.Text = ""
        End Select

        binding.ValidationRules.Add(New NotTextValidationRule)
        binding.Path = New PropertyPath(PropertyName)
        binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        binding.ValidatesOnDataErrors = True
        binding.NotifyOnValidationError = True
        ValueText.SetBinding(TextBox.TextProperty, binding)
    End Sub


    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String, Optional _SFlag As SFlag = SFlag.None)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter
        SpecialFlag = _SFlag

        Dim PropertyName As String = "Value"


        DatCommand = New DatCommand(DatFile, Parameter, ObjectID)

        Me.DataContext = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)
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


        Dim binding As New Binding()

        Select Case SpecialFlag
            Case SFlag.HP
                PropertyName = "HPValue"
                ValueText.Width = 7 * 8
            Case SFlag.HPV
                TextStr.Margin = New Thickness(0)
                TextStr.Text = ""
        End Select

        binding.ValidationRules.Add(New NotTextValidationRule)
        binding.Path = New PropertyPath(PropertyName)
        binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        binding.ValidatesOnDataErrors = True
        binding.NotifyOnValidationError = True
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
    End Sub



    Private Sub OpneMenu(sender As Object, e As ContextMenuEventArgs) Handles ValueText.ContextMenuOpening
        CopyItem.IsEnabled = DatCommand.IsEnabled(CopyItem.CommandParameter)
        PasteItem.IsEnabled = DatCommand.IsEnabled(PasteItem.CommandParameter)
        ResetItem.IsEnabled = DatCommand.IsEnabled(ResetItem.CommandParameter)
    End Sub
End Class
