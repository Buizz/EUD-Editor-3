Imports System.IO

Namespace IScript
    Module IscriptModule
        Public HEADERNAME() As String
        Public HEADERINFOR() As String

        Public Opcodedata As New List(Of OpcodeStructure)
        Structure OpcodeStructure
            Public name As String
            Public parmsize() As Byte
            Public Comment As String
        End Structure

        Private ReadOnly Property AnimOpcdesPath As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Texts\AnimOpcodes.txt"
            End Get
        End Property
        Private ReadOnly Property AnimHeaderPath As String
            Get
                Return System.AppDomain.CurrentDomain.BaseDirectory & "\Data\Texts\AnimHeader.txt"
            End Get
        End Property

        Private Sub LoadAnimHeader()
            Dim FileStream As New FileStream(AnimHeaderPath, FileMode.Open)
            Dim strreader As New StreamReader(FileStream)

            Dim text As String = strreader.ReadToEnd

            strreader.Close()
            FileStream.Close()

            Dim line() As String = text.Split(vbCrLf)
            Dim count As Integer = line.Count - 1

            ReDim HEADERNAME(count)
            ReDim HEADERINFOR(count)

            For i = 0 To count
                HEADERNAME(i) = line(i).Split("-")(0).Trim
                HEADERINFOR(i) = line(i).Split("-")(1).Trim
            Next
        End Sub
        Public Sub readOpcodes()
            Dim filestream As New FileStream(AnimOpcdesPath, FileMode.Open)
            Dim streamreader As New StreamReader(filestream)
            Dim temptext As String = streamreader.ReadToEnd


            Dim lines() As String = temptext.Split(vbCrLf)

            For k = 0 To lines.Count - 2
                Dim content() As String = lines(k).Split("	")
                Dim topcode As New OpcodeStructure
                With topcode
                    .name = content(1)
                    .Comment = content(4)

                    ReDim .parmsize(content(2).Split(",").Count - 2)

                    For i = 0 To .parmsize.Length - 1
                        Dim value As String = content(2).Split(",")(i).Trim

                        .parmsize(i) = value.Split(" ")(0).Replace("u", "")
                    Next
                End With

                Opcodedata.Add(topcode)
            Next



            'parcount = content(2).Split(",") - 1


            streamreader.Close()
            filestream.Close()

            LoadAnimHeader()
        End Sub

    End Module
    Public Class CIScript
        Public gfxturn As Boolean = True

        'Public x As Integer = 0
        'Public y As Integer = 0
        'Public currentHeader As UInt16
        'Public currentScriptID As Integer
        'Public currentAnimHeaderID As Integer
        'Public currentFrame As Integer

        'Private turnStatus As Boolean


        Public curretgrpMaxFrame As Integer

        '스크립트 ID에 맞는 리스트가 필요.
        'Ex Public IscriptEntry As New List(of isciptD)
        '
        '각각에 엔트리에는 코드만 들어있음.(나중을 위하여.)
        '엔트리는 클래스 이므로 해석하는 것도 다 들어있음.
        '전체 관리자에서 해당 엔트리에게 포인터 또는 자료만 넣어주면 알아서 해석하게 함.

        'iscript포맷
        '처음 2바이트는 포인터다.
        '포인터로 간 다음부터는 각각
        '2byte IscriptNum
        '2byte Pointer
        '만약 iscriptNum이 &HFFFF면 끝.


        '각 포인터로 간 다음에는 다음을 뜻한다.
        '4byte SCPE 매직 넘버.
        '4byte 애니메이션 타입
        '0:2
        '1:2
        '2:4
        '12:14
        '13:14
        '14:16
        '15:16
        '20:22
        '21:22
        '23:24
        '24:26
        '26:28
        '27:28
        '28:28
        '29:28
        '
        '나머지는 다 애니메이션 헤더
        '헤더 종류는
        'Init - Initial animation
        'Death - Death animation
        'GndAttkInit - Initial ground attack animation
        'AirAttkInit - Initial air attack animation
        'Unused1 - Unknown/unused animation
        'GndAttkRpt - Repeated ground attack animation
        'AirAttkRpt - Repeated air attack animation
        'CastSpell - Spell casting animation
        'GndAttkToIdle - Animation for returning to an idle state after a ground attack
        'AirAttkToIdle - Animation for returning to an idle state after an air attack
        'Unused2 - Unknown/unused animation
        'Walking - Walking/moving animation
        'WalkingToIdle - Animation for returning to an idle state after walking/moving
        'SpecialState1 - Some sort of category of special animations, in some cases an in-transit animation, sometimes used for special orders, sometimes having to do with the animation when something finishes morphing, or the first stage of a construction animation
        'SpecialState2 - Some sort of category of special animations, in some cases a burrowed animation, sometimes used for special orders, sometimes having to do with the animation when canceling a morph, or the second stage of a construction animation
        'AlmostBuilt - An animation for one part of the building process
        'Built - Final animation before finishing being built
        'Landing - Landing animation
        'LiftOff - Lifting off animation
        'IsWorking - Animation for when researching an upgrade/technology or training/building units and some other animations for some sort of work being done
        'WorkingToIdle - Animation for returning to an idle state after IsWorking
        'WarpIn - Warping in animation
        'Unused3 - Unknown/unused animation
        'StarEditInit - Previously called InitTurret, this is actually an alternate initial animation for StarEdit a.k.a. the Campaign Editor
        'Disable - Animation for becoming disabled, either through the "Set Doodad State" trigger action or by not being in the psi field of any pylons
        'Burrow - Burrowing animation
        'UnBurrow - Unburrowing animation
        'Enable - Animation for becoming enabled, either through the "Set Doodad State" trigger action or by being in the psi field of a pylon



        '해당 헤더로 이동 하면 OPCode가 나옴. OPCode는 다음을 따름.
        'playfram          0x00 - u16<frame#> - displays a particular frame, adjusted for direction.
        'playframtile      0x01 - u16<frame#> - displays a particular frame dependent on tileset.
        'sethorpos         0x02 - u8<x> - sets the current horizontal offset of the current image overlay.
        'setvertpos        0x03 - u8<y> - sets the vertical position of an image overlay.
        'setpos            0x04 - u8<x> u8<y> - sets the current horizontal and vertical position of the current image overlay.
        'wait              0x05 - u8<#ticks> - pauses script execution for a specific number of ticks.
        'waitrand          0x06 - u8<#ticks1> u8<#ticks2> - pauses script execution for a random number of ticks given two possible wait times. 
        'goto              0x07 - u16<labelname> - unconditionally jumps to a specific code block.
        'imgol             0x08 - u16<image#> u8<x> u8<y> - displays an active image overlay at an animation level higher than the current image overlay at a specified offset position.
        'imgul             0x09 - u16<image#> u8<x> u8<y> - displays an active image overlay at an animation level lower than the current image overlay at a specified offset position.
        'imgolorig         0x0a - u16<image#> - displays an active image overlay at an animation level higher than the current image overlay at the relative origin offset position.
        'switchul          0x0b - <image#> - only for powerups. Hypothesised to replace the image overlay that was first created by the current image overlay.
        '__0c              0x0c - no parameters - unknown.
        'imgoluselo        0x0d - <image#> <x> <y> - displays an active image overlay at an animation level higher than the current image overlay, using a LO* file to determine the offset position.
        'imguluselo        0x0e - <image#> <x> <y> - displays an active image overlay at an animation level lower than the current image overlay, using a LO* file to determine the offset position.
        'sprol             0x0f - <sprite#> <x> <y> - spawns a sprite one animation level above the current image overlay at a specific offset position.
        'highsprol         0x10 - <sprite#> <x> <y> - spawns a sprite at the highest animation level at a specific offset position.
        'lowsprul          0x11 - <sprite#> <x> <y> - spawns a sprite at the lowest animation level at a specific offset position.
        'uflunstable       0x12 - <flingy# - creates an flingy with restrictions; supposedly crashes in most cases.
        'spruluselo        0x13 - <sprite#> <x> <y> - spawns a sprite one animation level below the current image overlay at a specific offset position. The new sprite inherits the direction of the current sprite. Requires LO* file for unknown reason.
        'sprul             0x14 - <sprite#> <x> <y> - spawns a sprite one animation level below the current image overlay at a specific offset position. The new sprite inherits the direction of the current sprite.
        'sproluselo        0x15 - <sprite#> <overlay#> - spawns a sprite one animation level above the current image overlay, using a specified LO* file for the offset position information. The new sprite inherits the direction of the current sprite.
        'end               0x16 - no parameters - destroys the current active image overlay, also removing the current sprite if the image overlay is the last in one in the current sprite.
        'setflipstate      0x17 - <flipstate> - sets flip state of the current image overlay.
        'playsnd           0x18 - <sound#> - plays a sound.
        'playsndrand       0x19 - <#sounds> <sound#> <...> - plays a random sound from a list.
        'playsndbtwn       0x1a - <firstsound#> <lastsound#> - plays a random sound between two inclusive sfxdata.dat entry IDs.
        'domissiledmg      0x1b - no parameters - causes the damage of a weapon flingy to be applied according to its weapons.dat entry.
        'attackmelee       0x1c - <#sounds> <sound#> <...> - applies damage to target without creating a flingy and plays a sound.
        'followmaingraphic 0x1d - no parameters - causes the current image overlay to display the same frame as the parent image overlay.
        'randcondjmp       0x1e - <randchance#> <labelname> - random jump, chance of performing jump depends on the parameter.
        'turnccwise        0x1f - <turnamount> - turns the flingy counterclockwise by a particular amount.
        'turncwise         0x20 - <turnamount> - turns the flingy clockwise by a particular amount.
        'turn1cwise        0x21 - no parameters - turns the flingy clockwise by one direction unit.
        'turnrand          0x22 - <turnamount> - turns the flingy a specified amount in a random direction, with a heavy bias towards turning clockwise.
        'setspawnframe     0x23 - <direction> - in specific situations, performs a natural rotation to the given direction.
        'sigorder          0x24 - <signal#> - allows the current unit's order to proceed if it has paused for an animation to be completed.
        'attackwith        0x25 - <ground = 1, air = 2> - attack with either the ground or air weapon depending on a parameter.
        'attack            0x26 - no parameters - attack with either the ground or air weapon depending on target.
        'castspell         0x27 - no parameters - identifies when a spell should be cast in a spellcasting animation. The spell is determined by the unit's current order.
        'useweapon         0x28 - <weapon#> - makes the unit use a specific weapons.dat ID on its target.
        'move              0x29 - <movedistance> - sets the unit to move forward a certain number of pixels at the end of the current tick.
        'gotorepeatattk    0x2a - no parameters - signals to StarCraft that after this point, when the unit's cooldown time is over, the repeat attack animation can be called.
        'engframe          0x2b - <frame#> - plays a particular frame, often used in engine glow animations.
        'engset            0x2c - <frameset#> - plays a particular frame set, often used in engine glow animations.
        '__2d              0x2d - no parameters - hypothesised to hide the current image overlay until the next animation.
        'nobrkcodestart    0x2e - no parameters - holds the processing of player orders until a nobrkcodeend is encountered.
        'nobrkcodeend      0x2f - no parameters - allows the processing of player orders after a nobrkcodestart instruction.
        'ignorerest        0x30 - no parameters - conceptually, this causes the script to stop until the next animation is called.
        'attkshiftproj     0x31 - <distance> - creates the weapon flingy at a particular distance in front of the unit.
        'tmprmgraphicstart 0x32 - no parameters - sets the current image overlay state to hidden.
        'tmprmgraphicend   0x33 - no parameters - sets the current image overlay state to visible.
        'setfldirect       0x34 - <direction> - sets the current direction of the flingy.
        'call              0x35 - <labelname> - calls a code block.
        'return            0x36 - no parameters - returns from call.
        'setflspeed        0x37 - <speed> - sets the flingy.dat speed of the current flingy.
        'creategasoverlays 0x38 - <gasoverlay#> - creates gas image overlays at offsets specified by LO* files.
        'pwrupcondjmp      0x39 - <labelname> - jumps to a code block if the current unit is a powerup and it is currently picked up.
        'trgtrangecondjmp  0x3a - <distance> <labelname> - jumps to a block depending on the distance to the target.
        'trgtarccondjmp    0x3b - <angle1> <angle2> <labelname> - jumps to a block depending on the current angle of the target.
        'curdirectcondjmp  0x3c - <angle1> <angle2> <labelname> - only for units. Jump to a code block if the current sprite is facing a particular direction.
        'imgulnextid       0x3d - <x> <y> - displays an active image overlay at the shadow animation level at a specified offset position. The image overlay that will be displayed is the one that is after the current image overlay in images.dat.
        '__3e              0x3e - no parameters - unknown.
        'liftoffcondjmp    0x3f - <labelname> - jumps to a code block when the current unit that is a building that is lifted off.
        'warpoverlay       0x40 - <frame#> - hypothesised to display the current image overlay's frame clipped to the outline of the parent image overlay.
        'orderdone         0x41 - <signal#> - most likely used with orders that continually repeat, like the Medic's healing and the Valkyrie's afterburners (which no longer exist), to clear the sigorder flag to stop the order.
        'grdsprol          0x42 - <sprite#> <x> <y> - spawns a sprite one animation level above the current image overlay at a specific offset position, but only if the current sprite is over ground-passable terrain.
        '__43              0x43 - no parameters - unknown.
        'dogrddamage       0x44 - no parameters - applies damage like domissiledmg when on ground-unit-passable terrain.

        Public buffer() As Byte
        Public key As New Dictionary(Of Integer, Integer)




        Public iscriptEntry As New List(Of CIscriptEntry)
        Class CIscriptEntry
            Private ReadOnly ANIMTYPE() As Byte = {2, 2, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0,
             14, 14, 16, 16, 0, 0, 0, 0, 22, 22, 0, 24, 26, 0, 28, 28, 28, 28}

            Public Parent As CIScript

            Public IScriptID As Integer
            Public headeroffset As UInt16

            Public Function EntryType() As UInt32
                Dim memsteram As New MemoryStream(Parent.buffer)
                Dim bytereader As New BinaryReader(memsteram)


                memsteram.Position = headeroffset + 4


                Dim EntryTypen As UInt32 = bytereader.ReadUInt32
                EntryTypen = ANIMTYPE(EntryTypen)



                bytereader.Close()
                memsteram.Close()
                Return EntryTypen
            End Function
            Public Function AnimHeader(num As Integer) As UInt16
                Dim memsteram As New MemoryStream(Parent.buffer)
                Dim bytereader As New BinaryReader(memsteram)
                Dim tanimheader As UInt16

                memsteram.Position = headeroffset + 8 + (2 * num)

                tanimheader = bytereader.ReadUInt16()

                bytereader.Close()
                memsteram.Close()
                Return tanimheader
            End Function


        End Class



        Public Sub Reset()
            ReDim buffer(0)
            iscriptEntry.Clear()
        End Sub
        Public Sub LoadIscriptToBuff(buff() As Byte)
            Reset()

            buffer = buff

            LoadIscript()
        End Sub
        Public Sub LoadIscriptToFile(filename As String, Optional xscript As Boolean = False)
            Reset()
            Dim fs As New FileStream(filename, FileMode.Open)
            Dim br As New BinaryReader(fs)
            buffer = br.ReadBytes(fs.Length)
            br.Close()
            fs.Close()

            LoadIscript(xscript)
        End Sub

        Public Sub LoadIscript(Optional xscript As Boolean = False) '모든 스크립트를 읽는다.
            Dim memsteram As New MemoryStream(buffer)
            Dim bytereader As New BinaryReader(memsteram)

            Dim temp As Integer

            key.Clear()
            iscriptEntry.Clear()
            If Not xscript Then
                temp = bytereader.ReadUInt16() '헤더s 오프셋
                memsteram.Position = temp
            End If


            While True
                Dim id, headeroffset As UInt16
                id = bytereader.ReadUInt16() '스크립트아이디
                headeroffset = bytereader.ReadUInt16() '헤더 오프셋

                If id <> &HFFFF Then
                    Dim tempCIscriptEntry As New CIscriptEntry
                    tempCIscriptEntry.Parent = Me

                    tempCIscriptEntry.IScriptID = id
                    tempCIscriptEntry.headeroffset = headeroffset


                    key.Add(id, iscriptEntry.Count)
                    iscriptEntry.Add(tempCIscriptEntry)
                Else
                    Exit While
                End If
            End While


            bytereader.Close()
            memsteram.Close()
        End Sub

        'Public x As Integer = 0
        'Public y As Integer = 0
        'Public currentHeader As UInt16
        'Public currentScriptID As Integer
        'Public currentAnimHeaderID As Integer
        'Public currentFrame As Integer
        Public Function playScript(ByRef currentFrame As Integer, ByRef currentAnimHeaderID As Integer, currentScriptID As Integer, ByRef currentHeader As UInt16, ByRef pos As Point, ByRef WaitTimer As Integer, ByRef direction As Integer, GRPBOX As GRPBox, ByRef ControlStatus As Integer) As Boolean
            Try

                If currentHeader <> 0 Then
                    'DatEditForm.TextBox123.Text = currentHeader
                    Dim memsteram As New MemoryStream(buffer)
                    Dim bytereader As New BinaryReader(memsteram)


                    memsteram.Position = currentHeader


                    Dim opcode As Byte
                    opcode = bytereader.ReadByte() 'Opcode
                    'MsgBox(Opcodedata(opcode).name)
                    Dim values As New List(Of UInt32)

                    If (opcode = &H19) Or (opcode = &H1C) Then
                        Dim valuecount As Integer = bytereader.ReadByte()
                        For i = 0 To valuecount - 1
                            values.Add(bytereader.ReadUInt16())
                        Next
                    Else
                        For k = 0 To Opcodedata(opcode).parmsize.Length - 1
                            Select Case Opcodedata(opcode).parmsize(k)
                                Case 8
                                    values.Add(bytereader.ReadByte())
                                Case 16
                                    values.Add(bytereader.ReadUInt16())
                                Case 32
                                    values.Add(bytereader.ReadUInt32())
                            End Select
                        Next
                    End If

                    currentHeader = memsteram.Position



                    'd, e, 13, 15
                    Select Case opcode
                        Case &H0 'playfram
                            currentFrame = values(0)
                        Case &H2 'sethorpos
                            pos.X = values(0)
                        Case &H3 'setvertpos
                            pos.Y = values(0)
                        Case &H4 'setpos
                            pos.X = values(0)
                            pos.Y = values(1)
                        Case &H5 'wait
                            WaitTimer = values(0)
                        Case &H6 'waitrand
                            Dim random As New Random

                            Dim selectv As Integer = random.Next(values(0), values(1))


                            WaitTimer = selectv
                        Case &H7 'goto
                            currentHeader = values(0)


                        Case &H8 'imgol
                            'u16<image#> u8<x> u8<y> - displays an active image overlay at an animation level higher than the current image overlay at a specified offset position.
                            GRPBOX.CreateImage(values(0), values(1), values(2))

                        Case &H9 'imgul
                            'u16<image#> u8<x> u8<y> - displays an active image overlay at an animation level lower than the current image overlay at a specified offset position.
                            GRPBOX.CreateImage(values(0), values(1), values(2), opcode)
                        Case &HA 'imgolorig
                            'u16<image#> - displays an active image overlay at an animation level higher than the current image overlay at the relative origin offset position.
                            GRPBOX.CreateImage(values(0), pos.X, pos.Y)
                        Case &HB 'switchul
                            '<image#> - only for powerups. Hypothesised to replace the image overlay that was first created by the current image overlay.
                        Case &HD 'imgoluselo 
