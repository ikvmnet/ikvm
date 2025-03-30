using System;
using System.Linq;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// Provides various methods for working with Java identifiers.
    /// </summary>
    static class Identifer
    {

        static readonly string[] RESERVED =
        [
            "abstract", "assert", "boolean", "break", "byte", "case", "catch", "char", "class", "const", "continue",
            "default", "do", "double", "else", "enum", "extends", "final", "finally", "float", "for", "goto", "if",
            "implements", "import", "instanceof", "int", "interface", "long", "native", "new", "package", "private",
            "protected", "public", "return", "short", "static", "strictfp", "super", "switch", "synchronized", "this",
            "throw", "throws", "transient", "try", "void", "volatile", "while", "true", "false", "null", "_"
        ];

        /// <summary>
        /// Returns <c>true</c> if the given name is a legal package name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsPackageName(string name)
        {
            return IsPackageName(name.AsSpan());
        }

        /// <summary>
        /// Returns <c>true</c> if the given name is a legal package name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsPackageName(ReadOnlySpan<char> name)
        {
            return IsTypeName(name);
        }

        /// <summary>
        /// Returns <c>true</c> if the given name is a legal class name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsClassName(string name)
        {
            return IsClassName(name.AsSpan());
        }

        /// <summary>
        /// Returns <c>true</c> if the given name is a legal class name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsClassName(ReadOnlySpan<char> name)
        {
            return IsTypeName(name);
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="name"/> is a valid Java type name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsTypeName(ReadOnlySpan<char> name)
        {
            int next;
            int off = 0;
            while ((next = name[off..].IndexOf(['.'])) != -1)
            {
                var id = name.Slice(off, next);
                if (IsJavaIdentifier(id) == false)
                    return false;

                off = next + 1;
            }

            return IsJavaIdentifier(name[off..]);
        }

        /// <summary>
        /// Returns true if the given string is a legal Java identifier, otherwise false.
        /// </summary>
        public static bool IsJavaIdentifier(ReadOnlySpan<char> value)
        {
            if (value.IsWhiteSpace())
                return false;

            foreach (var i in RESERVED)
                if (value.SequenceEqual(i))
                    return false;

            // TODO this isn't fully implemented
            return true;

            //int first = Rune.GetRuneAt(value, 0);
            //if (!Character.isJavaIdentifierStart(first))
            //    return false;

            //int i = Character.charCount(first);
            //while (i < value.length())
            //{
            //    int cp = Character.codePointAt(value, i);
            //    if (!Character.isJavaIdentifierPart(cp))
            //        return false;
            //    i += Character.charCount(cp);
            //}

            //return true;
        }

    }

}
