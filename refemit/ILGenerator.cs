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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using IKVM.Reflection.Emit.Writer;
using System.Diagnostics.SymbolStore;
using System.Diagnostics;

namespace IKVM.Reflection.Emit
{
	public struct Label
	{
		// 1-based here, to make sure that an uninitialized Label isn't valid
		private readonly int index1;

		internal Label(int index)
		{
			this.index1 = index + 1;
		}

		internal int Index
		{
			get { return index1 - 1; }
		}
	}

	public sealed class LocalBuilder
	{
		private readonly Type localType;
		private readonly int index;
		internal string name;

		internal LocalBuilder(Type localType, int index)
		{
			this.localType = localType;
			this.index = index;
		}

		public void SetLocalSymInfo(string name)
		{
			this.name = name;
		}

		public Type LocalType
		{
			get { return localType; }
		}

		public int LocalIndex
		{
			get { return index; }
		}
	}

	public sealed class ILGenerator
	{
		private static readonly Type FAULT = typeof(ExceptionBlock); // the type we use here doesn't matter, as long as it can never be used as a real exception type
		private readonly ModuleBuilder moduleBuilder;
		private readonly ByteBuffer code = new ByteBuffer(16);
		private readonly List<LocalBuilder> locals = new List<LocalBuilder>();
		private readonly List<int> tokenFixups = new List<int>();
		private readonly List<int> labels = new List<int>();
		private readonly List<int> labelStackHeight = new List<int>();
		private readonly List<LabelFixup> labelFixups = new List<LabelFixup>();
		private readonly List<SequencePoint> sequencePoints = new List<SequencePoint>();
		private readonly List<ExceptionBlock> exceptions = new List<ExceptionBlock>();
		private readonly Stack<ExceptionBlock> exceptionStack = new Stack<ExceptionBlock>();
		private ushort maxStack;
		private int stackHeight;
		private Scope scope;

		private struct LabelFixup
		{
			internal int label;
			internal int offset;
		}

		private class ExceptionBlock : IComparer<ExceptionBlock>
		{
			internal readonly int ordinal;
			internal Label labelEnd;
			internal int tryOffset;
			internal int tryLength;
			internal int handlerOffset;
			internal int handlerLength;
			internal Type exceptionType;	// null = finally block, FAULT = fault block

			internal ExceptionBlock(int ordinal)
			{
				this.ordinal = ordinal;
			}

			int IComparer<ExceptionBlock>.Compare(ExceptionBlock x, ExceptionBlock y)
			{
				if (x.tryOffset == y.tryOffset && x.tryLength == y.tryLength)
				{
					return x.ordinal < y.ordinal ? -1 : 1;
				}
				if (x.tryOffset + x.tryLength <= y.tryOffset)
				{
					return -1;
				}
				if (y.tryOffset + y.tryLength <= x.tryOffset)
				{
					return 1;
				}
				if (x.tryOffset > y.tryOffset || (x.tryOffset == y.tryOffset && x.tryLength < y.tryLength))
				{
					return -1;
				}
				else
				{
					return 1;
				}
			}
		}

		private struct SequencePoint
		{
			internal ISymbolDocumentWriter document;
			internal int offset;
			internal int startLine;
			internal int startColumn;
			internal int endLine;
			internal int endColumn;
		}

		private class Scope
		{
			internal readonly Scope parent;
			internal readonly List<Scope> children = new List<Scope>();
			internal readonly List<LocalBuilder> locals = new List<LocalBuilder>();
			internal int startOffset;
			internal int endOffset;

			internal Scope(Scope parent)
			{
				this.parent = parent;
			}
		}

		internal ILGenerator(ModuleBuilder moduleBuilder)
		{
			this.moduleBuilder = moduleBuilder;
			if (moduleBuilder.symbolWriter != null)
			{
				scope = new Scope(null);
			}
		}

