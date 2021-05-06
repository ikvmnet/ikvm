@echo off

set CONFIGURATION=Release
set TARGETFRAMEWORK=netcoreapp3.1

set CLEANFLAGS=--nologo -c %CONFIGURATION% -f %TARGETFRAMEWORK%

dotnet clean %CLEANFLAGS% tools\updbaseaddresses\updbaseaddresses.csproj
dotnet clean %CLEANFLAGS% tools\depcheck\depcheck.csproj
dotnet clean %CLEANFLAGS% tools\pubkey\pubkey.csproj
dotnet clean %CLEANFLAGS% tools\SourceLicenseAnalyzer\SourceLicenseAnalyzer.csproj
dotnet clean %CLEANFLAGS% reflect\IKVM.Reflection.csproj
dotnet clean %CLEANFLAGS% ikvmstub\ikvmstub.csproj
dotnet clean %CLEANFLAGS% runtime\DummyLibrary\DummyLibrary.csproj
dotnet clean %CLEANFLAGS% runtime\IKVM.Runtime.FirstPass\IKVM.Runtime.FirstPass.csproj
dotnet clean %CLEANFLAGS% awt\IKVM.AWT.WinForms.FirstPass\IKVM.AWT.WinForms.FirstPass.csproj
dotnet clean %CLEANFLAGS% ikvmc\ikvmc.csproj
dotnet clean %CLEANFLAGS% openjdk\openjdk.csproj
dotnet clean %CLEANFLAGS% runtime\IKVM.Runtime.JNI\IKVM.Runtime.JNI.csproj
dotnet clean %CLEANFLAGS% runtime\IKVM.Runtime\IKVM.Runtime.csproj
dotnet clean %CLEANFLAGS% openjdk\openjdk.tools.csproj
dotnet clean %CLEANFLAGS% awt\IKVM.AWT.WinForms\IKVM.AWT.WinForms.csproj

rem dotnet clean %CLEANFLAGS% tools\implib\implib.csproj
rem dotnet clean %CLEANFLAGS% ikvm\ikvm.csproj
rem dotnet clean %CLEANFLAGS% jvm\jvm.csproj
