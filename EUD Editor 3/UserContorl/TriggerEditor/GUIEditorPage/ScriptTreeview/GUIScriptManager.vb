Imports System.Text.RegularExpressions

Partial Public Class GUIScriptEditorUI
    Private FuncUseList As New List(Of ScriptTreeviewItem)


    Private Sub AddFuncUseList(TreeViewItem As ScriptTreeviewItem)

        FuncUseList.Add(TreeViewItem)
    End Sub

    Public Sub DeleteFuncUseList(ScriptBlock As ScriptTreeviewItem)
        FuncUseList.Remove(ScriptBlock)


        For i = 0 To FuncUseList.Count - 1
            If FuncUseList(i).Parent Is Nothing Then
                'MsgBox("없음")
            End If
        Next



        'MsgBox("함수제거 : " & FuncUseList.Count)
    End Sub



    Public Shared Function GetArgumentType() As List(Of String)
        Dim TypeStrings As New List(Of String)

        Dim ScriptGroup As String = "Value"

        For i = 0 To tescm.dicTriggerScript(ScriptGroup).Count - 1
            If Not tescm.dicTriggerScript(ScriptGroup)(i).IsLock Then
                Dim keyname As String = tescm.dicTriggerScript(ScriptGroup)(i).SName


                TypeStrings.Add(keyname)
            End If
        Next

        Return TypeStrings
    End Function

    Public Function GetFuncList() As List(Of String)
        Dim FuncList As New List(Of String)

        Dim tmainTreeview As TreeView = MainTreeview

        For i = 0 To tmainTreeview.Items.Count - 1
            Dim tvi As TreeViewItem = tmainTreeview.Items(i)
            Dim stvi As ScriptTreeviewItem = tvi.Header

            If stvi.Script.TriggerScript.SName = "FunctionDefinition" Then
                If FuncList.IndexOf(stvi.Script.Value) = -1 Then
                    FuncList.Add(stvi.Script.Value)
                End If

            End If
        Next


        Return FuncList
    End Function
    Public Function GetFuncScript() As List(Of ScriptBlock)
        Dim FuncList As New List(Of ScriptBlock)

        Dim tmainTreeview As TreeView = MainTreeview

        For i = 0 To tmainTreeview.Items.Count - 1
            Dim tvi As TreeViewItem = tmainTreeview.Items(i)
            Dim stvi As ScriptTreeviewItem = tvi.Header

            If stvi.Script.TriggerScript.SName = "FunctionDefinition" Then
                If CheckvalidationnameFunc(stvi.Script.Value) = "" Then
                    FuncList.Add(stvi.Script)
                End If
            End If
        Next


        Return FuncList
    End Function

    Public Function GetFunc(Funcname As String) As ScriptBlock
        Dim FuncList As List(Of ScriptBlock) = GetFuncScript()

        For i = 0 To FuncList.Count - 1

            If FuncList(i).Value = Funcname Then
                Return FuncList(i)
            End If
        Next

        Return Nothing
    End Function


    Private AbleConst As String = ".,!@#$%^&*()[]{}<>/?:;'""\| "
    Public Function CheckvalidationnameFunc(str As String) As String
        If str Is Nothing Then
            Return "함수 이름은 비어있으면 안됩니다."
        End If
        If str.Trim = "" Then
            Return "함수 이름은 비어있으면 안됩니다."
        End If


        If IsNumeric(str(0)) Then
            Return "함수의 시작은 숫자가 될 수 없습니다."
        End If


        For i = 0 To str.Length - 1
            If AbleConst.IndexOf(str(i)) > -1 Then
                Return "'" & str(i) & "'는 허용되지 않는 문자입니다."
            End If
        Next

        Return ""
    End Function
End Class

Public Class GUIScriptTool
    Public Structure Arg
        Public type As String
        Public tag As String
        Public Sub New(ttype As String, ttag As String)
            type = ttype
            tag = ttag
        End Sub
    End Structure

    Public Shared Function GetArgList(str As String) As List(Of Arg)
        'Arg를 받고 인자 항목을 반환하는 함수. 인자의 타입과 이름을 구분해서 리스트로 리턴함
        Dim rlist As New List(Of Arg)
        If str Is Nothing Then
            Return rlist
        End If

        Dim regex As New Regex("{[^{}]+}")
        Dim mc As MatchCollection = regex.Matches(str)

        For i = 0 To mc.Count - 1
            Dim tstr As String = mc(i).Value
            tstr = Mid(tstr, 2, tstr.Length - 2)



            If tstr.IndexOf(":") = -1 Then
                rlist.Add(New Arg(tstr, tstr))
            Else
                Dim t As String() = tstr.Split(":")
                rlist.Add(New Arg(t.Last, t.First))
            End If
        Next

        Return rlist
    End Function


    Public Shared Function GetInfoArgList(str As String) As List(Of Arg)
        'Arg를 받고 인자 항목을 반환하는 함수. 인자의 타입과 이름을 구분해서 리스트로 리턴함
        Dim rlist As New List(Of Arg)
        If str Is Nothing Then
            Return rlist
        End If

        Dim regex As New Regex("{[^{}]+}")
        Dim mc As MatchCollection = regex.Matches(str)

        Dim lastindex As Integer = 1
        For i = 0 To mc.Count - 1
            If lastindex <> mc(i).Index Then
                Dim label As String = Mid(str, lastindex, mc(i).Index - lastindex + 1)
                lastindex = mc(i).Index + mc(i).Length + 1
                rlist.Add(New Arg("Label", label))
            End If






            Dim tstr As String = mc(i).Value
            tstr = Mid(tstr, 2, tstr.Length - 2)


            If tstr.IndexOf(":") = -1 Then
                rlist.Add(New Arg(tstr, tstr))
            Else
                Dim t As String() = tstr.Split(":")
                rlist.Add(New Arg(t.Last, t.First))
            End If
        Next

        If lastindex <= str.Length Then
            Dim tlabel As String = Mid(str, lastindex)
            rlist.Add(New Arg("Label", tlabel))
        End If




        Return rlist
    End Function
End Class
