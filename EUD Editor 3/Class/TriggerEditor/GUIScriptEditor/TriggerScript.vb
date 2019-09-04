Imports System.Text

<Serializable>
Public Class ScriptBlock
    Public ReadOnly Property TriggerScript As TriggerScript

    Public Sub New(keyname As String, Optional IsValue As Boolean = False)
        If IsValue Then
            TriggerScript = tescm.GetValue(keyname)
        Else
            TriggerScript = tescm.GetTriggerScript(keyname)
        End If




        ArgumentName = New List(Of String)
        Argument = New List(Of ScriptBlock)
        Child = New List(Of ScriptBlock)

        If TriggerScript.IsHaveValues Then
            For i = 0 To TriggerScript.ValuesDef.Count - 1


                Argument.Add(New ScriptBlock(TriggerScript.ValuesDef(i), True))
            Next
        End If
    End Sub

    Public Sub New(copySb As ScriptBlock)
        TriggerScript = tescm.GetTriggerScript(copySb.TriggerScript.SName)

        ArgumentName = New List(Of String)
        Argument = New List(Of ScriptBlock)
        Child = New List(Of ScriptBlock)

        If TriggerScript.IsHaveValues Then
            For i = 0 To TriggerScript.ValuesDef.Count - 1
                'MsgBox(copySb.Argument(i).TriggerScript.SName)

                Argument.Add(New ScriptBlock(copySb.Argument(i)))
            Next
        End If

        Value = copySb.Value
    End Sub


    '벨류도 이거로 만듬

    '인자안에 이게 들어있음.



    '트리뷰에 부여된 하나하나의 스크립트

    '데이터와 TriggerScript를 가짐

    '인자 데이터
    '추가데이터 등을 가짐

    '주인 Treeview도 가지고 있음



    '선언시 TriggerScript으로 부터 초기화해야함. 
    Public ReadOnly Property ArgumentName As List(Of String)
    Public ReadOnly Property Argument As List(Of ScriptBlock)


    Public Property Value As String
    Public Property ValueType As SCDatFiles.DatFiles




    Public ReadOnly Property Child As List(Of ScriptBlock)

    Private Function GetIntend(count As Integer) As String
        Dim indend As String = ""
        For i = 0 To count - 1
            indend = indend & vbTab
        Next
        Return indend
    End Function
    Public Function ToCode(Optional intend As Integer = 0) As String
        Dim tindend As String = GetIntend(intend)


        Dim returnstr As New StringBuilder

        returnstr.Append(TriggerScript.CodeText.Replace(vbTab, tindend))
        returnstr.Append(TriggerScript.CodeStart.Replace(vbTab, tindend))

        intend += TriggerScript.Intend
        tindend = GetIntend(intend)
        For i = 0 To Child.Count - 1
            If i <> 0 Then
                returnstr.Append(TriggerScript.CodeSeparator.Replace(vbTab, tindend))
            End If
            returnstr.Append(TriggerScript.Codehead.Replace(vbTab, tindend))
            returnstr.Append(Child(i).ToCode(intend))

            If Child(i).TriggerScript.IsFolder = False Then
                returnstr.Append(TriggerScript.Codetail.Replace(vbTab, tindend))
            End If
        Next
        intend -= TriggerScript.Intend
        tindend = GetIntend(intend)

        returnstr.Append(TriggerScript.CodeEnd.Replace(vbTab, tindend))


        Return returnstr.ToString
    End Function
End Class

<Serializable>
Public Class TriggerScript
    Public Shared Function GetColor(key As String, Optional A As Byte = 255)
        Dim header As String = Tool.GetText(key)

        Dim colors() As String = header.Split("|").Last.Split(",")

        If A = 255 Then
            Return Color.FromRgb(colors(0), colors(1), colors(2))
        Else
            Return Color.FromArgb(A, colors(0), colors(1), colors(2))
        End If
    End Function

    '스크립트 하나하나를 정의
    Public Enum ScriptType '스크립트의 타입 정의
        Action = 0
        Condition = 1
        Both = 2
        Special = 3
        '룰로 정해진 곳에만 들어올 수 있음


        OutSide = 4
        Free = 5
        Value = 6

        Null = 99
    End Enum


    Public Property SName As String
    '스크립트 이름 정의 ex "if"
    Public Group As String

    Public IsFolder As Boolean
    Public IsLock As Boolean '이동,복사,생성불가능 판단
    Public IsChildLock As Boolean '이동,복사,생성불가능 판단

    Public SType As ScriptType '스크립트가 어떤 타입인지.
    Public FolderType As ScriptType '내부가 어떤 타입인지


    Public Uniqueness As Boolean
    Public shortheader As Boolean
    Public FolderRull As List(Of String) '비어있으면 아무일도 일어나지 않음
    '만약 들어있으면 들어있는 블럭만 내부로 들어올수있음


    Public InitBlock As List(Of String) '첫 생성시 자식으로 들어가는 블럭 정의

    Public CodeText As String

    Public ReadOnly Property IsHaveValues As Boolean
        Get
            If ValuesDef.Count = 1 And ValuesDef(0).Trim = "" Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property
    Public ValuesDef As List(Of String) '인자들
    Public Texts As List(Of String) '소개글

    Public Intend As Integer
    Public CodeStart As String
    Public CodeEnd As String
    Public Codehead As String
    Public Codetail As String
    Public CodeSeparator As String

    Public Function GetTexts() As String
        If Texts.Count <= 1 Then
            Return ""
        End If

        Dim lan As String = pgData.Setting(ProgramData.TSetting.Language)

        Return Texts(Texts.IndexOf(lan) + 1)
    End Function


    Public Sub New()

    End Sub




    '조건들 정의
    '1. 부모조건
    '2. 자식조건
    '3. 위에 블럭 조건
End Class


