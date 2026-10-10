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
using System.Text;
using System.Reflection.Metadata;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    internal readonly struct CustomModifiers : IEquatable<CustomModifiers>, IEnumerable<CustomModifiers.Entry>
    {

        public readonly record struct Entry(Type Type, bool IsRequired);

        public struct Enumerator : IEnumerator<Entry>
        {

            readonly Type[] _types;
            int _index;
            bool _required;

            /// <summary>
            /// Initializes a new instance
            /// </summary>
            /// <param name="types"></param>
            internal Enumerator(Type[] types)
            {
                this._types = types;
                this._index = -1;
                this._required = Initial == MarkerType.ModReq;
            }

            void System.Collections.IEnumerator.Reset()
            {
                this._index = -1;
                this._required = Initial == MarkerType.ModReq;
            }

            public Entry Current => new Entry(_types[_index], _required);

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

            object System.Collections.IEnumerator.Current => Current;

            void IDisposable.Dispose()
            {
            }

        }

        static Type Initial => MarkerType.ModOpt; // note that FromReqOpt assumes that Initial == ModOpt

        readonly Type[] _types;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="types"></param>
        CustomModifiers(Type[] types)
        {
            Debug.Assert(types == null || types.Length != 0);
            this._types = types;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_types);
        }

        IEnumerator<Entry> IEnumerable<Entry>.GetEnumerator()
        {
            return GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool IsEmpty
        {
            get { return _types == null; }
        }

        public bool Equals(CustomModifiers other)
        {
            return Util.ArrayEquals(_types, other._types);
        }

        public override bool Equals(object obj)
        {
            var other = obj as CustomModifiers?;
            return other != null && Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Util.GetHashCode(_types);
        }

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

        public bool ContainsMissingType
        {
            get { return Type.ContainsMissingType(_types); }
        }

        Type[] GetRequiredOrOptional(bool required)
        {
            if (_types == null)
                return Type.EmptyTypes;

            int count = 0;
            foreach (var e in this)
                if (e.IsRequired == required)
                    count++;

            var result = new Type[count];
            foreach (var e in this)
                if (e.IsRequired == required)
                    result[--count] = e.Type; // FXBUG reflection (and ildasm) return custom modifiers in reverse order while SRE writes them in the specified order

            return result;
        }

        internal Type[] GetRequired()
        {
            return GetRequiredOrOptional(true);
        }

        internal Type[] GetOptional()
        {
            return GetRequiredOrOptional(false);
        }

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
                list ??= new List<Type>();
                list.Add(MarkerType.ModReq);
                list.AddRange(req);
            }

            if (list == null)
                return new CustomModifiers();
            else
                return new CustomModifiers(list.ToArray());
        }

        static bool IsCustomModifier(byte b)
        {
            return b == Signature.ELEMENT_TYPE_CMOD_OPT || b == Signature.ELEMENT_TYPE_CMOD_REQD;
        }

    }

}
