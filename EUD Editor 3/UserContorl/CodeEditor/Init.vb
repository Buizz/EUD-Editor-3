Imports System.IO
Imports System.Xml
Imports System.Reflection
Imports System.Windows.Threading
Imports ICSharpCode.AvalonEdit.Folding
Imports ICSharpCode.AvalonEdit.Highlighting
Imports ICSharpCode.AvalonEdit
Imports ICSharpCode.AvalonEdit.CodeCompletion

Partial Public Class CodeEditor
    Private Sub InitTextEditor()
        Dim customHighlighting As IHighlightingDefinition
        Dim s As Stream = GetType(TECUIPage).Assembly.GetManifestResourceStream("EUD_Editor_3.EpsHighlightingDark.xshd")
        Dim reader As New XmlTextReader(s)
        customHighlighting = Xshd.HighlightingLoader.Load(reader, HighlightingManager.Instance)

        HighlightingManager.Instance.RegisterHighlighting("EpsHighlightingDark", {".eps"}, customHighlighting)
        TextEditor.SyntaxHighlighting = customHighlighting



        Dim foldingUpdateTimer As DispatcherTimer = New DispatcherTimer()
        foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2)
        AddHandler foldingUpdateTimer.Tick, AddressOf foldingUpdateTimer_Tick
        foldingUpdateTimer.Start()

        FoldingManager = FoldingManager.Install(TextEditor.TextArea)
        FoldingStrategy = New EPSFoldingStrategy()
        FoldingStrategy.UpdateFoldings(FoldingManager, TextEditor.Document)

        AddHandler TextEditor.TextArea.TextEntering, AddressOf textEditor_TextArea_TextEntering
        AddHandler TextEditor.TextArea.TextEntered, AddressOf textEditor_TextArea_TextEntered
    End Sub

    Private FoldingManager As FoldingManager
    Private FoldingStrategy As EPSFoldingStrategy

    Private Sub foldingUpdateTimer_Tick(sender As Object, e As EventArgs)
        If FoldingStrategy IsNot Nothing Then
            FoldingStrategy.UpdateFoldings(FoldingManager, TextEditor.Document)
        End If
    End Sub

    Private LastStr As String
    Private Sub textEditor_TextArea_TextEntered(sender As Object, e As TextCompositionEventArgs)
        LastStr = LastStr & e.Text

        If e.Text = " " Then
            LastStr = ""
        End If
        Log.Text = e.Text & " " & LastStr & " " & TextEditor.SelectionStart



        ShowCompletion(e.Text, False)
    End Sub

    Private Sub textEditor_TextArea_TextEntering(sender As Object, e As TextCompositionEventArgs)
        If (e.Text.Length > 0 And completionWindow IsNot Nothing) Then
            If e.Text = vbTab Then
                completionWindow.CompletionList.RequestInsertion(e)
            ElseIf Not Char.IsLetterOrDigit(e.Text(0)) Then
                completionWindow.Close()
            End If
        End If
    End Sub

    Private Sub ShowCompletion(ByVal enteredText As String, ByVal controlSpace As Boolean)
        If Not controlSpace Then
            Debug.WriteLine("Code Completion: TextEntered: " & enteredText)
        Else
            Debug.WriteLine("Code Completion: Ctrl+Space")
        End If


        If completionWindow Is Nothing Then
            completionWindow = New CompletionWindow(TextEditor.TextArea)
            completionWindow.CloseWhenCaretAtBeginning = controlSpace
            'completionWindow.StartOffset -= 1


            Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = completionWindow.CompletionList.CompletionData

            For i = 0 To 10
                data.Add(New DataEditCompletionData("EudEditor 3", New TextBlock(), TextEditor, DataEditCompletionData.EIconType.Funcname))
            Next

            completionWindow.CompletionList.ListBox.Background = Application.Current.Resources("MaterialDesignPaper")
            completionWindow.CompletionList.ListBox.BorderBrush = Application.Current.Resources("MaterialDesignPaper")
            'If results.TriggerWordLength > 0 Then
            '    completionWindow.CompletionList.SelectItem(results.TriggerWord)
            'End If

            completionWindow.Show()
            AddHandler completionWindow.Closed, Sub()
                                                    completionWindow = Nothing
                                                End Sub
        End If


    End Sub


    Private completionWindow As ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow
    Private permissionChar() As String = {" ", "(", ")", "[", "]", "{", "}", vbTab}

    Private Sub completionWindowKeyDown(sender As Object, e As KeyEventArgs)
        If completionWindow.CompletionList.ListBox.Items.Count = 0 Then
            completionWindow.Close()
        End If
    End Sub
End Class
