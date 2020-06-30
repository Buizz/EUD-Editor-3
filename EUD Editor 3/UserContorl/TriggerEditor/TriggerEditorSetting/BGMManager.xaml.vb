Imports System.Media
Imports System.Threading
Imports System.Windows.Media.Animation
Imports NAudio.Vorbis
Imports NAudio.Wave


Public Class BGMManager

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        AnimationInit()

        List.ItemsSource = pjData.TEData.BGMData.BGMList
    End Sub
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        BGMPath.Text = ""
        BGMName.Text = ""

        SampleRateCombobox.SelectedIndex = 0
        BitRateCombobox.SelectedIndex = 0

        OpenNewWindow(Nothing)
    End Sub

    Private Sub EditItem_Click(sender As Object, e As RoutedEventArgs)
        Dim t As BGMData.BGMFile = CType(List.SelectedItem, BGMData.BGMFile)

        BGMPath.Text = t.BGMPath
        BGMName.Text = t.BGMName


        For i = 0 To SampleRateCombobox.Items.Count - 1
            Dim comboboxitem As ComboBoxItem = SampleRateCombobox.Items(i)
            If comboboxitem.Tag = t.BGMSampleRate Then
                SampleRateCombobox.SelectedIndex = i
                Exit For
            End If
        Next

        For i = 0 To BitRateCombobox.Items.Count - 1
            Dim comboboxitem As ComboBoxItem = BitRateCombobox.Items(i)
            If comboboxitem.Tag = t.BGMBitRate Then
                BitRateCombobox.SelectedIndex = i
                Exit For
            End If
        Next

        OpenEditWindow()
    End Sub

    Private Sub DeleteItem_Click(sender As Object, e As RoutedEventArgs)
        For Each bgmfile As BGMData.BGMFile In List.SelectedItems
            pjData.TEData.BGMData.BGMList.Remove(bgmfile)

        Next

        List.Items.Refresh()
        pjData.SetDirty(True)
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs)
        CloseStroyBoard.Begin(Me)
    End Sub

    Private Sub OkKey_Click(sender As Object, e As RoutedEventArgs)
        pjData.SetDirty(True)

        If IsEditWindowopen Then
            CType(List.SelectedItem, BGMData.BGMFile).Refresh(BGMPath.Text, BGMName.Text,
                                                              CType(SampleRateCombobox.SelectedItem, ComboBoxItem).Tag, CType(BitRateCombobox.SelectedItem, ComboBoxItem).Tag)
        Else
            If IsMultiFileOpne Then
                For i = 0 To Filelist.Count - 1
                    Dim fileinfo As IO.FileInfo = My.Computer.FileSystem.GetFileInfo(Filelist(i))
                    pjData.TEData.BGMData.BGMList.Add(New BGMData.BGMFile(fileinfo.FullName, fileinfo.Name,
                                                                  CType(SampleRateCombobox.SelectedItem, ComboBoxItem).Tag, CType(BitRateCombobox.SelectedItem, ComboBoxItem).Tag))
                Next
            Else
                pjData.TEData.BGMData.BGMList.Add(New BGMData.BGMFile(BGMPath.Text, BGMName.Text,
                                                                  CType(SampleRateCombobox.SelectedItem, ComboBoxItem).Tag, CType(BitRateCombobox.SelectedItem, ComboBoxItem).Tag))
            End If
        End If
        List.Items.Refresh()
        CloseStroyBoard.Begin(Me)
        pjData.SetDirty(True)
    End Sub


    Private IsMultiFileOpne As Boolean

    Private IsEditWindowopen As Boolean

    Private Filelist As New List(Of String)
    Private Sub OpenNewWindow(Files() As String)
        DefailtInfo.IsEnabled = True
        If Files Is Nothing Then
            DefailtInfo.Visibility = Visibility.Visible
            IsMultiFileOpne = False
        Else
            If Files.Count = 1 Then
                Dim fileinfo As IO.FileInfo = My.Computer.FileSystem.GetFileInfo(Files.First)

                IsMultiFileOpne = False
                BGMPath.Text = fileinfo.FullName
                BGMName.Text = fileinfo.Name
            Else
                IsMultiFileOpne = True
                Filelist.Clear()
                Filelist.AddRange(Files)
                BGMPath.Text = "다중 파일"
                BGMName.Text = "다중 파일"
                DefailtInfo.IsEnabled = False
            End If
        End If


        IsEditWindowopen = False
        OpenStroyBoard.Begin(Me)
        CreateEditWindow.Visibility = Visibility.Visible
        BtnRefresh()
    End Sub
    Private Sub OpenEditWindow()
        IsMultiFileOpne = False
        IsEditWindowopen = True
        OpenStroyBoard.Begin(Me)
        CreateEditWindow.Visibility = Visibility.Visible
        DefailtInfo.IsEnabled = True
        BtnRefresh()
    End Sub
    Private Sub ContextMenu_Opened(sender As Object, e As RoutedEventArgs)
        If List.SelectedItem Is Nothing Then
            EditItem.IsEnabled = False
            DeleteItem.IsEnabled = False
        Else
            EditItem.IsEnabled = True
            DeleteItem.IsEnabled = True
        End If
    End Sub

    Private OpenStroyBoard As Storyboard
    Private CloseStroyBoard As Storyboard
    Private Sub AnimationInit()
        If True Then
            Dim scale1 As ScaleTransform = New ScaleTransform(1, 1)

            InputDialog.RenderTransformOrigin = New Point(0.5, 0.5)
            InputDialog.RenderTransform = scale1



            Dim myHeightAnimation As DoubleAnimation = New DoubleAnimation()
            myHeightAnimation.From = 0.0
            myHeightAnimation.To = 1.0
            myHeightAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myWidthAnimation As DoubleAnimation = New DoubleAnimation()
            myWidthAnimation.From = 0.0
            myWidthAnimation.To = 1.0
            myWidthAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myOpacityAnimation As DoubleAnimation = New DoubleAnimation()
            myOpacityAnimation.From = 0.0
            myOpacityAnimation.To = 1.0
            myOpacityAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            OpenStroyBoard = New Storyboard()
            OpenStroyBoard.Children.Add(myOpacityAnimation)
            OpenStroyBoard.Children.Add(myWidthAnimation)
            OpenStroyBoard.Children.Add(myHeightAnimation)
            Storyboard.SetTargetName(myOpacityAnimation, CreateEditWindow.Name)
            Storyboard.SetTargetName(myWidthAnimation, InputDialog.Name)
            Storyboard.SetTargetName(myHeightAnimation, InputDialog.Name)
            Storyboard.SetTargetProperty(myOpacityAnimation, New PropertyPath(Border.OpacityProperty))
            Storyboard.SetTargetProperty(myHeightAnimation, New PropertyPath("RenderTransform.ScaleY"))
            Storyboard.SetTargetProperty(myWidthAnimation, New PropertyPath("RenderTransform.ScaleX"))
        End If
        If True Then
            Dim scale1 As ScaleTransform = New ScaleTransform(1, 1)

            InputDialog.RenderTransformOrigin = New Point(0.5, 0.5)
            InputDialog.RenderTransform = scale1



            Dim myHeightAnimation As DoubleAnimation = New DoubleAnimation()
            myHeightAnimation.From = 1.0
            myHeightAnimation.To = 0.0
            myHeightAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myWidthAnimation As DoubleAnimation = New DoubleAnimation()
            myWidthAnimation.From = 1.0
            myWidthAnimation.To = 0.0
            myWidthAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            Dim myOpacityAnimation As DoubleAnimation = New DoubleAnimation()
            myOpacityAnimation.From = 1.0
            myOpacityAnimation.To = 0.0
            myOpacityAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(150))

            CloseStroyBoard = New Storyboard()
            CloseStroyBoard.Children.Add(myOpacityAnimation)
            CloseStroyBoard.Children.Add(myWidthAnimation)
            CloseStroyBoard.Children.Add(myHeightAnimation)
            Storyboard.SetTargetName(myOpacityAnimation, CreateEditWindow.Name)
            Storyboard.SetTargetName(myWidthAnimation, InputDialog.Name)
            Storyboard.SetTargetName(myHeightAnimation, InputDialog.Name)
            Storyboard.SetTargetProperty(myOpacityAnimation, New PropertyPath(Border.OpacityProperty))
            Storyboard.SetTargetProperty(myHeightAnimation, New PropertyPath("RenderTransform.ScaleY"))
            Storyboard.SetTargetProperty(myWidthAnimation, New PropertyPath("RenderTransform.ScaleX"))

            AddHandler CloseStroyBoard.Completed, Sub(sender As Object, e As EventArgs)
                                                      CreateEditWindow.Visibility = Visibility.Hidden
                                                  End Sub
        End If



        'InputDialog
    End Sub

    Private Sub BGMPath_Click(sender As Object, e As RoutedEventArgs)
        Dim openfiledialog As New System.Windows.Forms.OpenFileDialog
        openfiledialog.Filter = Tool.GetText("BGM Fliter")

        Dim dialog As System.Windows.Forms.DialogResult = openfiledialog.ShowDialog

        If dialog = System.Windows.Forms.DialogResult.OK Then
            BGMPath.Text = openfiledialog.FileName
        End If
        BtnRefresh()
    End Sub

    Private Sub BtnRefresh()
        If BGMName.Text.Trim = "" Then
            OkKey.IsEnabled = False
            Return
        End If
        If BGMPath.Text.Trim = "" Then
            OkKey.IsEnabled = False
            Return
        End If
        If SampleRateCombobox.SelectedIndex = -1 Then
            OkKey.IsEnabled = False
            Return
        End If
        If BitRateCombobox.SelectedIndex = -1 Then
            OkKey.IsEnabled = False
            Return
        End If
        OkKey.IsEnabled = True
    End Sub

    Private Sub BGMName_TextChanged(sender As Object, e As TextChangedEventArgs)
        BtnRefresh()
    End Sub

    Public wo As New NAudio.Wave.WaveOut()
    Public vr As VorbisWaveReader
    Public af As AudioFileReader

    Private Sub SoundStop()
        wo.Stop()

        If vr IsNot Nothing Then
            vr.Close()
            vr.Dispose()
            vr = Nothing
        End If
        If af IsNot Nothing Then
            af.Close()
            af.Dispose()
            af = Nothing
        End If

    End Sub
    Private Sub BGMPlay(sender As Object, e As RoutedEventArgs)
        Dim b As Controls.Button = sender
        Dim bgmfile As BGMData.BGMFile = b.Tag

        SoundStop()

        If bgmfile.BGMBitRate = -1 And bgmfile.BGMSampleRate = -1 Then
            Dim extension As String = bgmfile.BGMPath.Split(".").Last.ToLower
            Select Case extension
                Case "ogg"
                    vr = New VorbisWaveReader(bgmfile.BGMPath)
                    wo.Init(vr)
                Case "mp3", "wav"
                    af = New AudioFileReader(bgmfile.BGMPath)
                    wo.Init(af)
            End Select

            wo.Play()
        Else
            Dim extension As String = bgmfile.BGMPath.Split(".").Last.ToLower

            Dim tlist As New List(Of BGMData.BGMFile)
            tlist.Add(bgmfile)

            Dim BGMWindow As New BGMPlayerConverter
            BGMWindow.WorkListRefresh(tlist)
            BGMWindow.ShowDialog()

            If BGMWindow.isSucess Then
                Dim folderPath As String = BuildData.SoundFilePath() & "\" & bgmfile.BGMName
                Dim output As String = folderPath & "\slow.ogg"


                vr = New VorbisWaveReader(output)
                wo.Init(vr)

                wo.Play()
            End If
        End If


        Dim view As ComponentModel.ICollectionView = CollectionViewSource.GetDefaultView(List.ItemsSource)
        view.Refresh()
    End Sub

    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        SoundStop()
    End Sub

    Private Sub BGMStop(sender As Object, e As RoutedEventArgs)
        SoundStop()
    End Sub

    Private Sub List_DragEnter(sender As Object, e As DragEventArgs)

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then

            Dim files() As String = e.Data.GetData(DataFormats.FileDrop)


            OpenNewWindow(files)
        End If

    End Sub

    Private Sub SampleRateCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        BtnRefresh()
    End Sub

    Private Sub BitRateCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        BtnRefresh()
    End Sub
End Class
