<#
.SYNOPSIS
Turns the trx files from an audit run into a table of excluded-test outcomes.

.DESCRIPTION
Emits one object per test with Outcome, Suite, Test and Message. Tests that
pass here are candidates for removal from the exclude list; tests that fail are
still legitimately excluded and want a comment recording why.

A test that appears in the staged AuditList.txt but not in the results was
never reported, usually because the host crashed partway through the batch.
Those are emitted with an outcome of 'NotRun' so they are not mistaken for
passes.

.EXAMPLE
./Get-AuditResult.ps1 -ResultsDirectory ../bin/Release/net8.0/TestResults `
    -AuditList ../bin/Release/net8.0/jdk/test/AuditList.txt |
    Export-Csv audit-net8.0.csv -NoTypeInformation
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)] [string] $ResultsDirectory,
    # AuditList.txt that was staged, used to spot tests that never reported.
    [string] $AuditList
)

$ErrorActionPreference = 'Stop'

$trx = Get-ChildItem $ResultsDirectory -Recurse -Filter *.trx -ErrorAction SilentlyContinue
if (-not $trx) { throw "no trx files under $ResultsDirectory" }

$seen = @{}
$results = foreach ($f in $trx) {
    $xml = [xml](Get-Content $f.FullName -Raw)
    $ns = New-Object System.Xml.XmlNamespaceManager($xml.NameTable)
    $ns.AddNamespace('t', 'http://microsoft.com/schemas/VisualStudio/TeamTest/2010')
    foreach ($r in $xml.SelectNodes('//t:UnitTestResult', $ns)) {
        $name = $r.testName
        $seen[$name] = $true
        [pscustomobject]@{
            Outcome = $r.outcome
            Test    = $name
            Message = ($r.Output.ErrorInfo.Message -replace '\s+', ' ').Trim()
        }
    }
}

$results | Sort-Object Outcome, Test

if ($AuditList -and (Test-Path $AuditList)) {
    $staged = Get-Content $AuditList |
        Where-Object { $_.Trim() } |
        ForEach-Object { ($_ -split '\s+')[0] }
    foreach ($s in $staged) {
        # results carry the short jtreg test name, so match on the leaf
        $leaf = [System.IO.Path]::GetFileNameWithoutExtension($s)
        if (-not $seen.ContainsKey($leaf)) {
            [pscustomobject]@{ Outcome = 'NotRun'; Test = $s; Message = '' }
        }
    }
}
