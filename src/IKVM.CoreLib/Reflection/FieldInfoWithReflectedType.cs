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

        readonly Type _reflectedType;
        readonly FieldInfo _field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reflectedType"></param>
        /// <param name="field"></param>
        internal FieldInfoWithReflectedType(Type reflectedType, FieldInfo field)
        {
            Debug.Assert(reflectedType != field.DeclaringType);
            this._reflectedType = reflectedType;
            this._field = field;
        }

        public override FieldAttributes Attributes
        {
            get { return _field.Attributes; }
        }

        public override bool __TryGetFieldOffset(out int offset)
        {
            return _field.__TryGetFieldOffset(out offset);
        }

        public override object GetRawConstantValue()
        {
            return _field.GetRawConstantValue();
        }

        internal override FieldSignature FieldSignature
        {
            get { return _field.FieldSignature; }
        }

        internal override int ImportTo(Emit.ModuleBuilder module)
        {
            return _field.ImportTo(module);
        }

        internal override FieldInfo BindTypeParameters(Type type)
        {
            return _field.BindTypeParameters(type);
        }

        public override bool __IsMissing
        {
            get { return _field.__IsMissing; }
        }

        public override Type DeclaringType
        {
            get { return _field.DeclaringType; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as FieldInfoWithReflectedType;
            return other != null
                && other._reflectedType == _reflectedType
                && other._field == _field;
        }

        public override int GetHashCode()
        {
            return _reflectedType.GetHashCode() ^ _field.GetHashCode();
        }

        public override int MetadataToken
        {
            get { return _field.MetadataToken; }
        }

        public override Module Module
        {
            get { return _field.Module; }
        }

        public override string Name
        {
            get { return _field.Name; }
        }

        public override string ToString()
        {
            return _field.ToString();
        }

        internal override int GetCurrentToken()
        {
            return _field.GetCurrentToken();
        }

        internal override bool IsBaked
        {
            get { return _field.IsBaked; }
        }

    }

}
