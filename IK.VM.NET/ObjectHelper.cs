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

public class ObjectHelper
{
	public static void notify(object o)
	{
		if(o == null)
		{
			throw new NullReferenceException();
		}
		System.Threading.Monitor.Pulse(o);
	}

	public static void notifyAll(object o)
	{
		if(o == null)
		{
			throw new NullReferenceException();
		}
		System.Threading.Monitor.PulseAll(o);
	}

	public static void wait(object o)
	{
		if(o == null)
		{
			throw new NullReferenceException();
		}
		System.Threading.Monitor.Wait(o);
	}

	public static void wait(object o, long timeout)
	{
		wait(o, timeout, 0);
	}

	public static void wait(object o, long timeout, int nanos)
	{
		if(o == null)
		{
			throw new NullReferenceException();
		}
		if(timeout == 0 && nanos == 0)
		{
			System.Threading.Monitor.Wait(o);
		}
		else
		{
			System.Threading.Monitor.Wait(o, new TimeSpan(timeout * 10000 + (nanos + 99) / 100));
		}
	}

	public static void clonecheck(object o)
	{
		if(!(o is java.lang.Cloneable))
		{
			throw (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.CloneNotSupportedException"));
		}
	}

	public static object virtualclone(object o)
	{
		// TODO because Object.clone() is protected it is accessible from other classes in the java.lang package,
		// so when they clone an array, we end up here (instead of being redirected to System.Array.Clone(), which
		// the compiler normally does because Object.clone() is inaccessible)
		if(o is Array)
		{
			return ((Array)o).Clone();
		}
		clonecheck(o);
		// TODO this doesn't happen very often, the only sensible pattern that I can think of that produces code
		// that ends up here is as follows:
		//   class Base {
		//      public Base CloneMe() { return (Base)clone(); }
		//   }
		//   case Derived extends Base {
		//      protected object clone() { ... }
		//   }
		// One way of implementing this is by calling the clone method thru reflection, not very fast, but
		// since this is an uncommon scenario, we might be able to get away with it
		throw new NotImplementedException("virtual clone invocation not implemented");
	}

	public static string toStringVirtual(object o)
	{
		if(o is Array)
		{
			return toStringSpecial(o);
		}
		try
		{
			return o.ToString();
		}
		catch(NullReferenceException)
		{
			return o.GetType().FullName;
		}
	}

	public static string toStringSpecial(object o)
	{
		// TODO hex string should be formatted differently
		return NativeCode.java.lang.Class.getName(o.GetType()) + "@" + o.GetHashCode().ToString("X");
	}

	public static object getClass(object o)
	{
		return NativeCode.java.lang.Class.getClassFromType(o.GetType());
	}
}
