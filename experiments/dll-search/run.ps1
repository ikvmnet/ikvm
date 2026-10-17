# Lays out the test DLLs and runs harness.exe in a fresh process for every (case, mode) combination.
param([string]$Build, [string]$Root, [string]$IkvmBin)
$ErrorActionPreference = 'Stop'
$E = "$Root\E"; $X = "$Root\X"; $Y = "$Root\Y"; $H = "$Root\H"; $W = "$Root\W"
foreach ($d in $E,$X,$Y,$H,$W) { New-Item -ItemType Directory -Force $d | Out-Null }
Copy-Item "$Build\harness.exe" $E
Copy-Item "$Build\eb.dll" $E
foreach ($n in 'xa','xm','xa2','xa4') { Copy-Item "$Build\$n.dll" $X }
foreach ($n in 'yb','ys') { Copy-Item "$Build\$n.dll" $Y }
foreach ($n in 'hp','hq','hr','hu','hp2') { Copy-Item "$Build\$n.dll" $H }

# minimal PATH: system dirs + Y only (X, H, E and IKVM bin are never on PATH)
$env:PATH = "$env:SystemRoot\system32;$env:SystemRoot;$Y"
Push-Location $W
$modes = 'plain','current','altered','retry'
$cases = [ordered]@{
  'xa  (X) -> yb (Y on PATH)'                       = "$X\xa.dll"
  'xa2 (X) -> xm (X) -> yb (Y on PATH)'             = "$X\xa2.dll"
  'hp  (H) -> hq (H) -> hr (H)'                     = "$H\hp.dll"
  'hp2 (H) -> ys (Y on PATH) -> hu (only H)'        = "$H\hp2.dll"
  'xa4 (X) -> eb (only host exe dir)'               = "$X\xa4.dll"
}
if ($IkvmBin) { Get-ChildItem "$IkvmBin\*.dll" | Sort-Object Name | ForEach-Object { $cases["JDK $($_.Name)"] = $_.FullName } }

$out = @()
foreach ($sd in '', 'setdefault') {
  $out += ""
  $out += "### SetDefaultDllDirectories(DEFAULT_DIRS) called first: $([bool]$sd)"
  $out += ""
  $out += "| case | " + ($modes -join ' | ') + " |"
  $out += "|---|" + (($modes | ForEach-Object { '---' }) -join '|') + "|"
  foreach ($c in $cases.Keys) {
    $row = @()
    foreach ($m in $modes) { $r = & "$E\harness.exe" $m $cases[$c] $sd; $row += ($r -join ' ') }
    $out += "| $c | " + ($row -join ' | ') + " |"
  }
}
Pop-Location
$out | ForEach-Object { Write-Host $_ }
if ($env:GITHUB_STEP_SUMMARY) { $out | Add-Content $env:GITHUB_STEP_SUMMARY }
