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

    abstract class ConstructorInfo : MethodBase
    {

        public static readonly string ConstructorName = ".ctor";
        public static readonly string TypeConstructorName = ".cctor";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal ConstructorInfo()
        {
        }

        public sealed override string ToString() => GetMethodInfo().ToString();

        internal abstract MethodInfo GetMethodInfo();

        internal override MethodBase BindTypeParameters(Type type) => new ConstructorInfoImpl((MethodInfo)GetMethodInfo().BindTypeParameters(type));

        public sealed override MemberTypes MemberType => MemberTypes.Constructor;

        public sealed override bool ContainsGenericParameters => GetMethodInfo().ContainsGenericParameters;

        public sealed override ParameterInfo[] GetParameters()
        {
            var parameters = GetMethodInfo().GetParameters();
            for (var i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoWrapper(this, parameters[i]);

            return parameters;
        }

        internal override Type[] GetParameterTypes() => GetMethodInfo().GetParameterTypes();

        public sealed override CallingConventions CallingConvention => GetMethodInfo().CallingConvention;

        public sealed override MethodAttributes Attributes => GetMethodInfo().Attributes;

        public sealed override MethodImplAttributes GetMethodImplementationFlags() => GetMethodInfo().GetMethodImplementationFlags();

        public sealed override Type DeclaringType => GetMethodInfo().DeclaringType;

        public sealed override string Name => GetMethodInfo().Name;

        public sealed override int MetadataToken => GetMethodInfo().MetadataToken;

        public sealed override Module Module => GetMethodInfo().Module;

        public sealed override bool __IsMissing => GetMethodInfo().__IsMissing;

        internal sealed override int ParameterCount => GetMethodInfo().ParameterCount;

        internal sealed override MemberInfo SetReflectedType(Type type) => new ConstructorInfoWithReflectedType(type, this);

        internal sealed override int GetCurrentToken() => GetMethodInfo().GetCurrentToken();

        internal sealed override List<CustomAttributeData> GetPseudoCustomAttributes(Type attributeType) =>
            GetMethodInfo().GetPseudoCustomAttributes(attributeType);

        internal sealed override bool IsBaked => GetMethodInfo().IsBaked;

        internal sealed override MethodSignature MethodSignature => GetMethodInfo().MethodSignature;

        internal sealed override int ImportTo(Emit.ModuleBuilder module) => GetMethodInfo().ImportTo(module);

    }

}
