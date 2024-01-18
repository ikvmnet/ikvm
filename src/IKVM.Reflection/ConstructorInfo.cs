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

    public abstract class ConstructorInfo : MethodBase
    {

        public static readonly string ConstructorName = ".ctor";
        public static readonly string TypeConstructorName = ".cctor";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal ConstructorInfo()
		{

		}

		public sealed override string ToString()
		{
			return GetMethodInfo().ToString();
		}

		internal abstract MethodInfo GetMethodInfo();

		internal override MethodBase BindTypeParameters(Type type)
		{
			return new ConstructorInfoImpl((MethodInfo)GetMethodInfo().BindTypeParameters(type));
		}

		public sealed override MethodBase __GetMethodOnTypeDefinition()
		{
			return new ConstructorInfoImpl((MethodInfo)GetMethodInfo().__GetMethodOnTypeDefinition());
		}

		public sealed override MemberTypes MemberType
		{
			get { return MemberTypes.Constructor; }
		}

		public sealed override int __MethodRVA
		{
			get { return GetMethodInfo().__MethodRVA; }
		}

		public sealed override bool ContainsGenericParameters
		{
			get { return GetMethodInfo().ContainsGenericParameters; }
		}

		public ParameterInfo __ReturnParameter
		{
			get { return new ParameterInfoWrapper(this, GetMethodInfo().ReturnParameter); }
		}

		public sealed override ParameterInfo[] GetParameters()
		{
			var parameters = GetMethodInfo().GetParameters();
			for (int i = 0; i < parameters.Length; i++)
				parameters[i] = new ParameterInfoWrapper(this, parameters[i]);

			return parameters;
		}

		public sealed override CallingConventions CallingConvention
		{
			get { return GetMethodInfo().CallingConvention; }
		}

		public sealed override MethodAttributes Attributes
		{
			get { return GetMethodInfo().Attributes; }
		}

		public sealed override MethodImplAttributes GetMethodImplementationFlags()
		{
			return GetMethodInfo().GetMethodImplementationFlags();
		}

		public sealed override Type DeclaringType
		{
			get { return GetMethodInfo().DeclaringType; }
		}

		public sealed override string Name
		{
			get { return GetMethodInfo().Name; }
		}

		public sealed override int MetadataToken
		{
			get { return GetMethodInfo().MetadataToken; }
		}

		public sealed override Module Module
		{
			get { return GetMethodInfo().Module; }
		}

		public sealed override MethodBody GetMethodBody()
		{
			return GetMethodInfo().GetMethodBody();
		}

		public sealed override bool __IsMissing
		{
			get { return GetMethodInfo().__IsMissing; }
		}

		internal sealed override int ParameterCount
		{
			get { return GetMethodInfo().ParameterCount; }
		}

		internal sealed override MemberInfo SetReflectedType(Type type)
		{
			return new ConstructorInfoWithReflectedType(type, this);
		}

		internal sealed override int GetCurrentToken()
		{
			return GetMethodInfo().GetCurrentToken();
		}

		internal sealed override List<CustomAttributeData> GetPseudoCustomAttributes(Type attributeType)
		{
			return GetMethodInfo().GetPseudoCustomAttributes(attributeType);
		}

		internal sealed override bool IsBaked
		{
			get { return GetMethodInfo().IsBaked; }
		}

		internal sealed override MethodSignature MethodSignature
		{
			get { return GetMethodInfo().MethodSignature; }
		}

		internal sealed override int ImportTo(Emit.ModuleBuilder module)
		{
			return GetMethodInfo().ImportTo(module);
		}

	}

}
