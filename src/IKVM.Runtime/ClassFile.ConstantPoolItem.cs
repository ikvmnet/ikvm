/*
  Copyright (C) 2002-2015 Jeroen Frijters

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

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        /// <summary>
        /// Type-model representation of a constant pool item.
        /// </summary>
        internal abstract class ConstantPoolItem
        {

            readonly RuntimeContext _context;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            protected ConstantPoolItem(RuntimeContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            /// <summary>
            /// Gets the <see cref="RuntimeContext"/> that hosts this instanc.
            /// </summary>
            public RuntimeContext Context => _context;

            /// <summary>
            /// Gets the type of constant represented by this item.
            /// </summary>
            /// <returns></returns>
            /// <exception cref="InvalidOperationException"></exception>
            public virtual ConstantType GetConstantType()
            {
                throw new InvalidOperationException();
            }

            /// <summary>
            /// Resolves the constant information from the specified class file, and UTF8 string cache.
            /// </summary>
            /// <param name="classFile"></param>
            /// <param name="utf8_cp"></param>
            /// <param name="options"></param>
            public virtual void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
            {

            }

            /// <summary>
            /// Marks the constant as requiring linkage.
            /// </summary>
            public virtual void MarkLinkRequired()
            {

            }

            /// <summary>
            /// Links the constant.
            /// </summary>
            /// <param name="thisType"></param>
            /// <param name="mode"></param>
            public virtual void Link(RuntimeJavaType thisType, LoadMode mode)
            {

            }

            /// <summary>
            /// Gets the runtime value of the constant.
            /// </summary>
            /// <returns></returns>
            public virtual object GetRuntimeValue()
            {
                return null;
            }

        }

    }

}
