#!/bin/sh
clang -S -emit-llvm jni.c -I../../../openjdk/jdk/src/share/javavm/export/ -I../../../openjdk/jdk/src/solaris/javavm/export -o osx-arm64.ll --target=arm64-apple-macosx
