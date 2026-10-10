// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Copied from .NET Core RuntimeMethodBuilder as of 1/24/2024.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Describes exception handler in a method body.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct ExceptionHandler : IEquatable<ExceptionHandler>
    {
        // Keep in sync with unmanged structure. 
        readonly int _exceptionClass;
        readonly int _tryStartOffset;
        readonly int _tryEndOffset;
        readonly int _filterOffset;
        readonly int _handlerStartOffset;
        readonly int _handlerEndOffset;
        readonly ExceptionHandlingClauseOptions _kind;

        public int ExceptionTypeToken
        {
            get { return _exceptionClass; }
        }

        public int TryOffset
        {
            get { return _tryStartOffset; }
        }

        public int TryLength
        {
            get { return _tryEndOffset - _tryStartOffset; }
        }

        public int FilterOffset
        {
            get { return _filterOffset; }
        }

        public int HandlerOffset
        {
            get { return _handlerStartOffset; }
        }

        public int HandlerLength
        {
            get { return _handlerEndOffset - _handlerStartOffset; }
        }

        public ExceptionHandlingClauseOptions Kind
        {
            get { return _kind; }
        }

#region Constructors

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

        private static bool IsValidKind(ExceptionHandlingClauseOptions kind)
        {
            switch (kind)
            {
                case ExceptionHandlingClauseOptions.Clause:
                case ExceptionHandlingClauseOptions.Filter:
                case ExceptionHandlingClauseOptions.Finally:
                case ExceptionHandlingClauseOptions.Fault:
                    return true;

                default:
                    return false;
            }
        }

        #endregion

        #region Equality

        public override int GetHashCode()
        {
            return _exceptionClass ^ _tryStartOffset ^ _tryEndOffset ^ _filterOffset ^ _handlerStartOffset ^ _handlerEndOffset ^ (int)_kind;
        }

        public override bool Equals(Object obj)
        {
            return obj is ExceptionHandler && Equals((ExceptionHandler)obj);
        }

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
