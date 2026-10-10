/*
  Copyright (C) 2011-2012 Jeroen Frijters

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

namespace IKVM.Reflection
{

    sealed class MissingType : Type
    {

        readonly Module _module;
        readonly Type _declaringType;
        readonly string _ns;
        readonly string _name;
        Type[] _typeArgs;
        int _token;
        int _flags;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        internal MissingType(Module module, Type declaringType, string ns, string name)
        {
            _module = module;
            _declaringType = declaringType;
            _ns = ns;
            _name = name;
            MarkKnownType(ns, name);
        }

        internal override MethodBase FindMethod(string name, MethodSignature signature)
        {
            var method = new MissingMethod(this, name, signature);
            if (name == ".ctor")
                return new ConstructorInfoImpl(method);

            return method;
        }

        internal override FieldInfo FindField(string name, FieldSignature signature) => new MissingField(this, name, signature);

        internal override Type FindNestedType(TypeName name) => null;

        internal override Type FindNestedTypeIgnoreCase(TypeName lowerCaseName) => null;

        public override bool __IsMissing => true;

        public override Type DeclaringType => _declaringType;

        internal override TypeName TypeName => new TypeName(_ns, _name);

        public override string Name => TypeNameParser.Escape(_name);

        public override string FullName => GetFullName();

        public override Module Module => _module;

        public override int MetadataToken => _token;

        protected override bool IsValueTypeImpl
        {
            get
            {
                return (typeFlags & (TypeFlags.ValueType | TypeFlags.NotValueType)) switch
                {
                    TypeFlags.ValueType => true,
                    TypeFlags.NotValueType => false,
                    _ => throw new MissingMemberException(this),
                };
            }
        }

        public override Type BaseType => throw new MissingMemberException(this);

        public override TypeAttributes Attributes => throw new MissingMemberException(this);

        public override Type[] __GetDeclaredTypes() => throw new MissingMemberException(this);

        public override Type[] __GetDeclaredInterfaces() => throw new MissingMemberException(this);

        public override MethodBase[] __GetDeclaredMethods() => throw new MissingMemberException(this);

        public override __MethodImplMap __GetMethodImplMap() => throw new MissingMemberException(this);

        public override FieldInfo[] __GetDeclaredFields() => throw new MissingMemberException(this);

        public override EventInfo[] __GetDeclaredEvents() => throw new MissingMemberException(this);

        public override PropertyInfo[] __GetDeclaredProperties() => throw new MissingMemberException(this);

        public override CustomModifiers __GetCustomModifiers() => throw new MissingMemberException(this);

        public override Type[] GetGenericArguments() => throw new MissingMemberException(this);

        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers() => throw new MissingMemberException(this);

        public override bool IsGenericType => throw new MissingMemberException(this);

        public override bool IsGenericTypeDefinition => throw new MissingMemberException(this);

        internal override Type GetGenericTypeArgument(int index)
        {
            if (_typeArgs == null)
                _typeArgs = new Type[index + 1];
            else if (_typeArgs.Length <= index)
                Array.Resize(ref _typeArgs, index + 1);

            return _typeArgs[index] ?? (_typeArgs[index] = new MissingTypeParameter(this, index));
        }

        internal override Type BindTypeParameters(IGenericBinder binder) => this;

        internal override Type SetMetadataTokenForMissing(int token, int flags)
        {
            _token = token;
            _flags = flags;
            return this;
        }

        internal override bool IsBaked => throw new MissingMemberException(this);

    }

}
