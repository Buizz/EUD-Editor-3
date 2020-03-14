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
            Select Case newscr.name
                Case "rawcode", "import", "var", "function", "folder"






                Case Else
                    SnackBarDialog("선언문만 들어 갈 수 있습니다.")
                    Return False
            End Select

        Else
            If newscr.name = "function" Then
                SnackBarDialog("함수 선언은 외부에 할 수 있습니다.")
                Return False
            End If
            If newscr.name = "import" Then
                SnackBarDialog("임포트는 외부에 할 수 있습니다.")
                Return False
            End If


            Select Case destscr.name
                Case "funargs"


                    If Not newscr.IsArgument() Then
                        SnackBarDialog("함수 인자만 들어 갈 수 있습니다.")
                        Return False
                    End If
                Case "switch"
                    If newscr.name <> "switchcase" Then
                        SnackBarDialog("케이스만 들어 갈 수 있습니다.")
                        Return False
                    End If
                Case "ifcondition", "ifcondition", "whilecondition"
                    If newscr.name = "rawcode" Or newscr.name = "or" Or newscr.name = "and" Then
                        Return True
                    End If

                    If newscr.IsCondition Or newscr.IsFunction Then
                        Return True
                    Else
                        SnackBarDialog("조건이나 함수만 들어 갈 수 있습니다.")
                        Return False
                    End If

            End Select
            Select Case newscr.name
                Case "funargblock"
                    Dim pname As String = destscr.Parent.value
                    If pname = "onPluginStart" Or pname = "beforeTriggerExec" Or pname = "afterTriggerExec" Then
                        SnackBarDialog("시작 함수에는 인자를 넣을 수 없습니다.")
                        Return False
                    End If

                    If destscr.name <> "funargs" Then
                        SnackBarDialog("함수 인자는 함수에 넣을 수 있습니다.")
                        Return False
                    End If
                Case "switchcase"
                    If destscr.name <> "switch" Then
                        SnackBarDialog("케이스는 스위치에만 들어 갈 수 있습니다.")
                        Return False
                    End If
            End Select

            If newscr.IsCondition Then
                Select Case destscr.name
                    Case "ifcondition", "ifcondition", "whilecondition", "or", "and"
                        Return True
                    Case Else
                        SnackBarDialog("조건을 넣을 수 없는 곳입니다.")
                        Return False
                End Select
            End If




            'MsgBox("넣는 블럭 : " & newscr.name & vbCrLf & "해당 위치 : " & destscr.name)
        End If




        Return True
    End Function
End Class
