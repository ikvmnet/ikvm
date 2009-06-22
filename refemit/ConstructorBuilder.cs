/*
  Copyright (C) 2008 Jeroen Frijters

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
using System.Reflection;

namespace IKVM.Reflection.Emit
{
	public sealed class ConstructorBuilder : ConstructorInfo
	{
		private readonly MethodBuilder methodBuilder;

		internal ConstructorBuilder(MethodBuilder mb)
		{
			this.methodBuilder = mb;
		}

		public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string strParamName)
		{
			return methodBuilder.DefineParameter(position, attributes, strParamName);
		}

		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			methodBuilder.SetCustomAttribute(customBuilder);
		}

		public void AddDeclarativeSecurity(System.Security.Permissions.SecurityAction securityAction, System.Security.PermissionSet permissionSet)
		{
			methodBuilder.AddDeclarativeSecurity(securityAction, permissionSet);
		}

		public void SetImplementationFlags(MethodImplAttributes attributes)
		{
			methodBuilder.SetImplementationFlags(attributes);
		}

		public ILGenerator GetILGenerator()
		{
			return methodBuilder.GetILGenerator();
		}

		public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		public override MethodAttributes Attributes
		{
			get { return methodBuilder.Attributes; }
		}

		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return methodBuilder.GetMethodImplementationFlags();
		}

		public override ParameterInfo[] GetParameters()
		{
			return methodBuilder.GetParameters();
		}

		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, System.Globalization.CultureInfo culture)
		{
			return methodBuilder.Invoke(obj, invokeAttr, binder, parameters, culture);
		}

		public override RuntimeMethodHandle MethodHandle
		{
			get { return methodBuilder.MethodHandle; }
		}

		public override Type DeclaringType
		{
			get { return methodBuilder.DeclaringType; }
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return methodBuilder.GetCustomAttributes(attributeType, inherit);
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			return methodBuilder.GetCustomAttributes(inherit);
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return methodBuilder.IsDefined(attributeType, inherit);
		}

		public override string Name
		{
			get { return methodBuilder.Name; }
		}

		public override Type ReflectedType
		{
			get { return methodBuilder.ReflectedType; }
		}

		public override int MetadataToken
		{
			get { return methodBuilder.MetadataToken; }
		}

#if NET_4_0
		public override Module Module
		{
			get { return methodBuilder.Module; }
		}
#endif

		internal ModuleBuilder ModuleBuilder
		{
			get { return methodBuilder.ModuleBuilder; }
		}
	}

	sealed class ConstructorInstance : ConstructorInfo, IMethodInstance
	{
		private readonly Type type;
		private readonly ConstructorInfo constructor;

		internal ConstructorInstance(Type type, ConstructorInfo constructor)
		{
			this.type = type;
			this.constructor = constructor;
		}

		public override bool Equals(object obj)
		{
			ConstructorInstance other = obj as ConstructorInstance;
			return other != null && other.type == type && other.constructor == constructor;
		}

		public override int GetHashCode()
		{
			return type.GetHashCode() * 23 + constructor.GetHashCode();
		}

		public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override MethodAttributes Attributes
		{
			get { return constructor.Attributes; }
		}

		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			throw new NotImplementedException();
		}

		public override ParameterInfo[] GetParameters()
		{
			return MethodInstance.ReplaceGenericParameters(type, constructor.GetParameters());
		}

		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override RuntimeMethodHandle MethodHandle
		{
			get { throw new NotImplementedException(); }
		}

		public override Type DeclaringType
		{
			get { return type; }
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override string Name
		{
			get { return constructor.Name; }
		}

		public override Type ReflectedType
		{
			get { return type; }
		}

		MethodBase IMethodInstance.GetMethodOnTypeDefinition()
		{
			return constructor;
		}
	}
}
