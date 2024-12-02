using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

using IKVM.CoreLib.Text;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Provides a set of methods to construct a type name.
    /// </summary>
    ref struct TypeSymbolNameBuilder
    {

        /// <summary>
        /// Supported formats.
        /// </summary>
        internal enum Format
        {
            FullName,
            AssemblyQualifiedName,
            ToString,
        }

        static bool IsTypeNameReservedChar(char ch) => ch is ',' or '[' or ']' or '&' or '*' or '+' or '\\';

        /// <summary>
        /// Returns the properly formatted name for the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        internal static string? ToString(TypeSymbol type, Format format)
        {
            if (type.IsMissing == false)
                if (format == Format.FullName || format == Format.AssemblyQualifiedName)
                    if (!type.IsGenericTypeDefinition && type.ContainsGenericParameters)
                        return null;

            var tnb = new TypeSymbolNameBuilder(stackalloc char[128]);
            tnb.AddAssemblyQualifiedName(type, format);
            return tnb.ToString();
        }

        /// <summary>
        /// Returns <c>true</c> if the given string contains a reserved character.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static bool ContainsReservedChar(string name)
        {
            foreach (char c in name)
            {
                if (c == '\0')
                    break;
                if (IsTypeNameReservedChar(c))
                    return true;
            }

            return false;
        }

        ValueStringBuilder _builder = new ValueStringBuilder();
        int _instNesting;
        bool _firstInstArg;
        bool _nestedName;
        bool _hasAssemblySpec;
        readonly List<int> _stack = new List<int>();
        int _stackIdx;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public TypeSymbolNameBuilder(Span<char> initialBuffer)
        {
            _builder = new ValueStringBuilder(initialBuffer);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public TypeSymbolNameBuilder(int initialCapacity)
        {
            _builder = new ValueStringBuilder(initialCapacity);
        }

        /// <summary>
        /// Opens the generic arguments set.
        /// </summary>
        void OpenGenericArguments()
        {
            _instNesting++;
            _firstInstArg = true;

            Append('[');
        }

        /// <summary>
        /// Closes the generic arguments set.
        /// </summary>
        void CloseGenericArguments()
        {
            Debug.Assert(_instNesting != 0);

            _instNesting--;

            if (_firstInstArg)
                _builder.Remove(_builder.Length - 1, 1);
            else
                Append(']');
        }

        void OpenGenericArgument()
        {
            Debug.Assert(_instNesting != 0);

            _nestedName = false;

            if (!_firstInstArg)
                Append(',');

            _firstInstArg = false;

            Append('[');

            PushOpenGenericArgument();
        }

        void CloseGenericArgument()
        {
            Debug.Assert(_instNesting != 0);

            if (_hasAssemblySpec)
                Append(']');

            PopOpenGenericArgument();
        }

        /// <summary>
        /// Adds the given name.
        /// </summary>
        /// <param name="name"></param>
        void AddName(string name)
        {
            Debug.Assert(name != null);

            if (_nestedName)
                Append('+');

            _nestedName = true;

            EscapeName(name);
        }

        /// <summary>
        /// Adds an array rank indicator.
        /// </summary>
        /// <param name="rank"></param>
        void AddArray(int rank)
        {
            Debug.Assert(rank > 0);

            if (rank == 1)
            {
                Append("[*]");
            }
            else if (rank > 64)
            {
                _builder.Append('[');
#if NET
                if (rank.TryFormat(_builder.AppendSpan((int)Math.Floor(Math.Log10(rank))), out _) == false)
                    throw new InvalidOperationException();
#else
                _builder.Append(rank.ToString());
#endif
                _builder.Append(']');
            }
            else
            {
                Append('[');

                for (int i = 1; i < rank; i++)
                    Append(',');

                Append(']');
            }
        }

        void AddAssemblySpec(string assemblySpec)
        {
            if (assemblySpec != null && !assemblySpec.Equals(""))
            {
                Append(", ");

                if (_instNesting > 0)
                    EscapeEmbeddedAssemblyName(assemblySpec);
                else
                    EscapeAssemblyName(assemblySpec);

                _hasAssemblySpec = true;
            }
        }

        void EscapeName(string name)
        {
            if (ContainsReservedChar(name))
            {
                foreach (var c in name)
                {
                    if (c == '\0')
                        break;

                    if (IsTypeNameReservedChar(c))
                        _builder.Append('\\');

                    _builder.Append(c);
                }
            }
            else
                Append(name);
        }

        void EscapeAssemblyName(string name)
        {
            Append(name);
        }

        void EscapeEmbeddedAssemblyName(string name)
        {
            if (name.Contains("]"))
            {
                foreach (var c in name)
                {
                    if (c == ']')
                        Append('\\');

                    Append(c);
                }
            }
            else
            {
                Append(name);
            }
        }

        /// <summary>
        /// Enters into a generic argument.
        /// </summary>
        void PushOpenGenericArgument()
        {
            _stack.Add(_builder.Length);
            _stackIdx++;
        }

        /// <summary>
        /// Exits from a generic argument.
        /// </summary>
        void PopOpenGenericArgument()
        {
            int index = _stack[--_stackIdx];
            _stack.RemoveAt(_stackIdx);

            if (!_hasAssemblySpec)
                _builder.Remove(index - 1, 1);

            _hasAssemblySpec = false;
        }

        /// <summary>
        /// Appends the specfied string, ensuring we stop at null characters.
        /// </summary>
        /// <param name="pStr"></param>
        void Append(string pStr)
        {
            int i = pStr.IndexOf('\0');
            if (i < 0)
                _builder.Append(pStr);
            else if (i > 0)
                _builder.Append(pStr.AsSpan(0, i));
        }

        /// <summary>
        /// Appends the specified character.
        /// </summary>
        /// <param name="c"></param>
        void Append(char c)
        {
            _builder.Append(c);
        }

        /// <summary>
        /// Apppends the symbol for the element type of the specified type symbol.
        /// </summary>
        /// <param name="type"></param>
        void AddElementType(TypeSymbol type)
        {
            if (!type.HasElementType)
                return;

            AddElementType(type.GetElementType()!);

            if (type.IsPointer)
                Append('*');
            else if (type.IsByRef)
                Append('&');
            else if (type.IsSZArray)
                Append("[]");
            else if (type.IsArray)
                AddArray(type.GetArrayRank());
        }

        /// <summary>
        /// Appends the type name in the given format.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="format"></param>
        void AddAssemblyQualifiedName(TypeSymbol type, Format format)
        {
            var rootType = type;
            while (rootType.HasElementType)
                rootType = rootType.GetElementType()!;

            // append namespace + nesting + name
            var nestings = new List<TypeSymbol>();
            for (TypeSymbol? t = rootType; t != null; t = t.IsGenericParameter ? null : t.DeclaringType)
                nestings.Add(t);

            for (int i = nestings.Count - 1; i >= 0; i--)
            {
                var enclosingType = nestings[i];
                var name = enclosingType.Name;

                if (i == nestings.Count - 1 && !string.IsNullOrEmpty(enclosingType.Namespace))
                    name = $"{enclosingType.Namespace}.{name}";

                AddName(name);
            }

            // append generic arguments
            if (rootType.IsMissing == false && rootType.IsGenericType && (!rootType.IsGenericTypeDefinition || format == Format.ToString))
            {
                var genericArguments = rootType.GenericArguments;

                OpenGenericArguments();

                for (int i = 0; i < genericArguments.Length; i++)
                {
                    var genericArgumentsFormat = format == Format.FullName ? Format.AssemblyQualifiedName : format;
                    OpenGenericArgument();
                    AddAssemblyQualifiedName(genericArguments[i], genericArgumentsFormat);
                    CloseGenericArgument();
                }

                CloseGenericArguments();
            }

            // Append pointer, byRef and array qualifiers
            AddElementType(type);

            if (format == Format.AssemblyQualifiedName)
                AddAssemblySpec(type.Module.Assembly.FullName);
        }

        /// <summary>
        /// Returns the built string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            Debug.Assert(_instNesting == 0);
            return _builder.ToString();
        }

    }

}
