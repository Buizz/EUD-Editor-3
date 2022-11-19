Public Class UnitData
        Public Property ObjectID As Integer

    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, tObjectID As Integer)
        ObjectID = tObjectID

        UsedCodeList.ReLoad(SCDatFiles.DatFiles.units, ObjectID)


        For i = 0 To LoadSatus.Count - 1
            LoadSatus(i) = False
        Next

        LoasScreen(MainTab.SelectedIndex, ObjectID)

        TypeListBox.DataContext = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.units, ObjectID)
    End Sub

    Public Sub New(tObjectID As Integer)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        UsedCodeList.Init(SCDatFiles.DatFiles.units, tObjectID)
        ObjectID = tObjectID

        ReDim LoadSatus(7)
        LoasScreen(0, ObjectID)


        TypeListBox.DataContext = pjData.BindingManager.UIManager(SCDatFiles.DatFiles.units, tObjectID)
    End Sub

    Private Unit_Default As Unit_Default
    Private Unit_Special As Unit_Special
    Private Unit_Sound As Unit_Sound
    Private Unit_Graphic As Unit_Graphic
    Private Unit_MapEdit As Unit_MapEdit
    Private Unit_AIOrder As Unit_AIOrder
    Private StatusInfoData As StatusInfoData
    Private RequireData As RequireData
    Private LoadSatus() As Boolean

    Private Sub LoasScreen(index As Byte, tObjectID As Integer)
        If Not LoadSatus(index) Then
            Select Case index
                Case 0
                    If Unit_Default Is Nothing Then
                        Unit_Default = New Unit_Default(tObjectID)
                        Default.Content = Unit_Default
                    Else
                        Unit_Default.ReLoad(SCDatFiles.DatFiles.units, tObjectID)
                    End If
                Case 1
                    If Unit_Special Is Nothing Then
                        Unit_Special = New Unit_Special(tObjectID)
                        Special.Content = Unit_Special
                    Else
                        Unit_Special.ReLoad(SCDatFiles.DatFiles.units, tObjectID)
                    End If
                Case 2
                    If Unit_Sound Is Nothing Then
                        Unit_Sound = New Unit_Sound(tObjectID)
                        Sound.Content = Unit_Sound
                    Else
                        Unit_Sound.ReLoad(SCDatFiles.DatFiles.units, tObjectID)
                    End If
                Case 3
                    If Unit_Graphic Is Nothing Then
                        Unit_Graphic = New Unit_Graphic(tObjectID)
                        Graphic.Content = Unit_Graphic
                    Else
                        Unit_Graphic.ReLoad(SCDatFiles.DatFiles.units, tObjectID)
                    End If
                Case 4
                    If Unit_MapEdit Is Nothing Then
                        Unit_MapEdit = New Unit_MapEdit(tObjectID)
                        MapEdit.Content = Unit_MapEdit
                    Else
                        Unit_MapEdit.ReLoad(SCDatFiles.DatFiles.units, tObjectID)
                    End If
                Case 5
                    If Unit_AIOrder Is Nothing Then
                        Unit_AIOrder = New Unit_AIOrder(tObjectID)
                        AIOrder.Content = Unit_AIOrder
                    Else
                        Unit_AIOrder.ReLoad(SCDatFiles.DatFiles.units, tObjectID)
                    End If
                Case 6
                    If StatusInfoData Is Nothing Then
                        StatusInfoData = New StatusInfoData(tObjectID)
                        SInfoData.Content = StatusInfoData
                    Else
                        StatusInfoData.ReLoad(SCDatFiles.DatFiles.statusinfor, ObjectID)
                    End If
                Case 7
                    If RequireData Is Nothing Then
                        RequireData = New RequireData(SCDatFiles.DatFiles.units, tObjectID)
                        Requir.Content = RequireData
                    Else
                        RequireData.ReLoad(SCDatFiles.DatFiles.units, ObjectID)
                    End If
            End Select
            LoadSatus(index) = True
        End If
    End Sub


    Private Sub ListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        'Dim tTabitem As TabItem = MainTab.Items(TypeListBox.SelectedIndex)
        LoasScreen(TypeListBox.SelectedIndex, ObjectID)
        MainTab.SelectedIndex = TypeListBox.SelectedIndex
    End Sub
End Class
