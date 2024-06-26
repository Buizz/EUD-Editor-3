﻿Public Class RequireData
    Private UnitDatPage As Integer = 7

    Private DatFiles As SCDatFiles.DatFiles
    Private RealDatFiles As SCDatFiles.DatFiles
    Private IsUseRequire As Boolean = False

    Public Property ObjectID As Integer


    Public Sub New(tDatFiles As SCDatFiles.DatFiles, tObjectID As Integer, Optional tIsUseRequire As Boolean = False)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DatFiles = tDatFiles
        ObjectID = tObjectID
        IsUseRequire = tIsUseRequire

        If IsUseRequire Then
            UnitDatPage = 8
        End If

        Select Case DatFiles
            Case SCDatFiles.DatFiles.Stechdata
                RealDatFiles = SCDatFiles.DatFiles.techdata
            Case Else
                RealDatFiles = DatFiles
        End Select

        If IsUseRequire Then
            ReqCapacity.DataContext = pjData.BindingManager.RequireCapacityBinding(SCDatFiles.DatFiles.Stechdata)
            ReqCapacityText.DataContext = pjData.BindingManager.RequireCapacityBinding(SCDatFiles.DatFiles.Stechdata)
        Else
            ReqCapacity.DataContext = pjData.BindingManager.RequireCapacityBinding(DatFiles)
            ReqCapacityText.DataContext = pjData.BindingManager.RequireCapacityBinding(DatFiles)
        End If

        DataContext = pjData

        NameBar.Init(ObjectID, DatFiles, UnitDatPage)
        If tIsUseRequire Then
            RequireListbox.Init(SCDatFiles.DatFiles.Stechdata, ObjectID, RequireListbox.Tag)
        Else
            RequireListbox.Init(DatFiles, ObjectID, RequireListbox.Tag)
        End If


        'CAI.Init(DatFiles, ObjectID, CAI.Tag, 80)
        'HAI.Init(DatFiles, ObjectID, HAI.Tag, 80)
        'RTI.Init(DatFiles, ObjectID, RTI.Tag, 80)
        'AU.Init(DatFiles, ObjectID, AU.Tag, 80)
        'AM.Init(DatFiles, ObjectID, AM.Tag, 80)

        'RCA.Init(DatFiles, ObjectID, RCA.Tag)
        'AI.Init(DatFiles, ObjectID, AI.Tag, 300)
    End Sub
    Public Sub ReLoad(tDatFiles As SCDatFiles.DatFiles, tObjectID As Integer, Optional tIsUseRequire As Boolean = False)
        DatFiles = tDatFiles
        ObjectID = tObjectID
        IsUseRequire = tIsUseRequire

        Select Case DatFiles
            Case SCDatFiles.DatFiles.Stechdata
                RealDatFiles = SCDatFiles.DatFiles.techdata
            Case Else
                RealDatFiles = DatFiles
        End Select


        If IsUseRequire Then
            ReqCapacity.DataContext = pjData.BindingManager.RequireCapacityBinding(SCDatFiles.DatFiles.Stechdata)
            ReqCapacityText.DataContext = pjData.BindingManager.RequireCapacityBinding(SCDatFiles.DatFiles.Stechdata)
        Else
            ReqCapacity.DataContext = pjData.BindingManager.RequireCapacityBinding(DatFiles)
            ReqCapacityText.DataContext = pjData.BindingManager.RequireCapacityBinding(DatFiles)
        End If

        NameBar.ReLoad(ObjectID, DatFiles, UnitDatPage)
        If tIsUseRequire Then
            RequireListbox.ReLoad(SCDatFiles.DatFiles.Stechdata, ObjectID, RequireListbox.Tag)
        Else
            RequireListbox.ReLoad(DatFiles, ObjectID, RequireListbox.Tag)
        End If
        'CAI.ReLoad(DatFiles, ObjectID, CAI.Tag)
        'HAI.ReLoad(DatFiles, ObjectID, HAI.Tag)
        'RTI.ReLoad(DatFiles, ObjectID, RTI.Tag)
        'AU.ReLoad(DatFiles, ObjectID, AU.Tag)
        'AM.ReLoad(DatFiles, ObjectID, AM.Tag)

        'RCA.ReLoad(DatFiles, ObjectID, RCA.Tag)
        'AI.ReLoad(DatFiles, ObjectID, AI.Tag)
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        If Tool.CustomMsgBox(Tool.GetLanText("FG_DefaultUseToolTip"), MessageBoxButton.OKCancel) = MsgBoxResult.Cancel Then
            Return
        End If

        For i = 0 To SCCodeCount(RealDatFiles) - 1
            If IsUseRequire Then
                pjData.BindingManager.RequireDataBinding(i, SCDatFiles.DatFiles.Stechdata).IsDefaultUse = True
            Else
                pjData.BindingManager.RequireDataBinding(i, DatFiles).IsDefaultUse = True
            End If
        Next
        RequireListbox.ListReset()
        pjData.SetDirty(True)
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        If Tool.CustomMsgBox(Tool.GetLanText("FG_DontUseToolTip"), MessageBoxButton.OKCancel) = MsgBoxResult.Cancel Then
            Return
        End If

        For i = 0 To SCCodeCount(RealDatFiles) - 1
            If IsUseRequire Then
                pjData.BindingManager.RequireDataBinding(i, SCDatFiles.DatFiles.Stechdata).IsDontUse = True
            Else
                pjData.BindingManager.RequireDataBinding(i, DatFiles).IsDontUse = True
            End If
        Next
        RequireListbox.ListReset()
        pjData.SetDirty(True)
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        If Tool.CustomMsgBox(Tool.GetLanText("FG_AlwaysUseToolTip"), MessageBoxButton.OKCancel) = MsgBoxResult.Cancel Then
            Return
        End If

        For i = 0 To SCCodeCount(RealDatFiles) - 1
            If IsUseRequire Then
                pjData.BindingManager.RequireDataBinding(i, SCDatFiles.DatFiles.Stechdata).IsAlwaysUse = True
            Else
                pjData.BindingManager.RequireDataBinding(i, DatFiles).IsAlwaysUse = True
            End If
        Next
        RequireListbox.ListReset()
        pjData.SetDirty(True)
    End Sub

    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        If Tool.CustomMsgBox(Tool.GetLanText("FG_AlwaysCurrentUseToolTip"), MessageBoxButton.OKCancel) = MsgBoxResult.Cancel Then
            Return
        End If

        For i = 0 To SCCodeCount(RealDatFiles) - 1
            If IsUseRequire Then
                pjData.BindingManager.RequireDataBinding(i, SCDatFiles.DatFiles.Stechdata).IsAlwaysCurrentUse = True
            Else
                pjData.BindingManager.RequireDataBinding(i, DatFiles).IsAlwaysCurrentUse = True
            End If
        Next
        RequireListbox.ListReset()
        pjData.SetDirty(True)
    End Sub
End Class
