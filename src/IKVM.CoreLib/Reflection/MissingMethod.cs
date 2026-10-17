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

    /// <summary>
    /// A method that is referenced but could not be found, either because its declaring type is missing or because
    /// the type does not declare it and <see cref="Universe.MissingMemberResolution"/> is enabled. The method is
    /// described by its declaring type, name and signature. Members that need more than the signature forward to the
    /// real method if it can be found later; otherwise they throw <see cref="MissingMemberException"/>.
    /// </summary>
    sealed class MissingMethod : MethodInfo
    {

        readonly Type _declaringType;
        readonly string _name;
        readonly MethodSignature _signature;

        // the real method, once it has been found
        MethodInfo _forwarder;

        // the missing type parameters of a generic method, created on first use
        Type[] _typeArgs;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType">The type that should declare the method.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="signature">The signature of the method.</param>
        internal MissingMethod(Type declaringType, string name, MethodSignature signature)
        {
            _declaringType = declaringType;
            _name = name;
            _signature = signature;
        }

        /// <summary>
        /// Gets the real method.
        /// </summary>
        /// <exception cref="MissingMemberException">The method still cannot be found.</exception>
        MethodInfo Forwarder => TryGetForwarder() ?? throw new MissingMemberException(this);

        /// <summary>
        /// Looks up the real method in the declaring type, if the declaring type is not itself missing, and caches it.
        /// A constructor that is found is unwrapped to its underlying method.
        /// </summary>
        /// <returns>The real method, or <see langword="null"/> if it cannot be found.</returns>
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

        /// <summary>
        /// Gets whether the method is still missing, that is, whether the real method cannot be found.
        /// </summary>
        public override bool __IsMissing => TryGetForwarder() == null;

        /// <summary>
        /// Gets the return type of the method, from its signature.
        /// </summary>
        public override Type ReturnType => _signature.GetReturnType(this);

        /// <summary>
        /// Gets the return parameter of the method. Its type and custom modifiers come from the signature; its other
        /// members need the real method.
        /// </summary>
        public override ParameterInfo ReturnParameter => new ParameterInfoImpl(this, -1);

        /// <inheritdoc />
        internal override MethodSignature MethodSignature => _signature;

        /// <inheritdoc />
        internal override int ParameterCount => _signature.GetParameterCount();

        /// <summary>
        /// A parameter of a <see cref="MissingMethod"/>. Its type and custom modifiers come from the method signature;
        /// its other members forward to the matching parameter of the real method.
        /// </summary>
        sealed class ParameterInfoImpl : ParameterInfo
        {

            readonly MissingMethod _method;
            readonly int _index;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="method">The method the parameter belongs to.</param>
            /// <param name="index">The zero-based position of the parameter, or -1 for the return parameter.</param>
            internal ParameterInfoImpl(MissingMethod method, int index)
            {
                _method = method;
                _index = index;
            }

            /// <summary>
            /// Gets the matching parameter of the real method.
            /// </summary>
            /// <exception cref="MissingMemberException">The method still cannot be found.</exception>
            ParameterInfo Forwarder => _index == -1 ? _method.Forwarder.ReturnParameter : _method.Forwarder.GetParameters()[_index];

            /// <inheritdoc />
            public override string Name => Forwarder.Name;

            /// <summary>
            /// Gets the type of the parameter, from the method signature.
            /// </summary>
            public override Type ParameterType =>
                _index == -1 ? _method._signature.GetReturnType(_method) : _method._signature.GetParameterType(_method, _index);

            /// <inheritdoc />
            public override ParameterAttributes Attributes => Forwarder.Attributes;

            /// <inheritdoc />
            public override int Position => _index;

            /// <inheritdoc />
            public override object RawDefaultValue => Forwarder.RawDefaultValue;

            /// <summary>
            /// Gets the custom modifiers of the parameter, from the method signature.
            /// </summary>
            /// <returns>The custom modifiers.</returns>
            public override CustomModifiers __GetCustomModifiers()
            {
                return _index == -1
                    ? _method._signature.GetReturnTypeCustomModifiers(_method)
                    : _method._signature.GetParameterCustomModifiers(_method, _index);
            }

            /// <inheritdoc />
            public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal) => Forwarder.__TryGetFieldMarshal(out fieldMarshal);

            /// <summary>
            /// Gets the missing method the parameter belongs to.
            /// </summary>
            public override MemberInfo Member => _method;

            /// <inheritdoc />
            public override int MetadataToken => Forwarder.MetadataToken;

            /// <inheritdoc />
            public override Module Module => _method.Module;

            /// <summary>
            /// Returns the string representation of the matching parameter of the real method.
            /// </summary>
            /// <returns>A string that represents the parameter.</returns>
            /// <exception cref="MissingMemberException">The method still cannot be found.</exception>
            public override string ToString() => Forwarder.ToString();

        }

        /// <summary>
        /// Gets the parameters of the method. Their types and custom modifiers come from the signature; their other
        /// members need the real method.
        /// </summary>
        /// <returns>The parameters.</returns>
        public override ParameterInfo[] GetParameters()
        {
            var parameters = new ParameterInfo[_signature.GetParameterCount()];
            for (var i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoImpl(this, i);

            return parameters;
        }

        /// <summary>
        /// Gets the parameter types of the method, from its signature.
        /// </summary>
        /// <returns>The parameter types.</returns>
        internal override Type[] GetParameterTypes()
        {
            var parameterTypes = new Type[_signature.GetParameterCount()];
            for (var i = 0; i < parameterTypes.Length; i++)
                parameterTypes[i] = _signature.GetParameterType(i);

            return parameterTypes;
        }

        /// <inheritdoc />
        public override MethodAttributes Attributes => Forwarder.Attributes;

        /// <inheritdoc />
        public override MethodImplAttributes GetMethodImplementationFlags() => Forwarder.GetMethodImplementationFlags();

        /// <summary>
        /// Gets the calling convention of the method, from its signature.
        /// </summary>
        public override CallingConventions CallingConvention => _signature.CallingConvention;

        /// <summary>
        /// Imports the method into a module being built: the real method if it can be found, otherwise a MemberRef
        /// made from the declaring type, name and signature.
        /// </summary>
        /// <param name="module">The module being built.</param>
        /// <returns>The token of the method in <paramref name="module"/>.</returns>
        internal override int ImportTo(IKVM.Reflection.Emit.ModuleBuilder module)
        {
            var method = TryGetForwarder();
            if (method != null)
                return method.ImportTo(module);

            return module.ImportMethodOrField(_declaringType, Name, MethodSignature);
        }

        /// <inheritdoc />
        public override string Name => _name;

        /// <summary>
        /// Gets the type that declares the method, or <see langword="null"/> if it is a global method declared by the
        /// module pseudo type.
        /// </summary>
        public override Type DeclaringType => _declaringType.IsModulePseudoType ? null : _declaringType;

        /// <inheritdoc />
        public override Module Module => _declaringType.Module;

        /// <summary>
        /// Determines whether the specified object is a missing method with the same declaring type, name and
        /// signature.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns><see langword="true"/> if the methods are equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as MissingMethod;
            return other != null
                && other._declaringType == _declaringType
                && other._name == _name
                && other._signature.Equals(_signature);
        }

        /// <inheritdoc />
        public override int GetHashCode() => _declaringType.GetHashCode() ^ _name.GetHashCode() ^ _signature.GetHashCode();

        /// <summary>
        /// Returns this method as a member of the given constructed generic type: the bound real method if it can be
        /// found, otherwise a <see cref="GenericMethodInstance"/> over this missing method.
        /// </summary>
        /// <param name="type">The constructed type that supplies the type arguments.</param>
        /// <returns>The bound method.</returns>
        internal override MethodBase BindTypeParameters(Type type)
        {
            var forwarder = TryGetForwarder();
            if (forwarder != null)
                return forwarder.BindTypeParameters(type);

            return new GenericMethodInstance(type, this, null);
        }

        /// <inheritdoc />
        public override bool ContainsGenericParameters => Forwarder.ContainsGenericParameters;

        /// <summary>
        /// Gets the type arguments of the real method if it can be found; otherwise one
        /// <see cref="MissingTypeParameter"/> for each generic parameter in the signature.
        /// </summary>
        /// <returns>A copy of the type arguments.</returns>
        public override Type[] GetGenericArguments()
        {
            var method = TryGetForwarder();
            if (method != null)
                return Forwarder.GetGenericArguments();

            if (_typeArgs == null)
            {
                _typeArgs = new Type[_signature.GenericParameterCount];
                for (var i = 0; i < _typeArgs.Length; i++)
                    _typeArgs[i] = new MissingTypeParameter(this, i);
            }

            return Util.Copy(_typeArgs);
        }

        /// <inheritdoc />
        internal override Type GetGenericMethodArgument(int index) => GetGenericArguments()[index];

        /// <inheritdoc />
        public override MethodInfo GetGenericMethodDefinition() => Forwarder.GetGenericMethodDefinition();

        /// <summary>
        /// Gets whether the method is generic, which for a missing method is the same as
        /// <see cref="IsGenericMethodDefinition"/>.
        /// </summary>
        public override bool IsGenericMethod => IsGenericMethodDefinition;

        /// <summary>
        /// Gets whether the signature of the method has generic parameters.
        /// </summary>
        public override bool IsGenericMethodDefinition => _signature.GenericParameterCount != 0;

        /// <summary>
        /// Constructs the method with the given type arguments: the real method if it can be found, otherwise a
        /// <see cref="GenericMethodInstance"/> over this missing method.
        /// </summary>
        /// <param name="typeArguments">The type arguments.</param>
        /// <returns>The constructed method.</returns>
        public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
        {
            var method = TryGetForwarder();
            if (method != null)
                return method.MakeGenericMethod(typeArguments);

            return new GenericMethodInstance(_declaringType, this, typeArguments);
        }

        /// <inheritdoc />
        public override int MetadataToken => Forwarder.MetadataToken;

        /// <inheritdoc />
        internal override int GetCurrentToken() => Forwarder.GetCurrentToken();

        /// <inheritdoc />
        internal override bool IsBaked => Forwarder.IsBaked;

    }

}
