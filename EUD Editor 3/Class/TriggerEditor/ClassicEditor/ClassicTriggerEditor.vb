

Imports System.Collections.ObjectModel
Imports System.Text

<Serializable>
Public Class ClassicTriggerEditor
    Inherits ScriptEditor




    '모든 트리거가 모여있음
    Public TriggerListCollection As New ObservableCollection(Of Trigger)


    '모든 트리거가 모여있음
    Public TriggerList As List(Of Trigger)


    '임포트된 파일들 모음
    Public ImportFiles As New List(Of DefineImport)

    Public Sub RefreshData()
        ImportFileRefresh()
    End Sub


    Public Sub LoadInit()
        If TriggerListCollection Is Nothing Then
            TriggerListCollection = New ObservableCollection(Of Trigger)
            '아무것도 없을 경우 구버전이므로 콜랙션으로 업데이트 해준다.
            For Each item In TriggerList
                TriggerListCollection.Add(item)
            Next


            TriggerList = Nothing
        End If

        For Each item In TriggerListCollection
            item.parentscripter = Me
        Next
    End Sub


    Public Sub ImportFileRefresh()
        If ImportFuncs Is Nothing Then
            ImportFuncs = New List(Of TriggerFunction)
        End If

        If ImportVars Is Nothing Then
            ImportVars = New List(Of DefineVariable)
        End If


        ImportFuncs.Clear()
        ImportVars.Clear()

        For i = 0 To ImportFiles.Count - 1
            Dim importLink As String = ImportFiles(i).vfolder
            Dim tTEFile As TEFile = pjData.TEData.GetTEFile(Nothing, importLink.Split("."), 0)


            Dim tText As String = tTEFile.Scripter.GetFileText


            ImportFuncs.AddRange(tmanager.GetListFromEpScript(tText, TriggerFunction.EFType.ExternFunc, ImportFiles(i).vname))
            ImportVars.AddRange(tmanager.GetVariableFromEpScript(tText, ImportFiles(i).vname))
        Next
    End Sub

    <NonSerialized>
    Public ImportFuncs As List(Of TriggerFunction)



    <NonSerialized>
    Public ImportVars As List(Of DefineVariable)




    '글로벌 변수 모음
    Public globalVar As New List(Of DefineVariable)






    Public Overrides Property ConnectFile As String
        Get
            'Throw New NotImplementedException()
            Return ""
        End Get
        Set(value As String)
            'Throw New NotImplementedException()
        End Set
    End Property

    Public Overrides Function GetFileText() As String
        Dim returnStr As String = GetEpsText(IsMain())

        If IsMain() Then
            returnStr = returnStr &
