<Serializable>
Public Class ScriptBlock
    Public ReadOnly Property TriggerScript As TriggerScript

    Public Sub New(keyname As String)
        TriggerScript = tescm.GetTriggerScript(keyname)

        Argument = New List(Of ScriptBlock)
        Child = New List(Of ScriptBlock)
    End Sub

    '벨류도 이거로 만듬

    '인자안에 이게 들어있음.



    '트리뷰에 부여된 하나하나의 스크립트

    '데이터와 TriggerScript를 가짐

    '인자 데이터
    '추가데이터 등을 가짐

    '주인 Treeview도 가지고 있음



    Public ReadOnly Property Argument As List(Of ScriptBlock)



    Public ReadOnly Property Child As List(Of ScriptBlock)
End Class

<Serializable>
Public Class TriggerScript
    Public Shared Function GetColor(key As String)
        Dim header As String = Tool.GetText(key)

        Dim colors() As String = header.Split("|").Last.Split(",")

        Return Color.FromRgb(colors(0), colors(1), colors(2))
    End Function

    '스크립트 하나하나를 정의
    Public Enum ScriptType '스크립트의 타입 정의
        Action = 0
        Condition = 1
        Both = 2
        Special = 3
        '룰로 정해진 곳에만 들어올 수 있음

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

    Public Sub New()

    End Sub

    Public Sub New(tSName As String, tIsFolder As Boolean, tIsLock As Boolean, tGroup As String, tSType As ScriptType, tFolderType As ScriptType, tFolderRull As String, tInitBlock As String)
        SName = tSName
        IsFolder = tIsFolder
        IsLock = tIsLock
        Group = tGroup

        SType = tSType
        FolderType = tFolderType

        FolderRull = New List(Of String)
        InitBlock = New List(Of String)



        FolderRull.AddRange(tFolderRull.Split(","))
        InitBlock.AddRange(tInitBlock.Split(","))
    End Sub


    '조건들 정의
    '1. 부모조건
    '2. 자식조건
    '3. 위에 블럭 조건
End Class


