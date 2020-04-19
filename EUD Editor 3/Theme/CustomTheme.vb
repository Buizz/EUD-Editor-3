Imports MaterialDesignColors
Imports MaterialDesignThemes.Wpf

Public Class CustomTheme
    Public PrimaryHueLight As Color
    Public PrimaryHueMid As Color
    Public PrimaryHueDark As Color
    Public PrimaryHueLightForeground As Color
    Public PrimaryHueMidForeground As Color
    Public PrimaryHueDarkForeground As Color

    Public SecondaryMid As Color
    Public SecondaryMidForeground As Color

    Public IsLight As Boolean

    Public Sub ApplyTheme()
        Dim ph As New PaletteHelper
        Dim ttheme As ITheme = ph.GetTheme

        If IsLight Then
            ttheme.SetBaseTheme(Theme.Light)
        Else
            ttheme.SetBaseTheme(Theme.Dark)
        End If



        ttheme.PrimaryLight = New ColorPair(PrimaryHueLight)
        ttheme.PrimaryMid = New ColorPair(PrimaryHueMid)
        ttheme.PrimaryDark = New ColorPair(PrimaryHueDark)

        ttheme.SecondaryMid = New ColorPair(SecondaryMid)



        ph.SetTheme(ttheme)
    End Sub

    Public Sub InitColor()
        Dim ph As New PaletteHelper

        Dim theme As ITheme = ph.GetTheme

        'theme.SetBaseTheme(SwatchHelper.Lookup(MaterialDesignColor.Amber500))
        theme.SetPrimaryColor(SwatchHelper.Lookup(MaterialDesignColor.Blue))
        theme.SetSecondaryColor(SwatchHelper.Lookup(MaterialDesignColor.BlueSecondary))


        PrimaryHueLight = theme.PrimaryLight.Color
        PrimaryHueMid = theme.PrimaryMid.Color
        PrimaryHueDark = theme.PrimaryDark.Color
        'PrimaryHueLightForeground = theme.PrimaryLight.ForegroundColor
        'PrimaryHueMidForeground = theme.PrimaryMid.ForegroundColor
        'PrimaryHueDarkForeground = theme.PrimaryDark.ForegroundColor


        SecondaryMid = theme.SecondaryMid.Color
        'SecondaryMidForeground = theme.SecondaryMid.ForegroundColor


    End Sub



    Public Shared Function CheckBlack(color As Color) As Color
        Dim h As Double
        Dim s As Double
        Dim v As Double

        ColorPicker.ColorToHSV(color, h, s, v)

        If s < 0.3 And v > 0.7 Then '밝음
            Return Colors.Black
        End If

        If v <= 0.7 Then '어두움
            Return Colors.White
        End If

        Select Case h
            Case 30 To 210
                Return Colors.Black
            Case Else
                Return Colors.White
        End Select

        Return Colors.Black
    End Function
End Class
