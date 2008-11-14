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

namespace IKVM.Reflection.Emit.Impl
{
	public abstract class TypeBase : Type
	{
		public sealed override Assembly Assembly
		{
			get { throw new NotSupportedException(); }
		}

		public abstract override string AssemblyQualifiedName
		{
			get;
		}

		public abstract override Type BaseType
		{
			get;
		}

		public abstract override string FullName
		{
			get;
		}

		public sealed override Guid GUID
		{
			get { throw new NotSupportedException(); }
		}

		protected abstract override TypeAttributes GetAttributeFlagsImpl();

		protected sealed override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		public sealed override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public override Type GetElementType()
		{
			return null;
		}

		public sealed override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public sealed override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public sealed override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public sealed override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public sealed override Type GetInterface(string name, bool ignoreCase)
		{
			throw new NotSupportedException();
		}

		public sealed override Type[] GetInterfaces()
		{
			throw new NotSupportedException();
		}

		public sealed override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		protected abstract override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public sealed override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public sealed override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public sealed override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		protected sealed override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		protected abstract override bool HasElementTypeImpl();

		public sealed override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, System.Globalization.CultureInfo culture, string[] namedParameters)
		{
			throw new NotSupportedException();
		}

		protected abstract override bool IsArrayImpl();

		protected abstract override bool IsByRefImpl();

		protected sealed override bool IsCOMObjectImpl()
		{
			throw new NotSupportedException();
		}

		protected sealed override bool IsPointerImpl()
		{
			return false;
		}

		protected sealed override bool IsPrimitiveImpl()
		{
			return false;
		}

		public sealed override Module Module
		{
			get { throw new NotSupportedException(); }
		}

		public override Type UnderlyingSystemType
		{
			get { return this; }
		}

		public override Type DeclaringType
		{
			get { return null; }
		}

		public sealed override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		public sealed override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException();
		}

		public sealed override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		public override string Name
		{
			get
			{
				string fullname = FullName;
				return fullname.Substring(fullname.LastIndexOf('.') + 1);
			}
		}

		public sealed override string Namespace
		{
			get
			{
				if (IsNested)
				{
					return null;
				}
				string fullname = FullName;
				int index = fullname.LastIndexOf('.');
				return index < 0 ? null : fullname.Substring(0, index);
			}
		}

		public override Type MakeArrayType()
		{
			return ArrayType.Make(this);
		}

		internal abstract ModuleBuilder ModuleBuilder { get; }

		internal abstract TypeToken GetToken();
	}
}