'<image#> <x> <y> - displays an active image overlay at an animation level higher than the current image overlay, using a LO* file to determine the offset position.
                            'If DatEditDATA(DTYPE.images).ReadValue("Attack Overlay", DatEditForm._OBJECTNUM) = 0 And
                            '    DatEditDATA(DTYPE.images).ReadValue("Special Overlay", DatEditForm._OBJECTNUM) = 0 Then

                            '    DatEditForm.IScriptPlayer.Enabled = False
                            '    DatEditForm.ListBox9.SelectedIndex = -1

                            '    Dim bitmap As New Bitmap(256, 256)
                            '    Dim grptool As Graphics
                            '    grptool = Graphics.FromImage(bitmap)
                            '    grptool.DrawString("스크립트에서 Lo파일을 요구하였으나 " & vbCrLf & "존재하지 않습니다.", DatEditForm.Font, Brushes.Red, New Point(0, 96))

                            '    DatEditForm.PictureBox26.Image = bitmap
                            'End If
                        Case &HE  'imguluselo
'<image#> <x> <y> - displays an active image overlay at an animation level lower than the current image overlay, using a LO* file to determine the offset position.
                            'If DatEditDATA(DTYPE.images).ReadValue("Attack Overlay", DatEditForm._OBJECTNUM) = 0 And
                            '    DatEditDATA(DTYPE.images).ReadValue("Special Overlay", DatEditForm._OBJECTNUM) = 0 Then

                            '    DatEditForm.IScriptPlayer.Enabled = False
                            '    DatEditForm.ListBox9.SelectedIndex = -1

                            '    Dim bitmap As New Bitmap(256, 256)
                            '    Dim grptool As Graphics
                            '    grptool = Graphics.FromImage(bitmap)
                            '    grptool.DrawString("스크립트에서 Lo파일을 요구하였으나 " & vbCrLf & "존재하지 않습니다.", DatEditForm.Font, Brushes.Red, New Point(0, 96))

                            '    DatEditForm.PictureBox26.Image = bitmap
                            'End If
                        Case &HF  'sprol
                        Case &H10 'highsprol
                        Case &H11 'lowsprul   
                        Case &H12 'uflunstable 
                        Case &H13 'spruluselo 
                        Case &H14 'sprul 
                        Case &H15 'sproluselo


                        Case &H15
                            'If DatEditDATA(DTYPE.images).ReadValue("Attack Overlay", DatEditForm._OBJECTNUM) = 0 And
                    '    DatEditDATA(DTYPE.images).ReadValue("Special Overlay", DatEditForm._OBJECTNUM) = 0 Then

                    '    DatEditForm.IScriptPlayer.Enabled = False
                    '    DatEditForm.ListBox9.SelectedIndex = -1

                    '    Dim bitmap As New Bitmap(256, 256)
                    '    Dim grptool As Graphics
                    '    grptool = Graphics.FromImage(bitmap)
                    '    grptool.DrawString("스크립트에서 Lo파일을 요구하였으나 " & vbCrLf & "존재하지 않습니다.", DatEditForm.Font, Brushes.Red, New Point(0, 96))

                    '    DatEditForm.PictureBox26.Image = bitmap
                     'End If


                        Case &H16 'end
                            currentHeader = 0
                            Return False
                        Case &H18
                            Tool.PlaySoundFromMPQIndex(values(0))
                        Case &H19
                            Dim random As New Random

                            Dim max As Integer = values.Count
                            Dim selectv As Integer = random.Next(0, max)

                            Tool.PlaySoundFromMPQIndex(values(selectv))
                        Case &H1A
                            Dim random As New Random

                            Dim selectv As Integer = random.Next(values(0), values(1))

                            Tool.PlaySoundFromMPQIndex(selectv)
                        Case &H1C
                            Dim random As New Random

                            Dim max As Integer = values.Count
                            Dim selectv As Integer = random.Next(0, max)

                            If values(selectv) <> 0 Then
                                Tool.PlaySoundFromMPQIndex(values(selectv))
                            End If
                        Case &H1D
                            'DatEditForm.drawImageGRP(GetFrameNum, turnStatus, x, y)
                        Case &H1E
                            Dim random As New Random

                            Dim selectv As Integer = random.Next(0, 255)


                            If selectv <= values(0) Then
                                currentHeader = values(1)
                            End If

                        Case &H1F 'turnccwise
                            '0~33이다.
                            '12인데 15를 빼면

                            Dim value As Integer = direction - values(0)

                            If value < 0 Then
                                direction = 33 + value
                            Else
                                direction = value
                            End If

                        Case &H20
                            Dim value As Integer = direction + values(0)

                            If value > 33 Then
                                direction = value - 33
                            Else
                                direction = value
                            End If
                        Case &H21
                            Dim value As Integer = direction + 1

                            If value > 33 Then
                                direction = value - 33
                            Else
                                direction = value
                            End If
                        Case &H22
                            Dim random As New Random

                            Dim selectv As Integer = random.Next(0, 255)
                            selectv = selectv Mod 2

                            Dim value As Integer
                            If selectv = 0 Then
                                value = direction + values(0)
                            Else
                                value = direction - values(0)
                            End If


                            If value > 33 Then
                                direction = value - 33
                            ElseIf value < 0 Then
                                direction = 33 + value
                            Else
                                direction = value
                            End If
                        Case &H2A
                            ''16590
                            If currentAnimHeaderID = 5 Then

                                WaitTimer = 8
                                currentHeader = iscriptEntry(key(currentScriptID)).AnimHeader(5)
                            End If
                            If currentAnimHeaderID = 6 Then

                                WaitTimer = 8
                                currentHeader = iscriptEntry(key(currentScriptID)).AnimHeader(6)
                            End If
                        Case &H2B
                            currentFrame += 17
                            If curretgrpMaxFrame <= currentFrame Then
                                currentFrame = 0
                            End If
                        Case &H2C
                            currentFrame += values(0) * 17
                            If curretgrpMaxFrame <= currentFrame Then
                                currentFrame = 0
                            End If

                        Case &H2E 'no parameters - holds the processing of player orders until a nobrkcodeend is encountered.
                            ControlStatus = 1
                        Case &H2F 'no parameters - allows the processing of player orders after a nobrkcodestart instruction.
                            ControlStatus = 0
                        Case &H34
                            gfxturn = False

                        Case &H35
                            currentHeader = values(0)
                        Case &H36
                            currentHeader = iscriptEntry(key(currentScriptID)).AnimHeader(currentAnimHeaderID)
                        Case &H40
                            currentFrame = values(0)
                    End Select





                    bytereader.Close()
                    memsteram.Close()


                End If

            Catch ex As Exception
                'currentHeader = 0
                'DatEditForm.ListBox9.Items.Clear()
            End Try

            Return True
        End Function


    End Class
End Namespace