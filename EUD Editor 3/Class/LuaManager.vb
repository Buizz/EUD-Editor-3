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


        For Each files As String In My.Computer.FileSystem.GetFiles(LuaFloderPath)
            Try
                LuaSc.DoFile(files)
            Catch ex As Exception
                ErrorLog(ex.Message)
            End Try
        Next



        RegFunction("ClearLog", "clear", ToolTipColorBlock("<1>function <0>clear()" & vbCrLf & "로그 창을 지웁니다."))

        For DatFiles = 0 To Datfilesname.Count - 1

            Propertys.Add(Datfilesname(DatFiles))
            PropertyToolTips.Add(ToolTipColorBlock(Datfilesname(DatFiles)))

        Next

        Dim DatIndex As String = ""
        For DatFiles = 0 To 7
            DatIndex = DatIndex & " " & Datfilesname(DatFiles)


            For i = 0 To pjData.Dat.GetDatFile(DatFiles).ParamaterList.Count - 1
                Dim Paramname As String = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetParamname
                Paramname = Paramname.Replace(" ", "_")
                Propertys.Add(Paramname)
                PropertyToolTips.Add(ToolTipColorBlock(Datfilesname(DatFiles) & " " & Paramname))
            Next
        Next
        RegFunction("SetDat", "setdat", ToolTipColorBlock("<1>function <0>setdat(DatName =<2>" & DatIndex & "<0>, ParamaterName , ObjectId, Value)" & vbCrLf & "Dat데이터를 수정합니다."))
        RegFunction("GetDat", "getdat", ToolTipColorBlock("<1>function <0>getdat(DatName =<2>" & DatIndex & "<0>, ParamaterName , ObjectId)" & vbCrLf & "Dat데이터를 읽어옵니다."))
        RegFunction("ResetDat", "resetdat", ToolTipColorBlock("<1>function <0>resetdat(DatName =<2>" & DatIndex & "<0>, ParamaterName , ObjectId)" & vbCrLf & "Dat데이터를 초기화 합니다."))


        RegFunction("GetCodeLabel", "getcodelabel", ToolTipColorBlock("<1>function <0>getcodelabel(DatName, ObjectId)" & vbCrLf & "오브젝트의 이름을 읽어옵니다."))
        RegFunction("GetCodeCount", "getcodecount", ToolTipColorBlock("<1>function <0>getcodecount(DatName)" & vbCrLf & "각 코드의 총 갯수를 반환합니다."))
        RegFunction("GetDatName", "getdatname", ToolTipColorBlock("<1>function <0>getdatname(Index)" & vbCrLf & "해당 DatFile의 이름을 가져옵니다."))


        RegFunction("SetToolTip", "settooltip", ToolTipColorBlock("<1>function <0>settooltip(DatName, ObjectId, Value)" & vbCrLf & "ToolTip테이터를 수정합니다."))
        RegFunction("GetToolTip", "gettooltip", ToolTipColorBlock("<1>function <0>gettooltip(DatName, ObjectId)" & vbCrLf & "ToolTip테이터를 읽어옵니다."))
        RegFunction("ResetToolTip", "resettooltip", ToolTipColorBlock("<1>function <0>resettooltip(DatName, ObjectId)" & vbCrLf & "ToolTip테이터를 초기화 합니다"))


        RegFunction("SetGroup", "setgroup", ToolTipColorBlock("<1>function <0>setgroup(DatName, ObjectId, Value)" & vbCrLf & "Group데이터를 수정합니다."))
        RegFunction("GetGroup", "getgroup", ToolTipColorBlock("<1>function <0>getgroup(DatName, ObjectId)" & vbCrLf & "Group데이터를 읽어옵니다."))
        RegFunction("ResetGroup", "resetgroup", ToolTipColorBlock("<1>function <0>resetgroup(DatName, ObjectId)" & vbCrLf & "Group데이터를 초기화 합니다."))
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
    Private TooltipColorTable() As SolidColorBrush = {
        Application.Current.Resources("MaterialDesignBackground"),
        New SolidColorBrush(Color.FromRgb(86, 156, 214)),
         New SolidColorBrush(Color.FromRgb(128, 193, 132)),
         New SolidColorBrush(Color.FromRgb(72, 180, 142))}
    Private Function ToolTipColorBlock(textstr As String) As TextBlock
        Dim TextBlcck As New TextBlock

        Dim inlines As InlineCollection = TextBlcck.Inlines
        inlines.Clear()
        Dim MainText As String = textstr '.Replace(vbCrLf, "<A>")


        Dim rgx As New Text.RegularExpressions.Regex("<([A-Za-z0-9])+>", Text.RegularExpressions.RegexOptions.IgnoreCase)

        Dim LastColor As Integer = 0
        Dim LastCode As Integer = 0
        Dim Startindex As Integer = 1
        For i = 0 To rgx.Matches(MainText).Count - 1
            Dim tMatch As Text.RegularExpressions.Match = rgx.Matches(MainText).Item(i)

            Dim Value As String = tMatch.Value
            Value = Mid(Value, 2, Value.Length - 2)

            Dim ColorCode As Integer = -1
            Try
                ColorCode = "&H" & Value
                If ColorCode > TooltipColorTable.Count - 1 Then
                    Continue For
                End If
            Catch ex As Exception
                Continue For
            End Try

            Dim AddedText As String = Mid(MainText, Startindex, tMatch.Index - Startindex + 1)


            Dim Run As New Run(AddedText)
            Run.Foreground = TooltipColorTable(LastColor)
            inlines.Add(Run)
            Startindex = tMatch.Index + Value.Length + 3
            LastCode = ColorCode
            If ColorCode <> -1 Then
                LastColor = ColorCode
            End If
            If LastCode = &HA Then
                inlines.Add(vbCrLf)
                LastColor = 0
            End If
        Next

        If True Then
            Dim AddedText As String = Mid(MainText, Startindex, MainText.Length - Startindex + 1)
            Dim Run As New Run(AddedText)
            Run.Foreground = TooltipColorTable(LastColor)
            inlines.Add(Run)
        End If

        Return TextBlcck
    End Function
    Private Sub RegFunction(MethodName As String, LuaFucnName As String, ToolTip As TextBlock)
        LuaSc.RegisterFunction(LuaFucnName, Me, Me.GetType().GetMethod(MethodName))
        Functions.Add(LuaFucnName)
        ToolTips.Add(ToolTip)
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



    Public Sub SetDat(DatName As String, ParamaterName As String, ObjectId As Integer, Value As Long)
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return
        End If
        pjData.BindingManager.DatBinding(Datfile, ParamaterName, ObjectId).Value = Value
    End Sub
    Public Function GetDat(DatName As String, ParamaterName As String, ObjectId As Integer) As Long
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return 0
        End If
        Return pjData.BindingManager.DatBinding(Datfile, ParamaterName, ObjectId).Value
    End Function
    Public Sub ResetDat(DatName As String, ParamaterName As String, ObjectId As Integer)
        Dim Datfile As SCDatFiles.DatFiles = GetDatFile(DatName)
        If Datfile = SCDatFiles.DatFiles.None Then
            Return
        End If
        pjData.BindingManager.DatBinding(Datfile, ParamaterName, ObjectId).DataReset()
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
