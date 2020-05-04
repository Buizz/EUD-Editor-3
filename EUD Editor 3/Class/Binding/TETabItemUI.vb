Imports System.ComponentModel
Imports MaterialDesignThemes.Wpf

<Serializable>
Public Class TETabItemUI
    Implements INotifyPropertyChanged

    Private TEFile As TEFile


    Public Sub New(tTEFile As TEFile)
        TEFile = tTEFile
    End Sub

    Public Sub PropertyChangedPack()
        NotifyPropertyChanged("TabName")
    End Sub



    Public ReadOnly Property TabName As Grid
        Get
            Dim tgrid As New Grid

            Dim ct As New ContextMenu


            If True Then
                Dim tmenu As New MenuItem
                Dim ticon As New PackIcon()
                ticon.Kind = PackIconKind.CloseBox
                tmenu.Icon = ticon
                tmenu.Header = "닫기"

                AddHandler tmenu.Click, New RoutedEventHandler(Sub(sender As Object, e As RoutedEventArgs)
                                                                   TECloseTabITem(TEFile)
                                                               End Sub)

                ct.Items.Add(tmenu)
            End If
            ct.Items.Add(New Separator)
            If True Then
                Dim tmenu As New MenuItem
                Dim ticon As New PackIcon()
                ticon.Kind = PackIconKind.CloseBoxMultiple
                tmenu.Icon = ticon
                tmenu.Header = "모든 문서 닫기"

                AddHandler tmenu.Click, New RoutedEventHandler(Sub(sender As Object, e As RoutedEventArgs)
                                                                   TECloseAllTabITem(TEFile)
                                                               End Sub)

                ct.Items.Add(tmenu)
            End If
            If True Then
                Dim tmenu As New MenuItem
                Dim ticon As New PackIcon()
                ticon.Kind = PackIconKind.CloseBoxMultiple
                tmenu.Icon = ticon
                tmenu.Header = "이 창을 제외하고 모든 문서 닫기"

                AddHandler tmenu.Click, New RoutedEventHandler(Sub(sender As Object, e As RoutedEventArgs)
                                                                   TECloseOtherTabITem(TEFile)
                                                               End Sub)

                ct.Items.Add(tmenu)
            End If



            '닫기
            '모든 문서 닫기
            '이 창을 제외하고 모든 문서 닫기
            'CloseBox
            'CloseBoxMultiple


            tgrid.ContextMenu = ct

            Dim tb As New TextBlock
            tb.Foreground = Application.Current.Resources("MaterialDesignBody")
            tb.Text = TEFile.FileName



            tgrid.Children.Add(tb)


            Return tgrid
        End Get
    End Property



    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
    <NonSerialized>
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
