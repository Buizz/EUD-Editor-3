Imports System.Windows.Media.Animation

Public Class TEGUIPage
    Private PTEFile As TEFile
    Public ReadOnly Property TEFile As TEFile
        Get
            Return PTEFile
        End Get
    End Property

    Public Function CheckTEFile(tTEfile As TEFile) As Boolean
        Return (tTEfile Is PTEFile)
    End Function

    Public Sub New(tTEFile As TEFile, Optional SelectTrigIndex As Integer = -1)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        PTEFile = tTEFile

        Script.SetTEGUIPage(Me)
        Script.LoadScript(tTEFile, CType(tTEFile.Scripter, GUIScriptEditor).GetItemsList)

        ObjectSelector.SetGUIScriptEditorUI(Script)
        'Script.SetObjectSelecter(ObjectSelector)

        globalObjectListRefreah()
    End Sub

    Public Sub globalObjectListRefreah()
        ExternList.Items.Clear()

        Dim scripter As GUIScriptEditor = CType(TEFile.Scripter, GUIScriptEditor)
        If True Then
            Dim listitme As New ListBoxItem
            listitme.Tag = scripter.GetItemsList
            listitme.Content = Tool.GetText("TE_AllScript")


            ExternList.Items.Add(listitme)
        End If


        For i = 0 To scripter.ItemCount - 1
            If scripter.GetItems(i).ScriptType = ScriptBlock.EBlockType.fundefine Or scripter.GetItems(i).ScriptType = ScriptBlock.EBlockType.objectdefine Then
                Dim listitme As New ListBoxItem
                listitme.Tag = scripter.GetItems(i).child
                listitme.Content = scripter.GetItems(i).value


                ExternList.Items.Add(listitme)
            End If
        Next
    End Sub

    Private Sub ExternList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If ExternList.SelectedItem IsNot Nothing Then
            Dim listitem As ListBoxItem = ExternList.SelectedItem

            Script.LoadScript(PTEFile, listitem.Tag)
        End If

        ExternList.SelectedIndex = -1
    End Sub





    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        AnimationInit()

        CType(PTEFile.Scripter, GUIScriptEditor).ExternLoader()
    End Sub

    Public Sub SaveData()
        Script.Save()
        'MsgBox("세이브 : " & TEFile.FileName)
    End Sub
    Private Sub UserControl_Unloaded(sender As Object, e As RoutedEventArgs)
        Script.Save()
        'MsgBox("세이브 : " & TEFile.FileName)
    End Sub

    Private Sub ObjectSelector_ItemSelect(sender As Object, e As RoutedEventArgs)
        Dim v() As Object = sender


        Script.AddItemClick(v.First, v.Last.ToString)
    End Sub
    Private Sub UserControl_GotFocus(sender As Object, e As RoutedEventArgs)
    End Sub

    Private leftctrldown As Boolean = False
    Private Sub UserControl_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        Select Case e.Key
            Case Key.LeftCtrl
                leftctrldown = True
        End Select
        If leftctrldown Then
            Select Case e.Key
                Case Key.Z
                    'Script.Undo()
                Case Key.R
                    'Script.Redo()
            End Select
        End If
    End Sub

    Private Sub UserControl_PreviewKeyUp(sender As Object, e As KeyEventArgs)
        Select Case e.Key
            Case Key.LeftCtrl
                leftctrldown = False
        End Select
    End Sub



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

            OpenStoryBoard = New Storyboard()
            OpenStoryBoard.Children.Add(myOpacityAnimation)
            OpenStoryBoard.Children.Add(myWidthAnimation)
            OpenStoryBoard.Children.Add(myHeightAnimation)
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

            CloseStoryBoard = New Storyboard()
            CloseStoryBoard.Children.Add(myOpacityAnimation)
            CloseStoryBoard.Children.Add(myWidthAnimation)
            CloseStoryBoard.Children.Add(myHeightAnimation)
            Storyboard.SetTargetName(myOpacityAnimation, CreateEditWindow.Name)
            Storyboard.SetTargetName(myWidthAnimation, InputDialog.Name)
            Storyboard.SetTargetName(myHeightAnimation, InputDialog.Name)
            Storyboard.SetTargetProperty(myOpacityAnimation, New PropertyPath(Border.OpacityProperty))
            Storyboard.SetTargetProperty(myHeightAnimation, New PropertyPath("RenderTransform.ScaleY"))
            Storyboard.SetTargetProperty(myWidthAnimation, New PropertyPath("RenderTransform.ScaleX"))

            AddHandler CloseStoryBoard.Completed, Sub(sender As Object, e As EventArgs)
                                                      CreateEditWindow.Visibility = Visibility.Hidden
                                                      InputDialog.Content = Nothing
                                                  End Sub
        End If



        'InputDialog
    End Sub

    Public Sub OpenEditWindow(ctr As Control, pos As Point)
        ObjectSelector.IsEnabled = False
        Script.IsEnabled = False
        OpenStoryBoard.Begin(Me)
        CreateEditWindow.Visibility = Visibility.Visible
        InputDialog.Content = ctr



        If pos = Nothing Then
            InputDialog.VerticalAlignment = VerticalAlignment.Center
            InputDialog.HorizontalAlignment = HorizontalAlignment.Left
            InputDialog.Margin = New Thickness(ObjectSelector.ActualWidth + 100, 0, 0, 0)
        Else InputDialog.VerticalAlignment = VerticalAlignment.Top
            InputDialog.HorizontalAlignment = HorizontalAlignment.Left


            Dim realh As Integer = Script.ActualHeight
            Dim windowh As Integer = 500 'ctr.Height
            Dim itemh As Integer = 80
            Dim cpos As Integer = pos.Y

            If realh > 300 Then
                If (cpos + windowh) > realh Then
                    InputDialog.VerticalAlignment = VerticalAlignment.Center
                    InputDialog.HorizontalAlignment = HorizontalAlignment.Center
                    InputDialog.Margin = New Thickness(0, 0, 0, 0)
                Else
                    InputDialog.Margin = New Thickness(ObjectSelector.ActualWidth + pos.X, cpos + itemh, 0, 0)
                End If
            Else
                InputDialog.VerticalAlignment = VerticalAlignment.Center
                InputDialog.HorizontalAlignment = HorizontalAlignment.Left
                InputDialog.Margin = New Thickness(ObjectSelector.ActualWidth + 100, 0, 0, 0)

                'InputDialog.Margin = New Thickness(ObjectSelector.ActualWidth + pos.X, cpos + itemh, 0, 0)
            End If
        End If
    End Sub

    Public Sub RefreshData()
        PTEFile.RefreshData()
    End Sub


    Public Sub CloseEditWindow()
        ValueSelecter.Visibility = Visibility.Collapsed
        ValueSelecter.Child = Nothing
        ObjectSelector.IsEnabled = True
        Script.IsEnabled = True
        CloseStoryBoard.Begin(Me)
    End Sub


    Private OpenStoryBoard As Storyboard
    Private CloseStoryBoard As Storyboard

End Class
