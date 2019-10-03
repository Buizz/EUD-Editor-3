Imports System.ComponentModel

Public Class ExtraDatBinding
    Implements INotifyPropertyChanged

    '바인딩 할 때 본체도 넘겨주자고.
    '잘못된 값일 경우 또는 지원안하는 부분일 경우 없에버리기.~

    Private Datfile As SCDatFiles.DatFiles
    Private Parameter As String
    Private ObjectID As Integer

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged



    Public Sub New(tDatfile As SCDatFiles.DatFiles, tParameter As String, tObjectID As Integer)
        Dim ValueStart As Integer = pjData.Dat.ParamInfo(tDatfile, tParameter, SCDatFiles.EParamInfo.VarStart)
        Dim ValueSize As Byte = pjData.Dat.ParamInfo(tDatfile, tParameter, SCDatFiles.EParamInfo.Size)

        Datfile = tDatfile
        Parameter = tParameter
        ObjectID = tObjectID + ValueStart

    End Sub

    Public Sub BackColorRefresh()
        NotifyPropertyChanged("BackColor")

    End Sub


    Public Sub PPropertyChangedPack()
        PropertyChangedPack()
    End Sub

    Private Sub PropertyChangedPack()
        pjData.BindingManager.UIManager(Datfile, ObjectID).ChangeProperty()
        'NotifyPropertyChanged("HPValue")
        'NotifyPropertyChanged("Checked")
        NotifyPropertyChanged("Value")
        NotifyPropertyChanged("BackColor")
        NotifyPropertyChanged("ValueText")
        NotifyPropertyChanged("ValueImage")
        'NotifyPropertyChanged("ValueFlag")

        'NotifyPropertyChanged("ToolTipText")

    End Sub

    Public ReadOnly Property ToolTipText() As TextBlock
        Get
            Dim DefaultValue As Long = scData.DefaultDat.Data(Datfile, Parameter, ObjectID)


            Dim returnStr As String = ""
            returnStr = "<0>" & Tool.GetText("datfliename") & " : <2>" & Tool.GetText(Datfilesname(Datfile)) & "(" & Datfilesname(Datfile) & ")" & vbCrLf &
             "<0>" & Tool.GetText("parameter") & " : <2>" & Tool.GetText(Datfilesname(Datfile) & "_" & Parameter) & "(" & Parameter & ")" & vbCrLf
            'returnStr = returnStr & "인덱스 : " & ObjectID & vbCrLf
            'returnStr = returnStr & "사이즈 : " & scData.DefaultDat.ParamInfo(Datfile, Parameter, SCDatFiles.EParamInfo.Size) & vbCrLf

            Dim ValueType As SCDatFiles.DatFiles = scData.DefaultDat.ParamInfo(Datfile, Parameter, SCDatFiles.EParamInfo.ValueType)
            If ValueType <> SCDatFiles.DatFiles.None Then
                returnStr = returnStr & "<0>" & Tool.GetText("valuetype") & " : <3>" & Tool.GetText(Datfilesname(ValueType)) & vbCrLf
            End If
            'returnStr = returnStr & "베이스오프셋 : 0x" & Hex(Tool.GetOffset(Datfile, Parameter)).ToUpper & vbCrLf

            If Not pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault Then
                returnStr = returnStr & "<0>" & Tool.GetText("orgvalue") & " : <3>" & DefaultValue & vbCrLf
            End If

            If pjData.IsMapLoading Then
                If Not pjData.MapData.DatFile.Values(Datfile, Parameter, ObjectID).IsDefault Then '기본 안 값 쓴다면
                    returnStr = returnStr & "<0>" & Tool.GetText("mapdatavalue") & " : <3>" & pjData.MapData.DatFile.Data(Datfile, Parameter, ObjectID) & vbCrLf
                End If
            End If

            'If Not pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault Then
            '    returnStr = returnStr & "   변경 된 값 : " & pjData.Dat.Data(Datfile, Parameter, ObjectID)
            'End If

            returnStr = returnStr & vbCrLf & "<0>" & Tool.GetText(Datfilesname(Datfile) & "_" & Parameter & "_ToolTip")

            Dim Tb As TextBlock = Tool.TextColorBlock(returnStr.Trim)
            Tb.TextWrapping = TextWrapping.WrapWithOverflow
            Tb.MaxWidth = 250

            Return Tb
        End Get
    End Property

    Public ReadOnly Property ValueText() As String
        Get
            Select Case Datfile
                Case SCDatFiles.DatFiles.wireframe
                    Dim valueType As SCDatFiles.DatFiles = SCDatFiles.DatFiles.wireframe

                    Dim Value As Long
                    If Parameter = "wire" Then
                        Value = pjData.ExtraDat.WireFrame(ObjectID)
                        If Value >= SCUnitCount Then
                            Return Tool.GetText("None")
                        End If
                    ElseIf Parameter = "grp" Then
                        Value = pjData.ExtraDat.GrpFrame(ObjectID)
                        If Value >= SCGrpWireCount Then
                            Return Tool.GetText("None")
                        End If
                    ElseIf Parameter = "tran" Then
                        Value = pjData.ExtraDat.TranFrame(ObjectID)
                        If Value >= SCMenCount Then
                            Return Tool.GetText("None")
                        End If
                    End If
                    Return pjData.CodeLabel(valueType, Value, True)
                Case SCDatFiles.DatFiles.ButtonSet
                    Dim valueType As SCDatFiles.DatFiles = SCDatFiles.DatFiles.ButtonData

                    Dim Value As Long
                    Value = pjData.ExtraDat.ButtonSet(ObjectID)
                    If Value >= SCButtonCount Then
                        Return Tool.GetText("None")
                    End If

                    Return pjData.CodeLabel(valueType, Value, True)
            End Select
            Return Nothing
        End Get
    End Property


    Public ReadOnly Property ValueImage() As ImageSource
        Get
            Select Case Datfile
                Case SCDatFiles.DatFiles.wireframe
                    Dim Value As Long
                    If Parameter = "wire" Then
                        Value = pjData.ExtraDat.WireFrame(ObjectID)
                        If Value >= SCUnitCount Then
                            Return scData.GetWireFrame(0)
                        End If
                        Return scData.GetWireFrame(Value)
                    ElseIf Parameter = "grp" Then
                        Value = pjData.ExtraDat.GrpFrame(ObjectID)
                        If Value >= SCGrpWireCount Then
                            Return scData.GetWireFrame(0)
                        End If
                        Return scData.GetGrpFrame(Value)
                    ElseIf Parameter = "tran" Then
                        Value = pjData.ExtraDat.TranFrame(ObjectID)
                        If Value >= SCMenCount Then
                            Return scData.GetWireFrame(0)
                        End If
                        Return scData.GetTranFrame(Value)
                    End If
                    Return scData.GetWireFrame(0)
                Case SCDatFiles.DatFiles.ButtonSet
                    Value = pjData.ExtraDat.ButtonSet(ObjectID)
                    If Value < SCUnitCount Then
                        Return scData.GetIcon(Value)
                    Else
                        Return scData.GetIcon(0)
                    End If
            End Select
            Return Nothing
        End Get
    End Property

    Public Property Value() As String
        Get
            Select Case Datfile
                Case SCDatFiles.DatFiles.statusinfor
                    If Parameter = "Status" Then
                        Return pjData.ExtraDat.StatusFunction1(ObjectID)
                    ElseIf Parameter = "Display" Then
                        Return pjData.ExtraDat.StatusFunction2(ObjectID)
                    ElseIf Parameter = "Joint" Then
                        Dim Status As Byte = pjData.ExtraDat.StatusFunction1(ObjectID)
                        Dim Display As Byte = pjData.ExtraDat.StatusFunction2(ObjectID)

                        For i = 0 To scData.StatusCodeCount - 1
                            If scData.StatusCode(i)(0) = Status And scData.StatusCode(i)(1) = Display Then
                                Return i
                            End If
                        Next


                        Return -1
                    End If
                Case SCDatFiles.DatFiles.wireframe
                    If Parameter = "wire" Then
                        Return pjData.ExtraDat.WireFrame(ObjectID)
                    ElseIf Parameter = "grp" Then
                        Return pjData.ExtraDat.GrpFrame(ObjectID)
                    ElseIf Parameter = "tran" Then
                        Return pjData.ExtraDat.TranFrame(ObjectID)
                    End If
                Case SCDatFiles.DatFiles.ButtonSet
                    Return pjData.ExtraDat.ButtonSet(ObjectID)
                Case SCDatFiles.DatFiles.ButtonData
                    Return pjData.ExtraDat.ButtonData.GetButtonSet(ObjectID).IsDefault
            End Select


            Return 0
        End Get

        Set(ByVal tvalue As String)
            Dim OldValue As Long
            Select Case Datfile
                Case SCDatFiles.DatFiles.statusinfor
                    If Parameter = "Status" Then
                        OldValue = pjData.ExtraDat.StatusFunction1(ObjectID)
                    ElseIf Parameter = "Display" Then
                        OldValue = pjData.ExtraDat.StatusFunction2(ObjectID)
                    ElseIf Parameter = "Joint" Then
                        OldValue = tvalue + 1
                    End If
                Case SCDatFiles.DatFiles.wireframe
                    If Parameter = "wire" Then
                        OldValue = pjData.ExtraDat.WireFrame(ObjectID)
                    ElseIf Parameter = "grp" Then
                        OldValue = pjData.ExtraDat.GrpFrame(ObjectID)
                    ElseIf Parameter = "tran" Then
                        OldValue = pjData.ExtraDat.TranFrame(ObjectID)
                    End If
                Case SCDatFiles.DatFiles.ButtonSet
                    OldValue = pjData.ExtraDat.ButtonSet(ObjectID)
                Case SCDatFiles.DatFiles.ButtonData
                    OldValue = pjData.ExtraDat.ButtonData.GetButtonSet(ObjectID).IsDefault

                    If tvalue = "True" Then
                        tvalue = 1
                    Else
                        tvalue = 0
                    End If
            End Select

            If tvalue <> OldValue Then
                pjData.SetDirty(True)

                Try
                    Select Case Datfile
                        Case SCDatFiles.DatFiles.statusinfor
                            If Parameter = "Status" Then
                                pjData.ExtraDat.DefaultStatusFunction1(ObjectID) = False
                                pjData.ExtraDat.StatusFunction1(ObjectID) = tvalue
                                pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Joint", ObjectID).PPropertyChangedPack()
                            ElseIf Parameter = "Display" Then
                                pjData.ExtraDat.DefaultStatusFunction2(ObjectID) = False
                                pjData.ExtraDat.StatusFunction2(ObjectID) = tvalue
                                pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Joint", ObjectID).PPropertyChangedPack()
                            ElseIf Parameter = "Joint" Then
                                If tvalue >= 0 And tvalue <= scData.StatusCodeCount Then
                                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Status", ObjectID).Value = scData.StatusCode(tvalue)(0)
                                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Display", ObjectID).Value = scData.StatusCode(tvalue)(1)
                                End If
                            End If
                        Case SCDatFiles.DatFiles.wireframe
                            If Parameter = "wire" Then
                                pjData.ExtraDat.DefaultWireFrame(ObjectID) = False
                                pjData.ExtraDat.WireFrame(ObjectID) = tvalue
                            ElseIf Parameter = "grp" Then
                                pjData.ExtraDat.DefaultGrpFrame(ObjectID) = False
                                pjData.ExtraDat.GrpFrame(ObjectID) = tvalue
                            ElseIf Parameter = "tran" Then
                                pjData.ExtraDat.DefaultTranFrame(ObjectID) = False
                                pjData.ExtraDat.TranFrame(ObjectID) = tvalue
                            End If
                        Case SCDatFiles.DatFiles.ButtonSet
                            pjData.ExtraDat.DefaultButtonSet(ObjectID) = False
                            pjData.ExtraDat.ButtonSet(ObjectID) = tvalue
                        Case SCDatFiles.DatFiles.ButtonData
                            pjData.ExtraDat.ButtonData.GetButtonSet(ObjectID).IsDefault = tvalue
                    End Select
                Catch ex As Exception

                End Try


                PropertyChangedPack()
            End If
        End Set
    End Property


    Public ReadOnly Property BackColor As SolidColorBrush
        Get
            Dim IsDefault As Boolean

            Select Case Datfile
                Case SCDatFiles.DatFiles.statusinfor
                    If Parameter = "Status" Then
                        IsDefault = pjData.ExtraDat.DefaultStatusFunction1(ObjectID)
                    ElseIf Parameter = "Display" Then
                        IsDefault = pjData.ExtraDat.DefaultStatusFunction2(ObjectID)
                    ElseIf Parameter = "Joint" Then
                        IsDefault = pjData.ExtraDat.DefaultStatusFunction1(ObjectID) And pjData.ExtraDat.DefaultStatusFunction2(ObjectID)
                    End If
                Case SCDatFiles.DatFiles.wireframe
                    If Parameter = "wire" Then
                        IsDefault = pjData.ExtraDat.DefaultWireFrame(ObjectID)
                    ElseIf Parameter = "grp" Then
                        IsDefault = pjData.ExtraDat.DefaultGrpFrame(ObjectID)
                    ElseIf Parameter = "tran" Then
                        IsDefault = pjData.ExtraDat.DefaultTranFrame(ObjectID)
                    End If
                Case SCDatFiles.DatFiles.ButtonSet
                    IsDefault = pjData.ExtraDat.DefaultButtonSet(ObjectID)
            End Select



            '맵에서 정의된 것일 경우.

            If IsDefault Then
                'MsgBox("회색")
                Return New SolidColorBrush(pgData.FiledDefault)
            Else

                'MsgBox("빨간색")
                Return New SolidColorBrush(pgData.FiledEditColor)
            End If


            Return New SolidColorBrush(pgData.FiledMapEditColor)
        End Get
    End Property

    Public Sub DataReset()
        pjData.SetDirty(True)
        Select Case Datfile
            Case SCDatFiles.DatFiles.statusinfor
                If Parameter = "Status" Then
                    pjData.ExtraDat.DefaultStatusFunction1(ObjectID) = True
                    pjData.ExtraDat.StatusFunction1(ObjectID) = scData.DefaultExtraDat.StatusFunction1(ObjectID)
                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Joint", ObjectID).PropertyChangedPack()
                ElseIf Parameter = "Display" Then
                    pjData.ExtraDat.DefaultStatusFunction2(ObjectID) = True
                    pjData.ExtraDat.StatusFunction2(ObjectID) = scData.DefaultExtraDat.StatusFunction2(ObjectID)
                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Joint", ObjectID).PropertyChangedPack()
                ElseIf Parameter = "Joint" Then
                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Status", ObjectID).DataReset()
                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Display", ObjectID).DataReset()
                End If
            Case SCDatFiles.DatFiles.wireframe
                If Parameter = "wire" Then
                    pjData.ExtraDat.DefaultWireFrame(ObjectID) = True
                    pjData.ExtraDat.WireFrame(ObjectID) = scData.DefaultExtraDat.WireFrame(ObjectID)
                ElseIf Parameter = "grp" Then
                    pjData.ExtraDat.DefaultGrpFrame(ObjectID) = True
                    pjData.ExtraDat.GrpFrame(ObjectID) = scData.DefaultExtraDat.GrpFrame(ObjectID)
                ElseIf Parameter = "tran" Then
                    pjData.ExtraDat.DefaultTranFrame(ObjectID) = True
                    pjData.ExtraDat.TranFrame(ObjectID) = scData.DefaultExtraDat.TranFrame(ObjectID)
                End If
            Case SCDatFiles.DatFiles.ButtonSet
                pjData.ExtraDat.DefaultButtonSet(ObjectID) = True
                pjData.ExtraDat.ButtonSet(ObjectID) = scData.DefaultExtraDat.ButtonSet(ObjectID)
            Case SCDatFiles.DatFiles.ButtonData
                pjData.ExtraDat.ButtonData.GetButtonSet(ObjectID).IsDefault = True
        End Select



        PropertyChangedPack()
    End Sub


    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
End Class

