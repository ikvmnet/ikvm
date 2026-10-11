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
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;

using static IKVM.Reflection.Module;

using CallingConvention = System.Runtime.InteropServices.CallingConvention;

namespace IKVM.Reflection
{

    /// <summary>
    /// Base class for metadata signatures (ECMA-335 II.23.2). Holds the signature byte constants and the helpers that
    /// read types from, and write types to, signature blobs.
    /// </summary>
    abstract class Signature
    {

        /// <summary>
        /// The default managed calling convention.
        /// </summary>
        internal const byte DEFAULT = 0x00;

        /// <summary>
        /// The managed vararg calling convention.
        /// </summary>
        internal const byte VARARG = 0x05;

        /// <summary>
        /// The flag indicating that a method signature has generic parameters.
        /// </summary>
        internal const byte GENERIC = 0x10;

        /// <summary>
        /// The flag indicating that a method signature has an instance (<c>this</c>) parameter.
        /// </summary>
        internal const byte HASTHIS = 0x20;

        /// <summary>
        /// The flag indicating that the instance parameter is listed explicitly in the method signature.
        /// </summary>
        internal const byte EXPLICITTHIS = 0x40;

        /// <summary>
        /// The first byte of a field signature.
        /// </summary>
        internal const byte FIELD = 0x06;

        /// <summary>
        /// The first byte of a local variable signature.
        /// </summary>
        internal const byte LOCAL_SIG = 0x07;

        /// <summary>
        /// The first byte of a property signature.
        /// </summary>
        internal const byte PROPERTY = 0x08;

        /// <summary>
        /// The first byte of a generic method instantiation (MethodSpec) signature.
        /// </summary>
        internal const byte GENERICINST = 0x0A;

        /// <summary>
        /// Marks the start of the variable arguments of a vararg call site signature.
        /// </summary>
        internal const byte SENTINEL = 0x41;

        /// <summary>
        /// The element type of <c>System.Void</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_VOID = 0x01;

        /// <summary>
        /// The element type of <c>System.Boolean</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_BOOLEAN = 0x02;

        /// <summary>
        /// The element type of <c>System.Char</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_CHAR = 0x03;

        /// <summary>
        /// The element type of <c>System.SByte</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_I1 = 0x04;

        /// <summary>
        /// The element type of <c>System.Byte</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_U1 = 0x05;

        /// <summary>
        /// The element type of <c>System.Int16</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_I2 = 0x06;

        /// <summary>
        /// The element type of <c>System.UInt16</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_U2 = 0x07;

        /// <summary>
        /// The element type of <c>System.Int32</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_I4 = 0x08;

        /// <summary>
        /// The element type of <c>System.UInt32</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_U4 = 0x09;

        /// <summary>
        /// The element type of <c>System.Int64</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_I8 = 0x0a;

        /// <summary>
        /// The element type of <c>System.UInt64</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_U8 = 0x0b;

        /// <summary>
        /// The element type of <c>System.Single</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_R4 = 0x0c;

        /// <summary>
        /// The element type of <c>System.Double</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_R8 = 0x0d;

        /// <summary>
        /// The element type of <c>System.String</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_STRING = 0x0e;

        /// <summary>
        /// The element type of an unmanaged pointer, followed by the pointed-to type.
        /// </summary>
        internal const byte ELEMENT_TYPE_PTR = 0x0f;

        /// <summary>
        /// The element type of a managed pointer, followed by the referenced type.
        /// </summary>
        internal const byte ELEMENT_TYPE_BYREF = 0x10;

        /// <summary>
        /// The element type of a value type, followed by its TypeDefOrRef coded index.
        /// </summary>
        internal const byte ELEMENT_TYPE_VALUETYPE = 0x11;

        /// <summary>
        /// The element type of a reference type, followed by its TypeDefOrRef coded index.
        /// </summary>
        internal const byte ELEMENT_TYPE_CLASS = 0x12;

        /// <summary>
        /// The element type of a generic type parameter, followed by its position.
        /// </summary>
        internal const byte ELEMENT_TYPE_VAR = 0x13;

