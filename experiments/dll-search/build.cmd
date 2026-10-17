@echo off
setlocal enabledelayedexpansion
for /f "usebackq tokens=*" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -property installationPath`) do set VSINSTALL=%%i
call "%VSINSTALL%\VC\Auxiliary\Build\vcvars64.bat" >nul || exit /b 1
cd /d %1
cl /nologo /W3 harness.c /Fe:harness.exe || exit /b 1
for %%p in (yb:- xa:yb xm:yb xa2:xm hr:- hq:hr hp:hq hu:- ys:hu hp2:ys eb:- xa4:eb) do (
  for /f "tokens=1,2 delims=:" %%a in ("%%p") do (
    if "%%b"=="-" ( cl /nologo /LD %%a.c /Fe:%%a.dll >nul || exit /b 1 ) else ( cl /nologo /LD %%a.c %%b.lib /Fe:%%a.dll >nul || exit /b 1 )
  )
)
