Imports System.Text.RegularExpressions
Imports net.r_eg.Conari.Types

Partial Public Class MacroManager
    Private _IsBuild As Boolean = False
    Public Property IsBulid As Boolean
        Get
            Return _IsBuild
        End Get
        Set(value As Boolean)
            If value = True Then
                If _IsBuild = False Then
                    BuildStart()
                    _IsBuild = True
                End If
            Else
                If _IsBuild = True Then
                    BuildEnd()
                    _IsBuild = False
                End If
            End If
        End Set
    End Property

    Public macroErrorList As New List(Of String)

    Public SCAScriptVariables As New List(Of String)


    Private Sub BuildStart()
        'MsgBox("빌드 시작")
        MSQCItems.Clear()
        ChatEventItems.Clear()

        macroErrorList.Clear()
        SCAScriptVariables.Clear()

        mainPreDefineStr.Clear()
        onpluginStr.Clear()
        beforeStr.Clear()
        afterStr.Clear()
    End Sub
    Private Sub BuildEnd()
        'MsgBox("빌드 끝")
        'AddMSQCPlugin("NotTyping ; KeyDown("..Key.. ") : EUDArray, 1")
    End Sub


    Public Function GetMSQCCode() As String
        Dim rstr As String = ""
        For i = 0 To MSQCItems.Keys.Count - 1
            Dim tstr As String = ""

            Dim key As String = MSQCItems.Keys(i)
            Dim v() As String = MSQCItems(key).Split(",")

            Dim cond As String = v.First
            Dim vname As String = v.Last

            If cond <> "" Then
                tstr = cond & " ; "
            End If
            tstr = tstr & key & " : " & vname & ", 1"


            rstr = rstr & tstr & vbCrLf
        Next
        For i = 0 To ChatEventItems.Count - 1
            Dim chat As String = ChatEventItems(i)
            Dim IsPattern As Boolean = CheckIsPattern(chat)
            Dim vname As String = "VChatIndex" '"VChat_" & i
            If IsPattern Then
                rstr = rstr & "0x" & Hex(pjData.TEData.__patternAddr__).ToUpper & ", Exactly, " & i + 2 & " : " & vname & ", " & i + 1 & vbCrLf
            Else
                rstr = rstr & "0x" & Hex(pjData.TEData.__addr__).ToUpper & ", Exactly, " & i + 2 & " : " & vname & ", " & i + 1 & vbCrLf
            End If
        Next

        For i = 0 To MouseItems.Count - 1
            Dim vname As String = "VMouseClickIndex" & i '"VChat_" & i
            rstr = rstr & MouseItems(i) & " : " & vname & ", 1" & vbCrLf
        Next


        Return rstr
    End Function

    Private Function CheckIsPattern(str As String) As Boolean
        '^시작.*중간.*끝$
        Dim regex As New Regex("\^[^\.\*\$]*\.\*[^\.\*\$]*\.\*[^\.\*\$]*\$")
        Return regex.Match(str).Success
    End Function
    Public Function GetChatEventCode() As String
        Dim rstr As String = ""

        rstr = rstr & "__addr__ : 0x" & Hex(pjData.TEData.__addr__).ToUpper & vbCrLf
        rstr = rstr & "__ptrAddr__ : 0x" & Hex(pjData.TEData.__ptrAddr__).ToUpper & vbCrLf
        rstr = rstr & "__patternAddr__ : 0x" & Hex(pjData.TEData.__patternAddr__).ToUpper & vbCrLf
        rstr = rstr & "__lenAddr__ : 0x" & Hex(pjData.TEData.__lenAddr__).ToUpper & vbCrLf

        For i = 0 To ChatEventItems.Count - 1
            Dim chat As String = ChatEventItems(i)

            Dim IsPattern As Boolean = CheckIsPattern(chat)

            rstr = rstr & chat & " : " & i + 2 & vbCrLf
        Next
        Return rstr
    End Function


    Public Function GetpreVarEPS() As String
        Dim sb As New Text.StringBuilder

        sb.AppendLine("from eudplib import *")
        If pjData.TEData.SCArchive.IsUsed Then
            sb.AppendLine("import TriggerEditor.SCArchive as sca")
        End If

        sb.AppendLine("")
        sb.AppendLine("")



        sb.AppendLine("VChatIndex = EUDArray(8)")
        For i = 0 To MouseItems.Count - 1
            Dim vname As String = "VMouseClickIndex" & i '"VChat_" & i
            sb.AppendLine(vname & " = EUDArray(8)")
        Next
        'For i = 0 To ChatEventItems.Count - 1
        '    sb.AppendLine("VChat_" & i & " = EUDArray(8)")
        'Next
        For i = 0 To MSQCItems.Keys.Count - 1
            Dim tstr As String = ""
            Dim key As String = MSQCItems.Keys(i)
            Dim v() As String = MSQCItems(key).Split(",")
            Dim vname As String = v.Last
            sb.AppendLine(vname & " = EUDArray(8)")
        Next

        sb.AppendLine()
        sb.AppendLine("def Reg():")
        sb.AppendLine("    print('... TERegVar ...')")
        For i = 0 To MSQCItems.Keys.Count - 1
            Dim tstr As String = ""
            Dim key As String = MSQCItems.Keys(i)
            Dim v() As String = MSQCItems(key).Split(",")
            Dim vname As String = v.Last
            sb.AppendLine("    EUDRegisterObjectToNamespace(""" & vname & """, " & vname & ")")
        Next

        sb.AppendLine("    EUDRegisterObjectToNamespace(""VChatIndex"", VChatIndex)")

        For i = 0 To MouseItems.Count - 1
            Dim vname As String = "VMouseClickIndex" & i '"VChat_" & i
            sb.AppendLine("    EUDRegisterObjectToNamespace(""" & vname & """, " & vname & ")")
        Next
        If pjData.TEData.SCArchive.IsUsed Then
            sb.AppendLine("    sca.Init()")
        End If
        'For i = 0 To ChatEventItems.Count - 1
        '    Dim vname As String = "VChat_" & i
        '    sb.AppendLine("    EUDRegisterObjectToNamespace(""" & vname & """, " & vname & ")")
        'Next


        sb.AppendLine("EUDOnStart(Reg)")



        Return sb.ToString
    End Function



    Public MouseItems As New List(Of String)
    Public Function AddMouseEvent(Line As String, MinX As String, MinY As String) As String
        If IsBulid Then
            'X 0x006CDDC4
            'Y 0x006CDDC8
            '(dwread_epd(EPD(0x6CDDC8)) - 110) / 16
            Dim LineInt As Integer = Line - 1

            Dim cond As String = "MouseUp(L) ; "

            cond &= String.Format("Memory(0x6CDDC8, AtLeast, {0})", LineInt * 16 + 110) & " ; "
            cond &= String.Format("Memory(0x6CDDC8, AtMost, {0})", (LineInt + 1) * 16 + 109) & " ; "
            cond &= String.Format("Memory(0x6CDDC4, AtLeast, {0})", MinX) & " ; "
            cond &= String.Format("Memory(0x6CDDC4, AtMost, {0})", MinY)

            Dim index As Integer = MouseItems.IndexOf(cond)
            If index = -1 Then
                index = MouseItems.Count
                MouseItems.Add(cond)
            End If
            Return index
        End If
        Return 0
    End Function
    Public MSQCItems As New Dictionary(Of String, String)
    Public Sub AddMSQCPlugin(keyboard As String, vname As String, action As String, cond As String)
        If IsBulid Then
            Dim key As String = action & "(" & keyboard & ")"

            If Not MSQCItems.ContainsKey(key) Then
                MSQCItems.Add(key, cond & "," & vname)
            End If
        End If
    End Sub
    Public ChatEventItems As New List(Of String)
    Public Function AddChatEventPlugin(chat As String) As String
        If IsBulid Then
            Dim index As Integer = ChatEventItems.IndexOf(chat)
            If index = -1 Then
                index = ChatEventItems.Count
                ChatEventItems.Add(chat)
            End If
            Return index
        End If
        Return 0
    End Function
End Class
