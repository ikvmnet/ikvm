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

using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;

namespace IKVM.CoreLib.Linking
{

    internal abstract class FieldOrMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> : IEquatable<FieldOrMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        readonly ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> _classFile;

        protected ClassFileFlags flags;
        protected ClassFileAccessFlags accessFlags;
        string name;
        string descriptor;
        protected string? signature;
        protected object[]? annotations;
        protected TypeAnnotationTable runtimeVisibleTypeAnnotations = TypeAnnotationTable.Empty;
        protected TypeAnnotationTable runtimeInvisibleTypeAnnotations = TypeAnnotationTable.Empty;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="accessFlags"></param>
        /// <param name="name"></param>
        /// <param name="descriptor"></param>
        internal FieldOrMethod(ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile, AccessFlag accessFlags, Utf8ConstantHandle name, Utf8ConstantHandle descriptor)
        {
            this._classFile = classFile ?? throw new ArgumentNullException(nameof(classFile));
            this.accessFlags = ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>.ReadAccessFlags(accessFlags);
            this.name = string.Intern(classFile.GetConstantPoolUtf8String(name));
            this.descriptor = classFile.GetConstantPoolUtf8String(descriptor);

            ValidateSig(classFile, this.descriptor);
            this.descriptor = string.Intern(this.descriptor.Replace('/', '.'));
        }

        /// <summary>
        /// Gets the declaring <see cref="ClassFile{TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod}"/>.
        /// </summary>
        public ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> ClassFile => _classFile;

        protected abstract void ValidateSig(ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile, string descriptor);

        internal string Name => name;

        internal string Signature => descriptor;

        internal object[]? Annotations => annotations;

        internal string? GenericSignature => signature;

        internal ClassFileAccessFlags AccessFlags => accessFlags;

        internal bool IsAbstract => (accessFlags & ClassFileAccessFlags.Abstract) != 0;

        internal bool IsFinal => (accessFlags & ClassFileAccessFlags.Final) != 0;

        internal bool IsPublic => (accessFlags & ClassFileAccessFlags.Public) != 0;

        internal bool IsPrivate => (accessFlags & ClassFileAccessFlags.Private) != 0;

        internal bool IsProtected => (accessFlags & ClassFileAccessFlags.Protected) != 0;

        internal bool IsStatic => (accessFlags & ClassFileAccessFlags.Static) != 0;

        internal bool IsSynchronized => (accessFlags & ClassFileAccessFlags.Synchronized) != 0;

        internal bool IsVolatile => (accessFlags & ClassFileAccessFlags.Volatile) != 0;

        internal bool IsTransient => (accessFlags & ClassFileAccessFlags.Transient) != 0;

        internal bool IsNative => (accessFlags & ClassFileAccessFlags.Native) != 0;

        internal bool IsEnum => (accessFlags & ClassFileAccessFlags.Enum) != 0;

        internal bool DeprecatedAttribute => (flags & ClassFileFlags.MASK_DEPRECATED) != 0;

        internal bool IsInternal => (flags & ClassFileFlags.MASK_INTERNAL) != 0;

        internal bool IsModuleInitializer => (flags & ClassFileFlags.MODULE_INITIALIZER) != 0;

        internal ref readonly TypeAnnotationTable RuntimeVisibleTypeAnnotations => ref runtimeVisibleTypeAnnotations;

        public sealed override int GetHashCode() => name.GetHashCode() ^ descriptor.GetHashCode();

        public bool Equals(FieldOrMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>? other) => other != null && ReferenceEquals(name, other.name) && ReferenceEquals(descriptor, other.descriptor);

    }

}
