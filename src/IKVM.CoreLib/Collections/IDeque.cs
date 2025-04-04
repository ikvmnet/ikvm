using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IKVM.CoreLib.Collections
{

    internal interface IDeque<T> : IReadOnlyCollection<T>
    {

        /// <summary>
        /// Removes all of the elements from this deque. The deque will be empty after this call returns.
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns <c>true</c> if there are no items in this deque.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Inserts the specified element at the front of this deque.
        /// </summary>
        void InsertFirst(T item);

        /// <summary>
        /// Attempts to retrieve and remove the first element of the deque.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        bool TryRemoveFirst([MaybeNullWhen(false)] out T result);

        /// <summary>
        /// Retrieves and removes the first element of the deque or throws if the deque is empty.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        T RemoveFirst();

        /// <summary>
        /// Removes the first occurrence of the specified element from this deque. If the deque does not contain the
        /// element, it is unchanged. Returns <c>true</c> if this deque contained the specified element (or equivalently, if
        /// this deque changed as a result of the call).
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool RemoveFirst(T item);

        /// <summary>
        /// Inserts the specified element at the end of this deque.
        /// </summary>
        void InsertLast(T item);

        /// <summary>
        /// Attempts to retrieve and remove the last element of the deque.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        bool TryRemoveLast([MaybeNullWhen(false)] out T result);

        /// <summary>
        /// Attempts to remove the last element of the queue represented by this deque or throws if the deque is empty.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        T RemoveLast();

        /// <summary>
        /// Retrieves, but does not remove, the first element of this deque, or returns <c>false</c> if this deque is empty.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        bool TryPeekFirst([MaybeNullWhen(false)] out T result);

        /// <summary>
        /// Retrieves, but does not remove, the first element of this deque, or throws if the deque is empty.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        T PeekFirst();

        /// <summary>
        /// Retrieves, but does not remove, the last element of this deque, or returns <c>false</c> if this deque is empty.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        bool TryPeekLast([MaybeNullWhen(false)] out T result);

        /// <summary>
        /// Retrieves, but does not remove, the last element of this deque, or throws if the deque is empty.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        T PeekLast();

        /// <summary>
        /// Copies the elements from our element array into the specified array, in order (from first to last element
        /// in the deque). It is assumed that the array is large enough to hold all elements in the deque.
        /// </summary>
        void CopyTo(T[] a);

    }

}
