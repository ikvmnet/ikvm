clang -S -emit-llvm jni.c -Wall -I..\..\..\openjdk\jdk\src\share\javavm\export\ -I..\..\..\openjdk\jdk\src\windows\javavm\export\ -o win7-x86.ll --target=i686-pc-windows-msvc -g
