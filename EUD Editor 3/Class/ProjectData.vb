Public Class ProjectData
    Private UnitTexts() As String 'Tbl상에서의 유닛 이름 배열 tbl이 바뀌면 리셋해줘야 됨
    Public ReadOnly Property UnitName(index As Byte) As String
        Get

            Return index & "미상"
        End Get
    End Property





    '여기에 모든게 들어간다
    '스타 dat데이터를 클래스로 만들어 관리하자.
    Public Sub Load(FilePath As String)

    End Sub



    Public Sub Save()

    End Sub


    '일단 코드불러오는거 먼저 하자. stat_txt.bin을 불러오고 그걸 바탕으로 만들자.(이미지나 스프라이트를 제외하고는 한글 이름으로 가능하니까 이미지나 스프라이트는 데이터로 준비)


End Class
