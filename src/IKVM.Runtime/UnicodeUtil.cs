/*
  Copyright (C) 2002-2015 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Internal
{
    static class UnicodeUtil
    {
        // We use part of the Supplementary Private Use Area-B to encode
        // invalid surrogates. If we encounter either of these two
        // markers, we always encode the surrogate (single or pair)
        private const char HighSurrogatePrefix = '\uDBFF';
        private const char LowSurrogatePrefix = '\uDBFE';

        // Identifiers in ECMA CLI metadata and strings in custom attribute blobs are encoded
        // using UTF-8 and don't allow partial surrogates, so we have to "complete" them to
        // produce valid Unicode and reverse the process when we read back the names.
        internal static string EscapeInvalidSurrogates(string str)
        {
            if (str != null)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    char c = str[i];
                    if (Char.IsLowSurrogate(c))
                    {
                        str = str.Substring(0, i) + LowSurrogatePrefix + c + str.Substring(i + 1);
                        i++;
                    }
                    else if (Char.IsHighSurrogate(c))
                    {
                        i++;
                        // always escape the markers
                        if (c == HighSurrogatePrefix || c == LowSurrogatePrefix || i == str.Length || !Char.IsLowSurrogate(str[i]))
                        {
                            str = str.Substring(0, i - 1) + HighSurrogatePrefix + (char)(c + 0x400) + str.Substring(i);
                        }
                    }
                }
            }
            return str;
        }

        internal static string UnescapeInvalidSurrogates(string str)
        {
            if (str != null)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    switch (str[i])
                    {
                        case HighSurrogatePrefix:
                            str = str.Substring(0, i) + (char)(str[i + 1] - 0x400) + str.Substring(i + 2);
                            break;
                        case LowSurrogatePrefix:
                            str = str.Substring(0, i) + str[i + 1] + str.Substring(i + 2);
                            break;
                    }
                }
            }
            return str;
        }

        internal static string[] EscapeInvalidSurrogates(string[] str)
        {
            if (str != null)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    str[i] = EscapeInvalidSurrogates(str[i]);
                }
            }
            return str;
        }

        internal static string[] UnescapeInvalidSurrogates(string[] str)
        {
            if (str != null)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    str[i] = UnescapeInvalidSurrogates(str[i]);
                }
            }
            return str;
        }
    }

}