		public void BeginCatchBlock(Type exceptionType)
		{
			ExceptionBlock block = exceptionStack.Peek();
			Emit(OpCodes.Leave, block.labelEnd);
			stackHeight = 0;
			UpdateStack(1);
			if (block.tryLength == 0)
			{
				block.tryLength = code.Position - block.tryOffset;
			}
			else
			{
				block.handlerLength = code.Position - block.handlerOffset;
				exceptionStack.Pop();
				ExceptionBlock newBlock = new ExceptionBlock(exceptions.Count);
				newBlock.labelEnd = block.labelEnd;
				newBlock.tryOffset = block.tryOffset;
				newBlock.tryLength = block.tryLength;
				block = newBlock;
				exceptions.Add(block);
				exceptionStack.Push(block);
			}
			block.handlerOffset = code.Position;
			block.exceptionType = exceptionType;
		}

		public void BeginExceptFilterBlock()
		{
			throw new NotImplementedException();
		}

		public Label BeginExceptionBlock()
		{
			ExceptionBlock block = new ExceptionBlock(exceptions.Count);
			block.labelEnd = DefineLabel();
			block.tryOffset = code.Position;
			exceptionStack.Push(block);
			exceptions.Add(block);
			return block.labelEnd;
		}

		public void BeginFaultBlock()
		{
			BeginFinallyBlock();
			exceptionStack.Peek().exceptionType = FAULT;
		}

		public void BeginFinallyBlock()
		{
			ExceptionBlock block = exceptionStack.Peek();
			Emit(OpCodes.Leave, block.labelEnd);
			stackHeight = 0;
			if (block.handlerOffset == 0)
			{
				block.tryLength = code.Position - block.tryOffset;
			}
			else
			{
				block.handlerLength = code.Position - block.handlerOffset;
				MarkLabel(block.labelEnd);
				Label labelEnd = DefineLabel();
				Emit(OpCodes.Leave, labelEnd);
				exceptionStack.Pop();
				ExceptionBlock newBlock = new ExceptionBlock(exceptions.Count);
				newBlock.labelEnd = labelEnd;
				newBlock.tryOffset = block.tryOffset;
				newBlock.tryLength = code.Position - block.tryOffset;
				block = newBlock;
				exceptions.Add(block);
				exceptionStack.Push(block);
			}
			block.handlerOffset = code.Position;
		}

		public void EndExceptionBlock()
		{
			ExceptionBlock block = exceptionStack.Pop();
			if (block.exceptionType != null && block.exceptionType != FAULT)
			{
				Emit(OpCodes.Leave, block.labelEnd);
			}
			else
			{
				Emit(OpCodes.Endfinally);
			}
			block.handlerLength = code.Position - block.handlerOffset;
			MarkLabel(block.labelEnd);
		}

		public void BeginScope()
		{
			Scope newScope = new Scope(scope);
			scope.children.Add(newScope);
			scope = newScope;
			scope.startOffset = code.Position;
		}

		public LocalBuilder DeclareLocal(Type localType)
		{
			LocalBuilder local = new LocalBuilder(localType, locals.Count);
			locals.Add(local);
			if (scope != null)
			{
				scope.locals.Add(local);
			}
			return local;
		}

		public Label DefineLabel()
		{
			Label label = new Label(labels.Count);
			labels.Add(-1);
			labelStackHeight.Add(-1);
			return label;
		}

		public void Emit(OpCode opc)
		{
			if (opc.Value < 0)
			{
				code.Write((byte)(opc.Value >> 8));
			}
			code.Write((byte)opc.Value);
			switch (opc.FlowControl)
			{
				case FlowControl.Branch:
				case FlowControl.Break:
				case FlowControl.Return:
				case FlowControl.Throw:
					stackHeight = -1;
					break;
				default:
					UpdateStack(opc.StackDiff);
					break;
			}
		}

