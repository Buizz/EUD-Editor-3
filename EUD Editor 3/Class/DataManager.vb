Public Class DataManager
    '복사 붙여넣기 등등을 담당.
    '바인딩을 이용하는게 특징.


    Public Sub CopyDatObject(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        Dim tempStr As String = ""
        For i = 0 To pjData.Dat.GetDatFile(DatFiles).ParamaterList.Count - 1
            Dim Paramname As String = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetParamname

            If pjData.BindingManager.DatBinding(DatFiles, Paramname, ObjectID) Is Nothing Then
                Continue For
            End If

            Dim ValueCount As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarCount)
            Dim ValueStart As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarStart)
            Dim ValueEnd As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarEnd)



            Dim Value As Long = pjData.Dat.Data(DatFiles, Paramname, ObjectID)
            tempStr = tempStr & Paramname & " : " & Value & vbCrLf
        Next

        My.Computer.Clipboard.SetText(tempStr)
    End Sub
    Public Sub PasteDatObject(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        If PasteAble(DatFiles, ObjectID) Then
            MsgBox("PasteDatObject " & DatFiles & "," & ObjectID)
        End If
    End Sub
    Public Sub ResetDatObject(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        For i = 0 To pjData.Dat.GetDatFile(DatFiles).ParamaterList.Count - 1
            Dim Paramname As String = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetParamname

            If pjData.BindingManager.DatBinding(DatFiles, Paramname, ObjectID) Is Nothing Then
                Continue For
            End If



            Dim ValueCount As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarCount)
            Dim ValueStart As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarStart)
            Dim ValueEnd As Integer = pjData.Dat.GetDatFile(DatFiles).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarEnd)

            pjData.BindingManager.DatBinding(DatFiles, Paramname, ObjectID).DataReset()
            'For j = ValueStart To ValueEnd
            '    pjData.BindingManager.DatBinding(DatFiles, Paramname, j).DataReset()
            'Next
        Next
    End Sub



    Public Function PasteAble(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer) As Boolean
        Return False
    End Function
End Class
