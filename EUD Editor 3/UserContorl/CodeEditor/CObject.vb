Public Class CObject
    'Object Asunaprevir {
    '   var x :  EUDArray, y;
    '   Function safhfh() {
    '      RemoveUnit('(any unit)', AllPlayers);
    '      return 1 + (1 == 1) ? 2 : 3;
    '   }
    '};
    Private _ObjName As String 'ex Asunaprevir
    Public ReadOnly Property ObjName As String
        Get
            Return _ObjName
        End Get
    End Property



    Private _Functions As CFunc
    Public ReadOnly Property Functions As CFunc
        Get
            Return _Functions
        End Get
    End Property



    'LoadFunc를 통해 CObject의 내용물을 넣는다!


    Public Sub New(tObjName As String, InitStr As String)
        _ObjName = tObjName

        _Functions = New CFunc()
        _Functions.LoadFunc(InitStr)
    End Sub
End Class
