/*
  Copyright (C) 2008-2012 Jeroen Frijters

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
using System.Runtime.InteropServices;

using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Provides methods for building signatures.
    /// </summary>
    abstract class SignatureHelper
    {

        /// <summary>
        /// The leading byte of the signature: the calling convention, or <see cref="Signature.FIELD"/> or <see cref="Signature.LOCAL_SIG"/>.
        /// </summary>
        protected readonly byte type;

        /// <summary>
        /// The number of arguments added so far, written as the parameter or local count of the signature.
        /// </summary>
        protected ushort argumentCount;

        /// <summary>
        /// A signature helper for a module that is not a <see cref="ModuleBuilder"/>; it records the arguments and only encodes
        /// them when the signature is written to a <see cref="ModuleBuilder"/>.
        /// </summary>
        sealed class Lazy : SignatureHelper
        {

            /// <summary>
            /// The recorded arguments, interleaved with <see cref="MarkerType"/> entries for pinned, sentinel and
            /// custom modifier markers.
            /// </summary>
            readonly List<Type> _args = [];

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type">The leading byte of the signature.</param>
            internal Lazy(byte type) :
                base(type)
            {
            }

            /// <inheritdoc />
            public override byte[] GetSignature() => throw new NotSupportedException();

            /// <inheritdoc />
            internal override BlobBuilder GetSignature(ModuleBuilder module)
            {
                var bb = new BlobBuilder(16);
                Signature.WriteSignatureHelper(module, bb, type, argumentCount, _args);
                return bb;
            }

            /// <inheritdoc />
            public override void AddSentinel() => _args.Add(MarkerType.Sentinel);

            /// <inheritdoc />
            public override void __AddArgument(Type argument, bool pinned, CustomModifiers customModifiers)
            {
                if (pinned)
                    _args.Add(MarkerType.Pinned);

                foreach (var mod in customModifiers)
                {
                    _args.Add(mod.IsRequired ? MarkerType.ModReq : MarkerType.ModOpt);
                    _args.Add(mod.Type);
                }

                _args.Add(argument);
                argumentCount++;
            }

        }

        /// <summary>
        /// A signature helper for a <see cref="ModuleBuilder"/>; it encodes each argument as soon as it is added.
        /// </summary>
        sealed class Eager : SignatureHelper
        {

            readonly ModuleBuilder _module;
            readonly BlobBuilder _arguments = new(16);
            readonly Type _returnType;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="module">The module the signature is built for.</param>
            /// <param name="type">The leading byte of the signature.</param>
            /// <param name="returnType">The return type of the signature, or <see langword="null"/>.</param>
            internal Eager(ModuleBuilder module, byte type, Type returnType) :
                base(type)
            {
                _module = module;
                _returnType = returnType;
            }

            /// <inheritdoc />
            public override byte[] GetSignature() => GetSignature(null).ToArray();

            /// <summary>
            /// Gets the signature encoded so far. The arguments were already encoded for the module this helper was
            /// created for, so <paramref name="module"/> is ignored.
            /// </summary>
            /// <param name="module">Ignored.</param>
            /// <returns>A blob builder containing the encoded signature.</returns>
            internal override BlobBuilder GetSignature(ModuleBuilder module)
            {
                // the argument count precedes the arguments but is only known once they have all been added
                var bb = new BlobBuilder(16 + _arguments.Count);
                bb.WriteByte(type);
                if (type != Signature.FIELD)
                    bb.WriteCompressedInteger(argumentCount);

                _arguments.WriteContentTo(bb);
                return bb;
            }

            /// <inheritdoc />
            public override void AddSentinel() => _arguments.WriteByte(Signature.SENTINEL);

            /// <inheritdoc />
            public override void __AddArgument(Type argument, bool pinned, CustomModifiers customModifiers)
            {
                if (pinned)
                    _arguments.WriteByte(Signature.ELEMENT_TYPE_PINNED);

                foreach (var mod in customModifiers)
                {
                    _arguments.WriteByte(mod.IsRequired ? Signature.ELEMENT_TYPE_CMOD_REQD : Signature.ELEMENT_TYPE_CMOD_OPT);
                    Signature.WriteTypeSpec(_module, _arguments, mod.Type);
                }

                Signature.WriteTypeSpec(_module, _arguments, argument ?? _module.Universe.System_Void);
                argumentCount++;
            }

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type">The leading byte of the signature.</param>
        SignatureHelper(byte type)
        {
            this.type = type;
        }

        /// <summary>
        /// Creates a signature helper that encodes eagerly when <paramref name="mod"/> is a <see cref="ModuleBuilder"/> and
        /// lazily otherwise.
        /// </summary>
        /// <param name="mod">The module for which the signature is built.</param>
        /// <param name="type">The leading byte of the signature.</param>
        /// <param name="returnType">The return type of the signature, or <see langword="null"/>.</param>
        /// <returns>The signature helper.</returns>
        static SignatureHelper Create(Module mod, byte type, Type returnType) =>
            mod is not ModuleBuilder mb ? new Lazy(type) : new Eager(mb, type, returnType);

        /// <summary>
        /// Returns a signature helper for a field.
        /// </summary>
        /// <param name="mod">The dynamic module that contains the field for which the signature helper is requested.</param>
        /// <returns>The signature helper object.</returns>
        public static SignatureHelper GetFieldSigHelper(Module mod) => Create(mod, Signature.FIELD, null);

        /// <summary>
        /// Returns a signature helper for a local variable.
        /// </summary>
        /// <param name="mod">The dynamic module that contains the local variable for which the signature helper is requested.</param>
        /// <returns>The signature helper object.</returns>
        public static SignatureHelper GetLocalVarSigHelper(Module mod) => Create(mod, Signature.LOCAL_SIG, null);

        /// <summary>
        /// Returns a signature helper for a method given the method's module, unmanaged calling convention, and return type.
        /// </summary>
        /// <param name="mod">The module that contains the method for which the signature helper is requested.</param>
        /// <param name="unmanagedCallConv">The unmanaged calling convention of the method.</param>
        /// <param name="returnType">The return type of the method, or <see langword="null"/> for a void return type.</param>
        /// <returns>The signature helper object.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="unmanagedCallConv"/> is not a supported unmanaged calling convention.</exception>
        public static SignatureHelper GetMethodSigHelper(Module mod, CallingConvention unmanagedCallConv, Type returnType)
        {
            var type = unmanagedCallConv switch
            {
                CallingConvention.Cdecl => (byte)0x01, // C
                CallingConvention.StdCall or CallingConvention.Winapi => (byte)0x02, // STDCALL
                CallingConvention.ThisCall => (byte)0x03, // THISCALL
                CallingConvention.FastCall => (byte)0x04, // FASTCALL
                _ => throw new ArgumentOutOfRangeException("unmanagedCallConv"),
            };

            var sig = Create(mod, type, returnType);
            sig.AddArgument(returnType);
            sig.argumentCount = 0;
            return sig;
        }

        /// <summary>
        /// Returns a signature helper for a method given the method's module, calling convention, and return type.
        /// </summary>
        /// <param name="mod">The module that contains the method for which the signature helper is requested.</param>
        /// <param name="callingConvention">The calling convention of the method.</param>
        /// <param name="returnType">The return type of the method, or <see langword="null"/> for a void return type.</param>
        /// <returns>The signature helper object.</returns>
        public static SignatureHelper GetMethodSigHelper(Module mod, CallingConventions callingConvention, Type returnType)
        {
            byte type = 0;

            if ((callingConvention & CallingConventions.HasThis) != 0)
                type |= Signature.HASTHIS;

            if ((callingConvention & CallingConventions.ExplicitThis) != 0)
                type |= Signature.EXPLICITTHIS;

            if ((callingConvention & CallingConventions.VarArgs) != 0)
                type |= Signature.VARARG;

            var sig = Create(mod, type, returnType);
            sig.AddArgument(returnType);
            sig.argumentCount = 0;
            return sig;
        }

        /// <summary>
        /// Gets the signature built so far.
        /// </summary>
        /// <returns>A byte array made up of the full signature.</returns>
        /// <exception cref="NotSupportedException">The signature helper was not created for a <see cref="ModuleBuilder"/>.</exception>
        public abstract byte[] GetSignature();

        /// <summary>
        /// Encodes the signature for use in <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module the signature is written to.</param>
        /// <returns>A blob builder containing the encoded signature.</returns>
        internal abstract BlobBuilder GetSignature(ModuleBuilder module);

        /// <summary>
        /// Marks the end of a vararg fixed part. This is only to be used if the user is calling a varargs method.
        /// </summary>
        public abstract void AddSentinel();

        /// <summary>
        /// Adds an argument to the signature.
        /// </summary>
        /// <param name="clsArgument">The type of the argument.</param>
        public void AddArgument(Type clsArgument) => AddArgument(clsArgument, false);

        /// <summary>
        /// Adds an argument of the specified type to the signature, specifying whether the argument is pinned.
        /// </summary>
        /// <param name="argument">The argument type.</param>
        /// <param name="pinned"><see langword="true"/> if the argument is pinned; otherwise, <see langword="false"/>.</param>
        public void AddArgument(Type argument, bool pinned) => __AddArgument(argument, pinned, new CustomModifiers());

        /// <summary>
        /// Adds an argument of the specified type to the signature, specifying whether the argument is pinned and which custom
        /// modifiers apply to it.
        /// </summary>
        /// <param name="argument">The argument type, or <see langword="null"/> for <see cref="System.Void"/>.</param>
        /// <param name="pinned"><see langword="true"/> if the argument is pinned; otherwise, <see langword="false"/>.</param>
        /// <param name="customModifiers">The custom modifiers of the argument.</param>
        public abstract void __AddArgument(Type argument, bool pinned, CustomModifiers customModifiers);

        /// <summary>
        /// Adds a set of arguments to the signature, with the specified custom modifiers.
        /// </summary>
        /// <param name="arguments">The types of the arguments to be added.</param>
        /// <param name="requiredCustomModifiers">An array of arrays of types. Each array of types represents the required custom modifiers for the corresponding argument.</param>
        /// <param name="optionalCustomModifiers">An array of arrays of types. Each array of types represents the optional custom modifiers for the corresponding argument.</param>
        public void AddArguments(Type[] arguments, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
        {
            if (arguments != null)
                for (var i = 0; i < arguments.Length; i++)
                    __AddArgument(arguments[i], false, CustomModifiers.FromReqOpt(Util.NullSafeElementAt(requiredCustomModifiers, i), Util.NullSafeElementAt(optionalCustomModifiers, i)));
        }

    }

}
