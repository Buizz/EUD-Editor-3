Partial Public Class ProjectExplorer
    Private Function DefaultColor() As SolidColorBrush
        Return New SolidColorBrush(Color.FromArgb(0, 255, 255, 255)) 'Application.Current.Resources("MaterialDesignTextFieldBoxBackground")
    End Function
    Private Function SelectColor() As SolidColorBrush
        Return New SolidColorBrush(Color.FromArgb(150, 100, 100, 170))
    End Function
End Class
