﻿/*
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

using System.Text;
using System;

namespace IKVM.Runtime
{

    static class TypeNameUtil
    {

        // note that MangleNestedTypeName() assumes that there are less than 16 special characters
        private const string specialCharactersString = "\\+,[]*&\u0000";
        internal const string ProxiesContainer = "__<Proxies>";

        internal static string ReplaceIllegalCharacters(string name)
        {
            name = UnicodeUtil.EscapeInvalidSurrogates(name);
            // only the NUL character is illegal in CLR type names, so we replace it with a space
            return name.Replace('\u0000', ' ');
        }

        internal static string Unescape(string name)
        {
            int pos = name.IndexOf('\\');
            if (pos == -1)
                return name;

            var sb = new ValueStringBuilder(name.Length);
            sb.Append(name.AsSpan(0, pos));

            for (int i = pos; i < name.Length; i++)
            {
                var c = name[i];
                if (c == '\\')
                    c = name[++i];

                sb.Append(c);
            }

            return sb.ToString();
        }

        internal static string MangleNestedTypeName(string name)
        {
            var sb = new ValueStringBuilder();

            foreach (char c in name)
            {
                var index = specialCharactersString.IndexOf(c);
                if (c == '.')
                {
                    sb.Append("_");
                }
                else if (c == '_')
                {
                    sb.Append("^-");
                }
                else if (index == -1)
                {
                    sb.Append(c);
                    if (c == '^')
                        sb.Append(c);
                }
                else
                {
                    sb.Append('^');
                    sb.Append(string.Format("{0:X1}", index));
                }
            }

            return sb.ToString();
        }

        internal static string UnmangleNestedTypeName(string name)
        {
            var sb = new ValueStringBuilder(name.Length);
            for (int i = 0; i < name.Length; i++)
            {
                var c = name[i];
                var index = specialCharactersString.IndexOf(c);
                if (c == '_')
                {
                    sb.Append('.');
                }
                else if (c == '^')
                {
                    c = name[++i];
                    if (c == '-')
                        sb.Append('_');
                    else if (c == '^')
                        sb.Append('^');
                    else
                        sb.Append(specialCharactersString[c - '0']);
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        internal static string GetProxyNestedName(RuntimeJavaType[] interfaces)
        {
            var sb = new ValueStringBuilder();

            foreach (var tw in interfaces)
            {
                sb.Append(tw.Name.Length.ToString());
                sb.Append('|');
                sb.Append(tw.Name);
            }

            return TypeNameUtil.MangleNestedTypeName(sb.ToString());
        }

        internal static string GetProxyName(RuntimeJavaType[] interfaces)
        {
            return ProxiesContainer + "+" + GetProxyNestedName(interfaces);
        }

    }

}
