Imports System.Globalization

Public Class TextValidationRule
    Inherits ValidationRule
    Private notAbleConst As String = "\/:*?""'<>|"
    Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
        Dim str As String = value

        If str Is Nothing Then
            Return New ValidationResult(False, "값은 비어있으면 안됩니다.")
        End If
        If str = "" Then
            Return New ValidationResult(False, "값은 비어있으면 안됩니다.")
        End If
        If str.Length > 20 Then
            Return New ValidationResult(False, "20글자 이하로 입력하세요.")
        End If


        For i = 0 To str.Length - 1
            If notAbleConst.IndexOf(str(i)) >= 0 Then
                Return New ValidationResult(False, "\ / : * ? "" ' < > | 는 입력 할 수 없습니다.")
            End If
        Next

        Return ValidationResult.ValidResult
    End Function
End Class
