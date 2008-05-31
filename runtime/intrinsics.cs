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
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;

namespace IKVM.Internal
{
	static class Intrinsics
	{
		private delegate bool Emitter(CountingILGenerator ilgen, MethodWrapper method, MethodAnalyzer ma, int opcodeIndex, MethodWrapper caller, ClassFile classFile, ClassFile.Method.Instruction[] code);
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
		private static readonly Type typeofFloatConverter = StaticCompiler.GetType("IKVM.Runtime.FloatConverter");
		private static readonly Type typeofDoubleConverter = StaticCompiler.GetType("IKVM.Runtime.DoubleConverter");
#else
		private static readonly Type typeofFloatConverter = typeof(IKVM.Runtime.FloatConverter);
		private static readonly Type typeofDoubleConverter = typeof(IKVM.Runtime.DoubleConverter);
#endif

		private static Dictionary<IntrinsicKey, Emitter> Register()
		{
			Dictionary<IntrinsicKey, Emitter> intrinsics = new Dictionary<IntrinsicKey, Emitter>();
			intrinsics.Add(new IntrinsicKey("java.lang.Float", "floatToRawIntBits", "(F)I"), Float_floatToRawIntBits);
			intrinsics.Add(new IntrinsicKey("java.lang.Float", "intBitsToFloat", "(I)F"), Float_intBitsToFloat);
			intrinsics.Add(new IntrinsicKey("java.lang.Double", "doubleToRawLongBits", "(D)J"), Double_doubleToRawLongBits);
			intrinsics.Add(new IntrinsicKey("java.lang.Double", "longBitsToDouble", "(J)D"), Double_longBitsToDouble);
			intrinsics.Add(new IntrinsicKey("java.lang.System", "arraycopy", "(Ljava.lang.Object;ILjava.lang.Object;II)V"), System_arraycopy);
#if !OPENDJK
			intrinsics.Add(new IntrinsicKey("java.lang.VMSystem", "arraycopy", "(Ljava.lang.Object;ILjava.lang.Object;II)V"), System_arraycopy);
#endif
			intrinsics.Add(new IntrinsicKey("java.util.concurrent.atomic.AtomicReferenceFieldUpdater", "newUpdater", "(Ljava.lang.Class;Ljava.lang.Class;Ljava.lang.String;)Ljava.util.concurrent.atomic.AtomicReferenceFieldUpdater;"), AtomicReferenceFieldUpdater_newUpdater);
			intrinsics.Add(new IntrinsicKey("java.lang.String", "toCharArray", "()[C"), String_toCharArray);
			return intrinsics;
		}

		internal static bool IsIntrinsic(MethodWrapper mw)
		{
			return intrinsics.ContainsKey(new IntrinsicKey(mw)) && mw.DeclaringType.GetClassLoader() == CoreClasses.java.lang.Object.Wrapper.GetClassLoader();
		}

		internal static bool Emit(CountingILGenerator ilgen, MethodWrapper method, MethodAnalyzer ma, int opcodeIndex, MethodWrapper caller, ClassFile classFile, ClassFile.Method.Instruction[] code)
		{
			// note that intrinsics can always refuse to emit code and the code generator will fall back to a normal method call
			return intrinsics[new IntrinsicKey(method)](ilgen, method, ma, opcodeIndex, caller, classFile, code);
		}

		private static bool Float_floatToRawIntBits(CountingILGenerator ilgen, MethodWrapper method, MethodAnalyzer ma, int opcodeIndex, MethodWrapper caller, ClassFile classFile, ClassFile.Method.Instruction[] code)
		{
			EmitConversion(ilgen, typeofFloatConverter, "ToInt");
			return true;
		}

		private static bool Float_intBitsToFloat(CountingILGenerator ilgen, MethodWrapper method, MethodAnalyzer ma, int opcodeIndex, MethodWrapper caller, ClassFile classFile, ClassFile.Method.Instruction[] code)
		{
			EmitConversion(ilgen, typeofFloatConverter, "ToFloat");
			return true;
		}

		private static bool Double_doubleToRawLongBits(CountingILGenerator ilgen, MethodWrapper method, MethodAnalyzer ma, int opcodeIndex, MethodWrapper caller, ClassFile classFile, ClassFile.Method.Instruction[] code)
		{
			EmitConversion(ilgen, typeofDoubleConverter, "ToLong");
			return true;
		}