"function beforeTriggerExec(){
	ClassicTriggerExec();
}"
        End If
        returnStr = macro.MacroApply("import PluginVariables as msqcvar;" & vbCrLf & returnStr, IsMain())
        Return returnStr
    End Function

    Public Overrides Function GetStringText() As String
        Return GetEpsText(IsMain())
    End Function



    Public Function GetLine(bLine As Integer) As Integer
        For i = 0 To TriggerListCollection.Count - 1
            Dim trg As Trigger = TriggerListCollection(i)
            If trg.StartLine <= bLine And bLine < trg.EndLine Then
                Return i
            End If
        Next
        Return -1
    End Function





    Public Function GetEpsText(Optional ByVal IsMainFile As Boolean = False) As String
        Dim sb As New StringBuilder
        sb.AppendLine("/*================Start Import================*/")
        For i = 0 To ImportFiles.Count - 1
            sb.AppendLine("import " & ImportFiles(i).vfolder & " as " & ImportFiles(i).vname & ";")
        Next
        sb.AppendLine("/*================End Import================*/")

        sb.AppendLine("/*================Start Var================*/")
        For i = 0 To globalVar.Count - 1
            Dim vtype As String = ""
            Dim initv As String = globalVar(i).vinit
            Select Case globalVar(i).vtype
                Case "Default"
                    vtype = "var"
                    If initv = "NULL" Then
                        sb.AppendLine(vtype & " " & globalVar(i).vname & ";")
                    Else
                        sb.AppendLine(vtype & " " & globalVar(i).vname & " = " & initv & ";")
                    End If
                Case "Const"
                    vtype = "const"
                    If initv = "NULL" Then
                        sb.AppendLine(vtype & " " & globalVar(i).vname & " = 0;")
                    Else
                        sb.AppendLine(vtype & " " & globalVar(i).vname & " = " & initv & ";")
                    End If
                Case "PVariable"
                    vtype = "const"
                    If initv = "NULL" Then
                        sb.AppendLine(vtype & " " & globalVar(i).vname & " = PVariable();")
                    Else
                        sb.AppendLine(vtype & " " & globalVar(i).vname & " = PVariable(list(" & initv & "));")
                    End If
                Case "Array"
                    vtype = "const"
                    If initv = "NULL" Then
                        sb.AppendLine(vtype & " " & globalVar(i).vname & " = EUDArray(10);")
                    Else
                        If initv.IndexOf(",") = -1 Then
                            sb.AppendLine(vtype & " " & globalVar(i).vname & " = EUDArray(" & initv & ");")
                        Else
                            sb.AppendLine(vtype & " " & globalVar(i).vname & " = [" & initv & "];")
                        End If
                    End If
                Case "VArray"
                    vtype = "const"
                    If initv = "NULL" Then
                        sb.AppendLine(vtype & " " & globalVar(i).vname & " = EUDVArray(10)();")
                    Else
                        If initv.IndexOf(",") = -1 Then
                            sb.AppendLine(vtype & " " & globalVar(i).vname & " = EUDVArray(" & initv & ")();")
                        Else
                            sb.AppendLine(vtype & " " & globalVar(i).vname & " = VArray(" & initv & ");")
                        End If
                    End If
            End Select
        Next
        sb.AppendLine("/*================End Var================*/")



        Dim playerTrigger As New Dictionary(Of Integer, List(Of Trigger))
        For i = 0 To 7
            playerTrigger.Add(i, New List(Of Trigger))
        Next

        For i = 0 To TriggerListCollection.Count - 1
            For p = 0 To 7
                If TriggerListCollection(i).PlayerEnabled(p) Then
                    TriggerListCollection(i).StartLine = -1
                    TriggerListCollection(i).EndLine = -1
                    playerTrigger(p).Add(TriggerListCollection(i))
                End If
            Next
        Next


        sb.AppendLine("function ClassicTriggerExec(){")
        If IsMainFile = False Then
            sb.AppendLine("    const _origcp = getcurpl();")
        End If
        For player = 0 To 7
            Dim tlist As List(Of Trigger) = playerTrigger(player)
            If tlist.Count = 0 Then
                Continue For
            End If
            sb.AppendLine("    /*================Start Player " & player + 1 & "================*/")



            sb.AppendLine("    if (playerexist(" & player & "))")
            sb.AppendLine("    {")
            sb.AppendLine("        setcurpl(" & player & ");")
            sb.AppendLine("        const cp = " & player & ";")
            sb.AppendLine("        <? LuaPlayerVariable = " & player & "?>")

            For t = 0 To tlist.Count - 1
                Dim StartLine As Integer = sb.ToString.Split(vbCrLf).Length
                Dim trg As Trigger = tlist(t)
                If trg.IsEnabled Then
                    sb.Append(trg.GetTriggerCodeText(2, Me))
                End If
                Dim EndLine As Integer = sb.ToString.Split(vbCrLf).Length

                If trg.StartLine = -1 Then
                    trg.StartLine = StartLine
                    trg.EndLine = EndLine
                End If
            Next
            sb.AppendLine("    }")



            sb.AppendLine("    /*================End Player " & player + 1 & "================*/")
        Next
        If IsMainFile = False Then
            sb.AppendLine("    setcurpl(_origcp);")
        End If
        sb.AppendLine("}")


        Return sb.ToString
    End Function

    Public Overrides Function CheckConnect() As Boolean
        Return False 'Throw New NotImplementedException()
    End Function
End Class

<Serializable>
Public Class DefineVariable
    Public vname As String
    Public vgroup As String
    Public vtype As String
    Public vinit As String

    Public Sub New(_vname As String, _vtype As String, _vinit As String, _vgroup As String)
        vname = _vname
        vtype = _vtype
        vinit = _vinit
        vgroup = _vgroup
    End Sub
End Class

<Serializable>
Public Class DefineImport
    Public vname As String
    Public vfolder As String
    Public Sub New(_vname As String, _vfolder As String)
        vname = _vname
        vfolder = _vfolder
    End Sub
End Class