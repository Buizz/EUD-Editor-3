Imports System.Globalization

Public Class EMailValidationRule
    Inherits ValidationRule

    Private AbleConst As String = ".@0123456789abcdefghijklnmopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
    Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
        '일단 숫자인지 아닌지 판단.
        Dim str As String = value

        If str Is Nothing Then
            Return New ValidationResult(False, "값은 비어있으면 안됩니다.")
        End If
        If str = "" Then
            Return New ValidationResult(False, "값은 비어있으면 안됩니다.")
        End If
        If str.Length > 40 Then
            Return New ValidationResult(False, "40글자 이하로 입력하세요.")
        End If

        If str.IndexOf("@") = -1 Then
            Return New ValidationResult(False, "올바른 이메일 형태가 아닙니다.")
        End If
        If str.IndexOf("@") <> str.LastIndexOf("@") Then
            Return New ValidationResult(False, "올바른 이메일 형태가 아닙니다.")
        End If


        If str.IndexOf(".") = -1 Then
            Return New ValidationResult(False, "올바른 이메일 형태가 아닙니다.")
        End If
        If str.IndexOf(".") <> str.LastIndexOf(".") Then
            Return New ValidationResult(False, "올바른 이메일 형태가 아닙니다.")
        End If


        If str.IndexOf("@") > str.IndexOf(".") Then
            Return New ValidationResult(False, "올바른 이메일 형태가 아닙니다.")
        End If

        For i = 0 To str.Length - 1
            If AbleConst.IndexOf(str(i)) = -1 Then
                Return New ValidationResult(False, "알파벳과 숫자를 입력하세요.")
            End If
        Next

        Return ValidationResult.ValidResult


    End Function


End Class
