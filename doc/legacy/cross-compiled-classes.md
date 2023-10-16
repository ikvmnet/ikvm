# Cross-compiled Classes

IKVM compiles the Java byte-code to .NET IL code. IKVM is not a cross-compiler but there are exceptions. Here is list of classes which will be cross-compiled.

```
java.lang.String
System.String
```

```
lava.lang.Throwable
System.Exception
printStackTrace() shows the Java class name.
```

```
java.lang.Class
System.Type
```