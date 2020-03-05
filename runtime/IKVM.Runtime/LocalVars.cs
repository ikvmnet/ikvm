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
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
#if STATIC_COMPILER
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection.Emit;
#endif
using IKVM.Internal;
using InstructionFlags = IKVM.Internal.ClassFile.Method.InstructionFlags;
using ExceptionTableEntry = IKVM.Internal.ClassFile.Method.ExceptionTableEntry;

sealed class LocalVar
{
	internal bool isArg;
	internal int local;
	internal TypeWrapper type;
	internal CodeEmitterLocal builder;
	// used to emit debugging info, only available if ClassLoaderWrapper.EmitDebugInfo is true
	internal string name;
	internal int start_pc;
	internal int end_pc;
}

struct LocalVarInfo
{
	private readonly LocalVar[/*instructionIndex*/] localVars;
	private readonly LocalVar[/*instructionIndex*/][/*localIndex*/] invokespecialLocalVars;
	private readonly LocalVar[/*index*/] allLocalVars;

	internal LocalVarInfo(CodeInfo ma, ClassFile classFile, ClassFile.Method method, UntangledExceptionTable exceptions, MethodWrapper mw, ClassLoaderWrapper classLoader)
	{
		Dictionary<int, string>[] localStoreReaders = FindLocalVariables(ma, mw, classFile, method);

		// now that we've done the code flow analysis, we can do a liveness analysis on the local variables
		ClassFile.Method.Instruction[] instructions = method.Instructions;
		Dictionary<long, LocalVar> localByStoreSite = new Dictionary<long, LocalVar>();
		List<LocalVar> locals = new List<LocalVar>();
		for (int i = 0; i < localStoreReaders.Length; i++)
		{
			if (localStoreReaders[i] != null)
			{
				VisitLocalLoads(ma, method, locals, localByStoreSite, localStoreReaders[i], i, classLoader.EmitDebugInfo);
			}
		}
		Dictionary<LocalVar, LocalVar> forwarders = new Dictionary<LocalVar, LocalVar>();
		if (classLoader.EmitDebugInfo)
		{
			InstructionFlags[] flags = MethodAnalyzer.ComputePartialReachability(ma, method.Instructions, exceptions, 0, false);
			// if we're emitting debug info, we need to keep dead stores as well...
			for (int i = 0; i < instructions.Length; i++)
			{
				if ((flags[i] & InstructionFlags.Reachable) != 0
					&& IsStoreLocal(instructions[i].NormalizedOpCode))
				{
					if (!localByStoreSite.ContainsKey(MakeKey(i, instructions[i].NormalizedArg1)))
					{
						LocalVar v = new LocalVar();
						v.local = instructions[i].NormalizedArg1;
						v.type = ma.GetStackTypeWrapper(i, 0);
						FindLvtEntry(v, method, i);
						locals.Add(v);
						localByStoreSite.Add(MakeKey(i, v.local), v);
					}
				}
			}
			// to make the debugging experience better, we have to trust the
			// LocalVariableTable (unless it's clearly bogus) and merge locals
			// together that are the same according to the LVT
			for (int i = 0; i < locals.Count - 1; i++)
			{
				for (int j = i + 1; j < locals.Count; j++)
				{
					LocalVar v1 = (LocalVar)locals[i];
					LocalVar v2 = (LocalVar)locals[j];
					if (v1.name != null && v1.name == v2.name && v1.start_pc == v2.start_pc && v1.end_pc == v2.end_pc)
					{
						// we can only merge if the resulting type is valid (this protects against incorrect
						// LVT data, but is also needed for constructors, where the uninitialized this is a different
						// type from the initialized this)
						TypeWrapper tw = InstructionState.FindCommonBaseType(v1.type, v2.type);
						if (tw != VerifierTypeWrapper.Invalid)
						{
							v1.isArg |= v2.isArg;
							v1.type = tw;
							forwarders.Add(v2, v1);
							locals.RemoveAt(j);
							j--;
						}
					}
				}
			}
		}
		else
		{
			for (int i = 0; i < locals.Count - 1; i++)
			{
				for (int j = i + 1; j < locals.Count; j++)
				{
					LocalVar v1 = (LocalVar)locals[i];
					LocalVar v2 = (LocalVar)locals[j];
					// if the two locals are the same, we merge them, this is a small
					// optimization, it should *not* be required for correctness.
					if (v1.local == v2.local && v1.type == v2.type)
					{
						v1.isArg |= v2.isArg;
						forwarders.Add(v2, v1);
						locals.RemoveAt(j);
						j--;
					}
				}
			}
		}
		invokespecialLocalVars = new LocalVar[instructions.Length][];
		localVars = new LocalVar[instructions.Length];
		for (int i = 0; i < localVars.Length; i++)
		{
			LocalVar v = null;
			if (localStoreReaders[i] != null)
			{
				Debug.Assert(IsLoadLocal(instructions[i].NormalizedOpCode));
				// lame way to look up the local variable for a load
				// (by indirecting through a corresponding store)
				foreach (int store in localStoreReaders[i].Keys)
				{
					v = localByStoreSite[MakeKey(store, instructions[i].NormalizedArg1)];
					break;
				}
			}
			else
			{
				if (instructions[i].NormalizedOpCode == NormalizedByteCode.__invokespecial || instructions[i].NormalizedOpCode == NormalizedByteCode.__dynamic_invokespecial)
				{
					invokespecialLocalVars[i] = new LocalVar[method.MaxLocals];
					for (int j = 0; j < invokespecialLocalVars[i].Length; j++)
					{
						localByStoreSite.TryGetValue(MakeKey(i, j), out invokespecialLocalVars[i][j]);
					}
				}
				else
				{
					localByStoreSite.TryGetValue(MakeKey(i, instructions[i].NormalizedArg1), out v);
				}
			}
			if (v != null)
			{
				LocalVar fwd;
				if (forwarders.TryGetValue(v, out fwd))
				{
					v = fwd;
				}
				localVars[i] = v;
			}
		}
		this.allLocalVars = locals.ToArray();
	}

