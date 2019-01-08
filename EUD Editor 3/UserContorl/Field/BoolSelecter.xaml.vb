Public Class BoolSelecter
    Private DatFile As SCDatFiles.DatFiles
    Private ObjectID As Integer
    Private Parameter As String

    Private DatCommand As DatCommand

    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        Field.Init(DatFile, ObjectID, Parameter)

        chbox.DataContext = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)
    End Sub


End Class
