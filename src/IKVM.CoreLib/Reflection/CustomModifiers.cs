/*
  Copyright (C) 2011 Jeroen Frijters

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
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    /// <summary>
    /// An immutable, ordered list of the optional (modopt) and required (modreq) custom modifiers on a type in a
    /// signature. Internally the modifier types are stored in one array in which <see cref="MarkerType.ModOpt"/> and
    /// <see cref="MarkerType.ModReq"/> entries switch between optional and required runs.
    /// </summary>
    readonly struct CustomModifiers : IEquatable<CustomModifiers>, IEnumerable<CustomModifiers.Entry>
    {

        /// <summary>
        /// A single custom modifier.
        /// </summary>
        /// <param name="Type">The modifier type.</param>
        /// <param name="IsRequired"><see langword="true"/> for a required modifier (modreq); <see langword="false"/> for an optional one (modopt).</param>
        public readonly record struct Entry(Type Type, bool IsRequired);

        /// <summary>
        /// Enumerates the custom modifiers in order, skipping the marker entries.
        /// </summary>
        public struct Enumerator : IEnumerator<Entry>
        {

            readonly Type[] _types;
            int _index;
            bool _required;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="types">The modifier array to enumerate, or <see langword="null"/> for no modifiers.</param>
            internal Enumerator(Type[] types)
            {
                _types = types;
                _index = -1;
                _required = Initial == MarkerType.ModReq;
            }

            /// <inheritdoc />
            void System.Collections.IEnumerator.Reset()
            {
                _index = -1;
                _required = Initial == MarkerType.ModReq;
            }

            /// <summary>
            /// Gets the modifier at the current position of the enumerator.
            /// </summary>
            public Entry Current => new(_types[_index], _required);

            /// <summary>
            /// Advances the enumerator to the next modifier, skipping a marker that switches between optional and
            /// required modifiers.
            /// </summary>
            /// <returns><see langword="true"/> if the enumerator was advanced to the next modifier; <see langword="false"/> if it has passed the end.</returns>
            public bool MoveNext()
            {
                if (_types == null || _index == _types.Length)
                    return false;

                _index++;

                if (_index == _types.Length)
                {
                    return false;
                }
                else if (_types[_index] == MarkerType.ModOpt)
                {
                    _required = false;
                    _index++;
                }
                else if (_types[_index] == MarkerType.ModReq)
                {
                    _required = true;
                    _index++;
                }

                return true;
            }

            /// <inheritdoc />
            object System.Collections.IEnumerator.Current => Current;

            /// <inheritdoc />
            void IDisposable.Dispose()
            {
            }

        }

        /// <summary>
        /// Gets the marker for the mode a modifier array starts in when it has no leading marker: modifiers are
        /// optional until a <see cref="MarkerType.ModReq"/> marker is seen.
        /// </summary>
        // note that FromReqOpt assumes that Initial == ModOpt
        static Type Initial => MarkerType.ModOpt;

        readonly Type[] _types;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="types">The modifier array, including markers, or <see langword="null"/> for no modifiers. Must not be empty.</param>
        CustomModifiers(Type[] types)
        {
            Debug.Assert(types == null || types.Length != 0);
            _types = types;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the custom modifiers.
        /// </summary>
        /// <returns>An enumerator for the custom modifiers.</returns>
        public Enumerator GetEnumerator() => new Enumerator(_types);

        /// <inheritdoc />
        IEnumerator<Entry> IEnumerable<Entry>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets whether there are no custom modifiers.
        /// </summary>
        public bool IsEmpty => _types == null;

        /// <summary>
        /// Returns whether this list contains the same modifiers in the same order as another one.
        /// </summary>
        /// <param name="other">The list to compare with.</param>
        /// <returns><see langword="true"/> if the lists are equal; otherwise <see langword="false"/>.</returns>
        public bool Equals(CustomModifiers other) => Util.ArrayEquals(_types, other._types);

        /// <summary>
        /// Returns whether the specified object is a <see cref="CustomModifiers"/> with the same modifiers in the same
        /// order.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is an equal list; otherwise <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as CustomModifiers?;
            return other != null && Equals(other.Value);
        }

        /// <inheritdoc />
        public override int GetHashCode() => Util.GetHashCode(_types);

        /// <summary>
        /// Returns the modifiers in IL assembler syntax, for example <c>modreq(System.Runtime.CompilerServices.IsVolatile)</c>,
        /// separated by spaces.
        /// </summary>
        /// <returns>The modifiers as a string, or an empty string if there are none.</returns>
        public override string ToString()
        {
            if (_types == null)
                return string.Empty;

            var sb = new StringBuilder();
            var sep = "";
            foreach (var e in this)
            {
                sb.Append(sep).Append(e.IsRequired ? "modreq(" : "modopt(").Append(e.Type.FullName).Append(')');
                sep = " ";
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets whether any of the modifier types is a missing type.
        /// </summary>
        public bool ContainsMissingType => Type.ContainsMissingType(_types);

        /// <summary>
        /// Gets either the required or the optional modifiers, in reverse order to match reflection.
        /// </summary>
        /// <param name="required"><see langword="true"/> to get the required modifiers; <see langword="false"/> to get the optional ones.</param>
        /// <returns>The modifier types.</returns>
        Type[] GetRequiredOrOptional(bool required)
        {
            if (_types == null)
                return Type.EmptyTypes;

            var count = 0;
            foreach (var e in this)
                if (e.IsRequired == required)
                    count++;

            var result = new Type[count];
            foreach (var e in this)
                if (e.IsRequired == required)
                    // FXBUG reflection (and ildasm) return custom modifiers in reverse order while SRE writes them in the specified order
                    result[--count] = e.Type;

            return result;
        }

        /// <summary>
        /// Gets the required (modreq) modifiers, in reverse order to match reflection.
        /// </summary>
        /// <returns>The required modifier types.</returns>
        internal Type[] GetRequired() => GetRequiredOrOptional(true);

        /// <summary>
        /// Gets the optional (modopt) modifiers, in reverse order to match reflection.
        /// </summary>
        /// <returns>The optional modifier types.</returns>
        internal Type[] GetOptional() => GetRequiredOrOptional(false);

        /// <summary>
        /// Returns the modifiers with any generic type parameters in the modifier types replaced using the specified
        /// binder.
        /// </summary>
        /// <param name="binder">The binder that supplies the type arguments.</param>
        /// <returns>The bound modifiers, sharing the array with this instance when nothing changed.</returns>
        internal CustomModifiers Bind(IGenericBinder binder)
        {
            if (_types == null)
                return this;

            var result = _types;
            for (var i = 0; i < _types.Length; i++)
            {
                if (_types[i] == MarkerType.ModOpt || _types[i] == MarkerType.ModReq)
                    continue;

                var type = _types[i].BindTypeParameters(binder);
                if (!ReferenceEquals(type, _types[i]))
                {
                    if (result == _types)
                        result = (Type[])_types.Clone();

                    result[i] = type;
                }
            }

            return new CustomModifiers(result);
        }

        /// <summary>
        /// Reads the custom modifiers, if any, at the current position of a signature blob.
        /// </summary>
        /// <param name="module">The module the signature belongs to.</param>
        /// <param name="br">The reader positioned at the start of the modifiers. It is advanced past them.</param>
        /// <param name="context">The generic context used to resolve type parameters.</param>
        /// <returns>The modifiers read, or an empty list if there are none.</returns>
        internal static CustomModifiers Read(ModuleReader module, ref BlobReader br, IGenericContext context)
        {
            var b = br.PeekByte();
            if (!IsCustomModifier(b))
                return new CustomModifiers();

            var list = new List<Type>();
            var mode = Initial;
            do
            {
                var cmod = br.ReadByte() == Signature.ELEMENT_TYPE_CMOD_REQD ? MarkerType.ModReq : MarkerType.ModOpt;
                if (mode != cmod)
                {
                    mode = cmod;
                    list.Add(mode);
                }

                list.Add(Signature.ReadTypeDefOrRefEncoded(module, ref br, context));
                b = br.PeekByte();
            }
            while (IsCustomModifier(b));

            return new CustomModifiers(list.ToArray());
        }

        /// <summary>
        /// Advances a signature blob reader past any custom modifiers at its current position without resolving them.
        /// </summary>
        /// <param name="br">The reader to advance.</param>
        internal static void Skip(ref BlobReader br)
        {
            var b = br.PeekByte();
            while (IsCustomModifier(b))
            {
                br.ReadByte();
                br.ReadCompressedInteger();
                b = br.PeekByte();
            }
        }

        /// <summary>
        /// Creates a modifier list from separate arrays of required and optional modifiers, as passed to the
        /// System.Reflection.Emit APIs. The optional modifiers are placed first.
        /// </summary>
        /// <param name="req">The required modifiers, or <see langword="null"/>.</param>
        /// <param name="opt">The optional modifiers, or <see langword="null"/>.</param>
        /// <returns>The combined modifier list.</returns>
        internal static CustomModifiers FromReqOpt(Type[] req, Type[] opt)
        {
            List<Type> list = null;

            if (opt != null && opt.Length != 0)
            {
                Debug.Assert(Initial == MarkerType.ModOpt);
                list = new List<Type>(opt);
            }

            if (req != null && req.Length != 0)
            {
                list ??= [];
                list.Add(MarkerType.ModReq);
                list.AddRange(req);
            }

            if (list == null)
                return new CustomModifiers();
            else
                return new CustomModifiers(list.ToArray());
        }

        /// <summary>
        /// Returns whether a signature byte is ELEMENT_TYPE_CMOD_OPT or ELEMENT_TYPE_CMOD_REQD.
        /// </summary>
        /// <param name="b">The signature byte.</param>
        /// <returns><see langword="true"/> if the byte introduces a custom modifier; otherwise <see langword="false"/>.</returns>
        static bool IsCustomModifier(byte b) => b == Signature.ELEMENT_TYPE_CMOD_OPT || b == Signature.ELEMENT_TYPE_CMOD_REQD;

    }

}
