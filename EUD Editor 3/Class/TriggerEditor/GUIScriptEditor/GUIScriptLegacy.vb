Partial Public Class GUIScriptEditor
    Public Sub scrLegacyInit()
        For i = 0 To items.Count - 1
            scrLegacyRefresh(items(i))
        Next
    End Sub
    Public Sub scrLegacyRefresh(scr As ScriptBlock)
        If scr.ScriptType = ScriptBlock.EBlockType.switch Then
            Dim tscr As New ScriptBlock(ScriptBlock.EBlockType.rawcode, "rawcode", False, False, scr.value, Me)
            tscr.Parent = scr
            scr.VChild = tscr
        End If

        For i = 0 To scr.child.Count - 1
            scrLegacyRefresh(scr.child(i))
        Next
    End Sub
End Class
