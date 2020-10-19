Partial Public Class TECTPage
    Public Function GetTrg(listbox As ListBoxItem) As Trigger
        Return CType(listbox.Content, TriggerBlock).trg
    End Function
    Public Function GetTrgBlock(listbox As ListBoxItem) As TriggerBlock
        Return listbox.Content
    End Function
End Class
