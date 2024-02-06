# Installation

The installation procedure for both Windows and Linux is straightforward. After downloading the binary distribution, simply extract the files from the archive. Open a command or shell window, cd to `ikvm\bin`, and type:

```console
ikvm
```

If your system is operating correctly, you should see the following output:

```console
usage: ikvm [-options] &lt;class&gt; [args...]
        (to execute a class)
  or ikvm -jar [-options] &lt;jarfile&gt; [args...]
         (to execute a jar file) ...
```

For convenience, you may wish to add the `\ikvm\bin` folder to your system's path, but this is not required. Now, if all you want to do is use IKVM as a Java VM, you're done -- no further configuration is needed. If you want to use IKVM for .NET / Mono development, read the configuration instructions below.

## Configuration for Development

If you plan to do .NET development with IKVM, you may wish to do the following:

- Download a Java SDK
  If you plan to develop code in Java that runs in .NET, you will need a Java compiler. IKVM does not come with a compiler. You may use any Java compiler that emits standard Java .class files. The [Debugging] is a little difficult but possible. Because not every Java syntax can 100% mirror to .NET here is a syntax help for [Using_Java_with_.NET].

- Windows: Install IKVM dll's in the Global Assembly Cache
  When running .NET applications in Windows that use IKVM dll's, the .NET framework must be able to locate the dll's. It looks in the Global Assembly Cache, then in the current directory. If you want to be able to do development without having the dll's in the current directory, you must install them in the Global Assembly Cache. To do this in Windows, access the Microsoft .NET Framework Configuration item in the Windows Control Panel, and add the assemblies to the Assembly Cache. At a minimum, you will want to install the IKVM.OpenJDK.*.dll and IKVM.Runtime.dll.

## Related

- [Convert a .jar file to a .dll](convert-a-jar-file-to-a-dll.md)
- [Debugging](debugging.md)
- [Download](https://github.com/ikvm-revived/ikvm/releases)
- [Run a .jar file on the fly](run-a-jar-file-on-the-fly.md)
- [Using Java with .NET](using-java-with-dotnet.md)
