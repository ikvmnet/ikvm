/*
  Copyright (C) 2008-2012 Jeroen Frijters

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
using System.Data;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

using IKVM.Reflection.Impl;
using IKVM.Reflection.Metadata;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    public sealed partial class ILGenerator
    {

        struct LabelFixup
        {

            internal int label;
            internal int offset;

        }

        internal sealed class ExceptionBlock
        {

            internal readonly int ordinal;
            internal Label labelEnd;
            internal int tryOffset;
            internal System.Reflection.Metadata.Ecma335.LabelHandle tryHandle;
            internal int tryLength;
            internal System.Reflection.Metadata.Ecma335.LabelHandle tryEndHandle;
            internal int handlerOffset;
            internal System.Reflection.Metadata.Ecma335.LabelHandle handlerHandle;
            internal int handlerLength;
            internal System.Reflection.Metadata.Ecma335.LabelHandle handlerEndHandle;
            internal int filterOffset;
            internal int exceptionTypeToken;
            internal ExceptionHandlingClauseOptions kind;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="ordinal"></param>
            internal ExceptionBlock(int ordinal)
            {
                this.ordinal = ordinal;
            }

        }

        struct SequencePoint
        {

            public PortablePdbSymbolDocumentWriter Document;
            public int Offset;
            public int StartLine;
            public int StartColumn;
            public int EndLine;
            public int EndColumn;

        }

        sealed class Scope
        {

            internal readonly Scope parent;
            internal readonly List<Scope> children = new List<Scope>();
            internal readonly List<LocalBuilder> locals = new List<LocalBuilder>();
            internal readonly List<string> namespaces = new List<string>();
            internal int startOffset;
            internal LabelHandle startHandle;
            internal int endOffset;
            internal LabelHandle endHandle;

            internal ImportScopeHandle handle;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="parent"></param>
            internal Scope(Scope parent)
            {
                this.parent = parent;
            }

        }

        readonly ModuleBuilder module;
        readonly BlobBuilder blob;
        readonly ControlFlowBuilder flow;
        readonly InstructionEncoder code;
        readonly SignatureHelper locals;
        int localVarLength;
        readonly List<int> tokenFixups = new List<int>();
        readonly List<int> labels = new List<int>();
        readonly List<int> labelStackHeight = new List<int>();
        readonly List<SequencePoint> sequencePoints = new List<SequencePoint>();
        readonly List<ExceptionBlock> exceptions = new List<ExceptionBlock>();
        readonly Stack<ExceptionBlock> exceptionStack = new Stack<ExceptionBlock>();
        ushort maxStack;
        bool fatHeader;
        int stackHeight;
        Scope scope;
        byte exceptionBlockAssistanceMode = EBAM_COMPAT;
        const byte EBAM_COMPAT = 0;
        const byte EBAM_DISABLE = 1;
        const byte EBAM_CLEVER = 2;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="moduleBuilder"></param>
        /// <param name="initialCapacity"></param>
        internal ILGenerator(ModuleBuilder moduleBuilder, int initialCapacity)
        {
            this.module = moduleBuilder;

            blob = new BlobBuilder(initialCapacity);
            flow = new ControlFlowBuilder();
            code = new InstructionEncoder(blob, flow);
            locals = SignatureHelper.GetLocalVarSigHelper(moduleBuilder);
            scope = new Scope(null);
        }

        // non-standard API
        public void __DisableExceptionBlockAssistance()
        {
            exceptionBlockAssistanceMode = EBAM_DISABLE;
        }

        // non-standard API
        public void __CleverExceptionBlockAssistance()
        {
            exceptionBlockAssistanceMode = EBAM_CLEVER;
        }

        // non-standard API
        public int __MaxStackSize
        {
            get { return maxStack; }
            set
            {
                maxStack = (ushort)value;
                fatHeader = true;
            }
        }

        // non-standard API
        // returns -1 if the current position is currently unreachable
        public int __StackHeight
        {
            get { return stackHeight; }
        }

        // new in .NET 4.0
        public int ILOffset
        {
            get { return code.Offset; }
        }

        public void BeginCatchBlock(Type exceptionType)
        {
            if (exceptionType == null)
            {
                // this must be a catch block after a filter
                var block = exceptionStack.Peek();
                if (block.kind != ExceptionHandlingClauseOptions.Filter || block.handlerOffset != 0)
                    throw new ArgumentNullException("exceptionType");

                if (exceptionBlockAssistanceMode == EBAM_COMPAT || (exceptionBlockAssistanceMode == EBAM_CLEVER && stackHeight != -1))
                    Emit(OpCodes.Endfilter);

                stackHeight = 0;
                UpdateStack(1);
                block.handlerOffset = code.Offset;
                block.handlerHandle = code.DefineLabel();
                code.MarkLabel(block.handlerHandle);
            }
            else
            {
                var block = BeginCatchOrFilterBlock();
                block.kind = ExceptionHandlingClauseOptions.Clause;
                block.exceptionTypeToken = module.GetTypeTokenForMemberRef(exceptionType);
                block.handlerOffset = code.Offset;
                block.handlerHandle = code.DefineLabel();
                code.MarkLabel(block.handlerHandle);
            }
        }

        ExceptionBlock BeginCatchOrFilterBlock()
        {
            var block = exceptionStack.Peek();
            if (exceptionBlockAssistanceMode == EBAM_COMPAT || (exceptionBlockAssistanceMode == EBAM_CLEVER && stackHeight != -1))
                Emit(OpCodes.Leave, block.labelEnd);

            stackHeight = 0;
            UpdateStack(1);

            if (block.tryLength == 0)
            {
                block.tryLength = code.Offset - block.tryOffset;
                block.tryEndHandle = code.DefineLabel();
                code.MarkLabel(block.tryEndHandle);
            }
            else
            {
                block.handlerLength = code.Offset - block.handlerOffset;
                block.handlerEndHandle = code.DefineLabel();
                code.MarkLabel(block.handlerEndHandle);

                exceptionStack.Pop();
                var newBlock = new ExceptionBlock(exceptions.Count);
                newBlock.labelEnd = block.labelEnd;
                newBlock.tryOffset = block.tryOffset;
                newBlock.tryHandle = block.tryHandle;
                newBlock.tryLength = block.tryLength;
                newBlock.tryEndHandle = block.tryEndHandle;
                block = newBlock;
                exceptions.Add(block);
                exceptionStack.Push(block);
            }

            return block;
        }

        public Label BeginExceptionBlock()
        {
            var block = new ExceptionBlock(exceptions.Count);
            block.labelEnd = DefineLabel();
            block.tryOffset = code.Offset;
            block.tryHandle = code.DefineLabel();
            code.MarkLabel(block.tryHandle);
            exceptionStack.Push(block);
            exceptions.Add(block);
            stackHeight = 0;
            return block.labelEnd;
        }

        public void BeginExceptFilterBlock()
        {
            var block = BeginCatchOrFilterBlock();
            block.kind = ExceptionHandlingClauseOptions.Filter;
            block.filterOffset = code.Offset;
        }

        public void BeginFaultBlock()
        {
            BeginFinallyFaultBlock(ExceptionHandlingClauseOptions.Fault);
        }

        public void BeginFinallyBlock()
        {
            BeginFinallyFaultBlock(ExceptionHandlingClauseOptions.Finally);
        }

        void BeginFinallyFaultBlock(ExceptionHandlingClauseOptions kind)
        {
            var block = exceptionStack.Peek();
            if (exceptionBlockAssistanceMode == EBAM_COMPAT || (exceptionBlockAssistanceMode == EBAM_CLEVER && stackHeight != -1))
                Emit(OpCodes.Leave, block.labelEnd);

            if (block.handlerOffset == 0)
            {
                block.tryLength = code.Offset - block.tryOffset;
                block.tryEndHandle = code.DefineLabel();
                code.MarkLabel(block.tryEndHandle);
            }
            else
            {
                block.handlerLength = code.Offset - block.handlerOffset;
                block.handlerEndHandle = code.DefineLabel();
                code.MarkLabel(block.handlerEndHandle);

                Label labelEnd;
                if (exceptionBlockAssistanceMode != EBAM_COMPAT)
                {
                    labelEnd = block.labelEnd;
                }
                else
                {
                    MarkLabel(block.labelEnd);
                    labelEnd = DefineLabel();
                    Emit(OpCodes.Leave, labelEnd);
                }
                exceptionStack.Pop();

                var newBlock = new ExceptionBlock(exceptions.Count);
                newBlock.labelEnd = labelEnd;
                newBlock.tryOffset = block.tryOffset;
                newBlock.tryHandle = block.tryHandle;
                newBlock.tryLength = code.Offset - block.tryOffset;
                newBlock.tryEndHandle = code.DefineLabel();
                code.MarkLabel(newBlock.tryEndHandle);
                block = newBlock;
                exceptions.Add(block);
                exceptionStack.Push(block);
            }

            block.handlerOffset = code.Offset;
            block.handlerHandle = code.DefineLabel();
            code.MarkLabel(block.handlerHandle);
            block.kind = kind;
            stackHeight = 0;
        }

        public void EndExceptionBlock()
        {
            var block = exceptionStack.Pop();
            if (exceptionBlockAssistanceMode == EBAM_COMPAT || (exceptionBlockAssistanceMode == EBAM_CLEVER && stackHeight != -1))
            {
                if (block.kind != ExceptionHandlingClauseOptions.Finally && block.kind != ExceptionHandlingClauseOptions.Fault)
                    Emit(OpCodes.Leave, block.labelEnd);
                else
                    Emit(OpCodes.Endfinally);
            }

            MarkLabel(block.labelEnd);
            block.handlerLength = code.Offset - block.handlerOffset;
            block.handlerEndHandle = code.DefineLabel();
            code.MarkLabel(block.handlerEndHandle);
        }

        public void BeginScope()
        {
            var newScope = new Scope(scope);
            scope.children.Add(newScope);
            scope = newScope;
            scope.startOffset = code.Offset;
            scope.startHandle = code.DefineLabel();
            code.MarkLabel(scope.startHandle);
        }

        public void UsingNamespace(string usingNamespace)
        {
            scope?.namespaces.Add(usingNamespace);
        }

        public LocalBuilder DeclareLocal(Type localType)
        {
            return DeclareLocal(localType, false);
        }

        public LocalBuilder DeclareLocal(Type localType, bool pinned)
        {
            var local = new LocalBuilder(localType, localVarLength++, pinned);
            locals.AddArgument(localType, pinned);
            scope.locals.Add(local);
            return local;
        }

        public LocalBuilder __DeclareLocal(Type localType, bool pinned, CustomModifiers customModifiers)
        {
            var local = new LocalBuilder(localType, localVarLength++, pinned, customModifiers);
            locals.__AddArgument(localType, pinned, customModifiers);
            scope.locals.Add(local);
            return local;
        }

        public Label DefineLabel()
        {
            var label = new Label(code.DefineLabel());
            labels.Add(-1);
            labelStackHeight.Add(-1);
            return label;
        }

        public void Emit(OpCode opc)
        {
            if (opc == OpCodes.Ret && stackHeight > 1)
                throw new BadImageFormatException("Unbalanced stack height.");

            UpdateStack(opc);
            code.OpCode((ILOpCode)opc.Value);
        }

        /// <summary>
        /// Updates the stack height in response to a previously written instruction.
        /// </summary>
        /// <param name="opc"></param>
        void UpdateStack(OpCode opc)
        {
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

        void UpdateStack(int stackdiff)
        {
            if (stackHeight == -1)
                stackHeight = 0; // we're about to emit code that is either unreachable or reachable only via a backward branch

            Debug.Assert(stackHeight >= 0 && stackHeight <= ushort.MaxValue);
            stackHeight += stackdiff;
            Debug.Assert(stackHeight >= 0 && stackHeight <= ushort.MaxValue);
            maxStack = Math.Max(maxStack, (ushort)stackHeight);
        }

        public void Emit(OpCode opc, byte arg)
        {
            Emit(opc);
            code.CodeBuilder.WriteByte(arg);
        }

        public void Emit(OpCode opc, double arg)
        {
            Emit(opc);
            code.CodeBuilder.WriteDouble(arg);
        }

        public void Emit(OpCode opc, FieldInfo field)
        {
            Emit(opc);
            WriteToken(module.GetFieldToken(field).Token);
        }

        public void Emit(OpCode opc, short arg)
        {
            Emit(opc);
            code.CodeBuilder.WriteInt16(arg);
        }

        public void Emit(OpCode opc, int arg)
        {
            Emit(opc);
            code.CodeBuilder.WriteInt32(arg);
        }

        public void Emit(OpCode opc, long arg)
        {
            Emit(opc);
            code.CodeBuilder.WriteInt64(arg);
        }

        /// <summary>
        /// Emits the given branch 
        /// </summary>
        /// <param name="opc"></param>
        /// <param name="label"></param>
        /// <exception cref="NotSupportedException"></exception>
        public void Emit(OpCode opc, Label label)
        {
            // We need special stackHeight handling for unconditional branches,
            // because the branch and next flows have differing stack heights.
            // Note that this assumes that unconditional branches do not push/pop.
            var flowStackHeight = stackHeight;
            UpdateStack(opc);
            if (opc == OpCodes.Leave || opc == OpCodes.Leave_S)
                flowStackHeight = 0;
            else if (opc.FlowControl != FlowControl.Branch)
                flowStackHeight = stackHeight;

            // emit branch instruction
            code.Branch((ILOpCode)opc.Value, label.Handle);
        }

        public void Emit(OpCode opc, Label[] labels)
        {
            if (opc != OpCodes.Switch)
                throw new NotSupportedException("Cannot emit non-Switch opcode with jump table.");

            UpdateStack(opc);
            var jmp = code.Switch(labels.Length);
            for (int i = 0; i < labels.Length; i++)
                jmp.Branch(labels[i].Handle);

            foreach (var label in labels)
            {
                if (this.labels[label.Index] != -1)
                {


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
            if (opc == OpCodes.Ldloc)
            {
                UpdateStack(opc);
                code.LoadLocal(local.LocalIndex);
            }
            else if (opc == OpCodes.Ldloca)
            {
                UpdateStack(opc);
                code.LoadLocalAddress(local.LocalIndex);
            }
            else if (opc == OpCodes.Stloc)
            {
                UpdateStack(opc);
                code.StoreLocal(local.LocalIndex);
            }
            else
            {
                Emit(opc);

                switch (opc.OperandType)
                {
                    case OperandType.InlineVar:
                        code.CodeBuilder.WriteUInt16((ushort)local.LocalIndex);
                        break;
                    case OperandType.ShortInlineVar:
                        code.CodeBuilder.WriteByte((byte)local.LocalIndex);
                        break;
                }
            }
        }

        void WriteToken(int token)
        {
            if (ModuleBuilder.IsPseudoToken(token))
                tokenFixups.Add(code.Offset);

            code.Token(token);
        }

        void UpdateStack(OpCode opc, bool hasthis, Type returnType, int parameterCount)
        {
            if (opc == OpCodes.Jmp)
            {
                stackHeight = -1;
            }
            else if (opc.FlowControl == FlowControl.Call)
            {
                int stackdiff = 0;

                if ((hasthis && opc != OpCodes.Newobj) || opc == OpCodes.Calli)
                    stackdiff--; // pop this

                // pop parameters
                stackdiff -= parameterCount;
                if (returnType != module.universe.System_Void)
                    stackdiff++; // push return value

                UpdateStack(stackdiff);
            }
        }

        public void Emit(OpCode opc, MethodInfo method)
        {
            Emit(opc);
            UpdateStack(opc, method.HasThis, method.ReturnType, method.ParameterCount);
            WriteToken(module.GetMethodTokenForIL(method).Token);
        }

        public void Emit(OpCode opc, ConstructorInfo constructor)
        {
            Emit(opc, constructor.GetMethodInfo());
        }

        public void Emit(OpCode opc, sbyte arg)
        {
            Emit(opc);
            code.CodeBuilder.WriteSByte(arg);
        }

        public void Emit(OpCode opc, float arg)
        {
            Emit(opc);
            code.CodeBuilder.WriteSingle(arg);
        }

        public void Emit(OpCode opc, string str)
        {
            if (opc == OpCodes.Ldstr)
            {
                UpdateStack(opc);
                code.LoadString(MetadataTokens.UserStringHandle(module.GetStringConstant(str).Token));
            }
            else
            {
                Emit(opc);
                WriteToken(module.GetStringConstant(str).Token);
            }
        }

        public void Emit(OpCode opc, Type type)
        {
            Emit(opc);
            if (opc == OpCodes.Ldtoken)
                WriteToken(module.GetTypeToken(type).Token);
            else
                WriteToken(module.GetTypeTokenForMemberRef(type));
        }

        public void Emit(OpCode opcode, SignatureHelper signature)
        {
            Emit(opcode);
            UpdateStack(opcode, signature.HasThis, signature.ReturnType, signature.ParameterCount);
            WriteToken(module.GetSignatureToken(signature).Token);
        }

        public void EmitCall(OpCode opc, MethodInfo method, Type[] optionalParameterTypes)
        {
            __EmitCall(opc, method, optionalParameterTypes, null);
        }

        public void __EmitCall(OpCode opc, MethodInfo method, Type[] optionalParameterTypes, CustomModifiers[] customModifiers)
        {
            if (optionalParameterTypes == null || optionalParameterTypes.Length == 0)
            {
                Emit(opc, method);
            }
            else
            {
                Emit(opc);
                UpdateStack(opc, method.HasThis, method.ReturnType, method.ParameterCount + optionalParameterTypes.Length);
                WriteToken(module.__GetMethodToken(method, optionalParameterTypes, customModifiers).Token);
            }
        }

        public void __EmitCall(OpCode opc, ConstructorInfo constructor, Type[] optionalParameterTypes)
        {
            EmitCall(opc, constructor.GetMethodInfo(), optionalParameterTypes);
        }

        public void __EmitCall(OpCode opc, ConstructorInfo constructor, Type[] optionalParameterTypes, CustomModifiers[] customModifiers)
        {
            __EmitCall(opc, constructor.GetMethodInfo(), optionalParameterTypes, customModifiers);
        }

        public void EmitCalli(OpCode opc, CallingConvention callingConvention, Type returnType, Type[] parameterTypes)
        {
            var sig = SignatureHelper.GetMethodSigHelper(module, callingConvention, returnType);
            sig.AddArguments(parameterTypes, null, null);
            Emit(opc, sig);
        }

        public void EmitCalli(OpCode opc, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
        {
            var sig = SignatureHelper.GetMethodSigHelper(module, callingConvention, returnType);
            sig.AddArguments(parameterTypes, null, null);
            if (optionalParameterTypes != null && optionalParameterTypes.Length != 0)
            {
                sig.AddSentinel();
                sig.AddArguments(optionalParameterTypes, null, null);
            }

            Emit(opc, sig);
        }

        public void __EmitCalli(OpCode opc, __StandAloneMethodSig sig)
        {
            Emit(opc);
            if (sig.IsUnmanaged)
            {
                UpdateStack(opc, false, sig.ReturnType, sig.ParameterCount);
            }
            else
            {
                var callingConvention = sig.CallingConvention;
                UpdateStack(opc, (callingConvention & CallingConventions.HasThis | CallingConventions.ExplicitThis) == CallingConventions.HasThis, sig.ReturnType, sig.ParameterCount);
            }

            var bb = new ByteBuffer(16);
            Signature.WriteStandAloneMethodSig(module, bb, sig);
            WriteToken(MetadataTokens.GetToken(MetadataTokens.StandaloneSignatureHandle(module.StandAloneSigTable.FindOrAddRecord(module.GetOrAddBlob(bb.ToArray())))));
        }

        public void EmitWriteLine(string text)
        {
            Emit(OpCodes.Ldstr, text);
            Emit(OpCodes.Call, module.universe.System_Console.GetMethod("WriteLine", new[] { module.universe.System_String }));
        }

        public void EmitWriteLine(FieldInfo field)
        {
            Emit(OpCodes.Call, module.universe.System_Console.GetMethod("get_Out"));
            if (field.IsStatic)
            {
                Emit(OpCodes.Ldsfld, field);
            }
            else
            {
                Emit(OpCodes.Ldarg_0);
                Emit(OpCodes.Ldfld, field);
            }

            Emit(OpCodes.Callvirt, module.universe.System_IO_TextWriter.GetMethod("WriteLine", new Type[] { field.FieldType }));
        }

        public void EmitWriteLine(LocalBuilder local)
        {
            Emit(OpCodes.Call, module.universe.System_Console.GetMethod("get_Out"));
            Emit(OpCodes.Ldloc, local);
            Emit(OpCodes.Callvirt, module.universe.System_IO_TextWriter.GetMethod("WriteLine", new Type[] { local.LocalType }));
        }

        public void EndScope()
        {
            scope.endOffset = code.Offset;
            scope = scope.parent;
        }

        public void MarkLabel(Label loc)
        {
            Debug.Assert(stackHeight == -1 || labelStackHeight[loc.Index] == -1 || stackHeight == labelStackHeight[loc.Index]);

            // store offset of label
            labels[loc.Index] = code.Offset;

            if (labelStackHeight[loc.Index] == -1)
            {
                if (stackHeight == -1)
                {
                    // We're at a location that can only be reached by a backward branch,
                    // so according to the "backward branch constraint" that must mean the stack is empty,
                    // but note that this may be an unused label followed by another label that is used and
                    // that does have a non-zero stack height, so we don't yet set stackHeight here.
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
            if (document is not PortablePdbSymbolDocumentWriter pdb)
                throw new NotSupportedException("IKVM.Reflection can only accept PortablePdbSymboldDocumentWriter instances created with ModuleBuilder.DefineDocument.");

            var sp = new SequencePoint();
            sp.Document = pdb;
            sp.Offset = code.Offset;
            sp.StartLine = startLine;
            sp.StartColumn = startColumn;
            sp.EndLine = endLine;
            sp.EndColumn = endColumn;
            sequencePoints.Add(sp);
        }

        public void ThrowException(Type excType)
        {
            Emit(OpCodes.Newobj, excType.GetConstructor(Type.EmptyTypes));
            Emit(OpCodes.Throw);
        }

        /// <summary>
        /// Writes the method body to the module.
        /// </summary>
        /// <param name="initLocals"></param>
        /// <returns></returns>
        internal int WriteBody(bool initLocals)
        {
            // close open scope
            Debug.Assert(scope != null && scope.parent == null);
            scope.endOffset = code.Offset;

            var bdy = new BlobBuilder();
            var enc = new MethodBodyStreamEncoder(bdy);

            var localSignature = default(StandaloneSignatureHandle);
            if (localVarLength != 0)
                localSignature = (StandaloneSignatureHandle)MetadataTokens.EntityHandle(module.GetSignatureToken(locals).Token);

            // determine header size so we know starting position of instructions
            var hsz = 12;
            if (code.CodeBuilder.Count < 64 && maxStack <= 8 && localSignature.IsNil && (!false || !initLocals) && exceptions.Count == 0)
                hsz = 1;
            else
                module.methodBodies.Align(4);

            // allocate new method body
            var off = enc.AddMethodBody(code, maxStack, localSignature, initLocals ? MethodBodyAttributes.InitLocals : MethodBodyAttributes.None, false);
            Debug.Assert(off == 0);

            // ensure our output is aligned
            int rva = module.methodBodies.Position + off;

            // add fixups for offsets of created method body
            if (tokenFixups != null)
                AddTokenFixups(rva + hsz, module.tokenFixupOffsets, tokenFixups);

            // dump blob builder contents into module method body
            module.methodBodies.Write(bdy);

            return rva;
        }

        /// <summary>
        /// Writes the debug information to the module builder.
        /// </summary>
        /// <param name="method"></param>
        internal void WriteDebugInformation(MethodDefinitionHandle method, out BlobHandle sequencePointsHandle)
        {
            Debug.Assert(scope != null && scope.parent == null);

            // encode the local signature
            var localSignature = MetadataTokens.StandaloneSignatureHandle(0);
            if (localVarLength != 0)
                localSignature = (StandaloneSignatureHandle)MetadataTokens.EntityHandle(module.GetSignatureToken(locals).Token);

            // if we have any debug information
            sequencePointsHandle = default;
            if (sequencePoints.Count != 0)
            {
                var sequencePointsBuf = new BlobBuilder();
                var sequencePointsEnc = new SequencePointEncoder(sequencePointsBuf);

                // encode local signature into sequence points blob
                sequencePointsEnc.LocalSignature(localSignature);

                // encode sequence points blob
                foreach (var sequencePoint in sequencePoints)
                {
                    var docHandle = default(DocumentHandle);
                    if (sequencePoint.Document != null)
                    {
                        docHandle = sequencePoint.Document.Handle;
                        Debug.Assert(docHandle.IsNil == false);
                    }

                    sequencePointsEnc.SequencePoint(docHandle, sequencePoint.Offset, sequencePoint.StartLine, sequencePoint.StartColumn, sequencePoint.EndLine, sequencePoint.EndColumn);
                }

                // add sequence points blob
                sequencePointsHandle = module.Metadata.GetOrAddBlob(sequencePointsBuf);
            }

            // write the root scope and all children
            WriteScope(method, scope);
        }

        internal static void AddTokenFixups(int codeOffset, List<int> tokenFixupOffsets, IEnumerable<int> tokenFixups)
        {
            foreach (int fixup in tokenFixups)
                tokenFixupOffsets.Add(fixup + codeOffset);
        }

        /// <summary>
        /// Writes the debug information for the scope to the module.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="scope"></param>
        void WriteScope(MethodDefinitionHandle method, Scope scope)
        {
            var importScopeHandle = default(ImportScopeHandle);

            // create import scope if we have any namespaces to import
            if (scope.namespaces.Count > 0)
            {
                // encode namespaces into imports
                var importsBuf = new BlobBuilder();
                var importsEnc = new ImportsEncoder(importsBuf);
                foreach (var ns in scope.namespaces)
                    importsEnc.ImportNamespace(module.GetOrAddBlobUTF8(ns));

                // add import scope record
                var importScopeRec = new ImportScopeTable.Record();
                importScopeRec.Parent = scope.parent != null ? scope.parent.handle : default;
                importScopeRec.Imports = module.GetOrAddBlob(importsBuf);
                importScopeHandle = MetadataTokens.ImportScopeHandle(module.ImportScopeTable.AddRecord(importScopeRec));
            }

            foreach (var local in scope.locals)
            {
                // define local variable record
                var localVariableRec = new LocalVariableTable.Record();
                localVariableRec.Attributes = LocalVariableAttributes.None;
                localVariableRec.Index = (ushort)local.LocalIndex;
                localVariableRec.Name = module.GetOrAddString(local.name ?? "");
                var localVariableHandle = MetadataTokens.LocalVariableHandle(module.LocalVariableTable.AddRecord(localVariableRec));

                // default offset is scope
                var startOffset = local.startOffset;
                if (startOffset == 0)
                    startOffset = scope.startOffset;

                // default end offset is scope
                var endOffset = local.endOffset;
                if (endOffset == 0)
                    endOffset = scope.endOffset;

                // each variable gets it's own local scope
                var localScopeRec = new LocalScopeTable.Record();
                localScopeRec.Method = method;
                localScopeRec.ImportScope = importScopeHandle;
                localScopeRec.VariableList = localVariableHandle;
                localScopeRec.StartOffset = startOffset;
                localScopeRec.Length = endOffset - startOffset;
                module.LocalScopeTable.AddRecord(localScopeRec);
            }

            // each scope gets it's own scope, with the imports
            var rec = new LocalScopeTable.Record();
            rec.Method = method;
            rec.ImportScope = importScopeHandle;
            rec.StartOffset = scope.startOffset;
            rec.Length = scope.endOffset - scope.startOffset;
            module.LocalScopeTable.AddRecord(rec);

            // write each child scope
            foreach (var child in scope.children)
                WriteScope(method, child);
        }

    }

}