	private static void FindLvtEntry(LocalVar lv, ClassFile.Method method, int instructionIndex)
	{
		ClassFile.Method.LocalVariableTableEntry[] lvt = method.LocalVariableTableAttribute;
		if (lvt != null)
		{
			int pc = method.Instructions[instructionIndex].PC;
			int nextPC = method.Instructions[instructionIndex + 1].PC;
			bool isStore = IsStoreLocal(method.Instructions[instructionIndex].NormalizedOpCode);
			foreach (ClassFile.Method.LocalVariableTableEntry e in lvt)
			{
				// TODO validate the contents of the LVT entry
				if (e.index == lv.local &&
					(e.start_pc <= pc || (e.start_pc == nextPC && isStore)) &&
					e.start_pc + e.length > pc)
				{
					lv.name = e.name;
					lv.start_pc = e.start_pc;
					lv.end_pc = e.start_pc + e.length;
					break;
				}
			}
		}
	}

	// NOTE for dead stores, this returns null
	internal LocalVar GetLocalVar(int instructionIndex)
	{
		return localVars[instructionIndex];
	}

	internal LocalVar[] GetLocalVarsForInvokeSpecial(int instructionIndex)
	{
		return invokespecialLocalVars[instructionIndex];
	}

	internal LocalVar[] GetAllLocalVars()
	{
		return allLocalVars;
	}

	private static bool IsLoadLocal(NormalizedByteCode bc)
	{
		return bc == NormalizedByteCode.__aload ||
			bc == NormalizedByteCode.__iload ||
			bc == NormalizedByteCode.__lload ||
			bc == NormalizedByteCode.__fload ||
			bc == NormalizedByteCode.__dload ||
			bc == NormalizedByteCode.__iinc ||
			bc == NormalizedByteCode.__ret;
	}

	private static bool IsStoreLocal(NormalizedByteCode bc)
	{
		return bc == NormalizedByteCode.__astore ||
			bc == NormalizedByteCode.__istore ||
			bc == NormalizedByteCode.__lstore ||
			bc == NormalizedByteCode.__fstore ||
			bc == NormalizedByteCode.__dstore;
	}

	struct FindLocalVarState
	{
		internal bool changed;
		internal FindLocalVarStoreSite[] sites;

