Imports BingsuCodeEditor

Public Class LuaImportManager : Inherits ImportManager
    Public Sub New()
        CodeType = BingsuCodeEditor.CodeTextEditor.CodeType.Lua
    End Sub

    Public Overrides Function GetFIleList() As List(Of String)
        Dim flielist As List(Of String) = New List(Of String)

        For Each t In IO.Directory.GetFiles(MacroManager.LuaFloderPath)
            Dim ext As String = IO.Path.GetExtension(t).ToLower()
            Dim filename As String = IO.Path.GetFileName(t)

            If ext = ".lua" Then
                flielist.Add(filename)
            End If
        Next

        Return flielist
    End Function


    Public Overrides Function GetFIleContent(pullpath As String) As String
        If pullpath <> "DEFAULTFUNCTIONLIST" Then
            Return System.IO.File.ReadAllText(MacroManager.LuaFloderPath & "\" & pullpath)
        Else
            Return ""
        End If
    End Function




    Public Overrides Function GetImportedFileList(Optional basefilename As String = "") As List(Of String)



        Dim rlist As List(Of String) = New List(Of String)




        'rlist.Add("a")
        'rlist.Add("b")
        'rlist.Add("c")
        'rlist.Add("d.f")
        'rlist.Add("functest")

        Return rlist
    End Function


    Public Overrides Function GetDefaultFunctions() As String
        Throw New NotImplementedException()
    End Function
End Class
