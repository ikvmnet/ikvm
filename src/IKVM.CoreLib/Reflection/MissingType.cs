﻿/*
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

        readonly Module module;
        readonly Type declaringType;
        readonly string ns;
        readonly string name;
        Type[] typeArgs;
        int token;
        int flags;
        bool cyclicTypeForwarder;
        bool cyclicTypeSpec;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        internal MissingType(Module module, Type declaringType, string ns, string name)
        {
            this.module = module;
            this.declaringType = declaringType;
            this.ns = ns;
            this.name = name;
            MarkKnownType(ns, name);
        }

        internal override MethodBase FindMethod(string name, MethodSignature signature)
        {
            var method = new MissingMethod(this, name, signature);
            if (name == ".ctor")
                return new ConstructorInfoImpl(method);

            return method;
        }

        internal override FieldInfo FindField(string name, FieldSignature signature)
        {
            return new MissingField(this, name, signature);
        }

        internal override Type FindNestedType(TypeName name)
        {
            return null;
        }

        internal override Type FindNestedTypeIgnoreCase(TypeName lowerCaseName)
        {
            return null;
        }

        public override bool __IsMissing
        {
            get { return true; }
        }

        public override Type DeclaringType
        {
            get { return declaringType; }
        }

        internal override TypeName TypeName
        {
            get { return new TypeName(ns, name); }
        }

        public override string Name
        {
            get { return TypeNameParser.Escape(name); }
        }

        public override string FullName
        {
            get { return GetFullName(); }
        }

        public override Module Module
        {
            get { return module; }
        }

        public override int MetadataToken
        {
            get { return token; }
        }

        protected override bool IsValueTypeImpl
        {
            get
            {
                switch (typeFlags & (TypeFlags.ValueType | TypeFlags.NotValueType))
                {
                    case TypeFlags.ValueType:
                        return true;
                    case TypeFlags.NotValueType:
                        return false;
                    default:
                        if (module.Universe.ResolveMissingTypeIsValueType(this))
                            typeFlags |= TypeFlags.ValueType;
                        else
                            typeFlags |= TypeFlags.NotValueType;

                        return (typeFlags & TypeFlags.ValueType) != 0;
                }
            }
        }

        public override Type BaseType
        {
            get { throw new MissingMemberException(this); }
        }

        public override TypeAttributes Attributes
        {
            get { throw new MissingMemberException(this); }
        }

        public override Type[] __GetDeclaredTypes()
        {
            throw new MissingMemberException(this);
        }

        public override Type[] __GetDeclaredInterfaces()
        {
            throw new MissingMemberException(this);
        }

        public override MethodBase[] __GetDeclaredMethods()
        {
            throw new MissingMemberException(this);
        }

        public override __MethodImplMap __GetMethodImplMap()
        {
            throw new MissingMemberException(this);
        }

        public override FieldInfo[] __GetDeclaredFields()
        {
            throw new MissingMemberException(this);
        }

        public override EventInfo[] __GetDeclaredEvents()
        {
            throw new MissingMemberException(this);
        }

        public override PropertyInfo[] __GetDeclaredProperties()
        {
            throw new MissingMemberException(this);
        }

        public override CustomModifiers __GetCustomModifiers()
        {
            throw new MissingMemberException(this);
        }

        public override Type[] GetGenericArguments()
        {
            throw new MissingMemberException(this);
        }

        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers()
        {
            throw new MissingMemberException(this);
        }

        public override bool __GetLayout(out int packingSize, out int typeSize)
        {
            throw new MissingMemberException(this);
        }

        public override bool IsGenericType
        {
            get { throw new MissingMemberException(this); }
        }

        public override bool IsGenericTypeDefinition
        {
            get { throw new MissingMemberException(this); }
        }

        internal override Type GetGenericTypeArgument(int index)
        {
            if (typeArgs == null)
                typeArgs = new Type[index + 1];
            else if (typeArgs.Length <= index)
                Array.Resize(ref typeArgs, index + 1);

            return typeArgs[index] ?? (typeArgs[index] = new MissingTypeParameter(this, index));
        }

        internal override Type BindTypeParameters(IGenericBinder binder)
        {
            return this;
        }

        internal override Type SetMetadataTokenForMissing(int token, int flags)
        {
            this.token = token;
            this.flags = flags;
            return this;
        }

        internal override Type SetCyclicTypeForwarder()
        {
            this.cyclicTypeForwarder = true;
            return this;
        }

        internal override Type SetCyclicTypeSpec()
        {
            this.cyclicTypeSpec = true;
            return this;
        }

        internal override bool IsBaked
        {
            get { throw new MissingMemberException(this); }
        }

        public override bool __IsTypeForwarder
        {
            // CorTypeAttr.tdForwarder
            get { return (flags & 0x00200000) != 0; }
        }

        public override bool __IsCyclicTypeForwarder
        {
            get { return cyclicTypeForwarder; }
        }

        public override bool __IsCyclicTypeSpec
        {
            get { return cyclicTypeSpec; }
        }

    }

}
