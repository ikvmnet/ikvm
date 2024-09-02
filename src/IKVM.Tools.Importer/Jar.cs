/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using System.Collections.Generic;
using System.IO;

namespace IKVM.Tools.Importer
{

    sealed class Jar
    {

        internal readonly string Name;
        private readonly List<JarItem> Items = new List<JarItem>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        internal Jar(string name)
        {
            this.Name = name;
        }

        internal Jar Copy()
        {
            Jar newJar = new Jar(Name);
            newJar.Items.AddRange(Items);
            return newJar;
        }

        internal void Add(string name, byte[] data, FileInfo fileInfo = null)
        {
            Items.Add(new JarItem(name, data, fileInfo));
        }

        private readonly struct JarItem
        {
            internal readonly string name;
            internal readonly byte[] data;
            internal readonly FileInfo path;            // path of the original file, if it was individual file (used to construct source file path)

            internal JarItem(string name, byte[] data, FileInfo path)
            {
                this.name = name;
                this.data = data;
                this.path = path;
            }
        }

        public struct Item
        {
            internal readonly Jar Jar;
            private readonly int Index;

            internal Item(Jar jar, int index)
            {
                this.Jar = jar;
                this.Index = index;
            }

            internal string Name
            {
                get { return Jar.Items[Index].name; }
            }

            internal byte[] GetData()
            {
                return Jar.Items[Index].data;
            }

            internal FileInfo Path
            {
                get { return Jar.Items[Index].path; }
            }

            internal void Remove()
            {
                Jar.Items[Index] = new JarItem();
            }

            internal void MarkAsStub()
            {
                Jar.Items[Index] = new JarItem(Jar.Items[Index].name, null, null);
            }

            internal bool IsStub
            {
                get { return Jar.Items[Index].data == null; }
            }
        }

        internal struct JarEnumerator
        {
            private readonly Jar jar;
            private int index;

            internal JarEnumerator(Jar jar)
            {
                this.jar = jar;
                this.index = -1;
            }

            public Item Current
            {
                get { return new Item(jar, index); }
            }

            public bool MoveNext()
            {
                if (index + 1 < jar.Items.Count)
                {
                    index++;
                    return true;
                }
                return false;
            }
        }

        public JarEnumerator GetEnumerator()
        {
            return new JarEnumerator(this);
        }

    }

}
