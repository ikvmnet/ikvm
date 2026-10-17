# generates one C file per test DLL: <name>.c exporting f_<name>, calling f_<dep> when dep given
param([string]$Out)
$defs = @(
  @('yb',  $null), @('xa', 'yb'),             # direct dep on PATH
  @('xm',  'yb'),  @('xa2','xm'),             # transitive: sibling -> PATH
  @('hr',  $null), @('hq', 'hr'), @('hp','hq'), # chain within own dir (JDK shape)
  @('hu',  $null), @('ys', 'hu'), @('hp2','ys'), # PATH dep whose own dep lives only in top-level dir
  @('eb',  $null), @('xa4','eb')              # dep only in host exe dir
)
foreach ($d in $defs) {
  $n = $d[0]; $dep = $d[1]
  if ($dep) { $src = "__declspec(dllimport) int f_$dep(void);`n__declspec(dllexport) int f_$n(void) { return f_$dep() + 1; }`n" }
  else { $src = "__declspec(dllexport) int f_$n(void) { return 1; }`n" }
  Set-Content -Path (Join-Path $Out "$n.c") -Value $src
  "$n $dep"
}
