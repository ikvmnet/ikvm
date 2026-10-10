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
using System.Reflection.Metadata;

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

            readonly IGenericContext _original;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="original"></param>
            internal UnboundGenericMethodContext(IGenericContext original)
            {
                _original = original;
            }

            public Type GetGenericTypeArgument(int index) => _original.GetGenericTypeArgument(index);

            public Type GetGenericMethodArgument(int index) => UnboundGenericMethodParameter.Make(index);

        }

        sealed class Binder : IGenericBinder
        {

            readonly Type _declaringType;
            readonly Type[] _methodArgs;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            /// <param name="methodArgs"></param>
            internal Binder(Type declaringType, Type[] methodArgs)
            {
                _declaringType = declaringType;
                _methodArgs = methodArgs;
            }

            public Type BindTypeParameter(Type type) => _declaringType.GetGenericTypeArgument(type.GenericParameterPosition);

            public Type BindMethodParameter(Type type)
            {
                if (_methodArgs == null)
                    return type;

                return _methodArgs[type.GenericParameterPosition];
            }
        }

        sealed class Unbinder : IGenericBinder
        {

            internal static readonly Unbinder Instance = new();

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            Unbinder()
            {

            }

            public Type BindTypeParameter(Type type) => type;

            public Type BindMethodParameter(Type type) => UnboundGenericMethodParameter.Make(type.GenericParameterPosition);

        }

        readonly Type _returnType;
        readonly Type[] _parameterTypes;
        readonly PackedCustomModifiers _modifiers;
        readonly CallingConventions _callingConvention;
        readonly int _genericParamCount;

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
            _returnType = returnType;
            _parameterTypes = parameterTypes;
            _modifiers = modifiers;
            _callingConvention = callingConvention;
            _genericParamCount = genericParamCount;
        }

        public override bool Equals(object obj)
        {
            return obj is MethodSignature other
                && other._callingConvention == _callingConvention
                && other._genericParamCount == _genericParamCount
                && other._returnType.Equals(_returnType)
                && Util.ArrayEquals(other._parameterTypes, _parameterTypes)
                && other._modifiers.Equals(_modifiers);
        }

        public override int GetHashCode()
        {
            return _genericParamCount ^ 77 * (int)_callingConvention
                ^ 3 * _returnType.GetHashCode()
                ^ Util.GetHashCode(_parameterTypes) * 5
                ^ _modifiers.GetHashCode() * 55;
        }

        internal static MethodSignature ReadSig(ModuleReader module, BlobReader br, IGenericContext context)
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
                genericParamCount = br.ReadCompressedInteger();
                context = new UnboundGenericMethodContext(context);
            }

            var paramCount = br.ReadCompressedInteger();
            CustomModifiers[] modifiers = null;
            PackedCustomModifiers.Pack(ref modifiers, 0, CustomModifiers.Read(module, ref br, context), paramCount + 1);
            var returnType = ReadRetType(module, ref br, context);

            var parameterTypes = new Type[paramCount];
            for (var i = 0; i < parameterTypes.Length; i++)
            {
                if ((callingConvention & CallingConventions.VarArgs) != 0 && br.PeekByte() == SENTINEL)
                {
                    Array.Resize(ref parameterTypes, i);
                    if (modifiers != null)
                        Array.Resize(ref modifiers, i + 1);

                    break;
                }

                PackedCustomModifiers.Pack(ref modifiers, i + 1, CustomModifiers.Read(module, ref br, context), paramCount + 1);
                parameterTypes[i] = ReadParam(module, ref br, context);
            }

            return new MethodSignature(returnType, parameterTypes, PackedCustomModifiers.Wrap(modifiers), callingConvention, genericParamCount);
        }

        internal static __StandAloneMethodSig ReadStandAloneMethodSig(ModuleReader module, ref BlobReader br, IGenericContext context)
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

            var paramCount = br.ReadCompressedInteger();
            CustomModifiers[] customModifiers = null;
            PackedCustomModifiers.Pack(ref customModifiers, 0, CustomModifiers.Read(module, ref br, context), paramCount + 1);

            var returnType = ReadRetType(module, ref br, context);

            var parameterTypes = new List<Type>();
            var optionalParameterTypes = new List<Type>();
            var curr = parameterTypes;
            for (var i = 0; i < paramCount; i++)
            {
                if (br.PeekByte() == SENTINEL)
                {
                    br.ReadByte();
                    curr = optionalParameterTypes;
                }

                PackedCustomModifiers.Pack(ref customModifiers, i + 1, CustomModifiers.Read(module, ref br, context), paramCount + 1);
                curr.Add(ReadParam(module, ref br, context));
            }

            return new __StandAloneMethodSig(unmanaged, unmanagedCallingConvention, callingConvention, returnType, parameterTypes.ToArray(), optionalParameterTypes.ToArray(), PackedCustomModifiers.Wrap(customModifiers));
        }

        internal int GetParameterCount() => _parameterTypes.Length;

        internal Type GetParameterType(int index) => _parameterTypes[index];

        internal Type GetReturnType(IGenericBinder binder) => _returnType.BindTypeParameters(binder);

        internal CustomModifiers GetReturnTypeCustomModifiers(IGenericBinder binder) => _modifiers.GetReturnTypeCustomModifiers().Bind(binder);

        internal Type GetParameterType(IGenericBinder binder, int index) => _parameterTypes[index].BindTypeParameters(binder);

        internal CustomModifiers GetParameterCustomModifiers(IGenericBinder binder, int index) =>
            _modifiers.GetParameterCustomModifiers(index).Bind(binder);

        internal CallingConventions CallingConvention => _callingConvention;

        internal int GenericParameterCount => _genericParamCount;

        internal MethodSignature Bind(Type type, Type[] methodArgs)
        {
            var binder = new Binder(type, methodArgs);
            return new MethodSignature(_returnType.BindTypeParameters(binder), BindTypeParameters(binder, _parameterTypes), _modifiers.Bind(binder), _callingConvention, _genericParamCount);
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

        internal bool MatchParameterTypes(MethodSignature other) => Util.ArrayEquals(other._parameterTypes, _parameterTypes);

        internal bool MatchParameterTypes(Type[] types) => Util.ArrayEquals(types, _parameterTypes);

        internal override void Write(ModuleBuilder module, BlobBuilder bb) => WriteImpl(module, bb, _parameterTypes.Length);

        internal void WriteMethodRef(ModuleBuilder module, BlobBuilder bb, Type[] optionalParameterTypes, CustomModifiers[] customModifiers)
        {
            WriteImpl(module, bb, _parameterTypes.Length + optionalParameterTypes.Length);

            if (optionalParameterTypes.Length > 0)
            {
                bb.WriteByte(SENTINEL);
                for (var i = 0; i < optionalParameterTypes.Length; i++)
                {
                    WriteCustomModifiers(module, bb, Util.NullSafeElementAt(customModifiers, i));
                    WriteType(module, bb, optionalParameterTypes[i]);
                }
            }
        }

        void WriteImpl(ModuleBuilder module, BlobBuilder bb, int parameterCount)
        {
            byte first;

            if ((_callingConvention & CallingConventions.Any) == CallingConventions.VarArgs)
            {
                Debug.Assert(_genericParamCount == 0);
                first = VARARG;
            }
            else if (_genericParamCount > 0)
            {
                first = GENERIC;
            }
            else
            {
                first = DEFAULT;
            }

            if ((_callingConvention & CallingConventions.HasThis) != 0)
                first |= HASTHIS;

            if ((_callingConvention & CallingConventions.ExplicitThis) != 0)
                first |= EXPLICITTHIS;

            bb.WriteByte(first);

            if (_genericParamCount > 0)
                bb.WriteCompressedInteger(_genericParamCount);

            bb.WriteCompressedInteger(parameterCount);
            // RetType
            WriteCustomModifiers(module, bb, _modifiers.GetReturnTypeCustomModifiers());
            WriteType(module, bb, _returnType);

            // Param
            for (var i = 0; i < _parameterTypes.Length; i++)
            {
                WriteCustomModifiers(module, bb, _modifiers.GetParameterCustomModifiers(i));
                WriteType(module, bb, _parameterTypes[i]);
            }
        }

    }

}
