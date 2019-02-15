Imports System.Collections.Specialized
Imports System.ComponentModel

Public Class CodeConnecter
    Implements INotifyCollectionChanged

    Private Datfile As SCDatFiles.DatFiles
    Private ObjectID As Integer

    Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged
    'Dat종류, 오브젝트 아이디만 있으면 됨

    'Dat종류에 따른 출처들을 미리 정리해두자
    'ex Dat종류 Weapon의 경우, 모든 유닛들의 UnitDat_GroundWeapon을 조사하여 오브젝트 아이디가 같으면 해당 아이디를 보유한 유닛을 추가.

    '이 관계를 이용하여 연결된 오브젝트의 이름도 변경해주면 좋겠음
    Private itemCollection As List(Of CodeCollection)


    Public Sub New(tDatfile As SCDatFiles.DatFiles, tObjectID As Integer)
        Datfile = tDatfile
        ObjectID = tObjectID

        itemCollection = New List(Of CodeCollection)
    End Sub


    Public Sub ItemsReferesh()
        ListReset()

        'MsgBox(Datfile & " " & ObjectID)
        'PropertyChangedPack()
    End Sub
    Private Sub PropertyChangedPack()
        'ListReset()
        'NotifyPropertyChanged("Items")
    End Sub

    Public Sub DeleteList(CC As CodeCollection)
        itemCollection.Remove(CC)
    End Sub
    Public ReadOnly Property Items() As CodeCollection
        Get
            itemCollection.Add(New CodeCollection)
            ListReset()
            'itemCollection.Add("작작하라했다")


            Return itemCollection.Last



            ''MsgBox("아이템 리프레쉬")

            'Dim listboxitems As New List(Of ListBoxItem)

            'Dim CC As CodeConnectGroup = pjData.BindingManager.CodeConnectGroup(Datfile)
            'For i = 0 To CC.Count - 1
            '    Dim Datfiles As SCDatFiles.DatFiles = CC.GetDatType(i)
            '    Dim ParamName As String = CC.GetParamName(i)

            '    For index = 0 To pjData.Dat.DatFileList(Datfiles).GetParamInfo(ParamName, SCDatFiles.EParamInfo.VarCount) - 1
            '        Dim Realindex As Integer = index + pjData.Dat.DatFileList(Datfiles).GetParamInfo(ParamName, SCDatFiles.EParamInfo.VarStart)

            '        If pjData.Dat.Data(Datfiles, ParamName, Realindex) = ObjectID Then
            '            Dim tListBox As New ListBoxItem
            '            Dim Textblock As New TextBlock
            '            Textblock.TextWrapping = TextWrapping.Wrap

            '            Dim myBinding As Binding = New Binding("TabName")
            '            myBinding.Source = pjData.BindingManager.UIManager(Datfiles, index)
            '            Textblock.SetBinding(TextBlock.TextProperty, myBinding)

            '            tListBox.Content = Textblock

            '            'tListBox.Content = Tool.GetText(Datfilesname(Datfiles)) & ParamName & " " & Realindex
            '            listboxitems.Add(tListBox)
            '        End If
            '        'MsgBox(Tool.GetText(Datfilesname(Datfiles)) & ParamName & " " & Realindex)
            '    Next
            'Next



            ''For i = 0 To Datfile + 2
            ''    Dim asfaga As New ListBoxItem
            ''    asfaga.Content = "asdfa " & i
            ''    listboxitems.Add(asfaga)
            ''Next





            'Return listboxitems.ToArray
        End Get

    End Property




    Private Sub ListReset()

        For ci = 0 To itemCollection.Count - 1
            itemCollection(ci).Clear()

            Dim CC As CodeConnectGroup = pjData.BindingManager.CodeConnectGroup(Datfile)
            For i = 0 To CC.Count - 1
                Dim Datfiles As SCDatFiles.DatFiles = CC.GetDatType(i)
                Dim ParamName As String = CC.GetParamName(i)

                For index = 0 To pjData.Dat.DatFileList(Datfiles).GetParamInfo(ParamName, SCDatFiles.EParamInfo.VarCount) - 1
                    Dim Realindex As Integer = index + pjData.Dat.DatFileList(Datfiles).GetParamInfo(ParamName, SCDatFiles.EParamInfo.VarStart)

                    If pjData.Dat.Data(Datfiles, ParamName, Realindex) = ObjectID Then
                        Dim tListBox As New ListBoxItem
                        tListBox.Tag = Datfiles & "," & ParamName & "," & Realindex
                        tListBox.ToolTip = Tool.GetText(Datfilesname(Datfiles)) & " '" & Tool.GetText(Datfilesname(Datfiles) & "_" & ParamName) & "'"

                        Dim Textblock As New TextBlock
                        Textblock.TextWrapping = TextWrapping.Wrap

                        Dim myBinding As Binding = New Binding("TabName")
                        myBinding.Source = pjData.BindingManager.UIManager(Datfiles, Realindex)
                        Textblock.SetBinding(TextBlock.TextProperty, myBinding)

                        tListBox.Content = Textblock

                        'tListBox.Content = Tool.GetText(Datfilesname(Datfiles)) & ParamName & " " & Realindex
                        itemCollection(ci).Add(tListBox)
                    End If
                    'MsgBox(Tool.GetText(Datfilesname(Datfiles)) & ParamName & " " & Realindex)
                Next
            Next
        Next

    End Sub

    Private Sub NotifyPropertyChanged(ByVal info As String)
        'MsgBox("리프레쉬")
        'MsgBox("아이템 리프레쉬")
        For ci = 0 To itemCollection.Count - 1
            itemCollection.Clear()
            Dim CC As CodeConnectGroup = pjData.BindingManager.CodeConnectGroup(Datfile)
            For i = 0 To CC.Count - 1
                Dim Datfiles As SCDatFiles.DatFiles = CC.GetDatType(i)
                Dim ParamName As String = CC.GetParamName(i)

                For index = 0 To pjData.Dat.DatFileList(Datfiles).GetParamInfo(ParamName, SCDatFiles.EParamInfo.VarCount) - 1
                    Dim Realindex As Integer = index + pjData.Dat.DatFileList(Datfiles).GetParamInfo(ParamName, SCDatFiles.EParamInfo.VarStart)

                    If pjData.Dat.Data(Datfiles, ParamName, Realindex) = ObjectID Then
                        Dim tListBox As New ListBoxItem
                        Dim Textblock As New TextBlock
                        Textblock.TextWrapping = TextWrapping.Wrap

                        Dim myBinding As Binding = New Binding("TabName")
                        myBinding.Source = pjData.BindingManager.UIManager(Datfiles, index)
                        Textblock.SetBinding(TextBlock.TextProperty, myBinding)

                        tListBox.Content = Textblock

                        'tListBox.Content = Tool.GetText(Datfilesname(Datfiles)) & ParamName & " " & Realindex
                        itemCollection(ci).Add(tListBox)
                    End If
                    'MsgBox(Tool.GetText(Datfilesname(Datfiles)) & ParamName & " " & Realindex)
                Next
            Next
        Next






        'For i = 0 To Datfile + 2
        '    Dim asfaga As New ListBoxItem
        '    asfaga.Content = "asdfa " & i
        '    listboxitems.Add(asfaga)
        'Next


        'itemCollection.Add("dsf")
        'RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, "sad"))
    End Sub
