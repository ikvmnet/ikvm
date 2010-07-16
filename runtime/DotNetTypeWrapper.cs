/*
  Copyright (C) 2002-2009 Jeroen Frijters

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
using System.Collections.Generic;
#if STATIC_COMPILER || STUB_GENERATOR
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;
using IKVM.Attributes;

namespace IKVM.Internal
{
	sealed class DotNetTypeWrapper : TypeWrapper
	{
		private const string NamePrefix = "cli.";
		internal const string DelegateInterfaceSuffix = "$Method";
		internal const string AttributeAnnotationSuffix = "$Annotation";
		internal const string AttributeAnnotationReturnValueSuffix = "$__ReturnValue";
		internal const string AttributeAnnotationMultipleSuffix = "$__Multiple";
		internal const string EnumEnumSuffix = "$__Enum";
		internal const string GenericEnumEnumTypeName = "ikvm.internal.EnumEnum`1";
		internal const string GenericDelegateInterfaceTypeName = "ikvm.internal.DelegateInterface`1";
		internal const string GenericAttributeAnnotationTypeName = "ikvm.internal.AttributeAnnotation`1";
		internal const string GenericAttributeAnnotationReturnValueTypeName = "ikvm.internal.AttributeAnnotationReturnValue`1";
		internal const string GenericAttributeAnnotationMultipleTypeName = "ikvm.internal.AttributeAnnotationMultiple`1";
		private static readonly Dictionary<Type, TypeWrapper> types = new Dictionary<Type, TypeWrapper>();
		private readonly Type type;
		private TypeWrapper[] innerClasses;
		private TypeWrapper outerClass;
		private TypeWrapper[] interfaces;

		private static Modifiers GetModifiers(Type type)
		{
			Modifiers modifiers = 0;
			if (type.IsPublic)
			{
				modifiers |= Modifiers.Public;
			}
			else if (type.IsNestedPublic)
			{
				modifiers |= Modifiers.Static;
				if (type.IsVisible)
				{
					modifiers |= Modifiers.Public;
				}
			}
			else if (type.IsNestedPrivate)
			{
				modifiers |= Modifiers.Private | Modifiers.Static;
			}
			else if (type.IsNestedFamily || type.IsNestedFamORAssem)
			{
				modifiers |= Modifiers.Protected | Modifiers.Static;
			}
			else if (type.IsNestedAssembly || type.IsNestedFamANDAssem)
			{
				modifiers |= Modifiers.Static;
			}

			if (type.IsSealed)
			{
				modifiers |= Modifiers.Final;
			}
			else if (type.IsAbstract) // we can't be abstract if we're final
			{
				modifiers |= Modifiers.Abstract;
			}
			if (type.IsInterface)
			{
				modifiers |= Modifiers.Interface;
			}
			return modifiers;
		}

		// NOTE when this is called on a remapped type, the "warped" underlying type name is returned.
		// E.g. GetName(typeof(object)) returns "cli.System.Object".
		internal static string GetName(Type type)
		{
			Debug.Assert(!type.Name.EndsWith("[]") && !AttributeHelper.IsJavaModule(type.Module));

			string name = type.FullName;

			if (name == null)
			{
				// generic type parameters don't have a full name
				return null;
			}

			if (type.IsGenericType && !type.ContainsGenericParameters)
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append(MangleTypeName(type.GetGenericTypeDefinition().FullName));
				sb.Append("_$$$_");
				string sep = "";
				foreach (Type t1 in type.GetGenericArguments())
				{
					Type t = t1;
					sb.Append(sep);
					// NOTE we can't use ClassLoaderWrapper.GetWrapperFromType() here to get t's name,
					// because we might be resolving a generic type that refers to a type that is in
					// the process of being constructed.
					//
					// For example:
					//   class Base<T> { } 
					//   class Derived : Base<Derived> { }
					//
					while (ReflectUtil.IsVector(t))
					{
						t = t.GetElementType();
						sb.Append('A');
					}
					if (PrimitiveTypeWrapper.IsPrimitiveType(t))
					{
						sb.Append(ClassLoaderWrapper.GetWrapperFromType(t).SigName);
					}
					else
					{
						string s;
						if (ClassLoaderWrapper.IsRemappedType(t) || AttributeHelper.IsJavaModule(t.Module))
						{
							s = ClassLoaderWrapper.GetWrapperFromType(t).Name;
						}
						else
						{
							s = DotNetTypeWrapper.GetName(t);
						}
						// only do the mangling for non-generic types (because we don't want to convert
						// the double underscores in two adjacent _$$$_ or _$$$$_ markers)
						if (s.IndexOf("_$$$_") == -1)
						{
							s = s.Replace("__", "$$005F$$005F");
							s = s.Replace(".", "__");
						}
						sb.Append('L').Append(s);
					}
					sep = "_$$_";
				}
				sb.Append("_$$$$_");
				return sb.ToString();
			}

			if (AttributeHelper.IsNoPackagePrefix(type)
				&& name.IndexOf('$') == -1)
			{
				return name.Replace('+', '$');
			}

			return MangleTypeName(name);
		}

		private static string MangleTypeName(string name)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder(NamePrefix, NamePrefix.Length + name.Length);
			bool escape = false;
			bool nested = false;
			for (int i = 0; i < name.Length; i++)
			{
				char c = name[i];
				if (c == '+' && !escape && (sb.Length == 0 || sb[sb.Length - 1] != '$'))
				{
					nested = true;
					sb.Append('$');
				}
				else if ("_0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c) != -1
					|| (c == '.' && !escape && !nested))
				{
					sb.Append(c);
				}
				else
				{
					sb.Append("$$");
					sb.Append(string.Format("{0:X4}", (int)c));
				}
				if (c == '\\')
				{
					escape = !escape;
				}
				else
				{
					escape = false;
				}
			}
			return sb.ToString();
		}

		// NOTE if the name is not a valid mangled type name, no demangling is done and the
		// original string is returned
		// NOTE we don't enforce canonical form, this is not required, because we cannot
		// guarantee it for unprefixed names anyway, so the caller is responsible for
		// ensuring that the original name was in fact the canonical name.
		internal static string DemangleTypeName(string name)
		{
			if (!name.StartsWith(NamePrefix))
			{
				return name.Replace('$', '+');
			}
			System.Text.StringBuilder sb = new System.Text.StringBuilder(name.Length - NamePrefix.Length);
			for (int i = NamePrefix.Length; i < name.Length; i++)
			{
				char c = name[i];
				if (c == '$')
				{
					if (i + 1 < name.Length && name[i + 1] != '$')
					{
						sb.Append('+');
					}
					else
					{
						i++;
						if (i + 5 > name.Length)
						{
							return name;
						}
						int digit0 = "0123456789ABCDEF".IndexOf(name[++i]);
						int digit1 = "0123456789ABCDEF".IndexOf(name[++i]);
						int digit2 = "0123456789ABCDEF".IndexOf(name[++i]);
						int digit3 = "0123456789ABCDEF".IndexOf(name[++i]);
						if (digit0 == -1 || digit1 == -1 || digit2 == -1 || digit3 == -1)
						{
							return name;
						}
						sb.Append((char)((digit0 << 12) + (digit1 << 8) + (digit2 << 4) + digit3));
					}
				}
				else
				{
					sb.Append(c);
				}
			}
			return sb.ToString();
		}

		// TODO from a perf pov it may be better to allow creation of TypeWrappers,
		// but to simply make sure they don't have ClassObject
		internal static bool IsAllowedOutside(Type type)
		{
			// SECURITY we never expose types from IKVM.Runtime, because doing so would lead to a security hole,
			// since the reflection implementation lives inside this assembly, all internal members would
			// be accessible through Java reflection.
#if !FIRST_PASS && !STATIC_COMPILER && !STUB_GENERATOR
			if (type.Assembly == typeof(DotNetTypeWrapper).Assembly)
			{
				return false;
			}
			if (type.Assembly == IKVM.NativeCode.java.lang.SecurityManager.jniAssembly)
			{
				return false;
			}
#endif
			return true;
		}

		// We allow open generic types to be visible to Java code as very limited classes (or interfaces).
		// They are always package private and have the abstract and final modifiers set, this makes them
		// inaccessible and invalid from a Java point of view. The intent is to avoid any usage of these
		// classes. They exist solely for the purpose of stack walking, because the .NET runtime will report
		// open generic types when walking the stack (as a performance optimization). We cannot (reliably) map
		// these classes to their instantiations, so we report the open generic type class instead.
		// Note also that these classes can only be used as a "handle" to the type, they expose no members,
		// don't implement any interfaces and the base class is always object.
		private sealed class OpenGenericTypeWrapper : TypeWrapper
		{
			private readonly Type type;

			private static Modifiers GetModifiers(Type type)
			{
				Modifiers modifiers = Modifiers.Abstract | Modifiers.Final;
				if (type.IsInterface)
				{
					modifiers |= Modifiers.Interface;
				}
				return modifiers;
			}

			internal OpenGenericTypeWrapper(Type type, string name)
				: base(GetModifiers(type), name, type.IsInterface ? null : CoreClasses.java.lang.Object.Wrapper)
			{
				this.type = type;
			}

			internal override TypeWrapper DeclaringTypeWrapper
			{
				get { return null; }
			}

			internal override TypeWrapper[] InnerClasses
			{
				get { return TypeWrapper.EmptyArray; }
			}

			internal override TypeWrapper[] Interfaces
			{
				get { return TypeWrapper.EmptyArray; }
			}

			internal override Type TypeAsTBD
			{
				get { return type; }
			}

			internal override void Finish()
			{
			}

			internal override ClassLoaderWrapper GetClassLoader()
			{
				return AssemblyClassLoader.FromAssembly(type.Assembly);
			}

			protected override void LazyPublishMembers()
			{
				SetFields(FieldWrapper.EmptyArray);
				SetMethods(MethodWrapper.EmptyArray);
			}
		}

		private sealed class DelegateInnerClassTypeWrapper : TypeWrapper
		{
			private readonly Type fakeType;

			internal DelegateInnerClassTypeWrapper(string name, Type delegateType, ClassLoaderWrapper classLoader)
				: base(Modifiers.Public | Modifiers.Interface | Modifiers.Abstract, name, null)
			{
#if STATIC_COMPILER || STUB_GENERATOR
				this.fakeType = FakeTypes.GetDelegateType(delegateType);
#elif !FIRST_PASS
				this.fakeType = typeof(ikvm.@internal.DelegateInterface<>).MakeGenericType(delegateType);
#endif
				MethodInfo invoke = delegateType.GetMethod("Invoke");
				ParameterInfo[] parameters = invoke.GetParameters();
				TypeWrapper[] argTypeWrappers = new TypeWrapper[parameters.Length];
				System.Text.StringBuilder sb = new System.Text.StringBuilder("(");
				for (int i = 0; i < parameters.Length; i++)
				{
					argTypeWrappers[i] = ClassLoaderWrapper.GetWrapperFromType(parameters[i].ParameterType);
					sb.Append(argTypeWrappers[i].SigName);
				}
				TypeWrapper returnType = ClassLoaderWrapper.GetWrapperFromType(invoke.ReturnType);
				sb.Append(")").Append(returnType.SigName);
				MethodWrapper invokeMethod = new DynamicOnlyMethodWrapper(this, "Invoke", sb.ToString(), returnType, argTypeWrappers);
				SetMethods(new MethodWrapper[] { invokeMethod });
				SetFields(FieldWrapper.EmptyArray);
			}

			internal override TypeWrapper DeclaringTypeWrapper
			{
				get
				{
					return ClassLoaderWrapper.GetWrapperFromType(fakeType.GetGenericArguments()[0]);
				}
			}

			internal override void Finish()
			{
			}

			internal override ClassLoaderWrapper GetClassLoader()
			{
				return DeclaringTypeWrapper.GetClassLoader();
			}

			internal override TypeWrapper[] InnerClasses
			{
				get
				{
					return TypeWrapper.EmptyArray;
				}
			}

			internal override TypeWrapper[] Interfaces
			{
				get
				{
					return TypeWrapper.EmptyArray;
				}
			}

			internal override Type TypeAsTBD
			{
				get
				{
					return fakeType;
				}
			}

			internal override bool IsFastClassLiteralSafe
			{
				get { return true; }
			}
		}

		private class DynamicOnlyMethodWrapper : MethodWrapper, ICustomInvoke
		{
			internal DynamicOnlyMethodWrapper(TypeWrapper declaringType, string name, string sig, TypeWrapper returnType, TypeWrapper[] parameterTypes)
				: base(declaringType, name, sig, null, returnType, parameterTypes, Modifiers.Public | Modifiers.Abstract, MemberFlags.None)
			{
			}

			internal override bool IsDynamicOnly
			{
				get
				{
					return true;
				}
			}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
			object ICustomInvoke.Invoke(object obj, object[] args, ikvm.@internal.CallerID callerID)
			{
				// a DynamicOnlyMethodWrapper is an interface method, but now that we've been called on an actual object instance,
				// we can resolve to a real method and call that instead
				TypeWrapper tw = TypeWrapper.FromClass(NativeCode.ikvm.runtime.Util.getClassFromObject(obj));
				MethodWrapper mw = tw.GetMethodWrapper(this.Name, this.Signature, true);
				if (mw == null)
				{
					throw new java.lang.AbstractMethodError(tw.Name + "." + this.Name + this.Signature);
				}
				java.lang.reflect.Method m = (java.lang.reflect.Method)mw.ToMethodOrConstructor(true);
				m.@override = true;
				return m.invoke(obj, args, callerID);
			}
#endif // !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
		}

		private sealed class EnumEnumTypeWrapper : TypeWrapper
		{
			private readonly Type fakeType;

			internal EnumEnumTypeWrapper(string name, Type enumType)
				: base(Modifiers.Public | Modifiers.Enum | Modifiers.Final, name, ClassLoaderWrapper.LoadClassCritical("java.lang.Enum"))
			{
#if STATIC_COMPILER || STUB_GENERATOR
				this.fakeType = FakeTypes.GetEnumType(enumType);
#elif !FIRST_PASS
				this.fakeType = typeof(ikvm.@internal.EnumEnum<>).MakeGenericType(enumType);
#endif
			}

#if !STATIC_COMPILER && !STUB_GENERATOR
			internal object GetUnspecifiedValue()
			{
				return ((EnumFieldWrapper)GetFieldWrapper("__unspecified", this.SigName)).GetValue();
			}
#endif

			private class EnumFieldWrapper : FieldWrapper
			{
#if !STATIC_COMPILER && !STUB_GENERATOR
				private readonly int ordinal;
				private object val;
#endif

				internal EnumFieldWrapper(TypeWrapper tw, string name, int ordinal)
					: base(tw, tw, name, tw.SigName, Modifiers.Public | Modifiers.Static | Modifiers.Final | Modifiers.Enum, null, MemberFlags.None)
				{
#if !STATIC_COMPILER && !STUB_GENERATOR
					this.ordinal = ordinal;
#endif
				}

#if !STATIC_COMPILER && !STUB_GENERATOR
				internal object GetValue()
				{
					if (val == null)
					{
						System.Threading.Interlocked.CompareExchange(ref val, Activator.CreateInstance(this.DeclaringType.TypeAsTBD, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[] { this.Name, ordinal }, null), null);
					}
					return val;
				}
#endif

#if !STUB_GENERATOR
				protected override void EmitGetImpl(CodeEmitter ilgen)
				{
#if STATIC_COMPILER
					Type typeofByteCodeHelper = StaticCompiler.GetRuntimeType("IKVM.Runtime.ByteCodeHelper");
#else
					Type typeofByteCodeHelper = typeof(IKVM.Runtime.ByteCodeHelper);
#endif
					ilgen.Emit(OpCodes.Ldstr, this.Name);
					ilgen.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("GetDotNetEnumField").MakeGenericMethod(this.DeclaringType.TypeAsBaseType));
				}

				protected override void EmitSetImpl(CodeEmitter ilgen)
				{
				}
#endif // !STUB_GENERATOR
			}

			private class EnumValuesMethodWrapper : MethodWrapper, ICustomInvoke
			{
				internal EnumValuesMethodWrapper(TypeWrapper declaringType)
					: base(declaringType, "values", "()[" + declaringType.SigName, null, declaringType.MakeArrayType(1), TypeWrapper.EmptyArray, Modifiers.Public | Modifiers.Static, MemberFlags.None)
				{
				}

				internal override bool IsDynamicOnly
				{
					get
					{
						return true;
					}
				}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
				object ICustomInvoke.Invoke(object obj, object[] args, ikvm.@internal.CallerID callerID)
				{
					FieldWrapper[] values = this.DeclaringType.GetFields();
					object[] array = (object[])Array.CreateInstance(this.DeclaringType.TypeAsArrayType, values.Length);
					for (int i = 0; i < values.Length; i++)
					{
						array[i] = ((EnumFieldWrapper)values[i]).GetValue();
					}
					return array;
				}
#endif // !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
			}

			private class EnumValueOfMethodWrapper : MethodWrapper, ICustomInvoke
			{
				internal EnumValueOfMethodWrapper(TypeWrapper declaringType)
					: base(declaringType, "valueOf", "(Ljava.lang.String;)" + declaringType.SigName, null, declaringType, new TypeWrapper[] { CoreClasses.java.lang.String.Wrapper }, Modifiers.Public | Modifiers.Static, MemberFlags.None)
				{
				}

				internal override bool IsDynamicOnly
				{
					get
					{
						return true;
					}
				}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
				object ICustomInvoke.Invoke(object obj, object[] args, ikvm.@internal.CallerID callerID)
				{
					FieldWrapper[] values = this.DeclaringType.GetFields();
					for (int i = 0; i < values.Length; i++)
					{
						if (values[i].Name.Equals(args[0]))
						{
							return ((EnumFieldWrapper)values[i]).GetValue();
						}
					}
					throw new java.lang.IllegalArgumentException("" + args[0]);
				}
#endif // !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
			}

			protected override void LazyPublishMembers()
			{
				List<FieldWrapper> fields = new List<FieldWrapper>();
				int ordinal = 0;
				foreach (FieldInfo field in this.DeclaringTypeWrapper.TypeAsTBD.GetFields(BindingFlags.Static | BindingFlags.Public))
				{
					if (field.IsLiteral)
					{
						fields.Add(new EnumFieldWrapper(this, field.Name, ordinal++));
					}
				}
				// TODO if the enum already has an __unspecified value, rename this one
				fields.Add(new EnumFieldWrapper(this, "__unspecified", ordinal++));
				SetFields(fields.ToArray());
				SetMethods(new MethodWrapper[] { new EnumValuesMethodWrapper(this), new EnumValueOfMethodWrapper(this) });
				base.LazyPublishMembers();
			}

			internal override TypeWrapper DeclaringTypeWrapper
			{
				get
				{
					return ClassLoaderWrapper.GetWrapperFromType(fakeType.GetGenericArguments()[0]);
				}
			}

			internal override void Finish()
			{
			}

			internal override ClassLoaderWrapper GetClassLoader()
			{
				return DeclaringTypeWrapper.GetClassLoader();
			}

			internal override TypeWrapper[] InnerClasses
			{
				get
				{
					return TypeWrapper.EmptyArray;
				}
			}

			internal override TypeWrapper[] Interfaces
			{
				get
				{
					return TypeWrapper.EmptyArray;
				}
			}

			internal override Type TypeAsTBD
			{
				get
				{
					return fakeType;
				}
			}

			internal override bool IsFastClassLiteralSafe
			{
				get { return true; }
			}
		}

		internal abstract class AttributeAnnotationTypeWrapperBase : TypeWrapper
		{
			internal AttributeAnnotationTypeWrapperBase(string name)
				: base(Modifiers.Public | Modifiers.Interface | Modifiers.Abstract | Modifiers.Annotation, name, null)
			{
			}

			internal sealed override void Finish()
			{
			}

			internal sealed override ClassLoaderWrapper GetClassLoader()
			{
				return DeclaringTypeWrapper.GetClassLoader();
			}

			internal sealed override TypeWrapper[] Interfaces
			{
				get
				{
					return new TypeWrapper[] { ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName("java.lang.annotation.Annotation") };
				}
			}

			internal sealed override bool IsFastClassLiteralSafe
			{
				get { return true; }
			}

			internal abstract AttributeTargets AttributeTargets { get; }
		}

		private sealed class AttributeAnnotationTypeWrapper : AttributeAnnotationTypeWrapperBase
		{
			private readonly Type fakeType;
			private readonly Type attributeType;
			private TypeWrapper[] innerClasses;

			internal AttributeAnnotationTypeWrapper(string name, Type attributeType)
				: base(name)
			{
#if STATIC_COMPILER || STUB_GENERATOR
				this.fakeType = FakeTypes.GetAttributeType(attributeType);
#elif !FIRST_PASS
				this.fakeType = typeof(ikvm.@internal.AttributeAnnotation<>).MakeGenericType(attributeType);
#endif
				this.attributeType = attributeType;
			}

			private static bool IsSupportedType(Type type)
			{
				// Java annotations only support one-dimensional arrays
				if (ReflectUtil.IsVector(type))
				{
					type = type.GetElementType();
				}
				return type == Types.String
					|| type == Types.Boolean
					|| type == Types.Byte
					|| type == Types.Char
					|| type == Types.Int16
					|| type == Types.Int32
					|| type == Types.Single
					|| type == Types.Int64
					|| type == Types.Double
					|| type == Types.Type
					|| type.IsEnum;
			}

			internal static void GetConstructors(Type type, out ConstructorInfo defCtor, out ConstructorInfo singleOneArgCtor)
			{
				defCtor = null;
				int oneArgCtorCount = 0;
				ConstructorInfo oneArgCtor = null;
				ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
				// HACK we have a special rule to make some additional custom attributes from mscorlib usable:
				// Attributes that have two constructors, one an enum and another one taking a byte, short or int,
				// we only expose the enum constructor.
				if (constructors.Length == 2 && type.Assembly == Types.Object.Assembly)
				{
					ParameterInfo[] p0 = constructors[0].GetParameters();
					ParameterInfo[] p1 = constructors[1].GetParameters();
					if (p0.Length == 1 && p1.Length == 1)
					{
						Type t0 = p0[0].ParameterType;
						Type t1 = p1[0].ParameterType;
						bool swapped = false;
						if (t1.IsEnum)
						{
							Type tmp = t0;
							t0 = t1;
							t1 = tmp;
							swapped = true;
						}
						if (t0.IsEnum && (t1 == Types.Byte || t1 == Types.Int16 || t1 == Types.Int32))
						{
							if (swapped)
							{
								singleOneArgCtor = constructors[1];
							}
							else
							{
								singleOneArgCtor = constructors[0];
							}
							return;
						}
					}
				}
				foreach (ConstructorInfo ci in constructors)
				{
					ParameterInfo[] args = ci.GetParameters();
					if (args.Length == 0)
					{
						defCtor = ci;
					}
					else if (args.Length == 1)
					{
						if (IsSupportedType(args[0].ParameterType))
						{
							oneArgCtor = ci;
							oneArgCtorCount++;
						}
						else
						{
							// set to two to make sure we don't see the oneArgCtor as viable
							oneArgCtorCount = 2;
						}
					}
				}
				singleOneArgCtor = oneArgCtorCount == 1 ? oneArgCtor : null;
			}

			private class AttributeAnnotationMethodWrapper : DynamicOnlyMethodWrapper
			{
				private bool optional;

				internal AttributeAnnotationMethodWrapper(AttributeAnnotationTypeWrapper tw, string name, Type type, bool optional)
					: this(tw, name, MapType(type, false), optional)
				{
				}

				private static TypeWrapper MapType(Type type, bool isArray)
				{
					if (type == Types.String)
					{
						return CoreClasses.java.lang.String.Wrapper;
					}
					else if (type == Types.Boolean)
					{
						return PrimitiveTypeWrapper.BOOLEAN;
					}
					else if (type == Types.Byte)
					{
						return PrimitiveTypeWrapper.BYTE;
					}
					else if (type == Types.Char)
					{
						return PrimitiveTypeWrapper.CHAR;
					}
					else if (type == Types.Int16)
					{
						return PrimitiveTypeWrapper.SHORT;
					}
					else if (type == Types.Int32)
					{
						return PrimitiveTypeWrapper.INT;
					}
					else if (type == Types.Single)
					{
						return PrimitiveTypeWrapper.FLOAT;
					}
					else if (type == Types.Int64)
					{
						return PrimitiveTypeWrapper.LONG;
					}
					else if (type == Types.Double)
					{
						return PrimitiveTypeWrapper.DOUBLE;
					}
					else if (type == Types.Type)
					{
						return CoreClasses.java.lang.Class.Wrapper;
					}
					else if (type.IsEnum)
					{
						foreach (TypeWrapper tw in ClassLoaderWrapper.GetWrapperFromType(type).InnerClasses)
						{
							if (tw is EnumEnumTypeWrapper)
							{
								if (!isArray && type.IsDefined(JVM.Import(typeof(FlagsAttribute)), false))
								{
									return tw.MakeArrayType(1);
								}
								return tw;
							}
						}
						throw new InvalidOperationException();
					}
					else if (!isArray && ReflectUtil.IsVector(type))
					{
						return MapType(type.GetElementType(), true).MakeArrayType(1);
					}
					else
					{
						throw new NotImplementedException();
					}
				}

				private AttributeAnnotationMethodWrapper(AttributeAnnotationTypeWrapper tw, string name, TypeWrapper returnType, bool optional)
					: base(tw, name, "()" + returnType.SigName, returnType, TypeWrapper.EmptyArray)
				{
					this.optional = optional;
				}

				internal override bool IsOptionalAttributeAnnotationValue
				{
					get { return optional; }
				}
			}

			protected override void LazyPublishMembers()
			{
				List<MethodWrapper> methods = new List<MethodWrapper>();
				ConstructorInfo defCtor;
				ConstructorInfo singleOneArgCtor;
				GetConstructors(attributeType, out defCtor, out singleOneArgCtor);
				if (singleOneArgCtor != null)
				{
					methods.Add(new AttributeAnnotationMethodWrapper(this, "value", singleOneArgCtor.GetParameters()[0].ParameterType, defCtor != null));
				}
				foreach (PropertyInfo pi in attributeType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
				{
					if (pi.CanRead && pi.CanWrite && IsSupportedType(pi.PropertyType))
					{
						methods.Add(new AttributeAnnotationMethodWrapper(this, pi.Name, pi.PropertyType, true));
					}
				}
				foreach (FieldInfo fi in attributeType.GetFields(BindingFlags.Public | BindingFlags.Instance))
				{
					// TODO add other field validations to make sure it is appropriate
					if (!fi.IsInitOnly && IsSupportedType(fi.FieldType))
					{
						methods.Add(new AttributeAnnotationMethodWrapper(this, fi.Name, fi.FieldType, true));
					}
				}
				SetMethods(methods.ToArray());
				base.LazyPublishMembers();
			}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
			internal override object GetAnnotationDefault(MethodWrapper mw)
			{
				if (mw.IsOptionalAttributeAnnotationValue)
				{
					if (mw.ReturnType == PrimitiveTypeWrapper.BOOLEAN)
					{
						return java.lang.Boolean.FALSE;
					}
					else if (mw.ReturnType == PrimitiveTypeWrapper.BYTE)
					{
						return java.lang.Byte.valueOf((byte)0);
					}
					else if (mw.ReturnType == PrimitiveTypeWrapper.CHAR)
					{
						return java.lang.Character.valueOf((char)0);
					}
					else if (mw.ReturnType == PrimitiveTypeWrapper.SHORT)
					{
						return java.lang.Short.valueOf((short)0);
					}
					else if (mw.ReturnType == PrimitiveTypeWrapper.INT)
					{
						return java.lang.Integer.valueOf(0);
					}
					else if (mw.ReturnType == PrimitiveTypeWrapper.FLOAT)
					{
						return java.lang.Float.valueOf(0F);
					}
					else if (mw.ReturnType == PrimitiveTypeWrapper.LONG)
					{
						return java.lang.Long.valueOf(0L);
					}
					else if (mw.ReturnType == PrimitiveTypeWrapper.DOUBLE)
					{
						return java.lang.Double.valueOf(0D);
					}
					else if (mw.ReturnType == CoreClasses.java.lang.String.Wrapper)
					{
						return "";
					}
					else if (mw.ReturnType == CoreClasses.java.lang.Class.Wrapper)
					{
						return (java.lang.Class)typeof(ikvm.@internal.__unspecified);
					}
					else if (mw.ReturnType is EnumEnumTypeWrapper)
					{
						EnumEnumTypeWrapper eetw = (EnumEnumTypeWrapper)mw.ReturnType;
						return eetw.GetUnspecifiedValue();
					}
					else if (mw.ReturnType.IsArray)
					{
						return Array.CreateInstance(mw.ReturnType.TypeAsArrayType, 0);
					}
				}
				return null;
			}
#endif // !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR

			internal override TypeWrapper DeclaringTypeWrapper
			{
				get
				{
					return ClassLoaderWrapper.GetWrapperFromType(attributeType);
				}
			}

			internal override Type TypeAsTBD
			{
				get
				{
					return fakeType;
				}
			}

			private sealed class ReturnValueAnnotationTypeWrapper : AttributeAnnotationTypeWrapperBase
			{
				private readonly Type fakeType;
				private readonly AttributeAnnotationTypeWrapper declaringType;

				internal ReturnValueAnnotationTypeWrapper(AttributeAnnotationTypeWrapper declaringType)
					: base(declaringType.Name + AttributeAnnotationReturnValueSuffix)
				{
#if STATIC_COMPILER || STUB_GENERATOR
					this.fakeType = FakeTypes.GetAttributeReturnValueType(declaringType.attributeType);
#elif !FIRST_PASS
					this.fakeType = typeof(ikvm.@internal.AttributeAnnotationReturnValue<>).MakeGenericType(declaringType.attributeType);
#endif
					this.declaringType = declaringType;
				}

				protected override void LazyPublishMembers()
				{
					TypeWrapper tw = declaringType;
					if (declaringType.GetAttributeUsage().AllowMultiple)
					{
						tw = tw.MakeArrayType(1);
					}
					SetMethods(new MethodWrapper[] { new DynamicOnlyMethodWrapper(this, "value", "()" + tw.SigName, tw, TypeWrapper.EmptyArray) });
					SetFields(FieldWrapper.EmptyArray);
				}

				internal override TypeWrapper DeclaringTypeWrapper
				{
					get
					{
						return declaringType;
					}
				}

				internal override TypeWrapper[] InnerClasses
				{
					get
					{
						return TypeWrapper.EmptyArray;
					}
				}

				internal override Type TypeAsTBD
				{
					get
					{
						return fakeType;
					}
				}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
				internal override object[] GetDeclaredAnnotations()
				{
					java.util.HashMap targetMap = new java.util.HashMap();
					targetMap.put("value", new java.lang.annotation.ElementType[] { java.lang.annotation.ElementType.METHOD });
					java.util.HashMap retentionMap = new java.util.HashMap();
					retentionMap.put("value", java.lang.annotation.RetentionPolicy.RUNTIME);
					return new object[] {
						java.lang.reflect.Proxy.newProxyInstance(null, new java.lang.Class[] { typeof(java.lang.annotation.Target) }, new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Target), targetMap)),
						java.lang.reflect.Proxy.newProxyInstance(null, new java.lang.Class[] { typeof(java.lang.annotation.Retention) }, new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Retention), retentionMap))
					};
				}
#endif

				private class ReturnValueAnnotation : Annotation
				{
					private AttributeAnnotationTypeWrapper type;

					internal ReturnValueAnnotation(AttributeAnnotationTypeWrapper type)
					{
						this.type = type;
					}

					internal override void ApplyReturnValue(ClassLoaderWrapper loader, MethodBuilder mb, ref ParameterBuilder pb, object annotation)
					{
						// TODO make sure the descriptor is correct
						Annotation ann = type.Annotation;
						object[] arr = (object[])annotation;
						for (int i = 2; i < arr.Length; i += 2)
						{
							if ("value".Equals(arr[i]))
							{
								if (pb == null)
								{
									pb = mb.DefineParameter(0, ParameterAttributes.None, null);
								}
								object[] value = (object[])arr[i + 1];
								if (value[0].Equals(AnnotationDefaultAttribute.TAG_ANNOTATION))
								{
									ann.Apply(loader, pb, value);
								}
								else
								{
									for (int j = 1; j < value.Length; j++)
									{
										ann.Apply(loader, pb, value[j]);
									}
								}
								break;
							}
						}
					}

					internal override void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation)
					{
					}

					internal override void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation)
					{
					}

					internal override void Apply(ClassLoaderWrapper loader, ConstructorBuilder cb, object annotation)
					{
					}

					internal override void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation)
					{
					}

					internal override void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation)
					{
					}

					internal override void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation)
					{
					}

					internal override void Apply(ClassLoaderWrapper loader, PropertyBuilder pb, object annotation)
					{
					}
				}

				internal override Annotation Annotation
				{
					get
					{
						return new ReturnValueAnnotation(declaringType);
					}
				}

				internal override AttributeTargets AttributeTargets
				{
					get { return AttributeTargets.ReturnValue; }
				}
			}

			private sealed class MultipleAnnotationTypeWrapper : AttributeAnnotationTypeWrapperBase
			{
				private readonly Type fakeType;
				private readonly AttributeAnnotationTypeWrapper declaringType;

				internal MultipleAnnotationTypeWrapper(AttributeAnnotationTypeWrapper declaringType)
					: base(declaringType.Name + AttributeAnnotationMultipleSuffix)
				{
#if STATIC_COMPILER || STUB_GENERATOR
					this.fakeType = FakeTypes.GetAttributeMultipleType(declaringType.attributeType);
#elif !FIRST_PASS
					this.fakeType = typeof(ikvm.@internal.AttributeAnnotationMultiple<>).MakeGenericType(declaringType.attributeType);
#endif
					this.declaringType = declaringType;
				}

				protected override void LazyPublishMembers()
				{
					TypeWrapper tw = declaringType.MakeArrayType(1);
					SetMethods(new MethodWrapper[] { new DynamicOnlyMethodWrapper(this, "value", "()" + tw.SigName, tw, TypeWrapper.EmptyArray) });
					SetFields(FieldWrapper.EmptyArray);
				}

				internal override TypeWrapper DeclaringTypeWrapper
				{
					get
					{
						return declaringType;
					}
				}

				internal override TypeWrapper[] InnerClasses
				{
					get
					{
						return TypeWrapper.EmptyArray;
					}
				}

				internal override Type TypeAsTBD
				{
					get
					{
						return fakeType;
					}
				}

#if !STATIC_COMPILER && !STUB_GENERATOR
				internal override object[] GetDeclaredAnnotations()
				{
					return declaringType.GetDeclaredAnnotations();
				}
#endif

				private class MultipleAnnotation : Annotation
				{
					private AttributeAnnotationTypeWrapper type;

					internal MultipleAnnotation(AttributeAnnotationTypeWrapper type)
					{
						this.type = type;
					}

					private static object[] UnwrapArray(object annotation)
					{
						// TODO make sure the descriptor is correct
						object[] arr = (object[])annotation;
						for (int i = 2; i < arr.Length; i += 2)
						{
							if ("value".Equals(arr[i]))
							{
								object[] value = (object[])arr[i + 1];
								object[] rc = new object[value.Length - 1];
								Array.Copy(value, 1, rc, 0, rc.Length);
								return rc;
							}
						}
						return new object[0];
					}

					internal override void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation)
					{
						Annotation annot = type.Annotation;
						foreach (object ann in UnwrapArray(annotation))
						{
							annot.Apply(loader, mb, ann);
						}
					}

					internal override void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation)
					{
						Annotation annot = type.Annotation;
						foreach (object ann in UnwrapArray(annotation))
						{
							annot.Apply(loader, ab, ann);
						}
					}

					internal override void Apply(ClassLoaderWrapper loader, ConstructorBuilder cb, object annotation)
					{
						Annotation annot = type.Annotation;
						foreach (object ann in UnwrapArray(annotation))
						{
							annot.Apply(loader, cb, ann);
						}
					}

					internal override void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation)
					{
						Annotation annot = type.Annotation;
						foreach (object ann in UnwrapArray(annotation))
						{
							annot.Apply(loader, fb, ann);
						}
					}

					internal override void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation)
					{
						Annotation annot = type.Annotation;
						foreach (object ann in UnwrapArray(annotation))
						{
							annot.Apply(loader, pb, ann);
						}
					}

					internal override void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation)
					{
						Annotation annot = type.Annotation;
						foreach (object ann in UnwrapArray(annotation))
						{
							annot.Apply(loader, tb, ann);
						}
					}

					internal override void Apply(ClassLoaderWrapper loader, PropertyBuilder pb, object annotation)
					{
						Annotation annot = type.Annotation;
						foreach (object ann in UnwrapArray(annotation))
						{
							annot.Apply(loader, pb, ann);
						}
					}
				}

				internal override Annotation Annotation
				{
					get
					{
						return new MultipleAnnotation(declaringType);
					}
				}

				internal override AttributeTargets AttributeTargets
				{
					get { return declaringType.AttributeTargets; }
				}
			}

			internal override TypeWrapper[] InnerClasses
			{
				get
				{
					lock (this)
					{
						if (innerClasses == null)
						{
							List<TypeWrapper> list = new List<TypeWrapper>();
							AttributeUsageAttribute attr = GetAttributeUsage();
							if ((attr.ValidOn & AttributeTargets.ReturnValue) != 0)
							{
								list.Add(GetClassLoader().RegisterInitiatingLoader(new ReturnValueAnnotationTypeWrapper(this)));
							}
							if (attr.AllowMultiple)
							{
								list.Add(GetClassLoader().RegisterInitiatingLoader(new MultipleAnnotationTypeWrapper(this)));
							}
							innerClasses = list.ToArray();
						}
					}
					return innerClasses;
				}
			}

			internal override bool IsFakeTypeContainer
			{
				get
				{
					return true;
				}
			}

			private AttributeUsageAttribute GetAttributeUsage()
			{
				AttributeTargets validOn = AttributeTargets.All;
				bool allowMultiple = false;
				bool inherited = true;
				foreach (CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(attributeType))
				{
					if (cad.Constructor.DeclaringType == JVM.Import(typeof(AttributeUsageAttribute)))
					{
						if (cad.ConstructorArguments.Count == 1 && cad.ConstructorArguments[0].ArgumentType == JVM.Import(typeof(AttributeTargets)))
						{
							validOn = (AttributeTargets)cad.ConstructorArguments[0].Value;
						}
						foreach (CustomAttributeNamedArgument cana in cad.NamedArguments)
						{
							if (cana.MemberInfo.Name == "AllowMultiple")
							{
								allowMultiple = (bool)cana.TypedValue.Value;
							}
							else if (cana.MemberInfo.Name == "Inherited")
							{
								inherited = (bool)cana.TypedValue.Value;
							}
						}
					}
				}
				AttributeUsageAttribute attr = new AttributeUsageAttribute(validOn);
				attr.AllowMultiple = allowMultiple;
				attr.Inherited = inherited;
				return attr;
			}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
			internal override object[] GetDeclaredAnnotations()
			{
				// note that AttributeUsageAttribute.Inherited does not map to java.lang.annotation.Inherited
				AttributeTargets validOn = GetAttributeUsage().ValidOn;
				List<java.lang.annotation.ElementType> targets = new List<java.lang.annotation.ElementType>();
				if ((validOn & (AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Assembly)) != 0)
				{
					targets.Add(java.lang.annotation.ElementType.TYPE);
				}
				if ((validOn & AttributeTargets.Constructor) != 0)
				{
					targets.Add(java.lang.annotation.ElementType.CONSTRUCTOR);
				}
				if ((validOn & AttributeTargets.Field) != 0)
				{
					targets.Add(java.lang.annotation.ElementType.FIELD);
				}
				if ((validOn & AttributeTargets.Method) != 0)
				{
					targets.Add(java.lang.annotation.ElementType.METHOD);
				}
				if ((validOn & AttributeTargets.Parameter) != 0)
				{
					targets.Add(java.lang.annotation.ElementType.PARAMETER);
				}
				java.util.HashMap targetMap = new java.util.HashMap();
				targetMap.put("value", targets.ToArray());
				java.util.HashMap retentionMap = new java.util.HashMap();
				retentionMap.put("value", java.lang.annotation.RetentionPolicy.RUNTIME);
				return new object[] {
					java.lang.reflect.Proxy.newProxyInstance(null, new java.lang.Class[] { typeof(java.lang.annotation.Target) }, new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Target), targetMap)),
					java.lang.reflect.Proxy.newProxyInstance(null, new java.lang.Class[] { typeof(java.lang.annotation.Retention) }, new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Retention), retentionMap))
				};
			}
#endif

			private class AttributeAnnotation : Annotation
			{
				private Type type;

				internal AttributeAnnotation(Type type)
				{
					this.type = type;
				}

				private CustomAttributeBuilder MakeCustomAttributeBuilder(ClassLoaderWrapper loader, object annotation)
				{
					object[] arr = (object[])annotation;
					ConstructorInfo defCtor;
					ConstructorInfo singleOneArgCtor;
					object ctorArg = null;
					GetConstructors(type, out defCtor, out singleOneArgCtor);
					List<PropertyInfo> properties = new List<PropertyInfo>();
					List<object> propertyValues = new List<object>();
					List<FieldInfo> fields = new List<FieldInfo>();
					List<object> fieldValues = new List<object>();
					for (int i = 2; i < arr.Length; i += 2)
					{
						string name = (string)arr[i];
						if (name == "value" && singleOneArgCtor != null)
						{
							ctorArg = ConvertValue(loader, singleOneArgCtor.GetParameters()[0].ParameterType, arr[i + 1]);
						}
						else
						{
							PropertyInfo pi = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
							if (pi != null)
							{
								properties.Add(pi);
								propertyValues.Add(ConvertValue(loader, pi.PropertyType, arr[i + 1]));
							}
							else
							{
								FieldInfo fi = type.GetField(name, BindingFlags.Public | BindingFlags.Instance);
								if (fi != null)
								{
									fields.Add(fi);
									fieldValues.Add(ConvertValue(loader, fi.FieldType, arr[i + 1]));
								}
							}
						}
					}
					if (ctorArg == null && defCtor == null)
					{
						// TODO required argument is missing
					}
					return new CustomAttributeBuilder(ctorArg == null ? defCtor : singleOneArgCtor,
						ctorArg == null ? new object[0] : new object[] { ctorArg },
						properties.ToArray(),
						propertyValues.ToArray(),
						fields.ToArray(),
						fieldValues.ToArray());
				}

				internal override void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation)
				{
					if (type == JVM.Import(typeof(System.Runtime.InteropServices.StructLayoutAttribute)) && tb.BaseType != Types.Object)
					{
						// we have to handle this explicitly, because if we apply an illegal StructLayoutAttribute,
						// TypeBuilder.CreateType() will later on throw an exception.
#if STATIC_COMPILER
						loader.IssueMessage(Message.IgnoredCustomAttribute, type.FullName, "Type '" + tb.FullName + "' does not extend cli.System.Object");
#else
						Tracer.Error(Tracer.Runtime, "StructLayoutAttribute cannot be applied to {0}, because it does not directly extend cli.System.Object", tb.FullName);
#endif
						return;
					}
					if (type.IsSubclassOf(Types.SecurityAttribute))
					{
#if STATIC_COMPILER
						tb.__AddDeclarativeSecurity(MakeCustomAttributeBuilder(loader, annotation));
#elif STUB_GENERATOR
#else
						SecurityAction action;
						PermissionSet permSet;
						if (MakeDeclSecurity(type, annotation, out action, out permSet))
						{
							tb.AddDeclarativeSecurity(action, permSet);
						}
#endif
					}
					else
					{
						tb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, ConstructorBuilder cb, object annotation)
				{
					if (type.IsSubclassOf(Types.SecurityAttribute))
					{
#if STATIC_COMPILER
						cb.__AddDeclarativeSecurity(MakeCustomAttributeBuilder(loader, annotation));
#elif STUB_GENERATOR
#else
						SecurityAction action;
						PermissionSet permSet;
						if (MakeDeclSecurity(type, annotation, out action, out permSet))
						{
							cb.AddDeclarativeSecurity(action, permSet);
						}
#endif
					}
					else
					{
						cb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation)
				{
					if (type.IsSubclassOf(Types.SecurityAttribute))
					{
#if STATIC_COMPILER
						mb.__AddDeclarativeSecurity(MakeCustomAttributeBuilder(loader, annotation));
#elif STUB_GENERATOR
#else
						SecurityAction action;
						PermissionSet permSet;
						if (MakeDeclSecurity(type, annotation, out action, out permSet))
						{
							mb.AddDeclarativeSecurity(action, permSet);
						}
#endif
					}
					else
					{
						mb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation)
				{
					if (type.IsSubclassOf(Types.SecurityAttribute))
					{
						// you can't add declarative security to a field
					}
					else
					{
						fb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation)
				{
					if (type.IsSubclassOf(Types.SecurityAttribute))
					{
						// you can't add declarative security to a parameter
					}
					else if (type == JVM.Import(typeof(System.Runtime.InteropServices.DefaultParameterValueAttribute)))
					{
						// TODO with the current custom attribute annotation restrictions it is impossible to use this CA,
						// but if we make it possible, we should also implement it here
						throw new NotImplementedException();
					}
					else
					{
						pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation)
				{
					if (type.IsSubclassOf(Types.SecurityAttribute))
					{
#if STATIC_COMPILER
						ab.__AddDeclarativeSecurity(MakeCustomAttributeBuilder(loader, annotation));
#endif
					}
#if STATIC_COMPILER
					else if (type == JVM.Import(typeof(System.Runtime.CompilerServices.TypeForwardedToAttribute)))
					{
						ab.__AddTypeForwarder((Type)ConvertValue(loader, Types.Type, ((object[])annotation)[3]));
					}
					else if (type == JVM.Import(typeof(System.Reflection.AssemblyVersionAttribute)))
					{
						string str = (string)ConvertValue(loader, Types.String, ((object[])annotation)[3]);
						Version version;
						if (IkvmcCompiler.TryParseVersion(str, out version))
						{
							ab.__SetAssemblyVersion(version);
						}
						else
						{
							loader.IssueMessage(Message.InvalidCustomAttribute, type.FullName, "The version '" + str + "' is invalid.");
						}
					}
					else if (type == JVM.Import(typeof(System.Reflection.AssemblyCultureAttribute)))
					{
						string str = (string)ConvertValue(loader, Types.String, ((object[])annotation)[3]);
						if (str != "")
						{
							ab.__SetAssemblyCulture(str);
						}
					}
					else if (type == JVM.Import(typeof(System.Reflection.AssemblyDelaySignAttribute))
						|| type == JVM.Import(typeof(System.Reflection.AssemblyKeyFileAttribute))
						|| type == JVM.Import(typeof(System.Reflection.AssemblyKeyNameAttribute)))
					{
						loader.IssueMessage(Message.IgnoredCustomAttribute, type.FullName, "Please use the corresponding compiler switch.");
					}
					else if (type == JVM.Import(typeof(System.Reflection.AssemblyAlgorithmIdAttribute)))
					{
						// this attribute is currently not exposed as an annotation and isn't very interesting
						throw new NotImplementedException();
					}
					else if (type == JVM.Import(typeof(System.Reflection.AssemblyFlagsAttribute)))
					{
						// this attribute is currently not exposed as an annotation and isn't very interesting
						throw new NotImplementedException();
					}
#endif
					else
					{
						ab.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, PropertyBuilder pb, object annotation)
				{
					if (type.IsSubclassOf(Types.SecurityAttribute))
					{
						// you can't add declarative security to a property
					}
					else
					{
						pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
					}
				}
			}

			internal override Annotation Annotation
			{
				get
				{
					return new AttributeAnnotation(attributeType);
				}
			}

			internal override AttributeTargets AttributeTargets
			{
				get { return GetAttributeUsage().ValidOn; }
			}
		}

		internal static TypeWrapper GetWrapperFromDotNetType(Type type)
		{
			TypeWrapper tw;
			lock (types)
			{
				types.TryGetValue(type, out tw);
			}
			if (tw == null)
			{
				tw = AssemblyClassLoader.FromAssembly(type.Assembly).GetWrapperFromAssemblyType(type);
				lock (types)
				{
					types[type] = tw;
				}
			}
			return tw;
		}

		private static TypeWrapper GetBaseTypeWrapper(Type type)
		{
			if (type.IsInterface)
			{
				return null;
			}
			else if (ClassLoaderWrapper.IsRemappedType(type))
			{
				// Remapped types extend their alter ego
				// (e.g. cli.System.Object must appear to be derived from java.lang.Object)
				// except when they're sealed, of course.
				if (type.IsSealed)
				{
					return CoreClasses.java.lang.Object.Wrapper;
				}
				return ClassLoaderWrapper.GetWrapperFromType(type);
			}
			else if (ClassLoaderWrapper.IsRemappedType(type.BaseType))
			{
				return GetWrapperFromDotNetType(type.BaseType);
			}
			else
			{
				return ClassLoaderWrapper.GetWrapperFromType(type.BaseType);
			}
		}

		internal static TypeWrapper Create(Type type, string name)
		{
			if (type.ContainsGenericParameters)
			{
				return new OpenGenericTypeWrapper(type, name);
			}
			else
			{
				return new DotNetTypeWrapper(type, name);
			}
		}

		private DotNetTypeWrapper(Type type, string name)
			: base(GetModifiers(type), name, GetBaseTypeWrapper(type))
		{
			Debug.Assert(!(type.IsByRef), type.FullName);
			Debug.Assert(!(type.IsPointer), type.FullName);
			Debug.Assert(!(type.Name.EndsWith("[]")), type.FullName);
			Debug.Assert(!(type is TypeBuilder), type.FullName);
			Debug.Assert(!(AttributeHelper.IsJavaModule(type.Module)));

			this.type = type;
		}

		internal override ClassLoaderWrapper GetClassLoader()
		{
			if (type.IsGenericType)
			{
				return ClassLoaderWrapper.GetGenericClassLoader(this);
			}
			return AssemblyClassLoader.FromAssembly(type.Assembly);
		}

		private sealed class MulticastDelegateCtorMethodWrapper : MethodWrapper
		{
			internal MulticastDelegateCtorMethodWrapper(TypeWrapper declaringType)
				: base(declaringType, "<init>", "()V", null, PrimitiveTypeWrapper.VOID, TypeWrapper.EmptyArray, Modifiers.Protected, MemberFlags.None)
			{
			}
		}

		private class DelegateMethodWrapper : MethodWrapper
		{
			private ConstructorInfo delegateConstructor;
			private DelegateInnerClassTypeWrapper iface;

			internal DelegateMethodWrapper(TypeWrapper declaringType, DelegateInnerClassTypeWrapper iface)
				: base(declaringType, "<init>", "(" + iface.SigName + ")V", null, PrimitiveTypeWrapper.VOID, new TypeWrapper[] { iface }, Modifiers.Public, MemberFlags.None)
			{
				this.delegateConstructor = declaringType.TypeAsTBD.GetConstructor(new Type[] { Types.Object, Types.IntPtr });
				this.iface = iface;
			}

#if !STUB_GENERATOR
			internal override void EmitNewobj(CodeEmitter ilgen, MethodAnalyzer ma, int opcodeIndex)
			{
				TypeWrapper targetType = ma == null ? null : ma.GetStackTypeWrapper(opcodeIndex, 0);
				if (targetType == null || targetType.IsInterface)
				{
					MethodInfo createDelegate = Types.Delegate.GetMethod("CreateDelegate", new Type[] { Types.Type, Types.Object, Types.String });
					LocalBuilder targetObj = ilgen.DeclareLocal(Types.Object);
					ilgen.Emit(OpCodes.Stloc, targetObj);
					ilgen.Emit(OpCodes.Ldtoken, delegateConstructor.DeclaringType);
					ilgen.Emit(OpCodes.Call, Types.Type.GetMethod("GetTypeFromHandle", new Type[] { Types.RuntimeTypeHandle }));
					ilgen.Emit(OpCodes.Ldloc, targetObj);
					ilgen.Emit(OpCodes.Ldstr, "Invoke");
					ilgen.Emit(OpCodes.Call, createDelegate);
					ilgen.Emit(OpCodes.Castclass, delegateConstructor.DeclaringType);
				}
				else
				{
					ilgen.Emit(OpCodes.Dup);
					// we know that a DelegateInnerClassTypeWrapper has only one method
					Debug.Assert(iface.GetMethods().Length == 1);
					MethodWrapper mw = targetType.GetMethodWrapper("Invoke", iface.GetMethods()[0].Signature, true);
					// TODO linking here is not safe
					mw.Link();
					ilgen.Emit(OpCodes.Ldvirtftn, (MethodInfo)mw.GetMethod());
					ilgen.Emit(OpCodes.Newobj, delegateConstructor);
				}
			}
#endif // !STUB_GENERATOR
		}

		private class ByRefMethodWrapper : SmartMethodWrapper
		{
#if !STATIC_COMPILER
			private bool[] byrefs;
#endif
			private Type[] args;

			internal ByRefMethodWrapper(Type[] args, bool[] byrefs, TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, bool hideFromReflection)
				: base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None)
			{
				this.args = args;
#if !STATIC_COMPILER
				this.byrefs = byrefs;
#endif
			}

#if !STUB_GENERATOR
			protected override void CallImpl(CodeEmitter ilgen)
			{
				MethodBase mb = GetMethod();
				MethodInfo mi = mb as MethodInfo;
				if (mi != null)
				{
					ilgen.Emit(OpCodes.Call, mi);
				}
				else
				{
					ilgen.Emit(OpCodes.Call, (ConstructorInfo)mb);
				}
			}

			protected override void CallvirtImpl(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Callvirt, (MethodInfo)GetMethod());
			}

			protected override void NewobjImpl(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Newobj, (ConstructorInfo)GetMethod());
			}

			protected override void PreEmit(CodeEmitter ilgen)
			{
				LocalBuilder[] locals = new LocalBuilder[args.Length];
				for (int i = args.Length - 1; i >= 0; i--)
				{
					Type type = args[i];
					if (type.IsByRef)
					{
						type = ArrayTypeWrapper.MakeArrayType(type.GetElementType(), 1);
					}
					locals[i] = ilgen.DeclareLocal(type);
					ilgen.Emit(OpCodes.Stloc, locals[i]);
				}
				for (int i = 0; i < args.Length; i++)
				{
					ilgen.Emit(OpCodes.Ldloc, locals[i]);
					if (args[i].IsByRef)
					{
						ilgen.Emit(OpCodes.Ldc_I4_0);
						ilgen.Emit(OpCodes.Ldelema, args[i].GetElementType());
					}
				}
				base.PreEmit(ilgen);
			}
#endif // !STUB_GENERATOR
		}

		private class EnumWrapMethodWrapper : MethodWrapper
		{
			internal EnumWrapMethodWrapper(DotNetTypeWrapper tw, TypeWrapper fieldType)
				: base(tw, "wrap", "(" + fieldType.SigName + ")" + tw.SigName, null, tw, new TypeWrapper[] { fieldType }, Modifiers.Static | Modifiers.Public, MemberFlags.None)
			{
			}

#if !STUB_GENERATOR
			internal override void EmitCall(CodeEmitter ilgen)
			{
				// We don't actually need to do anything here!
				// The compiler will insert a boxing operation after calling us and that will
				// result in our argument being boxed (since that's still sitting on the stack).
			}
#endif // !STUB_GENERATOR
		}

		internal class EnumValueFieldWrapper : FieldWrapper
		{
			private Type underlyingType;

			internal EnumValueFieldWrapper(DotNetTypeWrapper tw, TypeWrapper fieldType)
				: base(tw, fieldType, "Value", fieldType.SigName, new ExModifiers(Modifiers.Public | Modifiers.Final, false), null)
			{
				underlyingType = EnumHelper.GetUnderlyingType(tw.type);
			}

#if !STUB_GENERATOR
			protected override void EmitGetImpl(CodeEmitter ilgen)
			{
				// NOTE if the reference on the stack is null, we *want* the NullReferenceException, so we don't use TypeWrapper.EmitUnbox
				ilgen.LazyEmitUnbox(underlyingType);
				ilgen.LazyEmitLdobj(underlyingType);
			}

			protected override void EmitSetImpl(CodeEmitter ilgen)
			{
				// NOTE even though the field is final, JNI reflection can still be used to set its value!
				LocalBuilder temp = ilgen.AllocTempLocal(underlyingType);
				ilgen.Emit(OpCodes.Stloc, temp);
				ilgen.Emit(OpCodes.Unbox, underlyingType);
				ilgen.Emit(OpCodes.Ldloc, temp);
				ilgen.Emit(OpCodes.Stobj, underlyingType);
				ilgen.ReleaseTempLocal(temp);
			}
#endif // !STUB_GENERATOR
		}

		private class ValueTypeDefaultCtor : MethodWrapper
		{
			internal ValueTypeDefaultCtor(DotNetTypeWrapper tw)
				: base(tw, "<init>", "()V", null, PrimitiveTypeWrapper.VOID, TypeWrapper.EmptyArray, Modifiers.Public, MemberFlags.None)
			{
			}

#if !STUB_GENERATOR
			internal override void EmitNewobj(CodeEmitter ilgen, MethodAnalyzer ma, int opcodeIndex)
			{
				LocalBuilder local = ilgen.DeclareLocal(DeclaringType.TypeAsTBD);
				ilgen.Emit(OpCodes.Ldloc, local);
				ilgen.Emit(OpCodes.Box, DeclaringType.TypeAsTBD);
			}
#endif // !STUB_GENERATOR
		}

		private class FinalizeMethodWrapper : MethodWrapper
		{
			internal FinalizeMethodWrapper(DotNetTypeWrapper tw)
				: base(tw, "finalize", "()V", null, PrimitiveTypeWrapper.VOID, TypeWrapper.EmptyArray, Modifiers.Protected | Modifiers.Final, MemberFlags.None)
			{
			}

#if !STUB_GENERATOR
			internal override void EmitCall(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Pop);
			}

			internal override void EmitCallvirt(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Pop);
			}
#endif // !STUB_GENERATOR
		}

		private class CloneMethodWrapper : MethodWrapper
		{
			internal CloneMethodWrapper(DotNetTypeWrapper tw)
				: base(tw, "clone", "()Ljava.lang.Object;", null, CoreClasses.java.lang.Object.Wrapper, TypeWrapper.EmptyArray, Modifiers.Protected | Modifiers.Final, MemberFlags.None)
			{
			}

#if !STUB_GENERATOR
			internal override void EmitCall(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Dup);
				ilgen.Emit(OpCodes.Isinst, ClassLoaderWrapper.LoadClassCritical("java.lang.Cloneable").TypeAsBaseType);
				CodeEmitterLabel label1 = ilgen.DefineLabel();
				ilgen.Emit(OpCodes.Brtrue_S, label1);
				CodeEmitterLabel label2 = ilgen.DefineLabel();
				ilgen.Emit(OpCodes.Brfalse_S, label2);
				ilgen.EmitThrow("java.lang.CloneNotSupportedException");
				ilgen.MarkLabel(label2);
				ilgen.EmitThrow("java.lang.NullPointerException");
				ilgen.MarkLabel(label1);
				ilgen.Emit(OpCodes.Call, Types.Object.GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null));
			}

			internal override void EmitCallvirt(CodeEmitter ilgen)
			{
				EmitCall(ilgen);
			}
#endif // !STUB_GENERATOR
		}

		protected override void LazyPublishMembers()
		{
			// special support for enums
			if (type.IsEnum)
			{
				Type underlyingType = EnumHelper.GetUnderlyingType(type);
				Type javaUnderlyingType;
				if (underlyingType == Types.SByte)
				{
					javaUnderlyingType = Types.Byte;
				}
				else if (underlyingType == Types.UInt16)
				{
					javaUnderlyingType = Types.Int16;
				}
				else if (underlyingType == Types.UInt32)
				{
					javaUnderlyingType = Types.Int32;
				}
				else if (underlyingType == Types.UInt64)
				{
					javaUnderlyingType = Types.Int64;
				}
				else
				{
					javaUnderlyingType = underlyingType;
				}
				TypeWrapper fieldType = ClassLoaderWrapper.GetWrapperFromType(javaUnderlyingType);
				FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
				List<FieldWrapper> fieldsList = new List<FieldWrapper>();
				for (int i = 0; i < fields.Length; i++)
				{
					if (fields[i].FieldType == type)
					{
						string name = fields[i].Name;
						if (name == "Value")
						{
							name = "_Value";
						}
						else if (name.StartsWith("_") && name.EndsWith("Value"))
						{
							name = "_" + name;
						}
						object val = EnumHelper.GetPrimitiveValue(underlyingType, fields[i].GetRawConstantValue());
						fieldsList.Add(new ConstantFieldWrapper(this, fieldType, name, fieldType.SigName, Modifiers.Public | Modifiers.Static | Modifiers.Final, fields[i], val, MemberFlags.None));
					}
				}
				fieldsList.Add(new EnumValueFieldWrapper(this, fieldType));
				SetFields(fieldsList.ToArray());
				SetMethods(new MethodWrapper[] { new EnumWrapMethodWrapper(this, fieldType) });
			}
			else
			{
				List<FieldWrapper> fieldsList = new List<FieldWrapper>();
				FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				for (int i = 0; i < fields.Length; i++)
				{
					// TODO for remapped types, instance fields need to be converted to static getter/setter methods
					if (fields[i].FieldType.IsPointer)
					{
						// skip, pointer fields are not supported
					}
					else
					{
						// TODO handle name/signature clash
						fieldsList.Add(CreateFieldWrapperDotNet(AttributeHelper.GetModifiers(fields[i], true).Modifiers, fields[i].Name, fields[i].FieldType, fields[i]));
					}
				}
				SetFields(fieldsList.ToArray());

				Dictionary<string, MethodWrapper> methodsList = new Dictionary<string, MethodWrapper>();

				// special case for delegate constructors!
				if (IsDelegate(type))
				{
					TypeWrapper iface = InnerClasses[0];
					DelegateMethodWrapper mw = new DelegateMethodWrapper(this, (DelegateInnerClassTypeWrapper)iface);
					methodsList.Add(mw.Name + mw.Signature, mw);
				}

				// add a protected default constructor to MulticastDelegate to make it easier to define a delegate in Java
				if (type == Types.MulticastDelegate)
				{
					methodsList.Add("<init>()V", new MulticastDelegateCtorMethodWrapper(this));
				}

				ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				for (int i = 0; i < constructors.Length; i++)
				{
					string name;
					string sig;
					TypeWrapper[] args;
					TypeWrapper ret;
					if (MakeMethodDescriptor(constructors[i], out name, out sig, out args, out ret))
					{
						MethodWrapper mw = CreateMethodWrapper(name, sig, args, ret, constructors[i], false);
						string key = mw.Name + mw.Signature;
						if (!methodsList.ContainsKey(key))
						{
							methodsList.Add(key, mw);
						}
					}
				}

				if (type.IsValueType && !methodsList.ContainsKey("<init>()V"))
				{
					// Value types have an implicit default ctor
					methodsList.Add("<init>()V", new ValueTypeDefaultCtor(this));
				}

				MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				for (int i = 0; i < methods.Length; i++)
				{
					if (methods[i].IsStatic && type.IsInterface)
					{
						// skip, Java cannot deal with static methods on interfaces
					}
					else
					{
						string name;
						string sig;
						TypeWrapper[] args;
						TypeWrapper ret;
						if (MakeMethodDescriptor(methods[i], out name, out sig, out args, out ret))
						{
							if (!methods[i].IsStatic && !methods[i].IsPrivate && BaseTypeWrapper != null)
							{
								MethodWrapper baseMethod = BaseTypeWrapper.GetMethodWrapper(name, sig, true);
								if (baseMethod != null && baseMethod.IsFinal && !baseMethod.IsStatic && !baseMethod.IsPrivate)
								{
									continue;
								}
							}
							MethodWrapper mw = CreateMethodWrapper(name, sig, args, ret, methods[i], false);
							string key = mw.Name + mw.Signature;
							MethodWrapper existing;
							methodsList.TryGetValue(key, out existing);
							if (existing == null || existing is ByRefMethodWrapper)
							{
								methodsList[key] = mw;
							}
						}
						else if (methods[i].IsAbstract)
						{
							this.HasUnsupportedAbstractMethods = true;
						}
					}
				}

				// make sure that all the interface methods that we implement are available as public methods,
				// otherwise javac won't like the class.
				if (!type.IsInterface)
				{
					Type[] interfaces = type.GetInterfaces();
					for (int i = 0; i < interfaces.Length; i++)
					{
						// we only handle public (or nested public) types, because we're potentially adding a
						// method that should be callable by anyone through the interface
						if (interfaces[i].IsVisible)
						{
							InterfaceMapping map = type.GetInterfaceMap(interfaces[i]);
							for (int j = 0; j < map.InterfaceMethods.Length; j++)
							{
								if ((!map.TargetMethods[j].IsPublic || map.TargetMethods[j].Name != map.InterfaceMethods[j].Name)
									&& map.TargetMethods[j].DeclaringType == type)
								{
									string name;
									string sig;
									TypeWrapper[] args;
									TypeWrapper ret;
									if (MakeMethodDescriptor(map.InterfaceMethods[j], out name, out sig, out args, out ret))
									{
										string key = name + sig;
										MethodWrapper existing;
										methodsList.TryGetValue(key, out existing);
										if (existing == null && BaseTypeWrapper != null)
										{
											MethodWrapper baseMethod = BaseTypeWrapper.GetMethodWrapper(name, sig, true);
											if (baseMethod != null && !baseMethod.IsStatic && baseMethod.IsPublic)
											{
												continue;
											}
										}
										if (existing == null || existing is ByRefMethodWrapper || existing.IsStatic || !existing.IsPublic)
										{
											// TODO if existing != null, we need to rename the existing method (but this is complicated because
											// it also affects subclasses). This is especially required is the existing method is abstract,
											// because otherwise we won't be able to create any subclasses in Java.
											methodsList[key] = CreateMethodWrapper(name, sig, args, ret, map.InterfaceMethods[j], true);
										}
									}
								}
							}
						}
					}
				}

				// for non-final remapped types, we need to add all the virtual methods in our alter ego (which
				// appears as our base class) and make them final (to prevent Java code from overriding these
				// methods, which don't really exist).
				if (ClassLoaderWrapper.IsRemappedType(type) && !type.IsSealed && !type.IsInterface)
				{
					// Finish the type, to make sure the methods are populated
					this.BaseTypeWrapper.Finish();
					TypeWrapper baseTypeWrapper = this.BaseTypeWrapper;
					while (baseTypeWrapper != null)
					{
						foreach (MethodWrapper m in baseTypeWrapper.GetMethods())
						{
							if (!m.IsStatic && !m.IsFinal && (m.IsPublic || m.IsProtected) && m.Name != "<init>")
							{
								string key = m.Name + m.Signature;
								if (!methodsList.ContainsKey(key))
								{
									if (m.IsProtected)
									{
										if (m.Name == "finalize" && m.Signature == "()V")
										{
											methodsList.Add(key, new FinalizeMethodWrapper(this));
										}
										else if (m.Name == "clone" && m.Signature == "()Ljava.lang.Object;")
										{
											methodsList.Add(key, new CloneMethodWrapper(this));
										}
										else
										{
											// there should be a special MethodWrapper for this method
											throw new InvalidOperationException("Missing protected method support for " + baseTypeWrapper.Name + "::" + m.Name + m.Signature);
										}
									}
									else
									{
										methodsList.Add(key, new BaseFinalMethodWrapper(this, m));
									}
								}
							}
						}
						baseTypeWrapper = baseTypeWrapper.BaseTypeWrapper;
					}
				}
				MethodWrapper[] methodArray = new MethodWrapper[methodsList.Count];
				methodsList.Values.CopyTo(methodArray, 0);
				SetMethods(methodArray);
			}
		}

		private class BaseFinalMethodWrapper : MethodWrapper
		{
			private MethodWrapper m;

			internal BaseFinalMethodWrapper(DotNetTypeWrapper tw, MethodWrapper m)
				: base(tw, m.Name, m.Signature, m.GetMethod(), m.ReturnType, m.GetParameters(), (m.Modifiers & ~Modifiers.Abstract) | Modifiers.Final, MemberFlags.None)
			{
				this.m = m;
			}

#if !STUB_GENERATOR
			internal override void EmitCall(CodeEmitter ilgen)
			{
				// we direct EmitCall to EmitCallvirt, because we always want to end up at the instancehelper method
				// (EmitCall would go to our alter ego .NET type and that wouldn't be legal)
				m.EmitCallvirt(ilgen);
			}

			internal override void EmitCallvirt(CodeEmitter ilgen)
			{
				m.EmitCallvirt(ilgen);
			}
#endif // !STUB_GENERATOR
		}

		internal static bool IsUnsupportedAbstractMethod(MethodBase mb)
		{
			if (mb.IsAbstract)
			{
				MethodInfo mi = (MethodInfo)mb;
				if (mi.ReturnType.IsByRef || IsPointerType(mi.ReturnType))
				{
					return true;
				}
				foreach (ParameterInfo p in mi.GetParameters())
				{
					if (p.ParameterType.IsByRef || IsPointerType(p.ParameterType))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool IsPointerType(Type type)
		{
			while (type.HasElementType)
			{
				if (type.IsPointer)
				{
					return true;
				}
				type = type.GetElementType();
			}
			return false;
		}

		private bool MakeMethodDescriptor(MethodBase mb, out string name, out string sig, out TypeWrapper[] args, out TypeWrapper ret)
		{
			if (mb.IsGenericMethodDefinition)
			{
				name = null;
				sig = null;
				args = null;
				ret = null;
				return false;
			}
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append('(');
			ParameterInfo[] parameters = mb.GetParameters();
			args = new TypeWrapper[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				Type type = parameters[i].ParameterType;
				if (IsPointerType(type))
				{
					name = null;
					sig = null;
					args = null;
					ret = null;
					return false;
				}
				if (type.IsByRef)
				{
					type = ArrayTypeWrapper.MakeArrayType(type.GetElementType(), 1);
					if (mb.IsAbstract)
					{
						// Since we cannot override methods with byref arguments, we don't report abstract
						// methods with byref args.
						name = null;
						sig = null;
						args = null;
						ret = null;
						return false;
					}
				}
				TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(type);
				args[i] = tw;
				sb.Append(tw.SigName);
			}
			sb.Append(')');
			if (mb is ConstructorInfo)
			{
				ret = PrimitiveTypeWrapper.VOID;
				if (mb.IsStatic)
				{
					name = "<clinit>";
				}
				else
				{
					name = "<init>";
				}
				sb.Append(ret.SigName);
				sig = sb.ToString();
				return true;
			}
			else
			{
				Type type = ((MethodInfo)mb).ReturnType;
				if (IsPointerType(type) || type.IsByRef)
				{
					name = null;
					sig = null;
					ret = null;
					return false;
				}
				ret = ClassLoaderWrapper.GetWrapperFromType(type);
				sb.Append(ret.SigName);
				name = mb.Name;
				sig = sb.ToString();
				return true;
			}
		}

		internal override TypeWrapper[] Interfaces
		{
			get
			{
				if (interfaces == null)
				{
					interfaces = GetImplementedInterfacesAsTypeWrappers(type);
				}
				return interfaces;
			}
		}

		private static bool IsAttribute(Type type)
		{
			if (!type.IsAbstract && type.IsSubclassOf(Types.Attribute) && type.IsVisible)
			{
				//
				// Based on the number of constructors and their arguments, we distinguish several types
				// of attributes:
				//                                   | def ctor | single 1-arg ctor
				// -----------------------------------------------------------------
				// complex only (i.e. Annotation{N}) |          |
				// all optional fields/properties    |    X     |
				// required "value"                  |          |   X
				// optional "value"                  |    X     |   X
				// -----------------------------------------------------------------
				// 
				// TODO currently we don't support "complex only" attributes.
				//
				ConstructorInfo defCtor;
				ConstructorInfo singleOneArgCtor;
				AttributeAnnotationTypeWrapper.GetConstructors(type, out defCtor, out singleOneArgCtor);
				return defCtor != null || singleOneArgCtor != null;
			}
			return false;
		}

		private static bool IsDelegate(Type type)
		{
			// HACK non-public delegates do not get the special treatment (because they are likely to refer to
			// non-public types in the arg list and they're not really useful anyway)
			// NOTE we don't have to check in what assembly the type lives, because this is a DotNetTypeWrapper,
			// we know that it is a different assembly.
			if (!type.IsAbstract && type.IsSubclassOf(Types.MulticastDelegate) && type.IsVisible)
			{
				MethodInfo invoke = type.GetMethod("Invoke");
				if (invoke != null)
				{
					foreach (ParameterInfo p in invoke.GetParameters())
					{
						// TODO at the moment we don't support delegates with pointer or byref parameters
						if (p.ParameterType.IsPointer || p.ParameterType.IsByRef)
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		internal override TypeWrapper[] InnerClasses
		{
			get
			{
				lock (this)
				{
					if (innerClasses == null)
					{
						Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);
						List<TypeWrapper> list = new List<TypeWrapper>(nestedTypes.Length);
						for (int i = 0; i < nestedTypes.Length; i++)
						{
							if (!nestedTypes[i].IsGenericTypeDefinition)
							{
								list.Add(ClassLoaderWrapper.GetWrapperFromType(nestedTypes[i]));
							}
						}
						if (IsDelegate(type))
						{
							ClassLoaderWrapper classLoader = GetClassLoader();
							list.Add(classLoader.RegisterInitiatingLoader(new DelegateInnerClassTypeWrapper(Name + DelegateInterfaceSuffix, type, classLoader)));
						}
						if (IsAttribute(type))
						{
							list.Add(GetClassLoader().RegisterInitiatingLoader(new AttributeAnnotationTypeWrapper(Name + AttributeAnnotationSuffix, type)));
						}
						if (type.IsEnum && type.IsVisible)
						{
							list.Add(GetClassLoader().RegisterInitiatingLoader(new EnumEnumTypeWrapper(Name + EnumEnumSuffix, type)));
						}
						innerClasses = list.ToArray();
					}
				}
				return innerClasses;
			}
		}

		internal override bool IsFakeTypeContainer
		{
			get
			{
				return IsDelegate(type) || IsAttribute(type) || (type.IsEnum && type.IsVisible);
			}
		}

		internal override TypeWrapper DeclaringTypeWrapper
		{
			get
			{
				if (outerClass == null)
				{
					Type outer = type.DeclaringType;
					if (outer != null && !type.IsGenericType)
					{
						outerClass = ClassLoaderWrapper.GetWrapperFromType(outer);
					}
				}
				return outerClass;
			}
		}

		internal override Modifiers ReflectiveModifiers
		{
			get
			{
				if (DeclaringTypeWrapper != null)
				{
					return Modifiers | Modifiers.Static;
				}
				return Modifiers;
			}
		}

		private FieldWrapper CreateFieldWrapperDotNet(Modifiers modifiers, string name, Type fieldType, FieldInfo field)
		{
			TypeWrapper type = ClassLoaderWrapper.GetWrapperFromType(fieldType);
			if (field.IsLiteral)
			{
				return new ConstantFieldWrapper(this, type, name, type.SigName, modifiers, field, null, MemberFlags.None);
			}
			else
			{
				return FieldWrapper.Create(this, type, field, name, type.SigName, new ExModifiers(modifiers, false));
			}
		}

		// this method detects if type derives from our java.lang.Object or java.lang.Throwable implementation types
		private static bool IsRemappedImplDerived(Type type)
		{
			for (; type != null; type = type.BaseType)
			{
				if (!ClassLoaderWrapper.IsRemappedType(type) && ClassLoaderWrapper.GetWrapperFromType(type).IsRemapped)
				{
					return true;
				}
			}
			return false;
		}

		private MethodWrapper CreateMethodWrapper(string name, string sig, TypeWrapper[] argTypeWrappers, TypeWrapper retTypeWrapper, MethodBase mb, bool privateInterfaceImplHack)
		{
			ExModifiers exmods = AttributeHelper.GetModifiers(mb, true);
			Modifiers mods = exmods.Modifiers;
			if (name == "Finalize" && sig == "()V" && !mb.IsStatic &&
				IsRemappedImplDerived(TypeAsBaseType))
			{
				// TODO if the .NET also has a "finalize" method, we need to hide that one (or rename it, or whatever)
				MethodWrapper mw = new SimpleCallMethodWrapper(this, "finalize", "()V", (MethodInfo)mb, PrimitiveTypeWrapper.VOID, TypeWrapper.EmptyArray, mods, MemberFlags.None, SimpleOpCode.Call, SimpleOpCode.Callvirt);
				mw.SetDeclaredExceptions(new string[] { "java.lang.Throwable" });
				return mw;
			}
			ParameterInfo[] parameters = mb.GetParameters();
			Type[] args = new Type[parameters.Length];
			bool hasByRefArgs = false;
			bool[] byrefs = null;
			for (int i = 0; i < parameters.Length; i++)
			{
				args[i] = parameters[i].ParameterType;
				if (parameters[i].ParameterType.IsByRef)
				{
					if (byrefs == null)
					{
						byrefs = new bool[args.Length];
					}
					byrefs[i] = true;
					hasByRefArgs = true;
				}
			}
			if (privateInterfaceImplHack)
			{
				mods &= ~Modifiers.Abstract;
				mods |= Modifiers.Final;
			}
			if (hasByRefArgs)
			{
				if (!(mb is ConstructorInfo) && !mb.IsStatic)
				{
					mods |= Modifiers.Final;
				}
				return new ByRefMethodWrapper(args, byrefs, this, name, sig, mb, retTypeWrapper, argTypeWrappers, mods, false);
			}
			else
			{
				if (mb is ConstructorInfo)
				{
					return new SmartConstructorMethodWrapper(this, name, sig, (ConstructorInfo)mb, argTypeWrappers, mods, MemberFlags.None);
				}
				else
				{
					return new SmartCallMethodWrapper(this, name, sig, (MethodInfo)mb, retTypeWrapper, argTypeWrappers, mods, MemberFlags.None, SimpleOpCode.Call, SimpleOpCode.Callvirt);
				}
			}
		}

		internal override Type TypeAsTBD
		{
			get
			{
				return type;
			}
		}

		internal override bool IsRemapped
		{
			get
			{
				return ClassLoaderWrapper.IsRemappedType(type);
			}
		}

#if !STUB_GENERATOR
		internal override void EmitInstanceOf(TypeWrapper context, CodeEmitter ilgen)
		{
			if (IsRemapped)
			{
				TypeWrapper shadow = ClassLoaderWrapper.GetWrapperFromType(type);
				MethodInfo method = shadow.TypeAsBaseType.GetMethod("__<instanceof>");
				if (method != null)
				{
					ilgen.Emit(OpCodes.Call, method);
					return;
				}
			}
			ilgen.LazyEmit_instanceof(type);
		}

		internal override void EmitCheckcast(TypeWrapper context, CodeEmitter ilgen)
		{
			if (IsRemapped)
			{
				TypeWrapper shadow = ClassLoaderWrapper.GetWrapperFromType(type);
				MethodInfo method = shadow.TypeAsBaseType.GetMethod("__<checkcast>");
				if (method != null)
				{
					ilgen.Emit(OpCodes.Call, method);
					return;
				}
			}
			ilgen.EmitCastclass(type);
		}
#endif // !STUB_GENERATOR

		internal override void Finish()
		{
			if (BaseTypeWrapper != null)
			{
				BaseTypeWrapper.Finish();
			}
			foreach (TypeWrapper tw in this.Interfaces)
			{
				tw.Finish();
			}
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal override object[] GetDeclaredAnnotations()
		{
			return type.GetCustomAttributes(false);
		}

		internal override object[] GetFieldAnnotations(FieldWrapper fw)
		{
			FieldInfo fi = fw.GetField();
			if (fi == null)
			{
				return null;
			}
			return fi.GetCustomAttributes(false);
		}

		internal override object[] GetMethodAnnotations(MethodWrapper mw)
		{
			MethodBase mb = mw.GetMethod();
			if (mb == null)
			{
				return null;
			}
			return mb.GetCustomAttributes(false);
		}

		internal override object[][] GetParameterAnnotations(MethodWrapper mw)
		{
			MethodBase mb = mw.GetMethod();
			if (mb == null)
			{
				return null;
			}
			ParameterInfo[] parameters = mb.GetParameters();
			object[][] attribs = new object[parameters.Length][];
			for (int i = 0; i < parameters.Length; i++)
			{
				attribs[i] = parameters[i].GetCustomAttributes(false);
			}
			return attribs;
		}
#endif

		internal override bool IsFastClassLiteralSafe
		{
			get { return true; }
		}
	}
}
