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

using IKVM.ByteCode;
using IKVM.CoreLib.Linking;

namespace IKVM.Runtime
{

    /// <summary>
    /// A customized and simplified version of <see cref="MethodAnalyzer"/> that exists purely to inline jsr instructions.
    /// </summary>
    sealed class JsrInliner
    {

        internal static void InlineJsrs(RuntimeClassLoader classLoader, RuntimeJavaMethod mw, ClassFile classFile, Method m)
        {
            JsrInliner inliner;

            do
            {
                var codeCopy = (Instruction[])m.Instructions.Clone();
                var flags = new InstructionFlags[codeCopy.Length];
                var ma = new JsrMethodAnalyzer(mw, classFile, m, classLoader, flags);
                inliner = new JsrInliner(codeCopy, flags, m, ma);
            } while (inliner.InlineJsrs());
        }

        Instruction[] codeCopy;
        int codeLength;
        InstructionFlags[] flags;
        readonly Method m;
        readonly JsrMethodAnalyzer ma;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="codeCopy"></param>
        /// <param name="flags"></param>
        /// <param name="m"></param>
        /// <param name="ma"></param>
        JsrInliner(Instruction[] codeCopy, InstructionFlags[] flags, Method m, JsrMethodAnalyzer ma)
        {
            this.codeCopy = codeCopy;
            codeLength = codeCopy.Length;
            this.flags = flags;
            this.m = m;
            this.ma = ma;
        }

        void Add(Instruction instr)
        {
            if (codeLength == codeCopy.Length)
            {
                Array.Resize(ref codeCopy, codeLength * 2);
                Array.Resize(ref flags, codeLength * 2);
            }

            codeCopy[codeLength++] = instr;
        }

        /// <summary>
        /// Conducts a pass through the instruction set, inlining JSR instructions, and returning <c>true</c> if any JSR instructions were inlined.
        /// </summary>
        /// <returns></returns>
        bool InlineJsrs()
        {
            var hasJsrs = false;
            var subs = new List<SubroutineCall>();
            int len = codeLength;

            for (int i = 0; i < len; i++)
            {
                // note that we're also (needlessly) processing the subroutines here, but that shouldn't be a problem (just a minor waste of cpu)
                // because the code is unreachable anyway
                if ((flags[i] & InstructionFlags.Reachable) != 0 && m.Instructions[i].NormalizedOpCode == NormalizedOpCode.Jsr)
                {
                    int subroutineId = m.Instructions[i].TargetIndex;
                    codeCopy[i].PatchOpCode(NormalizedOpCode.Goto, codeLength);
                    var sub = new SubroutineCall(this, subroutineId, i + 1);
                    hasJsrs |= sub.InlineSubroutine();
                    subs.Add(sub);
                }
            }

            var exceptions = new List<ExceptionTableEntry>(m.ExceptionTable);
            foreach (var sub in subs)
                sub.DoExceptions(m.ExceptionTable, exceptions);

            m.ExceptionTable = exceptions.ToArray();
            var instr = new Instruction();
            instr.SetTermNop(0xFFFF);
            Add(instr);
            Array.Resize(ref codeCopy, codeLength);

            m.Instructions = codeCopy;
            return hasJsrs;
        }

        sealed class SubroutineCall
        {

            readonly JsrInliner inliner;
            readonly int subroutineIndex;
            readonly int returnIndex;
            readonly int[] branchMap;
            readonly int baseIndex;
            int endIndex;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="inliner"></param>
            /// <param name="subroutineIndex"></param>
            /// <param name="returnIndex"></param>
            internal SubroutineCall(JsrInliner inliner, int subroutineIndex, int returnIndex)
            {
                this.inliner = inliner;
                this.subroutineIndex = subroutineIndex;
                this.returnIndex = returnIndex;
                baseIndex = inliner.codeLength;
                branchMap = new int[inliner.m.Instructions.Length];
                for (int i = 0; i < branchMap.Length; i++)
                    branchMap[i] = i;
            }

            void Emit(Instruction instr)
            {
                inliner.Add(instr);
            }

            void EmitGoto(int targetIndex)
            {
                var instr = new Instruction();
                instr.PatchOpCode(NormalizedOpCode.Goto, targetIndex);
                instr.SetPC(-1);
                Emit(instr);
            }

