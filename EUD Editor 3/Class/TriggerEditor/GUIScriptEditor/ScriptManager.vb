Imports Newtonsoft.Json
Imports System.IO

Public Class ScriptManager


    Private TriggerScript As List(Of TriggerScript)
    Private tTriggerScript As Dictionary(Of String, TriggerScript)
    Public dicTriggerScript As Dictionary(Of String, List(Of TriggerScript))

    Public ReadOnly Property GetTriggerScript(key As String) As TriggerScript
        Get
            Return tTriggerScript(key)
        End Get
    End Property

    Public Sub New()
        dicTriggerScript = New Dictionary(Of String, List(Of TriggerScript))
        tTriggerScript = New Dictionary(Of String, TriggerScript)
        'If True Then
        '    Dim tts As New TriggerScript("If", True, False, EUD_Editor_3.TriggerScript.ScriptGroup.Control, EUD_Editor_3.TriggerScript.ScriptType.Action, EUD_Editor_3.TriggerScript.ScriptType.Null,
        '                             "Condition, Then, Else", "Condition, Then")


        '    TriggerScript.Add(tts)
        '    ControlScript.Add(tts)
        'End If

        'If True Then
        '    Dim tts As New TriggerScript("SetMemory", False, False, EUD_Editor_3.TriggerScript.ScriptGroup.Func, EUD_Editor_3.TriggerScript.ScriptType.Action, EUD_Editor_3.TriggerScript.ScriptType.Null,
        '                             "", "")


        '    TriggerScript.Add(tts)
        '    FuncScript.Add(tts)
        'End If

        Dim fs As New FileStream(Tool.TriggerEditorPath("GUIScript.txt"), FileMode.Open)
        Dim sw As New StreamReader(fs)

        TriggerScript = JsonConvert.DeserializeObject(Of List(Of TriggerScript))(sw.ReadToEnd)
        sw.Close()
        fs.Close()

        For i = 0 To TriggerScript.Count - 1
            Dim group As String = TriggerScript(i).Group
            Dim sname As String = TriggerScript(i).SName
            If dicTriggerScript.ContainsKey(group) Then
                dicTriggerScript(group).Add(TriggerScript(i))
            Else
                dicTriggerScript.Add(group, New List(Of TriggerScript))
                dicTriggerScript(group).Add(TriggerScript(i))
            End If

            If Not tTriggerScript.ContainsKey(sname) Then
                tTriggerScript.Add(sname, TriggerScript(i))
            End If
        Next




        'Dim fs As New FileStream("C:\Users\LeeJungHun\Desktop\새 텍스트 문서.txt", FileMode.Create)
        'Dim sw As New StreamWriter(fs)

        'sw.Write(JsonConvert.SerializeObject(TriggerScript))

        'sw.Close()
        'fs.Close()
    End Sub
End Class
