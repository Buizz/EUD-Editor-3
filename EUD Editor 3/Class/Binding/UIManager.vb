Imports System.ComponentModel

Public Class UIManager
    Implements INotifyPropertyChanged
    '바인딩 할 때 본체도 넘겨주자고.
    '잘못된 값일 경우 또는 지원안하는 부분일 경우 없에버리기.~

    Private Datfile As SCDatFiles.DatFiles
    Private ObjectID As Integer

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged


    Public Sub New(tDatfile As SCDatFiles.DatFiles, tObjectID As Integer)
        Datfile = tDatfile
        ObjectID = tObjectID
    End Sub

    Public ReadOnly Property Name() As String
        Get
            If Datfile = SCDatFiles.DatFiles.stattxt Then
                Return "[" & Format(ObjectID + 1, "000") & "]  " & pjData.CodeLabel(Datfile, ObjectID + 1, True)
            Else
                Return "[" & Format(ObjectID, "000") & "]  " & pjData.CodeLabel(Datfile, ObjectID, True)
            End If
        End Get
    End Property
    Public ReadOnly Property TabName() As String
        Get
            If Datfile = SCDatFiles.DatFiles.stattxt Then
                Return Tool.GetText(Datfilesname(Datfile)) & " '" & pjData.CodeLabel(Datfile, ObjectID + 1, True) & "'"
            Else
                Return Tool.GetText(Datfilesname(Datfile)) & " '" & pjData.CodeLabel(Datfile, ObjectID, True) & "'"
            End If
        End Get
    End Property

    Public Sub NameRefresh()
        NotifyPropertyChanged("Name")
        NotifyPropertyChanged("TabName")
        '이때 발동
    End Sub

    Public Sub BackColorRefresh()
        NotifyPropertyChanged("TreeviewBack")
        NotifyPropertyChanged("Back")
        NotifyPropertyChanged("BackPageReq1")
        NotifyPropertyChanged("BackPageReq2")
        NotifyPropertyChanged("BackPageD")
        NotifyPropertyChanged("BackPage0")
        NotifyPropertyChanged("BackPage1")
        NotifyPropertyChanged("BackPage2")
        NotifyPropertyChanged("BackPage3")
        NotifyPropertyChanged("BackPage4")
        NotifyPropertyChanged("BackPage5")
        NotifyPropertyChanged("BackPage6")
        NotifyPropertyChanged("BackPage7")
    End Sub

    Public Sub ChangeProperty()
        NotifyPropertyChanged("TreeviewBack")
        NotifyPropertyChanged("Back")
        NotifyPropertyChanged("BackPageReq1")
        NotifyPropertyChanged("BackPageReq2")
        NotifyPropertyChanged("BackPageD")
        NotifyPropertyChanged("BackPage0")
        NotifyPropertyChanged("BackPage1")
        NotifyPropertyChanged("BackPage2")
        NotifyPropertyChanged("BackPage3")
        NotifyPropertyChanged("BackPage4")
        NotifyPropertyChanged("BackPage5")
        NotifyPropertyChanged("BackPage6")
        NotifyPropertyChanged("BackPage7")
    End Sub

    Public ReadOnly Property BackPageReq1() As SolidColorBrush
        Get
            If Datfile = SCDatFiles.DatFiles.upgrades Or Datfile = SCDatFiles.DatFiles.techdata Or Datfile = SCDatFiles.DatFiles.orders Then
                If pjData.DataManager.CheckDirtyPageReq(Datfile, ObjectID) Then
                    Return Application.Current.Resources("MaterialDesignPaper")
                Else
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property BackPageReq2() As SolidColorBrush
        Get
            If Datfile = SCDatFiles.DatFiles.techdata Then
                If pjData.DataManager.CheckDirtyPageReq(SCDatFiles.DatFiles.Stechdata, ObjectID) Then
                    Return Application.Current.Resources("MaterialDesignPaper")
                Else
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property BackPageD() As SolidColorBrush
        Get
            If Datfile = SCDatFiles.DatFiles.upgrades Or Datfile = SCDatFiles.DatFiles.techdata Or Datfile = SCDatFiles.DatFiles.orders Then
                If pjData.DataManager.CheckDirtyPageDefault(Datfile, ObjectID) Then
                    Return Application.Current.Resources("MaterialDesignPaper")
                Else
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property BackPage0() As SolidColorBrush
        Get
            If Datfile = SCDatFiles.DatFiles.units Then
                If pjData.DataManager.CheckDirtyPage(ObjectID, 0) Then
                    Return Application.Current.Resources("MaterialDesignPaper")
                Else
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property BackPage1() As SolidColorBrush
        Get
            If Datfile = SCDatFiles.DatFiles.units Then
                If pjData.DataManager.CheckDirtyPage(ObjectID, 1) Then
                    Return Application.Current.Resources("MaterialDesignPaper")
                Else
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property BackPage2() As SolidColorBrush
        Get
            If Datfile = SCDatFiles.DatFiles.units Then
                If pjData.DataManager.CheckDirtyPage(ObjectID, 2) Then
                    Return Application.Current.Resources("MaterialDesignPaper")
                Else
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property BackPage3() As SolidColorBrush
        Get
            If Datfile = SCDatFiles.DatFiles.units Then
                If pjData.DataManager.CheckDirtyPage(ObjectID, 3) Then
                    Return Application.Current.Resources("MaterialDesignPaper")
                Else
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property BackPage4() As SolidColorBrush
        Get
            If Datfile = SCDatFiles.DatFiles.units Then
                If pjData.DataManager.CheckDirtyPage(ObjectID, 4) Then
                    Return Application.Current.Resources("MaterialDesignPaper")
                Else
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property BackPage5() As SolidColorBrush
        Get
            If Datfile = SCDatFiles.DatFiles.units Then
                If pjData.DataManager.CheckDirtyPage(ObjectID, 5) Then
                    Return Application.Current.Resources("MaterialDesignPaper")
                Else
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property BackPage6() As SolidColorBrush
        Get
            If Datfile = SCDatFiles.DatFiles.units Then
                If pjData.DataManager.CheckDirtyPage(ObjectID, 6) Then
                    Return Application.Current.Resources("MaterialDesignPaper")
                Else
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property BackPage7() As SolidColorBrush
        Get
            If Datfile = SCDatFiles.DatFiles.units Then
                If pjData.DataManager.CheckDirtyPage(ObjectID, 7) Then
                    Return Application.Current.Resources("MaterialDesignPaper")
                Else
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            End If
            Return Nothing
        End Get
    End Property





    Public ReadOnly Property Back() As SolidColorBrush
        Get
            If SCDatFiles.CheckValidDat(Datfile) Then
                Dim TrueFlag As Boolean = pjData.DataManager.CheckDirtyObject(Datfile, ObjectID)



                If TrueFlag Then
                    If pjData.IsMapLoading Then
                        If TrueFlag Then
                            Return Application.Current.Resources("MaterialDesignPaper")
                        Else
                            Return New SolidColorBrush(pgData.FiledMapEditColor)
                        End If
                    Else
                        Return Application.Current.Resources("MaterialDesignPaper")
                    End If
                Else
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            Else
                Select Case Datfile
                    Case SCDatFiles.DatFiles.stattxt
                        If pjData.ExtraDat.Stat_txt(ObjectID) = ExtraDatFiles.StatNullString Then
                            Return Application.Current.Resources("MaterialDesignPaper")
                        Else
                            Return New SolidColorBrush(pgData.FiledEditColor)
                        End If
                    Case SCDatFiles.DatFiles.ButtonData
                        Dim TrueFlag As Boolean = pjData.DataManager.CheckDirtyObject(Datfile, ObjectID)
                        If TrueFlag Then
                            Return Application.Current.Resources("MaterialDesignPaper")
                        Else
                            Return New SolidColorBrush(pgData.FiledEditColor)
                        End If
                End Select

                Return Application.Current.Resources("MaterialDesignPaper")
            End If
        End Get
    End Property

    Public ReadOnly Property TreeviewBack() As SolidColorBrush
        Get
            If SCDatFiles.CheckValidDat(Datfile) Then
                Dim TrueFlag As Boolean = pjData.DataManager.CheckDirtyObject(Datfile, ObjectID)



                If TrueFlag Then
                    If pjData.IsMapLoading Then
                        If TrueFlag Then
                            Return Application.Current.Resources("MaterialDesignPaper")
                        Else
                            Return New SolidColorBrush(pgData.FiledMapEditColor)
                        End If
                    Else
                        Return Application.Current.Resources("MaterialDesignPaper")
                    End If
                Else
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            Else
                Select Case Datfile
                    Case SCDatFiles.DatFiles.stattxt
                        If pjData.ExtraDat.Stat_txt(ObjectID) = ExtraDatFiles.StatNullString Then
                            Return Application.Current.Resources("MaterialDesignPaper")
                        Else
                            Return New SolidColorBrush(pgData.FiledEditColor)
                        End If
                    Case SCDatFiles.DatFiles.ButtonData
                        Dim TrueFlag As Boolean = pjData.DataManager.CheckDirtyObject(Datfile, ObjectID)
                        If TrueFlag Then
                            Return Application.Current.Resources("MaterialDesignPaper")
                        Else
                            Return New SolidColorBrush(pgData.FiledEditColor)
                        End If
                End Select

                Return Application.Current.Resources("MaterialDesignPaper")
            End If
        End Get
    End Property

    Public Property ToolTip() As String
        Get
            If SCDatFiles.CheckValidDat(Datfile) Then
                Return pjData.Dat.ToolTip(Datfile, ObjectID)
            Else
                Return pjData.ExtraDat.ToolTip(Datfile, ObjectID)
            End If


        End Get

        Set(ByVal tvalue As String)
            If SCDatFiles.CheckValidDat(Datfile) Then
                If Not (tvalue = pjData.Dat.ToolTip(Datfile, ObjectID)) Then
                    pjData.Dat.ToolTip(Datfile, ObjectID) = tvalue
                    NotifyPropertyChanged("ToolTip")
                    NotifyPropertyChanged("Name")
                    NotifyPropertyChanged("TabName")
                End If
            Else
                If Not (tvalue = pjData.ExtraDat.ToolTip(Datfile, ObjectID)) Then
                    pjData.ExtraDat.ToolTip(Datfile, ObjectID) = tvalue
                    NotifyPropertyChanged("ToolTip")
                    NotifyPropertyChanged("Name")
                    NotifyPropertyChanged("TabName")
                End If
            End If

        End Set
    End Property

    Public Property Group() As String
        Get
            If SCDatFiles.CheckValidDat(Datfile) Then
                Return pjData.Dat.Group(Datfile, ObjectID)
            Else
                Return pjData.ExtraDat.Group(Datfile, ObjectID)
            End If

        End Get

        Set(ByVal tvalue As String)
            If SCDatFiles.CheckValidDat(Datfile) Then
                If Not (tvalue = pjData.Dat.Group(Datfile, ObjectID)) Then
                    pjData.Dat.Group(Datfile, ObjectID) = tvalue
                    pjData.BindingManager.RefreshCodeTree(Datfile, ObjectID)
                    NotifyPropertyChanged("Group")
                End If
            Else
                If Not (tvalue = pjData.ExtraDat.Group(Datfile, ObjectID)) Then
                    pjData.ExtraDat.Group(Datfile, ObjectID) = tvalue
                    pjData.BindingManager.RefreshCodeTree(Datfile, ObjectID)
                    NotifyPropertyChanged("Group")
                End If
            End If
        End Set
    End Property

    Public Sub ToolTipReset()
        If SCDatFiles.CheckValidDat(Datfile) Then
            pjData.Dat.ToolTip(Datfile, ObjectID) = scData.DefaultDat.ToolTip(Datfile, ObjectID)
        Else
            pjData.ExtraDat.ToolTipReset(Datfile, ObjectID)
        End If

        NotifyPropertyChanged("ToolTip")
        NotifyPropertyChanged("Name")
        NotifyPropertyChanged("TabName")
    End Sub
    Public Sub GroupReset()
        If SCDatFiles.CheckValidDat(Datfile) Then
            pjData.Dat.Group(Datfile, ObjectID) = scData.DefaultDat.Group(Datfile, ObjectID)
        Else
            pjData.ExtraDat.GroupReset(Datfile, ObjectID)
        End If

        NotifyPropertyChanged("Group")
    End Sub


    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
End Class
