Imports System.IO
Imports System.Xml
Imports ICSharpCode.AvalonEdit.Highlighting

Partial Public Class CodeTextEditor
    Public Sub InitTextEditor()
        Dim customHighlighting As IHighlightingDefinition
        Dim highlightName As String = ""

        If pgData.Setting(ProgramData.TSetting.Theme) = "Dark" Then
            highlightName = "EpsHighlightingDark"
        Else
            highlightName = "EpsHighlightingLight"
        End If

        Dim s As Stream = GetType(TECUIPage).Assembly.GetManifestResourceStream("EUD_Editor_3." & highlightName & ".xshd")
        Dim reader As New XmlTextReader(s)
        customHighlighting = Xshd.HighlightingLoader.Load(reader, HighlightingManager.Instance)

        HighlightingManager.Instance.RegisterHighlighting(highlightName, {".eps"}, customHighlighting)
        TextEditor.SyntaxHighlighting = customHighlighting




        AddHandler TextEditor.TextArea.TextEntering, AddressOf textEditor_TextArea_TextEntering
        AddHandler TextEditor.TextArea.TextEntered, AddressOf textEditor_TextArea_TextEntered
    End Sub
End Class
