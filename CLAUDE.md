# Guidance for AI coding agents

## Attribution

Do not mention Claude, Anthropic, Copilot, or any other AI tool in commit messages, pull request
titles, or pull request bodies. No `Co-Authored-By` trailers naming an AI, no "generated with"
footers, no tool badges.

Using tools is fine and needs no disclosure. Whoever submits the work is responsible for it either
way, so there is nothing for the tool to take credit or blame for. Commits and pull requests are
attributed to their human author.

This file is the exception: documenting the rule here is fine. Discussing an AI tool in an issue or
pull request because it is the subject under discussion is also fine — the rule is about
attribution, not about the word.

## Build

See [CONTRIBUTING.md](CONTRIBUTING.md) for prerequisites, project layout, and the full build. A few
things that are easy to get wrong and are not covered there:

- A full build compiles the native libraries for every supported runtime, including macOS and Linux
  cross targets. To build only what a Windows host can, narrow the runtime set:

  ```
  dotnet build -p:EnabledRuntimes=win-x64 -p:EnabledImageRuntimes=win-x64 -p:EnabledImageBinRuntimes=win-x64 -p:EnabledToolRuntimes=win-x64
  ```

  The supported values are listed in `Directory.Build.props`.

- Running `IKVM.Tests` requires the native build to succeed, because the runtime needs a complete
  IKVM home (`$HOME/bin/jvm.dll` and the JDK image layout). Compiling `IKVM.Runtime` and `IKVM.Java`
  alone is not enough to run tests.

- Build logs are long and MSBuild builds in parallel, so a failing project's errors appear in the
  middle of the log rather than at the end. The tail of a failed build is frequently just warnings
  and `Process completed with exit code 1`. Search the whole log for `: error ` instead of reading
  the tail. CI also uploads an `msbuild.binlog` artifact.

## Native methods

A `native` method on a class in `IKVM.Java` binds to a static method on the matching
`IKVM.Java.Externs.<package>.<Class>` type in `IKVM.Runtime`, matched by name and signature. For an
instance method the receiver is prepended to the parameter list, and a `CallerID` parameter is
appended when the method has one.

The lookup happens in the importer, but an unmatched native does not fail the build — it produces a
stub that throws at runtime. A clean `ikvmc` run is not evidence that a new native bound correctly;
only executing it is.

## Tests

- `src/IKVM.Tests` — unit and interop tests.
- `src/IKVM.OpenJDK.Tests` — the OpenJDK jtreg suite, split across CI partitions.

jtreg tests that fail intermittently are listed in `src/IKVM.OpenJDK.Tests/jdk/test/ExcludeList.txt`,
one path per line with a platform scope (`generic-all`, or a comma-separated list such as
`linux-all,macosx-all,windows-all`). Prefer adding a genuinely flaky test there over re-running CI
repeatedly, and keep the scope as narrow as the evidence supports.
