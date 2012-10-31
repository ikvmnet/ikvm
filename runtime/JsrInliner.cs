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
using System.Diagnostics;
using InstructionFlags = IKVM.Internal.ClassFile.Method.InstructionFlags;

namespace IKVM.Internal
{
	sealed class JsrInliner
	{
		private ClassFile.Method.Instruction[] codeCopy;
		private int codeLength;
		private InstructionFlags[] flags;
		private readonly ClassFile.Method m;
		private readonly JsrMethodAnalyzer ma;

		internal static void InlineJsrs(ClassLoaderWrapper classLoader, MethodWrapper mw, ClassFile classFile, ClassFile.Method m)
		{
			JsrInliner inliner;
			do
			{
				ClassFile.Method.Instruction[] codeCopy = (ClassFile.Method.Instruction[])m.Instructions.Clone();
				InstructionFlags[] flags = new InstructionFlags[codeCopy.Length];
				JsrMethodAnalyzer ma = new JsrMethodAnalyzer(mw, classFile, m, classLoader, flags);
				inliner = new JsrInliner(codeCopy, flags, m, ma);
			} while (inliner.InlineJsrs());
		}

		private JsrInliner(ClassFile.Method.Instruction[] codeCopy, InstructionFlags[] flags, ClassFile.Method m, JsrMethodAnalyzer ma)
		{
			this.codeCopy = codeCopy;
			codeLength = codeCopy.Length;
			this.flags = flags;
			this.m = m;
			this.ma = ma;
		}

		private void Add(ClassFile.Method.Instruction instr)
		{
			if (codeLength == codeCopy.Length)
			{
				Array.Resize(ref codeCopy, codeLength * 2);
				Array.Resize(ref flags, codeLength * 2);
			}
			codeCopy[codeLength++] = instr;
		}

		private bool InlineJsrs()
		{
			bool hasJsrs = false;
			List<SubroutineCall> subs = new List<SubroutineCall>();
			int len = codeLength;
			for (int i = 0; i < len; i++)
			{
				// note that we're also (needlessly) processing the subroutines here, but that shouldn't be a problem (just a minor waste of cpu)
				// because the code is unreachable anyway
				if ((flags[i] & InstructionFlags.Reachable) != 0 && m.Instructions[i].NormalizedOpCode == NormalizedByteCode.__jsr)
				{
					int subroutineId = m.Instructions[i].TargetIndex;
					codeCopy[i].PatchOpCode(NormalizedByteCode.__goto, codeLength);
					SubroutineCall sub = new SubroutineCall(this, subroutineId, i + 1);
					hasJsrs |= sub.InlineSubroutine();
					subs.Add(sub);
				}
			}
			List<ClassFile.Method.ExceptionTableEntry> exceptions = new List<ClassFile.Method.ExceptionTableEntry>(m.ExceptionTable);
			foreach (SubroutineCall sub in subs)
			{
				sub.DoExceptions(m.ExceptionTable, exceptions);
			}
			m.ExceptionTable = exceptions.ToArray();
			ClassFile.Method.Instruction instr = new ClassFile.Method.Instruction();
			instr.SetTermNop(0xFFFF);
			Add(instr);
			Array.Resize(ref codeCopy, codeLength);

			m.Instructions = codeCopy;
			return hasJsrs;
		}

		private sealed class SubroutineCall
		{
			private readonly JsrInliner inliner;
			private readonly int subroutineIndex;
			private readonly int returnIndex;
			private readonly int[] branchMap;
			private readonly int baseIndex;
			private int endIndex;

			internal SubroutineCall(JsrInliner inliner, int subroutineIndex, int returnIndex)
			{
				this.inliner = inliner;
				this.subroutineIndex = subroutineIndex;
				this.returnIndex = returnIndex;
				baseIndex = inliner.codeLength;
				branchMap = new int[inliner.m.Instructions.Length];
				for (int i = 0; i < branchMap.Length; i++)
				{
					branchMap[i] = i;
				}
			}

			private void Emit(ClassFile.Method.Instruction instr)
			{
				inliner.Add(instr);
			}

			private void EmitGoto(int targetIndex)
			{
				ClassFile.Method.Instruction instr = new ClassFile.Method.Instruction();
				instr.PatchOpCode(NormalizedByteCode.__goto, targetIndex);
				instr.SetPC(-1);
				Emit(instr);
			}

			internal bool InlineSubroutine()
			{
				bool hasJsrs = false;
				// start with a pre-amble to load a dummy return address on the stack and to branch to the subroutine
				{
					// TODO consider exception handling around these instructions
					ClassFile.Method.Instruction instr = new ClassFile.Method.Instruction();
					instr.PatchOpCode(NormalizedByteCode.__aconst_null);
					instr.SetPC(inliner.m.Instructions[subroutineIndex].PC);
					Emit(instr);
					EmitGoto(subroutineIndex);
				}

				bool fallThru = false;
				for (int instructionIndex = 0; instructionIndex < inliner.m.Instructions.Length; instructionIndex++)
				{
					if ((inliner.flags[instructionIndex] & InstructionFlags.Reachable) != 0
						&& inliner.ma.IsSubroutineActive(instructionIndex, subroutineIndex))
					{
						fallThru = false;
						branchMap[instructionIndex] = inliner.codeLength;
						switch (inliner.m.Instructions[instructionIndex].NormalizedOpCode)
						{
							case NormalizedByteCode.__tableswitch:
							case NormalizedByteCode.__lookupswitch:
							case NormalizedByteCode.__ireturn:
							case NormalizedByteCode.__lreturn:
							case NormalizedByteCode.__freturn:
							case NormalizedByteCode.__dreturn:
							case NormalizedByteCode.__areturn:
							case NormalizedByteCode.__return:
							case NormalizedByteCode.__athrow:
							case NormalizedByteCode.__goto:
								Emit(inliner.m.Instructions[instructionIndex]);
								break;
							case NormalizedByteCode.__jsr:
								hasJsrs = true;
								goto default;
							case NormalizedByteCode.__ret:
								{
									int subid = inliner.ma.GetLocalTypeWrapper(instructionIndex, inliner.m.Instructions[instructionIndex].TargetIndex).SubroutineIndex;
									if (subid == subroutineIndex)
									{
										EmitGoto(returnIndex);
									}
									else
									{
										Emit(inliner.m.Instructions[instructionIndex]);
									}
									break;
								}
							default:
								fallThru = true;
								Emit(inliner.m.Instructions[instructionIndex]);
								break;
						}
					}
					else if (fallThru)
					{
						EmitGoto(instructionIndex);
					}
				}

				endIndex = inliner.codeLength;
				DoFixups();
				return hasJsrs;
			}

