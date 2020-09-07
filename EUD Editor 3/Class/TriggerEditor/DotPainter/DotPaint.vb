Imports System.Windows.Media

Public Class DotPaint


    Public Colorname() As String = {"red", "lightblue", "lightgreen", "darkred", "blue", "green", "brown", "indigoblue", "darkgreen", "pink", "orange", "white",
"deepblue", "lightpink", "lightred", "palered", "margenta", "darkpink", "lightgray", "gray", "darkgray"}

    Public ColorHex() As String = {"#f70400", "#3a57cc", "#249824", "#7c0200", "#1d2c66", "#124c12", "#3e0100", "#0f1633", "#092609", "#982e66", "#8d4e12", "#ffffff", "#020b47",
"#fb8280", "#f94340", "#bb4240", "#7c0824", "#8a1833", "#808080", "#404040", "#202020"}

    Public ColorPallet(20) As System.Drawing.Color


    Public Function GetColorIndex(c As System.Drawing.Color) As Integer
        Dim dir As Integer = Integer.MaxValue
        Dim SelectIndx As Integer = 0
        For i = 0 To ColorPallet.Count - 1
            Dim r As Integer = Int(ColorPallet(i).R) - c.R
            Dim g As Integer = Int(ColorPallet(i).G) - c.G
            Dim b As Integer = Int(ColorPallet(i).B) - c.B

            Dim td As Integer = 0
            td += Math.Pow(r, 2)
            td += Math.Pow(g, 2)
            td += Math.Pow(b, 2)

            If td < dir Then
                dir = td
                SelectIndx = i
            End If
        Next

        Return SelectIndx
    End Function



    Public Sub New()
        For i = 0 To ColorHex.Length - 1
            Dim Color As Color = ColorConverter.ConvertFromString(ColorHex(i))


            Dim bitColor As System.Drawing.Color = System.Drawing.Color.FromArgb(Color.R, Color.G, Color.B)

            ColorPallet(i) = bitColor
        Next

    End Sub

End Class
