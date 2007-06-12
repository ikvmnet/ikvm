/*
  Copyright (C) 2002, 2004, 2005, 2006 Jeroen Frijters

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
#if !COMPACT_FRAMEWORK
using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Diagnostics.SymbolStore;

namespace IKVM.Internal
{
	class CountingLabel
	{
		private Label label;
		private int offset = -1;

		internal CountingLabel(Label label)
		{
			this.label = label;
		}

		internal Label Label
		{
			get
			{
				return label;
			}
		}

		internal int Offset
		{
			get
			{
				return offset;
			}
			set
			{
				offset = value;
			}
		}
	}

	class CountingILGenerator
	{
		private ILGenerator ilgen;
		private int offset;
		private ArrayList locals = new ArrayList();
		private Stack exceptionStack = new Stack();
		private bool inFinally;
#if STATIC_COMPILER
		private IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter linenums;
#endif // STATIC_COMPILER
		private Expr stack;
		private CountingLabel lazyBranch;
#if LABELCHECK
		private Hashtable labels = new Hashtable();
#endif

		public static implicit operator CountingILGenerator(ILGenerator ilgen)
		{
			return new CountingILGenerator(ilgen);
		}

		internal CountingILGenerator(ILGenerator ilgen)
		{
			this.ilgen = ilgen;
		}

		internal bool IsStackEmpty
		{
			get
			{
				return stack == null;
			}
		}

		internal int GetILOffset()
		{
			LazyGen();
			return offset;
		}

		internal void BeginCatchBlock(Type exceptionType)
		{
			LazyGen();
			offset += 5;
			ilgen.BeginCatchBlock(exceptionType);
		}

		internal void BeginExceptFilterBlock()
		{
			LazyGen();
			offset += 5;
			ilgen.BeginExceptFilterBlock();
		}

		internal Label BeginExceptionBlock()
		{
			LazyGen();
			exceptionStack.Push(inFinally);
			inFinally = false;
			return ilgen.BeginExceptionBlock();
		}

		internal void BeginFaultBlock()
		{
			LazyGen();
			inFinally = true;
			offset += 5;
			ilgen.BeginFaultBlock();
		}

		internal void BeginFinallyBlock()
		{
			LazyGen();
			inFinally = true;
			offset += 5;
			ilgen.BeginFinallyBlock();
		}

		internal void BeginScope()
		{
			LazyGen();
			ilgen.BeginScope();
		}

		internal LocalBuilder DeclareLocal(Type localType)
		{
			LocalBuilder loc = ilgen.DeclareLocal(localType);
			locals.Add(loc);
			return loc;
		}

		internal CountingLabel DefineLabel()
		{
			Label label = ilgen.DefineLabel();
#if LABELCHECK
			labels.Add(label, new System.Diagnostics.StackFrame(1, true));
#endif
			return new CountingLabel(label);
		}

		internal void Emit(OpCode opcode)
		{
			LazyGen();
			offset += opcode.Size;
			ilgen.Emit(opcode);
		}

		internal void Emit(OpCode opcode, byte arg)
		{
			LazyGen();
			offset += opcode.Size + 1;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, ConstructorInfo con)
		{
			LazyGen();
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, con);
		}

		internal void Emit(OpCode opcode, double arg)
		{
			LazyGen();
			offset += opcode.Size + 8;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, FieldInfo field)
		{
			LazyGen();
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, field);
		}

		internal void Emit(OpCode opcode, short arg)
		{
			LazyGen();
			offset += opcode.Size + 2;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, int arg)
		{
			LazyGen();
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, long arg)
		{
			LazyGen();
			offset += opcode.Size + 8;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, CountingLabel label)
		{
			LazyGen();
			if(label.Offset == -1)
			{
				if(opcode.Value == OpCodes.Br.Value)
				{
					lazyBranch = label;
					return;
				}
			}
			else if(offset - label.Offset < 126)
			{
				if(opcode.Value == OpCodes.Brtrue.Value)
				{
					opcode = OpCodes.Brtrue_S;
				}
				else if(opcode.Value == OpCodes.Brfalse.Value)
				{
					opcode = OpCodes.Brfalse_S;
				}
				else if(opcode.Value == OpCodes.Br.Value)
				{
					opcode = OpCodes.Br_S;
				}
				else if(opcode.Value == OpCodes.Beq.Value)
				{
					opcode = OpCodes.Beq_S;
				}
				else if(opcode.Value == OpCodes.Bne_Un.Value)
				{
					opcode = OpCodes.Bne_Un_S;
				}
				else if(opcode.Value == OpCodes.Ble.Value)
				{
					opcode = OpCodes.Ble_S;
				}
				else if(opcode.Value == OpCodes.Blt.Value)
				{
					opcode = OpCodes.Blt_S;
				}
				else if(opcode.Value == OpCodes.Bge.Value)
				{
					opcode = OpCodes.Bge_S;
				}
				else if(opcode.Value == OpCodes.Bgt.Value)
				{
					opcode = OpCodes.Bgt_S;
				}
			}
			offset += opcode.Size;
			ilgen.Emit(opcode, label.Label);
			switch(opcode.OperandType)
			{
				case OperandType.InlineBrTarget:
					offset += 4;
					break;
				case OperandType.ShortInlineBrTarget:
					offset += 1;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		internal void Emit(OpCode opcode, CountingLabel[] labels)
		{
			LazyGen();
			offset += 5 + labels.Length * 4;
			Label[] real = new Label[labels.Length];
			for(int i = 0; i < labels.Length; i++)
			{
				real[i] = labels[i].Label;
			}
			ilgen.Emit(opcode, real);
		}

		internal void Emit(OpCode opcode, LocalBuilder local)
		{
			LazyGen();
			ilgen.Emit(opcode, local);
			int index = locals.IndexOf(local);
			if(index < 4 && opcode.Value != OpCodes.Ldloca.Value && opcode.Value != OpCodes.Ldloca_S.Value)
			{
				offset += 1;
			}
			else if(index < 256)
			{
				offset += 2;
			}
			else
			{
				offset += 4;
			}
		}

		internal void Emit(OpCode opcode, MethodInfo meth)
		{
			LazyGen();
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, meth);
		}

		internal void Emit(OpCode opcode, sbyte arg)
		{
			LazyGen();
			offset += opcode.Size + 1;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, SignatureHelper signature)
		{
			LazyGen();
			offset += opcode.Size;
			ilgen.Emit(opcode, signature);
			throw new NotImplementedException();
		}

		internal void Emit(OpCode opcode, float arg)
		{
			LazyGen();
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, string arg)
		{
			LazyGen();
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, Type cls)
		{
			LazyGen();
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, cls);
		}

		internal void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			LazyGen();
			offset += opcode.Size;
			ilgen.EmitCall(opcode, methodInfo, optionalParameterTypes);
			throw new NotImplementedException();
		}

		internal void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			LazyGen();
			offset += 5;
			ilgen.EmitCalli(opcode, unmanagedCallConv, returnType, parameterTypes);
		}

		internal void EmitCalli(OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			LazyGen();
			offset += 5;
			ilgen.EmitCalli(opcode, callingConvention, returnType, parameterTypes, optionalParameterTypes);
		}

		internal void EmitWriteLine(FieldInfo fld)
		{
			LazyGen();
			ilgen.EmitWriteLine(fld);
			throw new NotImplementedException();
		}

		internal void EmitWriteLine(LocalBuilder localBuilder)
		{
			LazyGen();
			ilgen.EmitWriteLine(localBuilder);
			throw new NotImplementedException();
		}

		internal void EmitWriteLine(string value)
		{
			LazyGen();
			offset += 10;
			ilgen.EmitWriteLine(value);
		}

		internal void EndExceptionBlock()
		{
			LazyGen();
			if(inFinally)
			{
				offset += 1;
			}
			else
			{
				offset += 5;
			}
			inFinally = (bool)exceptionStack.Pop();
			ilgen.EndExceptionBlock();
		}

		internal void EndScope()
		{
			LazyGen();
			ilgen.EndScope();
		}

		internal void MarkLabel(CountingLabel loc)
		{
			if(lazyBranch == loc)
			{
				lazyBranch = null;
			}
			LazyGen();
#if LABELCHECK
			labels.Remove(loc.Label);
#endif
			ilgen.MarkLabel(loc.Label);
			loc.Offset = offset;
		}

		internal void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
		{
			LazyGen();
			ilgen.MarkSequencePoint(document, startLine, startColumn, endLine, endColumn);
		}

		internal void ThrowException(Type excType)
		{
			LazyGen();
			offset += 6;
			ilgen.ThrowException(excType);
		}

		internal void UsingNamespace(string usingNamespace)
		{
			LazyGen();
			ilgen.UsingNamespace(usingNamespace);
		}

#if STATIC_COMPILER
		internal void SetLineNumber(ushort line)
		{
			LazyGen();
			if(linenums == null)
			{
				linenums = new IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter(32);
			}
			linenums.AddMapping(offset, line);
		}

		internal void EmitLineNumberTable(MethodBase mb)
		{
			if(linenums != null)
			{
				AttributeHelper.SetLineNumberTable(mb, linenums);
			}
		}
#endif // STATIC_COMPILER

		internal void LazyEmitBox(Type type)
		{
			stack = new BoxExpr(stack, type);
		}

		internal void LazyEmitUnbox(Type type)
		{
			BoxExpr box = stack as BoxExpr;
			if(box != null)
			{
				stack = new BoxUnboxExpr(box.Expr, type);
			}
			else
			{
				stack = new UnboxExpr(stack, type);
			}
		}

		internal void LazyEmitLdobj(Type type)
		{
			BoxUnboxExpr boxunbox = stack as BoxUnboxExpr;
			if(boxunbox != null)
			{
				// box/unbox+ldobj annihilate each other
				stack = boxunbox.Expr;
			}
			else
			{
				stack = new LdobjExpr(stack, type);
			}
		}

		internal void LazyEmitUnboxSpecial(Type type)
		{
			BoxExpr box = stack as BoxExpr;
			if(box != null)
			{
				// the unbox and lazy box cancel each other out
				stack = box.Expr;
			}
			else
			{
				// NOTE if the reference is null, we treat it as a default instance of the value type.
				Emit(OpCodes.Dup);
				CountingLabel label1 = DefineLabel();
				Emit(OpCodes.Brtrue_S, label1);
				Emit(OpCodes.Pop);
				Emit(OpCodes.Ldloc, DeclareLocal(type));
				CountingLabel label2 = DefineLabel();
				Emit(OpCodes.Br_S, label2);
				MarkLabel(label1);
				Emit(OpCodes.Unbox, type);
				Emit(OpCodes.Ldobj, type);
				MarkLabel(label2);
			}
		}

		internal void LazyEmitLdc_I4(int i)
		{
			LazyGen();
			stack = new ConstIntExpr(i);
		}

		internal void LazyEmitLdc_I8(long l)
		{
			LazyGen();
			stack = new ConstLongExpr(l);
		}

		internal void LazyEmitLdstr(string str)
		{
			LazyGen();
			stack = new ConstStringExpr(str);
		}

		internal void LazyEmit_idiv()
		{
			// we need to special case dividing by -1, because the CLR div instruction
			// throws an OverflowException when dividing Int32.MinValue by -1, and
			// Java just silently overflows
			ConstIntExpr v = stack as ConstIntExpr;
			if(v != null)
			{
				if(v.i == -1)
				{
					stack = null;
					Emit(OpCodes.Neg);
				}
				else
				{
					Emit(OpCodes.Div);
				}
			}
			else
			{
				Emit(OpCodes.Dup);
				Emit(OpCodes.Ldc_I4_M1);
				CountingLabel label = DefineLabel();
				Emit(OpCodes.Bne_Un_S, label);
				Emit(OpCodes.Pop);
				Emit(OpCodes.Neg);
				CountingLabel label2 = DefineLabel();
				Emit(OpCodes.Br_S, label2);
				MarkLabel(label);
				Emit(OpCodes.Div);
				MarkLabel(label2);
			}
		}

		internal void LazyEmit_ldiv()
		{
			// we need to special case dividing by -1, because the CLR div instruction
			// throws an OverflowException when dividing Int32.MinValue by -1, and
			// Java just silently overflows
			ConstLongExpr v = stack as ConstLongExpr;
			if(v != null)
			{
				if(v.l == -1)
				{
					stack = null;
					Emit(OpCodes.Neg);
				}
				else
				{
					Emit(OpCodes.Div);
				}
			}
			else
			{
				Emit(OpCodes.Dup);
				Emit(OpCodes.Ldc_I4_M1);
				Emit(OpCodes.Conv_I8);
				CountingLabel label = DefineLabel();
				Emit(OpCodes.Bne_Un_S, label);
				Emit(OpCodes.Pop);
				Emit(OpCodes.Neg);
				CountingLabel label2 = DefineLabel();
				Emit(OpCodes.Br_S, label2);
				MarkLabel(label);
				Emit(OpCodes.Div);
				MarkLabel(label2);
			}
		}

		internal void LazyEmit_instanceof(Type type)
		{
			LazyGen();
			stack = new InstanceOfExpr(type);
		}

		internal void LazyEmit_ifeq(CountingLabel label)
		{
			InstanceOfExpr instanceof = stack as InstanceOfExpr;
			if(instanceof != null)
			{
				stack = null;
				Emit(OpCodes.Isinst, instanceof.Type);
			}
			Emit(OpCodes.Brfalse, label);
		}

		internal void LazyEmit_ifne(CountingLabel label)
		{
			InstanceOfExpr instanceof = stack as InstanceOfExpr;
			if(instanceof != null)
			{
				stack = null;
				Emit(OpCodes.Isinst, instanceof.Type);
			}
			Emit(OpCodes.Brtrue, label);
		}

		internal string PopLazyLdstr()
		{
			ConstStringExpr str = stack as ConstStringExpr;
			if(str != null)
			{
				stack = null;
				return str.str;
			}
			return null;
		}

		private void LazyGen()
		{
			if(stack != null)
			{
				Expr exp = stack;
				stack = null;
				exp.Emit(this);
			}
			if(lazyBranch != null)
			{
				offset += OpCodes.Br.Size + 4;
				ilgen.Emit(OpCodes.Br, lazyBranch.Label);
				lazyBranch = null;
			}
		}

		internal void Finish()
		{
#if LABELCHECK
			foreach(System.Diagnostics.StackFrame frame in labels.Values)
			{
				string name = frame.GetFileName() + ":" + frame.GetFileLineNumber();
				IKVM.Internal.JVM.CriticalFailure("Label failure: " + name, null);
			}
#endif
		}

		abstract class Expr
		{
			internal readonly Type Type;

			protected Expr(Type type)
			{
				this.Type = type;
			}

			internal abstract void Emit(CountingILGenerator ilgen);
		}

		abstract class UnaryExpr : Expr
		{
			internal readonly Expr Expr;

			protected UnaryExpr(Expr expr, Type type)
				: base(type)
			{
				this.Expr = expr;
			}

			internal override void Emit(CountingILGenerator ilgen)
			{
				if (Expr != null)
				{
					Expr.Emit(ilgen);
				}
			}
		}

		class BoxExpr : UnaryExpr
		{
			internal BoxExpr(Expr expr, Type type)
				: base(expr, type)
			{
			}

			internal override void Emit(CountingILGenerator ilgen)
			{
				base.Emit(ilgen);
				ilgen.Emit(OpCodes.Box, Type);
			}
		}

		class UnboxExpr : UnaryExpr
		{
			internal UnboxExpr(Expr expr, Type type)
				: base(expr, type)
			{
			}

			internal override void Emit(CountingILGenerator ilgen)
			{
				base.Emit(ilgen);
				ilgen.Emit(OpCodes.Unbox, Type);
			}
		}

		class BoxUnboxExpr : UnaryExpr
		{
			internal BoxUnboxExpr(Expr expr, Type type)
				: base(expr, type)
			{
			}

			internal override void Emit(CountingILGenerator ilgen)
			{
				base.Emit(ilgen);
				// unbox leaves a pointer to the value of the stack (instead of the value)
				// so we have to copy the value into a local variable and load the address
				// of the local onto the stack
				LocalBuilder local = ilgen.DeclareLocal(Type);
				ilgen.Emit(OpCodes.Stloc, local);
				ilgen.Emit(OpCodes.Ldloca, local);
			}
		}

		class LdobjExpr : UnaryExpr
		{
			internal LdobjExpr(Expr expr, Type type)
				: base(expr, type)
			{
			}

			internal override void Emit(CountingILGenerator ilgen)
			{
				base.Emit(ilgen);
				ilgen.Emit(OpCodes.Ldobj, Type);
			}
		}

		class ConstIntExpr : Expr
		{
			internal readonly int i;

			internal ConstIntExpr(int i)
				: base(typeof(int))
			{
				this.i = i;
			}

			internal override void Emit(CountingILGenerator ilgen)
			{
				switch(i)
				{
					case -1:
						ilgen.Emit(OpCodes.Ldc_I4_M1);
						break;
					case 0:
						ilgen.Emit(OpCodes.Ldc_I4_0);
						break;
					case 1:
						ilgen.Emit(OpCodes.Ldc_I4_1);
						break;
					case 2:
						ilgen.Emit(OpCodes.Ldc_I4_2);
						break;
					case 3:
						ilgen.Emit(OpCodes.Ldc_I4_3);
						break;
					case 4:
						ilgen.Emit(OpCodes.Ldc_I4_4);
						break;
					case 5:
						ilgen.Emit(OpCodes.Ldc_I4_5);
						break;
					case 6:
						ilgen.Emit(OpCodes.Ldc_I4_6);
						break;
					case 7:
						ilgen.Emit(OpCodes.Ldc_I4_7);
						break;
					case 8:
						ilgen.Emit(OpCodes.Ldc_I4_8);
						break;
					default:
						if(i >= -128 && i <= 127)
						{
							ilgen.Emit(OpCodes.Ldc_I4_S, (sbyte)i);
						}
						else
						{
							ilgen.Emit(OpCodes.Ldc_I4, i);
						}
						break;
				}
			}
		}

		class ConstLongExpr : Expr
		{
			internal readonly long l;

			internal ConstLongExpr(long l)
				: base(typeof(long))
			{
				this.l = l;
			}

			internal override void Emit(CountingILGenerator ilgen)
			{
				switch(l)
				{
					case -1:
						ilgen.Emit(OpCodes.Ldc_I4_M1);
						ilgen.Emit(OpCodes.Conv_I8);
						break;
					case 0:
						ilgen.Emit(OpCodes.Ldc_I4_0);
						ilgen.Emit(OpCodes.Conv_I8);
						break;
					case 1:
						ilgen.Emit(OpCodes.Ldc_I4_1);
						ilgen.Emit(OpCodes.Conv_I8);
						break;
					case 2:
						ilgen.Emit(OpCodes.Ldc_I4_2);
						ilgen.Emit(OpCodes.Conv_I8);
						break;
					case 3:
						ilgen.Emit(OpCodes.Ldc_I4_3);
						ilgen.Emit(OpCodes.Conv_I8);
						break;
					case 4:
						ilgen.Emit(OpCodes.Ldc_I4_4);
						ilgen.Emit(OpCodes.Conv_I8);
						break;
					case 5:
						ilgen.Emit(OpCodes.Ldc_I4_5);
						ilgen.Emit(OpCodes.Conv_I8);
						break;
					case 6:
						ilgen.Emit(OpCodes.Ldc_I4_6);
						ilgen.Emit(OpCodes.Conv_I8);
						break;
					case 7:
						ilgen.Emit(OpCodes.Ldc_I4_7);
						ilgen.Emit(OpCodes.Conv_I8);
						break;
					case 8:
						ilgen.Emit(OpCodes.Ldc_I4_8);
						ilgen.Emit(OpCodes.Conv_I8);
						break;
					default:
						if(l >= -2147483648L && l <= 4294967295L)
						{
							if(l >= -128 && l <= 127)
							{
								ilgen.Emit(OpCodes.Ldc_I4_S, (sbyte)l);
							}
							else
							{
								ilgen.Emit(OpCodes.Ldc_I4, (int)l);
							}
							if(l < 0)
							{
								ilgen.Emit(OpCodes.Conv_I8);
							}
							else
							{
								ilgen.Emit(OpCodes.Conv_U8);
							}
						}
						else
						{
							ilgen.Emit(OpCodes.Ldc_I8, l);
						}
						break;
				}
			}
		}

		class ConstStringExpr : Expr
		{
			internal readonly string str;

			internal ConstStringExpr(string str)
				: base(typeof(string))
			{
				this.str = str;
			}

			internal override void Emit(CountingILGenerator ilgen)
			{
				ilgen.Emit(OpCodes.Ldstr, str);
			}
		}

		class InstanceOfExpr : Expr
		{
			internal InstanceOfExpr(Type type)
				: base(type)
			{
			}

			internal override void Emit(CountingILGenerator ilgen)
			{
				ilgen.Emit(OpCodes.Isinst, this.Type);
				ilgen.Emit(OpCodes.Ldnull);
				ilgen.Emit(OpCodes.Cgt_Un);
			}
		}
	}
}
#endif
