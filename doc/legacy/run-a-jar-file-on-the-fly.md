# Run a .jar file on-the-fly

With IKVM you can run dynamically one or multiple `.jar` files on the fly in a .NET application. This is only a solution for small .jar files or for a server because the startup time is slow. The `.jar` file need to compile on the fly to IL code. You can also call the code only via reflection.

First you need to download and install IKVM.

## Java sample code

First we create a hello world sample in Java and create a jar file. The sample Java code of `HelloWorld.java` is very simple:

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

You can save it as HelloWorld.java and compile it with the javac command line:

```console
javac HelloWorld.java
```

This should create a file HelloWorld.class. With any zip program you can create a jar file `HelloWorld.jar`. A .jar file is only a zip file. The .jar file must include the file `HelloWorld.class` in the folder `hello`. This is the package name of the class. In Java package name are identical to folder names.

## C# Sample code

In your C# IDE you create a new project. Add a reference to `System` and package reference on [IKVM](https://www.nuget.org/packages/IKVM). Copy the `HelloWorld.jar` file in the project root and set "Copy always". In your program file you can copy the follow C# sample code:

```c#
using System;

namespace DynamicLoading
{
   class Program
   {
        static void Main(string[] args)
        {
           // Create a URL instance for every jar file that you need
           java.net.URL url = new java.net.URL("file:HelloWorld.jar");
           // Create an array of all URLS
           java.net.URL[] urls = { url };
           // Create a ClassLoader
           java.net.URLClassLoader loader = new java.net.URLClassLoader(urls);
           try
           {
               // load the Class
               java.lang.Class cl = java.lang.Class.forName("hello.HelloWorld", true, loader);

               // Create a Object via Java reflection
               object obj = cl.newInstance();
               Console.WriteLine(obj);
               obj = cl.getConstructor(typeof(string)).newInstance("Java");
               Console.WriteLine(obj);

               //Create a object via C# reflection
               Type type = ikvm.runtime.Util.getInstanceTypeFromClass(cl);
               obj = type.GetConstructor(new Type[]{}).Invoke(null);
               Console.WriteLine(obj);
               obj = type.GetConstructor(new Type[] { typeof(string)}).Invoke( new object[]{"C#"});
               Console.WriteLine(obj);
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex);
               Console.WriteLine(ex.StackTrace);
           }
       }
   }
}
```

If you run this program then it produce the follow output:

```console
Hello World
Hello Java
Hello World
Hello C#
```

## Related

- [Installation](#installation.md)
