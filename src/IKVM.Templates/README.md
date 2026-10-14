# IKVM Project Templates

Project templates for Java projects compiled to .NET by `IKVM.NET.Sdk`. Projects created from these templates
use the version of `IKVM.NET.Sdk` released with the templates.

## Installation

```
dotnet new install IKVM.Templates
```

## Templates

| Template | Short name | Frameworks |
|---|---|---|
| Java Console App | `ikvm` | `net10.0` (default), `net8.0` |
| Java Console App (.NET Framework) | `ikvm-netfx` | `net472`, `net48` (default), `net481` |
| Java Class Library | `ikvmlib` | `net10.0` (default), `net8.0` |
| Java Class Library (.NET Framework) | `ikvmlib-netfx` | `net472`, `net48` (default), `net481` |

```
dotnet new ikvm -n MyApp
dotnet new ikvmlib -n MyLibrary -f net8.0
```

Java sources go under `src/main/java`, in a package named after the project.
