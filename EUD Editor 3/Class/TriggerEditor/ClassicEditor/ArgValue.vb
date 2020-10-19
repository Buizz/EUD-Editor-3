<Serializable>
Public Class ArgValue
    Public DefaultType As String
    Public ValueType As String
    Public ValueString As String
    Public ValueNumber As Long

    '코드로 출력될대 Number이나 String에 있는 값을 출력한다.
    Public IsArgNumber As Boolean

    '언어를 설정을 따르는지
    Public IsLangageable As Boolean

    Public IsQuotation As Boolean

    Public IsInit As Boolean


    Public Sub New(_ValueType As String)
        If _ValueType = "" Then
            _ValueType = "RawCode"
        End If


        IsInit = True
        ValueType = _ValueType
        DefaultType = _ValueType
        IsArgNumber = False
        IsLangageable = False
        IsQuotation = False
    End Sub

    Public Sub CopyTo(toArg As ArgValue)
        '해당 트리거의 내용을 toTrg에 넣는다.

        toArg.DefaultType = DefaultType
        toArg.ValueType = ValueType
        toArg.ValueString = ValueString
        toArg.ValueNumber = ValueNumber
        toArg.IsArgNumber = IsArgNumber
        toArg.IsLangageable = IsLangageable
        toArg.IsQuotation = IsQuotation
        toArg.IsInit = IsInit

        If CodeBlock IsNot Nothing Then
            If toArg.CodeBlock Is Nothing Then
                toArg.CodeBlock = CodeBlock.DeepCopy
            Else
                CodeBlock.CopyTo(toArg.CodeBlock)
            End If
        Else
            toArg.CodeBlock = Nothing
        End If
    End Sub

    Public CodeBlock As TriggerCodeBlock
End Class
