# Convert a .jar file to a Class Library (.dll)

This is the recommended solution if you want access a existing Java library from your .NET application. Depending on programming errors in the Java library this can be difficult.

First you need to [download](https://github.com/ikvm-revived/ikvm/releases) and [install](installation.md) IKVM.

## Java sample code

First we create a hello world sample in Java and create a .jar file. The sample Java code of `HelloWorld.java` is very simple:

```java
package hello;

public class HelloWorld {
    private String name;

    public HelloWorld() {
        this.name = "World";
    }

    public HelloWorld(String name) {
        this.name = name;
    }

    @Override
    public String toString() {
        return "Hello " + name;
    }
}
```

You can save it as `HelloWorld.java` and compile it with the javac command line:

```console
javac HelloWorld.java
```

This should create a file `HelloWorld.class`. With any zip program you can create a .jar file `HelloWorld.jar`. A jar file is only a zip file. The .jar file must include the file `HelloWorld.class` in the folder `hello`. This is the package name of the class. In Java package names are identical to folder names.

## Create a dll

The .jar file compile you with [ikvmc](tools/ikvmc.md). This can you do with the follow command line:

```console
ikvmc -target:library HelloWorld.jar
```

If you want compile more as one .jar file then follow syntax is recommended:

```console
ikvmc -target:library -sharedclassloader { first.jar } { second.jar } { third.jar }
```

Why this is recommend can you read in the [ClassLoader](class-loader.md) topic. This should produce the follow output:

```console
IKVM.NET Compiler version x.x.xxxx.x
Copyright © 2022 Jeroen Frijters, Windward Studios, Jerome Haltom, Shad Storhaug

note IKVMC0002: Output file is "HelloWorld.dll"
```

## C# Sample Code

In your C# IDE you create a new project. Add a reference to `System` and `HelloWorld.dll`. Add a package reference on [IKVM](https://www.nuget.org/packages/IKVM). In your program file you can copy the follow C# sample code:

```c#
using System;
using System.Reflection;

namespace JavaLibrary
{
   class Program
   {
       static void Main(string[] args)
       {
           //Add references to all jar files that you use not directly
           //ikvm.runtime.Startup.addBootClassPathAssemby(Assembly.Load("second"));
           //ikvm.runtime.Startup.addBootClassPathAssemby(Assembly.Load("third"));

           // set a context classloader, this is not needed for the HelloWorld sample 
           // but solv problems with some buggy Java libraries
           java.lang.Class clazz = typeof(hello.HelloWorld);
           java.lang.Thread.currentThread().setContextClassLoader(clazz.getClassLoader());

           object obj = new hello.HelloWorld();
           Console.WriteLine(obj);
           obj = new hello.HelloWorld("Java");
           Console.WriteLine(obj);
       }
   }
}
```

If you run this program then it produce the follow output:

```console
Hello World
Hello Java
```

## Related

- [ClassLoader](class-loader.md)
- [ikvmc](tools/ikvmc.md)
- [Installation](installation.md)
