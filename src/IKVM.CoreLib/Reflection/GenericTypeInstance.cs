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

		private readonly Type _type;
		private readonly Type[] _args;
		private readonly CustomModifiers[] _mods;
		private Type _baseType;
		private int _token;

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
			this._type = type;
			this._args = args;
			this._mods = mods;
		}

		public override bool Equals(object o)
		{
			GenericTypeInstance gt = o as GenericTypeInstance;
			return gt != null && gt._type.Equals(_type) && Util.ArrayEquals(gt._args, _args)
				&& Util.ArrayEquals(gt._mods, _mods);
		}

		public override int GetHashCode()
		{
			return _type.GetHashCode() * 3 ^ Util.GetHashCode(_args);
		}

		public override string AssemblyQualifiedName
		{
			get
			{
				string fn = FullName;
				return fn == null ? null : fn + ", " + _type.Assembly.FullName;
			}
		}

		public override Type BaseType
		{
			get
			{
				if (_baseType == null)
				{
					Type rawBaseType = _type.BaseType;
					if (rawBaseType == null)
					{
						_baseType = rawBaseType;
					}
					else
					{
						_baseType = rawBaseType.BindTypeParameters(this);
					}
				}
				return _baseType;
			}
		}

		protected override bool IsValueTypeImpl
		{
			get { return _type.IsValueType; }
		}

		public override bool IsVisible
		{
			get
			{
				if (base.IsVisible)
				{
					foreach (Type arg in _args)
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
			get { return _type.DeclaringType; }
		}

		public override TypeAttributes Attributes
		{
			get { return _type.Attributes; }
		}

		internal override void CheckBaked()
		{
			_type.CheckBaked();
		}

		public override FieldInfo[] __GetDeclaredFields()
		{
			FieldInfo[] fields = _type.__GetDeclaredFields();
			for (int i = 0; i < fields.Length; i++)
			{
				fields[i] = fields[i].BindTypeParameters(this);
			}
			return fields;
		}

		public override Type[] __GetDeclaredInterfaces()
		{
			Type[] interfaces = _type.__GetDeclaredInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				interfaces[i] = interfaces[i].BindTypeParameters(this);
			}
			return interfaces;
		}

		public override MethodBase[] __GetDeclaredMethods()
		{
			MethodBase[] methods = _type.__GetDeclaredMethods();
			for (int i = 0; i < methods.Length; i++)
			{
				methods[i] = methods[i].BindTypeParameters(this);
			}
			return methods;
		}

		public override Type[] __GetDeclaredTypes()
		{
			return _type.__GetDeclaredTypes();
		}

		public override EventInfo[] __GetDeclaredEvents()
		{
			EventInfo[] events = _type.__GetDeclaredEvents();
			for (int i = 0; i < events.Length; i++)
			{
				events[i] = events[i].BindTypeParameters(this);
			}
			return events;
		}

		public override PropertyInfo[] __GetDeclaredProperties()
		{
			PropertyInfo[] properties = _type.__GetDeclaredProperties();
			for (int i = 0; i < properties.Length; i++)
			{
				properties[i] = properties[i].BindTypeParameters(this);
			}
			return properties;
		}

		public override __MethodImplMap __GetMethodImplMap()
		{
			__MethodImplMap map = _type.__GetMethodImplMap();
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
			get { return _type.Namespace; }
		}

		public override string Name
		{
			get { return _type.Name; }
		}

		public override string FullName
		{
			get
			{
				if (!this.__ContainsMissingType && this.ContainsGenericParameters)
				{
					return null;
				}
				StringBuilder sb = new StringBuilder(this._type.FullName);
				sb.Append('[');
				string sep = "";
				foreach (Type type in _args)
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
			StringBuilder sb = new StringBuilder(_type.FullName);
			sb.Append('[');
			string sep = "";
			foreach (Type arg in _args)
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
			get { return _type.Module; }
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
			return _type;
		}

		public override Type[] GetGenericArguments()
		{
			return Util.Copy(_args);
		}

		public override CustomModifiers[] __GetGenericArgumentsCustomModifiers()
		{
			return _mods != null ? (CustomModifiers[])_mods.Clone() : new CustomModifiers[_args.Length];
		}

		internal override Type GetGenericTypeArgument(int index)
		{
			return _args[index];
		}

		public override bool ContainsGenericParameters
		{
			get
			{
				foreach (Type type in _args)
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
			get { return _type.__ContainsMissingType || ContainsMissingType(_args); }
		}

		internal override int GetModuleBuilderToken()
		{
			if (_token == 0)
			{
				_token = ((ModuleBuilder)_type.Module).ImportType(this);
			}
			return _token;
		}

		internal override Type BindTypeParameters(IGenericBinder binder)
		{
			for (int i = 0; i < _args.Length; i++)
			{
				Type xarg = _args[i].BindTypeParameters(binder);
				if (!ReferenceEquals(xarg, _args[i]))
				{
					Type[] xargs = new Type[_args.Length];
					Array.Copy(_args, xargs, i);
					xargs[i++] = xarg;
					for (; i < _args.Length; i++)
					{
						xargs[i] = _args[i].BindTypeParameters(binder);
					}
					return Make(_type, xargs, null);
				}
			}
			return this;
		}

		internal override int GetCurrentToken()
		{
			return _type.GetCurrentToken();
		}

		internal override bool IsBaked
		{
			get { return _type.IsBaked; }
		}

	}

}
