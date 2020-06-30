Public Class SampleMapList
    Public CurrentMapPath As String
    Public IsMapLoading As Boolean
    Public Sub New(fullPath As String)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        CurrentMapPath = fullPath

        Dim _MapData As New MapData(fullPath)
        IsMapLoading = _MapData.LoadComplete

        If IsMapLoading Then
            MapPath.Text = fullPath.Split("\").Last
            MapName.Text = _MapData.MapName
            MapDis.Text = _MapData.MapDes
        End If
    End Sub
End Class
