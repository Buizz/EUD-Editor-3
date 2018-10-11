Public Class ProgramData
    Private tVersion As String = "0.0.1"
    Public ReadOnly Property Version As String
        Get
            Return tVersion
        End Get
    End Property



    Private pgsetting As IniClass


    Public Sub New()
        pgsetting = New IniClass(Tool.GetSettingFile)
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
