Public Class UsedCodeList
    Private Datfile As SCDatFiles.DatFiles
    Private ObjectID As Integer


    'Dat종류, 오브젝트 아이디만 있으면 됨

    'Dat종류에 따른 출처들을 미리 정리해두자
    'ex Dat종류 Weapon의 경우, 모든 유닛들의 UnitDat_GroundWeapon을 조사하여 오브젝트 아이디가 같으면 해당 아이디를 보유한 유닛을 추가.

    '이 관계를 이용하여 연결된 오브젝트의 이름도 변경해주면 좋겠음
    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer)
        Datfile = _DatFile
        ObjectID = _ObjectID


        Me.DataContext = pjData.BindingManager.CodeConnecter(Datfile, ObjectID)
    End Sub


    Public Sub ReLoad(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer)

    End Sub
End Class