        /// <summary>
        /// The element type of a general (multi-dimensional) array.
        /// </summary>
        internal const byte ELEMENT_TYPE_ARRAY = 0x14;

        /// <summary>
        /// The element type of a generic type instantiation.
        /// </summary>
        internal const byte ELEMENT_TYPE_GENERICINST = 0x15;

        /// <summary>
        /// The element type of <c>System.TypedReference</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_TYPEDBYREF = 0x16;

        /// <summary>
        /// The element type of <c>System.IntPtr</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_I = 0x18;

        /// <summary>
        /// The element type of <c>System.UIntPtr</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_U = 0x19;

        /// <summary>
        /// The element type of a function pointer, followed by a method signature.
        /// </summary>
        internal const byte ELEMENT_TYPE_FNPTR = 0x1b;

        /// <summary>
        /// The element type of <c>System.Object</c>.
        /// </summary>
        internal const byte ELEMENT_TYPE_OBJECT = 0x1c;

        /// <summary>
        /// The element type of a single-dimensional, zero-based array.
        /// </summary>
        internal const byte ELEMENT_TYPE_SZARRAY = 0x1d;

        /// <summary>
        /// The element type of a generic method parameter, followed by its position.
        /// </summary>
        internal const byte ELEMENT_TYPE_MVAR = 0x1e;

        /// <summary>
        /// Introduces a required custom modifier.
        /// </summary>
        internal const byte ELEMENT_TYPE_CMOD_REQD = 0x1f;

        /// <summary>
        /// Introduces an optional custom modifier.
        /// </summary>
        internal const byte ELEMENT_TYPE_CMOD_OPT = 0x20;

        /// <summary>
        /// Marks a local variable as pinned.
        /// </summary>
        internal const byte ELEMENT_TYPE_PINNED = 0x45;

        /// <summary>
        /// Writes the signature to a blob.
        /// </summary>
        /// <param name="module">The module being built, used to obtain tokens for the types in the signature.</param>
        /// <param name="bb">The blob builder to write to.</param>
        internal abstract void Write(ModuleBuilder module, BlobBuilder bb);

        /// <summary>
        /// Reads a generic type instantiation, after the <see cref="ELEMENT_TYPE_GENERICINST"/> byte has been read.
        /// </summary>
        /// <param name="module">The module the signature belongs to.</param>
        /// <param name="br">The reader positioned after the <see cref="ELEMENT_TYPE_GENERICINST"/> byte.</param>
        /// <param name="context">The generic context used to resolve type parameters.</param>
        /// <returns>The constructed generic type.</returns>
        /// <exception cref="BadImageFormatException">The instantiation is malformed or its type is not a generic type definition.</exception>
        static Type ReadGenericInst(ModuleReader module, ref BlobReader br, IGenericContext context)
        {
            var type = br.ReadByte() switch
            {
                ELEMENT_TYPE_CLASS => ReadTypeDefOrRefEncoded(module, ref br, context).MarkNotValueType(),
                ELEMENT_TYPE_VALUETYPE => ReadTypeDefOrRefEncoded(module, ref br, context).MarkValueType(),
                _ => throw new BadImageFormatException(),
            };
            if (!type.__IsMissing && !type.IsGenericTypeDefinition)
                throw new BadImageFormatException();

            var genArgCount = br.ReadCompressedInteger();
            var args = new Type[genArgCount];
            CustomModifiers[] mods = null;
            for (var i = 0; i < genArgCount; i++)
            {
                // LAMESPEC the Type production (23.2.12) doesn't include CustomMod* for genericinst, but C++ uses it, the verifier allows it and ildasm also supports it
                var cm = CustomModifiers.Read(module, ref br, context);
                if (!cm.IsEmpty)
                {
                    mods ??= new CustomModifiers[genArgCount];
                    mods[i] = cm;
                }

                args[i] = ReadType(module, ref br, context);
            }

            return GenericTypeInstance.Make(type, args, mods);
        }

