/*
  Copyright (C) 2002 Jeroen Frijters

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
using System.Reflection.Emit;
using System.Diagnostics;
using OpenSystem.Java;

public abstract class CodeEmitter
{
	public static readonly CodeEmitter Nop = new OpCodeEmitter(OpCodes.Nop);
	public static readonly CodeEmitter Pop = new OpCodeEmitter(OpCodes.Pop);
	public static readonly CodeEmitter Volatile = new OpCodeEmitter(OpCodes.Volatile);
	public static readonly CodeEmitter InternalError = new InternalErrorEmitter();

	internal abstract void Emit(ILGenerator ilgen);

	private class InternalErrorEmitter : CodeEmitter
	{
		internal InternalErrorEmitter()
		{
		}

		internal override void Emit(ILGenerator ilgen)
		{
			throw new InvalidOperationException();
		}
	}

	private class ChainCodeEmitter : CodeEmitter
	{
		private CodeEmitter left;
		private CodeEmitter right;

		internal ChainCodeEmitter(CodeEmitter left, CodeEmitter right)
		{
			this.left = left;
			this.right = right;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			left.Emit(ilgen);
			right.Emit(ilgen);
		}
	}

	public static CodeEmitter operator+(CodeEmitter left, CodeEmitter right)
	{
		if(left == null)
		{
			return right;
		}
		return new ChainCodeEmitter(left, right);
	}

	private class MethodInfoCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private MethodInfo mi;

		internal MethodInfoCodeEmitter(OpCode opcode, MethodInfo mi)
		{
			this.opcode = opcode;
			this.mi = mi;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, mi);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, MethodInfo mi)
	{
		Debug.Assert(mi != null);
		return new MethodInfoCodeEmitter(opcode, mi);
	}

	internal static CodeEmitter Create(OpCode opcode, MethodBase mb)
	{
		Debug.Assert(mb != null);
		if(mb is MethodInfo)
		{
			return new MethodInfoCodeEmitter(opcode, (MethodInfo)mb);
		}
		else
		{
			return new ConstructorInfoCodeEmitter(opcode, (ConstructorInfo)mb);
		}
	}

	private class ConstructorInfoCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private ConstructorInfo ci;

		internal ConstructorInfoCodeEmitter(OpCode opcode, ConstructorInfo ci)
		{
			this.opcode = opcode;
			this.ci = ci;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, ci);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, ConstructorInfo ci)
	{
		Debug.Assert(ci != null);
		return new ConstructorInfoCodeEmitter(opcode, ci);
	}

	private class FieldInfoCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private FieldInfo fi;

		internal FieldInfoCodeEmitter(OpCode opcode, FieldInfo fi)
		{
			this.opcode = opcode;
			this.fi = fi;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, fi);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, FieldInfo fi)
	{
		Debug.Assert(fi != null);
		Debug.Assert(!fi.IsLiteral);
		return new FieldInfoCodeEmitter(opcode, fi);
	}

	internal static CodeEmitter CreateLoadConstantField(FieldInfo field)
	{
		// NOTE the ReflectionOnConstant bug is also a problem when we're not
		// statically compiling, but the warnings are too intrusive
		// TODO if we ever get a pedantic mode, we should enable this warning at
		// runtime as well
		if(JVM.IsStaticCompiler)
		{
			ReflectionOnConstant.IssueWarning(field);
		}
		return CreateLoadConstant(field.GetValue(null));
	}

	internal static CodeEmitter CreateLoadConstant(object constant)
	{
		if(constant == null)
		{
			return new CodeEmitter.OpCodeEmitter(OpCodes.Ldnull);
		}
		else if(constant is int || 
			constant is short || constant is ushort ||
			constant is byte || constant is sbyte ||
			constant is char ||
			constant is bool)
		{
			return CodeEmitter.Create(OpCodes.Ldc_I4, ((IConvertible)constant).ToInt32(null));
		}
		else if(constant is uint)
		{
			return CodeEmitter.Create(OpCodes.Ldc_I4, unchecked((int)((IConvertible)constant).ToUInt32(null)));
		}
		else if(constant is string)
		{
			return CodeEmitter.Create(OpCodes.Ldstr, (string)constant);
		}
		else if(constant is float)
		{
			return CodeEmitter.Create(OpCodes.Ldc_R4, (float)constant);
		}
		else if(constant is double)
		{
			return CodeEmitter.Create(OpCodes.Ldc_R8, (double)constant);
		}
		else if(constant is long)
		{
			return CodeEmitter.Create(OpCodes.Ldc_I8, (long)constant);
		}
		else if(constant is ulong)
		{
			return CodeEmitter.Create(OpCodes.Ldc_I8, unchecked((long)(ulong)constant));
		}
		else if(constant is Enum)
		{
			Type underlying = Enum.GetUnderlyingType(constant.GetType());
			if(underlying == typeof(long))
			{
				return CodeEmitter.Create(OpCodes.Ldc_I8, ((IConvertible)constant).ToInt64(null));
			}
			if(underlying == typeof(ulong))
			{
				return CodeEmitter.Create(OpCodes.Ldc_I8, unchecked((long)((IConvertible)constant).ToUInt64(null)));
			}
			else if(underlying == typeof(uint))
			{
				return CodeEmitter.Create(OpCodes.Ldc_I4, unchecked((int)((IConvertible)constant).ToUInt32(null)));
			}
			else
			{
				return CodeEmitter.Create(OpCodes.Ldc_I4, ((IConvertible)constant).ToInt32(null));
			}
		}
		else
		{
			throw new NotImplementedException(constant.GetType().FullName);
		}
	}

	private class EmitUnboxCallCodeEmitter : CodeEmitter
	{
		private TypeWrapper wrapper;

		internal EmitUnboxCallCodeEmitter(TypeWrapper wrapper)
		{
			this.wrapper = wrapper;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			wrapper.EmitUnbox(ilgen);
		}
	}

	internal static CodeEmitter CreateEmitUnboxCall(TypeWrapper wrapper)
	{
		return new EmitUnboxCallCodeEmitter(wrapper);
	}

	private class EmitBoxCallCodeEmitter : CodeEmitter
	{
		private TypeWrapper wrapper;

		internal EmitBoxCallCodeEmitter(TypeWrapper wrapper)
		{
			this.wrapper = wrapper;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			wrapper.EmitBox(ilgen);
		}
	}

	internal static CodeEmitter CreateEmitBoxCall(TypeWrapper wrapper)
	{
		return new EmitBoxCallCodeEmitter(wrapper);
	}

	private class OpCodeEmitter : CodeEmitter
	{
		private OpCode opcode;

		internal OpCodeEmitter(OpCode opcode)
		{
			this.opcode = opcode;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode);
		}
	}

	private class TypeCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private Type type;

		internal TypeCodeEmitter(OpCode opcode, Type type)
		{
			this.opcode = opcode;
			this.type = type;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, type);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, Type type)
	{
		return new TypeCodeEmitter(opcode, type);
	}

	private class IntCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private int i;

		internal IntCodeEmitter(OpCode opcode, int i)
		{
			this.opcode = opcode;
			this.i = i;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, i);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, int i)
	{
		return new IntCodeEmitter(opcode, i);
	}

	private class FloatCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private float f;

		internal FloatCodeEmitter(OpCode opcode, float f)
		{
			this.opcode = opcode;
			this.f = f;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, f);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, float f)
	{
		return new FloatCodeEmitter(opcode, f);
	}

	private class DoubleCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private double d;

		internal DoubleCodeEmitter(OpCode opcode, double d)
		{
			this.opcode = opcode;
			this.d = d;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, d);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, double d)
	{
		return new DoubleCodeEmitter(opcode, d);
	}

	private class StringCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private string s;

		internal StringCodeEmitter(OpCode opcode, string s)
		{
			this.opcode = opcode;
			this.s = s;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, s);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, string s)
	{
		return new StringCodeEmitter(opcode, s);
	}

	private class LongCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private long l;

		internal LongCodeEmitter(OpCode opcode, long l)
		{
			this.opcode = opcode;
			this.l = l;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, l);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, long l)
	{
		return new LongCodeEmitter(opcode, l);
	}
}

class ReturnCastEmitter : CodeEmitter
{
	private TypeWrapper type;

	internal ReturnCastEmitter(TypeWrapper type)
	{
		this.type = type;
	}

	internal override void Emit(ILGenerator ilgen)
	{
		if(type.IsGhost)
		{
			LocalBuilder local1 = ilgen.DeclareLocal(type.TypeAsLocalOrStackType);
			ilgen.Emit(OpCodes.Stloc, local1);
			LocalBuilder local2 = ilgen.DeclareLocal(type.TypeAsParameterType);
			ilgen.Emit(OpCodes.Ldloca, local2);
			ilgen.Emit(OpCodes.Ldloc, local1);
			ilgen.Emit(OpCodes.Stfld, type.GhostRefField);
			ilgen.Emit(OpCodes.Ldloca, local2);
			ilgen.Emit(OpCodes.Ldobj, type.TypeAsParameterType);
		}
		else if(type.TypeAsParameterType != typeof(object))
		{
			ilgen.Emit(OpCodes.Castclass, type.TypeAsParameterType);
		}
	}
}
