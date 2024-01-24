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
    public readonly struct ExceptionHandler : IEquatable<ExceptionHandler>
    {
        // Keep in sync with unmanged structure. 
        internal readonly int m_exceptionClass;
        internal readonly int m_tryStartOffset;
        internal readonly int m_tryEndOffset;
        internal readonly int m_filterOffset;
        internal readonly int m_handlerStartOffset;
        internal readonly int m_handlerEndOffset;
        internal readonly ExceptionHandlingClauseOptions m_kind;

        public int ExceptionTypeToken
        {
            get { return m_exceptionClass; }
        }

        public int TryOffset
        {
            get { return m_tryStartOffset; }
        }

        public int TryLength
        {
            get { return m_tryEndOffset - m_tryStartOffset; }
        }

        public int FilterOffset
        {
            get { return m_filterOffset; }
        }

        public int HandlerOffset
        {
            get { return m_handlerStartOffset; }
        }

        public int HandlerLength
        {
            get { return m_handlerEndOffset - m_handlerStartOffset; }
        }

        public ExceptionHandlingClauseOptions Kind
        {
            get { return m_kind; }
        }

        #region Constructors

        /// <summary>
        /// Creates a description of an exception handler.
        /// </summary>
        /// <param name="tryOffset">The offset of the first instruction protected by this handler.</param>
        /// <param name="tryLength">The number of bytes protected by this handler.</param>
        /// <param name="filterOffset">The filter code begins at the specified offset and ends at the first instruction of the handler block. Specify 0 if not applicable (this is not a filter handler).</param>
        /// <param name="handlerOffset">The offset of the first instruction of this handler.</param>
        /// <param name="handlerLength">The number of bytes of the handler.</param>
        /// <param name="kind">The kind of handler, the handler might be a catch handler, filter handler, fault handler, or finally handler.</param>
        /// <param name="exceptionTypeToken">The token of the exception type handled by this handler. Specify 0 if not applicable (this is finally handler).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Some of the instruction offset is negative, 
        /// the end offset of specified range is less than its start offset,
        /// or <paramref name="kind"/> has an invalid value.
        /// </exception>
        public ExceptionHandler(int tryOffset, int tryLength, int filterOffset, int handlerOffset, int handlerLength,
            ExceptionHandlingClauseOptions kind, int exceptionTypeToken)
        {
            if (tryOffset < 0)
            {
                throw new ArgumentOutOfRangeException("tryOffset", string.Format("Non-negative number required."));
            }

            if (tryLength < 0)
            {
                throw new ArgumentOutOfRangeException("tryLength", string.Format("Non-negative number required."));
            }

            if (filterOffset < 0)
            {
                throw new ArgumentOutOfRangeException("filterOffset", string.Format("Non-negative number required."));
            }

            if (handlerOffset < 0)
            {
                throw new ArgumentOutOfRangeException("handlerOffset", string.Format("Non-negative number required."));
            }

            if (handlerLength < 0)
            {
                throw new ArgumentOutOfRangeException("handlerLength", string.Format("Non-negative number required."));
            }

            if ((long)tryOffset + tryLength > Int32.MaxValue)
            {
                throw new ArgumentOutOfRangeException("tryLength", string.Format("Valid values are between {0} and {1}, inclusive.", 0, Int32.MaxValue - tryOffset));
            }

            if ((long)handlerOffset + handlerLength > Int32.MaxValue)
            {
                throw new ArgumentOutOfRangeException("handlerLength", string.Format("Valid values are between {0} and {1}, inclusive.", 0, Int32.MaxValue - handlerOffset));
            }

            // Other tokens migth also be invalid. We only check nil tokens as the implementation (SectEH_Emit in corhlpr.cpp) requires it,
            // and we can't check for valid tokens until the module is baked.
            if (kind == ExceptionHandlingClauseOptions.Clause && (exceptionTypeToken & 0x00FFFFFF) == 0)
            {
                throw new ArgumentException(string.Format("Token {0:x} is not a valid Type token.", exceptionTypeToken), "exceptionTypeToken");
            }

            if (!IsValidKind(kind))
            {
                throw new ArgumentOutOfRangeException("kind", string.Format("Enum value was out of legal range."));
            }

            m_tryStartOffset = tryOffset;
            m_tryEndOffset = tryOffset + tryLength;
            m_filterOffset = filterOffset;
            m_handlerStartOffset = handlerOffset;
            m_handlerEndOffset = handlerOffset + handlerLength;
            m_kind = kind;
            m_exceptionClass = exceptionTypeToken;
        }

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

            m_tryStartOffset = tryStartOffset;
            m_tryEndOffset = tryEndOffset;
            m_filterOffset = filterOffset;
            m_handlerStartOffset = handlerStartOffset;
            m_handlerEndOffset = handlerEndOffset;
            m_kind = (ExceptionHandlingClauseOptions)kind;
            m_exceptionClass = exceptionTypeToken;
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
            return m_exceptionClass ^ m_tryStartOffset ^ m_tryEndOffset ^ m_filterOffset ^ m_handlerStartOffset ^ m_handlerEndOffset ^ (int)m_kind;
        }

        public override bool Equals(Object obj)
        {
            return obj is ExceptionHandler && Equals((ExceptionHandler)obj);
        }

        public bool Equals(ExceptionHandler other)
        {
            return
                other.m_exceptionClass == m_exceptionClass &&
                other.m_tryStartOffset == m_tryStartOffset &&
                other.m_tryEndOffset == m_tryEndOffset &&
                other.m_filterOffset == m_filterOffset &&
                other.m_handlerStartOffset == m_handlerStartOffset &&
                other.m_handlerEndOffset == m_handlerEndOffset &&
                other.m_kind == m_kind;
        }

        public static bool operator ==(ExceptionHandler left, ExceptionHandler right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ExceptionHandler left, ExceptionHandler right)
        {
            return !left.Equals(right);
        }

        #endregion

    }

}
