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
	public static object virtualclone(object o)
	{
		// TODO because Object.clone() is protected it is accessible from other classes in the java.lang package,
		// so when they clone an array, we end up here (instead of being redirected to System.Array.Clone(), which
		// the compiler normally does because Object.clone() is inaccessible)
		if(o is Array)
		{
			return ((Array)o).Clone();
		}
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
		if(!(o is java.lang.Cloneable))
		{
			throw JavaException.CloneNotSupportedException();
		}
		return typeof(object).GetType().InvokeMember("MemberwiseClone", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null, o, new object[0]);
	}
}
