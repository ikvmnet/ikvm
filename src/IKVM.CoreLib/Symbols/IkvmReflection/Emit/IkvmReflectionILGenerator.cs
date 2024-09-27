using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionILGenerator : IILGenerator
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly ILGenerator _il;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="il"></param>
        public IkvmReflectionILGenerator(IkvmReflectionSymbolContext context, ILGenerator il)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _il = il ?? throw new ArgumentNullException(nameof(il));
        }

        /// <inheritdoc />
        public int ILOffset => _il.ILOffset;

        /// <inheritdoc />
        public void BeginCatchBlock(ITypeSymbol? exceptionType)
        {
            _il.BeginCatchBlock(exceptionType?.Unpack());
        }

        /// <inheritdoc />
        public void BeginExceptFilterBlock()
        {
            _il.BeginExceptFilterBlock();
        }

        /// <inheritdoc />
        public ILabel BeginExceptionBlock()
        {
            return new IkvmReflectionLabel(_il.BeginExceptionBlock());
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
            return new IkvmReflectionLocalBuilder(_context, _il.DeclareLocal(localType.Unpack(), pinned));
        }

        /// <inheritdoc />
        public ILocalBuilder DeclareLocal(ITypeSymbol localType)
        {
            return new IkvmReflectionLocalBuilder(_context, _il.DeclareLocal(localType.Unpack()));
        }

        /// <inheritdoc />
        public ILabel DefineLabel()
        {
            return new IkvmReflectionLabel(_il.DefineLabel());
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, ILocalBuilder local)
        {
            _il.Emit(Convert(opcode), local.Unpack());
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, ITypeSymbol cls)
        {
            _il.Emit(Convert(opcode), cls.Unpack());
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, string str)
        {
            _il.Emit(Convert(opcode), str);
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, float arg)
        {
            _il.Emit(Convert(opcode), arg);
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, sbyte arg)
        {
            _il.Emit(Convert(opcode), arg);
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, IMethodSymbol meth)
        {
            _il.Emit(Convert(opcode), meth.Unpack());
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, ISignatureHelper signature)
        {
            throw new NotImplementedException();
            //_il.Emit(Convert(opcode), signature.Unpack());
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, ILabel[] labels)
        {
            _il.Emit(Convert(opcode), labels.Unpack());
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, IFieldSymbol field)
        {
            _il.Emit(Convert(opcode), field.Unpack());
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, IConstructorSymbol con)
        {
            _il.Emit(Convert(opcode), con.Unpack());
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, long arg)
        {
            _il.Emit(Convert(opcode), arg);
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, int arg)
        {
            _il.Emit(Convert(opcode), arg);
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, short arg)
        {
            _il.Emit(Convert(opcode), arg);
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, double arg)
        {
            _il.Emit(Convert(opcode), arg);
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, byte arg)
        {
            _il.Emit(Convert(opcode), arg);
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode)
        {
            _il.Emit(Convert(opcode));
        }

        /// <inheritdoc />
        public void Emit(System.Reflection.Emit.OpCode opcode, ILabel label)
        {
            _il.Emit(Convert(opcode), label.Unpack());
        }

        /// <inheritdoc />
        public void EmitCall(System.Reflection.Emit.OpCode opcode, IMethodSymbol methodInfo, ITypeSymbol[]? optionalParameterTypes)
        {
            _il.EmitCall(Convert(opcode), methodInfo.Unpack(), optionalParameterTypes?.Unpack());
        }

        /// <inheritdoc />
        public void EmitCalli(System.Reflection.Emit.OpCode opcode, CallingConvention unmanagedCallConv, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes)
        {
            _il.EmitCalli(Convert(opcode), unmanagedCallConv, returnType?.Unpack(), parameterTypes?.Unpack());
        }

        /// <inheritdoc />
        public void EmitCalli(System.Reflection.Emit.OpCode opcode, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes, ITypeSymbol[]? optionalParameterTypes)
        {
            _il.EmitCalli(Convert(opcode), (CallingConventions)callingConvention, returnType?.Unpack(), parameterTypes?.Unpack(), optionalParameterTypes?.Unpack());
        }

        /// <inheritdoc />
        public void EmitWriteLine(string value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void EmitWriteLine(IFieldSymbol fld)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void EmitWriteLine(ILocalBuilder localBuilder)
        {
            throw new NotImplementedException();
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
            _il.MarkSequencePoint(document, startLine, startColumn, endLine, endColumn);
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

        /// <summary>
        /// Converts the given OpCode value into a IKVM OpCode value.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        OpCode Convert(System.Reflection.Emit.OpCode opcode)
        {
            return (ushort)opcode.Value switch
            {
                0x00 => OpCodes.Nop,
                0x01 => OpCodes.Break,
                0x02 => OpCodes.Ldarg_0,
                0x03 => OpCodes.Ldarg_1,
                0x04 => OpCodes.Ldarg_2,
                0x05 => OpCodes.Ldarg_3,
                0x06 => OpCodes.Ldloc_0,
                0x07 => OpCodes.Ldloc_1,
                0x08 => OpCodes.Ldloc_2,
                0x09 => OpCodes.Ldloc_3,
                0x0a => OpCodes.Stloc_0,
                0x0b => OpCodes.Stloc_1,
                0x0c => OpCodes.Stloc_2,
                0x0d => OpCodes.Stloc_3,
                0x0e => OpCodes.Ldarg_S,
                0x0f => OpCodes.Ldarga_S,
                0x10 => OpCodes.Starg_S,
                0x11 => OpCodes.Ldloc_S,
                0x12 => OpCodes.Ldloca_S,
                0x13 => OpCodes.Stloc_S,
                0x14 => OpCodes.Ldnull,
                0x15 => OpCodes.Ldc_I4_M1,
                0x16 => OpCodes.Ldc_I4_0,
                0x17 => OpCodes.Ldc_I4_1,
                0x18 => OpCodes.Ldc_I4_2,
                0x19 => OpCodes.Ldc_I4_3,
                0x1a => OpCodes.Ldc_I4_4,
                0x1b => OpCodes.Ldc_I4_5,
                0x1c => OpCodes.Ldc_I4_6,
                0x1d => OpCodes.Ldc_I4_7,
                0x1e => OpCodes.Ldc_I4_8,
                0x1f => OpCodes.Ldc_I4_S,
                0x20 => OpCodes.Ldc_I4,
                0x21 => OpCodes.Ldc_I8,
                0x22 => OpCodes.Ldc_R4,
                0x23 => OpCodes.Ldc_R8,
                0x25 => OpCodes.Dup,
                0x26 => OpCodes.Pop,
                0x27 => OpCodes.Jmp,
                0x28 => OpCodes.Call,
                0x29 => OpCodes.Calli,
                0x2a => OpCodes.Ret,
                0x2b => OpCodes.Br_S,
                0x2c => OpCodes.Brfalse_S,
                0x2d => OpCodes.Brtrue_S,
                0x2e => OpCodes.Beq_S,
                0x2f => OpCodes.Bge_S,
                0x30 => OpCodes.Bgt_S,
                0x31 => OpCodes.Ble_S,
                0x32 => OpCodes.Blt_S,
                0x33 => OpCodes.Bne_Un_S,
                0x34 => OpCodes.Bge_Un_S,
                0x35 => OpCodes.Bgt_Un_S,
                0x36 => OpCodes.Ble_Un_S,
                0x37 => OpCodes.Blt_Un_S,
                0x38 => OpCodes.Br,
                0x39 => OpCodes.Brfalse,
                0x3a => OpCodes.Brtrue,
                0x3b => OpCodes.Beq,
                0x3c => OpCodes.Bge,
                0x3d => OpCodes.Bgt,
                0x3e => OpCodes.Ble,
                0x3f => OpCodes.Blt,
                0x40 => OpCodes.Bne_Un,
                0x41 => OpCodes.Bge_Un,
                0x42 => OpCodes.Bgt_Un,
                0x43 => OpCodes.Ble_Un,
                0x44 => OpCodes.Blt_Un,
                0x45 => OpCodes.Switch,
                0x46 => OpCodes.Ldind_I1,
                0x47 => OpCodes.Ldind_U1,
                0x48 => OpCodes.Ldind_I2,
                0x49 => OpCodes.Ldind_U2,
                0x4a => OpCodes.Ldind_I4,
                0x4b => OpCodes.Ldind_U4,
                0x4c => OpCodes.Ldind_I8,
                0x4d => OpCodes.Ldind_I,
                0x4e => OpCodes.Ldind_R4,
                0x4f => OpCodes.Ldind_R8,
                0x50 => OpCodes.Ldind_Ref,
                0x51 => OpCodes.Stind_Ref,
                0x52 => OpCodes.Stind_I1,
                0x53 => OpCodes.Stind_I2,
                0x54 => OpCodes.Stind_I4,
                0x55 => OpCodes.Stind_I8,
                0x56 => OpCodes.Stind_R4,
                0x57 => OpCodes.Stind_R8,
                0x58 => OpCodes.Add,
                0x59 => OpCodes.Sub,
                0x5a => OpCodes.Mul,
                0x5b => OpCodes.Div,
                0x5c => OpCodes.Div_Un,
                0x5d => OpCodes.Rem,
                0x5e => OpCodes.Rem_Un,
                0x5f => OpCodes.And,
                0x60 => OpCodes.Or,
                0x61 => OpCodes.Xor,
                0x62 => OpCodes.Shl,
                0x63 => OpCodes.Shr,
                0x64 => OpCodes.Shr_Un,
                0x65 => OpCodes.Neg,
                0x66 => OpCodes.Not,
                0x67 => OpCodes.Conv_I1,
                0x68 => OpCodes.Conv_I2,
                0x69 => OpCodes.Conv_I4,
                0x6a => OpCodes.Conv_I8,
                0x6b => OpCodes.Conv_R4,
                0x6c => OpCodes.Conv_R8,
                0x6d => OpCodes.Conv_U4,
                0x6e => OpCodes.Conv_U8,
                0x6f => OpCodes.Callvirt,
                0x70 => OpCodes.Cpobj,
                0x71 => OpCodes.Ldobj,
                0x72 => OpCodes.Ldstr,
                0x73 => OpCodes.Newobj,
                0x74 => OpCodes.Castclass,
                0x75 => OpCodes.Isinst,
                0x76 => OpCodes.Conv_R_Un,
                0x79 => OpCodes.Unbox,
                0x7a => OpCodes.Throw,
                0x7b => OpCodes.Ldfld,
                0x7c => OpCodes.Ldflda,
                0x7d => OpCodes.Stfld,
                0x7e => OpCodes.Ldsfld,
                0x7f => OpCodes.Ldsflda,
                0x80 => OpCodes.Stsfld,
                0x81 => OpCodes.Stobj,
                0x82 => OpCodes.Conv_Ovf_I1_Un,
                0x83 => OpCodes.Conv_Ovf_I2_Un,
                0x84 => OpCodes.Conv_Ovf_I4_Un,
                0x85 => OpCodes.Conv_Ovf_I8_Un,
                0x86 => OpCodes.Conv_Ovf_U1_Un,
                0x87 => OpCodes.Conv_Ovf_U2_Un,
                0x88 => OpCodes.Conv_Ovf_U4_Un,
                0x89 => OpCodes.Conv_Ovf_U8_Un,
                0x8a => OpCodes.Conv_Ovf_I_Un,
                0x8b => OpCodes.Conv_Ovf_U_Un,
                0x8c => OpCodes.Box,
                0x8d => OpCodes.Newarr,
                0x8e => OpCodes.Ldlen,
                0x8f => OpCodes.Ldelema,
                0x90 => OpCodes.Ldelem_I1,
                0x91 => OpCodes.Ldelem_U1,
                0x92 => OpCodes.Ldelem_I2,
                0x93 => OpCodes.Ldelem_U2,
                0x94 => OpCodes.Ldelem_I4,
                0x95 => OpCodes.Ldelem_U4,
                0x96 => OpCodes.Ldelem_I8,
                0x97 => OpCodes.Ldelem_I,
                0x98 => OpCodes.Ldelem_R4,
                0x99 => OpCodes.Ldelem_R8,
                0x9a => OpCodes.Ldelem_Ref,
                0x9b => OpCodes.Stelem_I,
                0x9c => OpCodes.Stelem_I1,
                0x9d => OpCodes.Stelem_I2,
                0x9e => OpCodes.Stelem_I4,
                0x9f => OpCodes.Stelem_I8,
                0xa0 => OpCodes.Stelem_R4,
                0xa1 => OpCodes.Stelem_R8,
                0xa2 => OpCodes.Stelem_Ref,
                0xa3 => OpCodes.Ldelem,
                0xa4 => OpCodes.Stelem,
                0xa5 => OpCodes.Unbox_Any,
                0xb3 => OpCodes.Conv_Ovf_I1,
                0xb4 => OpCodes.Conv_Ovf_U1,
                0xb5 => OpCodes.Conv_Ovf_I2,
                0xb6 => OpCodes.Conv_Ovf_U2,
                0xb7 => OpCodes.Conv_Ovf_I4,
                0xb8 => OpCodes.Conv_Ovf_U4,
                0xb9 => OpCodes.Conv_Ovf_I8,
                0xba => OpCodes.Conv_Ovf_U8,
                0xc2 => OpCodes.Refanyval,
                0xc3 => OpCodes.Ckfinite,
                0xc6 => OpCodes.Mkrefany,
                0xd0 => OpCodes.Ldtoken,
                0xd1 => OpCodes.Conv_U2,
                0xd2 => OpCodes.Conv_U1,
                0xd3 => OpCodes.Conv_I,
                0xd4 => OpCodes.Conv_Ovf_I,
                0xd5 => OpCodes.Conv_Ovf_U,
                0xd6 => OpCodes.Add_Ovf,
                0xd7 => OpCodes.Add_Ovf_Un,
                0xd8 => OpCodes.Mul_Ovf,
                0xd9 => OpCodes.Mul_Ovf_Un,
                0xda => OpCodes.Sub_Ovf,
                0xdb => OpCodes.Sub_Ovf_Un,
                0xdc => OpCodes.Endfinally,
                0xdd => OpCodes.Leave,
                0xde => OpCodes.Leave_S,
                0xdf => OpCodes.Stind_I,
                0xe0 => OpCodes.Conv_U,
                0xf8 => OpCodes.Prefix7,
                0xf9 => OpCodes.Prefix6,
                0xfa => OpCodes.Prefix5,
                0xfb => OpCodes.Prefix4,
                0xfc => OpCodes.Prefix3,
                0xfd => OpCodes.Prefix2,
                0xfe => OpCodes.Prefix1,
                0xff => OpCodes.Prefixref,
                0xfe00 => OpCodes.Arglist,
                0xfe01 => OpCodes.Ceq,
                0xfe02 => OpCodes.Cgt,
                0xfe03 => OpCodes.Cgt_Un,
                0xfe04 => OpCodes.Clt,
                0xfe05 => OpCodes.Clt_Un,
                0xfe06 => OpCodes.Ldftn,
                0xfe07 => OpCodes.Ldvirtftn,
                0xfe09 => OpCodes.Ldarg,
                0xfe0a => OpCodes.Ldarga,
                0xfe0b => OpCodes.Starg,
                0xfe0c => OpCodes.Ldloc,
                0xfe0d => OpCodes.Ldloca,
                0xfe0e => OpCodes.Stloc,
                0xfe0f => OpCodes.Localloc,
                0xfe11 => OpCodes.Endfilter,
                0xfe15 => OpCodes.Initobj,
                0xfe17 => OpCodes.Cpblk,
                0xfe18 => OpCodes.Initblk,
                0xfe1a => OpCodes.Rethrow,
                0xfe1c => OpCodes.Sizeof,
                0xfe1d => OpCodes.Refanytype,
                0xfe13 => OpCodes.Volatile,
                _ => throw new NotImplementedException(),
            };
        }

    }

}
