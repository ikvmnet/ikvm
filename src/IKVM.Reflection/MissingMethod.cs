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
namespace IKVM.Reflection
{

    sealed class MissingMethod : MethodInfo
    {

        readonly Type declaringType;
        readonly string name;
        internal MethodSignature signature;
        MethodInfo forwarder;
        Type[] typeArgs;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        internal MissingMethod(Type declaringType, string name, MethodSignature signature)
        {
            this.declaringType = declaringType;
            this.name = name;
            this.signature = signature;
        }

        MethodInfo Forwarder => TryGetForwarder() ?? throw new MissingMemberException(this);

        MethodInfo TryGetForwarder()
        {
            if (forwarder == null && !declaringType.__IsMissing)
            {
                var mb = declaringType.FindMethod(name, signature);
                var ci = mb as ConstructorInfo;
                if (ci != null)
                    forwarder = ci.GetMethodInfo();
                else
                    forwarder = (MethodInfo)mb;
            }

            return forwarder;
        }

        public override bool __IsMissing
        {
            get { return TryGetForwarder() == null; }
        }

        public override Type ReturnType
        {
            get { return signature.GetReturnType(this); }
        }

        public override ParameterInfo ReturnParameter
        {
            get { return new ParameterInfoImpl(this, -1); }
        }

        internal override MethodSignature MethodSignature
        {
            get { return signature; }
        }

        internal override int ParameterCount
        {
            get { return signature.GetParameterCount(); }
        }

        sealed class ParameterInfoImpl : ParameterInfo
        {

            readonly MissingMethod method;
            readonly int index;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="method"></param>
            /// <param name="index"></param>
            internal ParameterInfoImpl(MissingMethod method, int index)
            {
                this.method = method;
                this.index = index;
            }

            ParameterInfo Forwarder
            {
                get { return index == -1 ? method.Forwarder.ReturnParameter : method.Forwarder.GetParameters()[index]; }
            }

            public override string Name
            {
                get { return Forwarder.Name; }
            }

            public override Type ParameterType
            {
                get { return index == -1 ? method.signature.GetReturnType(method) : method.signature.GetParameterType(method, index); }
            }

            public override ParameterAttributes Attributes
            {
                get { return Forwarder.Attributes; }
            }

            public override int Position
            {
                get { return index; }
            }

            public override object RawDefaultValue
            {
                get { return Forwarder.RawDefaultValue; }
            }

            public override CustomModifiers __GetCustomModifiers()
            {
                return index == -1
                    ? method.signature.GetReturnTypeCustomModifiers(method)
                    : method.signature.GetParameterCustomModifiers(method, index);
            }

            public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
            {
                return Forwarder.__TryGetFieldMarshal(out fieldMarshal);
            }

            public override MemberInfo Member
            {
                get { return method; }
            }

            public override int MetadataToken
            {
                get { return Forwarder.MetadataToken; }
            }

            internal override Module Module
            {
                get { return method.Module; }
            }

            public override string ToString()
            {
                return Forwarder.ToString();
            }

        }

        public override ParameterInfo[] GetParameters()
        {
            var parameters = new ParameterInfo[signature.GetParameterCount()];
            for (int i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoImpl(this, i);

            return parameters;
        }

        public override MethodAttributes Attributes
        {
            get { return Forwarder.Attributes; }
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return Forwarder.GetMethodImplementationFlags();
        }

        public override MethodBody GetMethodBody()
        {
            return Forwarder.GetMethodBody();
        }

        public override int __MethodRVA
        {
            get { return Forwarder.__MethodRVA; }
        }

        public override CallingConventions CallingConvention
        {
            get { return signature.CallingConvention; }
        }

        internal override int ImportTo(IKVM.Reflection.Emit.ModuleBuilder module)
        {
            var method = TryGetForwarder();
            if (method != null)
                return method.ImportTo(module);

            return module.ImportMethodOrField(declaringType, this.Name, this.MethodSignature);
        }

        public override string Name
        {
            get { return name; }
        }

        public override Type DeclaringType
        {
            get { return declaringType.IsModulePseudoType ? null : declaringType; }
        }

        public override Module Module
        {
            get { return declaringType.Module; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as MissingMethod;
            return other != null
                && other.declaringType == declaringType
                && other.name == name
                && other.signature.Equals(signature);
        }

        public override int GetHashCode()
        {
            return declaringType.GetHashCode() ^ name.GetHashCode() ^ signature.GetHashCode();
        }

        internal override MethodBase BindTypeParameters(Type type)
        {
            var forwarder = TryGetForwarder();
            if (forwarder != null)
                return forwarder.BindTypeParameters(type);

            return new GenericMethodInstance(type, this, null);
        }

        public override bool ContainsGenericParameters
        {
            get { return Forwarder.ContainsGenericParameters; }
        }

        public override Type[] GetGenericArguments()
        {
            var method = TryGetForwarder();
            if (method != null)
                return Forwarder.GetGenericArguments();

            if (typeArgs == null)
            {
                typeArgs = new Type[signature.GenericParameterCount];
                for (int i = 0; i < typeArgs.Length; i++)
                    typeArgs[i] = new MissingTypeParameter(this, i);
            }

            return Util.Copy(typeArgs);
        }

        internal override Type GetGenericMethodArgument(int index)
        {
            return GetGenericArguments()[index];
        }

        internal override int GetGenericMethodArgumentCount()
        {
            return Forwarder.GetGenericMethodArgumentCount();
        }

        public override MethodInfo GetGenericMethodDefinition()
        {
            return Forwarder.GetGenericMethodDefinition();
        }

        internal override MethodInfo GetMethodOnTypeDefinition()
        {
            return Forwarder.GetMethodOnTypeDefinition();
        }

        internal override bool HasThis
        {
            get { return (signature.CallingConvention & (CallingConventions.HasThis | CallingConventions.ExplicitThis)) == CallingConventions.HasThis; }
        }

        public override bool IsGenericMethod
        {
            get { return IsGenericMethodDefinition; }
        }

        public override bool IsGenericMethodDefinition
        {
            get { return signature.GenericParameterCount != 0; }
        }

        public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
        {
            var method = TryGetForwarder();
            if (method != null)
                return method.MakeGenericMethod(typeArguments);

            return new GenericMethodInstance(declaringType, this, typeArguments);
        }

        public override int MetadataToken
        {
            get { return Forwarder.MetadataToken; }
        }

        internal override int GetCurrentToken()
        {
            return Forwarder.GetCurrentToken();
        }

        internal override bool IsBaked
        {
            get { return Forwarder.IsBaked; }
        }

    }

}