		private static bool Double_longBitsToDouble(CountingILGenerator ilgen, MethodWrapper method, MethodAnalyzer ma, int opcodeIndex, MethodWrapper caller, ClassFile classFile, ClassFile.Method.Instruction[] code)
		{
			EmitConversion(ilgen, typeofDoubleConverter, "ToDouble");
			return true;
		}

		private static void EmitConversion(CountingILGenerator ilgen, Type converterType, string method)
		{
			LocalBuilder converter = ilgen.UnsafeAllocTempLocal(converterType);
			ilgen.Emit(OpCodes.Ldloca, converter);
			ilgen.Emit(OpCodes.Call, converterType.GetMethod(method));
		}

		private static bool System_arraycopy(CountingILGenerator ilgen, MethodWrapper method, MethodAnalyzer ma, int opcodeIndex, MethodWrapper caller, ClassFile classFile, ClassFile.Method.Instruction[] code)
		{
			// if the array arguments on the stack are of a known array type, we can redirect to an optimized version of arraycopy.
			// Note that we also have to handle VMSystem.arraycopy, because on GNU Classpath StringBuffer directly calls
			// this method to avoid prematurely initialising System.
			TypeWrapper dst_type = ma.GetStackTypeWrapper(opcodeIndex, 2);
			TypeWrapper src_type = ma.GetStackTypeWrapper(opcodeIndex, 4);
			if (!dst_type.IsUnloadable && dst_type.IsArray && dst_type == src_type)
			{
				switch (dst_type.Name[1])
				{
					case 'J':
					case 'D':
						ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.arraycopy_primitive_8);
						break;
					case 'I':
					case 'F':
						ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.arraycopy_primitive_4);
						break;
					case 'S':
					case 'C':
						ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.arraycopy_primitive_2);
						break;
					case 'B':
					case 'Z':
						ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.arraycopy_primitive_1);
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
							ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.arraycopy_fast);
						}
						else
						{
							ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.arraycopy);
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

		private static bool AtomicReferenceFieldUpdater_newUpdater(CountingILGenerator ilgen, MethodWrapper method, MethodAnalyzer ma, int opcodeIndex, MethodWrapper caller, ClassFile classFile, ClassFile.Method.Instruction[] code)
		{
			return AtomicReferenceFieldUpdaterEmitter.Emit(caller.DeclaringType, ilgen, classFile, opcodeIndex, code);
		}

		private static bool String_toCharArray(CountingILGenerator ilgen, MethodWrapper method, MethodAnalyzer ma, int opcodeIndex, MethodWrapper caller, ClassFile classFile, ClassFile.Method.Instruction[] code)
		{
			string str = ilgen.PopLazyLdstr();
			if (str != null)
			{
				// arbitrary length for "big" strings
				if (str.Length > 128)
				{
					EmitLoadCharArrayLiteral(ilgen, str, caller.DeclaringType);
					return true;
				}
				ilgen.Emit(OpCodes.Ldstr, str);
			}
			return false;
		}

		private static void EmitLoadCharArrayLiteral(CountingILGenerator ilgen, string str, TypeWrapper tw)
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
					TypeBuilder tb = mod.DefineType(typename, TypeAttributes.Sealed | TypeAttributes.Class | TypeAttributes.ExplicitLayout | TypeAttributes.NotPublic, typeof(ValueType), PackingSize.Size1, length);
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
				ilgen.Emit(OpCodes.Call, typeof(string).GetMethod("ToCharArray", Type.EmptyTypes));
				return;
			}
			ilgen.Emit(OpCodes.Ldc_I4, str.Length);
			ilgen.Emit(OpCodes.Newarr, typeof(char));
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
			if (!fb.FieldType.Equals(type))
			{
				// this is actually relatively harmless, but I would like to know about it, so we abort and hope that users report this when they encounter it
				JVM.CriticalFailure("Unsupported runtime: ModuleBuilder.DefineInitializedData() field type mispredicted", null);
			}
			ilgen.Emit(OpCodes.Ldtoken, fb);
			ilgen.Emit(OpCodes.Call, typeof(System.Runtime.CompilerServices.RuntimeHelpers).GetMethod("InitializeArray", new Type[] { typeof(Array), typeof(RuntimeFieldHandle) }));
		}
	}
}
