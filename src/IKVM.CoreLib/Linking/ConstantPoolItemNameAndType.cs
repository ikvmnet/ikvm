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

using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Type-model representation of a nameandtype constant.
    /// </summary>
    sealed class ConstantPoolItemNameAndType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> : ConstantPoolItem<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        internal Utf8ConstantHandle _nameHandle;
        internal Utf8ConstantHandle _descriptorHandle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="data"></param>
        public ConstantPoolItemNameAndType(ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile, NameAndTypeConstantData data) :
            base(classFile)
        {
            _nameHandle = data.Name;
            _descriptorHandle = data.Descriptor;
        }

        /// <inheritdoc />
        public override void Resolve()
        {
            if (ClassFile.GetConstantPoolUtf8String(_nameHandle) == null || ClassFile.GetConstantPoolUtf8String(_descriptorHandle) == null)
                throw new ClassFormatException("Illegal constant pool index");
        }

    }

}
