Imports System
Imports System.Collections.Generic
Imports ICSharpCode.AvalonEdit.Document
Imports ICSharpCode.AvalonEdit.Folding
Imports System.Runtime.InteropServices

Public Class EPSFoldingStrategy

    Public Property OpeningBrace As Char
    Public Property ClosingBrace As Char

    Public Sub New()
        Me.OpeningBrace = "{"c
        Me.ClosingBrace = "}"c
    End Sub


    Public Sub UpdateFoldings(manager As FoldingManager, document As TextDocument)
        Dim firstErrorOffset As Integer
        Dim foldings As IEnumerable(Of NewFolding) = CreateNewFoldings(document, firstErrorOffset)
        manager.UpdateFoldings(foldings, firstErrorOffset)
    End Sub


    Public Function CreateNewFoldings(ByVal document As TextDocument, <Out> ByRef firstErrorOffset As Integer) As IEnumerable(Of NewFolding)
        firstErrorOffset = -1
        Return CreateNewFoldings(document)
    End Function

    Public Function CreateNewFoldings(ByVal document As ITextSource) As IEnumerable(Of NewFolding)
        Dim newFoldings As List(Of NewFolding) = New List(Of NewFolding)()
        Dim startOffsets As Stack(Of Integer) = New Stack(Of Integer)()
        Dim lastNewLineOffset As Integer = 0
        Dim openingBrace As Char = Me.OpeningBrace
        Dim closingBrace As Char = Me.ClosingBrace

        For i As Integer = 0 To document.TextLength - 1
            Dim c As Char = document.GetCharAt(i)

            If c = openingBrace Then
                startOffsets.Push(i)
            ElseIf c = closingBrace AndAlso startOffsets.Count > 0 Then
                Dim startOffset As Integer = startOffsets.Pop()

                If startOffset <lastNewLineOffset Then
                    newFoldings.Add(New NewFolding(startOffset, i + 1))
                End If
            ElseIf c = vbLf OrElse c = vbCr Then
                lastNewLineOffset = i + 1
            End If
        Next

        newFoldings.Sort(Function(a, b) a.StartOffset.CompareTo(b.StartOffset))
        Return newFoldings
    End Function
End Class

