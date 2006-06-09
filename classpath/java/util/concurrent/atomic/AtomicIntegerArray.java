/*
 * Written by Doug Lea with assistance from members of JCP JSR-166
 * Expert Group and released to the public domain, as explained at
 * http://creativecommons.org/licenses/publicdomain
 *
 * Modified for IKVM.NET by Jeroen Frijters
 */

/*
  Parts Copyright (C) 2006 Jeroen Frijters

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

package java.util.concurrent.atomic;

import java.util.*;

/**
 * An <tt>int</tt> array in which elements may be updated atomically.
 * See the {@link java.util.concurrent.atomic} package
 * specification for description of the properties of atomic
 * variables.
 * @since 1.5
 * @author Doug Lea
 */
public class AtomicIntegerArray implements java.io.Serializable {
    private static final long serialVersionUID = 2862133569453604235L;

    private final int[] array;

    /**
     * Creates a new AtomicIntegerArray of given length.
     *
     * @param length the length of the array
     */
    public AtomicIntegerArray(int length) {
        array = new int[length];
        // must perform at least one volatile write to conform to JMM
        if (length > 0)
            set(0, 0);
    }

    /**
     * Creates a new AtomicIntegerArray with the same length as, and
     * all elements copied from, the given array.
     *
     * @param array the array to copy elements from
     * @throws NullPointerException if array is null
     */
    public AtomicIntegerArray(int[] array) {
        if (array == null)
            throw new NullPointerException();
        int length = array.length;
        this.array = new int[length];
        if (length > 0) {
            int last = length-1;
            for (int i = 0; i < last; ++i)
                this.array[i] = array[i];
            // Do the last write as volatile
            set(last, array[last]);
        }
    }

    /**
     * Returns the length of the array.
     *
     * @return the length of the array
     */
    public final int length() {
        return array.length;
    }

    /**
     * Gets the current value at position <tt>i</tt>.
     *
     * @param i the index
     * @return the current value
     */
    public final native int get(int i);

    /**
     * Sets the element at position <tt>i</tt> to the given value.
     *
     * @param i the index
     * @param newValue the new value
     */
    public final native void set(int i, int newValue);

    /**
     * Eventually sets the element at position <tt>i</tt> to the given value.
     *
     * @param i the index
     * @param newValue the new value
     * @since 1.6
     */
    public final void lazySet(int i, int newValue) {
        set(i, newValue);
    }

    /**
     * Atomically sets the element at position <tt>i</tt> to the given
     * value and returns the old value.
     *
     * @param i the index
     * @param newValue the new value
     * @return the previous value
     */
    public final int getAndSet(int i, int newValue) {
        while (true) {
            int current = get(i);
            if (compareAndSet(i, current, newValue))
                return current;
        }
    }

    /**
     * Atomically sets the element at position <tt>i</tt> to the given
     * updated value if the current value <tt>==</tt> the expected value.
     *
     * @param i the index
     * @param expect the expected value
     * @param update the new value
     * @return true if successful. False return indicates that
     * the actual value was not equal to the expected value.
     */
    public final native boolean compareAndSet(int i, int expect, int update);

    /**
     * Atomically sets the element at position <tt>i</tt> to the given
     * updated value if the current value <tt>==</tt> the expected value.
     * May fail spuriously.
     *
     * @param i the index
     * @param expect the expected value
     * @param update the new value
     * @return true if successful.
     */
    public final boolean weakCompareAndSet(int i, int expect, int update) {
        return compareAndSet(i, expect, update);
    }

    /**
     * Atomically increments by one the element at index <tt>i</tt>.
     *
     * @param i the index
     * @return the previous value
     */
    public final int getAndIncrement(int i) {
        while (true) {
            int current = get(i);
            int next = current + 1;
            if (compareAndSet(i, current, next))
                return current;
        }
    }

    /**
     * Atomically decrements by one the element at index <tt>i</tt>.
     *
     * @param i the index
     * @return the previous value
     */
    public final int getAndDecrement(int i) {
        while (true) {
            int current = get(i);
            int next = current - 1;
            if (compareAndSet(i, current, next))
                return current;
        }
    }

    /**
     * Atomically adds the given value to the element at index <tt>i</tt>.
     *
     * @param i the index
     * @param delta the value to add
     * @return the previous value
     */
    public final int getAndAdd(int i, int delta) {
        while (true) {
            int current = get(i);
            int next = current + delta;
            if (compareAndSet(i, current, next))
                return current;
        }
    }

    /**
     * Atomically increments by one the element at index <tt>i</tt>.
     *
     * @param i the index
     * @return the updated value
     */
    public final int incrementAndGet(int i) {
        while (true) {
            int current = get(i);
            int next = current + 1;
            if (compareAndSet(i, current, next))
                return next;
        }
    }

    /**
     * Atomically decrements by one the element at index <tt>i</tt>.
     *
     * @param i the index
     * @return the updated value
     */
    public final int decrementAndGet(int i) {
        while (true) {
            int current = get(i);
            int next = current - 1;
            if (compareAndSet(i, current, next))
                return next;
        }
    }

    /**
     * Atomically adds the given value to the element at index <tt>i</tt>.
     *
     * @param i the index
     * @param delta the value to add
     * @return the updated value
     */
    public final int addAndGet(int i, int delta) {
        while (true) {
            int current = get(i);
            int next = current + delta;
            if (compareAndSet(i, current, next))
                return next;
        }
    }

    /**
     * Returns the String representation of the current values of array.
     * @return the String representation of the current values of array.
     */
    public String toString() {
        if (array.length > 0) // force volatile read
            get(0);
        return Arrays.toString(array);
    }

}
