Imports System.Media
Imports System.Threading
Imports System.Windows.Media.Animation
Imports NAudio.Vorbis
Imports NAudio.Wave


Public Class SCAScriptImageManager

    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        ItemList.Items.Clear()
        For i = 0 To pjData.TEData.SCAImageDatas.Count - 1
            Dim tlist As New ListBoxItem

            tlist.Content = pjData.TEData.SCAImageDatas(i).ItemName
            tlist.Tag = pjData.TEData.SCAImageDatas(i)

            ItemList.Items.Add(tlist)
        Next
    End Sub

    Private SelectedItem As SCAScriptImageData

    Private Sub AddBtn(sender As Object, e As RoutedEventArgs)
        Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
            .Filter = Tool.GetText("IMG Fliter"),
            .Title = Tool.GetText("IMG Select")
            }

        Dim filename As String

        If opendialog.ShowDialog() = Forms.DialogResult.OK Then
            filename = opendialog.FileName
        Else
            Return
        End If


        Dim dotData As New SCAScriptImageData(filename)
        If Not dotData.LoadSuccess Then
            Return
        End If

        pjData.SetDirty(True)

        'dotData.ItemName = ItemList.Items.Count

        pjData.TEData.SCAImageDatas.Add(dotData)

        Dim tlist As New ListBoxItem

        tlist.Content = dotData.ItemName
        tlist.Tag = dotData

        ItemList.Items.Add(tlist)

        ItemList.SelectedItem = tlist
    End Sub

    Private Sub DeleteBtnClick(sender As Object, e As RoutedEventArgs)
        pjData.SetDirty(True)
        If ItemList.SelectedIndex <> -1 Then
            Dim tdotdata As SCAScriptImageData = CType(ItemList.SelectedItem, ListBoxItem).Tag

            pjData.TEData.SCAImageDatas.Remove(tdotdata)
            ItemList.Items.Remove(ItemList.SelectedItem)
        End If
    End Sub

    Private Sub RefreshItem(data As SCAScriptImageData)
        SelectedItem = data

        imgSize.Text = data.imgWidth & " x " & data.imgHeight
        filename.Text = data.ItemName
        img.Source = data.GetBitMap
    End Sub

    Private Sub ItemList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If ItemList.SelectedIndex = -1 Then
            DeleteBtn.IsEnabled = False
            EditPage.Visibility = Visibility.Hidden
        Else
            DeleteBtn.IsEnabled = True
            EditPage.Visibility = Visibility.Visible
            Dim tdotdata As SCAScriptImageData = CType(ItemList.SelectedItem, ListBoxItem).Tag

            RefreshItem(tdotdata)
        End If
    End Sub

    Private Sub ReSetting_Click(sender As Object, e As RoutedEventArgs)
        Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
            .Filter = Tool.GetText("IMG Fliter"),
            .Title = Tool.GetText("IMG Select")
            }

        Dim filename As String

        If opendialog.ShowDialog() = Forms.DialogResult.OK Then
            filename = opendialog.FileName
        Else
            Return
        End If

        SelectedItem.LoadImage(filename)
        If Not SelectedItem.LoadSuccess Then
            Return
        End If

        pjData.SetDirty(True)
        RefreshItem(SelectedItem)
    End Sub

    Private Sub filename_TextChanged(sender As Object, e As TextChangedEventArgs)
        If SelectedItem IsNot Nothing Then
            SelectedItem.ItemName = filename.Text

            Dim list As ListBoxItem = ItemList.SelectedItem

            list.Content = SelectedItem.ItemName
            RefreshItem(SelectedItem)
        End If
    End Sub
End Class
