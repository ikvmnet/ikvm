#!/bin/sh
clang -S -emit-llvm jni.c -I../../../openjdk/jdk/src/share/javavm/export/ -I../../../openjdk/jdk/src/solaris/javavm/export -o linux-arm.ll --target=armv7-unknown-linux-gnueabihf
