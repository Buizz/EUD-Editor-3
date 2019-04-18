Public Class GRPImageBox
    Private GRPBox As GRPBox


    Public Sub Init(ImageNum As Integer)
        '자채적으로 데이터를 읽고 쓴다.
        GRPBox = New GRPBox(ImageNum, ImageBox)
    End Sub
    Public Sub ChangeIScriptType(IScrptIndex As Integer)
        '재생 중인 이미지 스크립트 종류를 교체한다.
        If GRPBox IsNot Nothing Then
            GRPBox.ChangeIScriptType(IScrptIndex)
        End If
    End Sub
End Class
