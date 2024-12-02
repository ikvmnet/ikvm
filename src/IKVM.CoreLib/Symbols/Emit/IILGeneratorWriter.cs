using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols.Emit
{

    interface IILGeneratorWriter
    {

        readonly record struct LocalBuilderRef(int Index);

        readonly record struct LabelRef(int Index);

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
        void MarkSequencePoint(SourceDocument document, int startLine, int startColumn, int endLine, int endColumn);

        /// <summary>
        /// Declares a local variable of the specified type, optionally pinning the object referred to by the variable.
        /// </summary>
        /// <param name="localType"></param>
        /// <param name="pinned"></param>
        /// <returns></returns>
        LocalBuilderRef DeclareLocal(TypeSymbol localType, bool pinned);

        /// <summary>
        /// Declares a local variable of the specified type.
        /// </summary>
        /// <param name="localType"></param>
        /// <returns></returns>
        LocalBuilderRef DeclareLocal(TypeSymbol localType);

        /// <summary>
        /// Declares a new label.
        /// </summary>
        /// <returns></returns>
        LabelRef DefineLabel();

        /// <summary>
        /// Begins an exception block for a non-filtered exception.
        /// </summary>
        /// <returns></returns>
        LabelRef BeginExceptionBlock();

        /// <summary>
        /// Marks the Microsoft intermediate language (MSIL) stream's current position with the given label.
        /// </summary>
        /// <param name="label"></param>
        void MarkLabel(LabelRef label);

        /// <summary>
        /// Begins an exception block for a filtered exception.
        /// </summary>
        void BeginExceptFilterBlock();

        /// <summary>
        /// Begins a catch block.
        /// </summary>
        /// <param name="exceptionType"></param>
        void BeginCatchBlock(TypeSymbol? exceptionType);

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
        /// <param name="exceptionType"></param>
        void ThrowException(TypeSymbol exceptionType);

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the index of the given local variable.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(OpCodeValue opcode, LocalBuilderRef arg);

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given type.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(OpCodeValue opcode, TypeSymbol arg);

        /// <summary>
        /// Puts the specified instruction and metadata token for the specified field onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(OpCodeValue opcode, FieldSymbol arg);

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given method.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="meth"></param>
        void Emit(OpCodeValue opcode, MethodSymbol arg);

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given string.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="str"></param>
        void Emit(OpCodeValue opcode, string str);

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(OpCodeValue opcode, float arg);

        /// <summary>
        /// Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(OpCodeValue opcode, sbyte arg);

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="labels"></param>
        void Emit(OpCodeValue opcode, ImmutableArray<LabelRef> labels);

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(OpCodeValue opcode, long arg);

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(OpCodeValue opcode, int arg);

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(OpCodeValue opcode, short arg);

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(OpCodeValue opcode, double arg);

        /// <summary>
        /// Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="arg"></param>
        void Emit(OpCodeValue opcode, byte arg);

        /// <summary>
        /// Puts the specified instruction onto the stream of instructions.
        /// </summary>
        /// <param name="opcode"></param>
        void Emit(OpCodeValue opcode);

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="label"></param>
        void Emit(OpCodeValue opcode, LabelRef label);

        /// <summary>
        /// Puts a call or callvirt instruction onto the Microsoft intermediate language (MSIL) stream to call a varargs method.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="methodInfo"></param>
        /// <param name="optionalParameterTypes"></param>
        void EmitCall(OpCodeValue opcode, MethodSymbol methodInfo, ImmutableArray<TypeSymbol> optionalParameterTypes);

        /// <summary>
        /// Puts a Calli instruction onto the Microsoft intermediate language (MSIL) stream, specifying an unmanaged calling convention for the indirect call.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="unmanagedCallConv"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        void EmitCalli(OpCodeValue opcode, System.Runtime.InteropServices.CallingConvention unmanagedCallConv, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes);

        /// <summary>
        /// Puts a Calli instruction onto the Microsoft intermediate language (MSIL) stream, specifying a managed calling convention for the indirect call.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="optionalParameterTypes"></param>
        void EmitCalli(OpCodeValue opcode, System.Reflection.CallingConventions callingConvention, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<TypeSymbol> optionalParameterTypes);

    }

}
