Public Class SCDatFiles
    'Dat파일 정의
    Private Datfile As New List(Of CDatFile)


    Private Datfilesname() As String = {"flingy", "images", "orders", "portdata",
        "sfxdata", "sprites", "techdata", "units", "upgrades", "weapons"}

    Public Sub LoadFromDat()

    End Sub




    Private Class CDatFile
        Private FIleName As String 'ex sprites.dat
        Private Paramaters As New List(Of CParamater)




        '피라미터들
        Private Class CParamater
            Private FIleName As String 'ex sprites.dat
            Private ParamaterName As String
            Private Size As Byte
            Private VarStart As UInteger
            Private VarEnd As UInteger


            Private VarArray As UInteger
            Private VarIndex As UInteger

            Private Values As New List(Of UInteger)


            Public ReadOnly Property GetOffset() As String
                Get
                    Return scData.GetOffset(FIleName & "_" & ParamaterName)
                End Get
            End Property
        End Class
    End Class
End Class