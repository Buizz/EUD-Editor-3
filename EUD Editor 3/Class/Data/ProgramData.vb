Imports MaterialDesignThemes.Wpf

Public Class ProgramData
    '<주 버전>.<부 버전>.<빌드 번호>.<수정>
    Public ReadOnly Property Version As System.Version = Reflection.Assembly.GetExecutingAssembly().GetName().Version '"0.0.1"
    Public ReadOnly Property RecommendeuddraftVersion As New System.Version(0, 8, 3, 5)



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
        dict.Source = New Uri("Language\" & LanName & ".xaml", UriKind.Relative)

        Application.Current.Resources.MergedDictionaries.Add(dict)

        Setting(ProgramData.TSetting.Language) = LanName
    End Sub



    '사용자 지정 컬러
    Public Property PFiledDefault As Color
    Public Property PFiledEditColor As Color
    Public Property PFiledMapEditColor As Color
    Public Property PFiledFalgColor As Color

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


    Public Property IsCompilng As Boolean



    Private pgsetting As IniClass
    Public Sub New()
        pgsetting = New IniClass(Tool.GetSettingFile)

        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.CDLanuageChange)
        Catch ex As Exception
            Setting(ProgramData.TSetting.CDLanuageChange) = True
        End Try
        Try
            Dim uint As UInt32 = Setting(ProgramData.TSetting.CDLanuage)
        Catch ex As Exception
            Setting(ProgramData.TSetting.CDLanuage) = 0
        End Try

        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.DataEditorTopMost)
        Catch ex As Exception
            Setting(ProgramData.TSetting.DataEditorTopMost) = True
        End Try

        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.CheckReg)
        Catch ex As Exception
            Setting(ProgramData.TSetting.CheckReg) = True
        End Try

        Try
            Dim bool As Boolean = Setting(ProgramData.TSetting.TriggerEditrTopMost)
        Catch ex As Exception
            Setting(ProgramData.TSetting.TriggerEditrTopMost) = True
        End Try
        IsCompilng = False
        'Lan = New Language(Setting(TSetting.language))
    End Sub



    Public Enum TSetting
        euddraft = 0
        starcraft = 1
        Language = 2
        Theme = 3
        CDLanuage = 4

        PrimaryHueLightBrush = 5
        PrimaryHueLightForegroundBrush = 6
        PrimaryHueMidBrush = 7
        PrimaryHueMidForegroundBrush = 8
        PrimaryHueDarkBrush = 9
        PrimaryHueDarkForegroundBrush = 10
        SecondaryAccentBrush = 11
        SecondaryAccentForegroundBrush = 12
        DefaultData = 13
        MapEditorData = 14
        EditedData = 15
        CheckedData = 16
        CDLanuageChange = 17
        DataEditorTopMost = 18
        CheckReg = 19
        TriggerEditrTopMost = 20
    End Enum
    Private settingstr() As String = {"euddraft.exe", "StarCraft.exe", "Lanuage", "Theme", "CDLanuage",
    "PrimaryHueLightBrush", "PrimaryHueLightForegroundBrush", "PrimaryHueMidBrush", "PrimaryHueMidForegroundBrush",
    "PrimaryHueDarkBrush", "PrimaryHueDarkForegroundBrush", "SecondaryAccentBrush", "SecondaryAccentForegroundBrush",
    "DefaultData", "MapEditorData", "EditedData", "CheckedData",
    "CDLanuageChange", "DataEditorTopMost", "CheckReg", "TriggerEditrTopMost"}
    Public Property Setting(key As TSetting) As String
        Get
            Return pgsetting.SettingData(settingstr(key))
        End Get
        Set(value As String)
            pgsetting.SettingData(settingstr(key)) = value
        End Set
    End Property
    Public Sub SaveSetting()
        Setting(ProgramData.TSetting.DefaultData) = pgData.PFiledDefault.ToString()
        Setting(ProgramData.TSetting.MapEditorData) = pgData.PFiledMapEditColor.ToString()
        Setting(ProgramData.TSetting.EditedData) = pgData.PFiledEditColor.ToString()
        Setting(ProgramData.TSetting.CheckedData) = pgData.PFiledFalgColor.ToString()


        Setting(ProgramData.TSetting.PrimaryHueLightBrush) = CType(Application.Current.Resources("PrimaryHueLightBrush"), SolidColorBrush).Color.ToString
        Setting(ProgramData.TSetting.PrimaryHueMidBrush) = CType(Application.Current.Resources("PrimaryHueMidBrush"), SolidColorBrush).Color.ToString
        Setting(ProgramData.TSetting.PrimaryHueDarkBrush) = CType(Application.Current.Resources("PrimaryHueDarkBrush"), SolidColorBrush).Color.ToString
        Setting(ProgramData.TSetting.PrimaryHueLightForegroundBrush) = CType(Application.Current.Resources("PrimaryHueLightForegroundBrush"), SolidColorBrush).Color.ToString
        Setting(ProgramData.TSetting.PrimaryHueMidForegroundBrush) = CType(Application.Current.Resources("PrimaryHueMidForegroundBrush"), SolidColorBrush).Color.ToString
        Setting(ProgramData.TSetting.PrimaryHueDarkForegroundBrush) = CType(Application.Current.Resources("PrimaryHueDarkForegroundBrush"), SolidColorBrush).Color.ToString
        Setting(ProgramData.TSetting.SecondaryAccentBrush) = CType(Application.Current.Resources("SecondaryAccentBrush"), SolidColorBrush).Color.ToString
        Setting(ProgramData.TSetting.SecondaryAccentForegroundBrush) = CType(Application.Current.Resources("SecondaryAccentForegroundBrush"), SolidColorBrush).Color.ToString

        pgsetting.WriteIni()
    End Sub
End Class
