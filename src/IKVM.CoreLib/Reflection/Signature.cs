/*
  Copyright (C) 2009-2012 Jeroen Frijters

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
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Metadata;
using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;

using static IKVM.Reflection.Module;

using CallingConvention = System.Runtime.InteropServices.CallingConvention;

namespace IKVM.Reflection
{

    abstract class Signature
    {

        internal const byte DEFAULT = 0x00;
        internal const byte VARARG = 0x05;
        internal const byte GENERIC = 0x10;
        internal const byte HASTHIS = 0x20;
        internal const byte EXPLICITTHIS = 0x40;
        internal const byte FIELD = 0x06;
        internal const byte LOCAL_SIG = 0x07;
        internal const byte PROPERTY = 0x08;
        internal const byte GENERICINST = 0x0A;
        internal const byte SENTINEL = 0x41;
        internal const byte ELEMENT_TYPE_VOID = 0x01;
        internal const byte ELEMENT_TYPE_BOOLEAN = 0x02;
        internal const byte ELEMENT_TYPE_CHAR = 0x03;
        internal const byte ELEMENT_TYPE_I1 = 0x04;
        internal const byte ELEMENT_TYPE_U1 = 0x05;
        internal const byte ELEMENT_TYPE_I2 = 0x06;
        internal const byte ELEMENT_TYPE_U2 = 0x07;
        internal const byte ELEMENT_TYPE_I4 = 0x08;
        internal const byte ELEMENT_TYPE_U4 = 0x09;
        internal const byte ELEMENT_TYPE_I8 = 0x0a;
        internal const byte ELEMENT_TYPE_U8 = 0x0b;
        internal const byte ELEMENT_TYPE_R4 = 0x0c;
        internal const byte ELEMENT_TYPE_R8 = 0x0d;
        internal const byte ELEMENT_TYPE_STRING = 0x0e;
        internal const byte ELEMENT_TYPE_PTR = 0x0f;
        internal const byte ELEMENT_TYPE_BYREF = 0x10;
        internal const byte ELEMENT_TYPE_VALUETYPE = 0x11;
        internal const byte ELEMENT_TYPE_CLASS = 0x12;
        internal const byte ELEMENT_TYPE_VAR = 0x13;
        internal const byte ELEMENT_TYPE_ARRAY = 0x14;
        internal const byte ELEMENT_TYPE_GENERICINST = 0x15;
        internal const byte ELEMENT_TYPE_TYPEDBYREF = 0x16;
        internal const byte ELEMENT_TYPE_I = 0x18;
        internal const byte ELEMENT_TYPE_U = 0x19;
        internal const byte ELEMENT_TYPE_FNPTR = 0x1b;
        internal const byte ELEMENT_TYPE_OBJECT = 0x1c;
        internal const byte ELEMENT_TYPE_SZARRAY = 0x1d;
        internal const byte ELEMENT_TYPE_MVAR = 0x1e;
        internal const byte ELEMENT_TYPE_CMOD_REQD = 0x1f;
        internal const byte ELEMENT_TYPE_CMOD_OPT = 0x20;
        internal const byte ELEMENT_TYPE_PINNED = 0x45;

        internal abstract void Write(ModuleBuilder module, ByteBuffer bb);

        static Type ReadGenericInst(ModuleReader module, ByteReader br, IGenericContext context)
        {
            Type type;
            switch (br.ReadByte())
            {
                case ELEMENT_TYPE_CLASS:
                    type = ReadTypeDefOrRefEncoded(module, br, context).MarkNotValueType();
                    break;
                case ELEMENT_TYPE_VALUETYPE:
                    type = ReadTypeDefOrRefEncoded(module, br, context).MarkValueType();
                    break;
                default:
                    throw new BadImageFormatException();
            }

            if (!type.__IsMissing && !type.IsGenericTypeDefinition)
                throw new BadImageFormatException();

            int genArgCount = br.ReadCompressedUInt();
            var args = new Type[genArgCount];
            CustomModifiers[] mods = null;
            for (int i = 0; i < genArgCount; i++)
            {
                // LAMESPEC the Type production (23.2.12) doesn't include CustomMod* for genericinst, but C++ uses it, the verifier allows it and ildasm also supports it
                var cm = CustomModifiers.Read(module, br, context);
                if (!cm.IsEmpty)
                {
                    mods ??= new CustomModifiers[genArgCount];
                    mods[i] = cm;
                }

                args[i] = ReadType(module, br, context);
            }

            return GenericTypeInstance.Make(type, args, mods);
        }

        internal static Type ReadTypeSpec(ModuleReader module, ByteReader br, IGenericContext context)
        {
            // LAMESPEC a TypeSpec can contain custom modifiers (C++/CLI generates "newarr (TypeSpec with custom modifiers)")
            CustomModifiers.Skip(br);
            // LAMESPEC anything can be adorned by (useless) custom modifiers
            // also, VAR and MVAR are also used in TypeSpec (contrary to what the spec says)
            return ReadType(module, br, context);
        }

        private static Type ReadFunctionPointer(ModuleReader module, ByteReader br, IGenericContext context)
        {
            __StandAloneMethodSig sig = MethodSignature.ReadStandAloneMethodSig(module, br, context);
            if (module.Universe.EnableFunctionPointers)
            {
                return FunctionPointerType.Make(module.Universe, sig);
            }
            else
            {
                // by default, like .NET we return System.IntPtr here
                return module.Universe.System_IntPtr;
            }
        }

        internal static Type[] ReadMethodSpec(ModuleReader module, ByteReader br, IGenericContext context)
        {
            if (br.ReadByte() != GENERICINST)
                throw new BadImageFormatException();

            var args = new Type[br.ReadCompressedUInt()];
            for (int i = 0; i < args.Length; i++)
            {
                CustomModifiers.Skip(br);
                args[i] = ReadType(module, br, context);
            }

            return args;
        }

        static int[] ReadArraySizes(ByteReader br)
        {
            var num = br.ReadCompressedUInt();
            if (num == 0)
                return null;

            var arr = new int[num];
            for (var i = 0; i < num; i++)
                arr[i] = br.ReadCompressedUInt();

            return arr;
        }

        private static int[] ReadArrayBounds(ByteReader br)
        {
            var num = br.ReadCompressedUInt();
            if (num == 0)
                return null;

            var arr = new int[num];
            for (var i = 0; i < num; i++)
                arr[i] = br.ReadCompressedInt();

            return arr;
        }

        static Type ReadTypeOrVoid(ModuleReader module, ByteReader br, IGenericContext context)
        {
            if (br.PeekByte() == ELEMENT_TYPE_VOID)
            {
                br.ReadByte();
                return module.Universe.System_Void;
            }
            else
            {
                return ReadType(module, br, context);
            }
        }

        // see ECMA 335 CLI spec June 2006 section 23.2.12 for this production
        protected static Type ReadType(ModuleReader module, ByteReader br, IGenericContext context)
        {
            CustomModifiers mods;
            switch (br.ReadByte())
            {
                case ELEMENT_TYPE_CLASS:
                    return ReadTypeDefOrRefEncoded(module, br, context).MarkNotValueType();
                case ELEMENT_TYPE_VALUETYPE:
                    return ReadTypeDefOrRefEncoded(module, br, context).MarkValueType();
                case ELEMENT_TYPE_BOOLEAN:
                    return module.Universe.System_Boolean;
                case ELEMENT_TYPE_CHAR:
                    return module.Universe.System_Char;
                case ELEMENT_TYPE_I1:
                    return module.Universe.System_SByte;
                case ELEMENT_TYPE_U1:
                    return module.Universe.System_Byte;
                case ELEMENT_TYPE_I2:
                    return module.Universe.System_Int16;
                case ELEMENT_TYPE_U2:
                    return module.Universe.System_UInt16;
                case ELEMENT_TYPE_I4:
                    return module.Universe.System_Int32;
                case ELEMENT_TYPE_U4:
                    return module.Universe.System_UInt32;
                case ELEMENT_TYPE_I8:
                    return module.Universe.System_Int64;
                case ELEMENT_TYPE_U8:
                    return module.Universe.System_UInt64;
                case ELEMENT_TYPE_R4:
                    return module.Universe.System_Single;
                case ELEMENT_TYPE_R8:
                    return module.Universe.System_Double;
                case ELEMENT_TYPE_I:
                    return module.Universe.System_IntPtr;
                case ELEMENT_TYPE_U:
                    return module.Universe.System_UIntPtr;
                case ELEMENT_TYPE_STRING:
                    return module.Universe.System_String;
                case ELEMENT_TYPE_OBJECT:
                    return module.Universe.System_Object;
                case ELEMENT_TYPE_VAR:
                    return context.GetGenericTypeArgument(br.ReadCompressedUInt());
                case ELEMENT_TYPE_MVAR:
                    return context.GetGenericMethodArgument(br.ReadCompressedUInt());
                case ELEMENT_TYPE_GENERICINST:
                    return ReadGenericInst(module, br, context);
                case ELEMENT_TYPE_SZARRAY:
                    mods = CustomModifiers.Read(module, br, context);
                    return ReadType(module, br, context).__MakeArrayType(mods);
                case ELEMENT_TYPE_ARRAY:
                    mods = CustomModifiers.Read(module, br, context);
                    return ReadType(module, br, context).__MakeArrayType(br.ReadCompressedUInt(), ReadArraySizes(br), ReadArrayBounds(br), mods);
                case ELEMENT_TYPE_PTR:
                    mods = CustomModifiers.Read(module, br, context);
                    return ReadTypeOrVoid(module, br, context).__MakePointerType(mods);
                case ELEMENT_TYPE_FNPTR:
                    return ReadFunctionPointer(module, br, context);
                default:
                    throw new BadImageFormatException();
            }
        }

        internal static void ReadLocalVarSig(ModuleReader module, ByteReader br, IGenericContext context, List<LocalVariableInfo> list)
        {
            if (br.Length < 2 || br.ReadByte() != LOCAL_SIG)
                throw new BadImageFormatException("Invalid local variable signature");

            var count = br.ReadCompressedUInt();
            for (int i = 0; i < count; i++)
            {
                if (br.PeekByte() == ELEMENT_TYPE_TYPEDBYREF)
                {
                    br.ReadByte();
                    list.Add(new LocalVariableInfo(i, module.Universe.System_TypedReference, false));
                }
                else
                {
                    var mods1 = CustomModifiers.Read(module, br, context);
                    var pinned = false;
                    if (br.PeekByte() == ELEMENT_TYPE_PINNED)
                    {
                        br.ReadByte();
                        pinned = true;
                    }

                    var mods2 = CustomModifiers.Read(module, br, context);
                    var type = ReadTypeOrByRef(module, br, context);
                    list.Add(new LocalVariableInfo(i, type, pinned, mods2));
                }
            }
        }

        static Type ReadTypeOrByRef(ModuleReader module, ByteReader br, IGenericContext context)
        {
            if (br.PeekByte() == ELEMENT_TYPE_BYREF)
            {
                br.ReadByte();
                // LAMESPEC it is allowed (by C++/CLI, ilasm and peverify) to have custom modifiers after the BYREF
                // (which makes sense, as it is analogous to pointers)
                var mods = CustomModifiers.Read(module, br, context);
                // C++/CLI generates void& local variables, so we need to use ReadTypeOrVoid here
                return ReadTypeOrVoid(module, br, context).__MakeByRefType(mods);
            }
            else
            {
                return ReadType(module, br, context);
            }
        }

        protected static Type ReadRetType(ModuleReader module, ByteReader br, IGenericContext context)
        {
            switch (br.PeekByte())
            {
                case ELEMENT_TYPE_VOID:
                    br.ReadByte();
                    return module.Universe.System_Void;
                case ELEMENT_TYPE_TYPEDBYREF:
                    br.ReadByte();
                    return module.Universe.System_TypedReference;
                default:
                    return ReadTypeOrByRef(module, br, context);
            }
        }

        protected static Type ReadParam(ModuleReader module, ByteReader br, IGenericContext context)
        {
            switch (br.PeekByte())
            {
                case ELEMENT_TYPE_TYPEDBYREF:
                    br.ReadByte();
                    return module.Universe.System_TypedReference;
                default:
                    return ReadTypeOrByRef(module, br, context);
            }
        }

        protected static void WriteType(ModuleBuilder module, ByteBuffer bb, Type type)
        {
            while (type.HasElementType)
            {
                byte sigElementType = type.SigElementType;
                bb.Write(sigElementType);
                if (sigElementType == ELEMENT_TYPE_ARRAY)
                {
                    // LAMESPEC the Type production (23.2.12) doesn't include CustomMod* for arrays, but the verifier allows it and ildasm also supports it
                    WriteCustomModifiers(module, bb, type.__GetCustomModifiers());
                    WriteType(module, bb, type.GetElementType());
                    bb.WriteCompressedUInt(type.GetArrayRank());

                    var sizes = type.__GetArraySizes();
                    bb.WriteCompressedUInt(sizes.Length);
                    for (int i = 0; i < sizes.Length; i++)
                        bb.WriteCompressedUInt(sizes[i]);

                    var lobounds = type.__GetArrayLowerBounds();
                    bb.WriteCompressedUInt(lobounds.Length);
                    for (int i = 0; i < lobounds.Length; i++)
                        bb.WriteCompressedInt(lobounds[i]);

                    return;
                }
                WriteCustomModifiers(module, bb, type.__GetCustomModifiers());
                type = type.GetElementType();
            }

            if (type.__IsBuiltIn)
            {
                bb.Write(type.SigElementType);
            }
            else if (type.IsGenericParameter)
            {
                bb.Write(type.SigElementType);
                bb.WriteCompressedUInt(type.GenericParameterPosition);
            }
            else if (!type.__IsMissing && type.IsGenericType)
            {
                WriteGenericSignature(module, bb, type);
            }
            else if (type.IsFunctionPointer)
            {
                bb.Write(ELEMENT_TYPE_FNPTR);
                WriteStandAloneMethodSig(module, bb, type.__MethodSignature);
            }
            else
            {
                if (type.IsValueType)
                    bb.Write(ELEMENT_TYPE_VALUETYPE);
                else
                    bb.Write(ELEMENT_TYPE_CLASS);

                bb.WriteTypeDefOrRefEncoded(module.GetTypeToken(type).Token);
            }
        }

        static void WriteGenericSignature(ModuleBuilder module, ByteBuffer bb, Type type)
        {
            var typeArguments = type.GetGenericArguments();
            var customModifiers = type.__GetGenericArgumentsCustomModifiers();
            if (!type.IsGenericTypeDefinition)
                type = type.GetGenericTypeDefinition();

            bb.Write(ELEMENT_TYPE_GENERICINST);
            if (type.IsValueType)
                bb.Write(ELEMENT_TYPE_VALUETYPE);
            else
                bb.Write(ELEMENT_TYPE_CLASS);

            bb.WriteTypeDefOrRefEncoded(module.GetTypeToken(type).Token);
            bb.WriteCompressedUInt(typeArguments.Length);
            for (var i = 0; i < typeArguments.Length; i++)
            {
                WriteCustomModifiers(module, bb, customModifiers[i]);
                WriteType(module, bb, typeArguments[i]);
            }
        }

        protected static void WriteCustomModifiers(ModuleBuilder module, ByteBuffer bb, CustomModifiers modifiers)
        {
            foreach (var entry in modifiers)
            {
                bb.Write(entry.IsRequired ? ELEMENT_TYPE_CMOD_REQD : ELEMENT_TYPE_CMOD_OPT);
                bb.WriteTypeDefOrRefEncoded(module.GetTypeTokenForMemberRef(entry.Type));
            }
        }

        internal static Type ReadTypeDefOrRefEncoded(ModuleReader module, ByteReader br, IGenericContext context)
        {
            var encoded = br.ReadCompressedUInt();
            return (encoded & 3) switch
            {
                0 => module.ResolveType((TypeDefTable.Index << 24) + (encoded >> 2), null, null),
                1 => module.ResolveType((TypeRefTable.Index << 24) + (encoded >> 2), null, null),
                2 => module.ResolveType((TypeSpecTable.Index << 24) + (encoded >> 2), context),
                _ => throw new BadImageFormatException(),
            };
        }

        internal static void WriteStandAloneMethodSig(ModuleBuilder module, ByteBuffer bb, __StandAloneMethodSig sig)
        {
            if (sig.IsUnmanaged)
            {
                switch (sig.UnmanagedCallingConvention)
                {
                    case CallingConvention.Cdecl:
                        bb.Write((byte)0x01);   // C
                        break;
                    case CallingConvention.StdCall:
                    case CallingConvention.Winapi:
                        bb.Write((byte)0x02);   // STDCALL
                        break;
                    case CallingConvention.ThisCall:
                        bb.Write((byte)0x03);   // THISCALL
                        break;
                    case CallingConvention.FastCall:
                        bb.Write((byte)0x04);   // FASTCALL
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("callingConvention");
                }
            }
            else
            {
                var callingConvention = sig.CallingConvention;
                var flags = 0;
                if ((callingConvention & CallingConventions.HasThis) != 0)
                    flags |= HASTHIS;
                if ((callingConvention & CallingConventions.ExplicitThis) != 0)
                    flags |= EXPLICITTHIS;
                if ((callingConvention & CallingConventions.VarArgs) != 0)
                    flags |= VARARG;

                bb.Write(flags);
            }

            var parameterTypes = sig.ParameterTypes;
            var optionalParameterTypes = sig.OptionalParameterTypes;
            bb.WriteCompressedUInt(parameterTypes.Length + optionalParameterTypes.Length);
            WriteCustomModifiers(module, bb, sig.GetReturnTypeCustomModifiers());
            WriteType(module, bb, sig.ReturnType);

            int index = 0;
            foreach (var t in parameterTypes)
            {
                WriteCustomModifiers(module, bb, sig.GetParameterCustomModifiers(index++));
                WriteType(module, bb, t);
            }

            // note that optional parameters are only allowed for managed signatures (but we don't enforce that)
            if (optionalParameterTypes.Length > 0)
            {
                bb.Write(SENTINEL);
                foreach (var t in optionalParameterTypes)
                {
                    WriteCustomModifiers(module, bb, sig.GetParameterCustomModifiers(index++));
                    WriteType(module, bb, t);
                }
            }
        }

        internal static void WriteTypeSpec(ModuleBuilder module, ByteBuffer bb, Type type)
        {
            WriteType(module, bb, type);
        }

        internal static void WriteMethodSpec(ModuleBuilder module, ByteBuffer bb, Type[] genArgs)
        {
            bb.Write(GENERICINST);
            bb.WriteCompressedUInt(genArgs.Length);
            foreach (var arg in genArgs)
                WriteType(module, bb, arg);
        }

        // this reads just the optional parameter types, from a MethodRefSig
        internal static Type[] ReadOptionalParameterTypes(ModuleReader module, ByteReader br, IGenericContext context, out CustomModifiers[] customModifiers)
        {
            br.ReadByte();
            var paramCount = br.ReadCompressedUInt();
            CustomModifiers.Skip(br);
            ReadRetType(module, br, context);
            for (int i = 0; i < paramCount; i++)
            {
                if (br.PeekByte() == SENTINEL)
                {
                    br.ReadByte();
                    var types = new Type[paramCount - i];
                    customModifiers = new CustomModifiers[types.Length];
                    for (int j = 0; j < types.Length; j++)
                    {
                        customModifiers[j] = CustomModifiers.Read(module, br, context);
                        types[j] = ReadType(module, br, context);
                    }

                    return types;
                }

                CustomModifiers.Skip(br);
                ReadType(module, br, context);
            }

            customModifiers = Array.Empty<CustomModifiers>();
            return Type.EmptyTypes;
        }

        protected static Type[] BindTypeParameters(IGenericBinder binder, Type[] types)
        {
            if (types == null || types.Length == 0)
                return Type.EmptyTypes;

            var expanded = new Type[types.Length];
            for (var i = 0; i < types.Length; i++)
                expanded[i] = types[i].BindTypeParameters(binder);

            return expanded;
        }

        internal static void WriteSignatureHelper(ModuleBuilder module, ByteBuffer bb, byte flags, ushort paramCount, List<Type> args)
        {
            bb.Write(flags);
            if (flags != FIELD)
                bb.WriteCompressedUInt(paramCount);

            foreach (var type in args)
            {
                if (type == null)
                    bb.Write(ELEMENT_TYPE_VOID);
                else if (type is MarkerType)
                    bb.Write(type.SigElementType);
                else
                    WriteType(module, bb, type);
            }
        }

    }

}
