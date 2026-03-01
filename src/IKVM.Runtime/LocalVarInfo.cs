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
using System.Text;

using IKVM.ByteCode;
using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;

namespace IKVM.Runtime
{

    struct LocalVarInfo
    {

        readonly LocalVar[/*instructionIndex*/] localVars;
        readonly LocalVar[/*instructionIndex*/][/*localIndex*/] invokespecialLocalVars;
        readonly LocalVar[/*index*/] allLocalVars;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="classFile"></param>
        /// <param name="method"></param>
        /// <param name="exceptions"></param>
        /// <param name="mw"></param>
        /// <param name="classLoader"></param>
        internal LocalVarInfo(CodeInfo ma, ClassFile classFile, Method method, UntangledExceptionTable exceptions, RuntimeJavaMethod mw, RuntimeClassLoader classLoader)
        {
            var localStoreReaders = FindLocalVariables(ma, mw, classFile, method);

            // now that we've done the code flow analysis, we can do a liveness analysis on the local variables
            var instructions = method.Instructions;
            var localByStoreSite = new Dictionary<long, LocalVar>();
            var locals = new List<LocalVar>();

            for (int i = 0; i < localStoreReaders.Length; i++)
                if (localStoreReaders[i] != null)
                    VisitLocalLoads(classLoader.Context, ma, method, locals, localByStoreSite, localStoreReaders[i], i, classLoader.EmitSymbols);

            var forwarders = new Dictionary<LocalVar, LocalVar>();
            if (classLoader.EmitSymbols)
            {
                var flags = MethodAnalyzer.ComputePartialReachability(ma, method.Instructions, exceptions, 0, false);

                // if we're emitting debug info, we need to keep dead stores as well...
                for (int i = 0; i < instructions.Length; i++)
                {
                    if ((flags[i] & InstructionFlags.Reachable) != 0 && IsStoreLocal(instructions[i].NormalizedOpCode))
                    {
                        if (localByStoreSite.ContainsKey(MakeKey(i, instructions[i].NormalizedArg1)) == false)
                        {
                            var v = new LocalVar();
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
                        var v1 = locals[i];
                        var v2 = locals[j];

                        if (v1.name != null && v1.name == v2.name && v1.start_pc == v2.start_pc && v1.end_pc == v2.end_pc && v1.local == v2.local) // FIX: Kotlin’s constructor parameters for nested/anonymous classes often share the same LVT name, prevent merging distinct slots (e.g., two "$receiver" params in kotlin)
                        {
                            // we can only merge if the resulting type is valid (this protects against incorrect
                            // LVT data, but is also needed for constructors, where the uninitialized this is a different
                            // type from the initialized this)
                            var tw = InstructionState.FindCommonBaseType(classLoader.Context, v1.type, v2.type);
                            if (tw != classLoader.Context.VerifierJavaTypeFactory.Invalid)
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
                        var v1 = locals[i];
                        var v2 = locals[j];

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
                    if (instructions[i].NormalizedOpCode == NormalizedOpCode.InvokeSpecial || instructions[i].NormalizedOpCode == NormalizedOpCode.DynamicInvokeSpecial)
                    {
                        invokespecialLocalVars[i] = new LocalVar[method.MaxLocals];
                        for (int j = 0; j < invokespecialLocalVars[i].Length; j++)
                            localByStoreSite.TryGetValue(MakeKey(i, j), out invokespecialLocalVars[i][j]);
                    }
                    else
                    {
                        localByStoreSite.TryGetValue(MakeKey(i, instructions[i].NormalizedArg1), out v);
                    }
                }

                if (v != null)
                {
                    if (forwarders.TryGetValue(v, out var fwd))
                        v = fwd;

                    localVars[i] = v;
                }
            }

            allLocalVars = locals.ToArray();
        }

        static void FindLvtEntry(LocalVar lv, Method method, int instructionIndex)
        {
            var lvt = method.LocalVariableTable;
            if (lvt.Count > 0)
            {
                var pc = method.Instructions[instructionIndex].PC;
                var nextPC = method.Instructions[instructionIndex + 1].PC;
                var isStore = IsStoreLocal(method.Instructions[instructionIndex].NormalizedOpCode);

                foreach (var e in lvt)
                {
                    // TODO validate the contents of the LVT entry
                    if (e.Slot == lv.local && (e.StartPc <= pc || (e.StartPc == nextPC && isStore)) && e.StartPc + e.Length > pc)
                    {
                        lv.name = method.ClassFile.GetConstantPoolUtf8String(e.Name);
                        lv.start_pc = e.StartPc;
                        lv.end_pc = e.StartPc + e.Length;
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

        static bool IsLoadLocal(NormalizedOpCode bc)
        {
            return bc is
                NormalizedOpCode.Aload or
                NormalizedOpCode.Iload or
                NormalizedOpCode.Lload or
                NormalizedOpCode.Fload or
                NormalizedOpCode.Dload or
                NormalizedOpCode.Iinc or
                NormalizedOpCode.Ret;
        }

        static bool IsStoreLocal(NormalizedOpCode bc)
        {
            return bc is
                NormalizedOpCode.Astore or
                NormalizedOpCode.Istore or
                NormalizedOpCode.Lstore or
                NormalizedOpCode.Fstore or
                NormalizedOpCode.Dstore;
        }

        struct FindLocalVarState
        {

            internal bool changed;
            internal FindLocalVarStoreSite[] sites;

            internal void Store(int instructionIndex, int localIndex)
            {
                if (sites[localIndex].Count == 1 && sites[localIndex][0] == instructionIndex)
                    return;

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
                    var dirty = true;
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
                var copy = new FindLocalVarState();
                copy.sites = sites;
                return copy;
            }

            public override string ToString()
            {
                var sb = new ValueStringBuilder();

                if (sites != null)
                {
                    foreach (var site in sites)
                    {
                        sb.Append('[');

                        for (int i = 0; i < site.Count; i++)
                        {
                            sb.Append(site[i].ToString());
                            sb.Append(", ");
                        }

                        sb.Append(']');
                    }
                }

                return sb.ToString();
            }
        }

        struct FindLocalVarStoreSite
        {

            int[] data;

            internal bool Contains(int instructionIndex)
            {
                if (data != null)
                    for (int i = 0; i < data.Length; i++)
                        if (data[i] == instructionIndex)
                            return true;

                return false;
            }

            internal void Add(int instructionIndex)
            {
                data = data == null ? [instructionIndex] : [.. data, instructionIndex];
            }

            internal readonly int this[int index] => data[index];

            internal readonly int Count => data == null ? 0 : data.Length;

        }

        static Dictionary<int, string>[] FindLocalVariables(CodeInfo codeInfo, RuntimeJavaMethod mw, ClassFile classFile, Method method)
        {
            var state = new FindLocalVarState[method.Instructions.Length];
            state[0].changed = true;
            state[0].sites = new FindLocalVarStoreSite[method.MaxLocals];

            var parameters = mw.GetParameters();
            int argpos = 0;
            if (!mw.IsStatic)
                state[0].sites[argpos++].Add(-1);

            for (int i = 0; i < parameters.Length; i++)
            {
                state[0].sites[argpos++].Add(-1);
                if (parameters[i].IsWidePrimitive)
                    argpos++;
            }

            return FindLocalVariablesImpl(mw.DeclaringType.Context, codeInfo, classFile, method, state);
        }

        static Dictionary<int, string>[] FindLocalVariablesImpl(RuntimeContext context, CodeInfo codeInfo, ClassFile classFile, Method method, FindLocalVarState[] state)
        {
            var instructions = method.Instructions;
            var exceptions = method.ExceptionTable;
            var maxLocals = method.MaxLocals;
            var localStoreReaders = new Dictionary<int, string>[instructions.Length];

            var done = false;
            while (!done)
            {
                done = true;
                for (int i = 0; i < instructions.Length; i++)
                {
                    if (state[i].changed)
                    {
                        done = false;
                        state[i].changed = false;

                        var curr = state[i].Copy();

                        for (int j = 0; j < exceptions.Length; j++)
                            if (exceptions[j].StartIndex <= i && i < exceptions[j].EndIndex)
                                state[exceptions[j].HandlerIndex].Merge(curr);

                        if (IsLoadLocal(instructions[i].NormalizedOpCode) && (instructions[i].NormalizedOpCode != NormalizedOpCode.Aload || !RuntimeVerifierJavaType.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(i + 1, 0))))
                        {
                            localStoreReaders[i] ??= new Dictionary<int, string>();

                            for (int j = 0; j < curr.sites[instructions[i].NormalizedArg1].Count; j++)
                                localStoreReaders[i][curr.sites[instructions[i].NormalizedArg1][j]] = "";
                        }

                        if (IsStoreLocal(instructions[i].NormalizedOpCode) && (instructions[i].NormalizedOpCode != NormalizedOpCode.Astore || !RuntimeVerifierJavaType.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(i, 0))))
                        {
                            curr.Store(i, instructions[i].NormalizedArg1);

                            // if this is a store at the end of an exception block,
                            // we need to propagate the new state to the exception handler
                            for (int j = 0; j < exceptions.Length; j++)
                                if (exceptions[j].EndIndex == i + 1)
                                    state[exceptions[j].HandlerIndex].Merge(curr);
                        }

                        if (instructions[i].NormalizedOpCode == NormalizedOpCode.InvokeSpecial)
                        {
                            var cpi = classFile.GetMethodref(instructions[i].Arg1);
                            if (ReferenceEquals(cpi.Name, StringConstants.INIT))
                            {
                                var type = codeInfo.GetRawStackTypeWrapper(i, cpi.GetArgTypes().Length);
                                // after we've invoked the constructor, the uninitialized references are now initialized
                                if (type == context.VerifierJavaTypeFactory.UninitializedThis || RuntimeVerifierJavaType.IsNew(type))
                                    for (int j = 0; j < maxLocals; j++)
                                        if (codeInfo.GetLocalTypeWrapper(i, j) == type)
                                            curr.Store(i, j);
                            }
                        }
                        else if (instructions[i].NormalizedOpCode == NormalizedOpCode.GotoFinally)
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
                            FindLocalVariablesImpl(context, codeInfo, classFile, method, handlerState);

                            // Merge back to the target of our __goto_finally
                            for (int j = 0; j < handlerState.Length; j++)
                            {
                                if (instructions[j].NormalizedOpCode == NormalizedOpCode.Athrow
                                    && codeInfo.HasState(j)
                                    && RuntimeVerifierJavaType.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(j, 0))
                                    && ((RuntimeVerifierJavaType)codeInfo.GetRawStackTypeWrapper(j, 0)).Index == handler)
                                {
                                    curr.Merge(handlerState[j]);
                                }
                            }
                        }

                        switch (OpCodeMetaData.GetFlowKind(instructions[i].NormalizedOpCode))
                        {
                            case OpCodeFlowControl.Switch:
                                {
                                    for (int j = 0; j < instructions[i].SwitchEntryCount; j++)
                                        state[instructions[i].GetSwitchTargetIndex(j)].Merge(curr);

                                    state[instructions[i].DefaultTarget].Merge(curr);
                                    break;
                                }
                            case OpCodeFlowControl.Branch:
                                state[instructions[i].TargetIndex].Merge(curr);
                                break;
                            case OpCodeFlowControl.ConditionalBranch:
                                state[instructions[i].TargetIndex].Merge(curr);
                                state[i + 1].Merge(curr);
                                break;
                            case OpCodeFlowControl.Return:
                            case OpCodeFlowControl.Throw:
                                break;
                            case OpCodeFlowControl.Next:
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

        static void VisitLocalLoads(RuntimeContext context, CodeInfo codeInfo, Method method, List<LocalVar> locals, Dictionary<long, LocalVar> localByStoreSite, Dictionary<int, string> storeSites, int instructionIndex, bool debug)
        {
            Debug.Assert(IsLoadLocal(method.Instructions[instructionIndex].NormalizedOpCode));

            LocalVar local = null;
            var type = context.VerifierJavaTypeFactory.Null;
            var localIndex = method.Instructions[instructionIndex].NormalizedArg1;
            var isArg = false;
            foreach (int store in storeSites.Keys)
            {
                if (store == -1)
                {
                    // it's a method argument, it has no initial store, but the type is simply the parameter type
                    type = InstructionState.FindCommonBaseType(context, type, codeInfo.GetLocalTypeWrapper(0, localIndex));
                    isArg = true;
                }
                else
                {
                    if (method.Instructions[store].NormalizedOpCode == NormalizedOpCode.InvokeSpecial)
                    {
                        type = InstructionState.FindCommonBaseType(context, type, codeInfo.GetLocalTypeWrapper(store + 1, localIndex));
                    }
                    else if (method.Instructions[store].NormalizedOpCode == NormalizedOpCode.StaticError)
                    {
                        // it's an __invokespecial that turned into a __static_error
                        // (since a __static_error doesn't continue, we don't need to set type)
                    }
                    else
                    {
                        Debug.Assert(IsStoreLocal(method.Instructions[store].NormalizedOpCode));
                        type = InstructionState.FindCommonBaseType(context, type, codeInfo.GetStackTypeWrapper(store, 0));
                    }
                }
                // we can't have an invalid type, because that would have failed verification earlier
                Debug.Assert(type != context.VerifierJavaTypeFactory.Invalid);

                if (localByStoreSite.TryGetValue(MakeKey(store, localIndex), out var l))
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
                        local = MergeLocals(context, locals, localByStoreSite, local, l);
                    }
                }
            }

            if (local == null)
            {
                local = new LocalVar();
                local.local = localIndex;
                local.type = RuntimeVerifierJavaType.IsThis(type) ? ((RuntimeVerifierJavaType)type).UnderlyingType : type;
                local.isArg = isArg;

                if (debug)
                    FindLvtEntry(local, method, instructionIndex);

                locals.Add(local);
            }
            else
            {
                local.isArg |= isArg;
                local.type = InstructionState.FindCommonBaseType(context, local.type, type);
                Debug.Assert(local.type != context.VerifierJavaTypeFactory.Invalid);
            }

            foreach (int store in storeSites.Keys)
            {
                if (!localByStoreSite.TryGetValue(MakeKey(store, localIndex), out var v))
                {
                    localByStoreSite[MakeKey(store, localIndex)] = local;
                }
                else if (v != local)
                {
                    local = MergeLocals(context, locals, localByStoreSite, local, v);
                }
            }
        }

        static long MakeKey(int i, int j)
        {
            return (((long)(uint)i) << 32) + (uint)j;
        }

        static LocalVar MergeLocals(RuntimeContext context, List<LocalVar> locals, Dictionary<long, LocalVar> localByStoreSite, LocalVar l1, LocalVar l2)
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

            var temp = new Dictionary<long, LocalVar>(localByStoreSite);
            localByStoreSite.Clear();
            foreach (var kv in temp)
                localByStoreSite[kv.Key] = kv.Value == l2 ? l1 : kv.Value;

            l1.isArg |= l2.isArg;
            l1.type = InstructionState.FindCommonBaseType(context, l1.type, l2.type);
            Debug.Assert(l1.type != context.VerifierJavaTypeFactory.Invalid);

            return l1;
        }
    }

}
