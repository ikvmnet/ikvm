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

public class ReferenceQueue<T>
{
    static final ReferenceQueue ENQUEUED = new ReferenceQueue();
    static final ReferenceQueue NULL = new ReferenceQueue();
    private volatile Reference<T> activeHead;
    private volatile Reference<T> head;
    final Object lock = new Object();
    volatile boolean waitingForGC;
    
    private final class GCNotification {
        protected void finalize() {
            waitingForGC = false;
            synchronized (lock) {
                lock.notifyAll();
            }
        }
    }

    public Reference<? extends T> poll()
    {
        if (head == null && (activeHead == null || waitingForGC)) {
            return null;
        }
        synchronized (lock) {
            return pollImpl();
        }
    }

    private Reference<? extends T> pollImpl()
    {
        if (head == null) {
            if (activeHead == null || waitingForGC) {
                return null;
            }
            scanActiveList();
            if (head == null) {
                waitingForGC = true;
                new GCNotification();
                return null;
            }
        }
        Reference<T> ref = head;
        head = ref.next;
        ref.next = null;
        ref.queue = NULL;
        return ref;
    }

    public Reference<? extends T> remove(long timeout) throws IllegalArgumentException, InterruptedException
    {
        if (timeout < 0)
            throw new IllegalArgumentException("Negative timeout value");
        
        synchronized (lock) {
            long expiration = 0;
            for (;;) {
                Reference<? extends T> ref = pollImpl();
                if (ref != null)
                    return ref;
                    
                if (timeout == 0) {
                    lock.wait();
                } else {
                    long now = System.currentTimeMillis();
                    if (expiration == 0) {
                        expiration = now + timeout;
                        if (expiration < 0) {
                            expiration = Long.MAX_VALUE;
                        }
                    }
                    if (now >= expiration) {
                        return null;
                    }
                    lock.wait(expiration - now);
                }
            }
        }
    }

    public Reference<? extends T> remove() throws InterruptedException
    {
        return remove(0);
    }

    final void clear(Reference<T> ref)
    {
        synchronized (lock) {
            if (ref.queue != ENQUEUED) {
                ref.queue = NULL;
            }
        }
    }

    final boolean enqueue(Reference<T> ref)
    {
        synchronized (lock) {
            if (ref.queue != ENQUEUED && ref.queue != NULL) {
                ref.queue = ENQUEUED;
                
                if (ref.isStrongOrNullRef()) {
                    ref.next = head;
                    head = ref;
                    lock.notifyAll();
                    return true;
                }

                Reference<T> prev = null;
                Reference<T> curr = activeHead;
            
                while (curr != null) {
                    if (curr == ref) {
                        if (prev == null) {
                            activeHead = curr.next;
                        } else {
                            prev.next = curr.next;
                        }
                        curr.next = head;
                        head = curr;
                        lock.notifyAll();
                        return true;
                    }
                    prev = curr;
                    curr = curr.next;
                }
            }
        }
        return false;
    }

    final void addToActiveList(Reference<T> ref)
    {
        synchronized (lock) {
            ref.next = activeHead;
            activeHead = ref;
            if (ref.next == null) {
                lock.notifyAll();
            }
        }
    }

    private void scanActiveList()
    {
        Reference<T> prev = null;
        Reference<T> curr = activeHead;
    
        while (curr != null) {
            if (!curr.isActive()) {
                Reference<T> next = curr.next;
                if (prev == null) {
                    activeHead = next;
                } else {
                    prev.next = next;
                }
                curr.next = head;
                curr.queue = ENQUEUED;
                head = curr;
                curr = next;
                continue;
            }
            prev = curr;
            curr = curr.next;
        }
    }
}
