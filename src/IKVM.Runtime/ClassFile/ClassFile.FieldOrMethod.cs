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

namespace IKVM.Internal
{

    sealed partial class ClassFile
    {
        internal abstract class FieldOrMethod : IEquatable<FieldOrMethod>
        {
            // Note that Modifiers is a ushort, so it combines nicely with the following ushort field
            protected Modifiers access_flags;
            protected ushort flags;
            private string name;
            private string descriptor;
            protected string signature;
            protected object[] annotations;
            protected byte[] runtimeVisibleTypeAnnotations;

            internal FieldOrMethod(ClassFile classFile, string[] utf8_cp, BigEndianBinaryReader br)
            {
                access_flags = (Modifiers)br.ReadUInt16();
                name = String.Intern(classFile.GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16()));
                descriptor = classFile.GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16());
                ValidateSig(classFile, descriptor);
                descriptor = String.Intern(descriptor.Replace('/', '.'));
            }

            protected abstract void ValidateSig(ClassFile classFile, string descriptor);

            internal string Name
            {
                get
                {
                    return name;
                }
            }

            internal string Signature
            {
                get
                {
                    return descriptor;
                }
            }

            internal object[] Annotations
            {
                get
                {
                    return annotations;
                }
            }

            internal string GenericSignature
            {
                get
                {
                    return signature;
                }
            }

            internal Modifiers Modifiers
            {
                get
                {
                    return (Modifiers)access_flags;
                }
            }

            internal bool IsAbstract
            {
                get
                {
                    return (access_flags & Modifiers.Abstract) != 0;
                }
            }

            internal bool IsFinal
            {
                get
                {
                    return (access_flags & Modifiers.Final) != 0;
                }
            }

            internal bool IsPublic
            {
                get
                {
                    return (access_flags & Modifiers.Public) != 0;
                }
            }

            internal bool IsPrivate
            {
                get
                {
                    return (access_flags & Modifiers.Private) != 0;
                }
            }

            internal bool IsProtected
            {
                get
                {
                    return (access_flags & Modifiers.Protected) != 0;
                }
            }

            internal bool IsStatic
            {
                get
                {
                    return (access_flags & Modifiers.Static) != 0;
                }
            }

            internal bool IsSynchronized
            {
                get
                {
                    return (access_flags & Modifiers.Synchronized) != 0;
                }
            }

            internal bool IsVolatile
            {
                get
                {
                    return (access_flags & Modifiers.Volatile) != 0;
                }
            }

            internal bool IsTransient
            {
                get
                {
                    return (access_flags & Modifiers.Transient) != 0;
                }
            }

            internal bool IsNative
            {
                get
                {
                    return (access_flags & Modifiers.Native) != 0;
                }
            }

            internal bool IsEnum
            {
                get
                {
                    return (access_flags & Modifiers.Enum) != 0;
                }
            }

            internal bool DeprecatedAttribute
            {
                get
                {
                    return (flags & FLAG_MASK_DEPRECATED) != 0;
                }
            }

            internal bool IsInternal
            {
                get
                {
                    return (flags & FLAG_MASK_INTERNAL) != 0;
                }
            }

            internal byte[] RuntimeVisibleTypeAnnotations
            {
                get
                {
                    return runtimeVisibleTypeAnnotations;
                }
            }

            public sealed override int GetHashCode()
            {
                return name.GetHashCode() ^ descriptor.GetHashCode();
            }

            public bool Equals(FieldOrMethod other)
            {
                return ReferenceEquals(name, other.name) && ReferenceEquals(descriptor, other.descriptor);
            }
        }
    }

}
