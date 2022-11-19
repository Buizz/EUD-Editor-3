#If _MyType <> "Empty" Then

Namespace My
    ''' <summary>
    ''' WPF용 My 네임스페이스에 사용할 수 있는 속성을 정의하는 데 사용되는 모듈입니다.
    ''' </summary>
    ''' <remarks></remarks>
    <Global.Microsoft.VisualBasic.HideModuleName()> _
    Module MyWpfExtension
        Private s_Computer As New ThreadSafeObjectProvider(Of Global.Microsoft.VisualBasic.Devices.Computer)
        Private s_User As New ThreadSafeObjectProvider(Of Global.Microsoft.VisualBasic.ApplicationServices.User)
        Private s_Windows As New ThreadSafeObjectProvider(Of MyWindows)
        Private s_Log As New ThreadSafeObjectProvider(Of Global.Microsoft.VisualBasic.Logging.Log)
        ''' <summary>
        ''' 실행하는 응용 프로그램에 대한 응용 프로그램 개체를 반환합니다.
        ''' </summary>
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend ReadOnly Property Application() As Application
            Get
                Return CType(Global.System.Windows.Application.Current, Application)
            End Get
        End Property
        ''' <summary>
        ''' 호스트 컴퓨터에 대한 정보를 반환합니다.
        ''' </summary>
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend ReadOnly Property Computer() As Global.Microsoft.VisualBasic.Devices.Computer
            Get
                Return s_Computer.GetInstance()
            End Get
        End Property
        ''' <summary>
        ''' 현재 사용자에 대한 정보를 반환합니다.  현재 Windows 사용자 자격 증명을 사용하여 응용 프로그램을 실행하려면 
        ''' My.User.InitializeWithWindowsUser()를 호출하십시오.
        ''' </summary>
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend ReadOnly Property User() As Global.Microsoft.VisualBasic.ApplicationServices.User
            Get
                Return s_User.GetInstance()
            End Get
        End Property
        ''' <summary>
        ''' 응용 프로그램 로그를 반환합니다. 응용 프로그램의 구성 파일로 수신기를 구성할 수 있습니다.
        ''' </summary>
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend ReadOnly Property Log() As Global.Microsoft.VisualBasic.Logging.Log
            Get
                Return s_Log.GetInstance()
            End Get
        End Property

        ''' <summary>
        ''' 프로젝트에 정의된 Windows 컬렉션을 반환합니다.
        ''' </summary>
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend ReadOnly Property Windows() As MyWindows
            <Global.System.Diagnostics.DebuggerHidden()> _
            Get
                Return s_Windows.GetInstance()
            End Get
        End Property
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Never)> _
        <Global.Microsoft.VisualBasic.MyGroupCollection("System.Windows.Window", "Create__Instance__", "Dispose__Instance__", "My.MyWpfExtenstionModule.Windows")> _
        Friend NotInheritable Class MyWindows
            <Global.System.Diagnostics.DebuggerHidden()> _
            Private Shared Function Create__Instance__(Of T As {New, Global.System.Windows.Window})(ByVal Instance As T) As T
                If Instance Is Nothing Then
                    If s_WindowBeingCreated IsNot Nothing Then
                        If s_WindowBeingCreated.ContainsKey(GetType(T)) = True Then
                            Throw New Global.System.InvalidOperationException("The window cannot be accessed via My.Windows from the Window constructor.")
                        End If
                    Else
                        s_WindowBeingCreated = New Global.System.Collections.Hashtable()
                    End If
                    s_WindowBeingCreated.Add(GetType(T), Nothing)
                    Return New T()
                    s_WindowBeingCreated.Remove(GetType(T))
                Else
                    Return Instance
                End If
            End Function
            <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>  _
            <Global.System.Diagnostics.DebuggerHidden()> _
            Private Sub Dispose__Instance__(Of T As Global.System.Windows.Window)(ByRef instance As T)
                instance = Nothing
            End Sub
            <Global.System.Diagnostics.DebuggerHidden()> _
            <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Never)> _
            Public Sub New()
                MyBase.New()
            End Sub
            <Global.System.ThreadStatic()> Private Shared s_WindowBeingCreated As Global.System.Collections.Hashtable
            <Global.System.ComponentModel.EditorBrowsable(Global.System.ComponentModel.EditorBrowsableState.Never)> Public Overrides Function Equals(ByVal o As Object) As Boolean
                Return MyBase.Equals(o)
            End Function
            <Global.System.ComponentModel.EditorBrowsable(Global.System.ComponentModel.EditorBrowsableState.Never)> Public Overrides Function GetHashCode() As Integer
                Return MyBase.GetHashCode
            End Function
            <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>  _
            <Global.System.ComponentModel.EditorBrowsable(Global.System.ComponentModel.EditorBrowsableState.Never)> _
            Friend Overloads Function [GetType]() As Global.System.Type
                Return GetType(MyWindows)
            End Function
            <Global.System.ComponentModel.EditorBrowsable(Global.System.ComponentModel.EditorBrowsableState.Never)> Public Overrides Function ToString() As String
                Return MyBase.ToString
            End Function
        End Class
    End Module
End Namespace
Partial Class Application
    Inherits Global.System.Windows.Application
    <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")> _
    <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1822:MarkMembersAsStatic")> _
    Friend ReadOnly Property Info() As Global.Microsoft.VisualBasic.ApplicationServices.AssemblyInfo
        <Global.System.Diagnostics.DebuggerHidden()> _
        Get
            Return New Global.Microsoft.VisualBasic.ApplicationServices.AssemblyInfo(Global.System.Reflection.Assembly.GetExecutingAssembly())
        End Get
    End Property
End Class
#End If