# IKVM - Java Virtual Machine for .NET

## Build

The project can be opened in Visual Studio, or it can be be built with MSBuild on a Windows host. The project cannot currently be built on a Linux host, nor with an exclusively .NET Core version of MSBuild.

Prerequisites for building the project:
* A clone of the IKVM repository which includes submodules (e.g. `git clone --recurse-submodules https://github.com/ikvmnet/ikvm.git`)
* Visual Studio configured for: .NET desktop development, Desktop development with C++ (including C++ Clang tools for Windows), Linux and Embedded Development with C++, and Windows SDK with the appropriate libraries for x86, AMD64 and ARM64 development.
* A JDK 8 installation. The `JAVA_HOME` environment variable needs to be point to the JDK 8 directory, or the version of `javac` available on the path needs to be from JDK 8 (You can download a suitable JDK from [Adoptium](https://adoptium.net/))
* SDK Toolkits: These are located in the `ext/ikvm-native-sdk/` directory, and consist of a Windows SDK (windows), Linux SDK (linux) and Mac OS X SDK (macosx). The SDKs can be retrieved from the releases of the `ikvm-native-sdk` project.
  * We use IKVM.Clang projects to build the native libraries. This project type supports inner builds for TargetMachine, much as .NET supports inner builds for TFMs.
  * There is an IKVM.Clang Visual Studio extension for these projects to load properly in Visual Studio: https://marketplace.visualstudio.com/items?itemName=ikvm.clang
  * When on Windows, the Windows SDK distributed through this project is not required. But it is required that you have a Windows SDK installed. Building on other operating systems requires all of the SDKs.
    * A few of the SDKs contain symlinks for which there is no way to extract them properly (osx) on Windows. We find 7zip has the best capability to produce SYMLINK files, though it incorrectly interpretes symlinks to directories as symlinks to files. The Fix-SymbolicLinks.ps1 script is provided to patch these up post extraction. For building Linux, the `win` SDK package is required. For building on OS X the `win-ci` package is required. The former has symbolic links setup for various versions of header files that exist with different required cases. While the latter is suitable for a case-insensitive OS.
  * The Linux SDK contains a version for all of the required platforms. These are required to cross compile to each supported platform. They must all be present.
* LLVM installed with `clang` available on the path. LLVM is shipped with Visual Studio, but you will need to update your PATH to include the relevant bin directory (typically: `C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Tools\Llvm\bin`) or you can install a [standalone distribution of LLVM](https://releases.llvm.org/). If building on Windows, ensure you do not build within the Visual Studio Developer Command Prompt as clang is unable to properly discover the Windows SDK when this is applied.
* 200GB of free disk space
* If building the OS X libraries or binaries (on by default) the 'rcodesign' utility is required for ad-hoc signing. This utility allows one to sign executables for Mac OS X from within Windows or Linux. It is available from the Rust crate named 'apple-codesign'. For more information, see https://gregoryszorc.com/docs/apple-codesign/0.22.0/.
 
 Once the prerequisites are in place, to build IKVM from the command line you will use the following commands:

 ```
 dotnet restore IKVM.sln
 msbuild IKVM.dist.msbuildproj
 ```

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

# Package Layout

The main IKVM package is 'IKVM'. This package contains the IKVM.Runtime, IKVM.Java assemblies, and the
libikvm native library. This is the minimal set of files required to begin bootstrapping the JVM. However, the JVM
itself requires additional files which are present in the IKVM.Image package hierarchy.

The IKVM.Image set of packages contains per-TFM and per-RID Java Runtime Image files. These include what you would
normally expect to see an a JVM distribution: a bin/ directory with binaries, native libraries, etc; as well as a
lib/ directory with various security and property files. The IKVM Runtime Image packages are RID specific, and named
with the convention IKVM.Image.*.runtime.RID, much like Microsoft RID specific packages are named.

These packages deliver the additional IKVM Image directory into the output of the user's project. They do this with a
special targets file provided by IKVM.Image. Each runtime package contains a MSBuild script which appends to a
IkvmImageItem ItemGroup, where each item has a TargetFramework and RuntimeIdentifier property. The primary build script
then selects from this ItemGroup to match the current build configuration. Only files selected for the current TFM are
output. If a RID specific build is underway, only files matching that RID are selected. This populates the ikvm/{rid}
directory in the user output based on the current build or publish being done, and attempts to minimize the size of
that output.

Three levels of packages are at play: IKVM.Image.runtime.RID, IKVM.Image.JRE.runtime.RID, IKVM.Image.JDK.runtime.RID.
The first includes primary requirements of IKVM itself: native libraries, configuration files. The JRE image merges in
the executables for a JRE image (java.exe, etc). While the JDK image merges in additional executables that would be
present in a JDK: (javac.exe, etc). IKVM contains a direct dependency to IKVM.Image. However, IKVM.Image.JRE and
IKVM.Image.JDK are optional.