		private void UpdateStack(int stackdiff)
		{
			if (stackHeight == -1)
			{
				// we're about to emit unreachable code
				stackHeight = 0;
			}
			Debug.Assert(stackHeight >= 0 && stackHeight <= ushort.MaxValue);
			stackHeight += stackdiff;
			Debug.Assert(stackHeight >= 0 && stackHeight <= ushort.MaxValue);
			maxStack = Math.Max(maxStack, (ushort)stackHeight);
		}

		public void Emit(OpCode opc, byte arg)
		{
			Emit(opc);
			code.Write(arg);
		}

		public void Emit(OpCode opc, double arg)
		{
			Emit(opc);
			code.Write(arg);
		}

		public void Emit(OpCode opc, FieldInfo field)
		{
			Emit(opc);
			WriteToken(moduleBuilder.GetFieldToken(field));
		}

		public void Emit(OpCode opc, short arg)
		{
			Emit(opc);
			code.Write(arg);
		}

		public void Emit(OpCode opc, int arg)
		{
			Emit(opc);
			code.Write(arg);
		}

		public void Emit(OpCode opc, long arg)
		{
			Emit(opc);
			code.Write(arg);
		}

		public void Emit(OpCode opc, Label label)
		{
			// We need special stackHeight handling for unconditional branches,
			// because the branch and next flows have differing stack heights.
			// Note that this assumes that unconditional branches do not push/pop.
			int flowStackHeight = this.stackHeight;
			Emit(opc);
			if (opc.Value == OpCodes.Leave.Value || opc.Value == OpCodes.Leave_S.Value)
			{
				flowStackHeight = 0;
			}
			else if (opc.FlowControl != FlowControl.Branch)
			{
				flowStackHeight = this.stackHeight;
			}
			// if the label has already been marked, we can emit the branch offset directly
			if (labels[label.Index] != -1)
			{
				if (labelStackHeight[label.Index] != flowStackHeight)
				{
					// the "backward branch constraint" prohibits this, so we don't need to support it
					throw new NotSupportedException();
				}
				switch (opc.OperandType)
				{
					case OperandType.InlineBrTarget:
						code.Write(labels[label.Index] - (code.Position + 4));
						break;
					case OperandType.ShortInlineBrTarget:
						code.Write((byte)(labels[label.Index] - (code.Position + 1)));
						break;
					default:
						throw new NotImplementedException();
				}
			}
			else
			{
				Debug.Assert(labelStackHeight[label.Index] == -1 || labelStackHeight[label.Index] == flowStackHeight);
				labelStackHeight[label.Index] = flowStackHeight;
				LabelFixup fix = new LabelFixup();
				fix.label = label.Index;
				fix.offset = code.Position;
				labelFixups.Add(fix);
				switch (opc.OperandType)
				{
					case OperandType.InlineBrTarget:
						code.Write(4);
						break;
					case OperandType.ShortInlineBrTarget:
						code.Write((byte)1);
						break;
					default:
						throw new NotImplementedException();
				}
			}
		}

		public void Emit(OpCode opc, Label[] labels)
		{
			Emit(opc);
			LabelFixup fix = new LabelFixup();
			fix.label = -1;
			fix.offset = code.Position;
			labelFixups.Add(fix);
			code.Write(labels.Length);
			foreach (Label label in labels)
			{
				code.Write(label.Index);
				if (this.labels[label.Index] != -1)
				{
					if (labelStackHeight[label.Index] != stackHeight)
					{
						// the "backward branch constraint" prohibits this, so we don't need to support it
						throw new NotSupportedException();
					}
				}
				else
				{
					Debug.Assert(labelStackHeight[label.Index] == -1 || labelStackHeight[label.Index] == stackHeight);
					labelStackHeight[label.Index] = stackHeight;
				}
			}
		}

