Partial Public Class GUIScriptManager

    Public Function GetDefaultObject(GUIEditor As GUIScriptEditor) As List(Of ScriptBlock)
        Dim rscr As New List(Of ScriptBlock)

        Dim ObjectList As List(Of ScriptBlock) = tescm.GetObjectFromCFunc(Tool.TEEpsDefaultFunc, GUIEditor)
        rscr.AddRange(ObjectList)

        Return rscr
    End Function
    Public Function GetGlobalObject(GUIEditor As GUIScriptEditor) As List(Of ScriptBlock)
        Dim rscr As New List(Of ScriptBlock)

        For i = 0 To GUIEditor.ItemCount - 1
            If GUIEditor.GetItems(i).ScriptType = ScriptBlock.EBlockType.objectdefine Then
                rscr.Add(GUIEditor.GetItems(i))
            End If
        Next

        Return rscr
    End Function
    Public Function GetExternObject(GUIEditor As GUIScriptEditor, nspace As String) As List(Of ScriptBlock)
        Dim rscr As New List(Of ScriptBlock)


        For i = 0 To GUIEditor.ExternFile.Count - 1
            'MsgBox(GUIEditor.ExternFile(i).nameSpaceName & "," & nspace)
            If GUIEditor.ExternFile(i).nameSpaceName.Trim = nspace.Trim Then
                'MsgBox("objcount : " & GUIEditor.ExternFile(i).Funcs.ObjectCount)
                rscr.AddRange(GetObjectFromCFunc(GUIEditor.ExternFile(i).Funcs, GUIEditor))
            End If
        Next
        Return rscr
    End Function
    Public Function GetObjectFromCFunc(func As CFunc, GUIEditor As GUIScriptEditor) As List(Of ScriptBlock)
        Dim rscr As New List(Of ScriptBlock)

        For j = 0 To func.ObjectCount - 1
            Dim objname As String = func.GetObject(j).ObjName
            Dim tcfun As CFunc = func.GetObject(j).Functions

            'MsgBox(objname & " dlfma")

            Dim tscr As New ScriptBlock(ScriptBlock.EBlockType.objectdefine, "objectdefine", False, False, objname, GUIEditor)

            Dim tf As ScriptBlock = Nothing
            Dim tm As ScriptBlock = Nothing
            For k = 0 To tscr.child.Count - 1
                If tscr.child(k).ScriptType = ScriptBlock.EBlockType.objectmethod Then
                    tm = tscr.child(k)
                End If
                If tscr.child(k).ScriptType = ScriptBlock.EBlockType.objectfields Then
                    tf = tscr.child(k)
                End If
            Next

            If tf IsNot Nothing Then
                For f = 0 To tcfun.VariableCount - 1
                    Dim tv As New ScriptBlock(ScriptBlock.EBlockType.vardefine, "vardefine", False, False, tcfun.GetVariableNames(f), GUIEditor)

                    tf.AddChild(tv)
                Next
            End If

            If tm IsNot Nothing Then
                For m = 0 To tcfun.FuncCount - 1
                    Dim tv As New ScriptBlock(ScriptBlock.EBlockType.fundefine, "fundefine", False, False, tcfun.GetFuncName(m), GUIEditor)
                    tv.value2 = m
                    tv.tobject = tcfun
                    tm.AddChild(tv)
                Next
            End If

            rscr.Add(tscr)
        Next
        Return rscr
    End Function


    Public Function GetObjectByName(valname As String, GUIEditor As GUIScriptEditor, Optional nspace As String = "") As ScriptBlock
        If valname.IndexOf(".") <> -1 Then
            Dim tstr() As String = valname.Split(".")

            valname = tstr.Last
            nspace = tstr.First
        End If

        If True Then
            Dim scrs As New List(Of ScriptBlock)
            scrs.AddRange(GetObjectFromCFunc(Tool.TEEpsDefaultFunc, GUIEditor))

            For i = 0 To scrs.Count - 1
                If scrs(i).value.Trim = valname.Trim Then
                    Return scrs(i)
                End If
            Next
        End If

        If nspace = "" Then
            Dim scrs As New List(Of ScriptBlock)

            scrs.AddRange(GetGlobalObject(GUIEditor))

            For i = 0 To scrs.Count - 1
                If scrs(i).value = valname Then
                    Return scrs(i)
                End If
            Next
        Else
            Dim scrs As New List(Of ScriptBlock)

            scrs.AddRange(GetExternObject(GUIEditor, nspace))

            For i = 0 To scrs.Count - 1
                If scrs(i).value.Trim = valname.Trim Then
                    Return scrs(i)
                End If
            Next
        End If

        Return Nothing
    End Function

    Public Function GetObjectMethod(scr As ScriptBlock) As List(Of ScriptBlock)
        Dim scrs As New List(Of ScriptBlock)

        Dim methodscrlist As ScriptBlock = Nothing

        For i = 0 To scr.child.Count - 1
            If scr.child(i).ScriptType = ScriptBlock.EBlockType.objectmethod Then
                methodscrlist = scr.child(i)
                Exit For
            End If
        Next

        If methodscrlist Is Nothing Then
            Return scrs
        End If
        For i = 0 To methodscrlist.child.Count - 1
            scrs.Add(methodscrlist.child(i))
        Next


        Return scrs
    End Function
    Public Function GetObjectField(scr As ScriptBlock) As List(Of ScriptBlock)
        Dim scrs As New List(Of ScriptBlock)

        Dim fieldscrlist As ScriptBlock = Nothing

        For i = 0 To scr.child.Count - 1
            If scr.child(i).ScriptType = ScriptBlock.EBlockType.objectfields Then
                fieldscrlist = scr.child(i)
                Exit For
            End If
        Next

        If fieldscrlist Is Nothing Then
            Return scrs
        End If

        For i = 0 To fieldscrlist.child.Count - 1
            scrs.Add(fieldscrlist.child(i))
        Next


        Return scrs
    End Function
End Class
