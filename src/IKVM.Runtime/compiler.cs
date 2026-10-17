/*
  Copyright (C) 2002-2015 Jeroen Frijters

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
using System.Diagnostics.SymbolStore;

using IKVM.Attributes;
using IKVM.ByteCode;
using IKVM.CoreLib.Runtime;
using IKVM.CoreLib.Linking;
using IKVM.ByteCode.Decoding;

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    sealed class Compiler
    {

        readonly RuntimeByteCodeJavaType.FinishContext finish;
        readonly RuntimeByteCodeJavaType clazz;
        readonly RuntimeJavaMethod mw;
        readonly ClassFile classFile;
        readonly Method m;
        readonly CodeEmitter ilGenerator;
        readonly CodeInfo ma;
        readonly UntangledExceptionTable exceptions;
        readonly List<string> harderrors;
        readonly LocalVarInfo localVars;
        bool nonleaf;
        readonly bool debug;
        readonly bool keepAlive;
        readonly bool strictfp;
        readonly bool emitLineNumbers;
        int[] scopeBegin;
        int[] scopeClose;

#if IMPORTER
        readonly RuntimeJavaMethod[] replacedMethodWrappers;
#endif

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="finish"></param>
        /// <param name="host"></param>
        /// <param name="clazz"></param>
        /// <param name="mw"></param>
        /// <param name="classFile"></param>
        /// <param name="m"></param>
        /// <param name="ilGenerator"></param>
        /// <param name="classLoader"></param>
        Compiler(RuntimeByteCodeJavaType.FinishContext finish, RuntimeJavaType host, RuntimeByteCodeJavaType clazz, RuntimeJavaMethod mw, ClassFile classFile, Method m, CodeEmitter ilGenerator, RuntimeClassLoader classLoader)
        {
            this.finish = finish;
            this.clazz = clazz;
            this.mw = mw;
            this.classFile = classFile;
            this.m = m;
            this.ilGenerator = ilGenerator;
            this.debug = classLoader.EmitSymbols;
            this.strictfp = m.IsStrictfp;
            if (mw.IsConstructor)
            {
                var finalize = clazz.GetMethod(StringConstants.FINALIZE, StringConstants.SIG_VOID, true);
                keepAlive = finalize != null && finalize.DeclaringType != finish.Context.JavaBase.TypeOfJavaLangObject && finalize.DeclaringType != finish.Context.JavaBase.TypeOfCliSystemObject && finalize.DeclaringType != finish.Context.JavaBase.TypeOfjavaLangThrowable && finalize.DeclaringType != finish.Context.JavaBase.TypeOfCliSystemException;
            }

#if IMPORTER
            replacedMethodWrappers = clazz.GetReplacedMethodsFor(mw);
#endif

            var args = mw.GetParameters();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].IsUnloadable)
                {
                    ilGenerator.EmitLdarg(i + (m.IsStatic ? 0 : 1));
                    EmitDynamicCast(args[i]);
                    ilGenerator.Emit(OpCodes.Pop);
                }
            }

            Profiler.Enter("MethodAnalyzer");
            try
            {
                if (classFile.MajorVersion < 51 && m.HasJsr)
                    JsrInliner.InlineJsrs(classLoader, mw, classFile, m);
                var verifier = finish.Context.MethodAnalyzerFactory.Create(host, clazz, mw, classFile, m, classLoader);
                exceptions = MethodAnalyzer.UntangleExceptionBlocks(finish.Context, classFile, m);
                ma = verifier.GetCodeInfoAndErrors(exceptions, out harderrors);
                localVars = new LocalVarInfo(ma, classFile, m, exceptions, mw, classLoader);
            }
            finally
            {
                Profiler.Leave("MethodAnalyzer");
            }

            if (m.LineNumberTable.Count > 0)
            {
                if (classLoader.EmitSymbols)
                {
                    emitLineNumbers = true;
                }
                else if (classLoader.EmitStackTraceInfo)
                {
                    var flags = ComputePartialReachability(0, false);
                    for (int i = 0; i < m.Instructions.Length; i++)
                    {
                        if ((flags[i] & InstructionFlags.Reachable) == 0)
                        {
                            // skip unreachable instructions
                        }
                        else if (m.Instructions[i].NormalizedOpCode == NormalizedOpCode.GetField && RuntimeVerifierJavaType.IsThis(ma.GetRawStackTypeWrapper(i, 0)))
                        {
                            // loading a field from the current object cannot throw
                        }
                        else if (m.Instructions[i].NormalizedOpCode == NormalizedOpCode.PutField && RuntimeVerifierJavaType.IsThis(ma.GetRawStackTypeWrapper(i, 1)))
                        {
                            // storing a field in the current object cannot throw
                        }
                        else if (m.Instructions[i].NormalizedOpCode == NormalizedOpCode.GetStatic && classFile.GetFieldref(checked((ushort)m.Instructions[i].Arg1)).GetClassType() == clazz)
                        {
                            // loading a field from the current class cannot throw
                        }
                        else if (m.Instructions[i].NormalizedOpCode == NormalizedOpCode.PutStatic && classFile.GetFieldref(checked((ushort)m.Instructions[i].Arg1)).GetClassType() == clazz)
                        {
                            // storing a field to the current class cannot throw
                        }
                        else if (OpCodeMetaData.CanThrowException(m.Instructions[i].NormalizedOpCode))
                        {
                            emitLineNumbers = true;
                            break;
                        }
                    }
                }
            }

            var locals = localVars.GetAllLocalVars();
            foreach (var v in locals)
            {
                if (v.isArg)
                {
                    int arg = m.ArgMap[v.local];

                    RuntimeJavaType tw;
                    if (m.IsStatic)
                    {
                        tw = args[arg];
                    }
                    else if (arg == 0)
                    {
                        continue;
                    }
                    else
                    {
                        tw = args[arg - 1];
                    }

                    if (!tw.IsUnloadable && (v.type != tw || tw.TypeAsLocalOrStackType != tw.TypeAsSignatureType))
                    {
                        v.builder = ilGenerator.DeclareLocal(GetLocalBuilderType(v.type));
                        if (debug && v.name != null)
                            v.builder.SetLocalSymInfo(v.name);

                        v.isArg = false;
                        ilGenerator.EmitLdarg(arg);
                        tw.EmitConvSignatureTypeToStackType(ilGenerator);
                        ilGenerator.Emit(OpCodes.Stloc, v.builder);
                    }
                }
            }

            // if we're emitting debugging information, we need to use scopes for local variables
            if (debug)
                SetupLocalVariableScopes();

            Workaroundx64JitBug(args);
        }

        // workaround for x64 JIT bug
        // https://connect.microsoft.com/VisualStudio/feedback/details/636466/variable-is-not-incrementing-in-c-release-x64#details
        // (see also https://sourceforge.net/mailarchive/message.php?msg_id=28250469)
        void Workaroundx64JitBug(RuntimeJavaType[] args)
        {
            if (args.Length > (m.IsStatic ? 4 : 3) && m.ExceptionTable.Length != 0)
            {
                bool[] workarounds = null;
                var flags = ComputePartialReachability(0, false);
                for (int i = 0; i < m.Instructions.Length; i++)
                {
                    if ((flags[i] & InstructionFlags.Reachable) == 0)
                    {
                        // skip unreachable instructions
                    }
                    else
                    {
                        switch (m.Instructions[i].NormalizedOpCode)
                        {
                            case NormalizedOpCode.Iinc:
                            case NormalizedOpCode.Astore:
                            case NormalizedOpCode.Istore:
                            case NormalizedOpCode.Lstore:
                            case NormalizedOpCode.Fstore:
                            case NormalizedOpCode.Dstore:
                                int arg = m.IsStatic ? m.Instructions[i].Arg1 : m.Instructions[i].Arg1 - 1;
                                if (arg >= 3 && arg < args.Length)
                                {
                                    workarounds ??= new bool[args.Length + 1];
                                    workarounds[m.Instructions[i].Arg1] = true;
                                }
                                break;
                        }
                    }
                }

                if (workarounds != null)
                {
                    for (int i = 0; i < workarounds.Length; i++)
                    {
                        if (workarounds[i])
                        {
                            // TODO prevent this from getting optimized away
                            ilGenerator.EmitLdarga(i);
                            ilGenerator.Emit(OpCodes.Pop);
                        }
                    }
                }
            }
        }

        void SetupLocalVariableScopes()
        {
            var lvt = m.LocalVariableTable;
            if (lvt.Count > 0)
            {
                scopeBegin = new int[m.Instructions.Length];
                scopeClose = new int[m.Instructions.Length];

                // FXBUG make sure we always have an outer scope
                // (otherwise LocalBuilder.SetLocalSymInfo() might throw an IndexOutOfRangeException)
                // fix for bug 2881954.
                scopeBegin[0]++;
                scopeClose[m.Instructions.Length - 1]++;

                for (int i = 0; i < lvt.Count; i++)
                {
                    int startIndex = SafeFindPcIndex(lvt[i].StartPc);
                    int endIndex = SafeFindPcIndex(lvt[i].StartPc + lvt[i].Length);
                    if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
                    {
                        if (startIndex > 0)
                        {
                            // NOTE javac (correctly) sets start_pc of the LVT entry to the instruction
                            // following the store that first initializes the local, so we have to
                            // detect that case and adjust our local scope (because we'll be creating
                            // the local when we encounter the first store).
                            var v = localVars.GetLocalVar(startIndex - 1);
                            if (v != null && v.local == lvt[i].Slot)
                                startIndex--;
                        }

                        scopeBegin[startIndex]++;
                        scopeClose[endIndex]++;
                    }
                }
            }
        }

        int SafeFindPcIndex(int pc)
        {
            for (int i = 0; i < m.Instructions.Length; i++)
                if (m.Instructions[i].PC >= pc)
                    return i;

            return -1;
        }

        sealed class ReturnCookie
        {

            readonly CodeEmitterLabel stub;
            readonly CodeEmitterLocal local;

            internal ReturnCookie(CodeEmitterLabel stub, CodeEmitterLocal local)
            {
                this.stub = stub;
                this.local = local;
            }

            internal void EmitRet(CodeEmitter ilgen)
            {
                ilgen.MarkLabel(stub);
                if (local != null)
                    ilgen.Emit(OpCodes.Ldloc, local);

                ilgen.Emit(OpCodes.Ret);
            }

        }

        sealed class BranchCookie
        {

            // NOTE Stub gets used for both the push stub (inside the exception block) as well as the pop stub (outside the block)
            internal CodeEmitterLabel Stub;
            internal CodeEmitterLabel TargetLabel;
            internal bool ContentOnStack;
            internal readonly int TargetIndex;
            internal DupHelper dh;

            internal BranchCookie(Compiler compiler, int stackHeight, int targetIndex)
            {
                this.Stub = compiler.ilGenerator.DefineLabel();
                this.TargetIndex = targetIndex;
                this.dh = new DupHelper(compiler, stackHeight);
            }

            internal BranchCookie(CodeEmitterLabel label, int targetIndex)
            {
                this.Stub = label;
                this.TargetIndex = targetIndex;
            }

        }

        readonly struct DupHelper
        {

            enum StackType : byte
            {
                Null,
                New,
                This,
                UnitializedThis,
                FaultBlockException,
                Other
            }

            readonly Compiler _compiler;
            readonly StackType[] _types;
            readonly CodeEmitterLocal[] _locals;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="compiler"></param>
            /// <param name="count"></param>
            internal DupHelper(Compiler compiler, int count)
            {
                _compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
                _types = new StackType[count];
                _locals = new CodeEmitterLocal[count];
            }

            internal readonly void Release()
            {
                foreach (var lb in _locals)
                    if (lb != null)
                        _compiler.ilGenerator.ReleaseTempLocal(lb);
            }

            internal readonly int Count => _types.Length;

            internal readonly void SetType(int i, RuntimeJavaType type)
            {
                if (type == _compiler.finish.Context.VerifierJavaTypeFactory.Null)
                {
                    _types[i] = StackType.Null;
                }
                else if (RuntimeVerifierJavaType.IsNew(type))
                {
                    // new objects aren't really there on the stack
                    _types[i] = StackType.New;
                }
                else if (RuntimeVerifierJavaType.IsThis(type))
                {
                    _types[i] = StackType.This;
                }
                else if (type == _compiler.finish.Context.VerifierJavaTypeFactory.UninitializedThis)
                {
                    // uninitialized references cannot be stored in a local, but we can reload them
                    _types[i] = StackType.UnitializedThis;
                }
                else if (RuntimeVerifierJavaType.IsFaultBlockException(type))
                {
                    _types[i] = StackType.FaultBlockException;
                }
                else
                {
                    _types[i] = StackType.Other;
                    _locals[i] = _compiler.ilGenerator.AllocTempLocal(_compiler.GetLocalBuilderType(type));
                }
            }

            internal readonly void Load(int i)
            {
                switch (_types[i])
                {
                    case StackType.Null:
                        _compiler.ilGenerator.Emit(OpCodes.Ldnull);
                        break;
                    case StackType.New:
                    case StackType.FaultBlockException:
                        // objects aren't really there on the stack
                        break;
                    case StackType.This:
                    case StackType.UnitializedThis:
                        _compiler.ilGenerator.Emit(OpCodes.Ldarg_0);
                        break;
                    case StackType.Other:
                        _compiler.ilGenerator.Emit(OpCodes.Ldloc, _locals[i]);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

            internal readonly void Store(int i)
            {
                switch (_types[i])
                {
                    case StackType.Null:
                    case StackType.This:
                    case StackType.UnitializedThis:
                        _compiler.ilGenerator.Emit(OpCodes.Pop);
                        break;
                    case StackType.New:
                    case StackType.FaultBlockException:
                        // objects aren't really there on the stack
                        break;
                    case StackType.Other:
                        _compiler.ilGenerator.Emit(OpCodes.Stloc, _locals[i]);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

        }

        internal static void Compile(RuntimeByteCodeJavaType.FinishContext finish, RuntimeJavaType host, RuntimeByteCodeJavaType clazz, RuntimeJavaMethod mw, ClassFile classFile, Method m, CodeEmitter ilGenerator, ref bool nonleaf)
        {
            var classLoader = clazz.ClassLoader;
            if (classLoader.EmitSymbols)
            {
                if (classFile.SourcePath != null)
                {
                    ilGenerator.DefineSymbolDocument(classLoader.GetTypeWrapperFactory().ModuleBuilder, classFile.SourcePath, SymLanguageType.Java, Guid.Empty, SymDocumentType.Text);

                    // the very first instruction in the method must have an associated line number, to be able
                    // to step into the method in Visual Studio .NET
                    var table = m.LineNumberTable;
                    if (table.Count > 0)
                    {
                        int firstPC = int.MaxValue;
                        int firstLine = -1;
                        for (int i = 0; i < table.Count; i++)
                        {
                            if (table[i].StartPc < firstPC && table[i].LineNumber != 0)
                            {
                                firstLine = table[i].LineNumber;
                                firstPC = table[i].StartPc;
                            }
                        }

                        if (firstLine > 0)
                            ilGenerator.SetLineNumber((ushort)firstLine);
                    }
                }
            }

            Compiler c;
            try
            {
                Profiler.Enter("new Compiler");
                try
                {
                    c = new Compiler(finish, host, clazz, mw, classFile, m, ilGenerator, classLoader);
                }
                finally
                {
                    Profiler.Leave("new Compiler");
                }
            }
            catch (VerifyError x)
            {
                classLoader.Diagnostics.EmittedVerificationError(classFile.Name + "." + m.Name + m.Signature, x.Message);
                clazz.SetHasVerifyError();
                // because in Java the method is only verified if it is actually called,
                // we generate code here to throw the VerificationError
                ilGenerator.EmitThrow("java.lang.VerifyError", x.Message);
                return;
            }
            catch (ClassFormatException x)
            {
                classLoader.Diagnostics.EmittedClassFormatError(classFile.Name + "." + m.Name + m.Signature, x.Message);
                clazz.SetHasClassFormatError();
                ilGenerator.EmitThrow("java.lang.ClassFormatError", x.Message);
                return;
            }

            Profiler.Enter("Compile");

            try
            {
                if (m.IsSynchronized && m.IsStatic)
                {
                    clazz.EmitClassLiteral(ilGenerator);
                    ilGenerator.Emit(OpCodes.Dup);
                    CodeEmitterLocal monitor = ilGenerator.DeclareLocal(finish.Context.Types.Object);
                    ilGenerator.Emit(OpCodes.Stloc, monitor);
                    ilGenerator.EmitMonitorEnter();
                    ilGenerator.BeginExceptionBlock();
                    var b = new Block(c, 0, int.MaxValue, -1, new List<object>(), true);
                    c.Compile(b, 0);
                    b.Leave();
                    ilGenerator.BeginFinallyBlock();
                    ilGenerator.Emit(OpCodes.Ldloc, monitor);
                    ilGenerator.EmitMonitorExit();
                    ilGenerator.Emit(OpCodes.Endfinally);
                    ilGenerator.EndExceptionBlock();
                    b.LeaveStubs(new Block(c, 0, int.MaxValue, -1, null, false));
                }
                else
                {
                    var b = new Block(c, 0, int.MaxValue, -1, null, false);
                    c.Compile(b, 0);
                    b.Leave();
                }

                nonleaf = c.nonleaf;
            }
            finally
            {
                Profiler.Leave("Compile");
            }
        }

        sealed class Block
        {

            readonly Compiler compiler;
            readonly CodeEmitter ilgen;
            readonly int beginIndex;
            readonly int endIndex;
            readonly int exceptionIndex;
            List<object> exits;
            readonly bool nested;
            readonly object[] labels;

            internal Block(Compiler compiler, int beginIndex, int endIndex, int exceptionIndex, List<object> exits, bool nested)
            {
                this.compiler = compiler;
                this.ilgen = compiler.ilGenerator;
                this.beginIndex = beginIndex;
                this.endIndex = endIndex;
                this.exceptionIndex = exceptionIndex;
                this.exits = exits;
                this.nested = nested;
                labels = new object[compiler.m.Instructions.Length];
            }

            internal int EndIndex
            {
                get
                {
                    return endIndex;
                }
            }

            internal int ExceptionIndex
            {
                get
                {
                    return exceptionIndex;
                }
            }

            internal void SetBackwardBranchLabel(int instructionIndex, BranchCookie bc)
            {
                // NOTE we're overwriting the label that is already there
                labels[instructionIndex] = bc.Stub;
                if (exits == null)
                {
                    exits = new List<object>();
                }
                exits.Add(bc);
            }

            internal CodeEmitterLabel GetLabel(int targetIndex)
            {
                if (IsInRange(targetIndex))
                {
                    var l = (CodeEmitterLabel)labels[targetIndex];
                    if (l == null)
                    {
                        l = ilgen.DefineLabel();
                        labels[targetIndex] = l;
                    }
                    return l;
                }
                else
                {
                    var l = (BranchCookie)labels[targetIndex];
                    if (l == null)
                    {
                        // if we're branching out of the current exception block, we need to indirect this thru a stub
                        // that saves the stack and uses leave to leave the exception block (to another stub that recovers
                        // the stack)
                        int stackHeight = compiler.ma.GetStackHeight(targetIndex);
                        var bc = new BranchCookie(compiler, stackHeight, targetIndex);
                        bc.ContentOnStack = true;
                        for (int i = 0; i < stackHeight; i++)
                            bc.dh.SetType(i, compiler.ma.GetRawStackTypeWrapper(targetIndex, i));
                        exits.Add(bc);
                        l = bc;
                        labels[targetIndex] = l;
                    }
                    return l.Stub;
                }
            }

            internal bool HasLabel(int instructionIndex)
            {
                return labels[instructionIndex] != null;
            }

            internal void MarkLabel(int instructionIndex)
            {
                object label = labels[instructionIndex];
                if (label == null)
                {
                    var l = ilgen.DefineLabel();
                    ilgen.MarkLabel(l);
                    labels[instructionIndex] = l;
                }
                else
                {
                    ilgen.MarkLabel((CodeEmitterLabel)label);
                }
            }

            internal bool IsInRange(int index)
            {
                return beginIndex <= index && index < endIndex;
            }

            internal void Leave()
            {
                if (exits != null)
                {
                    for (int i = 0; i < exits.Count; i++)
                    {
                        var exit = exits[i];
                        var bc = exit as BranchCookie;
                        if (bc != null && bc.ContentOnStack)
                        {
                            bc.ContentOnStack = false;
                            int stack = bc.dh.Count;

                            // HACK this is unreachable code, but we make sure that
                            // forward pass verification always yields a valid stack
                            // (this is required for unreachable leave stubs that are
                            // generated for unreachable code that follows an
                            // embedded exception emitted by the compiler for invalid
                            // code (e.g. NoSuchFieldError))
                            for (int n = stack - 1; n >= 0; n--)
                                bc.dh.Load(n);

                            ilgen.MarkLabel(bc.Stub);
                            for (int n = 0; n < stack; n++)
                                bc.dh.Store(n);

                            if (bc.TargetIndex == -1)
                            {
                                ilgen.EmitBr(bc.TargetLabel);
                            }
                            else
                            {
                                bc.Stub = ilgen.DefineLabel();
                                ilgen.EmitLeave(bc.Stub);
                            }
                        }
                    }
                }
            }

            internal void LeaveStubs(Block newBlock)
            {
                if (exits != null)
                {
                    for (int i = 0; i < exits.Count; i++)
                    {
                        var exit = exits[i];
                        var rc = exit as ReturnCookie;
                        if (rc != null)
                        {
                            if (newBlock.IsNested)
                                newBlock.exits.Add(rc);
                            else
                                rc.EmitRet(ilgen);
                        }
                        else
                        {
                            var bc = exit as BranchCookie;
                            if (bc != null && bc.TargetIndex != -1)
                            {
                                Debug.Assert(!bc.ContentOnStack);

                                // if the target is within the new block, we handle it, otherwise we
                                // defer the cookie to our caller
                                if (newBlock.IsInRange(bc.TargetIndex))
                                {
                                    bc.ContentOnStack = true;
                                    ilgen.MarkLabel(bc.Stub);
                                    int stack = bc.dh.Count;
                                    for (int n = stack - 1; n >= 0; n--)
                                        bc.dh.Load(n);

                                    ilgen.EmitBr(newBlock.GetLabel(bc.TargetIndex));
                                }
                                else
                                {
                                    newBlock.exits.Add(bc);
                                }
                            }
                        }
                    }
                }
            }

            internal void AddExitHack(object bc)
            {
                exits.Add(bc);
            }

            internal bool IsNested => nested;

        }

        void Compile(Block block, int startIndex)
        {
            var flags = ComputePartialReachability(startIndex, true);
            var exceptions = GetExceptionTableFor(flags);
            var exceptionIndex = 0;
            var code = m.Instructions;
            var blockStack = new Stack<Block>();
            var instructionIsForwardReachable = true;
            if (startIndex != 0)
            {
                for (int i = 0; i < flags.Length; i++)
                {
                    if ((flags[i] & InstructionFlags.Reachable) != 0)
                    {
                        if (i < startIndex)
                        {
                            instructionIsForwardReachable = false;
                            ilGenerator.EmitBr(block.GetLabel(startIndex));
                        }

                        break;
                    }
                }
            }

            for (int i = 0; i < code.Length; i++)
            {
                var instr = code[i];

                if (scopeBegin != null)
                {
                    for (int j = scopeClose[i]; j > 0; j--)
                        ilGenerator.EndScope();
                    for (int j = scopeBegin[i]; j > 0; j--)
                        ilGenerator.BeginScope();
                }

                // if we've left the current exception block, do the exit processing
                while (block.EndIndex == i)
                {
                    block.Leave();

                    var exc = exceptions[block.ExceptionIndex];

                    var prevBlock = block;
                    block = blockStack.Pop();

                    exceptionIndex = block.ExceptionIndex + 1;
                    // skip over exception handlers that are no longer relevant
                    for (; exceptionIndex < exceptions.Length && exceptions[exceptionIndex].EndIndex <= i; exceptionIndex++)
                    {
                    }

                    int handlerIndex = exc.HandlerIndex;

                    if (exc.CatchType.IsNil && RuntimeVerifierJavaType.IsFaultBlockException(ma.GetRawStackTypeWrapper(handlerIndex, 0)))
                    {
                        if (exc.IsFinally)
                            ilGenerator.BeginFinallyBlock();
                        else
                            ilGenerator.BeginFaultBlock();

                        var b = new Block(this, 0, block.EndIndex, -1, null, false);
                        Compile(b, handlerIndex);
                        b.Leave();
                        ilGenerator.EndExceptionBlock();
                    }
                    else
                    {
                        RuntimeJavaType exceptionTypeWrapper;
                        bool remap;
                        if (exc.CatchType.IsNil)
                        {
                            exceptionTypeWrapper = finish.Context.JavaBase.TypeOfjavaLangThrowable;
                            remap = true;
                        }
                        else
                        {
                            exceptionTypeWrapper = classFile.GetConstantPoolClassType(exc.CatchType);
                            remap = exceptionTypeWrapper.IsUnloadable || !exceptionTypeWrapper.IsSubTypeOf(finish.Context.JavaBase.TypeOfCliSystemException);
                        }

                        var excType = exceptionTypeWrapper.TypeAsExceptionType;
                        var mapSafe = !exceptionTypeWrapper.IsUnloadable && !exceptionTypeWrapper.IsMapUnsafeException && !exceptionTypeWrapper.IsRemapped;
                        if (mapSafe)
                            ilGenerator.BeginCatchBlock(excType);
                        else
                            ilGenerator.BeginCatchBlock(finish.Context.Types.Exception);

                        var bc = new BranchCookie(this, 1, exc.HandlerIndex);
                        prevBlock.AddExitHack(bc);
                        var handlerInstr = code[handlerIndex];

                        var unusedException = handlerInstr.NormalizedOpCode == NormalizedOpCode.Pop || (handlerInstr.NormalizedOpCode == NormalizedOpCode.Astore && localVars.GetLocalVar(handlerIndex) == null);

                        int mapFlags = unusedException ? 2 : 0;
                        if (mapSafe && unusedException)
                        {
                            // we don't need to do anything with the exception
                        }
                        else if (mapSafe)
                        {
                            ilGenerator.EmitLdc_I4(mapFlags | 1);
                            ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.MapException.MakeGenericMethod(excType));
                        }
                        else if (exceptionTypeWrapper == finish.Context.JavaBase.TypeOfjavaLangThrowable)
                        {
                            ilGenerator.EmitLdc_I4(mapFlags);
                            ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.MapException.MakeGenericMethod(finish.Context.Types.Exception));
                        }
                        else
                        {
                            ilGenerator.EmitLdc_I4(mapFlags | (remap ? 0 : 1));
                            if (exceptionTypeWrapper.IsUnloadable)
                            {
                                Profiler.Count("EmitDynamicExceptionHandler");
                                EmitDynamicClassLiteral(exceptionTypeWrapper);
                                ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.DynamicMapException);
                            }
                            else
                            {
                                ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.MapException.MakeGenericMethod(excType));
                            }
                            if (!unusedException)
                            {
                                ilGenerator.Emit(OpCodes.Dup);
                            }

                            var leave = ilGenerator.DefineLabel();
                            ilGenerator.EmitBrtrue(leave);
                            ilGenerator.Emit(OpCodes.Rethrow);
                            ilGenerator.MarkLabel(leave);
                        }

                        if (unusedException)
                        {
                            // we must still have an item on the stack, even though it isn't used!
                            bc.dh.SetType(0, finish.Context.VerifierJavaTypeFactory.Null);
                        }
                        else
                        {
                            bc.dh.SetType(0, exceptionTypeWrapper);
                            bc.dh.Store(0);
                        }

                        ilGenerator.EmitLeave(bc.Stub);
                        ilGenerator.EndExceptionBlock();
                    }

                    prevBlock.LeaveStubs(block);
                }

                // skip any unreachable instructions
                if ((flags[i] & InstructionFlags.Reachable) == 0)
                    continue;

                // if there was a forward branch to this instruction, it is forward reachable
                instructionIsForwardReachable |= block.HasLabel(i);

                if (block.HasLabel(i) || (flags[i] & InstructionFlags.BranchTarget) != 0)
                    block.MarkLabel(i);

                // if the instruction is only backward reachable, ECMA says it must have an empty stack,
                // so we move the stack to locals
                if (!instructionIsForwardReachable)
                {
                    int stackHeight = ma.GetStackHeight(i);
                    if (stackHeight != 0)
                    {
                        var bc = new BranchCookie(this, stackHeight, -1);
                        bc.ContentOnStack = true;
                        bc.TargetLabel = ilGenerator.DefineLabel();
                        ilGenerator.MarkLabel(bc.TargetLabel);

                        for (int j = 0; j < stackHeight; j++)
                            bc.dh.SetType(j, ma.GetRawStackTypeWrapper(i, j));

                        for (int j = stackHeight - 1; j >= 0; j--)
                            bc.dh.Load(j);

                        block.SetBackwardBranchLabel(i, bc);
                    }
                }

                // if we're entering an exception block, we need to setup the exception block and
                // transfer the stack into it
                // Note that an exception block that *starts* at an unreachable instruction,
                // is completely unreachable, because it is impossible to branch into an exception block.
                for (; exceptionIndex < exceptions.Length && exceptions[exceptionIndex].StartIndex == i; exceptionIndex++)
                {
                    int stackHeight = ma.GetStackHeight(i);
                    if (stackHeight != 0)
                    {
                        var dh = new DupHelper(this, stackHeight);
                        for (int k = 0; k < stackHeight; k++)
                        {
                            dh.SetType(k, ma.GetRawStackTypeWrapper(i, k));
                            dh.Store(k);
                        }

                        ilGenerator.BeginExceptionBlock();
                        for (int k = stackHeight - 1; k >= 0; k--)
                            dh.Load(k);

                        dh.Release();
                    }
                    else
                    {
                        ilGenerator.BeginExceptionBlock();
                    }

                    blockStack.Push(block);
                    block = new Block(this, exceptions[exceptionIndex].StartIndex, exceptions[exceptionIndex].EndIndex, exceptionIndex, new List<object>(), true);
                    block.MarkLabel(i);
                }

                if (emitLineNumbers)
                {
                    var table = m.LineNumberTable;
                    for (int j = 0; j < table.Count; j++)
                    {
                        if (table[j].StartPc == code[i].PC && table[j].LineNumber != 0)
                        {
                            ilGenerator.SetLineNumber(table[j].LineNumber);
                            break;
                        }
                    }
                }

                if (keepAlive)
                {
                    // JSR 133 specifies that a finalizer cannot run while the constructor is still in progress.
                    // This code attempts to implement that by adding calls to GC.KeepAlive(this) before return,
                    // backward branches and throw instructions. I don't think it is perfect, you may be able to
                    // fool it by calling a trivial method that loops forever which the CLR JIT will then inline
                    // and see that control flow doesn't continue and hence the lifetime of "this" will be
                    // shorter than the constructor.
                    switch (OpCodeMetaData.GetFlowKind(instr.NormalizedOpCode))
                    {
                        case OpCodeFlowControl.Return:
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Call, finish.Context.CompilerFactory.KeepAliveMethod);
                            break;
                        case OpCodeFlowControl.Branch:
                        case OpCodeFlowControl.ConditionalBranch:
                            if (instr.TargetIndex <= i)
                            {
                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Call, finish.Context.CompilerFactory.KeepAliveMethod);
                            }
                            break;
                        case OpCodeFlowControl.Throw:
                        case OpCodeFlowControl.Switch:
                            if (ma.GetLocalTypeWrapper(i, 0) != finish.Context.VerifierJavaTypeFactory.UninitializedThis)
                            {
                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Call, finish.Context.CompilerFactory.KeepAliveMethod);
                            }
                            break;
                    }
                }

                switch (instr.NormalizedOpCode)
                {
                    case NormalizedOpCode.GetStatic:
                        {
                            var cpi = classFile.GetFieldref(checked((ushort)instr.Arg1));
                            if (cpi.GetClassType() != clazz)
                                nonleaf = true; // we may trigger a static initializer, which is equivalent to a call

                            var field = cpi.GetField();
                            field.EmitGet(ilGenerator);
                            field.FieldTypeWrapper.EmitConvSignatureTypeToStackType(ilGenerator);
                            break;
                        }
                    case NormalizedOpCode.GetField:
                        {
                            var cpi = classFile.GetFieldref(checked((ushort)instr.Arg1));
                            var field = cpi.GetField();
                            if (ma.GetStackTypeWrapper(i, 0).IsUnloadable)
                            {
                                if (field.IsProtected)
                                {
                                    // downcast receiver to our type
                                    clazz.EmitCheckcast(ilGenerator);
                                }
                                else
                                {
                                    // downcast receiver to field declaring type
                                    field.DeclaringType.EmitCheckcast(ilGenerator);
                                }
                            }
                            field.EmitGet(ilGenerator);
                            field.FieldTypeWrapper.EmitConvSignatureTypeToStackType(ilGenerator);
                            break;
                        }
                    case NormalizedOpCode.PutStatic:
                        {
                            var cpi = classFile.GetFieldref(checked((ushort)instr.Arg1));
                            if (cpi.GetClassType() != clazz)
                                nonleaf = true; // we may trigger a static initializer, which is equivalent to a call
                            RuntimeJavaField field = cpi.GetField();
                            RuntimeJavaType tw = field.FieldTypeWrapper;
                            tw.EmitConvStackTypeToSignatureType(ilGenerator, ma.GetStackTypeWrapper(i, 0));
                            if (strictfp)
                            {
                                // no need to convert
                            }
                            else if (tw == finish.Context.PrimitiveJavaTypeFactory.DOUBLE)
                            {
                                ilGenerator.Emit(OpCodes.Conv_R8);
                            }
                            field.EmitSet(ilGenerator);
                            break;
                        }
                    case NormalizedOpCode.PutField:
                        {
                            var cpi = classFile.GetFieldref(checked((ushort)instr.Arg1));
                            var field = cpi.GetField();
                            var tw = field.FieldTypeWrapper;
                            if (ma.GetStackTypeWrapper(i, 1).IsUnloadable)
                            {
                                var temp = ilGenerator.UnsafeAllocTempLocal(tw.TypeAsLocalOrStackType);
                                ilGenerator.Emit(OpCodes.Stloc, temp);
                                if (field.IsProtected)
                                {
                                    // downcast receiver to our type
                                    clazz.EmitCheckcast(ilGenerator);
                                }
                                else
                                {
                                    // downcast receiver to field declaring type
                                    field.DeclaringType.EmitCheckcast(ilGenerator);
                                }

                                ilGenerator.Emit(OpCodes.Ldloc, temp);
                            }

                            tw.EmitConvStackTypeToSignatureType(ilGenerator, ma.GetStackTypeWrapper(i, 0));
                            if (strictfp)
                            {
                                // no need to convert
                            }
                            else if (tw == finish.Context.PrimitiveJavaTypeFactory.DOUBLE)
                            {
                                ilGenerator.Emit(OpCodes.Conv_R8);
                            }

                            field.EmitSet(ilGenerator);
                            break;
                        }
                    case NormalizedOpCode.DynamicGetStatic:
                    case NormalizedOpCode.DynamicPutStatic:
                    case NormalizedOpCode.DynamicGetField:
                    case NormalizedOpCode.DynamicPutField:
                        nonleaf = true;
                        DynamicGetPutField(instr, i);
                        break;
                    case NormalizedOpCode.AconstNull:
                        ilGenerator.Emit(OpCodes.Ldnull);
                        break;
                    case NormalizedOpCode.Iconst:
                        ilGenerator.EmitLdc_I4(instr.NormalizedArg1);
                        break;
                    case NormalizedOpCode.Lconst0:
                        ilGenerator.EmitLdc_I8(0L);
                        break;
                    case NormalizedOpCode.Lconst1:
                        ilGenerator.EmitLdc_I8(1L);
                        break;
                    case NormalizedOpCode.Fconst0:
                    case NormalizedOpCode.Dconst0:
                        // floats are stored as native size on the stack, so both R4 and R8 are the same
                        ilGenerator.EmitLdc_R4(0.0f);
                        break;
                    case NormalizedOpCode.Fconst1:
                    case NormalizedOpCode.Dconst1:
                        // floats are stored as native size on the stack, so both R4 and R8 are the same
                        ilGenerator.EmitLdc_R4(1.0f);
                        break;
                    case NormalizedOpCode.Fconst2:
                        ilGenerator.EmitLdc_R4(2.0f);
                        break;
                    case NormalizedOpCode.LdcNothrow:
                    case NormalizedOpCode.Ldc:
                        EmitLoadConstant(ilGenerator, new ConstantHandle(ConstantKind.Unknown, (ushort)instr.Arg1));
                        break;
                    case NormalizedOpCode.InvokeDynamic:
                        {
                            var cpi = classFile.GetInvokeDynamic(new((ushort)instr.Arg1));
                            CastInterfaceArgs(null, cpi.GetArgTypes(), i, false);
                            if (!LambdaMetafactory.Emit(finish, classFile, instr.Arg1, cpi, ilGenerator))
                            {
                                EmitInvokeDynamic(cpi);
                                EmitReturnTypeConversion(cpi.GetRetType());
                            }
                            nonleaf = true;
                            break;
                        }
                    case NormalizedOpCode.DynamicInvokeStatic:
                    case NormalizedOpCode.PrivilegedInvokeStatic:
                    case NormalizedOpCode.InvokeStatic:
                    case NormalizedOpCode.MethodHandleLink:
                        {
                            var method = GetMethodCallEmitter(instr.NormalizedOpCode, instr.Arg1);
                            if (method.IsIntrinsic && method.EmitIntrinsic(new EmitIntrinsicContext(method, finish, ilGenerator, ma, i, mw, classFile, code, flags)))
                                break;

                            // if the stack values don't match the argument types (for interface argument types)
                            // we must emit code to cast the stack value to the interface type
                            CastInterfaceArgs(method.DeclaringType, method.GetParameters(), i, false);
                            if (method.HasCallerID)
                                finish.EmitCallerID(ilGenerator, m.IsLambdaFormCompiled);

                            method.EmitCall(ilGenerator);
                            EmitReturnTypeConversion(method.ReturnType);
                            nonleaf = true;
                            break;
                        }
                    case NormalizedOpCode.DynamicInvokeInterface:
                    case NormalizedOpCode.DynamicInvokeVirtual:
                    case NormalizedOpCode.DynamicInvokeSpecial:
                    case NormalizedOpCode.PrivilegedInvokeVirtual:
                    case NormalizedOpCode.PrivilegedInvokeSpecial:
                    case NormalizedOpCode.InvokeVirtual:
                    case NormalizedOpCode.InvokeInterface:
                    case NormalizedOpCode.InvokeSpecial:
                    case NormalizedOpCode.MethodHandleInvoke:
                        {
                            var isinvokespecial = instr.NormalizedOpCode == NormalizedOpCode.InvokeSpecial
                                || instr.NormalizedOpCode == NormalizedOpCode.DynamicInvokeSpecial
                                || instr.NormalizedOpCode == NormalizedOpCode.PrivilegedInvokeSpecial;
                            var method = GetMethodCallEmitter(instr.NormalizedOpCode, instr.Arg1);
                            var argcount = method.GetParameters().Length;
                            var type = ma.GetRawStackTypeWrapper(i, argcount);
                            var thisType = ComputeThisType(type, method, instr.NormalizedOpCode);

                            var eic = new EmitIntrinsicContext(method, finish, ilGenerator, ma, i, mw, classFile, code, flags);
                            if (method.IsIntrinsic && method.EmitIntrinsic(eic))
                            {
                                nonleaf |= eic.NonLeaf;
                                break;
                            }

                            nonleaf = true;

                            // HACK this code is duplicated in java.lang.invoke.cs
                            if (method.IsFinalizeOrClone)
                            {
                                // HACK we may need to redirect finalize or clone from java.lang.Object/Throwable
                                // to a more specific base type.
                                if (thisType.IsAssignableTo(finish.Context.JavaBase.TypeOfCliSystemObject))
                                {
                                    method = finish.Context.JavaBase.TypeOfCliSystemObject.GetMethod(method.Name, method.Signature, true);
                                }
                                else if (thisType.IsAssignableTo(finish.Context.JavaBase.TypeOfCliSystemException))
                                {
                                    method = finish.Context.JavaBase.TypeOfCliSystemException.GetMethod(method.Name, method.Signature, true);
                                }
                                else if (thisType.IsAssignableTo(finish.Context.JavaBase.TypeOfjavaLangThrowable))
                                {
                                    method = finish.Context.JavaBase.TypeOfjavaLangThrowable.GetMethod(method.Name, method.Signature, true);
                                }
                            }

                            // if the stack values don't match the argument types (for interface argument types)
                            // we must emit code to cast the stack value to the interface type
                            if (isinvokespecial && method.IsConstructor && RuntimeVerifierJavaType.IsNew(type))
                            {
                                CastInterfaceArgs(method.DeclaringType, method.GetParameters(), i, false);
                            }
                            else
                            {
                                // the this reference is included in the argument list because it may also need to be cast
                                var methodArgs = method.GetParameters();
                                var args = new RuntimeJavaType[methodArgs.Length + 1];
                                methodArgs.CopyTo(args, 1);
                                if (instr.NormalizedOpCode == NormalizedOpCode.InvokeInterface)
                                    args[0] = method.DeclaringType;
                                else
                                    args[0] = thisType;

                                CastInterfaceArgs(method.DeclaringType, args, i, true);
                            }

                            if (isinvokespecial && method.IsConstructor)
                            {
                                if (RuntimeVerifierJavaType.IsNew(type))
                                {
                                    // we have to construct a list of all the unitialized references to the object
                                    // we're about to create on the stack, so that we can reconstruct the stack after
                                    // the "newobj" instruction
                                    int trivcount = 0;
                                    var nontrivial = false;
                                    var stackfix = new bool[ma.GetStackHeight(i) - (argcount + 1)];
                                    for (int j = 0; j < stackfix.Length; j++)
                                    {
                                        if (ma.GetRawStackTypeWrapper(i, argcount + 1 + j) == type)
                                        {
                                            stackfix[j] = true;
                                            if (trivcount == j)
                                            {
                                                trivcount++;
                                            }
                                            else
                                            {
                                                // if there is other stuff on the stack between the new object
                                                // references, we need to do more work to construct the proper stack
                                                // layout after the newobj instruction
                                                nontrivial = true;
                                            }
                                        }
                                    }

                                    for (int j = 0; !nontrivial && j < m.MaxLocals; j++)
                                        if (ma.GetLocalTypeWrapper(i, j) == type)
                                            nontrivial = true;

                                    if (!thisType.IsUnloadable && thisType.IsSubTypeOf(finish.Context.JavaBase.TypeOfjavaLangThrowable))
                                    {
                                        // if the next instruction is an athrow and the exception type
                                        // doesn't override fillInStackTrace, we can suppress the call
                                        // to fillInStackTrace from the constructor (and this is
                                        // a huge perf win)
                                        // NOTE we also can't call suppressFillInStackTrace for non-Java
                                        // exceptions (because then the suppress flag won't be cleared),
                                        // but this case is handled by the "is fillInStackTrace overridden?"
                                        // test, because cli.System.Exception overrides fillInStackTrace.
                                        if (code[i + 1].NormalizedOpCode == NormalizedOpCode.Athrow)
                                        {
                                            if (thisType.GetMethod("fillInStackTrace", "()Ljava.lang.Throwable;", true).DeclaringType == finish.Context.JavaBase.TypeOfjavaLangThrowable)
                                                ilGenerator.Emit(OpCodes.Call, finish.Context.CompilerFactory.SuppressFillInStackTraceMethod);
                                            if ((flags[i + 1] & InstructionFlags.BranchTarget) == 0)
                                                code[i + 1].PatchOpCode(NormalizedOpCode.AthrowNoUnmap);
                                        }
                                    }

                                    method.EmitNewobj(ilGenerator);
                                    if (!thisType.IsUnloadable && thisType.IsSubTypeOf(finish.Context.JavaBase.TypeOfCliSystemException))
                                    {
                                        // we call Throwable.__<fixate>() to disable remapping the exception
                                        ilGenerator.Emit(OpCodes.Call, finish.Context.CompilerFactory.FixateExceptionMethod);
                                    }

                                    if (nontrivial)
                                    {
                                        // this could be done a little more efficiently, but since in practice this
                                        // code never runs (for code compiled from Java source) it doesn't
                                        // really matter
                                        var newobj = ilGenerator.DeclareLocal(GetLocalBuilderType(thisType));
                                        ilGenerator.Emit(OpCodes.Stloc, newobj);
                                        var tempstack = new CodeEmitterLocal[stackfix.Length];
                                        for (int j = 0; j < stackfix.Length; j++)
                                        {
                                            if (!stackfix[j])
                                            {
                                                var stacktype = ma.GetStackTypeWrapper(i, argcount + 1 + j);
                                                // it could be another new object reference (not from current invokespecial <init>
                                                // instruction)
                                                if (stacktype == finish.Context.VerifierJavaTypeFactory.Null)
                                                {
                                                    // NOTE we abuse the newobj local as a cookie to signal null!
                                                    tempstack[j] = newobj;
                                                    ilGenerator.Emit(OpCodes.Pop);
                                                }
                                                else if (!RuntimeVerifierJavaType.IsNotPresentOnStack(stacktype))
                                                {
                                                    var lb = ilGenerator.DeclareLocal(GetLocalBuilderType(stacktype));
                                                    ilGenerator.Emit(OpCodes.Stloc, lb);
                                                    tempstack[j] = lb;
                                                }
                                            }
                                        }

                                        for (int j = stackfix.Length - 1; j >= 0; j--)
                                        {
                                            if (stackfix[j])
                                            {
                                                ilGenerator.Emit(OpCodes.Ldloc, newobj);
                                            }
                                            else if (tempstack[j] != null)
                                            {
                                                // NOTE we abuse the newobj local as a cookie to signal null!
                                                if (tempstack[j] == newobj)
                                                    ilGenerator.Emit(OpCodes.Ldnull);
                                                else
                                                    ilGenerator.Emit(OpCodes.Ldloc, tempstack[j]);
                                            }
                                        }

                                        var locals = localVars.GetLocalVarsForInvokeSpecial(i);
                                        for (int j = 0; j < locals.Length; j++)
                                        {
                                            if (locals[j] != null)
                                            {
                                                if (locals[j].builder == null)
                                                {
                                                    // for invokespecial the resulting type can never be null
                                                    locals[j].builder = ilGenerator.DeclareLocal(GetLocalBuilderType(locals[j].type));
                                                }
                                                ilGenerator.Emit(OpCodes.Ldloc, newobj);
                                                ilGenerator.Emit(OpCodes.Stloc, locals[j].builder);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (trivcount == 0)
                                        {
                                            ilGenerator.Emit(OpCodes.Pop);
                                        }
                                        else
                                        {
                                            for (int j = 1; j < trivcount; j++)
                                            {
                                                ilGenerator.Emit(OpCodes.Dup);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Debug.Assert(type == finish.Context.VerifierJavaTypeFactory.UninitializedThis);

                                    method.EmitCall(ilGenerator);
                                    var locals = localVars.GetLocalVarsForInvokeSpecial(i);
                                    for (int j = 0; j < locals.Length; j++)
                                    {
                                        if (locals[j] != null)
                                        {
                                            // for invokespecial the resulting type can never be null
                                            locals[j].builder ??= ilGenerator.DeclareLocal(GetLocalBuilderType(locals[j].type));
                                            ilGenerator.Emit(OpCodes.Ldarg_0);
                                            ilGenerator.Emit(OpCodes.Stloc, locals[j].builder);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (method.HasCallerID)
                                    finish.EmitCallerID(ilGenerator, m.IsLambdaFormCompiled);

                                if (isinvokespecial)
                                {
                                    if (RuntimeVerifierJavaType.IsThis(type))
                                    {
                                        method.EmitCall(ilGenerator);
                                    }
                                    else if (method.IsPrivate)
                                    {
                                        // if the method is private, we can get away with a callvirt (and not generate the stub)
                                        method.EmitCallvirt(ilGenerator);
                                    }
                                    else if (instr.NormalizedOpCode == NormalizedOpCode.PrivilegedInvokeSpecial)
                                    {
                                        method.EmitCall(ilGenerator);
                                    }
                                    else
                                    {
                                        ilGenerator.Emit(OpCodes.Callvirt, finish.GetInvokeSpecialStub(method));
                                    }
                                }
                                else
                                {
                                    // NOTE this check is written somewhat pessimistically, because we need to
                                    // take remapped types into account. For example, Throwable.getClass() "overrides"
                                    // the final Object.getClass() method and we don't want to call Object.getClass()
                                    // on a Throwable instance, because that would yield unverifiable code (java.lang.Throwable
                                    // extends System.Exception instead of java.lang.Object in the .NET type system).
                                    if (RuntimeVerifierJavaType.IsThis(type) && (method.IsFinal || clazz.IsFinal) && clazz.GetMethod(method.Name, method.Signature, true) == method)
                                    {
                                        // we're calling a method on our own instance that can't possibly be overriden,
                                        // so we don't need to use callvirt
                                        method.EmitCall(ilGenerator);
                                    }
                                    else
                                    {
                                        method.EmitCallvirt(ilGenerator);
                                    }
                                }

                                EmitReturnTypeConversion(method.ReturnType);
                            }
                            break;
                        }
                    case NormalizedOpCode.CloneArray:
                        ilGenerator.Emit(OpCodes.Callvirt, RuntimeArrayJavaType.GetCloneMethod(finish.Context));
                        break;
                    case NormalizedOpCode.Return:
                    case NormalizedOpCode.Areturn:
                    case NormalizedOpCode.Ireturn:
                    case NormalizedOpCode.Lreturn:
                    case NormalizedOpCode.Freturn:
                    case NormalizedOpCode.Dreturn:
                        {
                            if (block.IsNested)
                            {
                                // if we're inside an exception block, copy TOS to local, emit "leave" and push item onto our "todo" list
                                CodeEmitterLocal local = null;
                                if (instr.NormalizedOpCode != NormalizedOpCode.Return)
                                {
                                    var retTypeWrapper = mw.ReturnType;
                                    retTypeWrapper.EmitConvStackTypeToSignatureType(ilGenerator, ma.GetStackTypeWrapper(i, 0));
                                    local = ilGenerator.UnsafeAllocTempLocal(retTypeWrapper.TypeAsSignatureType);
                                    ilGenerator.Emit(OpCodes.Stloc, local);
                                }

                                var label = ilGenerator.DefineLabel();
                                ilGenerator.EmitLeave(label);
                                block.AddExitHack(new ReturnCookie(label, local));
                            }
                            else
                            {
                                // HACK the x64 JIT is lame and optimizes calls before ret into a tail call
                                // and this makes the method disappear from the call stack, so we try to thwart that
                                // by inserting some bogus instructions between the call and the return.
                                // Note that this optimization doesn't appear to happen if the method has exception handlers,
                                // so in that case we don't do anything.
                                var x64hack = false;
                                if (exceptions.Length == 0 && i > 0)
                                {
                                    int k = i - 1;
                                    while (k > 0 && (code[k].NormalizedOpCode == NormalizedOpCode.Nop || code[k].NormalizedOpCode == NormalizedOpCode.Pop))
                                        k--;

                                    switch (code[k].NormalizedOpCode)
                                    {
                                        case NormalizedOpCode.InvokeInterface:
                                        case NormalizedOpCode.InvokeSpecial:
                                        case NormalizedOpCode.InvokeStatic:
                                        case NormalizedOpCode.InvokeVirtual:
                                            x64hack = true;
                                            break;
                                    }
                                }

                                // if there is junk on the stack (other than the return value), we must pop it off
                                // because in .NET this is invalid (unlike in Java)
                                var stackHeight = ma.GetStackHeight(i);
                                if (instr.NormalizedOpCode == NormalizedOpCode.Return)
                                {
                                    if (stackHeight != 0 || x64hack)
                                        ilGenerator.EmitClearStack();

                                    ilGenerator.Emit(OpCodes.Ret);
                                }
                                else
                                {
                                    var retTypeWrapper = mw.ReturnType;
                                    retTypeWrapper.EmitConvStackTypeToSignatureType(ilGenerator, ma.GetStackTypeWrapper(i, 0));
                                    if (stackHeight != 1)
                                    {
                                        var local = ilGenerator.AllocTempLocal(retTypeWrapper.TypeAsSignatureType);
                                        ilGenerator.Emit(OpCodes.Stloc, local);
                                        ilGenerator.EmitClearStack();
                                        ilGenerator.Emit(OpCodes.Ldloc, local);
                                        ilGenerator.ReleaseTempLocal(local);
                                    }
                                    else if (x64hack)
                                    {
                                        ilGenerator.EmitTailCallPrevention();
                                    }

                                    ilGenerator.Emit(OpCodes.Ret);
                                }
                            }
                            break;
                        }
                    case NormalizedOpCode.Aload:
                        {
                            var type = ma.GetLocalTypeWrapper(i, instr.NormalizedArg1);
                            if (type == finish.Context.VerifierJavaTypeFactory.Null)
                            {
                                // if the local is known to be null, we just emit a null
                                ilGenerator.Emit(OpCodes.Ldnull);
                            }
                            else if (RuntimeVerifierJavaType.IsNotPresentOnStack(type))
                            {
                                // since object isn't represented on the stack, we don't need to do anything here
                            }
                            else if (RuntimeVerifierJavaType.IsThis(type))
                            {
                                ilGenerator.Emit(OpCodes.Ldarg_0);
                            }
                            else if (type == finish.Context.VerifierJavaTypeFactory.UninitializedThis)
                            {
                                // any unitialized this reference has to be loaded from arg 0
                                // NOTE if the method overwrites the this references, it will always end up in
                                // a different local (due to the way the local variable liveness analysis works),
                                // so we don't have to worry about that.
                                ilGenerator.Emit(OpCodes.Ldarg_0);
                            }
                            else
                            {
                                var v = LoadLocal(i);
                                if (!type.IsUnloadable && (v.type.IsUnloadable || !v.type.IsAssignableTo(type)))
                                    type.EmitCheckcast(ilGenerator);
                            }

                            break;
                        }
                    case NormalizedOpCode.Astore:
                        {
                            var type = ma.GetRawStackTypeWrapper(i, 0);
                            if (RuntimeVerifierJavaType.IsNotPresentOnStack(type))
                            {
                                // object isn't really on the stack, so we can't copy it into the local
                                // (and the local doesn't exist anyway)
                            }
                            else if (type == finish.Context.VerifierJavaTypeFactory.UninitializedThis)
                            {
                                // any unitialized reference is always the this reference, we don't store anything
                                // here (because CLR won't allow unitialized references in locals) and then when
                                // the unitialized ref is loaded we redirect to the this reference
                                ilGenerator.Emit(OpCodes.Pop);
                            }
                            else
                            {
                                StoreLocal(i);
                            }
                            break;
                        }
                    case NormalizedOpCode.Iload:
                    case NormalizedOpCode.Lload:
                    case NormalizedOpCode.Fload:
                    case NormalizedOpCode.Dload:
                        LoadLocal(i);
                        break;
                    case NormalizedOpCode.Istore:
                    case NormalizedOpCode.Lstore:
                        StoreLocal(i);
                        break;
                    case NormalizedOpCode.Fstore:
                        StoreLocal(i);
                        break;
                    case NormalizedOpCode.Dstore:
                        if (ma.IsStackTypeExtendedDouble(i, 0))
                        {
                            ilGenerator.Emit(OpCodes.Conv_R8);
                        }
                        StoreLocal(i);
                        break;
                    case NormalizedOpCode.New:
                        {
                            var wrapper = classFile.GetConstantPoolClassType(instr.Arg1);
                            if (wrapper.IsUnloadable)
                            {
                                Profiler.Count("EmitDynamicNewCheckOnly");
                                // this is here to make sure we throw the exception in the right location (before
                                // evaluating the constructor arguments)
                                EmitDynamicClassLiteral(wrapper);
                                ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.DynamicNewCheckOnly);
                            }
                            else if (wrapper != clazz && RequiresExplicitClassInit(wrapper, i + 1, flags))
                            {
                                // trigger cctor (as the spec requires)
                                wrapper.EmitRunClassConstructor(ilGenerator);
                            }

                            // we don't actually allocate the object here, the call to <init> will be converted into a newobj instruction
                            break;
                        }
                    case NormalizedOpCode.Multianewarray:
                        {
                            var localArray = ilGenerator.UnsafeAllocTempLocal(finish.Context.Resolver.ResolveCoreType(typeof(int).FullName).MakeArrayType().AsReflection());
                            var localInt = ilGenerator.UnsafeAllocTempLocal(finish.Context.Types.Int32);
                            ilGenerator.EmitLdc_I4(instr.Arg2);
                            ilGenerator.Emit(OpCodes.Newarr, finish.Context.Types.Int32);
                            ilGenerator.Emit(OpCodes.Stloc, localArray);
                            for (int j = 1; j <= instr.Arg2; j++)
                            {
                                ilGenerator.Emit(OpCodes.Stloc, localInt);
                                ilGenerator.Emit(OpCodes.Ldloc, localArray);
                                ilGenerator.EmitLdc_I4(instr.Arg2 - j);
                                ilGenerator.Emit(OpCodes.Ldloc, localInt);
                                ilGenerator.Emit(OpCodes.Stelem_I4);
                            }
                            var wrapper = classFile.GetConstantPoolClassType(instr.Arg1);
                            if (wrapper.IsUnloadable)
                            {
                                Profiler.Count("EmitDynamicMultianewarray");
                                ilGenerator.Emit(OpCodes.Ldloc, localArray);
                                EmitDynamicClassLiteral(wrapper);
                                ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.DynamicMultianewarray);
                            }
                            else if (wrapper.IsGhost || wrapper.IsGhostArray)
                            {
                                var tw = wrapper;
                                while (tw.IsArray)
                                    tw = tw.ElementTypeWrapper;

                                ilGenerator.Emit(OpCodes.Ldtoken, RuntimeArrayJavaType.MakeArrayType(tw.TypeAsTBD, wrapper.ArrayRank));
                                ilGenerator.Emit(OpCodes.Ldloc, localArray);
                                ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.multianewarray_ghost);
                                ilGenerator.Emit(OpCodes.Castclass, wrapper.TypeAsArrayType);
                            }
                            else
                            {
                                Type type = wrapper.TypeAsArrayType;
                                ilGenerator.Emit(OpCodes.Ldtoken, type);
                                ilGenerator.Emit(OpCodes.Ldloc, localArray);
                                ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.multianewarray);
                                ilGenerator.Emit(OpCodes.Castclass, type);
                            }
                            break;
                        }
                    case NormalizedOpCode.Anewarray:
                        {
                            var wrapper = classFile.GetConstantPoolClassType(instr.Arg1);
                            if (wrapper.IsUnloadable)
                            {
                                Profiler.Count("EmitDynamicNewarray");
                                EmitDynamicClassLiteral(wrapper);
                                ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.DynamicNewarray);
                            }
                            else if (wrapper.IsGhost || wrapper.IsGhostArray)
                            {
                                // NOTE for ghost types we create object arrays to make sure that Ghost implementers can be
                                // stored in ghost arrays, but this has the unintended consequence that ghost arrays can
                                // contain *any* reference type (because they are compiled as Object arrays). We could
                                // modify aastore to emit code to check for this, but this would have an huge performance
                                // cost for all object arrays.
                                // Oddly, while the JVM accepts any reference for any other interface typed references, in the
                                // case of aastore it does check that the object actually implements the interface. This
                                // is unfortunate, but I think we can live with this minor incompatibility.
                                // Note that this does not break type safety, because when the incorrect object is eventually
                                // used as the ghost interface type it will generate a ClassCastException.
                                var tw = wrapper;
                                while (tw.IsArray)
                                    tw = tw.ElementTypeWrapper;

                                ilGenerator.Emit(OpCodes.Ldtoken, RuntimeArrayJavaType.MakeArrayType(tw.TypeAsTBD, wrapper.ArrayRank + 1));
                                ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.anewarray_ghost.MakeGenericMethod(wrapper.TypeAsArrayType));
                            }
                            else
                            {
                                ilGenerator.Emit(OpCodes.Newarr, wrapper.TypeAsArrayType);
                            }

                            break;
                        }
                    case NormalizedOpCode.Newarray:
                        switch (instr.Arg1)
                        {
                            case 4:
                                ilGenerator.Emit(OpCodes.Newarr, finish.Context.PrimitiveJavaTypeFactory.BOOLEAN.TypeAsArrayType);
                                break;
                            case 5:
                                ilGenerator.Emit(OpCodes.Newarr, finish.Context.PrimitiveJavaTypeFactory.CHAR.TypeAsArrayType);
                                break;
                            case 6:
                                ilGenerator.Emit(OpCodes.Newarr, finish.Context.PrimitiveJavaTypeFactory.FLOAT.TypeAsArrayType);
                                break;
                            case 7:
                                ilGenerator.Emit(OpCodes.Newarr, finish.Context.PrimitiveJavaTypeFactory.DOUBLE.TypeAsArrayType);
                                break;
                            case 8:
                                ilGenerator.Emit(OpCodes.Newarr, finish.Context.PrimitiveJavaTypeFactory.BYTE.TypeAsArrayType);
                                break;
                            case 9:
                                ilGenerator.Emit(OpCodes.Newarr, finish.Context.PrimitiveJavaTypeFactory.SHORT.TypeAsArrayType);
                                break;
                            case 10:
                                ilGenerator.Emit(OpCodes.Newarr, finish.Context.PrimitiveJavaTypeFactory.INT.TypeAsArrayType);
                                break;
                            case 11:
                                ilGenerator.Emit(OpCodes.Newarr, finish.Context.PrimitiveJavaTypeFactory.LONG.TypeAsArrayType);
                                break;
                            default:
                                // this can't happen, the verifier would have caught it
                                throw new InvalidOperationException();
                        }
                        break;
                    case NormalizedOpCode.Checkcast:
                        {
                            var wrapper = classFile.GetConstantPoolClassType(instr.Arg1);
                            if (wrapper.IsUnloadable)
                                EmitDynamicCast(wrapper);
                            else
                                wrapper.EmitCheckcast(ilGenerator);

                            break;
                        }
                    case NormalizedOpCode.InstanceOf:
                        {
                            var wrapper = classFile.GetConstantPoolClassType(instr.Arg1);
                            if (wrapper.IsUnloadable)
                                EmitDynamicInstanceOf(wrapper);
                            else
                                wrapper.EmitInstanceOf(ilGenerator);

                            break;
                        }
                    case NormalizedOpCode.Aaload:
                        {
                            var tw = ma.GetRawStackTypeWrapper(i, 1);
                            if (tw.IsUnloadable)
                            {
                                Profiler.Count("EmitDynamicAaload");
                                ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.DynamicAaload);
                            }
                            else
                            {
                                var elem = tw.ElementTypeWrapper;
                                if (elem.IsNonPrimitiveValueType)
                                {
                                    var t = elem.TypeAsTBD;
                                    ilGenerator.Emit(OpCodes.Ldelema, t);
                                    ilGenerator.Emit(OpCodes.Ldobj, t);
                                    elem.EmitBox(ilGenerator);
                                }
                                else
                                {
                                    ilGenerator.Emit(OpCodes.Ldelem_Ref);
                                }
                            }

                            break;
                        }
                    case NormalizedOpCode.Baload:
                        // NOTE both the JVM and the CLR use signed bytes for boolean arrays (how convenient!)
                        ilGenerator.Emit(OpCodes.Ldelem_I1);
                        break;
                    case NormalizedOpCode.Bastore:
                        ilGenerator.Emit(OpCodes.Stelem_I1);
                        break;
                    case NormalizedOpCode.Caload:
                        ilGenerator.Emit(OpCodes.Ldelem_U2);
                        break;
                    case NormalizedOpCode.Castore:
                        ilGenerator.Emit(OpCodes.Stelem_I2);
                        break;
                    case NormalizedOpCode.Saload:
                        ilGenerator.Emit(OpCodes.Ldelem_I2);
                        break;
                    case NormalizedOpCode.Sastore:
                        ilGenerator.Emit(OpCodes.Stelem_I2);
                        break;
                    case NormalizedOpCode.Iaload:
                        ilGenerator.Emit(OpCodes.Ldelem_I4);
                        break;
                    case NormalizedOpCode.Iastore:
                        ilGenerator.Emit(OpCodes.Stelem_I4);
                        break;
                    case NormalizedOpCode.Laload:
                        ilGenerator.Emit(OpCodes.Ldelem_I8);
                        break;
                    case NormalizedOpCode.Lastore:
                        ilGenerator.Emit(OpCodes.Stelem_I8);
                        break;
                    case NormalizedOpCode.Faload:
                        ilGenerator.Emit(OpCodes.Ldelem_R4);
                        break;
                    case NormalizedOpCode.Fastore:
                        ilGenerator.Emit(OpCodes.Stelem_R4);
                        break;
                    case NormalizedOpCode.Daload:
                        ilGenerator.Emit(OpCodes.Ldelem_R8);
                        break;
                    case NormalizedOpCode.Dastore:
                        if (ma.IsStackTypeExtendedDouble(i, 0))
                            ilGenerator.Emit(OpCodes.Conv_R8);
                        ilGenerator.Emit(OpCodes.Stelem_R8);
                        break;
                    case NormalizedOpCode.Aastore:
                        {
                            var tw = ma.GetRawStackTypeWrapper(i, 2);
                            if (tw.IsUnloadable)
                            {
                                Profiler.Count("EmitDynamicAastore");
                                ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.DynamicAastore);
                            }
                            else
                            {
                                var elem = tw.ElementTypeWrapper;
                                if (elem.IsNonPrimitiveValueType)
                                {
                                    var t = elem.TypeAsTBD;
                                    var local = ilGenerator.UnsafeAllocTempLocal(finish.Context.Types.Object);
                                    ilGenerator.Emit(OpCodes.Stloc, local);
                                    ilGenerator.Emit(OpCodes.Ldelema, t);
                                    ilGenerator.Emit(OpCodes.Ldloc, local);
                                    elem.EmitUnbox(ilGenerator);
                                    ilGenerator.Emit(OpCodes.Stobj, t);
                                }
                                else
                                {
                                    // NOTE for verifiability it is expressly *not* required that the
                                    // value matches the array type, so we don't need to handle interface
                                    // references here.
                                    ilGenerator.Emit(OpCodes.Stelem_Ref);
                                }
                            }

                            break;
                        }
                    case NormalizedOpCode.Arraylength:
                        if (ma.GetRawStackTypeWrapper(i, 0).IsUnloadable)
                        {
                            ilGenerator.Emit(OpCodes.Castclass, finish.Context.Types.Array);
                            ilGenerator.Emit(OpCodes.Callvirt, finish.Context.Types.Array.GetMethod("get_Length"));
                        }
                        else
                        {
                            ilGenerator.Emit(OpCodes.Ldlen);
                        }
                        break;
                    case NormalizedOpCode.Lcmp:
                        ilGenerator.Emit_lcmp();
                        break;
                    case NormalizedOpCode.Fcmpl:
                        ilGenerator.Emit_fcmpl();
                        break;
                    case NormalizedOpCode.Fcmpg:
                        ilGenerator.Emit_fcmpg();
                        break;
                    case NormalizedOpCode.Dcmpl:
                        ilGenerator.Emit_dcmpl();
                        break;
                    case NormalizedOpCode.Dcmpg:
                        ilGenerator.Emit_dcmpg();
                        break;
                    case NormalizedOpCode.IfIcmpeq:
                        ilGenerator.EmitBeq(block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.IfIcmpne:
                        ilGenerator.EmitBne_Un(block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.IfIcmple:
                        ilGenerator.EmitBle(block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.IfIcmplt:
                        ilGenerator.EmitBlt(block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.IfIcmpge:
                        ilGenerator.EmitBge(block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.IfIcmpgt:
                        ilGenerator.EmitBgt(block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.Ifle:
                        ilGenerator.Emit_if_le_lt_ge_gt(CodeEmitter.Comparison.LessOrEqual, block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.Iflt:
                        ilGenerator.Emit_if_le_lt_ge_gt(CodeEmitter.Comparison.LessThan, block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.Ifge:
                        ilGenerator.Emit_if_le_lt_ge_gt(CodeEmitter.Comparison.GreaterOrEqual, block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.Ifgt:
                        ilGenerator.Emit_if_le_lt_ge_gt(CodeEmitter.Comparison.GreaterThan, block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.Ifne:
                    case NormalizedOpCode.IfNonNull:
                        ilGenerator.EmitBrtrue(block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.Ifeq:
                    case NormalizedOpCode.IfNull:
                        ilGenerator.EmitBrfalse(block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.IfAcmpeq:
                        ilGenerator.EmitBeq(block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.IfAcmpne:
                        ilGenerator.EmitBne_Un(block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.Goto:
                    case NormalizedOpCode.GotoFinally:
                        ilGenerator.EmitBr(block.GetLabel(instr.TargetIndex));
                        break;
                    case NormalizedOpCode.Ineg:
                    case NormalizedOpCode.Lneg:
                    case NormalizedOpCode.Fneg:
                    case NormalizedOpCode.Dneg:
                        ilGenerator.Emit(OpCodes.Neg);
                        break;
                    case NormalizedOpCode.Iadd:
                    case NormalizedOpCode.Ladd:
                        ilGenerator.Emit(OpCodes.Add);
                        break;
                    case NormalizedOpCode.Fadd:
                        ilGenerator.Emit(OpCodes.Add);
                        ilGenerator.Emit(OpCodes.Conv_R4);
                        break;
                    case NormalizedOpCode.Dadd:
                        ilGenerator.Emit(OpCodes.Add);
                        if (strictfp)
                        {
                            ilGenerator.Emit(OpCodes.Conv_R8);
                        }
                        break;
                    case NormalizedOpCode.Isub:
                    case NormalizedOpCode.Lsub:
                        ilGenerator.Emit(OpCodes.Sub);
                        break;
                    case NormalizedOpCode.Fsub:
                        ilGenerator.Emit(OpCodes.Sub);
                        ilGenerator.Emit(OpCodes.Conv_R4);
                        break;
                    case NormalizedOpCode.Dsub:
                        ilGenerator.Emit(OpCodes.Sub);
                        if (strictfp)
                        {
                            ilGenerator.Emit(OpCodes.Conv_R8);
                        }
                        break;
                    case NormalizedOpCode.Ixor:
                    case NormalizedOpCode.Lxor:
                        ilGenerator.Emit(OpCodes.Xor);
                        break;
                    case NormalizedOpCode.Ior:
                    case NormalizedOpCode.Lor:
                        ilGenerator.Emit(OpCodes.Or);
                        break;
                    case NormalizedOpCode.Iand:
                    case NormalizedOpCode.Land:
                        ilGenerator.Emit(OpCodes.And);
                        break;
                    case NormalizedOpCode.Imul:
                    case NormalizedOpCode.Lmul:
                        ilGenerator.Emit(OpCodes.Mul);
                        break;
                    case NormalizedOpCode.Fmul:
                        ilGenerator.Emit(OpCodes.Mul);
                        ilGenerator.Emit(OpCodes.Conv_R4);
                        break;
                    case NormalizedOpCode.Dmul:
                        ilGenerator.Emit(OpCodes.Mul);
                        if (strictfp)
                        {
                            ilGenerator.Emit(OpCodes.Conv_R8);
                        }
                        break;
                    case NormalizedOpCode.Idiv:
                        ilGenerator.Emit_idiv();
                        break;
                    case NormalizedOpCode.Ldiv:
                        ilGenerator.Emit_ldiv();
                        break;
                    case NormalizedOpCode.Fdiv:
                        ilGenerator.Emit(OpCodes.Div);
                        ilGenerator.Emit(OpCodes.Conv_R4);
                        break;
                    case NormalizedOpCode.Ddiv:
                        ilGenerator.Emit(OpCodes.Div);
                        if (strictfp)
                        {
                            ilGenerator.Emit(OpCodes.Conv_R8);
                        }
                        break;
                    case NormalizedOpCode.Irem:
                    case NormalizedOpCode.Lrem:
                        {
                            // we need to special case taking the remainder of dividing by -1,
                            // because the CLR rem instruction throws an OverflowException when
                            // taking the remainder of dividing Int32.MinValue by -1, and
                            // Java just silently overflows
                            ilGenerator.Emit(OpCodes.Dup);
                            ilGenerator.Emit(OpCodes.Ldc_I4_M1);
                            if (instr.NormalizedOpCode == NormalizedOpCode.Lrem)
                                ilGenerator.Emit(OpCodes.Conv_I8);
                            var label = ilGenerator.DefineLabel();
                            ilGenerator.EmitBne_Un(label);
                            ilGenerator.Emit(OpCodes.Pop);
                            ilGenerator.Emit(OpCodes.Pop);
                            ilGenerator.Emit(OpCodes.Ldc_I4_0);
                            if (instr.NormalizedOpCode == NormalizedOpCode.Lrem)
                            {
                                ilGenerator.Emit(OpCodes.Conv_I8);
                            }
                            var label2 = ilGenerator.DefineLabel();
                            ilGenerator.EmitBr(label2);
                            ilGenerator.MarkLabel(label);
                            ilGenerator.Emit(OpCodes.Rem);
                            ilGenerator.MarkLabel(label2);
                            break;
                        }
                    case NormalizedOpCode.Frem:
                        ilGenerator.Emit(OpCodes.Rem);
                        ilGenerator.Emit(OpCodes.Conv_R4);
                        break;
                    case NormalizedOpCode.Drem:
                        ilGenerator.Emit(OpCodes.Rem);
                        if (strictfp)
                            ilGenerator.Emit(OpCodes.Conv_R8);
                        break;
                    case NormalizedOpCode.Ishl:
                        ilGenerator.Emit_And_I4(31);
                        ilGenerator.Emit(OpCodes.Shl);
                        break;
                    case NormalizedOpCode.Lshl:
                        ilGenerator.Emit_And_I4(63);
                        ilGenerator.Emit(OpCodes.Shl);
                        break;
                    case NormalizedOpCode.Iushr:
                        ilGenerator.Emit_And_I4(31);
                        ilGenerator.Emit(OpCodes.Shr_Un);
                        break;
                    case NormalizedOpCode.Lushr:
                        ilGenerator.Emit_And_I4(63);
                        ilGenerator.Emit(OpCodes.Shr_Un);
                        break;
                    case NormalizedOpCode.Ishr:
                        ilGenerator.Emit_And_I4(31);
                        ilGenerator.Emit(OpCodes.Shr);
                        break;
                    case NormalizedOpCode.Lshr:
                        ilGenerator.Emit_And_I4(63);
                        ilGenerator.Emit(OpCodes.Shr);
                        break;
                    case NormalizedOpCode.Swap:
                        {
                            var dh = new DupHelper(this, 2);
                            dh.SetType(0, ma.GetRawStackTypeWrapper(i, 0));
                            dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
                            dh.Store(0);
                            dh.Store(1);
                            dh.Load(0);
                            dh.Load(1);
                            dh.Release();
                            break;
                        }
                    case NormalizedOpCode.Dup:
                        // if the TOS contains a "new" object or a fault block exception, it isn't really there, so we don't dup it
                        if (!RuntimeVerifierJavaType.IsNotPresentOnStack(ma.GetRawStackTypeWrapper(i, 0)))
                            ilGenerator.Emit(OpCodes.Dup);

                        break;
                    case NormalizedOpCode.Dup2:
                        {
                            var type1 = ma.GetRawStackTypeWrapper(i, 0);
                            if (type1.IsWidePrimitive)
                            {
                                ilGenerator.Emit(OpCodes.Dup);
                            }
                            else
                            {
                                var dh = new DupHelper(this, 2);
                                dh.SetType(0, type1);
                                dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
                                dh.Store(0);
                                dh.Store(1);
                                dh.Load(1);
                                dh.Load(0);
                                dh.Load(1);
                                dh.Load(0);
                                dh.Release();
                            }
                            break;
                        }
                    case NormalizedOpCode.DupX1:
                        {
                            var dh = new DupHelper(this, 2);
                            dh.SetType(0, ma.GetRawStackTypeWrapper(i, 0));
                            dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
                            dh.Store(0);
                            dh.Store(1);
                            dh.Load(0);
                            dh.Load(1);
                            dh.Load(0);
                            dh.Release();
                            break;
                        }
                    case NormalizedOpCode.Dup2X1:
                        {
                            var type1 = ma.GetRawStackTypeWrapper(i, 0);
                            if (type1.IsWidePrimitive)
                            {
                                var dh = new DupHelper(this, 2);
                                dh.SetType(0, type1);
                                dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
                                dh.Store(0);
                                dh.Store(1);
                                dh.Load(0);
                                dh.Load(1);
                                dh.Load(0);
                                dh.Release();
                            }
                            else
                            {
                                var dh = new DupHelper(this, 3);
                                dh.SetType(0, type1);
                                dh.SetType(1, ma.GetRawStackTypeWrapper(i, 1));
                                dh.SetType(2, ma.GetRawStackTypeWrapper(i, 2));
                                dh.Store(0);
                                dh.Store(1);
                                dh.Store(2);
                                dh.Load(1);
                                dh.Load(0);
                                dh.Load(2);
                                dh.Load(1);
                                dh.Load(0);
                                dh.Release();
                            }
                            break;
                        }
                    case NormalizedOpCode.Dup2X2:
                        {
                            var type1 = ma.GetRawStackTypeWrapper(i, 0);
                            var type2 = ma.GetRawStackTypeWrapper(i, 1);
                            if (type1.IsWidePrimitive)
                            {
                                if (type2.IsWidePrimitive)
                                {
                                    // Form 4
                                    var dh = new DupHelper(this, 2);
                                    dh.SetType(0, type1);
                                    dh.SetType(1, type2);
                                    dh.Store(0);
                                    dh.Store(1);
                                    dh.Load(0);
                                    dh.Load(1);
                                    dh.Load(0);
                                    dh.Release();
                                }
                                else
                                {
                                    // Form 2
                                    var dh = new DupHelper(this, 3);
                                    dh.SetType(0, type1);
                                    dh.SetType(1, type2);
                                    dh.SetType(2, ma.GetRawStackTypeWrapper(i, 2));
                                    dh.Store(0);
                                    dh.Store(1);
                                    dh.Store(2);
                                    dh.Load(0);
                                    dh.Load(2);
                                    dh.Load(1);
                                    dh.Load(0);
                                    dh.Release();
                                }
                            }
                            else
                            {
                                var type3 = ma.GetRawStackTypeWrapper(i, 2);
                                if (type3.IsWidePrimitive)
                                {
                                    // Form 3
                                    var dh = new DupHelper(this, 3);
                                    dh.SetType(0, type1);
                                    dh.SetType(1, type2);
                                    dh.SetType(2, type3);
                                    dh.Store(0);
                                    dh.Store(1);
                                    dh.Store(2);
                                    dh.Load(1);
                                    dh.Load(0);
                                    dh.Load(2);
                                    dh.Load(1);
                                    dh.Load(0);
                                    dh.Release();
                                }
                                else
                                {
                                    // Form 1
                                    var dh = new DupHelper(this, 4);
                                    dh.SetType(0, type1);
                                    dh.SetType(1, type2);
                                    dh.SetType(2, type3);
                                    dh.SetType(3, ma.GetRawStackTypeWrapper(i, 3));
                                    dh.Store(0);
                                    dh.Store(1);
                                    dh.Store(2);
                                    dh.Store(3);
                                    dh.Load(1);
                                    dh.Load(0);
                                    dh.Load(3);
                                    dh.Load(2);
                                    dh.Load(1);
                                    dh.Load(0);
                                    dh.Release();
                                }
                            }
                            break;
                        }
                    case NormalizedOpCode.DupX2:
                        {
                            var type2 = ma.GetRawStackTypeWrapper(i, 1);
                            if (type2.IsWidePrimitive)
                            {
                                // Form 2
                                var dh = new DupHelper(this, 2);
                                dh.SetType(0, ma.GetRawStackTypeWrapper(i, 0));
                                dh.SetType(1, type2);
                                dh.Store(0);
                                dh.Store(1);
                                dh.Load(0);
                                dh.Load(1);
                                dh.Load(0);
                                dh.Release();
                            }
                            else
                            {
                                // Form 1
                                var dh = new DupHelper(this, 3);
                                dh.SetType(0, ma.GetRawStackTypeWrapper(i, 0));
                                dh.SetType(1, type2);
                                dh.SetType(2, ma.GetRawStackTypeWrapper(i, 2));
                                dh.Store(0);
                                dh.Store(1);
                                dh.Store(2);
                                dh.Load(0);
                                dh.Load(2);
                                dh.Load(1);
                                dh.Load(0);
                                dh.Release();
                            }
                            break;
                        }
                    case NormalizedOpCode.Pop2:
                        {
                            var type1 = ma.GetRawStackTypeWrapper(i, 0);
                            if (type1.IsWidePrimitive)
                            {
                                ilGenerator.Emit(OpCodes.Pop);
                            }
                            else
                            {
                                if (!RuntimeVerifierJavaType.IsNotPresentOnStack(type1))
                                    ilGenerator.Emit(OpCodes.Pop);
                                if (!RuntimeVerifierJavaType.IsNotPresentOnStack(ma.GetRawStackTypeWrapper(i, 1)))
                                    ilGenerator.Emit(OpCodes.Pop);
                            }
                            break;
                        }
                    case NormalizedOpCode.Pop:
                        // if the TOS is a new object or a fault block exception, it isn't really there, so we don't need to pop it
                        if (!RuntimeVerifierJavaType.IsNotPresentOnStack(ma.GetRawStackTypeWrapper(i, 0)))
                        {
                            ilGenerator.Emit(OpCodes.Pop);
                        }
                        break;
                    case NormalizedOpCode.MonitorEnter:
                        ilGenerator.EmitMonitorEnter();
                        break;
                    case NormalizedOpCode.MonitorExit:
                        ilGenerator.EmitMonitorExit();
                        break;
                    case NormalizedOpCode.AthrowNoUnmap:
                        if (ma.GetRawStackTypeWrapper(i, 0).IsUnloadable)
                        {
                            ilGenerator.Emit(OpCodes.Castclass, finish.Context.Types.Exception);
                        }
                        ilGenerator.Emit(OpCodes.Throw);
                        break;
                    case NormalizedOpCode.Athrow:
                        if (RuntimeVerifierJavaType.IsFaultBlockException(ma.GetRawStackTypeWrapper(i, 0)))
                        {
                            ilGenerator.Emit(OpCodes.Endfinally);
                        }
                        else
                        {
                            if (ma.GetRawStackTypeWrapper(i, 0).IsUnloadable)
                            {
                                ilGenerator.Emit(OpCodes.Castclass, finish.Context.Types.Exception);
                            }
                            ilGenerator.Emit(OpCodes.Call, finish.Context.CompilerFactory.UnmapExceptionMethod);
                            ilGenerator.Emit(OpCodes.Throw);
                        }
                        break;
                    case NormalizedOpCode.TableSwitch:
                        {
                            // note that a tableswitch always has at least one entry
                            // (otherwise it would have failed verification)
                            CodeEmitterLabel[] labels = new CodeEmitterLabel[instr.SwitchEntryCount];
                            for (int j = 0; j < labels.Length; j++)
                            {
                                labels[j] = block.GetLabel(instr.GetSwitchTargetIndex(j));
                            }
                            if (instr.GetSwitchValue(0) != 0)
                            {
                                ilGenerator.EmitLdc_I4(instr.GetSwitchValue(0));
                                ilGenerator.Emit(OpCodes.Sub);
                            }
                            ilGenerator.EmitSwitch(labels);
                            ilGenerator.EmitBr(block.GetLabel(instr.DefaultTarget));
                            break;
                        }
                    case NormalizedOpCode.LookupSwitch:
                        for (int j = 0; j < instr.SwitchEntryCount; j++)
                        {
                            ilGenerator.Emit(OpCodes.Dup);
                            ilGenerator.EmitLdc_I4(instr.GetSwitchValue(j));
                            CodeEmitterLabel label = ilGenerator.DefineLabel();
                            ilGenerator.EmitBne_Un(label);
                            ilGenerator.Emit(OpCodes.Pop);
                            ilGenerator.EmitBr(block.GetLabel(instr.GetSwitchTargetIndex(j)));
                            ilGenerator.MarkLabel(label);
                        }
                        ilGenerator.Emit(OpCodes.Pop);
                        ilGenerator.EmitBr(block.GetLabel(instr.DefaultTarget));
                        break;
                    case NormalizedOpCode.Iinc:
                        LoadLocal(i);
                        ilGenerator.EmitLdc_I4(instr.Arg2);
                        ilGenerator.Emit(OpCodes.Add);
                        StoreLocal(i);
                        break;
                    case NormalizedOpCode.I2b:
                        ilGenerator.Emit(OpCodes.Conv_I1);
                        break;
                    case NormalizedOpCode.I2c:
                        ilGenerator.Emit(OpCodes.Conv_U2);
                        break;
                    case NormalizedOpCode.I2s:
                        ilGenerator.Emit(OpCodes.Conv_I2);
                        break;
                    case NormalizedOpCode.L2i:
                        ilGenerator.Emit(OpCodes.Conv_I4);
                        break;
                    case NormalizedOpCode.F2i:
                        ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.f2i);
                        break;
                    case NormalizedOpCode.D2i:
                        ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.d2i);
                        break;
                    case NormalizedOpCode.F2l:
                        ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.f2l);
                        break;
                    case NormalizedOpCode.D2l:
                        ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.d2l);
                        break;
                    case NormalizedOpCode.I2l:
                        ilGenerator.Emit(OpCodes.Conv_I8);
                        break;
                    case NormalizedOpCode.I2f:
                    case NormalizedOpCode.L2f:
                    case NormalizedOpCode.D2f:
                        ilGenerator.Emit(OpCodes.Conv_R4);
                        break;
                    case NormalizedOpCode.I2d:
                    case NormalizedOpCode.L2d:
                    case NormalizedOpCode.F2d:
                        ilGenerator.Emit(OpCodes.Conv_R8);
                        break;
                    case NormalizedOpCode.Nop:
                        ilGenerator.Emit(OpCodes.Nop);
                        break;
                    case NormalizedOpCode.IntrinsicGettype:
                        ilGenerator.Emit(OpCodes.Callvirt, finish.Context.CompilerFactory.GetTypeMethod);
                        break;
                    case NormalizedOpCode.StaticError:
                        {
                            bool wrapIncompatibleClassChangeError = false;
                            RuntimeJavaType exceptionType;
                            switch (instr.HardError)
                            {
                                case HardError.AbstractMethodError:
                                    exceptionType = finish.Context.ClassLoaderFactory.LoadClassCritical("java.lang.AbstractMethodError");
                                    break;
                                case HardError.IllegalAccessError:
                                    exceptionType = finish.Context.ClassLoaderFactory.LoadClassCritical("java.lang.IllegalAccessError");
                                    break;
                                case HardError.IncompatibleClassChangeError:
                                    exceptionType = finish.Context.ClassLoaderFactory.LoadClassCritical("java.lang.IncompatibleClassChangeError");
                                    break;
                                case HardError.InstantiationError:
                                    exceptionType = finish.Context.ClassLoaderFactory.LoadClassCritical("java.lang.InstantiationError");
                                    break;
                                case HardError.LinkageError:
                                    exceptionType = finish.Context.ClassLoaderFactory.LoadClassCritical("java.lang.LinkageError");
                                    break;
                                case HardError.NoClassDefFoundError:
                                    exceptionType = finish.Context.ClassLoaderFactory.LoadClassCritical("java.lang.NoClassDefFoundError");
                                    break;
                                case HardError.NoSuchFieldError:
                                    exceptionType = finish.Context.ClassLoaderFactory.LoadClassCritical("java.lang.NoSuchFieldError");
                                    break;
                                case HardError.NoSuchMethodError:
                                    exceptionType = finish.Context.ClassLoaderFactory.LoadClassCritical("java.lang.NoSuchMethodError");
                                    break;
                                case HardError.IllegalAccessException:
                                    exceptionType = finish.Context.ClassLoaderFactory.LoadClassCritical("java.lang.IllegalAccessException");
                                    wrapIncompatibleClassChangeError = true;
                                    break;
                                default:
                                    throw new InvalidOperationException();
                            }
                            if (wrapIncompatibleClassChangeError)
                            {
                                finish.Context.ClassLoaderFactory.LoadClassCritical("java.lang.IncompatibleClassChangeError").GetMethod("<init>", "()V", false).EmitNewobj(ilGenerator);
                            }

                            var message = harderrors[instr.HardErrorMessageId];
                            ilGenerator.Emit(OpCodes.Ldstr, message);
                            RuntimeJavaMethod method = exceptionType.GetMethod("<init>", "(Ljava.lang.String;)V", false);
                            method.Link();
                            method.EmitNewobj(ilGenerator);
                            if (wrapIncompatibleClassChangeError)
                            {
                                finish.Context.JavaBase.TypeOfjavaLangThrowable.GetMethod("initCause", "(Ljava.lang.Throwable;)Ljava.lang.Throwable;", false).EmitCallvirt(ilGenerator);
                            }
                            ilGenerator.Emit(OpCodes.Throw);
                            break;
                        }
                    default:
                        throw new NotImplementedException(instr.NormalizedOpCode.ToString());
                }

                // mark next instruction as inuse
                switch (OpCodeMetaData.GetFlowKind(instr.NormalizedOpCode))
                {
                    case OpCodeFlowControl.Switch:
                    case OpCodeFlowControl.Branch:
                    case OpCodeFlowControl.Return:
                    case OpCodeFlowControl.Throw:
                        instructionIsForwardReachable = false;
                        break;
                    case OpCodeFlowControl.ConditionalBranch:
                    case OpCodeFlowControl.Next:
                        instructionIsForwardReachable = true;
                        Debug.Assert((flags[i + 1] & InstructionFlags.Reachable) != 0);
                        // don't fall through end of try block
                        if (block.EndIndex == i + 1)
                        {
                            // TODO instead of emitting a branch to the leave stub, it would be more efficient to put the leave stub here
                            ilGenerator.EmitBr(block.GetLabel(i + 1));
                        }
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        void EmitReturnTypeConversion(RuntimeJavaType returnType)
        {
            returnType.EmitConvSignatureTypeToStackType(ilGenerator);
            if (!strictfp)
            {
                // no need to convert
            }
            else if (returnType == finish.Context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                ilGenerator.Emit(OpCodes.Conv_R8);
            }
            else if (returnType == finish.Context.PrimitiveJavaTypeFactory.FLOAT)
            {
                ilGenerator.Emit(OpCodes.Conv_R4);
            }
        }

        void EmitLoadConstant(CodeEmitter ilgen, ConstantHandle constant)
        {
            switch (classFile.GetConstantPoolConstantType(constant))
            {
                case ConstantType.Double:
                    ilgen.EmitLdc_R8(classFile.GetConstantPoolConstantDouble((DoubleConstantHandle)constant));
                    break;
                case ConstantType.Float:
                    ilgen.EmitLdc_R4(classFile.GetConstantPoolConstantFloat((FloatConstantHandle)constant));
                    break;
                case ConstantType.Integer:
                    ilgen.EmitLdc_I4(classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)constant));
                    break;
                case ConstantType.Long:
                    ilgen.EmitLdc_I8(classFile.GetConstantPoolConstantLong((LongConstantHandle)constant));
                    break;
                case ConstantType.String:
                    ilgen.Emit(OpCodes.Ldstr, classFile.GetConstantPoolConstantString((StringConstantHandle)constant));
                    break;
                case ConstantType.Class:
                    EmitLoadClass(ilgen, classFile.GetConstantPoolClassType((ClassConstantHandle)constant));
                    break;
                case ConstantType.MethodHandle:
                    finish.GetValue<MethodHandleConstant>(constant.Slot).Emit(this, ilgen, (MethodHandleConstantHandle)constant);
                    break;
                case ConstantType.MethodType:
                    finish.GetValue<MethodTypeConstant>(constant.Slot).Emit(this, ilgen, (MethodTypeConstantHandle)constant);
                    break;
#if !IMPORTER
                case ConstantType.LiveObject:
                    finish.EmitLiveObjectLoad(ilgen, classFile.GetConstantPoolConstantLiveObject(constant.Slot));
                    break;
#endif
                default:
                    throw new InvalidOperationException();
            }
        }

        private void EmitDynamicCast(RuntimeJavaType tw)
        {
            Debug.Assert(tw.IsUnloadable);
            Profiler.Count("EmitDynamicCast");

            // NOTE it's important that we don't try to load the class if obj == null
            var ok = ilGenerator.DefineLabel();
            ilGenerator.Emit(OpCodes.Dup);
            ilGenerator.EmitBrfalse(ok);
            EmitDynamicClassLiteral(tw);
            ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.DynamicCast);
            ilGenerator.MarkLabel(ok);
        }

        void EmitDynamicInstanceOf(RuntimeJavaType tw)
        {
            // NOTE it's important that we don't try to load the class if obj == null
            var notnull = ilGenerator.DefineLabel();
            var end = ilGenerator.DefineLabel();
            ilGenerator.Emit(OpCodes.Dup);
            ilGenerator.EmitBrtrue(notnull);
            ilGenerator.Emit(OpCodes.Pop);
            ilGenerator.EmitLdc_I4(0);
            ilGenerator.EmitBr(end);
            ilGenerator.MarkLabel(notnull);
            EmitDynamicClassLiteral(tw);
            ilGenerator.Emit(OpCodes.Call, finish.Context.ByteCodeHelperMethods.DynamicInstanceOf);
            ilGenerator.MarkLabel(end);
        }

        void EmitDynamicClassLiteral(RuntimeJavaType tw)
        {
            finish.EmitDynamicClassLiteral(ilGenerator, tw, m.IsLambdaFormCompiled);
        }

        void EmitLoadClass(CodeEmitter ilgen, RuntimeJavaType tw)
        {
            if (tw.IsUnloadable)
            {
                Profiler.Count("EmitDynamicClassLiteral");
                finish.EmitDynamicClassLiteral(ilgen, tw, m.IsLambdaFormCompiled);
            }
            else
            {
                tw.EmitClassLiteral(ilgen);
            }
        }

        internal static bool HasUnloadable(RuntimeJavaType[] args, RuntimeJavaType ret)
        {
            RuntimeJavaType tw = ret;
            for (int i = 0; !tw.IsUnloadable && i < args.Length; i++)
            {
                tw = args[i];
            }
            return tw.IsUnloadable;
        }

        static class InvokeDynamicBuilder
        {

            internal static void Emit(Compiler compiler, ConstantPoolItemInvokeDynamic cpi, Type delegateType)
            {
                var typeofOpenIndyCallSite = compiler.finish.Context.Resolver.ResolveRuntimeType("IKVM.Runtime.IndyCallSite`1").AsReflection();
                var methodLookup = compiler.finish.Context.ClassLoaderFactory.LoadClassCritical("java.lang.invoke.MethodHandles").GetMethod("lookup", "()Ljava.lang.invoke.MethodHandles$Lookup;", false);
                methodLookup.Link();

                var typeofIndyCallSite = typeofOpenIndyCallSite.MakeGenericType(delegateType);
                MethodInfo methodCreateBootStrap;
                MethodInfo methodGetTarget;
                if (ReflectUtil.ContainsTypeBuilder(typeofIndyCallSite))
                {
                    methodCreateBootStrap = TypeBuilder.GetMethod(typeofIndyCallSite, typeofOpenIndyCallSite.GetMethod("CreateBootstrap"));
                    methodGetTarget = TypeBuilder.GetMethod(typeofIndyCallSite, typeofOpenIndyCallSite.GetMethod("GetTarget"));
                }
                else
                {
                    methodCreateBootStrap = typeofIndyCallSite.GetMethod("CreateBootstrap");
                    methodGetTarget = typeofIndyCallSite.GetMethod("GetTarget");
                }

                var tb = compiler.finish.DefineIndyCallSiteType();
                var fb = tb.DefineField("value", typeofIndyCallSite, FieldAttributes.Static | FieldAttributes.Assembly);
                var ilgen = compiler.finish.Context.CodeEmitterFactory.Create(ReflectUtil.DefineTypeInitializer(tb, compiler.clazz.ClassLoader));
                ilgen.Emit(OpCodes.Ldnull);
                ilgen.Emit(OpCodes.Ldftn, CreateBootstrapStub(compiler, cpi, delegateType, tb, fb, methodGetTarget));
                ilgen.Emit(OpCodes.Newobj, compiler.finish.Context.MethodHandleUtil.GetDelegateConstructor(delegateType));
                ilgen.Emit(OpCodes.Call, methodCreateBootStrap);
                ilgen.Emit(OpCodes.Stsfld, fb);
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();

                compiler.ilGenerator.Emit(OpCodes.Ldsfld, fb);
                compiler.ilGenerator.Emit(OpCodes.Call, methodGetTarget);
            }

            static MethodBuilder CreateBootstrapStub(Compiler compiler, ConstantPoolItemInvokeDynamic cpi, Type delegateType, TypeBuilder tb, FieldBuilder fb, MethodInfo methodGetTarget)
            {
                var typeofCallSite = compiler.finish.Context.ClassLoaderFactory.LoadClassCritical("java.lang.invoke.CallSite").TypeAsSignatureType;
                var args = Type.EmptyTypes;

                if (delegateType.IsGenericType)
                {
                    // MONOBUG we don't look at the invoke method directly here, because Mono doesn't support GetParameters() on a builder instantiation
                    args = delegateType.GetGenericArguments();
                    if (cpi.GetRetType() != compiler.finish.Context.PrimitiveJavaTypeFactory.VOID)
                        Array.Resize(ref args, args.Length - 1);
                }

                var mb = tb.DefineMethod("BootstrapStub", MethodAttributes.Static | MethodAttributes.PrivateScope, cpi.GetRetType().TypeAsSignatureType, args);
                var ilgen = compiler.finish.Context.CodeEmitterFactory.Create(mb);
                var cs = ilgen.DeclareLocal(typeofCallSite);
                var ex = ilgen.DeclareLocal(compiler.finish.Context.Types.Exception);
                var ok = ilgen.DeclareLocal(compiler.finish.Context.Types.Boolean);
                var label = ilgen.DefineLabel();
                ilgen.BeginExceptionBlock();

                if (EmitCallBootstrapMethod(compiler, cpi, ilgen, ok))
                {
                    ilgen.Emit(OpCodes.Isinst, typeofCallSite);
                    ilgen.Emit(OpCodes.Stloc, cs);
                }

                ilgen.EmitLeave(label);
                ilgen.BeginCatchBlock(compiler.finish.Context.Types.Exception);
                ilgen.Emit(OpCodes.Stloc, ex);
                ilgen.Emit(OpCodes.Ldloc, ok);
                var label2 = ilgen.DefineLabel();
                ilgen.EmitBrtrue(label2);
                ilgen.Emit(OpCodes.Rethrow);
                ilgen.MarkLabel(label2);
                ilgen.EmitLeave(label);
                ilgen.EndExceptionBlock();
                ilgen.MarkLabel(label);
                ilgen.Emit(OpCodes.Ldsflda, fb);
                ilgen.Emit(OpCodes.Ldloc, cs);
                ilgen.Emit(OpCodes.Ldloc, ex);

                if (HasUnloadable(cpi.GetArgTypes(), cpi.GetRetType()))
                {
                    ilgen.Emit(OpCodes.Ldstr, cpi.Signature);
                    compiler.finish.EmitCallerID(ilgen, compiler.m.IsLambdaFormHidden);
                    ilgen.Emit(OpCodes.Call, compiler.finish.Context.ByteCodeHelperMethods.DynamicLinkIndyCallSite.MakeGenericMethod(delegateType));
                }
                else
                {
                    ilgen.Emit(OpCodes.Call, compiler.finish.Context.ByteCodeHelperMethods.LinkIndyCallSite.MakeGenericMethod(delegateType));
                }

                ilgen.Emit(OpCodes.Ldsfld, fb);
                ilgen.Emit(OpCodes.Call, methodGetTarget);

                for (int i = 0; i < args.Length; i++)
                    ilgen.EmitLdarg(i);
                ilgen.Emit(OpCodes.Callvirt, compiler.finish.Context.MethodHandleUtil.GetDelegateInvokeMethod(delegateType));
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();
                return mb;
            }

            static bool EmitCallBootstrapMethod(Compiler compiler, ConstantPoolItemInvokeDynamic cpi, CodeEmitter ilgen, CodeEmitterLocal ok)
            {
                var methodLookup = compiler.finish.Context.ClassLoaderFactory.LoadClassCritical("java.lang.invoke.MethodHandles").GetMethod("lookup", "()Ljava.lang.invoke.MethodHandles$Lookup;", false);
                methodLookup.Link();

                var bsm = compiler.classFile.GetBootstrapMethod(cpi.BootstrapMethod);
                if (3 + bsm.Arguments.Count > 255)
                {
                    ilgen.EmitThrow("java.lang.BootstrapMethodError", "too many bootstrap method arguments");
                    return false;
                }

                var mh = compiler.classFile.GetConstantPoolConstantMethodHandle(bsm.Method);
                var mw = mh.Member as RuntimeJavaMethod;
                switch (mh.Kind)
                {
                    case MethodHandleKind.InvokeStatic:
                        if (mw != null && !mw.IsStatic)
                            goto default;
                        break;
                    case MethodHandleKind.NewInvokeSpecial:
                        if (mw != null && !mw.IsConstructor)
                            goto default;
                        break;
                    default:
                        // to throw the right exception, we have to resolve the MH constant here
                        compiler.finish.GetValue<MethodHandleConstant>(bsm.Method.Slot).Emit(compiler, ilgen, bsm.Method);
                        ilgen.Emit(OpCodes.Pop);
                        ilgen.EmitLdc_I4(1);
                        ilgen.Emit(OpCodes.Stloc, ok);
                        ilgen.EmitThrow("java.lang.invoke.WrongMethodTypeException");
                        return false;
                }

                if (mw == null)
                {
                    // to throw the right exception (i.e. without wrapping it in a BootstrapMethodError), we have to resolve the MH constant here
                    compiler.finish.GetValue<MethodHandleConstant>(bsm.Method.Slot).Emit(compiler, ilgen, bsm.Method);
                    ilgen.Emit(OpCodes.Pop);
                    if (mh.MemberConstantPoolItem is ConstantPoolItemMI cpiMI)
                    {
                        mw = new DynamicBinder().Get(compiler, mh.Kind, cpiMI, false);
                    }
                    else
                    {
                        ilgen.EmitLdc_I4(1);
                        ilgen.Emit(OpCodes.Stloc, ok);
                        ilgen.EmitThrow("java.lang.invoke.WrongMethodTypeException");
                        return false;
                    }
                }

                var parameters = mw.GetParameters();
                int extraArgs = parameters.Length - 3;
                int fixedArgs;
                int varArgs;
                if (extraArgs == 1 && parameters[3].IsArray && parameters[3].ElementTypeWrapper == compiler.finish.Context.JavaBase.TypeOfJavaLangObject)
                {
                    fixedArgs = 0;
                    varArgs = bsm.Arguments.Count - fixedArgs;
                }
                else if (extraArgs != bsm.Arguments.Count)
                {
                    ilgen.EmitLdc_I4(1);
                    ilgen.Emit(OpCodes.Stloc, ok);
                    ilgen.EmitThrow("java.lang.invoke.WrongMethodTypeException");
                    return false;
                }
                else
                {
                    fixedArgs = extraArgs;
                    varArgs = -1;
                }

                compiler.finish.EmitCallerID(ilgen, compiler.m.IsLambdaFormCompiled);
                methodLookup.EmitCall(ilgen);
                ilgen.Emit(OpCodes.Ldstr, cpi.Name);
                parameters[1].EmitConvStackTypeToSignatureType(ilgen, compiler.finish.Context.JavaBase.TypeOfJavaLangString);

                if (HasUnloadable(cpi.GetArgTypes(), cpi.GetRetType()))
                {
                    // the cache is useless since we only run once, so we use a local
                    ilgen.Emit(OpCodes.Ldloca, ilgen.DeclareLocal(compiler.finish.Context.JavaBase.TypeOfJavaLangInvokeMethodType.TypeAsSignatureType));
                    ilgen.Emit(OpCodes.Ldstr, cpi.Signature);
                    compiler.finish.EmitCallerID(ilgen, compiler.m.IsLambdaFormCompiled);
                    ilgen.Emit(OpCodes.Call, compiler.finish.Context.ByteCodeHelperMethods.DynamicLoadMethodType);
                }
                else
                {
                    ilgen.Emit(OpCodes.Call, compiler.finish.Context.ByteCodeHelperMethods.LoadMethodType.MakeGenericMethod(compiler.finish.Context.MethodHandleUtil.CreateDelegateTypeForLoadConstant(cpi.GetArgTypes(), cpi.GetRetType())));
                }

                parameters[2].EmitConvStackTypeToSignatureType(ilgen, compiler.finish.Context.JavaBase.TypeOfJavaLangInvokeMethodType);

                for (int i = 0; i < fixedArgs; i++)
                    EmitExtraArg(compiler, ilgen, bsm, i, parameters[i + 3], ok);

                if (varArgs >= 0)
                {
                    ilgen.EmitLdc_I4(varArgs);
                    var elemType = parameters[parameters.Length - 1].ElementTypeWrapper;
                    ilgen.Emit(OpCodes.Newarr, elemType.TypeAsArrayType);
                    for (int i = 0; i < varArgs; i++)
                    {
                        ilgen.Emit(OpCodes.Dup);
                        ilgen.EmitLdc_I4(i);
                        EmitExtraArg(compiler, ilgen, bsm, i + fixedArgs, elemType, ok);
                        ilgen.Emit(OpCodes.Stelem_Ref);
                    }
                }
                ilgen.EmitLdc_I4(1);
                ilgen.Emit(OpCodes.Stloc, ok);

                if (mw.IsConstructor)
                    mw.EmitNewobj(ilgen);
                else
                    mw.EmitCall(ilgen);

                return true;
            }

            static void EmitExtraArg(Compiler compiler, CodeEmitter ilgen, BootstrapMethod bootstrapMethod, int index, RuntimeJavaType targetType, CodeEmitterLocal wrapException)
            {
                var constant = bootstrapMethod.Arguments[index];
                compiler.EmitLoadConstant(ilgen, constant);

                var constType = compiler.classFile.GetConstantPoolConstantType(constant) switch
                {
                    ConstantType.Integer => compiler.finish.Context.PrimitiveJavaTypeFactory.INT,
                    ConstantType.Long => compiler.finish.Context.PrimitiveJavaTypeFactory.LONG,
                    ConstantType.Float => compiler.finish.Context.PrimitiveJavaTypeFactory.FLOAT,
                    ConstantType.Double => compiler.finish.Context.PrimitiveJavaTypeFactory.DOUBLE,
                    ConstantType.Class => compiler.finish.Context.JavaBase.TypeOfJavaLangClass,
                    ConstantType.String => compiler.finish.Context.JavaBase.TypeOfJavaLangString,
                    ConstantType.MethodHandle => compiler.finish.Context.JavaBase.TypeOfJavaLangInvokeMethodHandle,
                    ConstantType.MethodType => compiler.finish.Context.JavaBase.TypeOfJavaLangInvokeMethodType,
                    _ => throw new InvalidOperationException(),
                };

                if (constType != targetType)
                {
                    ilgen.EmitLdc_I4(1);
                    ilgen.Emit(OpCodes.Stloc, wrapException);

                    if (constType.IsPrimitive)
                    {
                        var wrapper = GetWrapperType(constType, out var dummy);
                        wrapper.GetMethod("valueOf", "(" + constType.SignatureName + ")" + wrapper.SignatureName, false).EmitCall(ilgen);
                    }

                    if (targetType.IsUnloadable)
                    {
                        // do nothing
                    }
                    else if (targetType.IsPrimitive)
                    {
                        var wrapper = GetWrapperType(targetType, out var unbox);
                        ilgen.Emit(OpCodes.Castclass, wrapper.TypeAsBaseType);
                        wrapper.GetMethod(unbox, "()" + targetType.SignatureName, false).EmitCallvirt(ilgen);
                    }
                    else if (!constType.IsAssignableTo(targetType))
                    {
                        ilgen.Emit(OpCodes.Castclass, targetType.TypeAsBaseType);
                    }

                    targetType.EmitConvStackTypeToSignatureType(ilgen, targetType);
                    ilgen.EmitLdc_I4(0);
                    ilgen.Emit(OpCodes.Stloc, wrapException);
                }
            }

            static RuntimeJavaType GetWrapperType(RuntimeJavaType tw, out string unbox)
            {
                if (tw == tw.Context.PrimitiveJavaTypeFactory.INT)
                {
                    unbox = "intValue";
                    return tw.Context.ClassLoaderFactory.LoadClassCritical("java.lang.Integer");
                }
                else if (tw == tw.Context.PrimitiveJavaTypeFactory.LONG)
                {
                    unbox = "longValue";
                    return tw.Context.ClassLoaderFactory.LoadClassCritical("java.lang.Long");
                }
                else if (tw == tw.Context.PrimitiveJavaTypeFactory.FLOAT)
                {
                    unbox = "floatValue";
                    return tw.Context.ClassLoaderFactory.LoadClassCritical("java.lang.Float");
                }
                else if (tw == tw.Context.PrimitiveJavaTypeFactory.DOUBLE)
                {
                    unbox = "doubleValue";
                    return tw.Context.ClassLoaderFactory.LoadClassCritical("java.lang.Double");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

        }

        void EmitInvokeDynamic(ConstantPoolItemInvokeDynamic cpi)
        {
            var ilgen = ilGenerator;
            var args = cpi.GetArgTypes();
            var temps = new CodeEmitterLocal[args.Length];
            for (int i = args.Length - 1; i >= 0; i--)
            {
                temps[i] = ilgen.DeclareLocal(args[i].TypeAsSignatureType);
                ilgen.Emit(OpCodes.Stloc, temps[i]);
            }

            var delegateType = finish.Context.MethodHandleUtil.CreateMethodHandleDelegateType(args, cpi.GetRetType());
            InvokeDynamicBuilder.Emit(this, cpi, delegateType);
            for (int i = 0; i < args.Length; i++)
                ilgen.Emit(OpCodes.Ldloc, temps[i]);

            finish.Context.MethodHandleUtil.EmitCallDelegateInvokeMethod(ilgen, delegateType);
        }

        sealed class MethodHandleConstant
        {

            FieldBuilder field;

            internal void Emit(Compiler compiler, CodeEmitter ilgen, MethodHandleConstantHandle handle)
            {
                if (field == null)
                    field = compiler.finish.DefineDynamicMethodHandleCacheField();

                var mh = compiler.classFile.GetConstantPoolConstantMethodHandle(handle);
                ilgen.Emit(OpCodes.Ldsflda, field);
                ilgen.EmitLdc_I4((int)mh.Kind);
                ilgen.Emit(OpCodes.Ldstr, mh.Class);
                ilgen.Emit(OpCodes.Ldstr, mh.Name);
                ilgen.Emit(OpCodes.Ldstr, mh.Signature);
                compiler.finish.EmitCallerID(ilgen, compiler.m.IsLambdaFormCompiled);
                ilgen.Emit(OpCodes.Call, compiler.finish.Context.ByteCodeHelperMethods.DynamicLoadMethodHandle);
            }

        }

        sealed class MethodTypeConstant
        {

            FieldBuilder field;
            bool dynamic;

            internal void Emit(Compiler compiler, CodeEmitter ilgen, MethodTypeConstantHandle handle)
            {
                if (field == null)
                    field = CreateField(compiler, handle, ref dynamic);

                if (dynamic)
                {
                    ilgen.Emit(OpCodes.Ldsflda, field);
                    ilgen.Emit(OpCodes.Ldstr, compiler.classFile.GetConstantPoolConstantMethodType(handle).Signature);
                    compiler.finish.EmitCallerID(ilgen, compiler.m.IsLambdaFormCompiled);
                    ilgen.Emit(OpCodes.Call, compiler.finish.Context.ByteCodeHelperMethods.DynamicLoadMethodType);
                }
                else
                {
                    ilgen.Emit(OpCodes.Ldsfld, field);
                }
            }

            static FieldBuilder CreateField(Compiler compiler, MethodTypeConstantHandle handle, ref bool dynamic)
            {
                var cpi = compiler.classFile.GetConstantPoolConstantMethodType(handle);
                var args = cpi.GetArgTypes();
                var ret = cpi.GetRetType();

                if (HasUnloadable(args, ret))
                {
                    dynamic = true;
                    return compiler.finish.DefineDynamicMethodTypeCacheField();
                }
                else
                {
                    var tb = compiler.finish.DefineMethodTypeConstantType(handle);
                    var field = tb.DefineField("value", compiler.finish.Context.JavaBase.TypeOfJavaLangInvokeMethodType.TypeAsSignatureType, FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);
                    var ilgen = compiler.finish.Context.CodeEmitterFactory.Create(ReflectUtil.DefineTypeInitializer(tb, compiler.clazz.ClassLoader));
                    var delegateType = compiler.finish.Context.MethodHandleUtil.CreateDelegateTypeForLoadConstant(args, ret);
                    ilgen.Emit(OpCodes.Call, compiler.finish.Context.ByteCodeHelperMethods.LoadMethodType.MakeGenericMethod(delegateType));
                    ilgen.Emit(OpCodes.Stsfld, field);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();
                    return field;
                }
            }
        }

        private bool RequiresExplicitClassInit(RuntimeJavaType tw, int index, InstructionFlags[] flags)
        {
            var code = m.Instructions;
            for (; index < code.Length; index++)
            {
                if (code[index].NormalizedOpCode == NormalizedOpCode.InvokeSpecial)
                {
                    var cpi = classFile.GetMethodref(code[index].Arg1);
                    var mw = cpi.GetMethodForInvokespecial();
                    return !mw.IsConstructor || mw.DeclaringType != tw;
                }
                if ((flags[index] & InstructionFlags.BranchTarget) != 0
                    || OpCodeMetaData.IsBranch(code[index].NormalizedOpCode)
                    || OpCodeMetaData.CanThrowException(code[index].NormalizedOpCode))
                {
                    break;
                }
            }
            return true;
        }

        // NOTE despite its name this also handles value type args
        void CastInterfaceArgs(RuntimeJavaType declaringType, RuntimeJavaType[] args, int instructionIndex, bool instanceMethod)
        {
            bool needsCast = false;
            int firstCastArg = -1;

            if (!needsCast)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].IsUnloadable)
                    {
                        // nothing to do, callee will (eventually) do the cast
                    }
                    else if (args[i].IsGhost)
                    {
                        needsCast = true;
                        firstCastArg = i;
                        break;
                    }
                    else if (args[i].IsInterfaceOrInterfaceArray)
                    {
                        var tw = ma.GetStackTypeWrapper(instructionIndex, args.Length - 1 - i);
                        if (tw.IsUnloadable || NeedsInterfaceDownCast(tw, args[i]))
                        {
                            needsCast = true;
                            firstCastArg = i;
                            break;
                        }
                    }
                    else if (args[i].IsNonPrimitiveValueType)
                    {
                        if (i == 0 && instanceMethod && declaringType != args[i])
                        {
                            // no cast needed because we're calling an inherited method
                        }
                        else
                        {
                            needsCast = true;
                            firstCastArg = i;
                            break;
                        }
                    }
                    // if the stack contains an unloadable, we might need to cast it
                    // (e.g. if the argument type is a base class that is loadable)
                    if (ma.GetRawStackTypeWrapper(instructionIndex, args.Length - 1 - i).IsUnloadable)
                    {
                        needsCast = true;
                        firstCastArg = i;
                        break;
                    }
                }
            }

            if (needsCast)
            {
                var dh = new DupHelper(this, args.Length);
                for (int i = firstCastArg + 1; i < args.Length; i++)
                {
                    var tw = ma.GetRawStackTypeWrapper(instructionIndex, args.Length - 1 - i);
                    if (tw != finish.Context.VerifierJavaTypeFactory.UninitializedThis
                        && !RuntimeVerifierJavaType.IsThis(tw))
                        tw = args[i];

                    dh.SetType(i, tw);
                }

                for (int i = args.Length - 1; i >= firstCastArg; i--)
                {
                    if (!args[i].IsUnloadable && !args[i].IsGhost)
                    {
                        var tw = ma.GetStackTypeWrapper(instructionIndex, args.Length - 1 - i);
                        if (tw.IsUnloadable || (args[i].IsInterfaceOrInterfaceArray && NeedsInterfaceDownCast(tw, args[i])))
                        {
                            ilGenerator.EmitAssertType(args[i].TypeAsTBD);
                            Profiler.Count("InterfaceDownCast");
                        }
                    }

                    if (i != firstCastArg)
                    {
                        dh.Store(i);
                    }
                }

                if (instanceMethod && args[0].IsUnloadable && !declaringType.IsUnloadable)
                {
                    if (declaringType.IsInterface)
                        ilGenerator.EmitAssertType(declaringType.TypeAsTBD);
                    else if (declaringType.IsNonPrimitiveValueType)
                        ilGenerator.Emit(OpCodes.Unbox, declaringType.TypeAsTBD);
                    else
                        ilGenerator.Emit(OpCodes.Castclass, declaringType.TypeAsSignatureType);
                }

                for (int i = firstCastArg; i < args.Length; i++)
                {
                    if (i != firstCastArg)
                        dh.Load(i);

                    if (!args[i].IsUnloadable && args[i].IsGhost)
                    {
                        if (i == 0 && instanceMethod && !declaringType.IsInterface)
                        {
                            // we're calling a java.lang.Object method through a ghost interface reference,
                            // no ghost handling is needed
                        }
                        else if (RuntimeVerifierJavaType.IsThis(ma.GetRawStackTypeWrapper(instructionIndex, args.Length - 1 - i)))
                        {
                            // we're an instance method in a ghost interface, so the this pointer is a managed pointer to the
                            // wrapper value and if we're not calling another instance method on ourself, we need to load
                            // the wrapper value onto the stack
                            if (!instanceMethod || i != 0)
                            {
                                ilGenerator.Emit(OpCodes.Ldobj, args[i].TypeAsSignatureType);
                            }
                        }
                        else
                        {
                            var ghost = ilGenerator.AllocTempLocal(finish.Context.Types.Object);
                            ilGenerator.Emit(OpCodes.Stloc, ghost);
                            var local = ilGenerator.AllocTempLocal(args[i].TypeAsSignatureType);
                            ilGenerator.Emit(OpCodes.Ldloca, local);
                            ilGenerator.Emit(OpCodes.Ldloc, ghost);
                            ilGenerator.Emit(OpCodes.Stfld, args[i].GhostRefField);
                            ilGenerator.Emit(OpCodes.Ldloca, local);
                            ilGenerator.ReleaseTempLocal(local);
                            ilGenerator.ReleaseTempLocal(ghost);

                            // NOTE when the this argument is a value type, we need the address on the stack instead of the value
                            if (i != 0 || !instanceMethod)
                                ilGenerator.Emit(OpCodes.Ldobj, args[i].TypeAsSignatureType);
                        }
                    }
                    else
                    {
                        if (!args[i].IsUnloadable)
                        {
                            if (args[i].IsNonPrimitiveValueType)
                            {
                                if (i == 0 && instanceMethod)
                                {
                                    // we only need to unbox if the method was actually declared on the value type
                                    if (declaringType == args[i])
                                        ilGenerator.Emit(OpCodes.Unbox, args[i].TypeAsTBD);
                                }
                                else
                                {
                                    args[i].EmitUnbox(ilGenerator);
                                }
                            }
                            else if (ma.GetRawStackTypeWrapper(instructionIndex, args.Length - 1 - i).IsUnloadable)
                            {
                                ilGenerator.Emit(OpCodes.Castclass, args[i].TypeAsSignatureType);
                            }
                        }
                    }
                }

                dh.Release();
            }
        }

        bool NeedsInterfaceDownCast(RuntimeJavaType tw, RuntimeJavaType arg)
        {
            if (tw == finish.Context.VerifierJavaTypeFactory.Null)
                return false;

            if (!tw.IsAccessibleFrom(clazz))
                tw = tw.GetPublicBaseTypeWrapper();

            return !tw.IsAssignableTo(arg);
        }

        void DynamicGetPutField(Instruction instr, int i)
        {
            MethodHandleKind kind;
            switch (instr.NormalizedOpCode)
            {
                case NormalizedOpCode.DynamicGetField:
                    Profiler.Count("EmitDynamicGetfield");
                    kind = MethodHandleKind.GetField;
                    break;
                case NormalizedOpCode.DynamicPutField:
                    Profiler.Count("EmitDynamicPutfield");
                    kind = MethodHandleKind.PutField;
                    break;
                case NormalizedOpCode.DynamicGetStatic:
                    Profiler.Count("EmitDynamicGetstatic");
                    kind = MethodHandleKind.GetStatic;
                    break;
                case NormalizedOpCode.DynamicPutStatic:
                    Profiler.Count("EmitDynamicPutstatic");
                    kind = MethodHandleKind.PutStatic;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            var cpi = classFile.GetFieldref(checked((ushort)instr.Arg1));
            var fieldType = cpi.GetFieldType();
            if (kind == MethodHandleKind.PutField || kind == MethodHandleKind.PutStatic)
            {
                fieldType.EmitConvStackTypeToSignatureType(ilGenerator, ma.GetStackTypeWrapper(i, 0));
                if (strictfp)
                {
                    // no need to convert
                }
                else if (fieldType == finish.Context.PrimitiveJavaTypeFactory.DOUBLE)
                {
                    ilGenerator.Emit(OpCodes.Conv_R8);
                }
            }

            finish.GetValue<DynamicFieldBinder>(instr.Arg1 | ((byte)kind << 24)).Emit(this, cpi, kind);
            if (kind == MethodHandleKind.GetField || kind == MethodHandleKind.GetStatic)
                fieldType.EmitConvSignatureTypeToStackType(ilGenerator);
        }

        static void EmitReturnTypeConversion(CodeEmitter ilgen, RuntimeJavaType typeWrapper)
        {
            if (typeWrapper.IsUnloadable)
            {
                // nothing to do for unloadables
            }
            else if (typeWrapper == ilgen.Context.PrimitiveJavaTypeFactory.VOID)
            {
                ilgen.Emit(OpCodes.Pop);
            }
            else if (typeWrapper.IsPrimitive)
            {
                // NOTE we don't need to use TypeWrapper.EmitUnbox, because the return value cannot be null
                ilgen.Emit(OpCodes.Unbox, typeWrapper.TypeAsTBD);
                ilgen.Emit(OpCodes.Ldobj, typeWrapper.TypeAsTBD);
                if (typeWrapper == ilgen.Context.PrimitiveJavaTypeFactory.BYTE)
                {
                    ilgen.Emit(OpCodes.Conv_I1);
                }
            }
            else
            {
                typeWrapper.EmitCheckcast(ilgen);
            }
        }

        internal sealed class MethodHandleMethodWrapper : RuntimeJavaMethod
        {

            readonly Compiler compiler;
            readonly RuntimeJavaType wrapper;
            readonly ConstantPoolItemMI cpi;

            internal MethodHandleMethodWrapper(Compiler compiler, RuntimeJavaType wrapper, ConstantPoolItemMI cpi) :
                base(compiler.finish.Context.JavaBase.TypeOfJavaLangInvokeMethodHandle, cpi.Name, cpi.Signature, null, cpi.GetRetType(), cpi.GetArgTypes(), Modifiers.Public, MemberFlags.None)
            {
                this.compiler = compiler;
                this.wrapper = wrapper;
                this.cpi = cpi;
            }

            static void ToBasic(RuntimeJavaType tw, CodeEmitter ilgen)
            {
                if (tw.IsNonPrimitiveValueType)
                    tw.EmitBox(ilgen);
                else if (tw.IsGhost)
                    tw.EmitConvSignatureTypeToStackType(ilgen);
            }

            static void FromBasic(RuntimeJavaType tw, CodeEmitter ilgen)
            {
                if (tw.IsNonPrimitiveValueType)
                {
                    tw.EmitUnbox(ilgen);
                }
                else if (tw.IsGhost)
                {
                    tw.EmitConvStackTypeToSignatureType(ilgen, null);
                }
                else if (!tw.IsPrimitive && tw != ilgen.Context.JavaBase.TypeOfJavaLangObject)
                {
                    tw.EmitCheckcast(ilgen);
                }
            }

            internal override void EmitCall(CodeEmitter ilgen)
            {
                Debug.Assert(cpi.Name == "linkToVirtual" || cpi.Name == "linkToStatic" || cpi.Name == "linkToSpecial" || cpi.Name == "linkToInterface");
                EmitLinkToCall(ilgen.Context, ilgen, cpi.GetArgTypes(), cpi.GetRetType());
            }

            internal static void EmitLinkToCall(RuntimeContext context, CodeEmitter ilgen, RuntimeJavaType[] args, RuntimeJavaType retType)
            {
#if !FIRST_PASS && !IMPORTER
                var temps = new CodeEmitterLocal[args.Length];
                for (int i = args.Length - 1; i > 0; i--)
                {
                    temps[i] = ilgen.DeclareLocal(context.MethodHandleUtil.AsBasicType(args[i]));
                    ToBasic(args[i], ilgen);
                    ilgen.Emit(OpCodes.Stloc, temps[i]);
                }
                temps[0] = ilgen.DeclareLocal(args[0].TypeAsSignatureType);
                ilgen.Emit(OpCodes.Stloc, temps[0]);

                Array.Resize(ref args, args.Length - 1);
                var delegateType = context.MethodHandleUtil.CreateMemberWrapperDelegateType(args, retType);
                ilgen.Emit(OpCodes.Ldloc, temps[args.Length]);
                ilgen.Emit(OpCodes.Ldfld, typeof(global::java.lang.invoke.MemberName).GetField("vmtarget", BindingFlags.Instance | BindingFlags.NonPublic));
                ilgen.Emit(OpCodes.Castclass, delegateType);
                for (int i = 0; i < args.Length; i++)
                    ilgen.Emit(OpCodes.Ldloc, temps[i]);

                context.MethodHandleUtil.EmitCallDelegateInvokeMethod(ilgen, delegateType);
                FromBasic(retType, ilgen);
#else
                throw new InvalidOperationException();
#endif
            }

            void EmitInvokeExact(CodeEmitter ilgen)
            {
                var args = cpi.GetArgTypes();
                var temps = new CodeEmitterLocal[args.Length];
                for (int i = args.Length - 1; i >= 0; i--)
                {
                    temps[i] = ilgen.DeclareLocal(args[i].TypeAsSignatureType);
                    ilgen.Emit(OpCodes.Stloc, temps[i]);
                }

                var delegateType = ilgen.Context.MethodHandleUtil.CreateMethodHandleDelegateType(args, cpi.GetRetType());
                if (HasUnloadable(cpi.GetArgTypes(), cpi.GetRetType()))
                {
                    // TODO consider sharing the cache for the same signatures
                    ilgen.Emit(OpCodes.Ldsflda, compiler.finish.DefineDynamicMethodTypeCacheField());
                    ilgen.Emit(OpCodes.Ldstr, cpi.Signature);
                    compiler.finish.EmitCallerID(ilgen, compiler.m.IsLambdaFormCompiled);
                    ilgen.Emit(OpCodes.Call, ilgen.Context.ByteCodeHelperMethods.DynamicLoadMethodType);
                    ilgen.Emit(OpCodes.Call, ilgen.Context.ByteCodeHelperMethods.LoadMethodType.MakeGenericMethod(delegateType));
                    ilgen.Emit(OpCodes.Call, ilgen.Context.ByteCodeHelperMethods.DynamicEraseInvokeExact);
                }

                var mi = ilgen.Context.ByteCodeHelperMethods.GetDelegateForInvokeExact.MakeGenericMethod(delegateType);
                ilgen.Emit(OpCodes.Call, mi);
                for (int i = 0; i < args.Length; i++)
                    ilgen.Emit(OpCodes.Ldloc, temps[i]);

                ilgen.Context.MethodHandleUtil.EmitCallDelegateInvokeMethod(ilgen, delegateType);
            }

            void EmitInvokeMaxArity(CodeEmitter ilgen)
            {
                var args = cpi.GetArgTypes();
                var temps = new CodeEmitterLocal[args.Length];
                for (int i = args.Length - 1; i >= 0; i--)
                {
                    temps[i] = ilgen.DeclareLocal(args[i].TypeAsSignatureType);
                    ilgen.Emit(OpCodes.Stloc, temps[i]);
                }

                var delegateType = compiler.finish.Context.MethodHandleUtil.CreateMethodHandleDelegateType(args, cpi.GetRetType());
                ilgen.Emit(OpCodes.Call, compiler.finish.Context.ByteCodeHelperMethods.LoadMethodType.MakeGenericMethod(delegateType));
                compiler.finish.Context.JavaBase.TypeOfJavaLangInvokeMethodHandle.GetMethod("asType", "(Ljava.lang.invoke.MethodType;)Ljava.lang.invoke.MethodHandle;", false).EmitCallvirt(ilgen);
                var mi = compiler.finish.Context.ByteCodeHelperMethods.GetDelegateForInvokeExact.MakeGenericMethod(delegateType);
                ilgen.Emit(OpCodes.Call, mi);
                for (int i = 0; i < args.Length; i++)
                    ilgen.Emit(OpCodes.Ldloc, temps[i]);

                compiler.finish.Context.MethodHandleUtil.EmitCallDelegateInvokeMethod(ilgen, delegateType);
            }

            void EmitInvoke(CodeEmitter ilgen)
            {
                if (cpi.GetArgTypes().Length >= 127 && compiler.finish.Context.MethodHandleUtil.SlotCount(cpi.GetArgTypes()) >= 254)
                {
                    EmitInvokeMaxArity(ilgen);
                    return;
                }

                var args = ArrayUtil.Concat(compiler.finish.Context.JavaBase.TypeOfJavaLangInvokeMethodHandle, cpi.GetArgTypes());
                var temps = new CodeEmitterLocal[args.Length];
                for (int i = args.Length - 1; i >= 0; i--)
                {
                    temps[i] = ilgen.DeclareLocal(args[i].TypeAsSignatureType);
                    ilgen.Emit(OpCodes.Stloc, temps[i]);
                }

                var delegateType = compiler.finish.Context.MethodHandleUtil.CreateMethodHandleDelegateType(args, cpi.GetRetType());
                var mi = ilgen.Context.ByteCodeHelperMethods.GetDelegateForInvoke.MakeGenericMethod(delegateType);

                var typeofInvokeCache = compiler.finish.Context.Resolver.ResolveRuntimeType("IKVM.Runtime.InvokeCache`1").AsReflection();
                var fb = compiler.finish.DefineMethodHandleInvokeCacheField(typeofInvokeCache.MakeGenericType(delegateType));
                ilgen.Emit(OpCodes.Ldloc, temps[0]);
                if (HasUnloadable(cpi.GetArgTypes(), cpi.GetRetType()))
                {
                    // TODO consider sharing the cache for the same signatures
                    ilgen.Emit(OpCodes.Ldsflda, compiler.finish.DefineDynamicMethodTypeCacheField());
                    ilgen.Emit(OpCodes.Ldstr, cpi.Signature);
                    compiler.finish.EmitCallerID(ilgen, compiler.m.IsLambdaFormCompiled);
                    ilgen.Emit(OpCodes.Call, ilgen.Context.ByteCodeHelperMethods.DynamicLoadMethodType);
                }
                else
                {
                    ilgen.Emit(OpCodes.Ldnull);
                }
                ilgen.Emit(OpCodes.Ldsflda, fb);
                ilgen.Emit(OpCodes.Call, mi);
                for (int i = 0; i < args.Length; i++)
                    ilgen.Emit(OpCodes.Ldloc, temps[i]);

                compiler.finish.Context.MethodHandleUtil.EmitCallDelegateInvokeMethod(ilgen, delegateType);
            }

            void EmitInvokeBasic(CodeEmitter ilgen)
            {
                RuntimeJavaType retType = cpi.GetRetType();
                EmitInvokeBasic(wrapper.Context, ilgen, cpi.GetArgTypes(), retType, true);
                FromBasic(retType, ilgen);
            }

            internal static void EmitInvokeBasic(RuntimeContext context, CodeEmitter ilgen, RuntimeJavaType[] args, RuntimeJavaType retType, bool toBasic)
            {
                args = ArrayUtil.Concat(context.JavaBase.TypeOfJavaLangInvokeMethodHandle, args);

                var temps = new CodeEmitterLocal[args.Length];
                for (int i = args.Length - 1; i > 0; i--)
                {
                    temps[i] = ilgen.DeclareLocal(context.MethodHandleUtil.AsBasicType(args[i]));
                    if (toBasic)
                        ToBasic(args[i], ilgen);

                    ilgen.Emit(OpCodes.Stloc, temps[i]);
                }
                temps[0] = ilgen.DeclareLocal(args[0].TypeAsSignatureType);
                ilgen.Emit(OpCodes.Stloc, temps[0]);
                var delegateType = context.MethodHandleUtil.CreateMemberWrapperDelegateType(args, retType);
                var mi = context.ByteCodeHelperMethods.GetDelegateForInvokeBasic.MakeGenericMethod(delegateType);
                ilgen.Emit(OpCodes.Ldloc, temps[0]);
                ilgen.Emit(OpCodes.Call, mi);
                for (int i = 0; i < args.Length; i++)
                    ilgen.Emit(OpCodes.Ldloc, temps[i]);

                context.MethodHandleUtil.EmitCallDelegateInvokeMethod(ilgen, delegateType);
            }

            internal override void EmitCallvirt(CodeEmitter ilgen)
            {
                switch (cpi.Name)
                {
                    case "invokeExact":
                        EmitInvokeExact(ilgen);
                        break;
                    case "invoke":
                        EmitInvoke(ilgen);
                        break;
                    case "invokeBasic":
                        EmitInvokeBasic(ilgen);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

            internal override void EmitNewobj(CodeEmitter ilgen)
            {
                throw new InvalidOperationException();
            }

        }

        sealed class DynamicFieldBinder
        {

            MethodInfo method;

            internal void Emit(Compiler compiler, ConstantPoolItemFieldref cpi, MethodHandleKind kind)
            {
                if (method == null)
                    method = CreateMethod(compiler, cpi, kind);

                compiler.ilGenerator.Emit(OpCodes.Call, method);
            }

            static MethodInfo CreateMethod(Compiler compiler, ConstantPoolItemFieldref cpi, MethodHandleKind kind)
            {
                RuntimeJavaType ret;
                RuntimeJavaType[] args;
                switch (kind)
                {
                    case MethodHandleKind.GetField:
                        ret = cpi.GetFieldType();
                        args = new[] { cpi.GetClassType() };
                        break;
                    case MethodHandleKind.GetStatic:
                        ret = cpi.GetFieldType();
                        args = Array.Empty<RuntimeJavaType>();
                        break;
                    case MethodHandleKind.PutField:
                        ret = compiler.finish.Context.PrimitiveJavaTypeFactory.VOID;
                        args = new[] { cpi.GetClassType(), cpi.GetFieldType() };
                        break;
                    case MethodHandleKind.PutStatic:
                        ret = compiler.finish.Context.PrimitiveJavaTypeFactory.VOID;
                        args = new[] { cpi.GetFieldType() };
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                return DynamicBinder.Emit(compiler, kind, cpi, ret, args, false);
            }

        }

        sealed class DynamicBinder
        {

            RuntimeJavaMethod mw;

            internal RuntimeJavaMethod Get(Compiler compiler, MethodHandleKind kind, ConstantPoolItemMI cpi, bool privileged)
            {
                return mw ??= new DynamicBinderMethodWrapper(cpi, Emit(compiler, kind, cpi, privileged), kind);
            }

            static MethodInfo Emit(Compiler compiler, MethodHandleKind kind, ConstantPoolItemMI cpi, bool privileged)
            {
                RuntimeJavaType ret;
                RuntimeJavaType[] args;
                if (kind == MethodHandleKind.InvokeStatic)
                {
                    ret = cpi.GetRetType();
                    args = cpi.GetArgTypes();
                }
                else if (kind == MethodHandleKind.NewInvokeSpecial)
                {
                    ret = cpi.GetClassType();
                    args = cpi.GetArgTypes();
                }
                else
                {
                    ret = cpi.GetRetType();
                    args = ArrayUtil.Concat(cpi.GetClassType(), cpi.GetArgTypes());
                }

                return Emit(compiler, kind, cpi, ret, args, privileged);
            }

            internal static MethodInfo Emit(Compiler compiler, MethodHandleKind kind, ConstantPoolItemFMI cpi, RuntimeJavaType ret, RuntimeJavaType[] args, bool privileged)
            {
                var ghostTarget = (kind == MethodHandleKind.InvokeSpecial || kind == MethodHandleKind.InvokeVirtual || kind == MethodHandleKind.InvokeInterface) && args[0].IsGhost;
                var delegateType = compiler.finish.Context.MethodHandleUtil.CreateMethodHandleDelegateType(args, ret);
                var fb = compiler.finish.DefineMethodHandleInvokeCacheField(delegateType);

                var types = new Type[args.Length];
                for (int i = 0; i < types.Length; i++)
                    types[i] = args[i].TypeAsSignatureType;

                if (ghostTarget)
                    types[0] = types[0].MakeByRefType();

                var mb = compiler.finish.DefineMethodHandleDispatchStub(ret.TypeAsSignatureType, types);
                var ilgen = compiler.finish.Context.CodeEmitterFactory.Create(mb);
                ilgen.Emit(OpCodes.Ldsfld, fb);
                var label = ilgen.DefineLabel();
                ilgen.EmitBrtrue(label);
                ilgen.EmitLdc_I4((int)kind);
                ilgen.Emit(OpCodes.Ldstr, cpi.Class);
                ilgen.Emit(OpCodes.Ldstr, cpi.Name);
                ilgen.Emit(OpCodes.Ldstr, cpi.Signature);
                if (privileged)
                    compiler.finish.EmitHostCallerID(ilgen);
                else
                    compiler.finish.EmitCallerID(ilgen, compiler.m.IsLambdaFormCompiled);

                ilgen.Emit(OpCodes.Call, compiler.finish.Context.ByteCodeHelperMethods.DynamicBinderMemberLookup.MakeGenericMethod(delegateType));
                ilgen.Emit(OpCodes.Volatile);
                ilgen.Emit(OpCodes.Stsfld, fb);
                ilgen.MarkLabel(label);
                ilgen.Emit(OpCodes.Ldsfld, fb);

                for (int i = 0; i < args.Length; i++)
                {
                    ilgen.EmitLdarg(i);
                    if (i == 0 && ghostTarget)
                        ilgen.Emit(OpCodes.Ldobj, args[0].TypeAsSignatureType);
                }

                compiler.finish.Context.MethodHandleUtil.EmitCallDelegateInvokeMethod(ilgen, delegateType);
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();
                return mb;
            }

            sealed class DynamicBinderMethodWrapper : RuntimeJavaMethod
            {

                readonly MethodInfo method;

                internal DynamicBinderMethodWrapper(ConstantPoolItemMI cpi, MethodInfo method, MethodHandleKind kind) :
                    base(cpi.GetClassType(), cpi.Name, cpi.Signature, null, cpi.GetRetType(), cpi.GetArgTypes(), kind == MethodHandleKind.InvokeStatic ? Modifiers.Public | Modifiers.Static : Modifiers.Public, MemberFlags.None)
                {
                    this.method = method;
                }

                internal override void EmitCall(CodeEmitter ilgen)
                {
                    ilgen.Emit(OpCodes.Call, method);
                }

                internal override void EmitCallvirt(CodeEmitter ilgen)
                {
                    ilgen.Emit(OpCodes.Call, method);
                }

                internal override void EmitNewobj(CodeEmitter ilgen)
                {
                    ilgen.Emit(OpCodes.Call, method);
                }

            }

        }

        RuntimeJavaMethod GetMethodCallEmitter(NormalizedOpCode invoke, int constantPoolIndex)
        {
            var cpi = classFile.GetMethodref(new MethodrefConstantHandle(checked((ushort)constantPoolIndex)));
#if IMPORTER
            if (replacedMethodWrappers != null)
            {
                for (int i = 0; i < replacedMethodWrappers.Length; i++)
                {
                    if (replacedMethodWrappers[i].DeclaringType == cpi.GetClassType()
                        && replacedMethodWrappers[i].Name == cpi.Name
                        && replacedMethodWrappers[i].Signature == cpi.Signature)
                    {
                        var rmw = replacedMethodWrappers[i];
                        rmw.Link();
                        return rmw;
                    }
                }
            }
#endif
            RuntimeJavaMethod mw = null;
            switch (invoke)
            {
                case NormalizedOpCode.InvokeSpecial:
                    mw = cpi.GetMethodForInvokespecial();
                    break;
                case NormalizedOpCode.InvokeInterface:
                    mw = cpi.GetMethod();
                    break;
                case NormalizedOpCode.InvokeStatic:
                case NormalizedOpCode.InvokeVirtual:
                    mw = cpi.GetMethod();
                    break;
                case NormalizedOpCode.DynamicInvokeInterface:
                case NormalizedOpCode.DynamicInvokeStatic:
                case NormalizedOpCode.DynamicInvokeVirtual:
                case NormalizedOpCode.DynamicInvokeSpecial:
                case NormalizedOpCode.PrivilegedInvokeStatic:
                case NormalizedOpCode.PrivilegedInvokeVirtual:
                case NormalizedOpCode.PrivilegedInvokeSpecial:
                    return GetDynamicMethodWrapper(constantPoolIndex, invoke, cpi);
                case NormalizedOpCode.MethodHandleInvoke:
                case NormalizedOpCode.MethodHandleLink:
                    return new MethodHandleMethodWrapper(this, clazz, cpi);
                default:
                    throw new InvalidOperationException();
            }
            if (mw.IsDynamicOnly)
            {
                return GetDynamicMethodWrapper(constantPoolIndex, invoke, cpi);
            }
            return mw;
        }

        RuntimeJavaMethod GetDynamicMethodWrapper(int index, NormalizedOpCode invoke, ConstantPoolItemMI cpi)
        {
            MethodHandleKind kind;
            switch (invoke)
            {
                case NormalizedOpCode.InvokeInterface:
                case NormalizedOpCode.DynamicInvokeInterface:
                    kind = MethodHandleKind.InvokeInterface;
                    break;
                case NormalizedOpCode.InvokeStatic:
                case NormalizedOpCode.DynamicInvokeStatic:
                case NormalizedOpCode.PrivilegedInvokeStatic:
                    kind = MethodHandleKind.InvokeStatic;
                    break;
                case NormalizedOpCode.InvokeVirtual:
                case NormalizedOpCode.DynamicInvokeVirtual:
                case NormalizedOpCode.PrivilegedInvokeVirtual:
                    kind = MethodHandleKind.InvokeVirtual;
                    break;
                case NormalizedOpCode.InvokeSpecial:
                case NormalizedOpCode.DynamicInvokeSpecial:
                    kind = MethodHandleKind.NewInvokeSpecial;
                    break;
                case NormalizedOpCode.PrivilegedInvokeSpecial:
                    // we don't support calling a base class constructor
                    kind = cpi.GetMethod().IsConstructor
                        ? MethodHandleKind.NewInvokeSpecial
                        : MethodHandleKind.InvokeSpecial;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            bool privileged;
            switch (invoke)
            {
                case NormalizedOpCode.PrivilegedInvokeStatic:
                case NormalizedOpCode.PrivilegedInvokeVirtual:
                case NormalizedOpCode.PrivilegedInvokeSpecial:
                    privileged = true;
                    break;
                default:
                    privileged = false;
                    break;
            }

            return finish.GetValue<DynamicBinder>(index | ((byte)kind << 24)).Get(this, kind, cpi, privileged);
        }

        RuntimeJavaType ComputeThisType(RuntimeJavaType type, RuntimeJavaMethod method, NormalizedOpCode invoke)
        {
            if (type == finish.Context.VerifierJavaTypeFactory.UninitializedThis || RuntimeVerifierJavaType.IsThis(type))
                return clazz;
            else if (RuntimeVerifierJavaType.IsNew(type))
                return ((RuntimeVerifierJavaType)type).UnderlyingType;
            else if (type == finish.Context.VerifierJavaTypeFactory.Null)
                return method.DeclaringType;
            else if (invoke == NormalizedOpCode.InvokeVirtual && method.IsProtected && type.IsUnloadable)
                return clazz;
            else
                return type;
        }

        LocalVar LoadLocal(int instructionIndex)
        {
            var v = localVars.GetLocalVar(instructionIndex);
            if (v.isArg)
            {
                var instr = m.Instructions[instructionIndex];
                int i = m.ArgMap[instr.NormalizedArg1];
                ilGenerator.EmitLdarg(i);
                if (v.type == finish.Context.PrimitiveJavaTypeFactory.DOUBLE)
                    ilGenerator.Emit(OpCodes.Conv_R8);
                if (v.type == finish.Context.PrimitiveJavaTypeFactory.FLOAT)
                    ilGenerator.Emit(OpCodes.Conv_R4);
            }
            else if (v.type == finish.Context.VerifierJavaTypeFactory.Null)
            {
                ilGenerator.Emit(OpCodes.Ldnull);
            }
            else
            {
                if (v.builder == null)
                {
                    v.builder = ilGenerator.DeclareLocal(GetLocalBuilderType(v.type));
                    if (debug && v.name != null)
                        v.builder.SetLocalSymInfo(v.name);
                }

                ilGenerator.Emit(OpCodes.Ldloc, v.builder);
            }

            return v;
        }

        LocalVar StoreLocal(int instructionIndex)
        {
            var v = localVars.GetLocalVar(instructionIndex);
            if (v == null)
            {
                // dead store
                ilGenerator.Emit(OpCodes.Pop);
            }
            else if (v.isArg)
            {
                var instr = m.Instructions[instructionIndex];
                int i = m.ArgMap[instr.NormalizedArg1];
                ilGenerator.EmitStarg(i);
            }
            else if (v.type == finish.Context.VerifierJavaTypeFactory.Null)
            {
                ilGenerator.Emit(OpCodes.Pop);
            }
            else
            {
                if (v.builder == null)
                {
                    v.builder = ilGenerator.DeclareLocal(GetLocalBuilderType(v.type));
                    if (debug && v.name != null)
                        v.builder.SetLocalSymInfo(v.name);
                }

                ilGenerator.Emit(OpCodes.Stloc, v.builder);
            }

            return v;
        }

        Type GetLocalBuilderType(RuntimeJavaType tw)
        {
            if (tw.IsUnloadable)
                return finish.Context.Types.Object;
            else if (tw.IsAccessibleFrom(clazz))
                return tw.TypeAsLocalOrStackType;
            else
                return tw.GetPublicBaseTypeWrapper().TypeAsLocalOrStackType;
        }

        ExceptionTableEntry[] GetExceptionTableFor(InstructionFlags[] flags)
        {
            var list = new List<ExceptionTableEntry>();

            // return only reachable exception handlers (because the code gen depends on that)
            for (int i = 0; i < exceptions.Length; i++)
            {
                // if the first instruction is unreachable, the entire block is unreachable,
                // because you can't jump into a block (we've just split the blocks to ensure that)
                if ((flags[exceptions[i].StartIndex] & InstructionFlags.Reachable) != 0)
                    list.Add(exceptions[i]);
            }

            return list.ToArray();
        }

        InstructionFlags[] ComputePartialReachability(int initialInstructionIndex, bool skipFaultBlocks)
        {
            return MethodAnalyzer.ComputePartialReachability(ma, m.Instructions, exceptions, initialInstructionIndex, skipFaultBlocks);
        }

    }

}
