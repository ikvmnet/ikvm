# IKVM - Java Virtual Machine for .NET (SDK)

This project contains the build infrastructure required for referencing Java libraries from .NET projects.

Install the `IKVM.Sdk` package in a project that wants to reference Java libraries. Use the `JavaReference` `ItemGroup`
to indicate which Java libraries your project references.

Example:

```
    <ItemGroup>
        <JavaReference Include="..\..\ext\helloworld-2.0.jar" />
    </ItemGroup>
```

The output assembly will be generated as part of your project's build process. Additional metadata can be added to
`JavaReference` to customize the generated assembly.

+ `Identity`: The identity of the `JavaReference` item can be either a) path to a JAR file b) path to a directory or c)
an otherwise unimportant name.
+ `AssemblyName`: By default the `AssemblyName` is generated using the rules defined by the `Automatic-Module-Name`
specification. To override this, do so here.
+ `AssemblyVersion`: By default the `AssemblyVersion` is generated using the rules defined by the `Automatic-Module-Name`
specification. To override this, do so here.  (not yet supported)
+ `Compile`: Optional semi-colon separated list of Java class path items to compile into the assembly. By default this
value is the `Identity` of the item, if the identity of the item is an existing JAR file or directory (not yet supported). MSBuild globs
are supported to reference multiple JAR or .class files.
+ `Sources`: Optional semi-colon separated list of Java source files to use during documentation generation. (not yet supported)
+ `References`: Optional semi-colon separated list of other `JavaReference` identity values to specify as a reference
to the current one. For instance, if `foo.jar` depends on `bar.jar`, include both as `JavaReference` items, but specify
the identity of `bar.jar` on the `References` metadata of `foo.jar`.
+ All other metadata supported on the `Reference` MSBuild item group definition.

`JavaReference` is not transitive. Including it in one project, and adding a dependency to that project from a second
project, will not result in the same reference being available on the second project. Instead, add the reference to
each project.


```
    <ItemGroup>
        <JavaReference Include="helloworld.jar">
            <AssemblyVersion>1.0.0.0</AssemblyVersion>
        </JavaReference>
        <JavaReference Include="helloworld-2.jar">
            <AssemblyName>helloworld-2</AssemblyName>
            <AssemblyVersion>2.0.0.0</AssemblyVersion>
            <References>helloworld.jar</References>
            <Aliases>helloworld2</Aliases>
        </JavaReference>
    </ItemGroup>

```
