Imports System.IO

<Serializable>
Public Class CButtonSets
    Private ButtonSets() As CButtonSet

    Public ReadOnly Property GetButtonSet(index As Integer) As CButtonSet
        Get
            Return ButtonSets(index)
        End Get
    End Property

    Public Sub New(ButtSetData As String)
        Dim fs As New FileStream(ButtSetData, FileMode.Open)
        Dim br As New BinaryReader(fs)

        Dim btncount As UInteger
        Dim btnadress As UInteger


        ReDim ButtonSets(SCButtonCount - 1)
        'ReDim Btnoffset(SCButtonCount - 1)

        'ReDim ProjectBtnData(btnobjectnum - 1)
        'ReDim ProjectBtnUSE(btnobjectnum - 1)

        For i = 0 To SCButtonCount - 1
            btncount = br.ReadUInt32() '버튼 수
            btnadress = br.ReadUInt32() '버튼 주소

            ButtonSets(i) = New CButtonSet(btnadress, i)


            '    Btnoffset(j) = btnadress

            '    BtnData(j) = New List(Of SBtnDATA)
            '    ProjectBtnData(j) = New List(Of SBtnDATA)
            For j = 0 To btncount - 1
                Dim TBtn As New CButtonData
                TBtn.pos = br.ReadUInt16() 'pos
                TBtn.icon = br.ReadUInt16() 'icon
                TBtn.con = br.ReadUInt32() 'con
                TBtn.act = br.ReadUInt32() 'act
                TBtn.conval = br.ReadUInt16() 'conval
                TBtn.actval = br.ReadUInt16() 'actval
                TBtn.enaStr = br.ReadUInt16() 'enaStr
                TBtn.disStr = br.ReadUInt16() 'disStr

                ButtonSets(i).ButtonS.Add(TBtn)
            Next

        Next

        br.Close()
        fs.Close()


    End Sub


End Class

