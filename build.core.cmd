@echo off

rem dotnet build -f netcoreapp3.1 tools\updbaseaddresses\updbaseaddresses.csproj
rem dotnet build -f netcoreapp3.1 tools\depcheck\depcheck.csproj
rem dotnet build -f netcoreapp3.1 tools\pubkey\pubkey.csproj
rem dotnet build -f netcoreapp3.1 tools\SourceLicenseAnalyzer\SourceLicenseAnalyzer.csproj
rem dotnet build -f netcoreapp3.1 reflect\IKVM.Reflection.csproj
rem dotnet build -f netcoreapp3.1 ikvmstub\ikvmstub.csproj
rem dotnet build -f netcoreapp3.1 runtime\DummyLibrary\DummyLibrary.csproj
rem dotnet build -f netcoreapp3.1 runtime\IKVM.Runtime.FirstPass\IKVM.Runtime.FirstPass.csproj
rem dotnet build -f netcoreapp3.1 awt\IKVM.AWT.WinForms.FirstPass\IKVM.AWT.WinForms.FirstPass.csproj
rem dotnet build -f netcoreapp3.1 ikvmc\ikvmc.csproj
rem dotnet build -f netcoreapp3.1 openjdk\openjdk.csproj
rem dotnet build -f netcoreapp3.1 runtime\IKVM.Runtime.JNI\IKVM.Runtime.JNI.csproj
dotnet build -f netcoreapp3.1 runtime\IKVM.Runtime\IKVM.Runtime.csproj
rem dotnet build -f netcoreapp3.1 openjdk\openjdk.tools.csproj
rem dotnet build -f netcoreapp3.1 awt\IKVM.AWT.WinForms\IKVM.AWT.WinForms.csproj
rem dotnet build -f netcoreapp3.1 tools\implib\implib.csproj
rem dotnet build -f netcoreapp3.1 ikvm\ikvm.csproj
rem dotnet build -f netcoreapp3.1 jvm\jvm.csproj
