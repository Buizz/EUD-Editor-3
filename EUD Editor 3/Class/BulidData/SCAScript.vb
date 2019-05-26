Imports System.Text

Partial Public Class BuildData
    Public Function GetSCAEps() As String



        Dim sb As New StringBuilder
        sb.AppendLine("function Init(){")
        sb.AppendLine("    MPQAddFile('SCARCHIVEMAPCODE', py_open('scatempfile', 'rb').read());")
        sb.AppendLine("}")


        Return sb.ToString
    End Function
End Class
