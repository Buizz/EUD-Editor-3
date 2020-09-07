Imports System.Windows.Controls.Primitives
Imports ICSharpCode.AvalonEdit.CodeCompletion
Imports ICSharpCode.AvalonEdit.Document
Imports ICSharpCode.AvalonEdit.Editing

Public Class CustomCompletionWindow
    Inherits CompletionWindowBase

    ReadOnly _completionList As CompletionList = New CompletionList()
    Public _toolTip As New ToolTip()

    Public ReadOnly Property CompletionList As CompletionList
        Get
            Return _completionList
        End Get
    End Property

    Public Sub New(ByVal textArea As TextArea)

        MyBase.New(textArea)
        Me.CloseAutomatically = True
        Me.SizeToContent = SizeToContent.Height
        Me.MaxHeight = 300
        Me.Width = 175
        Me.Content = CompletionList
        Me.MinHeight = 15
        Me.MinWidth = 30

        Me.Background = Application.Current.Resources("MaterialDesignPaper")
        Me.Foreground = Application.Current.Resources("MaterialDesignBody")

        CompletionList.ListBox.Background = Application.Current.Resources("MaterialDesignPaper")
        CompletionList.ListBox.Foreground = Application.Current.Resources("MaterialDesignBody")
        CompletionList.ListBox.BorderBrush = Application.Current.Resources("MaterialDesignPaper")


        _toolTip.PlacementTarget = Me
        _toolTip.Placement = PlacementMode.Right
        AddHandler _toolTip.Closed, AddressOf toolTip_Closed
        AttachEvents()
    End Sub

    Private Sub toolTip_Closed(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If _toolTip IsNot Nothing Then _toolTip.Content = Nothing
    End Sub

    Private Sub completionList_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
        Dim item = CompletionList.SelectedItem
        If item Is Nothing Then Return
        Dim description As Object = item.Description

        _toolTip.UpdateLayout()

        If description IsNot Nothing Then
            Dim descriptionText As String = TryCast(description, String)

            If descriptionText IsNot Nothing Then
                _toolTip.Content = New TextBlock With {
                    .Text = descriptionText,
                    .TextWrapping = TextWrapping.Wrap
                }
            Else
                _toolTip.Content = description
            End If

            _toolTip.IsOpen = True
        Else
            _toolTip.IsOpen = False
        End If
    End Sub

    Private Sub completionList_InsertionRequested(ByVal sender As Object, ByVal e As EventArgs)
        Close()
        Dim item = CompletionList.SelectedItem
        If item IsNot Nothing Then item.Complete(Me.TextArea, New AnchorSegment(Me.TextArea.Document, Me.StartOffset, Me.EndOffset - Me.StartOffset), e)
    End Sub

    Private Sub AttachEvents()
        AddHandler Me.CompletionList.InsertionRequested, AddressOf completionList_InsertionRequested
        AddHandler Me.CompletionList.SelectionChanged, AddressOf completionList_SelectionChanged
        AddHandler Me.TextArea.Caret.PositionChanged, AddressOf CaretPositionChanged
        AddHandler Me.TextArea.MouseWheel, AddressOf textArea_MouseWheel
        AddHandler Me.TextArea.PreviewTextInput, AddressOf textArea_PreviewTextInput
    End Sub

    Protected Overrides Sub DetachEvents()


        RemoveHandler Me.CompletionList.InsertionRequested, AddressOf completionList_InsertionRequested
        RemoveHandler Me.CompletionList.SelectionChanged, AddressOf completionList_SelectionChanged
        RemoveHandler Me.TextArea.Caret.PositionChanged, AddressOf CaretPositionChanged
        RemoveHandler Me.TextArea.MouseWheel, AddressOf textArea_MouseWheel
        RemoveHandler Me.TextArea.PreviewTextInput, AddressOf textArea_PreviewTextInput
        MyBase.DetachEvents()
    End Sub

    Protected Overrides Sub OnClosed(ByVal e As EventArgs)
        MyBase.OnClosed(e)

        If _toolTip IsNot Nothing Then
            _toolTip.IsOpen = False
            _toolTip = Nothing
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        If Not e.Handled Then
            CompletionList.HandleKey(e)
        End If
    End Sub

    Private Sub textArea_PreviewTextInput(ByVal sender As Object, ByVal e As TextCompositionEventArgs)
        e.Handled = RaiseEventPair(Me, PreviewTextInputEvent, TextInputEvent, New TextCompositionEventArgs(e.Device, e.TextComposition))
    End Sub

    Private Sub textArea_MouseWheel(ByVal sender As Object, ByVal e As MouseWheelEventArgs)
        e.Handled = RaiseEventPair(GetScrollEventTarget(), PreviewMouseWheelEvent, MouseWheelEvent, New MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta))
    End Sub

    Private Function GetScrollEventTarget() As UIElement
        If CompletionList Is Nothing Then Return Me
        Return If(CompletionList.ScrollViewer, If(CompletionList.ListBox, CType(CompletionList, UIElement)))
    End Function

    Public Property CloseAutomatically As Boolean

    Protected Overrides ReadOnly Property CloseOnFocusLost As Boolean
        Get
            Return Me.CloseAutomatically
        End Get
    End Property

    Public Property CloseWhenCaretAtBeginning As Boolean

    Private Sub CaretPositionChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim offset As Integer = Me.TextArea.Caret.Offset

        If offset = Me.StartOffset Then

            If CloseAutomatically AndAlso CloseWhenCaretAtBeginning Then
                Close()
            Else
                CompletionList.SelectItem(String.Empty)
            End If

            Return
        End If

        If offset < Me.StartOffset OrElse offset > Me.EndOffset Then

            If CloseAutomatically Then
                Close()
            End If
        Else
            Dim document As TextDocument = Me.TextArea.Document

            If document IsNot Nothing Then
                CompletionList.SelectItem(document.GetText(Me.StartOffset, offset - Me.StartOffset))
            End If
        End If
    End Sub
End Class
