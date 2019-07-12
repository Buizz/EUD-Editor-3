Public Class ScriptBlockItem
    Private Script As ScriptBlock
    Public Sub Init(tScript As ScriptBlock)
        Script = tScript
        TestLabel.Content = Script.TriggerScript.SName
    End Sub
End Class
