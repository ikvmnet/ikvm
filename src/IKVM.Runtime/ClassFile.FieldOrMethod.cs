/*
  Copyright (C) 2002-2015 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Collections.Generic;

using IKVM.Attributes;
using IKVM.ByteCode;
using IKVM.ByteCode.Reading;

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal abstract class FieldOrMethod : IEquatable<FieldOrMethod>
        {

            protected Modifiers accessFlags;
            protected ushort flags;
            string name;
            string descriptor;
            protected string signature;
            protected object[] annotations;
            protected IReadOnlyList<TypeAnnotationReader> runtimeVisibleTypeAnnotations;
            protected IReadOnlyList<TypeAnnotationReader> runtimeInvisibleTypeAnnotations;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="classFile"></param>
            /// <param name="utf8_cp"></param>
            /// <param name="accessFlags"></param>
            /// <param name="nameIndex"></param>
            /// <param name="descriptorIndex"></param>
            internal FieldOrMethod(ClassFile classFile, string[] utf8_cp, AccessFlag accessFlags, ushort nameIndex, ushort descriptorIndex)
            {
                this.accessFlags = (Modifiers)accessFlags;
                this.name = string.Intern(classFile.GetConstantPoolUtf8String(utf8_cp, nameIndex));
                this.descriptor = classFile.GetConstantPoolUtf8String(utf8_cp, descriptorIndex);

                ValidateSig(classFile, descriptor);
                this.descriptor = string.Intern(descriptor.Replace('/', '.'));
            }

            protected abstract void ValidateSig(ClassFile classFile, string descriptor);

            internal string Name => name;

            internal string Signature => descriptor;

            internal object[] Annotations => annotations;

            internal string GenericSignature => signature;

            internal Modifiers Modifiers => (Modifiers)accessFlags;

            internal bool IsAbstract => (accessFlags & Modifiers.Abstract) != 0;

            internal bool IsFinal => (accessFlags & Modifiers.Final) != 0;

            internal bool IsPublic => (accessFlags & Modifiers.Public) != 0;

            internal bool IsPrivate => (accessFlags & Modifiers.Private) != 0;

            internal bool IsProtected => (accessFlags & Modifiers.Protected) != 0;

            internal bool IsStatic => (accessFlags & Modifiers.Static) != 0;

            internal bool IsSynchronized => (accessFlags & Modifiers.Synchronized) != 0;

            internal bool IsVolatile => (accessFlags & Modifiers.Volatile) != 0;

            internal bool IsTransient => (accessFlags & Modifiers.Transient) != 0;

            internal bool IsNative => (accessFlags & Modifiers.Native) != 0;

            internal bool IsEnum => (accessFlags & Modifiers.Enum) != 0;

            internal bool DeprecatedAttribute => (flags & FLAG_MASK_DEPRECATED) != 0;

            internal bool IsInternal => (flags & FLAG_MASK_INTERNAL) != 0;

            internal bool IsModuleInitializer => (flags & FLAG_MODULE_INITIALIZER) != 0;

            internal IReadOnlyList<TypeAnnotationReader> RuntimeVisibleTypeAnnotations => runtimeVisibleTypeAnnotations;

            public sealed override int GetHashCode() => name.GetHashCode() ^ descriptor.GetHashCode();

            public bool Equals(FieldOrMethod other) => ReferenceEquals(name, other.name) && ReferenceEquals(descriptor, other.descriptor);

        }

    }

}
