Imports LuaInterface

Public Class LuaManager
    Public Shared ReadOnly Property LuaFloderPath As String
        Get
            If Not My.Computer.FileSystem.DirectoryExists(System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua\DataEditor") Then

                If Not My.Computer.FileSystem.DirectoryExists(System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua") Then
                    My.Computer.FileSystem.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua")
                End If
                My.Computer.FileSystem.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua\DataEditor")
            End If


            Return System.AppDomain.CurrentDomain.BaseDirectory & "Data\Lua\DataEditor"
        End Get
    End Property

    Public Shared LuaKeyWord() As String = {"and", "break", "do", "else", "elseif", "end", "false", "for", "function", "if", "in", "local", "nil", "not", "or", "repeat", "return", "then", "true", "until", "while"}

    Private LogBox As TextBox
    Public ReadOnly Functions As List(Of String)
    Public ReadOnly ToolTips As List(Of TextBlock)
    Public ReadOnly Propertys As List(Of String)
    Public ReadOnly PropertyToolTips As List(Of TextBlock)
    Public ReadOnly ArgumentDefine As List(Of String())
    Public ReadOnly ToolTipTexts As List(Of String)

    Public Function GetTooltipf(funcname As String) As TextBlock
        If Functions.IndexOf(funcname) >= 0 Then
            Return Tool.TextColorBlock(ToolTipTexts(Functions.IndexOf(funcname)), False)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetArgumentDefine(funcname As String, argumentindex As Integer) As String
        Dim funcindex As Integer = Functions.IndexOf(funcname)

        If funcindex >= 0 Then
            If ArgumentDefine(funcindex).Count > argumentindex Then
                Return ArgumentDefine(funcindex)(argumentindex)
            Else
                Return Nothing
            End If

        Else
            Return Nothing
        End If
    End Function


    Private LuaSc As Lua
    Public Sub DoString(str As String)
        LuaSc.DoString(str)
    End Sub

    Public Sub New(LogTextBox As TextBox)
        LuaSc = New Lua
        LogBox = LogTextBox
        LogBox.Clear()

        Functions = New List(Of String)
        ToolTips = New List(Of TextBlock)
        Propertys = New List(Of String)
        PropertyToolTips = New List(Of TextBlock)
        ArgumentDefine = New List(Of String())
        ToolTipTexts = New List(Of String)

        For Each files As String In My.Computer.FileSystem.GetFiles(LuaFloderPath)
            Try
                LuaSc.DoFile(files)
            Catch ex As Exception
                ErrorLog(ex.Message)
            End Try
        Next



        RegFunction("ClearLog", "clear", "<1>function <0>clear()" & vbCrLf & "로그 창을 지웁니다.")

        For DatFiles = 0 To Datfilesname.Count - 1
            Propertys.Add(Datfilesname(DatFiles))
            PropertyToolTips.Add(Tool.TextColorBlock(Datfilesname(DatFiles)))
        Next

        Dim DatIndex As String = ""
        For DatFiles = 0 To 7
            DatIndex = DatIndex & " " & Datfilesname(DatFiles)


            For i = 0 To pjData.Dat.GetDatFile(DatFiles).ParamaterList.Count - 1
                Dim Paramname As String = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetParamname
                Paramname = Paramname.Replace(" ", "_")
                Propertys.Add(Paramname)
                PropertyToolTips.Add(Tool.TextColorBlock(Datfilesname(DatFiles) & " " & Paramname))
            Next
        Next

        RegFunction("Log", "log", "<1>function <0>log(str)" & vbCrLf & "로그를 입력합니다.")

        'RegFunction("SetDat", "setdat", "<1>function <0>setdat(datfile =<2>" & DatIndex & "<0>, param , objectid, value)" & vbCrLf & "Dat데이터를 수정합니다.",
        '            {"datfile", "param", "objectid", "value"})

        RegFunction("SetDat", "setdat", "<1>function <0>setdat(datfile, param , objectid, value)" & vbCrLf & "Dat데이터를 수정합니다.",
                    {"datfile", "param", "objectid", "value"})
        RegFunction("GetDat", "getdat", "<1>function <0>getdat(datfile, param , objectid)" & vbCrLf & "Dat데이터를 읽어옵니다.",
                    {"datfile", "param", "objectid"})
        RegFunction("ResetDat", "resetdat", "<1>function <0>resetdat(datfile, param , objectid)" & vbCrLf & "Dat데이터를 초기화 합니다.",
                    {"datfile", "param", "objectid"})


        RegFunction("SetTbl", "settbl", "<1>function <0>settbl(tblid, value)" & vbCrLf & "Tbl을 수정합니다.",
                    {"tblid", "value"})
        RegFunction("GetTbl", "gettbl", "<1>function <0>gettbl(tblid)" & vbCrLf & "Tbl을 읽어옵니다.",
                    {"tblid"})
        RegFunction("ResetTbl", "resettbl", "<1>function <0>resettbl(tblid)" & vbCrLf & "Tbl을 초기화 합니다.",
                    {"tblid"})



        RegFunction("GetCodeLabel", "getcodelabel", "<1>function <0>getcodelabel(datfile, objectid)" & vbCrLf & "오브젝트의 이름을 읽어옵니다.",
                    {"datfile", "objectid"})
        RegFunction("GetCodeCount", "getcodecount", "<1>function <0>getcodecount(datfile)" & vbCrLf & "각 코드의 총 갯수를 반환합니다.",
                    {"datfile"})
        RegFunction("GetDatName", "getdatname", "<1>function <0>getdatname(index)" & vbCrLf & "해당 index의 DatFile의 이름을 가져옵니다.",
                    {"index"})


        RegFunction("SetToolTip", "settooltip", "<1>function <0>settooltip(datfile, objectid, value)" & vbCrLf & "ToolTip테이터를 수정합니다.",
                    {"datfile", "objectid", "value"})
        RegFunction("GetToolTip", "gettooltip", "<1>function <0>gettooltip(datfile, objectid)" & vbCrLf & "ToolTip테이터를 읽어옵니다.",
                    {"datfile", "objectid"})
        RegFunction("ResetToolTip", "resettooltip", "<1>function <0>resettooltip(datfile, objectid)" & vbCrLf & "ToolTip테이터를 초기화 합니다",
                    {"datfile", "objectid"})


        RegFunction("SetGroup", "setgroup", "<1>function <0>setgroup(datfile, objectid, value)" & vbCrLf & "Group데이터를 수정합니다.",
                    {"datfile", "objectid", "value"})
        RegFunction("GetGroup", "getgroup", "<1>function <0>getgroup(datfile, objectid)" & vbCrLf & "Group데이터를 읽어옵니다.",
                    {"datfile", "objectid"})
        RegFunction("ResetGroup", "resetgroup", "<1>function <0>resetgroup(datfile, objectid)" & vbCrLf & "Group데이터를 초기화 합니다.",
                    {"datfile", "objectid"})
    End Sub




    Public Function CheckExistFunc(str As String) As Boolean
        Try
            Dim num As Integer = str
            Return False
        Catch ex As Exception

        End Try

        For i = 0 To Functions.Count - 1
            If Functions(i).ToLower.IndexOf(str.ToLower) >= 0 Then
                Return True
            End If
        Next
        For i = 0 To Propertys.Count - 1
            If Propertys(i).ToLower.IndexOf(str.ToLower) >= 0 Then
                Return True
            End If
        Next

        Return False
    End Function

    Private Sub RegFunction(MethodName As String, LuaFucnName As String, ToolTip As String, Optional Arguments() As String = Nothing)
        LuaSc.RegisterFunction(LuaFucnName, Me, Me.GetType().GetMethod(MethodName))
        Functions.Add(LuaFucnName)
        ToolTips.Add(Tool.TextColorBlock(ToolTip))
        ToolTipTexts.Add(ToolTip)

        If Arguments Is Nothing Then
            ArgumentDefine.Add({""})
        Else
            ArgumentDefine.Add(Arguments)
        End If
    End Sub

    Public Sub Log(textstr As String)
        LogBox.AppendText(vbCrLf & textstr)
    End Sub
    Private Sub ErrorLog(textstr As String)
        LogBox.AppendText(vbCrLf & textstr)
    End Sub

    Public Sub ClearLog()
        LogBox.Clear()
    End Sub
    Public Function GetDatFile(DatName As String) As SCDatFiles.DatFiles
        Dim Datfile As SCDatFiles.DatFiles = SCDatFiles.DatFiles.None

        For i = 0 To SCConst.Datfilesname.Count - 1
            If DatName = SCConst.Datfilesname(i) Then
                Datfile = i
                Exit For
            End If
        Next
        For i = 0 To SCConst.Datfilesname.Count - 1
            If DatName = Tool.GetText(SCConst.Datfilesname(i)) Then
                Datfile = i
                Exit For
            End If
        Next

        If Datfile = SCDatFiles.DatFiles.None Then
            Dim DatIndex As String = ""
            For i = 0 To 7
                DatIndex = DatIndex & " " & Datfilesname(i)
            Next
            ErrorLog("올바른 Dat파일 이름이 아닙니다." & vbCrLf & DatIndex)
            Return SCDatFiles.DatFiles.None
        End If
        Return Datfile
    End Function


    Public Sub SetTbl(ObjectId As Integer, Value As String)
        pjData.BindingManager.StatTxtBinding(ObjectId - 1).Value = Value
    End Sub
    Public Function GetTbl(ObjectId As Integer) As String
        Return pjData.BindingManager.StatTxtBinding(ObjectId - 1).Value
    End Function
    Public Sub ResetTbl(ObjectId As Integer)
        pjData.BindingManager.StatTxtBinding(ObjectId).DataReset()
    End Sub




    Public Sub SetDat(DatName As String, ParamaterName As String, ObjectId As Integer, Value As Long)
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return
        End If
        DatName = Datfilesname(Datfile)
        ParamaterName = Tool.ParamaterParser(ParamaterName, DatName)

        If SCDatFiles.CheckValidDat(Datfile) Then
            pjData.BindingManager.DatBinding(Datfile, ParamaterName, ObjectId).Value = Value
        Else
            pjData.BindingManager.ExtraDatBinding(Datfile, ParamaterName, ObjectId).Value = Value
        End If

    End Sub
    Public Function GetDat(DatName As String, ParamaterName As String, ObjectId As Integer) As Long
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return 0
        End If
        DatName = Datfilesname(Datfile)
        ParamaterName = Tool.ParamaterParser(ParamaterName, DatName)


        If SCDatFiles.CheckValidDat(Datfile) Then
            Return pjData.BindingManager.DatBinding(Datfile, ParamaterName, ObjectId).Value
        Else
            Return pjData.BindingManager.ExtraDatBinding(Datfile, ParamaterName, ObjectId).Value
        End If
    End Function
    Public Sub ResetDat(DatName As String, ParamaterName As String, ObjectId As Integer)
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return
        End If
        DatName = Datfilesname(Datfile)
        ParamaterName = Tool.ParamaterParser(ParamaterName, DatName)


        If SCDatFiles.CheckValidDat(Datfile) Then
            pjData.BindingManager.DatBinding(Datfile, ParamaterName, ObjectId).DataReset()
        Else
            pjData.BindingManager.ExtraDatBinding(Datfile, ParamaterName, ObjectId).DataReset()
        End If
    End Sub


    Public Function GetCodeLabel(DatName As String, ObjectId As Integer) As String
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return ""
        End If
        Return pjData.BindingManager.UIManager(Datfile, ObjectId).Name
    End Function
    Public Function GetCodeCount(DatName As String) As Integer
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return ""
        End If
        Return SCCodeCount(Datfile)
    End Function
    Public Function GetDatName(Datindex As Integer) As String
        Return Datfilesname(Datindex)
    End Function

    Public Sub SetToolTip(DatName As String, ObjectId As Integer, NewValue As String)
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return
        End If
        pjData.BindingManager.UIManager(Datfile, ObjectId).ToolTip = NewValue
    End Sub
    Public Function GetToolTip(DatName As String, ObjectId As Integer) As String
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return ""
        End If
        Return pjData.BindingManager.UIManager(Datfile, ObjectId).ToolTip
    End Function
    Public Sub ResetToolTip(DatName As String, ObjectId As Integer)
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return
        End If
        pjData.BindingManager.UIManager(Datfile, ObjectId).ToolTipReset()
    End Sub


    Public Sub SetGroup(DatName As String, ObjectId As Integer, NewValue As String)
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return
        End If
        pjData.BindingManager.UIManager(Datfile, ObjectId).Group = NewValue
    End Sub
    Public Function GetGroup(DatName As String, ObjectId As Integer) As String
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return ""
        End If
        Return pjData.BindingManager.UIManager(Datfile, ObjectId).Group
    End Function
    Public Sub ResetGroup(DatName As String, ObjectId As Integer)
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return
        End If
        pjData.BindingManager.UIManager(Datfile, ObjectId).GroupReset()
    End Sub
End Class