        /// <summary>
        /// Reads a TypeSpec signature. Leading custom modifiers are skipped.
        /// </summary>
        /// <param name="module">The module the signature belongs to.</param>
        /// <param name="br">The reader positioned at the start of the signature.</param>
        /// <param name="context">The generic context used to resolve type parameters.</param>
        /// <returns>The type the signature describes.</returns>
        internal static Type ReadTypeSpec(ModuleReader module, BlobReader br, IGenericContext context)
        {
            // LAMESPEC a TypeSpec can contain custom modifiers (C++/CLI generates "newarr (TypeSpec with custom modifiers)")
            CustomModifiers.Skip(ref br);

            // LAMESPEC anything can be adorned by (useless) custom modifiers
            // also, VAR and MVAR are also used in TypeSpec (contrary to what the spec says)
            return ReadType(module, ref br, context);
        }

        /// <summary>
        /// Reads a function pointer type, after the <see cref="ELEMENT_TYPE_FNPTR"/> byte has been read.
        /// </summary>
        /// <param name="module">The module the signature belongs to.</param>
        /// <param name="br">The reader positioned at the method signature of the function pointer.</param>
        /// <param name="context">The generic context used to resolve type parameters.</param>
        /// <returns>
        /// The function pointer type if <see cref="Universe.EnableFunctionPointers"/> is set; otherwise
        /// <c>System.IntPtr</c>, as .NET reflection does.
        /// </returns>
        static Type ReadFunctionPointer(ModuleReader module, ref BlobReader br, IGenericContext context)
        {
            __StandAloneMethodSig sig = MethodSignature.ReadStandAloneMethodSig(module, ref br, context);
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

        /// <summary>
        /// Reads a MethodSpec signature, the type arguments of a generic method instantiation. Custom modifiers on the
        /// type arguments are skipped.
        /// </summary>
        /// <param name="module">The module the signature belongs to.</param>
        /// <param name="br">The reader positioned at the start of the signature.</param>
        /// <param name="context">The generic context used to resolve type parameters.</param>
        /// <returns>The type arguments.</returns>
        /// <exception cref="BadImageFormatException">The blob is not a MethodSpec signature.</exception>
        internal static Type[] ReadMethodSpec(ModuleReader module, BlobReader br, IGenericContext context)
        {
            if (br.ReadByte() != GENERICINST)
                throw new BadImageFormatException();

            var args = new Type[br.ReadCompressedInteger()];
            for (var i = 0; i < args.Length; i++)
            {
                CustomModifiers.Skip(ref br);
                args[i] = ReadType(module, ref br, context);
            }

            return args;
        }

        /// <summary>
        /// Reads the dimension sizes of a general array type.
        /// </summary>
        /// <param name="br">The reader positioned at the size count.</param>
        /// <returns>The sizes, or <see langword="null"/> if none are specified.</returns>
        static int[] ReadArraySizes(ref BlobReader br)
        {
            var num = br.ReadCompressedInteger();
            if (num == 0)
                return null;

            var arr = new int[num];
            for (var i = 0; i < num; i++)
                arr[i] = br.ReadCompressedInteger();

            return arr;
        }

        /// <summary>
        /// Reads the dimension lower bounds of a general array type.
        /// </summary>
        /// <param name="br">The reader positioned at the lower bound count.</param>
        /// <returns>The lower bounds, or <see langword="null"/> if none are specified.</returns>
        static int[] ReadArrayBounds(ref BlobReader br)
        {
            var num = br.ReadCompressedInteger();
            if (num == 0)
                return null;

            var arr = new int[num];
            for (var i = 0; i < num; i++)
                arr[i] = br.ReadCompressedSignedInteger();

            return arr;
        }

        /// <summary>
        /// Reads a type, also accepting <see cref="ELEMENT_TYPE_VOID"/>.
        /// </summary>
        /// <param name="module">The module the signature belongs to.</param>
        /// <param name="br">The reader positioned at the type.</param>
        /// <param name="context">The generic context used to resolve type parameters.</param>
        /// <returns>The type that was read.</returns>
        static Type ReadTypeOrVoid(ModuleReader module, ref BlobReader br, IGenericContext context)
        {
            if (br.PeekByte() == ELEMENT_TYPE_VOID)
            {
                br.ReadByte();
                return module.Universe.System_Void;
            }
            else
            {
                return ReadType(module, ref br, context);
            }
        }

        /// <summary>
        /// Reads a type, the Type production of ECMA-335 II.23.2.12.
        /// </summary>
        /// <param name="module">The module the signature belongs to.</param>
        /// <param name="br">The reader positioned at the type.</param>
        /// <param name="context">The generic context used to resolve type parameters.</param>
        /// <returns>The type that was read.</returns>
        /// <exception cref="BadImageFormatException">The element type is not valid in a Type production.</exception>
        protected static Type ReadType(ModuleReader module, ref BlobReader br, IGenericContext context)
        {
            CustomModifiers mods;
            switch (br.ReadByte())
            {
                case ELEMENT_TYPE_CLASS:
                    return ReadTypeDefOrRefEncoded(module, ref br, context).MarkNotValueType();
                case ELEMENT_TYPE_VALUETYPE:
                    return ReadTypeDefOrRefEncoded(module, ref br, context).MarkValueType();
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
                    return context.GetGenericTypeArgument(br.ReadCompressedInteger());
                case ELEMENT_TYPE_MVAR:
                    return context.GetGenericMethodArgument(br.ReadCompressedInteger());
                case ELEMENT_TYPE_GENERICINST:
                    return ReadGenericInst(module, ref br, context);
                case ELEMENT_TYPE_SZARRAY:
                    mods = CustomModifiers.Read(module, ref br, context);
                    return ReadType(module, ref br, context).__MakeArrayType(mods);
                case ELEMENT_TYPE_ARRAY:
                    mods = CustomModifiers.Read(module, ref br, context);
                    return ReadType(module, ref br, context).__MakeArrayType(br.ReadCompressedInteger(), ReadArraySizes(ref br), ReadArrayBounds(ref br), mods);
                case ELEMENT_TYPE_PTR:
                    mods = CustomModifiers.Read(module, ref br, context);
                    return ReadTypeOrVoid(module, ref br, context).__MakePointerType(mods);
                case ELEMENT_TYPE_FNPTR:
                    return ReadFunctionPointer(module, ref br, context);
                default:
                    throw new BadImageFormatException();
            }
        }

        /// <summary>
        /// Reads a type that may be a by-ref type.
        /// </summary>
        /// <param name="module">The module the signature belongs to.</param>
        /// <param name="br">The reader positioned at the type.</param>
        /// <param name="context">The generic context used to resolve type parameters.</param>
        /// <returns>The type that was read.</returns>
        static Type ReadTypeOrByRef(ModuleReader module, ref BlobReader br, IGenericContext context)
        {
            if (br.PeekByte() == ELEMENT_TYPE_BYREF)
            {
                br.ReadByte();
                // LAMESPEC it is allowed (by C++/CLI, ilasm and peverify) to have custom modifiers after the BYREF
                // (which makes sense, as it is analogous to pointers)
                var mods = CustomModifiers.Read(module, ref br, context);
                // C++/CLI generates void& local variables, so we need to use ReadTypeOrVoid here
                return ReadTypeOrVoid(module, ref br, context).__MakeByRefType(mods);
            }
            else
            {
                return ReadType(module, ref br, context);
            }
        }

        /// <summary>
        /// Reads a return type, the RetType production of ECMA-335 II.23.2.11, without its custom modifiers.
        /// </summary>
        /// <param name="module">The module the signature belongs to.</param>
        /// <param name="br">The reader positioned at the return type.</param>
        /// <param name="context">The generic context used to resolve type parameters.</param>
        /// <returns>The return type.</returns>
        protected static Type ReadRetType(ModuleReader module, ref BlobReader br, IGenericContext context)
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
                    return ReadTypeOrByRef(module, ref br, context);
            }
        }

