/*
  Copyright (C) 2014 Jeroen Frijters

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

import sun.misc.Cleaner;

public abstract class Reference<T>
{
    private final cli.System.WeakReference weakRef;
    T strongRef;
    ReferenceQueue queue;
    Reference<T> next;

    Reference(T referent)
    {
        this(referent, null);
    }

    Reference(T referent, ReferenceQueue queue)
    {
        this.queue = queue == null ? ReferenceQueue.NULL : queue;
        if (referent instanceof Class && noclassgc())
        {
            // We don't do Class gc, so no point in using a weak reference for classes.
            weakRef = null;
            strongRef = referent;
        }
        else if (referent == null)
        {
            weakRef = null;
        }
        else
        {
            weakRef = new cli.System.WeakReference(referent, this instanceof PhantomReference);
            if (queue != null)
            {
                queue.addToActiveList(this);
            }
            if (this instanceof Cleaner)
            {
                new CleanerGuard();
            }
        }
    }

    private final class CleanerGuard
    {
        protected void finalize()
        {
            if (isActive()) {
                cli.System.GC.ReRegisterForFinalize(this);
            } else {
                ((Cleaner)Reference.this).clean();
            }
        }
    }

    public T get()
    {
        if (weakRef == null)
        {
            return strongRef;
        }
        return (T)weakRef.get_Target();
    }

    public void clear()
    {
        if (weakRef != null)
        {
            try
            {
                if (false) throw new cli.System.InvalidOperationException();
                weakRef.set_Target(null);
            }
            catch (cli.System.InvalidOperationException x)
            {
                // we were already finalized
            }
        }
        strongRef = null;
        queue.clear(this);
    }
    
    public boolean isEnqueued()
    {
        if (queue == ReferenceQueue.ENQUEUED) {
            return true;
        }
        if (queue == ReferenceQueue.NULL) {
            return false;
        }
        if (isStrongOrNullRef() || isActive()) {
            return false;
        }
        queue.enqueue(this);
        return queue == ReferenceQueue.ENQUEUED;
    }

    public boolean enqueue()
    {
        return queue.enqueue(this);
    }

    final boolean isActive()
    {
        return strongRef != null || (weakRef != null && weakRef.get_IsAlive());
    }

    final boolean isStrongOrNullRef()
    {
        return weakRef == null;
    }

    static native boolean noclassgc();
}
