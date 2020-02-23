Imports MaterialDesignThemes.Wpf

Public Class FunctionDefine
    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        valueList.Children.Clear()

        For Each s As String In GUIScriptEditorUI.GetArgumentType

            Dim tChip As New Chip
            tChip.Tag = s
            tChip.Background = New SolidColorBrush(Color.FromRgb(&HFF, &H80, &H40))

            Dim tt As String
            tt = Tool.GetText(s)
            If tt = "" Then
                tChip.Content = s
            Else
                tChip.Content = tt
            End If




            AddHandler tChip.Click, AddressOf valueAdd
            valueList.Children.Add(tChip)
        Next
    End Sub

    Private script As ScriptBlock
    Private sb As ScriptBlockItem


    Private Property FuncArg As String
        Get
            If script.Argument.Count = 0 Then
                script.Argument.Add(New ScriptBlock("Label", True))
            End If
            Return script.Argument.First.Value
        End Get
        Set(value As String)
            If script.Argument.Count = 0 Then
                script.Argument.Add(New ScriptBlock("Label", True))
            End If
            script.Argument.First.Value = value
        End Set
    End Property



    Private Sub Grprefresh()
        isGrpChange = True
        FuncBody.Text = FuncArg
        isGrpChange = False
    End Sub


    Public Sub init(tscript As ScriptBlock, tsb As ScriptBlockItem)
        script = tscript
        sb = tsb


        Grprefresh()
    End Sub



    'Private Sub valueDelete(sender As Object, e As RoutedEventArgs)
    '    Dim tChip As Chip = sender
    '    Dim index As Integer = tChip.Tag

    '    script.Argument.RemoveAt(index)
    '    script.ArgumentName.RemoveAt(index)



    '    Grprefresh()
    '    sb.BlockGraphic()
    'End Sub

    Private Sub valueAdd(sender As Object, e As RoutedEventArgs)
        Dim tChip As Chip = sender
        Dim keyname As String = tChip.Tag

        FuncArg = FuncArg & "{" & keyname & "}"



        'MsgBox("변수추가")

        'script.Argument.Add(New ScriptBlock(keyname, True))
        'script.ArgumentName.Add(keyname)

        ''MsgBox(tChip.Tag)

        sb.BlockGraphic()
        Grprefresh()
    End Sub

    Private isGrpChange As Boolean
    Private Sub FuncBody_TextChanged(sender As Object, e As TextChangedEventArgs)
        If Not isGrpChange Then
            Dim ttext As String = FuncBody.Text

            FuncArg = ttext


            sb.BlockGraphic()
        End If
    End Sub
End Class
