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

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    public readonly struct CustomModifiers : IEquatable<CustomModifiers>, IEnumerable<CustomModifiers.Entry>
    {

        public readonly record struct Entry(Type Type, bool IsRequired);

        public struct Enumerator : IEnumerator<Entry>
        {

            readonly Type[] types;
            int index;
            bool required;

            /// <summary>
            /// Initializes a new instance
            /// </summary>
            /// <param name="types"></param>
            internal Enumerator(Type[] types)
            {
                this.types = types;
                this.index = -1;
                this.required = Initial == MarkerType.ModReq;
            }

            void System.Collections.IEnumerator.Reset()
            {
                this.index = -1;
                this.required = Initial == MarkerType.ModReq;
            }

            public Entry Current => new Entry(types[index], required);

            public bool MoveNext()
            {
                if (types == null || index == types.Length)
                    return false;

                index++;

                if (index == types.Length)
                {
                    return false;
                }
                else if (types[index] == MarkerType.ModOpt)
                {
                    required = false;
                    index++;
                }
                else if (types[index] == MarkerType.ModReq)
                {
                    required = true;
                    index++;
                }

                return true;
            }

            object System.Collections.IEnumerator.Current => Current;

            void IDisposable.Dispose()
            {

            }

        }

        static Type Initial => MarkerType.ModOpt; // note that FromReqOpt assumes that Initial == ModOpt

        readonly Type[] types;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="list"></param>
        internal CustomModifiers(List<CustomModifiersBuilder.Item> list)
        {
            var required = Initial == MarkerType.ModReq;

            var count = list.Count;
            foreach (var item in list)
            {
                if (item.required != required)
                {
                    required = item.required;
                    count++;
                }
            }

            types = new Type[count];
            required = Initial == MarkerType.ModReq;
            int index = 0;
            foreach (var item in list)
            {
                if (item.required != required)
                {
                    required = item.required;
                    types[index++] = required ? MarkerType.ModReq : MarkerType.ModOpt;
                }

                types[index++] = item.type;
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="types"></param>
        CustomModifiers(Type[] types)
        {
            Debug.Assert(types == null || types.Length != 0);
            this.types = types;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(types);
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
            get { return types == null; }
        }

        public bool Equals(CustomModifiers other)
        {
            return Util.ArrayEquals(types, other.types);
        }

        public override bool Equals(object obj)
        {
            var other = obj as CustomModifiers?;
            return other != null && Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Util.GetHashCode(types);
        }

        public override string ToString()
        {
            if (types == null)
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
            get { return Type.ContainsMissingType(types); }
        }

        Type[] GetRequiredOrOptional(bool required)
        {
            if (types == null)
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
            if (types == null)
                return this;

            var result = types;
            for (var i = 0; i < types.Length; i++)
            {
                if (types[i] == MarkerType.ModOpt || types[i] == MarkerType.ModReq)
                    continue;

                var type = types[i].BindTypeParameters(binder);
                if (!ReferenceEquals(type, types[i]))
                {
                    if (result == types)
                        result = (Type[])types.Clone();

                    result[i] = type;
                }
            }

            return new CustomModifiers(result);
        }

        internal static CustomModifiers Read(ModuleReader module, ByteReader br, IGenericContext context)
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

                list.Add(Signature.ReadTypeDefOrRefEncoded(module, br, context));
                b = br.PeekByte();
            }
            while (IsCustomModifier(b));

            return new CustomModifiers(list.ToArray());
        }

        internal static void Skip(ByteReader br)
        {
            var b = br.PeekByte();
            while (IsCustomModifier(b))
            {
                br.ReadByte();
                br.ReadCompressedUInt();
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

        internal static CustomModifiers Combine(CustomModifiers mods1, CustomModifiers mods2)
        {
            if (mods1.IsEmpty)
            {
                return mods2;
            }
            else if (mods2.IsEmpty)
            {
                return mods1;
            }
            else
            {
                var combo = new Type[mods1.types.Length + mods2.types.Length];
                Array.Copy(mods1.types, combo, mods1.types.Length);
                Array.Copy(mods2.types, 0, combo, mods1.types.Length, mods2.types.Length);
                return new CustomModifiers(combo);
            }
        }

    }

}
