/*
  Copyright (C) 2011-2013 Jeroen Frijters

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
using IKVM.Internal;
#if STATIC_COMPILER
using Type = IKVM.Reflection.Type;
#endif

static partial class MethodHandleUtil
{
	internal const int MaxArity = 8;
	private static readonly Type typeofMHA;
	private static readonly Type[] typeofMHV;
	private static readonly Type[] typeofMH;

	static MethodHandleUtil()
	{
#if STATIC_COMPILER
		typeofMHA = StaticCompiler.GetRuntimeType("IKVM.Runtime.MHA`8");
		typeofMHV = new Type[] {
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`1"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`2"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`3"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`4"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`5"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`6"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`7"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MHV`8"),
		};
		typeofMH = new Type[] {
			null,
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`1"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`2"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`3"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`4"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`5"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`6"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`7"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`8"),
			StaticCompiler.GetRuntimeType("IKVM.Runtime.MH`9"),
		};
#else
		typeofMHA = typeof(IKVM.Runtime.MHA<,,,,,,,>);
		typeofMHV = new Type[] {
			typeof(IKVM.Runtime.MHV),
			typeof(IKVM.Runtime.MHV<>),
			typeof(IKVM.Runtime.MHV<,>),
			typeof(IKVM.Runtime.MHV<,,>),
			typeof(IKVM.Runtime.MHV<,,,>),
			typeof(IKVM.Runtime.MHV<,,,,>),
			typeof(IKVM.Runtime.MHV<,,,,,>),
			typeof(IKVM.Runtime.MHV<,,,,,,>),
			typeof(IKVM.Runtime.MHV<,,,,,,,>),
		};
		typeofMH = new Type[] {
			null,
			typeof(IKVM.Runtime.MH<>),
			typeof(IKVM.Runtime.MH<,>),
			typeof(IKVM.Runtime.MH<,,>),
			typeof(IKVM.Runtime.MH<,,,>),
			typeof(IKVM.Runtime.MH<,,,,>),
			typeof(IKVM.Runtime.MH<,,,,,>),
			typeof(IKVM.Runtime.MH<,,,,,,>),
			typeof(IKVM.Runtime.MH<,,,,,,,>),
			typeof(IKVM.Runtime.MH<,,,,,,,,>),
		};
#endif
	}

	internal static bool IsPackedArgsContainer(Type type)
	{
		return type.IsGenericType
			&& type.GetGenericTypeDefinition() == typeofMHA;
	}

	internal static Type CreateDelegateType(TypeWrapper[] args, TypeWrapper ret)
	{
		Type[] typeArgs = new Type[args.Length];
		for (int i = 0; i < args.Length; i++)
		{
			typeArgs[i] = args[i].TypeAsSignatureType;
		}
		return CreateDelegateType(typeArgs, ret.TypeAsSignatureType);
	}

	internal static Type CreateDelegateType(TypeWrapper tw, MethodWrapper mw)
	{
		TypeWrapper[] args = mw.GetParameters();
		if (!mw.IsStatic)
		{
			args = ArrayUtil.Concat(tw, args);
		}
		return CreateDelegateType(args, mw.ReturnType);
	}

	private static Type CreateDelegateType(Type[] types, Type retType)
	{
		if (types.Length == 0 && retType == Types.Void)
		{
			return typeofMHV[0];
		}
		else if (types.Length > MaxArity)
		{
			int arity = types.Length;
			int remainder = (arity - 8) % 7;
			int count = (arity - 8) / 7;
			if (remainder == 0)
			{
				remainder = 7;
				count--;
			}
			Type last = typeofMHA.MakeGenericType(SubArray(types, types.Length - 8, 8));
			for (int i = 0; i < count; i++)
			{
				Type[] temp = SubArray(types, types.Length - 8 - 7 * (i + 1), 8);
				temp[7] = last;
				last = typeofMHA.MakeGenericType(temp);
			}
			types = SubArray(types, 0, remainder + 1);
			types[remainder] = last;
		}
		if (retType == Types.Void)
		{
			return typeofMHV[types.Length].MakeGenericType(types);
		}
		else
		{
			types = ArrayUtil.Concat(types, retType);
			return typeofMH[types.Length].MakeGenericType(types);
		}
	}

	private static Type[] SubArray(Type[] inArray, int start, int length)
	{
		Type[] outArray = new Type[length];
		Array.Copy(inArray, start, outArray, 0, length);
		return outArray;
	}
}
