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
    
    volatile ReferenceQueue queue;
    
    /* When active:   NULL
     *     pending:   this
     *    Enqueued:   next reference in queue (or this if last)
     *    Inactive:   this
     */
    @SuppressWarnings("rawtypes")
    volatile Reference<T> next;

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

    final boolean isActive()
    {
        return strongRef != null || (weakRef != null && weakRef.get_IsAlive());
    }

    final boolean isStrongOrNullRef()
    {
        return weakRef == null;
    }

    static native boolean noclassgc();

    /* -- Referent accessor and setters -- */

    /**
     * Returns this reference object's referent.  If this reference object has
     * been cleared, either by the program or by the garbage collector, then
     * this method returns <code>null</code>.
     *
     * @return   The object to which this reference refers, or
     *           <code>null</code> if this reference object has been cleared
     */
    public T get()
    {
        if (weakRef == null)
        {
            return strongRef;
        }
        return (T)weakRef.get_Target();
    }
    
    /**
     * Clears this reference object.  Invoking this method will not cause this
     * object to be enqueued.
     *
     * <p> This method is invoked only by Java code; when the garbage collector
     * clears references it does so directly, without invoking this method.
     */
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
    
    /**
     * Tells whether or not this reference object has been enqueued, either by
     * the program or by the garbage collector.  If this reference object was
     * not registered with a queue when it was created, then this method will
     * always return <code>false</code>.
     *
     * @return   <code>true</code> if and only if this reference object has
     *           been enqueued
     */
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
        queue.enqueue((Reference<T>)this);
        return queue == ReferenceQueue.ENQUEUED;
    }

    /**
     * Clears this reference object and adds it to the queue with which
     * it is registered, if any.
     *
     * <p> This method is invoked only by Java code; when the garbage collector
     * enqueues references it does so directly, without invoking this method.
     *
     * @return   <code>true</code> if this reference object was successfully
     *           enqueued; <code>false</code> if it was already enqueued or if
     *           it was not registered with a queue when it was created
     */
    public boolean enqueue() {
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
        return this.queue.enqueue((Reference<T>)this);
    }

    /**
     * Throws {@link CloneNotSupportedException}. A {@code Reference} cannot be
     * meaningfully cloned. Construct a new {@code Reference} instead.
     *
     * @apiNote This method is defined in Java SE 8 Maintenance Release 4.
     *
     * @return  never returns normally
     * @throws  CloneNotSupportedException always
     *
     * @since 8
     */
    @Override
    protected Object clone() throws CloneNotSupportedException {
        throw new CloneNotSupportedException();
    }

    /* -- Constructors -- */

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

}
