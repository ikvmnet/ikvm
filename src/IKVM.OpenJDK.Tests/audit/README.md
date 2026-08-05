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

## Shell tests do not survive a local run

Results for `.sh` entries from a developer machine are not trustworthy. The
tests run under WSL and are handed `TESTJAVA` pointing at the Windows image,
so they try to exec `ikvm/win-x64/bin/java` from inside Linux and die:

```
/mnt/d/.../NarrowNamesTest.sh: 37: /mnt/d/.../ikvm/win-x64/bin/java: not found
```

That is the harness, not IKVM. An exit code of 127 anywhere in the results
usually means the same thing. CI sets WSL up properly, so these have to be
measured there.

A local pass is still a pass — the risk is only false failures — so `.sh`
entries should be left alone until CI numbers exist for them.

## javac also dies under local concurrency

The other way a local run invents failures:

```
Error. unexpected exit code from javac: -1073741502
```

That is `0xC0000142`, `STATUS_DLL_INIT_FAILED` — the process never started,
so the test never ran. It appears under the concurrency jtreg uses by default
and has nothing to do with IKVM. One `java/rmi` batch of 100 produced 18 of
them.

Worth grepping the jtr files for before reading any failure list:

```powershell
Get-ChildItem TestResults -Recurse -Filter *.jtr |
    Select-String "unexpected exit code from javac: -107374" |
    Select-Object -ExpandProperty Path -Unique
```

Between this and the shell tests, the rule for local runs is simply: act on
passes, never on failures.

## Graphical tests take over the desktop

`java/awt`, `javax/swing`, `java/beans`, `sun/java2d` and `javax/imageio` open
real windows, grab focus and move the pointer. On a developer machine that
makes the desktop unusable for as long as the batch runs, which for 110
entries is tens of minutes.

Do not run those areas on a machine someone is using. They are fine in CI,
where nobody is looking at the screen, and that is where they belong.

The areas that can be swept locally without taking the display over:

| Area | Remaining |
|---|---|
| `sun/security` | 149 |
| `java/nio` | 70 |
| `sun/tools` | 54 |
| `security/infra` | 54 |
| `java/net` | 51 |
| `jdk/lambda` | 49 |
| `java/security` | 49 |
| `java/rmi` | 28 |

Plus what is left of `java/lang`, `java/util` and `com/sun` once their
structural blocks are set aside.

## RMI tests leave processes behind

Some `java/rmi` tests start activation daemons that outlive the run. A stray
`java.exe` under the test tree then holds a log file open, and the next
batch's `rm -rf TestResults` fails with "Device or resource busy". If that
`rm` is chained with `&&` the whole run silently does not happen.

Check for leftovers before starting a batch:

```powershell
Get-CimInstance Win32_Process -Filter "Name='java.exe'" |
    Where-Object { $_.ExecutablePath -like '*IKVM.OpenJDK.Tests*' }
```


| Kind | Count | Auditable locally |
|---|---|---|
| `.java` | 4293 | yes |
| `.sh` | 340 | no, needs CI |
| `.html` | 66 | needs a display |
| `.java#testcase` | 47 | yes, all CA path tests |

## Results so far

`java/lang`, four entries, smoke test of the harness:

| Test | Result |
|---|---|
| `java/lang/CharSequence/DefaultTest.java` | passes |
| `java/lang/Character/CheckProp.java` | passes |
| `java/lang/ClassLoader/Assert.java` | passes |
| `java/lang/Class/getEnclosingClass/EnclosingClassTest.java` | fails |

`java/util`, first 60 entries: **54 pass, 6 fail**. Of the six, three are `.sh`
tests and `Formatter/Basic` exited 127, so those four are unmeasured rather
than failing. Two look real:

| Test | Failure |
|---|---|
| `java/util/Arrays/TimSortStackSize2.java` | unexpected exit, code 1 |
| `java/util/Calendar/CldrFormatNamesTest.java` | `RuntimeException: test failed` |

`java/lang`, first 100 entries: **32 pass, 66 fail**. Eleven of the failures
are `.sh` and so unmeasured, leaving 55 real failures.

Nothing here extrapolates. `java/util` came out 90% stale and `java/lang`
33%, from adjacent areas of the same file. Every area has to be measured.

### Why the java/lang failures fail

Thirty-eight of the 55, better than two thirds, are `java/lang/instrument`,
and they all fail the same way, at compilation rather than at run time:

```
ATransformerManagementTestCase.java:26: error: package java.lang.instrument does not exist
```

The runtime image does not ship `java.lang.instrument`, so the agent and
transformer tests cannot compile, never mind run. That is structural and not
going to change on its own. 58 entries in the list are under
`java/lang/instrument`, out of 162 such tests in the tree, so the exclusion
is not even complete — the rest presumably fail the same way and were never
added.

This is the shape the finished list should have: one comment naming a cause,
covering the group beneath it, instead of 58 unexplained lines.

The remaining 17 are scattered, nine of them under `java/lang/annotation`,
and have not been looked at individually yet.

### Everything measured so far

| Area | Measured | Passed | Kept |
|---|---|---|---|
| `java/lang` | 100 | 32 | 66 |
| `java/util` (three batches) | 260 | 165 | 95 |
| `java/io`, `nio`, `net`, `security`, `math` | 98 | 85 | 13 |
| `java/rmi`, `java/text` | 100 | 73 | 27 |

The pass rate runs from 32% to 90% depending on the area, which is the whole
argument for measuring rather than estimating. The two areas that came out
worst, `java/lang` and `java/rmi`, did so for reasons that are not really
about staleness: `java/lang` is dominated by the `instrument` package that
does not exist, and 18 of the `java/rmi` failures were the javac startup
crash above rather than test results.

### Causes found so far

| Cause | Entries | How it shows |
|---|---|---|
| `java.lang.instrument` absent | 58 | `package java.lang.instrument does not exist` at compile |
| `-Xbootclasspath/a` unimplemented | 59 | `Unrecognized option: -Xbootclasspath/a` |
| CLDR vs COMPAT locale data | 3 so far | display names differ from what the test expects |

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
