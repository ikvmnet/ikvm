# User Guide

IKVM is a Java Virtual Machine (JVM) for the .NET and Mono runtimes. At a time when most people in the computer industry consider Java and .NET as mutually exclusive technologies, IKVM stands in the unique position of bringing them together. Initially born out of frustration with the limitations of tools like JUMP and J#, IKVM was created when Jeroen Frijters set out to create a way to migrate an existing Java database application to .NET.

IKVM has gone through a variety of designs and name changes to emerge as a sophisticated collection of tools offering a variety of integration patterns between the Java and .NET languages and platforms. It is still under development but people have reported success in running sophisticated applications and tools including Eclipse, JmDNS, JGroups, Jetty (with a few changes), etc.

## Overview

There are two main ways of using IKVM:

- Dynamically: In this mode, Java classes and jars are used directly to execute Java applications on the .NET runtime. Java bytecode is translated on the fly into CIL and no further steps are necessary. The full Java class loader model is supported in this mode.
- Statically: In order to allow Java code to be used by .NET applications, it must be compiled down to a DLL and used directly. The bytecode is translated to CIL and stored in this form. The assemblies can be referenced directly by the .NET applications and the "Java" objects can be used as if they were .NET objects. While the static mode does not support the full Java class loader mechanism, it is possible for statically-compiled code to create a class loader and load classes dynamically.

IKVM provides the VM-related technologies for byte-code translation and verification, classloading, etc. It is dependent upon the OpenJDK project for implementations of the JDK libraries.

IKVM is comprised by the different [Components](components.md).

## System Requirements

You must have one of the following .NET frameworks installed:

Microsoft .NET Framework 2.0 SP1 (or later) Framework (Windows platform)
Mono 2.0 (or later) (Windows or Linux)

## Related

- [Components](components.md)
