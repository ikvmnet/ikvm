/*
  Copyright (C) 2002 Jeroen Frijters

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
using System.Reflection;

public class ByteCodeHelper
{
	public static object multianewarray(RuntimeTypeHandle typeHandle, int[] lengths)
	{
		for(int i = 0; i < lengths.Length; i++)
		{
			if(lengths[i] < 0)
			{
				throw JavaException.NegativeArraySizeException();
			}
		}
		return MultianewarrayHelper(Type.GetTypeFromHandle(typeHandle).GetElementType(), lengths, 0);
	}

	private static object MultianewarrayHelper(Type elemType, int[] lengths, int index)
	{
		object o = Array.CreateInstance(elemType, lengths[index++]);
		if(index < lengths.Length)
		{
			elemType = elemType.GetElementType();
			object[] a = (object[])o;
			for(int i = 0; i < a.Length; i++)
			{
				a[i] = MultianewarrayHelper(elemType, lengths, index);
			}
		}
		return o;
	}

	public static object DynamicInvokeSpecialNew(RuntimeTypeHandle type, string clazz, string name, string sig, object[] args)
	{
		ClassLoaderWrapper classLoader = ClassLoaderWrapper.GetWrapperFromType(Type.GetTypeFromHandle(type)).GetClassLoader();
		TypeWrapper wrapper = classLoader.LoadClassByDottedNameFast(clazz);
		if(wrapper == null)
		{
			throw JavaException.NoClassDefFoundError(clazz);
		}
		wrapper.Finish();
		// TODO who checks that the arg types are loadable?
		// TODO check accessibility
		MethodWrapper mw = wrapper.GetMethodWrapper(new MethodDescriptor(classLoader, name, sig), false);
		if(mw == null)
		{
			// TODO throw the appropriate exception
			throw new NotImplementedException("constructor missing");
		}
		return mw.Invoke(null, args, false);
	}

	public static int f2i(float f)
	{
		if(f <= int.MinValue)
		{
			return int.MinValue;
		}
		if(f >= int.MaxValue)
		{
			return int.MaxValue;
		}
		if(f != f)
		{
			return 0;
		}
		return (int)f;
	}

	public static long f2l(float f)
	{
		if(f <= long.MinValue)
		{
			return long.MinValue;
		}
		if(f >= long.MaxValue)
		{
			return long.MaxValue;
		}
		if(f != f)
		{
			return 0;
		}
		return (long)f;
	}

	public static int d2i(double d)
	{
		if(d <= int.MinValue)
		{
			return int.MinValue;
		}
		if(d >= int.MaxValue)
		{
			return int.MaxValue;
		}
		if(d != d)
		{
			return 0;
		}
		return (int)d;
	}

	public static long d2l(double d)
	{
		if(d <= long.MinValue)
		{
			return long.MinValue;
		}
		if(d >= long.MaxValue)
		{
			return long.MaxValue;
		}
		if(d != d)
		{
			return 0;
		}
		return (long)d;
	}
}
