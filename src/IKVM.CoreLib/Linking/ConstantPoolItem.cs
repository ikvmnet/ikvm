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

using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Type-model representation of a constant pool item.
    /// </summary>
    internal abstract class ConstantPoolItem<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        readonly ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> _classFile;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        protected ConstantPoolItem(ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile)
        {
            _classFile = classFile ?? throw new ArgumentNullException(nameof(classFile));
        }

        /// <summary>
        /// Gets the <see cref="ClassFile{TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod}" /> that hosts this instance.
        /// </summary>
        public ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> ClassFile => _classFile;

        /// <summary>
        /// Gets the type of constant represented by this item.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual ConstantType ConstantType => throw new InvalidOperationException();

        /// <summary>
        /// Resolves the constant information from the specified class file, and UTF8 string cache.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="utf8_cp"></param>
        /// <param name="options"></param>
        public virtual void Resolve()
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
        public virtual void Link(TLinkingType thisType, LoadMode mode)
        {

        }

        /// <summary>
        /// Gets the runtime value of the constant.
        /// </summary>
        /// <returns></returns>
        public virtual object? GetRuntimeValue()
        {
            return null;
        }

    }

}
