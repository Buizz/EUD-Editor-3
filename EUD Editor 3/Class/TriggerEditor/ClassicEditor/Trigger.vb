
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable>
Public Class Trigger
    Public CName As String = "Trigger"





    '플레이어
    Public PlayerEnabled(7) As Boolean


    '트리거의 별칭(주석)
    Public CommentStr As String


    Public IsEnabled As Boolean = True
    Public Function IsPreserved() As Boolean
        '트리거의 무한반복 여부(액션을 한번 봐서 있는지 확인한다.)
        Return False
    End Function


    Public Condition As New List(Of TriggerCodeBlock)
    Public Actions As New List(Of TriggerCodeBlock)



    Public Sub CopyTo(toTrg As Trigger)
        '해당 트리거의 내용을 toTrg에 넣는다.

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
