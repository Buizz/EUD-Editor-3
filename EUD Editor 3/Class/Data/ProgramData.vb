Public Class ProgramData
    Public ReadOnly Property Version As String = "0.0.1"

    Public Sub SetTheme(IsLight As Boolean)
        If IsLight Then
            Dim dict As New ResourceDictionary()
            dict.Source = New Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml", UriKind.Absolute)
            Application.Current.Resources.MergedDictionaries.Add(dict)
            Setting(ProgramData.TSetting.Theme) = "Light"


            dict = New ResourceDictionary()
            dict.Source = New Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.BlueGrey.xaml", UriKind.Absolute)
            Application.Current.Resources.MergedDictionaries.Add(dict)
        Else
            Dim dict As New ResourceDictionary()
            dict.Source = New Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml", UriKind.Absolute)
            Application.Current.Resources.MergedDictionaries.Add(dict)
            Setting(ProgramData.TSetting.Theme) = "Dark"

            dict = New ResourceDictionary()
            dict.Source = New Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.BlueGrey.xaml", UriKind.Absolute)
            Application.Current.Resources.MergedDictionaries.Add(dict)
        End If
    End Sub
    Public Sub SetLanguage(LanName As String)
        Dim dict As New ResourceDictionary()
        dict.Source = New Uri("Data\Language\" & LanName & ".xaml", UriKind.Relative)

        Application.Current.Resources.MergedDictionaries.Add(dict)

        Setting(ProgramData.TSetting.Language) = LanName
    End Sub


    Public ReadOnly Property LuaManager As LuaManager

    '사용자 지정 컬러
    Public Property LightFiledEditColor As Color = Color.FromArgb(255, 250, 226, 255)
    Public Property DarkFiledEditColor As Color = Color.FromArgb(255, 152, 129, 157)
    Public Property LightFiledMapEditColor As Color = Color.FromArgb(255, 226, 230, 255)
    Public Property DarkFiledMapEditColor As Color = Color.FromArgb(255, 129, 132, 157)
    Public Property LightFiledDefault As Color = Color.FromArgb(255, 243, 243, 243)
    Public Property DarkFiledDefault As Color = Color.FromArgb(255, 90, 90, 90)


    Public ReadOnly Property FiledEditColor As Color
        Get
            If Setting(TSetting.Theme) = "Dark" Then
                Return DarkFiledEditColor
            ElseIf Setting(TSetting.Theme) = "Light" Then
                Return LightFiledEditColor
            End If
        End Get
    End Property

    Public ReadOnly Property FiledMapEditColor As Color
        Get
            If Setting(TSetting.Theme) = "Dark" Then
                Return DarkFiledMapEditColor
            ElseIf Setting(TSetting.Theme) = "Light" Then
                Return LightFiledMapEditColor
            End If
        End Get
    End Property

    Public ReadOnly Property FiledDefault As Color
        Get
            If Setting(TSetting.Theme) = "Dark" Then
                Return DarkFiledDefault
            ElseIf Setting(TSetting.Theme) = "Light" Then
                Return LightFiledDefault
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
