Imports System.Media
Imports NAudio.Vorbis

Public Class GUI_WavSelecter
    Public Event SelectEvent As RoutedEventHandler

    Public Sub New(val As String)

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()


        MainTreeview.BeginInit()
        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        For i = 0 To pjData.MapData.WavCount - 1
            AddTreeView(pjData.MapData.WavIndex(i).Split("/"))
        Next
        For i = 0 To scData.Sound_Count - 1
            AddTreeView(scData.SoundName(i).Split("/"))
        Next
        MainTreeview.EndInit()




        isLoad = True
    End Sub
    Private Function IsDefaultValue(str As String) As Boolean
        Dim strs() As String = str.Split(";")
        If strs.Length <> 2 Then
            Return False
        Else
            If strs.First = "defaultvalue" Then
                Return True
            End If
        End If
        Return False
    End Function

    Private Sub AddTreeView(path() As String)
        Dim citems As ItemCollection = MainTreeview.Items
        Dim index As Integer = 0

        While path.Length > index
            Dim findblock As Boolean = False

            For i = 0 To citems.Count - 1
                Dim treeveiwitems As TreeViewItem = citems(i)
                If treeveiwitems.Header = path(index) Then
                    findblock = True
                    citems = treeveiwitems.Items
                    index += 1
                    Exit For
                End If
            Next
            If Not findblock Then
                Dim tt As New TreeViewItem
                tt.Header = path(index)
                index += 1
                citems.Add(tt)
                citems = tt.Items
            End If
        End While


    End Sub



    Private isLoad As Boolean = False

    Private Sub MainTreeview_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object))
        Dim tvitem As TreeViewItem = MainTreeview.SelectedItem
        If tvitem IsNot Nothing Then
            If tvitem.Items.Count = 0 Then
                RaiseEvent SelectEvent(GetFullPath(tvitem), e)
            End If
        End If
    End Sub
    'Private Sub MainTB_TextChanged(sender As Object, e As TextChangedEventArgs)
    '    If isLoad Then
    '    End If
    'End Sub
    Public Function GetFullPath(ByVal node As TreeViewItem) As String
        Dim result As String = Convert.ToString(node.Header)
        Dim i As TreeViewItem = node.Parent

        While i IsNot Nothing
            result = i.Header & "/" + result
            If i.Parent.GetType Is GetType(TreeView) Then
                Return result
            End If

            i = i.Parent
        End While

        Return result
    End Function

    Private sp As SoundPlayer
    Private wo As New NAudio.Wave.WaveOut()
    Private vr As VorbisWaveReader

    Private Sub soundPlay()
        Dim tvitem As TreeViewItem = MainTreeview.SelectedItem
        If tvitem IsNot Nothing Then
            If tvitem.Items.Count = 0 Then
                Dim stream As New IO.MemoryStream(Tool.LoadDataFromMPQ(GetFullPath(tvitem)))
                If stream.Length = 0 Then
                    stream = New IO.MemoryStream(Tool.LoadDataFromMAP(GetFullPath(tvitem)))
                End If

                Try
                    sp = New SoundPlayer(stream)
                    sp.Play()
                    Return
                Catch ex As Exception

                End Try
                Try
                    vr = New VorbisWaveReader(stream)
                    wo.Init(vr)
                    wo.Play()
                    Return
                Catch ex As Exception

                End Try
            End If
        End If
    End Sub
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        soundPlay()
    End Sub
    Private Sub BGMStop()
        wo.Stop()

        If sp IsNot Nothing Then
            sp.Stop()
            sp = Nothing
        End If
        If vr IsNot Nothing Then
            vr.Close()
            vr.Dispose()
            vr = Nothing
        End If
    End Sub
    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        BGMStop()
    End Sub

    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        BGMStop()
    End Sub

    Private Sub MainTreeview_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        soundPlay()
    End Sub
End Class