			private void DoFixups()
			{
				for (int instructionIndex = baseIndex; instructionIndex < endIndex; instructionIndex++)
				{
					switch (inliner.codeCopy[instructionIndex].NormalizedOpCode)
					{
						case NormalizedByteCode.__lookupswitch:
						case NormalizedByteCode.__tableswitch:
							{
								int[] targets = new int[inliner.codeCopy[instructionIndex].SwitchEntryCount];
								for (int i = 0; i < targets.Length; i++)
								{
									targets[i] = branchMap[inliner.codeCopy[instructionIndex].GetSwitchTargetIndex(i)];
								}
								inliner.codeCopy[instructionIndex].SetSwitchTargets(targets);
								inliner.codeCopy[instructionIndex].DefaultTarget = branchMap[inliner.codeCopy[instructionIndex].DefaultTarget];
							}
							break;
						case NormalizedByteCode.__ifeq:
						case NormalizedByteCode.__ifne:
						case NormalizedByteCode.__iflt:
						case NormalizedByteCode.__ifge:
						case NormalizedByteCode.__ifgt:
						case NormalizedByteCode.__ifle:
						case NormalizedByteCode.__if_icmpeq:
						case NormalizedByteCode.__if_icmpne:
						case NormalizedByteCode.__if_icmplt:
						case NormalizedByteCode.__if_icmpge:
						case NormalizedByteCode.__if_icmpgt:
						case NormalizedByteCode.__if_icmple:
						case NormalizedByteCode.__if_acmpeq:
						case NormalizedByteCode.__if_acmpne:
						case NormalizedByteCode.__ifnull:
						case NormalizedByteCode.__ifnonnull:
						case NormalizedByteCode.__goto:
						case NormalizedByteCode.__jsr:
							inliner.codeCopy[instructionIndex].TargetIndex = branchMap[inliner.codeCopy[instructionIndex].TargetIndex];
							break;
					}
				}
			}

			private int MapExceptionStartEnd(int index)
			{
				while (branchMap[index] < baseIndex)
				{
					index++;
					if (index == branchMap.Length)
					{
						return endIndex;
					}
				}
				return branchMap[index];
			}

			internal void DoExceptions(ClassFile.Method.ExceptionTableEntry[] table, List<ClassFile.Method.ExceptionTableEntry> newExceptions)
			{
				foreach (ClassFile.Method.ExceptionTableEntry entry in table)
				{
					int start = MapExceptionStartEnd(entry.startIndex);
					int end = MapExceptionStartEnd(entry.endIndex);
					if (start != end)
					{
						ClassFile.Method.ExceptionTableEntry newEntry = new ClassFile.Method.ExceptionTableEntry(start, end, branchMap[entry.handlerIndex], entry.catch_type, entry.ordinal);
						newExceptions.Add(newEntry);
					}
				}
			}
		}

		class SimpleType
		{
			internal static readonly SimpleType Invalid = null;
			internal static readonly SimpleType Primitive = new SimpleType();
			internal static readonly SimpleType WidePrimitive = new SimpleType();
			internal static readonly SimpleType Object = new SimpleType();
			internal static readonly SimpleType[] EmptyArray = new SimpleType[0];

			private SimpleType() { }

			internal bool IsPrimitive
			{
				get
				{
					return this == SimpleType.Primitive
						|| this == SimpleType.WidePrimitive;
				}
			}

			internal bool IsWidePrimitive
			{
				get
				{
					return this == SimpleType.WidePrimitive;
				}
			}

			private sealed class ReturnAddressType : SimpleType
			{
				internal readonly int subroutineIndex;

				internal ReturnAddressType(int subroutineIndex)
				{
					this.subroutineIndex = subroutineIndex;
				}
			}

			internal static SimpleType MakeRet(int subroutineIndex)
			{
				return new ReturnAddressType(subroutineIndex);
			}

			internal static bool IsRet(SimpleType w)
			{
				return w is ReturnAddressType;
			}

			internal int SubroutineIndex
			{
				get
				{
					return ((ReturnAddressType)this).subroutineIndex;
				}
			}
		}

		sealed class JsrMethodAnalyzer
		{
			private ClassFile classFile;
			private InstructionState[] state;
			private List<int>[] callsites;
			private List<int>[] returnsites;