		internal void Store(int instructionIndex, int localIndex)
		{
			if (sites[localIndex].Count == 1 && sites[localIndex][0] == instructionIndex)
			{
				return;
			}
			sites = (FindLocalVarStoreSite[])sites.Clone();
			sites[localIndex] = new FindLocalVarStoreSite();
			sites[localIndex].Add(instructionIndex);
		}

		internal void Merge(FindLocalVarState state)
		{
			if (sites == null)
			{
				sites = state.sites;
				changed = true;
			}
			else
			{
				bool dirty = true;
				for (int i = 0; i < sites.Length; i++)
				{
					for (int j = 0; j < state.sites[i].Count; j++)
					{
						if (!sites[i].Contains(state.sites[i][j]))
						{
							if (dirty)
							{
								dirty = false;
								sites = (FindLocalVarStoreSite[])sites.Clone();
							}
							sites[i].Add(state.sites[i][j]);
							changed = true;
						}
					}
				}
			}
		}

		internal FindLocalVarState Copy()
		{
			FindLocalVarState copy = new FindLocalVarState();
			copy.sites = sites;
			return copy;
		}

		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			if (sites != null)
			{
				foreach (FindLocalVarStoreSite site in sites)
				{
					sb.Append('[');
					for (int i = 0; i < site.Count; i++)
					{
						sb.AppendFormat("{0}, ", site[i]);
					}
					sb.Append(']');
				}
			}
			return sb.ToString();
		}
	}

	struct FindLocalVarStoreSite
	{
		private int[] data;

		internal bool Contains(int instructionIndex)
		{
			if (data != null)
			{
				for (int i = 0; i < data.Length; i++)
				{
					if (data[i] == instructionIndex)
					{
						return true;
					}
				}
			}
			return false;
		}

		internal void Add(int instructionIndex)
		{
			if (data == null)
			{
				data = new int[] { instructionIndex };
			}
			else
			{
				data = ArrayUtil.Concat(data, instructionIndex);
			}
		}

		internal int this[int index]
		{
			get { return data[index]; }
		}

		internal int Count
		{
			get { return data == null ? 0 : data.Length; }
		}
	}

	private static Dictionary<int, string>[] FindLocalVariables(CodeInfo codeInfo, MethodWrapper mw, ClassFile classFile, ClassFile.Method method)
	{
		FindLocalVarState[] state = new FindLocalVarState[method.Instructions.Length];
		state[0].changed = true;
		state[0].sites = new FindLocalVarStoreSite[method.MaxLocals];
		TypeWrapper[] parameters = mw.GetParameters();
		int argpos = 0;
		if (!mw.IsStatic)
		{
			state[0].sites[argpos++].Add(-1);
		}
		for (int i = 0; i < parameters.Length; i++)
		{
			state[0].sites[argpos++].Add(-1);
			if (parameters[i].IsWidePrimitive)
			{
				argpos++;
			}
		}
		return FindLocalVariablesImpl(codeInfo, classFile, method, state);
	}

	private static Dictionary<int, string>[] FindLocalVariablesImpl(CodeInfo codeInfo, ClassFile classFile, ClassFile.Method method, FindLocalVarState[] state)
	{
		ClassFile.Method.Instruction[] instructions = method.Instructions;
		ExceptionTableEntry[] exceptions = method.ExceptionTable;
		int maxLocals = method.MaxLocals;
		Dictionary<int, string>[] localStoreReaders = new Dictionary<int, string>[instructions.Length];
		bool done = false;

		while (!done)
		{
			done = true;
			for (int i = 0; i < instructions.Length; i++)
			{
				if (state[i].changed)
				{
					done = false;
					state[i].changed = false;

					FindLocalVarState curr = state[i].Copy();

					for (int j = 0; j < exceptions.Length; j++)
					{
						if (exceptions[j].startIndex <= i && i < exceptions[j].endIndex)
						{
							state[exceptions[j].handlerIndex].Merge(curr);
						}
					}

					if (IsLoadLocal(instructions[i].NormalizedOpCode)
						&& (instructions[i].NormalizedOpCode != NormalizedByteCode.__aload || !VerifierTypeWrapper.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(i + 1, 0))))
					{
						if (localStoreReaders[i] == null)
						{
							localStoreReaders[i] = new Dictionary<int, string>();
						}
						for (int j = 0; j < curr.sites[instructions[i].NormalizedArg1].Count; j++)
						{
							localStoreReaders[i][curr.sites[instructions[i].NormalizedArg1][j]] = "";
						}
					}

					if (IsStoreLocal(instructions[i].NormalizedOpCode)
						&& (instructions[i].NormalizedOpCode != NormalizedByteCode.__astore || !VerifierTypeWrapper.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(i, 0))))
					{
						curr.Store(i, instructions[i].NormalizedArg1);
						// if this is a store at the end of an exception block,
						// we need to propagate the new state to the exception handler
						for (int j = 0; j < exceptions.Length; j++)
						{
							if (exceptions[j].endIndex == i + 1)
							{
								state[exceptions[j].handlerIndex].Merge(curr);
							}
						}
					}

					if (instructions[i].NormalizedOpCode == NormalizedByteCode.__invokespecial)
					{
						ClassFile.ConstantPoolItemMI cpi = classFile.GetMethodref(instructions[i].Arg1);
						if (ReferenceEquals(cpi.Name, StringConstants.INIT))
						{
							TypeWrapper type = codeInfo.GetRawStackTypeWrapper(i, cpi.GetArgTypes().Length);
							// after we've invoked the constructor, the uninitialized references
							// are now initialized
							if (type == VerifierTypeWrapper.UninitializedThis
								|| VerifierTypeWrapper.IsNew(type))
							{
								for (int j = 0; j < maxLocals; j++)
								{
									if (codeInfo.GetLocalTypeWrapper(i, j) == type)
									{
										curr.Store(i, j);
									}
								}
							}
						}
					}
					else if (instructions[i].NormalizedOpCode == NormalizedByteCode.__goto_finally)
					{
						int handler = instructions[i].HandlerIndex;

						// Normally a store at the end of a try block doesn't affect the handler block,
						// but in the case of a finally handler it does, so we need to make sure that
						// we merge here in case the try block ended with a store.
						state[handler].Merge(curr);

						// Now we recursively analyse the handler and afterwards merge the endfault locations back to us
						FindLocalVarState[] handlerState = new FindLocalVarState[instructions.Length];
						handlerState[handler].Merge(curr);
						curr = new FindLocalVarState();
						FindLocalVariablesImpl(codeInfo, classFile, method, handlerState);

						// Merge back to the target of our __goto_finally
						for (int j = 0; j < handlerState.Length; j++)
						{
							if (instructions[j].NormalizedOpCode == NormalizedByteCode.__athrow
								&& codeInfo.HasState(j)
								&& VerifierTypeWrapper.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(j, 0))
								&& ((VerifierTypeWrapper)codeInfo.GetRawStackTypeWrapper(j, 0)).Index == handler)
							{
								curr.Merge(handlerState[j]);
							}
						}
					}

					switch (ByteCodeMetaData.GetFlowControl(instructions[i].NormalizedOpCode))
					{
						case ByteCodeFlowControl.Switch:
							{
								for (int j = 0; j < instructions[i].SwitchEntryCount; j++)
								{
									state[instructions[i].GetSwitchTargetIndex(j)].Merge(curr);
								}
								state[instructions[i].DefaultTarget].Merge(curr);
								break;
							}
						case ByteCodeFlowControl.Branch:
							state[instructions[i].TargetIndex].Merge(curr);
							break;
						case ByteCodeFlowControl.CondBranch:
							state[instructions[i].TargetIndex].Merge(curr);
							state[i + 1].Merge(curr);
							break;
						case ByteCodeFlowControl.Return:
						case ByteCodeFlowControl.Throw:
							break;
						case ByteCodeFlowControl.Next:
							state[i + 1].Merge(curr);
							break;
						default:
							throw new InvalidOperationException();
					}
				}
			}
		}
		return localStoreReaders;
	}

	private static void VisitLocalLoads(CodeInfo codeInfo, ClassFile.Method method, List<LocalVar> locals, Dictionary<long, LocalVar> localByStoreSite, Dictionary<int, string> storeSites, int instructionIndex, bool debug)
	{
		Debug.Assert(IsLoadLocal(method.Instructions[instructionIndex].NormalizedOpCode));
		LocalVar local = null;
		TypeWrapper type = VerifierTypeWrapper.Null;
		int localIndex = method.Instructions[instructionIndex].NormalizedArg1;
		bool isArg = false;
		foreach (int store in storeSites.Keys)
		{
			if (store == -1)
			{
				// it's a method argument, it has no initial store, but the type is simply the parameter type
				type = InstructionState.FindCommonBaseType(type, codeInfo.GetLocalTypeWrapper(0, localIndex));
				isArg = true;
			}
			else
			{
				if (method.Instructions[store].NormalizedOpCode == NormalizedByteCode.__invokespecial)
				{
					type = InstructionState.FindCommonBaseType(type, codeInfo.GetLocalTypeWrapper(store + 1, localIndex));
				}
				else if (method.Instructions[store].NormalizedOpCode == NormalizedByteCode.__static_error)
				{
					// it's an __invokespecial that turned into a __static_error
					// (since a __static_error doesn't continue, we don't need to set type)
				}
				else
				{
					Debug.Assert(IsStoreLocal(method.Instructions[store].NormalizedOpCode));
					type = InstructionState.FindCommonBaseType(type, codeInfo.GetStackTypeWrapper(store, 0));
				}
			}
			// we can't have an invalid type, because that would have failed verification earlier
			Debug.Assert(type != VerifierTypeWrapper.Invalid);

			LocalVar l;
			if (localByStoreSite.TryGetValue(MakeKey(store, localIndex), out l))
			{
				if (local == null)
				{
					local = l;
				}
				else if (local != l)
				{
					// If we've already defined a LocalVar and we find another one, then we merge them
					// together.
					// This happens for the following code fragment:
					//
					// int i = -1;
					// try { i = 0; for(; ; ) System.out.println(i); } catch(Exception x) {}
					// try { i = 0; for(; ; ) System.out.println(i); } catch(Exception x) {}
					// System.out.println(i);
					//
					local = MergeLocals(locals, localByStoreSite, local, l);
				}
			}
		}
		if (local == null)
		{
			local = new LocalVar();
			local.local = localIndex;
			if (VerifierTypeWrapper.IsThis(type))
			{
				local.type = ((VerifierTypeWrapper)type).UnderlyingType;
			}
			else
			{
				local.type = type;
			}
			local.isArg = isArg;
			if (debug)
			{
				FindLvtEntry(local, method, instructionIndex);
			}
			locals.Add(local);
		}
		else
		{
			local.isArg |= isArg;
			local.type = InstructionState.FindCommonBaseType(local.type, type);
			Debug.Assert(local.type != VerifierTypeWrapper.Invalid);
		}
		foreach (int store in storeSites.Keys)
		{
			LocalVar v;
			if (!localByStoreSite.TryGetValue(MakeKey(store, localIndex), out v))
			{
				localByStoreSite[MakeKey(store, localIndex)] = local;
			}
			else if (v != local)
			{
				local = MergeLocals(locals, localByStoreSite, local, v);
			}
		}
	}

	private static long MakeKey(int i, int j)
	{
		return (((long)(uint)i) << 32) + (uint)j;
	}

	private static LocalVar MergeLocals(List<LocalVar> locals, Dictionary<long, LocalVar> localByStoreSite, LocalVar l1, LocalVar l2)
	{
		Debug.Assert(l1 != l2);
		Debug.Assert(l1.local == l2.local);
		for (int i = 0; i < locals.Count; i++)
		{
			if (locals[i] == l2)
			{
				locals.RemoveAt(i);
				i--;
			}
		}
		Dictionary<long, LocalVar> temp = new Dictionary<long, LocalVar>(localByStoreSite);
		localByStoreSite.Clear();
		foreach (KeyValuePair<long, LocalVar> kv in temp)
		{
			localByStoreSite[kv.Key] = kv.Value == l2 ? l1 : kv.Value;
		}
		l1.isArg |= l2.isArg;
		l1.type = InstructionState.FindCommonBaseType(l1.type, l2.type);
		Debug.Assert(l1.type != VerifierTypeWrapper.Invalid);
		return l1;
	}
}
