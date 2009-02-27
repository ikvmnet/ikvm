/*
  Copyright (C) 2007 Jeroen Frijters

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
using System.Reflection;
#if IKVM_REF_EMIT
using IKVM.Reflection.Emit;
#else
using System.Reflection.Emit;
#endif
using IKVM.Internal;

static class AtomicReferenceFieldUpdaterEmitter
{
	private static readonly Dictionary<FieldWrapper, ConstructorBuilder> map = new Dictionary<FieldWrapper, ConstructorBuilder>();

	internal static bool Emit(DynamicTypeWrapper.FinishContext context, TypeWrapper wrapper, CodeEmitter ilgen, ClassFile classFile, int i, ClassFile.Method.Instruction[] code)
	{
		if (i >= 3
			&& !code[i - 0].IsBranchTarget
			&& !code[i - 1].IsBranchTarget
			&& !code[i - 2].IsBranchTarget
			&& code[i - 1].NormalizedOpCode == NormalizedByteCode.__ldc
			&& code[i - 2].NormalizedOpCode == NormalizedByteCode.__ldc
			&& code[i - 3].NormalizedOpCode == NormalizedByteCode.__ldc)
		{
			// we now have a structural match, now we need to make sure that the argument values are what we expect
			TypeWrapper tclass = classFile.GetConstantPoolClassType(code[i - 3].Arg1);
			TypeWrapper vclass = classFile.GetConstantPoolClassType(code[i - 2].Arg1);
			string fieldName = classFile.GetConstantPoolConstantString(code[i - 1].Arg1);
			if (tclass == wrapper && !vclass.IsUnloadable && !vclass.IsPrimitive && !vclass.IsNonPrimitiveValueType)
			{
				FieldWrapper field = wrapper.GetFieldWrapper(fieldName, vclass.SigName);
				if (field != null && !field.IsStatic && field.IsVolatile && field.DeclaringType == wrapper && field.FieldTypeWrapper == vclass)
				{
					// everything matches up, now call the actual emitter
					DoEmit(context, wrapper, ilgen, field);
					return true;
				}
			}
		}
		return false;
	}

	private static void DoEmit(DynamicTypeWrapper.FinishContext context, TypeWrapper wrapper, CodeEmitter ilgen, FieldWrapper field)
	{
		ConstructorBuilder cb;
		bool exists;
		lock (map)
		{
			exists = map.TryGetValue(field, out cb);
		}
		if (!exists)
		{
			// note that we don't need to lock here, because we're running as part of FinishCore, which is already protected by a lock
			TypeWrapper arfuTypeWrapper = ClassLoaderWrapper.LoadClassCritical("java.util.concurrent.atomic.AtomicReferenceFieldUpdater");
			TypeBuilder tb = wrapper.TypeAsBuilder.DefineNestedType("__ARFU_" + field.Name + field.Signature.Replace('.', '/'), TypeAttributes.NestedPrivate | TypeAttributes.Sealed, arfuTypeWrapper.TypeAsBaseType);
			EmitCompareAndSet("compareAndSet", tb, field.GetField());
			EmitCompareAndSet("weakCompareAndSet", tb, field.GetField());
			EmitGet(tb, field.GetField());
			EmitSet("set", tb, field.GetField(), false);
			EmitSet("lazySet", tb, field.GetField(), true);

			cb = tb.DefineConstructor(MethodAttributes.Assembly, CallingConventions.Standard, Type.EmptyTypes);
			lock (map)
			{
				map.Add(field, cb);
			}
			CodeEmitter ctorilgen = CodeEmitter.Create(cb);
			ctorilgen.Emit(OpCodes.Ldarg_0);
			MethodWrapper basector = arfuTypeWrapper.GetMethodWrapper("<init>", "()V", false);
			basector.Link();
			basector.EmitCall(ctorilgen);
			ctorilgen.Emit(OpCodes.Ret);
			context.RegisterPostFinishProc(delegate
			{
				arfuTypeWrapper.Finish();
				tb.CreateType();
			});
		}
		ilgen.LazyEmitPop();
		ilgen.LazyEmitPop();
		ilgen.LazyEmitPop();
		ilgen.Emit(OpCodes.Newobj, cb);
	}

	private static void EmitCompareAndSet(string name, TypeBuilder tb, FieldInfo field)
	{
		MethodBuilder compareAndSet = tb.DefineMethod(name, MethodAttributes.Public | MethodAttributes.Virtual, typeof(bool), new Type[] { typeof(object), typeof(object), typeof(object) });
		ILGenerator ilgen = compareAndSet.GetILGenerator();
		ilgen.Emit(OpCodes.Ldarg_1);
		ilgen.Emit(OpCodes.Castclass, field.DeclaringType);
		ilgen.Emit(OpCodes.Ldflda, field);
		ilgen.Emit(OpCodes.Ldarg_3);
		ilgen.Emit(OpCodes.Castclass, field.FieldType);
		ilgen.Emit(OpCodes.Ldarg_2);
		ilgen.Emit(OpCodes.Castclass, field.FieldType);
		ilgen.Emit(OpCodes.Call, MakeCompareExchange(field.FieldType));
		ilgen.Emit(OpCodes.Ldarg_2);
		ilgen.Emit(OpCodes.Ceq);
		ilgen.Emit(OpCodes.Ret);
	}

	private static MethodInfo MakeCompareExchange(Type type)
	{
		return new CompareExchangeMethodInfo(type);
		// MONOBUG this doesn't work in Mono (because we're closing a generic method over our own Type implementation)
		/*
		MethodInfo interlockedCompareExchange = null;
		foreach (MethodInfo m in typeof(System.Threading.Interlocked).GetMethods())
		{
			if (m.Name == "CompareExchange" && m.IsGenericMethodDefinition)
			{
				interlockedCompareExchange = m;
				break;
			}
		}
		return interlockedCompareExchange.MakeGenericMethod(type);
		 */
	}

	private sealed class CompareExchangeMethodInfo : MethodInfo
	{
		private readonly Type type;

		internal CompareExchangeMethodInfo(Type type)
		{
			this.type = type;
		}

		public override MethodInfo GetBaseDefinition()
		{
			throw new NotImplementedException();
		}

		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get { throw new NotImplementedException(); }
		}

		public override MethodAttributes Attributes
		{
			get { return MethodAttributes.Public; }
		}

		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			throw new NotImplementedException();
		}

		private sealed class MyParameterInfo : ParameterInfo
		{
			private readonly Type type;

			internal MyParameterInfo(Type type)
			{
				this.type = type;
			}

			public override Type ParameterType
			{
				get
				{
					return type;
				}
			}
		}

		public override ParameterInfo[] GetParameters()
		{
			return new ParameterInfo[] { 
				new MyParameterInfo(type.MakeByRefType()),
				new MyParameterInfo(type),
				new MyParameterInfo(type)
			};
		}

		public override ParameterInfo ReturnParameter
		{
			get
			{
				return new MyParameterInfo(type);
			}
		}

		public override Type ReturnType
		{
			get
			{
				return type;
			}
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
			get { return typeof(System.Threading.Interlocked); }
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
			get { return "CompareExchange"; }
		}

		public override Type ReflectedType
		{
			get { return DeclaringType; }
		}

		public override bool IsGenericMethod
		{
			get { return true; }
		}

		public override Type[] GetGenericArguments()
		{
			return new Type[] { type };
		}

		public override MethodInfo GetGenericMethodDefinition()
		{
			foreach (MethodInfo m in typeof(System.Threading.Interlocked).GetMethods())
			{
				if (m.Name == "CompareExchange" && m.IsGenericMethodDefinition)
				{
					return m;
				}
			}
			throw new InvalidOperationException();
		}
	}

	private static void EmitGet(TypeBuilder tb, FieldInfo field)
	{
		MethodBuilder get = tb.DefineMethod("get", MethodAttributes.Public | MethodAttributes.Virtual, typeof(object), new Type[] { typeof(object) });
		ILGenerator ilgen = get.GetILGenerator();
		ilgen.Emit(OpCodes.Ldarg_1);
		ilgen.Emit(OpCodes.Castclass, field.DeclaringType);
		ilgen.Emit(OpCodes.Volatile);
		ilgen.Emit(OpCodes.Ldfld, field);
		ilgen.Emit(OpCodes.Ret);
	}

	private static void EmitSet(string name, TypeBuilder tb, FieldInfo field, bool lazy)
	{
		MethodBuilder set = tb.DefineMethod(name, MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), new Type[] { typeof(object), typeof(object) });
		ILGenerator ilgen = set.GetILGenerator();
		ilgen.Emit(OpCodes.Ldarg_1);
		ilgen.Emit(OpCodes.Castclass, field.DeclaringType);
		ilgen.Emit(OpCodes.Ldarg_2);
		ilgen.Emit(OpCodes.Castclass, field.FieldType);
		if (!lazy)
		{
			ilgen.Emit(OpCodes.Volatile);
		}
		ilgen.Emit(OpCodes.Stfld, field);
		ilgen.Emit(OpCodes.Ret);
	}
}