		public void Emit(OpCode opc, LocalBuilder local)
		{
			if ((opc.Value == OpCodes.Ldloc.Value || opc.Value == OpCodes.Ldloca.Value || opc.Value == OpCodes.Stloc.Value) && local.LocalIndex < 256)
			{
				if (opc.Value == OpCodes.Ldloc.Value)
				{
					switch (local.LocalIndex)
					{
						case 0:
							Emit(OpCodes.Ldloc_0);
							break;
						case 1:
							Emit(OpCodes.Ldloc_1);
							break;
						case 2:
							Emit(OpCodes.Ldloc_2);
							break;
						case 3:
							Emit(OpCodes.Ldloc_3);
							break;
						default:
							Emit(OpCodes.Ldloc_S);
							code.Write((byte)local.LocalIndex);
							break;
					}
				}
				else if (opc.Value == OpCodes.Ldloca.Value)
				{
					Emit(OpCodes.Ldloca_S);
					code.Write((byte)local.LocalIndex);
				}
				else if (opc.Value == OpCodes.Stloc.Value)
				{
					switch (local.LocalIndex)
					{
						case 0:
							Emit(OpCodes.Stloc_0);
							break;
						case 1:
							Emit(OpCodes.Stloc_1);
							break;
						case 2:
							Emit(OpCodes.Stloc_2);
							break;
						case 3:
							Emit(OpCodes.Stloc_3);
							break;
						default:
							Emit(OpCodes.Stloc_S);
							code.Write((byte)local.LocalIndex);
							break;
					}
				}
			}
			else
			{
				Emit(opc);
				switch (opc.OperandType)
				{
					case OperandType.InlineVar:
						code.Write((ushort)local.LocalIndex);
						break;
					case OperandType.ShortInlineVar:
						code.Write((byte)local.LocalIndex);
						break;
					default:
						throw new NotImplementedException();
				}
			}
		}

		private void WriteToken(FieldToken token)
		{
			if (token.IsPseudoToken)
			{
				tokenFixups.Add(code.Position);
			}
			code.Write(token.Token);
		}

		private void WriteToken(MethodToken token)
		{
			if (token.IsPseudoToken)
			{
				tokenFixups.Add(code.Position);
			}
			code.Write(token.Token);
		}

		private void WriteToken(ConstructorToken token)
		{
			if (token.IsPseudoToken)
			{
				tokenFixups.Add(code.Position);
			}
			code.Write(token.Token);
		}

		public void Emit(OpCode opc, MethodInfo method)
		{
			Emit(opc);
			WriteToken(moduleBuilder.GetMethodToken(method));
			if (opc.FlowControl == FlowControl.Call)
			{
				int stackdiff = 0;
				if (!method.IsStatic && opc.Value != OpCodes.Newobj.Value)
				{
					// pop this
					stackdiff--;
				}
				// pop parameters
				stackdiff -= GetParameterCount(method);
				if (method.ReturnType != typeof(void))
				{
					// push return value
					stackdiff++;
				}
				UpdateStack(stackdiff);
			}
		}

		public void Emit(OpCode opc, ConstructorInfo constructor)
		{
			Emit(opc);
			WriteToken(moduleBuilder.GetConstructorToken(constructor));
			if (opc.FlowControl == FlowControl.Call)
			{
				int stackdiff = 0;
				if (!constructor.IsStatic && opc.Value != OpCodes.Newobj.Value)
				{
					// pop this
					stackdiff--;
				}
				// pop parameters
				stackdiff -= GetParameterCount(constructor);
				UpdateStack(stackdiff);
			}
		}

		private static int GetParameterCount(MethodBase mb)
		{
			if (mb.IsGenericMethod)
			{
				return ((MethodInfo)mb).GetGenericMethodDefinition().GetParameters().Length;
			}
			else
			{
				return mb.GetParameters().Length;
			}
		}

		public void Emit(OpCode opc, sbyte arg)
		{
			Emit(opc);
			code.Write(arg);
		}

		public void Emit(OpCode opc, SignatureHelper sig)
		{
			throw new NotImplementedException();
		}

		public void Emit(OpCode opc, float arg)
		{
			Emit(opc);
			code.Write(arg);
		}

		public void Emit(OpCode opc, string str)
		{
			Emit(opc);
			code.Write(0x70000000 | moduleBuilder.UserStrings.Add(str));
		}

