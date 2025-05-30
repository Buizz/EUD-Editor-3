Imports MaterialDesignColors
Imports MaterialDesignColors.Recommended
Imports MaterialDesignThemes.Wpf

Public Class ThemeControl
    Private DefaultPalettName As String() = {"red", "pink", "purple", "deeppurple", "indigo", "blue", "lightblue",
        "cyan", "teal", "green", "lightgreen", "lime", "yellow", "amber", "orange", "deeporange", "brown", "grey", "bluegrey"}
    Private DefaultColors As String() = {"#FFF44336", "#FFE91E63", "#FF9C27B0", "#FF673AB7", "#FF3F51B5", "#FF2196F3", "#FF03A9F4",
    "#FF00BCD4", "#FF009688", "#FF4CAF50", "#FF8BC34A", "#FFCDDC39", "#FFFFEB3B", "#FFFFC107", "#FFFF9800", "#FFFF5722", "#FF795548", "#FF9E9E9E", "#FF607D8B"}

    Private DefaultPrimaryColor As MaterialDesignColor() = {
        MaterialDesignColor.Red, MaterialDesignColor.Pink, MaterialDesignColor.Purple, MaterialDesignColor.DeepPurple,
        MaterialDesignColor.Indigo, MaterialDesignColor.Blue, MaterialDesignColor.LightBlue, MaterialDesignColor.Cyan,
        MaterialDesignColor.Teal, MaterialDesignColor.Green, MaterialDesignColor.LightGreen, MaterialDesignColor.Lime,
        MaterialDesignColor.Yellow, MaterialDesignColor.Amber, MaterialDesignColor.Orange, MaterialDesignColor.DeepOrange,
        MaterialDesignColor.Brown, MaterialDesignColor.Grey, MaterialDesignColor.BlueGrey}
    Private DefaultSecondaryColor As MaterialDesignColor() = {
        MaterialDesignColor.RedSecondary, MaterialDesignColor.PinkSecondary, MaterialDesignColor.PurpleSecondary, MaterialDesignColor.DeepPurpleSecondary,
        MaterialDesignColor.IndigoSecondary, MaterialDesignColor.BlueSecondary, MaterialDesignColor.LightBlueSecondary, MaterialDesignColor.CyanSecondary,
        MaterialDesignColor.TealSecondary, MaterialDesignColor.GreenSecondary, MaterialDesignColor.LightGreenSecondary, MaterialDesignColor.LimeSecondary,
        MaterialDesignColor.YellowSecondary, MaterialDesignColor.AmberSecondary, MaterialDesignColor.OrangeSecondary, MaterialDesignColor.DeepOrangeSecondary,
        MaterialDesignColor.Lime, MaterialDesignColor.Lime, MaterialDesignColor.Lime}
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
        CheckedData.Background = New SolidColorBrush(pgData.PFiledFlagColor)


        For i = 0 To DefaultPalettName.Count - 1
            Dim btn As New Button
            btn.Style = Application.Current.Resources("MaterialDesignFlatButton")
            btn.Background = New SolidColorBrush(ColorConverter.ConvertFromString(DefaultColors(i)))
            btn.Width = 28.75
            btn.Tag = i
            AddHandler btn.Click, AddressOf ButtonClick
            DefaultPalettes.Children.Add(btn)
        Next
    End Sub
    Private Sub ButtonClick(sender As Object, e As RoutedEventArgs)
        Dim index As Integer = sender.Tag


        Dim helper As New PaletteHelper

        Dim theme As Theme = helper.GetTheme


        theme.SetPrimaryColor(SwatchHelper.Lookup(DefaultPrimaryColor(index)))
        theme.SetSecondaryColor(SwatchHelper.Lookup(DefaultSecondaryColor(index)))

        ctheme.PrimaryHueLight = theme.PrimaryLight.Color
        ctheme.PrimaryHueMid = theme.PrimaryMid.Color
        ctheme.PrimaryHueDark = theme.PrimaryDark.Color


        ctheme.SecondaryMid = theme.SecondaryMid.Color

        helper.SetTheme(theme)
    End Sub


    Private Sub ToggleButton_Checked(sender As Object, e As RoutedEventArgs)
        ctheme.IsLight = False
        ctheme.ApplyTheme()
        pgData.Setting(ProgramData.TSetting.Theme) = "Dark"
        If Tool.IsProjectLoad Then
            pjData.BindingManager.DataRefresh()
        End If
    End Sub

    Private Sub ToggleButton_Unchecked(sender As Object, e As RoutedEventArgs)
        ctheme.IsLight = True
        ctheme.ApplyTheme()
        pgData.Setting(ProgramData.TSetting.Theme) = "Light"
        If Tool.IsProjectLoad Then
            pjData.BindingManager.DataRefresh()
        End If
    End Sub





    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim mainBrush As SolidColorBrush = New SolidColorBrush(ctheme.PrimaryHueMid)
        MainColorPickerPopup.IsOpen = True
        MainColorPicker.InitColor(mainBrush.Color)
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Dim mainBrush As SolidColorBrush = New SolidColorBrush(ctheme.PrimaryHueLight)
        LightColorPickerPopup.IsOpen = True
        LightColorPicker.InitColor(mainBrush.Color)
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        Dim mainBrush As SolidColorBrush = New SolidColorBrush(ctheme.PrimaryHueMid)
        DefaultColorPickerPopup.IsOpen = True
        DefaultColorPicker.InitColor(mainBrush.Color)
    End Sub

    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        Dim mainBrush As SolidColorBrush = New SolidColorBrush(ctheme.PrimaryHueDark)
        DarkColorPickerPopup.IsOpen = True
        DarkColorPicker.InitColor(mainBrush.Color)
    End Sub

    Private Sub Button_Click_4(sender As Object, e As RoutedEventArgs)
        Dim mainBrush As SolidColorBrush = New SolidColorBrush(ctheme.SecondaryMid)
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

        ctheme.PrimaryHueLight = LightColor
        ctheme.PrimaryHueMid = MainColor
        ctheme.PrimaryHueDark = DarkColor


        ctheme.PrimaryHueLightForeground = CustomTheme.CheckBlack(LightColor)
        ctheme.PrimaryHueMidForeground = CustomTheme.CheckBlack(MainColor)
        ctheme.PrimaryHueDarkForeground = CustomTheme.CheckBlack(DarkColor)


        ctheme.SecondaryMid = AccentColor
        ctheme.SecondaryMidForeground = CustomTheme.CheckBlack(AccentColor)


        ctheme.ApplyTheme()

        'Dim PaletteHelper As New PaletteHelper()
        'PaletteHelper.SetLightDark(True)
    End Sub

    Private Sub LightColorPicker_ColorSelect(sender As Object, e As RoutedEventArgs)
        Dim MainColor As Color = sender

        ctheme.PrimaryHueLight = MainColor
        'ctheme.PrimaryHueLightForeground = CustomTheme.CheckBlack(MainColor)
        ctheme.ApplyTheme()
    End Sub

    Private Sub DefaultColorPicker_ColorSelect(sender As Object, e As RoutedEventArgs)
        Dim MainColor As Color = sender

        ctheme.PrimaryHueMid = MainColor
        'ctheme.PrimaryHueMidForeground = CustomTheme.CheckBlack(MainColor)
        ctheme.ApplyTheme()
    End Sub

    Private Sub DarkColorPicker_ColorSelect(sender As Object, e As RoutedEventArgs)
        Dim MainColor As Color = sender

        ctheme.PrimaryHueDark = MainColor
        'ctheme.PrimaryHueDarkForeground = CustomTheme.CheckBlack(MainColor)
        ctheme.ApplyTheme()
    End Sub

    Private Sub AccentColorPicker_ColorSelect(sender As Object, e As RoutedEventArgs)
        Dim MainColor As Color = sender

        ctheme.SecondaryMid = MainColor
        'ctheme.SecondaryMidForeground = CustomTheme.CheckBlack(MainColor)
        ctheme.ApplyTheme()
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
        CheckedDataColorPicker.InitColor(pgData.PFiledFlagColor)
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
        pgData.PFiledFlagColor = MainColor
        If Tool.IsProjectLoad Then
            pjData.BindingManager.DataRefresh()
        End If
        CheckedData.Background = New SolidColorBrush(pgData.PFiledFlagColor)
    End Sub

    Private Sub Button_Click_5(sender As Object, e As RoutedEventArgs)
        pgData.PFiledDefault = ColorConverter.ConvertFromString("#60DDCDCD")
        pgData.PFiledMapEditColor = ColorConverter.ConvertFromString("#60A1C7FF")
        pgData.PFiledEditColor = ColorConverter.ConvertFromString("#60FFA3FA")
        pgData.PFiledFlagColor = ColorConverter.ConvertFromString("#80FF8F6A")

        DefaultData.Background = New SolidColorBrush(pgData.PFiledDefault)
        MapEditorData.Background = New SolidColorBrush(pgData.PFiledMapEditColor)
        EditedData.Background = New SolidColorBrush(pgData.PFiledEditColor)
        CheckedData.Background = New SolidColorBrush(pgData.PFiledFlagColor)

        'Application.Current.Resources.Remove("PrimaryHueLightBrush")
        'Application.Current.Resources.Remove("PrimaryHueLightForegroundBrush")
        'Application.Current.Resources.Remove("PrimaryHueMidBrush")
        'Application.Current.Resources.Remove("PrimaryHueMidForegroundBrush")
        'Application.Current.Resources.Remove("PrimaryHueDarkBrush")
        'Application.Current.Resources.Remove("PrimaryHueDarkForegroundBrush")
        'Application.Current.Resources.Remove("SecondaryAccentBrush")
        'Application.Current.Resources.Remove("SecondaryAccentForegroundBrush")

        ctheme.InitColor()
        ctheme.ApplyTheme()


        'helper.ReplacePrimaryColor("bluegrey")
        'helper.ReplaceAccentColor("lime")
    End Sub
End Class