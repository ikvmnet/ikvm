# IKVM

## Build

Project should open within Visual Studio. Project can also be built with MSBuild on a Windows host. Project cannot currently be built on a Linux host, nor with an exclusively .NET Core version of MSBuild.

The OpenJDK JDK8u source hierarchy and build results for Linux/x64 for is required. These build artifacts cannot be built on Windows, or modern Linux hosts. Instead, they must be built on a host with GCC 4.3 available. Debian Lenny is known to work acceptably. The CI/CD GitHub action can serve as a demonstration of this. Alternatively, the GitHub action's generated artifact can simply be extract into the appropriate spot in openjdk/build.

Both of these issues are known and considered unacceptable.

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