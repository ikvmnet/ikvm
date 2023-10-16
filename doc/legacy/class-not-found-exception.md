# ClassNotFoundException

One of the most ask question in the IKVM mailing list is the java.lang.ClassNotFoundException like:

```java
Exception in thread "main" java.lang.ClassNotFoundException: com.xyz.Xyz
    at java.net.URLClassLoader$1.run(URLClassLoader.java:366)
    at java.net.URLClassLoader$1.run(URLClassLoader.java:355)
    at java.security.AccessController.doPrivileged(AccessController.java:279)
    at java.security.AccessController.doPrivileged(AccessController.java:520)
    at java.net.URLClassLoader.findClass(URLClassLoader.java:354)
    at java.lang.ClassLoader.loadClass(ClassLoader.java:450)
    at sun.misc.Launcher$AppClassLoader.loadClass(Launcher.java:308)
    at java.lang.ClassLoader.loadClass(ClassLoader.java:385)
    at java.lang.ClassLoader.loadClassInternal(ClassLoader.java:500)
    at IKVM.Internal.ClassLoaderWrapper.LoadClassImpl(Unknown Source)
    at IKVM.Internal.ClassLoaderWrapper.LoadClassByDottedNameFastImpl(Unknown Source)
    at IKVM.Internal.ClassLoaderWrapper.LoadClassByDottedName(Unknown Source)
    at IKVM.NativeCode.java.lang.Class.forName0(Unknown Source)
    at java.lang.Class.forName0(Native Method)
    at java.lang.Class.forName(Class.java:287)
```

This occurs if you do a mistake at compile time. There can be the following causes:

1. You forgot to compile a needed `.jar` file in an assembly. In this case you should have receive a IKVMC0105 error message from `ikvmc`. Add the missing `.jar` file to your `ikvmc` command line.
2. The different `.dll`s have no references to the other. A simple solution can be to use a [sharedclassloader](class-loader.md#sharedclassloader).
3. Analyze the `ikvmc` warnings to locate the name of the class which was not found.

If this does not help then open a new issue on GitHub. Your issue should include:

- The `ikvmc` command line(s).
- The name of the `.jar` file which include the missing class.
- The name of the `.jar` file which include the class which call `Class.forName`. This can you see in the line under `Class.forName`.
- The warnings from your `ikvmc` call.

## Related

- [ClassLoader](class-loader.md)
- [ikvmc Warnings](tools/ikvmc.md#warnings)
- [Tutorial](tutorial.md)