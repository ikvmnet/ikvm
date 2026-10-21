# IKVM.NET.Sdk - IKVM SDK for the Java Language

Provides a managed SDK for .NET projects targeting the IKVM runtime.

## Usage

IKVM.NET.Sdk provides a full MBuild SDK for compiling Java code into .NET assemblies. To use, create a `.msbuildproj`
file with contents like the following, using the latest version.

```
<Project Sdk="IKVM.NET.Sdk">
    <PropertyGroup>
        <TargetFrameworks>net472;net8.0</TargetFrameworks>
    </PropertyGroup>
</Project>
```

The SDK implicitly references the `IKVM` and `IKVM.MSBuild` packages, at the same version as the SDK. Set
`DisableImplicitIkvmPackageReference` to `true` to manage these references yourself.

When using [Central Package Management](https://learn.microsoft.com/nuget/consume-packages/central-package-management),
nothing needs to be added to `Directory.Packages.props`. To choose a different version than the one the SDK selects, add
a `PackageVersion` item for `IKVM` and `IKVM.MSBuild`, and the SDK will use the central version instead.

```
<ItemGroup>
    <PackageVersion Include="IKVM" Version="8.15.0" />
    <PackageVersion Include="IKVM.MSBuild" Version="8.15.0" />
</ItemGroup>
```

## Limitations

The project will compile fine in Visual Studio, with some caveats. Since we don't yet have a Visual Studio extension,
manual modifications will be required to the `.sln` file. Ensure that after you add the project to your solution, you
adjust the Project Type Guid to the C# project value of `{9A19103F-16F7-4668-BE54-9A1E7A4F7556}`.

```
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "MyProject", "src\MyProject\MyProject.msbuildproj", "ProjectGuid"
EndProject
```

.java files are included by default, just as .cs files are included by default in C# projects. However, Visual Studio does
not yet show them as included files within the solution explorer. Ensure you have "Show All" enabled. There is no need
to manually add them to item groups.

No Intellisense yet.

ProjectReferences work fine, except for the TFM limitations.

