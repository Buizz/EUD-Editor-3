Public Class CodeBlock
    Public BType As Parser.CodeType


    Public Items As List(Of CodeBlock)


    Public comment As CodeBlock


    Public Value1 As String
    Public Value2 As String
    Public Value3 As String
    Public Sub New(_Type As Parser.CodeType)
        BType = _Type
        Items = New List(Of CodeBlock)
    End Sub
End Class
