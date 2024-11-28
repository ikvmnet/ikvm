using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit.Writers
{

    /// <summary>
    /// Wraps a <see cref="ILGenerator"/>.
    /// </summary>
    struct ReflectionILGenerationWriter : IKVM.CoreLib.Symbols.Emit.IILGeneratorWriter
    {

        readonly ReflectionSymbolContext _context;
        readonly System.Reflection.Emit.ILGenerator _il;
        readonly bool _resolveComplete;

        int _labelsCount;
        IndexRangeDictionary<System.Reflection.Emit.Label> _labels = new IndexRangeDictionary<System.Reflection.Emit.Label>();

        int _localsCount;
        IndexRangeDictionary<System.Reflection.Emit.LocalBuilder> _locals = new IndexRangeDictionary<System.Reflection.Emit.LocalBuilder>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="il"></param>
        /// <param name="resolveComplete"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionILGenerationWriter(ReflectionSymbolContext context, System.Reflection.Emit.ILGenerator il, bool resolveComplete = false)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _il = il ?? throw new ArgumentNullException(nameof(il));
            _resolveComplete = resolveComplete;
        }

        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(type))]
        Type? ResolveType(TypeSymbol? type)
        {
            if (type is null)
                return null;
            else if (_resolveComplete)
                return _context.ResolveCompleteType(type);
            else
                return _context.ResolveType(type);
        }

        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Type[]? ResolveTypes(ImmutableArray<TypeSymbol> types)
        {
            if (types.IsDefault)
                return null;
            else if (types.IsEmpty)
                return [];
            else if (_resolveComplete)
                return _context.ResolveCompleteTypes(types);
            else
                return _context.ResolveTypes(types);
        }

        /// <summary>
        /// Resolves the specified field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(field))]
        FieldInfo? ResolveField(FieldSymbol? field)
        {
            if (field is null)
                return null;
            else if (_resolveComplete)
                return _context.ResolveCompleteField(field);
            else
                return _context.ResolveField(field);
        }

        /// <summary>
        /// Resolves the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(method))]
        MethodBase? ResolveMethod(MethodSymbol? method)
        {
            if (method is null)
                return null;
            else if (_resolveComplete)
                return _context.ResolveCompleteMethod(method);
            else
                return _context.ResolveMethod(method);
        }

        /// <inheritdoc />
        public void BeginCatchBlock(TypeSymbol? exceptionType)
        {
#if NET6_0
            // NET6 requires an exception
            _il.BeginCatchBlock(ResolveType(exceptionType) ?? typeof(Exception));
#else
            _il.BeginCatchBlock(ResolveType(exceptionType));
#endif
        }

        /// <inheritdoc />
        public void BeginExceptFilterBlock()
        {
            _il.BeginExceptFilterBlock();
        }

        /// <inheritdoc />
        public IKVM.CoreLib.Symbols.Emit.IILGeneratorWriter.LabelRef BeginExceptionBlock()
        {
            var id = _labelsCount++;
            _labels[id] = _il.BeginExceptionBlock();
            return new IKVM.CoreLib.Symbols.Emit.IILGeneratorWriter.LabelRef(id);
        }

        /// <inheritdoc />
        public void BeginFaultBlock()
        {
            _il.BeginFaultBlock();
        }

        /// <inheritdoc />
        public void BeginFinallyBlock()
        {
            _il.BeginFinallyBlock();
        }

        /// <inheritdoc />
        public void BeginScope()
        {
            _il.BeginScope();
        }

        /// <inheritdoc />
        public IKVM.CoreLib.Symbols.Emit.IILGeneratorWriter.LocalBuilderRef DeclareLocal(TypeSymbol localType, bool pinned)
        {
            var id = _localsCount++;
            _locals[id] = _il.DeclareLocal(ResolveType(localType), pinned);
            return new IKVM.CoreLib.Symbols.Emit.IILGeneratorWriter.LocalBuilderRef(id);
        }

        /// <inheritdoc />
        public IKVM.CoreLib.Symbols.Emit.IILGeneratorWriter.LocalBuilderRef DeclareLocal(TypeSymbol localType)
        {
            var id = _localsCount++;
            _locals[id] = _il.DeclareLocal(ResolveType(localType));
            return new IKVM.CoreLib.Symbols.Emit.IILGeneratorWriter.LocalBuilderRef(id);
        }

        /// <inheritdoc />
        public IKVM.CoreLib.Symbols.Emit.IILGeneratorWriter.LabelRef DefineLabel()
        {
            var id = _labelsCount++;
            _labels[id] = _il.DefineLabel();
            return new IKVM.CoreLib.Symbols.Emit.IILGeneratorWriter.LabelRef(id);
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, IKVM.CoreLib.Symbols.Emit.IILGeneratorWriter.LocalBuilderRef arg)
        {
            _il.Emit(opcode.ToOpCode(), _locals[arg.Index] ?? throw new InvalidOperationException());
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, TypeSymbol arg)
        {
            _il.Emit(opcode.ToOpCode(), ResolveType(arg));
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, FieldSymbol arg)
        {
            _il.Emit(opcode.ToOpCode(), ResolveField(arg));
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, MethodSymbol arg)
        {
            var methodBase = ResolveMethod(arg);
            if (methodBase is MethodInfo method)
                _il.Emit(opcode.ToOpCode(), method);
            else if (methodBase is ConstructorInfo ctor)
                _il.Emit(opcode.ToOpCode(), ctor);
            else
                throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, string arg)
        {
            _il.Emit(opcode.ToOpCode(), arg);
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, float arg)
        {
            _il.Emit(opcode.ToOpCode(), arg);
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, sbyte arg)
        {
            _il.Emit(opcode.ToOpCode(), arg);
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, long arg)
        {
            _il.Emit(opcode.ToOpCode(), arg);
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, int arg)
        {
            _il.Emit(opcode.ToOpCode(), arg);
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, short arg)
        {
            _il.Emit(opcode.ToOpCode(), arg);
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, double arg)
        {
            _il.Emit(opcode.ToOpCode(), arg);
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, byte arg)
        {
            _il.Emit(opcode.ToOpCode(), arg);
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode)
        {
            _il.Emit(opcode.ToOpCode());
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, ImmutableArray<IKVM.CoreLib.Symbols.Emit.IILGeneratorWriter.LabelRef> labels)
        {
            var labels_ = new System.Reflection.Emit.Label[labels.Length];
            for (int i = 0; i < labels.Length; i++)
                labels_[i] = _labels[labels[i].Index];

            _il.Emit(opcode.ToOpCode(), labels_);
        }

        /// <inheritdoc />
        public void Emit(OpCodeValue opcode, IKVM.CoreLib.Symbols.Emit.IILGeneratorWriter.LabelRef label)
        {
            _il.Emit(opcode.ToOpCode(), _labels[label.Index]);
        }

        /// <inheritdoc />
        public void EmitCall(OpCodeValue opcode, MethodSymbol method, ImmutableArray<TypeSymbol> optionalParameterTypes)
        {
            _il.EmitCall(opcode.ToOpCode(), ResolveMethod(method) is MethodInfo m ? m : throw new InvalidOperationException(), ResolveTypes(optionalParameterTypes));
        }

        /// <inheritdoc />
        public void EmitCalli(OpCodeValue opcode, CallingConvention unmanagedCallConv, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes)
        {
            _il.EmitCalli(opcode.ToOpCode(), unmanagedCallConv, ResolveType(returnType), ResolveTypes(parameterTypes));
        }

        /// <inheritdoc />
        public void EmitCalli(OpCodeValue opcode, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<TypeSymbol> optionalParameterTypes)
        {
            _il.EmitCalli(opcode.ToOpCode(), callingConvention, ResolveType(returnType), ResolveTypes(parameterTypes), ResolveTypes(optionalParameterTypes));
        }

        /// <inheritdoc />
        public void EndExceptionBlock()
        {
            _il.EndExceptionBlock();
        }

        /// <inheritdoc />
        public void EndScope()
        {
            _il.EndScope();
        }

        /// <inheritdoc />
        public void MarkLabel(IKVM.CoreLib.Symbols.Emit.IILGeneratorWriter.LabelRef label)
        {
            _il.MarkLabel(_labels[label.Index]);
        }

        /// <inheritdoc />
        public void MarkSequencePoint(SourceDocument document, int startLine, int startColumn, int endLine, int endColumn)
        {
#if NETFRAMEWORK
            //_il.MarkSequencePoint(document, startLine, startColumn, endLine, endColumn);
#endif
        }

        /// <inheritdoc />
        public void ThrowException(TypeSymbol exceptionType)
        {
            _il.ThrowException(_context.ResolveType(exceptionType));
        }

        /// <inheritdoc />
        public void UsingNamespace(string usingNamespace)
        {
            _il.UsingNamespace(usingNamespace);
        }

    }

}
