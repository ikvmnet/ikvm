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
using System.Runtime.InteropServices;
using System.Reflection.Metadata;

using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    internal abstract class SignatureHelper
    {

        protected readonly byte type;
        protected ushort argumentCount;

        sealed class Lazy : SignatureHelper
        {

            readonly List<Type> args = new List<Type>();

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type"></param>
            internal Lazy(byte type) :
                base(type)
            {
            }

            public override byte[] GetSignature()
            {
                throw new NotSupportedException();
            }

            internal override BlobBuilder GetSignature(ModuleBuilder module)
            {
                var bb = new BlobBuilder(16);
                Signature.WriteSignatureHelper(module, bb, type, argumentCount, args);
                return bb;
            }

            public override void AddSentinel()
            {
                args.Add(MarkerType.Sentinel);
            }

            public override void __AddArgument(Type argument, bool pinned, CustomModifiers customModifiers)
            {
                if (pinned)
                    args.Add(MarkerType.Pinned);

                foreach (var mod in customModifiers)
                {
                    args.Add(mod.IsRequired ? MarkerType.ModReq : MarkerType.ModOpt);
                    args.Add(mod.Type);
                }

                args.Add(argument);
                argumentCount++;
            }

        }

        sealed class Eager : SignatureHelper
        {

            readonly ModuleBuilder module;
            readonly BlobBuilder arguments = new BlobBuilder(16);
            readonly Type returnType;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="module"></param>
            /// <param name="type"></param>
            /// <param name="returnType"></param>
            internal Eager(ModuleBuilder module, byte type, Type returnType) :
                base(type)
            {
                this.module = module;
                this.returnType = returnType;
            }

            public override byte[] GetSignature()
            {
                return GetSignature(null).ToArray();
            }

            internal override BlobBuilder GetSignature(ModuleBuilder module)
            {
                // the argument count precedes the arguments but is only known once they have all been added
                var bb = new BlobBuilder(16 + arguments.Count);
                bb.WriteByte(type);
                if (type != Signature.FIELD)
                    bb.WriteCompressedInteger(argumentCount);

                arguments.WriteContentTo(bb);
                return bb;
            }

            public override void AddSentinel()
            {
                arguments.WriteByte(Signature.SENTINEL);
            }

            public override void __AddArgument(Type argument, bool pinned, CustomModifiers customModifiers)
            {
                if (pinned)
                    arguments.WriteByte(Signature.ELEMENT_TYPE_PINNED);

                foreach (var mod in customModifiers)
                {
                    arguments.WriteByte(mod.IsRequired ? Signature.ELEMENT_TYPE_CMOD_REQD : Signature.ELEMENT_TYPE_CMOD_OPT);
                    Signature.WriteTypeSpec(module, arguments, mod.Type);
                }

                Signature.WriteTypeSpec(module, arguments, argument ?? module.Universe.System_Void);
                argumentCount++;
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        SignatureHelper(byte type)
        {
            this.type = type;
        }

        private static SignatureHelper Create(Module mod, byte type, Type returnType)
        {
            return mod is not ModuleBuilder mb ? new Lazy(type) : new Eager(mb, type, returnType);
        }

        public static SignatureHelper GetFieldSigHelper(Module mod)
        {
            return Create(mod, Signature.FIELD, null);
        }

        public static SignatureHelper GetLocalVarSigHelper(Module mod)
        {
            return Create(mod, Signature.LOCAL_SIG, null);
        }

        public static SignatureHelper GetMethodSigHelper(Module mod, CallingConvention unmanagedCallConv, Type returnType)
        {
            var type = unmanagedCallConv switch
            {
                CallingConvention.Cdecl => (byte)0x01,// C
                CallingConvention.StdCall or CallingConvention.Winapi => (byte)0x02,// STDCALL
                CallingConvention.ThisCall => (byte)0x03,// THISCALL
                CallingConvention.FastCall => (byte)0x04,// FASTCALL
                _ => throw new ArgumentOutOfRangeException("unmanagedCallConv"),
            };

            var sig = Create(mod, type, returnType);
            sig.AddArgument(returnType);
            sig.argumentCount = 0;
            return sig;
        }

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

        public abstract byte[] GetSignature();

        internal abstract BlobBuilder GetSignature(ModuleBuilder module);

        public abstract void AddSentinel();

        public void AddArgument(Type clsArgument)
        {
            AddArgument(clsArgument, false);
        }

        public void AddArgument(Type argument, bool pinned)
        {
            __AddArgument(argument, pinned, new CustomModifiers());
        }

        public abstract void __AddArgument(Type argument, bool pinned, CustomModifiers customModifiers);

        public void AddArguments(Type[] arguments, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
        {
            if (arguments != null)
                for (int i = 0; i < arguments.Length; i++)
                    __AddArgument(arguments[i], false, CustomModifiers.FromReqOpt(Util.NullSafeElementAt(requiredCustomModifiers, i), Util.NullSafeElementAt(optionalCustomModifiers, i)));
        }

    }

}
