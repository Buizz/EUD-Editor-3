Partial Public Class GUIScriptEditorUI
    '"rawcode", "import", "var", "function", "funargs",
    '"funcontent", "if", "elseif", "ifcondition", "ifthen",
    '"ifelse", "for", "forcontent", "foraction", "forinit",
    '"forcondition", "forincre", "while", "whilecondition",
    '"whileaction", "switch", "switchvar", "switchcase", "or",
    '"and", "folder", "folderaction"


    Public Function CheckValidated(newscr As ScriptBlock, destscr As ScriptBlock) As Boolean
        'dest에 new가 들어갈 수 있는지 판단

        If destscr Is Nothing Then
            '함수 및 선언문만 가능
            'MsgBox("넣는 블럭 : " & newscr.name)
            Select Case newscr.ScriptType
                Case ScriptBlock.EBlockType.rawcode, ScriptBlock.EBlockType.import, ScriptBlock.EBlockType.vardefine,
                     ScriptBlock.EBlockType.fundefine, ScriptBlock.EBlockType.folder, ScriptBlock.EBlockType.objectdefine
                Case Else
                    SnackBarDialog(Tool.GetText("TE_SnackBar1"))
                    Return False
            End Select

        Else
            If newscr.ScriptType = ScriptBlock.EBlockType.fundefine Then
                If destscr.ScriptType <> ScriptBlock.EBlockType.objectmethod Or destscr.ScriptType <> ScriptBlock.EBlockType.folderaction Then
                    SnackBarDialog(Tool.GetText("TE_SnackBar2"))
                    Return False
                End If
            End If
            If newscr.ScriptType = ScriptBlock.EBlockType.objectdefine Then
                SnackBarDialog(Tool.GetText("TE_SnackBar3"))
                Return False
            End If
            If newscr.ScriptType = ScriptBlock.EBlockType.import Then
                SnackBarDialog(Tool.GetText("TE_SnackBar4"))
                Return False
            End If
            Select Case destscr.ScriptType
                Case ScriptBlock.EBlockType.funargs
                    If Not newscr.IsArgument() Then
                        SnackBarDialog(Tool.GetText("TE_SnackBar5"))
                        Return False
                    End If
                Case ScriptBlock.EBlockType.switch
                    If newscr.ScriptType <> ScriptBlock.EBlockType.switchcase Then
                        SnackBarDialog(Tool.GetText("TE_SnackBar6"))
                        Return False
                    End If
                Case ScriptBlock.EBlockType.ifcondition, ScriptBlock.EBlockType.whilecondition
                    If newscr.ScriptType = ScriptBlock.EBlockType.rawcode Or newscr.ScriptType = ScriptBlock.EBlockType._or Or
                        newscr.ScriptType = ScriptBlock.EBlockType._and Or newscr.ScriptType = ScriptBlock.EBlockType.exp Or newscr.ScriptType = ScriptBlock.EBlockType.macrofun Then
                        Return True
                    End If

                    If newscr.ScriptType = ScriptBlock.EBlockType.condition Or
                        newscr.ScriptType = ScriptBlock.EBlockType.funuse Or
                        newscr.ScriptType = ScriptBlock.EBlockType.plibfun Or
                        newscr.ScriptType = ScriptBlock.EBlockType.externfun Then
                        Return True
                    Else
                        SnackBarDialog(Tool.GetText("TE_SnackBar7"))
                        Return False
                    End If
                Case ScriptBlock.EBlockType.objectmethod
                    If newscr.ScriptType <> ScriptBlock.EBlockType.fundefine Then
                        SnackBarDialog(Tool.GetText("TE_SnackBar8"))
                        Return False
                    End If
                Case ScriptBlock.EBlockType.objectfields
                    If newscr.ScriptType <> ScriptBlock.EBlockType.vardefine Then
                        SnackBarDialog(Tool.GetText("TE_SnackBar9"))
                        Return False
                    End If
            End Select
            Select Case newscr.ScriptType
                Case ScriptBlock.EBlockType.funargblock
                    Dim pname As String = destscr.Parent.value
                    If pname = "onPluginStart" Or pname = "beforeTriggerExec" Or pname = "afterTriggerExec" Then
                        SnackBarDialog(Tool.GetText("TE_SnackBar10"))
                        Return False
                    End If

                    If destscr.ScriptType <> ScriptBlock.EBlockType.funargs Then
                        SnackBarDialog(Tool.GetText("TE_SnackBar11"))
                        Return False
                    End If
                Case ScriptBlock.EBlockType.switchcase
                    If destscr.ScriptType <> ScriptBlock.EBlockType.switch Then
                        SnackBarDialog(Tool.GetText("TE_SnackBar12"))
                        Return False
                    End If
            End Select

            If newscr.ScriptType = ScriptBlock.EBlockType.condition Then
                Select Case destscr.ScriptType
                    Case ScriptBlock.EBlockType.ifcondition, ScriptBlock.EBlockType.whilecondition, ScriptBlock.EBlockType._or, ScriptBlock.EBlockType._and
                        Return True
                    Case Else
                        SnackBarDialog(Tool.GetText("TE_SnackBar13"))
                        Return False
                End Select
            End If




            'MsgBox("넣는 블럭 : " & newscr.name & vbCrLf & "해당 위치 : " & destscr.name)
        End If




        Return True
    End Function
End Class