		public void Emit(OpCode opc, Type type)
		{
			Emit(opc);
			code.Write(moduleBuilder.GetTypeToken(type).Token);
		}

		public void EmitCall(OpCode opc, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			throw new NotImplementedException();
		}

		public void EmitCalli(OpCode opc, CallingConvention callingConvention, Type returnType, Type[] parameterTypes)
		{
			Emit(opc);
			ByteBuffer sig = new ByteBuffer(16);
			switch (callingConvention)
			{
				case CallingConvention.Cdecl:
					sig.Write((byte)0x01);	// C
					break;
				case CallingConvention.StdCall:
				case CallingConvention.Winapi:
					sig.Write((byte)0x02);	// STDCALL
					break;
				case CallingConvention.ThisCall:
					sig.Write((byte)0x03);	// THISCALL
					break;
				case CallingConvention.FastCall:
					sig.Write((byte)0x04);	// FASTCALL
					break;
				default:
					throw new InvalidOperationException();
			}
			sig.WriteCompressedInt(parameterTypes.Length);
			SignatureHelper.WriteType(moduleBuilder, sig, returnType);
			foreach (Type t in parameterTypes)
			{
				SignatureHelper.WriteType(moduleBuilder, sig, t);
			}
			code.Write(0x11000000 | moduleBuilder.Tables.StandAloneSig.Add(moduleBuilder.Blobs.Add(sig)));
		}

		public void EmitCalli(OpCode opc, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			throw new NotImplementedException();
		}

		public void EmitWriteLine(FieldInfo fld)
		{
			throw new NotImplementedException();
		}

		public void EmitWriteLine(LocalBuilder localBuilder)
		{
			throw new NotImplementedException();
		}

		private void EmitWriteLineHelper(Type type)
		{
			throw new NotImplementedException();
		}

		public void EmitWriteLine(string text)
		{
			Emit(OpCodes.Ldstr, text);
			Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
		}

		public void EndScope()
		{
			scope.endOffset = code.Position;
			scope = scope.parent;
		}

		public void MarkLabel(Label loc)
		{
			Debug.Assert(stackHeight == -1 || labelStackHeight[loc.Index] == -1 || stackHeight == labelStackHeight[loc.Index]);
			labels[loc.Index] = code.Position;
			if (labelStackHeight[loc.Index] == -1)
			{
				if (stackHeight == -1)
				{
					// we're at a location that can only be reached by a backward branch,
					// so according to the "backward branch constraint" that must mean the stack is empty
					stackHeight = 0;
					labelStackHeight[loc.Index] = 0;
				}
				else
				{
					labelStackHeight[loc.Index] = stackHeight;
				}
			}
			else
			{
				Debug.Assert(stackHeight == -1 || stackHeight == labelStackHeight[loc.Index]);
				stackHeight = labelStackHeight[loc.Index];
			}
		}

		public void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
		{
			SequencePoint sp = new SequencePoint();
			sp.document = document;
			sp.offset = code.Position;
			sp.startLine = startLine;
			sp.startColumn = startColumn;
			sp.endLine = endLine;
			sp.endColumn = endColumn;
			sequencePoints.Add(sp);
		}

		public void ThrowException(Type excType)
		{
			Emit(OpCodes.Newobj, excType.GetConstructor(Type.EmptyTypes));
			Emit(OpCodes.Throw);
		}

		public void UsingNamespace(string usingNamespace)
		{
			throw new NotImplementedException();
		}

