Partial Public Class GUIScriptManager

    Public Function GetGlobalObject(GUIEditor As GUIScriptEditor) As List(Of ScriptBlock)
        Dim rscr As New List(Of ScriptBlock)

        For i = 0 To GUIEditor.ItemCount - 1
            If GUIEditor.GetItems(i).ScriptType = ScriptBlock.EBlockType.objectdefine Then
                rscr.Add(GUIEditor.GetItems(i))
            End If
        Next

        Return rscr
    End Function
    Public Function GetExternObject(GUIEditor As GUIScriptEditor) As List(Of ScriptBlock)
        Dim rscr As New List(Of ScriptBlock)

        Return rscr
    End Function

    Public Function GetObjectByName(valname As String, GUIEditor As GUIScriptEditor) As ScriptBlock



        Dim scrs As New List(Of ScriptBlock)

        scrs.AddRange(GetGlobalObject(GUIEditor))
        scrs.AddRange(GetExternObject(GUIEditor))

        For i = 0 To scrs.Count - 1
            If scrs(i).value = valname Then
                Return scrs(i)
            End If
        Next


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
