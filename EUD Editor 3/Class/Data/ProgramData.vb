Imports MaterialDesignThemes.Wpf

Public Class ProgramData
    Public ReadOnly Property Version As String = "0.0.1"

    Public Sub SetTheme(IsLight As Boolean)
        Dim palettes As New PaletteHelper
        palettes.SetLightDark(Not IsLight)
        If IsLight Then
            Setting(ProgramData.TSetting.Theme) = "Light"
        Else
            Setting(ProgramData.TSetting.Theme) = "Dark"
        End If

        'If IsLight Then

        '    Dim dict As New ResourceDictionary()
        '    dict.Source = New Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml", UriKind.Absolute)
        '    Application.Current.Resources.MergedDictionaries.Add(dict)
        '    Setting(ProgramData.TSetting.Theme) = "Light"


        '    dict = New ResourceDictionary()
        '    dict.Source = New Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.BlueGrey.xaml", UriKind.Absolute)
        '    Application.Current.Resources.MergedDictionaries.Add(dict)
        'Else
        '    Dim dict As New ResourceDictionary()
        '    dict.Source = New Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml", UriKind.Absolute)
        '    Application.Current.Resources.MergedDictionaries.Add(dict)
        '    Setting(ProgramData.TSetting.Theme) = "Dark"

        '    dict = New ResourceDictionary()
        '    dict.Source = New Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.BlueGrey.xaml", UriKind.Absolute)
        '    Application.Current.Resources.MergedDictionaries.Add(dict)
        'End If
    End Sub
    Public Sub SetLanguage(LanName As String)
        Dim dict As New ResourceDictionary()
        dict.Source = New Uri("Data\Language\" & LanName & ".xaml", UriKind.Relative)

        Application.Current.Resources.MergedDictionaries.Add(dict)

        Setting(ProgramData.TSetting.Language) = LanName
    End Sub


    Public ReadOnly Property LuaManager As LuaManager

    '사용자 지정 컬러
    Public Property PFiledDefault As Color = Color.FromArgb(64, 157, 157, 157)
    Public Property PFiledEditColor As Color = Color.FromArgb(64, 255, 93, 255)
    Public Property PFiledMapEditColor As Color = Color.FromArgb(64, 94, 86, 255)
    Public Property PFiledFalgColor As Color = Color.FromArgb(64, 255, 162, 82)

    Private DarkBackGroundColor As Color = Color.FromRgb(48, 48, 48)
    Private LightBackGroundColor As Color = Color.FromRgb(255, 255, 255)


    Private Function colorConbersion(ARGB As Color, RGB As Color) As Color
        Dim alpha As Double = ARGB.A / 256
        Dim alphai As Double = (1 - alpha)

        Return Color.FromArgb(255, alphai * RGB.R + alpha * ARGB.R,
                             alphai * RGB.G + alpha * ARGB.G,
                             alphai * RGB.B + alpha * ARGB.B)
    End Function

    Public ReadOnly Property FiledDefault As Color
        Get
            If Setting(TSetting.Theme) = "Dark" Then
                Return colorConbersion(PFiledDefault, DarkBackGroundColor)
            ElseIf Setting(TSetting.Theme) = "Light" Then
                Return colorConbersion(PFiledDefault, LightBackGroundColor)
            End If
        End Get
    End Property
    Public ReadOnly Property FiledEditColor As Color
        Get
            If Setting(TSetting.Theme) = "Dark" Then
                Return colorConbersion(PFiledEditColor, DarkBackGroundColor)
            ElseIf Setting(TSetting.Theme) = "Light" Then
                Return colorConbersion(PFiledEditColor, LightBackGroundColor)
            End If
        End Get
    End Property

    Public ReadOnly Property FiledMapEditColor As Color
        Get
            If Setting(TSetting.Theme) = "Dark" Then
                Return colorConbersion(PFiledMapEditColor, DarkBackGroundColor)
            ElseIf Setting(TSetting.Theme) = "Light" Then
                Return colorConbersion(PFiledMapEditColor, LightBackGroundColor)
            End If
        End Get
    End Property
    Public ReadOnly Property FiledFalgColor As Color
        Get
            If Setting(TSetting.Theme) = "Dark" Then
                Return colorConbersion(PFiledFalgColor, DarkBackGroundColor)
            ElseIf Setting(TSetting.Theme) = "Light" Then
                Return colorConbersion(PFiledFalgColor, LightBackGroundColor)
            End If
        End Get
    End Property




    Private pgsetting As IniClass


    Public Sub New()
        pgsetting = New IniClass(Tool.GetSettingFile)
        LuaManager = New LuaManager
        'Lan = New Language(Setting(TSetting.language))
    End Sub



    Public Enum TSetting
        euddraft = 0
        starcraft = 1
        Language = 2
        Theme = 3
        CDLanuage = 4
    End Enum
    Private settingstr() As String = {"euddraft.exe", "StarCraft.exe", "Lanuage", "Theme", "CDLanuage"}
    Public Property Setting(key As TSetting) As String
        Get
            Return pgsetting.SettingData(settingstr(key))
        End Get
        Set(value As String)
            pgsetting.SettingData(settingstr(key)) = value
        End Set
    End Property
    Public Sub SaveSetting()
        pgsetting.WriteIni()
    End Sub
End Class
