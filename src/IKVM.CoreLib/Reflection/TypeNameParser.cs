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

    /// <summary>
    /// Parses an assembly-qualified type name in the syntax accepted by <c>Type.GetType</c>, such as
    /// <c>Outer+Inner`1[[Arg, ArgAssembly]][], Assembly</c>, and resolves it to a <see cref="Type"/>.
    /// </summary>
    struct TypeNameParser
    {

        /// <summary>
        /// The characters that have a special meaning in a type name and must be escaped with a backslash to appear in
        /// a name.
        /// </summary>
        const string SpecialChars = "\\+,[]*&";

        /// <summary>
        /// Modifier value for a single-dimensional, zero-based array (<c>[]</c>). Positive modifier values are the rank
        /// of a multi-dimensional array.
        /// </summary>
        const short SZARRAY = -1;

        /// <summary>
        /// Modifier value for a by-reference type (<c>&amp;</c>).
        /// </summary>
        const short BYREF = -2;

        /// <summary>
        /// Modifier value for a pointer type (<c>*</c>).
        /// </summary>
        const short POINTER = -3;

        /// <summary>
        /// The escaped name of the outermost type, including its namespace, or <see langword="null"/> if parsing failed.
        /// </summary>
        readonly string _name;

        /// <summary>
        /// The escaped names of the nested types, outermost first, or <see langword="null"/> if there are none.
        /// </summary>
        readonly string[] _nested;

        readonly string _assemblyName;

        /// <summary>
        /// The array, pointer and by-reference modifiers in the order they appear, or <see langword="null"/> if there
        /// are none. See <see cref="SZARRAY"/>, <see cref="BYREF"/> and <see cref="POINTER"/>.
        /// </summary>
        readonly short[] _modifiers;

        /// <summary>
        /// The parsed generic type arguments, or <see langword="null"/> if the type is not a constructed generic type.
        /// </summary>
        readonly TypeNameParser[] _genericParameters;

        /// <summary>
        /// Escapes the characters that have a special meaning in a type name.
        /// </summary>
        /// <param name="name">The name to escape, or <see langword="null"/>.</param>
        /// <returns>The escaped name, <paramref name="name"/> itself if nothing needed escaping, or <see langword="null"/> if <paramref name="name"/> is <see langword="null"/>.</returns>
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

        /// <summary>
        /// Removes the backslash escapes from a type name.
        /// </summary>
        /// <param name="name">The escaped name.</param>
        /// <returns>The unescaped name, or <paramref name="name"/> itself if it contains no escapes.</returns>
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

        /// <summary>
        /// Parses a type name.
        /// </summary>
        /// <param name="typeName">The type name to parse, optionally assembly-qualified.</param>
        /// <param name="throwOnError"><see langword="true"/> to throw if the name is malformed; <see langword="false"/> to return a parser whose <see cref="Error"/> is <see langword="true"/>.</param>
        /// <returns>The parsed type name.</returns>
        /// <exception cref="ArgumentException"><paramref name="typeName"/> is malformed and <paramref name="throwOnError"/> is <see langword="true"/>.</exception>
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

        /// <summary>
        /// Initializes a new instance by parsing a type name, or a generic type argument, from the current position of
        /// <paramref name="parser"/>.
        /// </summary>
        /// <param name="parser">The parser positioned at the start of the type name; advanced past it.</param>
        /// <param name="withAssemblyName"><see langword="true"/> if the type name may be followed by an assembly name.</param>
        /// <exception cref="ArgumentException">The type name is malformed.</exception>
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

        /// <summary>
        /// Gets a value indicating whether the type name could not be parsed.
        /// </summary>
        internal readonly bool Error => _name == null;

        /// <summary>
        /// Gets the escaped name of the outermost type, including its namespace.
        /// </summary>
        internal readonly string FirstNamePart => _name;

        /// <summary>
        /// Gets the assembly name that qualifies the type name, or <see langword="null"/> if it is not assembly-qualified.
        /// </summary>
        internal readonly string AssemblyName => _assemblyName;

        /// <summary>
        /// A cursor over a type name string that reads its components in turn.
        /// </summary>
        struct Parser
        {

            readonly string _typeName;

            /// <summary>
            /// The current position in the type name.
            /// </summary>
            internal int Pos;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="typeName">The type name to parse.</param>
            internal Parser(string typeName)
            {
                _typeName = typeName;
                Pos = 0;
            }

            /// <summary>
            /// Throws if the condition does not hold.
            /// </summary>
            /// <param name="condition">The condition that a well-formed type name satisfies.</param>
            /// <exception cref="ArgumentException"><paramref name="condition"/> is <see langword="false"/>.</exception>
            void Check(bool condition)
            {
                if (!condition)
                    throw new ArgumentException("Invalid type name '" + _typeName + "'");
            }

            /// <summary>
            /// Consumes the expected character.
            /// </summary>
            /// <param name="c">The character that must appear at the current position.</param>
            /// <exception cref="ArgumentException">The character at the current position is not <paramref name="c"/>.</exception>
            void Consume(char c) => Check(Pos < _typeName.Length && _typeName[Pos++] == c);

            /// <summary>
            /// Consumes the character if it appears at the current position.
            /// </summary>
            /// <param name="c">The character to consume.</param>
            /// <returns><see langword="true"/> if the character was consumed; otherwise, <see langword="false"/>.</returns>
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

            /// <summary>
            /// Reads a name up to the next unescaped special character, skipping leading white space.
            /// </summary>
            /// <returns>The name, still escaped.</returns>
            /// <exception cref="ArgumentException">The name is empty or contains an invalid escape.</exception>
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

            /// <summary>
            /// Reads the <c>+</c>-separated nested type names that follow the outermost type name.
            /// </summary>
            /// <param name="nested">The array the nested type names are appended to; allocated when the first one is found.</param>
            internal void ParseNested(ref string[] nested)
            {
                while (TryConsume('+'))
                    Add(ref nested, NextNamePart());
            }

            /// <summary>
            /// Reads a bracketed generic type argument list, if one follows. A bracket that opens an array specifier
            /// instead is left unconsumed. Each argument is either a plain type name or, in its own brackets, an
            /// assembly-qualified one.
            /// </summary>
            /// <param name="genericParameters">The array the parsed type arguments are appended to; allocated when the first one is found.</param>
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

            /// <summary>
            /// Reads the trailing pointer (<c>*</c>), by-reference (<c>&amp;</c>) and array (<c>[]</c>, <c>[*]</c>,
            /// <c>[,]</c>) modifiers.
            /// </summary>
            /// <param name="modifiers">The array the modifiers are appended to; allocated when the first one is found.</param>
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

            /// <summary>
            /// Reads the assembly name that follows a comma after the type name. Inside a bracketed generic type
            /// argument the assembly name ends at the closing bracket and may contain <c>\]</c> escapes; at the top level
            /// it extends to the end of the string.
            /// </summary>
            /// <param name="genericParameter"><see langword="true"/> if the type name being parsed is a bracketed generic type argument.</param>
            /// <param name="assemblyName">Set to the assembly name, if one is present.</param>
            /// <exception cref="ArgumentException">The assembly name is missing where required, or is empty.</exception>
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

            /// <summary>
            /// Reads the contents of an array specifier after its opening bracket, leaving the closing bracket unconsumed.
            /// </summary>
            /// <returns><see cref="SZARRAY"/> for <c>[]</c>, or the rank of a multi-dimensional array: 1 for <c>[*]</c>, and one more than the number of commas otherwise.</returns>
            /// <exception cref="ArgumentException">The specifier is truncated or the rank is too large.</exception>
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

            /// <summary>
            /// Advances past any white space at the current position.
            /// </summary>
            void SkipWhiteSpace()
            {
                while (Pos < _typeName.Length && char.IsWhiteSpace(_typeName[Pos]))
                    Pos++;
            }

            /// <summary>
            /// Appends an element to an array, allocating the array if it is <see langword="null"/>.
            /// </summary>
            /// <typeparam name="T">The element type.</typeparam>
            /// <param name="array">The array to append to; replaced by a larger copy.</param>
            /// <param name="elem">The element to append.</param>
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

        /// <summary>
        /// Resolves the parsed type name to a type. Without an assembly name the type is looked up in
        /// <paramref name="context"/> and then in the core library.
        /// </summary>
        /// <param name="universe">The universe to load assemblies from.</param>
        /// <param name="context">The module the name is resolved relative to, or <see langword="null"/> to use the core library.</param>
        /// <param name="throwOnError"><see langword="true"/> to throw if the type cannot be found; <see langword="false"/> to return <see langword="null"/>.</param>
        /// <param name="originalName">The full type name, used in exception messages.</param>
        /// <param name="resolve"><see langword="true"/> to go through the assembly's type resolution, which may produce a missing type, instead of a plain lookup.</param>
        /// <param name="ignoreCase"><see langword="true"/> to look up type names case-insensitively. Cannot be combined with <paramref name="resolve"/>.</param>
        /// <returns>The type, or <see langword="null"/> if it cannot be found and <paramref name="throwOnError"/> is <see langword="false"/>.</returns>
        /// <exception cref="TypeLoadException">The type cannot be found and <paramref name="throwOnError"/> is <see langword="true"/>.</exception>
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

        /// <summary>
        /// Applies the nested type names, generic type arguments and modifiers of the parsed name to an already resolved
        /// outermost type.
        /// </summary>
        /// <param name="type">The outermost type, or <see langword="null"/> if it could not be found.</param>
        /// <param name="context">The module generic type arguments are resolved relative to.</param>
        /// <param name="throwOnError"><see langword="true"/> to throw if a type cannot be found; <see langword="false"/> to return <see langword="null"/>.</param>
        /// <param name="originalName">The full type name, used in exception messages.</param>
        /// <param name="resolve"><see langword="true"/> to create missing types for nested types that cannot be found.</param>
        /// <param name="ignoreCase"><see langword="true"/> to look up type names case-insensitively. Cannot be combined with <paramref name="resolve"/>.</param>
        /// <returns>The final type, or <see langword="null"/> if a type cannot be found and <paramref name="throwOnError"/> is <see langword="false"/>.</returns>
        /// <exception cref="TypeLoadException">A type cannot be found and <paramref name="throwOnError"/> is <see langword="true"/>.</exception>
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
