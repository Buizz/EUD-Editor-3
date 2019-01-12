Imports LuaInterface

Public Class LuaManager
    Private LuaSc As Lua
    Public Sub DoString(str As String)
        LuaSc.DoString(str)
    End Sub


    Public Sub New()
        LuaSc = New Lua


        LuaSc.RegisterFunction("setdat", Me, Me.GetType().GetMethod("SetDat"))
        LuaSc.RegisterFunction("test", Me, Me.GetType().GetMethod("Test"))
    End Sub
    Public Function Test(str As String) As String
        Return str
    End Function
    Public Sub SetDat(DatName As String, ParamaterName As String, ObjectId As Integer, Value As Long)
        Dim Datfilename As SCDatFiles.DatFiles = SCDatFiles.DatFiles.None

        For i = 0 To SCConst.Datfilesname.Count - 1
            If DatName = SCConst.Datfilesname(i) Then
                Datfilename = i
                Exit For
            End If
        Next

        If Datfilename = SCDatFiles.DatFiles.None Then
            Return
        End If

        pjData.BindingManager.DatBinding(Datfilename, ParamaterName, ObjectId).Value = Value
    End Sub

End Class
