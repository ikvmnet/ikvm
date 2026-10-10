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

        readonly Type _declaringType;
        readonly string _name;
        MethodSignature _signature;
        MethodInfo _forwarder;
        Type[] _typeArgs;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        internal MissingMethod(Type declaringType, string name, MethodSignature signature)
        {
            this._declaringType = declaringType;
            this._name = name;
            this._signature = signature;
        }

        MethodInfo Forwarder => TryGetForwarder() ?? throw new MissingMemberException(this);

        MethodInfo TryGetForwarder()
        {
            if (_forwarder == null && !_declaringType.__IsMissing)
            {
                var mb = _declaringType.FindMethod(_name, _signature);
                var ci = mb as ConstructorInfo;
                if (ci != null)
                    _forwarder = ci.GetMethodInfo();
                else
                    _forwarder = (MethodInfo)mb;
            }

            return _forwarder;
        }

        public override bool __IsMissing
        {
            get { return TryGetForwarder() == null; }
        }

        public override Type ReturnType
        {
            get { return _signature.GetReturnType(this); }
        }

        public override ParameterInfo ReturnParameter
        {
            get { return new ParameterInfoImpl(this, -1); }
        }

        internal override MethodSignature MethodSignature
        {
            get { return _signature; }
        }

        internal override int ParameterCount
        {
            get { return _signature.GetParameterCount(); }
        }

        sealed class ParameterInfoImpl : ParameterInfo
        {

            readonly MissingMethod _method;
            readonly int _index;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="method"></param>
            /// <param name="index"></param>
            internal ParameterInfoImpl(MissingMethod method, int index)
            {
                this._method = method;
                this._index = index;
            }

            ParameterInfo Forwarder
            {
                get { return _index == -1 ? _method.Forwarder.ReturnParameter : _method.Forwarder.GetParameters()[_index]; }
            }

            public override string Name
            {
                get { return Forwarder.Name; }
            }

            public override Type ParameterType
            {
                get { return _index == -1 ? _method._signature.GetReturnType(_method) : _method._signature.GetParameterType(_method, _index); }
            }

            public override ParameterAttributes Attributes
            {
                get { return Forwarder.Attributes; }
            }

            public override int Position
            {
                get { return _index; }
            }

            public override object RawDefaultValue
            {
                get { return Forwarder.RawDefaultValue; }
            }

            public override CustomModifiers __GetCustomModifiers()
            {
                return _index == -1
                    ? _method._signature.GetReturnTypeCustomModifiers(_method)
                    : _method._signature.GetParameterCustomModifiers(_method, _index);
            }

            public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
            {
                return Forwarder.__TryGetFieldMarshal(out fieldMarshal);
            }

            public override MemberInfo Member
            {
                get { return _method; }
            }

            public override int MetadataToken
            {
                get { return Forwarder.MetadataToken; }
            }

            public override Module Module
            {
                get { return _method.Module; }
            }

            public override string ToString()
            {
                return Forwarder.ToString();
            }

        }

        public override ParameterInfo[] GetParameters()
        {
            var parameters = new ParameterInfo[_signature.GetParameterCount()];
            for (int i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoImpl(this, i);

            return parameters;
        }

        internal override Type[] GetParameterTypes()
        {
            var parameterTypes = new Type[_signature.GetParameterCount()];
            for (int i = 0; i < parameterTypes.Length; i++)
                parameterTypes[i] = _signature.GetParameterType(i);

            return parameterTypes;
        }

        public override MethodAttributes Attributes
        {
            get { return Forwarder.Attributes; }
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return Forwarder.GetMethodImplementationFlags();
        }

        public override CallingConventions CallingConvention
        {
            get { return _signature.CallingConvention; }
        }

        internal override int ImportTo(IKVM.Reflection.Emit.ModuleBuilder module)
        {
            var method = TryGetForwarder();
            if (method != null)
                return method.ImportTo(module);

            return module.ImportMethodOrField(_declaringType, this.Name, this.MethodSignature);
        }

        public override string Name
        {
            get { return _name; }
        }

        public override Type DeclaringType
        {
            get { return _declaringType.IsModulePseudoType ? null : _declaringType; }
        }

        public override Module Module
        {
            get { return _declaringType.Module; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as MissingMethod;
            return other != null
                && other._declaringType == _declaringType
                && other._name == _name
                && other._signature.Equals(_signature);
        }

        public override int GetHashCode()
        {
            return _declaringType.GetHashCode() ^ _name.GetHashCode() ^ _signature.GetHashCode();
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

            if (_typeArgs == null)
            {
                _typeArgs = new Type[_signature.GenericParameterCount];
                for (int i = 0; i < _typeArgs.Length; i++)
                    _typeArgs[i] = new MissingTypeParameter(this, i);
            }

            return Util.Copy(_typeArgs);
        }

        internal override Type GetGenericMethodArgument(int index)
        {
            return GetGenericArguments()[index];
        }

        public override MethodInfo GetGenericMethodDefinition()
        {
            return Forwarder.GetGenericMethodDefinition();
        }


        public override bool IsGenericMethod
        {
            get { return IsGenericMethodDefinition; }
        }

        public override bool IsGenericMethodDefinition
        {
            get { return _signature.GenericParameterCount != 0; }
        }

        public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
        {
            var method = TryGetForwarder();
            if (method != null)
                return method.MakeGenericMethod(typeArguments);

            return new GenericMethodInstance(_declaringType, this, typeArguments);
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
