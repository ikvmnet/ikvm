/*
  Copyright (C) 2008 Jeroen Frijters

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

package ikvm.internal;

import cli.System.Activator;
import cli.System.Reflection.BindingFlags;
import cli.System.Reflection.FieldInfo;
import cli.System.Reflection.MethodInfo;
import cli.System.Type;

final class MonoUtils
{
    static String unameProperty(String field)
    {
        Type syscallType = Type.GetType("Mono.Unix.Native.Syscall, Mono.Posix, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
        Type utsnameType = Type.GetType("Mono.Unix.Native.Utsname, Mono.Posix, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
	if (syscallType != null && utsnameType != null)
	{
	    Object[] arg = new Object[] { Activator.CreateInstance(utsnameType) };
	    MethodInfo uname = syscallType.GetMethod("uname", new Type[] { utsnameType.MakeByRefType() });
	    FieldInfo fi = utsnameType.GetField(field);
	    if (uname != null && fi != null)
	    {
		uname.Invoke(null, arg);
		return (String)fi.GetValue(arg[0]);
	    }
	}
	return null;
    }
}
