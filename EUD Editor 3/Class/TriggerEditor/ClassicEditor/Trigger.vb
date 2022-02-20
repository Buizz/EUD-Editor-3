
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text

<Serializable>
Public Class Trigger
    Public CName As String = "Trigger"



    <NonSerialized>
    Public parentscripter As ScriptEditor

    <NonSerialized>
    Public StartLine As Integer = -1
    <NonSerialized>
    Public EndLine As Integer = -1




    Public CodeText As String


    Public IsOnlyCode As Boolean = False




    '플레이어
    Public PlayerEnabled(7) As Boolean


    '트리거의 별칭(주석)
    Public Property CommentString As String


    Public ReadOnly Property HaveComment As System.Windows.Visibility
        Get
            If String.IsNullOrEmpty(CommentString) Then
                Return System.Windows.Visibility.Collapsed
            Else
                Return System.Windows.Visibility.Visible
            End If
        End Get
    End Property

    Public ReadOnly Property NotHaveComment As System.Windows.Visibility
        Get
            If String.IsNullOrEmpty(CommentString) Then
                Return System.Windows.Visibility.Visible
            Else
                Return System.Windows.Visibility.Collapsed
            End If
        End Get
    End Property


    Public ReadOnly Property ConditionString As String
        Get
            Dim rstr As String = ""

            For Each item In Condition
                If rstr <> "" Then
                    rstr = rstr & vbCrLf & item.GetEditorText(parentscripter)
                Else
                    rstr = rstr & item.GetEditorText(parentscripter)
                End If
            Next

            Return rstr
        End Get
    End Property


    Public ReadOnly Property ActionsString As String
        Get
            Dim rstr As String = ""

            For Each item In Actions
                If rstr <> "" Then
                    rstr = rstr & vbCrLf & item.GetEditorText(parentscripter)
                Else
                    rstr = rstr & item.GetEditorText(parentscripter)
                End If
            Next

            Return rstr
        End Get
    End Property



    Public IsEnabled As Boolean = True
    Public IsPreserved As Boolean = True



    Public Condition As New List(Of TriggerCodeBlock)
    Public Actions As New List(Of TriggerCodeBlock)


    Public Function GetTriggerCodeText(intend As Integer, _scripter As ScriptEditor) As String
        Dim sb As New StringBuilder

        Dim thash As String = "PreserveFlag"

        Dim tstart As Integer = 0
        Dim ispace As String = ""
        For i = 0 To (intend * 4) - 1
            ispace = ispace & " "
        Next


        Dim IsWaitable As Boolean = False
        For i = 0 To Actions.Count - 1
            If Actions(i).FType = TriggerFunction.EFType.Lua And Actions(i).FName = "Wait" Then
                IsWaitable = True
                Exit For
            End If
        Next

        If Not IsOnlyCode Then
            sb.AppendLine(ispace & "{")
        End If


        sb.AppendLine(CodeText)


        If Not IsOnlyCode Then
            If IsWaitable Then

                If Not IsPreserved Then
                    sb.AppendLine(ispace & "    static var " & thash & " = 0;")
                End If
                sb.AppendLine(ispace & "    <? WaitStart()?>")



                If Not IsPreserved Then
                    sb.AppendLine(ispace & "    if (" & thash & " == 0){")
                End If
                sb.AppendLine(ispace & "    <? WaitConditionStart()?>")


                Dim conText As String = ""
                For i = 0 To Condition.Count - 1
                    Dim rstr As String = Condition(i).GetCodeText(intend + 2, _scripter)
                    If tstart <> 0 Then
                        rstr = " && " & vbCrLf & rstr
                    End If
                    If rstr <> "" Then
                        conText = conText & rstr
                        tstart += 1
                    End If
                Next
                If conText = "" Then
                    conText = ispace & "        Always()"
                End If
                sb.AppendLine(conText)


                sb.AppendLine(ispace & "    <? WaitConditionEnd()?>")
                sb.AppendLine(ispace & "    <? WaitActionStart()?>")


                For i = 0 To Actions.Count - 1
                    Dim rstr As String = Actions(i).GetCodeText(intend + 2, _scripter) & ";"
                    sb.AppendLine(rstr)
                Next
                If Not IsPreserved Then
                    sb.AppendLine(ispace & "        " & thash & " = 1;")
                End If


                sb.AppendLine(ispace & "    <? WaitActionEnd()?>")
                sb.AppendLine(ispace & "    <? WaitEnd()?>")

                If Not IsPreserved Then
                    sb.AppendLine(ispace & "    }")
                End If



            Else

                If Not IsPreserved Then
                    sb.AppendLine(ispace & "    static var " & thash & " = 0;")
                End If
                If Not (Condition.Count = 0 And IsPreserved) Then
                    sb.AppendLine(ispace & "    if (")
                    If Not IsPreserved Then
                        sb.Append(ispace & "        " & thash & " == 0")
                        tstart += 1
                    End If

                    For i = 0 To Condition.Count - 1
                        Dim rstr As String = Condition(i).GetCodeText(intend + 2, _scripter)
                        If tstart <> 0 Then
                            rstr = " && " & vbCrLf & rstr
                        End If
                        If rstr <> "" Then
                            sb.Append(rstr)
                            tstart += 1
                        End If
                    Next
                    sb.AppendLine()
                    sb.AppendLine(ispace & "    ){")
                End If
                For i = 0 To Actions.Count - 1
                    Dim rstr As String = Actions(i).GetCodeText(intend + 2, _scripter) & ";"
                    sb.AppendLine(rstr)
                Next
                If Not IsPreserved Then
                    sb.AppendLine(ispace & "        " & thash & " = 1;")
                End If

                If Not (Condition.Count = 0 And IsPreserved) Then
                    sb.AppendLine(ispace & "    }")
                End If
            End If
        End If


        If Not IsOnlyCode Then
            sb.AppendLine(ispace & "}")
        End If






        Return sb.ToString
    End Function





    Public Sub CopyTo(toTrg As Trigger)
        '해당 트리거의 내용을 toTrg에 넣는다.


        toTrg.IsPreserved = IsPreserved
        toTrg.IsEnabled = IsEnabled
        toTrg.IsOnlyCode = IsOnlyCode
        toTrg.CodeText = CodeText
        For i = 0 To PlayerEnabled.Count - 1
            toTrg.PlayerEnabled(i) = PlayerEnabled(i)
        Next

        toTrg.CommentString = CommentString


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
