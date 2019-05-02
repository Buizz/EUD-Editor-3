Imports System.Text.RegularExpressions

Public Class CFunc
    Private FuncNames As List(Of String)
    Private FuncArgument As List(Of String)

    Public ReadOnly Property FuncCount As Integer
        Get
            Return FuncNames.Count
        End Get
    End Property
    Public ReadOnly Property GetFuncName(index As Integer) As String
        Get
            Return FuncNames(index)
        End Get
    End Property
    Public ReadOnly Property GetFuncArgument(index As Integer) As String
        Get
            Return FuncArgument(index)
        End Get
    End Property


    Public Function FindArgument(name As String, Argindex As Integer) As String
        Dim index As Integer = FuncNames.IndexOf(name)

        If index >= 0 Then
            Dim Arguments() As String = FuncArgument(index).Split(",")

            If Argindex < Arguments.Count Then
                Return Arguments(Argindex)
            End If
        End If

        Return ""
    End Function




    Public Function GetToolTip(name As String, Argindex As Integer) As Border
        Dim index As Integer = FuncNames.IndexOf(name)

        If index >= 0 Then
            Dim TBorder As New Border
            Dim sp As New StackPanel
            sp.Orientation = Orientation.Horizontal

            Dim textbox1 As New TextBlock
            textbox1.Text = FuncNames(index)
            textbox1.Foreground = Brushes.Red
            sp.Children.Add(textbox1)



            Dim textbox3 As New TextBlock
            textbox3.Text = "("
            sp.Children.Add(textbox3)

            Dim Arguments() As String = FuncArgument(index).Split(",")
            For i = 0 To Arguments.Count - 1
                Dim textbox2 As New TextBlock

                If Argindex = i Then
                    textbox2.Foreground = Brushes.Blue
                    textbox2.FontWeight = FontWeights.UltraBold
                End If
                If i = 0 Then
                    textbox2.Text = Arguments(i).Trim
                Else
                    textbox2.Text = "," & Arguments(i).Trim
                End If


                sp.Children.Add(textbox2)
            Next

            Dim textbox4 As New TextBlock
            textbox4.Text = ")"
            sp.Children.Add(textbox4)






            TBorder.Child = sp



            Return TBorder
        End If


        Return Nothing
    End Function




    Public Sub New()
        FuncNames = New List(Of String)
        FuncArgument = New List(Of String)
    End Sub

    'Private arguments As List(Of FunArgument)

    Public Sub LoadFunc(str As String)
        FuncNames.Clear()
        FuncArgument.Clear()


        Dim fregex As New Regex("function[\s]+([\w\d]+)\((.*)\)")

        Dim matches As MatchCollection = fregex.Matches(str)


        'MsgBox(matches.Count)
        For i = 0 To matches.Count - 1
            FuncNames.Add(matches(i).Groups(1).Value)
            FuncArgument.Add(matches(i).Groups(2).Value)
        Next


    End Sub
End Class

