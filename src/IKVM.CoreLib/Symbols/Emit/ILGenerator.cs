using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

using IKVM.CoreLib.Collections;

namespace IKVM.CoreLib.Symbols.Emit
{

    public class ILGenerator
    {

        const int BlockSize = 64;

        /// <summary>
        /// Decscribes the IL node.
        /// </summary>
        enum NodeKind : byte
        {

            OpCode,
            OpCode_Local,
            OpCode_Type,
            OpCode_Method,
            OpCode_Field,
            OpCode_Float,
            OpCode_String,
            OpCode_SByte,
            OpCode_Byte,
            OpCode_Short,
            OpCode_Double,
            OpCode_Integer,
            OpCode_Long,
            OpCode_Label,
            OpCode_ManyLabel,
            Call,
            Calli,
            CalliManaged,
            BeginScope,
            EndScope,
            UsingNamespace,
            DeclareLocal,
            SequencePoint,
            Label,
            BeginExceptionBlock,
            BeginCatchBlock,
            BeginFaultBlock,
            BeginFinallyBlock,
            BeginFilterBlock,
            EndExceptionBlock,

        }

        struct Node
        {

            public NodeKind Kind;
            public OpCodeValue OpCode;
            public object? ObjectArg;
            public long NumberArg0;
            public long NumberArg1;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public Node(NodeKind kind, OpCodeValue opcode, object? objectArg, long numberArg0, long numberArg1)
            {
                Kind = kind;
                OpCode = opcode;
                ObjectArg = objectArg;
                NumberArg0 = numberArg0;
                NumberArg1 = numberArg1;
            }

        }

#if NET8_0_OR_GREATER

        [System.Runtime.CompilerServices.InlineArray(BlockSize)]
        struct ILInlineNodeArray
        {

            public Node Item;

        }

#endif

        /// <summary>
        /// IL stream is a series of double-linked blocks. Head always points to the first in the sequence.
        /// </summary>
        class ILBlock
        {

            public ILBlock Head;
            public ILBlock Prev;
            public ILBlock Next;
#if NET8_0_OR_GREATER
            public ILInlineNodeArray Data;
#else
            public Node[] Data;
#endif
            public int Size = 0;

            /// <summary>
            /// Initializes a head block.
            /// </summary>
            public ILBlock()
            {
                Head = this;
                Prev = this;
                Next = this;
#if NET8_0_OR_GREATER
                Data = new ILInlineNodeArray();
#else
                Data = new Node[BlockSize];
#endif
            }

            /// <summary>
            /// Initializes a new tail block.
            /// </summary>
            /// <param name="prev"></param>
            public ILBlock(ILBlock prev) : this()
            {
                Head = prev.Head;
                Prev = prev;
                Prev.Next = this;
            }

        }

        /// <summary>
        /// Describes an IL stream, which is a pointer to the latest tail block, and to which <see cref="Node"/>s can be appended.
        /// </summary>
        struct NodeStream : IEnumerable<Node>
        {

            ILBlock Tail;
            int Size;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public NodeStream()
            {
                Tail = new ILBlock();
                Size = 0;
            }

            /// <summary>
            /// Appends a new node to the stream.
            /// </summary>
            /// <param name="node"></param>
            public void Append(Node node)
            {
                // append a new tail block if required
                if (Tail.Size >= BlockSize)
                    Tail = new ILBlock(Tail);

                // append onto existing tail
                Tail.Data[Tail.Size++] = node;
                Size++;
            }

            /// <inheritdoc />
            public IEnumerator<Node> GetEnumerator()
            {
                for (var node = Tail.Head; node != node.Next; node = node.Next)
                    for (int i = 0; i < node.Size; i++)
                        yield return node.Data[i];
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

        }

        record class CallNode(MethodSymbol Method, ImmutableArray<TypeSymbol> OptionalParameterTypes);

        record class CalliNode(CallingConvention UnmanagedCallConv, TypeSymbol? ReturnType, ImmutableArray<TypeSymbol> ParameterTypes);

        record class ManagedCalliNode(CallingConventions CallConv, TypeSymbol? ReturnType, ImmutableArray<TypeSymbol> ParameterTypes, ImmutableArray<TypeSymbol> OptionalParameterTypes);

