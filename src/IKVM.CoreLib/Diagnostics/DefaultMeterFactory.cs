using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace IKVM.CoreLib.Diagnostics
{

    /// <summary>
    /// Default <see cref="IMeterFactory"/> implementation.
    /// </summary>
    class DefaultMeterFactory : IMeterFactory
    {

        /// <summary>
        /// Default meter implementation for the default meter factory.
        /// </summary>
        sealed class DefaultMeter : Meter
        {

            public DefaultMeter(MeterOptions options) :
                base(options)
            {

            }

            public void Release() => base.Dispose(true);

            protected override void Dispose(bool disposing)
            {

            }

        }

        readonly Dictionary<string, List<DefaultMeter>> _cache = new();
        bool _disposed;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DefaultMeterFactory()
        {

        }

        public Meter Create(MeterOptions options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (options.Scope is not null && !object.ReferenceEquals(options.Scope, this))
                throw new InvalidOperationException("Invalid scope.");

            Debug.Assert(options.Name is not null);

            lock (_cache)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(DefaultMeterFactory));

                if (_cache.TryGetValue(options.Name ?? throw new InvalidOperationException(), out List<DefaultMeter>? meterList))
                {
                    foreach (var meter in meterList)
                    {
                        if (meter.Version == options.Version && CompareTags(meter.Tags as IList<KeyValuePair<string, object?>>, options.Tags))
                        {
                            return meter;
                        }
                    }
                }
                else
                {
                    meterList = new List<DefaultMeter>();
                    _cache.Add(options.Name, meterList);
                }

                object? scope = options.Scope;
                options.Scope = this;

                var m = new DefaultMeter(options);
                options.Scope = scope;

                meterList.Add(m);
                return m;
            }
        }

        public void Dispose()
        {
            lock (_cache)
            {
                if (_disposed)
                {
                    return;
                }

                _disposed = true;

                foreach (List<DefaultMeter> meterList in _cache.Values)
                {
                    foreach (DefaultMeter meter in meterList)
                    {
                        meter.Release();
                    }
                }

                _cache.Clear();
            }
        }

        ref struct BitMapper
        {

            int _maxIndex;
            Span<ulong> _bitMap;

            public BitMapper(Span<ulong> bitMap)
            {
                _bitMap = bitMap;
                _bitMap.Clear();
                _maxIndex = bitMap.Length * sizeof(ulong) * 8;
            }

            public int MaxIndex => _maxIndex;

            private static void GetIndexAndMask(int index, out int bitIndex, out ulong mask)
            {
                bitIndex = index >> 6; // divide by 64 == (sizeof(ulong) * 8) bits
                int bit = index & (sizeof(ulong) * 8 - 1);
                mask = 1UL << bit;
            }

            public bool SetBit(int index)
            {
                Debug.Assert(index >= 0);
                Debug.Assert(index < _maxIndex);

                GetIndexAndMask(index, out int bitIndex, out ulong mask);
                ulong value = _bitMap[bitIndex];
                _bitMap[bitIndex] = value | mask;
                return true;
            }

            public bool IsSet(int index)
            {
                Debug.Assert(index >= 0);
                Debug.Assert(index < _maxIndex);

                GetIndexAndMask(index, out int bitIndex, out ulong mask);
                ulong value = _bitMap[bitIndex];
                return ((value & mask) != 0);
            }

        }

        static bool CompareTags(IList<KeyValuePair<string, object?>>? sortedTags, IEnumerable<KeyValuePair<string, object?>>? tags2)
        {
            if (sortedTags == tags2)
            {
                return true;
            }

            if (sortedTags is null || tags2 is null)
            {
                return false;
            }

            int count = sortedTags.Count;
            int size = count / (sizeof(ulong) * 8) + 1;
            BitMapper bitMapper = new BitMapper((uint)size <= 100 ? stackalloc ulong[size] : new ulong[size]);

            if (tags2 is ICollection<KeyValuePair<string, object?>> tagsCol)
            {
                if (tagsCol.Count != count)
                {
                    return false;
                }

                if (tagsCol is IList<KeyValuePair<string, object?>> secondList)
                {
                    for (int i = 0; i < count; i++)
                    {
                        KeyValuePair<string, object?> pair = secondList[i];

                        for (int j = 0; j < count; j++)
                        {
                            if (bitMapper.IsSet(j))
                            {
                                continue;
                            }

                            KeyValuePair<string, object?> pair1 = sortedTags[j];

                            int compareResult = string.CompareOrdinal(pair.Key, pair1.Key);
                            if (compareResult == 0 && object.Equals(pair.Value, pair1.Value))
                            {
                                bitMapper.SetBit(j);
                                break;
                            }

                            if (compareResult < 0 || j == count - 1)
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }
            }

            int listCount = 0;
            using (IEnumerator<KeyValuePair<string, object?>> enumerator = tags2.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    listCount++;
                    if (listCount > sortedTags.Count)
                    {
                        return false;
                    }

                    KeyValuePair<string, object?> pair = enumerator.Current;
                    for (int j = 0; j < count; j++)
                    {
                        if (bitMapper.IsSet(j))
                        {
                            continue;
                        }

                        KeyValuePair<string, object?> pair1 = sortedTags[j];

                        int compareResult = string.CompareOrdinal(pair.Key, pair1.Key);
                        if (compareResult == 0 && object.Equals(pair.Value, pair1.Value))
                        {
                            bitMapper.SetBit(j);
                            break;
                        }

                        if (compareResult < 0 || j == count - 1)
                        {
                            return false;
                        }
                    }
                }

                return listCount == sortedTags.Count;
            }
        }


    }

}
