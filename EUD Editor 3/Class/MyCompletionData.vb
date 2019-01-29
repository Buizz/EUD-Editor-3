Imports ICSharpCode.AvalonEdit.CodeCompletion
Imports ICSharpCode.AvalonEdit.Document
Imports ICSharpCode.AvalonEdit.Editing

Public Class MyCompletionData
    Implements ICompletionData

    Public Sub New(ByVal text As String)
        Me.Text = text
    End Sub

    Public ReadOnly Property Image As ImageSource Implements ICompletionData.Image
        Get
            Return Nothing
        End Get
    End Property

    Public Property Text As String Implements ICompletionData.Text

    Public ReadOnly Property Content As Object Implements ICompletionData.Content
        Get
            Return Me.Text
        End Get
    End Property

    Public ReadOnly Property Description As Object Implements ICompletionData.Description
        Get
            Return "Description for " & Me.Text
        End Get
    End Property

    Public ReadOnly Property Priority As Double Implements ICompletionData.Priority
        Get
            Return 0
        End Get
    End Property


    Public Sub Complete(ByVal textArea As TextArea, ByVal completionSegment As ISegment, ByVal insertionRequestEventArgs As EventArgs) Implements ICompletionData.Complete
        textArea.Document.Replace(completionSegment, Me.Text)
    End Sub

End Class