        readonly SymbolContext _context;
        NodeStream _stream;
        int _iloffset;
        int _labelIndex;
        int _localIndex;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public ILGenerator(SymbolContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _stream = new NodeStream();
        }

        /// <summary>
        /// Gets the current offset, in bytes, in the Microsoft intermediate language (MSIL) stream that is being emitted by the <see cref="ILGenerator"/>.
        /// </summary>
        public int ILOffset => _iloffset;

        /// <summary>
        /// Begins a lexical scope.
        /// </summary>
        public void BeginScope()
        {
            _stream.Append(new Node(NodeKind.BeginScope, 0, null, 0, 0));
        }

        /// <summary>
        /// Specifies the namespace to be used in evaluating locals and watches for the current active lexical scope.
        /// </summary>
        /// <param name="usingNamespace"></param>
        public void UsingNamespace(string usingNamespace)
        {
            _stream.Append(new Node(NodeKind.UsingNamespace, 0, usingNamespace, 0, 0));
        }

        /// <summary>
        /// Ends a lexical scope.
        /// </summary>
        public void EndScope()
        {
            _stream.Append(new Node(NodeKind.EndScope, 0, null, 0, 0));
        }

        /// <summary>
        /// Marks a sequence point in the Microsoft intermediate language (MSIL) stream.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="startLine"></param>
        /// <param name="startColumn"></param>
        /// <param name="endLine"></param>
        /// <param name="endColumn"></param>
        public void MarkSequencePoint(SourceDocument document, int startLine, int startColumn, int endLine, int endColumn)
        {
            if (startLine < 1)
                throw new ArgumentOutOfRangeException(nameof(startLine));
            if (endLine < 1)
                throw new ArgumentOutOfRangeException(nameof(endLine));

            _stream.Append(new Node(NodeKind.SequencePoint, 0, document, startLine << 32 | startColumn, endLine << 32 | endColumn));
        }

        /// <summary>
        /// Declares a local variable of the specified type, optionally pinning the object referred to by the variable.
        /// </summary>
        /// <param name="localType"></param>
        /// <param name="pinned"></param>
        /// <returns></returns>
        public LocalBuilder DeclareLocal(TypeSymbol localType, bool pinned)
        {
            var b = new LocalBuilder(localType, pinned, _localIndex++);
            _stream.Append(new Node(NodeKind.DeclareLocal, 0, b, pinned ? 1 : 0, 0));
            return b;
        }

        /// <summary>
        /// Declares a local variable of the specified type.
        /// </summary>
        /// <param name="localType"></param>
        /// <returns></returns>
        public LocalBuilder DeclareLocal(TypeSymbol localType)
        {
            var b = new LocalBuilder(localType, false, _localIndex++);
            _stream.Append(new Node(NodeKind.DeclareLocal, 0, b, 0, 0));
            return b;
        }

        /// <summary>
        /// Declares a new label.
        /// </summary>
        /// <returns></returns>
        public Label DefineLabel()
        {
            return new Label(_labelIndex++);
        }

        /// <summary>
        /// Begins an exception block for a non-filtered exception.
        /// </summary>
        /// <returns></returns>
        public Label BeginExceptionBlock()
        {
            var l = new Label(_labelIndex++);
            _stream.Append(new Node(NodeKind.BeginExceptionBlock, 0, null, l.Index, 0));
            return l;
        }

        /// <summary>
        /// Marks the Microsoft intermediate language (MSIL) stream's current position with the given label.
        /// </summary>
        /// <param name="loc"></param>
        public void MarkLabel(Label loc)
        {
            _stream.Append(new Node(NodeKind.Label, 0, null, loc.Index, 0));
        }

        /// <summary>
        /// Begins an exception block for a filtered exception.
        /// </summary>
        public void BeginExceptFilterBlock()
        {
            _stream.Append(new Node(NodeKind.BeginFilterBlock, 0, null, 0, 0));
        }

        /// <summary>
        /// Begins a catch block.
        /// </summary>
        /// <param name="exceptionType"></param>
        public void BeginCatchBlock(TypeSymbol? exceptionType)
        {
            _stream.Append(new Node(NodeKind.BeginCatchBlock, 0, exceptionType, 0, 0));
        }

