Imports ICSharpCode.AvalonEdit
Imports ICSharpCode.AvalonEdit.CodeCompletion
Imports ICSharpCode.AvalonEdit.Document
Imports ICSharpCode.AvalonEdit.Editing

Public Class DataEditCompletionData
    Implements ICompletionData

    Private ToolTip As TextBlock
    Private TextEditor As TextEditor
    Private IconType As EIconType
    Public Enum EIconType
        KeyWord
        SettingValue
        Funcname
    End Enum


    Public Sub New(ByVal text As String, tToolTip As TextBlock, tTextEditor As TextEditor, tIconType As EIconType)
        Me.Text = text
        ToolTip = tToolTip
        TextEditor = tTextEditor
        IconType = tIconType
    End Sub

    Public ReadOnly Property Image As ImageSource Implements ICompletionData.Image
        Get
            Select Case IconType
                Case EIconType.KeyWord
                    Return New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/KeyWord.png"))
                Case EIconType.SettingValue
                    Return New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Setting.png"))
                Case EIconType.Funcname
                    Return New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Func.png"))
            End Select
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
            Return ToolTip
        End Get
    End Property

    Public ReadOnly Property Priority As Double Implements ICompletionData.Priority
        Get
            Return 0
        End Get
    End Property


    Public Sub Complete(ByVal textArea As TextArea, ByVal completionSegment As ISegment, ByVal insertionRequestEventArgs As EventArgs) Implements ICompletionData.Complete
        Dim ResultStr As String = ""

        'TextEditor.SelectionStart = completionSegment.Offset - 1
        'TextEditor.SelectionLength = completionSegment.Length + 1

        Select Case IconType
            Case EIconType.KeyWord
                ResultStr = Me.Text
                If ResultStr = "for" Then
                    ResultStr = "for i = 0, 10 do" & vbCrLf & "end"
                End If
            Case EIconType.SettingValue
                ResultStr = """" & Me.Text.Replace("_", " ") & """"
            Case EIconType.Funcname
                ResultStr = Me.Text
        End Select

        'TextEditor.SelectedText = ResultStr
        'TextEditor.SelectionLength = 0
        'TextEditor.SelectionStart += ResultStr.Length
        textArea.Document.Replace(completionSegment, ResultStr)
    End Sub

End Class
