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
using System.Collections.Generic;

namespace IKVM.Reflection
{

    /// <summary>
    /// Discovers the attributes of a class constructor and provides access to constructor metadata. A constructor is a
    /// view over the <see cref="MethodInfo"/> that defines it, to which every member delegates.
    /// </summary>
    abstract class ConstructorInfo : MethodBase
    {

        /// <summary>
        /// Represents the name of the class constructor method as it is stored in metadata. This field is read-only.
        /// </summary>
        public static readonly string ConstructorName = ".ctor";

        /// <summary>
        /// Represents the name of the type constructor method as it is stored in metadata. This field is read-only.
        /// </summary>
        public static readonly string TypeConstructorName = ".cctor";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal ConstructorInfo()
        {
        }

        /// <summary>
        /// Returns a string that represents the constructor, formatted as its underlying method.
        /// </summary>
        /// <returns>A string that represents the current constructor.</returns>
        public sealed override string ToString() => GetMethodInfo().ToString();

        /// <summary>
        /// Gets the method that defines this constructor.
        /// </summary>
        /// <returns>The underlying method.</returns>
        internal abstract MethodInfo GetMethodInfo();

        /// <inheritdoc />
        internal override MethodBase BindTypeParameters(Type type) => new ConstructorInfoImpl((MethodInfo)GetMethodInfo().BindTypeParameters(type));

        /// <summary>
        /// Gets a <see cref="MemberTypes"/> value indicating that this member is a constructor.
        /// </summary>
        public sealed override MemberTypes MemberType => MemberTypes.Constructor;

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters => GetMethodInfo().ContainsGenericParameters;

        /// <summary>
        /// Gets the parameters of the constructor, wrapped so that their <see cref="ParameterInfo.Member"/> is this
        /// constructor rather than the underlying method.
        /// </summary>
        /// <returns>An array of type <see cref="ParameterInfo"/> containing information that matches the signature of the constructor.</returns>
        public sealed override ParameterInfo[] GetParameters()
        {
            var parameters = GetMethodInfo().GetParameters();
            for (var i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoWrapper(this, parameters[i]);

            return parameters;
        }

        /// <inheritdoc />
        internal override Type[] GetParameterTypes() => GetMethodInfo().GetParameterTypes();

        /// <inheritdoc />
        public sealed override CallingConventions CallingConvention => GetMethodInfo().CallingConvention;

        /// <inheritdoc />
        public sealed override MethodAttributes Attributes => GetMethodInfo().Attributes;

        /// <inheritdoc />
        public sealed override MethodImplAttributes GetMethodImplementationFlags() => GetMethodInfo().GetMethodImplementationFlags();

        /// <inheritdoc />
        public sealed override Type DeclaringType => GetMethodInfo().DeclaringType;

        /// <inheritdoc />
        public sealed override string Name => GetMethodInfo().Name;

        /// <inheritdoc />
        public sealed override int MetadataToken => GetMethodInfo().MetadataToken;

        /// <inheritdoc />
        public sealed override Module Module => GetMethodInfo().Module;

        /// <inheritdoc />
        public sealed override bool __IsMissing => GetMethodInfo().__IsMissing;

        /// <inheritdoc />
        internal sealed override int ParameterCount => GetMethodInfo().ParameterCount;

        /// <inheritdoc />
        internal sealed override MemberInfo SetReflectedType(Type type) => new ConstructorInfoWithReflectedType(type, this);

        /// <inheritdoc />
        internal sealed override int GetCurrentToken() => GetMethodInfo().GetCurrentToken();

        /// <inheritdoc />
        internal sealed override List<CustomAttributeData> GetPseudoCustomAttributes(Type attributeType) =>
            GetMethodInfo().GetPseudoCustomAttributes(attributeType);

        /// <inheritdoc />
        internal sealed override bool IsBaked => GetMethodInfo().IsBaked;

        /// <inheritdoc />
        internal sealed override MethodSignature MethodSignature => GetMethodInfo().MethodSignature;

        /// <inheritdoc />
        internal sealed override int ImportTo(Emit.ModuleBuilder module) => GetMethodInfo().ImportTo(module);

    }

}
