Partial Public Class TEGUIPage
    Private Function DefaultColor() As SolidColorBrush
        Return Application.Current.Resources("MaterialDesignToolBackground")
    End Function
    Private Function SelectColor() As SolidColorBrush
        Return Brushes.Red
    End Function
End Class