End Class

Public Class CodeCollection
    Inherits ObjectModel.ObservableCollection(Of ListBoxItem)

End Class

Public Class CodeConnectGroup
    '

    Private DatFile As SCDatFiles.DatFiles

    Public ReadOnly Property GetDatFile As SCDatFiles.DatFiles
        Get
            Return DatFile
        End Get
    End Property


    Private DatType As List(Of SCDatFiles.DatFiles)
    Private ParamName As List(Of String)


    Public Sub New(tDatFile As SCDatFiles.DatFiles)
        DatType = New List(Of SCDatFiles.DatFiles)
        ParamName = New List(Of String)

        DatFile = tDatFile
    End Sub

    Public Sub Add(_DatType As SCDatFiles.DatFiles, _ParamName As String)
        DatType.Add(_DatType)
        ParamName.Add(_ParamName)
    End Sub

    Public ReadOnly Property Count As Integer
        Get
            Return DatType.Count
        End Get
    End Property
    Public ReadOnly Property GetDatType(index As Integer) As SCDatFiles.DatFiles
        Get
            Return DatType(index)
        End Get
    End Property

    Public ReadOnly Property GetParamName(index As Integer) As String
        Get
            Return ParamName(index)
        End Get
    End Property

    Public ReadOnly Property IsParamExist(tParamName As String) As Boolean
        Get
            If ParamName.IndexOf(tParamName) >= 0 Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    '코드 종속 관계
    '유닛 - 유닛_부가유닛12, 유닛_감염유닛
    '무기 - 유닛_공중지상무기, 명령_목표지정시
    '비행정보 - 유닛_비행정보, 무기_그래픽
    '스프라이트 - 비행정보_스프라이트
    '이미지 - 유닛_생산모습, 스프라이트_이미지
    '업그레이드 - 유닛_방어구, 무기_업그레이드
    '기술 - 무기_사용안함, 명령_에너지
    '명령 - 유닛_사람.컴터기본.원상태로.유닛공격.공격이동, 명령_불명확명령
    '텍스트 - 유닛_이름,계급 ,무기_이름, 에러메세지, 업그레이드_이름, 기술_이름, 명령_이름

    'ex 만약 업그레이드를 바꾸면 해당 방어구와 무기업그레이드의 ValueText가 바뀌어야함
    'ex 유닛_생산모습을 바꾸면 바꾸기 전의 이미지에한테 Refresh명령을 내려야함 바꾸기 후에도

End Class