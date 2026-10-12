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
    /// Represents a method signature from IL metadata: the calling convention, generic parameter count, return type,
    /// parameter types and their custom modifiers. Within a signature, the type parameters of a generic method are
    /// kept unbound so that signatures of a generic method definition and of its declarations compare equal.
    /// </summary>
    sealed class MethodSignature : Signature
    {

        /// <summary>
        /// A generic context used while reading the signature of a generic method. Type parameters of the declaring
        /// type come from the original context, while method type parameters are left unbound.
        /// </summary>
        sealed class UnboundGenericMethodContext : IGenericContext
        {

            readonly IGenericContext _original;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="original">The context that supplies the type arguments of the declaring type.</param>
            internal UnboundGenericMethodContext(IGenericContext original)
            {
                _original = original;
            }

            /// <summary>
            /// Gets the type argument at the specified position from the original context.
            /// </summary>
            /// <param name="index">The zero-based position of the type parameter.</param>
            /// <returns>The type argument supplied by the original context.</returns>
            public Type GetGenericTypeArgument(int index) => _original.GetGenericTypeArgument(index);

            /// <summary>
            /// Gets an unbound generic method parameter at the specified position.
            /// </summary>
            /// <param name="index">The zero-based position of the method type parameter.</param>
            /// <returns>The <see cref="UnboundGenericMethodParameter"/> for <paramref name="index"/>.</returns>
            public Type GetGenericMethodArgument(int index) => UnboundGenericMethodParameter.Make(index);

        }

        /// <summary>
        /// Binds the type parameters in a signature to the type arguments of a declaring type and, optionally, to the
        /// type arguments of a generic method instantiation.
        /// </summary>
        sealed class Binder : IGenericBinder
        {

            readonly Type _declaringType;
            readonly Type[] _methodArgs;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType">The type that supplies the type arguments for type parameters.</param>
            /// <param name="methodArgs">The type arguments for method type parameters, or <see langword="null"/> to leave them unbound.</param>
            internal Binder(Type declaringType, Type[] methodArgs)
            {
                _declaringType = declaringType;
                _methodArgs = methodArgs;
            }

            /// <summary>
            /// Binds a type parameter to the corresponding type argument of the declaring type.
            /// </summary>
            /// <param name="type">The type parameter to bind.</param>
            /// <returns>The type argument of the declaring type at the parameter's position.</returns>
            public Type BindTypeParameter(Type type) => _declaringType.GetGenericTypeArgument(type.GenericParameterPosition);

            /// <summary>
            /// Binds a method type parameter to the corresponding method type argument.
            /// </summary>
            /// <param name="type">The method type parameter to bind.</param>
            /// <returns>The method type argument at the parameter's position, or <paramref name="type"/> itself if no method type arguments were given.</returns>
            public Type BindMethodParameter(Type type)
            {
                if (_methodArgs == null)
                    return type;

                return _methodArgs[type.GenericParameterPosition];
            }

        }

        /// <summary>
        /// Replaces the method type parameters in a signature with unbound generic method parameters and leaves type
        /// parameters alone. Used by <see cref="MakeFromBuilder"/> to normalize the signatures of generic methods.
        /// </summary>
        sealed class Unbinder : IGenericBinder
        {

            /// <summary>
            /// The single shared instance.
            /// </summary>
            internal static readonly Unbinder Instance = new();

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            Unbinder()
            {
            }

            /// <summary>
            /// Leaves a type parameter unchanged.
            /// </summary>
            /// <param name="type">The type parameter.</param>
            /// <returns><paramref name="type"/>.</returns>
            public Type BindTypeParameter(Type type) => type;

            /// <summary>
            /// Replaces a method type parameter with the unbound generic method parameter at the same position.
            /// </summary>
            /// <param name="type">The method type parameter.</param>
            /// <returns>The <see cref="UnboundGenericMethodParameter"/> at the position of <paramref name="type"/>.</returns>
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
        /// <param name="returnType">The return type.</param>
        /// <param name="parameterTypes">The parameter types. The array is kept, not copied.</param>
        /// <param name="modifiers">The custom modifiers of the return type and parameters.</param>
        /// <param name="callingConvention">The calling convention.</param>
        /// <param name="genericParamCount">The number of generic method parameters, or 0 for a non-generic method.</param>
        internal MethodSignature(Type returnType, Type[] parameterTypes, PackedCustomModifiers modifiers, CallingConventions callingConvention, int genericParamCount)
        {
            _returnType = returnType;
            _parameterTypes = parameterTypes;
            _modifiers = modifiers;
            _callingConvention = callingConvention;
            _genericParamCount = genericParamCount;
        }

        /// <summary>
        /// Determines whether the specified object is a method signature with the same calling convention, generic
        /// parameter count, return type, parameter types and custom modifiers.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the signatures are equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is MethodSignature other
                && other._callingConvention == _callingConvention
                && other._genericParamCount == _genericParamCount
                && other._returnType.Equals(_returnType)
                && Util.ArrayEquals(other._parameterTypes, _parameterTypes)
                && other._modifiers.Equals(_modifiers);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _genericParamCount ^ 77 * (int)_callingConvention
                ^ 3 * _returnType.GetHashCode()
                ^ Util.GetHashCode(_parameterTypes) * 5
                ^ _modifiers.GetHashCode() * 55;
        }

        /// <summary>
        /// Reads a MethodDefSig or MethodRefSig blob. For a vararg signature, reading stops at the sentinel, so only
        /// the fixed parameters are returned.
        /// </summary>
        /// <param name="module">The module the signature is read from.</param>
        /// <param name="br">The reader positioned at the start of the signature.</param>
        /// <param name="context">The generic context used to resolve type parameters.</param>
        /// <returns>The method signature.</returns>
        /// <exception cref="BadImageFormatException">The calling convention is not a managed default or vararg convention.</exception>
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

        /// <summary>
        /// Reads a StandAloneMethodSig blob, as used by <c>calli</c>, including unmanaged calling conventions and the
        /// optional parameters after a vararg sentinel.
        /// </summary>
        /// <param name="module">The module the signature is read from.</param>
        /// <param name="br">The reader positioned at the start of the signature.</param>
        /// <param name="context">The generic context used to resolve type parameters.</param>
        /// <returns>The stand-alone method signature.</returns>
        /// <exception cref="BadImageFormatException">The calling convention is not recognized, or the signature is marked generic.</exception>
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

        /// <summary>
        /// Gets the number of parameters in the signature.
        /// </summary>
        /// <returns>The number of parameters.</returns>
        internal int GetParameterCount() => _parameterTypes.Length;

        /// <summary>
        /// Gets the unbound type of the parameter at the specified position.
        /// </summary>
        /// <param name="index">The zero-based position of the parameter.</param>
        /// <returns>The parameter type as stored in the signature.</returns>
        internal Type GetParameterType(int index) => _parameterTypes[index];

        /// <summary>
        /// Gets the return type with its type parameters bound by the specified binder.
        /// </summary>
        /// <param name="binder">The binder that supplies the generic arguments.</param>
        /// <returns>The bound return type.</returns>
        internal Type GetReturnType(IGenericBinder binder) => _returnType.BindTypeParameters(binder);

        /// <summary>
        /// Gets the custom modifiers of the return type with their type parameters bound by the specified binder.
        /// </summary>
        /// <param name="binder">The binder that supplies the generic arguments.</param>
        /// <returns>The bound custom modifiers of the return type.</returns>
        internal CustomModifiers GetReturnTypeCustomModifiers(IGenericBinder binder) => _modifiers.GetReturnTypeCustomModifiers().Bind(binder);

        /// <summary>
        /// Gets the type of the parameter at the specified position with its type parameters bound by the specified
        /// binder.
        /// </summary>
        /// <param name="binder">The binder that supplies the generic arguments.</param>
        /// <param name="index">The zero-based position of the parameter.</param>
        /// <returns>The bound parameter type.</returns>
        internal Type GetParameterType(IGenericBinder binder, int index) => _parameterTypes[index].BindTypeParameters(binder);

        /// <summary>
        /// Gets the custom modifiers of the parameter at the specified position with their type parameters bound by the
        /// specified binder.
        /// </summary>
        /// <param name="binder">The binder that supplies the generic arguments.</param>
        /// <param name="index">The zero-based position of the parameter.</param>
        /// <returns>The bound custom modifiers of the parameter.</returns>
        internal CustomModifiers GetParameterCustomModifiers(IGenericBinder binder, int index) =>
            _modifiers.GetParameterCustomModifiers(index).Bind(binder);

        /// <summary>
        /// Gets the calling convention of the signature.
        /// </summary>
        internal CallingConventions CallingConvention => _callingConvention;

        /// <summary>
        /// Gets the number of generic method parameters, or 0 for a non-generic method.
        /// </summary>
        internal int GenericParameterCount => _genericParamCount;

        /// <summary>
        /// Creates a copy of the signature with its type parameters bound to the type arguments of the specified type
        /// and, optionally, its method type parameters bound to the specified method type arguments.
        /// </summary>
        /// <param name="type">The type that supplies the type arguments.</param>
        /// <param name="methodArgs">The method type arguments, or <see langword="null"/> to leave method type parameters unbound.</param>
        /// <returns>The bound signature.</returns>
        internal MethodSignature Bind(Type type, Type[] methodArgs)
        {
            var binder = new Binder(type, methodArgs);
            return new MethodSignature(_returnType.BindTypeParameters(binder), BindTypeParameters(binder, _parameterTypes), _modifiers.Bind(binder), _callingConvention, _genericParamCount);
        }

        /// <summary>
        /// Creates a signature from the types given to a builder. For a generic method, the method's own type
        /// parameters are replaced with unbound generic method parameters so the signature compares equal to one read
        /// from metadata.
        /// </summary>
        /// <param name="returnType">The return type.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <param name="modifiers">The custom modifiers of the return type and parameters.</param>
        /// <param name="callingConvention">The calling convention.</param>
        /// <param name="genericParamCount">The number of generic method parameters, or 0 for a non-generic method.</param>
        /// <returns>The method signature.</returns>
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

        /// <summary>
        /// Determines whether another signature has the same parameter types, ignoring everything else.
        /// </summary>
        /// <param name="other">The signature to compare with.</param>
        /// <returns><see langword="true"/> if the parameter types are equal; otherwise, <see langword="false"/>.</returns>
        internal bool MatchParameterTypes(MethodSignature other) => Util.ArrayEquals(other._parameterTypes, _parameterTypes);

        /// <summary>
        /// Determines whether the signature has the specified parameter types.
        /// </summary>
        /// <param name="types">The parameter types to compare with.</param>
        /// <returns><see langword="true"/> if the parameter types are equal; otherwise, <see langword="false"/>.</returns>
        internal bool MatchParameterTypes(Type[] types) => Util.ArrayEquals(types, _parameterTypes);

        /// <summary>
        /// Writes the signature as a MethodDefSig blob.
        /// </summary>
        /// <param name="module">The module the signature is written for, used to obtain type tokens.</param>
        /// <param name="bb">The blob builder to write to.</param>
        internal override void Write(ModuleBuilder module, BlobBuilder bb) => WriteImpl(module, bb, _parameterTypes.Length);

        /// <summary>
        /// Writes the signature as a MethodRefSig blob, appending the optional vararg parameters after a sentinel.
        /// </summary>
        /// <param name="module">The module the signature is written for, used to obtain type tokens.</param>
        /// <param name="bb">The blob builder to write to.</param>
        /// <param name="optionalParameterTypes">The types of the optional vararg parameters.</param>
        /// <param name="customModifiers">The custom modifiers of the optional parameters, or <see langword="null"/>.</param>
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

        /// <summary>
        /// Writes the calling convention, generic parameter count, parameter count, return type and fixed parameters.
        /// </summary>
        /// <param name="module">The module the signature is written for, used to obtain type tokens.</param>
        /// <param name="bb">The blob builder to write to.</param>
        /// <param name="parameterCount">The parameter count to write, which includes any optional vararg parameters.</param>
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
