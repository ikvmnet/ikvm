/* VMThreadMXBeanImpl.java - VM impl. of a thread bean
   Copyright (C) 2006 Free Software Foundation

This file is part of GNU Classpath.

GNU Classpath is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2, or (at your option)
any later version.

GNU Classpath is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with GNU Classpath; see the file COPYING.  If not, write to the
Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
02110-1301 USA.

Linking this library statically or dynamically with other modules is
making a combined work based on this library.  Thus, the terms and
conditions of the GNU General Public License cover the whole
combination.

As a special exception, the copyright holders of this library give you
permission to link this library with independent modules to produce an
executable, regardless of the license terms of these independent
modules, and to copy and distribute the resulting executable under
terms of your choice, provided that you also meet, for each linked
independent module, the terms and conditions of the license of that
module.  An independent module is a module which is not derived from
or based on this library.  If you modify this library, you may extend
this exception to your version of the library, but you are not
obligated to do so.  If you do not wish to do so, delete this
exception statement from your version. */

package gnu.java.lang.management;

import java.lang.management.ThreadInfo;
import java.lang.VMThread;
import java.lang.reflect.Constructor;

/**
 * Provides access to information about the threads 
 * of the virtual machine.  An instance of this bean is
 * obtained by calling
 * {@link ManagementFactory#getThreadMXBean()}.
 * See {@link java.lang.management.ThreadMXBean} for
 * full documentation.
 *
 * @author Andrew John Hughes (gnu_andrew@member.fsf.org)
 * @since 1.5
 */
final class VMThreadMXBeanImpl
{
    /**
     * Returns the ids of cycles of deadlocked threads, occurring
     * due to monitor ownership.
     *
     * @return the ids of the deadlocked threads.
     */
    static long[] findMonitorDeadlockedThreads()
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns the id of all live threads at the time of execution.
     *
     * @return the live thread ids.
     */
    static long[] getAllThreadIds()
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns the number of nanoseconds of CPU time
     * the current thread has used in total.   This is
     * only called if this feature is enabled and
     * supported.
     *
     * @return the nanoseconds of CPU time used by
     *         the current thread.
     */
    static long getCurrentThreadCpuTime()
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns the number of nanoseconds of user time
     * the current thread has used in total.   This is
     * only called if this feature is enabled and
     * supported.
     *
     * @return the nanoseconds of user time used by
     *         the current thread.
     */
    static long getCurrentThreadUserTime()
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns the number of live daemon threads.
     *
     * @return the number of live daemon threads.
     */
    static int getDaemonThreadCount()
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns the current peak number of live threads.
     *
     * @return the peak number of live threads.
     */
    static int getPeakThreadCount()
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns the number of live threads.
     *
     * @return the number of live threads.
     */
    static int getThreadCount()
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns the number of nanoseconds of CPU time
     * the specified thread has used in total.   This is
     * only called if this feature is enabled and
     * supported.
     *
     * @param id the thread to obtain statistics on.
     * @return the nanoseconds of CPU time used by
     *         the thread.
     */
    static long getThreadCpuTime(long id)
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns the {@link java.lang.management.ThreadInfo}
     * which corresponds to the specified id.
     *
     * @param id the id of the thread.
     * @param maxDepth the depth of the stack trace.
     * @return the corresponding <code>ThreadInfo</code>.
     */
    static ThreadInfo getThreadInfoForId(long id, int maxDepth)
    {
        try
        {
            Constructor constructor = ThreadInfo.class.getDeclaredConstructor(new Class[] {
                Thread.class,
                long.class,
                long.class,
                Object.class,
                Thread.class,
                long.class,
                long.class,
                boolean.class,
                boolean.class,
                StackTraceElement[].class
            });
            constructor.setAccessible(true);
            Thread thread = VMThread.getThreadFromId(id);
            return (ThreadInfo)constructor.newInstance(new Object[] {
                thread,
                -1L,
                -1L,
                null,
                null,
                -1L,
                -1L,
                false,
                false,
                getStackTrace(thread, maxDepth)
            });
        }
        catch(Exception x)
        {
            throw (InternalError)new InternalError().initCause(x);
        }
    }

    private static StackTraceElement[] getStackTrace(Thread thread, int maxDepth)
    {
        cli.System.Threading.Thread nativeThread = VMThread.getNativeThread(thread);
        if(nativeThread == null || maxDepth == 0)
        {
            return new StackTraceElement[0];
        }
        else if(nativeThread == cli.System.Threading.Thread.get_CurrentThread())
        {
            return ExceptionHelper.getStackTrace(new cli.System.Diagnostics.StackTrace(3, true), maxDepth);
        }
        else
        {
            cli.System.Diagnostics.StackTrace st = null;
            try
            {
                nativeThread.Suspend();
                try
                {
                    st = new cli.System.Diagnostics.StackTrace(nativeThread, true);
                }
                finally
                {
                    nativeThread.Resume();
                }
            }
            catch(Throwable _)
            {
                return new StackTraceElement[0];
            }
            return ExceptionHelper.getStackTrace(st, maxDepth);
        }
    }
  
    /**
     * Returns the number of nanoseconds of user time
     * the specified thread has used in total.   This is
     * only called if this feature is enabled and
     * supported.
     *
     * @param id the thread to obtain statistics on.
     * @return the nanoseconds of user time used by
     *         the thread.
     */
    static long getThreadUserTime(long id)
    {
        throw new UnsupportedOperationException();
    }
  
    /**
     * Returns the total number of threads that have
     * been started over the lifetime of the virtual
     * machine.
     *
     * @return the total number of threads started.
     */
    static long getTotalStartedThreadCount()
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Resets the peak thread count to the current
     * number of live threads.
     */
    static void resetPeakThreadCount()
    {
        throw new UnsupportedOperationException();
    }
}
