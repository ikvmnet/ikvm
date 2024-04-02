The libikvm project provides a number of native functions used by IKVM itself, internally. This is a native library consumed from .NET, not from JNI, and thus doesn't include jni.h or any of the other Java oriented headers. It is not accessed from JNI code.

For instance, we provide IKVM_dl_* functions to wrap native dlopen, dlclose functions. These are used to bootstrap libjvm, which is then responsible for loading other JNI libraries.

