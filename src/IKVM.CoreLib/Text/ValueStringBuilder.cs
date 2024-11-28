using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace IKVM.CoreLib.Text
{

    /// <summary>
    /// Stolen from dotnet/runtime.
    /// </summary>
    internal ref partial struct ValueStringBuilder
    {

        char[]? _arrayToReturnToPool;
        Span<char> _chars;
        int _pos;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="initialBuffer"></param>
        public ValueStringBuilder(Span<char> initialBuffer)
        {
            _arrayToReturnToPool = null;
            _chars = initialBuffer;
            _pos = 0;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="initialCapacity"></param>
        public ValueStringBuilder(int initialCapacity)
        {
            _arrayToReturnToPool = ArrayPool<char>.Shared.Rent(initialCapacity);
            _chars = _arrayToReturnToPool;
            _pos = 0;
        }

        /// <summary>
        /// Gets or sets the current length of the string.
        /// </summary>
        public int Length
        {
            get => _pos;
            set
            {
                Debug.Assert(value >= 0);
                Debug.Assert(value <= _chars.Length);
                _pos = value;
            }
        }

        /// <summary>
        /// Gets the capacity.
        /// </summary>
        public int Capacity => _chars.Length;

        /// <summary>
        /// Ensures the underlying buffer is capable of meeting the specified capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public void EnsureCapacity(int capacity)
        {
            // This is not expected to be called this with negative capacity
            Debug.Assert(capacity >= 0);

            // If the caller has a bug and calls this with negative capacity, make sure to call Grow to throw an exception.
            if ((uint)capacity > (uint)_chars.Length)
                Grow(capacity - _pos);
        }

        /// <summary>
        /// Get a pinnable reference to the builder.
        /// Does not ensure there is a null char after <see cref="Length"/>
        /// This overload is pattern matched in the C# 7.3+ compiler so you can omit
        /// the explicit method call, and write eg "fixed (char* c = builder)"
        /// </summary>
        public ref char GetPinnableReference()
        {
            return ref MemoryMarshal.GetReference(_chars);
        }

        /// <summary>
        /// Get a pinnable reference to the builder.
        /// </summary>
        /// <param name="terminate">Ensures that the builder has a null char after <see cref="Length"/></param>
        public ref char GetPinnableReference(bool terminate)
        {
            if (terminate)
            {
                EnsureCapacity(Length + 1);
                _chars[Length] = '\0';
            }

            return ref MemoryMarshal.GetReference(_chars);
        }

        /// <summary>
        /// Gets a reference to the character at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ref char this[int index]
        {
            get
            {
                Debug.Assert(index < _pos);
                return ref _chars[index];
            }
        }

        /// <summary>
        /// Returns the string representation of the builder.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var s = _chars.Slice(0, _pos).ToString();
            Dispose();
            return s;
        }

        /// <summary>Returns the underlying storage of the builder.</summary>
        public Span<char> RawChars => _chars;

        /// <summary>
        /// Returns a span around the contents of the builder.
        /// </summary>
        /// <param name="terminate">Ensures that the builder has a null char after <see cref="Length"/></param>
        public ReadOnlySpan<char> AsSpan(bool terminate)
        {
            if (terminate)
            {
                EnsureCapacity(Length + 1);
                _chars[Length] = '\0';
            }

            return _chars.Slice(0, _pos);
        }

        /// <summary>
        /// Returns a span around the contents of the builder.
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<char> AsSpan() => _chars.Slice(0, _pos);

        /// <summary>
        /// Returns a span around the contents of the builder.
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public ReadOnlySpan<char> AsSpan(int start) => _chars.Slice(start, _pos - start);

        /// <summary>
        /// Returns a span around the contents of the builder.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public ReadOnlySpan<char> AsSpan(int start, int length) => _chars.Slice(start, length);

        /// <summary>
        /// Attempts to copy the characters to the output destination.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="charsWritten"></param>
        /// <returns></returns>
        public bool TryCopyTo(Span<char> destination, out int charsWritten)
        {
            if (_chars.Slice(0, _pos).TryCopyTo(destination))
            {
                charsWritten = _pos;
                Dispose();
                return true;
            }
            else
            {
                charsWritten = 0;
                Dispose();
                return false;
            }
        }

        /// <summary>
        /// Inesrts a repeating sequence of characters at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        public void Insert(int index, char value, int count)
        {
            if (_pos > _chars.Length - count)
            {
                Grow(count);
            }

            int remaining = _pos - index;
            _chars.Slice(index, remaining).CopyTo(_chars.Slice(index + count));
            _chars.Slice(index, count).Fill(value);
            _pos += count;
        }

        /// <summary>
        /// Inserts the specified string at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="s"></param>
        public void Insert(int index, string? s)
        {
            if (s == null)
                return;

            int count = s.Length;

            if (_pos > (_chars.Length - count))
                Grow(count);

            int remaining = _pos - index;
            _chars.Slice(index, remaining).CopyTo(_chars.Slice(index + count));
            s
#if !NET
                .AsSpan()
#endif
                .CopyTo(_chars.Slice(index));
            _pos += count;
        }

        /// <summary>
        /// Appends the specified character.
        /// </summary>
        /// <param name="c"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(char c)
        {
            var pos = _pos;
            var chars = _chars;
            if ((uint)pos < (uint)chars.Length)
            {
                chars[pos] = c;
                _pos = pos + 1;
            }
            else
            {
                GrowAndAppend(c);
            }
        }

        /// <summary>
        /// Appends the specified string.
        /// </summary>
        /// <param name="s"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(string? s)
        {
            if (s == null)
            {
                return;
            }

            int pos = _pos;
            if (s.Length == 1 && (uint)pos < (uint)_chars.Length) // very common case, e.g. appending strings from NumberFormatInfo like separators, percent symbols, etc.
            {
                _chars[pos] = s[0];
                _pos = pos + 1;
            }
            else
            {
                AppendSlow(s);
            }
        }

        /// <summary>
        /// Appends the specified string, using the slow mechanism.
        /// </summary>
        /// <param name="s"></param>
        void AppendSlow(string s)
        {
            int pos = _pos;
            if (pos > _chars.Length - s.Length)
            {
                Grow(s.Length);
            }

            s
#if !NET
                .AsSpan()
#endif
                .CopyTo(_chars.Slice(pos));
            _pos += s.Length;
        }

        /// <summary>
        /// Appends a repeating sequence of the specified character.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="count"></param>
        public void Append(char c, int count)
        {
            if (_pos > _chars.Length - count)
                Grow(count);

            var dst = _chars.Slice(_pos, count);
            for (int i = 0; i < dst.Length; i++)
                dst[i] = c;

            _pos += count;
        }

        /// <summary>
        /// Appends the string of characters pointed to by the specified pointer and length.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        public unsafe void Append(char* value, int length)
        {
            var pos = _pos;
            if (pos > _chars.Length - length)
                Grow(length);

            var dst = _chars.Slice(_pos, length);
            for (int i = 0; i < dst.Length; i++)
                dst[i] = *value++;

            _pos += length;
        }

        /// <summary>
        /// Appends the specified sequence of characters.
        /// </summary>
        /// <param name="value"></param>
        public void Append(scoped ReadOnlySpan<char> value)
        {
            var pos = _pos;
            if (pos > _chars.Length - value.Length)
                Grow(value.Length);

            value.CopyTo(_chars.Slice(_pos));
            _pos += value.Length;
        }

        /// <summary>
        /// Allocates the memory required to append the specified count of characters and returns a reference to that memory for writing.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<char> AppendSpan(int length)
        {
            int origPos = _pos;
            if (origPos > _chars.Length - length)
                Grow(length);

            _pos = origPos + length;
            return _chars.Slice(origPos, length);
        }

        /// <summary>
        /// Grows the array and appends the single specified character.
        /// </summary>
        /// <param name="c"></param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        void GrowAndAppend(char c)
        {
            Grow(1);
            Append(c);
        }

        /// <summary>
        /// Removes a range of characters from this builder.
        /// </summary>
        /// <remarks>
        /// This method does not reduce the capacity of this builder.
        /// </remarks>
        public void Remove(int startIndex, int length)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (length > Length - startIndex)
                throw new ArgumentOutOfRangeException(nameof(length));

            if (Length == length && startIndex == 0)
            {
                Length = 0;
                return;
            }

            if (length > 0)
            {
                var src = _chars.Slice(startIndex + length);
                var dst = _chars.Slice(startIndex);
                src.CopyTo(dst);
                Length -= length;
            }
        }

        /// <summary>
        /// Resize the internal buffer either by doubling current buffer size or
        /// by adding <paramref name="additionalCapacityBeyondPos"/> to
        /// <see cref="_pos"/> whichever is greater.
        /// </summary>
        /// <param name="additionalCapacityBeyondPos">
        /// Number of chars requested beyond current position.
        /// </param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Grow(int additionalCapacityBeyondPos)
        {
            Debug.Assert(additionalCapacityBeyondPos > 0);
            Debug.Assert(_pos > _chars.Length - additionalCapacityBeyondPos, "Grow called incorrectly, no resize is needed.");

            const uint ArrayMaxLength = 0x7FFFFFC7; // same as Array.MaxLength

            // Increase to at least the required size (_pos + additionalCapacityBeyondPos), but try
            // to double the size if possible, bounding the doubling to not go beyond the max array length.
            int newCapacity = (int)Math.Max(
                (uint)(_pos + additionalCapacityBeyondPos),
                Math.Min((uint)_chars.Length * 2, ArrayMaxLength));

            // Make sure to let Rent throw an exception if the caller has a bug and the desired capacity is negative.
            // This could also go negative if the actual required length wraps around.
            var poolArray = ArrayPool<char>.Shared.Rent(newCapacity);

            _chars.Slice(0, _pos).CopyTo(poolArray);

            var toReturn = _arrayToReturnToPool;
            _chars = _arrayToReturnToPool = poolArray;
            if (toReturn != null)
                ArrayPool<char>.Shared.Return(toReturn);
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            var toReturn = _arrayToReturnToPool;
            this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again
            if (toReturn != null)
                ArrayPool<char>.Shared.Return(toReturn);
        }

    }

}
