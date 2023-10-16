# Concepts

IKVM converts the Java byte-code into .NET IL code (Intermediate Language). This means the result is a .NET application. There are 2 modes for this conversion. On the fly with [ikvm](tools/ikvm.md) and via a compiler with [ikvmc](tools/ikvmc.md).

In both mode you convert byte-code and not the API. IKVM is not a cross-compiler with exceptions. This means you need a Java runtime API in .NET. This are all the dll's of IKVM. This are approximate 40MB overhead for your application. Depending of the used API some not used dll's can be removed if you are finish. This is not recommend.

There are also 2 types of code developments. You can write your code in any Java VM like Java or Scala. Or you write your code in any .NET language like C# or VB#. In the follow we will only speech from Java and C# also if any other language of this platform is possible.

- [Running on the fly](#running-on-the-fly)
- [Running with compiler](#running-with-compiler)
- [Developing in Java code](#developing-in-java-code)
- [Developing in C# code](#developing-in-c-code)

## Running on the fly

In this case you need to develop in Java code (see below). You need only to replace the Oracle or OpenJDK runtime with the IKVM runtime. In some cases it can help to rename ikvm.exe to java.exe. This is how you should begin. This mode is most compatible to Java.

The disadvantages is that this mode is very slow on start time. Every Java byte-code class need to converted to a IL class on every run. This is a large overhead for large projects.

More details can you find in a completely [step by step example](run-a-jar-file-on-the-fly.md).

## Running with compiler

In this mode you compile one or more jar files to a dll or exe file. It is recommended to compile every jar file to a separate dll or exe file. This start many faster than the on the fly mode.

The disadvantage is a switch from the Java class loading concept to the .NET class loading. Several strategies for dealing with the gap between .NET and Java are discussed in the [ClassLoader](class-loader.md) topic.

## Developing in Java code

To access .NET libraries in your Java code you create a stub file from a .NET library. This can you do with [ikvmstub](tools/ikvmstub.md). A IKVM stub file is a *.jar file, a Java library. It contains only definitions of classes and methods, but no code. If you run a stub file with the Oracle Java VM or OpenJDK you will receive an error.

The .NET classes start with the package CLI.

Some .NET constructs require a specific syntax in Java. This are list in [Using .NET with Java](using-dotnet-with-java.md).

## Developing in C# code

This is only possible with compiler mode. You compile you jar files to a dll or exe. Add it as reference to your IDE. You can use every class and method like from any other .NET class library.

Some Java constructs require a specific syntax in .NET. This is exemplified in [Using Java with .NET](using-java-with-dotnet.md).

See the [step by step example](convert-a-jar-file-to-a-dll.md) for details.