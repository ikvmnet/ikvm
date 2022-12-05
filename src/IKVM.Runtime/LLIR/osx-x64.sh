#!/bin/sh
clang -S -emit-llvm jni.c -I../../../openjdk/jdk/src/share/javavm/export/ -I../../../openjdk/jdk/src/solaris/javavm/export -o osx-x64.ll --target=x86_64-apple-macosx
