# Auditing the exclude lists

`ExcludeList.txt` has accumulated for years. Most entries carry no explanation,
and an unknown number describe problems that no longer exist. This directory
holds the harness for finding out which is which.

The goal is threefold: drop entries for tests that pass now, keep the ones that
genuinely fail, and record against each of those *why* it is excluded.

## Current size

| File | Entries | Comment lines |
|---|---|---|
| `jdk/test/ExcludeList.txt` | 4746 | 48 |
| `jdk/test/ExcludeList.net8.0.txt` | 6 | 2 |
| `jdk/test/ExcludeList.net6.0.txt` | 0 | 0 |
| `langtools/test/ExcludeList.txt` | 82 | 4 |
| `nashorn/test/ExcludeList.txt` | 0 | 0 |

Roughly one entry in a hundred says anything about why it is there.

## How it works

The adapter already supports both halves of what is needed, so nothing in the
product changes:

- `ExcludeListFile` in `.runsettings` **replaces** the default exclude lists.
  Setting it to just `ProblemList.txt` keeps OpenJDK's own list (from the
  submodule, which we do not maintain) while dropping ours.
- `IncludeListFile` restricts the run to the tests named in it.

So an audit run is: exclude nothing of ours, include exactly the tests we
currently exclude, and see what happens.

## Two things that will bite you

**Both settings resolve per suite.** `AuditList.txt` is looked up under every
suite root. A suite with no `AuditList.txt` is not skipped — it runs its
*entire* set, and because `ExcludeListFile` was overridden it runs with no
exclusions at all. `New-AuditList.ps1` writes an empty list into the other
suites for exactly this reason.

**Excluded tests crash the host.** Some are excluded precisely because they
abort the process or call `System.exit`. A crash aborts the whole run and the
remaining tests in that batch report nothing. Keep batches small, and treat
anything reported as `NotRun` as unmeasured rather than passing.

## Running a batch

```powershell
./New-AuditList.ps1 -TestRoot ../bin/Release/net8.0 -Skip 0 -Take 200
dotnet test -f net8.0 --settings ../bin/Release/net8.0/Audit.runsettings `
    --logger trx ../bin/Release/net8.0/IKVM.OpenJDK.Tests.dll
./Get-AuditResult.ps1 -ResultsDirectory ../bin/Release/net8.0/TestResults `
    -AuditList ../bin/Release/net8.0/jdk/test/AuditList.txt |
    Export-Csv audit-0000.csv -NoTypeInformation
```

Then walk `Skip` forward by `Take` and repeat.

## Early sample

Four entries from `java/lang` were run as a smoke test of the harness:

| Test | Result |
|---|---|
| `java/lang/CharSequence/DefaultTest.java` | passes |
| `java/lang/Character/CheckProp.java` | passes |
| `java/lang/ClassLoader/Assert.java` | passes |
| `java/lang/Class/getEnclosingClass/EnclosingClassTest.java` | fails |

Three of four are stale. That is far too small a sample to extrapolate from,
and `java/lang` is likely healthier than the AWT and Swing areas, but it does
suggest the list is worth going through.

## Duplicate entries

Nine tests are listed twice in `jdk/test/ExcludeList.txt` (4746 lines, 4737
distinct tests). Where the two entries disagree, the later one appears to win:
`java/awt/Modal/ToFront/DialogToFrontNonModalTest.java` was listed
`generic-all` at line 510 and `linux-all,macosx-all` at 4614, and was observed
running and failing on Windows. That rules out both first-wins and union.

On that reading, three of the duplicates silently narrow an exclusion that was
meant to be broad:

| Test | Earlier | Later | Effect |
|---|---|---|---|
| `java/awt/KeyboardFocusmanager/TypeAhead/FreezeTest/FreezeTest.java` | `generic-all` | `macosx-all,windows-all` | runs on Linux |
| `java/net/URLConnection/6212146/test.sh` | `windows-i586` | `linux-all` | no longer excluded on `windows-i586` |
| `javax/swing/JPopupMenu/6544309/bug6544309.java` | `generic-all` | `linux-all,macosx-all` | runs on Windows |

`sun/nio/cs/Test4200310.sh` also disagrees (`windows-i586` then `generic-all`)
but in the widening direction, so it is harmless. The remaining five are exact
repeats: `HaricaCA.java` and three `sun/security/pkcs11` entries.

These are worth resolving regardless of the audit, since a test running where
someone intended it not to is a plausible source of the intermittent failures
tracked in #730.

## Recording the outcome

For entries that stay, the intent is a comment above each group naming the
reason, following the style already used further down `ExcludeList.txt`
(`# posix isn't supported on IKVM`, `# JFR is not supported`). Grouping by
cause keeps this readable at four thousand entries where per-line comments
would not.
