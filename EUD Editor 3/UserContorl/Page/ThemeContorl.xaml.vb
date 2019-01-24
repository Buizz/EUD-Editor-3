Imports MaterialDesignColors
Imports MaterialDesignThemes.Wpf

Public Class ThemeContorl
    Private DefaultPalettName As String() = {"red", "pink", "purple", "deeppurple", "indigo", "blue", "lightblue",
        "cyan", "teal", "green", "lightgreen", "lime", "yellow", "amber", "orange", "deeporange", "brown", "grey", "bluegrey"}
    Private DefaultColors As String() = {"#FFF44336", "#FFE91E63", "#FF9C27B0", "#FF673AB7", "#FF3F51B5", "#FF2196F3", "#FF03A9F4",
    "#FF00BCD4", "#FF009688", "#FF4CAF50", "#FF8BC34A", "#FFCDDC39", "#FFFFEB3B", "#FFFFC107", "#FFFF9800", "#FFFF5722", "#FF795548", "#FF9E9E9E", "#FF607D8B"}

    Public Sub New()
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        '밝기
        If pgData.Setting(ProgramData.TSetting.Theme) = "Dark" Then
            ToggleBtn.IsChecked = True
        Else
            ToggleBtn.IsChecked = False
        End If


        DefaultData.Background = New SolidColorBrush(pgData.PFiledDefault)
        MapEditorData.Background = New SolidColorBrush(pgData.PFiledMapEditColor)
        EditedData.Background = New SolidColorBrush(pgData.PFiledEditColor)
        CheckedData.Background = New SolidColorBrush(pgData.PFiledFalgColor)


        For i = 0 To DefaultPalettName.Count - 1
            Dim btn As New Button
            btn.Style = Application.Current.Resources("MaterialDesignFlatButton")
            btn.Background = New SolidColorBrush(ColorConverter.ConvertFromString(DefaultColors(i)))
            btn.Width = 28.75
            btn.Tag = i
            AddHandler btn.Click, AddressOf ButtonClick
            DefaultPalettes.Children.Add(btn)
        Next
        'Dim tempStr As String = ""
        'For i = 0 To DefaultPalettName.Count - 1
        '    Dim MyBorder As New Border
        '    Dim helper As New PaletteHelper
        '    helper.ReplacePrimaryColor(DefaultPalettName(i))
        '    Dim palette As Palette = helper.QueryPalette


        '    Dim hue As Hue = palette.PrimarySwatch().ExemplarHue
        '    MyBorder.Background = New SolidColorBrush(hue.Color)
        '    MyBorder.Width = 30
        '    MyBorder.Height = 30
        '    DefaultPalettes.Children.Add(MyBorder)
        '    tempStr = tempStr & ", """ & hue.Color.ToString & """"
        'Next
        'My.Computer.Clipboard.SetText(tempStr)
    End Sub
    Private Sub ButtonClick(sender As Object, e As RoutedEventArgs)
        Dim index As Integer = sender.Tag


        Application.Current.Resources.Remove("PrimaryHueLightBrush")
        Application.Current.Resources.Remove("PrimaryHueLightForegroundBrush")
        Application.Current.Resources.Remove("PrimaryHueMidBrush")
        Application.Current.Resources.Remove("PrimaryHueMidForegroundBrush")
        Application.Current.Resources.Remove("PrimaryHueDarkBrush")
        Application.Current.Resources.Remove("PrimaryHueDarkForegroundBrush")
        Application.Current.Resources.Remove("SecondaryAccentBrush")
        Application.Current.Resources.Remove("SecondaryAccentForegroundBrush")


        Dim helper As New PaletteHelper
        helper.ReplacePrimaryColor(DefaultPalettName(index))

        Try
            helper.ReplaceAccentColor(DefaultPalettName(index))
        Catch ex As Exception
            helper.ReplaceAccentColor("lime")
        End Try
    End Sub


    Private Sub ToggleButton_Checked(sender As Object, e As RoutedEventArgs)
        pgData.SetTheme(False)
        If Tool.IsProjectLoad Then
            pjData.BindingManager.DataRefresh()
        End If

    End Sub

    Private Sub ToggleButton_Unchecked(sender As Object, e As RoutedEventArgs)
        pgData.SetTheme(True)
        If Tool.IsProjectLoad Then
            pjData.BindingManager.DataRefresh()
        End If
    End Sub



    Private Function CheckBlack(color As Color) As SolidColorBrush
        Dim h As Double
        Dim s As Double
        Dim v As Double

        ColorPicker.ColorToHSV(color, h, s, v)

        If s < 0.3 And v > 0.7 Then '밝음
            Return New SolidColorBrush(Colors.Black)
        End If

        If v <= 0.7 Then '어두움
            Return New SolidColorBrush(Colors.White)
        End If

        Select Case h
            Case 30 To 210
                Return New SolidColorBrush(Colors.Black)
            Case Else
                Return New SolidColorBrush(Colors.White)
        End Select

        Return New SolidColorBrush(Colors.Black)
    End Function


    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim mainBrush As SolidColorBrush = Application.Current.Resources("PrimaryHueMidBrush")
        MainColorPickerPopup.IsOpen = True
        MainColorPicker.InitColor(mainBrush.Color)
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Dim mainBrush As SolidColorBrush = Application.Current.Resources("PrimaryHueLightBrush")
        LightColorPickerPopup.IsOpen = True
        LightColorPicker.InitColor(mainBrush.Color)
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        Dim mainBrush As SolidColorBrush = Application.Current.Resources("PrimaryHueMidBrush")
        DefaultColorPickerPopup.IsOpen = True
        DefaultColorPicker.InitColor(mainBrush.Color)
    End Sub

    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        Dim mainBrush As SolidColorBrush = Application.Current.Resources("PrimaryHueDarkBrush")
        DarkColorPickerPopup.IsOpen = True
        DarkColorPicker.InitColor(mainBrush.Color)
    End Sub

    Private Sub Button_Click_4(sender As Object, e As RoutedEventArgs)
        Dim mainBrush As SolidColorBrush = Application.Current.Resources("SecondaryAccentBrush")
        AccentColorPickerPopup.IsOpen = True
        AccentColorPicker.InitColor(mainBrush.Color)
    End Sub


    Private Sub MainColorPicker_ColorSelect(sender As Object, e As RoutedEventArgs)
        Dim MainColor As Color = sender
        Dim h As Double
        Dim s As Double
        Dim v As Double

        ColorPicker.ColorToHSV(MainColor, h, s, v)

        Dim LightColor As Color = ColorPicker.ColorFromHSV(h, s * 0.6, Math.Min(v * 1.6, 1))
        Dim DarkColor As Color = ColorPicker.ColorFromHSV(h, s, v * 0.8)

        Dim AccentColor As Color = ColorPicker.ColorFromHSV(h + 40, s * 0.6, Math.Min(v * 1.6, 1))


        Application.Current.Resources("PrimaryHueLightBrush") = New SolidColorBrush(LightColor)
        Application.Current.Resources("PrimaryHueMidBrush") = New SolidColorBrush(MainColor)
        Application.Current.Resources("PrimaryHueDarkBrush") = New SolidColorBrush(DarkColor)

        Application.Current.Resources("PrimaryHueLightForegroundBrush") = CheckBlack(LightColor)
        Application.Current.Resources("PrimaryHueMidForegroundBrush") = CheckBlack(MainColor)
        Application.Current.Resources("PrimaryHueDarkForegroundBrush") = CheckBlack(DarkColor)

        Application.Current.Resources("SecondaryAccentBrush") = New SolidColorBrush(AccentColor)
        Application.Current.Resources("SecondaryAccentForegroundBrush") = CheckBlack(AccentColor)

        'Dim PaletteHelper As New PaletteHelper()
        'PaletteHelper.SetLightDark(True)
    End Sub

    Private Sub LightColorPicker_ColorSelect(sender As Object, e As RoutedEventArgs)
        Dim MainColor As Color = sender

        'For i = 0 To Application.Current.Resources.Keys.Count - 1
        '    MsgBox(Application.Current.Resources.Keys(i).ToString)
        'Next



        Application.Current.Resources("PrimaryHueLightBrush") = New SolidColorBrush(MainColor)
        Application.Current.Resources("PrimaryHueLightForegroundBrush") = CheckBlack(MainColor)
        'Application.Current.Resources.Remove("PrimaryHueLightBrush")


        'Application.Current.Resources.Remove("PrimaryHueLightForegroundBrush")
    End Sub

    Private Sub DefaultColorPicker_ColorSelect(sender As Object, e As RoutedEventArgs)
        Dim MainColor As Color = sender

        Application.Current.Resources("PrimaryHueMidBrush") = New SolidColorBrush(MainColor)
        Application.Current.Resources("PrimaryHueMidForegroundBrush") = CheckBlack(MainColor)
    End Sub

    Private Sub DarkColorPicker_ColorSelect(sender As Object, e As RoutedEventArgs)
        Dim MainColor As Color = sender

        Application.Current.Resources("PrimaryHueDarkBrush") = New SolidColorBrush(MainColor)
        Application.Current.Resources("PrimaryHueDarkForegroundBrush") = CheckBlack(MainColor)
    End Sub

    Private Sub AccentColorPicker_ColorSelect(sender As Object, e As RoutedEventArgs)
        Dim MainColor As Color = sender

        Application.Current.Resources("SecondaryAccentBrush") = New SolidColorBrush(MainColor)
        Application.Current.Resources("SecondaryAccentForegroundBrush") = CheckBlack(MainColor)
    End Sub

    Private Sub DefaultData_Click(sender As Object, e As RoutedEventArgs)
        DefaultDataPopup.IsOpen = True
        DefaultDataColorPicker.InitColor(pgData.PFiledDefault)
    End Sub

    Private Sub MapEditorData_Click(sender As Object, e As RoutedEventArgs)
        MapEditorDataPopup.IsOpen = True
        MapEditorDataColorPicker.InitColor(pgData.PFiledMapEditColor)
    End Sub

    Private Sub EditedData_Click(sender As Object, e As RoutedEventArgs)
        EditedDataPopup.IsOpen = True
        EditedDataColorPicker.InitColor(pgData.PFiledEditColor)
    End Sub

    Private Sub CheckedData_Click(sender As Object, e As RoutedEventArgs)
        CheckedDataPopup.IsOpen = True
        CheckedDataColorPicker.InitColor(pgData.PFiledFalgColor)
    End Sub

    Private Sub DefaultDataColorPicker_ColorSelect(sender As Object, e As RoutedEventArgs)
        Dim MainColor As Color = sender
        pgData.PFiledDefault = MainColor
        If Tool.IsProjectLoad Then
            pjData.BindingManager.DataRefresh()
        End If
        DefaultData.Background = New SolidColorBrush(pgData.PFiledDefault)
    End Sub

    Private Sub MapEditorColorPicker_ColorSelect(sender As Object, e As RoutedEventArgs)
        Dim MainColor As Color = sender
        pgData.PFiledMapEditColor = MainColor
        If Tool.IsProjectLoad Then
            pjData.BindingManager.DataRefresh()
        End If
        MapEditorData.Background = New SolidColorBrush(pgData.PFiledMapEditColor)
    End Sub

    Private Sub EditedDataColorPicker_ColorSelect(sender As Object, e As RoutedEventArgs)
        Dim MainColor As Color = sender
        pgData.PFiledEditColor = MainColor
        If Tool.IsProjectLoad Then
            pjData.BindingManager.DataRefresh()
        End If
        EditedData.Background = New SolidColorBrush(pgData.PFiledEditColor)
    End Sub

    Private Sub CheckedDataColorPicker_ColorSelect(sender As Object, e As RoutedEventArgs)
        Dim MainColor As Color = sender
        pgData.PFiledFalgColor = MainColor
        If Tool.IsProjectLoad Then
            pjData.BindingManager.DataRefresh()
        End If
        CheckedData.Background = New SolidColorBrush(pgData.PFiledFalgColor)
    End Sub

    Private Sub Button_Click_5(sender As Object, e As RoutedEventArgs)
        pgData.PFiledDefault = ColorConverter.ConvertFromString("#60DDCDCD")
        pgData.PFiledMapEditColor = ColorConverter.ConvertFromString("#60A1C7FF")
        pgData.PFiledEditColor = ColorConverter.ConvertFromString("#60FFA3FA")
        pgData.PFiledFalgColor = ColorConverter.ConvertFromString("#80FF8F6A")

        DefaultData.Background = New SolidColorBrush(pgData.PFiledDefault)
        MapEditorData.Background = New SolidColorBrush(pgData.PFiledMapEditColor)
        EditedData.Background = New SolidColorBrush(pgData.PFiledEditColor)
        CheckedData.Background = New SolidColorBrush(pgData.PFiledFalgColor)

        Application.Current.Resources.Remove("PrimaryHueLightBrush")
        Application.Current.Resources.Remove("PrimaryHueLightForegroundBrush")
        Application.Current.Resources.Remove("PrimaryHueMidBrush")
        Application.Current.Resources.Remove("PrimaryHueMidForegroundBrush")
        Application.Current.Resources.Remove("PrimaryHueDarkBrush")
        Application.Current.Resources.Remove("PrimaryHueDarkForegroundBrush")
        Application.Current.Resources.Remove("SecondaryAccentBrush")
        Application.Current.Resources.Remove("SecondaryAccentForegroundBrush")


        Dim helper As New PaletteHelper
        helper.ReplacePrimaryColor("bluegrey")
        helper.ReplaceAccentColor("lime")
    End Sub
End Class
'MaterialDesignToolButton