Public Class ProgramData
    Public Sub SetTheme(IsLight As Boolean)
        If IsLight Then
            Dim dict As New ResourceDictionary()
            dict.Source = New Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml", UriKind.Absolute)
            Application.Current.Resources.MergedDictionaries.Add(dict)
            Setting(ProgramData.TSetting.Theme) = "Light"


            dict = New ResourceDictionary()
            dict.Source = New Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Cyan.xaml", UriKind.Absolute)
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

    'Public ReadOnly Property Lan As Language
    Public ReadOnly Property Version As String = "0.0.1"



    Private pgsetting As IniClass


    Public Sub New()
        pgsetting = New IniClass(Tool.GetSettingFile)
        'Lan = New Language(Setting(TSetting.language))
    End Sub



    Public Enum TSetting
        euddraft = 0
        starcraft = 1
        Language = 2
        Theme = 3
    End Enum
    Private settingstr() As String = {"euddraft.exe", "StarCraft.exe", "Lanuage", "Theme"}
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