			internal JsrMethodAnalyzer(MethodWrapper mw, ClassFile classFile, ClassFile.Method method, ClassLoaderWrapper classLoader, InstructionFlags[] flags)
			{
				if (method.VerifyError != null)
				{
					throw new VerifyError(method.VerifyError);
				}

				this.classFile = classFile;
				state = new InstructionState[method.Instructions.Length];
				callsites = new List<int>[method.Instructions.Length];
				returnsites = new List<int>[method.Instructions.Length];

				// because types have to have identity, the subroutine return address types are cached here
				Dictionary<int, SimpleType> returnAddressTypes = new Dictionary<int, SimpleType>();

				try
				{
					// ensure that exception blocks and handlers start and end at instruction boundaries
					for (int i = 0; i < method.ExceptionTable.Length; i++)
					{
						int start = method.ExceptionTable[i].startIndex;
						int end = method.ExceptionTable[i].endIndex;
						int handler = method.ExceptionTable[i].handlerIndex;
						if (start >= end || start == -1 || end == -1 || handler <= 0)
						{
							throw new IndexOutOfRangeException();
						}
					}
				}
				catch (IndexOutOfRangeException)
				{
					throw new ClassFormatError(string.Format("Illegal exception table (class: {0}, method: {1}, signature: {2}", classFile.Name, method.Name, method.Signature));
				}

				// start by computing the initial state, the stack is empty and the locals contain the arguments
				state[0] = new InstructionState(method.MaxLocals, method.MaxStack);
				SimpleType thisType;
				int firstNonArgLocalIndex = 0;
				if (!method.IsStatic)
				{
					thisType = SimpleType.Object;
					state[0].SetLocalType(firstNonArgLocalIndex++, thisType, -1);
				}
				else
				{
					thisType = null;
				}
				TypeWrapper[] argTypeWrappers = mw.GetParameters();
				for (int i = 0; i < argTypeWrappers.Length; i++)
				{
					TypeWrapper tw = argTypeWrappers[i];
					SimpleType type;
					if (tw.IsWidePrimitive)
					{
						type = SimpleType.WidePrimitive;
					}
					else if (tw.IsPrimitive)
					{
						type = SimpleType.Primitive;
					}
					else
					{
						type = SimpleType.Object;
					}
					state[0].SetLocalType(firstNonArgLocalIndex++, type, -1);
					if (type.IsWidePrimitive)
					{
						firstNonArgLocalIndex++;
					}
				}
				SimpleType[] argumentsByLocalIndex = new SimpleType[firstNonArgLocalIndex];
				for (int i = 0; i < argumentsByLocalIndex.Length; i++)
				{
					argumentsByLocalIndex[i] = state[0].GetLocalTypeEx(i);
				}
				InstructionState s = state[0].Copy();
				bool done = false;
				ClassFile.Method.Instruction[] instructions = method.Instructions;
				while (!done)
				{
					done = true;
					for (int i = 0; i < instructions.Length; i++)
					{
						if (state[i] != null && state[i].changed)
						{
							try
							{
								//Console.WriteLine(method.Instructions[i].PC + ": " + method.Instructions[i].OpCode.ToString());
								done = false;
								state[i].changed = false;
								// mark the exception handlers reachable from this instruction
								for (int j = 0; j < method.ExceptionTable.Length; j++)
								{
									if (method.ExceptionTable[j].startIndex <= i && i < method.ExceptionTable[j].endIndex)
									{
										MergeExceptionHandler(method.ExceptionTable[j].handlerIndex, state[i]);
									}
								}
								state[i].CopyTo(s);
								ClassFile.Method.Instruction instr = instructions[i];
								switch (instr.NormalizedOpCode)
								{
									case NormalizedByteCode.__aload:
										{
											SimpleType type = s.GetLocalType(instr.NormalizedArg1);
											if (type == SimpleType.Invalid || type.IsPrimitive)
											{
												throw new VerifyError("Object reference expected");
											}
											s.PushType(type);
											break;
										}
									case NormalizedByteCode.__astore:
										s.SetLocalType(instr.NormalizedArg1, s.PopObjectType(), i);
										break;
									case NormalizedByteCode.__aconst_null:
										s.PushObject();
										break;
									case NormalizedByteCode.__aaload:
										s.PopPrimitive();
										s.PopObjectType();
										s.PushObject();
										break;
									case NormalizedByteCode.__aastore:
										s.PopObjectType();
										s.PopPrimitive();
										s.PopObjectType();
										break;
									case NormalizedByteCode.__baload:
										s.PopPrimitive();
										s.PopObjectType();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__bastore:
										s.PopPrimitive();
										s.PopPrimitive();
										s.PopObjectType();
										break;
									case NormalizedByteCode.__caload:
										s.PopPrimitive();
										s.PopObjectType();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__castore:
										s.PopPrimitive();
										s.PopPrimitive();
										s.PopObjectType();
										break;
									case NormalizedByteCode.__saload:
										s.PopPrimitive();
										s.PopObjectType();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__sastore:
										s.PopPrimitive();
										s.PopPrimitive();
										s.PopObjectType();
										break;
									case NormalizedByteCode.__iaload:
										s.PopPrimitive();
										s.PopObjectType();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__iastore:
										s.PopPrimitive();
										s.PopPrimitive();
										s.PopObjectType();
										break;
									case NormalizedByteCode.__laload:
										s.PopPrimitive();
										s.PopObjectType();
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__lastore:
										s.PopWidePrimitive();
										s.PopPrimitive();
										s.PopObjectType();
										break;
									case NormalizedByteCode.__daload:
										s.PopPrimitive();
										s.PopObjectType();
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__dastore:
										s.PopWidePrimitive();
										s.PopPrimitive();
										s.PopObjectType();
										break;
									case NormalizedByteCode.__faload:
										s.PopPrimitive();
										s.PopObjectType();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__fastore:
										s.PopPrimitive();
										s.PopPrimitive();
										s.PopObjectType();
										break;
									case NormalizedByteCode.__arraylength:
										s.PopObjectType();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__iconst:
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__if_icmpeq:
									case NormalizedByteCode.__if_icmpne:
									case NormalizedByteCode.__if_icmplt:
									case NormalizedByteCode.__if_icmpge:
									case NormalizedByteCode.__if_icmpgt:
									case NormalizedByteCode.__if_icmple:
										s.PopPrimitive();
										s.PopPrimitive();
										break;
									case NormalizedByteCode.__ifeq:
									case NormalizedByteCode.__ifge:
									case NormalizedByteCode.__ifgt:
									case NormalizedByteCode.__ifle:
									case NormalizedByteCode.__iflt:
									case NormalizedByteCode.__ifne:
										s.PopPrimitive();
										break;
									case NormalizedByteCode.__ifnonnull:
									case NormalizedByteCode.__ifnull:
										s.PopObjectType();
										break;
									case NormalizedByteCode.__if_acmpeq:
									case NormalizedByteCode.__if_acmpne:
										s.PopObjectType();
										s.PopObjectType();
										break;
									case NormalizedByteCode.__getstatic:
										s.PushType(GetFieldref(instr.Arg1).Signature);
										break;
									case NormalizedByteCode.__putstatic:
										s.PopType(GetFieldref(instr.Arg1).Signature);
										break;
									case NormalizedByteCode.__getfield:
										s.PopObjectType();
										s.PushType(GetFieldref(instr.Arg1).Signature);
										break;
									case NormalizedByteCode.__putfield:
										s.PopType(GetFieldref(instr.Arg1).Signature);
										s.PopObjectType();
										break;
									case NormalizedByteCode.__ldc:
										{
											switch (GetConstantPoolConstantType(instr.Arg1))
											{
												case ClassFile.ConstantType.Double:
													s.PushWidePrimitive();
													break;
												case ClassFile.ConstantType.Float:
													s.PushPrimitive();
													break;
												case ClassFile.ConstantType.Integer:
													s.PushPrimitive();
													break;
												case ClassFile.ConstantType.Long:
													s.PushWidePrimitive();
													break;
												case ClassFile.ConstantType.String:
												case ClassFile.ConstantType.Class:
													s.PushObject();
													break;
												default:
													// NOTE this is not a VerifyError, because it cannot happen (unless we have
													// a bug in ClassFile.GetConstantPoolConstantType)
													throw new InvalidOperationException();
											}
											break;
										}
									case NormalizedByteCode.__invokevirtual:
									case NormalizedByteCode.__invokespecial:
									case NormalizedByteCode.__invokeinterface:
									case NormalizedByteCode.__invokestatic:
										{
											ClassFile.ConstantPoolItemMI cpi = GetMethodref(instr.Arg1);
											s.MultiPopAnyType(cpi.GetArgTypes().Length);
											if (instr.NormalizedOpCode != NormalizedByteCode.__invokestatic)
											{
												s.PopType();
											}
											string sig = cpi.Signature;
											sig = sig.Substring(sig.IndexOf(')') + 1);
											if (sig != "V")
											{
												s.PushType(sig);
											}
											break;
										}
									case NormalizedByteCode.__goto:
										break;
									case NormalizedByteCode.__istore:
										s.PopPrimitive();
										s.SetLocalPrimitive(instr.NormalizedArg1, i);
										break;
									case NormalizedByteCode.__iload:
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__ineg:
										s.PopPrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__iadd:
									case NormalizedByteCode.__isub:
									case NormalizedByteCode.__imul:
									case NormalizedByteCode.__idiv:
									case NormalizedByteCode.__irem:
									case NormalizedByteCode.__iand:
									case NormalizedByteCode.__ior:
									case NormalizedByteCode.__ixor:
									case NormalizedByteCode.__ishl:
									case NormalizedByteCode.__ishr:
									case NormalizedByteCode.__iushr:
										s.PopPrimitive();
										s.PopPrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__lneg:
										s.PopWidePrimitive();
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__ladd:
									case NormalizedByteCode.__lsub:
									case NormalizedByteCode.__lmul:
									case NormalizedByteCode.__ldiv:
									case NormalizedByteCode.__lrem:
									case NormalizedByteCode.__land:
									case NormalizedByteCode.__lor:
									case NormalizedByteCode.__lxor:
										s.PopWidePrimitive();
										s.PopWidePrimitive();
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__lshl:
									case NormalizedByteCode.__lshr:
									case NormalizedByteCode.__lushr:
										s.PopPrimitive();
										s.PopWidePrimitive();
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__fneg:
										s.PopPrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__fadd:
									case NormalizedByteCode.__fsub:
									case NormalizedByteCode.__fmul:
									case NormalizedByteCode.__fdiv:
									case NormalizedByteCode.__frem:
										s.PopPrimitive();
										s.PopPrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__dneg:
										s.PopWidePrimitive();
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__dadd:
									case NormalizedByteCode.__dsub:
									case NormalizedByteCode.__dmul:
									case NormalizedByteCode.__ddiv:
									case NormalizedByteCode.__drem:
										s.PopWidePrimitive();
										s.PopWidePrimitive();
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__new:
										s.PushObject();
										break;
									case NormalizedByteCode.__multianewarray:
										{
											if (instr.Arg2 < 1)
											{
												throw new VerifyError("Illegal dimension argument");
											}
											for (int j = 0; j < instr.Arg2; j++)
											{
												s.PopPrimitive();
											}
											s.PushObject();
											break;
										}
									case NormalizedByteCode.__anewarray:
										s.PopPrimitive();
										s.PushObject();
										break;
									case NormalizedByteCode.__newarray:
										s.PopPrimitive();
										s.PushObject();
										break;
									case NormalizedByteCode.__swap:
										{
											SimpleType t1 = s.PopType();
											SimpleType t2 = s.PopType();
											s.PushType(t1);
											s.PushType(t2);
											break;
										}
									case NormalizedByteCode.__dup:
										{
											SimpleType t = s.PopType();
											s.PushType(t);
											s.PushType(t);
											break;
										}
									case NormalizedByteCode.__dup2:
										{
											SimpleType t = s.PopAnyType();
											if (t.IsWidePrimitive)
											{
												s.PushType(t);
												s.PushType(t);
											}
											else
											{
												SimpleType t2 = s.PopType();
												s.PushType(t2);
												s.PushType(t);
												s.PushType(t2);
												s.PushType(t);
											}
											break;
										}
									case NormalizedByteCode.__dup_x1:
										{
											SimpleType value1 = s.PopType();
											SimpleType value2 = s.PopType();
											s.PushType(value1);
											s.PushType(value2);
											s.PushType(value1);
											break;
										}
									case NormalizedByteCode.__dup2_x1:
										{
											SimpleType value1 = s.PopAnyType();
											if (value1.IsWidePrimitive)
											{
												SimpleType value2 = s.PopType();
												s.PushType(value1);
												s.PushType(value2);
												s.PushType(value1);
											}
											else
											{
												SimpleType value2 = s.PopType();
												SimpleType value3 = s.PopType();
												s.PushType(value2);
												s.PushType(value1);
												s.PushType(value3);
												s.PushType(value2);
												s.PushType(value1);
											}
											break;
										}
									case NormalizedByteCode.__dup_x2:
										{
											SimpleType value1 = s.PopType();
											SimpleType value2 = s.PopAnyType();
											if (value2.IsWidePrimitive)
											{
												s.PushType(value1);
												s.PushType(value2);
												s.PushType(value1);
											}
											else
											{
												SimpleType value3 = s.PopType();
												s.PushType(value1);
												s.PushType(value3);
												s.PushType(value2);
												s.PushType(value1);
											}
											break;
										}
									case NormalizedByteCode.__dup2_x2:
										{
											SimpleType value1 = s.PopAnyType();
											if (value1.IsWidePrimitive)
											{
												SimpleType value2 = s.PopAnyType();
												if (value2.IsWidePrimitive)
												{
													// Form 4
													s.PushType(value1);
													s.PushType(value2);
													s.PushType(value1);
												}
												else
												{
													// Form 2
													SimpleType value3 = s.PopType();
													s.PushType(value1);
													s.PushType(value3);
													s.PushType(value2);
													s.PushType(value1);
												}
											}
											else
											{
												SimpleType value2 = s.PopType();
												SimpleType value3 = s.PopAnyType();
												if (value3.IsWidePrimitive)
												{
													// Form 3
													s.PushType(value2);
													s.PushType(value1);
													s.PushType(value3);
													s.PushType(value2);
													s.PushType(value1);
												}
												else
												{
													// Form 4
													SimpleType value4 = s.PopType();
													s.PushType(value2);
													s.PushType(value1);
													s.PushType(value4);
													s.PushType(value3);
													s.PushType(value2);
													s.PushType(value1);
												}
											}
											break;
										}
									case NormalizedByteCode.__pop:
										s.PopType();
										break;
									case NormalizedByteCode.__pop2:
										{
											SimpleType type = s.PopAnyType();
											if (!type.IsWidePrimitive)
											{
												s.PopType();
											}
											break;
										}
									case NormalizedByteCode.__monitorenter:
									case NormalizedByteCode.__monitorexit:
										s.PopObjectType();
										break;
									case NormalizedByteCode.__return:
										break;
									case NormalizedByteCode.__areturn:
										s.PopObjectType();
										break;
									case NormalizedByteCode.__ireturn:
										s.PopPrimitive();
										break;
									case NormalizedByteCode.__lreturn:
										s.PopWidePrimitive();
										break;
									case NormalizedByteCode.__freturn:
										s.PopPrimitive();
										break;
									case NormalizedByteCode.__dreturn:
										s.PopWidePrimitive();
										break;
									case NormalizedByteCode.__fload:
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__fstore:
										s.PopPrimitive();
										s.SetLocalPrimitive(instr.NormalizedArg1, i);
										break;
									case NormalizedByteCode.__dload:
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__dstore:
										s.PopWidePrimitive();
										s.SetLocalWidePrimitive(instr.NormalizedArg1, i);
										break;
									case NormalizedByteCode.__lload:
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__lstore:
										s.PopWidePrimitive();
										s.SetLocalWidePrimitive(instr.NormalizedArg1, i);
										break;
									case NormalizedByteCode.__lconst_0:
									case NormalizedByteCode.__lconst_1:
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__fconst_0:
									case NormalizedByteCode.__fconst_1:
									case NormalizedByteCode.__fconst_2:
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__dconst_0:
									case NormalizedByteCode.__dconst_1:
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__lcmp:
										s.PopWidePrimitive();
										s.PopWidePrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__fcmpl:
									case NormalizedByteCode.__fcmpg:
										s.PopPrimitive();
										s.PopPrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__dcmpl:
									case NormalizedByteCode.__dcmpg:
										s.PopWidePrimitive();
										s.PopWidePrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__checkcast:
										s.PopObjectType();
										s.PushObject();
										break;
									case NormalizedByteCode.__instanceof:
										s.PopObjectType();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__iinc:
										break;
									case NormalizedByteCode.__athrow:
										s.PopObjectType();
										break;
									case NormalizedByteCode.__tableswitch:
									case NormalizedByteCode.__lookupswitch:
										s.PopPrimitive();
										break;
									case NormalizedByteCode.__i2b:
										s.PopPrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__i2c:
										s.PopPrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__i2s:
										s.PopPrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__i2l:
										s.PopPrimitive();
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__i2f:
										s.PopPrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__i2d:
										s.PopPrimitive();
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__l2i:
										s.PopWidePrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__l2f:
										s.PopWidePrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__l2d:
										s.PopWidePrimitive();
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__f2i:
										s.PopPrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__f2l:
										s.PopPrimitive();
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__f2d:
										s.PopPrimitive();
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__d2i:
										s.PopWidePrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__d2f:
										s.PopWidePrimitive();
										s.PushPrimitive();
										break;
									case NormalizedByteCode.__d2l:
										s.PopWidePrimitive();
										s.PushWidePrimitive();
										break;
									case NormalizedByteCode.__jsr:
										// TODO make sure we're not calling a subroutine we're already in
										break;
									case NormalizedByteCode.__ret:
										{
											// TODO if we're returning from a higher level subroutine, invalidate
											// all the intermediate return addresses
											int subroutineIndex = s.GetLocalRet(instr.Arg1);
											s.CheckSubroutineActive(subroutineIndex);
											break;
										}
									case NormalizedByteCode.__nop:
										if (i + 1 == instructions.Length)
										{
											throw new VerifyError("Falling off the end of the code");
										}
										break;
									case NormalizedByteCode.__invokedynamic:
										// it is impossible to have a valid invokedynamic in a pre-7.0 class file
										throw new VerifyError("Illegal type in constant pool");
									default:
										throw new NotImplementedException(instr.NormalizedOpCode.ToString());
								}
								if (s.GetStackHeight() > method.MaxStack)
								{
									throw new VerifyError("Stack size too large");
								}
								for (int j = 0; j < method.ExceptionTable.Length; j++)
								{
									if (method.ExceptionTable[j].endIndex == i + 1)
									{
										MergeExceptionHandler(method.ExceptionTable[j].handlerIndex, s);
									}
								}
								try
								{
									// another big switch to handle the opcode targets
									switch (instr.NormalizedOpCode)
									{
										case NormalizedByteCode.__tableswitch:
										case NormalizedByteCode.__lookupswitch:
											for (int j = 0; j < instr.SwitchEntryCount; j++)
											{
												state[instr.GetSwitchTargetIndex(j)] += s;
											}
											state[instr.DefaultTarget] += s;
											break;
										case NormalizedByteCode.__ifeq:
										case NormalizedByteCode.__ifne:
										case NormalizedByteCode.__iflt:
										case NormalizedByteCode.__ifge:
										case NormalizedByteCode.__ifgt:
										case NormalizedByteCode.__ifle:
										case NormalizedByteCode.__if_icmpeq:
										case NormalizedByteCode.__if_icmpne:
										case NormalizedByteCode.__if_icmplt:
										case NormalizedByteCode.__if_icmpge:
										case NormalizedByteCode.__if_icmpgt:
										case NormalizedByteCode.__if_icmple:
										case NormalizedByteCode.__if_acmpeq:
										case NormalizedByteCode.__if_acmpne:
										case NormalizedByteCode.__ifnull:
										case NormalizedByteCode.__ifnonnull:
											state[i + 1] += s;
											state[instr.TargetIndex] += s;
											break;
										case NormalizedByteCode.__goto:
											state[instr.TargetIndex] += s;
											break;
										case NormalizedByteCode.__jsr:
											{
												int index = instr.TargetIndex;
												s.SetSubroutineId(index);
												SimpleType retAddressType;
												if (!returnAddressTypes.TryGetValue(index, out retAddressType))
												{
													retAddressType = SimpleType.MakeRet(index);
													returnAddressTypes[index] = retAddressType;
												}
												s.PushType(retAddressType);
												state[index] += s;
												List<int> returns = GetReturnSites(i);
												if (returns != null)
												{
													foreach (int returnIndex in returns)
													{
														state[i + 1] = InstructionState.MergeSubroutineReturn(state[i + 1], s, state[returnIndex], state[returnIndex].GetLocalsModified(index));
													}
												}
												AddCallSite(index, i);
												break;
											}
										case NormalizedByteCode.__ret:
											{
												// HACK if the ret is processed before all of the jsr instructions to this subroutine
												// we wouldn't be able to properly merge, so that is why we track the number of callsites
												// for each subroutine instruction (see Instruction.AddCallSite())
												int subroutineIndex = s.GetLocalRet(instr.Arg1);
												int[] cs = GetCallSites(subroutineIndex);
												bool[] locals_modified = s.GetLocalsModified(subroutineIndex);
												for (int j = 0; j < cs.Length; j++)
												{
													AddReturnSite(cs[j], i);
													state[cs[j] + 1] = InstructionState.MergeSubroutineReturn(state[cs[j] + 1], state[cs[j]], s, locals_modified);
												}
												break;
											}
										case NormalizedByteCode.__ireturn:
										case NormalizedByteCode.__lreturn:
										case NormalizedByteCode.__freturn:
										case NormalizedByteCode.__dreturn:
										case NormalizedByteCode.__areturn:
										case NormalizedByteCode.__return:
										case NormalizedByteCode.__athrow:
											break;
										default:
											state[i + 1] += s;
											break;
									}
								}
								catch (IndexOutOfRangeException)
								{
									// we're going to assume that this always means that we have an invalid branch target
									// NOTE because PcIndexMap returns -1 for illegal PCs (in the middle of an instruction) and
									// we always use that value as an index into the state array, any invalid PC will result
									// in an IndexOutOfRangeException
									throw new VerifyError("Illegal target of jump or branch");
								}
							}
							catch (VerifyError x)
							{
								string opcode = instructions[i].NormalizedOpCode.ToString();
								if (opcode.StartsWith("__"))
								{
									opcode = opcode.Substring(2);
								}
								throw new VerifyError(string.Format("{5} (class: {0}, method: {1}, signature: {2}, offset: {3}, instruction: {4})",
									classFile.Name, method.Name, method.Signature, instructions[i].PC, opcode, x.Message), x);
							}
						}
					}
				}

				// Now we do another pass to compute reachability
				done = false;
				flags[0] |= InstructionFlags.Reachable;
				while (!done)
				{
					done = true;
					bool didJsrOrRet = false;
					for (int i = 0; i < instructions.Length; i++)
					{
						if ((flags[i] & (InstructionFlags.Reachable | InstructionFlags.Processed)) == InstructionFlags.Reachable)
						{
							done = false;
							flags[i] |= InstructionFlags.Processed;
							// mark the exception handlers reachable from this instruction
							for (int j = 0; j < method.ExceptionTable.Length; j++)
							{
								if (method.ExceptionTable[j].startIndex <= i && i < method.ExceptionTable[j].endIndex)
								{
									flags[method.ExceptionTable[j].handlerIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
								}
							}
							// mark the successor instructions
							switch (instructions[i].NormalizedOpCode)
							{
								case NormalizedByteCode.__tableswitch:
								case NormalizedByteCode.__lookupswitch:
									{
										bool hasbackbranch = false;
										for (int j = 0; j < instructions[i].SwitchEntryCount; j++)
										{
											hasbackbranch |= instructions[i].GetSwitchTargetIndex(j) < i;
											flags[instructions[i].GetSwitchTargetIndex(j)] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
										}
										hasbackbranch |= instructions[i].DefaultTarget < i;
										flags[instructions[i].DefaultTarget] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
										break;
									}
								case NormalizedByteCode.__goto:
									flags[instructions[i].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
									break;
								case NormalizedByteCode.__ifeq:
								case NormalizedByteCode.__ifne:
								case NormalizedByteCode.__iflt:
								case NormalizedByteCode.__ifge:
								case NormalizedByteCode.__ifgt:
								case NormalizedByteCode.__ifle:
								case NormalizedByteCode.__if_icmpeq:
								case NormalizedByteCode.__if_icmpne:
								case NormalizedByteCode.__if_icmplt:
								case NormalizedByteCode.__if_icmpge:
								case NormalizedByteCode.__if_icmpgt:
								case NormalizedByteCode.__if_icmple:
								case NormalizedByteCode.__if_acmpeq:
								case NormalizedByteCode.__if_acmpne:
								case NormalizedByteCode.__ifnull:
								case NormalizedByteCode.__ifnonnull:
									flags[instructions[i].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
									flags[i + 1] |= InstructionFlags.Reachable;
									break;
								case NormalizedByteCode.__jsr:
									flags[instructions[i].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
									// Note that we don't mark the next instruction as reachable,
									// because that depends on the corresponding ret actually being
									// reachable. We handle this in the loop below.
									didJsrOrRet = true;
									break;
								case NormalizedByteCode.__ret:
									// Note that we can't handle ret here, because we might encounter the ret
									// before having seen all the corresponding jsr instructions, so we can't
									// update all the call sites.
									// We handle ret in the loop below.
									didJsrOrRet = true;
									break;
								case NormalizedByteCode.__ireturn:
								case NormalizedByteCode.__lreturn:
								case NormalizedByteCode.__freturn:
								case NormalizedByteCode.__dreturn:
								case NormalizedByteCode.__areturn:
								case NormalizedByteCode.__return:
								case NormalizedByteCode.__athrow:
									break;
								default:
									flags[i + 1] |= InstructionFlags.Reachable;
									break;
							}
						}
					}
					if (didJsrOrRet)
					{
						for (int i = 0; i < instructions.Length; i++)
						{
							if (instructions[i].NormalizedOpCode == NormalizedByteCode.__ret
								&& (flags[i] & InstructionFlags.Reachable) != 0)
							{
								int subroutineIndex = state[i].GetLocalRet(instructions[i].Arg1);
								int[] cs = GetCallSites(subroutineIndex);
								for (int j = 0; j < cs.Length; j++)
								{
									if ((flags[cs[j]] & InstructionFlags.Reachable) != 0)
									{
										flags[cs[j] + 1] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
									}
								}
							}
						}
					}
				}
			}

			private void MergeExceptionHandler(int handlerIndex, InstructionState curr)
			{
				// NOTE this used to be CopyLocalsAndSubroutines, but it doesn't (always) make
				// sense to copy the subroutine state
				// TODO figure out if there are circumstances under which it does make sense
				// to copy the active subroutine state
				// UPDATE subroutines must be copied as well, but I think I now have a better
				// understanding of subroutine merges, so the problems that triggered the previous
				// change here hopefully won't arise anymore
				InstructionState ex = curr.CopyLocalsAndSubroutines();
				ex.PushObject();
				state[handlerIndex] += ex;
			}

			private ClassFile.ConstantPoolItemMI GetMethodref(int index)
			{
				try
				{
					ClassFile.ConstantPoolItemMI item = classFile.GetMethodref(index);
					if (item != null)
					{
						return item;
					}
				}
				catch (InvalidCastException)
				{
				}
				catch (IndexOutOfRangeException)
				{
				}
				throw new VerifyError("Illegal constant pool index");
			}

			private ClassFile.ConstantPoolItemFieldref GetFieldref(int index)
			{
				try
				{
					ClassFile.ConstantPoolItemFieldref item = classFile.GetFieldref(index);
					if (item != null)
					{
						return item;
					}
				}
				catch (InvalidCastException)
				{
				}
				catch (IndexOutOfRangeException)
				{
				}
				throw new VerifyError("Illegal constant pool index");
			}

			private ClassFile.ConstantType GetConstantPoolConstantType(int index)
			{
				try
				{
					return classFile.GetConstantPoolConstantType(index);
				}
				catch (IndexOutOfRangeException)
				{
					// constant pool index out of range
				}
				catch (InvalidOperationException)
				{
					// specified constant pool entry doesn't contain a constant
				}
				catch (NullReferenceException)
				{
					// specified constant pool entry is empty (entry 0 or the filler following a wide entry)
				}
				throw new VerifyError("Illegal constant pool index");
			}

			private void AddReturnSite(int callSiteIndex, int returnSiteIndex)
			{
				if (returnsites[callSiteIndex] == null)
				{
					returnsites[callSiteIndex] = new List<int>();
				}
				List<int> l = returnsites[callSiteIndex];
				if (l.IndexOf(returnSiteIndex) == -1)
				{
					state[callSiteIndex].changed = true;
					l.Add(returnSiteIndex);
				}
			}

			private List<int> GetReturnSites(int callSiteIndex)
			{
				return returnsites[callSiteIndex];
			}

			private void AddCallSite(int subroutineIndex, int callSiteIndex)
			{
				if (callsites[subroutineIndex] == null)
				{
					callsites[subroutineIndex] = new List<int>();
				}
				List<int> l = callsites[subroutineIndex];
				if (l.IndexOf(callSiteIndex) == -1)
				{
					l.Add(callSiteIndex);
					state[subroutineIndex].AddCallSite();
				}
			}

			private int[] GetCallSites(int subroutineIndex)
			{
				return callsites[subroutineIndex].ToArray();
			}

			internal SimpleType GetLocalTypeWrapper(int index, int local)
			{
				return state[index].GetLocalTypeEx(local);
			}

			internal bool IsSubroutineActive(int instructionIndex, int subroutineIndex)
			{
				return state[instructionIndex].IsSubroutineActive(subroutineIndex);
			}

			sealed class Subroutine
			{
				private int subroutineIndex;
				private bool[] localsModified;

				private Subroutine(int subroutineIndex, bool[] localsModified)
				{
					this.subroutineIndex = subroutineIndex;
					this.localsModified = localsModified;
				}

				internal Subroutine(int subroutineIndex, int maxLocals)
				{
					this.subroutineIndex = subroutineIndex;
					localsModified = new bool[maxLocals];
				}

				internal int SubroutineIndex
				{
					get
					{
						return subroutineIndex;
					}
				}

				internal bool[] LocalsModified
				{
					get
					{
						return localsModified;
					}
				}

				internal void SetLocalModified(int local)
				{
					localsModified[local] = true;
				}

				internal Subroutine Copy()
				{
					return new Subroutine(subroutineIndex, (bool[])localsModified.Clone());
				}
			}

			sealed class InstructionState
			{
				private SimpleType[] stack;
				private int stackSize;
				private int stackEnd;
				private SimpleType[] locals;
				private List<Subroutine> subroutines;
				private int callsites;
				internal bool changed = true;
				private enum ShareFlags : byte
				{
					None = 0,
					Stack = 1,
					Locals = 2,
					Subroutines = 4,
					All = Stack | Locals | Subroutines
				}
				private ShareFlags flags;

				private InstructionState(SimpleType[] stack, int stackSize, int stackEnd, SimpleType[] locals, List<Subroutine> subroutines, int callsites)
				{
					this.flags = ShareFlags.All;
					this.stack = stack;
					this.stackSize = stackSize;
					this.stackEnd = stackEnd;
					this.locals = locals;
					this.subroutines = subroutines;
					this.callsites = callsites;
				}

				internal InstructionState(int maxLocals, int maxStack)
				{
					this.flags = ShareFlags.None;
					this.stack = new SimpleType[maxStack];
					this.stackEnd = maxStack;
					this.locals = new SimpleType[maxLocals];
				}

				internal InstructionState Copy()
				{
					return new InstructionState(stack, stackSize, stackEnd, locals, subroutines, callsites);
				}

				internal void CopyTo(InstructionState target)
				{
					target.flags = ShareFlags.All;
					target.stack = stack;
					target.stackSize = stackSize;
					target.stackEnd = stackEnd;
					target.locals = locals;
					target.subroutines = subroutines;
					target.callsites = callsites;
					target.changed = true;
				}

				internal InstructionState CopyLocalsAndSubroutines()
				{
					InstructionState copy = new InstructionState(new SimpleType[stack.Length], 0, stack.Length, locals, subroutines, callsites);
					copy.flags &= ~ShareFlags.Stack;
					return copy;
				}

				private static List<Subroutine> CopySubroutines(List<Subroutine> l)
				{
					if (l == null)
					{
						return null;
					}
					List<Subroutine> n = new List<Subroutine>(l.Count);
					foreach (Subroutine s in l)
					{
						n.Add(s.Copy());
					}
					return n;
				}

				private void MergeSubroutineHelper(InstructionState s2)
				{
					if (subroutines == null || s2.subroutines == null)
					{
						if (subroutines != null)
						{
							subroutines = null;
							changed = true;
						}
					}
					else
					{
						SubroutinesCopyOnWrite();
						List<Subroutine> ss1 = subroutines;
						subroutines = new List<Subroutine>();
						foreach (Subroutine ss2 in s2.subroutines)
						{
							foreach (Subroutine ss in ss1)
							{
								if (ss.SubroutineIndex == ss2.SubroutineIndex)
								{
									subroutines.Add(ss);
									for (int i = 0; i < ss.LocalsModified.Length; i++)
									{
										if (ss2.LocalsModified[i] && !ss.LocalsModified[i])
										{
											ss.LocalsModified[i] = true;
											changed = true;
										}
									}
								}
							}
						}
						if (ss1.Count != subroutines.Count)
						{
							changed = true;
						}
					}

					if (s2.callsites > callsites)
					{
						//Console.WriteLine("s2.callsites = {0}, callsites = {1}", s2.callsites, callsites);
						callsites = s2.callsites;
						changed = true;
					}
				}

				internal static InstructionState MergeSubroutineReturn(InstructionState jsrSuccessor, InstructionState jsr, InstructionState ret, bool[] locals_modified)
				{
					InstructionState next = ret.Copy();
					next.LocalsCopyOnWrite();
					for (int i = 0; i < locals_modified.Length; i++)
					{
						if (!locals_modified[i])
						{
							next.locals[i] = jsr.locals[i];
						}
					}
					next.flags |= ShareFlags.Subroutines;
					next.subroutines = jsr.subroutines;
					next.callsites = jsr.callsites;
					return jsrSuccessor + next;
				}

				public static InstructionState operator+(InstructionState s1, InstructionState s2)
				{
					if (s1 == null)
					{
						return s2.Copy();
					}
					if (s1.stackSize != s2.stackSize || s1.stackEnd != s2.stackEnd)
					{
						throw new VerifyError(string.Format("Inconsistent stack height: {0} != {1}",
							s1.stackSize + s1.stack.Length - s1.stackEnd,
							s2.stackSize + s2.stack.Length - s2.stackEnd));
					}
					InstructionState s = s1.Copy();
					s.changed = s1.changed;
					for (int i = 0; i < s.stackSize; i++)
					{
						SimpleType type = s.stack[i];
						SimpleType type2 = s2.stack[i];
						if (type == type2)
						{
							// perfect match, nothing to do
						}
						else if (!type.IsPrimitive)
						{
							SimpleType baseType = InstructionState.FindCommonBaseType(type, type2);
							if (baseType == SimpleType.Invalid)
							{
								if (SimpleType.IsRet(type) && SimpleType.IsRet(type2))
								{
									// if we never return from a subroutine, it is legal to merge to subroutine flows
									// (this is from the Mauve test subr.pass.mergeok)
								}
								else
								{
									throw new VerifyError(string.Format("cannot merge {0} and {1}", type, type2));
								}
							}
							if (type != baseType)
							{
								s.StackCopyOnWrite();
								s.stack[i] = baseType;
								s.changed = true;
							}
						}
						else
						{
							throw new VerifyError(string.Format("cannot merge {0} and {1}", type, type2));
						}
					}
					for (int i = 0; i < s.locals.Length; i++)
					{
						SimpleType type = s.locals[i];
						SimpleType type2 = s2.locals[i];
						SimpleType baseType = InstructionState.FindCommonBaseType(type, type2);
						if (type != baseType)
						{
							s.LocalsCopyOnWrite();
							s.locals[i] = baseType;
							s.changed = true;
						}
					}
					s.MergeSubroutineHelper(s2);
					return s;
				}

				internal void AddCallSite()
				{
					callsites++;
					changed = true;
				}

				internal void SetSubroutineId(int subroutineIndex)
				{
					SubroutinesCopyOnWrite();
					if (subroutines == null)
					{
						subroutines = new List<Subroutine>();
					}
					else
					{
						foreach (Subroutine s in subroutines)
						{
							if (s.SubroutineIndex == subroutineIndex)
							{
								// subroutines cannot recursivly call themselves
								throw new VerifyError("subroutines cannot recurse");
							}
						}
					}
					subroutines.Add(new Subroutine(subroutineIndex, locals.Length));
				}

				internal bool[] GetLocalsModified(int subroutineIndex)
				{
					if (subroutines != null)
					{
						foreach (Subroutine s in subroutines)
						{
							if (s.SubroutineIndex == subroutineIndex)
							{
								return s.LocalsModified;
							}
						}
					}
					throw new VerifyError("return from wrong subroutine");
				}

				internal bool IsSubroutineActive(int subroutineIndex)
				{
					if (subroutines != null)
					{
						foreach (Subroutine s in subroutines)
						{
							if (s.SubroutineIndex == subroutineIndex)
							{
								return true;
							}
						}
					}
					return false;
				}

				internal void CheckSubroutineActive(int subroutineIndex)
				{
					if (!IsSubroutineActive(subroutineIndex))
					{
						throw new VerifyError("inactive subroutine");
					}
				}

				internal static SimpleType FindCommonBaseType(SimpleType type1, SimpleType type2)
				{
					if (type1 == type2)
					{
						return type1;
					}
					if (type1 == SimpleType.Object)
					{
						return type2;
					}
					if (type2 == SimpleType.Object)
					{
						return type1;
					}
					if (type1 == SimpleType.Invalid || type2 == SimpleType.Invalid)
					{
						return SimpleType.Invalid;
					}
					if (type1.IsPrimitive || type2.IsPrimitive)
					{
						return SimpleType.Invalid;
					}
					if (SimpleType.IsRet(type1) || SimpleType.IsRet(type2))
					{
						return SimpleType.Invalid;
					}
					return SimpleType.Object;
				}

				private void SetLocal1(int index, SimpleType type)
				{
					try
					{
						LocalsCopyOnWrite();
						SubroutinesCopyOnWrite();
						if (index > 0 && locals[index - 1] != SimpleType.Invalid && locals[index - 1].IsWidePrimitive)
						{
							locals[index - 1] = SimpleType.Invalid;
							if (subroutines != null)
							{
								foreach (Subroutine s in subroutines)
								{
									s.SetLocalModified(index - 1);
								}
							}
						}
						locals[index] = type;
						if (subroutines != null)
						{
							foreach (Subroutine s in subroutines)
							{
								s.SetLocalModified(index);
							}
						}
					}
					catch (IndexOutOfRangeException)
					{
						throw new VerifyError("Illegal local variable number");
					}
				}

				private void SetLocal2(int index, SimpleType type)
				{
					try
					{
						LocalsCopyOnWrite();
						SubroutinesCopyOnWrite();
						if (index > 0 && locals[index - 1] != SimpleType.Invalid && locals[index - 1].IsWidePrimitive)
						{
							locals[index - 1] = SimpleType.Invalid;
							if (subroutines != null)
							{
								foreach (Subroutine s in subroutines)
								{
									s.SetLocalModified(index - 1);
								}
							}
						}
						locals[index] = type;
						locals[index + 1] = SimpleType.Invalid;
						if (subroutines != null)
						{
							foreach (Subroutine s in subroutines)
							{
								s.SetLocalModified(index);
								s.SetLocalModified(index + 1);
							}
						}
					}
					catch (IndexOutOfRangeException)
					{
						throw new VerifyError("Illegal local variable number");
					}
				}

				internal void SetLocalPrimitive(int index, int instructionIndex)
				{
					SetLocal1(index, SimpleType.Primitive);
				}

				internal void SetLocalWidePrimitive(int index, int instructionIndex)
				{
					SetLocal2(index, SimpleType.WidePrimitive);
				}

				internal SimpleType GetLocalType(int index)
				{
					try
					{
						return locals[index];
					}
					catch (IndexOutOfRangeException)
					{
						throw new VerifyError("Illegal local variable number");
					}
				}

				// this is used by the compiler (indirectly, through MethodAnalyzer.GetLocalTypeWrapper),
				// we've already verified the code so we know we won't run outside the array boundary,
				// and we don't need to record the fact that we're reading the local.
				internal SimpleType GetLocalTypeEx(int index)
				{
					return locals[index];
				}

				internal int GetLocalRet(int index)
				{
					SimpleType type = GetLocalType(index);
					if (SimpleType.IsRet(type))
					{
						return type.SubroutineIndex;
					}
					throw new VerifyError("incorrect local type, not ret");
				}

				internal void SetLocalType(int index, SimpleType type, int instructionIndex)
				{
					if (type.IsWidePrimitive)
					{
						SetLocalWidePrimitive(index, instructionIndex);
					}
					else
					{
						SetLocal1(index, type);
					}
				}

				internal void PushType(string signature)
				{
					switch (signature[0])
					{
						case 'J':
						case 'D':
							PushWidePrimitive();
							break;
						case '[':
						case 'L':
							PushObject();
							break;
						default:
							PushPrimitive();
							break;
					}
				}

				internal void PushWidePrimitive()
				{
					PushType(SimpleType.WidePrimitive);
				}

				internal void PushPrimitive()
				{
					PushType(SimpleType.Primitive);
				}

				internal void PushObject()
				{
					PushType(SimpleType.Object);
				}

				// object reference or a subroutine return address
				internal SimpleType PopObjectType()
				{
					SimpleType type = PopType();
					if (type.IsPrimitive)
					{
						throw new VerifyError("Expected object reference on stack");
					}
					return type;
				}

				internal void MultiPopAnyType(int count)
				{
					while (count-- != 0)
					{
						PopAnyType();
					}
				}

				internal SimpleType PopAnyType()
				{
					if (stackSize == 0)
					{
						throw new VerifyError("Unable to pop operand off an empty stack");
					}
					SimpleType type = stack[--stackSize];
					if (type.IsWidePrimitive)
					{
						stackEnd++;
					}
					return type;
				}

				// NOTE this can *not* be used to pop double or long
				internal SimpleType PopType()
				{
					SimpleType type = PopAnyType();
					if (type.IsWidePrimitive)
					{
						throw new VerifyError("Attempt to split long or double on the stack");
					}
					return type;
				}

				internal void PopPrimitive()
				{
					if (!PopType().IsPrimitive)
					{
						throw new VerifyError("Primitive type expected on stack");
					}
				}

				internal void PopWidePrimitive()
				{
					SimpleType type = PopAnyType();
					if (type != SimpleType.WidePrimitive)
					{
						throw new VerifyError("Wide primitive type expected on stack");
					}
				}

				internal void PopType(string signature)
				{
					switch (signature[0])
					{
						case 'J':
						case 'D':
							PopWidePrimitive();
							break;
						case '[':
						case 'L':
							PopObjectType();
							break;
						default:
							PopPrimitive();
							break;
					}
				}

				internal int GetStackHeight()
				{
					return stackSize;
				}

				internal void PushType(SimpleType type)
				{
					if (type.IsWidePrimitive)
					{
						stackEnd--;
					}
					if (stackSize >= stackEnd)
					{
						throw new VerifyError("Stack overflow");
					}
					StackCopyOnWrite();
					stack[stackSize++] = type;
				}

				private void StackCopyOnWrite()
				{
					if ((flags & ShareFlags.Stack) != 0)
					{
						flags &= ~ShareFlags.Stack;
						stack = (SimpleType[])stack.Clone();
					}
				}

				private void LocalsCopyOnWrite()
				{
					if ((flags & ShareFlags.Locals) != 0)
					{
						flags &= ~ShareFlags.Locals;
						locals = (SimpleType[])locals.Clone();
					}
				}

				private void SubroutinesCopyOnWrite()
				{
					if ((flags & ShareFlags.Subroutines) != 0)
					{
						flags &= ~ShareFlags.Subroutines;
						subroutines = CopySubroutines(subroutines);
					}
				}

				internal void DumpLocals()
				{
					Console.Write("// ");
					string sep = "";
					for (int i = 0; i < locals.Length; i++)
					{
						Console.Write(sep);
						Console.Write(locals[i]);
						sep = ", ";
					}
					Console.WriteLine();
				}

				internal void DumpStack()
				{
					Console.Write("// ");
					string sep = "";
					for (int i = 0; i < stackSize; i++)
					{
						Console.Write(sep);
						Console.Write(stack[i]);
						sep = ", ";
					}
					Console.WriteLine();
				}

				internal void DumpSubroutines()
				{
					Console.Write("// subs: ");
					string sep = "";
					if (subroutines != null)
					{
						for (int i = 0; i < subroutines.Count; i++)
						{
							Console.Write(sep);
							Console.Write(((Subroutine)subroutines[i]).SubroutineIndex);
							sep = ", ";
						}
					}
					Console.WriteLine();
				}
			}
		}
	}
}