<Serializable>
Public Class CButtonSet
    Private ObjectID As Integer
    Private pButtonSets As List(Of CButtonData)
    Private _DefaultAddress As UInteger
    Public Property DefaultAddress As UInteger
        Get
            Return _DefaultAddress
        End Get
        Set(value As UInteger)
            _DefaultAddress = value
        End Set
    End Property


    Private _DefaultUse As Boolean

    Public Enum BType
        DefaultCommand
        MovingablebuildingCommand
        BurrowCommand
        GatherCommand
        TransportCommand
        EmptyButton
        UnitTraning
        UnitMorph
        UpgradeResearch
        TechResearch
        TechUse
        BuildingMorph
        BuildingBuild_Morph
        BuildingBuild_Terran
        BuildingBuild_Protoss
        BuildingBuild_Addon
    End Enum
    '기본 커맨드
    '이동 가능한 건물 커맨드
    '버러우 커맨드
    '채취자 커맨드
    '운송수단 커맨드
    '빈 버튼
    '유닛 훈련
    '유닛 훈련 - 변태
    '업글 연구
    '기술 연구
    '기술 사용
    '건물 변태
    '건물 건설 - 변태
    '건물 건설 - 테란
    '건물 건설 - 프로토스
    '건물 건설 - 애드온
    Public Function GetBytesArrayString() As String
        Dim returnstr As String = ""

        pButtonSets.Sort(Function(x As CButtonData, y As CButtonData)
                             Return x.pos.CompareTo(y.pos)
                         End Function)

        For i = 0 To pButtonSets.Count - 1
            If i = 0 Then
                returnstr = pButtonSets(i).GetBtnBytesArrayText()
            Else
                returnstr = returnstr & "," & pButtonSets(i).GetBtnBytesArrayText()
            End If

        Next
        Return returnstr
    End Function

    Public Sub AddButtonType(tBType As BType, Pos As Integer, Value As Integer)
        Select Case tBType
            Case CButtonSet.BType.DefaultCommand
                NewButton(1, 228, 4358864, 4342848, 0, 0, 664, 0)
                NewButton(2, 229, 4358864, 4338672, 0, 0, 665, 0)
                NewButton(3, 230, 4362032, 4342656, 0, 0, 666, 0)
                NewButton(4, 254, 4358864, 4342080, 0, 0, 667, 0)
                NewButton(5, 255, 4358864, 4338544, 0, 0, 668, 0)
            Case CButtonSet.BType.MovingablebuildingCommand
                NewButton(1, 228, 4359200, 4342848, 0, 0, 664, 0)
                NewButton(2, 229, 4359200, 4338672, 0, 0, 665, 0)
                NewButton(9, 283, 4359152, 4340784, 0, 106, 670, 0)
                NewButton(9, 282, 4360144, 4338224, 0, 0, 671, 0)
            Case CButtonSet.BType.BurrowCommand
                NewButton(9, 259, 4362480, 4338352, 11, 11, 372, 382)
                NewButton(9, 260, 4362352, 4338320, 11, 0, 373, 0)
            Case CButtonSet.BType.GatherCommand
                NewButton(5, 231, 4359344, 4340592, 0, 0, 675, 0)
                NewButton(6, 233, 4359296, 4339552, 0, 0, 676, 0)
            Case CButtonSet.BType.TransportCommand
                NewButton(8, 309, 4362224, 4340544, 0, 0, 683, 0)
                NewButton(9, 312, 4361888, 4340480, 0, 0, 684, 0)
            Case CButtonSet.BType.EmptyButton
                NewButton(Pos + 1, 0, 4358864, 4342848, 0, 0, 1, 1)
            Case CButtonSet.BType.UnitTraning
                NewButton(Pos + 1, Value, 4361824, 4338864, Value, Value, 1, 1)
            Case CButtonSet.BType.UnitMorph
                NewButton(Pos + 1, Value, 4361824, 4339600, Value, Value, 1, 1)
            Case CButtonSet.BType.UpgradeResearch
                Dim IconIndex As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.upgrades, "Icon", Value)

                NewButton(Pos + 1, IconIndex, 4363344, 4338448, Value, Value, 1, 1)
            Case CButtonSet.BType.TechResearch
                Dim IconIndex As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.techdata, "Icon", Value)

                NewButton(Pos + 1, IconIndex, 4363520, 4338512, Value, Value, 1, 1)
            Case CButtonSet.BType.TechUse
                Dim IconIndex As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.techdata, "Icon", Value)

                NewButton(Pos + 1, IconIndex, 4363488, 4341616, Value, Value, 1, 1)
            Case CButtonSet.BType.BuildingMorph
                NewButton(Pos + 1, Value, 4361824, 4339808, Value, Value, 1, 1)
            Case CButtonSet.BType.BuildingBuild_Morph
                NewButton(Pos + 1, Value, 4361824, 4340816, Value, Value, 1, 1)
            Case CButtonSet.BType.BuildingBuild_Terran
                NewButton(Pos + 1, Value, 4361824, 4341424, Value, Value, 1, 1)
            Case CButtonSet.BType.BuildingBuild_Protoss
                NewButton(Pos + 1, Value, 4361824, 4341200, Value, Value, 1, 1)
            Case CButtonSet.BType.BuildingBuild_Addon
                NewButton(Pos + 1, Value, 4361824, 4341008, Value, Value, 1, 1)
        End Select
    End Sub
    Public Function ButtonTypeDatFile(tBType As BType) As SCDatFiles.DatFiles
        Select Case tBType
            Case BType.UnitTraning, BType.UnitMorph, BType.BuildingBuild_Addon, BType.BuildingBuild_Morph, BType.BuildingBuild_Protoss, BType.BuildingBuild_Terran, BType.BuildingMorph
                Return SCDatFiles.DatFiles.units
            Case BType.UpgradeResearch
                Return SCDatFiles.DatFiles.upgrades
            Case BType.TechResearch, BType.TechUse
                Return SCDatFiles.DatFiles.techdata
            Case Else
                Return SCDatFiles.DatFiles.None
        End Select

    End Function

    Public ReadOnly Property ButtonS As List(Of CButtonData)
        Get
            Return pButtonSets
        End Get
    End Property

    Public Sub New(tDefaultAddress As UInteger, tObjectID As Integer)
        ObjectID = tObjectID

        pButtonSets = New List(Of CButtonData)

        _DefaultAddress = tDefaultAddress
        _DefaultUse = True
    End Sub

    Public Function GetCopyString() As String
        Dim returnStr As String = ""

        For i = 0 To pButtonSets.Count - 1
            If i = 0 Then
                returnStr = pButtonSets(i).GetBtnText()
            Else
                returnStr = returnStr & "." & pButtonSets(i).GetBtnText()
            End If
        Next

        Return returnStr
    End Function

    Public Sub PasteFromString(value As String)
        Dim btnsetStr() As String = value.Split(".")

        For i = 0 To btnsetStr.Count - 1
            AddBtnFromStr(btnsetStr(i))
        Next
    End Sub

    Public Function Pasteable() As Boolean
        Dim PasteText As String = My.Computer.Clipboard.GetText

        Try
            Dim valueText() As String = PasteText.Split(",")

            For i = 0 To 7
                If Not IsNumeric(valueText(i)) Then
                    Return False
                End If
            Next


            Return True
        Catch ex As Exception
        End Try
        Return False
    End Function

    Public Sub PasteBtn()
        Dim PasteText As String = My.Computer.Clipboard.GetText
        AddBtnFromStr(PasteText)
    End Sub
    Private Sub AddBtnFromStr(str As String)
        Dim valueText() As String = str.Split(",")

        Dim TBtn As New CButtonData

        TBtn.pos = valueText(0)
        TBtn.icon = valueText(1)
        TBtn.con = valueText(2)
        TBtn.act = valueText(3)
        TBtn.conval = valueText(4)
        TBtn.actval = valueText(5)
        TBtn.enaStr = valueText(6)
        TBtn.disStr = valueText(7)

        pButtonSets.Add(TBtn)
    End Sub


    Public Sub NewButton(tpos As UShort, ticon As UShort, tcon As UInteger, tact As UInteger, tconval As UShort, tactval As UShort, tenastr As UShort, tdisstr As UShort)

        Dim TBtn As New CButtonData

        TBtn.pos = tpos
        TBtn.icon = ticon
        TBtn.con = tcon
        TBtn.act = tact
        TBtn.conval = tconval
        TBtn.actval = tactval
        TBtn.enaStr = tenastr
        TBtn.disStr = tdisstr

        pButtonSets.Add(TBtn)
    End Sub


    Public Property IsDefault As Boolean
        Get
            Return _DefaultUse
        End Get
        Set(value As Boolean)
            If value And Not _DefaultUse Then
                ButtonReset()
            End If
            _DefaultUse = value
        End Set
    End Property

    Private Sub ButtonReset()
        pButtonSets.Clear()

        Dim buttonsets As List(Of CButtonData) = scData.DefaultExtraDat.ButtonData.GetButtonSet(ObjectID).ButtonS
        For i = 0 To buttonsets.Count - 1
            pButtonSets.Add(buttonsets(i).Clone)
        Next
    End Sub
