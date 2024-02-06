/*
  Copyright (C) 2009-2012 Jeroen Frijters

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
using System.Diagnostics;

namespace IKVM.Reflection
{

    sealed class FieldInfoWithReflectedType : FieldInfo
    {

        readonly Type reflectedType;
        readonly FieldInfo field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reflectedType"></param>
        /// <param name="field"></param>
        internal FieldInfoWithReflectedType(Type reflectedType, FieldInfo field)
        {
            Debug.Assert(reflectedType != field.DeclaringType);
            this.reflectedType = reflectedType;
            this.field = field;
        }

        public override FieldAttributes Attributes
        {
            get { return field.Attributes; }
        }

        public override void __GetDataFromRVA(byte[] data, int offset, int length)
        {
            field.__GetDataFromRVA(data, offset, length);
        }

        public override int __FieldRVA
        {
            get { return field.__FieldRVA; }
        }

        public override bool __TryGetFieldOffset(out int offset)
        {
            return field.__TryGetFieldOffset(out offset);
        }

        public override Object GetRawConstantValue()
        {
            return field.GetRawConstantValue();
        }

        internal override FieldSignature FieldSignature
        {
            get { return field.FieldSignature; }
        }

        public override FieldInfo __GetFieldOnTypeDefinition()
        {
            return field.__GetFieldOnTypeDefinition();
        }

        internal override int ImportTo(Emit.ModuleBuilder module)
        {
            return field.ImportTo(module);
        }

        internal override FieldInfo BindTypeParameters(Type type)
        {
            return field.BindTypeParameters(type);
        }

        public override bool __IsMissing
        {
            get { return field.__IsMissing; }
        }

        public override Type DeclaringType
        {
            get { return field.DeclaringType; }
        }

        public override Type ReflectedType
        {
            get { return reflectedType; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as FieldInfoWithReflectedType;
            return other != null
                && other.reflectedType == reflectedType
                && other.field == field;
        }

        public override int GetHashCode()
        {
            return reflectedType.GetHashCode() ^ field.GetHashCode();
        }

        public override int MetadataToken
        {
            get { return field.MetadataToken; }
        }

        public override Module Module
        {
            get { return field.Module; }
        }

        public override string Name
        {
            get { return field.Name; }
        }

        public override string ToString()
        {
            return field.ToString();
        }

        internal override int GetCurrentToken()
        {
            return field.GetCurrentToken();
        }

        internal override bool IsBaked
        {
            get { return field.IsBaked; }
        }

    }

}
