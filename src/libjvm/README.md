The libjvm project builds a version of the jvm.dll or libjvm.so library that is typically distributed with Java Runtimes.

There are two major uses of this library. The first is for external hosting of the JVM. The JNI_ prefixed family of functions are used to create VMs, attach threads, etc. We implement these methods partially because some OpenJDK C code calls into them. But we aren't capable of fully hosting the JVM, since that would require fully hosting .NET itself.

The second set of functions prefixed with JVM_ are used internally by OpenJDK C code for various low level operations. To the extent possible we copy the implementations from OpenJDK. But, many of these functions need to call back into the internals of IKVM.

We also make use of JVM_LoadLibrary and JVM_UnloadLibrary in our JNI implementation, because this is how OpenJDK does so, and for good reasons: it causes loading of JNI libraries to originate from libjvm.dylib on OSX and thus inherit its loader path capabilities to load other JNI libraries from arbitrary locations.

