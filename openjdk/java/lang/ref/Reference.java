/*
  Copyright (C) 2003-2012 Jeroen Frijters

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
package java.lang.ref;

import cli.System.Collections.Hashtable;
import sun.misc.Cleaner;

public abstract class Reference<T>
{
    // accessed by inner class
    volatile cli.System.WeakReference weakRef;
    volatile T strongRef;
    volatile ReferenceQueue<? super T> queue;
    volatile Reference next;
    
    private static native boolean noclassgc();

    Reference(T referent)
    {
        this(referent, null);
    }

    Reference(T referent, ReferenceQueue<? super T> queue)
    {
        this.queue = queue == null ? ReferenceQueue.NULL : queue;
        if (referent != null)
        {
            if (referent instanceof Class && noclassgc())
            {
                // We don't do Class gc, so no point in using a weak reference for classes.
                strongRef = referent;
            }
            else
            {
                weakRef = new cli.System.WeakReference(referent, this instanceof PhantomReference);
                if (queue != null || referent instanceof Cleaner || this instanceof SoftReference)
                {
                    new QueueWatcher(this);
                }
            }
        }
    }

    private static final boolean debug = false;

    private static final class QueueWatcher
    {
        private static final Hashtable keepAlive = Hashtable.Synchronized(new Hashtable());
        private cli.System.WeakReference handle;

        QueueWatcher(Reference r)
        {
            handle = new cli.System.WeakReference(r, true);
            // FXBUG when a WeakReference is finalizer reachable, it gets cleared by the GC (even if we call GC.SuppressFinalize),
            // so we have to maintain a strong reference to it to prevent it from being cleared.
            keepAlive.Add(handle, null);
        }

        boolean check(Reference r)
        {
            r.strongRef = null;
            boolean alive = false;
            try
            {
                if (false) throw new cli.System.InvalidOperationException();
                cli.System.WeakReference referent = r.weakRef;
                if (referent == null)
                {
                    // ref was explicitly cleared, so we don't enqueue
                    return false;
                }
                alive = referent.get_IsAlive();
            }
            catch (cli.System.InvalidOperationException x)
            {
                // this happens if the reference is already finalized (if we were
                // the only one still hanging on to it)
            }
            if (alive)
            {
                // we don't want to keep creating finalizable objects during shutdown
                if (!cli.System.Environment.get_HasShutdownStarted())
                {
                    return true;
                }
            }
            else
            {
                if (r instanceof Cleaner)
                {
                    ((Cleaner)r).clean();
                }
                else if (r.queue != null)
                {
                    r.queue.enqueue(r);
                }
            }
            return false;
        }

        protected void finalize()
        {
            Reference r = (Reference)handle.get_Target();
            if (debug)
                cli.System.Console.WriteLine("~QueueWatcher: " + hashCode() + " on " + r);
            if (r != null && r.next == null && check(r))
            {
                cli.System.GC.ReRegisterForFinalize(QueueWatcher.this);
            }
            else
            {
                handle.set_Target(null);
                keepAlive.Remove(handle);
            }
        }
    }

    public T get()
    {
        try
        {
            if (false) throw new cli.System.InvalidOperationException();
            cli.System.WeakReference referent = this.weakRef;
            if (referent == null)
            {
                return strongRef;
            }
            T value = (T)referent.get_Target();
            if (value == null)
            {
                queue.enqueue(this);
            }
            return value;
        }
        catch (cli.System.InvalidOperationException x)
        {
            // we were already finalized, so we just return null.
            return null;
        }
    }

    public void clear()
    {
        weakRef = null;
        strongRef = null;
    }

    public synchronized boolean isEnqueued()
    {
        return queue != ReferenceQueue.NULL && next != null;
    }

    public boolean enqueue() 
    {
        return queue.enqueue(this);
    }
}
