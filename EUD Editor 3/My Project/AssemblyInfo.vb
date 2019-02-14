Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Resources
Imports System.Windows

' 어셈블리에 대한 일반 정보는 다음 특성 집합을 통해 
' 제어됩니다. 어셈블리와 관련된 정보를 수정하려면
' 이러한 특성 값을 변경하세요.

' 어셈블리 특성 값을 검토합니다.

<Assembly: AssemblyTitle("EUD Editor 3")>
<Assembly: AssemblyDescription("")>
<Assembly: AssemblyCompany("")>
<Assembly: AssemblyProduct("EUD Editor 3")>
<Assembly: AssemblyCopyright("Copyright ©  2019 JPoker")>
<Assembly: AssemblyTrademark("")>
<Assembly: ComVisible(false)>

'지역화 가능 응용 프로그램 빌드를 시작하려면 다음을 설정하세요.
'.vbproj 파일에서 <PropertyGroup> 내에 <UICulture>CultureYouAreCodingWith</UICulture>를
'설정하십시오. 예를 들어 소스 파일에서 영어(미국)를
'사용하는 경우 <UICulture>를 "en-US"로 설정합니다. 그런 다음 아래
'NeutralResourceLanguage 특성의 주석 처리를 제거합니다. 아래 줄의 "en-US"를 업데이트하여
'프로젝트 파일의 UICulture 설정과 일치시킵니다.

'<Assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)>


'ThemeInfo 특성은 모든 테마별 리소스 사전과 제네릭 리소스 사전을 찾을 수 있는 위치를 나타냅니다.
'첫 번째 매개 변수: 테마별 리소스 사전의 위치
'(페이지 또는 응용 프로그램 리소스 사진에
' 리소스가 없는 경우에 사용됨)

'두 번째 매개 변수: 제네릭 리소스 사전의 위치
'(페이지 또는 응용 프로그램 리소스 사진에
'리소스 사전에 리소스가 없는 경우에 사용됨)
<Assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)>



'이 프로젝트가 COM에 노출되는 경우 다음 GUID는 typelib의 ID를 나타냅니다.
<Assembly: Guid("5f564870-11c8-4d50-b537-e6ac98e9dcbe")>

' 어셈블리의 버전 정보는 다음 네 가지 값으로 구성됩니다.
'
'      주 버전
'      부 버전 
'      빌드 번호
'      수정 버전
'
' 모든 값을 지정하거나 아래와 같이 '*'를 사용하여 빌드 번호 및 수정 번호가 자동으로
' 지정되도록 할 수 있습니다.
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("0.1.0.0")>
<Assembly: AssemblyFileVersion("0.0.0.1")>
