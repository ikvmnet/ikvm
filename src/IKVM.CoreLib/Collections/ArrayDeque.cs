using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IKVM.CoreLib.Collections
{

    class ArrayDeque<T>
    {

        const int MIN_INITIAL_CAPACITY = 8;

        static int calculateSize(int numElements)
        {
            int initialCapacity = MIN_INITIAL_CAPACITY;

            // Find the best power of two to hold elements.
            // Tests "<=" because arrays aren't kept full.
            if (numElements >= initialCapacity)
            {
                initialCapacity = numElements;
                initialCapacity |= (initialCapacity >>> 1);
                initialCapacity |= (initialCapacity >>> 2);
                initialCapacity |= (initialCapacity >>> 4);
                initialCapacity |= (initialCapacity >>> 8);
                initialCapacity |= (initialCapacity >>> 16);
                initialCapacity++;

                if (initialCapacity < 0)   // Too many elements, must back off
                    initialCapacity >>>= 1;// Good luck allocating 2 ^ 30 elements
            }
            return initialCapacity;
        }

        T?[] _elements;
        int _head;
        int _tail;

        /**
         * Constructs an empty array deque with an initial capacity
         * sufficient to hold 16 elements.
         */
        public ArrayDeque()
        {
            _elements = new T?[16];
        }

        /**
         * Constructs an empty array deque with an initial capacity
         * sufficient to hold the specified number of elements.
         *
         * @param numElements  lower bound on initial capacity of the deque
         */
        public ArrayDeque(int numElements)
        {
            allocateElements(numElements);
        }

        /**
         * Constructs a deque containing the elements of the specified
         * collection, in the order they are returned by the collection's
         * iterator.  (The first element returned by the collection's
         * iterator becomes the first element, or <i>front</i> of the
         * deque.)
         *
         * @param c the collection whose elements are to be placed into the deque
         * @throws NullPointerException if the specified collection is null
         */
        public ArrayDeque(ICollection<T> c)
        {
            allocateElements(c.Count);
            addAll(c);
        }

        /**
         * Allocates empty array to hold the given number of elements.
         *
         * @param numElements  the number of elements to hold
         */
        void allocateElements(int numElements)
        {
            _elements = new T?[calculateSize(numElements)];
        }

        /**
         * Doubles the capacity of this deque.  Call only when full, i.e.,
         * when head and tail have wrapped around to become equal.
         */
        void doubleCapacity()
        {
            Debug.Assert(_head == _tail);
            int p = _head;
            int n = _elements.Length;
            int r = n - p; // number of elements to the right of p
            int newCapacity = n << 1;
            if (newCapacity < 0)
                throw new InvalidOperationException("Sorry, deque too big");

            var a = new T?[newCapacity];
            Array.Copy(_elements, p, a, 0, r);
            Array.Copy(_elements, 0, a, r, p);
            _elements = a;
            _head = 0;
            _tail = n;
        }

        /**
         * Copies the elements from our element array into the specified array,
         * in order (from first to last element in the deque).  It is assumed
         * that the array is large enough to hold all elements in the deque.
         *
         * @return its argument
         */
        T[] copyElements(T[] a)
        {
            if (_head < _tail)
            {
                Array.Copy(_elements, _head, a, 0, size());
            }
            else if (_head > _tail)
            {
                int headPortionLen = _elements.Length - _head;
                Array.Copy(_elements, _head, a, 0, headPortionLen);
                Array.Copy(_elements, 0, a, headPortionLen, _tail);
            }

            return a;
        }

        // The main insertion and extraction methods are addFirst,
        // addLast, pollFirst, pollLast. The other methods are defined in
        // terms of these.

        /**
         * Inserts the specified element at the front of this deque.
         *
         * @param e the element to add
         * @throws NullPointerException if the specified element is null
         */
        public void addFirst(T e)
        {
            _elements[_head = (_head - 1) & (_elements.Length - 1)] = e;
            if (_head == _tail)
                doubleCapacity();
        }

        /**
         * Inserts the specified element at the end of this deque.
         *
         * <p>This method is equivalent to {@link #add}.
         *
         * @param e the element to add
         * @throws NullPointerException if the specified element is null
         */
        public void addLast(T e)
        {
            _elements[_tail] = e;
            if ((_tail = (_tail + 1) & (_elements.Length - 1)) == _head)
                doubleCapacity();
        }

        /**
         * Inserts the specified element at the front of this deque.
         *
         * @param e the element to add
         * @return {@code true} (as specified by {@link Deque#offerFirst})
         * @throws NullPointerException if the specified element is null
         */
        public bool offerFirst(T e)
        {
            addFirst(e);
            return true;
        }

        /**
         * Inserts the specified element at the end of this deque.
         *
         * @param e the element to add
         * @return {@code true} (as specified by {@link Deque#offerLast})
         * @throws NullPointerException if the specified element is null
         */
        public bool offerLast(T e)
        {
            addLast(e);
            return true;
        }

        /**
         * @throws NoSuchElementException {@inheritDoc}
         */
        public T removeFirst()
        {
            T? x = pollFirst();
            if (x == null)
                throw new InvalidOperationException("The deque is empty.");

            return x;
        }

        public T? pollFirst()
        {
            int h = _head;
            var result = _elements[h];
            if (result == null)
                return default(T);

            _elements[h] = default;
            _head = (h + 1) & (_elements.Length - 1);
            return result;
        }

        public T? pollLast()
        {
            int t = (_tail - 1) & (_elements.Length - 1);
            var result = _elements[t];
            if (result == null)
                return default;

            _elements[t] = default;
            _tail = t;
            return result;
        }

        /**
         * @throws NoSuchElementException {@inheritDoc}
         */
        public T getFirst()
        {
            var result = _elements[_head];
            if (result == null)
                throw new InvalidOperationException("The deque is empty.");

            return result;
        }

        /**
         * @throws NoSuchElementException {@inheritDoc}
         */
        public T getLast()
        {
            var result = _elements[(_tail - 1) & (_elements.Length - 1)];
            if (result == null)
                throw new InvalidOperationException("The deque is empty.");

            return result;
        }

        public T? peekFirst()
        {
            // elements[head] is null if deque empty
            return _elements[_head];
        }

        public T? peekLast()
        {
            return _elements[(_tail - 1) & (_elements.Length - 1)];
        }

        /**
         * Removes the first occurrence of the specified element in this
         * deque (when traversing the deque from head to tail).
         * If the deque does not contain the element, it is unchanged.
         * More formally, removes the first element {@code e} such that
         * {@code o.equals(e)} (if such an element exists).
         * Returns {@code true} if this deque contained the specified element
         * (or equivalently, if this deque changed as a result of the call).
         *
         * @param o element to be removed from this deque, if present
         * @return {@code true} if the deque contained the specified element
         */
        public bool removeFirstOccurrence(T o)
        {
            int mask = _elements.Length - 1;
            int i = _head;
            T? x;
            while ((x = _elements[i]) != null)
            {
                if (o.Equals(x))
                {
                    delete(i);
                    return true;
                }
                i = (i + 1) & mask;
            }
            return false;
        }

        /**
         * Removes the last occurrence of the specified element in this
         * deque (when traversing the deque from head to tail).
         * If the deque does not contain the element, it is unchanged.
         * More formally, removes the last element {@code e} such that
         * {@code o.equals(e)} (if such an element exists).
         * Returns {@code true} if this deque contained the specified element
         * (or equivalently, if this deque changed as a result of the call).
         *
         * @param o element to be removed from this deque, if present
         * @return {@code true} if the deque contained the specified element
         */
        public bool removeLastOccurrence(T o)
        {
            int mask = _elements.Length - 1;
            int i = (_tail - 1) & mask;
            T? x;
            while ((x = _elements[i]) != null)
            {
                if (o.Equals(x))
                {
                    delete(i);
                    return true;
                }
                i = (i - 1) & mask;
            }
            return false;
        }

        // *** Queue methods ***

        /**
         * Inserts the specified element at the end of this deque.
         *
         * <p>This method is equivalent to {@link #addLast}.
         *
         * @param e the element to add
         * @return {@code true} (as specified by {@link Collection#add})
         * @throws NullPointerException if the specified element is null
         */
        public bool add(T e)
        {
            addLast(e);
            return true;
        }

        /**
         * {@inheritDoc}
         *
         * <p>This implementation iterates over the specified collection, and adds
         * each object returned by the iterator to this collection, in turn.
         *
         * <p>Note that this implementation will throw an
         * <tt>UnsupportedOperationException</tt> unless <tt>add</tt> is
         * overridden (assuming the specified collection is non-empty).
         *
         * @throws UnsupportedOperationException {@inheritDoc}
         * @throws ClassCastException            {@inheritDoc}
         * @throws NullPointerException          {@inheritDoc}
         * @throws IllegalArgumentException      {@inheritDoc}
         * @throws IllegalStateException         {@inheritDoc}
         *
         * @see #add(Object)
         */
        public bool addAll(IEnumerable<T> c)
        {
            var modified = false;
            foreach (var e in c)
                if (add(e))
                    modified = true;

            return modified;
        }

        /**
         * Inserts the specified element at the end of this deque.
         *
         * <p>This method is equivalent to {@link #offerLast}.
         *
         * @param e the element to add
         * @return {@code true} (as specified by {@link Queue#offer})
         * @throws NullPointerException if the specified element is null
         */
        public bool offer(T e)
        {
            return offerLast(e);
        }

        /**
         * Retrieves and removes the head of the queue represented by this deque.
         *
         * This method differs from {@link #poll poll} only in that it throws an
         * exception if this deque is empty.
         *
         * <p>This method is equivalent to {@link #removeFirst}.
         *
         * @return the head of the queue represented by this deque
         * @throws NoSuchElementException {@inheritDoc}
         */
        public T remove()
        {
            return removeFirst();
        }

        /**
         * Retrieves and removes the head of the queue represented by this deque
         * (in other words, the first element of this deque), or returns
         * {@code null} if this deque is empty.
         *
         * <p>This method is equivalent to {@link #pollFirst}.
         *
         * @return the head of the queue represented by this deque, or
         *         {@code null} if this deque is empty
         */
        public T? poll()
        {
            return pollFirst();
        }

        /**
         * Retrieves, but does not remove, the head of the queue represented by
         * this deque.  This method differs from {@link #peek peek} only in
         * that it throws an exception if this deque is empty.
         *
         * <p>This method is equivalent to {@link #getFirst}.
         *
         * @return the head of the queue represented by this deque
         * @throws NoSuchElementException {@inheritDoc}
         */
        public T element()
        {
            return getFirst();
        }

        /**
         * Retrieves, but does not remove, the head of the queue represented by
         * this deque, or returns {@code null} if this deque is empty.
         *
         * <p>This method is equivalent to {@link #peekFirst}.
         *
         * @return the head of the queue represented by this deque, or
         *         {@code null} if this deque is empty
         */
        public T? peek()
        {
            return peekFirst();
        }

        // *** Stack methods ***

        /**
         * Pushes an element onto the stack represented by this deque.  In other
         * words, inserts the element at the front of this deque.
         *
         * <p>This method is equivalent to {@link #addFirst}.
         *
         * @param e the element to push
         * @throws NullPointerException if the specified element is null
         */
        public void push(T e)
        {
            addFirst(e);
        }

        /**
         * Pops an element from the stack represented by this deque.  In other
         * words, removes and returns the first element of this deque.
         *
         * <p>This method is equivalent to {@link #removeFirst()}.
         *
         * @return the element at the front of this deque (which is the top
         *         of the stack represented by this deque)
         * @throws NoSuchElementException {@inheritDoc}
         */
        public T pop()
        {
            return removeFirst();
        }

        private void checkInvariants()
        {
            Debug.Assert(_elements[_tail] == null);
            Debug.Assert(_head == _tail ? _elements[_head] == null : (_elements[_head] != null && _elements[(_tail - 1) & (_elements.Length - 1)] != null));
            Debug.Assert(_elements[(_head - 1) & (_elements.Length - 1)] == null);
        }

        /**
         * Removes the element at the specified position in the elements array,
         * adjusting head and tail as necessary.  This can result in motion of
         * elements backwards or forwards in the array.
         *
         * <p>This method is called delete rather than remove to emphasize
         * that its semantics differ from those of {@link List#remove(int)}.
         *
         * @return true if elements moved backwards
         */
        private bool delete(int i)
        {
            checkInvariants();
            T?[] elements = this._elements;
            int mask = elements.Length - 1;
            int h = _head;
            int t = _tail;
            int front = (i - h) & mask;
            int back = (t - i) & mask;

            // Invariant: head <= i < tail mod circularity
            if (front >= ((t - h) & mask))
                throw new InvalidOperationException("Concurrent operation.");

            // Optimize for least element motion
            if (front < back)
            {
                if (h <= i)
                {
                    Array.Copy(elements, h, elements, h + 1, front);
                }
                else
                { // Wrap around
                    Array.Copy(elements, 0, elements, 1, i);
                    elements[0] = elements[mask];
                    Array.Copy(elements, h, elements, h + 1, mask - h);
                }
                elements[h] = default;
                _head = (h + 1) & mask;
                return false;
            }
            else
            {
                if (i < t)
                { // Copy the null tail as well
                    Array.Copy(elements, i + 1, elements, i, back);
                    _tail = t - 1;
                }
                else
                { // Wrap around
                    Array.Copy(elements, i + 1, elements, i, mask - i);
                    elements[mask] = elements[0];
                    Array.Copy(elements, 1, elements, 0, t);
                    _tail = (t - 1) & mask;
                }
                return true;
            }
        }

        // *** Collection Methods ***

        /**
         * Returns the number of elements in this deque.
         *
         * @return the number of elements in this deque
         */
        public int size()
        {
            return (_tail - _head) & (_elements.Length - 1);
        }

        /**
         * Returns {@code true} if this deque contains no elements.
         *
         * @return {@code true} if this deque contains no elements
         */
        public bool isEmpty()
        {
            return _head == _tail;
        }

        /**
         * Returns {@code true} if this deque contains the specified element.
         * More formally, returns {@code true} if and only if this deque contains
         * at least one element {@code e} such that {@code o.equals(e)}.
         *
         * @param o object to be checked for containment in this deque
         * @return {@code true} if this deque contains the specified element
         */
        public bool contains(T o)
        {
            int mask = _elements.Length - 1;
            int i = _head;
            T? x;
            while ((x = _elements[i]) != null)
            {
                if (o.Equals(x))
                    return true;
                i = (i + 1) & mask;
            }
            return false;
        }

        /**
         * Removes a single instance of the specified element from this deque.
         * If the deque does not contain the element, it is unchanged.
         * More formally, removes the first element {@code e} such that
         * {@code o.equals(e)} (if such an element exists).
         * Returns {@code true} if this deque contained the specified element
         * (or equivalently, if this deque changed as a result of the call).
         *
         * <p>This method is equivalent to {@link #removeFirstOccurrence(Object)}.
         *
         * @param o element to be removed from this deque, if present
         * @return {@code true} if this deque contained the specified element
         */
        public bool remove(T o)
        {
            return removeFirstOccurrence(o);
        }

        /**
         * Removes all of the elements from this deque.
         * The deque will be empty after this call returns.
         */
        public void clear()
        {
            int h = _head;
            int t = _tail;
            if (h != t)
            { // clear all cells
                _head = _tail = 0;
                int i = h;
                int mask = _elements.Length - 1;
                do
                {
                    _elements[i] = default;
                    i = (i + 1) & mask;
                } while (i != t);
            }
        }

        /**
         * Returns an array containing all of the elements in this deque
         * in proper sequence (from first to last element).
         *
         * <p>The returned array will be "safe" in that no references to it are
         * maintained by this deque.  (In other words, this method must allocate
         * a new array).  The caller is thus free to modify the returned array.
         *
         * <p>This method acts as bridge between array-based and collection-based
         * APIs.
         *
         * @return an array containing all of the elements in this deque
         */
        public T[] toArray()
        {
            return copyElements(new T[size()]);
        }

    }

}
