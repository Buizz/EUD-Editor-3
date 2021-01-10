Public Class GRPImageBox
    Private GRPBox As GRPBox

    Public Enum BoxType
        Image
        Unit
        UnitBrith
        Weapon
        Sprite
    End Enum

    Public Sub Init(ImageNum As Long, AnimHeaderIndex As Integer, Optional Flag As BoxType = BoxType.Image, Optional TFObjectID As Integer = 0)
        If GRPBox IsNot Nothing Then
            GRPBox.Delete()
        End If

        If (ImageNum >= UShort.MaxValue) Then
            ImageNum = UShort.MaxValue - 1
        End If


        '자채적으로 데이터를 읽고 쓴다.
        GRPBox = New GRPBox(ImageNum, ImageBox, Me, AnimHeaderIndex, Flag, TFObjectID)
    End Sub
    Public Sub ChangeIScriptType(IScrptIndex As Integer)
        '재생 중인 이미지 스크립트 종류를 교체한다.
        If GRPBox IsNot Nothing Then
            GRPBox.ChangeIScriptType(IScrptIndex)
        End If
    End Sub

    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        If GRPBox IsNot Nothing Then
            GRPBox.Delete()
        End If
    End Sub

    Public Sub WriteDebugText(str As String)
        'DebugText.Text = str
    End Sub
End Class
