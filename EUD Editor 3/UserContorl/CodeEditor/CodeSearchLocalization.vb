Public Class CodeSearchLocalization
    Inherits ICSharpCode.AvalonEdit.Search.Localization

    Public Overrides ReadOnly Property MatchCaseText As String
        Get
            Return Tool.GetText("MatchCaseText")
        End Get
    End Property

    Public Overrides ReadOnly Property MatchWholeWordsText As String
        Get
            Return Tool.GetText("MatchWholeWordsText")
        End Get
    End Property

    Public Overrides ReadOnly Property UseRegexText As String
        Get
            Return Tool.GetText("UseRegexText")
        End Get
    End Property

    Public Overrides ReadOnly Property FindNextText As String
        Get
            Return Tool.GetText("FindNextText")
        End Get
    End Property

    Public Overrides ReadOnly Property FindPreviousText As String
        Get
            Return Tool.GetText("FindPreviousText")
        End Get
    End Property

    Public Overrides ReadOnly Property ErrorText As String
        Get
            Return Tool.GetText("ErrorText")
        End Get
    End Property

    Public Overrides ReadOnly Property NoMatchesFoundText As String
        Get
            Return Tool.GetText("NoMatchesFoundText")
        End Get
    End Property
End Class
