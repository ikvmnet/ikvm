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
using System.Collections.Generic;

static class Java_java_lang_invoke_MethodHandleImpl
{
	// hooked up via map.xml (as a replacement for makePairwiseConvertByEditor)
	public static java.lang.invoke.MethodHandle makePairwiseConvert(java.lang.invoke.MethodHandle target, global::java.lang.invoke.MethodType srcType, bool strict, bool monobox)
	{
#if FIRST_PASS
		return null;
#else
		object[] convSpecs = java.lang.invoke.MethodHandleImpl.computeValueConversions(srcType, target.type(), strict, monobox);
		List<java.lang.invoke.LambdaForm.Name> names = new List<java.lang.invoke.LambdaForm.Name>();
		names.Add(new java.lang.invoke.LambdaForm.Name(0, java.lang.invoke.LambdaForm.BasicType.L_TYPE));
		for (int i = 0; i < srcType.parameterCount(); i++)
		{
			names.Add(new java.lang.invoke.LambdaForm.Name(i + 1, java.lang.invoke.LambdaForm.BasicType.basicType(srcType.parameterType(i))));
		}
		java.lang.invoke.LambdaForm.Name[] invokeArgs = new java.lang.invoke.LambdaForm.Name[srcType.parameterCount()];
		for (int i = 0; i < invokeArgs.Length; i++)
		{
			object convSpec = convSpecs[i];
			if (convSpec == null)
			{
				invokeArgs[i] = names[i + 1];
			}
			else
			{
				java.lang.invoke.LambdaForm.Name temp = new java.lang.invoke.LambdaForm.Name(convSpec as java.lang.invoke.MethodHandle ?? java.lang.invoke.MethodHandleImpl.Lazy.MH_castReference.bindTo(convSpec), names[i + 1]);
				names.Add(temp);
				invokeArgs[i] = temp;
			}
		}
		names.Add(new java.lang.invoke.LambdaForm.Name(target, invokeArgs));
		if (convSpecs[convSpecs.Length - 1] != null)
		{
			object convSpec = convSpecs[convSpecs.Length - 1];
			if (convSpec != java.lang.Void.TYPE)
			{
				names.Add(new java.lang.invoke.LambdaForm.Name(convSpec as java.lang.invoke.MethodHandle ?? java.lang.invoke.MethodHandleImpl.Lazy.MH_castReference.bindTo(convSpec), names[names.Count - 1]));
			}
		}
		if (target.type().returnType() == java.lang.Void.TYPE && srcType.returnType() != java.lang.Void.TYPE)
		{
			names.Add(new java.lang.invoke.LambdaForm.Name(java.lang.invoke.LambdaForm.constantZero(java.lang.invoke.LambdaForm.BasicType.basicType(srcType.returnType()))));
		}
		java.lang.invoke.LambdaForm form = new java.lang.invoke.LambdaForm("PairwiseConvert", srcType.parameterCount() + 1, names.ToArray(), srcType.returnType() == java.lang.Void.TYPE ? java.lang.invoke.LambdaForm.VOID_RESULT : java.lang.invoke.LambdaForm.LAST_RESULT, false);
		return new java.lang.invoke.LightWeightMethodHandle(srcType, form);
#endif
	}
}
