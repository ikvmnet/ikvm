/*
  Copyright (C) 2002, 2004 Jeroen Frijters

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
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Diagnostics.SymbolStore;

namespace IKVM.Internal
{
	class CountingILGenerator
	{
		private ILGenerator ilgen;
		private int offset;
		private ArrayList locals = new ArrayList();
		private Stack exceptionStack = new Stack();
		private bool inFinally;
		private IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter linenums;
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
			return offset;
		}

		internal void BeginCatchBlock(Type exceptionType)
		{
			offset += 5;
			ilgen.BeginCatchBlock(exceptionType);
		}

		internal void BeginExceptFilterBlock()
		{
			offset += 5;
			ilgen.BeginExceptFilterBlock();
		}

		internal Label BeginExceptionBlock()
		{
			exceptionStack.Push(inFinally);
			inFinally = false;
			return ilgen.BeginExceptionBlock();
		}

		internal void BeginFaultBlock()
		{
			offset += 5;
			ilgen.BeginFaultBlock();
		}

		internal void BeginFinallyBlock()
		{
			inFinally = true;
			offset += 5;
			ilgen.BeginFinallyBlock();
		}

		internal void BeginScope()
		{
			ilgen.BeginScope();
		}

		internal LocalBuilder DeclareLocal(Type localType)
		{
			LocalBuilder loc = ilgen.DeclareLocal(localType);
			locals.Add(loc);
			return loc;
		}

		internal Label DefineLabel()
		{
			Label label = ilgen.DefineLabel();
#if LABELCHECK
		labels.Add(label, new System.Diagnostics.StackFrame(1, true));
#endif
			return label;
		}

		internal void Emit(OpCode opcode)
		{
			offset += opcode.Size;
			ilgen.Emit(opcode);
		}

		internal void Emit(OpCode opcode, byte arg)
		{
			offset += opcode.Size + 1;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, ConstructorInfo con)
		{
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, con);
		}

		internal void Emit(OpCode opcode, double arg)
		{
			offset += opcode.Size + 8;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, FieldInfo field)
		{
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, field);
		}

		internal void Emit(OpCode opcode, short arg)
		{
			offset += opcode.Size + 2;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, int arg)
		{
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, long arg)
		{
			offset += opcode.Size + 8;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, Label label)
		{
			offset += opcode.Size;
			ilgen.Emit(opcode, label);
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

		internal void Emit(OpCode opcode, Label[] labels)
		{
			offset += 5 + labels.Length * 4;
			ilgen.Emit(opcode, labels);
		}

		internal void Emit(OpCode opcode, LocalBuilder local)
		{
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
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, meth);
		}

		internal void Emit(OpCode opcode, sbyte arg)
		{
			offset += opcode.Size + 1;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, SignatureHelper signature)
		{
			offset += opcode.Size;
			ilgen.Emit(opcode, signature);
			throw new NotImplementedException();
		}

		internal void Emit(OpCode opcode, float arg)
		{
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, string arg)
		{
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, arg);
		}

		internal void Emit(OpCode opcode, Type cls)
		{
			offset += opcode.Size + 4;
			ilgen.Emit(opcode, cls);
		}

		internal void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			offset += opcode.Size;
			ilgen.EmitCall(opcode, methodInfo, optionalParameterTypes);
			throw new NotImplementedException();
		}

		internal void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			offset += 5;
			ilgen.EmitCalli(opcode, unmanagedCallConv, returnType, parameterTypes);
		}

		internal void EmitCalli(OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			offset += 5;
			ilgen.EmitCalli(opcode, callingConvention, returnType, parameterTypes, optionalParameterTypes);
		}

		internal void EmitWriteLine(FieldInfo fld)
		{
			ilgen.EmitWriteLine(fld);
			throw new NotImplementedException();
		}

		internal void EmitWriteLine(LocalBuilder localBuilder)
		{
			ilgen.EmitWriteLine(localBuilder);
			throw new NotImplementedException();
		}

		internal void EmitWriteLine(string value)
		{
			offset += 10;
			ilgen.EmitWriteLine(value);
		}

		internal void EndExceptionBlock()
		{
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
			ilgen.EndScope();
		}

		internal void MarkLabel(Label loc)
		{
#if LABELCHECK
		labels.Remove(loc);
#endif
			ilgen.MarkLabel(loc);
		}

		internal void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
		{
			ilgen.MarkSequencePoint(document, startLine, startColumn, endLine, endColumn);
		}

		internal void ThrowException(Type excType)
		{
			offset += 6;
			ilgen.ThrowException(excType);
		}

		internal void UsingNamespace(string usingNamespace)
		{
			ilgen.UsingNamespace(usingNamespace);
		}

		internal void SetLineNumber(ushort line)
		{
			if(linenums == null)
			{
				linenums = new IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter(32);
			}
			linenums.AddMapping(offset, line);
		}

		internal void EmitLineNumberTable(MethodBase mb)
		{
			if(!IKVM.Internal.JVM.NoStackTraceInfo && linenums != null)
			{
				AttributeHelper.SetLineNumberTable(mb, linenums.ToArray());
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

	public abstract class CodeEmitter
	{
		internal abstract void Emit(CountingILGenerator ilgen);
	}
}
