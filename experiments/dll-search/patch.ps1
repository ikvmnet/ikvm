# Rewrites the LoadLibraryExW flags immediate in os_dll_load (0x1100 -> 0x8, LOAD_WITH_ALTERED_SEARCH_PATH).
param([string]$Dll)
$b = [IO.File]::ReadAllBytes($Dll)
$pat = [byte[]](0x41,0xB8,0x00,0x11,0x00,0x00)  # mov r8d, 0x1100
$hits = @()
for ($i = 0; $i -le $b.Length - $pat.Length; $i++) { $ok = $true; for ($j = 0; $j -lt $pat.Length; $j++) { if ($b[$i+$j] -ne $pat[$j]) { $ok = $false; break } }; if ($ok) { $hits += $i } }
Write-Host "matches of 'mov r8d,0x1100' in $Dll : $($hits.Count) at $($hits -join ',')"
if ($hits.Count -ne 1) { throw "expected exactly one match" }
$b[$hits[0]+3] = 0x00; $b[$hits[0]+2] = 0x08
[IO.File]::WriteAllBytes($Dll, $b)
