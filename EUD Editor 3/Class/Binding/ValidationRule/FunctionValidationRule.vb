Imports System.Globalization
Imports System.ComponentModel

Public Class FunctionValidationRule
    Inherits ValidationRule

    Private sbitem As ScriptBlockItem

    Public Sub New(tsbitem As ScriptBlockItem)
        sbitem = tsbitem
    End Sub

    Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
        '일단 숫자인지 아닌지 판단.
        Dim str As String = value

        Dim rstr As String = sbitem.parrentScript.CheckvalidationnameFunc(value)


        If rstr = "" Then
            Return ValidationResult.ValidResult

        Else
            Return New ValidationResult(False, rstr)
        End If
    End Function
End Class


Public Class FunctionNameBinding
    Implements INotifyPropertyChanged



    Private sbitem As ScriptBlockItem

    Public Sub New(tsbitem As ScriptBlockItem)
        sbitem = tsbitem
    End Sub


    Public Property FunctionName() As String
        Get
            Return sbitem.Script.Value
        End Get
        Set(value As String)

            Dim script As ScriptBlock = sbitem.Script


            If sbitem.parrentScript.CheckvalidationnameFunc(value) = "" Then
                script.Value = value
                sbitem.parrentScript.ObjectSelecter.ToolBoxListRefresh("Func")
            End If


        End Set
    End Property



    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class