        /// <summary>
        /// Reads a parameter type, the Param production of ECMA-335 II.23.2.10, without its custom modifiers.
        /// </summary>
        /// <param name="module">The module the signature belongs to.</param>
        /// <param name="br">The reader positioned at the parameter type.</param>
        /// <param name="context">The generic context used to resolve type parameters.</param>
        /// <returns>The parameter type.</returns>
        protected static Type ReadParam(ModuleReader module, ref BlobReader br, IGenericContext context)
        {
            switch (br.PeekByte())
            {
                case ELEMENT_TYPE_TYPEDBYREF:
                    br.ReadByte();
                    return module.Universe.System_TypedReference;
                default:
                    return ReadTypeOrByRef(module, ref br, context);
            }
        }

        /// <summary>
        /// Writes a type, including the custom modifiers on its element types, as a Type production.
        /// </summary>
        /// <param name="module">The module being built, used to obtain tokens for the types.</param>
        /// <param name="bb">The blob builder to write to.</param>
        /// <param name="type">The type to write.</param>
        protected static void WriteType(ModuleBuilder module, BlobBuilder bb, Type type)
        {
            while (type.HasElementType)
            {
                var sigElementType = type.SigElementType;
                bb.WriteByte(sigElementType);
                if (sigElementType == ELEMENT_TYPE_ARRAY)
                {
                    // LAMESPEC the Type production (23.2.12) doesn't include CustomMod* for arrays, but the verifier allows it and ildasm also supports it
                    WriteCustomModifiers(module, bb, type.__GetCustomModifiers());
                    WriteType(module, bb, type.GetElementType());
                    bb.WriteCompressedInteger(type.GetArrayRank());

                    var sizes = type.__GetArraySizes();
                    bb.WriteCompressedInteger(sizes.Length);
                    for (var i = 0; i < sizes.Length; i++)
                        bb.WriteCompressedInteger(sizes[i]);

                    var lobounds = type.__GetArrayLowerBounds();
                    bb.WriteCompressedInteger(lobounds.Length);
                    for (var i = 0; i < lobounds.Length; i++)
                        bb.WriteCompressedSignedInteger(lobounds[i]);

                    return;
                }
                WriteCustomModifiers(module, bb, type.__GetCustomModifiers());
                type = type.GetElementType();
            }

            if (type.__IsBuiltIn)
            {
                bb.WriteByte(type.SigElementType);
            }
            else if (type.IsGenericParameter)
            {
                bb.WriteByte(type.SigElementType);
                bb.WriteCompressedInteger(type.GenericParameterPosition);
            }
            else if (!type.__IsMissing && type.IsGenericType)
            {
                WriteGenericSignature(module, bb, type);
            }
            else if (type.IsFunctionPointer)
            {
                bb.WriteByte(ELEMENT_TYPE_FNPTR);
                WriteStandAloneMethodSig(module, bb, type.__MethodSignature);
            }
            else
            {
                if (type.IsValueType)
                    bb.WriteByte(ELEMENT_TYPE_VALUETYPE);
                else
                    bb.WriteByte(ELEMENT_TYPE_CLASS);

                bb.WriteTypeDefOrRefEncoded(module.GetTypeToken(type).Token);
            }
        }

