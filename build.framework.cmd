@echo off

set CONFIGURATION=Release
set TARGETFRAMEWORK=net461

set BUILDFLAGS=--nologo --no-dependencies -c %CONFIGURATION% -f %TARGETFRAMEWORK%

dotnet build %BUILDFLAGS% tools\updbaseaddresses\updbaseaddresses.csproj
dotnet build %BUILDFLAGS% tools\depcheck\depcheck.csproj
dotnet build %BUILDFLAGS% tools\pubkey\pubkey.csproj
dotnet build %BUILDFLAGS% tools\SourceLicenseAnalyzer\SourceLicenseAnalyzer.csproj
dotnet build %BUILDFLAGS% reflect\IKVM.Reflection.csproj
dotnet build %BUILDFLAGS% ikvmstub\ikvmstub.csproj
dotnet build %BUILDFLAGS% runtime\DummyLibrary\DummyLibrary.csproj
dotnet build %BUILDFLAGS% runtime\IKVM.Runtime.FirstPass\IKVM.Runtime.FirstPass.csproj
dotnet build %BUILDFLAGS% awt\IKVM.AWT.WinForms.FirstPass\IKVM.AWT.WinForms.FirstPass.csproj
dotnet build %BUILDFLAGS% ikvmc\ikvmc.csproj
dotnet build %BUILDFLAGS% openjdk\openjdk.csproj
dotnet build %BUILDFLAGS% runtime\IKVM.Runtime.JNI\IKVM.Runtime.JNI.csproj

rem Remove first-pass runtime binaries.
del /F /Q bin\%CONFIGURATION%\%TARGETFRAMEWORK%\IKVM.Runtime.dll bin\%CONFIGURATION%\%TARGETFRAMEWORK%\IKVM.Runtime.deps.json bin\%CONFIGURATION%\%TARGETFRAMEWORK%\IKVM.Runtime.xml bin\%CONFIGURATION%\%TARGETFRAMEWORK%\IKVM.Runtime.pdb
dotnet build %BUILDFLAGS% runtime\IKVM.Runtime\IKVM.Runtime.csproj
dotnet build %BUILDFLAGS% openjdk\openjdk.tools.csproj
rem Remove first-pass awt binaries.
del /F /Q bin\%CONFIGURATION%\%TARGETFRAMEWORK%\IKVM.AWT.WinForms.*
dotnet build %BUILDFLAGS% awt\IKVM.AWT.WinForms\IKVM.AWT.WinForms.csproj
dotnet build %BUILDFLAGS% tools\implib\implib.csproj
dotnet build %BUILDFLAGS% ikvm\ikvm.csproj
dotnet build %BUILDFLAGS% jvm\jvm.csproj
