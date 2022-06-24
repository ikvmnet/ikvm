# IKVM - Java Virtual Machine for .NET

[![Nuget](https://img.shields.io/nuget/dt/IKVM)](https://www.nuget.org/packages/IKVM)
[![GitHub](https://img.shields.io/github/license/ikvm-revived/ikvm)](https://github.com/ikvm-revived/ikvm/blob/develop/LICENSE.md)

IKVM.NET is an implementation of Java for the Microsoft .NET Framework and .NET Core.

IKVM.NET includes the following components:

* A Java virtual machine (JVM) implemented in .NET
* A .NET implementation of the Java class libraries
* A tool that translates Java bytecode (JAR files) to .NET IL (DLLs or EXE files).
* Tools that enable Java and .NET interoperability
* With IKVM.NET you can run compiled Java code (bytecode) directly on Microsoft .NET Framework or .NET Core. The bytecode is converted on the fly to CIL and executed.

## Documentation

See the [tutorial](https://sourceforge.net/p/ikvm/wiki/Tutorial/) to get started or [IKVM.NET In Details](https://www.c-sharpcorner.com/UploadFile/abhijmk/ikvm-net-in-details/) for a more in-depth look.

## Support

- .NET Framework 4.6.1 and higher
- .NET Core 3.1 and higher
- .NET 5 and higher
- Java SE 8

## IkvmReference

IKVM includes build-time support for translating Java libraries to .NET assemblies. Install the `IKVM` package in a project that wants to reference Java libraries. Use the `IkvmReference` `ItemGroup` to indicate which Java libraries your project required.

Example:

```
    <ItemGroup>
        <IkvmReference Include="..\..\ext\helloworld-2.0.jar" />
    </ItemGroup>
```

The output assembly will be generated as part of your project's build process. Additional metadata can be added to `IkvmReference` to customize the generated assembly.

+ `Identity`: The identity of the `IkvmReference` item can be either a) path to a JAR file b) path to a directory or c)
an otherwise unimportant name.
+ `AssemblyName`: By default the `AssemblyName` is generated using the rules defined by the `Automatic-Module-Name`
specification. To override this, do so here.
+ `AssemblyVersion`: By default the `AssemblyVersion` is generated using the rules defined by the `Automatic-Module-Name`
specification. To override this, do so here.
+ `DisableAutoAssemblyName`: If `true` disables detection of `AssemblyName`.
+ `DisableAutoAssemblyVersion`: If `true` disables detection of `AssemblyVersion`.
+ `FallbackAssemblyName`: If `AssemblyName` is not provided or cannot be calculated, use this value.
+ `FallbackAssemblyVersion`: If `AssemblyVersion` is not provided or cannot be calculated, use this value.
+ `Compile`: Optional semi-colon separated list of Java class path items to compile into the assembly. By default this
value is the `Identity` of the item, if the identity of the item is an existing JAR file or directory (not yet supported). MSBuild globs
are supported to reference multiple JAR or .class files.
+ `Sources`: Optional semi-colon separated list of Java source files to use during documentation generation. (not yet supported)
+ `References`: Optional semi-colon separated list of other `IkvmReference` identity values to specify as a reference
to the current one. For instance, if `foo.jar` depends on `bar.jar`, include both as `IkvmReference` items, but specify
the identity of `bar.jar` on the `References` metadata of `foo.jar`.
+ `Debug`: Optional boolean indicating whether to generate debug symbols (non-portable). By default this is determined
based on the overall setting of the project.
+ All other metadata supported on the `Reference` MSBuild item group definition.

`IkvmReference` is not transitive. Including it in one project and adding a dependency to that project from a second
project will not result in the same reference being available on the second project. Instead add the reference to
each project.

For each project to resolve to the same resulting assembly ensure their settings are identical.


```
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

