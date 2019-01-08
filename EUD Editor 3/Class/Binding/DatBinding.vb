Imports System.ComponentModel

Public Class DatBinding
    Implements INotifyPropertyChanged

    '바인딩 할 때 본체도 넘겨주자고.
    '잘못된 값일 경우 또는 지원안하는 부분일 경우 없에버리기.~

    Private Datfile As SCDatFiles.DatFiles
    Private Parameter As String
    Private ObjectID As Integer

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged


    Public Sub New(tDatfile As SCDatFiles.DatFiles, tParameter As String, tObjectID As Integer)
        Datfile = tDatfile
        Parameter = tParameter
        ObjectID = tObjectID
    End Sub

    Private Sub PropertyChangedPack()
        NotifyPropertyChanged("HPValue")
        NotifyPropertyChanged("Checked")
        NotifyPropertyChanged("Value")
        NotifyPropertyChanged("BackColor")
        NotifyPropertyChanged("ValueText")
    End Sub

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
                Return pjData.CodeLabel(valueType, pjData.Dat.Data(Datfile, Parameter, ObjectID))
            Else
                Return "None"
            End If
        End Get
    End Property


    Public ReadOnly Property Image As ImageSource
        Get
            Return scData.GetGRP(0, 37, False)
        End Get
    End Property


    Public ReadOnly Property Explain As String
        Get
            Return Tool.GetText(Datfilesname(Datfile) & "_" & Parameter)
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
                Return New SolidColorBrush(Color.FromArgb(140, 107, 102, 255))
            Else
                If IsDefault Then

                    'MsgBox("회색")
                    Return New SolidColorBrush(Color.FromArgb(40, 213, 213, 213))
                Else

                    'MsgBox("빨간색")
                    Return New SolidColorBrush(Color.FromArgb(140, 255, 178, 217))
                End If
            End If
            Return New SolidColorBrush(Color.FromArgb(140, 107, 102, 255))
        End Get
    End Property
    'MaterialDesignBody

    Public Sub DataReset()
        pjData.Dat.Values(Datfile, Parameter, ObjectID).IsDefault = True
        pjData.Dat.Data(Datfile, Parameter, ObjectID) = scData.DefaultDat.Data(Datfile, Parameter, ObjectID)

        PropertyChangedPack()
    End Sub




    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
End Class
