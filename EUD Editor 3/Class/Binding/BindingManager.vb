Public Class BindingManager
    Private _UIManager() As List(Of UIManager)
    Public ReadOnly Property UIManager(key As SCDatFiles.DatFiles, index As Integer) As UIManager
        Get
            Return _UIManager(key)(index)
        End Get
    End Property

    Public Sub RefreshCodeTree(key As SCDatFiles.DatFiles, ObjectID As String)
        For i = 0 To pjData.CodeSelecters.Count - 1
            pjData.CodeSelecters(i).RefreshTreeviewItem(key, ObjectID)
        Next
    End Sub

    Public Sub New()
        'NullBinding = New DatBinding()

        ReDim _UIManager(7)
        For k = 0 To 7
            _UIManager(k) = New List(Of UIManager)
            For i = 0 To SCCodeCount(k) - 1
                _UIManager(k).Add(New UIManager(k, i))
            Next
        Next

        ReDim DataBindings(scData.DefaultDat.DatFileList.Count)
        ReDim DataParamKeys(scData.DefaultDat.DatFileList.Count)
        For k = 0 To scData.DefaultDat.DatFileList.Count - 1
            DataBindings(k) = New List(Of List(Of DatBinding))
            DataParamKeys(k) = New Dictionary(Of String, Integer)

            For i = 0 To scData.DefaultDat.DatFileList(k).ParamaterList.Count - 1
                Dim keyName As String = scData.DefaultDat.DatFileList(k).ParamaterList(i).GetParamname

                DataParamKeys(k).Add(keyName, i)
                DataBindings(k).Add(New List(Of DatBinding))
                For j = 0 To scData.DefaultDat.DatFileList(k).ParamaterList(i).GetValueCount - 1
                    DataBindings(k)(i).Add(New DatBinding(k, keyName, j))
                Next
            Next
        Next
    End Sub

    Public Sub DataRefresh()
        For datindex = 0 To 6
            For i = 0 To pjData.Dat.GetDatFile(datindex).ParamaterList.Count - 1
                Dim Paramname As String = pjData.Dat.GetDatFile(datindex).ParamaterList(i).GetParamname



                Dim ValueCount As Integer = pjData.Dat.GetDatFile(datindex).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarCount)
                Dim ValueStart As Integer = pjData.Dat.GetDatFile(datindex).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarStart)
                Dim ValueEnd As Integer = pjData.Dat.GetDatFile(datindex).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.VarEnd)

                For j = ValueStart To ValueEnd
                    pjData.BindingManager.DatBinding(datindex, Paramname, j).BackColorRefresh()
                Next
            Next

            For k = 0 To SCCodeCount(datindex) - 1
                _UIManager(datindex)(k).BackColorRefresh()
            Next
        Next
    End Sub
    Public ReadOnly Property DatBinding(key As SCDatFiles.DatFiles, name As String, index As Integer) As DatBinding
        Get
            Dim ValueStart As Integer = pjData.Dat.ParamInfo(key, name, SCDatFiles.EParamInfo.VarStart)
            Dim RealIndex As Integer = index - ValueStart
            If RealIndex >= 0 And DataBindings(key)(DataParamKeys(key)(name)).Count > RealIndex Then
                Return DataBindings(key)(DataParamKeys(key)(name))(RealIndex)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property NomalBinding(key As SCDatFiles.DatFiles, name As String) As DatBinding
        Get
            Dim RealIndex As Integer = 0
            Return DataBindings(key)(DataParamKeys(key)(name))(RealIndex)
        End Get
    End Property


    'Private NullBinding As DatBinding
    Private DataBindings() As List(Of List(Of DatBinding))
    Private DataParamKeys() As Dictionary(Of String, Integer)

End Class
