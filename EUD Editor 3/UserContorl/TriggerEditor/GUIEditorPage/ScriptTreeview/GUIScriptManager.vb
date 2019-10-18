Partial Public Class GUIScriptEditorUI
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
                FuncList.Add(stvi.Script.Value)
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
                FuncList.Add(stvi.Script)
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
End Class
