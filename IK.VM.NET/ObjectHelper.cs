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

public class ObjectHelper
{
	public static void notify(object o)
	{
		System.Threading.Monitor.Pulse(o);
	}

	public static void notifyAll(object o)
	{
		System.Threading.Monitor.PulseAll(o);
	}

	public static void wait(object o)
	{
		System.Threading.Monitor.Wait(o);
	}

	public static void wait(object o, long timeout)
	{
		if(timeout < 0)
		{
			throw JavaException.IllegalArgumentException("timeout < 0");
		}
		wait(o, timeout, 0);
	}

	public static void wait(object o, long timeout, int nanos)
	{
		if(o == null)
		{
			throw new NullReferenceException();
		}
		if(timeout < 0 || nanos < 0 || nanos > 999999)
		{
			throw JavaException.IllegalArgumentException("argument out of range");
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
			throw JavaException.CloneNotSupportedException();
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
		MethodInfo clone = o.GetType().GetMethod("clone", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, Type.EmptyTypes, null);
		if(clone != null)
		{
			return clone.Invoke(o, new object[0]);
		}
		return typeof(object).GetType().InvokeMember("MemberwiseClone", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null, o, new object[0]);
	}

	public static string toStringVirtual(object o)
	{
		if(o is Array)
		{
			return toStringSpecial(o);
		}
		return o.ToString();
	}

	private delegate string toHexStringDelegate(int i);
	private static toHexStringDelegate toHexString;

	public static string toStringSpecial(object o)
	{
		if(toHexString == null)
		{
			toHexString = (toHexStringDelegate)Delegate.CreateDelegate(typeof(toHexStringDelegate), ClassLoaderWrapper.GetType("java.lang.Integer").GetMethod("toHexString"));
		}
		int h;
		if(o is string)
		{
			h = StringHelper.hashCode(o);
		}
		else
		{
			h = o.GetHashCode();
		}
		return NativeCode.java.lang.Class.getName(o.GetType()) + "@" + toHexString(h);
	}

	public static object getClass(object o)
	{
		return NativeCode.java.lang.Class.getClassFromType(o.GetType());
	}
}
