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
using IKVM.Internal;

static class Java_java_lang_invoke_DirectMethodHandle
{
	// this is called from DirectMethodHandle.makeAllocator() via a map.xml prologue patch
	public static java.lang.invoke.DirectMethodHandle makeStringAllocator(java.lang.invoke.MemberName member)
	{
#if FIRST_PASS
		return null;
#else
		// we cannot construct strings via the standard two-pass approach (allocateObject followed by constructor invocation),
		// so we special case string construction here (to call our static factory method instead)
		if (member.getDeclaringClass() == CoreClasses.java.lang.String.Wrapper.ClassObject)
		{
			java.lang.invoke.MethodType mt = member.getMethodType().changeReturnType(CoreClasses.java.lang.String.Wrapper.ClassObject);
			return new java.lang.invoke.DirectMethodHandle(mt, java.lang.invoke.DirectMethodHandle._preparedLambdaForm(mt, java.lang.invoke.MethodTypeForm.LF_INVSTATIC), member, null);
		}
		return null;
#endif
	}
}
