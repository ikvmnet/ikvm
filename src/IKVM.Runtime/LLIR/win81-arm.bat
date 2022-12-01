clang -S -emit-llvm jni.c -Wall -I..\..\..\openjdk\jdk\src\share\javavm\export\ -I..\..\..\openjdk\jdk\src\windows\javavm\export\ -o win81-arm.ll --target=thumbv7-pc-windows-msvc -g
