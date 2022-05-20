/*
  Copyright (C) 2010 Jeroen Frijters

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

import java.util.concurrent.atomic.AtomicReferenceFieldUpdater;

// This is the base class for intrinsified AtomicReferenceFieldUpdater.
// The real class of an intrinsic ARFU is a subclass of this, but since it
// is not visible to Java code, this class serves as the class.

public class IntrinsicAtomicReferenceFieldUpdater<T,V> extends AtomicReferenceFieldUpdater<T,V>
{
    protected IntrinsicAtomicReferenceFieldUpdater()
    {
    }

    public boolean compareAndSet(T obj, V expect, V update)
    {
        throw new AbstractMethodError();
    }
    
    public final boolean weakCompareAndSet(T obj, V expect, V update)
    {
        return compareAndSet(obj, expect, update);
    }
    
    public void set(T obj, V newValue)
    {
        throw new AbstractMethodError();
    }
    
    public final void lazySet(T obj, V newValue)
    {
        set(obj, newValue);
    }
    
    public V get(T obj)
    {
        throw new AbstractMethodError();
    }
}
