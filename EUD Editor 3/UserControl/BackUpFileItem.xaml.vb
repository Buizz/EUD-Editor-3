Public Class BackUpFileItem
    Public BackFileInfo As IO.FileInfo
    Public Sub New(fileinfo As IO.FileInfo)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        BackFileInfo = fileinfo

        FileName.Text = fileinfo.FullName
        FileDate.Text = fileinfo.LastWriteTime
    End Sub
End Class
