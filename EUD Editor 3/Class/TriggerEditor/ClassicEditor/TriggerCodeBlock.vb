Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary


<Serializable>
Public Class TriggerCodeBlock
    Public CName As String = "TriggerCodeBlock"


    Public Function GetCodeText(intend As Integer, _scripter As ScriptEditor, Optional IsLuaCode As Boolean = False) As String
        Dim tfun As TriggerFunction = GetCodeFunction(_scripter)
        If tfun Is Nothing Then
            Return ""
        End If
        Dim ispace As String = ""
        For i = 0 To (intend * 4) - 1
            ispace = ispace & " "
        Next

        Dim firstCode As Boolean = False

        Dim rstr As String = ""
        If Not IsLuaCode Then
            '루아코드 바깥
            If FType = TriggerFunction.EFType.Lua Then
                rstr = "<?"
                firstCode = True
                IsLuaCode = True
            End If
        End If


        If FType = TriggerFunction.EFType.ExternFunc Then
            rstr = rstr & FGroup & "." & FName & "("
        Else
            rstr = rstr & FName & "("
        End If
        For i = 0 To Args.Count - 1
            If i <> 0 Then
                rstr = rstr & ", "
            End If
            rstr = rstr & Args(i).GetCodeText(_scripter, IsLuaCode)
        Next



        rstr = rstr & ")"
        If firstCode Then
            rstr = rstr & "?>"
        End If




        Return ispace & rstr
    End Function



    <NonSerialized>
    Public _LoadedArgString As List(Of String)
    Public ReadOnly Property LoadedArgString(Index As Integer) As String
        Get
            If Index >= _LoadedArgString.Count Then
                Return "NULL"
            End If


            Return _LoadedArgString(Index)
        End Get
    End Property



    Public Sub LoadArgText(_scripter As ScriptEditor)
        'ArgText를 미리 불러온다.
        If _LoadedArgString Is Nothing Then
            _LoadedArgString = New List(Of String)
        End If

        _LoadedArgString.Clear()

        Dim tfun As TriggerFunction = GetCodeFunction(_scripter)
        If tfun IsNot Nothing Then
            If tfun.Args.Count <> Args.Count Then
                '함수 인자 횟수 조절
                If tfun.Args.Count < Args.Count Then
                    '인자가 더 많음
                    For i = tfun.Args.Count To Args.Count - 1
                        Args.RemoveAt(Args.Count - 1)
                    Next
                Else
                    '인자가 더 적음
                    For i = Args.Count To tfun.Args.Count - 1
                        Dim tArg As New ArgValue(tfun.Args(i).AType)



                        Args.Add(tArg)
                    Next
                End If
            End If



            For i = 0 To Args.Count - 1
                If Args(i).IsInit Then
                    '초기값일 경우 타입과 이름을 가져온다.
                    Dim v As String = tfun.Args(i).AName

                    _LoadedArgString.Add(v)
                Else
                    Dim v As String = Args(i).GetEditorText

                    'If Args(i).IsArgNumber Then
                    '    v = Args(i).ValueNumber & "숫자"
                    'Else
                    '    v = Args(i).ValueString & "텍스트"
                    'End If

                    _LoadedArgString.Add(v)
                End If
            Next
        Else
            For i = 0 To Args.Count - 1
                If Args(i).IsInit Then
                    '초기값일 경우 타입과 이름을 가져온다.
                    Dim v As String = Args(i).ValueType

                    _LoadedArgString.Add(v)
                Else
                    Dim v As String = Args(i).GetEditorText

                    'If Args(i).IsArgNumber Then
                    '    v = Args(i).ValueNumber & "숫자"
                    'Else
                    '    v = Args(i).ValueString & "텍스트"
                    'End If

                    _LoadedArgString.Add(v)
                End If
            Next
        End If

    End Sub






    <NonSerialized>
    Private IsLockedFunction As Boolean = False

    <NonSerialized>
    Private LockedFunction As TriggerFunction


    Public Function GetCodeFunction(_scripter As ScriptEditor) As TriggerFunction
        If Not IsLockedFunction Then
            For Each item As TriggerFunction In tmanager.GetTriggerList(FType, _scripter)
                If item.FName = FName Then
                    If item.FType = TriggerFunction.EFType.Action Or item.FType = TriggerFunction.EFType.Condition Or
                        item.FType = TriggerFunction.EFType.Lua Or item.FType = TriggerFunction.EFType.Plib Then
                        IsLockedFunction = True
                        LockedFunction = item
                    Else
                        IsLockedFunction = False
                    End If

                    Return item
                End If
            Next
        Else
            Return LockedFunction
        End If



        Return Nothing
    End Function


    Public FType As TriggerFunction.EFType
    Public FName As String
    Public FGroup As String

    '값도 들어있어야됨.




    '유저 함수나 외부함수일 경우
    '참조할 수 있는 이름만 기록.
    '그 이름을 통해 찾아가기


    Public Sub New()
        Args = New List(Of ArgValue)
    End Sub



    Public Sub SetFunction(tfun As TriggerFunction)
        IsLockedFunction = False

        '함수를 지정
        FName = tfun.FName
        FType = tfun.FType
        FGroup = tfun.FGruop
    End Sub

    Public Sub Refresh(_scripter As ScriptEditor)
        'GetCodeFunction
        '코드를 완전 초기화
        If Args Is Nothing Then
            Args = New List(Of ArgValue)
        End If

        Args.Clear()

        Dim tfun As TriggerFunction = GetCodeFunction(_scripter)

        If tfun IsNot Nothing Then
            For i = 0 To tfun.Args.Count - 1



                Dim tArg As New ArgValue(tfun.Args(i).AType)


                Args.Add(tArg)
            Next
        End If

    End Sub

    Public Args As List(Of ArgValue)



    Public Sub CopyTo(toTrg As TriggerCodeBlock)
        '해당 트리거의 내용을 toTrg에 넣는다.
        IsLockedFunction = False

        toTrg.FName = Me.FName
        toTrg.FType = Me.FType
        toTrg.FGroup = Me.FGroup
        toTrg.IsLockedFunction = False


        toTrg.Args.Clear()

        For i = 0 To Me.Args.Count - 1
            toTrg.Args.Add(Args(i).DeepCopy)

            'Args(i).CopyTo(toTrg.Args(i))
        Next
    End Sub


    Public Function DeepCopy() As TriggerCodeBlock
        Dim stream As MemoryStream = New MemoryStream()


        Dim formatter As BinaryFormatter = New BinaryFormatter()

        formatter.Serialize(stream, Me)
        stream.Position = 0

        Dim rScriptBlock As TriggerCodeBlock = formatter.Deserialize(stream)


        Return rScriptBlock
    End Function
End Class
