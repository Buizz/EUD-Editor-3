Imports MaterialDesignThemes.Wpf

Public Class IconSelecterP
    Private DatFile As SCDatFiles.DatFiles
    Private pValue As Long
    Public ReadOnly Property Value As Long
        Get
            Return pValue
        End Get
    End Property



    Public Sub Init(_DatFile As SCDatFiles.DatFiles, InitText As String, tValue As Long)
        DatFile = _DatFile
        pValue = tValue


        ValueText.Width = 7 * 3


        TextStr.Text = InitText
        ValueText.Text = pValue
        ValueRefresh()

        Select Case DatFile
            Case SCDatFiles.DatFiles.wireframe

                OpenNew.Visibility = Visibility.Collapsed


            Case Else
                btn.IsEnabled = True
                IconImage.IsEnabled = True


        End Select




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

    Private Sub btnMouseWhell(sender As Object, e As MouseWheelEventArgs) Handles btn.MouseWheel
        Dim ChangeValue As Integer = e.Delta / 100

        Dim CurrentValue As Integer = ValueText.Text
        If CheckOverFlow(DatFile, CurrentValue + ChangeValue) Then
            ValueText.Text += ChangeValue
        End If

    End Sub

    Private Sub OpenNew_Click(sender As Object, e As RoutedEventArgs) Handles OpenNew.Click
        If CheckOverFlow(DatFile, pValue) Then
            TabItemTool.WindowTabItem(DatFile, pValue)
        End If
    End Sub

    Private Sub ValueText_TextChanged(sender As Object, e As TextChangedEventArgs)
        pValue = ValueText.Text

        ValueRefresh()
    End Sub
    Private Sub ValueRefresh()
        If DatFile <> SCDatFiles.DatFiles.None Then
            If SCCodeCount(DatFile) > pValue Then
                btn.Content = pjData.CodeLabel(DatFile, pValue, True)
            Else
                btn.Content = Tool.GetText("None")
            End If
        Else
            btn.Content = Tool.GetText("None")
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
            IconImage.Source = scData.GetIcon(ImageIndex, False)
        Else
            IconImage.Source = scData.GetGRP(ImageIndex, 12, False)
        End If
    End Sub
End Class
