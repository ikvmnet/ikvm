using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

using IKVM.CoreLib.System;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// A module's version string.
    /// </summary>
    public readonly struct ModuleVersion : IComparable<ModuleVersion>
    {

        public static bool operator ==(ModuleVersion x, ModuleVersion y) => x.Equals(y);

        public static bool operator !=(ModuleVersion x, ModuleVersion y) => x.Equals(y) == false;

        public static bool operator <(ModuleVersion x, ModuleVersion y) => CompareTo(x, y) < 0;

        public static bool operator >(ModuleVersion x, ModuleVersion y) => CompareTo(x, y) > 0;

        public static bool operator <=(ModuleVersion x, ModuleVersion y) => CompareTo(x, y) <= 0;

        public static bool operator >=(ModuleVersion x, ModuleVersion y) => CompareTo(x, y) >= 0;

        public static int CompareTo(ModuleVersion x, ModuleVersion y) => x.CompareTo(y);

        /// <summary>
        /// Attempts to parse the specified version value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parsed"></param>
        public static bool TryParse(string value, out ModuleVersion parsed)
        {
            return TryParse(value, out parsed);
        }

        /// <summary>
        /// Attempts to parse the specified version value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parsed"></param>
        public static bool TryParse(ReadOnlySpan<char> value, out ModuleVersion parsed)
        {
            return TryParse(value, out parsed, out _);
        }

        /// <summary>
        /// Attempts to parse the specified version value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parsed"></param>
        /// <param name="error"></param>
        public static bool TryParse(ReadOnlySpan<char> value, out ModuleVersion parsed, out string? error)
        {
            parsed = default;
            error = null;

            int n = value.Length;
            if (n == 0)
            {
                error = "Empty version string";
                return false;
            }

            int i = 0;
            char c = value[i];
            if (!(c >= '0' && c <= '9'))
            {
                error = $"{value}: Version string does not start with a number";
                return false;
            }

            var sequence = ImmutableArray.CreateBuilder<ModuleVersionComponent>(3);
            var pre = ImmutableArray.CreateBuilder<ModuleVersionComponent>(0);
            var build = ImmutableArray.CreateBuilder<ModuleVersionComponent>(0);

            i = TakeNumber(value, i, sequence);

            while (i < n)
            {
                c = value[i];
                if (c == '.')
                {
                    i++;
                    continue;
                }
                if (c == '-' || c == '+')
                {
                    i++;
                    break;
                }
                if (c >= '0' && c <= '9')
                    i = TakeNumber(value, i, sequence);
                else
                    i = TakeString(value, i, sequence);
            }

            if (c == '-' && i >= n)
            {
                error = $"{value}: Empty pre-release";
                return false;
            }

            while (i < n)
            {
                c = value[i];
                if (c == '.' || c == '-')
                {
                    i++;
                    continue;
                }
                if (c == '+')
                {
                    i++;
                    break;
                }

                if (c >= '0' && c <= '9')
                    i = TakeNumber(value, i, pre);
                else
                    i = TakeString(value, i, pre);
            }

            if (c == '+' && i >= n)
            {
                error = $"{value}: Empty pre-release";
                return false;
            }

            while (i < n)
            {
                c = value[i];
                if (c == '.' || c == '-' || c == '+')
                {
                    i++;
                    continue;
                }
                if (c >= '0' && c <= '9')
                    i = TakeNumber(value, i, build);
                else
                    i = TakeString(value, i, build);
            }

            parsed = new ModuleVersion(value.ToString(), sequence.DrainToImmutable(), pre.DrainToImmutable(), build.DrainToImmutable());
            return true;
        }

        /// <summary>
        /// Parses the specified version value.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ModuleVersion Parse(ReadOnlySpan<char> version)
        {
            if (TryParse(version, out var parsed, out var error))
                return parsed;

            throw new ArgumentException(error, nameof(version));
        }

        /// <summary>
        /// Parses the specified version value.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ModuleVersion Parse(string version)
        {
            if (version is null)
                throw new ArgumentNullException(nameof(version));

            return Parse(version.AsSpan());
        }

        /// <summary>
        /// Take a numeric token starting at position <paramref name="i"/> and appends it to <paramref name="list"/>.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        static int TakeNumber(ReadOnlySpan<char> s, int i, ImmutableArray<ModuleVersionComponent>.Builder list)
        {
            char c = s[i];
            int d = (c - '0');
            int n = s.Length;
            while (++i < n)
            {
                c = s[i];
                if (c >= '0' && c <= '9')
                {
                    d = d * 10 + (c - '0');
                    continue;
                }

                break;
            }

            list.Add(new ModuleVersionComponent(d));
            return i;
        }

        /// <summary>
        /// Take a string token starting at position <paramref name="i"/> and append it to <paramref name="list"/>.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        static int TakeString(ReadOnlySpan<char> s, int i, ImmutableArray<ModuleVersionComponent>.Builder list)
        {
            int b = i;
            int n = s.Length;
            while (++i < n)
            {
                char c = s[i];
                if (c != '.' && c != '-' && c != '+' && !(c >= '0' && c <= '9'))
                    continue;

                break;
            }

            AddModuleVersionComponent(list, s.Slice(b, i - b).ToString());
            return i;
        }

        /// <summary>
        /// We are crashing on .NET 8 for some reason, and moving this out fixes it.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void AddModuleVersionComponent(ImmutableArray<ModuleVersionComponent>.Builder builder, string value)
        {
            builder.Add(new ModuleVersionComponent(value));
        }

        readonly string _rawValue;
        readonly ImmutableArray<ModuleVersionComponent> _sequence;
        readonly ImmutableArray<ModuleVersionComponent> _pre;
        readonly ImmutableArray<ModuleVersionComponent> _build;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="rawValue"></param>
        /// <param name="sequence"></param>
        /// <param name="pre"></param>
        /// <param name="build"></param>
        public ModuleVersion(string rawValue, ImmutableArray<ModuleVersionComponent> sequence, ImmutableArray<ModuleVersionComponent> pre, ImmutableArray<ModuleVersionComponent> build)
        {
            _rawValue = rawValue ?? throw new ArgumentNullException(nameof(rawValue));
            _sequence = sequence;
            _pre = pre;
            _build = build;
        }

        /// <summary>
        /// Gets whether the version represents a valid version.
        /// </summary>
        public readonly bool IsValid => _sequence.IsDefaultOrEmpty == false;

        /// <summary>
        /// Gets the raw string value of the version.
        /// </summary>
        public readonly string RawValue => _rawValue;

        /// <summary>
        /// Gets the sequence components of the version.
        /// </summary>
        public readonly ImmutableArray<ModuleVersionComponent> Sequence => _sequence;

        /// <summary>
        /// Gets the pre components of the version.
        /// </summary>
        public readonly ImmutableArray<ModuleVersionComponent> Pre => _pre;

        /// <summary>
        /// Gets the build components of the version.
        /// </summary>
        public readonly ImmutableArray<ModuleVersionComponent> Build => _build;

        /// <summary>
        /// Compares the two lists of components.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        readonly int CompareComponents(ImmutableArray<ModuleVersionComponent> x, ImmutableArray<ModuleVersionComponent> y)
        {
            return LexicographicListComparer<ModuleVersionComponent, IComparer<ModuleVersionComponent>, ImmutableArray<ModuleVersionComponent>>.Default.Compare(x, y);
        }

        /// <inheritdoc />
        public readonly int CompareTo(ModuleVersion other)
        {
            if (IsValid && other.IsValid == false)
                return +1;
            if (IsValid == false && other.IsValid)
                return -1;

            // initially compare sequence components
            int c = CompareComponents(_sequence, other._sequence);
            if (c != 0)
                return c;

            // compare pre components
            if (_pre.IsEmpty)
            {
                if (other._pre.IsEmpty == false)
                    return +1;
            }
            else
            {
                if (other._pre.IsEmpty)
                    return -1;
            }

            // compare pre components
            c = CompareComponents(_pre, other._pre);
            if (c != 0)
                return c;

            // fallback to build components
            return CompareComponents(_build, other._build);
        }

        /// <inheritdoc />
        public readonly override int GetHashCode()
        {
            var hc = new HashCode();

            foreach (var i in _sequence)
                hc.Add(i);

            foreach (var i in _pre)
                hc.Add(i);

            foreach (var i in _build)
                hc.Add(i);

            return hc.ToHashCode();
        }

        /// <inheritdoc />
        public readonly override bool Equals(object? other)
        {
            return other is ModuleVersion v && Equals(v);
        }

        /// <summary>
        /// Returns whether the two versions are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(ModuleVersion other)
        {
            return _sequence.SequenceEqual(other._sequence) && _pre.SequenceEqual(other._pre) && _build.SequenceEqual(other._build);
        }

        /// <inheritdoc />
        public readonly override string ToString()
        {
            return _rawValue;
        }

    }

}
