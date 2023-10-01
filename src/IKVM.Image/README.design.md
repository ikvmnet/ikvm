# IKVM.Image

IKVM.Image.* packages deliver IKVM Runtime Images, which consist of the standard items expected in a JAVA_HOME path. A bin/ directory and a lib/ directory, and various text files. The bin/ directory has the standard Java executables, either for a JRE or a JDK, and the native libraries required for IKVM, such as libjava, libjvm, etc. The lib/ directory contains non-executable objects for OpenJDK, such as the time database, etc.

At runtime IKVM is responsible for locating the appropriate ikvm.home property. It does this by determing the the root of the possible Image directories (ikvm.root.arch), and then finding a compatible IKVM RID for the current execution environment. ikvm.root is then built based on that.

The IKVM.Image and IKVM.Image.JRE and IKVM.Image.JDK packages share a lot of common infrastructure. On the user's machine, they include props files which add items to the `IkvmImageItem` ItemGroup. Each of these items has a TargetFramework and RuntimeIdentifier metadata property. The IKVM.Image.targets file is transitive, and runs in every user project. It includes the appropriate TFM and RID files for the build target. If the user is doing a RuntimeIdentifier specific build, it tries to only include that one copy.

This results in an ikvm/{rid} directory being created from the IkvmImageItem items for each RID, or a single RID. The IkvmImageItem items are expanded. If TargetFramework is any, then it is copied to each TFM output. If RuntimeIdentifier is any, then it is copied to each RID output. For instance, graphics files which appear on all platforms would be any/any, and thus appear in ikvm/net6.0/win-x64, win-x86, linux-x64, etc; as well as net472 and net6.0. Each IKVM.Image.*.runtime project thus includes only the files relevant to a specific runtime, while the IKVM.Image.* packages include those files that are applicable to all runtimes. This reduces individual package size.

Within the IKVM source tree itself, the same logic is used, as we have projects which require IKVM to be distributed along with them. The test projects, for instance. The .runtime projects themselves do this as well, expanding items to dependent projects.

They also include the IKVM.Image.pack.targets file, which includes IkvmImageItems into _PackageFiles, resulting in the build of the final nuget package without expansion. IkvmImageItems is thus used to build the NuGet files themselves, as well as extract the files out of the NuGet files.
