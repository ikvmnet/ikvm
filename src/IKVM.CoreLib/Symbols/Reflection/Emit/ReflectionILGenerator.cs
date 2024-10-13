using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionILGenerator : IILGenerator
    {

        readonly ReflectionSymbolContext _context;
        readonly ILGenerator _il;
        readonly bool _dynamic = false;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="il"></param>
        /// <param name="dynamic"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionILGenerator(ReflectionSymbolContext context, ILGenerator il, bool dynamic = false)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _il = il ?? throw new ArgumentNullException(nameof(il));
            _dynamic = dynamic;
        }

        /// <inheritdoc />
        public int ILOffset => _il.ILOffset;

        /// <summary>
        /// Gets the underlying emit type for the given <see cref="ITypeSymbol"/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(symbol))]
        Type? GetEmitType(ITypeSymbol? symbol)
        {
            if (_dynamic)
                return ((IReflectionTypeSymbol?)symbol)?.UnderlyingRuntimeType;
            else
                return ((IReflectionTypeSymbol?)symbol)?.UnderlyingType;
        }

        /// <summary>
        /// Gets the underlying emit type for the given <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(symbol))]
        FieldInfo? GetEmitField(IFieldSymbol? symbol)
        {
            if (_dynamic)
                return ((IReflectionFieldSymbol?)symbol)?.UnderlyingRuntimeField;
            else
                return ((IReflectionFieldSymbol?)symbol)?.UnderlyingField;
        }

        /// <summary>
        /// Gets the underlying emit type for the given <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(symbol))]
        ConstructorInfo? GetEmitConstructor(IConstructorSymbol? symbol)
        {
            if (_dynamic)
                return ((IReflectionConstructorSymbol?)symbol)?.UnderlyingRuntimeConstructor;
            else
                return ((IReflectionConstructorSymbol?)symbol)?.UnderlyingConstructor;
        }

        /// <summary>
        /// Gets the underlying emit type for the given <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(symbol))]
        MethodInfo? GetEmitMethod(IMethodSymbol? symbol)
        {
            if (_dynamic)
                return ((IReflectionMethodSymbol?)symbol)?.UnderlyingRuntimeMethod;
            else
                return ((IReflectionMethodSymbol?)symbol)?.UnderlyingMethod;
        }

        /// <inheritdoc />
        public void BeginCatchBlock(ITypeSymbol? exceptionType)
        {
            _il.BeginCatchBlock(GetEmitType(exceptionType)!);
        }

        /// <inheritdoc />
        public void BeginExceptFilterBlock()
        {
            _il.BeginExceptFilterBlock();
        }

        /// <inheritdoc />
        public ILabel BeginExceptionBlock()
        {
            return new ReflectionLabel(_il.BeginExceptionBlock());
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
        public ILocalBuilder DeclareLocal(ITypeSymbol localType, bool pinned)
        {
            return new ReflectionLocalBuilder(_context, _il.DeclareLocal(GetEmitType(localType), pinned));
        }

        /// <inheritdoc />
        public ILocalBuilder DeclareLocal(ITypeSymbol localType)
        {
            return new ReflectionLocalBuilder(_context, _il.DeclareLocal(GetEmitType(localType)));
        }

        /// <inheritdoc />
        public ILabel DefineLabel()
        {
            return new ReflectionLabel(_il.DefineLabel());
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, ITypeSymbol cls)
        {
            _il.Emit(opcode, GetEmitType(cls));
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, IFieldSymbol field)
        {
            _il.Emit(opcode, GetEmitField(field));
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, IConstructorSymbol con)
        {
            _il.Emit(opcode, GetEmitConstructor(con));
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, IMethodSymbol method)
        {
            _il.Emit(opcode, GetEmitMethod(method));
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, ISignatureHelper signature)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, ILabel[] labels)
        {
            _il.Emit(opcode, labels.Unpack());
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, ILocalBuilder local)
        {
            _il.Emit(opcode, local.Unpack());
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, string str)
        {
            _il.Emit(opcode, str);
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, float arg)
        {
            _il.Emit(opcode, arg);
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, sbyte arg)
        {
            _il.Emit(opcode, arg);
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, long arg)
        {
            _il.Emit(opcode, arg);
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, int arg)
        {
            _il.Emit(opcode, arg);
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, short arg)
        {
            _il.Emit(opcode, arg);
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, double arg)
        {
            _il.Emit(opcode, arg);
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, byte arg)
        {
            _il.Emit(opcode, arg);
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode)
        {
            _il.Emit(opcode);
        }

        /// <inheritdoc />
        public void Emit(OpCode opcode, ILabel label)
        {
            _il.Emit(opcode, label.Unpack());
        }

        /// <inheritdoc />
        public void EmitCall(OpCode opcode, IMethodSymbol methodInfo, ITypeSymbol[]? optionalParameterTypes)
        {
            _il.EmitCall(opcode, methodInfo.Unpack(), optionalParameterTypes?.Unpack());
        }

        /// <inheritdoc />
        public void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes)
        {
            _il.EmitCalli(opcode, unmanagedCallConv, returnType?.Unpack(), parameterTypes?.Unpack());
        }

        /// <inheritdoc />
        public void EmitCalli(OpCode opcode, CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes, ITypeSymbol[]? optionalParameterTypes)
        {
            _il.EmitCalli(opcode, callingConvention, returnType?.Unpack(), parameterTypes?.Unpack(), optionalParameterTypes?.Unpack());
        }

        /// <inheritdoc />
        public void EmitWriteLine(string value)
        {
            _il.EmitWriteLine(value);
        }

        /// <inheritdoc />
        public void EmitWriteLine(IFieldSymbol fld)
        {
            _il.EmitWriteLine(fld.Unpack());
        }

        /// <inheritdoc />
        public void EmitWriteLine(ILocalBuilder localBuilder)
        {
            _il.EmitWriteLine(localBuilder.Unpack());
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
        public void MarkLabel(ILabel loc)
        {
            _il.MarkLabel(loc.Unpack());
        }

        /// <inheritdoc />
        public void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
        {
#if NETFRAMEWORK
            _il.MarkSequencePoint(document, startLine, startColumn, endLine, endColumn);
#endif
        }

        /// <inheritdoc />
        public void ThrowException(ITypeSymbol excType)
        {
            _il.ThrowException(excType.Unpack());
        }

        /// <inheritdoc />
        public void UsingNamespace(string usingNamespace)
        {
            _il.UsingNamespace(usingNamespace);
        }

    }

}