End Class


<Serializable>
Public Class CButtonData
    Private _pos As Integer
    Private _icon As Integer
    Private _con As UInteger
    Private _act As UInteger
    Private _conval As Integer
    Private _actval As Integer
    Private _enaStr As Integer
    Private _disStr As Integer

    Public Sub OverWriteData()
        Dim PasteText As String = My.Computer.Clipboard.GetText

        Dim valueText() As String = PasteText.Split(",")

        pos = valueText(0)
        icon = valueText(1)
        con = valueText(2)
        act = valueText(3)
        conval = valueText(4)
        actval = valueText(5)
        enaStr = valueText(6)
        disStr = valueText(7)
    End Sub
    Public Sub CopyData()
        Dim copyText As String

        copyText = GetBtnText()



        My.Computer.Clipboard.SetText(copyText)
    End Sub
    Public Function GetBtnText() As String
        Return _pos & "," & _icon & "," & _con & "," & _act & "," & _conval & "," & _actval & "," & _enaStr & "," & _disStr
    End Function
    Public Function GetBtnBytesArrayText() As String
        Return ByteSpliter(_pos, 2) & "," & ByteSpliter(_icon, 2) & "," & ByteSpliter(_con, 4) & "," & ByteSpliter(_act, 4) & "," &
               ByteSpliter(_conval, 2) & "," & ByteSpliter(_actval, 2) & "," & ByteSpliter(_enaStr, 2) & "," & ByteSpliter(_disStr, 2)
    End Function
    Private Function ByteSpliter(num As Integer, Spliter As Byte) As String
        Select Case Spliter
            Case 4
                Return (num And &HFF) & "," & (num And &HFF00) \ &HFF & "," & (num And &HFF0000) \ &HFF00 & "," & (num And &HFF000000) \ &HFF0000
            Case 2
                Return (num And &HFF) & "," & (num And &HFF00) \ &HFF
            Case 1
                Return num
        End Select
        Return num
    End Function



    Public Function Clone() As CButtonData
        Dim returnData As New CButtonData
        returnData.pos = _pos
        returnData.icon = _icon
        returnData.con = _con
        returnData.act = _act
        returnData.conval = _conval
        returnData.actval = _actval
        returnData.enaStr = _enaStr
        returnData.disStr = _disStr



        Return returnData
    End Function
    Public Function GetEnaStr() As String
        Return pjData.CodeLabel(SCDatFiles.DatFiles.stattxt, enaStr - 1)
    End Function
    Public Function GetDisStr() As String
        Return pjData.CodeLabel(SCDatFiles.DatFiles.stattxt, disStr - 1)
    End Function


    Public Function GetPos() As Point
        Dim returnPoint As New Point(((pos - 1) Mod 3) * 32, ((pos - 1) \ 3) * 32)

        Return returnPoint
    End Function
    Public Function GetIcon() As BitmapImage
        Return scData.GetIcon(icon, False)
    End Function
    Public Function GetListBoxItem() As ListBoxItem
        Dim returnItem As New ListBoxItem

        returnItem.Content = GetListBoxContent()

        returnItem.Tag = Me

        Return returnItem
    End Function
    Public Function GetListBoxContent() As DockPanel
        Dim dockpanel As New DockPanel


        Dim tImage As New Image
        Dim tborder As New Border
        Dim textblock As New TextBlock

        textblock.Text = pjData.CodeLabel(SCDatFiles.DatFiles.Icon, icon)
        tImage.Source = scData.GetIcon(icon, False)

        tborder.Background = Brushes.Black
        tborder.Width = 20
        tborder.Height = 20

        textblock.Margin = New Thickness(5, 0, 0, 0)
        tborder.Child = tImage
        dockpanel.Children.Add(tborder)
        dockpanel.Children.Add(textblock)


        Return dockpanel
    End Function

    Public Property pos As Integer
        Get
            Return _pos
        End Get
        Set(value As Integer)
            _pos = value
        End Set
    End Property

    Public Property icon As Integer
        Get
            Return _icon
        End Get
        Set(value As Integer)
            _icon = value
        End Set
    End Property
    Public Property con As UInteger
        Get
            Return _con
        End Get
        Set(value As UInteger)
            _con = value
        End Set
    End Property
    Public Property act As UInteger
        Get
            Return _act
        End Get
        Set(value As UInteger)
            _act = value
        End Set
    End Property
    Public Property conval As Integer
        Get
            Return _conval
        End Get
        Set(value As Integer)
            _conval = value
        End Set
    End Property
    Public Property actval As Integer
        Get
            Return _actval
        End Get
        Set(value As Integer)
            _actval = value
        End Set
    End Property
    Public Property enaStr As Integer
        Get
            Return _enaStr
        End Get
        Set(value As Integer)
            _enaStr = value
        End Set
    End Property
    Public Property disStr As Integer
        Get
            Return _disStr
        End Get
        Set(value As Integer)
            _disStr = value
        End Set
    End Property
End Class
