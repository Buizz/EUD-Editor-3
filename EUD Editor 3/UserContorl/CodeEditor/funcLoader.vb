Partial Public Class CodeEditor


    '함수 목록작성.

    '가져야 할 리스트 작성

    '리스트를 아래에서 옮긴다


    Public Function LoadData(TextEditor As ICSharpCode.AvalonEdit.TextEditor, cmpData As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData), FuncNameas As String, ArgumentCount As Integer) As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData)
        Dim data As IList(Of ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData) = cmpData


        'TrgAllyStatus
        'TrgComparison
        'TrgCount
        'TrgModifier
        'TrgOrder
        'TrgPlayer
        'TrgProperty
        'TrgPropState
        'TrgResource
        'TrgScore
        'TrgSwitchAction
        'TrgSwitchState
        'TrgAIScript
        'TrgLocation
        'TrgLocationIndex
        'TrgString
        'TrgSwitch
        'TrgUnit
        Dim Argument As String = LocalFunc.FindArgument(FuncNameas, ArgumentCount)
        Dim ArgumentType As String = Argument.Split(":").Last.Trim

        Select Case ArgumentType
            Case "TrgUnit"
                For i = 0 To SCUnitCount
                    data.Add(New DataEditCompletionData(pjData.CodeLabel(SCDatFiles.DatFiles.units, i), New TextBlock(), TextEditor, DataEditCompletionData.EIconType.Funcname))
                Next
                Return data
        End Select



        For i = 0 To LocalFunc.FuncCount - 1
            data.Add(New DataEditCompletionData(LocalFunc.GetFuncName(i), New TextBlock(), TextEditor, DataEditCompletionData.EIconType.Funcname))
        Next


        Return data
    End Function
End Class
