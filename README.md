# IKVM - Java Virtual Machine for .NET

[![Nuget](https://img.shields.io/nuget/dt/IKVM)](https://www.nuget.org/packages/IKVM)
[![Discord](https://img.shields.io/badge/Chat-on%20Discord-brightgreen)](https://discord.gg/MpzNd5Tk8P)


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
* A full JRE/JDK 8 runtime image.

### Run Java Applications with .NET

1. **Statically:** By compiling a Java application into a .NET assembly using `<MavenReference>`, `<IkvmReference>` or `ikvmc`.
   - Library assemblies can be referenced by any .NET application with a compatible target framework and platform. Types can be referenced by using the Java package name like a .NET namespace.
   - Executable assemblies can be launched by specifying the class containing the `main()` method to execute at runtime when building using `ikvmc`.
2. **Dynamically:** By running a Java application using the `java` executable inside of the JDK Runtime Image. The Java bytecode is converted on-the-fly to CIL and executed. The experience should be identical to a normal JDK.

## What IKVM is Not

- A converter utility to transform Java source code to C# source code
- A decompiler utitity to transform compiled Java bytecode to C# source code
- A tool that runs .NET code in Java - all IKVM conversions are Java > .NET

## Support

- .NET Framework 4.7.2 and higher (SDK-style projects)
- .NET 6 and higher
- Java SE 8
- Windows x86/x64/ARM/ARM64
- Linux x64/ARM/ARM64 (GLIBC and MUSL)
- Mac OS X x64/arm64
 
Various differences exist between support for all of our different artifacts. For instance, there are some platforms we cannot distribute images for and some platforms we cannot distribute tools for.

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

The tools are available for download on the [Releases](https://github.com/ikvmnet/ikvm/releases) page.

### Runtime Images

Both a JRE and JDK runtime image are available. These images are standard JRE or JDK directory structures containing all of the standard tools: javac, jdeps, policytool, keytool, etc. Some Java libraries may require either a JRE or JDK, and if so, the `IKVM.Image.JRE` or `IKVM.Image.JDK` package should be added to your project.

```console
PM> Install-Package IKVM.Image.JRE
PM> Install-Package IKVM.Image.JDK
```

A standalone JDK distributable is available for download on the [Releases](https://github.com/ikvmnet/ikvm/releases) page. This directory structure should suffice as a `JAVA_HOME` path for standard Java applications.

## Usage

IKVM supports integration with .NET SDK-style projects as well as low level tools for running compiled Java code directly or for advanced build scenarios. The 2 main entry points for integration with the .NET SDK-style projects are `IkvmReference` and `MavenReference`. .NET SDK-style projects can be built on the command line directly or using an IDE that supports them, such as recent versions [Visual Studio](https://visualstudio.microsoft.com/downloads/) or [JetBrains Rider](https://www.jetbrains.com/rider/).

### IkvmReference

IKVM includes build-time support for translating Java libraries to .NET assemblies. Install the `IKVM` package in a project that requires references to Java libraries. Use `IkvmReference` within an `ItemGroup` to indicate which Java libraries your project requires.

#### Example

```xml
<ItemGroup>
  <PackageReference Include="IKVM" Version="Version" />
</ItemGroup>

<ItemGroup>
  <IkvmReference Include="..\..\ext\helloworld\helloworld-2.0.jar" />
</ItemGroup>
```

The output assembly will be generated as part of your project's build process and a reference will automatically be added to your project so you can call APIs of the compiled `.jar` assembly. Additional metadata can be added to `IkvmReference` to customize the assembly that is generated.

#### Syntax

```xml
<ItemGroup>
   <IkvmReference Include="..\..\ext\helloworld\helloworld-2.0.jar">
      <AssemblyName>MyAssembly</AssemblyName>
      <AssemblyVersion>3.2.1.0</AssemblyVersion>
      <AssemblyFileVersion>3.0.0.0</AssemblyFileVersion>
      <DisableAutoAssemblyName>true</DisableAutoAssemblyName>
      <DisableAutoAssemblyVersion>true</DisableAutoAssemblyVersion>
      <FallbackAssemblyName>MyAssemblyFallback</FallbackAssemblyName>
      <FallbackAssemblyVersion>3.1.0.0</FallbackAssemblyVersion>
      <KeyFile>MyKey.snk</KeyFile>
      <DelaySign>true</DelaySign>
      <Compile>SomeInternalDependency.jar;SomeOtherInternalDependency.jar</Compile>
      <Sources>MyClass.java;YourClass.java</Sources>
      <References>SomeExternalDependency.jar;SomeOtherExternalDependency.jar</References>
      <Aliases>MyAssemblyAlias;helloworld2_0</Aliases>
      <Debug>portable</Debug>
   </IkvmReference>
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
| `Debug` | Optional value indicating how to generate debug symbols. By default this is determined based on the `<DebugType>` properties of the project. Only full debug symbols are currently supported. |
| `Aliases` | A semi-colon separated list of aliases that can be used to reference the assembly in `References`. |
| `ClassLoader` | A fully qualified classs name of the custom ClassLoader implementation to use as a delegation parent. Examples include `ikvm.runtime.AppDomainAssemblyClassLoader` and `ikvm.runtime.ClassPathAssemblyClassLoader`. |
| All other metadata supported on the [`Reference`](https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items#reference) MSBuild item group definition. | |


`IkvmReference` is not transitive. Including it in one project and adding a dependency to that project from a second
project will not result in the same reference being available on the second project. Instead, add the reference to
each project.

For each project to resolve to the same resulting assembly ensure their settings are identical.

Multiple `IkvmReference` entries can be configured to include each other as references.

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

The `Automatic-Module-Name` is either a specified attribute of the JAR manifest, which can be found in the `META-INF/MANIFEST.MF` file inside the JAR, or a generated value based on the name of the JAR file. See the [documentation](https://docs.oracle.com/javase/9/docs/api/java/lang/module/ModuleFinder.html#automatic-modules) for more information.

### MavenReference

See the [ikvm-maven Readme](https://github.com/ikvmnet/ikvm-maven#readme) for usage instructions.

### Notice To Project Owners

The IKVM project recommends that people do not redistribute FOSS Java libraries compiled with IKVM over public systems such as NuGet.org, unless you are the original owner of that software and have a compelling reason.

Creating copies of FOSS Java libraries and publishing them to distribution mechanisms such as NuGet.org creates eco-system confusion and dependency conflicts downstream. We provide a system so that .NET users of Java libraries can reference those libraries directly out of the standard Java ecosystem mechanisms: Maven Central, etc though IKVM.Maven. Remember, very few libraries exist in a vacuum. Libraries often depend on dozens of other libraries. Two unrelated Java libraries often depend on the same underlying Java library. A complex method of dependency conflict resolution and version unification has to be involved in resolving this hierarchy for any individual downstream project. You are likely going to be introducing duplicate classes into the users of your versions, or causing your users to depend upon the wrong version of other libraries.

In addition to the above, we presently do not guarentee that API between statically compiled assemblies and the IKVM.Java and IKVM.Runtime libraries remains stable. A statically compiled assembly built against one version of IKVM is not guarenteed to run against a higher version of IKVM, even at the patch level. We would like to advertise better support for this in the future, but at present there is a lot of work planned for the IKVM.Runtime and IKVM.Java APIs that we don't want to inhibit. Since we also provide build tools to make the process of recompiling assemblies from the build machine easy, if recommendations are followed, this should be of limited impact.

There are exceptions to this advice, such as the library not being published to Maven. In that case, guidance would be to advocate that the original Java library in fact be published to Maven, or do the work yourself, as that is the appropriate place for Java libraries.

We cannot force you to conform to this advice. But for the health of the ecosystem, we urge you to take it under consideration yourself.
