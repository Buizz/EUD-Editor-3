Partial Public Class TECTPage
    Public Sub RefreshGlobalObject()
        GlobalList.Items.Clear()
        For i = 0 To Scripter.globalVar.Count - 1
            GlobalList.Items.Add(Scripter.globalVar(i))
        Next
    End Sub
End Class
