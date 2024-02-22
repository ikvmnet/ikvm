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

using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    public abstract class SignatureHelper
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

            internal override Type ReturnType
            {
                get { return args[0]; }
            }

            public override byte[] GetSignature()
            {
                throw new NotSupportedException();
            }

            internal override ByteBuffer GetSignature(ModuleBuilder module)
            {
                var bb = new ByteBuffer(16);
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
            readonly ByteBuffer bb = new ByteBuffer(16);
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

                bb.Write(type);
                if (type != Signature.FIELD)
                    bb.Write((byte)0); // space for parameterCount
            }

            internal override Type ReturnType
            {
                get { return returnType; }
            }

            public override byte[] GetSignature()
            {
                return GetSignature(null).ToArray();
            }

            internal override ByteBuffer GetSignature(ModuleBuilder module)
            {
                if (type != Signature.FIELD)
                {
                    bb.Position = 1;
                    bb.Insert(MetadataWriter.GetCompressedUIntLength(argumentCount) - bb.GetCompressedUIntLength());
                    bb.WriteCompressedUInt(argumentCount);
                }

                return bb;
            }

            public override void AddSentinel()
            {
                bb.Write(Signature.SENTINEL);
            }

            public override void __AddArgument(Type argument, bool pinned, CustomModifiers customModifiers)
            {
                if (pinned)
                    bb.Write(Signature.ELEMENT_TYPE_PINNED);

                foreach (var mod in customModifiers)
                {
                    bb.Write(mod.IsRequired ? Signature.ELEMENT_TYPE_CMOD_REQD : Signature.ELEMENT_TYPE_CMOD_OPT);
                    Signature.WriteTypeSpec(module, bb, mod.Type);
                }

                Signature.WriteTypeSpec(module, bb, argument ?? module.Universe.System_Void);
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

        internal bool HasThis
        {
            get { return (type & Signature.HASTHIS) != 0; }
        }

        internal abstract Type ReturnType
        {
            get;
        }

        internal int ArgumentCount 
        {
            get { return argumentCount; }
        }

        private static SignatureHelper Create(Module mod, byte type, Type returnType)
        {
            return mod is not ModuleBuilder mb ? new Lazy(type) : new Eager(mb, type, returnType);
        }

        public static SignatureHelper GetFieldSigHelper(Module mod)
        {
            return Create(mod, Signature.FIELD, null);
        }

        public static SignatureHelper GetLocalVarSigHelper()
        {
            return new Lazy(Signature.LOCAL_SIG);
        }

        public static SignatureHelper GetLocalVarSigHelper(Module mod)
        {
            return Create(mod, Signature.LOCAL_SIG, null);
        }

        public static SignatureHelper GetPropertySigHelper(Module mod, Type returnType, Type[] parameterTypes)
        {
            var sig = Create(mod, Signature.PROPERTY, returnType);
            sig.AddArgument(returnType);
            sig.argumentCount = 0;
            sig.AddArguments(parameterTypes, null, null);
            return sig;
        }

        public static SignatureHelper GetPropertySigHelper(Module mod, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
        {
            return GetPropertySigHelper(mod, CallingConventions.Standard, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
        }

        public static SignatureHelper GetPropertySigHelper(Module mod, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
        {
            var type = Signature.PROPERTY;
            if ((callingConvention & CallingConventions.HasThis) != 0)
                type |= Signature.HASTHIS;

            var sig = Create(mod, type, returnType);
            sig.AddArgument(returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers);
            sig.argumentCount = 0;
            sig.AddArguments(parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
            return sig;
        }

        public static SignatureHelper GetMethodSigHelper(CallingConvention unmanagedCallingConvention, Type returnType)
        {
            return GetMethodSigHelper(null, unmanagedCallingConvention, returnType);
        }

        public static SignatureHelper GetMethodSigHelper(CallingConventions callingConvention, Type returnType)
        {
            return GetMethodSigHelper(null, callingConvention, returnType);
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

        public static SignatureHelper GetMethodSigHelper(Module mod, Type returnType, Type[] parameterTypes)
        {
            var sig = Create(mod, 0, returnType);
            sig.AddArgument(returnType);
            sig.argumentCount = 0;
            sig.AddArguments(parameterTypes, null, null);
            return sig;
        }

        public abstract byte[] GetSignature();

        internal abstract ByteBuffer GetSignature(ModuleBuilder module);

        public abstract void AddSentinel();

        public void AddArgument(Type clsArgument)
        {
            AddArgument(clsArgument, false);
        }

        public void AddArgument(Type argument, bool pinned)
        {
            __AddArgument(argument, pinned, new CustomModifiers());
        }

        public void AddArgument(Type argument, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
        {
            __AddArgument(argument, false, CustomModifiers.FromReqOpt(requiredCustomModifiers, optionalCustomModifiers));
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
