
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text

<Serializable>
Public Class Trigger
    Public CName As String = "Trigger"



    '플레이어
    Public PlayerEnabled(7) As Boolean


    '트리거의 별칭(주석)
    Public CommentStr As String


    Public IsEnabled As Boolean = True
    Public IsPreserved As Boolean = True



    Public Condition As New List(Of TriggerCodeBlock)
    Public Actions As New List(Of TriggerCodeBlock)


    Public Function GetTriggerCodeText(intend As Integer, _scripter As ScriptEditor) As String
        Dim sb As New StringBuilder

        Dim tstart As Integer
        Dim ispace As String = ""
        For i = 0 To (intend * 4) - 1
            ispace = ispace & " "
        Next

        tstart = 0

        Dim rv As New Random

        Dim thash As String = "t" & _scripter.tv
        If Not IsPreserved Then
            sb.AppendLine(ispace & "static var " & thash & " = 0;")
        End If


        If Condition.Count = 0 And IsPreserved Then
            sb.AppendLine(ispace & "{")
        Else
            sb.AppendLine(ispace & "if (")
            If Not IsPreserved Then
                sb.Append(ispace & "    " & thash & " == 0")
                tstart += 1
            End If

            For i = 0 To Condition.Count - 1
                Dim rstr As String = Condition(i).GetCodeText(intend + 1, _scripter)
                If tstart <> 0 Then
                    rstr = " && " & vbCrLf & rstr
                End If
                If rstr <> "" Then
                    sb.Append(rstr)
                    tstart += 1
                End If
            Next
            sb.AppendLine()
            sb.AppendLine(ispace & "){")
        End If


        For i = 0 To Actions.Count - 1
            Dim rstr As String = Actions(i).GetCodeText(intend + 1, _scripter) & ";"
            sb.AppendLine(rstr)
        Next
        If Not IsPreserved Then
            sb.AppendLine(ispace & "    " & thash & " = 1;")
        End If

        sb.AppendLine(ispace & "}")


        _scripter.tv += 1

        'sb.AppendLine(ispace & "Trigger(")
        'sb.AppendLine(ispace & "    conditions = list(")
        'For i = 0 To Condition.Count - 1
        '    Dim rstr As String = Condition(i).GetCodeText(intend + 2, _scripter)
        '    If tstart <> 0 Then
        '        rstr = "," & vbCrLf & rstr
        '    End If
        '    If rstr <> "" Then
        '        sb.Append(rstr)
        '        tstart += 1
        '    End If
        'Next
        'sb.AppendLine()
        'tstart = 0
        'sb.AppendLine(ispace & "    ),")
        'sb.AppendLine(ispace & "    actions = list(")
        'For i = 0 To Actions.Count - 1
        '    Dim rstr As String = Actions(i).GetCodeText(intend + 2, _scripter)
        '    If tstart <> 0 Then
        '        rstr = "," & vbCrLf & rstr
        '    End If
        '    If rstr <> "" Then
        '        sb.Append(rstr)
        '        tstart += 1
        '    End If
        'Next
        'sb.AppendLine()
        'sb.AppendLine(ispace & "    ),")
        'sb.AppendLine(ispace & "    preserved = " & IsPreserved)
        'sb.AppendLine(ispace & ");")

        Return sb.ToString
    End Function





    Public Sub CopyTo(toTrg As Trigger)
        '해당 트리거의 내용을 toTrg에 넣는다.


        toTrg.IsPreserved = IsPreserved
        toTrg.IsEnabled = IsEnabled
        For i = 0 To PlayerEnabled.Count - 1
            toTrg.PlayerEnabled(i) = PlayerEnabled(i)
        Next

        toTrg.CommentStr = CommentStr


        toTrg.Condition.Clear()
        toTrg.Actions.Clear()

        For i = 0 To Condition.Count - 1
            toTrg.Condition.Add(Condition(i).DeepCopy())
        Next
        For i = 0 To Actions.Count - 1
            toTrg.Actions.Add(Actions(i).DeepCopy())
        Next


    End Sub


    Public Function DeepCopy() As Trigger
        Dim stream As MemoryStream = New MemoryStream()


        Dim formatter As BinaryFormatter = New BinaryFormatter()

        formatter.Serialize(stream, Me)
        stream.Position = 0

        Dim rScriptBlock As Trigger = formatter.Deserialize(stream)


        Return rScriptBlock
    End Function
End Class
