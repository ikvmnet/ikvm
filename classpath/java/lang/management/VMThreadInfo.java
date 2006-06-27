/* VMThreadInfo.java - Information on a thread
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

package java.lang.management;

/**
 * Provides low-level information about a thread.
 *
 * @author Andrew John Hughes (gnu_andrew@member.fsf.org)
 * @since 1.5
 * @see java.lang.management.ThreadInfo
 */
final class VMThreadInfo
{

    /**
     * Return the number of times the specified thread
     * has been in the {@link java.lang.Thread.State#BLOCKED}
     * state.
     *
     * @param thread the thread to return statistics on.
     * @return the number of times the thread has been blocked.
     */
    static long getBlockedCount(Thread thread)
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns the accumulated number of milliseconds the
     * specified thread has spent in the
     * {@link java.lang.Thread.State#BLOCKED} state.  This
     * method is only called if thread contention monitoring
     * is both supported and enabled.
     *
     * @param thread the thread to return statistics on.
     * @return the accumulated number of milliseconds the
     *         thread has been blocked for.
     */
    static long getBlockedTime(Thread thread)
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns the monitor lock the specified thread is
     * waiting for.  This is only called when the thread
     * is in the {@link java.lang.Thread.State#BLOCKED}
     * state.
     *
     * @param thread the thread to return statistics on.
     * @return the monitor lock the thread is waiting for.
     */
    static Object getLock(Thread thread)
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns the thread which currently owns the monitor
     * lock the specified thread is waiting for.  This is
     * only called when the thread is in the
     * {@link java.lang.Thread.State#BLOCKED} state.  It
     * may return <code>null</code> if the lock is not held
     * by another thread.
     *
     * @param thread the thread to return statistics on.
     * @return the thread which owns the monitor lock the
     *         thread is waiting for, or <code>null</code>
     *         if it doesn't have an owner.
     */
    static Thread getLockOwner(Thread thread)
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Return the number of times the specified thread
     * has been in the {@link java.lang.Thread.State#WAITING}
     * or {@link java.lang.Thread.State#TIMED_WAITING} states.
     *
     * @param thread the thread to return statistics on.
     * @return the number of times the thread has been in 
     *         waiting state.
     */
    static long getWaitedCount(Thread thread)
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns the accumulated number of milliseconds the
     * specified thread has spent in either the
     * {@link java.lang.Thread.State#WAITING} or
     * {@link java.lang.Thread.State#TIMED_WAITING} states.
     * This method is only called if thread contention
     * monitoring is both supported and enabled.
     *
     * @param thread the thread to return statistics on.
     * @return the accumulated number of milliseconds the
     *         thread has been in a waiting state for.
     */
    static long getWaitedTime(Thread thread)
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns true if the specified thread is in a native
     * method.
     *
     * @param thread the thread to return statistics on.
     * @return true if the thread is in a native method, false
     *         otherwise.
     */
    static boolean isInNative(Thread thread)
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns true if the specified thread is suspended.
     *
     * @param thread the thread to return statistics on.
     * @return true if the thread is suspended, false otherwise.
     */
    static boolean isSuspended(Thread thread)
    {
        throw new UnsupportedOperationException();
    }

    /**
     * Returns a stack trace for the specified thread of
     * the supplied depth.  If the depth is
     * <code>Integer.MAX_VALUE</code>, then the returned
     * depth is equal to the full stack trace available.
     * The depth will be greater than zero, due to
     * filtering in methods prior to this call.
     *
     * @param thread the thread whose stack trace should
     *               be returned.
     * @param maxDepth the maximum depth of the trace.
     *                 This will be greater than zero.
     *                 <code>Integer.MAX_VALUE</code>
     *                 represents the full trace.
     */
    static StackTraceElement[] getStackTrace(Thread thread, int maxDepth)
    {
        throw new UnsupportedOperationException();
    }
}
