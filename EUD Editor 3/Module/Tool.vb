Namespace Tool
    Module Tool
        Public ReadOnly Property GetSettingFile() As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "Setting.ini"
            End Get
        End Property

        Public Function GetTitleName() As String
            Return "EUD Editor 3 v" & pgData.Version
        End Function
    End Module
End Namespace
