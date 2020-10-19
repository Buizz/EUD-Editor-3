Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary


<Serializable>
Public Class TriggerCodeBlock
    Public CName As String = "TriggerCodeBlock"


    Public Function GetCodeText() As String
        'TODO:코드화 시킬때
    End Function



    <NonSerialized>
    Public LoadedArgString As List(Of String)
    Public Sub LoadArgText()
        'ArgText를 미리 불러온다.
        If LoadedArgString Is Nothing Then
            LoadedArgString = New List(Of String)
        End If

        LoadedArgString.Clear()

        Dim tfun As TriggerFunction = GetCodeFunction()
        For i = 0 To Args.Count - 1
            If Args(i).IsInit Then
                '초기값일 경우 타입과 이름을 가져온다.
                Dim v As String = tfun.Args(i).AName

                LoadedArgString.Add(v)
            Else
                Dim v As String
                'TODO:타입에 따라 출력을 다르게 한다.
                If Args(i).ValueType = "Variable" Then
                    v = "변수:" & Args(i).ValueString
                ElseIf Args(i).ValueType = "Function" Then
                    If Args(i).CodeBlock Is Nothing Then
                        v = "함수지정"
                    Else
                        v = "함수:" & Args(i).CodeBlock.FName
                    End If
                Else
                    If Args(i).IsArgNumber Then
                        v = "ID:" & Args(i).ValueNumber & " " & Args(i).ValueString
                    Else
                        If Args(i).IsLangageable Then
                            Dim lanstr As String = Tool.GetLanText("TrgArg" & Args(i).ValueString)
                            If lanstr = "TrgArg" & Args(i).ValueString Then
                                v = Args(i).ValueString
                            Else
                                v = lanstr
                            End If
                        Else
                            v = Args(i).ValueString
                        End If
                    End If
                End If




                'If Args(i).IsArgNumber Then
                '    v = Args(i).ValueNumber & "숫자"
                'Else
                '    v = Args(i).ValueString & "텍스트"
                'End If



                LoadedArgString.Add(v)
            End If
        Next
    End Sub



    <NonSerialized>
    Private IsLockedFunction As Boolean = False

    <NonSerialized>
    Private LockedFunction As TriggerFunction


    Public Function GetCodeFunction() As TriggerFunction
        If Not IsLockedFunction Then
            For Each item As TriggerFunction In tmanager.GetTriggerList(FType)
                If item.FName = FName Then
                    If item.FType = TriggerFunction.EFType.Action Or item.FType = TriggerFunction.EFType.Condition Or
                        item.FType = TriggerFunction.EFType.Lua Or item.FType = TriggerFunction.EFType.Plib Then
                        IsLockedFunction = True
                        LockedFunction = item
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
    End Sub

    Public Sub Refresh()
        'GetCodeFunction
        '코드를 완전 초기화
        If Args Is Nothing Then
            Args = New List(Of ArgValue)
        End If

        Args.Clear()

        Dim tfun As TriggerFunction = GetCodeFunction()

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


        For i = 0 To Me.Args.Count - 1
            Args(i).CopyTo(toTrg.Args(i))
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
