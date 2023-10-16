# ikvmstub Tool

The `ikvmstub` tool generates Java stubs from .NET assemblies.

## Usage

```console
ikvmstub [options] <assemblyNameOrPath>
```

<strong>assemblyNameOrPath</strong>

Name of an assembly. May be a fully-qualified path.

## Options

| <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Source<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  | <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Description<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  |
|---|---|
| No `<assemblyNameOrPath>` provided | Displays command syntax and options for the tool. |
| `-out:<outputfile>` | Specifies the name of the `outputfile`. |
| `-r:\|-reference:<filespec>` | Reference a class library DLL (.NET assembly). This option may be specified more than once to reference multiple DLLs. |
| `-japi` | Generate `.jar` suitable for comparison with japitools. |
| `-skiperror` | Continue when errors are encountered. |
| `-shared` |  Process all assemblies in shared group. |
| `-nostdlib`  | Do not reference shared reference assemblies. |
| `-lib:<dir>`  | Additional directories to search for references. This option may be specified more than once.  |
| `-namespace:<namespace>` | Only include types from specified `namespace`. This option may be specified more than once. |
| `-forwarders` |  Export forwarded types too. |
| `-parameters` |  Emit Java 8 classes with parameter names. |
| `-bootstrap` | Undocumented. |
| `-serialver` | Deprecated. Use `-japi` instead. |



## Notes

`ikvmstub` reads the specified assembly and generates a Java `.jar` file containing Java interfaces and stub classes. For more information about the generated stubs, see the [Tutorial](../tutorial.md).

The tool uses the following algorithm to locate the assembly:

1. First it attempts to load the assembly from the default load context of `ikvmstub`. For practical purposes, this usually means it searches the Global Assembly Cache.
2. If not found in the default load context, `ikvmstub` looks for the assembly at the indicated path (or the current directory, if no path is supplied).


## Examples

```console
ikvmstub mscorlib.dll
```

Generates `mscorlib.jar`, containing stubs for classes, interfaces, etc., defined in `mscorlib.dll`.

```console
ikvmstub c:\lib\mylib.dll
```

Generates `mylib.jar`, containing stubs for classes, interfaces, etc., defined in `c:\lib\mylib.dll`.

## Related

- [Components](../components.md)
- [Tools](index.md)
- [Tutorial](../tutorial.md)