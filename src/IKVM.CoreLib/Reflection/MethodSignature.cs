/*
  Copyright (C) 2009-2011 Jeroen Frijters

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

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection
{

    /// <summary>
    /// Represents a method signature from IL metadadata.
    /// </summary>
    sealed class MethodSignature : Signature
    {

        sealed class UnboundGenericMethodContext : IGenericContext
        {

            readonly IGenericContext original;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="original"></param>
            internal UnboundGenericMethodContext(IGenericContext original)
            {
                this.original = original;
            }

            public Type GetGenericTypeArgument(int index)
            {
                return original.GetGenericTypeArgument(index);
            }

            public Type GetGenericMethodArgument(int index)
            {
                return UnboundGenericMethodParameter.Make(index);
            }

        }

        sealed class Binder : IGenericBinder
        {

            readonly Type declaringType;
            readonly Type[] methodArgs;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            /// <param name="methodArgs"></param>
            internal Binder(Type declaringType, Type[] methodArgs)
            {
                this.declaringType = declaringType;
                this.methodArgs = methodArgs;
            }

            public Type BindTypeParameter(Type type)
            {
                return declaringType.GetGenericTypeArgument(type.GenericParameterPosition);
            }

            public Type BindMethodParameter(Type type)
            {
                if (methodArgs == null)
                    return type;

                return methodArgs[type.GenericParameterPosition];
            }
        }

        sealed class Unbinder : IGenericBinder
        {

            internal static readonly Unbinder Instance = new Unbinder();

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            Unbinder()
            {

            }

            public Type BindTypeParameter(Type type)
            {
                return type;
            }

            public Type BindMethodParameter(Type type)
            {
                return UnboundGenericMethodParameter.Make(type.GenericParameterPosition);
            }

        }

        readonly Type returnType;
        readonly Type[] parameterTypes;
        readonly PackedCustomModifiers modifiers;
        readonly CallingConventions callingConvention;
        readonly int genericParamCount;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="modifiers"></param>
        /// <param name="callingConvention"></param>
        /// <param name="genericParamCount"></param>
        internal MethodSignature(Type returnType, Type[] parameterTypes, PackedCustomModifiers modifiers, CallingConventions callingConvention, int genericParamCount)
        {
            this.returnType = returnType;
            this.parameterTypes = parameterTypes;
            this.modifiers = modifiers;
            this.callingConvention = callingConvention;
            this.genericParamCount = genericParamCount;
        }

        public override bool Equals(object obj)
        {
            var other = obj as MethodSignature;
            return other != null
                && other.callingConvention == callingConvention
                && other.genericParamCount == genericParamCount
                && other.returnType.Equals(returnType)
                && Util.ArrayEquals(other.parameterTypes, parameterTypes)
                && other.modifiers.Equals(modifiers);
        }

        public override int GetHashCode()
        {
            return genericParamCount ^ 77 * (int)callingConvention
                ^ 3 * returnType.GetHashCode()
                ^ Util.GetHashCode(parameterTypes) * 5
                ^ modifiers.GetHashCode() * 55;
        }

        internal static MethodSignature ReadSig(ModuleReader module, ByteReader br, IGenericContext context)
        {
            var flags = br.ReadByte();
            var callingConvention = (flags & 7) switch
            {
                DEFAULT => CallingConventions.Standard,
                VARARG => CallingConventions.VarArgs,
                _ => throw new BadImageFormatException(),
            };

            if ((flags & HASTHIS) != 0)
                callingConvention |= CallingConventions.HasThis;

            if ((flags & EXPLICITTHIS) != 0)
                callingConvention |= CallingConventions.ExplicitThis;

            var genericParamCount = 0;
            if ((flags & GENERIC) != 0)
            {
                genericParamCount = br.ReadCompressedUInt();
                context = new UnboundGenericMethodContext(context);
            }

            var paramCount = br.ReadCompressedUInt();
            CustomModifiers[] modifiers = null;
            PackedCustomModifiers.Pack(ref modifiers, 0, CustomModifiers.Read(module, br, context), paramCount + 1);
            var returnType = ReadRetType(module, br, context);

            var parameterTypes = new Type[paramCount];
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                if ((callingConvention & CallingConventions.VarArgs) != 0 && br.PeekByte() == SENTINEL)
                {
                    Array.Resize(ref parameterTypes, i);
                    if (modifiers != null)
                        Array.Resize(ref modifiers, i + 1);

                    break;
                }

                PackedCustomModifiers.Pack(ref modifiers, i + 1, CustomModifiers.Read(module, br, context), paramCount + 1);
                parameterTypes[i] = ReadParam(module, br, context);
            }

            return new MethodSignature(returnType, parameterTypes, PackedCustomModifiers.Wrap(modifiers), callingConvention, genericParamCount);
        }

        internal static __StandAloneMethodSig ReadStandAloneMethodSig(ModuleReader module, ByteReader br, IGenericContext context)
        {
            CallingConventions callingConvention = 0;
            System.Runtime.InteropServices.CallingConvention unmanagedCallingConvention = 0;
            bool unmanaged;

            var flags = br.ReadByte();
            switch (flags & 7)
            {
                case DEFAULT:
                    callingConvention = CallingConventions.Standard;
                    unmanaged = false;
                    break;
                case 0x01:  // C
                    unmanagedCallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl;
                    unmanaged = true;
                    break;
                case 0x02:  // STDCALL
                    unmanagedCallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall;
                    unmanaged = true;
                    break;
                case 0x03:  // THISCALL
                    unmanagedCallingConvention = System.Runtime.InteropServices.CallingConvention.ThisCall;
                    unmanaged = true;
                    break;
                case 0x04:  // FASTCALL
                    unmanagedCallingConvention = System.Runtime.InteropServices.CallingConvention.FastCall;
                    unmanaged = true;
                    break;
                case VARARG:
                    callingConvention = CallingConventions.VarArgs;
                    unmanaged = false;
                    break;
                default:
                    throw new BadImageFormatException();
            }

            if ((flags & HASTHIS) != 0)
                callingConvention |= CallingConventions.HasThis;
            if ((flags & EXPLICITTHIS) != 0)
                callingConvention |= CallingConventions.ExplicitThis;
            if ((flags & GENERIC) != 0)
                throw new BadImageFormatException();

            var paramCount = br.ReadCompressedUInt();
            CustomModifiers[] customModifiers = null;
            PackedCustomModifiers.Pack(ref customModifiers, 0, CustomModifiers.Read(module, br, context), paramCount + 1);

            var returnType = ReadRetType(module, br, context);

            var parameterTypes = new List<Type>();
            var optionalParameterTypes = new List<Type>();
            var curr = parameterTypes;
            for (int i = 0; i < paramCount; i++)
            {
                if (br.PeekByte() == SENTINEL)
                {
                    br.ReadByte();
                    curr = optionalParameterTypes;
                }

                PackedCustomModifiers.Pack(ref customModifiers, i + 1, CustomModifiers.Read(module, br, context), paramCount + 1);
                curr.Add(ReadParam(module, br, context));
            }

            return new __StandAloneMethodSig(unmanaged, unmanagedCallingConvention, callingConvention, returnType, parameterTypes.ToArray(), optionalParameterTypes.ToArray(), PackedCustomModifiers.Wrap(customModifiers));
        }

        internal int GetParameterCount()
        {
            return parameterTypes.Length;
        }

        internal Type GetParameterType(int index)
        {
            return parameterTypes[index];
        }

        internal Type GetReturnType(IGenericBinder binder)
        {
            return returnType.BindTypeParameters(binder);
        }

        internal CustomModifiers GetReturnTypeCustomModifiers(IGenericBinder binder)
        {
            return modifiers.GetReturnTypeCustomModifiers().Bind(binder);
        }

        internal Type GetParameterType(IGenericBinder binder, int index)
        {
            return parameterTypes[index].BindTypeParameters(binder);
        }

        internal CustomModifiers GetParameterCustomModifiers(IGenericBinder binder, int index)
        {
            return modifiers.GetParameterCustomModifiers(index).Bind(binder);
        }

        internal CallingConventions CallingConvention
        {
            get { return callingConvention; }
        }

        internal int GenericParameterCount
        {
            get { return genericParamCount; }
        }

        internal MethodSignature Bind(Type type, Type[] methodArgs)
        {
            var binder = new Binder(type, methodArgs);
            return new MethodSignature(returnType.BindTypeParameters(binder), BindTypeParameters(binder, parameterTypes), modifiers.Bind(binder), callingConvention, genericParamCount);
        }

        internal static MethodSignature MakeFromBuilder(Type returnType, Type[] parameterTypes, PackedCustomModifiers modifiers, CallingConventions callingConvention, int genericParamCount)
        {
            if (genericParamCount > 0)
            {
                returnType = returnType.BindTypeParameters(Unbinder.Instance);
                parameterTypes = BindTypeParameters(Unbinder.Instance, parameterTypes);
                modifiers = modifiers.Bind(Unbinder.Instance);
            }

            return new MethodSignature(returnType, parameterTypes, modifiers, callingConvention, genericParamCount);
        }

        internal bool MatchParameterTypes(MethodSignature other)
        {
            return Util.ArrayEquals(other.parameterTypes, parameterTypes);
        }

        internal bool MatchParameterTypes(Type[] types)
        {
            return Util.ArrayEquals(types, parameterTypes);
        }

        internal override void Write(ModuleBuilder module, ByteBuffer bb)
        {
            WriteImpl(module, bb, parameterTypes.Length);
        }

        internal void WriteMethodRef(ModuleBuilder module, ByteBuffer bb, Type[] optionalParameterTypes, CustomModifiers[] customModifiers)
        {
            WriteImpl(module, bb, parameterTypes.Length + optionalParameterTypes.Length);

            if (optionalParameterTypes.Length > 0)
            {
                bb.Write(SENTINEL);
                for (int i = 0; i < optionalParameterTypes.Length; i++)
                {
                    WriteCustomModifiers(module, bb, Util.NullSafeElementAt(customModifiers, i));
                    WriteType(module, bb, optionalParameterTypes[i]);
                }
            }
        }

        void WriteImpl(ModuleBuilder module, ByteBuffer bb, int parameterCount)
        {
            byte first;

            if ((callingConvention & CallingConventions.Any) == CallingConventions.VarArgs)
            {
                Debug.Assert(genericParamCount == 0);
                first = VARARG;
            }
            else if (genericParamCount > 0)
            {
                first = GENERIC;
            }
            else
            {
                first = DEFAULT;
            }

            if ((callingConvention & CallingConventions.HasThis) != 0)
                first |= HASTHIS;

            if ((callingConvention & CallingConventions.ExplicitThis) != 0)
                first |= EXPLICITTHIS;

            bb.Write(first);

            if (genericParamCount > 0)
                bb.WriteCompressedUInt(genericParamCount);

            bb.WriteCompressedUInt(parameterCount);
            // RetType
            WriteCustomModifiers(module, bb, modifiers.GetReturnTypeCustomModifiers());
            WriteType(module, bb, returnType);

            // Param
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                WriteCustomModifiers(module, bb, modifiers.GetParameterCustomModifiers(i));
                WriteType(module, bb, parameterTypes[i]);
            }
        }

    }

}
