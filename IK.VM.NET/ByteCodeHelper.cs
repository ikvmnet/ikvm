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

	public static void monitorenter(object o)
	{
		if(o == null)
		{
			throw new NullReferenceException();
		}
		System.Threading.Monitor.Enter(o);
	}

	public static void monitorexit(object o)
	{
		if(o == null)
		{
			throw new NullReferenceException();
		}
		System.Threading.Monitor.Exit(o);
	}
}
