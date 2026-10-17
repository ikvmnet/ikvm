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
using System;

namespace IKVM.Reflection
{

    /// <summary>
    /// Represents both generic method instantiations and methods on generic type instantiations. This means that it
    /// can be a generic method definition (a generic method on a constructed type, not yet instantiated) as well as a
    /// generic method instance.
    /// </summary>
    sealed class GenericMethodInstance : MethodInfo
    {

        readonly Type _declaringType;
        readonly MethodInfo _method;

        // the method type arguments, or null if the method is not instantiated
        readonly Type[] _methodArgs;
        MethodSignature _lazyMethodSignature;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType">The type that declares the method, which may be a constructed generic type.</param>
        /// <param name="method">The method as declared on its generic type definition; never a <see cref="GenericMethodInstance"/>.</param>
        /// <param name="methodArgs">The method type arguments, or <see langword="null"/> if the method is not instantiated.</param>
        internal GenericMethodInstance(Type declaringType, MethodInfo method, Type[] methodArgs)
        {
            System.Diagnostics.Debug.Assert(method is not GenericMethodInstance);
            _declaringType = declaringType;
            _method = method;
            _methodArgs = methodArgs;
        }

        /// <summary>
        /// Determines whether the specified object is a generic method instance of the same method, declaring type and
        /// method type arguments.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns><see langword="true"/> if the methods are equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as GenericMethodInstance;
            return other != null
                && other._method.Equals(_method)
                && other._declaringType.Equals(_declaringType)
                && Util.ArrayEquals(other._methodArgs, _methodArgs);
        }

        /// <inheritdoc />
        public override int GetHashCode() => _declaringType.GetHashCode() * 33 ^ _method.GetHashCode() ^ Util.GetHashCode(_methodArgs);

        /// <summary>
        /// Gets the return type of the method, with the type parameters bound to the type arguments.
        /// </summary>
        public override Type ReturnType => _method.ReturnType.BindTypeParameters(this);

        /// <summary>
        /// Gets the return parameter of the method, with the type parameters in its type bound to the type arguments.
        /// </summary>
        public override ParameterInfo ReturnParameter => new GenericParameterInfoImpl(this, _method.ReturnParameter);

        /// <summary>
        /// Gets the parameters of the method, with the type parameters in their types bound to the type arguments.
        /// </summary>
        /// <returns>The parameters.</returns>
        public override ParameterInfo[] GetParameters()
        {
            var parameters = _method.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
                parameters[i] = new GenericParameterInfoImpl(this, parameters[i]);

            return parameters;
        }

        /// <summary>
        /// Gets the parameter types of the method, bound against the underlying method.
        /// </summary>
        /// <returns>The parameter types.</returns>
        internal override Type[] GetParameterTypes()
        {
            var parameterTypes = _method.GetParameterTypes();
            for (var i = 0; i < parameterTypes.Length; i++)
                parameterTypes[i] = parameterTypes[i].BindTypeParameters(this);

            return parameterTypes;
        }

        /// <inheritdoc />
        internal override int ParameterCount => _method.ParameterCount;

        /// <inheritdoc />
        public override CallingConventions CallingConvention => _method.CallingConvention;

        /// <inheritdoc />
        public override MethodAttributes Attributes => _method.Attributes;

        /// <inheritdoc />
        public override MethodImplAttributes GetMethodImplementationFlags() => _method.GetMethodImplementationFlags();

        /// <inheritdoc />
        public override string Name => _method.Name;

        /// <summary>
        /// Gets the type that declares the method, or <see langword="null"/> if it is a global method declared by the
        /// module pseudo type.
        /// </summary>
        public override Type DeclaringType => _declaringType.IsModulePseudoType ? null : _declaringType;

        /// <inheritdoc />
        public override Module Module => _method.Module;

        /// <inheritdoc />
        public override int MetadataToken => _method.MetadataToken;