        /// <summary>
        /// Begins an exception fault block in the Microsoft intermediate language (MSIL) stream.
        /// </summary>
        public void BeginFaultBlock()
        {
            _stream.Append(new Node(NodeKind.BeginFaultBlock, 0, null, 0, 0));
        }

        /// <summary>
        /// Begins a finally block in the Microsoft intermediate language (MSIL) instruction stream.
        /// </summary>
        public void BeginFinallyBlock()
        {
            _stream.Append(new Node(NodeKind.BeginFinallyBlock, 0, null, 0, 0));
        }

        /// <summary>
        /// Ends an exception block.
        /// </summary>
        public void EndExceptionBlock()
        {
            _stream.Append(new Node(NodeKind.EndExceptionBlock, 0, null, 0, 0));
        }

        /// <summary>
        /// Emits an instruction to throw an exception.
        /// </summary>
        /// <param name="exceptionType"></param>
        public void ThrowException(TypeSymbol exceptionType)
        {
            if (exceptionType is null)
                throw new ArgumentNullException(nameof(exceptionType));

            var exceptionTypeSymbol = _context.ResolveCoreType("System.Exception");
            if (exceptionType.IsSubclassOf(exceptionTypeSymbol) == false && exceptionType != exceptionTypeSymbol)
                throw new ArgumentException("Not exception type.");

            var con = exceptionType.GetConstructor([]);
            if (con == null)
                throw new ArgumentException("No default constructor.");

            Emit(OpCodes.Newobj, con);
            Emit(OpCodes.Throw);
        }

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the index of the given local variable.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="local"></param>
        public void Emit(OpCode opcode, LocalBuilder local)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_Local, (OpCodeValue)opcode.Value, local, 0, 0));
        }

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given type.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="cls"></param>
        public void Emit(OpCode opcode, TypeSymbol cls)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_Type, (OpCodeValue)opcode.Value, cls, 0, 0));
        }

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given string.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="str"></param>
        public void Emit(OpCode opcode, string str)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_String, (OpCodeValue)opcode.Value, str, 0, 0));
        }

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        public unsafe void Emit(OpCode opcode, float arg)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_Float, (OpCodeValue)opcode.Value, null, *(int*)&arg, 0));
        }

        /// <summary>
        /// Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        public void Emit(OpCode opcode, sbyte arg)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_SByte, (OpCodeValue)opcode.Value, null, arg, 0));
        }

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given method.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="method"></param>
        public void Emit(OpCode opcode, MethodSymbol method)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_Method, (OpCodeValue)opcode.Value, method, 0, 0));
        }

        /// <summary>
        /// Puts the specified instruction and a signature token onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="signature"></param>
        public void Emit(OpCode opcode, SignatureHelper signature)
        {
            _iloffset += opcode.Size;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="labels"></param>
        public void Emit(OpCode opcode, ImmutableArray<Label> labels)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_ManyLabel, (OpCodeValue)opcode.Value, labels, 0, 0));
        }

        /// <summary>
        /// Puts the specified instruction and metadata token for the specified field onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="field"></param>
        public void Emit(OpCode opcode, FieldSymbol field)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_Field, (OpCodeValue)opcode.Value, field, 0, 0));
        }

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        public void Emit(OpCode opcode, long arg)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_Long, (OpCodeValue)opcode.Value, null, arg, 0));
        }

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        public void Emit(OpCode opcode, int arg)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_Integer, (OpCodeValue)opcode.Value, null, arg, 0));
        }

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        public void Emit(OpCode opcode, short arg)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_Short, (OpCodeValue)opcode.Value, null, arg, 0));
        }

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        public unsafe void Emit(OpCode opcode, double arg)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_Double, (OpCodeValue)opcode.Value, null, *(long*)&arg, 0));
        }

        /// <summary>
        /// Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        public void Emit(OpCode opcode, byte arg)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_Byte, (OpCodeValue)opcode.Value, null, arg, 0));
        }

        /// <summary>
        /// Puts the specified instruction onto the stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        public void Emit(OpCode opcode)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode, (OpCodeValue)opcode.Value, null, 0, 0));
        }

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="label"></param>
        public void Emit(OpCode opcode, Label label)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.OpCode_Label, (OpCodeValue)opcode.Value, null, label.Index, 0));
        }

        /// <summary>
        /// Puts a call or callvirt instruction onto the Microsoft intermediate language (MSIL) stream to call a varargs method.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="methodInfo"></param>
        /// <param name="optionalParameterTypes"></param>
        public void EmitCall(OpCode opcode, MethodSymbol methodInfo, ImmutableArray<TypeSymbol> optionalParameterTypes)
        {
            _iloffset += opcode.Size;
            _stream.Append(new Node(NodeKind.Call, (OpCodeValue)opcode.Value, new CallNode(methodInfo, optionalParameterTypes), 0, 0));
        }

        /// <summary>
        /// Puts a Calli instruction onto the Microsoft intermediate language (MSIL) stream, specifying an unmanaged calling convention for the indirect call.
        /// </summary>
        /// <param name="unmanagedCallConv"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        public void EmitCalli(CallingConvention unmanagedCallConv, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes)
        {
            _iloffset += OpCodes.Calli.Size;
            _stream.Append(new Node(NodeKind.Calli, OpCodeValue.Calli, new CalliNode(unmanagedCallConv, returnType, parameterTypes), 0, 0));
        }

        /// <summary>
        /// Puts a Calli instruction onto the Microsoft intermediate language (MSIL) stream, specifying a managed calling convention for the indirect call.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="optionalParameterTypes"></param>
        public void EmitCalli(CallingConventions callingConvention, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<TypeSymbol> optionalParameterTypes)
        {
            _iloffset += OpCodes.Calli.Size;
            _stream.Append(new Node(NodeKind.CalliManaged, OpCodeValue.Calli, new ManagedCalliNode(callingConvention, returnType, parameterTypes, optionalParameterTypes), 0, 0));
        }

        /// <summary>
        /// Writes the IL to the specified writer.
        /// </summary>
        /// <typeparam name="TWriter"></typeparam>
        /// <param name="writer"></param>
        internal void Write<TWriter>(TWriter writer)
            where TWriter : IILGeneratorWriter
        {
            var state = new WriteState();
            foreach (var node in _stream)
                WriteNode(writer, node, ref state);
        }

        struct WriteState
        {

            public IndexRangeDictionary<IILGeneratorWriter.LocalBuilderRef> Locals = new();
            public IndexRangeDictionary<IILGeneratorWriter.LabelRef> Labels = new();

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public WriteState()
            {

            }

        }

        /// <summary>
        /// Writes the IL for the given node to the specified writer.
        /// </summary>
        /// <typeparam name="TWriter"></typeparam>
        /// <param name="writer"></param>
        /// <param name="node"></param>
        /// <param name="state"></param>
        /// <exception cref="NotImplementedException"></exception>
        unsafe void WriteNode<TWriter>(TWriter writer, Node node, ref WriteState state)
            where TWriter : IILGeneratorWriter
        {
            switch (node.Kind)
            {
                case NodeKind.OpCode:
                    writer.Emit(node.OpCode);
                    break;
                case NodeKind.OpCode_Local:
                    {
                        var localBuilder = (LocalBuilder)(node.ObjectArg ?? throw new InvalidOperationException());
                        writer.Emit(node.OpCode, state.Locals[localBuilder.LocalIndex]);
                        break;
                    }
                case NodeKind.OpCode_Type:
                    writer.Emit(node.OpCode, (TypeSymbol)(node.ObjectArg ?? throw new InvalidOperationException()));
                    break;
                case NodeKind.OpCode_Method:
                    writer.Emit(node.OpCode, (MethodSymbol)(node.ObjectArg ?? throw new InvalidOperationException()));
                    break;
                case NodeKind.OpCode_Field:
                    writer.Emit(node.OpCode, (FieldSymbol)(node.ObjectArg ?? throw new InvalidOperationException()));
                    break;
                case NodeKind.OpCode_Float:
                    var i = (int)node.NumberArg0;
                    writer.Emit(node.OpCode, *(float*)&i);
                    break;
                case NodeKind.OpCode_String:
                    writer.Emit(node.OpCode, (string)(node.ObjectArg ?? throw new InvalidOperationException()));
                    break;
                case NodeKind.OpCode_SByte:
                    writer.Emit(node.OpCode, (sbyte)node.NumberArg0);
                    break;
                case NodeKind.OpCode_Byte:
                    writer.Emit(node.OpCode, (byte)node.NumberArg0);
                    break;
                case NodeKind.OpCode_Short:
                    writer.Emit(node.OpCode, (short)node.NumberArg0);
                    break;
                case NodeKind.OpCode_Double:
                    writer.Emit(node.OpCode, *(double*)&node.NumberArg0);
                    break;
                case NodeKind.OpCode_Integer:
                    writer.Emit(node.OpCode, (int)node.NumberArg0);
                    break;
                case NodeKind.OpCode_Long:
                    writer.Emit(node.OpCode, node.NumberArg0);
                    break;
                case NodeKind.OpCode_Label:
                    {
                        var labelRef = state.Labels[(int)node.NumberArg0];
                        writer.Emit(node.OpCode, labelRef);
                        break;
                    }
                case NodeKind.OpCode_ManyLabel:
                    {
                        var manyLabel = (ImmutableArray<Label>)(node.ObjectArg ?? throw new InvalidOperationException());
                        var manyLabelRefs = ImmutableArray.CreateBuilder<IILGeneratorWriter.LabelRef>(manyLabel.Length);
                        for (int id = 0; id < manyLabel.Length; id++)
                            manyLabelRefs[id] = state.Labels[manyLabel[id].Index];

                        writer.Emit(node.OpCode, manyLabelRefs.ToImmutable());
                        break;
                    }
                case NodeKind.Call:
                    {
                        var call = (CallNode)(node.ObjectArg ?? throw new InvalidOperationException());
                        writer.EmitCall(node.OpCode, call.Method, call.OptionalParameterTypes);
                        break;
                    }
                case NodeKind.Calli:
                    {
                        var calli = (CalliNode)(node.ObjectArg ?? throw new InvalidOperationException());
                        writer.EmitCalli(node.OpCode, calli.UnmanagedCallConv, calli.ReturnType, calli.ParameterTypes);
                        break;
                    }
                case NodeKind.CalliManaged:
                    {
                        var managedCalli = (ManagedCalliNode)(node.ObjectArg ?? throw new InvalidOperationException());
                        writer.EmitCalli(node.OpCode, managedCalli.CallConv, managedCalli.ReturnType, managedCalli.ParameterTypes, managedCalli.OptionalParameterTypes);
                        break;
                    }
                case NodeKind.BeginScope:
                    writer.BeginScope();
                    break;
                case NodeKind.EndScope:
                    writer.EndScope();
                    break;
                case NodeKind.UsingNamespace:
                    writer.UsingNamespace((string)(node.ObjectArg ?? throw new InvalidOperationException()));
                    break;
                case NodeKind.DeclareLocal:
                    {
                        var localBuilder = (LocalBuilder)(node.ObjectArg ?? throw new InvalidOperationException());
                        var localBuilderRef = writer.DeclareLocal(localBuilder.LocalType, localBuilder.IsPinned);
                        state.Locals[localBuilder.LocalIndex] = localBuilderRef;
                        break;
                    }
                case NodeKind.SequencePoint:
                    writer.MarkSequencePoint(
                        (SourceDocument)(node.ObjectArg ?? throw new InvalidOperationException()),
                        (int)(node.NumberArg0 >> 32),
                        (int)(node.NumberArg0 & 0x00000000FFFFFFFF),
                        (int)(node.NumberArg1 >> 32),
                        (int)(node.NumberArg1 & 0x00000000FFFFFFFF));
                    break;
                case NodeKind.Label:
                    {
                        var labelRef = state.Labels[(int)node.NumberArg0];
                        writer.MarkLabel(labelRef);
                        break;
                    }
                case NodeKind.BeginExceptionBlock:
                    writer.BeginExceptionBlock();
                    break;
                case NodeKind.BeginCatchBlock:
                    writer.BeginCatchBlock((TypeSymbol?)node.ObjectArg);
                    break;
                case NodeKind.BeginFaultBlock:
                    writer.BeginFaultBlock();
                    break;
                case NodeKind.BeginFinallyBlock:
                    writer.BeginFinallyBlock();
                    break;
                case NodeKind.BeginFilterBlock:
                    writer.BeginExceptFilterBlock();
                    break;
                case NodeKind.EndExceptionBlock:
                    writer.EndExceptionBlock();
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

    }

}