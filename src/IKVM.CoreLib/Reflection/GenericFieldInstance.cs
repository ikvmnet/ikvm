/*
  Copyright (C) 2009, 2010 Jeroen Frijters

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
namespace IKVM.Reflection
{

    sealed class GenericFieldInstance : FieldInfo
    {

        readonly Type _declaringType;
        readonly FieldInfo _field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="field"></param>
        internal GenericFieldInstance(Type declaringType, FieldInfo field)
        {
            _declaringType = declaringType;
            _field = field;
        }

        public override bool Equals(object obj)
        {
            var other = obj as GenericFieldInstance;
            return other != null && other._declaringType.Equals(_declaringType) && other._field.Equals(_field);
        }

        public override int GetHashCode() => _declaringType.GetHashCode() * 3 ^ _field.GetHashCode();

        public override FieldAttributes Attributes => _field.Attributes;

        public override string Name => _field.Name;

        public override Type DeclaringType => _declaringType;

        public override Module Module => _declaringType.Module;

        public override int MetadataToken => _field.MetadataToken;

        public override object GetRawConstantValue() => _field.GetRawConstantValue();

        public override bool __TryGetFieldOffset(out int offset) => _field.__TryGetFieldOffset(out offset);

        internal override FieldSignature FieldSignature => _field.FieldSignature.ExpandTypeParameters(_declaringType);

        internal override int ImportTo(Emit.ModuleBuilder module) => module.ImportMethodOrField(_declaringType, _field.Name, _field.FieldSignature);

        internal override FieldInfo BindTypeParameters(Type type) => new GenericFieldInstance(_declaringType.BindTypeParameters(type), _field);

        internal override int GetCurrentToken() => _field.GetCurrentToken();

        internal override bool IsBaked => _field.IsBaked;

    }

}
