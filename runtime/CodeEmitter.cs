/*
  Copyright (C) 2002, 2004, 2005, 2006, 2008, 2009 Jeroen Frijters

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
using System.Runtime.InteropServices;
using System.Diagnostics.SymbolStore;
using System.Diagnostics;

namespace IKVM.Internal
{
	sealed class CodeEmitterLabel
	{
		private Label label;
		private int offset = -1;

		internal CodeEmitterLabel(Label label)
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

		internal void Mark(ILGenerator ilgen)
		{
			ilgen.MarkLabel(label);
		}
	}

	sealed class CodeEmitterLocal
	{
		private Type type;
		private string name;
		private LocalBuilder local;

		internal CodeEmitterLocal(Type type)
		{
			this.type = type;
		}

		internal Type LocalType
		{
			get { return type; }
		}

		internal void SetLocalSymInfo(string name)
		{
			this.name = name;
		}

		internal int __LocalIndex
		{
			get { return local == null ? 0xFFFF : local.LocalIndex; }
		}

		internal void Emit(ILGenerator ilgen, OpCode opcode)
		{
			ilgen.Emit(opcode, local);
		}

		internal void Declare(ILGenerator ilgen)
		{
			local = ilgen.DeclareLocal(type);
			if (name != null)
			{
				local.SetLocalSymInfo(name);
			}
		}
	}

	sealed class CodeEmitter
	{
		private static readonly MethodInfo objectToString = Types.Object.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
		private static readonly MethodInfo verboseCastFailure = JVM.SafeGetEnvironmentVariable("IKVM_VERBOSE_CAST") == null ? null : ByteCodeHelperMethods.VerboseCastFailure;
		private ILGenerator ilgen_real;
		private bool inFinally;
		private Stack<bool> exceptionStack = new Stack<bool>();
		private IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter linenums;
		private Expr[] stackArray = new Expr[8];
		private int topOfStack;
		private CodeEmitterLocal[] tempLocals = new CodeEmitterLocal[32];
		private ISymbolDocumentWriter symbols;
		private List<OpCodeWrapper> code = new List<OpCodeWrapper>();
#if LABELCHECK
		private Dictionary<CodeEmitterLabel, System.Diagnostics.StackFrame> labels = new Dictionary<CodeEmitterLabel, System.Diagnostics.StackFrame>();
#endif

		enum CodeType
		{
			OpCode,
			Nop,
			BeginScope,
			EndScope,
			DeclareLocal,
			SequencePoint,
			LineNumber,
			Label,
			WriteLine,
			ThrowException,
			BeginExceptionBlock,
			BeginCatchBlock,
			BeginFaultBlock,
			BeginFinallyBlock,
			EndExceptionBlock,
			EndExceptionBlockFinally,
		}

		struct OpCodeWrapper
		{
			internal readonly CodeType type;
			internal readonly OpCode opcode;
			internal readonly object data;

			internal OpCodeWrapper(CodeType type, object data)
			{
				this.type = type;
				this.opcode = OpCodes.Nop;
				this.data = data;
			}

			internal OpCodeWrapper(OpCode opcode, object data)
			{
				this.type = CodeType.OpCode;
				this.opcode = opcode;
				this.data = data;
			}

			internal int Size
			{
				get
				{
					switch (type)
					{
						case CodeType.Nop:
						case CodeType.BeginScope:
						case CodeType.EndScope:
						case CodeType.DeclareLocal:
						case CodeType.LineNumber:
						case CodeType.Label:
						case CodeType.BeginExceptionBlock:
							return 0;
						case CodeType.SequencePoint:
							return 1;
						case CodeType.WriteLine:
							return 10;
						case CodeType.ThrowException:
							return 6;
						case CodeType.BeginCatchBlock:
						case CodeType.BeginFaultBlock:
						case CodeType.BeginFinallyBlock:
						case CodeType.EndExceptionBlock:
							return 5;
						case CodeType.EndExceptionBlockFinally:
							return 1;
						case CodeType.OpCode:
							if (data == null)
							{
								return opcode.Size;
							}
							else if (data is int)
							{
								return opcode.Size + 4;;
							}
							else if (data is long)
							{
								return opcode.Size + 8;
							}
							else if (data is MethodInfo)
							{
								return opcode.Size + 4;
							}
							else if (data is ConstructorInfo)
							{
								return opcode.Size + 4;
							}
							else if (data is FieldInfo)
							{
								return opcode.Size + 4;
							}
							else if (data is sbyte)
							{
								return opcode.Size + 1;
							}
							else if (data is byte)
							{
								return opcode.Size + 1;
							}
							else if (data is short)
							{
								return opcode.Size + 2;
							}
							else if (data is float)
							{
								return opcode.Size + 4;
							}
							else if (data is double)
							{
								return opcode.Size + 8;
							}
							else if (data is string)
							{
								return opcode.Size + 4;
							}
							else if (data is Type)
							{
								return opcode.Size + 4;
							}
							else if (data is CodeEmitterLocal)
							{
								int index = ((CodeEmitterLocal)data).__LocalIndex;
								if(index < 4 && opcode.Value != OpCodes.Ldloca.Value && opcode.Value != OpCodes.Ldloca_S.Value)
								{
									return 1;
								}
								else if(index < 256)
								{
									return 2;
								}
								else
								{
									return 4;
								}
							}
							else if (data is CodeEmitterLabel)
							{
								switch(opcode.OperandType)
								{
									case OperandType.InlineBrTarget:
										return opcode.Size + 4;
									case OperandType.ShortInlineBrTarget:
										return opcode.Size + 1;
									default:
										throw new InvalidOperationException();
								}
							}
							else if (data is CodeEmitterLabel[])
							{
								return 5 + ((CodeEmitterLabel[])data).Length * 4;
							}
							else if (data is CalliWrapper)
							{
								return 5;
							}
							else
							{
								throw new InvalidOperationException();
							}
						default:
							throw new InvalidOperationException();
					}
				}
			}
		}

		sealed class CalliWrapper
		{
			internal readonly CallingConvention unmanagedCallConv;
			internal readonly Type returnType;
			internal readonly Type[] parameterTypes;

			internal CalliWrapper(CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
			{
				this.unmanagedCallConv = unmanagedCallConv;
				this.returnType = returnType;
				this.parameterTypes = parameterTypes == null ? null : (Type[])parameterTypes.Clone();
			}
		}

		internal static CodeEmitter Create(MethodBuilder mb)
		{
			return new CodeEmitter(mb.GetILGenerator());
		}

		internal static CodeEmitter Create(ConstructorBuilder cb)
		{
			return new CodeEmitter(cb.GetILGenerator());
		}

#if !STATIC_COMPILER
		internal static CodeEmitter Create(DynamicMethod dm)
		{
			return new CodeEmitter(dm.GetILGenerator());
		}
#endif

		private CodeEmitter(ILGenerator ilgen)
		{
#if STATIC_COMPILER
			ilgen.__CleverExceptionBlockAssistance();
#endif
			this.ilgen_real = ilgen;
		}

		private void EmitPseudoOpCode(CodeType type, object data)
		{
			code.Add(new OpCodeWrapper(type, data));
		}

		private void EmitOpCode(OpCode opcode, object arg)
		{
			code.Add(new OpCodeWrapper(opcode, arg));
		}

		private void RealEmitPseudoOpCode(int ilOffset, CodeType type, object data)
		{
			switch (type)
			{
				case CodeType.Nop:
					break;
				case CodeType.BeginScope:
					ilgen_real.BeginScope();
					break;
				case CodeType.EndScope:
					ilgen_real.EndScope();
					break;
				case CodeType.DeclareLocal:
					((CodeEmitterLocal)data).Declare(ilgen_real);
					break;
				case CodeType.SequencePoint:
					ilgen_real.MarkSequencePoint(symbols, (int)data, 0, (int)data + 1, 0);
					// we emit a nop to make sure we always have an instruction associated with the sequence point
					ilgen_real.Emit(OpCodes.Nop);
					break;
				case CodeType.LineNumber:
					if (linenums == null)
					{
						linenums = new IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter(32);
					}
					linenums.AddMapping(ilOffset, (int)data);
					break;
				case CodeType.Label:
					((CodeEmitterLabel)data).Mark(ilgen_real);
					break;
				case CodeType.WriteLine:
					ilgen_real.EmitWriteLine((string)data);
					break;
				case CodeType.ThrowException:
					ilgen_real.ThrowException((Type)data);
					break;
				case CodeType.BeginExceptionBlock:
					ilgen_real.BeginExceptionBlock();
					break;
				case CodeType.BeginCatchBlock:
					ilgen_real.BeginCatchBlock((Type)data);
					break;
				case CodeType.BeginFaultBlock:
					ilgen_real.BeginFaultBlock();
					break;
				case CodeType.BeginFinallyBlock:
					ilgen_real.BeginFinallyBlock();
					break;
				case CodeType.EndExceptionBlockFinally:
					ilgen_real.EndExceptionBlock();
					break;
				case CodeType.EndExceptionBlock:
					ilgen_real.EndExceptionBlock();
					break;
				default:
					throw new InvalidOperationException();
			}
		}

		private void RealEmitOpCode(OpCode opcode, object arg)
		{
			if (arg == null)
			{
				ilgen_real.Emit(opcode);
			}
			else if (arg is int)
			{
				ilgen_real.Emit(opcode, (int)arg);
			}
			else if (arg is long)
			{
				ilgen_real.Emit(opcode, (long)arg);
			}
			else if (arg is MethodInfo)
			{
				ilgen_real.Emit(opcode, (MethodInfo)arg);
			}
			else if (arg is ConstructorInfo)
			{
				ilgen_real.Emit(opcode, (ConstructorInfo)arg);
			}
			else if (arg is FieldInfo)
			{
				ilgen_real.Emit(opcode, (FieldInfo)arg);
			}
			else if (arg is sbyte)
			{
				ilgen_real.Emit(opcode, (sbyte)arg);
			}
			else if (arg is byte)
			{
				ilgen_real.Emit(opcode, (byte)arg);
			}
			else if (arg is short)
			{
				ilgen_real.Emit(opcode, (short)arg);
			}
			else if (arg is float)
			{
				ilgen_real.Emit(opcode, (float)arg);
			}
			else if (arg is double)
			{
				ilgen_real.Emit(opcode, (double)arg);
			}
			else if (arg is string)
			{
				ilgen_real.Emit(opcode, (string)arg);
			}
			else if (arg is Type)
			{
				ilgen_real.Emit(opcode, (Type)arg);
			}
			else if (arg is CodeEmitterLocal)
			{
				CodeEmitterLocal local = (CodeEmitterLocal)arg;
				local.Emit(ilgen_real, opcode);
			}
			else if (arg is CodeEmitterLabel)
			{
				CodeEmitterLabel label = (CodeEmitterLabel)arg;
				ilgen_real.Emit(opcode, label.Label);
			}
			else if (arg is CodeEmitterLabel[])
			{
				CodeEmitterLabel[] labels = (CodeEmitterLabel[])arg;
				Label[] real = new Label[labels.Length];
				for (int i = 0; i < labels.Length; i++)
				{
					real[i] = labels[i].Label;
				}
				ilgen_real.Emit(opcode, real);
			}
			else if (arg is CalliWrapper)
			{
				CalliWrapper args = (CalliWrapper)arg;
				ilgen_real.EmitCalli(opcode, args.unmanagedCallConv, args.returnType, args.parameterTypes);
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		private void PushStack(Expr expr)
		{
			Debug.Assert(expr != null);
			if (topOfStack == stackArray.Length)
			{
				Array.Resize(ref stackArray, stackArray.Length * 2);
			}
			stackArray[topOfStack++] = expr;
		}

		private void PushStackMayBeNull(Expr expr)
		{
			if (expr != null)
			{
				PushStack(expr);
			}
		}

		private Expr PopStack()
		{
			if (topOfStack == 0)
			{
				return null;
			}
			return stackArray[--topOfStack];
		}

		private Expr PeekStack()
		{
			if (topOfStack == 0)
			{
				return null;
			}
			return stackArray[topOfStack - 1];
		}

		private void RemoveJumpNext()
		{
			for (int i = 1; i < code.Count; i++)
			{
				if (code[i].type == CodeType.Label
					&& code[i - 1].type == CodeType.OpCode
					&& code[i - 1].opcode == OpCodes.Br
					&& code[i - 1].data == code[i].data)
				{
					code.RemoveAt(i - 1);
					i--;
				}
			}
		}

		private void AnnihilatePops()
		{
			for (int i = 1; i < code.Count; i++)
			{
				if (code[i].opcode == OpCodes.Pop
					&& IsSideEffectFreePush(i - 1))
				{
					code.RemoveRange(i - 1, 2);
					i -= 2;
				}
			}
		}

		private bool IsSideEffectFreePush(int index)
		{
			if (code[index].opcode == OpCodes.Ldstr)
			{
				return true;
			}
			else if (code[index].opcode == OpCodes.Ldnull)
			{
				return true;
			}
			else if (code[index].opcode == OpCodes.Ldsfld)
			{
				// Here we are considering BeforeFieldInit to mean that we really don't care about
				// when the type is initialized (which is what we mean in the rest of the IKVM code as well)
				// but it is good to point it out here because strictly speaking we're violating the
				// BeforeFieldInit contract here by considering dummy loads not to be field accesses.
				FieldInfo field = code[index].data as FieldInfo;
				if (field != null && (field.DeclaringType.Attributes & TypeAttributes.BeforeFieldInit) != 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		private void OptimizeBranchSizes()
		{
			int offset = 0;
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].type == CodeType.Label)
				{
					((CodeEmitterLabel)code[i].data).Offset = offset;
				}
				offset += code[i].Size;
			}
			offset = 0;
			for (int i = 0; i < code.Count; i++)
			{
				int prevOffset = offset;
				offset += code[i].Size;
				CodeEmitterLabel label = code[i].data as CodeEmitterLabel;
				if (label != null && code[i].opcode.OperandType == OperandType.InlineBrTarget)
				{
					int diff = label.Offset - (prevOffset + code[i].opcode.Size + 1);
					if (-128 <= diff && diff <= 127)
					{
						OpCode opcode = code[i].opcode;
						if (opcode == OpCodes.Brtrue)
						{
							opcode = OpCodes.Brtrue_S;
						}
						else if (opcode == OpCodes.Brfalse)
						{
							opcode = OpCodes.Brfalse_S;
						}
						else if (opcode == OpCodes.Br)
						{
							opcode = OpCodes.Br_S;
						}
						else if (opcode == OpCodes.Beq)
						{
							opcode = OpCodes.Beq_S;
						}
						else if (opcode == OpCodes.Bne_Un)
						{
							opcode = OpCodes.Bne_Un_S;
						}
						else if (opcode == OpCodes.Ble)
						{
							opcode = OpCodes.Ble_S;
						}
						else if (opcode == OpCodes.Ble_Un)
						{
							opcode = OpCodes.Ble_Un_S;
						}
						else if (opcode == OpCodes.Blt)
						{
							opcode = OpCodes.Blt_S;
						}
						else if (opcode == OpCodes.Blt_Un)
						{
							opcode = OpCodes.Blt_Un_S;
						}
						else if (opcode == OpCodes.Bge)
						{
							opcode = OpCodes.Bge_S;
						}
						else if (opcode == OpCodes.Bge_Un)
						{
							opcode = OpCodes.Bge_Un_S;
						}
						else if (opcode == OpCodes.Bgt)
						{
							opcode = OpCodes.Bgt_S;
						}
						else if (opcode == OpCodes.Bgt_Un)
						{
							opcode = OpCodes.Bgt_Un_S;
						}
						else if (opcode == OpCodes.Leave)
						{
							opcode = OpCodes.Leave_S;
						}
						code[i] = new OpCodeWrapper(opcode, code[i].data);
					}
				}
			}
		}

		internal void DoEmit()
		{
			RemoveJumpNext();
			AnnihilatePops();
			OptimizeBranchSizes();
			int ilOffset = 0;
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].type == CodeType.OpCode)
				{
					RealEmitOpCode(code[i].opcode, code[i].data);
				}
				else
				{
					RealEmitPseudoOpCode(ilOffset, code[i].type, code[i].data);
				}
#if STATIC_COMPILER || NET_4_0
				ilOffset = ilgen_real.ILOffset;
#else
				ilOffset += code[i].Size;
#endif
			}
		}

		internal void DefineSymbolDocument(ModuleBuilder module, string url, Guid language, Guid languageVendor, Guid documentType)
		{
			symbols = module.DefineDocument(url, language, languageVendor, documentType);
		}

		internal CodeEmitterLocal UnsafeAllocTempLocal(Type type)
		{
			int free = -1;
			for (int i = 0; i < tempLocals.Length; i++)
			{
				CodeEmitterLocal lb = tempLocals[i];
				if (lb == null)
				{
					if (free == -1)
					{
						free = i;
					}
				}
				else if (lb.LocalType == type)
				{
					return lb;
				}
			}
			CodeEmitterLocal lb1 = DeclareLocal(type);
			if (free != -1)
			{
				tempLocals[free] = lb1;
			}
			return lb1;
		}

		internal CodeEmitterLocal AllocTempLocal(Type type)
		{
			for (int i = 0; i < tempLocals.Length; i++)
			{
				CodeEmitterLocal lb = tempLocals[i];
				if (lb != null && lb.LocalType == type)
				{
					tempLocals[i] = null;
					return lb;
				}
			}
			return DeclareLocal(type);
		}

		internal void ReleaseTempLocal(CodeEmitterLocal lb)
		{
			for (int i = 0; i < tempLocals.Length; i++)
			{
				if (tempLocals[i] == null)
				{
					tempLocals[i] = lb;
					break;
				}
			}
		}

		internal void BeginCatchBlock(Type exceptionType)
		{
			LazyGen();
			EmitPseudoOpCode(CodeType.BeginCatchBlock, exceptionType);
		}

		internal void BeginExceptionBlock()
		{
			LazyGen();
			exceptionStack.Push(inFinally);
			inFinally = false;
			EmitPseudoOpCode(CodeType.BeginExceptionBlock, null);
		}

		internal void BeginFaultBlock()
		{
			LazyGen();
			inFinally = true;
			EmitPseudoOpCode(CodeType.BeginFaultBlock, null);
		}

		internal void BeginFinallyBlock()
		{
			LazyGen();
			inFinally = true;
			EmitPseudoOpCode(CodeType.BeginFinallyBlock, null);
		}

		internal void BeginScope()
		{
			LazyGen();
			EmitPseudoOpCode(CodeType.BeginScope, null);
		}

		internal CodeEmitterLocal DeclareLocal(Type localType)
		{
			CodeEmitterLocal local = new CodeEmitterLocal(localType);
			EmitPseudoOpCode(CodeType.DeclareLocal, local);
			return local;
		}

		internal CodeEmitterLabel DefineLabel()
		{
			CodeEmitterLabel label = new CodeEmitterLabel(ilgen_real.DefineLabel());
#if LABELCHECK
			labels.Add(label, new System.Diagnostics.StackFrame(1, true));
#endif
			return label;
		}

		internal void Emit(OpCode opcode)
		{
			LazyGen();
			EmitOpCode(opcode, null);
		}

		internal void Emit(OpCode opcode, byte arg)
		{
			LazyGen();
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, ConstructorInfo con)
		{
			LazyGen();
			EmitOpCode(opcode, con);
		}

		internal void Emit(OpCode opcode, double arg)
		{
			LazyGen();
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, FieldInfo field)
		{
			LazyGen();
			EmitOpCode(opcode, field);
		}

		internal void Emit(OpCode opcode, short arg)
		{
			LazyGen();
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, int arg)
		{
			LazyGen();
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, long arg)
		{
			LazyGen();
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, CodeEmitterLabel label)
		{
			LazyGen();
			EmitOpCode(opcode, label);
		}

		internal void Emit(OpCode opcode, CodeEmitterLabel[] labels)
		{
			LazyGen();
			EmitOpCode(opcode, labels);
		}

		internal void Emit(OpCode opcode, CodeEmitterLocal local)
		{
			LazyGen();
			EmitOpCode(opcode, local);
		}

		internal void Emit(OpCode opcode, MethodInfo meth)
		{
			LazyGen();
			EmitOpCode(opcode, meth);
		}

		internal void Emit(OpCode opcode, sbyte arg)
		{
			LazyGen();
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, float arg)
		{
			LazyGen();
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, string arg)
		{
			LazyGen();
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, Type cls)
		{
			LazyGen();
			EmitOpCode(opcode, cls);
		}

		internal void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			LazyGen();
			EmitOpCode(opcode, new CalliWrapper(unmanagedCallConv, returnType, parameterTypes));
		}

		internal void EmitWriteLine(string value)
		{
			LazyGen();
			EmitPseudoOpCode(CodeType.WriteLine, value);
		}

		internal void EndExceptionBlockNoFallThrough()
		{
			EndExceptionBlock();
#if !STATIC_COMPILER
			// HACK to keep the verifier happy we need this bogus jump
			// (because of the bogus Leave that Ref.Emit ends the try block with)
			Emit(OpCodes.Br_S, (sbyte)-2);
#endif
		}

		internal void EndExceptionBlock()
		{
			LazyGen();
			if(inFinally)
			{
				EmitPseudoOpCode(CodeType.EndExceptionBlockFinally, null);
			}
			else
			{
				EmitPseudoOpCode(CodeType.EndExceptionBlock, null);
			}
			inFinally = exceptionStack.Pop();
		}

		internal void EndScope()
		{
			LazyGen();
			EmitPseudoOpCode(CodeType.EndScope, null);
		}

		internal void MarkLabel(CodeEmitterLabel loc)
		{
			LazyGen();
#if LABELCHECK
			labels.Remove(loc);
#endif
			EmitPseudoOpCode(CodeType.Label, loc);
		}

		internal void ThrowException(Type excType)
		{
			LazyGen();
			EmitPseudoOpCode(CodeType.ThrowException, excType);
		}

		internal void SetLineNumber(ushort line)
		{
			if (symbols != null)
			{
				LazyGen();
				EmitPseudoOpCode(CodeType.SequencePoint, (int)line);
			}
			// we only add a line number mapping if the stack is empty because the CLR JIT only generates native to IL mappings
			// for locations where the stack is empty and we don't want to needlessly flush the stack
			if (topOfStack == 0)
			{
				EmitPseudoOpCode(CodeType.LineNumber, (int)line);
			}
		}

		internal byte[] GetLineNumberTable()
		{
			return linenums == null ? null : linenums.ToArray();
		}

#if STATIC_COMPILER
		internal void EmitLineNumberTable(MethodBase mb)
		{
			if(linenums != null)
			{
				AttributeHelper.SetLineNumberTable(mb, linenums);
			}
		}
#endif // STATIC_COMPILER

		internal void EmitThrow(string dottedClassName)
		{
			TypeWrapper exception = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(dottedClassName);
			MethodWrapper mw = exception.GetMethodWrapper("<init>", "()V", false);
			mw.Link();
			mw.EmitNewobj(this);
			Emit(OpCodes.Throw);
		}

		internal void EmitThrow(string dottedClassName, string message)
		{
			TypeWrapper exception = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(dottedClassName);
			Emit(OpCodes.Ldstr, message);
			MethodWrapper mw = exception.GetMethodWrapper("<init>", "(Ljava.lang.String;)V", false);
			mw.Link();
			mw.EmitNewobj(this);
			Emit(OpCodes.Throw);
		}

		internal void EmitNullCheck()
		{
			// I think this is the most efficient way to generate a NullReferenceException if the reference is null
			Emit(OpCodes.Ldvirtftn, objectToString);
			Emit(OpCodes.Pop);
		}

		internal void EmitCastclass(Type type)
		{
			if (verboseCastFailure != null)
			{
				CodeEmitterLocal lb = DeclareLocal(Types.Object);
				Emit(OpCodes.Stloc, lb);
				Emit(OpCodes.Ldloc, lb);
				Emit(OpCodes.Isinst, type);
				Emit(OpCodes.Dup);
				CodeEmitterLabel ok = DefineLabel();
				Emit(OpCodes.Brtrue_S, ok);
				Emit(OpCodes.Ldloc, lb);
				Emit(OpCodes.Brfalse_S, ok);	// handle null
				Emit(OpCodes.Ldtoken, type);
				Emit(OpCodes.Ldloc, lb);
				Emit(OpCodes.Call, verboseCastFailure);
				MarkLabel(ok);
			}
			else
			{
				Emit(OpCodes.Castclass, type);
			}
		}

		// This is basically the same as Castclass, except that it
		// throws an IncompatibleClassChangeError on failure.
		internal void EmitAssertType(Type type)
		{
			CodeEmitterLocal lb = DeclareLocal(Types.Object);
			Emit(OpCodes.Stloc, lb);
			Emit(OpCodes.Ldloc, lb);
			Emit(OpCodes.Isinst, type);
			Emit(OpCodes.Dup);
			CodeEmitterLabel ok = DefineLabel();
			Emit(OpCodes.Brtrue_S, ok);
			Emit(OpCodes.Ldloc, lb);
			Emit(OpCodes.Brfalse_S, ok);	// handle null
			EmitThrow("java.lang.IncompatibleClassChangeError");
			MarkLabel(ok);
		}

		internal void LazyEmitPop()
		{
			Emit(OpCodes.Pop);
		}

		internal void LazyEmitLoadClass(TypeWrapper type)
		{
			PushStack(new ClassLiteralExpr(type));
		}

		internal void LazyEmitBox(Type type)
		{
			PushStack(new BoxExpr(PopStack(), type));
		}

		internal void LazyEmitUnbox(Type type)
		{
			BoxExpr box = PeekStack() as BoxExpr;
			if(box != null)
			{
				PopStack();
				PushStack(new BoxUnboxExpr(box.Expr, type));
			}
			else
			{
				PushStack(new UnboxExpr(PopStack(), type));
			}
		}

		internal void LazyEmitLdobj(Type type)
		{
			BoxUnboxExpr boxunbox = PeekStack() as BoxUnboxExpr;
			if(boxunbox != null)
			{
				PopStack();
				// box/unbox+ldobj annihilate each other
				PushStackMayBeNull(boxunbox.Expr);
			}
			else
			{
				PushStack(new LdobjExpr(PopStack(), type));
			}
		}

		internal void LazyEmitUnboxSpecial(Type type)
		{
			BoxExpr box = PeekStack() as BoxExpr;
			if(box != null)
			{
				PopStack();
				// the unbox and lazy box cancel each other out
				PushStackMayBeNull(box.Expr);
			}
			else
			{
				// NOTE if the reference is null, we treat it as a default instance of the value type.
				Emit(OpCodes.Dup);
				CodeEmitterLabel label1 = DefineLabel();
				Emit(OpCodes.Brtrue_S, label1);
				Emit(OpCodes.Pop);
				Emit(OpCodes.Ldloc, DeclareLocal(type));
				CodeEmitterLabel label2 = DefineLabel();
				Emit(OpCodes.Br_S, label2);
				MarkLabel(label1);
				Emit(OpCodes.Unbox, type);
				Emit(OpCodes.Ldobj, type);
				MarkLabel(label2);
			}
		}

		internal void LazyEmitLdnull()
		{
			PushStack(new NullExpr());
		}

		internal void LazyEmitLdc_I4(int i)
		{
			PushStack(new ConstIntExpr(i));
		}

		internal void LazyEmitLdc_I8(long l)
		{
			PushStack(new ConstLongExpr(l));
		}

		internal void LazyEmitLdstr(string str)
		{
			PushStack(new ConstStringExpr(str));
		}

		internal void LazyEmit_idiv()
		{
			// we need to special case dividing by -1, because the CLR div instruction
			// throws an OverflowException when dividing Int32.MinValue by -1, and
			// Java just silently overflows
			ConstIntExpr v = PeekStack() as ConstIntExpr;
			if(v != null)
			{
				if(v.i == -1)
				{
					PopStack();
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
				CodeEmitterLabel label = DefineLabel();
				Emit(OpCodes.Bne_Un_S, label);
				Emit(OpCodes.Pop);
				Emit(OpCodes.Neg);
				CodeEmitterLabel label2 = DefineLabel();
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
			ConstLongExpr v = PeekStack() as ConstLongExpr;
			if(v != null)
			{
				if(v.l == -1)
				{
					PopStack();
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
				CodeEmitterLabel label = DefineLabel();
				Emit(OpCodes.Bne_Un_S, label);
				Emit(OpCodes.Pop);
				Emit(OpCodes.Neg);
				CodeEmitterLabel label2 = DefineLabel();
				Emit(OpCodes.Br_S, label2);
				MarkLabel(label);
				Emit(OpCodes.Div);
				MarkLabel(label2);
			}
		}

		internal void LazyEmit_instanceof(Type type)
		{
			PushStack(new InstanceOfExpr(type));
		}

		internal void LazyEmit_ifeq(CodeEmitterLabel label)
		{
			LazyEmit_if_ne_eq(label, false);
		}

		internal void LazyEmit_ifne(CodeEmitterLabel label)
		{
			LazyEmit_if_ne_eq(label, true);
		}

		private void LazyEmit_if_ne_eq(CodeEmitterLabel label, bool brtrue)
		{
			InstanceOfExpr instanceof = PeekStack() as InstanceOfExpr;
			if (instanceof != null)
			{
				PopStack();
				Emit(OpCodes.Isinst, instanceof.Type);
			}
			else
			{
				CmpExpr cmp = PeekStack() as CmpExpr;
				if (cmp != null)
				{
					PopStack();
					Emit(brtrue ? OpCodes.Bne_Un : OpCodes.Beq, label);
					return;
				}
			}
			Emit(brtrue ? OpCodes.Brtrue : OpCodes.Brfalse, label);
		}

		internal enum Comparison
		{
			LessOrEqual,
			LessThan,
			GreaterOrEqual,
			GreaterThan
		}

		private void EmitBcc(Comparison comp, CodeEmitterLabel label)
		{
			switch (comp)
			{
				case Comparison.LessOrEqual:
					Emit(OpCodes.Ble, label);
					break;
				case Comparison.LessThan:
					Emit(OpCodes.Blt, label);
					break;
				case Comparison.GreaterOrEqual:
					Emit(OpCodes.Bge, label);
					break;
				case Comparison.GreaterThan:
					Emit(OpCodes.Bgt, label);
					break;
			}
		}

		internal void LazyEmit_if_le_lt_ge_gt(Comparison comp, CodeEmitterLabel label)
		{
			CmpExpr cmp = PeekStack() as CmpExpr;
			if (cmp != null)
			{
				PopStack();
				cmp.EmitBcc(this, comp, label);
			}
			else
			{
				Emit(OpCodes.Ldc_I4_0);
				EmitBcc(comp, label);
			}
		}

		internal void LazyEmit_lcmp()
		{
			PushStack(new LCmpExpr());
		}

		internal void LazyEmit_fcmpl()
		{
			PushStack(new FCmplExpr());
		}

		internal void LazyEmit_fcmpg()
		{
			PushStack(new FCmpgExpr());
		}

		internal void LazyEmit_dcmpl()
		{
			PushStack(new DCmplExpr());
		}

		internal void LazyEmit_dcmpg()
		{
			PushStack(new DCmpgExpr());
		}

		internal void LazyEmitAnd_I4(int v2)
		{
			ConstIntExpr v1 = PeekStack() as ConstIntExpr;
			if (v1 != null)
			{
				PopStack();
				LazyEmitLdc_I4(v1.i & v2);
			}
			else
			{
				LazyEmitLdc_I4(v2);
				Emit(OpCodes.And);
			}
		}

		internal void LazyEmit_baload()
		{
			PushStack(new ByteArrayLoadExpr());
		}

		private bool Match<T1, T2>(out T1 t1, out T2 t2)
			where T1 : Expr
			where T2 : Expr
		{
			if (topOfStack < 2)
			{
				t1 = null;
				t2 = null;
				return false;
			}
			t1 = stackArray[topOfStack - 1] as T1;
			t2 = stackArray[topOfStack - 2] as T2;
			return t1 != null && t2 != null;
		}

		internal void LazyEmit_iand()
		{
			ConstIntExpr v1;
			ByteArrayLoadExpr v2;
			if (Match(out v1, out v2) && v1.i == 0xFF)
			{
				PopStack();
				PopStack();
				Emit(OpCodes.Ldelem_U1);
			}
			else
			{
				Emit(OpCodes.And);
			}
		}

		internal void LazyEmit_land()
		{
			ConstLongExpr v1;
			ConvertLong v2;
			if (Match(out v1, out v2) && v1.l == 0xFF && v2.Expr is ByteArrayLoadExpr)
			{
				PopStack();
				PopStack();
				Emit(OpCodes.Ldelem_U1);
				Emit(OpCodes.Conv_I8);
			}
			else
			{
				Emit(OpCodes.And);
			}
		}

		internal void LazyEmit_i2l()
		{
			PushStack(new ConvertLong(PopStack()));
		}

		internal string PopLazyLdstr()
		{
			ConstStringExpr str = PeekStack() as ConstStringExpr;
			if(str != null)
			{
				PopStack();
				return str.str;
			}
			return null;
		}

		internal TypeWrapper PeekLazyClassLiteral()
		{
			ClassLiteralExpr lit = PeekStack() as ClassLiteralExpr;
			if (lit != null)
			{
				return lit.Type;
			}
			return null;
		}

		private void LazyGen()
		{
			int len = topOfStack;
			topOfStack = 0;
			for(int i = 0; i < len; i++)
			{
				stackArray[i].Emit(this);
			}
		}

		internal void CheckLabels()
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
			internal virtual bool HasSideEffect { get { return false; } }

			internal virtual bool IsIncomplete { get { return false; } }

			internal abstract void Emit(CodeEmitter ilgen);
		}

		abstract class UnaryExpr : Expr
		{
			internal readonly Expr Expr;

			protected UnaryExpr(Expr expr)
			{
				this.Expr = expr;
			}

			internal sealed override bool IsIncomplete
			{
				get { return Expr == null; }
			}

			internal override bool HasSideEffect
			{
				get { return Expr != null && Expr.HasSideEffect; }
			}

			internal override void Emit(CodeEmitter ilgen)
			{
				if (Expr != null)
				{
					Expr.Emit(ilgen);
				}
			}
		}

		abstract class ExprWithExprAndType : UnaryExpr
		{
			internal readonly Type Type;

			protected ExprWithExprAndType(Expr expr, Type type)
				: base(expr)
			{
				this.Type = type;
			}
		}

		sealed class BoxExpr : ExprWithExprAndType
		{
			internal BoxExpr(Expr expr, Type type)
				: base(expr, type)
			{
			}

			internal override void Emit(CodeEmitter ilgen)
			{
				base.Emit(ilgen);
				ilgen.Emit(OpCodes.Box, Type);
			}
		}

		sealed class UnboxExpr : ExprWithExprAndType
		{
			internal UnboxExpr(Expr expr, Type type)
				: base(expr, type)
			{
			}

			internal override void Emit(CodeEmitter ilgen)
			{
				base.Emit(ilgen);
				ilgen.Emit(OpCodes.Unbox, Type);
			}
		}

		sealed class BoxUnboxExpr : ExprWithExprAndType
		{
			internal BoxUnboxExpr(Expr expr, Type type)
				: base(expr, type)
			{
			}

			internal override void Emit(CodeEmitter ilgen)
			{
				base.Emit(ilgen);
				// unbox leaves a pointer to the value of the stack (instead of the value)
				// so we have to copy the value into a local variable and load the address
				// of the local onto the stack
				CodeEmitterLocal local = ilgen.DeclareLocal(Type);
				ilgen.Emit(OpCodes.Stloc, local);
				ilgen.Emit(OpCodes.Ldloca, local);
			}
		}

		sealed class LdobjExpr : ExprWithExprAndType
		{
			internal LdobjExpr(Expr expr, Type type)
				: base(expr, type)
			{
			}

			internal override void Emit(CodeEmitter ilgen)
			{
				base.Emit(ilgen);
				ilgen.Emit(OpCodes.Ldobj, Type);
			}
		}

		sealed class NullExpr : Expr
		{
			internal override void Emit(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Ldnull);
			}
		}

		sealed class ConstIntExpr : Expr
		{
			internal readonly int i;

			internal ConstIntExpr(int i)
			{
				this.i = i;
			}

			internal override void Emit(CodeEmitter ilgen)
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

		sealed class ConstLongExpr : Expr
		{
			internal readonly long l;

			internal ConstLongExpr(long l)
			{
				this.l = l;
			}

			internal override void Emit(CodeEmitter ilgen)
			{
				switch (l)
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
						if (l >= -2147483648L && l <= 4294967295L)
						{
							if (l >= -128 && l <= 127)
							{
								ilgen.Emit(OpCodes.Ldc_I4_S, (sbyte)l);
							}
							else
							{
								ilgen.Emit(OpCodes.Ldc_I4, (int)l);
							}
							if (l < 0)
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

		sealed class ConstStringExpr : Expr
		{
			internal readonly string str;

			internal ConstStringExpr(string str)
			{
				this.str = str;
			}

			internal override void Emit(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Ldstr, str);
			}
		}

		sealed class InstanceOfExpr : Expr
		{
			internal readonly Type Type;

			internal InstanceOfExpr(Type type)
			{
				this.Type = type;
			}

			internal override bool IsIncomplete
			{
				get
				{
					return true;
				}
			}

			internal override void Emit(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Isinst, this.Type);
				ilgen.Emit(OpCodes.Ldnull);
				ilgen.Emit(OpCodes.Cgt_Un);
			}
		}

		abstract class CmpExpr : Expr
		{
			internal CmpExpr()
			{
			}

			internal override bool IsIncomplete
			{
				get
				{
					return true;
				}
			}

			internal abstract void EmitBcc(CodeEmitter ilgen, Comparison comp, CodeEmitterLabel label);
		}

		sealed class LCmpExpr : CmpExpr
		{
			internal LCmpExpr()
			{
			}

			internal sealed override void Emit(CodeEmitter ilgen)
			{
				CodeEmitterLocal value1 = ilgen.AllocTempLocal(Types.Int64);
				CodeEmitterLocal value2 = ilgen.AllocTempLocal(Types.Int64);
				ilgen.Emit(OpCodes.Stloc, value2);
				ilgen.Emit(OpCodes.Stloc, value1);
				ilgen.Emit(OpCodes.Ldloc, value1);
				ilgen.Emit(OpCodes.Ldloc, value2);
				ilgen.Emit(OpCodes.Cgt);
				ilgen.Emit(OpCodes.Ldloc, value1);
				ilgen.Emit(OpCodes.Ldloc, value2);
				ilgen.Emit(OpCodes.Clt);
				ilgen.Emit(OpCodes.Sub);
				ilgen.ReleaseTempLocal(value2);
				ilgen.ReleaseTempLocal(value1);
			}

			internal sealed override void EmitBcc(CodeEmitter ilgen, Comparison comp, CodeEmitterLabel label)
			{
				ilgen.EmitBcc(comp, label);
			}
		}

		class FCmplExpr : CmpExpr
		{
			protected virtual Type FloatOrDouble()
			{
				return Types.Single;
			}

			internal sealed override void Emit(CodeEmitter ilgen)
			{
				CodeEmitterLocal value1 = ilgen.AllocTempLocal(FloatOrDouble());
				CodeEmitterLocal value2 = ilgen.AllocTempLocal(FloatOrDouble());
				ilgen.Emit(OpCodes.Stloc, value2);
				ilgen.Emit(OpCodes.Stloc, value1);
				ilgen.Emit(OpCodes.Ldloc, value1);
				ilgen.Emit(OpCodes.Ldloc, value2);
				ilgen.Emit(OpCodes.Cgt);
				ilgen.Emit(OpCodes.Ldloc, value1);
				ilgen.Emit(OpCodes.Ldloc, value2);
				ilgen.Emit(OpCodes.Clt_Un);
				ilgen.Emit(OpCodes.Sub);
				ilgen.ReleaseTempLocal(value1);
				ilgen.ReleaseTempLocal(value2);
			}

			internal sealed override void EmitBcc(CodeEmitter ilgen, Comparison comp, CodeEmitterLabel label)
			{
				switch (comp)
				{
					case Comparison.LessOrEqual:
						ilgen.Emit(OpCodes.Ble_Un, label);
						break;
					case Comparison.LessThan:
						ilgen.Emit(OpCodes.Blt_Un, label);
						break;
					case Comparison.GreaterOrEqual:
						ilgen.Emit(OpCodes.Bge, label);
						break;
					case Comparison.GreaterThan:
						ilgen.Emit(OpCodes.Bgt, label);
						break;
				}
			}
		}

		class FCmpgExpr : CmpExpr
		{
			protected virtual Type FloatOrDouble()
			{
				return Types.Single;
			}

			internal sealed override void Emit(CodeEmitter ilgen)
			{
				CodeEmitterLocal value1 = ilgen.AllocTempLocal(FloatOrDouble());
				CodeEmitterLocal value2 = ilgen.AllocTempLocal(FloatOrDouble());
				ilgen.Emit(OpCodes.Stloc, value2);
				ilgen.Emit(OpCodes.Stloc, value1);
				ilgen.Emit(OpCodes.Ldloc, value1);
				ilgen.Emit(OpCodes.Ldloc, value2);
				ilgen.Emit(OpCodes.Cgt_Un);
				ilgen.Emit(OpCodes.Ldloc, value1);
				ilgen.Emit(OpCodes.Ldloc, value2);
				ilgen.Emit(OpCodes.Clt);
				ilgen.Emit(OpCodes.Sub);
				ilgen.ReleaseTempLocal(value1);
				ilgen.ReleaseTempLocal(value2);
			}

			internal sealed override void EmitBcc(CodeEmitter ilgen, Comparison comp, CodeEmitterLabel label)
			{
				switch (comp)
				{
					case Comparison.LessOrEqual:
						ilgen.Emit(OpCodes.Ble, label);
						break;
					case Comparison.LessThan:
						ilgen.Emit(OpCodes.Blt, label);
						break;
					case Comparison.GreaterOrEqual:
						ilgen.Emit(OpCodes.Bge_Un, label);
						break;
					case Comparison.GreaterThan:
						ilgen.Emit(OpCodes.Bgt_Un, label);
						break;
				}
			}
		}

		sealed class DCmplExpr : FCmplExpr
		{
			protected override Type FloatOrDouble()
			{
				return Types.Double;
			}
		}

		sealed class DCmpgExpr : FCmpgExpr
		{
			protected override Type FloatOrDouble()
			{
				return Types.Double;
			}
		}

		sealed class ClassLiteralExpr : Expr
		{
			internal readonly TypeWrapper Type;

			internal ClassLiteralExpr(TypeWrapper type)
			{
				this.Type = type;
			}

			internal override void Emit(CodeEmitter ilgen)
			{
				Type.EmitClassLiteral(ilgen);
			}
		}

		sealed class ByteArrayLoadExpr : Expr
		{
			internal override void Emit(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Ldelem_I1);
			}

			internal override bool HasSideEffect
			{
				get { return true; }
			}

			internal override bool IsIncomplete
			{
				get { return true; }
			}
		}

		sealed class ConvertLong : UnaryExpr
		{
			internal ConvertLong(Expr expr)
				: base(expr)
			{
			}

			internal override void Emit(CodeEmitter ilgen)
			{
				base.Emit(ilgen);
				ilgen.Emit(OpCodes.Conv_I8);
			}
		}
	}
}
