# Components of IKVM

## Tools

- [ikvm](tools/ikvm.md)
  Starter executable, comparable to java.exe ("dynamic mode"). The Java application will start slow because all classes need to converted on the fly.

- [ikvmc](tools/ikvmc.md)
  Static compiler. Used to compile Java classes and jars into a .NET assembly ("static mode"). There are different classloader options.

- [ikvmstub](tools/ikvmstub.md)
  A tool that generates stub class files from a .NET assembly, so that Java code can be compiled against .NET code. IKVM understands the stubs and replaces the references to the stubs by references to the actual .NET types.

- IKVM.Runtime.dll
  The VM runtime and all supporting code. It contains (among other things):

  - Byte Code JIT compiler/verifier: Just-in-time compiles Java Byte Code to CIL.
  - Object model remapping infrastructure: Makes System.Object, System.String and System.Exception appear to Java code as java.lang.Object, java.lang.String and java.lang.Throwable.
  - Managed .NET re-implementations of the native methods in Classpath.
 
- IKVM.Java.dll
  This are a compiled version of the Java class libraries, plus some additional IKVM specific code.

- ikvm-native.dll / libikvm-native.so
Small unmanaged C library that is used by the JNI interface. This is an optional part, and on Windows it is only required when an application uses it's own native libraries. When running on Linux, this library is also used by the NIO memory mapped files implementation. This library has been designed not to require any changes between IKVM versions, so if you download or compile a new version of IKVM, you don't necessarily need to update this native library.

- IKVM.AWT.WinForms.dll
  This are the AWT and Swing peers. This include the java.awt.Toolkit, fonts, images and more GUI stuff.

## Related

- [ClassLoader](class-loader.md)
- [ikvm](tools/ikvm.md)
- [ikvmc](tools/ikvmc.md)
- [ikvmstub](tools/ikvmstub.md)
- [User Guide](user-guide.md)