        /// <summary>
        /// Instantiates the underlying method on the same declaring type with the given type arguments.
        /// </summary>
        /// <param name="typeArguments">The method type arguments.</param>
        /// <returns>The constructed method.</returns>
        public override MethodInfo MakeGenericMethod(params Type[] typeArguments) =>
            new GenericMethodInstance(_declaringType, _method, typeArguments);

        /// <inheritdoc />
        public override bool IsGenericMethod => _method.IsGenericMethod;

        /// <summary>
        /// Gets whether the method is a generic method definition, which is the case when the underlying method is one
        /// and it has not been instantiated.
        /// </summary>
        public override bool IsGenericMethodDefinition => _method.IsGenericMethodDefinition && _methodArgs == null;

        /// <summary>
        /// Gets whether the declaring type or any of the method type arguments contains unassigned generic type
        /// parameters.
        /// </summary>
        public override bool ContainsGenericParameters
        {
            get
            {
                if (_declaringType.ContainsGenericParameters)
                    return true;

                // a generic method definition that has not been instantiated has its own type parameters
                if (_methodArgs == null && _method.IsGenericMethodDefinition)
                    return true;

                if (_methodArgs != null)
                    foreach (var type in _methodArgs)
                        if (type.ContainsGenericParameters)
                            return true;

                return false;
            }
        }

        /// <summary>
        /// Returns the generic method definition this method is constructed from: this method if it is a definition,
        /// the uninstantiated method on the same constructed type, or the underlying method.
        /// </summary>
        /// <returns>The generic method definition.</returns>
        /// <exception cref="InvalidOperationException">The method is not generic.</exception>
        public override MethodInfo GetGenericMethodDefinition()
        {
            if (IsGenericMethod)
            {
                if (IsGenericMethodDefinition)
                    return this;
                else if (_declaringType.IsConstructedGenericType)
                    return new GenericMethodInstance(_declaringType, _method, null);
                else
                    return _method;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the method type arguments, or the type parameters of the underlying method if it has not been
        /// instantiated.
        /// </summary>
        /// <returns>A copy of the type arguments.</returns>
        public override Type[] GetGenericArguments()
        {
            if (_methodArgs == null)
                return _method.GetGenericArguments();
            else
                return (Type[])_methodArgs.Clone();
        }

        /// <inheritdoc />
        internal override Type GetGenericMethodArgument(int index)
        {
            if (_methodArgs == null)
                return _method.GetGenericMethodArgument(index);
            else
                return _methodArgs[index];
        }

        /// <summary>
        /// Imports the method into a module being built: as a MemberRef on the declaring type if it has not been
        /// instantiated, otherwise as a MethodSpec.
        /// </summary>
        /// <param name="module">The module being built.</param>
        /// <returns>The token of the method in <paramref name="module"/>.</returns>
        internal override int ImportTo(Emit.ModuleBuilder module)
        {
            if (_methodArgs == null)
                return module.ImportMethodOrField(_declaringType, _method.Name, _method.MethodSignature);
            else
                return module.ImportMethodSpec(_declaringType, _method, _methodArgs);
        }

        /// <summary>
        /// Gets the signature of the method, bound to the type arguments of the declaring type and the method. It is
        /// computed on first use.
        /// </summary>
        internal override MethodSignature MethodSignature => _lazyMethodSignature ??= _method.MethodSignature.Bind(_declaringType, _methodArgs);

        /// <summary>
        /// Returns this method as a member of the declaring type bound to the given type arguments. Only valid for a
        /// method that has not been instantiated.
        /// </summary>
        /// <param name="type">The constructed type that supplies the type arguments.</param>
        /// <returns>The bound method.</returns>
        internal override MethodBase BindTypeParameters(Type type)
        {
            System.Diagnostics.Debug.Assert(_methodArgs == null);
            return new GenericMethodInstance(_declaringType.BindTypeParameters(type), _method, null);
        }

        /// <inheritdoc />
        internal override int GetCurrentToken() => _method.GetCurrentToken();

        /// <inheritdoc />
        internal override bool IsBaked => _method.IsBaked;

    }

}
