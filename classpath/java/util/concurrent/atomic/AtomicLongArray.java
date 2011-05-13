/*
 * Written by Doug Lea with assistance from members of JCP JSR-166
 * Expert Group and released to the public domain, as explained at
 * http://creativecommons.org/licenses/publicdomain
 *
 * Modified for IKVM.NET by Jeroen Frijters
 */

/*
  Parts Copyright (C) 2006-2011 Jeroen Frijters

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
 * A <tt>long</tt> array in which elements may be updated atomically.
 * See the {@link java.util.concurrent.atomic} package specification
 * for description of the properties of atomic variables.
 * @since 1.5
 * @author Doug Lea
 */
public class AtomicLongArray implements java.io.Serializable {
    private static final long serialVersionUID = -2308431214976778248L;

    private final long[] array;

    /**
     * Creates a new AtomicLongArray of given length.
     *
     * @param length the length of the array
     */
    public AtomicLongArray(int length) {
        array = new long[length];
        // must perform at least one volatile write to conform to JMM
        if (length > 0)
            set(0, 0);
    }

    /**
     * Creates a new AtomicLongArray with the same length as, and
     * all elements copied from, the given array.
     *
     * @param array the array to copy elements from
     * @throws NullPointerException if array is null
     */
    public AtomicLongArray(long[] array) {
        if (array == null)
            throw new NullPointerException();
        int length = array.length;
        this.array = new long[length];
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
    public final native long get(int i);

    /**
     * Sets the element at position <tt>i</tt> to the given value.
     *
     * @param i the index
     * @param newValue the new value
     */
    public final native void set(int i, long newValue);

    /**
     * Eventually sets the element at position <tt>i</tt> to the given value.
     *
     * @param i the index
     * @param newValue the new value
     * @since 1.6
     */
    public final void lazySet(int i, long newValue) {
        set(i, newValue);
    }


    /**
     * Atomically sets the element at position <tt>i</tt> to the given value
     * and returns the old value.
     *
     * @param i the index
     * @param newValue the new value
     * @return the previous value
     */
    public final native long getAndSet(int i, long newValue);

    /**
     * Atomically sets the value to the given updated value
     * if the current value <tt>==</tt> the expected value.
     *
     * @param i the index
     * @param expect the expected value
     * @param update the new value
     * @return true if successful. False return indicates that
     * the actual value was not equal to the expected value.
     */
    public final native boolean compareAndSet(int i, long expect, long update);

    /**
     * Atomically sets the value to the given updated value
     * if the current value <tt>==</tt> the expected value.
     * May fail spuriously and does not provide ordering guarantees,
     * so is only rarely an appropriate alternative to <tt>compareAndSet</tt>.
     *
     * @param i the index
     * @param expect the expected value
     * @param update the new value
     * @return true if successful.
     */
    public final boolean weakCompareAndSet(int i, long expect, long update) {
        return compareAndSet(i, expect, update);
    }

    /**
     * Atomically increments by one the element at index <tt>i</tt>.
     *
     * @param i the index
     * @return the previous value
     */
    public final long getAndIncrement(int i) {
        return incrementAndGet(i) - 1;
    }

    /**
     * Atomically decrements by one the element at index <tt>i</tt>.
     *
     * @param i the index
     * @return the previous value
     */
    public final long getAndDecrement(int i) {
        return decrementAndGet(i) + 1;
    }

    /**
     * Atomically adds the given value to the element at index <tt>i</tt>.
     *
     * @param i the index
     * @param delta the value to add
     * @return the previous value
     */
    public final long getAndAdd(int i, long delta) {
        return addAndGet(i, delta) - delta;
    }

    /**
     * Atomically increments by one the element at index <tt>i</tt>.
     *
     * @param i the index
     * @return the updated value
     */
    public final native long incrementAndGet(int i);

    /**
     * Atomically decrements by one the element at index <tt>i</tt>.
     *
     * @param i the index
     * @return the updated value
     */
    public final native long decrementAndGet(int i);

    /**
     * Atomically adds the given value to the element at index <tt>i</tt>.
     *
     * @param i the index
     * @param delta the value to add
     * @return the updated value
     */
    public native long addAndGet(int i, long delta);

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
