/*
  Copyright (C) 2003 Jeroen Frijters

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
package java.lang;

public class ObjectHelper
{
    private ObjectHelper() {}

    public static void wait(Object o, long timeout)
    {
	wait(o, timeout, 0);
    }

    public static void wait(Object o, long timeout, int nanos)
    {
	if(o == null)
	{
	    throw new NullPointerException();
	}
	if(timeout < 0 || nanos < 0 || nanos > 999999)
	{
	    throw new IllegalArgumentException("argument out of range");
	}
	if(timeout == 0 && nanos == 0)
	{
	    system.threading.Monitor.Wait(o);
	}
	else
	{
	    // TODO handle time span calculation overflow
	    system.threading.Monitor.Wait(o, new system.TimeSpan(timeout * 10000 + (nanos + 99) / 100));
	}
    }

    public static String toStringSpecial(Object o)
    {
	return o.getClass().getName() + '@' + Integer.toHexString(o.hashCode());
    }
}
