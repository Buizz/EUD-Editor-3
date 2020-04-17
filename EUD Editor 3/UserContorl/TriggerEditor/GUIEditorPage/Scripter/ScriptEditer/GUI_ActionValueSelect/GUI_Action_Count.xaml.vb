Public Class GUI_Action_Count
    Public Event SelectEvent As RoutedEventHandler

    Private isUseAll As Boolean
    Public Sub New(initvalue As String, tisUseAll As Boolean)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        isUseAll = tisUseAll
        If Not isUseAll Then
            ChBox.Visibility = Visibility.Collapsed
        End If


        If IsNumeric(initvalue) Then
            If isUseAll Then
                If initvalue = 0 Then
                    ChBox.IsChecked = True
                    countVal.IsEnabled = False
                    countVal.Text = 1
                Else
                    countVal.Text = initvalue
                End If
            Else
                countVal.Text = initvalue
            End If

        Else
            If isUseAll Then
                countVal.Text = 1
                If initvalue = "All" Then
                    ChBox.IsChecked = True
                    countVal.IsEnabled = False
                End If
            Else
                countVal.Text = 0
            End If
        End If
        isLoad = True
    End Sub

    Private isLoad As Boolean = False
    Private Sub countVal_TextChanged(sender As Object, e As TextChangedEventArgs)
        If isLoad Then
            If IsNumeric(countVal.Text) Then
                If isUseAll Then
                    If countVal.Text = 0 Then
                        ChBox.IsChecked = True
                        Return
                    End If
                End If

                RaiseEvent SelectEvent(countVal.Text, e)
            End If
        End If
    End Sub

    Private Sub ChBox_Checked(sender As Object, e As RoutedEventArgs)
        If isLoad Then
            countVal.IsEnabled = False
            RaiseEvent SelectEvent("All", e)
        End If
    End Sub

    Private Sub ChBox_Unchecked(sender As Object, e As RoutedEventArgs)
        If isLoad Then
            countVal.IsEnabled = True
            RaiseEvent SelectEvent(countVal.Text, e)
        End If
    End Sub

    Private Sub countVal_MouseWheel(sender As Object, e As MouseWheelEventArgs)
        Dim v As Integer = 0
        If Not IsNumeric(countVal.Text) Then
            countVal.Text = 0
            Return
        Else
            v = countVal.Text
        End If
        If e.Delta > 0 Then
            countVal.Text = v + 1
        Else
            countVal.Text = v - 1
        End If
    End Sub
End Class
