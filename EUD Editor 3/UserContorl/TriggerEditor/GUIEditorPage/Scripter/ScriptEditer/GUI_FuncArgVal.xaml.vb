﻿Imports System.Text.RegularExpressions

Public Class GUI_FuncArgVal
    Private p As GUIScriptEditerWindow
    Private scr As ScriptBlock

    Private isLoad As Boolean = False
    Public Sub New(tp As GUIScriptEditerWindow, tscr As ScriptBlock)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        p = tp
        scr = tscr


        Dim index As Integer = -1
        For i = 0 To tescm.SCValueType.Count - 1
            Dim comboboxitem As New ComboBoxItem
            comboboxitem.Tag = tescm.SCValueType(i)
            comboboxitem.Content = tescm.SCValueType(i)
            typecombobox.Items.Add(comboboxitem)

            If tescm.SCValueType(i) = scr.name Then
                index = i
            End If
        Next
        typecombobox.SelectedIndex = index


        AddHandler p.OkayBtnEvent, AddressOf OkayAction

        Dim colorcode As String = tescm.Tabkeys("Value")
        colorbox.Background = New SolidColorBrush(ColorConverter.ConvertFromString(colorcode))



        ttb.Text = scr.value
        argtip.Text = scr.value2

        If CheckEditable() Then
            ErrorLog.Content = ""
            p.OkBtn.IsEnabled = True
        Else
            p.OkBtn.IsEnabled = False
        End If

        isLoad = True
    End Sub



    Public Sub OkayAction(sender As Object, e As RoutedEventArgs)


        scr.value = ttb.Text
        scr.value2 = argtip.Text


        scr.name = CType(typecombobox.SelectedItem, ComboBoxItem).Tag
    End Sub

    Private Sub ttb_TextChanged(sender As Object, e As TextChangedEventArgs)
        If CheckEditable() Then
            ErrorLog.Content = ""
            p.OkBtn.IsEnabled = True
        Else
            p.OkBtn.IsEnabled = False
        End If
    End Sub


    Private Function CheckEditable() As Boolean
        Dim nametext As String = ttb.Text

        If typecombobox.SelectedItem Is Nothing Then
            Return False
        End If


        If nametext.Count = 0 Then
            ErrorLog.Content = "변수명은 비어있을 수 없습니다."
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


        Return True
    End Function

    Private Sub typecombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If IsLoad Then
            If CheckEditable() Then
                ErrorLog.Content = ""
                p.OkBtn.IsEnabled = True
            Else
                p.OkBtn.IsEnabled = False
            End If
        End If

    End Sub
End Class
