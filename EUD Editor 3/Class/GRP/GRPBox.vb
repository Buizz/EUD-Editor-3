Public Class GRPBox
    Private ImageBox As Image
    Private ImageID As Integer

    'GRP여러개가 한 공간에서 그려짐, 이미지스크립트도 같이 돌아감.,

    'GRP들이 여러개 있음
    '각각의 GRP들은 이미지스크립트를 통해 GRP를 생성 할 수 있음
    'GRP들은 이미지 스크립트에 의해 사라질 수 있음.

    Public Sub ChangeIScriptType(IScrptIndex As Integer)
        '이미지 스크립트를 교체한다.
    End Sub

    Public Sub New(ImageNum As Integer, tImageBox As Image)
        '자채적으로 데이터를 읽고 쓴다.
        ImageBox = tImageBox
        ImageID = ImageNum
    End Sub
End Class
