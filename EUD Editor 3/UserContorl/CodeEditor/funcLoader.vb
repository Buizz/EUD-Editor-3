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
        Dim Argument As String = GetArgument(FuncNameas, ArgumentCount)
        Dim ArgumentType As String = Argument.Split(":").Last.Trim
        If IsFirstArgumnet Then
            Select Case ArgumentType
                Case "TrgAllyStatus"
                    Dim strs() As String = {"Enemy", "Ally", "AlliedVictory"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgComparison"
                    Dim strs() As String = {"AtLeast", "AtMost", "Exactly"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgCount"
                    '숫자
                    Dim tb As New TextBox
                    tb.Text = "All"

                    data.Add(New TECompletionData(0, "All", "All", tb, TextEditor, TECompletionData.EIconType.StarConst))

                Case "TrgModifier"
                    '상수
                    Dim strs() As String = {"SetTo", "Add", "Subtract"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgOrder"
                    Dim strs() As String = {"Move", "Patrol", "Attack"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgPlayer"
                    Dim strs() As String = {"P1", "P2", "P3", "P4", "P5", "P6", "P7", "P8", "P9", "P10", "P11", "P12",
                        "CurrentPlayer", "Foes", "Allies", "NeutralPlayers", "AllPlayers", "Force1", "Force2", "Force3", "Force4", "NonAlliedVictoryPlayers"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgProperty"

                Case "TrgPropState"
                    Dim strs() As String = {"Enable", "Disable", "Toggle"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgResource"
                    Dim strs() As String = {"Ore", "Gas", "OreAndGas"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgScore"
                    Dim strs() As String = {"Total", "Units", "Buildings", "UnitsAndBuildings", "Kills",
                        "Razings", "KillsAndRazings", "Custom"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgSwitchAction"
                    Dim strs() As String = {"Set", "Clear", "Toggle", "Random"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgSwitchState"
                    Dim strs() As String = {"Set", "Cleared"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgAIScript"
                    Dim strs() As String = {"Terran Custom Level", "Zerg Custom Level", "Protoss Custom Level", "Terran Expansion Custom Level",
"Zerg Expansion Custom Level", "Protoss Expansion Custom Level", "Terran Campaign Easy", "Terran Campaign Medium", "Terran Campaign Difficult",
"Terran Campaign Insane", "Terran Campaign Area Town", "Zerg Campaign Easy", "Zerg Campaign Medium", "Zerg Campaign Difficult",
"Zerg Campaign Insane", "Zerg Campaign Area Town", "Protoss Campaign Easy", "Protoss Campaign Medium", "Protoss Campaign Difficult",
"Protoss Campaign Insane", "Protoss Campaign Area Town", "Expansion Terran Campaign Easy", "Expansion Terran Campaign Medium", "Expansion Terran Campaign Difficult",
"Expansion Terran Campaign Insane", "Expansion Terran Campaign Area Town", "Expansion Zerg Campaign Easy", "Expansion Zerg Campaign Medium", "Expansion Zerg Campaign Difficult",
"Expansion Zerg Campaign Insane", "Expansion Zerg Campaign Area Town", "Expansion Protoss Campaign Easy", "Expansion Protoss Campaign Medium", "Expansion Protoss Campaign Difficult",
"Expansion Protoss Campaign Insane", "Expansion Protoss Campaign Area Town", "Send All Units on Strategic Suicide Missions", "Send All Units on Random Suicide Missions",
"Switch Computer Player to Rescue Passive", "Turn ON Shared Vision for Player 1", "Turn ON Shared Vision for Player 2", "Turn ON Shared Vision for Player 3",
"Turn ON Shared Vision for Player 4", "Turn ON Shared Vision for Player 5", "Turn ON Shared Vision for Player 6", "Turn ON Shared Vision for Player 7",
"Turn ON Shared Vision for Player 8", "Turn OFF Shared Vision for Player 1", "Turn OFF Shared Vision for Player 2", "Turn OFF Shared Vision for Player 3",
"Turn OFF Shared Vision for Player 4", "Turn OFF Shared Vision for Player 5", "Turn OFF Shared Vision for Player 6", "Turn OFF Shared Vision for Player 7",
"Turn OFF Shared Vision for Player 8", "Move Dark Templars to Region", "Clear Previous Combat Data", "Set Player to Enemy", "Set Player to Ally  ",
"Value This Area Higher", "Enter Closest Bunker", "Set Generic Command Target", "Make These Units Patrol", "Enter Transport", "Exit Transport",
"AI Nuke Here", "AI Harass Here", "Set Unit Order To: Junk Yard Dog", "Disruption Web Here", "Recall Here", "Terran 3 - Zerg Town", "Terran 5 - Terran Main Town",
"Terran 5 - Terran Harvest Town", "Terran 6 - Air Attack Zerg", "Terran 6 - Ground Attack Zerg", "Terran 6 - Zerg Support Town", "Terran 7 - Bottom Zerg Town",
"Terran 7 - Right Zerg Town", "Terran 7 - Middle Zerg Town", "Terran 8 - Confederate Town", "Terran 9 - Light Attack", "Terran 9 - Heavy Attack", "Terran 10 - Confederate Towns",
"Terran 11 - Zerg Town", "Terran 11 - Lower Protoss Town", "Terran 11 - Upper Protoss Town", "Terran 12 - Nuke Town", "Terran 12 - Phoenix Town", "Terran 12 - Tank Town",
"Terran 1 - Electronic Distribution", "Terran 2 - Electronic Distribution", "Terran 3 - Electronic Distribution", "Terran 1 - Shareware",
"Terran 2 - Shareware", "Terran 3 - Shareware", "Terran 4 - Shareware", "Terran 5 - Shareware",
"Zerg 1 - Terran Town", "Zerg 2 - Protoss Town", "Zerg 3 - Terran Town", "Zerg 4 - Right Terran Town",
"Zerg 4 - Lower Terran Town", "Zerg 6 - Protoss Town", "Zerg 7 - Air Town", "Zerg 7 - Ground Town",
"Zerg 7 - Support Town", "Zerg 8 - Scout Town", "Zerg 8 - Templar Town", "Zerg 9 - Teal Protoss",
"Zerg 9 - Left Yellow Protoss", "Zerg 9 - Right Yellow Protoss", "Zerg 9 - Left Orange Protoss", "Zerg 9 - Right Orange Protoss",
"Zerg 10 - Left Teal (Attack", "Zerg 10 - Right Teal (Support", "Zerg 10 - Left Yellow (Support", "Zerg 10 - Right Yellow (Attack",
"Zerg 10 - Red Protoss", "Protoss 1 - Zerg Town", "Protoss 2 - Zerg Town", "Protoss 3 - Air Zerg Town",
"Protoss 3 - Ground Zerg Town", "Protoss 4 - Zerg Town", "Protoss 5 - Zerg Town Island", "Protoss 5 - Zerg Town Base",
"Protoss 7 - Left Protoss Town", "Protoss 7 - Right Protoss Town", "Protoss 7 - Shrine Protoss", "Protoss 8 - Left Protoss Town",
"Protoss 8 - Right Protoss Town", "Protoss 8 - Protoss Defenders", "Protoss 9 - Ground Zerg", "Protoss 9 - Air Zerg",
"Protoss 9 - Spell Zerg", "Protoss 10 - Mini-Towns", "Protoss 10 - Mini-Town Master", "Protoss 10 - Overmind Defenders",
"Brood Wars Protoss 1 - Town A", "Brood Wars Protoss 1 - Town B", "Brood Wars Protoss 1 - Town C", "Brood Wars Protoss 1 - Town D",
"Brood Wars Protoss 1 - Town E", "Brood Wars Protoss 1 - Town F", "Brood Wars Protoss 2 - Town A", "Brood Wars Protoss 2 - Town B",
"Brood Wars Protoss 2 - Town C", "Brood Wars Protoss 2 - Town D", "Brood Wars Protoss 2 - Town E", "Brood Wars Protoss 2 - Town F",
"Brood Wars Protoss 3 - Town A", "Brood Wars Protoss 3 - Town B", "Brood Wars Protoss 3 - Town C", "Brood Wars Protoss 3 - Town D",
"Brood Wars Protoss 3 - Town E", "Brood Wars Protoss 3 - Town F", "Brood Wars Protoss 4 - Town A", "Brood Wars Protoss 4 - Town B",
"Brood Wars Protoss 4 - Town C", "Brood Wars Protoss 4 - Town D", "Brood Wars Protoss 4 - Town E", "Brood Wars Protoss 4 - Town F",
"Brood Wars Protoss 5 - Town A", "Brood Wars Protoss 5 - Town B", "Brood Wars Protoss 5 - Town C", "Brood Wars Protoss 5 - Town D",
"Brood Wars Protoss 5 - Town E", "Brood Wars Protoss 5 - Town F", "Brood Wars Protoss 6 - Town A", "Brood Wars Protoss 6 - Town B",
"Brood Wars Protoss 6 - Town C", "Brood Wars Protoss 6 - Town D", "Brood Wars Protoss 6 - Town E", "Brood Wars Protoss 6 - Town F",
"Brood Wars Protoss 7 - Town A", "Brood Wars Protoss 7 - Town B", "Brood Wars Protoss 7 - Town C", "Brood Wars Protoss 7 - Town D",
"Brood Wars Protoss 7 - Town E", "Brood Wars Protoss 7 - Town F", "Brood Wars Protoss 8 - Town A", "Brood Wars Protoss 8 - Town B",
"Brood Wars Protoss 8 - Town C", "Brood Wars Protoss 8 - Town D", "Brood Wars Protoss 8 - Town E", "Brood Wars Protoss 8 - Town F",
"Brood Wars Terran 1 - Town A", "Brood Wars Terran 1 - Town B", "Brood Wars Terran 1 - Town C", "Brood Wars Terran 1 - Town D",
"Brood Wars Terran 1 - Town E", "Brood Wars Terran 1 - Town F", "Brood Wars Terran 2 - Town A", "Brood Wars Terran 2 - Town B",
"Brood Wars Terran 2 - Town C", "Brood Wars Terran 2 - Town D", "Brood Wars Terran 2 - Town E", "Brood Wars Terran 2 - Town F",
"Brood Wars Terran 3 - Town A", "Brood Wars Terran 3 - Town B", "Brood Wars Terran 3 - Town C", "Brood Wars Terran 3 - Town D",
"Brood Wars Terran 3 - Town E", "Brood Wars Terran 3 - Town F", "Brood Wars Terran 4 - Town A", "Brood Wars Terran 4 - Town B",
"Brood Wars Terran 4 - Town C", "Brood Wars Terran 4 - Town D", "Brood Wars Terran 4 - Town E", "Brood Wars Terran 4 - Town F",
"Brood Wars Terran 5 - Town A", "Brood Wars Terran 5 - Town B", "Brood Wars Terran 5 - Town C", "Brood Wars Terran 5 - Town D",
"Brood Wars Terran 5 - Town E", "Brood Wars Terran 5 - Town F", "Brood Wars Terran 6 - Town A", "Brood Wars Terran 6 - Town B",
"Brood Wars Terran 6 - Town C", "Brood Wars Terran 6 - Town D", "Brood Wars Terran 6 - Town E", "Brood Wars Terran 6 - Town F",
"Brood Wars Terran 7 - Town A", "Brood Wars Terran 7 - Town B", "Brood Wars Terran 7 - Town C", "Brood Wars Terran 7 - Town D",
"Brood Wars Terran 7 - Town E", "Brood Wars Terran 7 - Town F", "Brood Wars Terran 8 - Town A", "Brood Wars Terran 8 - Town B",
"Brood Wars Terran 8 - Town C", "Brood Wars Terran 8 - Town D", "Brood Wars Terran 8 - Town E", "Brood Wars Terran 8 - Town F",
"Brood Wars Zerg 1 - Town A", "Brood Wars Zerg 1 - Town B", "Brood Wars Zerg 1 - Town C", "Brood Wars Zerg 1 - Town D",
"Brood Wars Zerg 1 - Town E", "Brood Wars Zerg 1 - Town F", "Brood Wars Zerg 2 - Town A", "Brood Wars Zerg 2 - Town B",
"Brood Wars Zerg 2 - Town C", "Brood Wars Zerg 2 - Town D", "Brood Wars Zerg 2 - Town E", "Brood Wars Zerg 2 - Town F",
"Brood Wars Zerg 3 - Town A", "Brood Wars Zerg 3 - Town B", "Brood Wars Zerg 3 - Town C", "Brood Wars Zerg 3 - Town D",
"Brood Wars Zerg 3 - Town E", "Brood Wars Zerg 3 - Town F", "Brood Wars Zerg 4 - Town A", "Brood Wars Zerg 4 - Town B",
"Brood Wars Zerg 4 - Town C", "Brood Wars Zerg 4 - Town D", "Brood Wars Zerg 4 - Town E", "Brood Wars Zerg 4 - Town F",
"Brood Wars Zerg 5 - Town A", "Brood Wars Zerg 5 - Town B", "Brood Wars Zerg 5 - Town C", "Brood Wars Zerg 5 - Town D",
"Brood Wars Zerg 5 - Town E", "Brood Wars Zerg 5 - Town F", "Brood Wars Zerg 6 - Town A", "Brood Wars Zerg 6 - Town B",
"Brood Wars Zerg 6 - Town C", "Brood Wars Zerg 6 - Town D", "Brood Wars Zerg 6 - Town E", "Brood Wars Zerg 6 - Town F",
"Brood Wars Zerg 7 - Town A", "Brood Wars Zerg 7 - Town B", "Brood Wars Zerg 7 - Town C", "Brood Wars Zerg 7 - Town D",
"Brood Wars Zerg 7 - Town E", "Brood Wars Zerg 7 - Town F", "Brood Wars Zerg 8 - Town A", "Brood Wars Zerg 8 - Town B",
"Brood Wars Zerg 8 - Town C", "Brood Wars Zerg 8 - Town D", "Brood Wars Zerg 8 - Town E", "Brood Wars Zerg 8 - Town F",
"Brood Wars Zerg 9 - Town A", "Brood Wars Zerg 9 - Town B", "Brood Wars Zerg 9 - Town C", "Brood Wars Zerg 9 - Town D",
"Brood Wars Zerg 9 - Town E", "Brood Wars Zerg 9 - Town F", "Brood Wars Zerg 10 - Town A", "Brood Wars Zerg 10 - Town B",
"Brood Wars Zerg 10 - Town C", "Brood Wars Zerg 10 - Town D", "Brood Wars Zerg 10 - Town E", "Brood Wars Zerg 10 - Town F"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, strs(i), strs(i), tb, TextEditor, TECompletionData.EIconType.StarConst))
                    Next
                Case "TrgLocation", "TrgLocationIndex"
                    For i = 0 To 254
                        Dim tb As New TextBox
                        tb.Text = "[" & i & "] " & pjData.MapData.LocationName(i)

                        data.Add(New TECompletionData(0, """" & pjData.MapData.LocationName(i) & """", """" & pjData.MapData.LocationName(i) & """", tb, TextEditor, TECompletionData.EIconType.StarStringConst))
                    Next
                Case "TrgSwitch"
                    For i = 0 To 255
                        Dim tb As New TextBox
                        tb.Text = "[" & i & "] " & pjData.MapData.SwitchName(i)

                        data.Add(New TECompletionData(0, """" & pjData.MapData.SwitchName(i) & """", """" & pjData.MapData.SwitchName(i) & """", tb, TextEditor, TECompletionData.EIconType.StarStringConst))
                    Next
                Case "TrgUnit"
                    '스트링
                    For i = 0 To SCUnitCount - 1
                        Dim tb As New TextBox
                        tb.Text = "[" & i & "] " & pjData.UnitInGameName(i)

                        data.Add(New TECompletionData(0, """" & pjData.CodeLabel(SCDatFiles.DatFiles.units, i) & """", """" & pjData.UnitInGameName(i) & """", tb, TextEditor, TECompletionData.EIconType.StarStringConst))
                    Next

                    Dim strs() As String = {"(men)", "(any unit)", "(factories)", "(buildings)"}

                    For i = 0 To strs.Length - 1
                        Dim tb As New TextBox
                        tb.Text = strs(i)

                        data.Add(New TECompletionData(0, """" & strs(i) & """", """" & strs(i) & """", tb, TextEditor, TECompletionData.EIconType.StarStringConst))
                    Next
            End Select
        End If




        For i = 0 To LocalFunc.FuncCount - 1
            data.Add(New TECompletionData(10, LocalFunc, i, TextEditor, TECompletionData.EIconType.localFunction))
        Next
        For i = 0 To ExternFunc.FuncCount - 1
            data.Add(New TECompletionData(15, ExternFunc, i, TextEditor, TECompletionData.EIconType.Auto))
        Next
        For i = 0 To Tool.TEEpsDefaultFunc.FuncCount - 1
            data.Add(New TECompletionData(20, Tool.TEEpsDefaultFunc, i, TextEditor, TECompletionData.EIconType.Auto))
        Next

        'KeyWord추가
        For i = 0 To KeyWords.Count - 1
            data.Add(New TECompletionData(25, KeyWords(i), KeyWords(i), Nothing, TextEditor, TECompletionData.EIconType.KeyWord))
        Next
        Return data
    End Function
    Private KeyWords() As String = {"object", "static", "once", "if", "else", "for", "function", "foreach",
        "return", "true", "True", "false", "False", "switch", "case", "break", "var", "const"}
    '<Word>object</Word>
    '<Word>static</Word>
    '<Word>once</Word>
    '<Word>if</Word>
    ' <Word>else</Word>
    '  <Word>for</Word>
    '   <Word>function</Word>
    '    <Word>foreach</Word>
    '     <Word>return</Word>
    '
    '<Word>true</Word>
    '<Word>True</Word>
    '<Word>false</Word>
    '<Word>False</Word>


    Private Function GetArgument(name As String, Argindex As Integer) As String
        Dim str As String

        str = LocalFunc.FindArgument(name, Argindex)
        If str <> "" Then
            Return str
        End If

        str = Tool.TEEpsDefaultFunc.FindArgument(name, Argindex)
        If str <> "" Then
            Return str
        End If

        str = ExternFunc.FindArgument(name, Argindex)
        If str <> "" Then
            Return str
        End If

        Return ""
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

        If tToolTip Is Nothing Then
            Dim textb As New TextBlock
            textb.Text = tinputtext
            ToolTip = textb
        Else
            ToolTip = tToolTip
        End If
        TextEditor = tTextEditor
        IconType = tIconType
        _Priority = tPriority
    End Sub
    Public Sub New(tPriority As Double, tfunc As CFunc, FuncIndex As Integer, tTextEditor As TextEditor, tIconType As EIconType)

        'LocalFunc.GetFuncName(i), LocalFunc.GetFuncName(i), LocalFunc.GetFuncTooltip(i)

        Me.Text = tfunc.GetFuncName(FuncIndex)
        inputtext = tfunc.GetFuncName(FuncIndex)
        TextEditor = tTextEditor

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

        Dim textblock As New TextBlock
        textblock.Text = tfunc.GetFuncTooltip(FuncIndex).Summary
        ToolTip = textblock
    End Sub

    Public ReadOnly Property Image As ImageSource Implements ICompletionData.Image
        Get
            Select Case IconType
                Case EIconType.StarConst
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
                Case EIconType.KeyWord
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/KeyWord.png"))
                    Imageicon.Freeze()
                    Return Imageicon
                Case EIconType.SettingValue
                    Dim Imageicon As New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/Setting.png"))
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