		internal int WriteBody()
		{
			if (moduleBuilder.symbolWriter != null)
			{
				Debug.Assert(scope != null && scope.parent == null);
				scope.endOffset = code.Position;
			}

			ResolveBranches();

			ByteBuffer bb = moduleBuilder.methodBodies;

			int rva;
			if (locals.Count == 0 && exceptions.Count == 0 && maxStack <= 8 && code.Length < 64)
			{
				rva = WriteTinyHeaderAndCode(bb);
			}
			else
			{
				rva = WriteFatHeaderAndCode(bb);
			}

			if (moduleBuilder.symbolWriter != null)
			{
				if (sequencePoints.Count != 0)
				{
					ISymbolDocumentWriter document = sequencePoints[0].document;
					int[] offsets = new int[sequencePoints.Count];
					int[] lines = new int[sequencePoints.Count];
					int[] columns = new int[sequencePoints.Count];
					int[] endLines = new int[sequencePoints.Count];
					int[] endColumns = new int[sequencePoints.Count];
					for (int i = 0; i < sequencePoints.Count; i++)
					{
						if (sequencePoints[i].document != document)
						{
							throw new NotImplementedException();
						}
						offsets[i] = sequencePoints[i].offset;
						lines[i] = sequencePoints[i].startLine;
						columns[i] = sequencePoints[i].startColumn;
						endLines[i] = sequencePoints[i].endLine;
						endColumns[i] = sequencePoints[i].endColumn;
					}
					moduleBuilder.symbolWriter.DefineSequencePoints(document, offsets, lines, columns, endLines, endColumns);
				}

				WriteScope(scope);
			}
			return rva;
		}

		private void ResolveBranches()
		{
			foreach (LabelFixup fixup in labelFixups)
			{
				// is it a switch?
				if (fixup.label == -1)
				{
					code.Position = fixup.offset;
					int count = code.GetInt32AtCurrentPosition();
					int offset = fixup.offset + 4 + 4 * count;
					code.Position += 4;
					for (int i = 0; i < count; i++)
					{
						int index = code.GetInt32AtCurrentPosition();
						code.Write(labels[index] - offset);
					}
				}
				else
				{
					code.Position = fixup.offset;
					byte size = code.GetByteAtCurrentPosition();
					int branchOffset = labels[fixup.label] - (code.Position + size);
					if (size == 1)
					{
						code.Write((byte)branchOffset);
					}
					else
					{
						code.Write(branchOffset);
					}
				}
			}
		}

		private int WriteTinyHeaderAndCode(ByteBuffer bb)
		{
			int rva = bb.Position;
			const byte CorILMethod_TinyFormat = 0x2;
			bb.Write((byte)(CorILMethod_TinyFormat | (code.Length << 2)));
			WriteCode(bb);
			return rva;
		}

