Public Class EditDotData
    Public Sub init(data As DotData)


        Image.Source = data.GetBitMap

        Dim colors() As System.Drawing.Color = Tool.DotPaint.ColorPallet
        Dim pallets As List(Of System.Drawing.Color) = data.ImagePallet
        ColorPallet.Children.Clear()

        For i = 0 To colors.Length - 1
            Dim btn As New Button
            btn.Width = 32
            btn.Height = 32
            btn.Background = New SolidColorBrush(Color.FromRgb(colors(i).R, colors(i).G, colors(i).B))

            btn.Style = Application.Current.Resources("MaterialDesignFlatButton")

            ColorPallet.Children.Add(btn)
        Next

        ImageColors.Children.Clear()
        For i = 0 To pallets.Count - 1
            Dim btn As New Button
            btn.Width = 32
            btn.Height = 32
            btn.Background = New SolidColorBrush(Color.FromRgb(pallets(i).R, pallets(i).G, pallets(i).B))

            btn.Style = Application.Current.Resources("MaterialDesignFlatButton")

            ImageColors.Children.Add(btn)
        Next
    End Sub


    Public Sub disable()

    End Sub
End Class
