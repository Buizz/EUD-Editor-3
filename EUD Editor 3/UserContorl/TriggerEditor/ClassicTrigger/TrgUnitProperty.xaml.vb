Public Class TrgUnitProperty
    Public Event SelectEvent As RoutedEventHandler

    Public Sub New()
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.

        IsLoad = True
    End Sub


    Public Sub ResetValue(v As String)
        IsLoad = False
        HPDefault.IsChecked = False
        ShildDefault.IsChecked = False
        EnergyDefault.IsChecked = False
        ResourceDefault.IsChecked = False
        HangerDefault.IsChecked = False
        Clackefault.IsChecked = False
        BurrowDefault.IsChecked = False
        LiftDefault.IsChecked = False
        HallDefault.IsChecked = False
        InviDefault.IsChecked = False
        If v Is Nothing Then
            IsLoad = True
            Return
        End If

        Dim data() As String = v.Split(",")

        For i = 0 To data.Count - 1
            Dim c() As String = data(i).Split("=")
            Dim _type As String = c.First
            Dim _value As String = c.Last


            Select Case _type
                Case "hitpoint"
                    HPDefault.IsChecked = True
                    HPTextbox.Text = _value
                Case "shield"
                    ShildDefault.IsChecked = True
                    ShildTextbox.Text = _value
                Case "energy"
                    EnergyDefault.IsChecked = True
                    EnergyTextbox.Text = _value
                Case "resource"
                    ResourceDefault.IsChecked = True
                    ResourceTextbox.Text = _value
                Case "hanger"
                    HangerDefault.IsChecked = True
                    ClackeCb.IsChecked = _value
                Case "cloaked"
                    Clackefault.IsChecked = True
                    ClackeCb.IsChecked = _value
                Case "burrowed"
                    BurrowDefault.IsChecked = True
                    BurrowCb.IsChecked = _value
                Case "intransit"
                    LiftDefault.IsChecked = True
                    LiftCb.IsChecked = _value
                Case "hallucinated"
                    HallDefault.IsChecked = True
                    HallCb.IsChecked = _value
                Case "invincible"
                    InviDefault.IsChecked = True
                    InviCb.IsChecked = _value
            End Select
        Next
        IsLoad = True
    End Sub


    Private IsLoad As Boolean = False
    Public Sub RefreshValue()
        Dim dic As New Dictionary(Of String, String)

        If HPDefault.IsChecked Then
            Dim v As Integer = HPTextbox.Text
            If 0 <= v And v <= 100 Then
                dic.Add("hitpoint", v)
            End If
        End If
        If ShildDefault.IsChecked Then
            Dim v As Integer = ShildTextbox.Text
            If 0 <= v And v <= 100 Then
                dic.Add("shield", v)
            End If
        End If
        If EnergyDefault.IsChecked Then
            Dim v As Integer = EnergyTextbox.Text
            If 0 <= v And v <= 100 Then
                dic.Add("energy", v)
            End If
        End If


        If ResourceDefault.IsChecked Then
            Dim v As Integer = ResourceTextbox.Text
            If 0 <= v And v <= 100 Then
                dic.Add("resource", v)
            End If
        End If
        If HangerDefault.IsChecked Then
            Dim v As Integer = HangerTextbox.Text
            If 0 <= v And v <= 100 Then
                dic.Add("hanger", v)
            End If
        End If


        If Clackefault.IsChecked Then
            Dim v As Boolean = ClackeCb.IsChecked
            If v Then
                dic.Add("cloaked", "True")
            Else
                dic.Add("cloaked", "False")
            End If
        End If
        If BurrowDefault.IsChecked Then
            Dim v As Boolean = BurrowCb.IsChecked
            If v Then
                dic.Add("burrowed", "True")
            Else
                dic.Add("burrowed", "False")
            End If
        End If
        If LiftDefault.IsChecked Then
            Dim v As Boolean = LiftCb.IsChecked
            If v Then
                dic.Add("intransit", "True")
            Else
                dic.Add("intransit", "False")
            End If
        End If
        If HallDefault.IsChecked Then
            Dim v As Boolean = HallCb.IsChecked
            If v Then
                dic.Add("hallucinated", "True")
            Else
                dic.Add("hallucinated", "False")
            End If
        End If
        If InviDefault.IsChecked Then
            Dim v As Boolean = InviCb.IsChecked
            If v Then
                dic.Add("invincible", "True")
            Else
                dic.Add("invincible", "False")
            End If
        End If
        '* 퍼센트 조절
        '* hitpoint, shield, energy
        '* 수량 조절
        '* resource, hanger
        '* 논리형
        '* cloaked, burrowed, intransit, hallucinated, invincible
        Dim rval As String = ""
        For i = 0 To dic.Count - 1
            If i <> 0 Then
                rval = rval & ","
            End If

            rval = rval & dic.Keys(i) & "=" & dic.Values(i)
        Next


        RaiseEvent SelectEvent(rval, Nothing)
    End Sub

    Private Sub Default_Checked(sender As Object, e As RoutedEventArgs)
        If IsLoad Then
            RefreshValue()
        End If
    End Sub


    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        If IsLoad Then
            RefreshValue()
        End If
    End Sub
End Class
