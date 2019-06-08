Imports System.Windows.Threading
Imports BondTech.HotKeyManagement.WPF._4
Imports BondTech.HotKeyManagement.WPF._4.KeyboardHookEventArgs

Public Class ProjectControl
    Private Sub MyHotKeyManager_LocalHotKeyPressed(sender As Object, e As LocalHotKeyEventArgs)
        Select Case e.HotKey.Name
            Case "Setting"
                WindowMenu.Setting()
            Case "NewFile"
                WindowMenu.NewFile()
            Case "Load"
                WindowMenu.Load()
            Case "Save"
                WindowMenu.Save()
            Case "ScmdOpen"
                WindowMenu.ScmdOpen()
            Case "insert"
                WindowMenu.insert()
            Case "OpenDataEditor"
                WindowMenu.OpenDataEditor()
            Case "OpenTriggerEdit"
                WindowMenu.OpenTriggerEdit()
            Case "OpenPlugin"
                WindowMenu.OpenPlugin()
            Case "CodeFold"
                WindowMenu.CodeFold()
        End Select
    End Sub


    Private MyHotKeyManager As HotKeyManager
    Public Sub HotkeyInit(twindow As Window)
        MyHotKeyManager = New HotKeyManager(twindow)
        Dim hSetting As New LocalHotKey("Setting", ModifierKeys.Control, Keys.E)
        Dim hNewFile As New LocalHotKey("NewFile", ModifierKeys.Control Xor ModifierKeys.Shift, Keys.N)
        Dim hLoad As New LocalHotKey("Load", ModifierKeys.Control, Keys.O)
        Dim hSave As New LocalHotKey("Save", ModifierKeys.Control, Keys.S)
        Dim hScmdOpen As New LocalHotKey("ScmdOpen", ModifierKeys.Control, Keys.W)
        Dim hinsert As New LocalHotKey("insert", ModifierKeys.Control, Keys.B)
        Dim hOpenDataEditor As New LocalHotKey("OpenDataEditor", ModifierKeys.Control, Keys.D)
        Dim hOpenTriggerEdit As New LocalHotKey("OpenTriggerEdit", ModifierKeys.Control, Keys.T)
        Dim hOpenPlugin As New LocalHotKey("OpenPlugin", ModifierKeys.Control, Keys.P)
        Dim hCodeFold As New LocalHotKey("CodeFold", ModifierKeys.Control, Keys.F)


        MyHotKeyManager.AddLocalHotKey(hSetting)
        MyHotKeyManager.AddLocalHotKey(hNewFile)
        MyHotKeyManager.AddLocalHotKey(hLoad)
        MyHotKeyManager.AddLocalHotKey(hSave)
        MyHotKeyManager.AddLocalHotKey(hScmdOpen)
        MyHotKeyManager.AddLocalHotKey(hinsert)
        MyHotKeyManager.AddLocalHotKey(hOpenDataEditor)
        MyHotKeyManager.AddLocalHotKey(hOpenTriggerEdit)
        MyHotKeyManager.AddLocalHotKey(hOpenPlugin)
        MyHotKeyManager.AddLocalHotKey(hCodeFold)

        AddHandler MyHotKeyManager.LocalHotKeyPressed, AddressOf MyHotKeyManager_LocalHotKeyPressed
    End Sub


    Private Sub Control_Unloaded(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub Control_Loaded(sender As Object, e As RoutedEventArgs)
        Me.DataContext = ProjectControlBinding

    End Sub



    Private Sub MenuItemSave_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.MenuItemSave()
    End Sub
    Private Sub MenuItemSaveAs_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.MenuItemSaveAs()
    End Sub

    Private Sub BtnNewFile_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.NewFile()
    End Sub

    Private Sub BtnSetting_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.Setting()
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.Close()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.Save()
    End Sub

    Private Sub BtnLoad_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.Load()
    End Sub

    Private Sub Btn_scmd_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.ScmdOpen()
    End Sub

    Private Sub Btn_insert_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.insert()
    End Sub


    Private Sub BtnDataEditor_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.OpenDataEditor()
    End Sub

    Private Sub Btn_TriggerEdit_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.OpenTriggerEdit()
    End Sub

    Private Sub Btn_Plugin_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.OpenPlugin()
    End Sub

    Private Sub MenuItemEdd_Click(sender As Object, e As RoutedEventArgs)
        WindowMenu.insert(True)
    End Sub
End Class
