Public Class CodeBlock
    Public Items As List(Of CodeBlock)

    Public Value As String
    Public Sub New()
        Items = New List(Of CodeBlock)
    End Sub
End Class
