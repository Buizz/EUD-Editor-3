Public Class ProgramData
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
    End Enum
    Private settingstr() As String = {"euddraft.exe", "StarCraft.exe"}
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
