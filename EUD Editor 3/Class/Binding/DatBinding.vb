Imports System.ComponentModel

Public Class DatBinding
    Implements INotifyPropertyChanged

    '바인딩 할 때 본체도 넘겨주자고.
    '잘못된 값일 경우 또는 지원안하는 부분일 경우 없에버리기.~

    Private Datfile As SCDatFiles.DatFiles
    Private Parameter As String
    Private ObjectID As Integer

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged


    Private FlagBindingManager As List(Of FlagBinding)
    Public ReadOnly Property GetFlagBinding(index As Integer) As FlagBinding
        Get
            Return FlagBindingManager(index)
        End Get
    End Property

    Public Sub New(tDatfile As SCDatFiles.DatFiles, tParameter As String, tObjectID As Integer)
        Dim ValueStart As Integer = pjData.Dat.ParamInfo(tDatfile, tParameter, SCDatFiles.EParamInfo.VarStart)
        Dim ValueSize As Byte = pjData.Dat.ParamInfo(tDatfile, tParameter, SCDatFiles.EParamInfo.Size)

        Datfile = tDatfile
        Parameter = tParameter
        ObjectID = tObjectID + ValueStart

        FlagBindingManager = New List(Of FlagBinding)
        For i = 0 To ValueSize * 8 - 1
            FlagBindingManager.Add(New FlagBinding(Me, Datfile, Parameter, ObjectID, i))
        Next
    End Sub

    Public Sub BackColorRefresh()
        NotifyPropertyChanged("BackColor")
        For i = 0 To FlagBindingManager.Count - 1
            FlagBindingManager(i).PropertyChangedPack()
        Next
    End Sub

    Private Sub PropertyChangedPack()
        pjData.BindingManager.UIManager(Datfile, ObjectID).ChangeProperty()
        NotifyPropertyChanged("HPValue")
        NotifyPropertyChanged("Checked")
        NotifyPropertyChanged("Value")
        NotifyPropertyChanged("BackColor")
        NotifyPropertyChanged("ValueText")
        'NotifyPropertyChanged("ValueTextBinding")
        NotifyPropertyChanged("ValueImage")
        NotifyPropertyChanged("ValueFlag")

        NotifyPropertyChanged("ToolTipText")
        For i = 0 To FlagBindingManager.Count - 1
            FlagBindingManager(i).PropertyChangedPack()
        Next
    End Sub

    Public ReadOnly Property ToolTipText() As TextBlock
        Get
            Dim DefaultValue As Long = scData.DefaultDat.Data(Datfile, Parameter, ObjectID)


            Dim returnStr As String = ""
            returnStr = "<0>뎃파일 이름 : <2>" & Tool.GetText(Datfilesname(Datfile)) & "(" & Datfilesname(Datfile) & ")" & vbCrLf &
             "<0>파라미터 : <2>" & Tool.GetText(Datfilesname(Datfile) & "_" & Parameter) & "(" & Parameter & ")" & vbCrLf
            'returnStr = returnStr & "인덱스 : " & ObjectID & vbCrLf
            'returnStr = returnStr & "사이즈 : " & scData.DefaultDat.ParamInfo(Datfile, Parameter, SCDatFiles.EParamInfo.Size) & vbCrLf

            Dim ValueType As SCDatFiles.DatFiles = scData.DefaultDat.ParamInfo(Datfile, Parameter, SCDatFiles.EParamInfo.ValueType)
            If ValueType <> SCDatFiles.DatFiles.None Then
                returnStr = returnStr & "<0>값 타입 : <3>" & Tool.GetText(Datfilesname(ValueType)) & vbCrLf
            End If
            'returnStr = returnStr & "베이스오프셋 : 0x" & Hex(Tool.GetOffset(Datfile, Parameter)).ToUpper & vbCrLf

            If Not pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault Then
                returnStr = returnStr & "<0>원본 값 : <3>" & DefaultValue & vbCrLf
            End If

            If pjData.IsMapLoading Then
                If Not pjData.MapData.DatFile.Values(Datfile, Parameter, ObjectID).IsDefault Then '기본 안 값 쓴다면
                    returnStr = returnStr & "<0>맵 데이터 값 : <3>" & pjData.MapData.DatFile.Data(Datfile, Parameter, ObjectID) & vbCrLf
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



    Public Property Value() As String
        Get
            'MsgBox("데이터 파인딩 겟")
            '만약 맵데이터에 있는 항목이라면? 
            If pjData.IsMapLoading Then
                'MsgBox(pjData.MapData.DatFile.Data(Datfile, Parameter, ObjectID) & vbCrLf &
                'pjData.Dat.Data(Datfile, Parameter, ObjectID))
                If pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault And Not pjData.MapData.DatFile.Values(Datfile, Parameter, ObjectID).IsDefault Then '기본 안 값 쓴다면
                    Return pjData.MapData.DatFile.Data(Datfile, Parameter, ObjectID)
                End If
            End If


            Return pjData.Dat.Data(Datfile, Parameter, ObjectID)
        End Get

        Set(ByVal tvalue As String)
            If Not (tvalue = pjData.Dat.Data(Datfile, Parameter, ObjectID)) Then
                pjData.SetDirty(True)
                'MsgBox("데이터 파인딩 셋")
                Dim tData As Long = pjData.Dat.Data(Datfile, Parameter, ObjectID)

                pjData.Dat.Data(Datfile, Parameter, ObjectID) = tvalue
                pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault = False
                Try
                    pjData.BindingManager.RefreshCodeUseData(Datfile, Parameter, tData)
                    pjData.BindingManager.RefreshCodeUseData(Datfile, Parameter, tvalue)
                Catch ex As Exception

                End Try


                '만약 요주의 데이터들일 경우. Ex라벨에 관련된것.
                If Parameter = "Label" Then
                    pjData.BindingManager.UIManager(Datfile, ObjectID).NameRefresh()
                End If


                PropertyChangedPack()
            End If
        End Set
    End Property

    Public Property ValueFlag() As String
        Get
            'MsgBox("데이터 파인딩 겟")
            '만약 맵데이터에 있는 항목이라면? 
            If pjData.IsMapLoading Then
                'MsgBox(pjData.MapData.DatFile.Data(Datfile, Parameter, ObjectID) & vbCrLf &
                'pjData.Dat.Data(Datfile, Parameter, ObjectID))
                If pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault And Not pjData.MapData.DatFile.Values(Datfile, Parameter, ObjectID).IsDefault Then '기본 안 값 쓴다면
                    Return Hex(pjData.MapData.DatFile.Data(Datfile, Parameter, ObjectID))
                End If
            End If


            Return Hex(pjData.Dat.Data(Datfile, Parameter, ObjectID))
        End Get

        Set(ByVal tvalue As String)
            tvalue = "&H" & tvalue
            If Not (tvalue = pjData.Dat.Data(Datfile, Parameter, ObjectID)) Then
                'MsgBox("데이터 파인딩 셋")
                pjData.Dat.Data(Datfile, Parameter, ObjectID) = tvalue
                pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault = False
                PropertyChangedPack()
            End If
        End Set
    End Property

    Public Property HPValue() As String
        Get
            Dim returnVal As Long
            'MsgBox("데이터 파인딩 겟")
            '만약 맵데이터에 있는 항목이라면? 
            If pjData.IsMapLoading Then
                'MsgBox(pjData.MapData.DatFile.Data(Datfile, Parameter, ObjectID) & vbCrLf &
                'pjData.Dat.Data(Datfile, Parameter, ObjectID))
                If pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault And Not pjData.MapData.DatFile.Values(Datfile, Parameter, ObjectID).IsDefault Then '기본 안 값 쓴다면
                    returnVal = pjData.MapData.DatFile.Data(Datfile, Parameter, ObjectID)
                Else
                    returnVal = pjData.Dat.Data(Datfile, Parameter, ObjectID)
                End If
            Else
                returnVal = pjData.Dat.Data(Datfile, Parameter, ObjectID)
            End If



            If returnVal > 2147483647 Then '음수일 경우
                '2147483648 = -2147483648
                '4294967295 = -1
                returnVal -= 4294967296
            End If


            Return Math.Floor(returnVal / 256)
        End Get

        Set(ByVal tvalue As String)
            If Not (tvalue = pjData.Dat.Data(Datfile, Parameter, ObjectID)) Then
                Dim Setvalue As Long = tvalue * 256

                If Setvalue > Integer.MaxValue Then
                    Setvalue = Integer.MaxValue
                End If
                If Setvalue < Integer.MinValue Then
                    Setvalue = Integer.MinValue
                End If
                'MsgBox("데이터 파인딩 셋")
                If Setvalue < 0 Then '음수일 경우
                    '2147483648 = -2147483648
                    '4294967295 = -1
                    Setvalue += 4294967296
                End If


                pjData.Dat.Data(Datfile, Parameter, ObjectID) = Setvalue
                pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault = False
                PropertyChangedPack()
            End If
        End Set
    End Property



    Public Property Checked() As Boolean
        Get
            'MsgBox("데이터 파인딩 겟")
            '만약 맵데이터에 있는 항목이라면? 
            If pjData.IsMapLoading Then
                'MsgBox(pjData.MapData.DatFile.Data(Datfile, Parameter, ObjectID) & vbCrLf &
                'pjData.Dat.Data(Datfile, Parameter, ObjectID))
                If pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault And Not pjData.MapData.DatFile.Values(Datfile, Parameter, ObjectID).IsDefault Then '기본 안 값 쓴다면
                    Return pjData.MapData.DatFile.Data(Datfile, Parameter, ObjectID)
                End If
            End If




            Return pjData.Dat.Data(Datfile, Parameter, ObjectID)
        End Get

        Set(ByVal tvalue As Boolean)
            If Not (tvalue = pjData.Dat.Data(Datfile, Parameter, ObjectID)) Then
                'MsgBox("데이터 파인딩 셋")
                If tvalue Then
                    pjData.Dat.Data(Datfile, Parameter, ObjectID) = 1
                Else
                    pjData.Dat.Data(Datfile, Parameter, ObjectID) = 0
                End If
                pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault = False
                PropertyChangedPack()
            End If
        End Set
    End Property

    Public ReadOnly Property ValueText As String
        Get
            Dim valueType As SCDatFiles.DatFiles = pjData.Dat.ParamInfo(Datfile, Parameter, SCDatFiles.EParamInfo.ValueType)
            If valueType <> SCDatFiles.DatFiles.None Then
                Dim Value As Long = pjData.Dat.Data(Datfile, Parameter, ObjectID)
                If SCCodeCount(valueType) > Value Then
                    Return pjData.CodeLabel(valueType, Value, True)
                Else
                    Return Tool.GetText("None")
                End If
            Else
                Return Tool.GetText("None")
            End If
        End Get
    End Property
    'Public ReadOnly Property ValueTextBinding As UIManager
    '    Get
    '        'MsgBox("ㅆ:빋")
    '        Dim valueType As SCDatFiles.DatFiles = pjData.Dat.ParamInfo(Datfile, Parameter, SCDatFiles.EParamInfo.ValueType)
    '        If valueType <> SCDatFiles.DatFiles.None Then
    '            Dim Value As Long = pjData.Dat.Data(Datfile, Parameter, ObjectID)

    '            If SCCodeCount(valueType) > Value Then
    '                Return pjData.BindingManager.UIManager(valueType, Value)
    '            Else
    '                Return Nothing
    '            End If
    '        Else
    '            Return Nothing
    '        End If
    '    End Get
    'End Property


    Public ReadOnly Property ValueImage As ImageSource
        Get
            Dim isIcon As Boolean = False
            Dim ImageIndex As Integer

            Dim valueType As SCDatFiles.DatFiles = pjData.Dat.ParamInfo(Datfile, Parameter, SCDatFiles.EParamInfo.ValueType)
            Dim value As Long = pjData.Dat.Data(Datfile, Parameter, ObjectID)

            Select Case valueType
                Case SCDatFiles.DatFiles.units
                    Dim tGraphics As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Graphics", value)
                    Dim tSprite As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", tGraphics)
                    Dim timage As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", tSprite)

                    ImageIndex = timage
                Case SCDatFiles.DatFiles.weapons
                    Dim tIcon As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.weapons, "Icon", value)

                    isIcon = True
                    ImageIndex = tIcon
                Case SCDatFiles.DatFiles.flingy
                    Dim tSprite As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", value)
                    Dim timage As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", tSprite)

                    ImageIndex = timage
                Case SCDatFiles.DatFiles.sprites
                    Dim timage As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", value)

                    ImageIndex = timage
                Case SCDatFiles.DatFiles.images
                    ImageIndex = value
                Case SCDatFiles.DatFiles.upgrades
                    Dim tIcon As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.upgrades, "Icon", value)

                    isIcon = True
                    ImageIndex = tIcon
                Case SCDatFiles.DatFiles.techdata
                    Dim tIcon As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.techdata, "Icon", value)

                    isIcon = True
                    ImageIndex = tIcon
                Case SCDatFiles.DatFiles.orders
                    Dim tIcon As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.orders, "Highlight", value)

                    isIcon = True
                    ImageIndex = tIcon
                Case SCDatFiles.DatFiles.Icon
                    isIcon = True
                    ImageIndex = value
            End Select
            If isIcon Then
                Return scData.GetIcon(ImageIndex, False)
            Else
                Return scData.GetGRP(ImageIndex, 12, False)
            End If
        End Get
    End Property

    Public ReadOnly Property Explain As String
        Get
            Return Tool.GetText(Datfilesname(Datfile) & "_" & Parameter)
        End Get
    End Property

    Public ReadOnly Property ComboxItems As String()
        Get
            Return Tool.GetText(Datfilesname(Datfile) & "_" & Parameter & "_V").Split("|")
        End Get
    End Property


    Public ReadOnly Property BackColor As SolidColorBrush
        Get
            Dim tvalue As Long = pjData.Dat.Data(Datfile, Parameter, ObjectID)
            Dim TrueValue As Long = scData.DefaultDat.Data(Datfile, Parameter, ObjectID)
            Dim IsDefault As Boolean = pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault
            Dim IsMapDataDefault As Boolean = True

            If pjData.IsMapLoading Then
                IsMapDataDefault = pjData.MapData.DatFile.Values(Datfile, Parameter, ObjectID).IsDefault
            End If


            '맵에서 정의된 것일 경우.

            If IsDefault And Not IsMapDataDefault Then '만약 맵 데이터가 존재 할 경우
                Return New SolidColorBrush(pgData.FiledMapEditColor)
            Else
                If IsDefault Then

                    'MsgBox("회색")
                    Return New SolidColorBrush(pgData.FiledDefault)
                Else

                    'MsgBox("빨간색")
                    Return New SolidColorBrush(pgData.FiledEditColor)
                End If
            End If
            Return New SolidColorBrush(pgData.FiledMapEditColor)
        End Get
    End Property

    'MaterialDesignBody

    Public Sub DataReset()
        pjData.SetDirty(True)

        pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault = True
        pjData.Dat.Data(Datfile, Parameter, ObjectID) = scData.DefaultDat.Data(Datfile, Parameter, ObjectID)

        PropertyChangedPack()
    End Sub




    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Class FlagBinding
        Implements INotifyPropertyChanged

        Private ParrentBinding As DatBinding

        Private Datfile As SCDatFiles.DatFiles
        Private Parameter As String
        Private ObjectID As Integer
        Private FlagIndex As Integer

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Public Sub New(tParrentBinding As DatBinding, tDatfile As SCDatFiles.DatFiles, tParameter As String, tObjectID As Integer, tFlagIndex As Integer)
            ParrentBinding = tParrentBinding

            Datfile = tDatfile
            Parameter = tParameter
            ObjectID = tObjectID
            FlagIndex = tFlagIndex
        End Sub


        Public Property MiniFlag As Boolean
            Get
                Dim value As Long = pjData.Dat.Data(Datfile, Parameter, ObjectID)
                Dim flag As Boolean = (value And Math.Pow(2, FlagIndex))


                Return flag
            End Get

            Set(ByVal tvalue As Boolean)
                Dim value As Long = pjData.Dat.Data(Datfile, Parameter, ObjectID)

                '원래 값
                Dim flag As Boolean = (value And Math.Pow(2, FlagIndex))

                If tvalue <> flag Then '값이 변했음
                    If tvalue Then
                        Dim CValue As Long = value + Math.Pow(2, FlagIndex)
                        pjData.Dat.Data(Datfile, Parameter, ObjectID) = CValue
                    Else
                        Dim CValue As Long = value - Math.Pow(2, FlagIndex)
                        pjData.Dat.Data(Datfile, Parameter, ObjectID) = CValue
                    End If

                    '1111 1001
                    '1111 1101

                    '1111 1001
                    '0000 0100


                    '1111 1101
                    '1111 1001

                    '1111 1011

                    pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault = False

                    ParrentBinding.PropertyChangedPack()
                    PropertyChangedPack()
                End If




                'If Not (tvalue = pjData.Dat.Data(Datfile, Parameter, ObjectID)) Then
                '    'MsgBox("데이터 파인딩 셋")
                '    pjData.Dat.Data(Datfile, Parameter, ObjectID) = tvalue
                '    pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault = False

                'End If
            End Set
        End Property

        Public ReadOnly Property MiniBackColor As SolidColorBrush
            Get
                Dim value As Long = pjData.Dat.Data(Datfile, Parameter, ObjectID)
                '현재 값
                Dim flag As Boolean = (value And Math.Pow(2, FlagIndex))

                Dim Truevalue As Long = scData.DefaultDat.Data(Datfile, Parameter, ObjectID)
                '실제 값
                Dim Trueflag As Boolean = (Truevalue And Math.Pow(2, FlagIndex))


                Dim IsDefault As Boolean = pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault



                If Not IsDefault Then
                    If (flag <> Trueflag) Then '수정된 값일 경우
                        Return New SolidColorBrush(pgData.FiledFalgColor)
                    Else
                        If flag Then
                            Return New SolidColorBrush(pgData.FiledMapEditColor)
                        Else
                            Return New SolidColorBrush(Color.FromArgb(0, 0, 0, 0))
                        End If
                    End If
                End If


                'New SolidColorBrush(pgData.FiledEditColor)
                If flag Then
                    Return New SolidColorBrush(pgData.FiledMapEditColor)
                Else
                    Return New SolidColorBrush(pgData.FiledDefault)
                End If



                'If IsDefault And Not IsMapDataDefault Then '만약 맵 데이터가 존재 할 경우
                '    Return New SolidColorBrush(pgData.FiledMapEditColor)
                'Else
                '    If IsDefault Then

                '        'MsgBox("회색")
                '        Return New SolidColorBrush(pgData.FiledDefault)
                '    Else

                '        'MsgBox("빨간색")
                '        Return New SolidColorBrush(pgData.FiledEditColor)
                '    End If
                'End If
                'Return New SolidColorBrush(pgData.FiledMapEditColor)
            End Get
        End Property



        Public Sub PropertyChangedPack()
            NotifyPropertyChanged("MiniFlag")
            NotifyPropertyChanged("MiniBackColor")
        End Sub


        Private Sub NotifyPropertyChanged(ByVal info As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
        End Sub
    End Class
End Class

