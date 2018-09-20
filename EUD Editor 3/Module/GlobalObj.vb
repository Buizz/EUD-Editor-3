Module GlobalObj
    Public pgData As ProgramData
    Public pjData As ProjectData
    Public scData As StarCraftData

    Public Sub InitProgram()
        pgData = New ProgramData
        scData = New StarCraftData

        '로드 테스트
        pjData = New ProjectData
    End Sub
End Module
