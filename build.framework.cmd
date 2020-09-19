@echo off

dotnet build -f net461 tools\updbaseaddresses\updbaseaddresses.csproj
dotnet build -f net461 tools\depcheck\depcheck.csproj
dotnet build -f net461 tools\pubkey\pubkey.csproj
dotnet build -f net461 tools\SourceLicenseAnalyzer\SourceLicenseAnalyzer.csproj
dotnet build -f net461 reflect\IKVM.Reflection.csproj
dotnet build -f net461 ikvmstub\ikvmstub.csproj
dotnet build -f net461 runtime\DummyLibrary\DummyLibrary.csproj
dotnet build -f net461 runtime\IKVM.Runtime.FirstPass\IKVM.Runtime.FirstPass.csproj
dotnet build -f net461 awt\IKVM.AWT.WinForms.FirstPass\IKVM.AWT.WinForms.FirstPass.csproj
dotnet build -f net461 ikvmc\ikvmc.csproj
dotnet build -f net461 openjdk\openjdk.csproj
dotnet build -f net461 runtime\IKVM.Runtime.JNI\IKVM.Runtime.JNI.csproj
dotnet build -f net461 runtime\IKVM.Runtime\IKVM.Runtime.csproj
dotnet build -f net461 openjdk\openjdk.tools.csproj
dotnet build -f net461 awt\IKVM.AWT.WinForms\IKVM.AWT.WinForms.csproj
dotnet build -f net461 tools\implib\implib.csproj
dotnet build -f net461 ikvm\ikvm.csproj
dotnet build -f net461 jvm\jvm.csproj
