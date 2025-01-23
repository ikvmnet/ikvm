// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Parses an assembly name.
    /// </summary>
    internal ref partial struct AssemblyIdentityParser
    {

        /// <summary>
        /// Simple structure that holds the parts of an assembly identity.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Version"></param>
        /// <param name="CultureName"></param>
        /// <param name="Flags"></param>
        /// <param name="PublicKeyOrToken"></param>
        public readonly record struct Parts(
            string Name,
            Version? Version,
            string? CultureName,
            AssemblyNameFlags Flags,
            byte[]? PublicKeyOrToken);

        /// <summary>
        /// Token categories for the lexer.
        /// </summary>
        enum Token
        {
            Equals = 1,
            Comma = 2,
            String = 3,
            End = 4,
        }

        /// <summary>
        /// Kinds of attributes.
        /// </summary>
        enum AttributeKind
        {
            Version = 1,
            Culture = 2,
            PublicKeyOrToken = 4,
            ProcessorArchitecture = 8,
            Retargetable = 16,
            ContentType = 32
        }

        readonly ReadOnlySpan<char> _input;
        int _index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="ArgumentException"></exception>
        AssemblyIdentityParser(ReadOnlySpan<char> input)
        {
            if (input.Length == 0)
                throw new ArgumentException(nameof(input));

            _input = input;
            _index = 0;
        }

        /// <summary>
        /// Attempts to parse the assembly identity from the given characters, into the specified <see cref="Parts."/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parts"></param>
        /// <returns></returns>
        internal static bool TryParse(ReadOnlySpan<char> name, ref Parts parts)
        {
            AssemblyIdentityParser parser = new(name);
            return parser.TryParse(ref parts);
        }

        static bool TryRecordNewSeen(scoped ref AttributeKind seenAttributes, AttributeKind newAttribute)
        {
            if ((seenAttributes & newAttribute) != 0)
                return false;

            seenAttributes |= newAttribute;
            return true;
        }

        /// <summary>
        /// Attempts to parse the next assembly identity from the current characters.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        bool TryParse(ref Parts result)
        {
            // Name must come first.
            if (!TryGetNextToken(out string name, out Token token) || token != Token.String || string.IsNullOrEmpty(name))
                return false;

            Version? version = null;
            string? cultureName = null;
            byte[]? pkt = null;
            AssemblyNameFlags flags = 0;

            AttributeKind alreadySeen = default;
            if (!TryGetNextToken(out _, out token))
                return false;

            while (token != Token.End)
            {
                if (token != Token.Comma)
                    return false;

                if (!TryGetNextToken(out string attributeName, out token) || token != Token.String)
                    return false;

                if (!TryGetNextToken(out _, out token) || token != Token.Equals)
                    return false;

                if (!TryGetNextToken(out string attributeValue, out token) || token != Token.String)
                    return false;

                if (attributeName == string.Empty)
                    return false;

                if (IsAttribute(attributeName, "Version"))
                {
                    if (!TryRecordNewSeen(ref alreadySeen, AttributeKind.Version))
                    {
                        return false;
                    }
                    if (!TryParseVersion(attributeValue, ref version))
                    {
                        return false;
                    }
                }
                else if (IsAttribute(attributeName, "Culture"))
                {
                    if (!TryRecordNewSeen(ref alreadySeen, AttributeKind.Culture))
                    {
                        return false;
                    }
                    if (!TryParseCulture(attributeValue, out cultureName))
                    {
                        return false;
                    }
                }
                else if (IsAttribute(attributeName, "PublicKeyToken"))
                {
                    if (!TryRecordNewSeen(ref alreadySeen, AttributeKind.PublicKeyOrToken))
                    {
                        return false;
                    }
                    if (!TryParsePKT(attributeValue, isToken: true, out pkt))
                    {
                        return false;
                    }
                }
                else if (IsAttribute(attributeName, "PublicKey"))
                {
                    if (!TryRecordNewSeen(ref alreadySeen, AttributeKind.PublicKeyOrToken))
                    {
                        return false;
                    }
                    if (!TryParsePKT(attributeValue, isToken: false, out pkt))
                    {
                        return false;
                    }
                    flags |= AssemblyNameFlags.PublicKey;
                }
                else if (IsAttribute(attributeName, "ProcessorArchitecture"))
                {
                    if (!TryRecordNewSeen(ref alreadySeen, AttributeKind.ProcessorArchitecture))
                    {
                        return false;
                    }
                    if (!TryParseProcessorArchitecture(attributeValue, out ProcessorArchitecture arch))
                    {
                        return false;
                    }
                    flags |= (AssemblyNameFlags)(((int)arch) << 4);
                }
                else if (IsAttribute(attributeName, "Retargetable"))
                {
                    if (!TryRecordNewSeen(ref alreadySeen, AttributeKind.Retargetable))
                    {
                        return false;
                    }

                    if (attributeValue.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                    {
                        flags |= AssemblyNameFlags.Retargetable;
                    }
                    else if (attributeValue.Equals("No", StringComparison.OrdinalIgnoreCase))
                    {
                        // nothing to do
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (IsAttribute(attributeName, "ContentType"))
                {
                    if (!TryRecordNewSeen(ref alreadySeen, AttributeKind.ContentType))
                    {
                        return false;
                    }

                    if (attributeValue.Equals("WindowsRuntime", StringComparison.OrdinalIgnoreCase))
                    {
                        flags |= (AssemblyNameFlags)(((int)AssemblyContentType.WindowsRuntime) << 9);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // Desktop compat: If we got here, the attribute name is unknown to us. Ignore it.
                }

                if (!TryGetNextToken(out _, out token))
                {
                    return false;
                }
            }

            result = new Parts(name, version, cultureName, flags, pkt);
            return true;
        }

        static bool IsAttribute(string candidate, string attributeKind) => candidate.Equals(attributeKind, StringComparison.OrdinalIgnoreCase);

        static bool TryParseVersion(string attributeValue, ref Version? version)
        {
#if NET8_0_OR_GREATER
            ReadOnlySpan<char> attributeValueSpan = attributeValue;
            Span<Range> parts = stackalloc Range[5];
            parts = parts.Slice(0, attributeValueSpan.Split(parts, '.'));
#else
            var parts = attributeValue.Split('.');
#endif
            if (parts.Length is < 2 or > 4)
                return false;

            Span<ushort> versionNumbers = [ushort.MaxValue, ushort.MaxValue, ushort.MaxValue, ushort.MaxValue];
            for (int i = 0; i < parts.Length; i++)
            {
                if (!ushort.TryParse(
#if NET8_0_OR_GREATER
                    attributeValueSpan[parts[i]],
#else
                    parts[i],
#endif
                    NumberStyles.None, NumberFormatInfo.InvariantInfo, out versionNumbers[i]))
                {
                    return false;
                }
            }

            if (versionNumbers[0] == ushort.MaxValue ||
                versionNumbers[1] == ushort.MaxValue)
                return false;

            version =
                versionNumbers[2] == ushort.MaxValue ? new Version(versionNumbers[0], versionNumbers[1]) :
                versionNumbers[3] == ushort.MaxValue ? new Version(versionNumbers[0], versionNumbers[1], versionNumbers[2]) :
                new Version(versionNumbers[0], versionNumbers[1], versionNumbers[2], versionNumbers[3]);

            return true;
        }

        /// <summary>
        /// Attempts to parse the given value as a culture.
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        static bool TryParseCulture(string attributeValue, out string? result)
        {
            if (attributeValue.Equals("Neutral", StringComparison.OrdinalIgnoreCase))
            {
                result = "";
                return true;
            }

            result = attributeValue;
            return true;
        }

        static bool TryParsePKT(string attributeValue, bool isToken, out byte[]? result)
        {
            if (attributeValue.Equals("null", StringComparison.OrdinalIgnoreCase) || attributeValue == string.Empty)
            {
                result = Array.Empty<byte>();
                return true;
            }

            if (attributeValue.Length % 2 != 0 || (isToken && attributeValue.Length != 8 * 2))
            {
                result = null;
                return false;
            }

            var pkt = new byte[attributeValue.Length / 2];
            if (!HexConverter.TryDecodeFromUtf16(attributeValue.AsSpan(), pkt, out int _))
            {
                result = null;
                return false;
            }

            result = pkt;
            return true;
        }

        /// <summary>
        /// Tries to parse the specified attribute value as a processor architecture.
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        static bool TryParseProcessorArchitecture(string attributeValue, out ProcessorArchitecture result)
        {
            result = attributeValue switch
            {
                _ when attributeValue.Equals("msil", StringComparison.OrdinalIgnoreCase) => ProcessorArchitecture.MSIL,
                _ when attributeValue.Equals("x86", StringComparison.OrdinalIgnoreCase) => ProcessorArchitecture.X86,
                _ when attributeValue.Equals("ia64", StringComparison.OrdinalIgnoreCase) => ProcessorArchitecture.IA64,
                _ when attributeValue.Equals("amd64", StringComparison.OrdinalIgnoreCase) => ProcessorArchitecture.Amd64,
                _ when attributeValue.Equals("arm", StringComparison.OrdinalIgnoreCase) => ProcessorArchitecture.Arm,
                _ when attributeValue.Equals("msil", StringComparison.OrdinalIgnoreCase) => ProcessorArchitecture.MSIL,
                _ => ProcessorArchitecture.None
            };
            return result != ProcessorArchitecture.None;
        }

        /// <summary>
        /// Returns <c>true</c> if the specified character is whitespace.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        static bool IsWhiteSpace(char ch) => ch is '\n' or '\r' or ' ' or '\t';

        /// <summary>
        /// Attempts to get the next character.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool TryGetNextChar(out char ch)
        {
            if (_index < _input.Length)
            {
                ch = _input[_index++];
                if (ch == '\0')
                    return false;
            }
            else
            {
                ch = '\0';
            }

            return true;
        }

        /// <summary>
        /// Return the next token in assembly name. If the result is Token.String, sets "tokenString" to the tokenized string.
        /// </summary>
        /// <param name="tokenString"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool TryGetNextToken(out string tokenString, out Token token)
        {
            tokenString = string.Empty;
            char c;

            while (true)
            {
                if (!TryGetNextChar(out c))
                {
                    token = default;
                    return false;
                }

                switch (c)
                {
                    case ',':
                        {
                            token = Token.Comma;
                            return true;
                        }
                    case '=':
                        {
                            token = Token.Equals;
                            return true;
                        }
                    case '\0':
                        {
                            token = Token.End;
                            return true;
                        }
                }

                if (!IsWhiteSpace(c))
                {
                    break;
                }
            }

            using ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[64]);

            var quoteChar = '\0';
            if (c is '\'' or '\"')
            {
                quoteChar = c;
                if (!TryGetNextChar(out c))
                {
                    token = default;
                    return false;
                }
            }

            for (; ; )
            {
                if (c == 0)
                {
                    if (quoteChar != 0)
                    {
                        // EOS and unclosed quotes is an error
                        token = default;
                        return false;
                    }
                    // Reached end of input and therefore of string
                    break;
                }

                if (quoteChar != 0 && c == quoteChar)
                    break;  // Terminate: Found closing quote of quoted string.

                if (quoteChar == 0 && (c is ',' or '='))
                {
                    _index--;
                    break;  // Terminate: Found start of a new ',' or '=' token.
                }

                if (quoteChar == 0 && (c is '\'' or '\"'))
                {
                    token = default;
                    return false;
                }

                if (c is '\\')
                {
                    if (!TryGetNextChar(out c))
                    {
                        token = default;
                        return false;
                    }

                    switch (c)
                    {
                        case '\\':
                        case ',':
                        case '=':
                        case '\'':
                        case '"':
                            sb.Append(c);
                            break;
                        case 't':
                            sb.Append('\t');
                            break;
                        case 'r':
                            sb.Append('\r');
                            break;
                        case 'n':
                            sb.Append('\n');
                            break;
                        default:
                            token = default;
                            return false;
                    }
                }
                else
                {
                    sb.Append(c);
                }

                if (!TryGetNextChar(out c))
                {
                    token = default;
                    return false;
                }
            }


            int length = sb.Length;
            if (quoteChar == 0)
            {
                while (length > 0 && IsWhiteSpace(sb[length - 1]))
                    length--;
            }

            tokenString = sb.AsSpan(0, length).ToString();
            token = Token.String;
            return true;
        }

    }

}