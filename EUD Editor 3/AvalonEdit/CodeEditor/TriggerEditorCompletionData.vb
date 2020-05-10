Imports ICSharpCode.AvalonEdit
Imports ICSharpCode.AvalonEdit.CodeCompletion
Imports ICSharpCode.AvalonEdit.Document
Imports ICSharpCode.AvalonEdit.Editing

Public Class TriggerEditorCompletionData
    Implements ICompletionData

    Private TextEditor As TextEditor
    Private IconType As EIconType
    Public Enum EIconType
        Auto
        '키워드
        KeyWord
        LuaKeyWord
        SettingValue
        Funcname
        StarKeyWord
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
        Variable
        NameSpace_
    End Enum

    Private ToolTip As String
    Private inputtext As String
    Public Sub New(tPriority As Double, tlisttext As String, tinputtext As String, tToolTip As String, tTextEditor As TextEditor, tIconType As EIconType)
        Me.Text = tlisttext
        inputtext = tinputtext


        ToolTip = tToolTip
        TextEditor = tTextEditor
        IconType = tIconType
        _Priority = tPriority
    End Sub
    Public Sub New(tPriority As Double, tfunc As CFunc, FuncIndex As Integer, tTextEditor As TextEditor, tIconType As EIconType)
        'LocalFunc.GetFuncName(i), LocalFunc.GetFuncName(i), LocalFunc.GetFuncTooltip(i)
        IconType = tIconType
        If tIconType = EIconType.Auto Then
            Select Case tfunc.GetFuncTooltip(FuncIndex).Type
                Case FunctionToolTip.FType.Act
                    IconType = EIconType.Action
                Case FunctionToolTip.FType.Cond
                    IconType = EIconType.Condiction
                Case FunctionToolTip.FType.Func
                    IconType = EIconType.Funcname
            End Select
        End If

        _Priority = tPriority

        If IconType = EIconType.Variable Then
            Text = tfunc.GetVariableNames(FuncIndex)
            inputtext = tfunc.GetVariableNames(FuncIndex)
            TextEditor = tTextEditor
            ToolTip = tfunc.GetVariableType(FuncIndex)
        Else
            Text = tfunc.GetFuncName(FuncIndex)
            inputtext = tfunc.GetFuncName(FuncIndex)
            TextEditor = tTextEditor
            ToolTip = tfunc.GetFuncTooltip(FuncIndex).Summary
        End If

    End Sub

    Public ReadOnly Property Image As ImageSource Implements ICompletionData.Image
        Get
            Select Case IconType
                Case EIconType.NameSpace_
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/nameSpace.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.StarConst, EIconType.StarKeyWord
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Const.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.StarStringConst
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Const.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.Condiction
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Condiction.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.Action
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Action.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.KeyWord, EIconType.LuaKeyWord
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/KeyWord.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.SettingValue
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Setting.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.Variable
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Variable.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.Funcname, EIconType.localFunction
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Function.png"))
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
            Case EIconType.LuaKeyWord
                ResultStr = inputtext
                Select Case ResultStr
                    Case "for"
                        ResultStr = "for i = 0, 10 do" & vbCrLf & "end"
                    Case "if"
                        ResultStr = "if condition then" & vbCrLf & "end"
                    Case "while"
                        ResultStr = "while condition do" & vbCrLf & "end"
                    Case "function"
                        ResultStr = "function name(args)" & vbCrLf & "end"
                    Case "elseif"
                        ResultStr = "elseif condition then" & vbCrLf & "end"
                    Case "refeat"
                        ResultStr = "repeat" & vbCrLf & "until condition"
                End Select
            Case EIconType.KeyWord
                ResultStr = inputtext
                If ResultStr = "for" Then
                    ResultStr = "for(;;){}"
                End If
            Case EIconType.StarStringConst
                ResultStr = inputtext
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
