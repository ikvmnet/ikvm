@echo off

dotnet build -f netcoreapp3.1 tools\updbaseaddresses\updbaseaddresses.csproj
dotnet build -f netcoreapp3.1 tools\depcheck\depcheck.csproj
dotnet build -f netcoreapp3.1 tools\pubkey\pubkey.csproj
dotnet build -f netcoreapp3.1 tools\SourceLicenseAnalyzer\SourceLicenseAnalyzer.csproj
dotnet build -f netcoreapp3.1 reflect\IKVM.Reflection.csproj
dotnet build -f netcoreapp3.1 ikvmstub\ikvmstub.csproj
dotnet build -f netcoreapp3.1 runtime\DummyLibrary\DummyLibrary.csproj
dotnet build -f netcoreapp3.1 runtime\IKVM.Runtime.FirstPass\IKVM.Runtime.FirstPass.csproj
dotnet build -f netcoreapp3.1 awt\IKVM.AWT.WinForms.FirstPass\IKVM.AWT.WinForms.FirstPass.csproj
dotnet build -f netcoreapp3.1 ikvmc\ikvmc.csproj
dotnet build -f netcoreapp3.1 openjdk\openjdk.csproj
dotnet build -f netcoreapp3.1 runtime\IKVM.Runtime.JNI\IKVM.Runtime.JNI.csproj
dotnet build -f netcoreapp3.1 runtime\IKVM.Runtime\IKVM.Runtime.csproj
rem dotnet build -f netcoreapp3.1 openjdk\openjdk.tools.csproj
rem dotnet build -f netcoreapp3.1 awt\IKVM.AWT.WinForms\IKVM.AWT.WinForms.csproj
rem dotnet build -f netcoreapp3.1 tools\implib\implib.csproj
rem dotnet build -f netcoreapp3.1 ikvm\ikvm.csproj
rem dotnet build -f netcoreapp3.1 jvm\jvm.csproj
