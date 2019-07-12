Imports System
Imports System.Runtime.InteropServices

Public Class CascLib

    <DllImport("CascLib.dll")>
    Public Shared Function CascOpenStorage(szDataPath As String, dwLocaleMask As UInteger, ByRef phStorage As IntPtr) As Boolean
    End Function

    <DllImport("CascLib.dll")>
    Public Shared Function CascOpenFile(hStorage As IntPtr, szFileName As String, dwLocale As UInteger, dwFlags As UInteger, ByRef phFile As IntPtr) As Boolean
    End Function

    <DllImport("CascLib.dll")>
    Public Shared Function CascReadFile(hFile As IntPtr, lpBuffer() As Byte, dwToRead As UInteger, ByRef pdwRead As IntPtr) As Boolean
    End Function

    <DllImport("CascLib.dll")>
    Public Shared Function CascCloseFile(hFile As IntPtr) As Boolean
    End Function

    <DllImport("CascLib.dll")>
    Public Shared Function CascCloseStorage(hStorage As IntPtr) As Boolean
    End Function







End Class