        /// <summary>
        /// Writes a generic type instantiation, with the custom modifiers on its type arguments.
        /// </summary>
        /// <param name="module">The module being built, used to obtain tokens for the types.</param>
        /// <param name="bb">The blob builder to write to.</param>
        /// <param name="type">The constructed generic type to write.</param>
        static void WriteGenericSignature(ModuleBuilder module, BlobBuilder bb, Type type)
        {
            var typeArguments = type.GetGenericArguments();
            var customModifiers = type.__GetGenericArgumentsCustomModifiers();
            if (!type.IsGenericTypeDefinition)
                type = type.GetGenericTypeDefinition();

            bb.WriteByte(ELEMENT_TYPE_GENERICINST);
            if (type.IsValueType)
                bb.WriteByte(ELEMENT_TYPE_VALUETYPE);
            else
                bb.WriteByte(ELEMENT_TYPE_CLASS);

            bb.WriteTypeDefOrRefEncoded(module.GetTypeToken(type).Token);
            bb.WriteCompressedInteger(typeArguments.Length);
            for (var i = 0; i < typeArguments.Length; i++)
            {
                WriteCustomModifiers(module, bb, customModifiers[i]);
                WriteType(module, bb, typeArguments[i]);
            }
        }

        /// <summary>
        /// Writes a sequence of custom modifiers.
        /// </summary>
        /// <param name="module">The module being built, used to obtain tokens for the modifier types.</param>
        /// <param name="bb">The blob builder to write to.</param>
        /// <param name="modifiers">The custom modifiers to write.</param>
        protected static void WriteCustomModifiers(ModuleBuilder module, BlobBuilder bb, CustomModifiers modifiers)
        {
            foreach (var entry in modifiers)
            {
                bb.WriteByte(entry.IsRequired ? ELEMENT_TYPE_CMOD_REQD : ELEMENT_TYPE_CMOD_OPT);
                bb.WriteTypeDefOrRefEncoded(module.GetTypeTokenForMemberRef(entry.Type));
            }
        }

