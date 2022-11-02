Imports MaterialDesignThemes.Wpf

Public Class IconSelecterP
    Private DatFile As SCDatFiles.DatFiles
    Private pValue As Long
    Public Property Value As Long
        Get
            Return pValue
        End Get
        Set(value As Long)
            ValueText.Text = value
        End Set
    End Property

    Public Event ValueChange As RoutedEventHandler


    Public Sub Init(_DatFile As SCDatFiles.DatFiles, InitText As String, tValue As Long)
        CodeList.DataLoadCmp = False

        DatFile = _DatFile
        pValue = tValue


        ValueText.Width = 7 * 3


        TextStr.Text = InitText
        ValueText.Text = pValue
        ValueRefresh()



        'Select Case DatFile
        '    Case SCDatFiles.DatFiles.wireframe
        '        OpenNew.Visibility = Visibility.Collapsed
        '    Case Else
        '        btn.IsEnabled = True
        '        IconImage.IsEnabled = True
        'End Select


        If Not SCDatFiles.CheckValidDat(DatFile) Then
            Select Case DatFile
                Case SCDatFiles.DatFiles.ButtonSet

                Case SCDatFiles.DatFiles.wireframe, SCDatFiles.DatFiles.Icon
                    OpenNew.Visibility = Visibility.Collapsed
                Case SCDatFiles.DatFiles.stattxt
                    IconBox.Visibility = Visibility.Collapsed
                Case Else
                    OpenNew.Visibility = Visibility.Collapsed
                    IconBox.Visibility = Visibility.Collapsed
            End Select
        End If

        ValueText.Background = New SolidColorBrush(pgData.FiledDefault)
        btn.Background = New SolidColorBrush(pgData.FiledDefault)
    End Sub



    Private Sub Click(sender As Object, e As RoutedEventArgs) Handles btn.Click
        Select Case DatFile
            Case SCDatFiles.DatFiles.wireframe
                'CodeList.SetFliter(CodeSelecter.ESortType.n123)
                Dim valueType As SCDatFiles.DatFiles = SCDatFiles.DatFiles.wireframe

                Dim flag As Integer

                Dim value As Long



                If Not CodeList.DataLoadCmp Then
                    CodeList.ListReset(valueType, True, value, flag)
                Else
                    CodeList.Refresh(value, flag)
                End If

                CodeList.MaxWidth = btn.RenderSize.Width + 32
                CodeList.Width = btn.RenderSize.Width + 32
                CodeSelect.IsOpen = True
            Case Else
                'CodeList.SetFliter(CodeSelecter.ESortType.n123)

                If DatFile <> SCDatFiles.DatFiles.None Then
                    If Not CodeList.DataLoadCmp Then
                        CodeList.ListReset(DatFile, True, pValue)
                    Else
                        CodeList.Refresh(pValue)
                    End If

                    CodeList.MaxWidth = btn.RenderSize.Width + 32
                    CodeList.Width = btn.RenderSize.Width + 32
                    CodeSelect.IsOpen = True
                End If
        End Select
    End Sub

    Private Sub CodeList_Select(Code As Object, e As RoutedEventArgs)
        CodeSelect.IsOpen = False
        ValueText.Text = Code(1)

        'MsgBox(Code(1))
    End Sub

    Private Sub btnMouseWheel(sender As Object, e As MouseWheelEventArgs) Handles btn.MouseWheel
        Dim ChangeValue As Integer = e.Delta / 100

        Dim CurrentValue As Integer = pValue
        If CheckOverFlow(DatFile, CurrentValue + ChangeValue) Then
            ValueText.Text = pValue + ChangeValue
        End If
    End Sub

    Private Sub OpenNew_Click(sender As Object, e As RoutedEventArgs) Handles OpenNew.Click
        Dim rv As Long = pValue
        If DatFile = SCDatFiles.DatFiles.stattxt Then
            rv -= 1
        End If
        If CheckOverFlow(DatFile, pValue) Then
            TabItemTool.WindowTabItem(DatFile, rv)
        End If
    End Sub

    Private Sub ValueText_TextChanged(sender As Object, e As TextChangedEventArgs)
        If IsNumeric(ValueText.Text) Then
            pValue = ValueText.Text
            If pValue >= 65536 Then
                pValue = 65535
                ValueText.Text = 65535
            End If
            RaiseEvent ValueChange(Me, e)
            ValueRefresh()
        End If

    End Sub
    Private Sub ValueRefresh()
        If DatFile <> SCDatFiles.DatFiles.None Then
            If SCCodeCount(DatFile) > pValue Then
                btn.Content = pjData.CodeLabel(DatFile, pValue, True)
            Else
                btn.Content = Tool.GetText("None")
                Return
            End If
        Else
            btn.Content = Tool.GetText("None")
            Return
        End If

        Dim isIcon As Boolean = False
        Dim ImageIndex As Integer


        Select Case DatFile
            Case SCDatFiles.DatFiles.units
                Dim tGraphics As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Graphics", pValue)
                Dim tSprite As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", tGraphics)
                Dim timage As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", tSprite)

                ImageIndex = timage
            Case SCDatFiles.DatFiles.weapons
                Dim tIcon As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.weapons, "Icon", pValue)

                isIcon = True
                ImageIndex = tIcon
            Case SCDatFiles.DatFiles.flingy
                Dim tSprite As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", pValue)
                Dim timage As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", tSprite)

                ImageIndex = timage
            Case SCDatFiles.DatFiles.sprites
                Dim timage As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", pValue)

                ImageIndex = timage
            Case SCDatFiles.DatFiles.images
                ImageIndex = pValue
            Case SCDatFiles.DatFiles.upgrades
                Dim tIcon As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.upgrades, "Icon", pValue)

                isIcon = True
                ImageIndex = tIcon
            Case SCDatFiles.DatFiles.techdata
                Dim tIcon As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.techdata, "Icon", pValue)

                isIcon = True
                ImageIndex = tIcon
            Case SCDatFiles.DatFiles.orders
                Dim tIcon As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.orders, "Highlight", pValue)

                isIcon = True
                ImageIndex = tIcon
            Case SCDatFiles.DatFiles.Icon
                isIcon = True
                ImageIndex = pValue
        End Select
        If isIcon Then
            IconImage.Source = scData.GetIcon(ImageIndex)
        Else
            IconImage.Source = scData.GetGRPImage(ImageIndex, 12)
        End If
    End Sub
End Class
