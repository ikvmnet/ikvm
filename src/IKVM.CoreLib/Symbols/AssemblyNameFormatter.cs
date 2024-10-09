using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;

using IKVM.CoreLib.Text;

namespace IKVM.CoreLib.Symbols
{

    static class AssemblyNameFormatter
    {

        public static string ComputeDisplayName(string name, Version? version, string? cultureName, ImmutableArray<byte> pkt, AssemblyNameFlags flags, AssemblyContentType contentType, ImmutableArray<byte> pk)
        {
            const int PUBLIC_KEY_TOKEN_LEN = 8;

            Debug.Assert(name.Length != 0);

            var vsb = new ValueStringBuilder(stackalloc char[256]);
            vsb.AppendQuoted(name);

            if (version != null)
            {
                ushort major = (ushort)version.Major;
                if (major != ushort.MaxValue)
                {
                    vsb.Append(", Version=");
                    vsb.AppendSpanFormattable(major);

                    ushort minor = (ushort)version.Minor;
                    if (minor != ushort.MaxValue)
                    {
                        vsb.Append('.');
                        vsb.AppendSpanFormattable(minor);

                        ushort build = (ushort)version.Build;
                        if (build != ushort.MaxValue)
                        {
                            vsb.Append('.');
                            vsb.AppendSpanFormattable(build);

                            ushort revision = (ushort)version.Revision;
                            if (revision != ushort.MaxValue)
                            {
                                vsb.Append('.');
                                vsb.AppendSpanFormattable(revision);
                            }
                        }
                    }
                }
            }

            if (cultureName != null)
            {
                if (cultureName.Length == 0)
                    cultureName = "neutral";
                vsb.Append(", Culture=");
                vsb.AppendQuoted(cultureName);
            }

            var keyOrToken = pkt.IsDefaultOrEmpty == false ? pkt : pk;
            if (keyOrToken != null)
            {
                if (pkt != null)
                {
                    if (pkt.Length > PUBLIC_KEY_TOKEN_LEN)
                        throw new ArgumentException();

                    vsb.Append(", PublicKeyToken=");
                }
                else
                {
                    vsb.Append(", PublicKey=");
                }

                if (keyOrToken.Length == 0)
                {
                    vsb.Append("null");
                }
                else
                {
                    HexConverter.EncodeToUtf16(keyOrToken.AsSpan(), vsb.AppendSpan(keyOrToken.Length * 2), HexConverter.Casing.Lower);
                }
            }

            if (0 != (flags & AssemblyNameFlags.Retargetable))
                vsb.Append(", Retargetable=Yes");

            if (contentType == AssemblyContentType.WindowsRuntime)
                vsb.Append(", ContentType=WindowsRuntime");

            return vsb.ToString();
        }

        static void AppendQuoted(this ref ValueStringBuilder vsb, string s)
        {
            bool needsQuoting = false;
            const char quoteChar = '\"';

            // App-compat: You can use double or single quotes to quote a name, and Fusion (or rather the IdentityAuthority) picks one
            // by some algorithm. Rather than guess at it, we use double quotes consistently.
            ReadOnlySpan<char> span = s.AsSpan();
            if (s.Length != span.Trim().Length || span.IndexOfAny('\"', '\'') >= 0)
                needsQuoting = true;

            if (needsQuoting)
                vsb.Append(quoteChar);

            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case '\\':
                    case ',':
                    case '=':
                    case '\'':
                    case '"':
                        vsb.Append('\\');
                        break;
                    case '\t':
                        vsb.Append("\\t");
                        continue;
                    case '\r':
                        vsb.Append("\\r");
                        continue;
                    case '\n':
                        vsb.Append("\\n");
                        continue;
                }

                vsb.Append(s[i]);
            }

            if (needsQuoting)
                vsb.Append(quoteChar);
        }

        static void AppendSpanFormattable(this ref ValueStringBuilder vsb, ushort value)
        {
            vsb.Append(value.ToString());
        }

    }

}
