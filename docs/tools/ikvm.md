# ikvm Tool

The ikvm tool is a Java virtual machine implemented in .NET.

## Usage

```console
ikvm [ options ] <classname> [ args ... ]
ikvm [ options ] -jar <jarfile> [ args ... ]
```

<strong>options</strong>

Command-line options for the virtual machine.

<strong>classname</strong>

Fully-qualified name of a class containing the main method to execute. Do not include a path or a `.class` extension. Do not use this with the `-jar` option.

<strong>jarfile</strong>

The name of an executable `.jar` file to execute. Used only with the `-jar` option.

<strong>args</strong>

Command-line arguments passed to the main class.


## Options

| Source  | Description  |
|---|---|
| `-?\|-help`  | Displays command syntax and options for the tool. |
| `-version`  | Displays IKVM and runtime version. |
| `-showversion`  |  Display version and continue running. |
| `-cp\|-classpath` | &lt;directories and zip/jar files separated by ;&gt; Set search path for application classes and resources. |
| `-D<name>=<value>` | Set a Java system property.  |
| `-ea\|-enableassertions[:<packagename> [<packagename>...] \|:<classname>]` |  Set Java system property to enable assertions. |
| `-da\|-disableassertions[:<packagename> [<packagename>...] \|:<classname>]`  |  Set Java system property to disable assertions. |
| `-Xsave` |  Save the generated assembly (for debugging). |
| `-Xtime` |  Time the execution. |
|  `-Xtrace:<string>` | Displays all trace points with the given name.  |
| `-Xmethodtrace:<string>` |  Builds method trace into the specified output methods. |
| `-Xwait` | Keep process hanging around after exit.  |
| `-Xbreak` | Trigger a user defined breakpoint at startup.  |
| `-Xnoclassgc` |  Disable class garbage collection.  |
| `-Xnoglobbing` | Disable argument globbing.  |
| `-Xverify` |  Enable strict class file verification. |
| `-Xreference:<path>/<assembly>.dll` | Add a assembly (dll or exe) as reference to the classpath. This equals the C# code `Startup.addBootClassPathAssemby(Assembly.LoadFrom(name));`. |
| `-jar` |  Run a jar file. |
| `-Xdebug` |  Enable debugging.  |
| `-Xnoagent` | For compatibility with java.exe. This option will be ignored and a warning will be print to the console.  |
| `-XX:+AllowNonVirtualCalls` | Allow non-virtual calls.  |
| `-XX:<anything>`  | For compatibility with java.exe. This option will be ignored and a warning will be print to the console.  |
| `-Xms` |  For compatibility with java.exe. This option will be ignored and a warning will be print to the console. |
| `-Xmx` |  For compatibility with java.exe. This option will be ignored and a warning will be print to the console. |
| `-Xss` |  For compatibility with java.exe. This option will be ignored and a warning will be print to the console. |
| `-Xmixed` |  For compatibility with java.exe. This option will be ignored and a warning will be print to the console. |
| `-Xint` |  For compatibility with java.exe. This option will be ignored and a warning will be print to the console. |
| `-Xincgc` |  For compatibility with java.exe. This option will be ignored and a warning will be print to the console. |
| `-Xbatch` |  For compatibility with java.exe. This option will be ignored and a warning will be print to the console. |
| `-Xfuture` |  For compatibility with java.exe. This option will be ignored and a warning will be print to the console. |
| `-Xrs` |  For compatibility with java.exe. This option will be ignored and a warning will be print to the console. |
| `-Xcheck:jni` |  For compatibility with java.exe. This option will be ignored and a warning will be print to the console. |
| `-Xshare:off` |  For compatibility with java.exe. This option will be ignored and a warning will be print to the console. |
| `-Xshare:auto` |  For compatibility with java.exe. This option will be ignored and a warning will be print to the console. |
| `-Xshare:on` |  For compatibility with java.exe. This option will be ignored and a warning will be print to the console. |

## Related

- [Components](../components.md)
- [Tools](index.md)
- [Tutorial](../tutorial.md)