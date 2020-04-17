Public Class GUI_VarFuncUse

    Public Sub CrlInit(_scr As ScriptBlock)
        TestLabel.Content = _scr.name & ":" & _scr.value & ":" & _scr.value2
    End Sub
End Class
