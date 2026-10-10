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

        readonly string _name;
        readonly string[] _nested;
        readonly string _assemblyName;
        readonly short[] _modifiers;
        readonly TypeNameParser[] _genericParameters;

        internal static string Escape(string name)
        {
            if (name == null)
                return null;

            StringBuilder sb = null;
            for (var pos = 0; pos < name.Length; pos++)
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
            var pos = name.IndexOf('\\');
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
            var genericParameter = parser.Pos != 0;
            _name = parser.NextNamePart();

            _nested = null;
            parser.ParseNested(ref _nested);

            _genericParameters = null;
            parser.ParseGenericParameters(ref _genericParameters);

            _modifiers = null;
            parser.ParseModifiers(ref _modifiers);

            _assemblyName = null;
            if (withAssemblyName)
                parser.ParseAssemblyName(genericParameter, ref _assemblyName);
        }

        internal readonly bool Error => _name == null;

        internal readonly string FirstNamePart => _name;

        internal readonly string AssemblyName => _assemblyName;

        struct Parser
        {

            readonly string _typeName;
            internal int Pos;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="typeName"></param>
            internal Parser(string typeName)
            {
                _typeName = typeName;
                Pos = 0;
            }

            void Check(bool condition)
            {
                if (!condition)
                    throw new ArgumentException("Invalid type name '" + _typeName + "'");
            }

            void Consume(char c) => Check(Pos < _typeName.Length && _typeName[Pos++] == c);

            bool TryConsume(char c)
            {
                if (Pos < _typeName.Length && _typeName[Pos] == c)
                {
                    Pos++;
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

                var start = Pos;
                for (; Pos < _typeName.Length; Pos++)
                {
                    var c = _typeName[Pos];
                    if (c == '\\')
                    {
                        Pos++;
                        Check(Pos < _typeName.Length && SpecialChars.IndexOf(_typeName[Pos]) != -1);
                    }
                    else if (SpecialChars.IndexOf(c) != -1)
                    {
                        break;
                    }
                }

                Check(Pos - start != 0);
                if (start == 0 && Pos == _typeName.Length)
                    return _typeName;
                else
                    return _typeName[start..Pos];
            }

            internal void ParseNested(ref string[] nested)
            {
                while (TryConsume('+'))
                    Add(ref nested, NextNamePart());
            }

            internal void ParseGenericParameters(ref TypeNameParser[] genericParameters)
            {
                var saved = Pos;
                if (TryConsume('['))
                {
                    SkipWhiteSpace();

                    if (TryConsume(']') || TryConsume('*') || TryConsume(','))
                    {
                        // it's not a generic parameter list, but an array instead
                        Pos = saved;
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
                while (Pos < _typeName.Length)
                {
                    switch (_typeName[Pos])
                    {
                        case '*':
                            Pos++;
                            Add(ref modifiers, POINTER);
                            break;
                        case '&':
                            Pos++;
                            Add(ref modifiers, BYREF);
                            break;
                        case '[':
                            Pos++;
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
                if (Pos < _typeName.Length)
                {
                    if (_typeName[Pos] == ']' && genericParameter)
                    {
                        // ok
                    }
                    else
                    {
                        Consume(',');
                        SkipWhiteSpace();

                        if (genericParameter)
                        {
                            var start = Pos;
                            while (Pos < _typeName.Length)
                            {
                                var c = _typeName[Pos];
                                if (c == '\\')
                                {
                                    Pos++;
                                    // a backslash itself is not legal in an assembly name, so we don't need to check for an escaped backslash
                                    Check(Pos < _typeName.Length && _typeName[Pos++] == ']');
                                }
                                else if (c == ']')
                                {
                                    break;
                                }
                                else
                                {
                                    Pos++;
                                }
                            }
                            Check(Pos < _typeName.Length && _typeName[Pos] == ']');
                            assemblyName = _typeName[start..Pos].Replace("\\]", "]");
                        }
                        else
                        {
                            // only when an assembly name is used in a generic type parameter, will it be escaped
                            assemblyName = _typeName[Pos..];
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
                Check(Pos < _typeName.Length);

                var c = _typeName[Pos];
                if (c == ']')
                {
                    return SZARRAY;
                }
                else if (c == '*')
                {
                    Pos++;
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
                while (Pos < _typeName.Length && char.IsWhiteSpace(_typeName[Pos]))
                    Pos++;
            }

            static void Add<T>(ref T[] array, T elem)
            {
                if (array == null)
                {
                    array = new T[] { elem };
                    return;
                }

                Array.Resize(ref array, array.Length + 1);
                array[^1] = elem;
            }
        }

        internal Type GetType(Universe universe, Module context, bool throwOnError, string originalName, bool resolve, bool ignoreCase)
        {
            Debug.Assert(!resolve || !ignoreCase);
            var name = TypeName.Split(_name);

            Type type;
            if (_assemblyName != null)
            {
                Assembly asm = universe.Load(_assemblyName, context, throwOnError);
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

            if (_nested != null)
            {
                Type outer;
                foreach (var nest in _nested)
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

            if (_genericParameters != null)
            {
                var typeArgs = new Type[_genericParameters.Length];
                for (var i = 0; i < typeArgs.Length; i++)
                {
                    typeArgs[i] = _genericParameters[i].GetType(type.Assembly.Universe, context, throwOnError, originalName, resolve, ignoreCase);
                    if (typeArgs[i] == null)
                        return null;
                }

                type = type.MakeGenericType(typeArgs);
            }

            if (_modifiers != null)
            {
                foreach (var modifier in _modifiers)
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
