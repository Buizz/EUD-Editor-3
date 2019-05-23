<Serializable>
Public Class CRequireData
    Public Shared Function HasValueOpCode(opcode As EOpCode) As Boolean
        Dim OpCodes() As EOpCode = {EOpCode._Must_have_, EOpCode.Current_unit_is, EOpCode.Must_have, EOpCode.Must_have_add_On, EOpCode.Is_researched}

        Return Not OpCodes.ToList.IndexOf(opcode)
    End Function
    Private Datfile As SCDatFiles.DatFiles
    Private Flag As Boolean

    Private RequireDatas As List(Of RequireObject)
    Private OrigRequireDatas As List(Of RequireObject)


    Public Enum RequireUse
        DefaultUse
        DontUse
        AlwaysUse
        AlwaysCurrentUse
        CustomUse
    End Enum
    Public Sub New(tDatfile As SCDatFiles.DatFiles, Codes As List(Of ExtraDatFiles.SReqDATA), Optional tFlag As Boolean = False)
        Datfile = tDatfile
        Flag = tFlag

        RequireDatas = New List(Of RequireObject)
        OrigRequireDatas = New List(Of RequireObject)


        For i = 0 To Codes.Count - 1
            RequireDatas.Add(New RequireObject(i, Datfile, Codes(i).Code, Codes(i).pos))
            OrigRequireDatas.Add(New RequireObject(i, Datfile, Codes(i).Code, Codes(i).pos))
        Next
    End Sub

    Public Sub WriteBinaryData(index As Integer, bw As IO.BinaryWriter)
        For i = 0 To RequireDatas(index).ReauireBlock.Count - 1
            RequireDatas(index).ReauireBlock(i).CodeWrite(bw)
        Next
    End Sub
    Public Function GetCapacity() As Integer
        Dim TotalSize As Integer
        TotalSize += 4
        For i = 0 To RequireDatas.Count - 1
            If (RequireDatas(i).StartPos > 0 And RequireDatas(i).UseStatus <> RequireUse.DontUse) Or RequireDatas(i).UseStatus = RequireUse.CustomUse Then
                TotalSize += 2 '시작 부호2개와 끝 부호2개
                For j = 0 To RequireDatas(i).ReauireBlock.Count - 1
                    TotalSize += RequireDatas(i).ReauireBlock(j).GetSize()
                Next
            End If
        Next

        Return TotalSize
    End Function
    Public Function GetMaxCapacity() As Integer
        Select Case Datfile
            Case SCDatFiles.DatFiles.units
                Return 1096
            Case SCDatFiles.DatFiles.upgrades
                Return 840
            Case SCDatFiles.DatFiles.techdata
                If Flag Then
                    Return 688
                Else
                    Return 320
                End If
            Case SCDatFiles.DatFiles.orders
                Return 1316
        End Select
        Return 0
    End Function

    Public Sub PasteCopyData(index As Integer, pasteString As String)
        Dim str As String() = pasteString.Split(".")

        If str(0) = RequireUse.CustomUse Then
            If Datfile = SCDatFiles.DatFiles.techdata And Flag Then
                pjData.BindingManager.RequireDataBinding(index, SCDatFiles.DatFiles.Stechdata).IsCustomUse = True
            Else
                pjData.BindingManager.RequireDataBinding(index, Datfile).IsCustomUse = True
            End If

            RequireDatas(index).PasteCopyData(pasteString)
        Else
            Select Case str(0)
                Case RequireUse.AlwaysUse
                    pjData.BindingManager.RequireDataBinding(index, Datfile).IsAlwaysUse = True
                Case RequireUse.AlwaysCurrentUse
                    pjData.BindingManager.RequireDataBinding(index, Datfile).IsAlwaysCurrentUse = True
                Case RequireUse.DontUse
                    pjData.BindingManager.RequireDataBinding(index, Datfile).IsDontUse = True
            End Select

            'RequireObjectUsed(index) = str(0)


        End If



    End Sub
    Public Function GetCopyString(index As Integer) As String
        Select Case RequireDatas(index).UseStatus
            Case RequireUse.CustomUse, RequireUse.DefaultUse
                Dim tString As String = ""
                For i = 0 To RequireDatas(index).ReauireBlock.Count - 1
                    If i = 0 Then
                        tString = RequireDatas(index).ReauireBlock(i).GetCopyText
                    Else
                        tString = tString & "." & RequireDatas(index).ReauireBlock(i).GetCopyText
                    End If
                Next
                If tString <> "" Then
                    Return RequireUse.CustomUse & "." & tString
                Else
                    Return RequireUse.CustomUse
                End If
            Case Else
                Return RequireDatas(index).UseStatus
        End Select
    End Function



    'Public Property GetRequireUseStatus(index As Integer) As RequireUse
    '    Get
    '        Return RequireDatas(index).UseStatus
    '    End Get
    '    Set(value As RequireUse)
    '        RequireDatas(index).UseStatus = value
    '    End Set
    'End Property

    Public ReadOnly Property GetRequireObject(index As Integer) As RequireObject
        Get
            Return RequireDatas(index)
        End Get
    End Property

    Public ReadOnly Property GetRequireBlocks(index As Integer) As List(Of RequireBlock)
        Get
            Return RequireDatas(index).ReauireBlock
        End Get
    End Property
    Public Property RequireObjectUsed(index As Integer) As RequireUse
        Get
            Return RequireDatas(index).UseStatus
        End Get
        Set(value As RequireUse)
            RequireDatas(index).UseStatus = value
            Select Case value
                Case RequireUse.DefaultUse
                    RequireDatas(index).ReauireBlock.Clear()
                    For i = 0 To OrigRequireDatas(index).ReauireBlock.Count - 1
                        RequireDatas(index).ReauireBlock.Add(New RequireBlock(OrigRequireDatas(index).ReauireBlock(i).opCode, OrigRequireDatas(index).ReauireBlock(i).Value))
                    Next

                Case RequireUse.DontUse
                    RequireDatas(index).ReauireBlock.Clear()
                Case RequireUse.AlwaysUse
                    RequireDatas(index).ReauireBlock.Clear()
                Case RequireUse.AlwaysCurrentUse
                    RequireDatas(index).ReauireBlock.Clear()

                    For i = 0 To OrigRequireDatas(index).ReauireBlock.Count - 1
                        If OrigRequireDatas(index).ReauireBlock(i).opCode = EOpCode.Current_unit_is Then
                            If RequireDatas(index).ReauireBlock.Count > 0 Then
                                RequireDatas(index).ReauireBlock.Add(New RequireBlock(EOpCode.Or_))
                            End If
                            RequireDatas(index).ReauireBlock.Add(New RequireBlock(OrigRequireDatas(index).ReauireBlock(i).opCode, OrigRequireDatas(index).ReauireBlock(i).Value))
                        End If
                    Next
                Case RequireUse.CustomUse
                    RequireDatas(index).ReauireBlock.Clear()
                    For i = 0 To OrigRequireDatas(index).ReauireBlock.Count - 1
                        RequireDatas(index).ReauireBlock.Add(New RequireBlock(OrigRequireDatas(index).ReauireBlock(i).opCode, OrigRequireDatas(index).ReauireBlock(i).Value))
                    Next
            End Select
        End Set
    End Property





    <Serializable>
    Public Class RequireObject
        Private Datfile As SCDatFiles.DatFiles
        Private CodeNum As Integer
        Private _StartPos As UShort
        Private _UseStatus As RequireUse
        Public Property UseStatus As RequireUse
            Get
                Return _UseStatus
            End Get
            Set(value As RequireUse)
                _UseStatus = value
            End Set
        End Property

        Private ReauireBlocks As List(Of RequireBlock)
        Public ReadOnly Property ReauireBlock As List(Of RequireBlock)
            Get
                Return ReauireBlocks
            End Get
        End Property
        Public Sub PasteCopyData(pasteString As String)
            Dim str As String() = pasteString.Split(".")
            ReauireBlocks.Clear()
            For i = 1 To str.Count - 1
                Dim tstr As String() = str(i).Split(",")
                If tstr.Count = 1 Then
                    ReauireBlocks.Add(New RequireBlock(tstr(0)))
                Else
                    ReauireBlocks.Add(New RequireBlock(tstr(0), tstr(1)))
                End If
            Next

        End Sub
        Public Property StartPos As UShort
            Get
                Return _StartPos
            End Get
            Set(value As UShort)
                _StartPos = value
            End Set
        End Property


        Public Sub New(tCodeNum As Integer, tDatfile As SCDatFiles.DatFiles, Codes As List(Of UShort), tStartPos As UShort)
            Datfile = tDatfile
            CodeNum = tCodeNum
            _StartPos = tStartPos

            _UseStatus = RequireUse.DefaultUse
            ReauireBlocks = New List(Of RequireBlock)
            'MsgBox("코드시작  인덱스 : " & CodeNum)

            For i = 0 To Codes.Count - 1
                Dim Code As UShort = Codes(i)
                If Code > &HFF Then 'OPCode
                    Dim Opcode As UShort = Code - &HFF00

                    Select Case Opcode
                        Case 2, 3, 4, 37
                            i += 1
                            ReauireBlocks.Add(New RequireBlock(Opcode, Codes(i)))
                            'MsgBox(Opcode & " : " & Codes(i))
                        Case Else
                            ReauireBlocks.Add(New RequireBlock(Opcode))
                            'MsgBox(Opcode)
                    End Select

                Else '값
                    ReauireBlocks.Add(New RequireBlock(EOpCode._Must_have_, Codes(i)))
                    'MsgBox("MustHave  " & Code)
                End If


            Next

            'MsgBox("코드끝")
        End Sub

        Public Sub ReLoad(tCodeNum As Integer, tDatfile As SCDatFiles.DatFiles, Codes As List(Of UShort), tStartPos As UShort)
            Datfile = tDatfile
            CodeNum = tCodeNum
            _StartPos = tStartPos

            _UseStatus = RequireUse.DefaultUse
            ReauireBlocks = New List(Of RequireBlock)
            'MsgBox("코드시작  인덱스 : " & CodeNum)

            For i = 0 To Codes.Count - 1
                Dim Code As UShort = Codes(i)
                If Code > &HFF Then 'OPCode
                    Dim Opcode As UShort = Code - &HFF00

                    Select Case Opcode
                        Case 2, 3, 4, 37
                            i += 1
                            ReauireBlocks.Add(New RequireBlock(Opcode, Codes(i)))
                            'MsgBox(Opcode & " : " & Codes(i))
                        Case Else
                            ReauireBlocks.Add(New RequireBlock(Opcode))
                            'MsgBox(Opcode)
                    End Select

                Else '값
                    ReauireBlocks.Add(New RequireBlock(EOpCode._Must_have_, Codes(i)))
                    'MsgBox("MustHave  " & Code)
                End If


            Next

            'MsgBox("코드끝")
        End Sub

        Public Function LagacyGetCodes() As List(Of UShort)
            Dim ReturnCodes As New List(Of UShort)


            For i = 0 To ReauireBlocks.Count - 1
                If ReauireBlocks(i).HasValue Then '값을 가지고 있을 경우
                    If ReauireBlocks(i).opCode = EOpCode._Must_have_ Then
                        ReturnCodes.Add(ReauireBlocks(i).Value)
                    Else
                        ReturnCodes.Add(ReauireBlocks(i).opCode + &HFF00)
                        ReturnCodes.Add(ReauireBlocks(i).Value)
                    End If
                Else
                    ReturnCodes.Add(ReauireBlocks(i).Value)
                End If
            Next


            Return ReturnCodes
        End Function
    End Class
    <Serializable>
    Public Class RequireBlock
        Private _opCode As EOpCode
        Private _value As Byte

        Public Function Clone() As RequireBlock
            Return New RequireBlock(_opCode, _value)
        End Function


        Public Sub New(topCode As EOpCode, Optional tvalue As Byte = 255)
            _opCode = topCode
            _value = tvalue
        End Sub

        Public Function GetCopyText() As String
            If HasValue() Then
                Return opCode & "," & Value
            Else
                Return opCode
            End If
        End Function

        Public Property Value As Byte
            Get
                Return _value
            End Get
            Set(tvalue As Byte)
                _value = tvalue
            End Set
        End Property
        Public Property opCode As EOpCode
            Get
                Return _opCode
            End Get
            Set(tvalue As EOpCode)
                If opCode = 39 Then
                    _opCode = EOpCode.End_of_Sublist
                Else
                    _opCode = tvalue
                End If
            End Set
        End Property
        Public ReadOnly Property opCodeIndex As Integer
            Get
                If _opCode = EOpCode.End_of_Sublist Then
                    Return 39
                Else
                    Return _opCode
                End If
            End Get
        End Property

        Public Sub CodeWrite(bw As IO.BinaryWriter)
            If HasValue() Then
                If opCode = EOpCode._Must_have_ Then
                    bw.Write(CUShort(Value))
                Else
                    bw.Write(CUShort(opCode + &HFF00))
                    bw.Write(CUShort(Value))
                End If
            Else
                bw.Write(CUShort(opCode + &HFF00))
            End If

        End Sub
        Public ReadOnly Property opText As String
            Get
                If _opCode = EOpCode.End_of_Sublist Then
                    Return Tool.GetText("RequireSubListEnd")
                Else
                    Dim RequireTexts As String() = Tool.GetText("RequireText").Split("|")

                    Return RequireTexts(_opCode)
                End If
            End Get
        End Property
        Public ReadOnly Property ValueText As String
            Get
                If _opCode = EOpCode.Is_researched Then
                    If SCCodeCount(SCDatFiles.DatFiles.techdata) > _value Then
                        Return pjData.CodeLabel(SCDatFiles.DatFiles.techdata, _value, True)
                    Else
                        Return Tool.GetText("None")
                    End If
                Else
                    If SCCodeCount(SCDatFiles.DatFiles.units) > _value Then
                        Return pjData.CodeLabel(SCDatFiles.DatFiles.units, _value, True)
                    Else
                        Return Tool.GetText("None")
                    End If
                End If
            End Get
        End Property
        Public Function GetSize() As Integer
            If HasValue() Then
                If _opCode = EOpCode._Must_have_ Then
                    Return 2
                End If
                Return 4
            Else
                Return 2
            End If
        End Function

        Public Function HasValue() As Boolean
            Return HasValueOpCode(_opCode)
        End Function
    End Class
    Public Enum EOpCode
        _Must_have_ = 0
        Or_ = 1
        Current_unit_is = 2
        Must_have = 3
        Must_have_add_On = 4
        Is_not_lifted_off = 5
        Is_lifted_off = 6
        Is_not_training_or_morphing = 7
        Is_not_constructing_add_On = 8
        Is_not_researching = 9
        Is_not_upgrading = 10
        Is_not_constructing = 11
        Does_not_have_add_on_attached = 12
        Does_not_have_exit = 13
        Has_hangar_space = 14
        Must_be_researched = 15
        Does_not_have_loaded_nuke = 16
        Is_not_burrowed = 17
        Can_attack = 18
        Can_set_rally_point = 19
        Can_move = 20
        Has_weapon = 21
        Is_worker = 22
        Is_flying_building = 23
        Is_transport = 24
        Is_powerup = 25
        Is_Subunit = 26
        Has_spidermines = 27
        Is_hero_and_enabled = 28
        Can_hold_position = 29
        Allow_on_hallucinations = 30
        Upgrade_Lv_1_Require = 31
        Upgrade_Lv_2_Require = 32
        Upgrade_Lv_3__Require = 33
        Grey = 34
        Blank = 35
        Must_be_Brood_War = 36
        Is_researched = 37
        Is_burrowed = 38
        End_of_Sublist = 255
    End Enum
End Class
