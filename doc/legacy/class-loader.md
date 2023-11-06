# ClassLoader

The class path or the usage of `ClassLoader`s is a little difficult because Java and .NET are not compatible. When there is an issue, it often results in a [ClassNotFoundException](class-not-found-exception.md).

In Java you can set the class path with:

- the environment variable CLASSPATH
- use the -cp switch on command line
- the property Class-Path in the MANIFEST.MF of a jar file.
- Use a `URLClassLoader` or your own `ClassLoader`

This is a little different in .NET which can produce class loading problems. The following is a list of strategies that can be used to solve class loading problems on .NET.

- [Compile time solutions](#compile-time-solutions)
    - [One large assembly](#one-large-assembly)
    - [AppDomainAssemblyClassLoader](#appdomainassemblyclassloader)
    - [sharedclassloader](#sharedclassloader)
    - [ClassPathAssemblyClassLoader](#classpathassemblyclassloader)
- [Runtime solutions](#runtime-solutions)
    - [BootClassPathAssemby](#bootclasspathassembly)
    - [Context class loader](#context-class-loader)
    - [URLClassLoader](#urlclassloader)
- [Related blog entries](#related-blog-entries)

## Compile time solutions

### One large assembly

The simplest solution is to compile all with [ikvmc](tools/ikvmc.md) in a single assembly. There is no class loading problem in this case. The disadvantages are:

- a performance degree, you need to load all possible components. For example all supported JDBC driver if you only need one at one moment.
- duplicate resources files are lost. Like in one jar also in one dll a single file can exist only once. If the resources identical it is no problem. But with service for dynamic loading of components it is fatal. For example a JDBC driver registers it self if it in the classpath. If you have multiple JDBC drivers then only one is registers.

### AppDomainAssemblyClassLoader

This classloader loads from all assemblies that are currently loaded in your application (AppDomain in .NET lingo). So if a referenced assembly has not yet been loaded, it will not be searched at least not by the "AppDomain" part of the class loader, the default assembly class loader mechanism will still load referenced assemblies that are directly referenced by your assembly.

You can set it with the follow command line:

```console
ikvmc -classloader:ikvm.runtime.AppDomainAssemblyClassLoader MyJar.jar
```

### sharedclassloader

If you compile multiple jar files to multiple assemblies then you can use the option sharedclassloader.

```console
ikvmc -sharedclassloader { first.jar } { second.jar } { third.jar }
```
You can only share the classloader between the jar files that was create in one step.

### ClassPathAssemblyClassLoader

It first searches the assembly (and, again, the assembly directly referenced) and then the class path.

```console
ikvmc -classloader:ikvm.runtime.ClassPathAssemblyClassLoader MyJar.jar
```

## Runtime solutions

### BootClassPathAssembly

If you have a Visual Studio project then it does not help if you add the needed dlls as reference. You need also to use it. But an internal, dynamic use with Class.forName(x) can not detect as usage from Visual Studio. If you have a main program written in .NET then you can register your dll with the follow program line at startup of your program.

```c#
ikvm.runtime.Startup.addBootClassPathAssembly(Assembly.Load("YourDll"));
```
The class ikvm.runtime.Startup is saved in IKVM.OpenJDK.Core.dll.

### Context class loader

If you use one of the 3 class loaders for multiple DLL's then this has an effect to the default class loader. But Java know also the concept of the context class loader per thread. The standard class loader use the hierarchical class loader first and then the context class loader.

There are some bad design Java library which only use the context class loader. This look like:

```java
Class.forName( "org.company.abc.DynamicLoadedClass", true, Thread.currentThread().getContextClassLoader() );
```

If you do not set the context class loader then it point to your main assembly and all assembly that are referenced from the compiler. Typical this means you can load all classes from your main assembly, the `BootClassPathAssembly` and from the first Java dll which you call directly.

You can add references to all DLL's in your main assembly with lines like:

```c#
GC.KeepAlive(typeof(org.company.abc.DynamicLoadedClass));
```

This can be difficult if you have a large count of DLLs. Another solution is to set the context class loader to the same as the default class loader before you load the first class.

```c#
java.lang.Class clazz = typeof(org.company.xyz.FirstClass);
java.lang.Thread.currentThread().setContextClassLoader( clazz.getClassLoader() );
new org.company.xyz.FirstClass();
```

### URLClassLoader

Of course, you can also reuse existing class loader classes. Here's an example with `URLClassLoader`:

```java
class MyCustomClassLoader extends java.net.URLClassLoader
{
  MyCustomClassLoader(cli.System.Reflection.Assembly asm)
  {
    super(new java.net.URL[0], new ikvm.runtime.AssemblyClassLoader(asm));
    // explicitly calling addURL() is safer than passing it to the super constructor,
    // because this class loader instance may be used during the URL construction.
    addURL(new java.net.URL("..."));
  }
}
```

## Related blog entries

- [Class Loading Architecture](https://web.archive.org/web/20210518052001/https://weblog.ikvm.net/PermaLink.aspx?guid=4e0b7f7c-6f5d-42a3-a4d6-5d05a99c84ff)
- [New Development Snapshot](https://web.archive.org/web/20080502034606/http://weblog.ikvm.net/PermaLink.aspx?guid=8a457a80-1e5f-4182-8f78-b2cd67845553)
- [Writing a Custom Assembly Class Loader](https://web.archive.org/web/20210518035313/http://weblog.ikvm.net/PermaLink.aspx?guid=375f1ff8-912a-4458-9120-f0a8cfb23b68)

## Related

- [ClassNotFoundException](class-not-found-exception.md)
- [Components](components.md)
- [Convert a .jar file to a .dll](convert-a-jar-file-to-a-dll.md)
- [ikvmc](tools/ikvmc.md)
- [Tutorial](tutorial.md)