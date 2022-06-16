# IKVM - Java Virtual Machine for .NET

[![Nuget](https://img.shields.io/nuget/dt/IKVM)](https://www.nuget.org/packages/IKVM)
[![GitHub](https://img.shields.io/github/license/ikvm-revived/ikvm)](https://github.com/ikvm-revived/ikvm/blob/master/LICENSE.md)

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

> **Warning**
> IkvmReference fails to properly build assemblies under a TFM > `netcoreapp3.1`. Until this is resolved, you can use `netcoreapp3.1` to access the Java assembly in a library that you reference from other projects.

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

## Build

Project should open within Visual Studio. Project can also be built with MSBuild on a Windows host. Project cannot currently be built on a Linux host, nor with an exclusively .NET Core version of MSBuild.

The OpenJDK JDK8u source hierarchy and build results for Linux/x64 is required. These build artifacts cannot be built on Windows, or modern Linux hosts. Instead, they must be built on a host with GCC 4.3 available. Debian Lenny is known to work acceptably. The CI/CD GitHub action can serve as a demonstration of this. 

The GitHub action's generated artifact can simply be extract into the appropriate spot in `openjdk/build` to avoid building it yourself. Navigate to the GitHub Actions, find the latest successfuly build for the branch you're concerned with, and download the `openjdk-build-linux-x86_64-normal-server-release` artifact. Extract this zip file into `openjdk/build`.

IKVM includes a native library named 'ikvm-native' which must be built for the JNI functionality to work. The solution includes `.vcxproj` files that build both the win-x86, win-x64, linux-x86 and linux-x64 versions of these libraries. However, the linux-* version require WSL to be enabled on your development machine. Within this WSL distribution ensure you have installed the GCC toolset. For Debian based distributions, this should be as simple as typing `apt-get install g++`.

## Project

+ IKVM.sln
  Main solution file for the project.
+ IKVM.artifacts.msbuildproj
  MSBuild project file that builds the output artifacts, including the NuGet packages.
+ IKVM.Runtime
  The main executable core of IKVM. Provides services used by IKVM.Java.
+ IKVM.Java
  The OpenJDK distribution included with IKVM. This project is heavily customized to compile the OpenJDK Java source files and produce a .NET assembly from them.
+ ikvm
  `java` compatibility executable. Launches a JVM. Can be used to execute Java applications with entry points.
+ ikvmc
  `ikvmc` executable. Transforms Java class files or JAR files into .NET libraries or executables.
+ ikvmstub
  `ikvmstub` executable. Generates Java JAR files for .NET assemblies. When building Java code that depends on .NET code, these stubs can be used as references.
+ IKVM.Tests
  Various unit tests against IKVM functionality.
+ IKVM.Runtime-ref
  "Reference" version of the IKVM.Runtime project. Due to a circular dependency between IKVM.Java and IKVM.Runtime, IKVM.Java must build against a partial copy of IKVM.Runtime.
+ IKVM.Java-ref
  "Reference" version of the IKVM.Java project. Due to the circular dependency between IKVM.Java and IKVM.Runtime, IKVM.Runtime must build against a partial copy of IKVM.Java.
+ IKVM-pkg
  To untangle the ProjectReferences between the circular dependencies, this project generates the NuGet package output, including all of it's required dependencies, and the full version of the underlying IKVM assemblies.

## Versioning

IKVM uses the Semantic Versioning specification, with a unique twist. Since the project tracks compatibility with the Java SE specification, the major version is always the version of the JDK we claim to support. Otherwise major/breaking IKVM releases are denoted by an increment of the minor version. Other releases are denoted by an increment of the patch version.

Semantic Versioning is accomplished automatically by GitVersion in Mainline mode. The `main` branch functions as the release branch. Every commit to main results in an increment of the patch version and a release. The `develop` branch represents a prerelease staging area. Builds within the `develop` branch inherit the NEXT version number of the `main` branch, with a prerelease tag followed by the number of commit separating develop from the last release.

Increases in the major and minor version are accomplished manually by introducing a commit with a message containing a line such as `+semver: major` or `+semver: minor`. The process of creating a new major or minor release is simply to bump the version with the introduction of a commit message. The GitHub Actions should automatically generate the git tag and GitHub release, and publish the proper NuGet packages to the proper places.
