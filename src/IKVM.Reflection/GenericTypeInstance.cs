/*
  Copyright (C) 2009-2015 Jeroen Frijters

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
using System.Text;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection
{

    sealed class GenericTypeInstance : TypeInfo
	{

		private readonly Type type;
		private readonly Type[] args;
		private readonly CustomModifiers[] mods;
		private Type baseType;
		private int token;

		internal static Type Make(Type type, Type[] typeArguments, CustomModifiers[] mods)
		{
			bool identity = true;
			if (type is TypeBuilder || type is BakedType || type.__IsMissing)
			{
				// a TypeBuiler identity must be instantiated
				identity = false;
			}
			else
			{
				// we must not instantiate the identity instance, because typeof(Foo<>).MakeGenericType(typeof(Foo<>).GetGenericArguments()) == typeof(Foo<>)
				for (int i = 0; i < typeArguments.Length; i++)
				{
					if (typeArguments[i] != type.GetGenericTypeArgument(i)
						|| !IsEmpty(mods, i))
					{
						identity = false;
						break;
					}
				}
			}
			if (identity)
			{
				return type;
			}
			else
			{
				return type.Universe.CanonicalizeType(new GenericTypeInstance(type, typeArguments, mods));
			}
		}

		private static bool IsEmpty(CustomModifiers[] mods, int i)
		{
			// we need to be extra careful, because mods doesn't not need to be in canonical format
			// (Signature.ReadGenericInst() calls Make() directly, without copying the modifier arrays)
			return mods == null || mods[i].IsEmpty;
		}

		private GenericTypeInstance(Type type, Type[] args, CustomModifiers[] mods)
		{
			this.type = type;
			this.args = args;
			this.mods = mods;
		}

		public override bool Equals(object o)
		{
			GenericTypeInstance gt = o as GenericTypeInstance;
			return gt != null && gt.type.Equals(type) && Util.ArrayEquals(gt.args, args)
				&& Util.ArrayEquals(gt.mods, mods);
		}

		public override int GetHashCode()
		{
			return type.GetHashCode() * 3 ^ Util.GetHashCode(args);
		}

		public override string AssemblyQualifiedName
		{
			get
			{
				string fn = FullName;
				return fn == null ? null : fn + ", " + type.Assembly.FullName;
			}
		}

		public override Type BaseType
		{
			get
			{
				if (baseType == null)
				{
					Type rawBaseType = type.BaseType;
					if (rawBaseType == null)
					{
						baseType = rawBaseType;
					}
					else
					{
						baseType = rawBaseType.BindTypeParameters(this);
					}
				}
				return baseType;
			}
		}

		protected override bool IsValueTypeImpl
		{
			get { return type.IsValueType; }
		}

		public override bool IsVisible
		{
			get
			{
				if (base.IsVisible)
				{
					foreach (Type arg in args)
					{
						if (!arg.IsVisible)
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
		}

		public override Type DeclaringType
		{
			get { return type.DeclaringType; }
		}

		public override TypeAttributes Attributes
		{
			get { return type.Attributes; }
		}

		internal override void CheckBaked()
		{
			type.CheckBaked();
		}

		public override FieldInfo[] __GetDeclaredFields()
		{
			FieldInfo[] fields = type.__GetDeclaredFields();
			for (int i = 0; i < fields.Length; i++)
			{
				fields[i] = fields[i].BindTypeParameters(this);
			}
			return fields;
		}

		public override Type[] __GetDeclaredInterfaces()
		{
			Type[] interfaces = type.__GetDeclaredInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				interfaces[i] = interfaces[i].BindTypeParameters(this);
			}
			return interfaces;
		}

		public override MethodBase[] __GetDeclaredMethods()
		{
			MethodBase[] methods = type.__GetDeclaredMethods();
			for (int i = 0; i < methods.Length; i++)
			{
				methods[i] = methods[i].BindTypeParameters(this);
			}
			return methods;
		}

		public override Type[] __GetDeclaredTypes()
		{
			return type.__GetDeclaredTypes();
		}

		public override EventInfo[] __GetDeclaredEvents()
		{
			EventInfo[] events = type.__GetDeclaredEvents();
			for (int i = 0; i < events.Length; i++)
			{
				events[i] = events[i].BindTypeParameters(this);
			}
			return events;
		}

		public override PropertyInfo[] __GetDeclaredProperties()
		{
			PropertyInfo[] properties = type.__GetDeclaredProperties();
			for (int i = 0; i < properties.Length; i++)
			{
				properties[i] = properties[i].BindTypeParameters(this);
			}
			return properties;
		}

		public override __MethodImplMap __GetMethodImplMap()
		{
			__MethodImplMap map = type.__GetMethodImplMap();
			map.TargetType = this;
			for (int i = 0; i < map.MethodBodies.Length; i++)
			{
				map.MethodBodies[i] = (MethodInfo)map.MethodBodies[i].BindTypeParameters(this);
				for (int j = 0; j < map.MethodDeclarations[i].Length; j++)
				{
					Type interfaceType = map.MethodDeclarations[i][j].DeclaringType;
					if (interfaceType.IsGenericType)
					{
						map.MethodDeclarations[i][j] = (MethodInfo)map.MethodDeclarations[i][j].BindTypeParameters(this);
					}
				}
			}
			return map;
		}

		public override string Namespace
		{
			get { return type.Namespace; }
		}

		public override string Name
		{
			get { return type.Name; }
		}

		public override string FullName
		{
			get
			{
				if (!this.__ContainsMissingType && this.ContainsGenericParameters)
				{
					return null;
				}
				StringBuilder sb = new StringBuilder(this.type.FullName);
				sb.Append('[');
				string sep = "";
				foreach (Type type in args)
				{
					sb.Append(sep).Append('[').Append(type.FullName).Append(", ").Append(type.Assembly.FullName.Replace("]", "\\]")).Append(']');
					sep = ",";
				}
				sb.Append(']');
				return sb.ToString();
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(type.FullName);
			sb.Append('[');
			string sep = "";
			foreach (Type arg in args)
			{
				sb.Append(sep);
				sb.Append(arg);
				sep = ",";
			}
			sb.Append(']');
			return sb.ToString();
		}

		public override Module Module
		{
			get { return type.Module; }
		}

		public override bool IsGenericType
		{
			get { return true; }
		}

		public override bool IsConstructedGenericType
		{
			get { return true; }
		}

		public override Type GetGenericTypeDefinition()
		{
			return type;
		}

		public override Type[] GetGenericArguments()
		{
			return Util.Copy(args);
		}

		public override CustomModifiers[] __GetGenericArgumentsCustomModifiers()
		{
			return mods != null ? (CustomModifiers[])mods.Clone() : new CustomModifiers[args.Length];
		}

		internal override Type GetGenericTypeArgument(int index)
		{
			return args[index];
		}

		public override bool ContainsGenericParameters
		{
			get
			{
				foreach (Type type in args)
				{
					if (type.ContainsGenericParameters)
					{
						return true;
					}
				}
				return false;
			}
		}

		protected override bool ContainsMissingTypeImpl
		{
			get { return type.__ContainsMissingType || ContainsMissingType(args); }
		}

		public override bool __GetLayout(out int packingSize, out int typeSize)
		{
			return type.__GetLayout(out packingSize, out typeSize);
		}

		internal override int GetModuleBuilderToken()
		{
			if (token == 0)
			{
				token = ((ModuleBuilder)type.Module).ImportType(this);
			}
			return token;
		}

		internal override Type BindTypeParameters(IGenericBinder binder)
		{
			for (int i = 0; i < args.Length; i++)
			{
				Type xarg = args[i].BindTypeParameters(binder);
				if (!ReferenceEquals(xarg, args[i]))
				{
					Type[] xargs = new Type[args.Length];
					Array.Copy(args, xargs, i);
					xargs[i++] = xarg;
					for (; i < args.Length; i++)
					{
						xargs[i] = args[i].BindTypeParameters(binder);
					}
					return Make(type, xargs, null);
				}
			}
			return this;
		}

		internal override int GetCurrentToken()
		{
			return type.GetCurrentToken();
		}

		internal override bool IsBaked
		{
			get { return type.IsBaked; }
		}

	}

}
