# IKVM - Java Virtual Machine for .NET

[![Nuget](https://img.shields.io/nuget/dt/IKVM)](https://www.nuget.org/packages/IKVM)
[![GitHub](https://img.shields.io/github/license/ikvm-revived/ikvm)](https://github.com/ikvm-revived/ikvm/blob/develop/LICENSE.md)

## What is IKVM?

IKVM is an implementation of Java for the Microsoft .NET platform. It can be used to quickly and easily:

- Execute compiled Java code (bytecode) on .NET Framework or .NET Core
- Convert bytecode to a .NET assembly to directly access its API in a .NET project

These tasks can be done **without porting source code** to .NET.

### IKVM Components

* A Java virtual machine (JVM) implemented in .NET
* A .NET implementation of the Java class libraries
* A tool that translates Java bytecode (JAR files) to .NET IL (DLL or EXE files).
* Tools that enable Java and .NET interoperability

### Run Java Applications with .NET

1. **Statically:** By compiling a Java application into a .NET assembly using `<MavenReference>`, `<IkvmReference>` or `ikvmc`.
   - Libary assemblies can be referenced by any .NET application with a compatible target framework and platform. Types can be referenced by using the Java package name like a .NET namespace.
   - Executable assemblies can be launched by specifying the class containing the `main()` method to execute at runtime when building using `ikvmc`.
2. **Dynamically:** By running a Java application using the `ikvm` tool, which can be used as a direct replacement for `java.exe`. The Java bytecode is converted on-the-fly to CIL and executed.

## What IKVM is Not

- A converter utility to transform Java source code to C# source code
- A decompiler utitity to transform compiled Java bytecode to C# source code
- A tool that runs .NET code in Java - all IKVM conversions are Java > .NET

## Support

- .NET Framework 4.6.1 and higher
- .NET Core 3.1 and higher
- .NET 5 and higher
- Java SE 8

## Documentation

See the [tutorial](https://sourceforge.net/p/ikvm/wiki/Tutorial/) to get started or [IKVM.NET In Details](https://www.c-sharpcorner.com/UploadFile/abhijmk/ikvm-net-in-details/) for a more in-depth look.

## Installation

### NuGet

```console
PM> Install-Package IKVM
```

Or, to use `MavenReference`:

```console
PM> Install-Package IKVM.Maven.Sdk
```

### Tools

The tools are a available for download on the [Releases](https://github.com/ikvm-revived/ikvm/releases) page.

## Usage

IKVM supports integration with .NET SDK projects as well as low level tools for running compiled Java code directly or for advanced build scenarios. The 2 main entry points for integration with the .NET SDK are `IkvmReference` and `MavenReference`. .NET SDK projects can be built on the command line directly or using an IDE that supports them, such as recent versions [Visual Studio](https://visualstudio.microsoft.com/downloads/) or [JetBrains Rider](https://www.jetbrains.com/rider/).

### IkvmReference

IKVM includes build-time support for translating Java libraries to .NET assemblies. Install the `IKVM` package in a project that requires references to Java libraries. Use `IkvmReference` within an `ItemGroup` to indicate which Java libraries your project requires.

#### Example

```xml
<ItemGroup>
  <PackageReference Include="IKVM" Version="8.2.0" />
</ItemGroup>

<ItemGroup>
  <IkvmReference Include="..\..\ext\helloworld-2.0.jar" />
</ItemGroup>
```

The output assembly will be generated as part of your project's build process and a reference will automatically be added to your project so you can call APIs of the compiled `.jar` assembly. Additional metadata can be added to `IkvmReference` to customize the assembly that is generated.

#### Syntax

```xml
<ItemGroup>
  <IkvmReference Include="..\..\ext\helloworld-2.0.jar"
                 AssemblyName="MyAssembly"
                 AssemblyVersion="3.2.1.0"
                 FileVersion="3.0.0.0"
                 DisableAutoAssemblyName="true"
                 DisableAutoAssemblyVersion="true"
                 FallbackAssemblyName="MyAssemblyFallback"
                 FallbackAssemblyVersion="3.1.0.0"
                 KeyFile="MyKey.snk"
                 DelaySign="true"
                 Compile="SomeInternalDependency.jar;SomeOtherInternalDependency.jar"
                 Sources="MyClass.java;YourClass.java"
                 References="SomeExternalDependency.jar;SomeOtherExternalDependency.jar"
                 Aliases="MyAssemblyAlias;helloworld2_0"
                 Debug="true" />
</ItemGroup>
```


#### Attributes and Elements

The following values can be used as either an attribute or a nested element of `<IkvmReference>`.

| Attribute or Element  | Description  |
|---|---|
| `Include` (attribute only) | The identity of the `IkvmReference` item. The value can be one of: <ul><li>path to a JAR file</li><li>path to a directory</li><li>an otherwise unimportant name</li></ul> |
| `AssemblyName` | By default the `AssemblyName` is generated using the rules defined by the [`Automatic-Module-Name` specification](#automatic-module-name-specification). To override this, do so here. The value should not include a file extension, `.dll` will be appended automatically. |
| `AssemblyVersion` | By default the `AssemblyVersion` is generated using the rules defined by the [`Automatic-Module-Name` specification](#automatic-module-name-specification). To override this, do so here. |
| `AssemblyFileVersion` | By default the `AssemblyFileVersion` is generated using the rules defined by the [`Automatic-Module-Name` specification](#automatic-module-name-specification) or, if overridden, the same value as `AssemblyVersion`. To override this, do so here. |
| `DisableAutoAssemblyName` | If `true` disables detection of `AssemblyName`. |
| `DisableAutoAssemblyVersion` | If `true` disables detection of `AssemblyVersion`. |
| `FallbackAssemblyName` | If `AssemblyName` is not provided or cannot be calculated, use this value. |
| `FallbackAssemblyVersion` | If `AssemblyVersion` is not provided or cannot be calculated, use this value. |
| `KeyFile` | Specifies the filename containing the cryptographic key. When this option is used, the compiler inserts the public key from the specified file into the assembly manifest and then signs the final assembly with the private key. |
| `DelaySign` | This option causes the compiler to reserve space in the output file so that a digital signature can be added later. Use `DelaySign` if you only want to place the public key in the assembly. The `DelaySign` option has no effect unless used with `KeyFile`. |
| `Compile` | A semi-colon separated list of Java class path items to compile into the assembly. By default this value is the `Identity` of the item, if the identity of the item is an existing JAR file or directory (not yet supported). MSBuild globs are supported to reference multiple JAR or .class files. |
| `Sources` | A semi-colon separated list of Java source files to use during documentation generation. (not yet supported) |
| `References` | Optional semi-colon separated list of other `IkvmReference` identity values to specify as a reference to the current one. For example, if `foo.jar` depends on `bar.jar`, include both as `IkvmReference` items, but specify the identity of `bar.jar` on the `References` metadata of `foo.jar`. |
| `Debug` | Optional boolean indicating whether to generate debug symbols. By default this is determined based on the `<DebugType>` and `<DebugSymbols>`  properties of the project. Only full debug symbols are currently supported. |
| `Aliases` | A semi-colon separated list of aliases that can be used to reference the assembly in `References`. |
| All other metadata supported on the [`Reference`](https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items#reference) MSBuild item group definition. | |


`IkvmReference` is not transitive. Including it in one project and adding a dependency to that project from a second
project will not result in the same reference being available on the second project. Instead, add the reference to
each project.

For each project to resolve to the same resulting assembly ensure their settings are identical.


```xml
<ItemGroup>
  <IkvmReference Include="helloworld.jar">
    <AssemblyVersion>1.0.0.0</AssemblyVersion>
  </IkvmReference>
  <IkvmReference Include="helloworld-2.jar">
    <AssemblyName>helloworld-2</AssemblyName>
    <AssemblyVersion>2.0.0.0</AssemblyVersion>
    <References>helloworld.jar</References>
    <Aliases>helloworld2</Aliases>
  </IkvmReference>
</ItemGroup>
```

#### `Automatic-Module-Name` Specification

The `Automatic-Module-Name` is an attribute of the JAR manifest, which can be found in the `META-INF/MANIFEST.MF` file inside the JAR. See the [Java SE 9 JAR documentation](https://docs.oracle.com/javase/9/docs/specs/jar/jar.html#main_attributesmain-attributes) for more information.

> A JAR file is simply a `.zip` file with another extension, so it can be extracted using any zip file library or utility.

### MavenReference

See the [ikvm-maven Readme](https://github.com/ikvm-revived/ikvm-maven#readme) for usage instructions.
