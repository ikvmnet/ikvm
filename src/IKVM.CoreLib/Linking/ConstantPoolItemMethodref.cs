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

using IKVM.ByteCode.Decoding;
using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Type-model representation of a methodref constant.
    /// </summary>
    internal sealed class ConstantPoolItemMethodref<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> : ConstantPoolItemMI<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="data"></param>
        public ConstantPoolItemMethodref(ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile, MethodrefConstantData data) :
            base(classFile, data.Class, data.NameAndType)
        {

        }

        /// <inheritdoc />
        public override void Link(TLinkingType thisJavaType, LoadMode mode)
        {
            base.Link(thisJavaType, mode);

            var javaType = GetClassType();
            if (javaType != null && javaType.IsUnloadable == false)
            {
                _method = javaType.GetMethod(Name, Signature, !ReferenceEquals(Name, StringConstants.INIT));
                _method?.Link(mode);

                if (Name != StringConstants.INIT &&
                    thisJavaType.IsInterface == false &&
                    (ClassFile.Context.AllowNonVirtualCalls == false || (thisJavaType.AccessFlags & ClassFileAccessFlags.Super) == ClassFileAccessFlags.Super) &&
                    thisJavaType != javaType &&
                    thisJavaType.IsSubTypeOf(javaType))
                {
                    _invokespecialMethod = thisJavaType.BaseType.GetMethod(Name, Signature, true);
                    _invokespecialMethod?.Link(mode);
                }
            }
        }

    }

}
