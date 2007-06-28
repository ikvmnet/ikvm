/*
  Copyright (C) 2003, 2004, 2005, 2006, 2007 Jeroen Frijters

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

import ikvm.lang.Internal;

@Internal
public final class VMThread
{
    public static native void jniDetach();

    @Internal
    public static interface InterruptProc extends sun.nio.ch.Interruptible
    {
    }

    public static void enterInterruptableWait(InterruptProc proc) throws InterruptedException
    {
	Thread.currentThread().blockedOn(proc);
	if (Thread.interrupted())
	{
	    Thread.currentThread().blockedOn(null);
	    throw new InterruptedException();
	}
    }

    public static void leaveInterruptableWait() throws InterruptedException
    {
	Thread.currentThread().blockedOn(null);
	if (Thread.interrupted())
	{
	    throw new InterruptedException();
	}
    }

    // these are used by gnu.java.lang.management.VMThreadMXBeanImpl
    public static native cli.System.Threading.Thread getNativeThread(Thread t);
    public static native Thread getThreadFromId(long id);
}
