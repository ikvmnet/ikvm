<#
.SYNOPSIS
Stages a batch of currently-excluded tests to be run again.

.DESCRIPTION
Takes a slice of a suite's ExcludeList.txt, writes it into the built test tree
as AuditList.txt, and writes empty AuditList.txt files into the remaining
suites so they match nothing. Also drops Audit.runsettings beside the test
assembly.

Batching matters. Excluded tests include ones that crash the test host, and a
crash aborts the whole run, so a large batch can lose all of its results. Keep
batches small enough that a crash is cheap to redo.

.EXAMPLE
./New-AuditList.ps1 -TestRoot ../bin/Release/net8.0 -Skip 0 -Take 200
dotnet test -f net8.0 --settings ../bin/Release/net8.0/Audit.runsettings `
    --logger trx ../bin/Release/net8.0/IKVM.OpenJDK.Tests.dll
#>
[CmdletBinding()]
param(
    # Built test tree, i.e. the directory holding IKVM.OpenJDK.Tests.dll.
    [Parameter(Mandatory = $true)] [string] $TestRoot,
    # Suite to audit.
    [ValidateSet('jdk', 'langtools', 'nashorn')] [string] $Suite = 'jdk',
    [int] $Skip = 0,
    [int] $Take = 200
)

$ErrorActionPreference = 'Stop'

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$src = Join-Path $here ".."
$allSuites = @('jdk', 'langtools', 'nashorn')

if (-not (Test-Path $TestRoot)) { throw "no such test root: $TestRoot" }

$excludeList = Join-Path $src "$Suite/test/ExcludeList.txt"
if (-not (Test-Path $excludeList)) { throw "no exclude list for suite '$Suite'" }

# entries are "<test path><padding><platforms>"; comments and blanks are skipped
$entries = Get-Content $excludeList |
    Where-Object { $_.Trim() -and -not $_.TrimStart().StartsWith('#') } |
    ForEach-Object { ($_ -split '\s+')[0] } |
    Select-Object -Unique

$batch = $entries | Select-Object -Skip $Skip -First $Take
if (-not $batch) { throw "empty batch: only $($entries.Count) entries, skipped $Skip" }

foreach ($s in $allSuites) {
    $dir = Join-Path $TestRoot "$s/test"
    if (-not (Test-Path $dir)) { continue }
    $out = Join-Path $dir 'AuditList.txt'
    if ($s -eq $Suite) {
        # include lists use the same two-column layout as the exclude lists
        $batch | ForEach-Object { $_.PadRight(120) + 'generic-all' } | Set-Content $out -Encoding UTF8
    } else {
        Set-Content $out '' -NoNewline -Encoding UTF8
    }
}

Copy-Item (Join-Path $here 'Audit.runsettings') (Join-Path $TestRoot 'Audit.runsettings') -Force

"suite     : $Suite"
"entries   : $($entries.Count)"
"batch     : $Skip..$($Skip + $batch.Count - 1) ($($batch.Count) tests)"
"staged in : $TestRoot"
