/*
  Copyright (C) 2007-2011 Jeroen Frijters

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
using System.IO;

using IKVM.Internal;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Represents an assembly class file within the virtual file system.
    /// </summary>
    sealed class VfsAssemblyClassFile : VfsFile
    {

        readonly TypeWrapper type;
        readonly Lazy<byte[]> buff;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        internal VfsAssemblyClassFile(VfsContext context, TypeWrapper type) :
            base(context)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
            this.buff = new Lazy<byte[]>(GenerateClassFile, true);
        }

        /// <summary>
        /// Gets the class file.
        /// </summary>
        /// <returns></returns>
        byte[] GenerateClassFile()
        {
#if FIRST_PASS
            throw new PlatformNotSupportedException();
#else
            var stream = new MemoryStream();
            var includeNonPublicInterfaces = bool.TryParse(java.lang.Props.props.getProperty("ikvm.stubgen.skipNonPublicInterfaces"), out var b) && b;
            IKVM.StubGen.StubGenerator.WriteClass(stream, type, includeNonPublicInterfaces, false, false, true);
            return stream.ToArray();
#endif
        }

        /// <summary>
        /// Opens the class file.
        /// </summary>
        /// <returns></returns>
        protected override Stream OpenRead() => new MemoryStream(buff.Value);

        /// <summary>
        /// Gets the length of the class file.
        /// </summary>
        public override long Size => buff.Value.Length;

    }

}
