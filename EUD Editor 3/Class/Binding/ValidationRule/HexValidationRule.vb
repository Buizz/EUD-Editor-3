Imports System.Globalization

Public Class HexValidationRule
    Inherits ValidationRule

    Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
        '일단 숫자인지 아닌지 판단.
        Try
            Dim number As Long = "&H" & value
            Return ValidationResult.ValidResult
        Catch ex As Exception
            Return New ValidationResult(False, "16진수를 입력하세요")
            'Return New ValidationResult(False, value.ToString)
        End Try


    End Function
End Class