        /// <summary>
        /// Reads a TypeDefOrRefOrSpec coded index (ECMA-335 II.23.2.8) and resolves the type it refers to.
        /// </summary>
        /// <param name="module">The module the signature belongs to.</param>
        /// <param name="br">The reader positioned at the coded index.</param>
        /// <param name="context">The generic context used to resolve a TypeSpec.</param>
        /// <returns>The resolved type.</returns>
        /// <exception cref="BadImageFormatException">The coded index has an invalid tag.</exception>
        internal static Type ReadTypeDefOrRefEncoded(ModuleReader module, ref BlobReader br, IGenericContext context)
        {
            var encoded = br.ReadCompressedInteger();
            return (encoded & 3) switch
            {
                0 => module.ResolveType(((int)TableIndex.TypeDef << 24) + (encoded >> 2), null, null),
                1 => module.ResolveType(((int)TableIndex.TypeRef << 24) + (encoded >> 2), null, null),
                2 => module.ResolveType(((int)TableIndex.TypeSpec << 24) + (encoded >> 2), context),
                _ => throw new BadImageFormatException(),
            };
        }

        /// <summary>
        /// Writes a stand-alone method signature, as used by <c>calli</c> and function pointer types.
        /// </summary>
        /// <param name="module">The module being built, used to obtain tokens for the types.</param>
        /// <param name="bb">The blob builder to write to.</param>
        /// <param name="sig">The signature to write.</param>
        /// <exception cref="ArgumentOutOfRangeException">The unmanaged calling convention is not supported.</exception>
        internal static void WriteStandAloneMethodSig(ModuleBuilder module, BlobBuilder bb, __StandAloneMethodSig sig)
        {
            if (sig.IsUnmanaged)
            {
                switch (sig.UnmanagedCallingConvention)
                {
                    case CallingConvention.Cdecl:
                        bb.WriteByte((byte)0x01);   // C
                        break;
                    case CallingConvention.StdCall:
                    case CallingConvention.Winapi:
                        bb.WriteByte((byte)0x02);   // STDCALL
                        break;
                    case CallingConvention.ThisCall:
                        bb.WriteByte((byte)0x03);   // THISCALL
                        break;
                    case CallingConvention.FastCall:
                        bb.WriteByte((byte)0x04);   // FASTCALL
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("callingConvention");
                }
            }
            else
            {
                var callingConvention = sig.CallingConvention;
                byte flags = 0;
                if ((callingConvention & CallingConventions.HasThis) != 0)
                    flags |= HASTHIS;
                if ((callingConvention & CallingConventions.ExplicitThis) != 0)
                    flags |= EXPLICITTHIS;
                if ((callingConvention & CallingConventions.VarArgs) != 0)
                    flags |= VARARG;

                bb.WriteByte(flags);
            }

            var parameterTypes = sig.ParameterTypes;
            var optionalParameterTypes = sig.OptionalParameterTypes;
            bb.WriteCompressedInteger(parameterTypes.Length + optionalParameterTypes.Length);
            WriteCustomModifiers(module, bb, sig.GetReturnTypeCustomModifiers());
            WriteType(module, bb, sig.ReturnType);

            var index = 0;
            foreach (var t in parameterTypes)
            {
                WriteCustomModifiers(module, bb, sig.GetParameterCustomModifiers(index++));
                WriteType(module, bb, t);
            }

            // note that optional parameters are only allowed for managed signatures (but we don't enforce that)
            if (optionalParameterTypes.Length > 0)
            {
                bb.WriteByte(SENTINEL);
                foreach (var t in optionalParameterTypes)
                {
                    WriteCustomModifiers(module, bb, sig.GetParameterCustomModifiers(index++));
                    WriteType(module, bb, t);
                }
            }
        }

