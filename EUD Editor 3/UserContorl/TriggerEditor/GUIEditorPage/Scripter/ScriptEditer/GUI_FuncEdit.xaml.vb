Imports System.Text.RegularExpressions
Imports MaterialDesignThemes.Wpf

Public Class GUI_FuncEdit
    Private p As GUIScriptEditerWindow
    Private scr As ScriptBlock


    Public Sub New(tp As GUIScriptEditerWindow, tscr As ScriptBlock)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        p = tp
        scr = tscr

        AddHandler p.OkayBtnEvent, AddressOf OkayAction

        'Dim colorcode As String = tescm.Tabkeys("Func")
        'colorbox.Background = New SolidColorBrush(ColorConverter.ConvertFromString(colorcode))

        Dim funclist() As String = {"onPluginStart", "beforeTriggerExec", "afterTriggerExec", "constructor"}

        Dim index As Integer = funclist.ToList.IndexOf(scr.value)

        If index = -1 Then
            ttb.SelectedIndex = 0
            FuncNametb.Text = scr.value
            FuncNametb.Visibility = Visibility.Visible
            If CheckEditable() Then
                ErrorLog.Content = ""
                p.OkBtn.IsEnabled = True
            Else
                p.OkBtn.IsEnabled = False
            End If
        Else
            ttb.SelectedIndex = index + 1
            FuncNametb.Visibility = Visibility.Collapsed
        End If



        argtip.Text = scr.GetFuncTooltip


        Dim argsb As List(Of ScriptBlock) = tescm.GetFuncArgs(scr)
        For i = 0 To argsb.Count - 1
            Dim argname As String = argsb(i).value.Split("ᒧ").First

            Dim chip As New Chip
            chip.Content = argname
            chip.Tag = argname

            AddHandler chip.Click, AddressOf ChipClick
            chippanel.Children.Add(chip)

            'argname
        Next
        '<materialDesign:Chip
        'Content = "James Willock" >
        '<materialDesign:Chip.Icon>
        '    <Image
        '        Source="Resources/ProfilePic.jpg"/>
        '</materialDesign:Chip.Icon>
        '</materialDesign:Chip>
    End Sub

    Private Sub ChipClick(sender As Object, e As RoutedEventArgs)
        Dim chip As Chip = sender
        Dim str As String = "[" & chip.Tag & "]"

        argtip.SelectedText = str
        argtip.SelectionLength = 0
        argtip.SelectionStart += str.Length
        argtip.Focus()
    End Sub




    Public Sub OkayAction(sender As Object, e As RoutedEventArgs)
        If ttb.SelectedIndex = 0 Then
            scr.value = FuncNametb.Text
            scr.SetFuncTooltip(argtip.Text)
        Else
            scr.value = ttb.Text
            scr.SetFuncTooltip(argtip.Text)
        End If
    End Sub


    Private Sub FuncNametb_TextChanged(sender As Object, e As TextChangedEventArgs)
        If FuncNametb.Visibility = Visibility.Visible Then
            If CheckEditable() Then
                ErrorLog.Content = ""
                p.OkBtn.IsEnabled = True
            Else
                p.OkBtn.IsEnabled = False
            End If
        End If
    End Sub

    Private Function CheckEditable() As Boolean
        Dim nametext As String = FuncNametb.Text


        If nametext.Count = 0 Then
            ErrorLog.Content = "함수명은 비어있을 수 없습니다."
            Return False
        End If

        If IsNumeric(Mid(nametext, 1, 1)) Then
            ErrorLog.Content = "첫 문자는 숫자일 수 없습니다."
            Return False
        End If



        Dim rgx As New Regex("[ !@#$%^&*=]")

        If rgx.IsMatch(nametext) Then
            ErrorLog.Content = "잘못된 문자가 포함되어 있습니다."
            Return False
        End If


        Dim funcstr As List(Of String) = tescm.GetFuncList(p._GUIScriptEditorUI.PTEFile)
        If funcstr.IndexOf(nametext) <> -1 Then
            If tescm.GetFuncInfor(scr.value, p._GUIScriptEditorUI.Script) IsNot scr Then
                ErrorLog.Content = "중복된 함수명입니다."
                Return False
            End If
        End If



        Return True
    End Function

    Private Sub ComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If ttb.SelectedIndex = 0 Then
            FuncNametb.Visibility = Visibility.Visible
            If CheckEditable() Then
                ErrorLog.Content = ""
                p.OkBtn.IsEnabled = True
            Else
                p.OkBtn.IsEnabled = False
            End If
        Else
            FuncNametb.Visibility = Visibility.Collapsed
            ErrorLog.Content = ""
            p.OkBtn.IsEnabled = True
        End If


    End Sub

    Private Sub ComboBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        If CheckEditable() Then
            ErrorLog.Content = ""
            p.OkBtn.IsEnabled = True
        Else
            p.OkBtn.IsEnabled = False
        End If
    End Sub

End Class
