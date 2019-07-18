Public Class ScriptTreeviewItem
    '연결된 스크립트블럭
    Public ReadOnly Property Script As ScriptBlock
    Private parrent As GUIScriptEditorUI

    Public Sub New(tparrent As GUIScriptEditorUI, tScript As ScriptBlock)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        Script = tScript
        parrent = tparrent

        ContentPanel.Init(parrent, Script)
    End Sub

    Private level As Byte
    Private Sub Grid_PreviewMouseMove(sender As Object, e As MouseEventArgs)
        Dim acturalHeight As Integer = mainGrid.ActualHeight

        If Not isMouseDown Then

            If Script.TriggerScript.IsFolder Then
                If Script.TriggerScript.IsLock Then
                    level = 1
                    TopBorder.Visibility = Visibility.Visible
                    BottomBorder.Visibility = Visibility.Visible
                Else
                    Select Case e.GetPosition(mainGrid).Y
                        Case 0 To acturalHeight / 3
                            level = 0

                            TopBorder.Visibility = Visibility.Visible
                            If SelectLevel <= 1 Then
                                BottomBorder.Visibility = Visibility.Hidden
                            End If
                        Case acturalHeight / 3 To acturalHeight / 3 * 2
                            level = 1
                            TopBorder.Visibility = Visibility.Visible
                            BottomBorder.Visibility = Visibility.Visible
                        Case acturalHeight / 3 * 2 To acturalHeight
                            level = 2
                            If SelectLevel = 0 Or SelectLevel = 3 Then
                                TopBorder.Visibility = Visibility.Hidden
                            End If
                            BottomBorder.Visibility = Visibility.Visible
                    End Select
                End If


            Else
                Select Case e.GetPosition(mainGrid).Y
                    Case 0 To acturalHeight / 2
                        level = 0

                        TopBorder.Visibility = Visibility.Visible
                        If SelectLevel <= 1 Then
                            BottomBorder.Visibility = Visibility.Hidden
                        End If
                    Case acturalHeight / 2 To acturalHeight
                        level = 2
                        If SelectLevel = 0 Or SelectLevel = 3 Then
                            TopBorder.Visibility = Visibility.Hidden
                        End If
                        BottomBorder.Visibility = Visibility.Visible
                End Select
            End If
        End If
    End Sub

    Private Sub MainGrid_MouseLeave(sender As Object, e As MouseEventArgs)
        If SelectLevel = 0 Or SelectLevel = 3 Then
            TopBorder.Visibility = Visibility.Hidden
        End If
        If SelectLevel <= 1 Then
            BottomBorder.Visibility = Visibility.Hidden
        End If


        'mainGrid.Background = New SolidColorBrush(Color.FromArgb(1, 128, 128, 128))
        isMouseDown = False
    End Sub

    Private isMouseDown As Boolean
    Private Sub MainGrid_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        'mainGrid.Background = New SolidColorBrush(Color.FromArgb(150, 100, 100, 170))
        isMouseDown = True
    End Sub


    Public SelectLevel As Byte

    Private Sub MainGrid_PreviewMouseUp(sender As Object, e As MouseButtonEventArgs)
        isMouseDown = False
        parrent.SelectScript(Me, level)
    End Sub

    Public Sub Selectthis(level As Byte)
        SelectLevel = level + 1
        If Script.TriggerScript.IsLock Then
            '잠겨있을 경우 위 아래에 추가 불가.

            If Script.TriggerScript.IsFolder Then
                SelectLevel = 2
            Else
                '폴더도 아닐 경우 선택불가
                SelectLevel = 0
                Exit Sub
            End If
        End If
        If Script.TriggerScript.IsChildLock Then
            If SelectLevel = 2 Then
                SelectLevel = 1
            End If
        End If

        Select Case SelectLevel
            Case 1
                TopBorder.Opacity = 1
                BottomBorder.Opacity = 0.5
                TopBorder.Visibility = Visibility.Visible
                BottomBorder.Visibility = Visibility.Hidden
            Case 2
                TopBorder.Opacity = 1
                BottomBorder.Opacity = 1
                TopBorder.Visibility = Visibility.Visible
                BottomBorder.Visibility = Visibility.Visible
            Case 3
                TopBorder.Opacity = 0.5
                BottomBorder.Opacity = 1
                TopBorder.Visibility = Visibility.Hidden
                BottomBorder.Visibility = Visibility.Visible
        End Select
    End Sub
    Public Sub dSelectthis()
        SelectLevel = 0
        TopBorder.Opacity = 0.5
        BottomBorder.Opacity = 0.5
        TopBorder.Visibility = Visibility.Hidden
        BottomBorder.Visibility = Visibility.Hidden
    End Sub



    Public Sub MulitSelect()
        mainGrid.Background = New SolidColorBrush(Color.FromArgb(150, 100, 100, 170))
    End Sub
    Public Sub dMulitSelect()
        mainGrid.Background = Nothing
    End Sub

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
