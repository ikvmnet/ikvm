# ikvmc Tool

> The `ikvmc` tool is considered an advanced option and it is recommended that new projects use the `IkvmReference` for the .NET SDK for most use cases.

The `ikvmc` tool converts Java bytecode to .NET DLL and EXE files.

- [Usage](#usage)
- [Options](#options)
- [Notes](#notes)
- [Examples](#examples)
  - [Single jar file](#single-jar-file)
  - [jar file and class files to a single exe](#jar-file-and-class-files-to-a-single-exe)
  - [jar file to dll and class files to an exe](#jar-file-to-dll-and-class-files-to-an-exe)
- [Warnings](#warnings)

## Usage

```console
ikvmc [ options ] <classOrJarfile> [ classOrJarfile... ]
```

<strong>options</strong>

See below.

<strong>classOrJarfile</strong>

Name of a Java `.class` or `.jar file`. May contain wildcards (`*.class`).

## Options

### Command Help

| <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Source<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  | <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Description<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  |
|---|---|
| `-?\|-help`  | Displays command syntax and options for the tool. |

### Output Files

| <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Source<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  | <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Description<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  |
|---|---|
| `-out:<outputfile>` | Specifies the name of the `outputfile`. Should have a .dll extension (if -target is library) or an .exe extension (if -target is exe or winexe). In most cases, if you omit this option, `ikvmc` will choose an output name based on the -target and the name of the input files. However, if you specify input files using wildcards, you must use this option to specify the output file. |
| `-assembly:<assembly-name>` | Specifies the name of the generated assembly. If omitted, the assembly name is (usually) the output filename. |
| `-version:<M.m.b.r>` | Specifies the output assembly version. |
| `-fileversion:<version>` | Specifies the output assembly file version. |
| `-target:<target-type>` | Specifies whether to generate an .exe or .dll. `target-type` is one of <ul><li>`exe` - generates an executable that runs in a Windows command window</li><li>`winexe` - generates an .exe for GUI applications</li><li>`library` - generates a .dll</li><li>`module` - generates a .netmodule</li></ul> On Linux, there is no difference between exe and winexe. |
| `-platform:<platform>` | Limit which platforms this code can run on:<ul><li>`x86` - 32 bit</li><li>`Itanium`</li><li>`x64`</li><li>`anycpu`</li></ul> The default is `anycpu`. This can be helpful if your Java code use a dll via JNI. For example a 32 bit dll should also run on a 64 bit system as 32 bit application. |
| `-keyfile:<keyfilename>` | Uses `keyfilename` to sign the resulting assembly. |
| `-key<keycontainer>` | Use `keycontainer` to sign the assembly. |
| `-delaysign` | Delay-sign the assembly. |

### Input Files


| <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Source<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  | <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Description<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  |
|---|---|
| `-r:\|-reference:<library-filespec>` | Add a reference to a class library DLL (.NET assembly). When building Java projects (`.jar` or `.class` files), this can be used to add dependent projects that had previously been compiled.  The dependency must be compiled first. This option can appear more than once to reference multiple libraries. |
| `-recurse:<filespec>` | Processes all files matching `filespec` in and under the directory specified by `filespec`. Example: `-recurse:*.class` |
| `-exclude:<filename>` | `filename` is a file containing a list of class names to exclude. The file may contain a single .NET-compatible regular expression per line. If the line starts with a `\`, it will be ignored as a comment. |

### Resources

| <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Source<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  | <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Description<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  |
|---|---|
| `-win32icon:<file>` | Embed specified icon in output. |
| `-win32manifest:<file>` | Specify a Win32 manifest file (.xml). |
| `-resource:<name>=<path>` | Includes path as a Java resource named `name`. |
| `-externalresource:<name>=<path>` | Reference file as Java resource. |
| `-compressresources` | Compress resources. |

### Code Generation

| <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Source<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  | <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Description<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  |
|---|---|
| `-debug` | Generates debugging information in the output. Note that this is only helpful if the `.class` files contain debug information (compiled with the `javac -g` option). This option also produces somewhat less efficient CIL code. |
| `-noautoserialization` | Disable automatic .NET serialization support. |
| `-noglobbing` | Don't glob the arguments passed to `-main`. |
| `-nojni` | Do not generate JNI stub for native methods. |
| `-opt:fields` | Remove unused private fields. |
| `-removeassertions` | Remove all assert statements. |
| `-strictfinalfieldsemantics` | Don't allow final fields to be modified outside of initializer methods. |

### Errors and Warnings

| <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Source<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  | <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Description<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  |
|---|---|
| `-nowarn:<warning[:key]>` | Suppress specified warnings. |
| `-warnaserror` | Treat all warnings as errors. |
| `-warnaserror:<warning[:key]>` | Treat specified warnings as errors. |
| `-writeSuppressWarningsFile:<file>` | Write response file with `-nowarn:<warning[:key]>` options to suppress all encountered warnings. |


### Miscellaneous

| <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Source<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  | <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Description<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  |
|---|---|
| `@<filename>` | Read more options from the provided file. |
| `-nologo` | Suppress compiler copyright message. |

### Advanced

| <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Source<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  | <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Description<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  |
|---|---|
| `-main:<classname>` | Specifies the name of the class containing the main method. If omitted and the -target is exe or winexe, ikvmc searches for a qualifying main method and reports if it finds one. |
| `-srcpath:<path>` | Specifies the location of source code. Use with `-debug`. The package of the class is appended to the specified `path` to locate the source code for the class. |
| `-apartment:<threadstate>` | Apply a thread apartment state attribute to `-main` based on the selected option:<ul><li>`sta` - Apply `STAThreadAttribute`</li><li>`mta` - Apply `MTAThreadAttribute`</li><li>`none` - Apply no attribute</li></ul> The default option is `sta`. |
| `-D<name>=<value>` | Set a Java system property (at runtime). |
| `-ea\|-enableassertions[:[<packagename>\|<classname>]]` |  Set Java system property to enable assertions. A class name or package name can be specified to enable assertions for a specific class or package. |
| `-da\|-disableassertions[:[<packagename>\|<classname>]]` |  Set Java system property to disable assertions. A class name or package name can be specified to disable assertions for a specific class or package. |
| `-nostacktraceinfo` | Don't create metadata to emit rich stack traces. |
| `-Xtrace:<name>` | Displays all trace points with `name`. |
| `-Xmethodtrace:<methodname>` | Builds method trace into the specified output method. |
| `-privatepackage:<prefix>` | Mark all classes with a package name starting with `prefix` as internal to the assembly. |
| `-publicpackage:<prefix>` | Mark all classes with a package name starting with `prefix` as public to the assembly. |
| `-time` | Display timing statistics. |
| `-classloader:<class>` | Set custom class loader class for assembly. |
| `-sharedclassloader` | All targets below this level share a common class loader. |
| `-baseaddress:<address>` | Base address for the library to be built. |
| `-filealign:<n>` | Specify the alignment used for output file. |
| `-nopeercrossreference` | Do not automatically cross reference all peers. |
| `-nostdlib` | Do not reference shared reference assemblies. |
| `-lib:<dir>` | Additional directories to search for references. This option may be specified more than once. |
| `-highentropyva` | Enable high entropy ASLR. |
| `-static` | Disable dynamic binding. |
| `-assemblyattributes:<file>` | Read assembly custom attributes from specified class file. |
| `-proxy:<string>` | Undocumented. |
| `-nojarstubs` | Undocumented temporary option to mitigate risk. |
| `-w4` | Undocumented option to compile core class libraries with, to disable MethodParameter attribute. |
| `-strictfinalfieldsemantics` | Undocumented. For internal use. |
| `-remap` | Undocumented. For internal use. |
| `-runtime` | Specifies the `IKVM.Runtime.dll` library to use. Not fully functional. |

## Notes

The `ikvmc` tool generates .NET assemblies from Java class files and jar files. It converts the Java bytecode in the input files to .NET CIL. Use it to produce

- .NET executables (`-target:exe` or `-target:winexe`)
- .NET libraries (`-target:library`)
- .NET modules (`-target:module`)

Java applications often consist of a collection of `.jar` files. `ikvmc` can process several input `.jar` files (and class files) and produce a single .NET executable or library. For example, an application consisting of `main.jar`, `lib1.jar`, and `lib2.jar` can be converted to a single `main.exe`.

When processing multiple input `.jar` files that contain duplicate classes / resources, `ikvmc` will use the first class / resource it encounters, and ignore duplicates encountered in `.jar`s that appear later on the command line. It will produce a warning in this case. Thus, the order of `.jar` files can be significant.

#### Note

- When converting a Java application with `ikvmc`, for best results, list the `.jar`s on the `ikvmc` command line in the same order that they appear in the Java application's classpath.
  See also the concepts of [ClassLoader](../class-loader.md).
- [ikvmc messages](ikvmc-messages.md)


## Examples

### Single jar file

```console
ikvmc myProg.jar
```

Scans `myprog.jar` for a `main()` method. If found, an `.exe` is produced; otherwise, a `.dll` is generated.

### jar file and class files to a single exe

```console
ikvmc -out:myapp.exe -main:org.anywhere.Main -recurse:bin\*.class lib\mylib.jar
```

Processes all `.class` files in and under the bin directory, and `mylib.jar` in the lib directory. Generates an executable named `myapp.exe` using the class `org.anywhere.Main` as the `main()` method.

### jar file to dll and class files to an exe

```console
ikvmc mylib.jar
ikvmc -out:myapp.exe -main:org.anywhere.Main -recurse:bin\*.class -reference:mylib.dll
```

First it generates a library `mylib.dll` from `mylib.jar`. Then it generates an executable named `myapp.exe` from all `.class` files in and under the bin directory using the class `org.anywhere.Main` as the main method. This make duplicate file names possible which are used from the Java service API.

## Warnings


| Warning Number  | Message  | Cause |
|---|---|---|
| IKVMC0100  | class "com.company.mypackage.Xyz" not found (in mylibrary.dll) | This can be a follow error of IKVMC0105. |
| IKVMC0105 | unable to compile class "com.company.mypackage.Xyz" (missing class "com.company.mypackage.Abc") | The class com.company.mypackage.Abc was not found. If this is an optional external class then you can ignore it. |
| IKVMC0109  | skipping class: "javax.activation.ActivationDataFlavor" (class is already available in referenced assembly "IKVM.OpenJDK.Beans ...) | This class was only needed for older Java versions. In the current version it is already included. <ul><li>You have the class added more as once to the compiler</li><li>You can ignore it if you does not have a special version.</li></ul> |

## Related

- [ClassLoader](../class-loader.md)
- [Components](../components.md)
- [Convert a .jar file to a .dll](../convert-a-jar-file-to-a-dll.md)
- [Tools](index.md)
- [Tutorial](../tutorial.md)
