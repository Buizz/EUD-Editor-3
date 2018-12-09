Public Class NameBar
    Private ObjectID As Integer
    Private DatFile As SCDatFiles.DatFiles

    Public Sub Init(tObjectID As Integer, tDatFile As SCDatFiles.DatFiles)
        ObjectID = tObjectID
        DatFile = tDatFile

        'ObjName.Content = pjData.UnitName(ObjectID)
        'ObjToolTip.Text = pjData.Dat.ToolTip(DatFile, ObjectID)
        'ObjGroup.Text = pjData.Dat.Group(DatFile, ObjectID)

        ObjName.DataContext = pjData.UnitName(ObjectID)
        ObjToolTip.DataContext = pjData.Dat.ToolTip(DatFile, ObjectID)
        ObjGroup.DataContext = pjData.Dat.Group(DatFile, ObjectID)
    End Sub

    Private Sub ObjToolTip_TextChanged(sender As Object, e As TextChangedEventArgs)
        pjData.Dat.ToolTip(DatFile, ObjectID) = ObjToolTip.Text
    End Sub

    Private Sub ObjGroup_TextChanged(sender As Object, e As TextChangedEventArgs)
        pjData.Dat.Group(DatFile, ObjectID) = ObjGroup.Text
    End Sub
End Class
