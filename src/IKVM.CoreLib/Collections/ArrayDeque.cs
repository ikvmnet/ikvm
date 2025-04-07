using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace IKVM.CoreLib.Collections
{

    /// <summary>
    /// Resizable-array implementation of the IDeque interface. Array deques have no capacity restrictions; they grow
    /// as necessary to support usage. They are not thread-safe; in the absence of external synchronization, they do
    /// not support concurrent access by multiple threads. This class is likely to be faster than <see cref="Stack{T}"/>
    /// when used as a stack, and faster than <see cref="LinkedList{T}"/> when used as a queue.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ArrayDeque<T> : IDeque<T>
    {

        const int MIN_INITIAL_CAPACITY = 8;

        /// <summary>
        /// Calculates the appropriate element array size to hold the specified number of elements.
        /// </summary>
        /// <param name="numElements"></param>
        /// <returns></returns>
        static int CalculateSize(int numElements)
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

        /// <summary>
        /// Constructs an empty array deque with an initial capacity sufficient to hold 16 elements.
        /// </summary>
        public ArrayDeque()
        {
            _elements = new T?[16];
        }

        /// <summary>
        /// Constructs an empty array deque with an initial capacity sufficient to hold the specified number of elements.
        /// </summary>
        /// <param name="initialCapacity">lower bound on initial capacity of the deque</param>
        public ArrayDeque(int initialCapacity)
        {
            EnsureCapacity(initialCapacity);
        }

        /// <summary>
        /// Allocates array to hold the given number of elements.
        /// </summary>
        /// <param name="capacity"></param>
        [MemberNotNull(nameof(_elements))]
        void EnsureCapacity(int capacity)
        {
            _elements = new T?[CalculateSize(capacity)];
        }

        /// <summary>
        /// Doubles the capacity of this deque. Call only when full, i.e., when head and tail have wrapped around to
        /// become equal.
        /// </summary>
        void DoubleCapacity()
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

        /// <inheritdoc />
        public int Count => (_tail - _head) & (_elements.Length - 1);

        /// <inheritdoc />
        public bool IsEmpty => _head == _tail;

        /// <inheritdoc />
        public void InsertFirst(T item)
        {
            var m = _elements.Length - 1;
            var p = (_head - 1) & m;

            _elements[p] = item;
            _head = p;

            if (_head == _tail)
                DoubleCapacity();
        }

        /// <inheritdoc />
        public bool TryRemoveFirst([MaybeNullWhen(false)] out T result)
        {
            if (_head == _tail)
            {
                result = default;
                return false;
            }

            var m = _elements.Length - 1;
            var h = _head;

            result = _elements[h]!;
#if NET
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
#endif
                _elements[h] = default;
            _head = (h + 1) & m;
            return true;
        }

        /// <inheritdoc />
        public T RemoveFirst()
        {
            if (TryRemoveFirst(out var result) == false)
                throw new InvalidOperationException("The deque is empty.");

            return result;
        }

        /// <inheritdoc />
        public void InsertLast(T item)
        {
            var m = _elements.Length - 1;
            var p = (_tail + 1) & m;

            _elements[_tail] = item;
            _tail = p;

            if (_tail == _head)
                DoubleCapacity();
        }

        /// <inheritdoc />
        public bool TryRemoveLast([MaybeNullWhen(false)] out T result)
        {
            if (_head == _tail)
            {
                result = default;
                return false;
            }

            var m = _elements.Length - 1;
            var p = (_tail - 1) & m;

            result = _elements[p]!;
#if NET
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
#endif
                _elements[p] = default;
            _tail = p;
            return true;
        }

        /// <inheritdoc />
        public T RemoveLast()
        {
            if (TryRemoveLast(out var result) == false)
                throw new InvalidOperationException("The deque is empty.");

            return result;
        }

        /// <inheritdoc />
        public bool TryPeekFirst([MaybeNullWhen(false)] out T result)
        {
            if (_head == _tail)
            {
                result = default;
                return false;
            }

            result = _elements[_head]!;
            return true;
        }

        /// <inheritdoc />
        public T PeekFirst()
        {
            if (TryPeekFirst(out var result) == false)
                throw new InvalidOperationException("The deque is empty.");

            return result;
        }

        /// <inheritdoc />
        public bool TryPeekLast([MaybeNullWhen(false)] out T result)
        {
            if (_head == _tail)
            {
                result = default;
                return false;
            }

            var m = _elements.Length - 1;
            var t = (_tail - 1) & m;
            result = _elements[t]!;
            return true;
        }

        /// <inheritdoc />
        public T PeekLast()
        {
            if (TryPeekLast(out var result) == false)
                throw new InvalidOperationException("The deque is empty.");

            return result;
        }

        /// <inheritdoc />
        public bool RemoveFirst(T item)
        {
            int m = _elements.Length - 1;
            int p = _head;

            while (p != _tail)
            {
                if (EqualityComparer<T?>.Default.Equals(item, _elements[p]))
                {
                    DeleteAt(p);
                    return true;
                }

                p = (p + 1) & m;
            }

            return false;
        }

        /// <inheritdoc />
        public bool RemoveLast(T item)
        {
            int m = _elements.Length - 1;
            int p = _tail;

            while (p != _head)
            {
                p = (p - 1) & m;

                if (EqualityComparer<T?>.Default.Equals(item, _elements[p]))
                {
                    DeleteAt(p);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the element at the specified position in the elements array, adjusting head and tail as necessary.
        /// This can result in motion of elements backwards or forwards in the array.
        /// </summary>
        /// <remarks>
        /// This method is called delete rather than remove to emphasize that its semantics differ from those of <see
        /// cref="IList.RemoveAt(int)"/>
        /// </remarks>
        /// <param name="i"></param>
        /// <returns>true if elements moved backwards</returns>
        /// <exception cref="InvalidOperationException"></exception>
        bool DeleteAt(int i)
        {
            var elements = _elements;
            int m = elements.Length - 1;
            int h = _head;
            int t = _tail;
            int f = (i - h) & m;
            int b = (t - i) & m;

            // invariant: head <= i < tail mod circularity
            if (f >= ((t - h) & m))
                throw new InvalidOperationException("Concurrent operation.");

            if (f < b)
            {
                if (h <= i)
                {
                    Array.Copy(elements, h, elements, h + 1, f);
                }
                else
                {
                    Array.Copy(elements, 0, elements, 1, i);
                    elements[0] = elements[m];
                    Array.Copy(elements, h, elements, h + 1, m - h);
                }

                elements[h] = default;
                _head = (h + 1) & m;
                return false;
            }
            else
            {
                if (i < t)
                {
                    Array.Copy(elements, i + 1, elements, i, b);
                    _tail = t - 1;
                }
                else
                {
                    Array.Copy(elements, i + 1, elements, i, m - i);
                    elements[m] = elements[0];
                    Array.Copy(elements, 1, elements, 0, t);
                    _tail = (t - 1) & m;
                }

                return true;
            }
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            int m = _elements.Length - 1;
            int i = _head;

            while (i != _tail)
            {
                if (EqualityComparer<T?>.Default.Equals(item, _elements[i]))
                    return true;

                i = (i + 1) & m;
            }

            return false;
        }

        /// <inheritdoc />
        public void Clear()
        {
            int h = _head;
            int t = _tail;

            if (h != t)
            {
                _head = _tail = 0;

#if NET
                // no need to zero out elements for unmanaged structs
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                {
#endif
                    int i = h;
                    int m = _elements.Length - 1;

                    do
                    {
                        _elements[i] = default;
                        i = (i + 1) & m;
                    } while (i != t);
#if NET
                }
#endif
            }
        }

        /// <summary>
        /// Copies the elements from our element array into the specified array, in order (from first to last element
        /// in the deque). It is assumed that the array is large enough to hold all elements in the deque.
        /// </summary>
        public void CopyTo(T[] a)
        {
            if (_head < _tail)
            {
                Array.Copy(_elements, _head, a, 0, Count);
            }
            else if (_head > _tail)
            {
                int headPortionLen = _elements.Length - _head;
                Array.Copy(_elements, _head, a, 0, headPortionLen);
                Array.Copy(_elements, 0, a, headPortionLen, _tail);
            }
        }

        /// <summary>
        /// Copies the elements from our element array into the specified list, in order (from first to last element
        /// in the deque).
        /// </summary>
        public void CopyTo(IList<T> list)
        {
            if (_head < _tail)
            {
                for (int i = _head; i < Count; i++)
                    list.Add(_elements[i]!);
            }
            else if (_head > _tail)
            {
                int headPortionLen = _elements.Length - _head;

                for (int i = _head; i < headPortionLen; i++)
                    list.Add(_elements[i]!);

                for (int i = headPortionLen; i < _tail; i++)
                    list.Add(_elements[i]!);
            }
        }

        /// <summary>
        /// Copies the elements from our element array into the specified array builder, in order (from first to last element
        /// in the deque). It is assumed that the array is large enough to hold all elements in the deque.
        /// </summary>
        public void CopyTo(ImmutableArray<T>.Builder builder)
        {
            if (_head < _tail)
            {
                builder.AddRange(_elements.AsSpan().Slice(_head, Count)!);
            }
            else if (_head > _tail)
            {
                int headPortionLen = _elements.Length - _head;
                builder.AddRange(_elements.AsSpan().Slice(_head, headPortionLen)!);
                builder.AddRange(_elements.AsSpan().Slice(headPortionLen, _tail)!);
            }
        }

        /// <summary>
        /// Returns an array containing all of the elements in this deque in proper sequence (from first to last
        /// element).
        /// 
        /// The returned array will be "safe" in that no references to it are maintained by this deque. In other word,
        /// this method must allocate a new array). The caller is thus free to modify the returned array.
        /// </summary>
        public T[] ToArray()
        {
            var a = new T[Count];
            CopyTo(a);
            return a;
        }

        /// <summary>
        /// Returns an immutable array containing all of the elements in this deque in proper sequence (from first to last
        /// element).
        /// </summary>
        public ImmutableArray<T> ToImmutableArray()
        {
            var a = ImmutableArray.CreateBuilder<T>(Count);
            CopyTo(a);
            return a.DrainToImmutable();
        }

        /// <inheritdoc />
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (_head < _tail)
            {
                for (int i = _head; i < Count; i++)
                    yield return _elements[i]!;
            }
            else if (_head > _tail)
            {
                int headPortionLen = _elements.Length - _head;

                for (int i = _head; i < headPortionLen; i++)
                    yield return _elements[i]!;

                for (int i = headPortionLen; i < _tail; i++)
                    yield return _elements[i]!;
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

    }

}
