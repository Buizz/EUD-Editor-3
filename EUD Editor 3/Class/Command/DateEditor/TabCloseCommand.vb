Imports System.ComponentModel
Imports Dragablz

Public Class TabCloseCommand
    Implements ICommand

    Public Enum CommandType
        RightClose
        OtherClose
    End Enum

    Private CurrentTab As TabItem

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged


    Public Sub New(Ctab As TabItem)
        CurrentTab = Ctab
    End Sub


    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Dim PType As CommandType = parameter

        Dim ParentTab As TabablzControl = CurrentTab.Parent
        Dim CurrentIndex As Integer = ParentTab.Items.IndexOf(CurrentTab)
        Dim ParentSize As Integer = ParentTab.Items.Count

        '0 1 2(현재) 3 4 5 6
        '카운트 = 7

        Select Case PType
            Case CommandType.RightClose
                For i = 1 To ParentSize - CurrentIndex - 1
                    ParentTab.Items.RemoveAt(CurrentIndex + 1)
                Next
            Case CommandType.OtherClose
                For i = 1 To CurrentIndex
                    ParentTab.Items.RemoveAt(0)
                Next
                For i = 1 To ParentSize - CurrentIndex - 1
                    ParentTab.Items.RemoveAt(1)
                Next
        End Select

    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        'Dim PType As CommandType = parameter

        'Dim ParentTab As TabablzControl = CurrentTab.Parent
        'Dim CurrentIndex As Integer = ParentTab.Items.IndexOf(CurrentTab)
        'Dim ParentSize As Integer = ParentTab.Items.Count

        'Select Case PType
        '    Case CommandType.RightClose
        '        If ParentSize - CurrentIndex - 1 = 0 Then
        '            Return False
        '        Else
        '            Return True
        '        End If
        '    Case CommandType.OtherClose
        '        If CurrentIndex = 0 And ParentSize - CurrentIndex - 1 = 0 Then
        '            Return False
        '        Else
        '            Return True
        '        End If
        'End Select

        Return True
    End Function
End Class

Public Class TabCloseEnabled

    Private CurrentTab As TabItem
    Dim RightCloseMenuItem As New MenuItem
    Dim OtherCloseMenuItem As New MenuItem

    Public Sub New(Ctab As TabItem, RCloseMenuItem As MenuItem, OCloseMenuItem As MenuItem)
        CurrentTab = Ctab
        RightCloseMenuItem = RCloseMenuItem
        OtherCloseMenuItem = OCloseMenuItem
    End Sub


    Public Sub OpenEvent()
        Dim ParentTab As TabablzControl = CurrentTab.Parent
        Dim CurrentIndex As Integer = ParentTab.Items.IndexOf(CurrentTab)
        Dim ParentSize As Integer = ParentTab.Items.Count

        If ParentSize - CurrentIndex - 1 = 0 Then
            RightCloseMenuItem.IsEnabled = False
        Else
            RightCloseMenuItem.IsEnabled = True
        End If

        If CurrentIndex = 0 And ParentSize - CurrentIndex - 1 = 0 Then
            OtherCloseMenuItem.IsEnabled = False
        Else
            OtherCloseMenuItem.IsEnabled = True
        End If
    End Sub


End Class