Partial Public Class ConsoleTextbox
    Private Sub LoadAutocmp(data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData), funcname As String, argumentindex As Integer, str As String)
        Dim ArgumentDefien As String = LuaManager.GetArgumentDefine(funcname, argumentindex)

        Select Case ArgumentDefien
            Case "datfile"
                Dim DatFileList() As String = {"units", "weapons", "flingy", "sprites", "images", "upgrades", "techdata", "orders",
                    "wireframe", "buttonSet", "statusinfor"}


                For DatFiles = 0 To DatFileList.Count - 1
                    data.Add(New DataEditCompletionData(DatFileList(DatFiles), Tool.TextColorBlock(DatFileList(DatFiles)), ConsoleText, DataEditCompletionData.EIconType.SettingValue))
                Next
                For i = 0 To LuaManager.Functions.Count - 1
                    data.Add(New DataEditCompletionData(LuaManager.Functions(i), LuaManager.ToolTips(i), ConsoleText, DataEditCompletionData.EIconType.Funcname))
                Next
            Case "param"
                Dim fvalue As String = GetnumArgument(str, 0)
                Dim datindex As Integer = Datfilesname.ToList.IndexOf(fvalue)
                If datindex <> -1 Then
                    If SCDatFiles.CheckValidDat(datindex) Then
                        For i = 0 To pjData.Dat.GetDatFile(datindex).ParamaterList.Count - 1
                            Dim Paramname As String = pjData.Dat.GetDatFile(datindex).ParamaterList(i).GetParamname
                            Paramname = Paramname.Replace(" ", "_")
                            data.Add(New DataEditCompletionData(Paramname, Tool.TextColorBlock(Datfilesname(datindex) & " " & Paramname), ConsoleText, DataEditCompletionData.EIconType.SettingValue))
                        Next
                    Else
                        Select Case fvalue
                            Case "wireframe"
                                data.Add(New DataEditCompletionData("wire", Tool.TextColorBlock(Datfilesname(datindex) & " wire"), ConsoleText, DataEditCompletionData.EIconType.SettingValue))
                                data.Add(New DataEditCompletionData("grp", Tool.TextColorBlock(Datfilesname(datindex) & " grp"), ConsoleText, DataEditCompletionData.EIconType.SettingValue))
                                data.Add(New DataEditCompletionData("tran", Tool.TextColorBlock(Datfilesname(datindex) & " tran"), ConsoleText, DataEditCompletionData.EIconType.SettingValue))
                            Case "buttonSet"
                                data.Add(New DataEditCompletionData("ButtonSet", Tool.TextColorBlock(Datfilesname(datindex) & " ButtonSet"), ConsoleText, DataEditCompletionData.EIconType.SettingValue))
                            Case "statusinfor"
                                data.Add(New DataEditCompletionData("Status", Tool.TextColorBlock(Datfilesname(datindex) & " Status"), ConsoleText, DataEditCompletionData.EIconType.SettingValue))
                                data.Add(New DataEditCompletionData("Display", Tool.TextColorBlock(Datfilesname(datindex) & " Display"), ConsoleText, DataEditCompletionData.EIconType.SettingValue))

                        End Select
                    End If


                End If
                For i = 0 To LuaManager.Functions.Count - 1
                    data.Add(New DataEditCompletionData(LuaManager.Functions(i), LuaManager.ToolTips(i), ConsoleText, DataEditCompletionData.EIconType.Funcname))
                Next
            Case "objectid"
                Dim fvalue As String = GetnumArgument(str, 0)


                For i = 0 To LuaManager.Functions.Count - 1
                    data.Add(New DataEditCompletionData(LuaManager.Functions(i), LuaManager.ToolTips(i), ConsoleText, DataEditCompletionData.EIconType.Funcname))
                Next
            Case "tblid"
                For i = 0 To SCtbltxtCount - 1
                    Dim tstr As String = pjData.BindingManager.StatTxtBinding(i).Value

                    data.Add(New DataEditCompletionData(tstr, Tool.TextColorBlock(tstr), ConsoleText, DataEditCompletionData.EIconType.SettingValue))
                Next
                For i = 0 To LuaManager.Functions.Count - 1
                    data.Add(New DataEditCompletionData(LuaManager.Functions(i), LuaManager.ToolTips(i), ConsoleText, DataEditCompletionData.EIconType.Funcname))
                Next
            Case Else
                For i = 0 To LuaManager.Functions.Count - 1
                    data.Add(New DataEditCompletionData(LuaManager.Functions(i), LuaManager.ToolTips(i), ConsoleText, DataEditCompletionData.EIconType.Funcname))
                Next


                For i = 0 To LuaManager.Propertys.Count - 1
                    data.Add(New DataEditCompletionData(LuaManager.Propertys(i), LuaManager.PropertyToolTips(i), ConsoleText, DataEditCompletionData.EIconType.SettingValue))
                Next
                For i = 0 To LuaManager.LuaKeyWord.Count - 1
                    data.Add(New DataEditCompletionData(LuaManager.LuaKeyWord(i), Nothing, ConsoleText, DataEditCompletionData.EIconType.KeyWord))
                Next
        End Select



    End Sub

    Private Function GetnumArgument(func As String, argindex As Integer) As String
        Dim tstr As String = func.Split("(").Last
        tstr = tstr.Replace(")", "")

        Dim argmentsv As String() = tstr.Split(",")
        If argmentsv.Count > argindex Then
            Return argmentsv(argindex).Replace("""", "")
        Else
            Return ""
        End If


    End Function

End Class
