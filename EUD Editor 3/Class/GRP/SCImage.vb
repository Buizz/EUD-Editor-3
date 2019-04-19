Public Class SCImage
    Private GRPBOX As GRPBox

    '스타와 동일하게 작동하는 이미지.
    Private grpID As Integer
    Private DeleteFlag As Boolean = False
    Public ReadOnly Property IsDelete As Boolean
        Get
            Return DeleteFlag
        End Get
    End Property
    Private currentHeader As UInt16
    Private currentScriptID As Integer
    Private currentAnimHeaderID As Integer
    Private currentFrame As Integer

    Public Property direction As Integer

    Private turnStatus As Boolean

    Public WaitTimer As Integer


    Public Property ControlStatus As Integer


    Private Grp As GRP

    Private pos As Point
    Public Property Location As Point
        Get
            Return pos
        End Get
        Set(value As Point)
            pos = value
        End Set
    End Property


    Public Function IsTrunable() As Boolean
        Return pjData.Dat.Data(SCDatFiles.DatFiles.images, "Gfx Turns", grpID)
    End Function


    Public Sub New(tgrpID As Integer, tImageScript As Integer, tGRPBOX As GRPBox, AnimHeaderIndex As Integer)
        grpID = tgrpID
        Grp = scData.GetGrp(tgrpID)
        currentScriptID = tImageScript
        GRPBOX = tGRPBOX

        Dim animheader As UInt16
        Try
            animheader = scData.IscriptData.iscriptEntry(scData.IscriptData.key(tImageScript)).AnimHeader(AnimHeaderIndex)
        Catch ex As Exception
            animheader = 0
        End Try
        'MsgBox(animheader & ", " & tImageScript)
        currentHeader = animheader

    End Sub
    Public Sub New(tgrpID As Integer, tImageScript As Integer, tpos As Point, tGRPBOX As GRPBox)
        grpID = tgrpID
        Grp = scData.GetGrp(tgrpID)
        currentScriptID = tImageScript
        GRPBOX = tGRPBOX
        pos = tpos
        Dim animheader As UInt16
        Try
            animheader = scData.IscriptData.iscriptEntry(scData.IscriptData.key(tImageScript)).AnimHeader(0)
        Catch ex As Exception
            animheader = 0
        End Try
        currentHeader = animheader

    End Sub



    Public Sub Exec()
        If Not DeleteFlag Then
            If WaitTimer = 0 Then
                DeleteFlag = Not scData.IscriptData.playScript(currentFrame, currentAnimHeaderID, currentScriptID, currentHeader, pos, WaitTimer, direction, GRPBOX, ControlStatus)
            Else
                WaitTimer -= 1
            End If
        End If


        'currentFrame += 1
    End Sub


    Public ReadOnly Property GetGRP() As GRP
        Get
            Return Grp
        End Get
    End Property

    Public ReadOnly Property GetFrameGRP() As Integer
        Get
            Dim gfxturn As Boolean = pjData.Dat.Data(SCDatFiles.DatFiles.images, "Gfx Turns", grpID)
            Dim ReturnFrame As Integer
            If ControlStatus = 0 Then
                If gfxturn = True Then
                    If direction > 16 Then
                        ReturnFrame = currentFrame + 33 - direction

                        turnStatus = True
                    Else
                        ReturnFrame = currentFrame + direction

                        turnStatus = False
                    End If
                Else
                    ReturnFrame = currentFrame
                    turnStatus = False
                End If
            Else
                Return currentFrame
            End If

            Return ReturnFrame
        End Get
    End Property
End Class
