/*
  Copyright (C) 2009-2012 Jeroen Frijters

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
using System.Diagnostics;
using System.Text;

namespace IKVM.Reflection
{

    struct TypeNameParser
    {

        const string SpecialChars = "\\+,[]*&";
        const short SZARRAY = -1;
        const short BYREF = -2;
        const short POINTER = -3;

        readonly string name;
        readonly string[] nested;
        readonly string assemblyName;
        readonly short[] modifiers;
        readonly TypeNameParser[] genericParameters;

        internal static string Escape(string name)
        {
            if (name == null)
                return null;

            StringBuilder sb = null;
            for (int pos = 0; pos < name.Length; pos++)
            {
                var c = name[pos];
                switch (c)
                {
                    case '\\':
                    case '+':
                    case ',':
                    case '[':
                    case ']':
                    case '*':
                    case '&':
                        sb ??= new StringBuilder(name, 0, pos, name.Length + 3);
                        sb.Append("\\").Append(c);
                        break;
                    default:
                        sb?.Append(c);
                        break;
                }
            }

            return sb != null ? sb.ToString() : name;
        }

        internal static string Unescape(string name)
        {
            int pos = name.IndexOf('\\');
            if (pos == -1)
                return name;

            var sb = new StringBuilder(name, 0, pos, name.Length - 1);
            for (; pos < name.Length; pos++)
            {
                var c = name[pos];
                if (c == '\\')
                    c = name[++pos];

                sb.Append(c);
            }

            return sb.ToString();
        }

        internal static TypeNameParser Parse(string typeName, bool throwOnError)
        {
            if (throwOnError)
            {
                var parser = new Parser(typeName);
                return new TypeNameParser(ref parser, true);
            }
            else
            {
                try
                {
                    var parser = new Parser(typeName);
                    return new TypeNameParser(ref parser, true);
                }
                catch (ArgumentException)
                {
                    return new TypeNameParser();
                }
            }
        }

        TypeNameParser(ref Parser parser, bool withAssemblyName)
        {
            var genericParameter = parser.pos != 0;
            name = parser.NextNamePart();

            nested = null;
            parser.ParseNested(ref nested);

            genericParameters = null;
            parser.ParseGenericParameters(ref genericParameters);

            modifiers = null;
            parser.ParseModifiers(ref modifiers);

            assemblyName = null;
            if (withAssemblyName)
                parser.ParseAssemblyName(genericParameter, ref assemblyName);
        }

        internal readonly bool Error => name == null;

        internal readonly string FirstNamePart => name;

        internal readonly string AssemblyName => assemblyName;

        private struct Parser
        {

            readonly string typeName;
            internal int pos;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="typeName"></param>
            internal Parser(string typeName)
            {
                this.typeName = typeName;
                this.pos = 0;
            }

            void Check(bool condition)
            {
                if (!condition)
                    throw new ArgumentException("Invalid type name '" + typeName + "'");
            }

            void Consume(char c)
            {
                Check(pos < typeName.Length && typeName[pos++] == c);
            }

            bool TryConsume(char c)
            {
                if (pos < typeName.Length && typeName[pos] == c)
                {
                    pos++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            internal string NextNamePart()
            {
                SkipWhiteSpace();

                int start = pos;
                for (; pos < typeName.Length; pos++)
                {
                    var c = typeName[pos];
                    if (c == '\\')
                    {
                        pos++;
                        Check(pos < typeName.Length && SpecialChars.IndexOf(typeName[pos]) != -1);
                    }
                    else if (SpecialChars.IndexOf(c) != -1)
                    {
                        break;
                    }
                }

                Check(pos - start != 0);
                if (start == 0 && pos == typeName.Length)
                    return typeName;
                else
                    return typeName.Substring(start, pos - start);
            }

            internal void ParseNested(ref string[] nested)
            {
                while (TryConsume('+'))
                    Add(ref nested, NextNamePart());
            }

            internal void ParseGenericParameters(ref TypeNameParser[] genericParameters)
            {
                int saved = pos;
                if (TryConsume('['))
                {
                    SkipWhiteSpace();

                    if (TryConsume(']') || TryConsume('*') || TryConsume(','))
                    {
                        // it's not a generic parameter list, but an array instead
                        pos = saved;
                        return;
                    }

                    do
                    {
                        SkipWhiteSpace();

                        if (TryConsume('['))
                        {
                            Add(ref genericParameters, new TypeNameParser(ref this, true));
                            Consume(']');
                        }
                        else
                        {
                            Add(ref genericParameters, new TypeNameParser(ref this, false));
                        }
                    }

                    while (TryConsume(','));

                    Consume(']');
                    SkipWhiteSpace();
                }
            }

            internal void ParseModifiers(ref short[] modifiers)
            {
                while (pos < typeName.Length)
                {
                    switch (typeName[pos])
                    {
                        case '*':
                            pos++;
                            Add(ref modifiers, POINTER);
                            break;
                        case '&':
                            pos++;
                            Add(ref modifiers, BYREF);
                            break;
                        case '[':
                            pos++;
                            Add(ref modifiers, ParseArray());
                            Consume(']');
                            break;
                        default:
                            return;
                    }

                    SkipWhiteSpace();
                }
            }

            internal void ParseAssemblyName(bool genericParameter, ref string assemblyName)
            {
                if (pos < typeName.Length)
                {
                    if (typeName[pos] == ']' && genericParameter)
                    {
                        // ok
                    }
                    else
                    {
                        Consume(',');
                        SkipWhiteSpace();

                        if (genericParameter)
                        {
                            int start = pos;
                            while (pos < typeName.Length)
                            {
                                char c = typeName[pos];
                                if (c == '\\')
                                {
                                    pos++;
                                    // a backslash itself is not legal in an assembly name, so we don't need to check for an escaped backslash
                                    Check(pos < typeName.Length && typeName[pos++] == ']');
                                }
                                else if (c == ']')
                                {
                                    break;
                                }
                                else
                                {
                                    pos++;
                                }
                            }
                            Check(pos < typeName.Length && typeName[pos] == ']');
                            assemblyName = typeName.Substring(start, pos - start).Replace("\\]", "]");
                        }
                        else
                        {
                            // only when an assembly name is used in a generic type parameter, will it be escaped
                            assemblyName = typeName.Substring(pos);
                        }
                        Check(assemblyName.Length != 0);
                    }
                }
                else
                {
                    Check(!genericParameter);
                }
            }

            short ParseArray()
            {
                SkipWhiteSpace();
                Check(pos < typeName.Length);

                var c = typeName[pos];
                if (c == ']')
                {
                    return SZARRAY;
                }
                else if (c == '*')
                {
                    pos++;
                    SkipWhiteSpace();
                    return 1;
                }
                else
                {
                    short rank = 1;
                    while (TryConsume(','))
                    {
                        Check(rank < short.MaxValue);
                        rank++;
                        SkipWhiteSpace();
                    }

                    return rank;
                }
            }

            void SkipWhiteSpace()
            {
                while (pos < typeName.Length && Char.IsWhiteSpace(typeName[pos]))
                    pos++;
            }

            static void Add<T>(ref T[] array, T elem)
            {
                if (array == null)
                {
                    array = new T[] { elem };
                    return;
                }

                Array.Resize(ref array, array.Length + 1);
                array[array.Length - 1] = elem;
            }
        }

        internal Type GetType(Universe universe, Module context, bool throwOnError, string originalName, bool resolve, bool ignoreCase)
        {
            Debug.Assert(!resolve || !ignoreCase);
            var name = TypeName.Split(this.name);

            Type type;
            if (assemblyName != null)
            {
                Assembly asm = universe.Load(assemblyName, context, throwOnError);
                if (asm == null)
                {
                    return null;
                }
                if (resolve)
                {
                    type = asm.ResolveType(context, name);
                }
                else if (ignoreCase)
                {
                    type = asm.FindTypeIgnoreCase(name.ToLowerInvariant());
                }
                else
                {
                    type = asm.FindType(name);
                }
            }
            else if (context == null)
            {
                if (resolve)
                {
                    type = universe.CoreLib.ResolveType(context, name);
                }
                else if (ignoreCase)
                {
                    type = universe.CoreLib.FindTypeIgnoreCase(name.ToLowerInvariant());
                }
                else
                {
                    type = universe.CoreLib.FindType(name);
                }
            }
            else
            {
                if (ignoreCase)
                {
                    name = name.ToLowerInvariant();
                    type = context.FindTypeIgnoreCase(name);
                }
                else
                {
                    type = context.FindType(name);
                }
                if (type == null && context != universe.CoreLib.ManifestModule)
                {
                    if (ignoreCase)
                    {
                        type = universe.CoreLib.FindTypeIgnoreCase(name);
                    }
                    else
                    {
                        type = universe.CoreLib.FindType(name);
                    }
                }
                if (type == null && resolve)
                {
                    if (universe.CoreLib.__IsMissing && !context.__IsMissing)
                    {
                        type = universe.CoreLib.ResolveType(context, name);
                    }
                    else
                    {
                        type = context.Assembly.ResolveType(context, name);
                    }
                }
            }

            return Expand(type, context, throwOnError, originalName, resolve, ignoreCase);
        }

        internal Type Expand(Type type, Module context, bool throwOnError, string originalName, bool resolve, bool ignoreCase)
        {
            Debug.Assert(!resolve || !ignoreCase);

            if (type == null)
            {
                if (throwOnError)
                    throw new TypeLoadException(originalName);

                return null;
            }

            if (nested != null)
            {
                Type outer;
                foreach (string nest in nested)
                {
                    outer = type;
                    var name = TypeName.Split(TypeNameParser.Unescape(nest));
                    type = ignoreCase ? outer.FindNestedTypeIgnoreCase(name.ToLowerInvariant()) : outer.FindNestedType(name);
                    if (type == null)
                    {
                        if (resolve)
                            type = outer.Module.Universe.GetMissingTypeOrThrow(context, outer.Module, outer, name);
                        else if (throwOnError)
                            throw new TypeLoadException(originalName);
                        else
                            return null;
                    }
                }
            }

            if (genericParameters != null)
            {
                var typeArgs = new Type[genericParameters.Length];
                for (int i = 0; i < typeArgs.Length; i++)
                {
                    typeArgs[i] = genericParameters[i].GetType(type.Assembly.Universe, context, throwOnError, originalName, resolve, ignoreCase);
                    if (typeArgs[i] == null)
                        return null;
                }

                type = type.MakeGenericType(typeArgs);
            }

            if (modifiers != null)
            {
                foreach (var modifier in modifiers)
                {
                    type = modifier switch
                    {
                        SZARRAY => type.MakeArrayType(),
                        BYREF => type.MakeByRefType(),
                        POINTER => type.MakePointerType(),
                        _ => type.MakeArrayType(modifier),
                    };
                }
            }

            return type;
        }

    }

}