		private int WriteFatHeaderAndCode(ByteBuffer bb)
		{
			// fat headers require 4-byte alignment
			bb.Align(4);
			int rva = bb.Position;

			int localVarSigTok = 0;
			if (locals.Count != 0)
			{
				const byte LOCAL_SIG = 0x07;

				ByteBuffer localVarSig = new ByteBuffer(locals.Count + 2);
				localVarSig.Write(LOCAL_SIG);
				localVarSig.WriteCompressedInt(locals.Count);
				foreach (LocalBuilder local in locals)
				{
					SignatureHelper.WriteType(moduleBuilder, localVarSig, local.LocalType);
				}

				localVarSigTok = 0x11000000 | moduleBuilder.Tables.StandAloneSig.Add(moduleBuilder.Blobs.Add(localVarSig));
			}

			const byte CorILMethod_FatFormat = 0x03;
			const byte CorILMethod_MoreSects = 0x08;
			const byte CorILMethod_InitLocals = 0x10;

			short flagsAndSize = (short)(CorILMethod_FatFormat | CorILMethod_InitLocals | (3 << 12));

			if (exceptions.Count > 0)
			{
				flagsAndSize |= CorILMethod_MoreSects;
			}

			bb.Write(flagsAndSize);
			bb.Write(maxStack);
			bb.Write(code.Length);
			bb.Write(localVarSigTok);

			WriteCode(bb);

			if (exceptions.Count > 0)
			{
				bb.Align(4);

				bool fat = false;
				foreach (ExceptionBlock block in exceptions)
				{
					if (block.tryOffset > 65535 || block.tryLength > 255 || block.handlerOffset > 65535 || block.handlerLength > 255)
					{
						fat = true;
						break;
					}
				}
				exceptions.Sort(exceptions[0]);
				if (exceptions.Count * 12 + 4 > 255)
				{
					fat = true;
				}
				const byte CorILMethod_Sect_EHTable = 0x1;
				const byte CorILMethod_Sect_FatFormat = 0x40;
				const short COR_ILEXCEPTION_CLAUSE_EXCEPTION = 0x0000;
				const short COR_ILEXCEPTION_CLAUSE_FINALLY = 0x0002;
				const short COR_ILEXCEPTION_CLAUSE_FAULT = 0x0004;

				if (fat)
				{
					bb.Write((byte)(CorILMethod_Sect_EHTable | CorILMethod_Sect_FatFormat));
					int dataSize = exceptions.Count * 24 + 4;
					bb.Write((byte)dataSize);
					bb.Write((short)(dataSize >> 8));
					foreach (ExceptionBlock block in exceptions)
					{
						if (block.exceptionType == FAULT)
						{
							bb.Write((int)COR_ILEXCEPTION_CLAUSE_FAULT);
						}
						else if (block.exceptionType != null)
						{
							bb.Write((int)COR_ILEXCEPTION_CLAUSE_EXCEPTION);
						}
						else
						{
							bb.Write((int)COR_ILEXCEPTION_CLAUSE_FINALLY);
						}
						bb.Write(block.tryOffset);
						bb.Write(block.tryLength);
						bb.Write(block.handlerOffset);
						bb.Write(block.handlerLength);
						if (block.exceptionType != null)
						{
							bb.Write(moduleBuilder.GetTypeToken(block.exceptionType).Token);
						}
						else
						{
							bb.Write(0);
						}
					}
				}
				else
				{
					bb.Write(CorILMethod_Sect_EHTable);
					bb.Write((byte)(exceptions.Count * 12 + 4));
					bb.Write((short)0);
					foreach (ExceptionBlock block in exceptions)
					{
						if (block.exceptionType == FAULT)
						{
							bb.Write(COR_ILEXCEPTION_CLAUSE_FAULT);
						}
						else if (block.exceptionType != null)
						{
							bb.Write(COR_ILEXCEPTION_CLAUSE_EXCEPTION);
						}
						else
						{
							bb.Write(COR_ILEXCEPTION_CLAUSE_FINALLY);
						}
						bb.Write((short)block.tryOffset);
						bb.Write((byte)block.tryLength);
						bb.Write((short)block.handlerOffset);
						bb.Write((byte)block.handlerLength);
						if (block.exceptionType != null)
						{
							bb.Write(moduleBuilder.GetTypeToken(block.exceptionType).Token);
						}
						else
						{
							bb.Write(0);
						}
					}
				}
			}
			return rva;
		}

		private void WriteCode(ByteBuffer bb)
		{
			int codeOffset = bb.Position;
			foreach (int fixup in this.tokenFixups)
			{
				moduleBuilder.tokenFixupOffsets.Add(fixup + codeOffset);
			}
			bb.Write(code);
		}

		private void WriteScope(Scope scope)
		{
			moduleBuilder.symbolWriter.OpenScope(scope.startOffset);
			ByteBuffer localVarSig = new ByteBuffer(6);
			foreach (LocalBuilder local in scope.locals)
			{
				if (local.name != null)
				{
					localVarSig.Clear();
					SignatureHelper.WriteType(moduleBuilder, localVarSig, local.LocalType);
					moduleBuilder.symbolWriter.DefineLocalVariable(local.name, 0, localVarSig.ToArray(), SymAddressKind.ILOffset, local.LocalIndex, 0, 0, scope.startOffset, scope.endOffset);
				}
			}
			foreach (Scope child in scope.children)
			{
				WriteScope(child);
			}
			moduleBuilder.symbolWriter.CloseScope(scope.endOffset);
		}
	}
}
