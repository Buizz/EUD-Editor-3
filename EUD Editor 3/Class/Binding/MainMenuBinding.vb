Imports System.ComponentModel

Public Class MainMenuBinding
    Implements INotifyPropertyChanged


    'Setting
    'NewProject
    'ProjectOpen
    'SaveProject
    'CloseProject
    'OpenMap
    'ComplieProject
    'DataEditor
    'TriggerEditor
    'PluginSetting

    Public Sub PropertyChangedPack()
        NotifyPropertyChanged("DataEditorName")
        NotifyPropertyChanged("ProgramName")
        NotifyPropertyChanged("GridOpacity")
        NotifyPropertyChanged("ProgressBarVisble")
        NotifyPropertyChanged("IsEnableSetting")
        NotifyPropertyChanged("IsEnableNewfile")
        NotifyPropertyChanged("IsEnableOpenFile")
        NotifyPropertyChanged("IsEnableSave")
        NotifyPropertyChanged("IsEnableClose")
        NotifyPropertyChanged("IsEnableOpenMap")
        NotifyPropertyChanged("IsEnableInsert")
        NotifyPropertyChanged("IsEnableDatEdit")
        NotifyPropertyChanged("IsEnableTriggerEdit")
        NotifyPropertyChanged("IsEnablePlugin")
        NotifyPropertyChanged("BackgroundOpenMap")
        NotifyPropertyChanged("BackgroundInsert")
        NotifyPropertyChanged("BackgroundDatEdit")
        NotifyPropertyChanged("BackgroundTriggerEdit")
        NotifyPropertyChanged("BackgroundPlugin")
    End Sub

    Public ReadOnly Property DataEditorName As String
        Get
            If pgData.IsCompilng Then
                Return Tool.GetText("Compile")
            Else
                Return Tool.GetProjectName & " - DATA EDITOR v" & pgData.Version.ToString
            End If
        End Get
    End Property
    Public ReadOnly Property ProgramName As String
        Get
            If pgData.IsCompilng Then
                Return Tool.GetText("Compile")
            Else
                Return Tool.GetTitleName
            End If
        End Get
    End Property



    Public ReadOnly Property GridOpacity As Double
        Get
            If pgData.IsCompilng Then
                Return 0.7
            Else
                Return 1
            End If
        End Get
    End Property
    Public ReadOnly Property ProgressBarVisble As Visibility
        Get
            If pgData.IsCompilng Then
                Return Visibility.Visible
            Else
                Return Visibility.Hidden
            End If
        End Get
    End Property


    Public ReadOnly Property IsEnableSetting As Boolean
        Get
            If pgData.IsCompilng Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Public ReadOnly Property IsEnableNewfile As Boolean
        Get
            If pgData.IsCompilng Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Public ReadOnly Property IsEnableOpenFile As Boolean
        Get
            If pgData.IsCompilng Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Public ReadOnly Property IsEnableSave As Boolean
        Get
            If pgData.IsCompilng Then
                Return False
            Else
                Return Tool.IsProjectLoad
            End If
        End Get
    End Property

    Public ReadOnly Property IsEnableClose As Boolean
        Get
            If pgData.IsCompilng Then
                Return False
            Else
                Return Tool.IsProjectLoad
            End If
        End Get
    End Property

    Public ReadOnly Property IsEnableOpenMap As Boolean
        Get
            Return Tool.IsProjectLoad
        End Get
    End Property

    Public ReadOnly Property IsEnableInsert As Boolean
        Get
            Return Tool.IsProjectLoad
        End Get
    End Property

    Public ReadOnly Property IsEnableDatEdit As Boolean
        Get
            Return Tool.IsProjectLoad
        End Get
    End Property

    Public ReadOnly Property IsEnableTriggerEdit As Boolean
        Get
            Return Tool.IsProjectLoad
        End Get
    End Property

    Public ReadOnly Property IsEnablePlugin As Boolean
        Get
            Return Tool.IsProjectLoad
        End Get
    End Property



    Public ReadOnly Property BackgroundOpenMap As SolidColorBrush
        Get
            Try
                If My.Computer.FileSystem.FileExists(pjData.OpenMapName) Then
                    Return Application.Current.Resources("PrimaryHueMidBrush")
                Else
                    Return Application.Current.Resources("SecondaryAccentBrush")
                End If
            Catch ex As Exception
                Return Application.Current.Resources("PrimaryHueMidBrush")
            End Try

        End Get
    End Property

    Public ReadOnly Property BackgroundInsert As SolidColorBrush
        Get
            Try
                If My.Computer.FileSystem.FileExists(pjData.OpenMapName) And
My.Computer.FileSystem.DirectoryExists(pjData.SaveMapdirectory) And
My.Computer.FileSystem.FileExists(pgData.Setting(ProgramData.TSetting.euddraft)) Then
                    Return Application.Current.Resources("PrimaryHueMidBrush")
                Else
                    Return Application.Current.Resources("SecondaryAccentBrush")
                End If
            Catch ex As Exception
                Return Application.Current.Resources("PrimaryHueMidBrush")
            End Try
        End Get
    End Property

    Public ReadOnly Property BackgroundDatEdit As SolidColorBrush
        Get
            If scData.LoadStarCraftData Then
                Return Application.Current.Resources("PrimaryHueMidBrush")
            Else
                Return Application.Current.Resources("SecondaryAccentBrush")
            End If
        End Get
    End Property

    Public ReadOnly Property BackgroundTriggerEdit As SolidColorBrush
        Get
            If scData.LoadStarCraftData Then
                Return Application.Current.Resources("PrimaryHueMidBrush")
            Else
                Return Application.Current.Resources("SecondaryAccentBrush")
            End If
        End Get
    End Property

    Public ReadOnly Property BackgroundPlugin As SolidColorBrush
        Get
            If scData.LoadStarCraftData Then
                Return Application.Current.Resources("PrimaryHueMidBrush")
            Else
                Return Application.Current.Resources("SecondaryAccentBrush")
            End If
        End Get
    End Property



    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
End Class