        /// <summary>
        /// Writes a TypeSpec signature.
        /// </summary>
        /// <param name="module">The module being built, used to obtain tokens for the types.</param>
        /// <param name="bb">The blob builder to write to.</param>
        /// <param name="type">The type to write.</param>
        internal static void WriteTypeSpec(ModuleBuilder module, BlobBuilder bb, Type type) => WriteType(module, bb, type);

        /// <summary>
        /// Writes a MethodSpec signature, the type arguments of a generic method instantiation.
        /// </summary>
        /// <param name="module">The module being built, used to obtain tokens for the types.</param>
        /// <param name="bb">The blob builder to write to.</param>
        /// <param name="genArgs">The type arguments.</param>
        internal static void WriteMethodSpec(ModuleBuilder module, BlobBuilder bb, Type[] genArgs)
        {
            bb.WriteByte(GENERICINST);
            bb.WriteCompressedInteger(genArgs.Length);
            foreach (var arg in genArgs)
                WriteType(module, bb, arg);
        }

        /// <summary>
        /// Binds the type parameters in each of the given types.
        /// </summary>
        /// <param name="binder">The binder that supplies the type arguments.</param>
        /// <param name="types">The types to bind, or <see langword="null"/>.</param>
        /// <returns>The bound types, or <see cref="Type.EmptyTypes"/> if <paramref name="types"/> is <see langword="null"/> or empty.</returns>
        protected static Type[] BindTypeParameters(IGenericBinder binder, Type[] types)
        {
            if (types == null || types.Length == 0)
                return Type.EmptyTypes;

            var expanded = new Type[types.Length];
            for (var i = 0; i < types.Length; i++)
                expanded[i] = types[i].BindTypeParameters(binder);

            return expanded;
        }

        /// <summary>
        /// Writes the signature accumulated by a <see cref="SignatureHelper"/>.
        /// </summary>
        /// <param name="module">The module being built, used to obtain tokens for the types.</param>
        /// <param name="bb">The blob builder to write to.</param>
        /// <param name="flags">The first byte of the signature; for a field signature no count follows.</param>
        /// <param name="paramCount">The number of parameters or locals.</param>
        /// <param name="args">
        /// The types to write; <see langword="null"/> writes <see cref="ELEMENT_TYPE_VOID"/> and marker types write their
        /// element type byte.
        /// </param>
        internal static void WriteSignatureHelper(ModuleBuilder module, BlobBuilder bb, byte flags, ushort paramCount, List<Type> args)
        {
            bb.WriteByte(flags);
            if (flags != FIELD)
                bb.WriteCompressedInteger(paramCount);

            foreach (var type in args)
            {
                if (type == null)
                    bb.WriteByte(ELEMENT_TYPE_VOID);
                else if (type is MarkerType)
                    bb.WriteByte(type.SigElementType);
                else
                    WriteType(module, bb, type);
            }
        }

    }

}
