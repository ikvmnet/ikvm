/*
  Copyright (C) 2002-2012 Jeroen Frijters

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
//#define LABELCHECK
#define CHECK_INVARIANTS
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
		internal readonly Label Label;
		internal int Temp;

		internal CodeEmitterLabel(Label label)
		{
			this.Label = label;
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
		private static readonly MethodInfo monitorEnter = JVM.Import(typeof(System.Threading.Monitor)).GetMethod("Enter", BindingFlags.Public | BindingFlags.Static, null, new Type[] { Types.Object }, null);
		private static readonly MethodInfo monitorExit = JVM.Import(typeof(System.Threading.Monitor)).GetMethod("Exit", BindingFlags.Public | BindingFlags.Static, null, new Type[] { Types.Object }, null);
		private static readonly bool experimentalOptimizations = JVM.SafeGetEnvironmentVariable("IKVM_EXPERIMENTAL_OPTIMIZATIONS") != null;
		private static MethodInfo memoryBarrier;
		private ILGenerator ilgen_real;
#if !STATIC_COMPILER
		private bool inFinally;
		private Stack<bool> exceptionStack = new Stack<bool>();
#endif
		private IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter linenums;
		private CodeEmitterLocal[] tempLocals = new CodeEmitterLocal[32];
		private ISymbolDocumentWriter symbols;
		private List<OpCodeWrapper> code = new List<OpCodeWrapper>(10);
		private readonly Type declaringType;
#if LABELCHECK
		private Dictionary<CodeEmitterLabel, System.Diagnostics.StackFrame> labels = new Dictionary<CodeEmitterLabel, System.Diagnostics.StackFrame>();
#endif

		static CodeEmitter()
		{
			if (experimentalOptimizations)
			{
				Console.Error.WriteLine("IKVM.NET experimental optimizations enabled.");
			}
		}

		enum CodeType : short
		{
			Unreachable,
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
			MemoryBarrier,
			TailCallPrevention,
			ClearStack,
			MonitorEnter,
			MonitorExit,
		}

		enum CodeTypeFlags : short
		{
			None = 0,
			EndFaultOrFinally = 1,
		}

		struct OpCodeWrapper
		{
			internal readonly CodeType pseudo;
			private readonly CodeTypeFlags flags;
			internal readonly OpCode opcode;
			private readonly object data;

			internal OpCodeWrapper(CodeType pseudo, object data)
			{
				this.pseudo = pseudo;
				this.flags = CodeTypeFlags.None;
				this.opcode = OpCodes.Nop;
				this.data = data;
			}

			internal OpCodeWrapper(CodeType pseudo, CodeTypeFlags flags)
			{
				this.pseudo = pseudo;
				this.flags = flags;
				this.opcode = OpCodes.Nop;
				this.data = null;
			}

			internal OpCodeWrapper(OpCode opcode, object data)
			{
				this.pseudo = CodeType.OpCode;
				this.flags = CodeTypeFlags.None;
				this.opcode = opcode;
				this.data = data;
			}

			internal bool Match(OpCodeWrapper other)
			{
				return other.pseudo == pseudo
					&& other.opcode == opcode
					&& (other.data == data || (data != null && data.Equals(other.data)));
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

			internal CodeEmitterLabel[] Labels
			{
				get { return (CodeEmitterLabel[])data; }
			}

			internal bool HasLocal
			{
				get { return data is CodeEmitterLocal; }
			}

			internal CodeEmitterLocal Local
			{
				get { return (CodeEmitterLocal)data; }
			}

			internal bool MatchLocal(OpCodeWrapper other)
			{
				return data == other.data;
			}

			internal bool HasValueByte
			{
				get { return data is byte; }
			}

			internal byte ValueByte
			{
				get { return (byte)data; }
			}

			internal short ValueInt16
			{
				get { return (short)data; }
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

			internal MethodBase MethodBase
			{
				get { return (MethodBase)data; }
			}

			internal int Size
			{
				get
				{
					switch (pseudo)
					{
						case CodeType.Unreachable:
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
#if STATIC_COMPILER
							return 0;
#else
							if ((flags & CodeTypeFlags.EndFaultOrFinally) != 0)
							{
								return 1 + 2;
							}
							return 5 + 2;
#endif
						case CodeType.MemoryBarrier:
						case CodeType.MonitorEnter:
						case CodeType.MonitorExit:
							return 5;
						case CodeType.TailCallPrevention:
							return 2;
						case CodeType.ClearStack:
							return 2;
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

			internal void RealEmit(int ilOffset, CodeEmitter codeEmitter, ref int lineNumber)
			{
				if (pseudo == CodeType.OpCode)
				{
					if (lineNumber != -1)
					{
						if (codeEmitter.linenums == null)
						{
							codeEmitter.linenums = new IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter(32);
						}
						codeEmitter.linenums.AddMapping(ilOffset, lineNumber);
						lineNumber = -1;
					}
					codeEmitter.RealEmitOpCode(opcode, data);
				}
				else if (pseudo == CodeType.LineNumber)
				{
					lineNumber = (int)data;
				}
				else
				{
					codeEmitter.RealEmitPseudoOpCode(ilOffset, pseudo, data);
				}
			}

			public override string ToString()
			{
				return pseudo.ToString() + " " + data;
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
			return new CodeEmitter(mb.GetILGenerator(), mb.DeclaringType);
		}

#if !STATIC_COMPILER
		internal static CodeEmitter Create(DynamicMethod dm)
		{
			return new CodeEmitter(dm.GetILGenerator(), null);
		}
#endif

		private CodeEmitter(ILGenerator ilgen, Type declaringType)
		{
#if STATIC_COMPILER
			ilgen.__DisableExceptionBlockAssistance();
#endif
			this.ilgen_real = ilgen;
			this.declaringType = declaringType;
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
				case CodeType.Unreachable:
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
				case CodeType.ReleaseTempLocal:
					break;
				case CodeType.SequencePoint:
					ilgen_real.MarkSequencePoint(symbols, (int)data, 0, (int)data + 1, 0);
					// we emit a nop to make sure we always have an instruction associated with the sequence point
					ilgen_real.Emit(OpCodes.Nop);
					break;
				case CodeType.Label:
					ilgen_real.MarkLabel(((CodeEmitterLabel)data).Label);
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
				case CodeType.EndExceptionBlock:
					ilgen_real.EndExceptionBlock();
#if !STATIC_COMPILER
					// HACK to keep the verifier happy we need this bogus jump
					// (because of the bogus Leave that Ref.Emit ends the try block with)
					ilgen_real.Emit(OpCodes.Br_S, (sbyte)-2);
#endif
					break;
				case CodeType.MemoryBarrier:
					if (memoryBarrier == null)
					{
						memoryBarrier = JVM.Import(typeof(System.Threading.Thread)).GetMethod("MemoryBarrier", Type.EmptyTypes);
					}
					ilgen_real.Emit(OpCodes.Call, memoryBarrier);
					break;
				case CodeType.MonitorEnter:
					ilgen_real.Emit(OpCodes.Call, monitorEnter);
					break;
				case CodeType.MonitorExit:
					ilgen_real.Emit(OpCodes.Call, monitorExit);
					break;
				case CodeType.TailCallPrevention:
					ilgen_real.Emit(OpCodes.Ldnull);
					ilgen_real.Emit(OpCodes.Pop);
					break;
				case CodeType.ClearStack:
					ilgen_real.Emit(OpCodes.Leave_S, (byte)0);
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
				if (code[i].pseudo == CodeType.Label)
				{
					if (code[i - 1].opcode == OpCodes.Br
						&& code[i - 1].MatchLabel(code[i]))
					{
						code.RemoveAt(i - 1);
						i--;
					}
					else if (i >= 2
						&& code[i - 1].pseudo == CodeType.LineNumber
						&& code[i - 2].opcode == OpCodes.Br
						&& code[i - 2].MatchLabel(code[i]))
					{
						code.RemoveAt(i - 2);
						i--;
					}
				}
			}
		}

		private void AnnihilateStoreReleaseTempLocals()
		{
			for (int i = 1; i < code.Count; i++)
			{
				if (code[i].opcode == OpCodes.Stloc)
				{
					if (code[i + 1].pseudo == CodeType.ReleaseTempLocal
						&& code[i].Local == code[i + 1].Local)
					{
						code[i] = new OpCodeWrapper(OpCodes.Pop, null);
					}
					else if (code[i + 1].opcode == OpCodes.Ldloc
						&& code[i + 1].Local == code[i].Local
						&& code[i + 2].pseudo == CodeType.ReleaseTempLocal
						&& code[i + 2].Local == code[i].Local)
					{
						code.RemoveRange(i, 2);
					}
				}
			}
		}

		private void AnnihilatePops()
		{
			for (int i = 1; i < code.Count; i++)
			{
				if (code[i].opcode == OpCodes.Pop)
				{
					// search backwards for a candidate push to annihilate
					int stack = 0;
					for (int j = i - 1; j >= 0; j--)
					{
						if (IsSideEffectFreePush(j))
						{
							if (stack == 0)
							{
								code.RemoveAt(i);
								code.RemoveAt(j);
								i -= 2;
								break;
							}
							stack++;
						}
						else if (code[j].opcode == OpCodes.Stloc)
						{
							stack--;
						}
						else if (code[j].opcode == OpCodes.Shl
							|| code[j].opcode == OpCodes.And
							|| code[j].opcode == OpCodes.Add
							|| code[j].opcode == OpCodes.Sub)
						{
							if (stack == 0)
							{
								break;
							}
							stack--;
						}
						else if (code[j].opcode == OpCodes.Conv_Ovf_I4
							|| code[j].opcode == OpCodes.Conv_I8
							|| code[j].opcode == OpCodes.Ldlen)
						{
							if (stack == 0)
							{
								break;
							}
							// no stack effect
						}
						else
						{
							break;
						}
					}
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
				FieldInfo field = code[index].FieldInfo;
				if (field != null)
				{
					// Here we are considering BeforeFieldInit to mean that we really don't care about
					// when the type is initialized (which is what we mean in the rest of the IKVM code as well)
					// but it is good to point it out here because strictly speaking we're violating the
					// BeforeFieldInit contract here by considering dummy loads not to be field accesses.
					if ((field.DeclaringType.Attributes & TypeAttributes.BeforeFieldInit) != 0)
					{
						return true;
					}
					// If we're accessing a field in the current type, it can't trigger the static initializer
					// (unless beforefieldinit is set, but see above for that scenario)
					if (field.DeclaringType == declaringType)
					{
						return true;
					}
				}
				return false;
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
			else if (code[index].opcode == OpCodes.Ldarg_S)
			{
				return true;
			}
			else if (code[index].opcode == OpCodes.Ldarg_0)
			{
				return true;
			}
			else if (code[index].opcode == OpCodes.Ldarg_1)
			{
				return true;
			}
			else if (code[index].opcode == OpCodes.Ldarg_2)
			{
				return true;
			}
			else if (code[index].opcode == OpCodes.Ldarg_3)
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
					code[i].Label.Temp = offset;
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
					int diff = label.Temp - (prevOffset + code[i].opcode.Size + 1);
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
			SetLabelRefCounts();
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
					&& code[i + 3].opcode == OpCodes.Bne_Un
					&& code[i + 4].opcode == OpCodes.Pop
					&& code[i + 5].opcode == OpCodes.Neg
					&& code[i + 6].opcode == OpCodes.Br
					&& code[i + 7].pseudo == CodeType.Label && code[i + 7].MatchLabel(code[i + 3]) && code[i + 7].Label.Temp == 1
					&& code[i + 8].opcode == OpCodes.Div
					&& code[i + 9].pseudo == CodeType.Label && code[i + 9].Label == code[i + 6].Label && code[i + 9].Label.Temp == 1)
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
					&& code[i + 4].opcode == OpCodes.Bne_Un
					&& code[i + 5].opcode == OpCodes.Pop
					&& code[i + 6].opcode == OpCodes.Neg
					&& code[i + 7].opcode == OpCodes.Br
					&& code[i + 8].pseudo == CodeType.Label && code[i + 8].MatchLabel(code[i + 4]) && code[i + 8].Label.Temp == 1
					&& code[i + 9].opcode == OpCodes.Div
					&& code[i + 10].pseudo == CodeType.Label && code[i + 10].MatchLabel(code[i + 7]) && code[i + 10].Label.Temp == 1)
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
					&& code[i + 2].opcode == OpCodes.Brtrue
					&& code[i + 3].opcode == OpCodes.Pop
					&& code[i + 4].opcode == OpCodes.Ldloca && code[i + 4].Local.LocalType == code[i + 0].Type
					&& code[i + 5].opcode == OpCodes.Initobj && code[i + 5].Type == code[i + 0].Type
					&& code[i + 6].opcode == OpCodes.Ldloc && code[i + 6].Local == code[i + 4].Local
					&& code[i + 7].pseudo == CodeType.ReleaseTempLocal && code[i + 7].Local == code[i + 6].Local
					&& code[i + 8].opcode == OpCodes.Br
					&& code[i + 9].pseudo == CodeType.Label && code[i + 9].MatchLabel(code[i + 2]) && code[i + 9].Label.Temp == 1
					&& code[i + 10].opcode == OpCodes.Unbox && code[i + 10].Type == code[i + 0].Type
					&& code[i + 11].opcode == OpCodes.Ldobj && code[i + 11].Type == code[i + 0].Type
					&& code[i + 12].pseudo == CodeType.Label && code[i + 12].MatchLabel(code[i + 8]) && code[i + 12].Label.Temp == 1)
				{
					code.RemoveRange(i, 13);
				}

				// NOTE intentionally not an else, because we want to optimize the code generated by the earlier compare optimization
				if (i < code.Count - 6
					&& code[i].opcode.FlowControl == FlowControl.Cond_Branch
					&& code[i + 1].opcode == OpCodes.Ldc_I4 && code[i + 1].ValueInt32 == 1
					&& code[i + 2].opcode == OpCodes.Br
					&& code[i + 3].pseudo == CodeType.Label && code[i + 3].MatchLabel(code[i]) && code[i + 3].Label.Temp == 1
					&& code[i + 4].opcode == OpCodes.Ldc_I4 && code[i + 4].ValueInt32 == 0
					&& code[i + 5].pseudo == CodeType.Label && code[i + 5].MatchLabel(code[i + 2]) && code[i + 5].Label.Temp == 1)
				{
					if (code[i].opcode == OpCodes.Bne_Un)
					{
						code[i] = new OpCodeWrapper(OpCodes.Ceq, null);
						code.RemoveRange(i + 1, 5);
					}
					else if (code[i].opcode == OpCodes.Beq)
					{
						code[i + 0] = new OpCodeWrapper(OpCodes.Ceq, null);
						code[i + 1] = new OpCodeWrapper(OpCodes.Ldc_I4, 0);
						code[i + 2] = new OpCodeWrapper(OpCodes.Ceq, null);
						code.RemoveRange(i + 3, 3);
					}
					else if (code[i].opcode == OpCodes.Ble || code[i].opcode == OpCodes.Ble_Un)
					{
						code[i] = new OpCodeWrapper(OpCodes.Cgt, null);
						code.RemoveRange(i + 1, 5);
					}
					else if (code[i].opcode == OpCodes.Blt)
					{
						code[i] = new OpCodeWrapper(OpCodes.Clt, null);
						code[i + 1] = new OpCodeWrapper(OpCodes.Ldc_I4, 0);
						code[i + 2] = new OpCodeWrapper(OpCodes.Ceq, null);
						code.RemoveRange(i + 3, 3);
					}
					else if (code[i].opcode == OpCodes.Blt_Un)
					{
						code[i] = new OpCodeWrapper(OpCodes.Clt_Un, null);
						code[i + 1] = new OpCodeWrapper(OpCodes.Ldc_I4, 0);
						code[i + 2] = new OpCodeWrapper(OpCodes.Ceq, null);
						code.RemoveRange(i + 3, 3);
					}
					else if (code[i].opcode == OpCodes.Bge || code[i].opcode == OpCodes.Bge_Un)
					{
						code[i] = new OpCodeWrapper(OpCodes.Clt, null);
						code.RemoveRange(i + 1, 5);
					}
					else if (code[i].opcode == OpCodes.Bgt)
					{
						code[i] = new OpCodeWrapper(OpCodes.Cgt, null);
						code[i + 1] = new OpCodeWrapper(OpCodes.Ldc_I4, 0);
						code[i + 2] = new OpCodeWrapper(OpCodes.Ceq, null);
						code.RemoveRange(i + 3, 3);
					}
					else if (code[i].opcode == OpCodes.Bgt_Un)
					{
						code[i] = new OpCodeWrapper(OpCodes.Cgt_Un, null);
						code[i + 1] = new OpCodeWrapper(OpCodes.Ldc_I4, 0);
						code[i + 2] = new OpCodeWrapper(OpCodes.Ceq, null);
						code.RemoveRange(i + 3, 3);
					}
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

		private void ChaseBranches()
		{
			/*
			 * Here we do a couple of different optimizations to unconditional branches:
			 *  - a branch to a ret or endfinally will be replaced
			 *    by the ret or endfinally instruction (because that is always at least as efficient)
			 *  - a branch to a branch will remove the indirection
			 *  - a leave to a branch or leave will remove the indirection
			 */
			SetLabelIndexes();
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].opcode == OpCodes.Br)
				{
					int target = code[i].Label.Temp + 1;
					if (code[target].pseudo == CodeType.LineNumber)
					{
						// line number info on endfinally or ret is probably useless anyway
						target++;
					}
					if (code[target].opcode == OpCodes.Endfinally || code[target].opcode == OpCodes.Ret)
					{
						code[i] = code[target];
					}
					else
					{
						CodeEmitterLabel label = null;
						while (code[target].opcode == OpCodes.Br && target != i)
						{
							label = code[target].Label;
							target = code[target].Label.Temp + 1;
						}
						if (label != null)
						{
							code[i] = new OpCodeWrapper(OpCodes.Br, label);
						}
					}
				}
				else if (code[i].opcode == OpCodes.Leave)
				{
					int target = code[i].Label.Temp + 1;
					CodeEmitterLabel label = null;
					while ((code[target].opcode == OpCodes.Br || code[target].opcode == OpCodes.Leave) && target != i)
					{
						label = code[target].Label;
						target = code[target].Label.Temp + 1;
					}
					if (label != null)
					{
						code[i] = new OpCodeWrapper(OpCodes.Leave, label);
					}
				}
			}
		}

		private void RemoveSingletonBranches()
		{
			/*
			 * Here we try to remove unconditional branches that jump to a label with ref count of one
			 * and where the code is not otherwise used.
			 */
			SetLabelRefCounts();
			// now increment label refcounts for labels that are also reachable via the preceding instruction
			bool reachable = true;
			for (int i = 0; i < code.Count; i++)
			{
				if (reachable)
				{
					switch (code[i].pseudo)
					{
						case CodeType.Label:
							code[i].Label.Temp++;
							break;
						case CodeType.BeginCatchBlock:
						case CodeType.BeginFaultBlock:
						case CodeType.BeginFinallyBlock:
						case CodeType.EndExceptionBlock:
							throw new InvalidOperationException();
						case CodeType.OpCode:
							switch (code[i].opcode.FlowControl)
							{
								case FlowControl.Branch:
								case FlowControl.Return:
								case FlowControl.Throw:
									reachable = false;
									break;
							}
							break;
					}
				}
				else
				{
					switch (code[i].pseudo)
					{
						case CodeType.Label:
							reachable = code[i].Label.Temp > 0;
							break;
						case CodeType.BeginCatchBlock:
						case CodeType.BeginFaultBlock:
						case CodeType.BeginFinallyBlock:
							reachable = true;
							break;
					}
				}
			}

			// now remove the unconditional branches to labels with a refcount of one
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].opcode == OpCodes.Br && code[i].Label.Temp == 1)
				{
					int target = FindLabel(code[i].Label) + 1;
					for (int j = target; j < code.Count; j++)
					{
						switch (code[j].pseudo)
						{
							case CodeType.OpCode:
								if (code[j].HasLocal && FindLocal(code[j].Local) > i)
								{
									// we cannot local variable usage before the declaration
									goto breakOuter;
								}
								switch (code[j].opcode.FlowControl)
								{
									case FlowControl.Branch:
									case FlowControl.Return:
									case FlowControl.Throw:
										// we've found a viable sequence of opcode to move to the branch location
										List<OpCodeWrapper> range = code.GetRange(target, j - target + 1);
										if (target < i)
										{
											code.RemoveAt(i);
											code.InsertRange(i, range);
											code.RemoveRange(target - 1, range.Count + 1);
											i -= range.Count + 1;
										}
										else
										{
											code.RemoveRange(target - 1, range.Count + 1);
											code.RemoveAt(i);
											code.InsertRange(i, range);
										}
										goto breakOuter;
								}
								break;
							case CodeType.Label:
							case CodeType.BeginExceptionBlock:
							case CodeType.DeclareLocal:
								goto breakOuter;
						}
					}
				breakOuter: ;
				}
			}
		}

		private int FindLabel(CodeEmitterLabel label)
		{
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].pseudo == CodeType.Label && code[i].Label == label)
				{
					return i;
				}
			}
			throw new InvalidOperationException();
		}

		private int FindLocal(CodeEmitterLocal local)
		{
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].pseudo == CodeType.DeclareLocal && code[i].Local == local)
				{
					return i;
				}
			}
			// if the local variable isn't declared, it is a temporary that is allocated on demand
			// (so we can move their usage freely)
			return 0;
		}

		private void SortPseudoOpCodes()
		{
			for (int i = 0; i < code.Count - 1; i++)
			{
				switch (code[i].pseudo)
				{
					case CodeType.ReleaseTempLocal:
						for (int j = i - 1; ; j--)
						{
							if (j == -1)
							{
								code.RemoveAt(i);
								break;
							}
							if (code[j].HasLocal && code[j].Local == code[i].Local)
							{
								MoveInstruction(i, j + 1);
								break;
							}
						}
						break;
				}
			}
		}

		private void MoveInstruction(int i, int j)
		{
			if (i == j - 1 || i == j + 1)
			{
				OpCodeWrapper temp = code[i];
				code[i] = code[j];
				code[j] = temp;
			}
			else if (i < j)
			{
				code.Insert(j, code[i]);
				code.RemoveAt(i);
			}
			else if (i > j)
			{
				OpCodeWrapper temp = code[i];
				code.RemoveAt(i);
				code.Insert(j, temp);
			}
		}

		private void ClearLabelTemp()
		{
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].pseudo == CodeType.Label)
				{
					code[i].Label.Temp = 0;
				}
			}
		}

		private void SetLabelIndexes()
		{
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].pseudo == CodeType.Label)
				{
					code[i].Label.Temp = i;
				}
			}
		}

		private void SetLabelRefCounts()
		{
			ClearLabelTemp();
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].pseudo == CodeType.OpCode)
				{
					if (code[i].HasLabel)
					{
						code[i].Label.Temp++;
					}
					else if (code[i].opcode == OpCodes.Switch)
					{
						foreach (CodeEmitterLabel label in code[i].Labels)
						{
							label.Temp++;
						}
					}
				}
			}
		}

		private void RemoveUnusedLabels()
		{
			SetLabelRefCounts();
			for (int i = 0; i < code.Count; i++)
			{
				while (code[i].pseudo == CodeType.Label && code[i].Label.Temp == 0)
				{
					code.RemoveAt(i);
				}
			}
		}

		private void RemoveDeadCode()
		{
			ClearLabelTemp();
			const int ReachableFlag = 1;
			const int ProcessedFlag = 2;
			bool reachable = true;
			bool done = false;
			while (!done)
			{
				done = true;
				for (int i = 0; i < code.Count; i++)
				{
					if (reachable)
					{
						if (code[i].pseudo == CodeType.Label)
						{
							if (code[i].Label.Temp == ProcessedFlag)
							{
								done = false;
							}
							code[i].Label.Temp |= ReachableFlag;
						}
						else if (code[i].pseudo == CodeType.OpCode)
						{
							if (code[i].HasLabel)
							{
								if (code[i].Label.Temp == ProcessedFlag)
								{
									done = false;
								}
								code[i].Label.Temp |= ReachableFlag;
							}
							else if (code[i].opcode == OpCodes.Switch)
							{
								foreach (CodeEmitterLabel label in code[i].Labels)
								{
									if (label.Temp == ProcessedFlag)
									{
										done = false;
									}
									label.Temp |= ReachableFlag;
								}
							}
							switch (code[i].opcode.FlowControl)
							{
								case FlowControl.Cond_Branch:
									if (!code[i].HasLabel && code[i].opcode != OpCodes.Switch)
									{
										throw new NotSupportedException();
									}
									break;
								case FlowControl.Branch:
								case FlowControl.Return:
								case FlowControl.Throw:
									reachable = false;
									break;
							}
						}
					}
					else if (code[i].pseudo == CodeType.BeginCatchBlock)
					{
						reachable = true;
					}
					else if (code[i].pseudo == CodeType.BeginFaultBlock)
					{
						reachable = true;
					}
					else if (code[i].pseudo == CodeType.BeginFinallyBlock)
					{
						reachable = true;
					}
					else if (code[i].pseudo == CodeType.Label && (code[i].Label.Temp & ReachableFlag) != 0)
					{
						reachable = true;
					}
					if (code[i].pseudo == CodeType.Label)
					{
						code[i].Label.Temp |= ProcessedFlag;
					}
				}
			}
			reachable = true;
			int firstUnreachable = -1;
			for (int i = 0; i < code.Count; i++)
			{
				if (reachable)
				{
					if (code[i].pseudo == CodeType.OpCode)
					{
						switch (code[i].opcode.FlowControl)
						{
							case FlowControl.Branch:
							case FlowControl.Return:
							case FlowControl.Throw:
								reachable = false;
								firstUnreachable = i + 1;
								break;
						}
					}
				}
				else
				{
					switch (code[i].pseudo)
					{
						case CodeType.OpCode:
							break;
						case CodeType.Label:
							if ((code[i].Label.Temp & ReachableFlag) != 0)
							{
								goto case CodeType.BeginCatchBlock;
							}
							break;
						case CodeType.BeginCatchBlock:
						case CodeType.BeginFaultBlock:
						case CodeType.BeginFinallyBlock:
							code.RemoveRange(firstUnreachable, i - firstUnreachable);
							i = firstUnreachable;
							firstUnreachable = -1;
							reachable = true;
							break;
						default:
							code.RemoveRange(firstUnreachable, i - firstUnreachable);
							i = firstUnreachable;
							firstUnreachable++;
							break;
					}
				}
			}
			if (!reachable)
			{
				code.RemoveRange(firstUnreachable, code.Count - firstUnreachable);
			}

			// TODO can't we incorporate this in the above code?
			// remove exception blocks with empty try blocks
			// (which can happen if the try block is unreachable)
			for (int i = 0; i < code.Count; i++)
			{
			restart:
				if (code[i].pseudo == CodeType.BeginExceptionBlock)
				{
					for (int k = 0; ; k++)
					{
						switch (code[i + k].pseudo)
						{
							case CodeType.BeginCatchBlock:
							case CodeType.BeginFaultBlock:
							case CodeType.BeginFinallyBlock:
								int depth = 0;
								for (int j = i + 1; ; j++)
								{
									switch (code[j].pseudo)
									{
										case CodeType.BeginExceptionBlock:
											depth++;
											break;
										case CodeType.EndExceptionBlock:
											if (depth == 0)
											{
												code.RemoveRange(i, (j - i) + 1);
												goto restart;
											}
											depth--;
											break;
									}
								}
							case CodeType.OpCode:
								goto next;
						}
					}
				}
			next: ;
			}
		}

		private void DeduplicateBranchSourceTargetCode()
		{
			SetLabelIndexes();
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].opcode == OpCodes.Br && code[i].HasLabel)
				{
					int source = i - 1;
					int target = code[i].Label.Temp - 1;
					while (source >= 0 && target >= 0)
					{
						switch (code[source].pseudo)
						{
							case CodeType.LineNumber:
							case CodeType.OpCode:
								break;
							default:
								goto break_while;
						}
						if (!code[source].Match(code[target]))
						{
							break;
						}
						switch (code[source].opcode.FlowControl)
						{
							case FlowControl.Branch:
							case FlowControl.Cond_Branch:
								goto break_while;
						}
						source--;
						target--;
					}
				break_while: ;
					source++;
					target++;
					if (source != i && target > 0 && source != target - 1)
					{
						// TODO for now we only do this optimization if there happens to be an appriopriate label
						if (code[target - 1].pseudo == CodeType.Label)
						{
							code[source] = new OpCodeWrapper(OpCodes.Br, code[target - 1].Label);
							for (int j = source + 1; j <= i; j++)
							{
								// We can't depend on DCE for code correctness (we have to maintain all MSIL invariants at all times),
								// so we patch out the unused code.
								code[j] = new OpCodeWrapper(CodeType.Unreachable, null);
							}
						}
					}
				}
			}
		}

		private void OptimizeStackTransfer()
		{
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].opcode == OpCodes.Ldloc
					&& code[i + 1].opcode == OpCodes.Stloc
					&& code[i + 2].pseudo == CodeType.BeginExceptionBlock
					&& code[i + 3].opcode == OpCodes.Ldloc && code[i + 3].MatchLocal(code[i + 1])
					&& code[i + 4].pseudo == CodeType.ReleaseTempLocal && code[i + 4].MatchLocal(code[i + 3]))
				{
					code[i + 1] = code[i];
					code[i] = code[i + 2];
					code.RemoveRange(i + 2, 3);
				}
			}
		}

		private void MergeExceptionBlocks()
		{
			// The first loop will convert all Begin[Exception|Catch|Fault|Finally]Block and EndExceptionBlock
			// pseudo opcodes into a cyclic linked list (EndExceptionBlock links back to BeginExceptionBlock)
			// to allow for easy traversal in the next loop.
			int[] extra = new int[code.Count];
			Stack<int> stack = new Stack<int>();
			int currentBeginExceptionBlock = -1;
			int currentLast = -1;
			for (int i = 0; i < code.Count; i++)
			{
				switch (code[i].pseudo)
				{
					case CodeType.BeginExceptionBlock:
						stack.Push(currentBeginExceptionBlock);
						currentBeginExceptionBlock = i;
						currentLast = i;
						break;
					case CodeType.EndExceptionBlock:
						extra[currentLast] = i;
						extra[i] = currentBeginExceptionBlock;
						currentBeginExceptionBlock = stack.Pop();
						currentLast = currentBeginExceptionBlock;
						if (currentLast != -1)
						{
							while (extra[currentLast] != 0)
							{
								currentLast = extra[currentLast];
							}
						}
						break;
					case CodeType.BeginCatchBlock:
					case CodeType.BeginFaultBlock:
					case CodeType.BeginFinallyBlock:
						extra[currentLast] = i;
						currentLast = i;
						break;
				}
			}

			// Now we look for consecutive exception blocks that have the same fault handler
			for (int i = 0; i < code.Count - 1; i++)
			{
				if (code[i].pseudo == CodeType.EndExceptionBlock
					&& code[i + 1].pseudo == CodeType.BeginExceptionBlock)
				{
					if (IsFaultOnlyBlock(extra, extra[i]) && IsFaultOnlyBlock(extra, i + 1))
					{
						int beginFault1 = extra[extra[i]];
						int beginFault2 = extra[i + 1];
						int length1 = extra[beginFault1] - beginFault1;
						int length2 = extra[beginFault2] - beginFault2;
						if (length1 == length2 && MatchHandlers(beginFault1, beginFault2, length1))
						{
							// Check if the labels at the start of the handler are reachable from outside
							// of the new combined block.
							for (int j = i + 2; j < beginFault2; j++)
							{
								if (code[j].pseudo == CodeType.OpCode)
								{
									break;
								}
								else if (code[j].pseudo == CodeType.Label)
								{
									if (HasBranchTo(0, extra[i], code[j].Label)
										|| HasBranchTo(beginFault2 + length2, code.Count, code[j].Label))
									{
										goto no_merge;
									}
								}
							}
							// Merge the two blocks by overwritting the first fault block and
							// the BeginExceptionBlock of the second block.
							for (int j = beginFault1; j < i + 2; j++)
							{
								code[j] = new OpCodeWrapper(OpCodes.Nop, null);
							}
							// Repair the linking structure.
							extra[extra[i]] = beginFault2;
							extra[extra[beginFault2]] = extra[i];
						}
					}
				no_merge: ;
				}
			}
		}

		private bool HasBranchTo(int start, int end, CodeEmitterLabel label)
		{
			for (int i = start; i < end; i++)
			{
				if (code[i].HasLabel)
				{
					if (code[i].Label == label)
					{
						return true;
					}
				}
				else if (code[i].opcode == OpCodes.Switch)
				{
					foreach (CodeEmitterLabel swlbl in code[i].Labels)
					{
						if (swlbl == label)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private bool MatchHandlers(int beginFault1, int beginFault2, int length)
		{
			for (int i = 0; i < length; i++)
			{
				if (!code[beginFault1 + i].Match(code[beginFault2 + i]))
				{
					return false;
				}
			}
			return true;
		}

		private bool IsFaultOnlyBlock(int[] extra, int begin)
		{
			return code[extra[begin]].pseudo == CodeType.BeginFaultBlock
				&& code[extra[extra[begin]]].pseudo == CodeType.EndExceptionBlock;
		}

		private void ConvertSynchronizedFaultToFinally()
		{
			bool labelIndexSet = false;
			int start = -1;
			int nest = 0;
			int next = -1;
			for (int i = 0; i < code.Count; i++)
			{
				switch (code[i].pseudo)
				{
					case CodeType.BeginExceptionBlock:
						if (nest == 0)
						{
							start = i;
						}
						else if (nest == 1 && next <= start)
						{
							next = i;
						}
						nest++;
						break;
					case CodeType.BeginCatchBlock:
					case CodeType.BeginFinallyBlock:
						if (nest == 1)
						{
							nest = 0;
							if (next > start)
							{
								// while we were processing the outer block, we encountered a nested BeginExceptionBlock
								// so now that we've failed the outer, restart at the first nested block
								i = start = next;
								nest = 1;
							}
						}
						else
						{
							next = -1;
						}
						break;
					case CodeType.BeginFaultBlock:
						if (nest == 1)
						{
							int beginFault = i;
							if (code[i + 1].pseudo == CodeType.LineNumber)
							{
								i++;
							}
							// check if the fault handler is the synchronized block exit pattern
							if (code[i + 1].opcode == OpCodes.Ldloc
								&& code[i + 2].pseudo == CodeType.MonitorExit
								&& code[i + 3].opcode == OpCodes.Endfinally)
							{
								if (!labelIndexSet)
								{
									labelIndexSet = true;
									SetLabelIndexes();
								}
								// now make two passes through the try block to 1) see if all leave
								// opcodes that leave the try block do a synchronized block exit
								// and 2) patch out the synchronized block exit
								for (int pass = 0; pass < 2; pass++)
								{
									for (int j = start; j < i; j++)
									{
										if (code[j].opcode == OpCodes.Leave)
										{
											int target = code[j].Label.Temp;
											if (target < start || target > i)
											{
												// check if the code preceding the leave matches the fault block
												if ((code[j - 1].opcode == OpCodes.Pop || code[j - 1].opcode == OpCodes.Stloc)
													&& code[j - 2].pseudo == CodeType.MonitorExit
													&& code[j - 3].Match(code[i + 1]))
												{
													if (pass == 1)
													{
														// move the leave to the top of the sequence we're removing
														code[j - 3] = code[j - 1];
														code[j - 2] = code[j - 0];
														code[j - 1] = new OpCodeWrapper(CodeType.Unreachable, CodeTypeFlags.None);
														code[j - 0] = new OpCodeWrapper(CodeType.Unreachable, CodeTypeFlags.None);
													}
												}
												else if (code[j - 1].pseudo == CodeType.MonitorExit
													&& code[j - 2].Match(code[i + 1]))
												{
													if (pass == 1)
													{
														// move the leave to the top of the sequence we're removing
														code[j - 2] = code[j];
														code[j - 1] = new OpCodeWrapper(CodeType.Unreachable, CodeTypeFlags.None);
														code[j - 0] = new OpCodeWrapper(CodeType.Unreachable, CodeTypeFlags.None);
													}
												}
												else
												{
													goto fail;
												}
											}
										}
									}
								}
								// if we end up here, all leaves have been successfully patched,
								// so now we turn the BeginFaultBlock into a BeginFinallyBlock
								code[beginFault] = new OpCodeWrapper(CodeType.BeginFinallyBlock, CodeTypeFlags.None);
							fail: ;
							}
							goto case CodeType.BeginFinallyBlock;
						}
						break;
					case CodeType.EndExceptionBlock:
						nest--;
						break;
				}
			}
		}

		private void RemoveRedundantMemoryBarriers()
		{
			int lastMemoryBarrier = -1;
			for (int i = 0; i < code.Count; i++)
			{
				switch (code[i].pseudo)
				{
					case CodeType.MemoryBarrier:
						if (lastMemoryBarrier != -1)
						{
							code.RemoveAt(lastMemoryBarrier);
							i--;
						}
						lastMemoryBarrier = i;
						break;
					case CodeType.OpCode:
						if (code[i].opcode == OpCodes.Volatile)
						{
							if (code[i + 1].opcode != OpCodes.Stfld && code[i + 1].opcode != OpCodes.Stsfld)
							{
								lastMemoryBarrier = -1;
							}
						}
						else if (code[i].opcode.FlowControl != FlowControl.Next)
						{
							lastMemoryBarrier = -1;
						}
						break;
				}
			}
		}

		private static bool MatchLdarg(OpCodeWrapper opc, out short arg)
		{
			if (opc.opcode == OpCodes.Ldarg)
			{
				arg = opc.ValueInt16;
				return true;
			}
			else if (opc.opcode == OpCodes.Ldarg_S)
			{
				arg = opc.ValueByte;
				return true;
			}
			else if (opc.opcode == OpCodes.Ldarg_0)
			{
				arg = 0;
				return true;
			}
			else if (opc.opcode == OpCodes.Ldarg_1)
			{
				arg = 1;
				return true;
			}
			else if (opc.opcode == OpCodes.Ldarg_2)
			{
				arg = 2;
				return true;
			}
			else if (opc.opcode == OpCodes.Ldarg_3)
			{
				arg = 3;
				return true;
			}
			else
			{
				arg = -1;
				return false;
			}
		}

		private bool IsBranchEqNe(OpCode opcode)
		{
			return opcode == OpCodes.Beq
				|| opcode == OpCodes.Bne_Un;
		}

		private void CLRv4_x64_JIT_Workaround()
		{
			for (int i = 0; i < code.Count - 2; i++)
			{
				// This is a workaround for https://connect.microsoft.com/VisualStudio/feedback/details/566946/x64-jit-optimization-bug
				// 
				// Testing shows that the bug appears to be very specific and requires a comparison of a method argument with zero.
				// For example, the problem goes away when the method argument is first assigned to a local variable and then
				// the comparison (and subsequent use) is done against the local variable.
				//
				// This means we only have to detect these specific patterns:
				//
				//   ldc.i8 0x0        ldarg
				//   ldarg             ldc.i8 0x0
				//   beq/bne           beq/bne
				//
				// The workaround is to replace ldarg with ldarga/ldind.i8. Looking at the generated code by the x86 and x64 JITs
				// this appears to be as efficient as the ldarg and it avoids the x64 bug.
				if (code[i].opcode == OpCodes.Ldc_I8 && code[i].ValueInt64 == 0)
				{
					short arg;
					int m;
					if (i > 0 && MatchLdarg(code[i - 1], out arg) && IsBranchEqNe(code[i + 1].opcode))
					{
						m = i - 1;
					}
					else if (MatchLdarg(code[i + 1], out arg) && IsBranchEqNe(code[i + 2].opcode))
					{
						m = i + 1;
					}
					else
					{
						continue;
					}
					code[m] = new OpCodeWrapper(OpCodes.Ldarga, arg);
					code.Insert(m + 1, new OpCodeWrapper(OpCodes.Ldind_I8, null));
				}
			}
		}

		[Conditional("CHECK_INVARIANTS")]
		private void CheckInvariants()
		{
			CheckInvariantBranchInOrOutOfBlocks();
			CheckInvariantOpCodeUsage();
			CheckInvariantLocalVariables();
		}

		private void CheckInvariantBranchInOrOutOfBlocks()
		{
			/*
			 * We maintain an invariant that a branch (other than an explicit leave)
			 * can never branch out or into an exception block (try or handler).
			 * This is a stronger invariant than requirement by MSIL, because
			 * we also disallow the following sequence:
			 * 
			 *    Br Label0
			 *    ...
			 *    BeginExceptionBlock
			 *    Label0:
			 *    ...
			 *    Br Label0
			 *    
			 * This should be rewritten as:
			 * 
			 *    Br Label0
			 *    ...
			 *    Label0:
			 *    BeginExceptionBlock
			 *    Label1:
			 *    ...
			 *    Br Label1
			 */
			int blockId = 0;
			int nextBlockId = 1;
			Stack<int> blocks = new Stack<int>();
			for (int i = 0; i < code.Count; i++)
			{
				switch (code[i].pseudo)
				{
					case CodeType.Label:
						code[i].Label.Temp = blockId;
						break;
					case CodeType.BeginExceptionBlock:
						blocks.Push(blockId);
						goto case CodeType.BeginFinallyBlock;
					case CodeType.BeginFinallyBlock:
					case CodeType.BeginFaultBlock:
					case CodeType.BeginCatchBlock:
						blockId = nextBlockId++;
						break;
					case CodeType.EndExceptionBlock:
						blockId = blocks.Pop();
						break;
				}
			}
			if (blocks.Count != 0)
			{
				throw new InvalidOperationException("Unbalanced exception blocks");
			}
			blockId = 0;
			nextBlockId = 1;
			for (int i = 0; i < code.Count; i++)
			{
				switch (code[i].pseudo)
				{
					case CodeType.OpCode:
						if (code[i].HasLabel
							&& code[i].opcode != OpCodes.Leave
							&& code[i].Label.Temp != blockId)
						{
							DumpMethod();
							throw new InvalidOperationException("Invalid branch " + code[i].opcode.Name + " at offset " + i + " from block " + blockId + " to " + code[i].Label.Temp);
						}
						break;
					case CodeType.BeginExceptionBlock:
						blocks.Push(blockId);
						goto case CodeType.BeginFinallyBlock;
					case CodeType.BeginFinallyBlock:
					case CodeType.BeginFaultBlock:
					case CodeType.BeginCatchBlock:
						blockId = nextBlockId++;
						break;
					case CodeType.EndExceptionBlock:
						blockId = blocks.Pop();
						break;
				}
			}
		}

		private void CheckInvariantOpCodeUsage()
		{
			for (int i = 0; i < code.Count; i++)
			{
				switch (code[i].opcode.FlowControl)
				{
					case FlowControl.Branch:
					case FlowControl.Cond_Branch:
						if (!code[i].HasLabel && code[i].opcode != OpCodes.Switch)
						{
							throw new InvalidOperationException();
						}
						break;
				}
			}
		}

		private void CheckInvariantLocalVariables()
		{
			List<CodeEmitterLocal> locals = new List<CodeEmitterLocal>();
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].pseudo == CodeType.DeclareLocal)
				{
					if (locals.Contains(code[i].Local))
					{
						throw new InvalidOperationException("Local variable used before declaration");
					}
				}
				else if (code[i].HasLocal)
				{
					locals.Add(code[i].Local);
				}
			}
		}

		private void MoveLocalDeclarationToBeginScope()
		{
			int pos = 0;
			for (int i = 0; i < code.Count; i++)
			{
				switch (code[i].pseudo)
				{
					case CodeType.BeginScope:
						pos = i + 1;
						break;
					case CodeType.DeclareLocal:
						OpCodeWrapper decl = code[i];
						code.RemoveAt(i);
						code.Insert(pos++, decl);
						break;
				}
			}
		}

		internal void DoEmit()
		{
			OptimizePatterns();
			CLRv4_x64_JIT_Workaround();
			RemoveRedundantMemoryBarriers();

			if (experimentalOptimizations)
			{
				CheckInvariants();
				MoveLocalDeclarationToBeginScope();

				for (int i = 0; i < 4; i++)
				{
					RemoveJumpNext();
					CheckInvariants();
					ChaseBranches();
					CheckInvariants();
					RemoveSingletonBranches();
					CheckInvariants();
					RemoveUnusedLabels();
					CheckInvariants();
					SortPseudoOpCodes();
					CheckInvariants();
					AnnihilatePops();
					CheckInvariants();
					AnnihilateStoreReleaseTempLocals();
					CheckInvariants();
					DeduplicateBranchSourceTargetCode();
					CheckInvariants();
					OptimizeStackTransfer();
					CheckInvariants();
					MergeExceptionBlocks();
					CheckInvariants();
					ConvertSynchronizedFaultToFinally();
					CheckInvariants();
					RemoveDeadCode();
					CheckInvariants();
				}
			}

#if STATIC_COMPILER
			OptimizeEncodings();
			OptimizeBranchSizes();
#endif

			int ilOffset = 0;
			int lineNumber = -1;
			for (int i = 0; i < code.Count; i++)
			{
				code[i].RealEmit(ilOffset, this, ref lineNumber);
#if STATIC_COMPILER || NET_4_0
				ilOffset = ilgen_real.ILOffset;
#else
				ilOffset += code[i].Size;
#endif
			}
		}

		internal void DumpMethod()
		{
			Dictionary<CodeEmitterLabel, int> labelIndexes = new Dictionary<CodeEmitterLabel, int>();
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].pseudo == CodeType.Label)
				{
					labelIndexes.Add(code[i].Label, i);
				}
			}
			Console.WriteLine("======================");
			for (int i = 0; i < code.Count; i++)
			{
				if (code[i].pseudo == CodeType.OpCode)
				{
					Console.Write("  " + code[i].opcode.Name);
					if (code[i].HasLabel)
					{
						Console.Write(" label" + labelIndexes[code[i].Label]);
					}
					else if (code[i].opcode == OpCodes.Ldarg_S || code[i].opcode == OpCodes.Ldarga_S)
					{
						Console.Write(" " + code[i].ValueByte);
					}
					else if (code[i].opcode == OpCodes.Ldarg || code[i].opcode == OpCodes.Ldarga)
					{
						Console.Write(" " + code[i].ValueInt16);
					}
					else if (code[i].opcode == OpCodes.Isinst || code[i].opcode == OpCodes.Castclass || code[i].opcode == OpCodes.Box || code[i].opcode == OpCodes.Unbox || code[i].opcode == OpCodes.Ldobj || code[i].opcode == OpCodes.Newarr)
					{
						Console.Write(" " + code[i].Type);
					}
					else if (code[i].opcode == OpCodes.Call || code[i].opcode == OpCodes.Callvirt)
					{
						Console.Write(" " + code[i].MethodBase);
					}
					else if (code[i].opcode == OpCodes.Ldfld || code[i].opcode == OpCodes.Ldsfld || code[i].opcode == OpCodes.Stfld || code[i].opcode == OpCodes.Stsfld)
					{
						Console.Write(" " + code[i].FieldInfo);
					}
					else if (code[i].opcode == OpCodes.Ldc_I4)
					{
						Console.Write(" " + code[i].ValueInt32);
					}
					else if (code[i].opcode == OpCodes.Ldloc || code[i].opcode == OpCodes.Stloc)
					{
						Console.Write(" " + code[i].Local.__LocalIndex);
					}
					Console.WriteLine();
				}
				else if (code[i].pseudo == CodeType.Label)
				{
					Console.WriteLine("label{0}:  // temp = {1}", i, code[i].Label.Temp);
				}
				else if (code[i].pseudo == CodeType.DeclareLocal)
				{
					Console.WriteLine("local #{0} = {1}", code[i].Local.__LocalIndex, code[i].Local.LocalType);
				}
				else
				{
					Console.WriteLine(code[i]);
				}
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
#if !STATIC_COMPILER
			exceptionStack.Push(inFinally);
			inFinally = false;
#endif
			EmitPseudoOpCode(CodeType.BeginExceptionBlock, null);
		}

		internal void BeginFaultBlock()
		{
#if !STATIC_COMPILER
			inFinally = true;
#endif
			EmitPseudoOpCode(CodeType.BeginFaultBlock, null);
		}

		internal void BeginFinallyBlock()
		{
#if !STATIC_COMPILER
			inFinally = true;
#endif
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

		internal void EmitUnaligned(byte alignment)
		{
			EmitOpCode(OpCodes.Unaligned, alignment);
		}

		internal void Emit(OpCode opcode, MethodBase mb)
		{
			EmitOpCode(opcode, mb);
		}

		internal void EmitLdc_R8(double arg)
		{
			EmitOpCode(OpCodes.Ldc_R8, arg);
		}

		internal void Emit(OpCode opcode, FieldInfo field)
		{
			EmitOpCode(opcode, field);
		}

		internal void EmitLdarg(int arg)
		{
			Debug.Assert(0 <= arg && arg < 65536);
			switch (arg)
			{
				case 0:
					EmitOpCode(OpCodes.Ldarg_0, null);
					break;
				case 1:
					EmitOpCode(OpCodes.Ldarg_1, null);
					break;
				case 2:
					EmitOpCode(OpCodes.Ldarg_2, null);
					break;
				case 3:
					EmitOpCode(OpCodes.Ldarg_3, null);
					break;
				default:
					if (arg < 256)
					{
						EmitOpCode(OpCodes.Ldarg_S, (byte)arg);
					}
					else
					{
						EmitOpCode(OpCodes.Ldarg, (short)arg);
					}
					break;
			}
		}

		internal void EmitLdarga(int arg)
		{
			Debug.Assert(0 <= arg && arg < 65536);
			if (arg < 256)
			{
				EmitOpCode(OpCodes.Ldarga_S, (byte)arg);
			}
			else
			{
				EmitOpCode(OpCodes.Ldarga, (short)arg);
			}
		}

		internal void EmitStarg(int arg)
		{
			Debug.Assert(0 <= arg && arg < 65536);
			if (arg < 256)
			{
				EmitOpCode(OpCodes.Starg_S, (byte)arg);
			}
			else
			{
				EmitOpCode(OpCodes.Starg, (short)arg);
			}
		}

		internal void EmitLdc_I8(long arg)
		{
			EmitOpCode(OpCodes.Ldc_I8, arg);
		}

		internal void EmitBr(CodeEmitterLabel label)
		{
			EmitOpCode(OpCodes.Br, label);
		}

		internal void EmitBeq(CodeEmitterLabel label)
		{
			EmitOpCode(OpCodes.Beq, label);
		}

		internal void EmitBne_Un(CodeEmitterLabel label)
		{
			EmitOpCode(OpCodes.Bne_Un, label);
		}

		internal void EmitBle_Un(CodeEmitterLabel label)
		{
			EmitOpCode(OpCodes.Ble_Un, label);
		}

		internal void EmitBlt_Un(CodeEmitterLabel label)
		{
			EmitOpCode(OpCodes.Blt_Un, label);
		}

		internal void EmitBge_Un(CodeEmitterLabel label)
		{
			EmitOpCode(OpCodes.Bge_Un, label);
		}

		internal void EmitBle(CodeEmitterLabel label)
		{
			EmitOpCode(OpCodes.Ble, label);
		}

		internal void EmitBlt(CodeEmitterLabel label)
		{
			EmitOpCode(OpCodes.Blt, label);
		}

		internal void EmitBge(CodeEmitterLabel label)
		{
			EmitOpCode(OpCodes.Bge, label);
		}

		internal void EmitBgt(CodeEmitterLabel label)
		{
			EmitOpCode(OpCodes.Bgt, label);
		}

		internal void EmitBrtrue(CodeEmitterLabel label)
		{
			EmitOpCode(OpCodes.Brtrue, label);
		}

		internal void EmitBrfalse(CodeEmitterLabel label)
		{
			EmitOpCode(OpCodes.Brfalse, label);
		}

		internal void EmitLeave(CodeEmitterLabel label)
		{
			EmitOpCode(OpCodes.Leave, label);
		}

		internal void EmitSwitch(CodeEmitterLabel[] labels)
		{
			EmitOpCode(OpCodes.Switch, labels);
		}

		internal void Emit(OpCode opcode, CodeEmitterLocal local)
		{
			EmitOpCode(opcode, local);
		}

		internal void EmitLdc_R4(float arg)
		{
			EmitOpCode(OpCodes.Ldc_R4, arg);
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

		internal void EndExceptionBlock()
		{
#if STATIC_COMPILER
			EmitPseudoOpCode(CodeType.EndExceptionBlock, null);
#else
			EmitPseudoOpCode(CodeType.EndExceptionBlock, inFinally ? CodeTypeFlags.EndFaultOrFinally : CodeTypeFlags.None);
			inFinally = exceptionStack.Pop();
#endif
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
		internal void EmitLineNumberTable(MethodBuilder mb)
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
				EmitBrtrue(ok);
				Emit(OpCodes.Ldloc, lb);
				EmitBrfalse(ok);	// handle null
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
			CodeEmitterLabel isnull = DefineLabel();
			Emit(OpCodes.Dup);
			EmitBrfalse(isnull);
			Emit(OpCodes.Isinst, type);
			Emit(OpCodes.Dup);
			CodeEmitterLabel ok = DefineLabel();
			EmitBrtrue(ok);
			EmitThrow("java.lang.IncompatibleClassChangeError");
			MarkLabel(isnull);
			Emit(OpCodes.Pop);
			Emit(OpCodes.Ldnull);
			MarkLabel(ok);
		}

		internal void EmitUnboxSpecial(Type type)
		{
			// NOTE if the reference is null, we treat it as a default instance of the value type.
			Emit(OpCodes.Dup);
			CodeEmitterLabel label1 = DefineLabel();
			EmitBrtrue(label1);
			Emit(OpCodes.Pop);
			CodeEmitterLocal local = AllocTempLocal(type);
			Emit(OpCodes.Ldloca, local);
			Emit(OpCodes.Initobj, type);
			Emit(OpCodes.Ldloc, local);
			ReleaseTempLocal(local);
			CodeEmitterLabel label2 = DefineLabel();
			EmitBr(label2);
			MarkLabel(label1);
			Emit(OpCodes.Unbox, type);
			Emit(OpCodes.Ldobj, type);
			MarkLabel(label2);
		}

		internal void EmitLdc_I4(int i)
		{
			EmitOpCode(OpCodes.Ldc_I4, i);
		}

		internal void Emit_idiv()
		{
			// we need to special case dividing by -1, because the CLR div instruction
			// throws an OverflowException when dividing Int32.MinValue by -1, and
			// Java just silently overflows
			Emit(OpCodes.Dup);
			Emit(OpCodes.Ldc_I4_M1);
			CodeEmitterLabel label = DefineLabel();
			EmitBne_Un(label);
			Emit(OpCodes.Pop);
			Emit(OpCodes.Neg);
			CodeEmitterLabel label2 = DefineLabel();
			EmitBr(label2);
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
			EmitBne_Un(label);
			Emit(OpCodes.Pop);
			Emit(OpCodes.Neg);
			CodeEmitterLabel label2 = DefineLabel();
			EmitBr(label2);
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
					EmitBle(label);
					break;
				case Comparison.LessThan:
					EmitBlt(label);
					break;
				case Comparison.GreaterOrEqual:
					EmitBge(label);
					break;
				case Comparison.GreaterThan:
					EmitBgt(label);
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
			EmitLdc_I4(v);
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

		internal void EmitMemoryBarrier()
		{
			EmitPseudoOpCode(CodeType.MemoryBarrier, null);
		}

		internal void EmitTailCallPrevention()
		{
			EmitPseudoOpCode(CodeType.TailCallPrevention, null);
		}

		internal void EmitClearStack()
		{
			EmitPseudoOpCode(CodeType.ClearStack, null);
		}

		internal void EmitMonitorEnter()
		{
			EmitPseudoOpCode(CodeType.MonitorEnter, null);
		}

		internal void EmitMonitorExit()
		{
			EmitPseudoOpCode(CodeType.MonitorExit, null);
		}
	}
}
