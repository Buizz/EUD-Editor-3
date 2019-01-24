Imports Newtonsoft.Json

Public Class DataManager
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


    '복사 붙여넣기 등등을 담당.
    '바인딩을 이용하는게 특징.
    Public Sub CopyDatPage(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        CopyDatObject(DatFiles, ObjectID)
    End Sub

    Public Sub CopyDatObject(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        Dim Template As New Template(Datfilesname(DatFiles), ObjectID)
        For i = 0 To pjData.Dat.GetDatFile(DatFiles).ParamaterList.Count - 1
            Dim Paramname As String = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetParamname

            If pjData.BindingManager.DatBinding(DatFiles, Paramname, ObjectID) Is Nothing Then
                Continue For
            End If

            Dim ValueCount As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarCount)
            Dim ValueStart As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarStart)
            Dim ValueEnd As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarEnd)


            Dim Value As Long = pjData.Dat.Data(DatFiles, Paramname, ObjectID)
            Template.Values.Add(Paramname & ":" & Value)
        Next
        Dim tempStr As String = JsonConvert.SerializeObject(Template)

        My.Computer.Clipboard.SetText(tempStr)
    End Sub

    Public Sub PasteDatPage(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer, UnitDatPage As Integer)
        If DataPagePasteAble(DatFiles, ObjectID) Then
            Dim tTemplate As Template = JsonConvert.DeserializeObject(Of Template)((My.Computer.Clipboard.GetText))


            For i = 0 To tTemplate.Values.Count - 1
                Dim valueStrs As String() = tTemplate.Values(i).Split(":")
                Dim ParamaterName As String = valueStrs(0)
                Dim Value As Long = valueStrs(1)

                If PageMask(UnitDatPage).IndexOf(ParamaterName) >= 0 Then
                    If pjData.BindingManager.DatBinding(DatFiles, ParamaterName, ObjectID) IsNot Nothing Then
                        pjData.BindingManager.DatBinding(DatFiles, ParamaterName, ObjectID).Value = Value
                    End If
                End If
            Next
        End If
    End Sub
    Public Sub PasteDatObject(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        If DataObjectPasteAble(DatFiles, ObjectID) Then
            Dim tTemplate As Template = JsonConvert.DeserializeObject(Of Template)((My.Computer.Clipboard.GetText))


            For i = 0 To tTemplate.Values.Count - 1
                Dim valueStrs As String() = tTemplate.Values(i).Split(":")
                Dim ParamaterName As String = valueStrs(0)
                Dim Value As Long = valueStrs(1)

                If pjData.BindingManager.DatBinding(DatFiles, ParamaterName, ObjectID) IsNot Nothing Then
                    If pjData.Dat.ParamInfo(DatFiles, ParamaterName, SCDatFiles.EParamInfo.IsEnabled) Then
                        pjData.BindingManager.DatBinding(DatFiles, ParamaterName, ObjectID).Value = Value
                    End If
                End If
            Next
        End If
    End Sub
    Public Sub ResetDatPage(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer, UnitDatPage As Integer)
        If DatFiles = SCDatFiles.DatFiles.units Then
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
        Else
            PasteDatObject(DatFiles, ObjectID)
        End If
    End Sub
    Public Sub ResetDatObject(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
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
