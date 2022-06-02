/*
  Copyright (C) 2011-2015 Jeroen Frijters

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

static class Java_java_lang_invoke_MethodHandle
{
	public static object invokeExact(java.lang.invoke.MethodHandle thisObject, object[] args)
	{
#if FIRST_PASS
		return null;
#else
		return IKVM.Runtime.ByteCodeHelper.GetDelegateForInvokeExact<IKVM.Runtime.MH<object[], object>>(thisObject)(args);
#endif
	}

	public static object invoke(java.lang.invoke.MethodHandle thisObject, object[] args)
	{
#if FIRST_PASS
		return null;
#else
		return thisObject.invokeWithArguments(args);
#endif
	}

	public static object invokeBasic(java.lang.invoke.MethodHandle thisObject, object[] args)
	{
		throw new InvalidOperationException();
	}

	public static object linkToVirtual(object[] args)
	{
		throw new InvalidOperationException();
	}

	public static object linkToStatic(object[] args)
	{
		throw new InvalidOperationException();
	}

	public static object linkToSpecial(object[] args)
	{
		throw new InvalidOperationException();
	}

	public static object linkToInterface(object[] args)
	{
		throw new InvalidOperationException();
	}
}
