Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable>
Public Class ArgValue
    Public DefaultType As String
    Public ValueType As String
    Public ValueString As String
    Public ValueString2 As String
    Public ValueNumber As Long

    '코드로 출력될대 Number이나 String에 있는 값을 출력한다.
    Public IsArgNumber As Boolean

    '언어를 설정을 따르는지
    Public IsLangageable As Boolean

    Public IsQuotation As Boolean

    Public IsInit As Boolean


    Public Sub New(_ValueType As String)
        If _ValueType = "" Then
            _ValueType = "RawCode"
        End If


        IsInit = True
        ValueType = _ValueType
        DefaultType = _ValueType
        IsArgNumber = False
        IsLangageable = False
        IsQuotation = False
    End Sub


    Public Function GetCodeText(_scripter As ScriptEditor, isLuaCover As Boolean) As String
        Dim v As String
        If ValueType = "Variable" Then
            v = ValueString & ValueString2
            If isLuaCover Then
                v = """" & v & """"
            End If
        ElseIf ValueType = "Function" Then
            If CodeBlock Is Nothing Then
                '함수지정 안됨
                v = "FuncNoSelect"
            Else
                '함수
                v = CodeBlock.GetCodeText(0, _scripter, isLuaCover)
                If isLuaCover Then
                    If CodeBlock.FType <> TriggerFunction.EFType.Lua Then
                        v = """" & v & """"
                    End If
                End If
            End If
        Else
            If IsArgNumber Then
                If ValueType = "TrgLocation" Then
                    v = ValueNumber + 1
                Else
                    v = ValueNumber
                End If
            Else
                v = ValueString
                If ValueType = "TrgProperty" Then
                    v = "UnitProperty(" & v & ")"
                End If

                If ValueType = "TrgString" Or ValueType = "FormatString" Or ValueType = "WAVName" Then
                    v = """" & v & """"
                ElseIf isLuaCover Then
                    Dim tlist() As String = {"TrgAllyStatus", "TrgComparison", "TrgModifier", "TrgOrder",
        "TrgPlayer", "TrgPropState", "TrgResource", "TrgScore", "TrgSwitchAction", "TrgSwitchState"}

                    If tlist.ToList.IndexOf(ValueType) = -1 Then
                        v = """" & v & """"
                    End If
                End If
            End If
        End If

        If isLuaCover Then
            v = v.Replace("\", "\\")
        End If

        Return v
    End Function

    Public Function GetEditorText() As String
        Dim v As String
        If ValueType = "Variable" Then
            v = "변수:" & ValueString & ValueString2
        ElseIf ValueType = "Function" Then
            If CodeBlock Is Nothing Then
                v = "함수지정"
            Else
                v = "함수:" & CodeBlock.FName
            End If
        Else
            If IsArgNumber Then
                If ValueType = "TrgLocation" Then
                    v = "ID:" & (ValueNumber + 1) & " " & ValueString
                Else
                    v = "ID:" & ValueNumber & " " & ValueString
                End If
            Else
                If IsLangageable Then
                    Dim lanstr As String = Tool.GetLanText("TrgArg" & ValueString)
                    If lanstr = "TrgArg" & ValueString Then
                        v = ValueString
                    Else
                        v = lanstr
                    End If
                Else
                    v = ValueString
                End If
            End If
        End If

        Return v
    End Function




    Public Sub CopyTo(toArg As ArgValue)
        '해당 트리거의 내용을 toTrg에 넣는다.

        toArg.DefaultType = DefaultType
        toArg.ValueType = ValueType
        toArg.ValueString = ValueString
        toArg.ValueString2 = ValueString2
        toArg.ValueNumber = ValueNumber
        toArg.IsArgNumber = IsArgNumber
        toArg.IsLangageable = IsLangageable
        toArg.IsQuotation = IsQuotation
        toArg.IsInit = IsInit

        If CodeBlock IsNot Nothing Then
            If toArg.CodeBlock Is Nothing Then
                toArg.CodeBlock = CodeBlock.DeepCopy
            Else
                CodeBlock.CopyTo(toArg.CodeBlock)
            End If
        Else
            toArg.CodeBlock = Nothing
        End If
    End Sub
    Public Function DeepCopy() As ArgValue
        Dim stream As MemoryStream = New MemoryStream()


        Dim formatter As BinaryFormatter = New BinaryFormatter()

        formatter.Serialize(stream, Me)
        stream.Position = 0

        Dim rScriptBlock As ArgValue = formatter.Deserialize(stream)


        Return rScriptBlock
    End Function


    Public CodeBlock As TriggerCodeBlock
End Class
