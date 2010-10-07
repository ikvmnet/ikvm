/*
  Copyright (C) 2002-2010 Jeroen Frijters

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
			if (local == null)
			{
				// it's a temporary local that is only allocated on-demand
				local = ilgen.DeclareLocal(type);
			}
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
		private CodeEmitterLocal[] tempLocals = new CodeEmitterLocal[32];
		private ISymbolDocumentWriter symbols;
		private List<OpCodeWrapper> code = new List<OpCodeWrapper>();
#if LABELCHECK
		private Dictionary<CodeEmitterLabel, System.Diagnostics.StackFrame> labels = new Dictionary<CodeEmitterLabel, System.Diagnostics.StackFrame>();
#endif

		enum CodeType
		{
			OpCode,
			BeginScope,
			EndScope,
			DeclareLocal,
			ReleaseTempLocal,
			SequencePoint,
			LineNumber,
			Label,
			BeginExceptionBlock,
			BeginCatchBlock,
			BeginFaultBlock,
			BeginFinallyBlock,
			EndExceptionBlock,
			EndExceptionBlockFinally,
		}

		struct OpCodeWrapper
		{
			internal readonly CodeType pseudo;
			internal readonly OpCode opcode;
			private readonly object data;

			internal OpCodeWrapper(CodeType pseudo, object data)
			{
				this.pseudo = pseudo;
				this.opcode = OpCodes.Nop;
				this.data = data;
			}

			internal OpCodeWrapper(OpCode opcode, object data)
			{
				this.pseudo = CodeType.OpCode;
				this.opcode = opcode;
				this.data = data;
			}

			internal bool HasLabel
			{
				get { return data is CodeEmitterLabel; }
			}

			internal CodeEmitterLabel Label
			{
				get { return (CodeEmitterLabel)data; }
			}

			internal bool MatchLabel(OpCodeWrapper other)
			{
				return data == other.data;
			}

			internal CodeEmitterLocal Local
			{
				get { return (CodeEmitterLocal)data; }
			}

			internal bool MatchLocal(OpCodeWrapper other)
			{
				return data == other.data;
			}

			internal int ValueInt32
			{
				get { return (int)data; }
			}

			internal long ValueInt64
			{
				get { return (long)data; }
			}

			internal Type Type
			{
				get { return (Type)data; }
			}

			internal FieldInfo FieldInfo
			{
				get { return (FieldInfo)data; }
			}

			internal int Size
			{
				get
				{
					switch (pseudo)
					{
						case CodeType.BeginScope:
						case CodeType.EndScope:
						case CodeType.DeclareLocal:
						case CodeType.ReleaseTempLocal:
						case CodeType.LineNumber:
						case CodeType.Label:
						case CodeType.BeginExceptionBlock:
							return 0;
						case CodeType.SequencePoint:
							return 1;
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

			internal void RealEmit(int ilOffset, CodeEmitter codeEmitter)
			{
				if (pseudo == CodeType.OpCode)
				{
					codeEmitter.RealEmitOpCode(opcode, data);
				}
				else
				{
					codeEmitter.RealEmitPseudoOpCode(ilOffset, pseudo, data);
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
				case CodeType.BeginScope:
					ilgen_real.BeginScope();
					break;
				case CodeType.EndScope:
					ilgen_real.EndScope();
					break;
				case CodeType.DeclareLocal:
					((CodeEmitterLocal)data).Declare(ilgen_real);
					break;
				case CodeType.ReleaseTempLocal:
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

		private void RemoveJumpNext()
		{
			for (int i = 1; i < code.Count; i++)
			{
				if (code[i].pseudo == CodeType.Label
					&& code[i - 1].opcode == OpCodes.Br
					&& code[i - 1].MatchLabel(code[i]))
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
				FieldInfo field = code[index].FieldInfo;
				if (field != null && (field.DeclaringType.Attributes & TypeAttributes.BeforeFieldInit) != 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else if (code[index].opcode == OpCodes.Ldc_I4)
			{
				return true;
			}
			else if (code[index].opcode == OpCodes.Ldc_I8)
			{
				return true;
			}
			else if (code[index].opcode == OpCodes.Ldc_R4)
			{
				return true;
			}
			else if (code[index].opcode == OpCodes.Ldc_R8)
			{
				return true;
			}
			else if (code[index].opcode == OpCodes.Ldloc)
			{
				return true;
			}
			else if (code[index].opcode == OpCodes.Ldarg)
			{
				return true;
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
				if (code[i].pseudo == CodeType.Label)
				{
					code[i].Label.Offset = offset;
				}
				offset += code[i].Size;
			}
			offset = 0;
			for (int i = 0; i < code.Count; i++)
			{
				int prevOffset = offset;
				offset += code[i].Size;
				if (code[i].HasLabel && code[i].opcode.OperandType == OperandType.InlineBrTarget)
				{
					CodeEmitterLabel label = code[i].Label;
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
						code[i] = new OpCodeWrapper(opcode, label);
					}
				}
			}
		}

		private void OptimizePatterns()
		{
			for (int i = 1; i < code.Count; i++)
			{
				if (code[i].opcode == OpCodes.Isinst
					&& code[i + 1].opcode == OpCodes.Ldnull
					&& code[i + 2].opcode == OpCodes.Cgt_Un
					&& (code[i + 3].opcode == OpCodes.Brfalse || code[i + 3].opcode == OpCodes.Brtrue))
				{
					code.RemoveRange(i + 1, 2);
				}
				else if (code[i].opcode == OpCodes.Ldelem_I1
					&& code[i + 1].opcode == OpCodes.Ldc_I4 && code[i + 1].ValueInt32 == 255
					&& code[i + 2].opcode == OpCodes.And)
				{
					code[i] = new OpCodeWrapper(OpCodes.Ldelem_U1, null);
					code.RemoveRange(i + 1, 2);
				}
				else if (code[i].opcode == OpCodes.Ldelem_I1
					&& code[i + 1].opcode == OpCodes.Conv_I8
					&& code[i + 2].opcode == OpCodes.Ldc_I8 && code[i + 2].ValueInt64 == 255
					&& code[i + 3].opcode == OpCodes.And)
				{
					code[i] = new OpCodeWrapper(OpCodes.Ldelem_U1, null);
					code.RemoveRange(i + 2, 2);
				}
				else if (code[i].opcode == OpCodes.Ldc_I4
					&& code[i + 1].opcode == OpCodes.Ldc_I4
					&& code[i + 2].opcode == OpCodes.And)
				{
					code[i] = new OpCodeWrapper(OpCodes.Ldc_I4, code[i].ValueInt32 & code[i + 1].ValueInt32);
					code.RemoveRange(i + 1, 2);
				}
				else if (MatchCompare(i, OpCodes.Cgt, OpCodes.Clt_Un, Types.Double)		// dcmpl
					|| MatchCompare(i, OpCodes.Cgt, OpCodes.Clt_Un, Types.Single))		// fcmpl
				{
					PatchCompare(i, OpCodes.Ble_Un, OpCodes.Blt_Un, OpCodes.Bge, OpCodes.Bgt);
				}
				else if (MatchCompare(i, OpCodes.Cgt_Un, OpCodes.Clt, Types.Double)		// dcmpg
					|| MatchCompare(i, OpCodes.Cgt_Un, OpCodes.Clt, Types.Single))		// fcmpg
				{
					PatchCompare(i, OpCodes.Ble, OpCodes.Blt, OpCodes.Bge_Un, OpCodes.Bgt_Un);
				}
				else if (MatchCompare(i, OpCodes.Cgt, OpCodes.Clt, Types.Int64))		// lcmp
				{
					PatchCompare(i, OpCodes.Ble, OpCodes.Blt, OpCodes.Bge, OpCodes.Bgt);
				}
				else if (i < code.Count - 10
					&& code[i].opcode == OpCodes.Ldc_I4
					&& code[i + 1].opcode == OpCodes.Dup
					&& code[i + 2].opcode == OpCodes.Ldc_I4_M1
					&& code[i + 3].opcode == OpCodes.Bne_Un_S
					&& code[i + 4].opcode == OpCodes.Pop
					&& code[i + 5].opcode == OpCodes.Neg
					&& code[i + 6].opcode == OpCodes.Br_S
					&& code[i + 7].pseudo == CodeType.Label && code[i + 7].Label == code[i + 3].Label
					&& code[i + 8].opcode == OpCodes.Div
					&& code[i + 9].pseudo == CodeType.Label && code[i + 9].Label == code[i + 6].Label)
				{
					int divisor = code[i].ValueInt32;
					if (divisor == -1)
					{
						code[i] = code[i + 5];
						code.RemoveRange(i + 1, 9);
					}
					else
					{
						code[i + 1] = code[i + 8];
						code.RemoveRange(i + 2, 8);
					}
				}
				else if (i < code.Count - 11
					&& code[i].opcode == OpCodes.Ldc_I8
					&& code[i + 1].opcode == OpCodes.Dup
					&& code[i + 2].opcode == OpCodes.Ldc_I4_M1
					&& code[i + 3].opcode == OpCodes.Conv_I8
					&& code[i + 4].opcode == OpCodes.Bne_Un_S
					&& code[i + 5].opcode == OpCodes.Pop
					&& code[i + 6].opcode == OpCodes.Neg
					&& code[i + 7].opcode == OpCodes.Br_S
					&& code[i + 8].pseudo == CodeType.Label && code[i + 8].Label == code[i + 4].Label
					&& code[i + 9].opcode == OpCodes.Div
					&& code[i + 10].pseudo == CodeType.Label && code[i + 10].Label == code[i + 7].Label)
				{
					long divisor = code[i].ValueInt64;
					if (divisor == -1)
					{
						code[i] = code[i + 6];
						code.RemoveRange(i + 1, 10);
					}
					else
					{
						code[i + 1] = code[i + 9];
						code.RemoveRange(i + 2, 9);
					}
				}
				else if (code[i].opcode == OpCodes.Box
					&& code[i + 1].opcode == OpCodes.Unbox && code[i + 1].Type == code[i].Type)
				{
					CodeEmitterLocal local = new CodeEmitterLocal(code[i].Type);
					code[i] = new OpCodeWrapper(OpCodes.Stloc, local);
					code[i + 1] = new OpCodeWrapper(OpCodes.Ldloca, local);
				}
				else if (i < code.Count - 13
					&& code[i + 0].opcode == OpCodes.Box
					&& code[i + 1].opcode == OpCodes.Dup
					&& code[i + 2].opcode == OpCodes.Brtrue_S
					&& code[i + 3].opcode == OpCodes.Pop
					&& code[i + 4].opcode == OpCodes.Ldloca && code[i + 4].Local.LocalType == code[i + 0].Type
					&& code[i + 5].opcode == OpCodes.Initobj && code[i + 5].Type == code[i + 0].Type
					&& code[i + 6].opcode == OpCodes.Ldloc && code[i + 6].Local == code[i + 4].Local
					&& code[i + 7].pseudo == CodeType.ReleaseTempLocal && code[i + 7].Local == code[i + 6].Local
					&& code[i + 8].opcode == OpCodes.Br_S
					&& code[i + 9].pseudo == CodeType.Label && code[i + 9].Label == code[i + 2].Label
					&& code[i + 10].opcode == OpCodes.Unbox && code[i + 10].Type == code[i + 0].Type
					&& code[i + 11].opcode == OpCodes.Ldobj && code[i + 11].Type == code[i + 0].Type
					&& code[i + 12].pseudo == CodeType.Label && code[i + 12].Label == code[i + 8].Label)
				{
					code.RemoveRange(i, 13);
				}
			}
		}

		private bool MatchCompare(int index, OpCode cmp1, OpCode cmp2, Type type)
		{
			return code[index].opcode == OpCodes.Stloc && code[index].Local.LocalType == type
				&& code[index + 1].opcode == OpCodes.Stloc && code[index + 1].Local.LocalType == type
				&& code[index + 2].opcode == OpCodes.Ldloc && code[index + 2].MatchLocal(code[index + 1])
				&& code[index + 3].opcode == OpCodes.Ldloc && code[index + 3].MatchLocal(code[index])
				&& code[index + 4].opcode == cmp1
				&& code[index + 5].opcode == OpCodes.Ldloc && code[index + 5].MatchLocal(code[index + 1])
				&& code[index + 6].opcode == OpCodes.Ldloc && code[index + 6].MatchLocal(code[index])
				&& code[index + 7].opcode == cmp2
				&& code[index + 8].opcode == OpCodes.Sub
				&& code[index + 9].pseudo == CodeType.ReleaseTempLocal && code[index + 9].Local == code[index].Local
				&& code[index + 10].pseudo == CodeType.ReleaseTempLocal && code[index + 10].Local == code[index + 1].Local
				&& ((code[index + 11].opcode.FlowControl == FlowControl.Cond_Branch && code[index + 11].HasLabel) ||
					(code[index + 11].opcode == OpCodes.Ldc_I4_0
					&& (code[index + 12].opcode.FlowControl == FlowControl.Cond_Branch && code[index + 12].HasLabel)));
		}

		private void PatchCompare(int index, OpCode ble, OpCode blt, OpCode bge, OpCode bgt)
		{
			if (code[index + 11].opcode == OpCodes.Brtrue)
			{
				code[index] = new OpCodeWrapper(OpCodes.Bne_Un, code[index + 11].Label);
				code.RemoveRange(index + 1, 11);
			}
			else if (code[index + 11].opcode == OpCodes.Brfalse)
			{
				code[index] = new OpCodeWrapper(OpCodes.Beq, code[index + 11].Label);
				code.RemoveRange(index + 1, 11);
			}
			else if (code[index + 11].opcode == OpCodes.Ldc_I4_0)
			{
				if (code[index + 12].opcode == OpCodes.Ble)
				{
					code[index] = new OpCodeWrapper(ble, code[index + 12].Label);
					code.RemoveRange(index + 1, 12);
				}
				else if (code[index + 12].opcode == OpCodes.Blt)
				{
					code[index] = new OpCodeWrapper(blt, code[index + 12].Label);
					code.RemoveRange(index + 1, 12);
				}
				else if (code[index + 12].opcode == OpCodes.Bge)
				{
					code[index] = new OpCodeWrapper(bge, code[index + 12].Label);
					code.RemoveRange(index + 1, 12);
				}
				else if (code[index + 12].opcode == OpCodes.Bgt)
				{
					code[index] = new OpCodeWrapper(bgt, code[index + 12].Label);
					code.RemoveRange(index + 1, 12);
				}
			}
		}

		private void OptimizeEncodings()
		{
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].opcode == OpCodes.Ldc_I4)
				{
					code[i] = OptimizeLdcI4(code[i].ValueInt32);
				}
				else if (code[i].opcode == OpCodes.Ldc_I8)
				{
					OptimizeLdcI8(i);
				}
			}
		}

		private OpCodeWrapper OptimizeLdcI4(int value)
		{
			switch (value)
			{
				case -1:
					return new OpCodeWrapper(OpCodes.Ldc_I4_M1, null);
				case 0:
					return new OpCodeWrapper(OpCodes.Ldc_I4_0, null);
				case 1:
					return new OpCodeWrapper(OpCodes.Ldc_I4_1, null);
				case 2:
					return new OpCodeWrapper(OpCodes.Ldc_I4_2, null);
				case 3:
					return new OpCodeWrapper(OpCodes.Ldc_I4_3, null);
				case 4:
					return new OpCodeWrapper(OpCodes.Ldc_I4_4, null);
				case 5:
					return new OpCodeWrapper(OpCodes.Ldc_I4_5, null);
				case 6:
					return new OpCodeWrapper(OpCodes.Ldc_I4_6, null);
				case 7:
					return new OpCodeWrapper(OpCodes.Ldc_I4_7, null);
				case 8:
					return new OpCodeWrapper(OpCodes.Ldc_I4_8, null);
				default:
					if (value >= -128 && value <= 127)
					{
						return new OpCodeWrapper(OpCodes.Ldc_I4_S, (sbyte)value);
					}
					else
					{
						return new OpCodeWrapper(OpCodes.Ldc_I4, value);
					}
			}
		}

		private void OptimizeLdcI8(int index)
		{
			long value = code[index].ValueInt64;
			OpCode opc = OpCodes.Nop;
			switch (value)
			{
				case -1:
					opc = OpCodes.Ldc_I4_M1;
					break;
				case 0:
					opc = OpCodes.Ldc_I4_0;
					break;
				case 1:
					opc = OpCodes.Ldc_I4_1;
					break;
				case 2:
					opc = OpCodes.Ldc_I4_2;
					break;
				case 3:
					opc = OpCodes.Ldc_I4_3;
					break;
				case 4:
					opc = OpCodes.Ldc_I4_4;
					break;
				case 5:
					opc = OpCodes.Ldc_I4_5;
					break;
				case 6:
					opc = OpCodes.Ldc_I4_6;
					break;
				case 7:
					opc = OpCodes.Ldc_I4_7;
					break;
				case 8:
					opc = OpCodes.Ldc_I4_8;
					break;
				default:
					if (value >= -2147483648L && value <= 4294967295L)
					{
						if (value >= -128 && value <= 127)
						{
							code[index] = new OpCodeWrapper(OpCodes.Ldc_I4_S, (sbyte)value);
						}
						else
						{
							code[index] = new OpCodeWrapper(OpCodes.Ldc_I4, (int)value);
						}
						if (value < 0)
						{
							code.Insert(index + 1, new OpCodeWrapper(OpCodes.Conv_I8, null));
						}
						else
						{
							code.Insert(index + 1, new OpCodeWrapper(OpCodes.Conv_U8, null));
						}
					}
					break;
			}
			if (opc != OpCodes.Nop)
			{
				code[index] = new OpCodeWrapper(opc, null);
				code.Insert(index + 1, new OpCodeWrapper(OpCodes.Conv_I8, null));
			}
		}

		internal void DoEmit()
		{
			RemoveJumpNext();
			AnnihilatePops();
			OptimizePatterns();
			OptimizeEncodings();
			OptimizeBranchSizes();
			int ilOffset = 0;
			for (int i = 0; i < code.Count; i++)
			{
				code[i].RealEmit(ilOffset, this);
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
			return new CodeEmitterLocal(type);
		}

		internal void ReleaseTempLocal(CodeEmitterLocal lb)
		{
			EmitPseudoOpCode(CodeType.ReleaseTempLocal, lb);
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
			EmitPseudoOpCode(CodeType.BeginCatchBlock, exceptionType);
		}

		internal void BeginExceptionBlock()
		{
			exceptionStack.Push(inFinally);
			inFinally = false;
			EmitPseudoOpCode(CodeType.BeginExceptionBlock, null);
		}

		internal void BeginFaultBlock()
		{
			inFinally = true;
			EmitPseudoOpCode(CodeType.BeginFaultBlock, null);
		}

		internal void BeginFinallyBlock()
		{
			inFinally = true;
			EmitPseudoOpCode(CodeType.BeginFinallyBlock, null);
		}

		internal void BeginScope()
		{
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
			EmitOpCode(opcode, null);
		}

		internal void Emit(OpCode opcode, byte arg)
		{
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, ConstructorInfo con)
		{
			EmitOpCode(opcode, con);
		}

		internal void Emit(OpCode opcode, double arg)
		{
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, FieldInfo field)
		{
			EmitOpCode(opcode, field);
		}

		internal void Emit(OpCode opcode, short arg)
		{
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, int arg)
		{
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, long arg)
		{
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, CodeEmitterLabel label)
		{
			EmitOpCode(opcode, label);
		}

		internal void Emit(OpCode opcode, CodeEmitterLabel[] labels)
		{
			EmitOpCode(opcode, labels);
		}

		internal void Emit(OpCode opcode, CodeEmitterLocal local)
		{
			EmitOpCode(opcode, local);
		}

		internal void Emit(OpCode opcode, MethodInfo meth)
		{
			EmitOpCode(opcode, meth);
		}

		internal void Emit(OpCode opcode, sbyte arg)
		{
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, float arg)
		{
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, string arg)
		{
			EmitOpCode(opcode, arg);
		}

		internal void Emit(OpCode opcode, Type cls)
		{
			EmitOpCode(opcode, cls);
		}

		internal void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			EmitOpCode(opcode, new CalliWrapper(unmanagedCallConv, returnType, parameterTypes));
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
			EmitPseudoOpCode(CodeType.EndScope, null);
		}

		internal void MarkLabel(CodeEmitterLabel loc)
		{
#if LABELCHECK
			labels.Remove(loc);
#endif
			EmitPseudoOpCode(CodeType.Label, loc);
		}

		internal void ThrowException(Type excType)
		{
			Emit(OpCodes.Newobj, excType.GetConstructor(Type.EmptyTypes));
			Emit(OpCodes.Throw);
		}

		internal void SetLineNumber(ushort line)
		{
			if (symbols != null)
			{
				EmitPseudoOpCode(CodeType.SequencePoint, (int)line);
			}
			EmitPseudoOpCode(CodeType.LineNumber, (int)line);
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

		internal void EmitUnboxSpecial(Type type)
		{
			// NOTE if the reference is null, we treat it as a default instance of the value type.
			Emit(OpCodes.Dup);
			CodeEmitterLabel label1 = DefineLabel();
			Emit(OpCodes.Brtrue_S, label1);
			Emit(OpCodes.Pop);
			CodeEmitterLocal local = AllocTempLocal(type);
			Emit(OpCodes.Ldloca, local);
			Emit(OpCodes.Initobj, type);
			Emit(OpCodes.Ldloc, local);
			ReleaseTempLocal(local);
			CodeEmitterLabel label2 = DefineLabel();
			Emit(OpCodes.Br_S, label2);
			MarkLabel(label1);
			Emit(OpCodes.Unbox, type);
			Emit(OpCodes.Ldobj, type);
			MarkLabel(label2);
		}

		// the purpose of this method is to avoid calling a wrong overload of Emit()
		// (e.g. when passing a byte or short)
		internal void Emit_Ldc_I4(int i)
		{
			Emit(OpCodes.Ldc_I4, i);
		}

		internal void Emit_idiv()
		{
			// we need to special case dividing by -1, because the CLR div instruction
			// throws an OverflowException when dividing Int32.MinValue by -1, and
			// Java just silently overflows
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

		internal void Emit_ldiv()
		{
			// we need to special case dividing by -1, because the CLR div instruction
			// throws an OverflowException when dividing Int32.MinValue by -1, and
			// Java just silently overflows
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

		internal void Emit_instanceof(Type type)
		{
			Emit(OpCodes.Isinst, type);
			Emit(OpCodes.Ldnull);
			Emit(OpCodes.Cgt_Un);
		}

		internal enum Comparison
		{
			LessOrEqual,
			LessThan,
			GreaterOrEqual,
			GreaterThan
		}

		internal void Emit_if_le_lt_ge_gt(Comparison comp, CodeEmitterLabel label)
		{
			// don't change this Ldc_I4_0 to Ldc_I4(0) because the optimizer recognizes
			// only this specific pattern
			Emit(OpCodes.Ldc_I4_0);
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

		private void EmitCmp(Type type, OpCode cmp1, OpCode cmp2)
		{
			CodeEmitterLocal value1 = AllocTempLocal(type);
			CodeEmitterLocal value2 = AllocTempLocal(type);
			Emit(OpCodes.Stloc, value2);
			Emit(OpCodes.Stloc, value1);
			Emit(OpCodes.Ldloc, value1);
			Emit(OpCodes.Ldloc, value2);
			Emit(cmp1);
			Emit(OpCodes.Ldloc, value1);
			Emit(OpCodes.Ldloc, value2);
			Emit(cmp2);
			Emit(OpCodes.Sub);
			ReleaseTempLocal(value2);
			ReleaseTempLocal(value1);
		}

		internal void Emit_lcmp()
		{
			EmitCmp(Types.Int64, OpCodes.Cgt, OpCodes.Clt);
		}

		internal void Emit_fcmpl()
		{
			EmitCmp(Types.Single, OpCodes.Cgt, OpCodes.Clt_Un);
		}

		internal void Emit_fcmpg()
		{
			EmitCmp(Types.Single, OpCodes.Cgt_Un, OpCodes.Clt);
		}

		internal void Emit_dcmpl()
		{
			EmitCmp(Types.Double, OpCodes.Cgt, OpCodes.Clt_Un);
		}

		internal void Emit_dcmpg()
		{
			EmitCmp(Types.Double, OpCodes.Cgt_Un, OpCodes.Clt);
		}

		internal void Emit_And_I4(int v)
		{
			Emit(OpCodes.Ldc_I4, v);
			Emit(OpCodes.And);
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
	}
}
