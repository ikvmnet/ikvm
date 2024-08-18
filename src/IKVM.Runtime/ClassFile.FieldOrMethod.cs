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

using IKVM.Attributes;
using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal abstract class FieldOrMethod : IEquatable<FieldOrMethod>
        {

            readonly ClassFile clazz;
            protected Modifiers accessFlags;
            protected ushort flags;
            string name;
            string descriptor;
            protected string signature;
            protected object[] annotations;
            protected TypeAnnotationTable runtimeVisibleTypeAnnotations = TypeAnnotationTable.Empty;
            protected TypeAnnotationTable runtimeInvisibleTypeAnnotations = TypeAnnotationTable.Empty;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="clazz"></param>
            /// <param name="utf8_cp"></param>
            /// <param name="accessFlags"></param>
            /// <param name="name"></param>
            /// <param name="descriptor"></param>
            internal FieldOrMethod(ClassFile clazz, string[] utf8_cp, AccessFlag accessFlags, Utf8ConstantHandle name, Utf8ConstantHandle descriptor)
            {
                this.clazz = clazz ?? throw new ArgumentNullException(nameof(clazz));
                this.accessFlags = (Modifiers)accessFlags;
                this.name = string.Intern(clazz.GetConstantPoolUtf8String(utf8_cp, name));
                this.descriptor = clazz.GetConstantPoolUtf8String(utf8_cp, descriptor);

                ValidateSig(clazz, this.descriptor);
                this.descriptor = string.Intern(this.descriptor.Replace('/', '.'));
            }

            /// <summary>
            /// Gets the declaring <see cref="ClassFile"/>.
            /// </summary>
            public ClassFile Class => clazz;

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

            internal ref readonly TypeAnnotationTable RuntimeVisibleTypeAnnotations => ref runtimeVisibleTypeAnnotations;

            public sealed override int GetHashCode() => name.GetHashCode() ^ descriptor.GetHashCode();

            public bool Equals(FieldOrMethod other) => ReferenceEquals(name, other.name) && ReferenceEquals(descriptor, other.descriptor);

        }

    }

}
