Imports ICSharpCode.AvalonEdit
Imports ICSharpCode.AvalonEdit.CodeCompletion
Imports ICSharpCode.AvalonEdit.Document
Imports ICSharpCode.AvalonEdit.Editing


Partial Public Class CodeEditor


    '함수 목록작성.

    '가져야 할 리스트 작성

    '리스트를 아래에서 옮긴다


    Public Function LoadData(TextEditor As ICSharpCode.AvalonEdit.TextEditor, cmpData As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData), FuncNameas As String, ArgumentCount As Integer, IsFirstArgumnet As Boolean) As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData)
        Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = cmpData



        'TrgAllyStatus

        'TrgComparison
        '상수
        'TrgCount
        '숫자
        'TrgModifier
        '상수
        'TrgOrder
        '상수
        'TrgPlayer

        'TrgProperty

        'TrgPropState

        'TrgResource

        'TrgScore

        'TrgSwitchAction
        '상수
        'TrgSwitchState
        '상수
        'TrgAIScript
        '스트링
        'TrgLocation
        '스트링
        'TrgLocationIndex

        'TrgString
        '스트링
        'TrgSwitch
        '스트링
        'TrgUnit
        '스트링
        Dim Argument As String = LocalFunc.FindArgument(FuncNameas, ArgumentCount)
        Dim ArgumentType As String = Argument.Split(":").Last.Trim

        If IsFirstArgumnet Then
            Select Case ArgumentType
                Case "TrgUnit"
                    For i = 0 To SCUnitCount - 1
                        Dim tb As New TextBox
                        tb.Text = pjData.UnitInGameName(i)

                        data.Add(New TECompletionData(0, "[" & i & "] " & pjData.CodeLabel(SCDatFiles.DatFiles.units, i), pjData.UnitInGameName(i), tb, TextEditor, TECompletionData.EIconType.StarStringConst))
                    Next
                    Return data
                Case "TrgLocation"

            End Select
        End If




        For i = 0 To LocalFunc.FuncCount - 1
            data.Add(New TECompletionData(10, LocalFunc.GetFuncName(i), LocalFunc.GetFuncName(i), New TextBlock(), TextEditor, TECompletionData.EIconType.localFunction))
        Next
        For i = 0 To Tool.TEEpsExternFunc.FuncCount - 1
            data.Add(New TECompletionData(20, Tool.TEEpsExternFunc.GetFuncName(i), Tool.TEEpsExternFunc.GetFuncName(i), New TextBlock(), TextEditor, TECompletionData.EIconType.Auto))
        Next

        Return data
    End Function
End Class


Public Class TECompletionData
    Implements ICompletionData

    Private ToolTip As DependencyObject
    Private TextEditor As TextEditor
    Private IconType As EIconType
    Public Enum EIconType
        Auto

        '키워드
        KeyWord
        SettingValue
        Funcname

        '에디터 설정 값(AtLeast, AtMost, Enabled등등
        StarConst

        '스타 스트링 값
        StarStringConst

        '액션들
        Action

        '조건들
        Condiction

        'eps기본 함수
        plibFunction

        '로컬 함수
        localFunction




        '로컬 항목 및 매개 변수
        '상수
        '속성
        '필드
        '매서드
        '인터페이스
        '클래스
        '모듈
        '구조
        '열거형
        '네임스페이스
        '키워드
    End Enum


    Private inputtext As String
    Public Sub New(tPriority As Double, tlisttext As String, tinputtext As String, tToolTip As DependencyObject, tTextEditor As TextEditor, tIconType As EIconType)
        Me.Text = tlisttext
        inputtext = tinputtext
        ToolTip = tToolTip
        TextEditor = tTextEditor
        IconType = tIconType
        _Priority = tPriority
    End Sub

    Public ReadOnly Property Image As ImageSource Implements ICompletionData.Image
        Get
            Select Case IconType
                Case EIconType.KeyWord
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/KeyWord.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.SettingValue
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Setting.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.Funcname
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Func.png"))
                    Imageicon.Freeze()
                    Return Imageicon
            End Select
            Return Nothing
        End Get
    End Property

    '검색기준
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

    Private _Priority As Double
    Public ReadOnly Property Priority As Double Implements ICompletionData.Priority
        Get
            Return _Priority
        End Get
    End Property


    Public Sub Complete(ByVal textArea As TextArea, ByVal completionSegment As ISegment, ByVal insertionRequestEventArgs As EventArgs) Implements ICompletionData.Complete
        Dim ResultStr As String = ""

        'TextEditor.SelectionStart = completionSegment.Offset - 1
        'TextEditor.SelectionLength = completionSegment.Length + 1

        Select Case IconType
            Case EIconType.KeyWord
                ResultStr = inputtext
                If ResultStr = "for" Then
                    ResultStr = "for i = 0, 10 do" & vbCrLf & "end"
                End If
            Case EIconType.StarStringConst
                ResultStr = """" & inputtext.Replace("_", " ") & """"
            Case EIconType.Funcname
                ResultStr = inputtext
            Case Else
                ResultStr = inputtext
        End Select

        'TextEditor.SelectedText = ResultStr
        'TextEditor.SelectionLength = 0
        'TextEditor.SelectionStart += ResultStr.Length
        textArea.Document.Replace(completionSegment, ResultStr)
    End Sub

End Class
