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
using IKVM.Reflection;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{
	public sealed class SignatureHelper
	{
		private readonly ModuleBuilder module;
		private readonly List<Type> args = new List<Type>();
		private readonly byte type;
		private ushort paramCount;

		private SignatureHelper(ModuleBuilder module, byte type)
		{
			this.module = module;
			this.type = type;
		}

		internal bool HasThis
		{
			get { return (type & Signature.HASTHIS) != 0; }
		}

		internal Type ReturnType
		{
			get { return args[0]; }
		}

		internal int ParameterCount
		{
			get { return paramCount; }
		}

		public static SignatureHelper GetFieldSigHelper(Module mod)
		{
			return new SignatureHelper(mod as ModuleBuilder, Signature.FIELD);
		}

		public static SignatureHelper GetLocalVarSigHelper()
		{
			return new SignatureHelper(null, Signature.LOCAL_SIG);
		}

		public static SignatureHelper GetLocalVarSigHelper(Module mod)
		{
			return new SignatureHelper(mod as ModuleBuilder, Signature.LOCAL_SIG);
		}

		public static SignatureHelper GetPropertySigHelper(Module mod, Type returnType, Type[] parameterTypes)
		{
			SignatureHelper sig = new SignatureHelper(mod as ModuleBuilder, Signature.PROPERTY);
			sig.args.Add(returnType);
			sig.AddArguments(parameterTypes, null, null);
			return sig;
		}

		public static SignatureHelper GetPropertySigHelper(Module mod, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			return GetPropertySigHelper(mod, CallingConventions.Standard, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
		}

		public static SignatureHelper GetPropertySigHelper(Module mod, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			byte type = Signature.PROPERTY;
			if ((callingConvention & CallingConventions.HasThis) != 0)
			{
				type |= Signature.HASTHIS;
			}
			SignatureHelper sig = new SignatureHelper(mod as ModuleBuilder, type);
			sig.AddArgument(returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers);
			sig.paramCount = 0;
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
			byte type;
			switch (unmanagedCallConv)
			{
				case CallingConvention.Cdecl:
					type = 0x01;	// C
					break;
				case CallingConvention.StdCall:
				case CallingConvention.Winapi:
					type = 0x02;	// STDCALL
					break;
				case CallingConvention.ThisCall:
					type = 0x03;	// THISCALL
					break;
				case CallingConvention.FastCall:
					type = 0x04;	// FASTCALL
					break;
				default:
					throw new ArgumentOutOfRangeException("unmanagedCallConv");
			}
			SignatureHelper sig = new SignatureHelper(mod as ModuleBuilder, type);
			sig.args.Add(returnType);
			return sig;
		}

		public static SignatureHelper GetMethodSigHelper(Module mod, CallingConventions callingConvention, Type returnType)
		{
			byte type = 0;
			if ((callingConvention & CallingConventions.HasThis) != 0)
			{
				type |= Signature.HASTHIS;
			}
			if ((callingConvention & CallingConventions.ExplicitThis) != 0)
			{
				type |= Signature.EXPLICITTHIS;
			}
			if ((callingConvention & CallingConventions.VarArgs) != 0)
			{
				type |= Signature.VARARG;
			}
			SignatureHelper sig = new SignatureHelper(mod as ModuleBuilder, type);
			sig.args.Add(returnType);
			return sig;
		}

		public static SignatureHelper GetMethodSigHelper(Module mod, Type returnType, Type[] parameterTypes)
		{
			SignatureHelper sig = new SignatureHelper(mod as ModuleBuilder, 0);
			sig.args.Add(returnType);
			sig.AddArguments(parameterTypes, null, null);
			return sig;
		}

		public byte[] GetSignature()
		{
			if (module == null)
			{
				throw new NotSupportedException();
			}
			return GetSignature(module).ToArray();
		}

		internal ByteBuffer GetSignature(ModuleBuilder module)
		{
			ByteBuffer bb = new ByteBuffer(16);
			Signature.WriteSignatureHelper(module, bb, type, paramCount, args);
			return bb;
		}

		public void AddSentinel()
		{
			args.Add(MarkerType.Sentinel);
		}

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

		public void __AddArgument(Type argument, bool pinned, CustomModifiers customModifiers)
		{
			if (pinned)
			{
				args.Add(MarkerType.Pinned);
			}
			foreach (CustomModifiers.Entry mod in customModifiers)
			{
				args.Add(mod.IsRequired ? MarkerType.ModReq : MarkerType.ModOpt);
				args.Add(mod.Type);
			}
			args.Add(argument);
			paramCount++;
		}

		public void AddArguments(Type[] arguments, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
		{
			if (arguments != null)
			{
				for (int i = 0; i < arguments.Length; i++)
				{
					__AddArgument(arguments[i], false, CustomModifiers.FromReqOpt(Util.NullSafeElementAt(requiredCustomModifiers, i), Util.NullSafeElementAt(optionalCustomModifiers, i)));
				}
			}
		}
	}
}
