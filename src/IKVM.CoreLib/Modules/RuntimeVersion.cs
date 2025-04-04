using System;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// A representation of a version string for an implementation of the Java SE Platform. A version string consists
    /// of a version number optionally followed by pre-release and build information.
    /// </summary>
    public readonly struct RuntimeVersion : IComparable<RuntimeVersion>
    {

        /// <summary>
        /// Various utilities for parsing versions.
        /// </summary>
        static class VersionPattern
        {

            // $VNUM(-$PRE)?(\+($BUILD)?(\-$OPT)?)?
            // RE limits the format of version strings
            // ([1-9][0-9]*(?:(?:\.0)*\.[1-9][0-9]*)*)(?:-([a-zA-Z0-9]+))?(?:(\+)(0|[1-9][0-9]*)?)?(?:-([-a-zA-Z0-9.]+))?
            static readonly string VNUM = "(?<VNUM>[1-9][0-9]*(?:(?:\\.0)*\\.[1-9][0-9]*)*)";
            static readonly string PRE = "(?:-(?<PRE>[a-zA-Z0-9]+))?";
            static readonly string BUILD = "(?:(?<PLUS>\\+)(?<BUILD>0|[1-9][0-9]*)?)?";
            static readonly string OPT = "(?:-(?<OPT>[-a-zA-Z0-9.]+))?";
            static readonly string VSTR_FORMAT = VNUM + PRE + BUILD + OPT;

            public static readonly string VNUM_GROUP = "VNUM";
            public static readonly string PRE_GROUP = "PRE";
            public static readonly string PLUS_GROUP = "PLUS";
            public static readonly string BUILD_GROUP = "BUILD";
            public static readonly string OPT_GROUP = "OPT";

            public static readonly Regex VSTR_PATTERN = new Regex(VSTR_FORMAT, RegexOptions.Compiled);

        }

        /// <summary>
        /// Parses the given string as a valid version string containing a version number followed by pre-release and build information.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static RuntimeVersion Parse(string value)
        {
            if (IsSimpleNumber(value.AsSpan()))
                return new RuntimeVersion([int.Parse(value)], null, null, null);

            var m = VersionPattern.VSTR_PATTERN.Match(value);
            if (m.Success == false)
                throw new ArgumentException("Invalid version string: '" + value.ToString() + "'", nameof(value));

            // version is a dot-separated list of integers of arbitrary length
            var split = m.Groups[VersionPattern.VNUM_GROUP].Value.Split('.');
            var version = ImmutableArray.CreateBuilder<int>(split.Length);
            for (int i = 0; i < split.Length; i++)
                version.Add(int.Parse(split[i]));

            var preGroup = m.Groups[VersionPattern.PRE_GROUP];
            var pre = preGroup.Success ? preGroup.Value : null;

            var buildGroup = m.Groups[VersionPattern.BUILD_GROUP];
            var build = buildGroup.Success ? (int?)int.Parse(buildGroup.Value) : null;

            var optionalGroup = m.Groups[VersionPattern.OPT_GROUP];
            var optional = optionalGroup.Success ? optionalGroup.Value : null;

            // empty '+'
            if (build is null)
            {
                if (m.Groups[VersionPattern.PLUS_GROUP].Success)
                {
                    if (optional is not null)
                    {
                        if (pre is not null)
                            throw new ArgumentException($"'+' found with pre-release and optional components: '{value}'");
                    }
                    else
                    {
                        throw new ArgumentException($"'+' found with neither build or optional components: '{value}'");
                    }
                }
                else
                {
                    if (optional is not null && pre is null)
                        throw new ArgumentException($"optional component must be preceded by a pre-release component or '+': '{value}'");
                }
            }

            return new RuntimeVersion(version.DrainToImmutable(), pre, build, optional);
        }

        /// <summary>
        /// Returns <c>true</c> if the given span is a simple number.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static bool IsSimpleNumber(ReadOnlySpan<char> s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                var c = s[i];
                var lowerBound = (i > 0) ? '0' : '1';
                if (c < lowerBound || c > '9')
                    return false;
            }

            return true;
        }

        readonly ImmutableArray<int> _version;
        readonly string? _pre;
        readonly int? _build;
        readonly string? _optional;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="pre"></param>
        /// <param name="build"></param>
        /// <param name="optional"></param>
        public RuntimeVersion(ImmutableArray<int> version, string? pre, int? build, string? optional)
        {
            _version = version;
            _pre = pre;
            _build = build;
            _optional = optional;
        }

        /// <summary>
        /// Returns the value of the feature element of the version number.
        /// </summary>
        public readonly int Feature => _version[0];

        /// <summary>
        /// Returns the value of the interim element of the version number.
        /// </summary>
        public readonly int Interim => _version.Length > 1 ? _version[1] : 0;

        /// <summary>
        /// Returns the value of the update element of the version number.
        /// </summary>
        public readonly int Update => _version.Length > 2 ? _version[2] : 0;

        /// <summary>
        /// Returns an unmodifiable list of the integers represented in the version number.
        /// </summary>
        public ImmutableArray<int> Version => _version;

        /// <summary>
        /// Returns the optional pre-release information.
        /// </summary>
        public readonly string? Pre => _pre;

        /// <summary>
        /// Returns the build number.
        /// </summary>
        public readonly int? Build => _build;

        /// <summary>
        /// Returns additional identifying build information.
        /// </summary>
        public readonly string? Optional => _optional;

        /// <inheritdoc />
        public override readonly bool Equals(object? obj)
        {
            return obj is RuntimeVersion other && Equals(other);
        }

        /// <summary>
        /// Returns <c>true</c> if this instance is equal to <paramref name="other"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(RuntimeVersion other)
        {
            return
                _version.SequenceEqual(other._version) &&
                _pre == other._pre &&
                _build == other._build;
        }

        /// <inheritdoc />
        public override readonly int GetHashCode()
        {
            var hc = new HashCode();

            foreach (var i in _version)
                hc.Add(i);

            hc.Add(_pre);
            hc.Add(_build);
            hc.Add(_optional);

            return hc.ToHashCode();
        }

        /// <inheritdoc />
        public int CompareTo(RuntimeVersion other)
        {
            return CompareTo(other, false);
        }

        /// <summary>
        /// Compares this <see cref="RuntimeVersion"/> with <paramref name="other"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="ignoreOptional"></param>
        /// <returns></returns>
        readonly int CompareTo(RuntimeVersion other, bool ignoreOptional)
        {
            int ret = CompareVersion(other);
            if (ret != 0)
                return ret;

            ret = ComparePre(other);
            if (ret != 0)
                return ret;

            ret = CompareBuild(other);
            if (ret != 0)
                return ret;

            if (ignoreOptional == false)
                return CompareOptional(other);

            return 0;
        }

        /// <summary>
        /// Compares the version value of this <see cref="RuntimeVersion"/> to <paramref name="other"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        readonly int CompareVersion(RuntimeVersion other)
        {
            int size = _version.Length;
            int oSize = other._version.Length;
            int min = Math.Min(size, oSize);
            for (int i = 0; i < min; i++)
            {
                int val = _version[i];
                int oVal = other._version[i];
                if (val != oVal)
                    return val - oVal;
            }

            return size - oSize;
        }

        /// <summary>
        /// Compares the pre value of this <see cref="RuntimeVersion"/> to <paramref name="other"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        readonly int ComparePre(RuntimeVersion other)
        {
            if (_pre is null)
            {
                if (other._pre is not null)
                    return 1;
            }
            else
            {
                if (other._pre is null)
                    return -1;

                if (Regex.IsMatch(_pre, "\\d+"))
                {
                    return Regex.IsMatch(other._pre, "\\d+")
                        ? (BigInteger.Parse(_pre)).CompareTo(BigInteger.Parse(other._pre))
                        : -1;
                }
                else
                {
                    return Regex.IsMatch(other._pre, "\\d+")
                        ? 1
                        : _pre.CompareTo(other._pre);
                }
            }

            return 0;
        }

        /// <summary>
        /// Compares the build value of this <see cref="RuntimeVersion"/> to <paramref name="other"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        readonly int CompareBuild(RuntimeVersion obj)
        {
            if (obj._build is not null)
            {
                return _build is not null
                    ? _build.Value.CompareTo(obj._build.Value)
                    : -1;
            }
            else if (_build is not null)
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Compares the optional value of this <see cref="RuntimeVersion"/> to <paramref name="other"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        readonly int CompareOptional(RuntimeVersion other)
        {
            if (_optional is null)
            {
                if (other._optional is not null)
                    return -1;
            }
            else
            {
                if (other._optional is null)
                    return 1;

                return _optional.CompareTo(other._optional);
            }

            return 0;
        }

        /// <inheritdoc />
        public override readonly string? ToString()
        {
            var sb = new ValueStringBuilder(16);
            sb.Append(string.Join(".", _version));

            if (_pre is not null)
            {
                sb.Append("-");
                sb.Append(_pre);
            }

            if (_build is not null)
            {
                sb.Append("+");
                sb.Append(_build.ToString());

                if (_optional is not null)
                {
                    sb.Append("-");
                    sb.Append(_optional);
                }
            }
            else
            {
                if (_optional is not null)
                {
                    sb.Append(_pre is not null ? "-" : "+-");
                    sb.Append(_optional);
                }
            }

            return sb.ToString();
        }

    }

}
