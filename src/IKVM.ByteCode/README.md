# IKVM.ByteCode

Provides a Java class file parser, reader and writer implementation used by the IKVM project.

The project is organized into three related sections.

## IKVM.ByteCode.Parsing

Contains Record structures and classes that represent the raw values that might appear in a Java class file. These
types are not blittable, but do contain a TryRead method and TryWrite method, targeting a ClassFormatReader and
ClassFormatWriter respectively.

## IVKM.ByteCode.Reading

Contains Reader classes that wrap Record structures. The main top-level class is ClassReader. ClassReader operates on
a ClassRecord. ClassReader exposes collections and properties which allow navigation through a parsed class file. An
attempt is made to only initialize data from the underlying records on demand.

The Readers allow you to navigate through the constant pool, resolve constant values, and navigate through the
interfaces, fields, methods and attributes of the class.
