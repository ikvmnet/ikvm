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
		private Type boxType;
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
			LazyGen();
			boxType = type;
		}

		internal bool IsBoxPending(Type type)
		{
			return boxType == type;
		}

		internal void ClearPendingBox()
		{
			boxType = null;
		}

		internal void LazyEmitUnbox(Type type)
		{
			if(boxType != null && boxType == type)
			{
				// the unbox and lazy box cancel each other out
				boxType = null;
				// unbox leaves a pointer to the value of the stack (instead of the value)
				// so we have to copy the value into a local variable and load the address
				// of the local onto the stack
				LocalBuilder local = DeclareLocal(type);
				Emit(OpCodes.Stloc, local);
				Emit(OpCodes.Ldloca, local);
			}
			else
			{
				Emit(OpCodes.Unbox, type);
			}
		}

		internal void LazyEmitUnboxSpecial(Type type)
		{
			if(boxType != null && boxType == type)
			{
				// the unbox and lazy box cancel each other out
				boxType = null;
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

		private void LazyGen()
		{
			if(boxType != null)
			{
				Type t = boxType;
				boxType = null;
				Emit(OpCodes.Box, t);
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
	}
}
#endif
