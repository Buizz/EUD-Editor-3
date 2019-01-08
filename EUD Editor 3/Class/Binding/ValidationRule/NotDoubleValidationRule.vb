Imports System.Globalization

Public Class NotDoubleValidationRule
    Inherits ValidationRule

    Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
        '일단 숫자인지 아닌지 판단.
        Try
            Dim number As Double = value
            Return ValidationResult.ValidResult
        Catch ex As Exception
            Return New ValidationResult(False, "숫자를 입력하세요")
            'Return New ValidationResult(False, value.ToString)
        End Try


    End Function
End Class
