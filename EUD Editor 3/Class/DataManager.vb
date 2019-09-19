Imports Newtonsoft.Json

Public Class DataManager
    Private Const ValueSpliter As String = "ᚏ"

    Public Shared Function GetGroups(Datfile As SCDatFiles.DatFiles, index As Integer) As String
        If SCDatFiles.CheckValidDat(Datfile) Then
            Return pjData.Dat.Group(Datfile, index)
        Else
            Return pjData.ExtraDat.Group(Datfile, index)
        End If
    End Function


    Public Function CheckDirtyObject(Datfile As SCDatFiles.DatFiles, ObjectID As Integer) As Boolean
        Dim TrueFlag As Boolean
        Select Case Datfile
            Case SCDatFiles.DatFiles.units
                TrueFlag = pjData.Dat.GetDatFile(Datfile).CheckDirty(ObjectID) And pjData.ExtraDat.CheckDirty(SCDatFiles.DatFiles.statusinfor, ObjectID) And pjData.ExtraDat.CheckDirty(SCDatFiles.DatFiles.wireframe, ObjectID) And pjData.ExtraDat.CheckDirty(SCDatFiles.DatFiles.ButtonSet, ObjectID) And (pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.units).RequireObjectUsed(ObjectID) = CRequireData.RequireUse.DefaultUse)

            Case SCDatFiles.DatFiles.upgrades
                TrueFlag = pjData.Dat.GetDatFile(Datfile).CheckDirty(ObjectID) And (pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.upgrades).RequireObjectUsed(ObjectID) = CRequireData.RequireUse.DefaultUse)
            Case SCDatFiles.DatFiles.techdata
                TrueFlag = pjData.Dat.GetDatFile(Datfile).CheckDirty(ObjectID) And (pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.techdata).RequireObjectUsed(ObjectID) = CRequireData.RequireUse.DefaultUse) And (pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.Stechdata).RequireObjectUsed(ObjectID) = CRequireData.RequireUse.DefaultUse)
            Case SCDatFiles.DatFiles.orders
                TrueFlag = pjData.Dat.GetDatFile(Datfile).CheckDirty(ObjectID) And (pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.orders).RequireObjectUsed(ObjectID) = CRequireData.RequireUse.DefaultUse)
            Case SCDatFiles.DatFiles.ButtonData, SCDatFiles.DatFiles.stattxt
                TrueFlag = pjData.ExtraDat.CheckDirty(Datfile, ObjectID)
            Case Else
                TrueFlag = pjData.Dat.GetDatFile(Datfile).CheckDirty(ObjectID)
        End Select

        Return TrueFlag
    End Function


    'UnitDat의 경우 페이지 별 담당하는 파라미터들이 다름. 그걸 정의해야됨
    Private PageMask() As List(Of String)
    Public Sub New()
        ReDim PageMask(10)
        PageMask(0) = New List(Of String)
        PageMask(0).AddRange({"Hit Points", "Shield Amount", "Shield Enable", "Armor Upgrade",
                             "Armor", "Mineral Cost", "Vespene Cost", "Build Time", "Broodwar Unit Flag",
                             "Build Score", "Destroy Score", "Ground Weapon", "Max Ground Hits", "Air Weapon",
                             "Max Air Hits", "Supply Required", "Supply Provided", "Space Required",
                             "Space Provided", "Sight Range", "Target Acquisition Range", "Unit Size"})

        PageMask(1) = New List(Of String)
        PageMask(1).AddRange({"Special Ability Flags", "Infestation", "Subunit 1", "Subunit 2", "Unknown (old Movement)"})

        PageMask(2) = New List(Of String)
        PageMask(2).AddRange({"Ready Sound", "What Sound Start", "What Sound End", "Piss Sound Start", "Piss Sound End", "Yes Sound Start", "Yes Sound End"})

        PageMask(3) = New List(Of String)
        PageMask(3).AddRange({"Graphics", "Construction Animation", "Portrait", "Elevation Level", "Unit Direction", "Unit Size Left", "Unit Size Right", "Unit Size Up", "Unit Size Down", "StarEdit Placement Box Width", "StarEdit Placement Box Height", "Addon Horizontal (X) Position", "Addon Vertical (Y) Position"})

        PageMask(4) = New List(Of String)
        PageMask(4).AddRange({"Staredit Availability Flags", "Staredit Group Flags", "Rank/Sublabel"})

        PageMask(5) = New List(Of String)
        PageMask(5).AddRange({"Comp AI Idle", "Human AI Idle", "Return to Idle", "Attack Unit", "Attack Move", "Right-click Action", "AI Internal"})
    End Sub

    ' Upgrade, Tech, Order Default
    Public Function CheckDirtyPageReq(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer) As Boolean
        Return (pjData.ExtraDat.RequireData(DatFiles).RequireObjectUsed(ObjectID) = CRequireData.RequireUse.DefaultUse)
    End Function
    Public Function CheckDirtyPageDefault(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer) As Boolean
        Return pjData.Dat.GetDatFile(DatFiles).CheckDirty(ObjectID)
    End Function
    Public Function CheckDirtyPage(ObjectID As Integer, UnitDatPage As Integer) As Boolean

        Dim DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.units
        Select Case UnitDatPage
            Case 6
                Return pjData.ExtraDat.CheckDirty(SCDatFiles.DatFiles.statusinfor, ObjectID) And pjData.ExtraDat.CheckDirty(SCDatFiles.DatFiles.wireframe, ObjectID) And pjData.ExtraDat.CheckDirty(SCDatFiles.DatFiles.ButtonSet, ObjectID)
            Case 7
                Return (pjData.ExtraDat.RequireData(DatFiles).RequireObjectUsed(ObjectID) = CRequireData.RequireUse.DefaultUse)
            Case Else
                For i = 0 To pjData.Dat.GetDatFile(DatFiles).ParamaterList.Count - 1
                    Dim Paramname As String = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetParamname
                    If PageMask(UnitDatPage).IndexOf(Paramname) < 0 Then
                        Continue For
                    End If

                    If pjData.BindingManager.DatBinding(DatFiles, Paramname, ObjectID) Is Nothing Then
                        Continue For
                    End If

                    If Not pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetValue(ObjectID).IsDefault Then
                        Return False
                    End If
                Next
        End Select

        Return True
    End Function
    '복사 붙여넣기 등등을 담당.
    '바인딩을 이용하는게 특징.
    Public Sub CopyDatPage(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        CopyDatObject(DatFiles, ObjectID)
    End Sub

    Public Sub CopyDatObject(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        Dim Template As New Template(Datfilesname(DatFiles), ObjectID)
        If SCDatFiles.CheckValidDat(DatFiles) Then
            For i = 0 To pjData.Dat.GetDatFile(DatFiles).ParamaterList.Count - 1
                Dim Paramname As String = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetParamname

                If pjData.BindingManager.DatBinding(DatFiles, Paramname, ObjectID) Is Nothing Then
                    Continue For
                End If

                Dim ValueCount As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarCount)
                Dim ValueStart As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarStart)
                Dim ValueEnd As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarEnd)


                Dim Value As Long = pjData.Dat.Data(DatFiles, Paramname, ObjectID)
                Template.Values.Add(Paramname & ValueSpliter & Value)
            Next
            Select Case DatFiles
                Case SCDatFiles.DatFiles.units
                    Template.Values.Add("Status" & ValueSpliter & pjData.ExtraDat.StatusFunction1(ObjectID))
                    Template.Values.Add("Display" & ValueSpliter & pjData.ExtraDat.StatusFunction2(ObjectID))
                    Template.Values.Add("wire" & ValueSpliter & pjData.ExtraDat.WireFrame(ObjectID))
                    Template.Values.Add("grp" & ValueSpliter & pjData.ExtraDat.GrpFrame(ObjectID))
                    Template.Values.Add("tran" & ValueSpliter & pjData.ExtraDat.TranFrame(ObjectID))
                    Template.Values.Add("ButtonSet" & ValueSpliter & pjData.ExtraDat.ButtonSet(ObjectID))
            End Select
            Select Case DatFiles
                Case SCDatFiles.DatFiles.units, SCDatFiles.DatFiles.upgrades, SCDatFiles.DatFiles.orders
                    Template.Values.Add("RequireData" & ValueSpliter & pjData.ExtraDat.RequireData(DatFiles).GetCopyString(ObjectID))
                Case SCDatFiles.DatFiles.techdata
                    Template.Values.Add("RequireData1" & ValueSpliter & pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.techdata).GetCopyString(ObjectID))
                    Template.Values.Add("RequireData2" & ValueSpliter & pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.Stechdata).GetCopyString(ObjectID))
            End Select
        Else
            Select Case DatFiles
                Case SCDatFiles.DatFiles.stattxt
                    Template.Values.Add("Value" & ValueSpliter & pjData.Stat_txt(ObjectID))
                Case SCDatFiles.DatFiles.ButtonData
                    Template.Values.Add("Value" & ValueSpliter & pjData.ExtraDat.ButtonData.GetButtonSet(ObjectID).GetCopyString)
            End Select
        End If


        Dim tempStr As String = JsonConvert.SerializeObject(Template)

        My.Computer.Clipboard.SetText(tempStr)
    End Sub

    Public Sub PasteDatPage(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer, UnitDatPage As Integer)
        Try
            Select Case DatFiles
                Case SCDatFiles.DatFiles.units
                    Select Case UnitDatPage
                        Case 6 '와이어 프레임 등
                            If DataPagePasteAble(DatFiles, ObjectID) Then
                                Dim tTemplate As Template = JsonConvert.DeserializeObject(Of Template)((My.Computer.Clipboard.GetText))


                                For i = 0 To tTemplate.Values.Count - 1
                                    Dim valueStrs As String() = tTemplate.Values(i).Split(ValueSpliter)
                                    Dim ParamaterName As String = valueStrs(0)
                                    Dim Value As String = valueStrs(1)
                                    If pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, ParamaterName, ObjectID) IsNot Nothing Then
                                        pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, ParamaterName, ObjectID).Value = Value
                                    End If
                                    If pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.wireframe, ParamaterName, ObjectID) IsNot Nothing Then
                                        pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.wireframe, ParamaterName, ObjectID).Value = Value
                                    End If
                                    If pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.ButtonSet, ParamaterName, ObjectID) IsNot Nothing Then
                                        pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.ButtonSet, ParamaterName, ObjectID).Value = Value
                                    End If
                                Next
                            End If
                        Case 7 '요구사항 등
                            If DataPagePasteAble(DatFiles, ObjectID) Then
                                Dim tTemplate As Template = JsonConvert.DeserializeObject(Of Template)((My.Computer.Clipboard.GetText))


                                For i = 0 To tTemplate.Values.Count - 1
                                    Dim valueStrs As String() = tTemplate.Values(i).Split(ValueSpliter)
                                    Dim ParamaterName As String = valueStrs(0)
                                    Dim Value As String = valueStrs(1)
                                    If ParamaterName = "RequireData" Then
                                        pjData.ExtraDat.RequireData(DatFiles).PasteCopyData(ObjectID, Value)
                                        pjData.BindingManager.RequireCapacityBinding(DatFiles).PropertyChangedPack()
                                    End If
                                Next
                            End If

                        Case Else
                            If DataPagePasteAble(DatFiles, ObjectID) Then
                                Dim tTemplate As Template = JsonConvert.DeserializeObject(Of Template)((My.Computer.Clipboard.GetText))


                                For i = 0 To tTemplate.Values.Count - 1
                                    Dim valueStrs As String() = tTemplate.Values(i).Split(ValueSpliter)
                                    Dim ParamaterName As String = valueStrs(0)
                                    Dim Value As String = valueStrs(1)

                                    If PageMask(UnitDatPage).IndexOf(ParamaterName) >= 0 Then
                                        If pjData.BindingManager.DatBinding(DatFiles, ParamaterName, ObjectID) IsNot Nothing Then
                                            pjData.BindingManager.DatBinding(DatFiles, ParamaterName, ObjectID).Value = Value
                                        End If
                                    End If
                                Next
                            End If
                    End Select
                Case SCDatFiles.DatFiles.upgrades, SCDatFiles.DatFiles.techdata, SCDatFiles.DatFiles.orders
                    Select Case UnitDatPage
                        Case 7
                            If DataPagePasteAble(DatFiles, ObjectID) Then
                                Dim tTemplate As Template = JsonConvert.DeserializeObject(Of Template)((My.Computer.Clipboard.GetText))

                                For i = 0 To tTemplate.Values.Count - 1
                                    Dim valueStrs As String() = tTemplate.Values(i).Split(ValueSpliter)
                                    Dim ParamaterName As String = valueStrs(0)
                                    Dim Value As String = valueStrs(1)
                                    If ParamaterName = "RequireData" Or ParamaterName = "RequireData1" Then
                                        pjData.ExtraDat.RequireData(DatFiles).PasteCopyData(ObjectID, Value)
                                        pjData.BindingManager.RequireCapacityBinding(DatFiles).PropertyChangedPack()
                                    End If
                                Next
                            End If
                        Case 8
                            If DataPagePasteAble(DatFiles, ObjectID) Then
                                Dim tTemplate As Template = JsonConvert.DeserializeObject(Of Template)((My.Computer.Clipboard.GetText))


                                For i = 0 To tTemplate.Values.Count - 1
                                    Dim valueStrs As String() = tTemplate.Values(i).Split(ValueSpliter)
                                    Dim ParamaterName As String = valueStrs(0)
                                    Dim Value As String = valueStrs(1)
                                    If ParamaterName = "RequireData2" Then
                                        pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.Stechdata).PasteCopyData(ObjectID, Value)
                                        pjData.BindingManager.RequireCapacityBinding(SCDatFiles.DatFiles.Stechdata).PropertyChangedPack()
                                    End If
                                Next
                            End If
                        Case Else
                            If DataPagePasteAble(DatFiles, ObjectID) Then
                                Dim tTemplate As Template = JsonConvert.DeserializeObject(Of Template)((My.Computer.Clipboard.GetText))


                                For i = 0 To tTemplate.Values.Count - 1
                                    Dim valueStrs As String() = tTemplate.Values(i).Split(ValueSpliter)
                                    Dim ParamaterName As String = valueStrs(0)
                                    Dim Value As String = valueStrs(1)

                                    If pjData.BindingManager.DatBinding(DatFiles, ParamaterName, ObjectID) IsNot Nothing Then
                                        If pjData.Dat.ParamInfo(DatFiles, ParamaterName, SCDatFiles.EParamInfo.IsEnabled) Then
                                            pjData.BindingManager.DatBinding(DatFiles, ParamaterName, ObjectID).Value = Value
                                        End If
                                    End If
                                Next
                            End If
                    End Select
                Case Else
                    PasteDatObject(DatFiles, ObjectID)
            End Select
        Catch ex As Exception

        End Try


    End Sub
    Public Sub PasteDatObject(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        If DataObjectPasteAble(DatFiles, ObjectID) Then
            Dim tTemplate As Template = JsonConvert.DeserializeObject(Of Template)((My.Computer.Clipboard.GetText))
            Try
                If SCDatFiles.CheckValidDat(DatFiles) Then
                    For i = 0 To tTemplate.Values.Count - 1
                        Dim valueStrs As String() = tTemplate.Values(i).Split(ValueSpliter)
                        Dim ParamaterName As String = valueStrs(0)
                        Dim Value As String = valueStrs(1)


                        Select Case DatFiles
                            Case SCDatFiles.DatFiles.units
                                If ParamaterName = "Status" Then
                                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, ParamaterName, ObjectID).Value = Value
                                ElseIf ParamaterName = "Display" Then
                                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, ParamaterName, ObjectID).Value = Value
                                ElseIf ParamaterName = "wire" Then
                                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.wireframe, ParamaterName, ObjectID).Value = Value
                                ElseIf ParamaterName = "grp" Then
                                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.wireframe, ParamaterName, ObjectID).Value = Value
                                ElseIf ParamaterName = "tran" Then
                                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.wireframe, ParamaterName, ObjectID).Value = Value
                                ElseIf ParamaterName = "ButtonSet" Then
                                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.ButtonSet, ParamaterName, ObjectID).Value = Value
                                Else
                                    If pjData.BindingManager.DatBinding(DatFiles, ParamaterName, ObjectID) IsNot Nothing Then
                                        If pjData.Dat.ParamInfo(DatFiles, ParamaterName, SCDatFiles.EParamInfo.IsEnabled) Then
                                            pjData.BindingManager.DatBinding(DatFiles, ParamaterName, ObjectID).Value = Value
                                        End If
                                    End If
                                End If
                            Case Else
                                If pjData.BindingManager.DatBinding(DatFiles, ParamaterName, ObjectID) IsNot Nothing Then
                                    If pjData.Dat.ParamInfo(DatFiles, ParamaterName, SCDatFiles.EParamInfo.IsEnabled) Then
                                        pjData.BindingManager.DatBinding(DatFiles, ParamaterName, ObjectID).Value = Value
                                    End If
                                End If
                        End Select


                        Select Case DatFiles
                            Case SCDatFiles.DatFiles.units, SCDatFiles.DatFiles.upgrades, SCDatFiles.DatFiles.orders
                                If ParamaterName = "RequireData" Then
                                    pjData.ExtraDat.RequireData(DatFiles).PasteCopyData(ObjectID, Value)
                                    pjData.BindingManager.RequireCapacityBinding(DatFiles).PropertyChangedPack()
                                End If


                            'Template.Values.Add("RequireData" & ValueSpliter & pjData.ExtraDat.RequireData(DatFiles).GetCopyString(ObjectID))
                            Case SCDatFiles.DatFiles.techdata
                                If ParamaterName = "RequireData1" Then
                                    pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.techdata).PasteCopyData(ObjectID, Value)
                                    pjData.BindingManager.RequireCapacityBinding(SCDatFiles.DatFiles.techdata).PropertyChangedPack()
                                End If
                                If ParamaterName = "RequireData2" Then
                                    pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.Stechdata).PasteCopyData(ObjectID, Value)
                                    pjData.BindingManager.RequireCapacityBinding(SCDatFiles.DatFiles.Stechdata).PropertyChangedPack()
                                End If
                                'Template.Values.Add("RequireData1" & ValueSpliter & pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.techdata).GetCopyString(ObjectID))
                                'Template.Values.Add("RequireData2" & ValueSpliter & pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.Stechdata).GetCopyString(ObjectID))
                        End Select
                    Next
                Else
                    Select Case DatFiles
                        Case SCDatFiles.DatFiles.stattxt
                            For i = 0 To tTemplate.Values.Count - 1
                                Dim valueStrs As String() = tTemplate.Values(i).Split(ValueSpliter)
                                Dim ParamaterName As String = valueStrs(0)
                                Dim Value As String = valueStrs(1)
                                If ParamaterName = "Value" Then
                                    pjData.BindingManager.StatTxtBinding(ObjectID).Value = Value
                                End If
                            Next
                        Case SCDatFiles.DatFiles.ButtonData
                            For i = 0 To tTemplate.Values.Count - 1
                                Dim valueStrs As String() = tTemplate.Values(i).Split(ValueSpliter)
                                Dim ParamaterName As String = valueStrs(0)
                                Dim Value As String = valueStrs(1)
                                If ParamaterName = "Value" Then
                                    pjData.ExtraDat.ButtonData.GetButtonSet(ObjectID).PasteFromString(Value)
                                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.ButtonData, "ButtonData", ObjectID).Value = False
                                End If
                            Next
                    End Select
                End If
            Catch ex As Exception

            End Try

        End If
    End Sub
    Public Sub ResetDatPage(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer, UnitDatPage As Integer)
        Select Case DatFiles
            Case SCDatFiles.DatFiles.units
                Select Case UnitDatPage
                    Case 6
                        pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Status", ObjectID).DataReset()
                        pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Display", ObjectID).DataReset()
                        pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.wireframe, "wire", ObjectID).DataReset()
                        pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.wireframe, "grp", ObjectID).DataReset()
                        pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.wireframe, "tran", ObjectID).DataReset()
                        pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.ButtonSet, "ButtonSet", ObjectID).DataReset()

                        pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Joint", ObjectID).PPropertyChangedPack()
                    Case 7
                        pjData.BindingManager.RequireDataBinding(ObjectID, DatFiles).IsDefaultUse = True
                        pjData.BindingManager.RequireCapacityBinding(DatFiles).PropertyChangedPack()
                    Case Else
                        For i = 0 To pjData.Dat.GetDatFile(DatFiles).ParamaterList.Count - 1
                            Dim Paramname As String = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetParamname

                            If pjData.BindingManager.DatBinding(DatFiles, Paramname, ObjectID) Is Nothing Then
                                Continue For
                            End If



                            Dim ValueCount As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarCount)
                            Dim ValueStart As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarStart)
                            Dim ValueEnd As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarEnd)

                            If PageMask(UnitDatPage).IndexOf(Paramname) >= 0 Then
                                pjData.BindingManager.DatBinding(DatFiles, Paramname, ObjectID).DataReset()
                            End If

                            'For j = ValueStart To ValueEnd
                            '    pjData.BindingManager.DatBinding(DatFiles, Paramname, j).DataReset()
                            'Next
                        Next
                End Select
            Case SCDatFiles.DatFiles.upgrades, SCDatFiles.DatFiles.techdata, SCDatFiles.DatFiles.orders
                Select Case UnitDatPage
                    Case 7
                        pjData.BindingManager.RequireDataBinding(ObjectID, DatFiles).IsDefaultUse = True
                        pjData.BindingManager.RequireCapacityBinding(DatFiles).PropertyChangedPack()
                    Case 8
                        pjData.BindingManager.RequireDataBinding(ObjectID, SCDatFiles.DatFiles.Stechdata).IsDefaultUse = True
                        pjData.BindingManager.RequireCapacityBinding(SCDatFiles.DatFiles.Stechdata).PropertyChangedPack()
                    Case Else
                        For i = 0 To pjData.Dat.GetDatFile(DatFiles).ParamaterList.Count - 1
                            Dim Paramname As String = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetParamname

                            If pjData.BindingManager.DatBinding(DatFiles, Paramname, ObjectID) Is Nothing Then
                                Continue For
                            End If



                            Dim ValueCount As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarCount)
                            Dim ValueStart As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarStart)
                            Dim ValueEnd As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarEnd)

                            pjData.BindingManager.DatBinding(DatFiles, Paramname, ObjectID).DataReset()
                            'For j = ValueStart To ValueEnd
                            '    pjData.BindingManager.DatBinding(DatFiles, Paramname, j).DataReset()
                            'Next
                        Next
                End Select
            Case Else
                ResetDatObject(DatFiles, ObjectID)
        End Select

    End Sub
    Public Sub ResetDatObject(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        If SCDatFiles.CheckValidDat(DatFiles) Then
            For i = 0 To pjData.Dat.GetDatFile(DatFiles).ParamaterList.Count - 1
                Dim Paramname As String = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetParamname

                If pjData.BindingManager.DatBinding(DatFiles, Paramname, ObjectID) Is Nothing Then
                    Continue For
                End If



                Dim ValueCount As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarCount)
                Dim ValueStart As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarStart)
                Dim ValueEnd As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarEnd)

                pjData.BindingManager.DatBinding(DatFiles, Paramname, ObjectID).DataReset()
                'For j = ValueStart To ValueEnd
                '    pjData.BindingManager.DatBinding(DatFiles, Paramname, j).DataReset()
                'Next
            Next
            Select Case DatFiles
                Case SCDatFiles.DatFiles.units
                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Status", ObjectID).DataReset()
                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Display", ObjectID).DataReset()

                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.wireframe, "wire", ObjectID).DataReset()
                    If ObjectID < SCGrpWireCount Then
                        pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.wireframe, "grp", ObjectID).DataReset()
                    End If
                    If ObjectID < SCMenCount Then
                        pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.wireframe, "tran", ObjectID).DataReset()
                    End If

                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.ButtonSet, "ButtonSet", ObjectID).DataReset()

                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.statusinfor, "Joint", ObjectID).PPropertyChangedPack()
            End Select
            Select Case DatFiles
                Case SCDatFiles.DatFiles.units, SCDatFiles.DatFiles.upgrades, SCDatFiles.DatFiles.orders
                    pjData.BindingManager.RequireDataBinding(ObjectID, DatFiles).IsDefaultUse = True
                    pjData.BindingManager.RequireCapacityBinding(DatFiles).PropertyChangedPack()


                            'Template.Values.Add("RequireData" & ValueSpliter & pjData.ExtraDat.RequireData(DatFiles).GetCopyString(ObjectID))
                Case SCDatFiles.DatFiles.techdata
                    pjData.BindingManager.RequireDataBinding(ObjectID, SCDatFiles.DatFiles.techdata).IsDefaultUse = True
                    pjData.BindingManager.RequireCapacityBinding(SCDatFiles.DatFiles.techdata).PropertyChangedPack()
                    pjData.BindingManager.RequireDataBinding(ObjectID, SCDatFiles.DatFiles.Stechdata).IsDefaultUse = True
                    pjData.BindingManager.RequireCapacityBinding(SCDatFiles.DatFiles.Stechdata).PropertyChangedPack()
                    'Template.Values.Add("RequireData1" & ValueSpliter & pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.techdata).GetCopyString(ObjectID))
                    'Template.Values.Add("RequireData2" & ValueSpliter & pjData.ExtraDat.RequireData(SCDatFiles.DatFiles.Stechdata).GetCopyString(ObjectID))
            End Select
        Else
            Select Case DatFiles
                Case SCDatFiles.DatFiles.stattxt
                    pjData.BindingManager.StatTxtBinding(ObjectID).DataReset()
                Case SCDatFiles.DatFiles.ButtonData
                    pjData.BindingManager.ExtraDatBinding(SCDatFiles.DatFiles.ButtonData, "ButtonData", ObjectID).DataReset()
            End Select
        End If
    End Sub



    Public Function DataPagePasteAble(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer) As Boolean
        Return DataObjectPasteAble(DatFiles, ObjectID)
    End Function
    Public Function DataObjectPasteAble(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer) As Boolean
        Dim tTemplate As Template
        Try
            tTemplate = JsonConvert.DeserializeObject(Of Template)((My.Computer.Clipboard.GetText))
            If Datfilesname(DatFiles) = tTemplate.DatName Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function



    Private Class Template
        Public DatName As String
        Public Index As String
        Public Values As New List(Of String)

        Public Sub New(tDatName As String, tindex As Integer)
            DatName = tDatName
            Index = tindex
        End Sub
    End Class
End Class
