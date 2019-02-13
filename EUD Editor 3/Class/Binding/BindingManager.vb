Public Class BindingManager
    Private pStatTxtBinding() As StatTxtBinding

    'Private NullBinding As DatBinding
    Private DataBindings() As List(Of List(Of DatBinding))
    Private DataParamKeys() As Dictionary(Of String, Integer)



    Private _CodeConnectGroup As Dictionary(Of SCDatFiles.DatFiles, CodeConnectGroup)
    Public ReadOnly Property CodeConnectGroup(key As SCDatFiles.DatFiles) As CodeConnectGroup
        Get
            Return _CodeConnectGroup(key)
        End Get
    End Property


    Private _CodeConnecter As Dictionary(Of SCDatFiles.DatFiles, List(Of CodeConnecter))
    Public ReadOnly Property CodeConnecter(key As SCDatFiles.DatFiles, index As Integer) As CodeConnecter
        Get
            Return _CodeConnecter(key)(index)
        End Get
    End Property
    Public Sub RefreshCodeUseData(Datfile As SCDatFiles.DatFiles, ParamName As String, index As Integer)
        For i = 0 To _CodeConnectGroup.Count - 1
            If _CodeConnectGroup.Values(i).IsParamExist(ParamName) Then
                CodeConnecter(_CodeConnectGroup.Values(i).GetDatFile, index).ItemsReferesh()


            End If
        Next
    End Sub


    Private _UIManager() As List(Of UIManager)
    Private _UIManagerKey As Dictionary(Of SCDatFiles.DatFiles, Integer)
    Public ReadOnly Property UIManager(key As SCDatFiles.DatFiles, index As Integer) As UIManager
        Get
            Return _UIManager(_UIManagerKey(key))(index)
        End Get
    End Property

    Public Shared Function CheckUIAble(DatFile As SCDatFiles.DatFiles) As Boolean
        If SCDatFiles.CheckValidDat(DatFile) Then
            Return True
        Else
            If DatFile = SCDatFiles.DatFiles.stattxt Then
                Return True
            Else
                Return False
            End If
        End If


    End Function

    Public Sub RefreshCodeTree(key As SCDatFiles.DatFiles, ObjectID As String)
        For i = 0 To pjData.CodeSelecters.Count - 1
            pjData.CodeSelecters(i).RefreshTreeviewItem(key, ObjectID)
        Next
    End Sub

    Public Sub New()
        'NullBinding = New DatBinding()
        _UIManagerKey = New Dictionary(Of SCDatFiles.DatFiles, Integer)
        _CodeConnecter = New Dictionary(Of SCDatFiles.DatFiles, List(Of CodeConnecter))
        _CodeConnectGroup = New Dictionary(Of SCDatFiles.DatFiles, CodeConnectGroup)

        For k = 0 To SCDatFiles.DatFiles.orders
            _CodeConnectGroup.Add(k, New CodeConnectGroup(k))
        Next
        _CodeConnectGroup.Add(SCDatFiles.DatFiles.stattxt, New CodeConnectGroup(SCDatFiles.DatFiles.stattxt))


        ReDim _UIManager(8)
        For k = 0 To SCDatFiles.DatFiles.orders
            _UIManager(k) = New List(Of UIManager)
            _UIManagerKey.Add(k, k)
            For i = 0 To SCCodeCount(k) - 1
                _UIManager(k).Add(New UIManager(k, i))
            Next
        Next

        Dim DatIndex As SCDatFiles.DatFiles = SCDatFiles.DatFiles.stattxt
        Dim ArrayIndex As Integer = 8
        _UIManager(ArrayIndex) = New List(Of UIManager)
        _UIManagerKey.Add(DatIndex, ArrayIndex)
        For i = 0 To SCCodeCount(DatIndex) - 1
            _UIManager(ArrayIndex).Add(New UIManager(DatIndex, i))
        Next


        ReDim pStatTxtBinding(SCtbltxtCount)
        For i = 0 To SCtbltxtCount - 1
            pStatTxtBinding(i) = New StatTxtBinding(i)
        Next


        ReDim DataBindings(scData.DefaultDat.DatFileList.Count)
        ReDim DataParamKeys(scData.DefaultDat.DatFileList.Count)
        For k = 0 To scData.DefaultDat.DatFileList.Count - 1
            DataBindings(k) = New List(Of List(Of DatBinding))
            DataParamKeys(k) = New Dictionary(Of String, Integer)

            For i = 0 To scData.DefaultDat.DatFileList(k).ParamaterList.Count - 1
                Dim keyName As String = scData.DefaultDat.DatFileList(k).ParamaterList(i).GetParamname
                Dim ValueType As SCDatFiles.DatFiles = scData.DefaultDat.DatFileList(k).ParamaterList(i).GetInfo(SCDatFiles.EParamInfo.ValueType)

                If ValueType <> SCDatFiles.DatFiles.None Then
                    If _CodeConnectGroup.Keys.ToList.IndexOf(ValueType) >= 0 Then
                        _CodeConnectGroup(ValueType).Add(k, keyName)
                    End If

                End If


                    DataParamKeys(k).Add(keyName, i)
                DataBindings(k).Add(New List(Of DatBinding))
                For j = 0 To scData.DefaultDat.DatFileList(k).ParamaterList(i).GetValueCount - 1
                    DataBindings(k)(i).Add(New DatBinding(k, keyName, j))
                Next
            Next
        Next

        For k = 0 To SCDatFiles.DatFiles.orders
            _CodeConnecter.Add(k, New List(Of CodeConnecter))
            For i = 0 To SCCodeCount(k) - 1
                _CodeConnecter(k).Add(New CodeConnecter(k, i))
            Next
        Next
        _CodeConnecter.Add(SCDatFiles.DatFiles.stattxt, New List(Of CodeConnecter))
        For i = 0 To SCtbltxtCount - 1
            _CodeConnecter(SCDatFiles.DatFiles.stattxt).Add(New CodeConnecter(SCDatFiles.DatFiles.stattxt, i))
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



    Public ReadOnly Property StatTxtBinding(index As Integer) As StatTxtBinding
        Get
            Return pStatTxtBinding(index)
        End Get
    End Property
End Class
