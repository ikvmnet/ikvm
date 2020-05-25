/*
  Copyright (C) 2008-2013 Jeroen Frijters

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
#if STATIC_COMPILER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif
using System.Diagnostics;
using Instruction = IKVM.Internal.ClassFile.Method.Instruction;
using InstructionFlags = IKVM.Internal.ClassFile.Method.InstructionFlags;

namespace IKVM.Internal
{
	sealed class EmitIntrinsicContext
	{
		internal readonly MethodWrapper Method;
		internal readonly DynamicTypeWrapper.FinishContext Context;
		internal readonly CodeEmitter Emitter;
		private readonly CodeInfo ma;
		internal readonly int OpcodeIndex;
		internal readonly MethodWrapper Caller;
		internal readonly ClassFile ClassFile;
		internal readonly Instruction[] Code;
		internal readonly InstructionFlags[] Flags;
		internal bool NonLeaf = true;

		internal EmitIntrinsicContext(MethodWrapper method, DynamicTypeWrapper.FinishContext context, CodeEmitter ilgen, CodeInfo ma, int opcodeIndex, MethodWrapper caller, ClassFile classFile, Instruction[] code, InstructionFlags[] flags)
		{
			this.Method = method;
			this.Context = context;
			this.Emitter = ilgen;
			this.ma = ma;
			this.OpcodeIndex = opcodeIndex;
			this.Caller = caller;
			this.ClassFile = classFile;
			this.Code = code;
			this.Flags = flags;
		}

		internal bool MatchRange(int offset, int length)
		{
			if (OpcodeIndex + offset < 0)
			{
				return false;
			}
			if (OpcodeIndex + offset + length > Code.Length)
			{
				return false;
			}
			// we check for branches *into* the range, the start of the range may be a branch target
			for (int i = OpcodeIndex + offset + 1, end = OpcodeIndex + offset + length; i < end; i++)
			{
				if ((Flags[i] & InstructionFlags.BranchTarget) != 0)
				{
					return false;
				}
			}
			return true;
		}

		internal bool Match(int offset, NormalizedByteCode opcode)
		{
			return Code[OpcodeIndex + offset].NormalizedOpCode == opcode;
		}

		internal bool Match(int offset, NormalizedByteCode opcode, int arg)
		{
			return Code[OpcodeIndex + offset].NormalizedOpCode == opcode && Code[OpcodeIndex + offset].Arg1 == arg;
		}

		internal TypeWrapper GetStackTypeWrapper(int offset, int pos)
		{
			return ma.GetStackTypeWrapper(OpcodeIndex + offset, pos);
		}

		internal ClassFile.ConstantPoolItemMI GetMethodref(int offset)
		{
			return ClassFile.GetMethodref(Code[OpcodeIndex + offset].Arg1);
		}

		internal ClassFile.ConstantPoolItemFieldref GetFieldref(int offset)
		{
			return ClassFile.GetFieldref(Code[OpcodeIndex + offset].Arg1);
		}

		internal TypeWrapper GetClassLiteral(int offset)
		{
			return ClassFile.GetConstantPoolClassType(Code[OpcodeIndex + offset].Arg1);
		}

		internal string GetStringLiteral(int offset)
		{
			return ClassFile.GetConstantPoolConstantString(Code[OpcodeIndex + offset].Arg1);
		}

		internal ClassFile.ConstantType GetConstantType(int offset)
		{
			return ClassFile.GetConstantPoolConstantType(Code[OpcodeIndex + offset].Arg1);
		}

		internal void PatchOpCode(int offset, NormalizedByteCode opc)
		{
			Code[OpcodeIndex + offset].PatchOpCode(opc);
		}
	}

	static class Intrinsics
	{
		private delegate bool Emitter(EmitIntrinsicContext eic);
		private struct IntrinsicKey : IEquatable<IntrinsicKey>
		{
			private readonly string className;
			private readonly string methodName;
			private readonly string methodSignature;

			internal IntrinsicKey(string className, string methodName, string methodSignature)
			{
				this.className = string.Intern(className);
				this.methodName = string.Intern(methodName);
				this.methodSignature = string.Intern(methodSignature);
			}

			internal IntrinsicKey(MethodWrapper mw)
			{
				this.className = mw.DeclaringType.Name;
				this.methodName = mw.Name;
				this.methodSignature = mw.Signature;
			}

			public override bool Equals(object obj)
			{
				return Equals((IntrinsicKey)obj);
			}

			public bool Equals(IntrinsicKey other)
			{
				return ReferenceEquals(className, other.className) && ReferenceEquals(methodName, other.methodName) && ReferenceEquals(methodSignature, other.methodSignature);
			}

			public override int GetHashCode()
			{
				return methodName.GetHashCode();
			}
		}
		private static readonly Dictionary<IntrinsicKey, Emitter> intrinsics = Register();
#if STATIC_COMPILER
		private static readonly Type typeofFloatConverter = StaticCompiler.GetRuntimeType("IKVM.Runtime.FloatConverter");
		private static readonly Type typeofDoubleConverter = StaticCompiler.GetRuntimeType("IKVM.Runtime.DoubleConverter");
#else
		private static readonly Type typeofFloatConverter = typeof(IKVM.Runtime.FloatConverter);
		private static readonly Type typeofDoubleConverter = typeof(IKVM.Runtime.DoubleConverter);
#endif

		private static Dictionary<IntrinsicKey, Emitter> Register()
		{
			Dictionary<IntrinsicKey, Emitter> intrinsics = new Dictionary<IntrinsicKey, Emitter>();
			intrinsics.Add(new IntrinsicKey("java.lang.Object", "getClass", "()Ljava.lang.Class;"), Object_getClass);
			intrinsics.Add(new IntrinsicKey("java.lang.Class", "desiredAssertionStatus", "()Z"), Class_desiredAssertionStatus);
			intrinsics.Add(new IntrinsicKey("java.lang.Float", "floatToRawIntBits", "(F)I"), Float_floatToRawIntBits);
			intrinsics.Add(new IntrinsicKey("java.lang.Float", "intBitsToFloat", "(I)F"), Float_intBitsToFloat);
			intrinsics.Add(new IntrinsicKey("java.lang.Double", "doubleToRawLongBits", "(D)J"), Double_doubleToRawLongBits);
			intrinsics.Add(new IntrinsicKey("java.lang.Double", "longBitsToDouble", "(J)D"), Double_longBitsToDouble);
			intrinsics.Add(new IntrinsicKey("java.lang.System", "arraycopy", "(Ljava.lang.Object;ILjava.lang.Object;II)V"), System_arraycopy);
			intrinsics.Add(new IntrinsicKey("java.util.concurrent.atomic.AtomicReferenceFieldUpdater", "newUpdater", "(Ljava.lang.Class;Ljava.lang.Class;Ljava.lang.String;)Ljava.util.concurrent.atomic.AtomicReferenceFieldUpdater;"), AtomicReferenceFieldUpdater_newUpdater);
#if STATIC_COMPILER
			// String_toCharArray relies on globals, which aren't usable in dynamic mode
			intrinsics.Add(new IntrinsicKey("java.lang.String", "toCharArray", "()[C"), String_toCharArray);
			intrinsics.Add(new IntrinsicKey("sun.reflect.Reflection", "getCallerClass", "()Ljava.lang.Class;"), Reflection_getCallerClass);
			intrinsics.Add(new IntrinsicKey("ikvm.internal.CallerID", "getCallerID", "()Likvm.internal.CallerID;"), CallerID_getCallerID);
#endif
			intrinsics.Add(new IntrinsicKey("ikvm.runtime.Util", "getInstanceTypeFromClass", "(Ljava.lang.Class;)Lcli.System.Type;"), Util_getInstanceTypeFromClass);
#if STATIC_COMPILER
			// this only applies to the core class library, so makes no sense in dynamic mode
			intrinsics.Add(new IntrinsicKey("java.lang.Class", "getPrimitiveClass", "(Ljava.lang.String;)Ljava.lang.Class;"), Class_getPrimitiveClass);
			intrinsics.Add(new IntrinsicKey("java.lang.Class", "getDeclaredField", "(Ljava.lang.String;)Ljava.lang.reflect.Field;"), Class_getDeclaredField);
#endif
			intrinsics.Add(new IntrinsicKey("java.lang.ThreadLocal", "<init>", "()V"), ThreadLocal_new);
			intrinsics.Add(new IntrinsicKey("sun.misc.Unsafe", "ensureClassInitialized", "(Ljava.lang.Class;)V"), Unsafe_ensureClassInitialized);
			// note that the following intrinsics don't pay off on CLR v2, but they do on CLR v4
			intrinsics.Add(new IntrinsicKey("sun.misc.Unsafe", "putObject", "(Ljava.lang.Object;JLjava.lang.Object;)V"), Unsafe_putObject);
			intrinsics.Add(new IntrinsicKey("sun.misc.Unsafe", "putOrderedObject", "(Ljava.lang.Object;JLjava.lang.Object;)V"), Unsafe_putOrderedObject);
			intrinsics.Add(new IntrinsicKey("sun.misc.Unsafe", "putObjectVolatile", "(Ljava.lang.Object;JLjava.lang.Object;)V"), Unsafe_putObjectVolatile);
			intrinsics.Add(new IntrinsicKey("sun.misc.Unsafe", "getObjectVolatile", "(Ljava.lang.Object;J)Ljava.lang.Object;"), Unsafe_getObjectVolatile);
			intrinsics.Add(new IntrinsicKey("sun.misc.Unsafe", "getObject", "(Ljava.lang.Object;J)Ljava.lang.Object;"), Unsafe_getObjectVolatile);
			intrinsics.Add(new IntrinsicKey("sun.misc.Unsafe", "compareAndSwapObject", "(Ljava.lang.Object;JLjava.lang.Object;Ljava.lang.Object;)Z"), Unsafe_compareAndSwapObject);
			intrinsics.Add(new IntrinsicKey("sun.misc.Unsafe", "getAndSetObject", "(Ljava.lang.Object;JLjava.lang.Object;)Ljava.lang.Object;"), Unsafe_getAndSetObject);
			intrinsics.Add(new IntrinsicKey("sun.misc.Unsafe", "compareAndSwapInt", "(Ljava.lang.Object;JII)Z"), Unsafe_compareAndSwapInt);
			intrinsics.Add(new IntrinsicKey("sun.misc.Unsafe", "getAndAddInt", "(Ljava.lang.Object;JI)I"), Unsafe_getAndAddInt);
			intrinsics.Add(new IntrinsicKey("sun.misc.Unsafe", "compareAndSwapLong", "(Ljava.lang.Object;JJJ)Z"), Unsafe_compareAndSwapLong);
			return intrinsics;
		}

		internal static bool IsIntrinsic(MethodWrapper mw)
		{
			return intrinsics.ContainsKey(new IntrinsicKey(mw)) && mw.DeclaringType.GetClassLoader() == CoreClasses.java.lang.Object.Wrapper.GetClassLoader();
		}

		internal static bool Emit(EmitIntrinsicContext context)
		{
			// note that intrinsics can always refuse to emit code and the code generator will fall back to a normal method call
			return intrinsics[new IntrinsicKey(context.Method)](context);
		}

		private static bool Object_getClass(EmitIntrinsicContext eic)
		{
			// this is the null-check idiom that javac uses (both in its own source and in the code it generates)
			if (eic.MatchRange(0, 2)
				&& eic.Match(1, NormalizedByteCode.__pop))
			{
				eic.Emitter.Emit(OpCodes.Dup);
				eic.Emitter.EmitNullCheck();
				return true;
			}
			// this optimizes obj1.getClass() ==/!= obj2.getClass()
			else if (eic.MatchRange(0, 4)
				&& eic.Match(1, NormalizedByteCode.__aload)
				&& eic.Match(2, NormalizedByteCode.__invokevirtual)
				&& (eic.Match(3, NormalizedByteCode.__if_acmpeq) || eic.Match(3, NormalizedByteCode.__if_acmpne))
				&& (IsSafeForGetClassOptimization(eic.GetStackTypeWrapper(0, 0)) || IsSafeForGetClassOptimization(eic.GetStackTypeWrapper(2, 0))))
			{
				ClassFile.ConstantPoolItemMI cpi = eic.GetMethodref(2);
				if (cpi.Class == "java.lang.Object" && cpi.Name == "getClass" && cpi.Signature == "()Ljava.lang.Class;")
				{
					// we can't patch the current opcode, so we have to emit the first call to GetTypeHandle here
					eic.Emitter.Emit(OpCodes.Callvirt, Compiler.getTypeMethod);
					eic.PatchOpCode(2, NormalizedByteCode.__intrinsic_gettype);
					return true;
				}
			}
			// this optimizes obj.getClass() == Xxx.class
			else if (eic.MatchRange(0, 3)
				&& eic.Match(1, NormalizedByteCode.__ldc) && eic.GetConstantType(1) == ClassFile.ConstantType.Class
				&& (eic.Match(2, NormalizedByteCode.__if_acmpeq) || eic.Match(2, NormalizedByteCode.__if_acmpne)))
			{
				TypeWrapper tw = eic.GetClassLiteral(1);
				if (tw.IsGhost || tw.IsGhostArray || tw.IsUnloadable || (tw.IsRemapped && tw.IsFinal && tw is DotNetTypeWrapper))
				{
					return false;
				}
				eic.Emitter.Emit(OpCodes.Callvirt, Compiler.getTypeMethod);
				eic.Emitter.Emit(OpCodes.Ldtoken, (tw.IsRemapped && tw.IsFinal) ? tw.TypeAsTBD : tw.TypeAsBaseType);
				eic.Emitter.Emit(OpCodes.Call, Compiler.getTypeFromHandleMethod);
				eic.PatchOpCode(1, NormalizedByteCode.__nop);
				return true;
			}
			return false;
		}

		private static bool Class_desiredAssertionStatus(EmitIntrinsicContext eic)
		{
			if (eic.MatchRange(-1, 2)
				&& eic.Match(-1, NormalizedByteCode.__ldc))
			{
				TypeWrapper classLiteral = eic.GetClassLiteral(-1);
				if (!classLiteral.IsUnloadable && classLiteral.GetClassLoader().RemoveAsserts)
				{
					eic.Emitter.Emit(OpCodes.Pop);
					eic.Emitter.EmitLdc_I4(0);
					return true;
				}
			}
			return false;
		}

#if STATIC_COMPILER
		// this intrinsifies the following two patterns:
		//   unsafe.objectFieldOffset(XXX.class.getDeclaredField("xxx"));
		// and
		//   Class k = XXX.class;
		//   unsafe.objectFieldOffset(k.getDeclaredField("xxx"));
		// to avoid initializing the full reflection machinery at this point
		private static bool Class_getDeclaredField(EmitIntrinsicContext eic)
		{
			if (eic.Caller.DeclaringType.GetClassLoader() != CoreClasses.java.lang.Object.Wrapper.GetClassLoader())
			{
				// we can only do this optimization when compiling the trusted core classes
				return false;
			}
			TypeWrapper fieldClass;
			if (eic.MatchRange(-2, 4)
				&& eic.Match(-2, NormalizedByteCode.__ldc)
				&& eic.Match(-1, NormalizedByteCode.__ldc_nothrow)
				&& eic.Match(1, NormalizedByteCode.__invokevirtual))
			{
				// unsafe.objectFieldOffset(XXX.class.getDeclaredField("xxx"));
				fieldClass = eic.GetClassLiteral(-2);
			}
			else if (eic.MatchRange(-5, 7)
				&& eic.Match(-5, NormalizedByteCode.__ldc)
				&& eic.Match(-4, NormalizedByteCode.__astore)
				&& eic.Match(-3, NormalizedByteCode.__getstatic)
				&& eic.Match(-2, NormalizedByteCode.__aload, eic.Code[eic.OpcodeIndex - 4].NormalizedArg1)
				&& eic.Match(-1, NormalizedByteCode.__ldc_nothrow)
				&& eic.Match(1, NormalizedByteCode.__invokevirtual))
			{
				// Class k = XXX.class;
				// unsafe.objectFieldOffset(k.getDeclaredField("xxx"));
				fieldClass = eic.GetClassLiteral(-5);
			}
			else
			{
				return false;
			}
			FieldWrapper field = null;
			string fieldName = eic.GetStringLiteral(-1);
			foreach (FieldWrapper fw in fieldClass.GetFields())
			{
				if (fw.Name == fieldName)
				{
					if (field != null)
					{
						return false;
					}
					field = fw;
				}
			}
			if (field == null || field.IsStatic)
			{
				return false;
			}
			ClassFile.ConstantPoolItemMI cpi = eic.GetMethodref(1);
			if (cpi.Class == "sun.misc.Unsafe" && cpi.Name == "objectFieldOffset" && cpi.Signature == "(Ljava.lang.reflect.Field;)J")
			{
				MethodWrapper mw = ClassLoaderWrapper.LoadClassCritical("sun.misc.Unsafe")
					.GetMethodWrapper("objectFieldOffset", "(Ljava.lang.Class;Ljava.lang.String;)J", false);
				mw.Link();
				mw.EmitCallvirt(eic.Emitter);
				eic.PatchOpCode(1, NormalizedByteCode.__nop);
				return true;
			}
			return false;
		}
#endif

		private static bool IsSafeForGetClassOptimization(TypeWrapper tw)
		{
			// because of ghost arrays, we don't optimize if both types are either java.lang.Object or an array
			return tw != CoreClasses.java.lang.Object.Wrapper && !tw.IsArray;
		}

		private static bool Float_floatToRawIntBits(EmitIntrinsicContext eic)
		{
			EmitConversion(eic.Emitter, typeofFloatConverter, "ToInt");
			return true;
		}

		private static bool Float_intBitsToFloat(EmitIntrinsicContext eic)
		{
			EmitConversion(eic.Emitter, typeofFloatConverter, "ToFloat");
			return true;
		}

		private static bool Double_doubleToRawLongBits(EmitIntrinsicContext eic)
		{
			EmitConversion(eic.Emitter, typeofDoubleConverter, "ToLong");
			return true;
		}

		private static bool Double_longBitsToDouble(EmitIntrinsicContext eic)
		{
			EmitConversion(eic.Emitter, typeofDoubleConverter, "ToDouble");
			return true;
		}

		private static void EmitConversion(CodeEmitter ilgen, Type converterType, string method)
		{
			CodeEmitterLocal converter = ilgen.UnsafeAllocTempLocal(converterType);
			ilgen.Emit(OpCodes.Ldloca, converter);
			ilgen.Emit(OpCodes.Call, converterType.GetMethod(method));
		}

		private static bool System_arraycopy(EmitIntrinsicContext eic)
		{
			// if the array arguments on the stack are of a known array type, we can redirect to an optimized version of arraycopy.
			TypeWrapper dst_type = eic.GetStackTypeWrapper(0, 2);
			TypeWrapper src_type = eic.GetStackTypeWrapper(0, 4);
			if (!dst_type.IsUnloadable && dst_type.IsArray && dst_type == src_type)
			{
				switch (dst_type.Name[1])
				{
					case 'J':
					case 'D':
						eic.Emitter.Emit(OpCodes.Call, ByteCodeHelperMethods.arraycopy_primitive_8);
						break;
					case 'I':
					case 'F':
						eic.Emitter.Emit(OpCodes.Call, ByteCodeHelperMethods.arraycopy_primitive_4);
						break;
					case 'S':
					case 'C':
						eic.Emitter.Emit(OpCodes.Call, ByteCodeHelperMethods.arraycopy_primitive_2);
						break;
					case 'B':
					case 'Z':
						eic.Emitter.Emit(OpCodes.Call, ByteCodeHelperMethods.arraycopy_primitive_1);
						break;
					default:
						// TODO once the verifier tracks actual types (i.e. it knows that
						// a particular reference is the result of a "new" opcode) we can
						// use the fast version if the exact destination type is known
						// (in that case the "dst_type == src_type" above should
						// be changed to "src_type.IsAssignableTo(dst_type)".
						TypeWrapper elemtw = dst_type.ElementTypeWrapper;
						// note that IsFinal returns true for array types, so we have to be careful!
						if (!elemtw.IsArray && elemtw.IsFinal)
						{
							eic.Emitter.Emit(OpCodes.Call, ByteCodeHelperMethods.arraycopy_fast);
						}
						else
						{
							eic.Emitter.Emit(OpCodes.Call, ByteCodeHelperMethods.arraycopy);
						}
						break;
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		private static bool AtomicReferenceFieldUpdater_newUpdater(EmitIntrinsicContext eic)
		{
			return AtomicReferenceFieldUpdaterEmitter.Emit(eic.Context, eic.Caller.DeclaringType, eic.Emitter, eic.ClassFile, eic.OpcodeIndex, eic.Code, eic.Flags);
		}

#if STATIC_COMPILER
		private static bool String_toCharArray(EmitIntrinsicContext eic)
		{
			if (eic.MatchRange(-1, 2)
				&& eic.Match(-1, NormalizedByteCode.__ldc_nothrow))
			{
				string str = eic.GetStringLiteral(-1);
				// arbitrary length for "big" strings
				if (str.Length > 128)
				{
					eic.Emitter.Emit(OpCodes.Pop);
					EmitLoadCharArrayLiteral(eic.Emitter, str, eic.Caller.DeclaringType);
					return true;
				}
			}
			return false;
		}

		private static void EmitLoadCharArrayLiteral(CodeEmitter ilgen, string str, TypeWrapper tw)
		{
			ModuleBuilder mod = tw.GetClassLoader().GetTypeWrapperFactory().ModuleBuilder;
			// FXBUG on .NET 1.1 & 2.0 the value type that Ref.Emit automatically generates is public,
			// so we pre-create a non-public type with the right name here and it will "magically" use
			// that instead.
			// If we're running on Mono this isn't necessary, but for simplicitly we'll simply create
			// the type as well (it is useless, but all it does is waste a little space).
			int length = str.Length * 2;
			string typename = "$ArrayType$" + length;
			Type type = mod.GetType(typename, false, false);
			if (type == null)
			{
				if (tw.GetClassLoader().GetTypeWrapperFactory().ReserveName(typename))
				{
					TypeBuilder tb = mod.DefineType(typename, TypeAttributes.Sealed | TypeAttributes.Class | TypeAttributes.ExplicitLayout | TypeAttributes.NotPublic, Types.ValueType, PackingSize.Size1, length);
					AttributeHelper.HideFromJava(tb);
					type = tb.CreateType();
				}
			}
			if (type == null
				|| !type.IsValueType
				|| type.StructLayoutAttribute.Pack != 1 || type.StructLayoutAttribute.Size != length)
			{
				// the type that we found doesn't match (must mean we've compiled a Java type with that name),
				// so we fall back to the string approach
				ilgen.Emit(OpCodes.Ldstr, str);
				ilgen.Emit(OpCodes.Call, Types.String.GetMethod("ToCharArray", Type.EmptyTypes));
				return;
			}
			ilgen.EmitLdc_I4(str.Length);
			ilgen.Emit(OpCodes.Newarr, Types.Char);
			ilgen.Emit(OpCodes.Dup);
			byte[] data = new byte[length];
			for (int j = 0; j < str.Length; j++)
			{
				data[j * 2 + 0] = (byte)(str[j] >> 0);
				data[j * 2 + 1] = (byte)(str[j] >> 8);
			}
			// NOTE we define a module field, because type fields on Mono don't use the global $ArrayType$<n> type.
			// NOTE this also means that this will only work during static compilation, because ModuleBuilder.CreateGlobalFunctions() must
			// be called before the field can be used.
			FieldBuilder fb = mod.DefineInitializedData("__<str>", data, FieldAttributes.Static | FieldAttributes.PrivateScope);
			ilgen.Emit(OpCodes.Ldtoken, fb);
			ilgen.Emit(OpCodes.Call, JVM.Import(typeof(System.Runtime.CompilerServices.RuntimeHelpers)).GetMethod("InitializeArray", new Type[] { Types.Array, JVM.Import(typeof(RuntimeFieldHandle)) }));
		}

		private static bool Reflection_getCallerClass(EmitIntrinsicContext eic)
		{
			if (eic.Caller.HasCallerID)
			{
				int arg = eic.Caller.GetParametersForDefineMethod().Length - 1;
				if (!eic.Caller.IsStatic)
				{
					arg++;
				}
				eic.Emitter.EmitLdarg(arg);
				MethodWrapper mw;
				if (MatchInvokeStatic(eic, 1, "java.lang.ClassLoader", "getClassLoader", "(Ljava.lang.Class;)Ljava.lang.ClassLoader;"))
				{
					eic.PatchOpCode(1, NormalizedByteCode.__nop);
					mw = CoreClasses.ikvm.@internal.CallerID.Wrapper.GetMethodWrapper("getCallerClassLoader", "()Ljava.lang.ClassLoader;", false);
				}
				else
				{
					mw = CoreClasses.ikvm.@internal.CallerID.Wrapper.GetMethodWrapper("getCallerClass", "()Ljava.lang.Class;", false);
				}
				mw.Link();
				mw.EmitCallvirt(eic.Emitter);
				return true;
			}
			else if (DynamicTypeWrapper.RequiresDynamicReflectionCallerClass(eic.ClassFile.Name, eic.Caller.Name, eic.Caller.Signature))
			{
				// since the non-intrinsic version of Reflection.getCallerClass() always throws an exception, we have to redirect to the dynamic version
				MethodWrapper getCallerClass = ClassLoaderWrapper.LoadClassCritical("sun.reflect.Reflection").GetMethodWrapper("getCallerClass", "(I)Ljava.lang.Class;", false);
				getCallerClass.Link();
				eic.Emitter.EmitLdc_I4(2);
				getCallerClass.EmitCall(eic.Emitter);
				return true;
			}
			else
			{
				StaticCompiler.IssueMessage(Message.ReflectionCallerClassRequiresCallerID, eic.ClassFile.Name, eic.Caller.Name, eic.Caller.Signature);
			}
			return false;
		}

		private static bool CallerID_getCallerID(EmitIntrinsicContext eic)
		{
			if (eic.Caller.HasCallerID)
			{
				int arg = eic.Caller.GetParametersForDefineMethod().Length - 1;
				if (!eic.Caller.IsStatic)
				{
					arg++;
				}
				eic.Emitter.EmitLdarg(arg);
				return true;
			}
			else
			{
				throw new FatalCompilerErrorException(Message.CallerIDRequiresHasCallerIDAnnotation);
			}
		}
#endif

		private static bool Util_getInstanceTypeFromClass(EmitIntrinsicContext eic)
		{
			if (eic.MatchRange(-1, 2)
				&& eic.Match(-1, NormalizedByteCode.__ldc))
			{
				TypeWrapper tw = eic.GetClassLiteral(-1);
				if (!tw.IsUnloadable)
				{
					eic.Emitter.Emit(OpCodes.Pop);
					if (tw.IsRemapped && tw.IsFinal)
					{
						eic.Emitter.Emit(OpCodes.Ldtoken, tw.TypeAsTBD);
					}
					else
					{
						eic.Emitter.Emit(OpCodes.Ldtoken, tw.TypeAsBaseType);
					}
					eic.Emitter.Emit(OpCodes.Call, Compiler.getTypeFromHandleMethod);
					return true;
				}
			}
			return false;
		}

#if STATIC_COMPILER
		private static bool Class_getPrimitiveClass(EmitIntrinsicContext eic)
		{
			eic.Emitter.Emit(OpCodes.Pop);
			eic.Emitter.Emit(OpCodes.Ldnull);
			MethodWrapper mw = CoreClasses.java.lang.Class.Wrapper.GetMethodWrapper("<init>", "(Lcli.System.Type;)V", false);
			mw.Link();
			mw.EmitNewobj(eic.Emitter);
			return true;
		}
#endif

		private static bool ThreadLocal_new(EmitIntrinsicContext eic)
		{
			// it is only valid to replace a ThreadLocal instantiation by our ThreadStatic based version, if we can prove that the instantiation only happens once
			// (which is the case when we're in <clinit> and there aren't any branches that lead to the current position)
			if (!eic.Caller.IsClassInitializer)
			{
				return false;
			}
#if CLASSGC
			if (JVM.classUnloading)
			{
				// RunAndCollect assemblies do not support ThreadStaticAttribute
				return false;
			}
#endif
			for (int i = 0; i <= eic.OpcodeIndex; i++)
			{
				if ((eic.Flags[i] & InstructionFlags.BranchTarget) != 0)
				{
					return false;
				}
			}
			eic.Emitter.Emit(OpCodes.Newobj, eic.Context.DefineThreadLocalType());
			return true;
		}

		private static bool Unsafe_ensureClassInitialized(EmitIntrinsicContext eic)
		{
			if (eic.MatchRange(-1, 2)
				&& eic.Match(-1, NormalizedByteCode.__ldc))
			{
				TypeWrapper classLiteral = eic.GetClassLiteral(-1);
				if (!classLiteral.IsUnloadable)
				{
					eic.Emitter.Emit(OpCodes.Pop);
					eic.Emitter.EmitNullCheck();
					classLiteral.EmitRunClassConstructor(eic.Emitter);
					return true;
				}
			}
			return false;
		}

		internal static bool IsSupportedArrayTypeForUnsafeOperation(TypeWrapper tw)
		{
			return tw.IsArray
				&& !tw.IsGhostArray
				&& !tw.ElementTypeWrapper.IsPrimitive
				&& !tw.ElementTypeWrapper.IsNonPrimitiveValueType;
		}

		private static bool Unsafe_putObject(EmitIntrinsicContext eic)
		{
			return Unsafe_putObjectImpl(eic, false);
		}

		private static bool Unsafe_putOrderedObject(EmitIntrinsicContext eic)
		{
			return Unsafe_putObjectImpl(eic, false);
		}

		private static bool Unsafe_putObjectVolatile(EmitIntrinsicContext eic)
		{
			return Unsafe_putObjectImpl(eic, true);
		}

		private static bool Unsafe_putObjectImpl(EmitIntrinsicContext eic, bool membarrier)
		{
			TypeWrapper tw = eic.GetStackTypeWrapper(0, 2);
			if (IsSupportedArrayTypeForUnsafeOperation(tw)
				&& eic.GetStackTypeWrapper(0, 0).IsAssignableTo(tw.ElementTypeWrapper))
			{
				CodeEmitterLocal value = eic.Emitter.AllocTempLocal(tw.ElementTypeWrapper.TypeAsLocalOrStackType);
				CodeEmitterLocal index = eic.Emitter.AllocTempLocal(Types.Int32);
				CodeEmitterLocal array = eic.Emitter.AllocTempLocal(tw.TypeAsLocalOrStackType);
				eic.Emitter.Emit(OpCodes.Stloc, value);
				eic.Emitter.Emit(OpCodes.Conv_Ovf_I4);
				eic.Emitter.Emit(OpCodes.Stloc, index);
				eic.Emitter.Emit(OpCodes.Stloc, array);
				EmitConsumeUnsafe(eic);
				eic.Emitter.Emit(OpCodes.Ldloc, array);
				eic.Emitter.Emit(OpCodes.Ldloc, index);
				eic.Emitter.Emit(OpCodes.Ldloc, value);
				eic.Emitter.ReleaseTempLocal(array);
				eic.Emitter.ReleaseTempLocal(index);
				eic.Emitter.ReleaseTempLocal(value);
				eic.Emitter.Emit(OpCodes.Stelem_Ref);
				if (membarrier)
				{
					eic.Emitter.EmitMemoryBarrier();
				}
				eic.NonLeaf = false;
				return true;
			}
			if ((eic.Flags[eic.OpcodeIndex] & InstructionFlags.BranchTarget) != 0
				|| (eic.Flags[eic.OpcodeIndex - 1] & InstructionFlags.BranchTarget) != 0)
			{
				return false;
			}
			if ((eic.Match(-1, NormalizedByteCode.__aload) || eic.Match(-1, NormalizedByteCode.__aconst_null))
				&& eic.Match(-2, NormalizedByteCode.__getstatic))
			{
				FieldWrapper fw = GetUnsafeField(eic, eic.GetFieldref(-2));
				if (fw != null
					&& (!fw.IsFinal || (!fw.IsStatic && eic.Caller.Name == "<init>") || (fw.IsStatic && eic.Caller.Name == "<clinit>"))
					&& fw.IsAccessibleFrom(fw.DeclaringType, eic.Caller.DeclaringType, fw.DeclaringType)
					&& eic.GetStackTypeWrapper(0, 0).IsAssignableTo(fw.FieldTypeWrapper)
					&& (fw.IsStatic || fw.DeclaringType == eic.GetStackTypeWrapper(0, 2)))
				{
					CodeEmitterLocal value = eic.Emitter.AllocTempLocal(fw.FieldTypeWrapper.TypeAsLocalOrStackType);
					eic.Emitter.Emit(OpCodes.Stloc, value);
					eic.Emitter.Emit(OpCodes.Pop);		// discard offset field
					if (fw.IsStatic)
					{
						eic.Emitter.Emit(OpCodes.Pop);	// discard object
						EmitConsumeUnsafe(eic);
					}
					else
					{
						CodeEmitterLocal obj = eic.Emitter.AllocTempLocal(fw.DeclaringType.TypeAsLocalOrStackType);
						eic.Emitter.Emit(OpCodes.Stloc, obj);
						EmitConsumeUnsafe(eic);
						eic.Emitter.Emit(OpCodes.Ldloc, obj);
						eic.Emitter.ReleaseTempLocal(obj);
					}
					eic.Emitter.Emit(OpCodes.Ldloc, value);
					eic.Emitter.ReleaseTempLocal(value);
					// note that we assume the CLR memory model where all writes are ordered,
					// so we don't need a volatile store or a memory barrier and putOrderedObject
					// is typically used with a volatile field, so to avoid the memory barrier,
					// we don't use FieldWrapper.EmitSet(), but emit the store directly
					eic.Emitter.Emit(fw.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fw.GetField());
					if (membarrier)
					{
						eic.Emitter.EmitMemoryBarrier();
					}
					eic.NonLeaf = false;
					return true;
				}
			}
			return false;
		}

		private static bool Unsafe_getObjectVolatile(EmitIntrinsicContext eic)
		{
			// the check here must be kept in sync with the hack in MethodAnalyzer.AnalyzeTypeFlow()
			TypeWrapper tw = eic.GetStackTypeWrapper(0, 1);
			if (IsSupportedArrayTypeForUnsafeOperation(tw))
			{
				CodeEmitterLocal index = eic.Emitter.AllocTempLocal(Types.Int32);
				CodeEmitterLocal obj = eic.Emitter.AllocTempLocal(tw.TypeAsLocalOrStackType);
				eic.Emitter.Emit(OpCodes.Conv_Ovf_I4);
				eic.Emitter.Emit(OpCodes.Stloc, index);
				eic.Emitter.Emit(OpCodes.Stloc, obj);
				EmitConsumeUnsafe(eic);
				eic.Emitter.Emit(OpCodes.Ldloc, obj);
				eic.Emitter.Emit(OpCodes.Ldloc, index);
				eic.Emitter.ReleaseTempLocal(obj);
				eic.Emitter.ReleaseTempLocal(index);
				eic.Emitter.Emit(OpCodes.Ldelema, tw.TypeAsLocalOrStackType.GetElementType());
				eic.Emitter.Emit(OpCodes.Volatile);
				eic.Emitter.Emit(OpCodes.Ldind_Ref);
				// remove the redundant checkcast that usually follows
				if (eic.Code[eic.OpcodeIndex + 1].NormalizedOpCode == NormalizedByteCode.__checkcast
					&& tw.ElementTypeWrapper.IsAssignableTo(eic.ClassFile.GetConstantPoolClassType(eic.Code[eic.OpcodeIndex + 1].Arg1)))
				{
					eic.PatchOpCode(1, NormalizedByteCode.__nop);
				}
				eic.NonLeaf = false;
				return true;
			}
			return false;
		}

		private static bool Unsafe_compareAndSwapObject(EmitIntrinsicContext eic)
		{
			TypeWrapper tw = eic.GetStackTypeWrapper(0, 3);
			if (IsSupportedArrayTypeForUnsafeOperation(tw)
				&& eic.GetStackTypeWrapper(0, 0).IsAssignableTo(tw.ElementTypeWrapper)
				&& eic.GetStackTypeWrapper(0, 1).IsAssignableTo(tw.ElementTypeWrapper))
			{
				Type type = tw.TypeAsLocalOrStackType.GetElementType();
				CodeEmitterLocal update = eic.Emitter.AllocTempLocal(type);
				CodeEmitterLocal expect = eic.Emitter.AllocTempLocal(type);
				CodeEmitterLocal index = eic.Emitter.AllocTempLocal(Types.Int32);
				CodeEmitterLocal obj = eic.Emitter.AllocTempLocal(tw.TypeAsLocalOrStackType);
				eic.Emitter.Emit(OpCodes.Stloc, update);
				eic.Emitter.Emit(OpCodes.Stloc, expect);
				eic.Emitter.Emit(OpCodes.Conv_Ovf_I4);
				eic.Emitter.Emit(OpCodes.Stloc, index);
				eic.Emitter.Emit(OpCodes.Stloc, obj);
				EmitConsumeUnsafe(eic);
				eic.Emitter.Emit(OpCodes.Ldloc, obj);
				eic.Emitter.Emit(OpCodes.Ldloc, index);
				eic.Emitter.Emit(OpCodes.Ldelema, type);
				eic.Emitter.Emit(OpCodes.Ldloc, update);
				eic.Emitter.Emit(OpCodes.Ldloc, expect);
				eic.Emitter.Emit(OpCodes.Call, AtomicReferenceFieldUpdaterEmitter.MakeCompareExchange(type));
				eic.Emitter.Emit(OpCodes.Ldloc, expect);
				eic.Emitter.Emit(OpCodes.Ceq);
				eic.Emitter.ReleaseTempLocal(obj);
				eic.Emitter.ReleaseTempLocal(index);
				eic.Emitter.ReleaseTempLocal(expect);
				eic.Emitter.ReleaseTempLocal(update);
				eic.NonLeaf = false;
				return true;
			}
			if ((eic.Flags[eic.OpcodeIndex] & InstructionFlags.BranchTarget) != 0
				|| (eic.Flags[eic.OpcodeIndex - 1] & InstructionFlags.BranchTarget) != 0
				|| (eic.Flags[eic.OpcodeIndex - 2] & InstructionFlags.BranchTarget) != 0)
			{
				return false;
			}
			if ((eic.Match(-1, NormalizedByteCode.__aload) || eic.Match(-1, NormalizedByteCode.__aconst_null))
				&& (eic.Match(-2, NormalizedByteCode.__aload) || eic.Match(-2, NormalizedByteCode.__aconst_null))
				&& eic.Match(-3, NormalizedByteCode.__getstatic))
			{
				FieldWrapper fw = GetUnsafeField(eic, eic.GetFieldref(-3));
				if (fw != null
					&& fw.IsAccessibleFrom(fw.DeclaringType, eic.Caller.DeclaringType, fw.DeclaringType)
					&& eic.GetStackTypeWrapper(0, 0).IsAssignableTo(fw.FieldTypeWrapper)
					&& eic.GetStackTypeWrapper(0, 1).IsAssignableTo(fw.FieldTypeWrapper)
					&& (fw.IsStatic || fw.DeclaringType == eic.GetStackTypeWrapper(0, 3)))
				{
					Type type = fw.FieldTypeWrapper.TypeAsLocalOrStackType;
					CodeEmitterLocal update = eic.Emitter.AllocTempLocal(type);
					CodeEmitterLocal expect = eic.Emitter.AllocTempLocal(type);
					eic.Emitter.Emit(OpCodes.Stloc, update);
					eic.Emitter.Emit(OpCodes.Stloc, expect);
					eic.Emitter.Emit(OpCodes.Pop);			// discard index
					if (fw.IsStatic)
					{
						eic.Emitter.Emit(OpCodes.Pop);		// discard obj
						EmitConsumeUnsafe(eic);
						eic.Emitter.Emit(OpCodes.Ldsflda, fw.GetField());
					}
					else
					{
						CodeEmitterLocal obj = eic.Emitter.AllocTempLocal(eic.Caller.DeclaringType.TypeAsLocalOrStackType);
						eic.Emitter.Emit(OpCodes.Stloc, obj);
						EmitConsumeUnsafe(eic);
						eic.Emitter.Emit(OpCodes.Ldloc, obj);
						eic.Emitter.ReleaseTempLocal(obj);
						eic.Emitter.Emit(OpCodes.Ldflda, fw.GetField());
					}
					eic.Emitter.Emit(OpCodes.Ldloc, update);
					eic.Emitter.Emit(OpCodes.Ldloc, expect);
					eic.Emitter.Emit(OpCodes.Call, AtomicReferenceFieldUpdaterEmitter.MakeCompareExchange(type));
					eic.Emitter.Emit(OpCodes.Ldloc, expect);
					eic.Emitter.Emit(OpCodes.Ceq);
					eic.Emitter.ReleaseTempLocal(expect);
					eic.Emitter.ReleaseTempLocal(update);
					eic.NonLeaf = false;
					return true;
				}
			}
			// stack layout at call site:
			// 4 Unsafe (receiver)
			// 3 Object (obj)
			// 2 long (offset)
			// 1 Object (expect)
			// 0 Object (update)
			TypeWrapper twUnsafe = eic.GetStackTypeWrapper(0, 4);
			if (twUnsafe == VerifierTypeWrapper.Null)
			{
				return false;
			}
			for (int i = 0; ; i--)
			{
				if ((eic.Flags[eic.OpcodeIndex + i] & InstructionFlags.BranchTarget) != 0)
				{
					return false;
				}
				if (eic.GetStackTypeWrapper(i, 0) == twUnsafe)
				{
					// the pattern we recognize is:
					// aload
					// getstatic <offset field>
					if (eic.Match(i, NormalizedByteCode.__aload) && eic.GetStackTypeWrapper(i + 1, 0) == eic.Caller.DeclaringType
						&& eic.Match(i + 1, NormalizedByteCode.__getstatic))
					{
						FieldWrapper fw = GetUnsafeField(eic, eic.GetFieldref(i + 1));
						if (fw != null && !fw.IsStatic && fw.DeclaringType == eic.Caller.DeclaringType)
						{
							Type type = fw.FieldTypeWrapper.TypeAsLocalOrStackType;
							CodeEmitterLocal update = eic.Emitter.AllocTempLocal(type);
							CodeEmitterLocal expect = eic.Emitter.AllocTempLocal(type);
							CodeEmitterLocal obj = eic.Emitter.AllocTempLocal(eic.Caller.DeclaringType.TypeAsLocalOrStackType);
							eic.Emitter.Emit(OpCodes.Stloc, update);
							eic.Emitter.Emit(OpCodes.Stloc, expect);
							eic.Emitter.Emit(OpCodes.Pop);			// discard offset
							eic.Emitter.Emit(OpCodes.Stloc, obj);
							EmitConsumeUnsafe(eic);
							eic.Emitter.Emit(OpCodes.Ldloc, obj);
							eic.Emitter.Emit(OpCodes.Ldflda, fw.GetField());
							eic.Emitter.Emit(OpCodes.Ldloc, update);
							eic.Emitter.Emit(OpCodes.Ldloc, expect);
							eic.Emitter.Emit(OpCodes.Call, AtomicReferenceFieldUpdaterEmitter.MakeCompareExchange(type));
							eic.Emitter.Emit(OpCodes.Ldloc, expect);
							eic.Emitter.Emit(OpCodes.Ceq);
							eic.Emitter.ReleaseTempLocal(expect);
							eic.Emitter.ReleaseTempLocal(update);
							eic.NonLeaf = false;
							return true;
						}
					}
					return false;
				}
			}
		}

		private static bool Unsafe_getAndSetObject(EmitIntrinsicContext eic)
		{
			TypeWrapper tw = eic.GetStackTypeWrapper(0, 2);
			if (IsSupportedArrayTypeForUnsafeOperation(tw)
				&& eic.GetStackTypeWrapper(0, 0).IsAssignableTo(tw.ElementTypeWrapper))
			{
				Type type = tw.TypeAsLocalOrStackType.GetElementType();
				CodeEmitterLocal newValue = eic.Emitter.AllocTempLocal(type);
				CodeEmitterLocal index = eic.Emitter.AllocTempLocal(Types.Int32);
				CodeEmitterLocal obj = eic.Emitter.AllocTempLocal(tw.TypeAsLocalOrStackType);
				eic.Emitter.Emit(OpCodes.Stloc, newValue);
				eic.Emitter.Emit(OpCodes.Conv_Ovf_I4);
				eic.Emitter.Emit(OpCodes.Stloc, index);
				eic.Emitter.Emit(OpCodes.Stloc, obj);
				EmitConsumeUnsafe(eic);
				eic.Emitter.Emit(OpCodes.Ldloc, obj);
				eic.Emitter.Emit(OpCodes.Ldloc, index);
				eic.Emitter.Emit(OpCodes.Ldelema, type);
				eic.Emitter.Emit(OpCodes.Ldloc, newValue);
				eic.Emitter.Emit(OpCodes.Call, MakeExchange(type));
				eic.Emitter.ReleaseTempLocal(obj);
				eic.Emitter.ReleaseTempLocal(index);
				eic.Emitter.ReleaseTempLocal(newValue);
				eic.NonLeaf = false;
				return true;
			}
			return false;
		}

		private static bool Unsafe_compareAndSwapInt(EmitIntrinsicContext eic)
		{
			// stack layout at call site:
			// 4 Unsafe (receiver)
			// 3 Object (obj)
			// 2 long (offset)
			// 1 int (expect)
			// 0 int (update)
			TypeWrapper twUnsafe = eic.GetStackTypeWrapper(0, 4);
			if (twUnsafe == VerifierTypeWrapper.Null)
			{
				return false;
			}
			for (int i = 0; ; i--)
			{
				if ((eic.Flags[eic.OpcodeIndex + i] & InstructionFlags.BranchTarget) != 0)
				{
					return false;
				}
				if (eic.GetStackTypeWrapper(i, 0) == twUnsafe)
				{
					// the pattern we recognize is:
					// aload
					// getstatic <offset field>
					if (eic.Match(i, NormalizedByteCode.__aload) && eic.GetStackTypeWrapper(i + 1, 0) == eic.Caller.DeclaringType
						&& eic.Match(i + 1, NormalizedByteCode.__getstatic))
					{
						FieldWrapper fw = GetUnsafeField(eic, eic.GetFieldref(i + 1));
						if (fw != null && !fw.IsStatic && fw.DeclaringType == eic.Caller.DeclaringType)
						{
							CodeEmitterLocal update = eic.Emitter.AllocTempLocal(Types.Int32);
							CodeEmitterLocal expect = eic.Emitter.AllocTempLocal(Types.Int32);
							CodeEmitterLocal obj = eic.Emitter.AllocTempLocal(eic.Caller.DeclaringType.TypeAsLocalOrStackType);
							eic.Emitter.Emit(OpCodes.Stloc, update);
							eic.Emitter.Emit(OpCodes.Stloc, expect);
							eic.Emitter.Emit(OpCodes.Pop);			// discard offset
							eic.Emitter.Emit(OpCodes.Stloc, obj);
							EmitConsumeUnsafe(eic);
							eic.Emitter.Emit(OpCodes.Ldloc, obj);
							eic.Emitter.Emit(OpCodes.Ldflda, fw.GetField());
							eic.Emitter.Emit(OpCodes.Ldloc, update);
							eic.Emitter.Emit(OpCodes.Ldloc, expect);
							eic.Emitter.Emit(OpCodes.Call, InterlockedMethods.CompareExchangeInt32);
							eic.Emitter.Emit(OpCodes.Ldloc, expect);
							eic.Emitter.Emit(OpCodes.Ceq);
							eic.Emitter.ReleaseTempLocal(expect);
							eic.Emitter.ReleaseTempLocal(update);
							eic.NonLeaf = false;
							return true;
						}
					}
					return false;
				}
			}
		}

		private static bool Unsafe_getAndAddInt(EmitIntrinsicContext eic)
		{
			// stack layout at call site:
			// 3 Unsafe (receiver)
			// 2 Object (obj)
			// 1 long (offset)
			// 0 int (delta)
			TypeWrapper twUnsafe = eic.GetStackTypeWrapper(0, 3);
			if (twUnsafe == VerifierTypeWrapper.Null)
			{
				return false;
			}
			for (int i = 0; ; i--)
			{
				if ((eic.Flags[eic.OpcodeIndex + i] & InstructionFlags.BranchTarget) != 0)
				{
					return false;
				}
				if (eic.GetStackTypeWrapper(i, 0) == twUnsafe)
				{
					// the pattern we recognize is:
					// aload_0 
					// getstatic <offset field>
					if (eic.Match(i, NormalizedByteCode.__aload, 0)
						&& eic.Match(i + 1, NormalizedByteCode.__getstatic))
					{
						FieldWrapper fw = GetUnsafeField(eic, eic.GetFieldref(i + 1));
						if (fw != null && !fw.IsStatic && fw.DeclaringType == eic.Caller.DeclaringType)
						{
							CodeEmitterLocal delta = eic.Emitter.AllocTempLocal(Types.Int32);
							eic.Emitter.Emit(OpCodes.Stloc, delta);
							eic.Emitter.Emit(OpCodes.Pop);			// discard offset
							eic.Emitter.Emit(OpCodes.Pop);			// discard obj
							EmitConsumeUnsafe(eic);
							eic.Emitter.Emit(OpCodes.Ldarg_0);
							eic.Emitter.Emit(OpCodes.Ldflda, fw.GetField());
							eic.Emitter.Emit(OpCodes.Ldloc, delta);
							eic.Emitter.Emit(OpCodes.Call, InterlockedMethods.AddInt32);
							eic.Emitter.Emit(OpCodes.Ldloc, delta);
							eic.Emitter.Emit(OpCodes.Sub);
							eic.Emitter.ReleaseTempLocal(delta);
							eic.NonLeaf = false;
							return true;
						}
					}
					return false;
				}
			}
		}

		private static bool Unsafe_compareAndSwapLong(EmitIntrinsicContext eic)
		{
			// stack layout at call site:
			// 4 Unsafe (receiver)
			// 3 Object (obj)
			// 2 long (offset)
			// 1 long (expect)
			// 0 long (update)
			TypeWrapper twUnsafe = eic.GetStackTypeWrapper(0, 4);
			if (twUnsafe == VerifierTypeWrapper.Null)
			{
				return false;
			}
			for (int i = 0; ; i--)
			{
				if ((eic.Flags[eic.OpcodeIndex + i] & InstructionFlags.BranchTarget) != 0)
				{
					return false;
				}
				if (eic.GetStackTypeWrapper(i, 0) == twUnsafe)
				{
					// the pattern we recognize is:
					// aload
					// getstatic <offset field>
					if (eic.Match(i, NormalizedByteCode.__aload) && eic.GetStackTypeWrapper(i + 1, 0) == eic.Caller.DeclaringType
						&& eic.Match(i + 1, NormalizedByteCode.__getstatic))
					{
						FieldWrapper fw = GetUnsafeField(eic, eic.GetFieldref(i + 1));
						if (fw != null && !fw.IsStatic && fw.DeclaringType == eic.Caller.DeclaringType)
						{
							CodeEmitterLocal update = eic.Emitter.AllocTempLocal(Types.Int64);
							CodeEmitterLocal expect = eic.Emitter.AllocTempLocal(Types.Int64);
							CodeEmitterLocal obj = eic.Emitter.AllocTempLocal(eic.Caller.DeclaringType.TypeAsLocalOrStackType);
							eic.Emitter.Emit(OpCodes.Stloc, update);
							eic.Emitter.Emit(OpCodes.Stloc, expect);
							eic.Emitter.Emit(OpCodes.Pop);			// discard offset
							eic.Emitter.Emit(OpCodes.Stloc, obj);
							EmitConsumeUnsafe(eic);
							eic.Emitter.Emit(OpCodes.Ldloc, obj);
							eic.Emitter.Emit(OpCodes.Ldflda, fw.GetField());
							eic.Emitter.Emit(OpCodes.Ldloc, update);
							eic.Emitter.Emit(OpCodes.Ldloc, expect);
							eic.Emitter.Emit(OpCodes.Call, InterlockedMethods.CompareExchangeInt64);
							eic.Emitter.Emit(OpCodes.Ldloc, expect);
							eic.Emitter.Emit(OpCodes.Ceq);
							eic.Emitter.ReleaseTempLocal(expect);
							eic.Emitter.ReleaseTempLocal(update);
							eic.NonLeaf = false;
							return true;
						}
					}
					return false;
				}
			}
		}

		internal static MethodInfo MakeExchange(Type type)
		{
			return InterlockedMethods.ExchangeOfT.MakeGenericMethod(type);
		}

		private static void EmitConsumeUnsafe(EmitIntrinsicContext eic)
		{
#if STATIC_COMPILER
			if (eic.Caller.DeclaringType.GetClassLoader() == CoreClasses.java.lang.Object.Wrapper.GetClassLoader())
			{
				// we're compiling the core library (which is obviously trusted), so we don't need to check
				// if we really have an Unsafe instance
				eic.Emitter.Emit(OpCodes.Pop);
			}
			else
#endif
			{
				eic.Emitter.EmitNullCheck();
			}
		}

		private static FieldWrapper GetUnsafeField(EmitIntrinsicContext eic, ClassFile.ConstantPoolItemFieldref field)
		{
			if (eic.Caller.DeclaringType.GetClassLoader() != CoreClasses.java.lang.Object.Wrapper.GetClassLoader())
			{
				// this code does not solve the general problem and assumes non-hostile, well behaved static initializers
				// so we only support the core class library
				return null;
			}

			// the field offset field must be a static field inside the current class
			// (we don't need to check that the field is static, because the caller already ensured that)
			if (field.GetField().DeclaringType == eic.Caller.DeclaringType)
			{
				// now look inside the static initializer to see if we can found out what field it refers to
				foreach (ClassFile.Method method in eic.ClassFile.Methods)
				{
					if (method.IsClassInitializer)
					{
						// TODO should we first verify the method?
						// TODO should we attempt to make sure the field is definitely assigned (and only once)?

						// TODO special case/support the pattern used by:
						//  - java.util.concurrent.atomic.AtomicMarkableReference
						//  - java.util.concurrent.atomic.AtomicStampedReference
						//  - java.util.concurrent.locks.AbstractQueuedLongSynchronizer

						/*
						 *  ldc_w test
						 *  astore_0
						 *  ...
						 *  getstatic <Field test sun/misc/Unsafe UNSAFE>
						 *  aload_0 | ldc <Class>
						 *  ldc "next"
						 *  invokevirtual <Method java/lang/Class getDeclaredField(Ljava/lang/String;)Ljava/lang/reflect/Field;>
						 *  invokevirtual <Method sun/misc/Unsafe objectFieldOffset(Ljava/lang/reflect/Field;)J>
						 *  putstatic <Field test long nextOffset>
						 */
						for (int i = 0; i < method.Instructions.Length; i++)
						{
							if (method.Instructions[i].NormalizedOpCode == NormalizedByteCode.__putstatic
								&& eic.ClassFile.GetFieldref(method.Instructions[i].Arg1) == field)
							{
								if (MatchInvokeVirtual(eic, ref method.Instructions[i - 1], "sun.misc.Unsafe", "objectFieldOffset", "(Ljava.lang.reflect.Field;)J")
									&& MatchInvokeVirtual(eic, ref method.Instructions[i - 2], "java.lang.Class", "getDeclaredField", "(Ljava.lang.String;)Ljava.lang.reflect.Field;")
									&& MatchLdc(eic, ref method.Instructions[i - 3], ClassFile.ConstantType.String)
									&& (method.Instructions[i - 4].NormalizedOpCode == NormalizedByteCode.__aload || method.Instructions[i - 4].NormalizedOpCode == NormalizedByteCode.__ldc)
									&& method.Instructions[i - 5].NormalizedOpCode == NormalizedByteCode.__getstatic && eic.ClassFile.GetFieldref(method.Instructions[i - 5].Arg1).Signature == "Lsun.misc.Unsafe;")
								{
									if (method.Instructions[i - 4].NormalizedOpCode == NormalizedByteCode.__ldc)
									{
										if (eic.ClassFile.GetConstantPoolClassType(method.Instructions[i - 4].Arg1) == eic.Caller.DeclaringType)
										{
											string fieldName = eic.ClassFile.GetConstantPoolConstantString(method.Instructions[i - 3].Arg1);
											FieldWrapper fw = null;
											foreach (FieldWrapper fw1 in eic.Caller.DeclaringType.GetFields())
											{
												if (fw1.Name == fieldName)
												{
													if (fw == null)
													{
														fw = fw1;
													}
													else
													{
														// duplicate name
														return null;
													}
												}
											}
											return fw;
										}
										return null;
									}
									// search backward for the astore that corresponds to the aload (of the class object)
									for (int j = i - 6; j > 0; j--)
									{
										if (method.Instructions[j].NormalizedOpCode == NormalizedByteCode.__astore
											&& method.Instructions[j].Arg1 == method.Instructions[i - 4].Arg1
											&& MatchLdc(eic, ref method.Instructions[j - 1], ClassFile.ConstantType.Class)
											&& eic.ClassFile.GetConstantPoolClassType(method.Instructions[j - 1].Arg1) == eic.Caller.DeclaringType)
										{
											string fieldName = eic.ClassFile.GetConstantPoolConstantString(method.Instructions[i - 3].Arg1);
											FieldWrapper fw = null;
											foreach (FieldWrapper fw1 in eic.Caller.DeclaringType.GetFields())
											{
												if (fw1.Name == fieldName)
												{
													if (fw == null)
													{
														fw = fw1;
													}
													else
													{
														// duplicate name
														return null;
													}
												}
											}
											return fw;
										}
									}
									break;
								}
							}
						}
						break;
					}
				}
			}
			return null;
		}

		private static bool MatchInvokeVirtual(EmitIntrinsicContext eic, ref Instruction instr, string clazz, string name, string sig)
		{
			return MatchInvoke(eic, ref instr, NormalizedByteCode.__invokevirtual, clazz, name, sig);
		}

		private static bool MatchInvokeStatic(EmitIntrinsicContext eic, int offset, string clazz, string name, string sig)
		{
			return MatchInvoke(eic, ref eic.Code[eic.OpcodeIndex + offset], NormalizedByteCode.__invokestatic, clazz, name, sig);
		}

		private static bool MatchInvoke(EmitIntrinsicContext eic, ref Instruction instr, NormalizedByteCode opcode, string clazz, string name, string sig)
		{
			if (instr.NormalizedOpCode == opcode)
			{
				ClassFile.ConstantPoolItemMI method = eic.ClassFile.GetMethodref(instr.Arg1);
				return method.Class == clazz
					&& method.Name == name
					&& method.Signature == sig;
			}
			return false;
		}

		private static bool MatchLdc(EmitIntrinsicContext eic, ref Instruction instr, ClassFile.ConstantType constantType)
		{
			return (instr.NormalizedOpCode == NormalizedByteCode.__ldc || instr.NormalizedOpCode == NormalizedByteCode.__ldc_nothrow)
				&& eic.ClassFile.GetConstantPoolConstantType(instr.NormalizedArg1) == constantType;
		}
	}
}
