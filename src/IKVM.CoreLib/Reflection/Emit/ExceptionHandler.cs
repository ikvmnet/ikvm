// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Copied from .NET Core RuntimeMethodBuilder as of 1/24/2024.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Represents an exception handler in a byte array of IL to be passed to a method such as
    /// <c>MethodBuilder.SetMethodBody</c>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    readonly struct ExceptionHandler : IEquatable<ExceptionHandler>
    {

        readonly int _exceptionClass;
        readonly int _tryStartOffset;
        readonly int _tryEndOffset;
        readonly int _filterOffset;
        readonly int _handlerStartOffset;
        readonly int _handlerEndOffset;
        readonly ExceptionHandlingClauseOptions _kind;

        /// <summary>
        /// Gets the token of the exception type handled by this handler.
        /// </summary>
        public int ExceptionTypeToken => _exceptionClass;

        /// <summary>
        /// Gets the byte offset at which the code that is protected by this exception handler begins.
        /// </summary>
        public int TryOffset => _tryStartOffset;

        /// <summary>
        /// Gets the length, in bytes, of the code protected by this exception handler.
        /// </summary>
        public int TryLength => _tryEndOffset - _tryStartOffset;

        /// <summary>
        /// Gets the byte offset at which the filter code for the exception handler begins.
        /// </summary>
        public int FilterOffset => _filterOffset;

        /// <summary>
        /// Gets the byte offset of the first instruction of the exception handler.
        /// </summary>
        public int HandlerOffset => _handlerStartOffset;

        /// <summary>
        /// Gets the length, in bytes, of the exception handler.
        /// </summary>
        public int HandlerLength => _handlerEndOffset - _handlerStartOffset;

        /// <summary>
        /// Gets a value that represents the kind of exception handler this object represents.
        /// </summary>
        public ExceptionHandlingClauseOptions Kind => _kind;

        #region Constructors

        /// <summary>
        /// Initializes a new instance from start and end offsets.
        /// </summary>
        /// <param name="tryStartOffset">The byte offset of the first instruction protected by this handler.</param>
        /// <param name="tryEndOffset">The byte offset just past the last instruction protected by this handler.</param>
        /// <param name="filterOffset">The byte offset of the filter code, or 0 if the handler has no filter.</param>
        /// <param name="handlerStartOffset">The byte offset of the first instruction of the handler.</param>
        /// <param name="handlerEndOffset">The byte offset just past the last instruction of the handler.</param>
        /// <param name="kind">The kind of handler, as an <see cref="ExceptionHandlingClauseOptions"/> value.</param>
        /// <param name="exceptionTypeToken">The token of the exception type handled, for a catch clause.</param>
        internal ExceptionHandler(int tryStartOffset, int tryEndOffset, int filterOffset, int handlerStartOffset, int handlerEndOffset,
            int kind, int exceptionTypeToken)
        {
            Debug.Assert(tryStartOffset >= 0);
            Debug.Assert(tryEndOffset >= 0);
            Debug.Assert(filterOffset >= 0);
            Debug.Assert(handlerStartOffset >= 0);
            Debug.Assert(handlerEndOffset >= 0);
            Debug.Assert(IsValidKind((ExceptionHandlingClauseOptions)kind));
            Debug.Assert(kind != (int)ExceptionHandlingClauseOptions.Clause || (exceptionTypeToken & 0x00FFFFFF) != 0);

            _tryStartOffset = tryStartOffset;
            _tryEndOffset = tryEndOffset;
            _filterOffset = filterOffset;
            _handlerStartOffset = handlerStartOffset;
            _handlerEndOffset = handlerEndOffset;
            _kind = (ExceptionHandlingClauseOptions)kind;
            _exceptionClass = exceptionTypeToken;
        }

        /// <summary>
        /// Gets a value indicating whether the specified value is one of the four handler kinds.
        /// </summary>
        /// <param name="kind">The value to check.</param>
        /// <returns><see langword="true"/> if <paramref name="kind"/> is a catch, filter, finally or fault clause; otherwise, <see langword="false"/>.</returns>
        static bool IsValidKind(ExceptionHandlingClauseOptions kind)
        {
            return kind switch
            {
                ExceptionHandlingClauseOptions.Clause or ExceptionHandlingClauseOptions.Filter or ExceptionHandlingClauseOptions.Finally or ExceptionHandlingClauseOptions.Fault => true,
                _ => false,
            };
        }

        #endregion

        #region Equality

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>The hash code for the current object.</returns>
        public override int GetHashCode() =>
            _exceptionClass ^ _tryStartOffset ^ _tryEndOffset ^ _filterOffset ^ _handlerStartOffset ^ _handlerEndOffset ^ (int)_kind;

        /// <summary>
        /// Indicates whether this instance of the <see cref="ExceptionHandler"/> object is equal to a specified object.
        /// </summary>
        /// <param name="obj">The object to compare this instance to.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> and this instance are equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj) => obj is ExceptionHandler && Equals((ExceptionHandler)obj);

        /// <summary>
        /// Indicates whether this instance of the <see cref="ExceptionHandler"/> object is equal to another
        /// <see cref="ExceptionHandler"/> object.
        /// </summary>
        /// <param name="other">The exception handler object to compare this instance to.</param>
        /// <returns><see langword="true"/> if <paramref name="other"/> and this instance are equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(ExceptionHandler other)
        {
            return
                other._exceptionClass == _exceptionClass &&
                other._tryStartOffset == _tryStartOffset &&
                other._tryEndOffset == _tryEndOffset &&
                other._filterOffset == _filterOffset &&
                other._handlerStartOffset == _handlerStartOffset &&
                other._handlerEndOffset == _handlerEndOffset &&
                other._kind == _kind;
        }

        #endregion

    }

}
