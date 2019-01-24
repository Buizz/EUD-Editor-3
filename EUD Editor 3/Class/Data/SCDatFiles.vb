Imports System.IO

<Serializable()>
Public Class SCDatFiles
    'Dat파일 정의
    Private Datfile As List(Of CDatFile)



    Private DatfileDic As Dictionary(Of DatFiles, CDatFile)

    Public Enum DatFiles
        units = 0
        weapons = 1
        flingy = 2
        sprites = 3
        images = 4
        upgrades = 5
        techdata = 6
        orders = 7
        portdata = 8
        sfxdata = 9

        'Firegraft
        statusinfor = 10
        button = 11
        require = 12
        None = 255
    End Enum
    Public Enum EParamInfo
        Size = 0
        VarStart = 1
        VarEnd = 2
        VarCount = 3
        ValueType = 4
        IsEnabled = 5
    End Enum

    Public Sub New(IsProjectData As Boolean, Optional TemporyData As Boolean = False, Optional IsBindingData As Boolean = False)
        DatfileDic = New Dictionary(Of DatFiles, CDatFile)

        Datfile = New List(Of CDatFile)

        For Each str As String In Datfilesname
            Dim tDatfile As New CDatFile(str, IsProjectData, TemporyData, IsBindingData)


            Datfile.Add(tDatfile)
            DatfileDic.Add(Datfile.Count - 1, Datfile.Last)
        Next
    End Sub
    Public Sub New()
        DatfileDic = New Dictionary(Of DatFiles, CDatFile)

        Datfile = New List(Of CDatFile)

        For Each str As String In Datfilesname
            Dim tDatfile As New CDatFile(str, True, False, False)


            Datfile.Add(tDatfile)
            DatfileDic.Add(Datfile.Count - 1, Datfile.Last)
        Next
    End Sub

    Public ReadOnly Property DatFileList As List(Of CDatFile)
        Get
            Return Datfile
        End Get
    End Property

    Public ReadOnly Property GetDatFile(_DatFile As DatFiles) As CDatFile
        Get
            Return DatfileDic(_DatFile)
        End Get
    End Property




    Public ReadOnly Property Values(key As DatFiles, paramName As String, index As Integer) As CDatFile.CParamater.Value
        Get
            Return DatfileDic(key).GetParamValue(paramName, index)
        End Get
    End Property
    Public Property Data(key As DatFiles, paramName As String, index As Integer) As Long
        Get
            Return DatfileDic(key).GetParamData(paramName, index)
        End Get
        Set(value As Long)
            DatfileDic(key).GetParamData(paramName, index) = value
        End Set
    End Property


    Public ReadOnly Property ParamInfo(key As DatFiles, paramName As String, Type As EParamInfo) As Integer
        Get
            Return DatfileDic(key).GetParamInfo(paramName, Type)
        End Get
    End Property



    Public Property Group(key As DatFiles, index As Integer) As String
        Get
            Return DatfileDic(key).Group(index)
        End Get
        Set(value As String)
            DatfileDic(key).Group(index) = value
        End Set
    End Property
    Public Property ToolTip(key As DatFiles, index As Integer) As String
        Get
            Return DatfileDic(key).ToolTip(index)
        End Get
        Set(value As String)
            DatfileDic(key).ToolTip(index) = value
        End Set
    End Property


    <Serializable()>
    Public Class CDatFile
        Public Function CheckDirty(ObjectID As Integer) As Boolean
            For i = 0 To Paramaters.Count - 1
                If Paramaters(i).GetValue(ObjectID) IsNot Nothing Then
                    If Not Paramaters(i).GetValue(ObjectID).IsDefault Then
                        Return False
                    End If
                End If
            Next
            Return True
        End Function


        Private CodeGroup As String()
        Private CodeToolTip As String()
        Public Property Group(index As Integer) As String
            Get
                Return CodeGroup(index)
            End Get
            Set(value As String)
                CodeGroup(index) = value
            End Set
        End Property
        Public Property ToolTip(index As Integer) As String
            Get
                If CodeToolTip.Count > index Then
                    Return CodeToolTip(index)
                Else
                    Return ""
                End If
            End Get
            Set(value As String)
                CodeToolTip(index) = value
            End Set
        End Property


        Private FIleName As String 'ex sprites
        Private Paramaters As List(Of CParamater)
        Public ReadOnly Property ParamaterList As List(Of CParamater)
            Get
                Return Paramaters
            End Get
        End Property


        Public ReadOnly Property GetParamValue(name As String, index As Integer) As CParamater.Value
            Get
                Return ParamDic(name).GetValue(index)
            End Get
        End Property

        Public Property GetParamData(name As String, index As Integer) As Long
            Get
                Return ParamDic(name).Data(index)
            End Get
            Set(value As Long)
                ParamDic(name).Data(index) = value
            End Set
        End Property
        Public ReadOnly Property GetParamInfo(name As String, Type As EParamInfo) As Long
            Get
                Return ParamDic(name).GetInfo(Type)
            End Get
        End Property



        Public Sub New(tFIleName As String, IsProjectData As Boolean, TemporyData As Boolean, IsBindingData As Boolean)
            ParamDic = New Dictionary(Of String, CParamater)

            FIleName = tFIleName
            Paramaters = New List(Of CParamater)


            Dim filepath As String = Tool.GetDatFolder & "\" & FIleName


            Dim fs As New FileStream(filepath & ".dat", FileMode.Open)
            Dim br As New BinaryReader(fs)
            Dim sr As New StreamReader(filepath & ".def")

            sr.ReadLine() '헤더
            Dim varcount = ReadValue(sr.ReadLine()) 'Varcount
            Dim InputEntrycount = ReadValue(sr.ReadLine()) 'InputEntrycount
            Dim OutputEntrycount = ReadValue(sr.ReadLine()) 'OutputEntrycount

            sr.ReadLine() ' 빈공간
            sr.ReadLine() ' 값

            For i = 0 To varcount - 1
                Paramaters.Add(New CParamater(FIleName, sr, br, Paramaters.Count, InputEntrycount, IsProjectData, IsBindingData))
                ParamDic.Add(Paramaters.Last.GetParamname, Paramaters.Last)

            Next

            sr.Close()







            br.Close()
            fs.Close()

            If Not TemporyData Then
                CodeGrouping.CodeGrouping(FIleName, CodeGroup, CodeToolTip)

            End If
        End Sub
        Private ReadOnly Property ReadValue(str As String) As String
            Get
                Return str.Split("=").Last
            End Get
        End Property

        Private ParamDic As Dictionary(Of String, CParamater)
        '피라미터들
        <Serializable()>
        Public Class CParamater
            'Public ReadOnly Property GetOffsetName() As String
            '    Get
            '        Return scData.GetOffset(FIleName & "_" & ParamaterName)
            '    End Get
            'End Property



            '만약 잘못된 수를 불러올 경우 안전장치 추가해야됨
            '꼮!!!!!!!!!!!!!! 일단 지금은 바쁘니까 넘어간다.
            Public Property Data(index As Integer) As Long
                Get
                    index -= VarStart
                    If index < Values.Count Then
                        Return Values(index).Data
                    Else
                        Return 0
                    End If


                End Get
                Set(value As Long)
                    index -= VarStart
                    If (0 <= value) And (value < Math.Pow(256, Size)) Then
                        Values(index).Data = value
                    ElseIf value < 0 Then
                        Values(index).Data = 0
                    Else
                        Values(index).Data = Math.Pow(256, Size) - 1
                    End If

                End Set
            End Property
            Public ReadOnly Property GetValue(index As Integer) As Value
                Get
                    Dim realIndex As Integer = index - VarStart

                    If Values.Count > realIndex And realIndex >= 0 Then
                        Return Values(realIndex)
                    Else
                        Return Nothing 'ew Value(0, False)
                    End If
                End Get
            End Property


            Public Sub New(_FIleName As String, sr As StreamReader, br As BinaryReader, prcount As Integer, encount As Integer, IsProjectData As Boolean, IsBindingData As Boolean)
                FIleName = _FIleName
                VarEnd = encount - 1
                While Not sr.EndOfStream
                    Dim text As String = sr.ReadLine()
                    If text.IndexOf("=") = -1 Then
                        Exit While
                    End If
                    Dim key As String = text.Split("=").First.Replace(prcount, "")
                    Dim value As String = text.Split("=").Last
                    If value.IndexOf(":") >= 0 Then
                        value = value.Split(":").First
                    End If

                    Select Case key
                        Case "Name"
                            ParamaterName = value
                        Case "Size"
                            Size = value
                        Case "VarStart"
                            VarStart = value
                        Case "VarEnd"
                            VarEnd = value
                        Case "VarArray"
                            VarArray = value
                        Case "VarArrayIndex"
                            VarIndex = value
                        Case "Type"
                            ValueType = value
                        Case Else
                            MsgBox(key)
                    End Select
                End While


                If Tool.ProhibitParam.ToList.IndexOf(ParamaterName) >= 0 Then
                    Enabled = False
                End If

                Values = New List(Of Value)

                Dim currentpos As UInteger = br.BaseStream.Position '베이스 스트림을 기억하고
                br.BaseStream.Position -= (VarIndex - 1) * Size * (VarEnd - VarStart + 1)
                For i As Integer = 0 To VarEnd - VarStart
                    br.BaseStream.Position += (VarIndex - 1) * Size '인덱스 만큼 앞으로 간다.

                    Dim value As UInteger


                    Select Case Size
                        Case 4
                            value = br.ReadUInt32
                        Case 2
                            value = br.ReadUInt16
                        Case 1
                            value = br.ReadByte
                    End Select

                    If IsProjectData Then
                        Values.Add(New Value(0))
                    Else
                        Values.Add(New Value(value))
                    End If


                    'If VarArray = 4 Then
                    '    MsgBox(Hex(br.BaseStream.Position - Size) & vbCrLf & "유닛코드 : " & i & " 이름 : " & ParamaterName & " 값 : " & Values.Last)
                    'End If


                    br.BaseStream.Position += (VarArray - VarIndex) * Size '인덱스 만큼 앞으로 간다.
                Next

                '베이스 스트림에서 Size * encount만큼 간 곳으로 되돌린다.
                br.BaseStream.Position = currentpos + Size * (VarEnd - VarStart + 1)
            End Sub


            Private FIleName As String
            Private ParamaterName As String
            Public ReadOnly Property GetParamname As String
                Get
                    Return ParamaterName
                End Get
            End Property


            Private Size As Byte
            Public ReadOnly Property GetInfo(Type As EParamInfo) As Integer
                Get
                    Select Case Type
                        Case EParamInfo.Size
                            Return Size
                        Case EParamInfo.VarStart
                            Return VarStart
                        Case EParamInfo.VarEnd
                            Return VarEnd
                        Case EParamInfo.VarCount
                            Return Values.Count
                        Case EParamInfo.ValueType
                            Return ValueType
                        Case EParamInfo.IsEnabled
                            Return Enabled
                    End Select
                    Return 0
                End Get
            End Property


            Private VarStart As UInteger = 0
            Private VarEnd As UInteger = 0

            Private VarArray As UInteger = 1
            Private VarIndex As UInteger = 1


            Private ValueType As SCDatFiles.DatFiles = DatFiles.None

            Private Enabled As Boolean = True

            Private Values As List(Of Value)
            <Serializable()>
            Public Class Value
                Public Property Data As Long

                Public Property Enabled As Boolean
                Public Property IsDefault As Boolean

                Public Sub New(_Data As Long, Optional _Enabled As Boolean = True)
                    Data = _Data
                    Enabled = _Enabled
                    IsDefault = True
                End Sub
            End Class



            Public ReadOnly Property GetValueCount As Integer
                Get
                    Return Values.Count
                End Get
            End Property
        End Class
    End Class
End Class