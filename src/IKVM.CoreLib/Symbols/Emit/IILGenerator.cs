namespace IKVM.CoreLib.Symbols.Emit
{

    interface IILGenerator
    {

        /// <summary>
        /// Gets the current offset, in bytes, in the Microsoft intermediate language (MSIL) stream that is being emitted by the <see cref="IILGenerator"/>.
        /// </summary>
        int ILOffset { get; }

        /// <summary>
        /// Begins a lexical scope.
        /// </summary>
        void BeginScope();

        /// <summary>
        /// Specifies the namespace to be used in evaluating locals and watches for the current active lexical scope.
        /// </summary>
        /// <param name="usingNamespace"></param>
        void UsingNamespace(string usingNamespace);

        /// <summary>
        /// Ends a lexical scope.
        /// </summary>
        void EndScope();

        /// <summary>
        /// Marks a sequence point in the Microsoft intermediate language (MSIL) stream.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="startLine"></param>
        /// <param name="startColumn"></param>
        /// <param name="endLine"></param>
        /// <param name="endColumn"></param>
        void MarkSequencePoint(System.Diagnostics.SymbolStore.ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn);

        /// <summary>
        /// Declares a local variable of the specified type, optionally pinning the object referred to by the variable.
        /// </summary>
        /// <param name="localType"></param>
        /// <param name="pinned"></param>
        /// <returns></returns>
        ILocalBuilder DeclareLocal(ITypeSymbol localType, bool pinned);

        /// <summary>
        /// Declares a local variable of the specified type.
        /// </summary>
        /// <param name="localType"></param>
        /// <returns></returns>
        ILocalBuilder DeclareLocal(ITypeSymbol localType);

        /// <summary>
        /// Declares a new label.
        /// </summary>
        /// <returns></returns>
        System.Reflection.Emit.Label DefineLabel();

        /// <summary>
        /// Begins an exception block for a non-filtered exception.
        /// </summary>
        /// <returns></returns>
        System.Reflection.Emit.Label BeginExceptionBlock();

        /// <summary>
        /// Marks the Microsoft intermediate language (MSIL) stream's current position with the given label.
        /// </summary>
        /// <param name="loc"></param>
        void MarkLabel(System.Reflection.Emit.Label loc);

        /// <summary>
        /// Begins an exception block for a filtered exception.
        /// </summary>
        void BeginExceptFilterBlock();

        /// <summary>
        /// Begins a catch block.
        /// </summary>
        /// <param name="exceptionType"></param>
        void BeginCatchBlock(ITypeSymbol? exceptionType);

        /// <summary>
        /// Begins an exception fault block in the Microsoft intermediate language (MSIL) stream.
        /// </summary>
        void BeginFaultBlock();

        /// <summary>
        /// Begins a finally block in the Microsoft intermediate language (MSIL) instruction stream.
        /// </summary>
        void BeginFinallyBlock();

        /// <summary>
        /// Ends an exception block.
        /// </summary>
        void EndExceptionBlock();

        /// <summary>
        /// Emits an instruction to throw an exception.
        /// </summary>
        /// <param name="excType"></param>
        void ThrowException(ITypeSymbol excType);

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the index of the given local variable.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="local"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, ILocalBuilder local);

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given type.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="cls"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, ITypeSymbol cls);

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given string.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="str"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, string str);

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, float arg);

        /// <summary>
        /// Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, sbyte arg);

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given method.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="meth"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, IMethodSymbol meth);

        /// <summary>
        /// Puts the specified instruction and a signature token onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="signature"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, ISignatureHelper signature);

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="labels"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, ILabel[] labels);

        /// <summary>
        /// Puts the specified instruction and metadata token for the specified field onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="field"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, IFieldSymbol field);

        /// <summary>
        /// Puts the specified instruction and metadata token for the specified constructor onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="con"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, IConstructorSymbol con);

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, long arg);

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, int arg);

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, short arg);

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, double arg);

        /// <summary>
        /// Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, byte arg);

        /// <summary>
        /// Puts the specified instruction onto the stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        void Emit(System.Reflection.Emit.OpCode opcode);

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="label"></param>
        void Emit(System.Reflection.Emit.OpCode opcode, ILabel label);

        /// <summary>
        /// Puts a call or callvirt instruction onto the Microsoft intermediate language (MSIL) stream to call a varargs method.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="methodInfo"></param>
        /// <param name="optionalParameterTypes"></param>
        void EmitCall(System.Reflection.Emit.OpCode opcode, IMethodSymbol methodInfo, ITypeSymbol[]? optionalParameterTypes);

        /// <summary>
        /// Puts a Calli instruction onto the Microsoft intermediate language (MSIL) stream, specifying an unmanaged calling convention for the indirect call.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="unmanagedCallConv"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        void EmitCalli(System.Reflection.Emit.OpCode opcode, System.Runtime.InteropServices.CallingConvention unmanagedCallConv, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes);

        /// <summary>
        /// Puts a Calli instruction onto the Microsoft intermediate language (MSIL) stream, specifying a managed calling convention for the indirect call.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="optionalParameterTypes"></param>
        void EmitCalli(System.Reflection.Emit.OpCode opcode, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes, ITypeSymbol[]? optionalParameterTypes);

        /// <summary>
        /// Emits the Microsoft intermediate language (MSIL) to call <see cref="System.Console.WriteLine"/> with a string.
        /// </summary>
        /// <param name="value"></param>
        void EmitWriteLine(string value);

        /// <summary>
        /// Emits the Microsoft intermediate language (MSIL) necessary to call <see cref="System.Console.WriteLine"/> with the given field.
        /// </summary>
        /// <param name="fld"></param>
        void EmitWriteLine(IFieldSymbol fld);

        /// <summary>
        /// Emits the Microsoft intermediate language (MSIL) necessary to call <see cref="System.Console.WriteLine"/> with the given local variable.
        /// </summary>
        /// <param name="localBuilder"></param>
        void EmitWriteLine(ILocalBuilder localBuilder);

    }

}