/*
  Copyright (C) 2006 Jeroen Frijters

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

package java.util.concurrent.locks;

public class LockSupport
{
    private LockSupport() {}

    public static void unpark(Thread thread)
    {
        VMThread.unpark(thread);
    }

    public static void park()
    {
        VMThread.park(Long.MAX_VALUE);
    }

    public static void park(Object blocker)
    {
        VMThread.park(blocker, Long.MAX_VALUE);
    }

    public static void parkNanos(long nanos)
    {
        VMThread.park(nanos);
    }

    public static void parkNanos(Object blocker, long nanos)
    {
        VMThread.park(blocker, nanos);
    }

    public static void parkUntil(long deadline)
    {
        long nanos = deadline - System.currentTimeMillis();
        if(nanos > 0)
        {
            if(nanos < Long.MAX_VALUE / 1000000)
            {
                nanos *= 1000000;
            }
            else
            {
                nanos = Long.MAX_VALUE;
            }
            VMThread.park(nanos);
        }
    }

    public static void parkUntil(Object blocker, long deadline)
    {
        long nanos = deadline - System.currentTimeMillis();
        if(nanos > 0)
        {
            if(nanos < Long.MAX_VALUE / 1000000)
            {
                nanos *= 1000000;
            }
            else
            {
                nanos = Long.MAX_VALUE;
            }
            VMThread.park(blocker, nanos);
        }
    }

    public static Object getBlocker(Thread t)
    {
        return VMThread.getBlocker(t);
    }
}
