Imports System.IO
Imports System.Text

Partial Public Class BuildData
    Public Sub WriteDotData()

        Dim fs As New FileStream(TriggerEditorPath & "\dotData.eps", FileMode.Create)
        Dim sw As New StreamWriter(fs)

        sw.Write(GetDotEps)

        sw.Close()
        fs.Close()
    End Sub

    Public Function GetDotEps() As String
        Dim sb As New StringBuilder

        sb.AppendLine("
const arrdata = EUDArray(list(")

        Dim dotdatas As List(Of DotData) = pjData.TEData.DotData
        For i = 0 To dotdatas.Count - 1
            If (i <> 0) Then
                sb.AppendLine(",")
            End If


            sb.Append(dotdatas(i).GetepsData)

        Next


        sb.Append(")
);

function GetData(id){
	return arrdata[id];
}
")




        Return sb.ToString
    End Function
End Class
