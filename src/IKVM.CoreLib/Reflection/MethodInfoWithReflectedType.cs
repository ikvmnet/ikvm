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
using System.Diagnostics;

namespace IKVM.Reflection
{

    /// <summary>
    /// Wraps a method obtained through a type other than the one that declares it, such as an inherited method
    /// returned from a derived type. The wrapper delegates to the underlying method but remembers the type it was
    /// obtained through, which takes part in equality, and wraps parameters so that they report the wrapper as their
    /// member.
    /// </summary>
    sealed class MethodInfoWithReflectedType : MethodInfo
    {

        readonly Type _reflectedType;
        readonly MethodInfo _method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reflectedType">The type the method was obtained through; must differ from the declaring type.</param>
        /// <param name="method">The underlying method.</param>
        internal MethodInfoWithReflectedType(Type reflectedType, MethodInfo method)
        {
            Debug.Assert(reflectedType != method.DeclaringType);
            _reflectedType = reflectedType;
            _method = method;
        }

        /// <summary>
        /// Determines whether the specified object is a wrapper of the same method obtained through the same type.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> wraps the same method and reflected type; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as MethodInfoWithReflectedType;
            return other != null
                && other._reflectedType == _reflectedType
                && other._method == _method;
        }

        /// <inheritdoc />
        public override int GetHashCode() => _reflectedType.GetHashCode() ^ _method.GetHashCode();

        /// <inheritdoc />
        internal override MethodSignature MethodSignature => _method.MethodSignature;

        /// <inheritdoc />
        internal override int ParameterCount => _method.ParameterCount;

        /// <summary>
        /// Gets the parameters of the underlying method, each wrapped so that its member is this wrapper.
        /// </summary>
        /// <returns>An array containing the parameters of the method.</returns>
        public override ParameterInfo[] GetParameters()
        {
            var parameters = _method.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoWrapper(this, parameters[i]);

            return parameters;
        }

        /// <inheritdoc />
        internal override Type[] GetParameterTypes() => _method.GetParameterTypes();

        /// <inheritdoc />
        public override MethodAttributes Attributes => _method.Attributes;

        /// <inheritdoc />
        public override MethodImplAttributes GetMethodImplementationFlags() => _method.GetMethodImplementationFlags();

        /// <inheritdoc />
        public override CallingConventions CallingConvention => _method.CallingConvention;

        /// <inheritdoc />
        public override Type ReturnType => _method.ReturnType;

        /// <summary>
        /// Gets the return parameter of the underlying method, wrapped so that its member is this wrapper.
        /// </summary>
        public override ParameterInfo ReturnParameter => new ParameterInfoWrapper(this, _method.ReturnParameter);

        /// <summary>
        /// Substitutes the elements of an array of types for the type parameters of the current generic method
        /// definition, and keeps the reflected type on the result.
        /// </summary>
        /// <param name="typeArguments">An array of types to be substituted for the type parameters of the current generic method definition.</param>
        /// <returns>The constructed method, obtained through the same reflected type as this one.</returns>
        public override MethodInfo MakeGenericMethod(params Type[] typeArguments) =>
            SetReflectedType(_method.MakeGenericMethod(typeArguments), _reflectedType);

        /// <inheritdoc />
        public override MethodInfo GetGenericMethodDefinition() => _method.GetGenericMethodDefinition();

        /// <inheritdoc />
        public override string ToString() => _method.ToString();

        /// <inheritdoc />
        internal override Type GetGenericMethodArgument(int index) => _method.GetGenericMethodArgument(index);

        /// <inheritdoc />
        public override Module Module => _method.Module;

        /// <inheritdoc />
        public override Type DeclaringType => _method.DeclaringType;

        /// <inheritdoc />
        public override string Name => _method.Name;

        /// <inheritdoc />
        internal override int ImportTo(IKVM.Reflection.Emit.ModuleBuilder module) => _method.ImportTo(module);

        /// <inheritdoc />
        public override bool __IsMissing => _method.__IsMissing;

        /// <inheritdoc />
        internal override MethodBase BindTypeParameters(Type type) => _method.BindTypeParameters(type);

        /// <inheritdoc />
        public override bool ContainsGenericParameters => _method.ContainsGenericParameters;

        /// <inheritdoc />
        public override Type[] GetGenericArguments() => _method.GetGenericArguments();

        /// <inheritdoc />
        public override bool IsGenericMethod => _method.IsGenericMethod;

        /// <inheritdoc />
        public override bool IsGenericMethodDefinition => _method.IsGenericMethodDefinition;

        /// <inheritdoc />
        public override int MetadataToken => _method.MetadataToken;

        /// <inheritdoc />
        internal override int GetCurrentToken() => _method.GetCurrentToken();

        /// <inheritdoc />
        internal override bool IsBaked => _method.IsBaked;

    }

}
