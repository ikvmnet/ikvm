# IKVM - Java Virtual Machine for .NET

## Build

Project should open within Visual Studio. Project can also be built with MSBuild on a Windows host. Project cannot currently be built on a Linux host, nor with an exclusively .NET Core version of MSBuild.

The OpenJDK JDK8u source hierarchy and build results for Linux/x64 is required. These build artifacts cannot be built on Windows, or modern Linux hosts. Instead, they must be built on a host with GCC 4.3 available. Debian Lenny is known to work acceptably. The CI/CD GitHub action can serve as a demonstration of this. 

The GitHub action's generated artifact can simply be extract into the appropriate spot in `openjdk/build` to avoid building it yourself. Navigate to the GitHub Actions, find the latest successfuly build for the branch you're concerned with, and download the `openjdk-build-linux-x86_64-normal-server-release` artifact. Extract this zip file into `openjdk/build`.

IKVM includes a native library named 'ikvm-native' which must be built for the JNI functionality to work. The solution includes `.vcxproj` files that build both the win-x64, linux-x86 and linux-x64 versions of these libraries. However, the linux-* version require WSL to be enabled on your development machine. Within this WSL distribution ensure you have installed the GCC toolset. For Debian based distributions, this should be as simple as typing `apt-get install g++`.

## Project

+ IKVM.sln
  Main solution file for the project.
+ IKVM.dist.msbuildproj
  MSBuild project file that builds the output artifacts, including the NuGet packages.
+ IKVM.Runtime
  The main executable core of IKVM. Provides services used by IKVM.Java.
+ IKVM.Java
  The OpenJDK distribution included with IKVM. This project is heavily customized to compile the OpenJDK Java source files and produce a .NET assembly from them.
+ IKVM.Image
  Outputs the files that make up the Java Runtime Image base. That is, the lib/ directory.
+ IKVM.Image.JRE
  Outputs the files that make up the Java Runtime Image JRE. That is, the contents of the bin/ directory typically associated with a JRE (java.exe, policytool.exe, etc).
+ IKVM.Image.JRE.runtime.*
  Outputs the runtime-specific files that make up the Java Runtime Image JRE.
+ IKVM.Image.JDK
  Outputs the files that make up the Java Runtime Image JDK. That is, the contents of the bin/ directory typically associated with a JDK (javac.exe, etc).
+ IKVM.Image.JDK.runtime.*
  Outputs the runtime-specific files that make up the Java Runtime Image JDK.
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
+ IKVM
  To untangle the ProjectReferences between the circular dependencies, this project generates the NuGet package output, including all of it's required dependencies, and the full version of the underlying IKVM assemblies.
+ ikvm-native-*
  Native vcxproj files for building 'ikvm-native'. This code facilitates the functionality of IKVM.Runtime.JNI.
+ IKVM.MSBuild
  Contains tasks and targets used by both the IKVM package and the IKVM.NET.Sdk package. Two divergent paths exist: Tasks and NoTasks. When doing in-tree builds, NoTasks is used.
+ IKVM.MSBuild.Tasks
  Source code for the task contained within IKVM.MSBuild.
+ IKVM.NET.Sdk
  MSBuild SDK package which provides support for building managed code form Java sources. To divergent paths exist: Tasks and NoTasks. When doing in-tree builds, NoTasks is used.
+ dist-*
  Outputs content files that describe the summation of various directories to be produced when doing a distribution build.
+ IKVM.Tools.Runner
  Various .NET libraries for executing the IKVM tools programatically. These are used by the MSBuild Tasks to launch ikvmc.exe and ikvmstub.exe.
+ IKVM.Java.Extensions
  Various extension methods and such for bridiging IKVM.Java with .NET patterns and practices.

## Versioning

IKVM uses the Semantic Versioning specification, with a unique twist. Since the project tracks compatibility with the Java SE specification, the major version is always the version of the JDK we claim to support. Otherwise major/breaking IKVM releases are denoted by an increment of the minor version. Other releases are denoted by an increment of the patch version.

Semantic Versioning is accomplished automatically by GitVersion in Mainline mode. The `main` branch functions as the release branch. Every commit to main results in an increment of the patch version and a release. The `develop` branch represents a prerelease staging area. Builds within the `develop` branch inherit the NEXT version number of the `main` branch, with a prerelease tag followed by the number of commit separating develop from the last release.

Increases in the major and minor version are accomplished manually by introducing a commit with a message containing a line such as `+semver: major` or `+semver: minor`. The process of creating a new major or minor release is simply to bump the version with the introduction of a commit message. The GitHub Actions should automatically generate the git tag and GitHub release, and publish the proper NuGet packages to the proper places.