            internal bool InlineSubroutine()
            {
                var hasJsrs = false;

                // start with a pre-amble to load a dummy return address on the stack and to branch to the subroutine
                {
                    // TODO consider exception handling around these instructions
                    var instr = new Instruction();
                    instr.PatchOpCode(NormalizedOpCode.AconstNull);
                    instr.SetPC(inliner.m.Instructions[subroutineIndex].PC);
                    Emit(instr);
                    EmitGoto(subroutineIndex);
                }

                var fallThru = false;
                for (int instructionIndex = 0; instructionIndex < inliner.m.Instructions.Length; instructionIndex++)
                {
                    if ((inliner.flags[instructionIndex] & InstructionFlags.Reachable) != 0 && inliner.ma.IsSubroutineActive(instructionIndex, subroutineIndex))
                    {
                        fallThru = false;
                        branchMap[instructionIndex] = inliner.codeLength;

                        switch (inliner.m.Instructions[instructionIndex].NormalizedOpCode)
                        {
                            case NormalizedOpCode.TableSwitch:
                            case NormalizedOpCode.LookupSwitch:
                            case NormalizedOpCode.Ireturn:
                            case NormalizedOpCode.Lreturn:
                            case NormalizedOpCode.Freturn:
                            case NormalizedOpCode.Dreturn:
                            case NormalizedOpCode.Areturn:
                            case NormalizedOpCode.Return:
                            case NormalizedOpCode.Athrow:
                            case NormalizedOpCode.Goto:
                                Emit(inliner.m.Instructions[instructionIndex]);
                                break;
                            case NormalizedOpCode.Jsr:
                                hasJsrs = true;
                                goto default;
                            case NormalizedOpCode.Ret:
                                {
                                    int subid = inliner.ma.GetLocalTypeWrapper(instructionIndex, inliner.m.Instructions[instructionIndex].TargetIndex).SubroutineIndex;
                                    if (subid == subroutineIndex)
                                        EmitGoto(returnIndex);
                                    else
                                        Emit(inliner.m.Instructions[instructionIndex]);
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

            void DoFixups()
            {
                for (int instructionIndex = baseIndex; instructionIndex < endIndex; instructionIndex++)
                {
                    switch (inliner.codeCopy[instructionIndex].NormalizedOpCode)
                    {
                        case NormalizedOpCode.LookupSwitch:
                        case NormalizedOpCode.TableSwitch:
                            {
                                var targets = new int[inliner.codeCopy[instructionIndex].SwitchEntryCount];
                                for (int i = 0; i < targets.Length; i++)
                                    targets[i] = branchMap[inliner.codeCopy[instructionIndex].GetSwitchTargetIndex(i)];

                                inliner.codeCopy[instructionIndex].SetSwitchTargets(targets);
                                inliner.codeCopy[instructionIndex].DefaultTarget = branchMap[inliner.codeCopy[instructionIndex].DefaultTarget];
                            }
                            break;
                        case NormalizedOpCode.Ifeq:
                        case NormalizedOpCode.Ifne:
                        case NormalizedOpCode.Iflt:
                        case NormalizedOpCode.Ifge:
                        case NormalizedOpCode.Ifgt:
                        case NormalizedOpCode.Ifle:
                        case NormalizedOpCode.IfIcmpeq:
                        case NormalizedOpCode.IfIcmpne:
                        case NormalizedOpCode.IfIcmplt:
                        case NormalizedOpCode.IfIcmpge:
                        case NormalizedOpCode.IfIcmpgt:
                        case NormalizedOpCode.IfIcmple:
                        case NormalizedOpCode.IfAcmpeq:
                        case NormalizedOpCode.IfAcmpne:
                        case NormalizedOpCode.IfNull:
                        case NormalizedOpCode.IfNonNull:
                        case NormalizedOpCode.Goto:
                        case NormalizedOpCode.Jsr:
                            inliner.codeCopy[instructionIndex].TargetIndex = branchMap[inliner.codeCopy[instructionIndex].TargetIndex];
                            break;
                    }
                }
            }

            int MapExceptionStartEnd(int index)
            {
                while (branchMap[index] < baseIndex)
                {
                    index++;
                    if (index == branchMap.Length)
                        return endIndex;
                }

                return branchMap[index];
            }

            internal void DoExceptions(ExceptionTableEntry[] table, List<ExceptionTableEntry> newExceptions)
            {
                foreach (var entry in table)
                {
                    int start = MapExceptionStartEnd(entry.StartIndex);
                    int end = MapExceptionStartEnd(entry.EndIndex);
                    if (start != end)
                    {
                        var newEntry = new ExceptionTableEntry(start, end, branchMap[entry.HandlerIndex], entry.CatchType, entry.Ordinal);
                        newExceptions.Add(newEntry);
                    }
                }
            }

        }

        /// <summary>
        /// Simple data flow type sufficient for <see cref="JsrInliner"/>.
        /// </summary>
        class SimpleType
        {

            sealed class ReturnAddressType : SimpleType
            {

                internal readonly int subroutineIndex;

                internal ReturnAddressType(int subroutineIndex)
                {
                    this.subroutineIndex = subroutineIndex;
                }

            }

            internal static readonly SimpleType Invalid = null;
            internal static readonly SimpleType Primitive = new SimpleType();
            internal static readonly SimpleType WidePrimitive = new SimpleType();
            internal static readonly SimpleType Object = new SimpleType();
            internal static readonly SimpleType[] EmptyArray = [];

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            SimpleType()
            {

            }

            internal bool IsPrimitive => this == SimpleType.Primitive || this == SimpleType.WidePrimitive;

            internal bool IsWidePrimitive => this == SimpleType.WidePrimitive;

            internal static SimpleType MakeRet(int subroutineIndex)
            {
                return new ReturnAddressType(subroutineIndex);
            }

            internal static bool IsRet(SimpleType w)
            {
                return w is ReturnAddressType;
            }

            internal int SubroutineIndex => ((ReturnAddressType)this).subroutineIndex;

        }

        sealed class JsrMethodAnalyzer
        {

            readonly ClassFile _classFile;
            readonly InstructionState[] _state;
            readonly List<int>[] _callsites;
            readonly List<int>[] _returnsites;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="mw"></param>
            /// <param name="classFile"></param>
            /// <param name="method"></param>
            /// <param name="classLoader"></param>
            /// <param name="flags"></param>
            /// <exception cref="VerifyError"></exception>
            /// <exception cref="ClassFormatError"></exception>
            /// <exception cref="InvalidOperationException"></exception>
            /// <exception cref="NotImplementedException"></exception>
            internal JsrMethodAnalyzer(RuntimeJavaMethod mw, ClassFile classFile, Method method, RuntimeClassLoader classLoader, InstructionFlags[] flags)
            {
                if (method.VerifyError != null)
                    throw new VerifyError(method.VerifyError);

                _classFile = classFile;
                _state = new InstructionState[method.Instructions.Length];
                _callsites = new List<int>[method.Instructions.Length];
                _returnsites = new List<int>[method.Instructions.Length];

                // because types have to have identity, the subroutine return address types are cached here
                var returnAddressTypes = new Dictionary<int, SimpleType>();

                try
                {
                    // ensure that exception blocks and handlers start and end at instruction boundaries
                    for (int i = 0; i < method.ExceptionTable.Length; i++)
                    {
                        int start = method.ExceptionTable[i].StartIndex;
                        int end = method.ExceptionTable[i].EndIndex;
                        int handler = method.ExceptionTable[i].HandlerIndex;
                        if (start >= end || start == -1 || end == -1 || handler <= 0)
                            throw new IndexOutOfRangeException();
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new ClassFormatError(string.Format("Illegal exception table (class: {0}, method: {1}, signature: {2}", classFile.Name, method.Name, method.Signature));
                }

                // start by computing the initial state, the stack is empty and the locals contain the arguments
                _state[0] = new InstructionState(method.MaxLocals, method.MaxStack);
                SimpleType thisType;
                int firstNonArgLocalIndex = 0;
                if (method.IsStatic == false)
                {
                    thisType = SimpleType.Object;
                    _state[0].SetLocalType(firstNonArgLocalIndex++, thisType, -1);
                }
                else
                {
                    thisType = null;
                }

                var argTypeWrappers = mw.GetParameters();
                for (int i = 0; i < argTypeWrappers.Length; i++)
                {
                    var tw = argTypeWrappers[i];

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

                    _state[0].SetLocalType(firstNonArgLocalIndex++, type, -1);
                    if (type.IsWidePrimitive)
                        firstNonArgLocalIndex++;
                }

                var argumentsByLocalIndex = new SimpleType[firstNonArgLocalIndex];
                for (int i = 0; i < argumentsByLocalIndex.Length; i++)
                    argumentsByLocalIndex[i] = _state[0].GetLocalTypeEx(i);

                var s = _state[0].Copy();
                var done = false;
                var instructions = method.Instructions;

                while (!done)
                {
                    done = true;

                    for (int i = 0; i < instructions.Length; i++)
                    {
                        if (_state[i] != null && _state[i].changed)
                        {
                            try
                            {
                                done = false;
                                _state[i].changed = false;

                                // mark the exception handlers reachable from this instruction
                                for (int j = 0; j < method.ExceptionTable.Length; j++)
                                    if (method.ExceptionTable[j].StartIndex <= i && i < method.ExceptionTable[j].EndIndex)
                                        MergeExceptionHandler(method.ExceptionTable[j].HandlerIndex, _state[i]);

                                _state[i].CopyTo(s);
                                var instr = instructions[i];

                                switch (instr.NormalizedOpCode)
                                {
                                    case NormalizedOpCode.Aload:
                                        {
                                            var type = s.GetLocalType(instr.NormalizedArg1);
                                            if (type == SimpleType.Invalid || type.IsPrimitive)
                                                throw new VerifyError("Object reference expected");

                                            s.PushType(type);
                                            break;
                                        }
                                    case NormalizedOpCode.Astore:
                                        s.SetLocalType(instr.NormalizedArg1, s.PopObjectType(), i);
                                        break;
                                    case NormalizedOpCode.AconstNull:
                                        s.PushObject();
                                        break;
                                    case NormalizedOpCode.Aaload:
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        s.PushObject();
                                        break;
                                    case NormalizedOpCode.Aastore:
                                        s.PopObjectType();
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.Baload:
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Bastore:
                                        s.PopPrimitive();
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.Caload:
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Castore:
                                        s.PopPrimitive();
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.Saload:
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Sastore:
                                        s.PopPrimitive();
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.Iaload:
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Iastore:
                                        s.PopPrimitive();
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.Laload:
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.Lastore:
                                        s.PopWidePrimitive();
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.Daload:
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.Dastore:
                                        s.PopWidePrimitive();
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.Faload:
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Fastore:
                                        s.PopPrimitive();
                                        s.PopPrimitive();
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.Arraylength:
                                        s.PopObjectType();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Iconst:
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.IfIcmpeq:
                                    case NormalizedOpCode.IfIcmpne:
                                    case NormalizedOpCode.IfIcmplt:
                                    case NormalizedOpCode.IfIcmpge:
                                    case NormalizedOpCode.IfIcmpgt:
                                    case NormalizedOpCode.IfIcmple:
                                        s.PopPrimitive();
                                        s.PopPrimitive();
                                        break;
                                    case NormalizedOpCode.Ifeq:
                                    case NormalizedOpCode.Ifge:
                                    case NormalizedOpCode.Ifgt:
                                    case NormalizedOpCode.Ifle:
                                    case NormalizedOpCode.Iflt:
                                    case NormalizedOpCode.Ifne:
                                        s.PopPrimitive();
                                        break;
                                    case NormalizedOpCode.IfNonNull:
                                    case NormalizedOpCode.IfNull:
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.IfAcmpeq:
                                    case NormalizedOpCode.IfAcmpne:
                                        s.PopObjectType();
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.GetStatic:
                                        s.PushType(GetFieldref(instr.Arg1).Signature);
                                        break;
                                    case NormalizedOpCode.PutStatic:
                                        s.PopType(GetFieldref(instr.Arg1).Signature);
                                        break;
                                    case NormalizedOpCode.GetField:
                                        s.PopObjectType();
                                        s.PushType(GetFieldref(instr.Arg1).Signature);
                                        break;
                                    case NormalizedOpCode.PutField:
                                        s.PopType(GetFieldref(instr.Arg1).Signature);
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.Ldc:
                                        {
                                            switch (GetConstantPoolConstantType(instr.Arg1))
                                            {
                                                case ConstantType.Double:
                                                    s.PushWidePrimitive();
                                                    break;
                                                case ConstantType.Float:
                                                    s.PushPrimitive();
                                                    break;
                                                case ConstantType.Integer:
                                                    s.PushPrimitive();
                                                    break;
                                                case ConstantType.Long:
                                                    s.PushWidePrimitive();
                                                    break;
                                                case ConstantType.String:
                                                case ConstantType.Class:
                                                    s.PushObject();
                                                    break;
                                                default:
                                                    // NOTE this is not a VerifyError, because it cannot happen (unless we have
                                                    // a bug in ClassFile.GetConstantPoolConstantType)
                                                    throw new InvalidOperationException();
                                            }
                                            break;
                                        }
                                    case NormalizedOpCode.InvokeVirtual:
                                    case NormalizedOpCode.InvokeSpecial:
                                    case NormalizedOpCode.InvokeInterface:
                                    case NormalizedOpCode.InvokeStatic:
                                        {
                                            var cpi = GetMethodref(instr.Arg1);
                                            s.MultiPopAnyType(cpi.GetArgTypes().Length);
                                            if (instr.NormalizedOpCode != NormalizedOpCode.InvokeStatic)
                                                s.PopType();

                                            var sig = cpi.Signature;
                                            sig = sig.Substring(sig.IndexOf(')') + 1);
                                            if (sig != "V")
                                                s.PushType(sig);
                                            break;
                                        }
                                    case NormalizedOpCode.Goto:
                                        break;
                                    case NormalizedOpCode.Istore:
                                        s.PopPrimitive();
                                        s.SetLocalPrimitive(instr.NormalizedArg1, i);
                                        break;
                                    case NormalizedOpCode.Iload:
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Ineg:
                                        s.PopPrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Iadd:
                                    case NormalizedOpCode.Isub:
                                    case NormalizedOpCode.Imul:
                                    case NormalizedOpCode.Idiv:
                                    case NormalizedOpCode.Irem:
                                    case NormalizedOpCode.Iand:
                                    case NormalizedOpCode.Ior:
                                    case NormalizedOpCode.Ixor:
                                    case NormalizedOpCode.Ishl:
                                    case NormalizedOpCode.Ishr:
                                    case NormalizedOpCode.Iushr:
                                        s.PopPrimitive();
                                        s.PopPrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Lneg:
                                        s.PopWidePrimitive();
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.Ladd:
                                    case NormalizedOpCode.Lsub:
                                    case NormalizedOpCode.Lmul:
                                    case NormalizedOpCode.Ldiv:
                                    case NormalizedOpCode.Lrem:
                                    case NormalizedOpCode.Land:
                                    case NormalizedOpCode.Lor:
                                    case NormalizedOpCode.Lxor:
                                        s.PopWidePrimitive();
                                        s.PopWidePrimitive();
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.Lshl:
                                    case NormalizedOpCode.Lshr:
                                    case NormalizedOpCode.Lushr:
                                        s.PopPrimitive();
                                        s.PopWidePrimitive();
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.Fneg:
                                        s.PopPrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Fadd:
                                    case NormalizedOpCode.Fsub:
                                    case NormalizedOpCode.Fmul:
                                    case NormalizedOpCode.Fdiv:
                                    case NormalizedOpCode.Frem:
                                        s.PopPrimitive();
                                        s.PopPrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Dneg:
                                        s.PopWidePrimitive();
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.Dadd:
                                    case NormalizedOpCode.Dsub:
                                    case NormalizedOpCode.Dmul:
                                    case NormalizedOpCode.Ddiv:
                                    case NormalizedOpCode.Drem:
                                        s.PopWidePrimitive();
                                        s.PopWidePrimitive();
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.New:
                                        s.PushObject();
                                        break;
                                    case NormalizedOpCode.Multianewarray:
                                        {
                                            if (instr.Arg2 < 1)
                                                throw new VerifyError("Illegal dimension argument");

                                            for (int j = 0; j < instr.Arg2; j++)
                                                s.PopPrimitive();

                                            s.PushObject();
                                            break;
                                        }
                                    case NormalizedOpCode.Anewarray:
                                        s.PopPrimitive();
                                        s.PushObject();
                                        break;
                                    case NormalizedOpCode.Newarray:
                                        s.PopPrimitive();
                                        s.PushObject();
                                        break;
                                    case NormalizedOpCode.Swap:
                                        {
                                            var t1 = s.PopType();
                                            var t2 = s.PopType();
                                            s.PushType(t1);
                                            s.PushType(t2);
                                            break;
                                        }
                                    case NormalizedOpCode.Dup:
                                        {
                                            var t = s.PopType();
                                            s.PushType(t);
                                            s.PushType(t);
                                            break;
                                        }
                                    case NormalizedOpCode.Dup2:
                                        {
                                            var t = s.PopAnyType();
                                            if (t.IsWidePrimitive)
                                            {
                                                s.PushType(t);
                                                s.PushType(t);
                                            }
                                            else
                                            {
                                                var t2 = s.PopType();
                                                s.PushType(t2);
                                                s.PushType(t);
                                                s.PushType(t2);
                                                s.PushType(t);
                                            }
                                            break;
                                        }
                                    case NormalizedOpCode.DupX1:
                                        {
                                            var value1 = s.PopType();
                                            var value2 = s.PopType();
                                            s.PushType(value1);
                                            s.PushType(value2);
                                            s.PushType(value1);
                                            break;
                                        }
                                    case NormalizedOpCode.Dup2X1:
                                        {
                                            var value1 = s.PopAnyType();
                                            if (value1.IsWidePrimitive)
                                            {
                                                var value2 = s.PopType();
                                                s.PushType(value1);
                                                s.PushType(value2);
                                                s.PushType(value1);
                                            }
                                            else
                                            {
                                                var value2 = s.PopType();
                                                var value3 = s.PopType();
                                                s.PushType(value2);
                                                s.PushType(value1);
                                                s.PushType(value3);
                                                s.PushType(value2);
                                                s.PushType(value1);
                                            }
                                            break;
                                        }
                                    case NormalizedOpCode.DupX2:
                                        {
                                            var value1 = s.PopType();
                                            var value2 = s.PopAnyType();
                                            if (value2.IsWidePrimitive)
                                            {
                                                s.PushType(value1);
                                                s.PushType(value2);
                                                s.PushType(value1);
                                            }
                                            else
                                            {
                                                var value3 = s.PopType();
                                                s.PushType(value1);
                                                s.PushType(value3);
                                                s.PushType(value2);
                                                s.PushType(value1);
                                            }
                                            break;
                                        }
                                    case NormalizedOpCode.Dup2X2:
                                        {
                                            var value1 = s.PopAnyType();
                                            if (value1.IsWidePrimitive)
                                            {
                                                var value2 = s.PopAnyType();
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
                                                    var value3 = s.PopType();
                                                    s.PushType(value1);
                                                    s.PushType(value3);
                                                    s.PushType(value2);
                                                    s.PushType(value1);
                                                }
                                            }
                                            else
                                            {
                                                var value2 = s.PopType();
                                                var value3 = s.PopAnyType();
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
                                                    var value4 = s.PopType();
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
                                    case NormalizedOpCode.Pop:
                                        s.PopType();
                                        break;
                                    case NormalizedOpCode.Pop2:
                                        {
                                            var type = s.PopAnyType();
                                            if (!type.IsWidePrimitive)
                                                s.PopType();

                                            break;
                                        }
                                    case NormalizedOpCode.MonitorEnter:
                                    case NormalizedOpCode.MonitorExit:
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.Return:
                                        break;
                                    case NormalizedOpCode.Areturn:
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.Ireturn:
                                        s.PopPrimitive();
                                        break;
                                    case NormalizedOpCode.Lreturn:
                                        s.PopWidePrimitive();
                                        break;
                                    case NormalizedOpCode.Freturn:
                                        s.PopPrimitive();
                                        break;
                                    case NormalizedOpCode.Dreturn:
                                        s.PopWidePrimitive();
                                        break;
                                    case NormalizedOpCode.Fload:
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Fstore:
                                        s.PopPrimitive();
                                        s.SetLocalPrimitive(instr.NormalizedArg1, i);
                                        break;
                                    case NormalizedOpCode.Dload:
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.Dstore:
                                        s.PopWidePrimitive();
                                        s.SetLocalWidePrimitive(instr.NormalizedArg1, i);
                                        break;
                                    case NormalizedOpCode.Lload:
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.Lstore:
                                        s.PopWidePrimitive();
                                        s.SetLocalWidePrimitive(instr.NormalizedArg1, i);
                                        break;
                                    case NormalizedOpCode.Lconst0:
                                    case NormalizedOpCode.Lconst1:
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.Fconst0:
                                    case NormalizedOpCode.Fconst1:
                                    case NormalizedOpCode.Fconst2:
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Dconst0:
                                    case NormalizedOpCode.Dconst1:
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.Lcmp:
                                        s.PopWidePrimitive();
                                        s.PopWidePrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Fcmpl:
                                    case NormalizedOpCode.Fcmpg:
                                        s.PopPrimitive();
                                        s.PopPrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Dcmpl:
                                    case NormalizedOpCode.Dcmpg:
                                        s.PopWidePrimitive();
                                        s.PopWidePrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Checkcast:
                                        s.PopObjectType();
                                        s.PushObject();
                                        break;
                                    case NormalizedOpCode.InstanceOf:
                                        s.PopObjectType();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.Iinc:
                                        break;
                                    case NormalizedOpCode.Athrow:
                                        s.PopObjectType();
                                        break;
                                    case NormalizedOpCode.TableSwitch:
                                    case NormalizedOpCode.LookupSwitch:
                                        s.PopPrimitive();
                                        break;
                                    case NormalizedOpCode.I2b:
                                        s.PopPrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.I2c:
                                        s.PopPrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.I2s:
                                        s.PopPrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.I2l:
                                        s.PopPrimitive();
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.I2f:
                                        s.PopPrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.I2d:
                                        s.PopPrimitive();
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.L2i:
                                        s.PopWidePrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.L2f:
                                        s.PopWidePrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.L2d:
                                        s.PopWidePrimitive();
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.F2i:
                                        s.PopPrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.F2l:
                                        s.PopPrimitive();
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.F2d:
                                        s.PopPrimitive();
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.D2i:
                                        s.PopWidePrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.D2f:
                                        s.PopWidePrimitive();
                                        s.PushPrimitive();
                                        break;
                                    case NormalizedOpCode.D2l:
                                        s.PopWidePrimitive();
                                        s.PushWidePrimitive();
                                        break;
                                    case NormalizedOpCode.Jsr:
                                        // TODO make sure we're not calling a subroutine we're already in
                                        break;
                                    case NormalizedOpCode.Ret:
                                        {
                                            // TODO if we're returning from a higher level subroutine, invalidate
                                            // all the intermediate return addresses
                                            var subroutineIndex = s.GetLocalRet(instr.Arg1);
                                            s.CheckSubroutineActive(subroutineIndex);
                                            break;
                                        }
                                    case NormalizedOpCode.Nop:
                                        if (i + 1 == instructions.Length)
                                            throw new VerifyError("Falling off the end of the code");

                                        break;
                                    case NormalizedOpCode.InvokeDynamic:
                                        // it is impossible to have a valid invokedynamic in a pre-7.0 class file
                                        throw new VerifyError("Illegal type in constant pool");
                                    default:
                                        throw new NotImplementedException(instr.NormalizedOpCode.ToString());
                                }

                                if (s.GetStackHeight() > method.MaxStack)
                                    throw new VerifyError("Stack size too large");

                                for (int j = 0; j < method.ExceptionTable.Length; j++)
                                    if (method.ExceptionTable[j].EndIndex == i + 1)
                                        MergeExceptionHandler(method.ExceptionTable[j].HandlerIndex, s);

                                try
                                {
                                    // another big switch to handle the opcode targets
                                    switch (instr.NormalizedOpCode)
                                    {
                                        case NormalizedOpCode.TableSwitch:
                                        case NormalizedOpCode.LookupSwitch:
                                            for (int j = 0; j < instr.SwitchEntryCount; j++)
                                            {
                                                _state[instr.GetSwitchTargetIndex(j)] += s;
                                            }
                                            _state[instr.DefaultTarget] += s;
                                            break;
                                        case NormalizedOpCode.Ifeq:
                                        case NormalizedOpCode.Ifne:
                                        case NormalizedOpCode.Iflt:
                                        case NormalizedOpCode.Ifge:
                                        case NormalizedOpCode.Ifgt:
                                        case NormalizedOpCode.Ifle:
                                        case NormalizedOpCode.IfIcmpeq:
                                        case NormalizedOpCode.IfIcmpne:
                                        case NormalizedOpCode.IfIcmplt:
                                        case NormalizedOpCode.IfIcmpge:
                                        case NormalizedOpCode.IfIcmpgt:
                                        case NormalizedOpCode.IfIcmple:
                                        case NormalizedOpCode.IfAcmpeq:
                                        case NormalizedOpCode.IfAcmpne:
                                        case NormalizedOpCode.IfNull:
                                        case NormalizedOpCode.IfNonNull:
                                            _state[i + 1] += s;
                                            _state[instr.TargetIndex] += s;
                                            break;
                                        case NormalizedOpCode.Goto:
                                            _state[instr.TargetIndex] += s;
                                            break;
                                        case NormalizedOpCode.Jsr:
                                            {
                                                int index = instr.TargetIndex;
                                                s.SetSubroutineId(index);

                                                if (returnAddressTypes.TryGetValue(index, out var retAddressType) == false)
                                                {
                                                    retAddressType = SimpleType.MakeRet(index);
                                                    returnAddressTypes[index] = retAddressType;
                                                }

                                                s.PushType(retAddressType);
                                                _state[index] += s;

                                                var returns = GetReturnSites(i);
                                                if (returns != null)
                                                    foreach (int returnIndex in returns)
                                                        _state[i + 1] = InstructionState.MergeSubroutineReturn(_state[i + 1], s, _state[returnIndex], _state[returnIndex].GetLocalsModified(index));

                                                AddCallSite(index, i);
                                                break;
                                            }
                                        case NormalizedOpCode.Ret:
                                            {
                                                // HACK if the ret is processed before all of the jsr instructions to this subroutine
                                                // we wouldn't be able to properly merge, so that is why we track the number of callsites
                                                // for each subroutine instruction (see Instruction.AddCallSite())
                                                var subroutineIndex = s.GetLocalRet(instr.Arg1);
                                                var cs = GetCallSites(subroutineIndex);
                                                var locals_modified = s.GetLocalsModified(subroutineIndex);
                                                for (int j = 0; j < cs.Length; j++)
                                                {
                                                    AddReturnSite(cs[j], i);
                                                    _state[cs[j] + 1] = InstructionState.MergeSubroutineReturn(_state[cs[j] + 1], _state[cs[j]], s, locals_modified);
                                                }

                                                break;
                                            }
                                        case NormalizedOpCode.Ireturn:
                                        case NormalizedOpCode.Lreturn:
                                        case NormalizedOpCode.Freturn:
                                        case NormalizedOpCode.Dreturn:
                                        case NormalizedOpCode.Areturn:
                                        case NormalizedOpCode.Return:
                                        case NormalizedOpCode.Athrow:
                                            break;
                                        default:
                                            _state[i + 1] += s;
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
                                var opcode = instructions[i].NormalizedOpCode.ToString();
                                if (opcode.StartsWith("__"))
                                    opcode = opcode.Substring(2);

                                throw new VerifyError($"{x.Message} (class: {classFile.Name}, method: {method.Name}, signature: {method.Signature}, offset: {instructions[i].PC}, instruction: {opcode})", x);
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
                    var didJsrOrRet = false;
                    for (int i = 0; i < instructions.Length; i++)
                    {
                        if ((flags[i] & (InstructionFlags.Reachable | InstructionFlags.Processed)) == InstructionFlags.Reachable)
                        {
                            done = false;
                            flags[i] |= InstructionFlags.Processed;

                            // mark the exception handlers reachable from this instruction
                            for (int j = 0; j < method.ExceptionTable.Length; j++)
                                if (method.ExceptionTable[j].StartIndex <= i && i < method.ExceptionTable[j].EndIndex)
                                    flags[method.ExceptionTable[j].HandlerIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;

                            // mark the successor instructions
                            switch (instructions[i].NormalizedOpCode)
                            {
                                case NormalizedOpCode.TableSwitch:
                                case NormalizedOpCode.LookupSwitch:
                                    {
                                        var hasbackbranch = false;
                                        for (int j = 0; j < instructions[i].SwitchEntryCount; j++)
                                        {
                                            hasbackbranch |= instructions[i].GetSwitchTargetIndex(j) < i;
                                            flags[instructions[i].GetSwitchTargetIndex(j)] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                                        }

                                        hasbackbranch |= instructions[i].DefaultTarget < i;
                                        flags[instructions[i].DefaultTarget] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                                        break;
                                    }
                                case NormalizedOpCode.Goto:
                                    flags[instructions[i].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                                    break;
                                case NormalizedOpCode.Ifeq:
                                case NormalizedOpCode.Ifne:
                                case NormalizedOpCode.Iflt:
                                case NormalizedOpCode.Ifge:
                                case NormalizedOpCode.Ifgt:
                                case NormalizedOpCode.Ifle:
                                case NormalizedOpCode.IfIcmpeq:
                                case NormalizedOpCode.IfIcmpne:
                                case NormalizedOpCode.IfIcmplt:
                                case NormalizedOpCode.IfIcmpge:
                                case NormalizedOpCode.IfIcmpgt:
                                case NormalizedOpCode.IfIcmple:
                                case NormalizedOpCode.IfAcmpeq:
                                case NormalizedOpCode.IfAcmpne:
                                case NormalizedOpCode.IfNull:
                                case NormalizedOpCode.IfNonNull:
                                    flags[instructions[i].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                                    flags[i + 1] |= InstructionFlags.Reachable;
                                    break;
                                case NormalizedOpCode.Jsr:
                                    flags[instructions[i].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                                    // Note that we don't mark the next instruction as reachable,
                                    // because that depends on the corresponding ret actually being
                                    // reachable. We handle this in the loop below.
                                    didJsrOrRet = true;
                                    break;
                                case NormalizedOpCode.Ret:
                                    // Note that we can't handle ret here, because we might encounter the ret
                                    // before having seen all the corresponding jsr instructions, so we can't
                                    // update all the call sites.
                                    // We handle ret in the loop below.
                                    didJsrOrRet = true;
                                    break;
                                case NormalizedOpCode.Ireturn:
                                case NormalizedOpCode.Lreturn:
                                case NormalizedOpCode.Freturn:
                                case NormalizedOpCode.Dreturn:
                                case NormalizedOpCode.Areturn:
                                case NormalizedOpCode.Return:
                                case NormalizedOpCode.Athrow:
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
                            if (instructions[i].NormalizedOpCode == NormalizedOpCode.Ret && (flags[i] & InstructionFlags.Reachable) != 0)
                            {
                                var subroutineIndex = _state[i].GetLocalRet(instructions[i].Arg1);
                                var cs = GetCallSites(subroutineIndex);
                                for (int j = 0; j < cs.Length; j++)
                                    if ((flags[cs[j]] & InstructionFlags.Reachable) != 0)
                                        flags[cs[j] + 1] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                            }
                        }
                    }
                }
            }

            void MergeExceptionHandler(int handlerIndex, InstructionState curr)
            {
                // NOTE this used to be CopyLocalsAndSubroutines, but it doesn't (always) make
                // sense to copy the subroutine state
                // TODO figure out if there are circumstances under which it does make sense
                // to copy the active subroutine state
                // UPDATE subroutines must be copied as well, but I think I now have a better
                // understanding of subroutine merges, so the problems that triggered the previous
                // change here hopefully won't arise anymore
                var ex = curr.CopyLocalsAndSubroutines();
                ex.PushObject();
                _state[handlerIndex] += ex;
            }

            ConstantPoolItemMI GetMethodref(int index)
            {
                try
                {
                    var item = _classFile.GetMethodref(new MethodrefConstantHandle(checked((ushort)index)));
                    if (item != null)
                        return item;
                }
                catch (OverflowException)
                {
                    // constant pool index isn't ushort
                }
                catch (InvalidCastException)
                {

                }
                catch (IndexOutOfRangeException)
                {

                }

                throw new VerifyError("Illegal constant pool index");
            }

            ConstantPoolItemFieldref GetFieldref(int index)
            {
                try
                {
                    var item = _classFile.GetFieldref(new FieldrefConstantHandle(checked((ushort)index)));
                    if (item != null)
                        return item;
                }
                catch (OverflowException)
                {
                    // constant pool index isn't ushort
                }
                catch (InvalidCastException)
                {
                    // constant pool index isn't ushort
                }
                catch (IndexOutOfRangeException)
                {

                }

                throw new VerifyError("Illegal constant pool index");
            }

            ConstantType GetConstantPoolConstantType(int index)
            {
                try
                {
                    return _classFile.GetConstantPoolConstantType(new ConstantHandle(ConstantKind.Unknown, checked((ushort)index)));
                }
                catch (OverflowException)
                {
                    // constant pool index isn't ushort
                }
                catch (InvalidCastException)
                {
                    // constant pool index isn't ushort
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

            void AddReturnSite(int callSiteIndex, int returnSiteIndex)
            {
                _returnsites[callSiteIndex] ??= new List<int>();

                var l = _returnsites[callSiteIndex];
                if (l.IndexOf(returnSiteIndex) == -1)
                {
                    _state[callSiteIndex].changed = true;
                    l.Add(returnSiteIndex);
                }
            }

            List<int> GetReturnSites(int callSiteIndex)
            {
                return _returnsites[callSiteIndex];
            }

            void AddCallSite(int subroutineIndex, int callSiteIndex)
            {
                _callsites[subroutineIndex] ??= new List<int>();

                var l = _callsites[subroutineIndex];
                if (l.IndexOf(callSiteIndex) == -1)
                {
                    l.Add(callSiteIndex);
                    _state[subroutineIndex].AddCallSite();
                }
            }

            int[] GetCallSites(int subroutineIndex)
            {
                return _callsites[subroutineIndex].ToArray();
            }

            internal SimpleType GetLocalTypeWrapper(int index, int local)
            {
                return _state[index].GetLocalTypeEx(local);
            }

            internal bool IsSubroutineActive(int instructionIndex, int subroutineIndex)
            {
                return _state[instructionIndex].IsSubroutineActive(subroutineIndex);
            }

            sealed class Subroutine
            {

                readonly int subroutineIndex;
                readonly bool[] localsModified;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="subroutineIndex"></param>
                /// <param name="localsModified"></param>
                Subroutine(int subroutineIndex, bool[] localsModified)
                {
                    this.subroutineIndex = subroutineIndex;
                    this.localsModified = localsModified;
                }

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="subroutineIndex"></param>
                /// <param name="maxLocals"></param>
                internal Subroutine(int subroutineIndex, int maxLocals)
                {
                    this.subroutineIndex = subroutineIndex;
                    localsModified = new bool[maxLocals];
                }

                internal int SubroutineIndex => subroutineIndex;

                internal bool[] LocalsModified => localsModified;

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

                enum ShareFlags : byte
                {
                    None = 0,
                    Stack = 1,
                    Locals = 2,
                    Subroutines = 4,
                    All = Stack | Locals | Subroutines
                }

                SimpleType[] stack;
                int stackSize;
                int stackEnd;
                SimpleType[] locals;
                List<Subroutine> subroutines;
                int callsites;
                internal bool changed = true;
                ShareFlags flags;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="stack"></param>
                /// <param name="stackSize"></param>
                /// <param name="stackEnd"></param>
                /// <param name="locals"></param>
                /// <param name="subroutines"></param>
                /// <param name="callsites"></param>
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

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="maxLocals"></param>
                /// <param name="maxStack"></param>
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
                    var copy = new InstructionState(new SimpleType[stack.Length], 0, stack.Length, locals, subroutines, callsites);
                    copy.flags &= ~ShareFlags.Stack;
                    return copy;
                }

                private static List<Subroutine> CopySubroutines(List<Subroutine> l)
                {
                    if (l == null)
                        return null;

                    var n = new List<Subroutine>(l.Count);
                    foreach (var s in l)
                        n.Add(s.Copy());

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

                        var ss1 = subroutines;
                        subroutines = new List<Subroutine>();
                        foreach (var ss2 in s2.subroutines)
                        {
                            foreach (var ss in ss1)
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
                            changed = true;
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
                    var next = ret.Copy();
                    next.LocalsCopyOnWrite();

                    for (int i = 0; i < locals_modified.Length; i++)
                        if (!locals_modified[i])
                            next.locals[i] = jsr.locals[i];

                    next.flags |= ShareFlags.Subroutines;
                    next.subroutines = jsr.subroutines;
                    next.callsites = jsr.callsites;
                    return jsrSuccessor + next;
                }

                public static InstructionState operator +(InstructionState s1, InstructionState s2)
                {
                    if (s1 == null)
                        return s2.Copy();

                    if (s1.stackSize != s2.stackSize || s1.stackEnd != s2.stackEnd)
                        throw new VerifyError($"Inconsistent stack height: {s1.stackSize + s1.stack.Length - s1.stackEnd} != {s2.stackSize + s2.stack.Length - s2.stackEnd}");

                    var s = s1.Copy();
                    s.changed = s1.changed;
                    for (int i = 0; i < s.stackSize; i++)
                    {
                        var type = s.stack[i];
                        var type2 = s2.stack[i];
                        if (type == type2)
                        {
                            // perfect match, nothing to do
                        }
                        else if (!type.IsPrimitive)
                        {
                            var baseType = InstructionState.FindCommonBaseType(type, type2);
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
                        var type = s.locals[i];
                        var type2 = s2.locals[i];
                        var baseType = InstructionState.FindCommonBaseType(type, type2);
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
                        foreach (var s in subroutines)
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
                        foreach (var s in subroutines)
                            if (s.SubroutineIndex == subroutineIndex)
                                return s.LocalsModified;

                    throw new VerifyError("return from wrong subroutine");
                }

                internal bool IsSubroutineActive(int subroutineIndex)
                {
                    if (subroutines != null)
                        foreach (var s in subroutines)
                            if (s.SubroutineIndex == subroutineIndex)
                                return true;

                    return false;
                }

                internal void CheckSubroutineActive(int subroutineIndex)
                {
                    if (!IsSubroutineActive(subroutineIndex))
                        throw new VerifyError("inactive subroutine");
                }

                internal static SimpleType FindCommonBaseType(SimpleType type1, SimpleType type2)
                {
                    if (type1 == type2)
                        return type1;

                    if (type1 == SimpleType.Object)
                        return type2;

                    if (type2 == SimpleType.Object)
                        return type1;

                    if (type1 == SimpleType.Invalid || type2 == SimpleType.Invalid)
                        return SimpleType.Invalid;

                    if (type1.IsPrimitive || type2.IsPrimitive)
                        return SimpleType.Invalid;

                    if (SimpleType.IsRet(type1) || SimpleType.IsRet(type2))
                        return SimpleType.Invalid;

                    return SimpleType.Object;
                }

                void SetLocal1(int index, SimpleType type)
                {
                    try
                    {
                        LocalsCopyOnWrite();
                        SubroutinesCopyOnWrite();

                        if (index > 0 && locals[index - 1] != SimpleType.Invalid && locals[index - 1].IsWidePrimitive)
                        {
                            locals[index - 1] = SimpleType.Invalid;
                            if (subroutines != null)
                                foreach (var s in subroutines)
                                    s.SetLocalModified(index - 1);
                        }

                        locals[index] = type;
                        if (subroutines != null)
                            foreach (var s in subroutines)
                                s.SetLocalModified(index);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new VerifyError("Illegal local variable number");
                    }
                }

                void SetLocal2(int index, SimpleType type)
                {
                    try
                    {
                        LocalsCopyOnWrite();
                        SubroutinesCopyOnWrite();

                        if (index > 0 && locals[index - 1] != SimpleType.Invalid && locals[index - 1].IsWidePrimitive)
                        {
                            locals[index - 1] = SimpleType.Invalid;
                            if (subroutines != null)
                                foreach (var s in subroutines)
                                    s.SetLocalModified(index - 1);
                        }

                        locals[index] = type;
                        locals[index + 1] = SimpleType.Invalid;

                        if (subroutines != null)
                        {
                            foreach (var s in subroutines)
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
                    var type = GetLocalType(index);
                    if (SimpleType.IsRet(type))
                        return type.SubroutineIndex;

                    throw new VerifyError("incorrect local type, not ret");
                }

                internal void SetLocalType(int index, SimpleType type, int instructionIndex)
                {
                    if (type.IsWidePrimitive)
                        SetLocalWidePrimitive(index, instructionIndex);
                    else
                        SetLocal1(index, type);
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
                    var type = PopType();
                    if (type.IsPrimitive)
                        throw new VerifyError("Expected object reference on stack");

                    return type;
                }

                internal void MultiPopAnyType(int count)
                {
                    while (count-- != 0)
                        PopAnyType();
                }

                internal SimpleType PopAnyType()
                {
                    if (stackSize == 0)
                        throw new VerifyError("Unable to pop operand off an empty stack");

                    var type = stack[--stackSize];
                    if (type.IsWidePrimitive)
                        stackEnd++;

                    return type;
                }

                // NOTE this can *not* be used to pop double or long
                internal SimpleType PopType()
                {
                    var type = PopAnyType();
                    if (type.IsWidePrimitive)
                        throw new VerifyError("Attempt to split long or double on the stack");

                    return type;
                }

                internal void PopPrimitive()
                {
                    if (!PopType().IsPrimitive)
                        throw new VerifyError("Primitive type expected on stack");
                }

                internal void PopWidePrimitive()
                {
                    var type = PopAnyType();
                    if (type != SimpleType.WidePrimitive)
                        throw new VerifyError("Wide primitive type expected on stack");
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
                        stackEnd--;

                    if (stackSize >= stackEnd)
                        throw new VerifyError("Stack overflow");

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
