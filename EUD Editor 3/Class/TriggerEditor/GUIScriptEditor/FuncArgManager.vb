Public Class FuncDefine
    Public IsCompleteFunction As Boolean
    Public FunNoExist As Boolean


    Public ArgStartIndex As Integer = -1
    Public ArgViewCount As Integer = 0

    Public Sub ResetScriptBlock(scr As ScriptBlock)
        scr.child.Clear()

        For i = 0 To Args.Count - 1
            Dim vname As String = "defaultvalue;" & Args(i).ArgName.Trim
            Dim vtype As String = Args(i).ArgType.Trim

            'ReplaceChild(New ScriptBlock(vtype, False, False, vname, Scripter), i)

            Dim tscr As New ScriptBlock(ScriptBlock.EBlockType.constVal, vtype, False, False, vname, scr.Scripter)
            scr.AddChild(tscr)
        Next
    End Sub

    Public ReadOnly Property ValueCount As Integer
        Get
            Return Args.Count
        End Get
    End Property

    Public Sub New()

    End Sub

    Public Sub InitMacro(luafunc As MacroManager.LuaFunction)
        FuncComment = luafunc.Fcomment

        For i = 0 To luafunc.ArgName.Count - 1
            Dim argname As String = luafunc.ArgName(i)
            If argname.Length > 0 Then
                If Mid(argname, 1, 1) = "*" Then
                    ArgStartIndex = i
                End If
            End If

            Args.Add(New FuncArgs(argname, luafunc.ArgType(i), ""))
        Next

        If Not luafunc.IsCompleteFunction Then
            IsCompleteFunction = False
            Return
        End If

        For i = 0 To luafunc.ArgLists.Count - 1
            Dim argblock As MacroManager.LuaFunction.ArgBlock = luafunc.ArgLists(i)

            If argblock.BType = MacroManager.LuaFunction.ArgBlock.BlockType.Arg Then
                ArgCommentList.Add(New FuncDefine.ArgBlock(FuncDefine.ArgBlock.BlockType.Arg, argblock.ArgIndex, ""))
                ArgViewCount += 1
            Else
                ArgCommentList.Add(New FuncDefine.ArgBlock(FuncDefine.ArgBlock.BlockType.Label, -1, argblock.Label))
            End If
        Next
    End Sub
    Public Sub InitCFunc(i As Integer, cfun As CFunc)
        Dim functooltip As FunctionToolTip = cfun.GetFuncTooltip(i)

        Dim targs() As String = cfun.GetFuncArgument(i).Split(",")
        If cfun.GetFuncArgument(i).Trim <> "" Then
            For k = 0 To targs.Count - 1
                Dim argtext As String = targs(k)

                If argtext.IndexOf("/*") <> -1 And argtext.IndexOf("*/") <> -1 And argtext.IndexOf(":") = -1 Then
                    argtext = argtext.Replace("*/", "")
                    argtext = argtext.Replace("/*", ":")
                End If

                Dim aname As String = argtext.Split(":").First.Trim
                Dim atype As String = argtext.Split(":").Last.Trim


                If aname.Length > 0 Then
                    If Mid(aname, 1, 1) = "*" Then
                        ArgStartIndex = k
                    End If
                End If


                Args.Add(New FuncArgs(aname, atype, functooltip.GetTooltip(k)))
            Next
        End If


        If functooltip.Summary.Trim = "" Then
            IsCompleteFunction = False
            Return
        End If

        FuncComment = functooltip.Summary
        Dim argumentstr As String = FuncComment.Split(vbCrLf).First
        argumentstr = FuncComment.Replace(argumentstr, "").Trim
        FuncComment = argumentstr


        For k = 0 To Args.Count - 1
            Dim argname As String = "[" & Args(k).ArgName.Trim & "]"

            argumentstr = argumentstr.Replace(argname, "ᐱᐳ" & k & "ᐱ")
        Next

        Dim argblock() As String = argumentstr.Split("ᐱ")

        For k = 0 To argblock.Count - 1
            If argblock(k).Trim <> "" Then
                If Mid(argblock(k), 1, 1) = "ᐳ" Then
                    Dim argindex As String = Mid(argblock(k), 2)
                    If IsNumeric(argindex) Then
                        ArgCommentList.Add(New FuncDefine.ArgBlock(FuncDefine.ArgBlock.BlockType.Arg, argindex, ""))
                        ArgViewCount += 1
                        Continue For
                    End If
                End If
            End If

            ArgCommentList.Add(New FuncDefine.ArgBlock(FuncDefine.ArgBlock.BlockType.Label, -1, argblock(k)))
        Next
    End Sub
    Public Sub InitScript(func As ScriptBlock)
        Dim argumentstr As String = func.GetFuncTooltip
        FuncComment = argumentstr

        Dim argsb As List(Of ScriptBlock) = tescm.GetFuncArgs(func)

        For i = 0 To argsb.Count - 1
            Dim argtypr As String = argsb(i).name
            Dim argname As String = argsb(i).value
            Dim sargtooltip As String = argsb(i).value2


            If argname.Length > 0 Then
                If Mid(argname, 1, 1) = "*" Then
                    ArgStartIndex = i
                End If
            End If


            Args.Add(New FuncArgs(argname, argtypr, sargtooltip))
        Next
        If argumentstr.Trim = "" Then
            IsCompleteFunction = False
            Return
        End If

        For k = 0 To Args.Count - 1
            Dim argname As String = "[" & Args(k).ArgName.Trim & "]"

            argumentstr = argumentstr.Replace(argname, "ᐱᐳ" & k & "ᐱ")
        Next

        Dim argblock() As String = argumentstr.Split("ᐱ")


        For k = 0 To argblock.Count - 1
            If argblock(k).Trim <> "" Then
                If Mid(argblock(k), 1, 1) = "ᐳ" Then
                    Dim argindex As String = Mid(argblock(k), 2)
                    If IsNumeric(argindex) Then
                        ArgCommentList.Add(New FuncDefine.ArgBlock(FuncDefine.ArgBlock.BlockType.Arg, argindex, ""))
                        ArgViewCount += 1
                        Continue For
                    End If
                End If
            End If

            ArgCommentList.Add(New FuncDefine.ArgBlock(FuncDefine.ArgBlock.BlockType.Label, -1, argblock(k)))
        Next
    End Sub
    Public Sub New(scr As ScriptBlock)
        IsCompleteFunction = True
        FunNoExist = False
        FName = scr.name
        Select Case scr.ScriptType
            Case ScriptBlock.EBlockType.macrofun
                Dim luafunc As MacroManager.LuaFunction = macro.GetFunction(scr.name)
                If luafunc Is Nothing Then
                    IsCompleteFunction = False
                    FunNoExist = True
                    Return
                End If

                InitMacro(luafunc)
            Case ScriptBlock.EBlockType.funuse
                Dim func As ScriptBlock = tescm.GetFuncInfor(scr.name, scr.Scripter)
                If func Is Nothing Then
                    IsCompleteFunction = False
                    FunNoExist = True
                    Return
                End If

                InitScript(func)
            Case ScriptBlock.EBlockType.plibfun, ScriptBlock.EBlockType.action, ScriptBlock.EBlockType.condition, ScriptBlock.EBlockType.externfun
                Dim i As Integer = Tool.TEEpsDefaultFunc.SearchFunc(scr.name)
                Dim cfun As CFunc = Nothing
                If i >= 0 Then
                    cfun = Tool.TEEpsDefaultFunc
                Else
                    Dim Scripter As GUIScriptEditor = scr.Scripter

                    Dim sname As String = scr.name

                    Dim _namespace As String = sname.Split(".").First
                    Dim funcname As String = sname.Split(".").Last
                    For k = 0 To Scripter.ExternFile.Count - 1
                        If _namespace = Scripter.ExternFile(k).nameSpaceName Then
                            i = Scripter.ExternFile(k).Funcs.SearchFunc(funcname)
                            cfun = Scripter.ExternFile(k).Funcs
                            Exit For
                        End If
                    Next
                End If

                If cfun IsNot Nothing Then
                    InitCFunc(i, cfun)
                Else
                    IsCompleteFunction = False
                    FunNoExist = True
                    Return
                End If
        End Select
    End Sub



    Public FName As String
    Public FuncComment As String


    Public Args As New List(Of FuncArgs)
    Public ArgCommentList As New List(Of ArgBlock)
    Public Structure ArgBlock
        Public Enum BlockType
            Label
            Arg
        End Enum
        Public BType As BlockType
        Public ArgIndex As Integer
        Public Label As String
        Public Sub New(_BType As BlockType, _ArgIndex As Integer, _Label As String)
            BType = _BType
            ArgIndex = _ArgIndex
            Label = _Label.Trim
        End Sub
    End Structure
    Public Class FuncArgs
        Public ArgName As String
        Public ArgType As String
        Public ArgComment As String

        Public Sub New(_ArgName As String, _ArgType As String, _ArgComment As String)
            ArgName = _ArgName
            ArgType = _ArgType
            ArgComment = _ArgComment
        End Sub
    End Class
End Class
