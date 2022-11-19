Public Class ColorPicker
    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        Dim LinearGradientBrush As New LinearGradientBrush
        LinearGradientBrush.StartPoint = New Point(0.5, 0)
        LinearGradientBrush.EndPoint = New Point(0.5, 1)
        LinearGradientBrush.GradientStops.Add(New GradientStop(Color.FromArgb(255, 255, 0, 0), 0.02))
        LinearGradientBrush.GradientStops.Add(New GradientStop(Color.FromArgb(255, 255, 255, 0), 0.167))
        LinearGradientBrush.GradientStops.Add(New GradientStop(Color.FromArgb(255, 0, 255, 0), 0.334))
        LinearGradientBrush.GradientStops.Add(New GradientStop(Color.FromArgb(255, 0, 255, 255), 0.501))
        LinearGradientBrush.GradientStops.Add(New GradientStop(Color.FromArgb(255, 0, 0, 255), 0.668))
        LinearGradientBrush.GradientStops.Add(New GradientStop(Color.FromArgb(255, 255, 0, 255), 0.835))
        LinearGradientBrush.GradientStops.Add(New GradientStop(Color.FromArgb(255, 255, 0, 0), 0.975))
        HueBar.Background = LinearGradientBrush

        TextBoxuseable = True
    End Sub

    Public Sub InitColor(color As Color)
        SVPanelDrag = False

        FristColor.Background = New SolidColorBrush(color)

        R = color.R
        G = color.G
        B = color.B
        A = color.A

        RText.Text = R
        GText.Text = G
        BText.Text = B
        AText.Text = A

        ColorToHSV(color, Hue, Saturation, Value)

        Dim RealWidth As Double = SVPanel.ActualWidth
        Dim RealHeight As Double = SVPanel.ActualHeight
        PickerStylus.Margin = New Thickness(Saturation * RealWidth - 7, (1 - Value) * RealHeight - 7, 0, 0)

        ColorHSVRefresh()
    End Sub

    Public Event ColorSelect As RoutedEventHandler

    Property PHue As Double
    Private Property Hue As Double
        Get
            Return PHue
        End Get
        Set(value As Double)
            PHue = value
            Dim RealHeight As Double = HueBar.ActualHeight
            HueBarGage.Margin = New Thickness(0, (((Hue / 360) * RealHeight) * 2 - RealHeight - 6), 0, 0)
            BaseColor.Background = New SolidColorBrush(ColorFromHSV(Hue, 1, 1))
        End Set
    End Property
    Private Saturation As Double
    Private Value As Double


    Private PR As Byte
    Private PG As Byte
    Private PB As Byte
    Private PA As Byte
    Private Property R As Byte
        Get
            Return PR
        End Get
        Set(value As Byte)
            PR = value
        End Set
    End Property
    Private Property G As Byte
        Get
            Return PG
        End Get
        Set(value As Byte)
            PG = value
        End Set
    End Property
    Private Property B As Byte
        Get
            Return PB
        End Get
        Set(value As Byte)
            PB = value
        End Set
    End Property
    Private Property A As Byte
        Get
            Return PA
        End Get
        Set(value As Byte)
            PA = value
        End Set
    End Property


    Private Sub ColorHSVRefresh()
        Dim MainColor As Color = ColorFromHSV(Hue, Saturation, Value)
        TextBoxuseable = False
        PR = MainColor.R
        PG = MainColor.G
        PB = MainColor.B
        RText.Text = PR
        GText.Text = PG
        BText.Text = PB


        TextBoxuseable = True

        CurrentColor.Background = New SolidColorBrush(MainColor)
    End Sub

    Private Sub ColorRGBRefresh()
        Dim MainColor As Color = Color.FromRgb(R, G, B)

        ColorToHSV(MainColor, Hue, Saturation, Value)
        'ColorHSVRefresh()
        Dim RealWidth As Double = SVPanel.ActualWidth
        Dim RealHeight As Double = SVPanel.ActualHeight
        PickerStylus.Margin = New Thickness(Saturation * RealWidth - 7, (1 - Value) * RealHeight - 7, 0, 0)

        CurrentColor.Background = New SolidColorBrush(MainColor)

        LastColor.Background = New SolidColorBrush(Color.FromArgb(A, R, G, B))
        RaiseEvent ColorSelect(Color.FromArgb(A, R, G, B), New RoutedEventArgs())
    End Sub

    Private HueBarDrag As Boolean = False
    Private Sub HueBar_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Dim RealHeight As Double = HueBar.ActualHeight

        Hue = (e.GetPosition(HueBar).Y / RealHeight) * 360

        If Hue < 0 Then
            Hue = 0
        End If
        If Hue > 360 Then
            Hue = 360
        End If
        ColorHSVRefresh()
        'MsgBox(Hue & " " & ColorFromHSV(Hue, 1, 1).ToString)
        HueBarDrag = True
    End Sub

    Private Sub HueBar_PreviewMouseMove(sender As Object, e As MouseEventArgs)
    End Sub

    Private Sub HueBar_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        LastColor.Background = New SolidColorBrush(Color.FromArgb(A, R, G, B))
        RaiseEvent ColorSelect(Color.FromArgb(A, R, G, B), e)
        HueBarDrag = False
    End Sub

    Private SVPanelDrag As Boolean = False
    Private Sub SVPanel_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Dim RealWidth As Double = SVPanel.ActualWidth
        Dim RealHeight As Double = SVPanel.ActualHeight

        Saturation = e.GetPosition(SVPanel).X / RealWidth
        Value = 1 - e.GetPosition(SVPanel).Y / RealHeight

        PickerStylus.Margin = New Thickness(Saturation * RealWidth - 7, (1 - Value) * RealHeight - 7, 0, 0)
        ColorHSVRefresh()

        SVPanelDrag = True
    End Sub

    Private Sub SVPanel_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        LastColor.Background = New SolidColorBrush(Color.FromArgb(A, R, G, B))
        RaiseEvent ColorSelect(Color.FromArgb(A, R, G, B), e)
        SVPanelDrag = False
    End Sub

    Private Sub SVPanel_PreviewMouseMove(sender As Object, e As MouseEventArgs)

    End Sub




    Public Shared Sub ColorToHSV(ByVal tcolor As Color, ByRef hue As Double, ByRef saturation As Double, ByRef value As Double)
        Dim color As System.Drawing.Color = System.Drawing.Color.FromArgb(tcolor.A, tcolor.R, tcolor.G, tcolor.B)


        Dim max As Integer = Math.Max(color.R, Math.Max(color.G, color.B))
        Dim min As Integer = Math.Min(color.R, Math.Min(color.G, color.B))
        hue = color.GetHue()
        saturation = If((max = 0), 0, 1.0R - (1.0R * min / max))
        value = max / 255.0R
    End Sub

    Public Shared Function ColorFromHSV(ByVal hue As Double, ByVal saturation As Double, ByVal value As Double) As Color
        Dim hi As Integer = Convert.ToInt32(Math.Floor(hue / 60)) Mod 6
        Dim f As Double = hue / 60 - Math.Floor(hue / 60)
        value = value * 255
        Dim v As Integer = Convert.ToInt32(value)
        Dim p As Integer = Convert.ToInt32(value * (1 - saturation))
        Dim q As Integer = Convert.ToInt32(value * (1 - f * saturation))
        Dim t As Integer = Convert.ToInt32(value * (1 - (1 - f) * saturation))

        If hi = 0 Then
            Return Color.FromArgb(255, v, t, p)
        ElseIf hi = 1 Then
            Return Color.FromArgb(255, q, v, p)
        ElseIf hi = 2 Then
            Return Color.FromArgb(255, p, v, t)
        ElseIf hi = 3 Then
            Return Color.FromArgb(255, p, q, v)
        ElseIf hi = 4 Then
            Return Color.FromArgb(255, t, p, v)
        Else
            Return Color.FromArgb(255, v, p, q)
        End If
    End Function

    Private TextBoxuseable As Boolean = False
    Private Sub RText_TextChanged(sender As Object, e As KeyEventArgs)
        If TextBoxuseable And e.Key = Key.Enter Then
            Dim tlong As Long
            Try
                tlong = RText.Text
            Catch ex As Exception
                RText.Text = R
                Exit Sub
            End Try
            If tlong > 255 Then
                RText.Text = 255
                tlong = 255
            End If
            If tlong < 0 Then
                RText.Text = 0
                tlong = 0
            End If

            R = tlong
            ColorRGBRefresh()
        End If
    End Sub

    Private Sub GText_TextChanged(sender As Object, e As KeyEventArgs)

        If TextBoxuseable And e.Key = Key.Enter Then
            Dim tlong As Long
            Try
                tlong = GText.Text
            Catch ex As Exception
                GText.Text = G
                Exit Sub
            End Try
            If tlong > 255 Then
                GText.Text = 255
                tlong = 255
            End If
            If tlong < 0 Then
                GText.Text = 0
                tlong = 0
            End If

            G = tlong
            ColorRGBRefresh()
        End If
    End Sub

    Private Sub BText_TextChanged(sender As Object, e As KeyEventArgs)

        If TextBoxuseable And e.Key = Key.Enter Then
            Dim tlong As Long
            Try
                tlong = BText.Text
            Catch ex As Exception
                BText.Text = B
                Exit Sub
            End Try
            If tlong > 255 Then
                BText.Text = 255
                tlong = 255
            End If
            If tlong < 0 Then
                BText.Text = 0
                tlong = 0
            End If

            B = tlong
            ColorRGBRefresh()
        End If
    End Sub

    Private Sub AText_TextChanged(sender As Object, e As KeyEventArgs)
        If TextBoxuseable And e.Key = Key.Enter Then
            Dim tlong As Long
            Try
                tlong = AText.Text
            Catch ex As Exception
                AText.Text = A
                Exit Sub
            End Try
            If tlong > 255 Then
                AText.Text = 255
                tlong = 255
            End If
            If tlong < 0 Then
                AText.Text = 0
                tlong = 0
            End If

            A = tlong
            ColorRGBRefresh()
        End If

    End Sub

    Private Sub RText_LostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        If TextBoxuseable Then
            Dim tlong As Long
            Try
                tlong = RText.Text
            Catch ex As Exception
                RText.Text = R
                Exit Sub
            End Try
            If tlong > 255 Then
                RText.Text = 255
                tlong = 255
            End If
            If tlong < 0 Then
                RText.Text = 0
                tlong = 0
            End If

            R = tlong
            ColorRGBRefresh()
        End If
    End Sub

    Private Sub GText_LostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        If TextBoxuseable Then
            Dim tlong As Long
            Try
                tlong = GText.Text
            Catch ex As Exception
                GText.Text = G
                Exit Sub
            End Try
            If tlong > 255 Then
                GText.Text = 255
                tlong = 255
            End If
            If tlong < 0 Then
                GText.Text = 0
                tlong = 0
            End If

            G = tlong
            ColorRGBRefresh()
        End If
    End Sub

    Private Sub BText_LostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        If TextBoxuseable Then
            Dim tlong As Long
            Try
                tlong = BText.Text
            Catch ex As Exception
                BText.Text = B
                Exit Sub
            End Try
            If tlong > 255 Then
                BText.Text = 255
                tlong = 255
            End If
            If tlong < 0 Then
                BText.Text = 0
                tlong = 0
            End If

            B = tlong
            ColorRGBRefresh()
        End If
    End Sub

    Private Sub AText_LostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        If TextBoxuseable Then
            Dim tlong As Long
            Try
                tlong = AText.Text
            Catch ex As Exception
                AText.Text = A
                Exit Sub
            End Try
            If tlong > 255 Then
                AText.Text = 255
                tlong = 255
            End If
            If tlong < 0 Then
                AText.Text = 0
                tlong = 0
            End If
            A = tlong
        End If

    End Sub

    Private Sub UserControl_PreviewMouseMove(sender As Object, e As MouseEventArgs)
        If SVPanelDrag Then
            Dim RealWidth As Double = SVPanel.ActualWidth
            Dim RealHeight As Double = SVPanel.ActualHeight

            Saturation = e.GetPosition(SVPanel).X / RealWidth
            Value = 1 - e.GetPosition(SVPanel).Y / RealHeight

            If Saturation < 0 Then
                Saturation = 0
            End If
            If Saturation > 1 Then
                Saturation = 1
            End If
            If Value < 0 Then
                Value = 0
            End If
            If Value > 1 Then
                Value = 1
            End If

            PickerStylus.Margin = New Thickness(Saturation * RealWidth - 7, (1 - Value) * RealHeight - 7, 0, 0)
            ColorHSVRefresh()
        End If
        If HueBarDrag Then
            Dim RealHeight As Double = HueBar.ActualHeight

            Hue = (e.GetPosition(HueBar).Y / RealHeight) * 360
            If Hue < 0 Then
                Hue = 0
            End If
            If Hue > 360 Then
                Hue = 360
            End If

            ColorHSVRefresh()
            'MsgBox(Hue & " " & ColorFromHSV(Hue, 1, 1).ToString)
        End If
    End Sub

    Private Sub UserControl_PreviewMouseUp(sender As Object, e As MouseButtonEventArgs)
        If SVPanelDrag Or HueBarDrag Then
            SVPanelDrag = False
            HueBarDrag = False
            LastColor.Background = New SolidColorBrush(Color.FromArgb(A, R, G, B))
            RaiseEvent ColorSelect(Color.FromArgb(A, R, G, B), e)
        End If
    End Sub
End Class
